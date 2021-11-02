//using Logitude.AmitalMessaging.Customs.
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.AmitalMessaging.Utils;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.Messaging.U2L.ImportDeclaration;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using Unifreight.Data.AmitalModel;

namespace Logitude.Customs.BL.Messaging.U2L.CommMasterCourier
{
    public class CommMasterCourierService : UnifreightGenericService
    {
        private LOGIMASTERCOUR _LOGIMASTERCOUR;
        private LogitudeMasterCourier _LogitudeMasterCourier;
        private CourierMasterPM _CourierMasterPM;
        private ICustomContext _context;

        private AmitalContext amitalContext;

        public const string UpsertActionConst = "Logitude.Customs.BL.Messaging.U2L.CommMasterCourier.CommMasterCourierService.Upsert()";
        
        private Stopwatch _Stopwatch;

        public CommMasterCourierService()
            : base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
            true
            )
        {

        }

        protected int ResolvedTenantLocal() 
        {
            if (int.Parse(_LOGIMASTERCOUR.LogitudeMasterCourier[0].Tenant) > 0)
            {
                return int.Parse(_LOGIMASTERCOUR.LogitudeMasterCourier[0].Tenant);
            }
            
            return ResolvedTenant();
        }

        public override void ProccessGenericRequest(
              string xmlLOGIMASTERCOUR,
              ref string MoreParams,
              out string MessageOut)
        {
            MessageOut = "";
            _Stopwatch = Stopwatch.StartNew();
            MyCommunicationsParams.Subject = "CommMasterCourierService ";

            DeserilazeObject(xmlLOGIMASTERCOUR);
            AppendLogLine("DeserilazeObject:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

            //CheckIntegrity();
            AppendLogLine("CheckIntegrity:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
            MyGenericResponseObj.Stage = "GetContext";
            MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.Success;
            _context = CustomContext.GetContext(ResolvedTenant());
            amitalContext = AmitalContext.GetContext(ResolvedTenant());
            

            ICustomContext dbContext = CustomContext.GetContext(ResolvedTenant());
            MyGenericResponseObj.Stage = "CourierMasterUpsert";
            
            try
            {
                var myQueryService = new CourierMasterQueryService(_context);
                var myCourierMasterUpdateService = new CourierMasterUpdateService(_context, new Dictionary<string, IContext>(), ResolvedTenantLocal());

                MyGenericResponseObj.Stage = "Check integrity ";
                ///must 

                if (String.IsNullOrWhiteSpace(_LogitudeMasterCourier.AirlineId))
                {
                    MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                    MyGenericResponseObj.Message = "AirlineId is missing";
                    AppendLogLine(MyGenericResponseObj.Message);
                    return;
                }
                /*
                int index = _LogitudeMasterCourier.AirlineId.IndexOf('-');
                if (index < 1)
                {
                    MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                    MyGenericResponseObj.Message = "airlineId does not contains code and prefix";
                    AppendLogLine(MyGenericResponseObj.Message);
                    return;
                }
                */
                var airlineId = TranslateAirline(_LogitudeMasterCourier.AirlineId);
                if (String.IsNullOrWhiteSpace(airlineId))
                {
                    MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                    MyGenericResponseObj.Message = "AirlineId " + _LogitudeMasterCourier.AirlineId + " Doesn't exist";
                    AppendLogLine(MyGenericResponseObj.Message);
                    return;
                }

                if (String.IsNullOrWhiteSpace(_LogitudeMasterCourier.MAWB))
                {
                    MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                    MyGenericResponseObj.Message = "MAWB is missing";
                    AppendLogLine(MyGenericResponseObj.Message);
                    return;
                }
                /*
                if (String.IsNullOrWhiteSpace(_LogitudeMasterCourier.HAWB))
                {
                    MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                    MyGenericResponseObj.Message = "HAWB is missing";
                    AppendLogLine(MyGenericResponseObj.Message);
                    return;
                }
                */
                MyGenericResponseObj.Stage = "GetSingle";
                this._CourierMasterPM = myQueryService.GetSingleByAirlineAWBs(airlineId, _LogitudeMasterCourier.HAWB, _LogitudeMasterCourier.MAWB, ResolvedTenant());
                /// Exist
                if (_CourierMasterPM == null || String.IsNullOrWhiteSpace(_CourierMasterPM.Id))
                {
                    this._CourierMasterPM = new Def.EntityPMs.CourierMasterPM();
                    this._CourierMasterPM.ChangeSetOp = ChangeSetOperation.Insert;
                }
                else
                {
                    if (_CourierMasterPM.IsOpen != true)
                    {
                        MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                        MyGenericResponseObj.Message = "Courier Master " + _CourierMasterPM.AirlinePrefix + "-" + _CourierMasterPM.MAWB + "is closed";
                        AppendLogLine(MyGenericResponseObj.Message);
                        return;
                    }
                    this._CourierMasterPM.ChangeSetOp = ChangeSetOperation.Update;
                }

                _CourierMasterPM.AirlineId = airlineId;
                _CourierMasterPM.MAWB = _LogitudeMasterCourier.MAWB;
                if(string.IsNullOrWhiteSpace(_CourierMasterPM.MAWBTypeCode))_CourierMasterPM.MAWBTypeCode = "740";
                if (!string.IsNullOrWhiteSpace(_LogitudeMasterCourier.HAWB) && string.IsNullOrWhiteSpace(_CourierMasterPM.HAWB)) _CourierMasterPM.HAWB = _LogitudeMasterCourier.HAWB;
                decimal grossMassMeasure = 0;
                if (_CourierMasterPM.GrossMassMeasure == null)
                {
                    if (decimal.TryParse(_LogitudeMasterCourier.GrossMassMeasure, out grossMassMeasure) || string.IsNullOrWhiteSpace(_LogitudeMasterCourier.GrossMassMeasure))
                    {
                        _CourierMasterPM.GrossMassMeasure = grossMassMeasure;
                    }
                }
                /*
                else if(!string.IsNullOrWhiteSpace(_LogitudeMasterCourier.GrossMassMeasure))
                {
                    if (decimal.TryParse(_LogitudeMasterCourier.GrossMassMeasure, out grossMassMeasure))
                    {
                        _CourierMasterPM.GrossMassMeasure = grossMassMeasure;
                    }
                }*/
                int packageQuantity = 0;
                if (_CourierMasterPM.PackageQuantity == null)
                {
                    if (int.TryParse(_LogitudeMasterCourier.PackageQuantityTy, out packageQuantity) || string.IsNullOrWhiteSpace(_LogitudeMasterCourier.PackageQuantityTy))
                    {
                        _CourierMasterPM.PackageQuantity = packageQuantity;
                    }
                }
                /*
                else if (!string.IsNullOrWhiteSpace(_LogitudeMasterCourier.PackageQuantityTy))
                {
                    if (int.TryParse(_LogitudeMasterCourier.PackageQuantityTy, out packageQuantity))
                    {
                        _CourierMasterPM.PackageQuantity = packageQuantity;
                    }
                }*/
                DateTime temp;
                if (!string.IsNullOrWhiteSpace(_LogitudeMasterCourier.EstimatedArrivalDate) && _CourierMasterPM.EstimatedArrivalDate.HasValue == false)
                {
                    if (DateTime.TryParse(_LogitudeMasterCourier.EstimatedArrivalDate, out temp))
                    {
                        _CourierMasterPM.EstimatedArrivalDate = temp;
                    }
                    else
                    {
                        _CourierMasterPM.EstimatedArrivalDate = AmitalConvertUtil.GetUnifreightFormatedDate(_LogitudeMasterCourier.EstimatedArrivalDate, "LogitudeMasterCourier.EstimatedArrivalDate");
                    }
                }
                if (!string.IsNullOrWhiteSpace(_LogitudeMasterCourier.DepartureDate) && _CourierMasterPM.DepartureDate.HasValue == false)
                {
                    if (DateTime.TryParse(_LogitudeMasterCourier.DepartureDate, out temp))
                    {
                        _CourierMasterPM.DepartureDate = temp;
                    }
                    else
                    {
                        _CourierMasterPM.DepartureDate = AmitalConvertUtil.GetUnifreightFormatedDate(_LogitudeMasterCourier.DepartureDate, "LogitudeMasterCourier.DepartureDate");
                    }
                }
                if (!string.IsNullOrWhiteSpace(_LogitudeMasterCourier.GatewayPortCode) && string.IsNullOrWhiteSpace(_CourierMasterPM.GatewayPortCode)) _CourierMasterPM.GatewayPortCode = TranslateInternationalSite(_LogitudeMasterCourier.GatewayPortCode);
                if (!string.IsNullOrWhiteSpace(_LogitudeMasterCourier.OriginPortCode) && string.IsNullOrWhiteSpace(_CourierMasterPM.OriginPortCode)) _CourierMasterPM.OriginPortCode = TranslateInternationalSite(_LogitudeMasterCourier.OriginPortCode);
                if (_CourierMasterPM.Tenant < 1) _CourierMasterPM.Tenant = ResolvedTenant();
                if (!string.IsNullOrWhiteSpace(_LogitudeMasterCourier.FlightNumber) && string.IsNullOrWhiteSpace(_CourierMasterPM.FlightNumber)) _CourierMasterPM.FlightNumber = _LogitudeMasterCourier.FlightNumber;
                if (!string.IsNullOrWhiteSpace(_LogitudeMasterCourier.WeightValueCode) && string.IsNullOrWhiteSpace(_CourierMasterPM.WeightValueCode)) _CourierMasterPM.WeightValueCode = TranslateWeightValue(_LogitudeMasterCourier.WeightValueCode);
                if (!string.IsNullOrWhiteSpace(_LogitudeMasterCourier.IntegratorIndex) && string.IsNullOrWhiteSpace(_CourierMasterPM.IntegratorCode)) _CourierMasterPM.IntegratorCode = TranslateIntegratorIndex(_LogitudeMasterCourier.IntegratorIndex);
                _CourierMasterPM.CurrentContextTag = UpsertActionConst;
                if (!string.IsNullOrWhiteSpace(_LogitudeMasterCourier.StorageSiteCode) && string.IsNullOrWhiteSpace(_CourierMasterPM.StorageSiteCode)) _CourierMasterPM.StorageSiteCode = TranslateStorageSite(_LogitudeMasterCourier.StorageSiteCode);
                if (!string.IsNullOrWhiteSpace(_LogitudeMasterCourier.NoOfCourierHawb) && string.IsNullOrWhiteSpace(_CourierMasterPM.NoOfCourierHawb)) _CourierMasterPM.NoOfCourierHawb = _LogitudeMasterCourier.NoOfCourierHawb;
                /*
                if (!string.IsNullOrWhiteSpace(_LogitudeMasterCourier.TruckerId) && string.IsNullOrWhiteSpace(_CourierMasterPM.TruckerId))
                {
                    CardRepository cardRep = new CardRepository(_CourierMasterPM.Tenant);
                    Card card = cardRep.GetSingleCard(_LogitudeMasterCourier.TruckerId, _CourierMasterPM.Tenant);
                    if (card != null)
                    {
                        _CourierMasterPM.TruckerId = _LogitudeMasterCourier.TruckerId;
                    }
                    else
                    {
                        card = cardRep.GetSingleCardByCode(_LogitudeMasterCourier.TruckerId, _CourierMasterPM.Tenant, true);
                        if (card != null)
                        {
                            _CourierMasterPM.TruckerId = card.Id;
                        }
                    }
                }
                */
                int packageQuantityInMAWB = 0;
                _CourierMasterPM.PackageQuantityInMAWB = packageQuantityInMAWB;
                if (!string.IsNullOrWhiteSpace(_LogitudeMasterCourier.PackageQuantityInMAWB))
                {
                    if (int.TryParse(_LogitudeMasterCourier.PackageQuantityInMAWB, out packageQuantityInMAWB))
                    {
                        _CourierMasterPM.PackageQuantityInMAWB = packageQuantityInMAWB;
                    }
                }
                if (!string.IsNullOrWhiteSpace(_LogitudeMasterCourier.UnifreightLeadingFile) && string.IsNullOrWhiteSpace(_CourierMasterPM.UnifreightLeadingFile)) _CourierMasterPM.UnifreightLeadingFile = _LogitudeMasterCourier.UnifreightLeadingFile;

                if (_LOGIMASTERCOUR.WAYBILLS != null && _LOGIMASTERCOUR.WAYBILLS[0].wb != null)
                {
                    if (this._LOGIMASTERCOUR.WAYBILLS.FirstOrDefault().wb.Count() > 0)
                    {
                        MyGenericResponseObj.Stage = "Start Connect Declarations To Master By WayBill ";
                        foreach (var wayBill in this._LOGIMASTERCOUR.WAYBILLS[0].wb)
                        {
                            ConnectDeclarationToMasterByWayBill(wayBill, _LogitudeMasterCourier.MAWB);
                        }
                        MyGenericResponseObj.Stage = "Done Connecting Declarations To Master By WayBill";
                    }
                }

                myCourierMasterUpdateService.Update(this._CourierMasterPM, true);

                AppendLogLine("CourierMasterUpdate:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

                MyGenericResponseObj.Stage = "Done All ";
                MyGenericResponseObj.ApplicationId = this._CourierMasterPM.Id;
                MyCommunicationsParams.LoggingEntityId = MyGenericResponseObj.ApplicationId;
                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.Success;
            }
            catch (DbEntityValidationException ex)
            {

                var FormatedException = ExceptionFormatUtil.GetFormated(ex);
                AppendLogLine("Master Courier Upsert Error " + Environment.NewLine + Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.ToString(1000));
                AppendLogLine("ProccessRequest():Exception " + FormatedException.ToString() + Environment.NewLine + "---------------------------------------------");
                MyGenericResponseObj.Message = "Master Courier Upsert Error ";
                MyGenericResponseObj.InnerException = FormatedException.ToString();
                MyGenericResponseObj.ExceptionType = ex.GetType().ToString();
                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;

                return;
            }
            catch (Exception e)
            {
                AppendLogLine("Master Courier Upsert Error " + Environment.NewLine + Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.ToString(1000));
                AppendLogLine("ProccessRequest():Exception " + e.ToString() + Environment.NewLine + "---------------------------------------------");
                MyGenericResponseObj.Message = "Master Courier Upsert Error ";
                MyGenericResponseObj.InnerException = e.ToString();
                MyGenericResponseObj.ExceptionType = e.GetType().ToString();
                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                return;
            }
            if(!String.IsNullOrWhiteSpace(MyGenericResponseObj.StatusType.ToString()) && MyGenericResponseObj.StatusType != GenericResponseObj.StatusEnum.Success)
            {
                AppendLogLine("Master Courier Upsert Error " + Environment.NewLine + Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.ToString(1000));
                return;
            }
            
        }

        private void ConnectDeclarationToMasterByWayBill(string wayBill, string mAWB)
        {
            var myQueryService = new DeclarationQueryService(_context);
            var MyDeclarationIDs = myQueryService.GetListByCourierHAWB(wayBill, this._CourierMasterPM.Tenant);
            if(MyDeclarationIDs == null || String.IsNullOrWhiteSpace(MyDeclarationIDs.FirstOrDefault()))
            {
                AppendLogLine("couldn't find Declaration by wayBill " + wayBill);
                return;
            }
            foreach (var id in MyDeclarationIDs)
            {
                DeclarationPM MyDeclarationPM = myQueryService.GetSingle(id, false, true);
                if(MyDeclarationPM != null && MyDeclarationPM.Id != null)
                {
                    CheckMasterToUpdate(MyDeclarationPM, mAWB);
                }
                else
                {
                    AppendLogLine("couldn't find Declaration by wayBill " + wayBill);
                }
            }
        }

        

        private void CheckMasterToUpdate(DeclarationPM _MyDeclarationPM, string mAWB)
        {
            
            if (_CourierMasterPM != null)
            {
                CourierDeclarationPM _CourierDeclarationPM = new CourierDeclarationPM();
                var myCourierDeclarationQueryService = new CourierDeclarationQueryService(_context);
                var myCourierDeclarationUpdateService = new CourierDeclarationUpdateService(_context, new Dictionary<string, IContext>(), _CourierMasterPM.Tenant);
                _CourierDeclarationPM = myCourierDeclarationQueryService.GetSingle(_MyDeclarationPM.Id, _CourierMasterPM.Id, false, true);
                if (_CourierDeclarationPM == null)
                {
                    _CourierDeclarationPM = new CourierDeclarationPM();
                    _CourierDeclarationPM.ChangeSetOp = ChangeSetOperation.Insert;
                    _CourierDeclarationPM.DeclarationId = _MyDeclarationPM.Id;
                    _CourierDeclarationPM.CourierMasterId = _CourierMasterPM.Id;
                }
                else
                {
                    _CourierDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                }
                if (_CourierDeclarationPM.SequenceNumeric == null)
                {
                    int? sequenceNumericMax = myCourierDeclarationQueryService.GetCourierMasterMaxSequenceNumeric(_CourierDeclarationPM.CourierMasterId, _CourierMasterPM.Tenant);
                    if (sequenceNumericMax == null)
                    {
                        sequenceNumericMax = 0;
                    }
                    _CourierDeclarationPM.SequenceNumeric = sequenceNumericMax + 1;
                }
                _CourierDeclarationPM.Tenant = _CourierMasterPM.Tenant;

                _context = CustomContext.GetContext(ResolvedTenant());

                AppendLogLine("try to update CourierDeclaration for DeclarationPM.Id: " + _MyDeclarationPM.Id + " CourierMasterPM.Id: " + _CourierMasterPM.Id);
                try
                {
                    myCourierDeclarationUpdateService.Update(_CourierDeclarationPM, true);
                }
                catch (DbEntityValidationException ex)
                {
                    var FormatedException = ExceptionFormatUtil.GetFormated(ex);
                    AppendLogLine("ProccessRequest():Exception " + FormatedException.ToString() + Environment.NewLine + "---------------------------------------------");
                    return;
                }
                catch (Exception e)
                {
                    AppendLogLine("ProccessRequest():Exception " + e.ToString() + Environment.NewLine + "---------------------------------------------");
                    return;
                }
            }
        }



        private string TranslateIntegratorIndex(string integratorIndex)
        {
            if (String.IsNullOrWhiteSpace(integratorIndex))
            {
                AppendLogLine("integratorIndex is null");
                return null;
            }
            string integratorIndexId = null;

            CardQuery cardQuery = new CardQuery(ResolvedTenant());
            CardPM cardPM = cardQuery.GetSinglePMByCode(integratorIndex, ResolvedTenant());
            if (cardPM != null)
            {
                integratorIndexId = cardPM.Id;
            }
            else
            {
                AppendLogLine("integratorIndex = " + integratorIndex + " could not translate to Logitude Card Id");
                return null;
            }
            AppendLogLine("integratorIndex = " + integratorIndex + " Translated to Card Id" + integratorIndexId);
            return integratorIndexId;
        }

        private string TranslateStorageSite(string amitalstorageSiteCode)
        {
            if (String.IsNullOrWhiteSpace(amitalstorageSiteCode))
            {
                AppendLogLine("amitalDepartmentCode is null");
                return null;
            }
            var deliverySiteType = new DeliverySiteTypeRepository(ResolvedTenant());
            var myDeliverySite = deliverySiteType.GetSingle(amitalstorageSiteCode);
            if (myDeliverySite == null)
            {
                AppendLogLine("amitalstorageSiteCode = " + amitalstorageSiteCode + " could not translate to Logitude Id");
                return null;
            }
            AppendLogLine("amitalstorageSiteCode = " + amitalstorageSiteCode + " Translated to " + myDeliverySite.Code);
            return myDeliverySite.Code;
        }


        private string TranslateWeightValue(string weightValueCode)
        {
            if (String.IsNullOrWhiteSpace(weightValueCode))
            {
                AppendLogLine("weightValueCode is null");
                return null;
            }
            string weightValueCodeId = null;
            
            FreightPaymentMethodQueryService freightPaymentMethodQueryService = new FreightPaymentMethodQueryService(ResolvedTenant());
            FreightPaymentMethodPM freightPaymentMethodPM = freightPaymentMethodQueryService.GetSingle(weightValueCode, true, false);
            if (freightPaymentMethodPM != null && !freightPaymentMethodPM.Inactive)
            {
                weightValueCodeId = freightPaymentMethodPM.Code;
            }
            else
            {
                AppendLogLine("freightPaymentMethod = " + weightValueCode + " could not translate to Logitude Id");
                return null;
            }
            AppendLogLine("freightPaymentMethod = " + weightValueCode + " Translated to " + weightValueCodeId);
            return weightValueCodeId;
        }

        private string TranslateAirline(string airlineId)
        {
            if (String.IsNullOrWhiteSpace(airlineId))
            {
                AppendLogLine("airlineId is null");
                return null;
            }

            //GET Airline.Id BY PREFIX
            int index = airlineId.IndexOf('-');
            if (index > 0)
            {
                string code = airlineId.Substring(0, index);
                string prefix = airlineId.Substring(index + 1);
                CustomsAirlineRepository airlineRepository = new CustomsAirlineRepository(ResolvedTenantLocal());
                CustomsAirline airline = airlineRepository.GetByAirlineAndPrefix(code, prefix, ResolvedTenantLocal());
                if (airline != null) return airline.Id;
            }
            else
            {
                CustomsAirlineRepository airlineRepository = new CustomsAirlineRepository(ResolvedTenantLocal());
                CustomsAirline airline = airlineRepository.GetByPrefix(airlineId, ResolvedTenantLocal());
                if (airline != null) return airline.Id;
            }
            
            AppendLogLine("No Airline found for airlineId " + airlineId);
            return null;
        }

        private string TranslateInternationalSite(string internationalSite)
        {
            if (String.IsNullOrWhiteSpace(internationalSite))
            {
                AppendLogLine("internationalSite is null");
                return null;
            }
            string internationalSiteId = null;
            InternationalSiteQueryService internationalSiteQueryService = new InternationalSiteQueryService(ResolvedTenant());
            InternationalSitePM internationalSitePM = internationalSiteQueryService.GetSingle(internationalSite, true, false);
            if (internationalSitePM != null && !internationalSitePM.Inactive)
            {
                internationalSiteId = internationalSitePM.Code;
            }
            else
            {
                AppendLogLine("internationalSite = " + internationalSite + " could not translate to Logitude Id");
                return null;
            }
            AppendLogLine("internationalSite = " + internationalSite + " Translated to " + internationalSiteId);
            return internationalSiteId;
        }
        

        void DeserilazeObject(string xmlLOGIMASTERCOUR)
        {

            MyGenericResponseObj.Stage = "Initalize ProccessRequest";
            AppendLogLine("CommMasterCourierService.ProccessRequest");

            AppendLogLine("Deserialize(DataIn1) ..");

            if (string.IsNullOrWhiteSpace(xmlLOGIMASTERCOUR))
            {
                throw new BusinessErrorException("DataIn1 is missing");
            }
            if (xmlLOGIMASTERCOUR.Length > 1000)
            {
                AppendLogLine("XmlIn=" + xmlLOGIMASTERCOUR.Substring(0, 1000));
                AppendLogLine(".Substring(0, 1000)");
            }
            else
            {
                AppendLogLine("XmlIn=" + xmlLOGIMASTERCOUR);
            }

            AppendLogLine("Tring DeserilazeObject");
            MyGenericResponseObj.Stage = "Trying DeserilazeObject";
            this._LOGIMASTERCOUR = XmlGenericUtil<LOGIMASTERCOUR>.DeSerializeObject(xmlLOGIMASTERCOUR);

            if (_LOGIMASTERCOUR.LogitudeMasterCourier == null || _LOGIMASTERCOUR.LogitudeMasterCourier.Length != 1)
            {
                throw new BusinessErrorException("_LOGIMASTERCOUR.CommMasterCourier.Length != 1");
            }
            this._LogitudeMasterCourier = _LOGIMASTERCOUR.LogitudeMasterCourier[0];
        }

        public override string GetAssemblyQualifiedName()
        {
            throw new NotImplementedException();
        }

        public override string GetExampleDataIn1()
        {
            var xml = "";
            var amitalObjExample = new LOGIMASTERCOUR();
            var myAmitalCommMasterCourier = new LogitudeMasterCourier();
            
            myAmitalCommMasterCourier.AirlineId = "1-1";
            myAmitalCommMasterCourier.EstimatedArrivalDate = System.DateTime.Now.ToString();
            myAmitalCommMasterCourier.GatewayPortCode = "HKG";
            myAmitalCommMasterCourier.GrossMassMeasure = "1";
            myAmitalCommMasterCourier.HAWB = "12345678";
            myAmitalCommMasterCourier.MAWB = "114-12345678";
            myAmitalCommMasterCourier.OriginPortCode = "HKG";
            myAmitalCommMasterCourier.PackageQuantityTy = "1";
            myAmitalCommMasterCourier.Tenant = "1";

            amitalObjExample.LogitudeMasterCourier = new LogitudeMasterCourier[] { myAmitalCommMasterCourier };

            xml = XmlGenericUtil<LOGIMASTERCOUR>.SerializeObject(amitalObjExample);

            return xml;
        }

        public override string GetExampleDataIn2()
        {
            return "";
        }

        public override string GetExampleDataout1()
        {
            return "";
        }

        public override string GetExampleDataout2()
        {
            return "";
        }

        public override void ProccessRequest(string DataIn1, string DataIn2, out string DataOut1, out string DataOut2, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }

        public override void ProccessBASE64Request(string BASE64DataIn1, string BASE64DataIn2, string BASE64DataIn3, out string BASE64DataOut1, out string BASE64DataOut2, out string BASE64DataOut3, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }


        public string GetTranslationL2P(string partnerID, string tableID, string localCode)
        {
            var rec = (from a in amitalContext.GTRTRANs
                       where a.PARTNERID == partnerID && a.TABLEID == tableID && a.LOCALCODE == localCode
                       select a).FirstOrDefault();
            if (rec == null)
            {
                return null;
            }
            return rec.PARTNERCODE;
        }

    }
}

