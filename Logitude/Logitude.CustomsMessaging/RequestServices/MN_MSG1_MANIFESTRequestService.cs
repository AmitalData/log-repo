using Logitude.AmitalMessaging.Utils;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.RequestServices;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UnifreightIIG.Common.MANIFESTRequestServiceReference;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;


using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.EntityUpdateServices;
using Simplog.Server.Infrastructure;
using Logitude.CustomsMessaging.Helpers;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Server.Tools.Models;
using Unifreight.Data.AmitalModel.Repsitories;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.BL.BL;
using System.Diagnostics;
using Logitude.Customs.Data.EntityMapping;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class MN_MSG1_MANIFESTRequestService
        : RequestServiceBase<MN_MSG1_MANIFEST, MANIFESTRequestRequestParams>
    {
        private CardQuery _CardQuery;
        private PortQuery _PortQuery;
        private Tenant _Tenant;
        private string _MasterShipmentDataId;
        private CustomsVendorQueryService _CustomsVendorQueryService;
        private ClientQueryService _ClientQueryService;


        private ICustomContext _Context;
        private DeclarationPM _DeclarationPM;
        private CourierMasterPM _CourierMasterPM;
        private CourierDeclarationPM _CourierDeclarationPM;
        private ForbiddenSignsUtil _ForbiddenSignsUtil = new ForbiddenSignsUtil();
        private string _forbiddenSigns = "";
        public override void OnRequestFail(MANIFESTRequestRequestParams requestParams)
        {
            if (!String.IsNullOrWhiteSpace(requestParams.DeclarationId))
            {
                CalculateDeclarationCourierStatus.UpdateCourierManifestStatusCode(requestParams.Tenant, requestParams.DeclarationId);
            }

            base.OnRequestFail(requestParams);
        }

        public override MN_MSG1_MANIFEST GetRequest(MANIFESTRequestRequestParams requestParams)
        {
             _forbiddenSigns = _ForbiddenSignsUtil.GetForbiddenSigns(requestParams.Tenant);

            var myMN_MSG1_MANIFEST = new MN_MSG1_MANIFEST();
            _Context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationUpdateService = new DeclarationUpdateService(_Context, new Dictionary<string, IContext>(), requestParams.Tenant);

            //requestParams.Tenant = 1;
            //requestParams.ImportManifest = "1-1";

            var stopwatch = Stopwatch.StartNew();
            TenantRepository tenantRepository = new TenantRepository(requestParams.Tenant);
            _Tenant = tenantRepository.GetSingleTenant(requestParams.Tenant);
            LogMessagingUtil.Instance.AppendLine("GetSingleTenant:Elapsed:" + stopwatch.ElapsedMilliseconds);
            if (_Tenant == null)
            {
                return myMN_MSG1_MANIFEST;
            }

            myMN_MSG1_MANIFEST.Declaration = BuildDeclaration(requestParams);
           
            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            this.MyRequestSheetParam.EntityId1 = requestParams.LoggingEntityId;
            this.MyRequestSheetParam.RequestDescription = "מסר מניפסט";

            stopwatch = Stopwatch.StartNew();
            FeatureQuery featureQuery = new FeatureQuery(_DeclarationPM.Tenant);
            var features = featureQuery.GetAllowedFeaturesForLoggedUser(AuthenticationUtil.ResolveUserId(_DeclarationPM.Tenant), _DeclarationPM.Tenant);
            var feature = features.Features.FirstOrDefault(x => x.Code == "SendManifestEvent");
            LogMessagingUtil.Instance.AppendLine("GetAllowedFeaturesForLoggedUser:Elapsed:" + stopwatch.ElapsedMilliseconds);
            if (feature != null)
            {
                EventContextTagModel myEventContextTagModel = new EventContextTagModel()
                {
                    CallProccessID = EventContextTagModel.ProccessEnum.MN_MSG4_SendManifestFeedBack_MessageResponseService,
                    EventCode = "MNS",
                    EventRemarks = "Manifest Sent ",
                    StatusDateTime = DateTime.Now,
                };
                string loggingUserId = AuthenticationUtil.ResolveUserId(_DeclarationPM.Tenant);
                RaiseEvent(_DeclarationPM, loggingUserId, myEventContextTagModel);
            }


            stopwatch = Stopwatch.StartNew();
            //this._DeclarationPM.CurrentContextTag = myInsertEventContextTagModel;
            _DeclarationPM.ManifestCargoStatusCode = "4";
            _DeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
            myDeclarationUpdateService.Update(_DeclarationPM, true);
            LogMessagingUtil.Instance.AppendLine("Update:Elapsed:" + stopwatch.ElapsedMilliseconds);

            //var xml=XmlGenericUtil<MN_MSG1_MANIFEST>.SerializeObject(myMN_MSG1_MANIFEST);
            return myMN_MSG1_MANIFEST;
        }

        private void RaiseEvent(DeclarationPM dirtyDeclarationPM, string loggingUserId, EventContextTagModel myEventContextTagModel)
        {
            try
            {
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = dirtyDeclarationPM.Tenant,
                    objectTableName = "Customs.Declaration",
                    EventCode = myEventContextTagModel.EventCode,
                    notes = myEventContextTagModel.EventRemarks,
                    CommunicationLoggingEntityReference = dirtyDeclarationPM.DeclarationNumber,
                    EntityId = dirtyDeclarationPM.Id,
                    UserId = loggingUserId,

                    CommunicationSubject = "FU Status " + myEventContextTagModel.EventCode + " from logitude ",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = dirtyDeclarationPM.CustomFileNo,
                        status = "new",
                        xml_status = "new",
                        status_id = myEventContextTagModel.EventCode,
                        status_DateTime = DateTime.Now,
                        status_save = "no_fail",
                        comments = myEventContextTagModel.EventRemarks,
                    }
                };

                LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent  eventCode =" + myEventContextTagModel.EventCode + "  CustomFileNo= " + dirtyDeclarationPM.CustomFileNo + "   ");
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);

            }
            catch (System.Exception ex)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }

        }


        private UnifreightIIG.Common.MANIFESTRequestServiceReference.Declaration BuildDeclaration(MANIFESTRequestRequestParams requestParams)
        {
            UnifreightIIG.Common.MANIFESTRequestServiceReference.Declaration _DeclarationPM = new UnifreightIIG.Common.MANIFESTRequestServiceReference.Declaration();
            DeclarationPM OrgDeclaration=null;
            //Get Declaration
            DeclarationQueryService myDeclarationQueryService = new DeclarationQueryService(_Context);
            this._DeclarationPM = myDeclarationQueryService.GetSingle(requestParams.DeclarationId, true, false);
            if (this._DeclarationPM == null)
            {
                return _DeclarationPM;
            }
            CheckLock(requestParams, this._DeclarationPM);

            if(this._DeclarationPM.IsAmendment == true)
            {
                OrgDeclaration = myDeclarationQueryService.GetAcceptDeclarationAmendment(this._DeclarationPM.AmendmentOriginalDeclartation, this._DeclarationPM.Tenant);

            }

            string declarationId = this._DeclarationPM.Id;

            if (OrgDeclaration!= null) {
                declarationId = OrgDeclaration.Id;
            }
            //Get CourierDeclaration
            CourierDeclarationQueryService myCourierDeclarationQueryService = new CourierDeclarationQueryService(_Context);
            _CourierDeclarationPM = myCourierDeclarationQueryService.GetCourierDeclarationByDeclarationId(declarationId, _Tenant.Id);
            if (_CourierDeclarationPM == null)
            {
                return _DeclarationPM;
            }

            //Get CourierMaster
            CourierMasterQueryService myCourierMasterQueryService = new CourierMasterQueryService(_Context);
            _CourierMasterPM = myCourierMasterQueryService.GetSingle(_CourierDeclarationPM.CourierMasterId, false, false);
            if (_CourierMasterPM == null)
            {
                return _DeclarationPM;
            }


            _DeclarationPM.TypeCode = new DeclarationTypeCodeType() { Value = "785" };
            List<DeclarationAdditionalInformation> declarationAdditionalInformationList = new List<DeclarationAdditionalInformation>();
            DeclarationAdditionalInformation declarationAdditionalInformation = new DeclarationAdditionalInformation()
            {
                StatementCode = new AdditionalInformationStatementCodeType() { Value = "3" },
                StatementTypeCode = new AdditionalInformationStatementTypeCodeType() { Value = "1" }
            };
            declarationAdditionalInformationList.Add(declarationAdditionalInformation);
            _DeclarationPM.AdditionalInformation = declarationAdditionalInformationList.ToArray();



            if (!string.IsNullOrWhiteSpace(_CourierMasterPM.AirlinePrefix))
            {
                CustomsAirlineRepository airlineRepository = new CustomsAirlineRepository(_Tenant.Id);
                CustomsAirline airline = airlineRepository.GetByPrefix(_CourierMasterPM.AirlinePrefix, _Tenant.Id);
                if (airline != null)
                {
                    List<DeclarationCarrier> declarationCarrierList = new List<DeclarationCarrier>();
                    DeclarationCarrier declarationCarrier = new DeclarationCarrier()
                    {
                        ID = new CarrierIdentificationIDType() { Value = airline.ICAO },
                    };
                    declarationCarrierList.Add(declarationCarrier);
                    _DeclarationPM.Carrier = declarationCarrierList.ToArray();
                }

            }

            _DeclarationPM.ID = new DeclarationIdentificationIDType() { Value = _CourierMasterPM.ManifestNumber };
            _DeclarationPM.Submitter = new DeclarationSubmitter() { ID = new SubmitterIdentificationIDType() { Value = this._DeclarationPM.AgentId } };


            if (this._DeclarationPM.Consignments != null && this._DeclarationPM.Consignments.Count() > 0)
            {
                List<DeclarationConsignment> declarationConsignmentList = new List<DeclarationConsignment>();
                foreach (ConsignmentPM consignment in this._DeclarationPM.Consignments)
                {
                    DeclarationConsignment declarationConsignment = BuildConsignment(consignment, this._DeclarationPM);
                    declarationConsignmentList.Add(declarationConsignment);
                }
                _DeclarationPM.Consignment = declarationConsignmentList.ToArray();
            }

           
            return _DeclarationPM;
        }

        private void CheckLock(MANIFESTRequestRequestParams requestParams, DeclarationPM declarationPM)
        {
            var objectTableIdCourierMaster = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");
            if (requestParams.LoggingObjectTableId2 != objectTableIdCourierMaster)
            {
                return;
            }
            LogMessagingUtil.Instance.AppendLine("CourierMaster Send Batch===> CheckLock");

            long lCUSTOMFILENO;
            if (!long.TryParse(declarationPM.CustomFileNo, out lCUSTOMFILENO))
            {
                throw new BusinessErrorException("_DirtyDeclarationPaymentPM.DeclarationId could not convert to long ");
            }
            var myCCUFILEMRepository = new CCUFILEMRepository(declarationPM.Tenant);
            var ccufilem = myCCUFILEMRepository.GetFILENOByCUSTOMFILENO(lCUSTOMFILENO, declarationPM.Tenant);


            var myCCUQUELOCKRepository = new CCUQUELOCKRepository(requestParams.Tenant);
            try
            {
                var cculock = myCCUQUELOCKRepository.GetSingleGeneralLockNOWAIT("CCUFILEM", ccufilem.ToString());

            }
            catch (System.Exception)
            {

                LogMessagingUtil.Instance.AppendLine($"GetSingleGeneralLockNOWAIT(CCUFILEM, {ccufilem.ToString()}) ==> Already Lock => try later (*5) ");
                throw;
            }

        }

        private DeclarationConsignment BuildConsignment(ConsignmentPM consignmentPM, DeclarationPM declarationPM)
        {
            
            decimal decimalValue;
            DeclarationConsignment declarationConsignment = new DeclarationConsignment();

            declarationConsignment.SequenceNumericSpecified = false;
            if (decimal.TryParse(_CourierDeclarationPM.SequenceNumeric.ToString(), out decimalValue))
            {
                declarationConsignment.SequenceNumeric = decimalValue;
                declarationConsignment.SequenceNumericSpecified = true;
            }

            List<DeclarationConsignmentConsignmentItem> declarationConsignmentConsignmentItemList = new List<DeclarationConsignmentConsignmentItem>();
            decimal totalPackageQuantity = 0;
            if (consignmentPM.ConsignmentPackages != null && consignmentPM.ConsignmentPackages.Count() > 0)
            {
                foreach (ConsignmentPackagePM consignmentPackagePM in consignmentPM.ConsignmentPackages)
                {
                    if (consignmentPackagePM.PackageMeasureQualifierCode == "2")
                    {
                        DeclarationConsignmentConsignmentItem declarationConsignmentConsignmentItem = new DeclarationConsignmentConsignmentItem();
                        declarationConsignmentConsignmentItem = BuildConsignmentItem(consignmentPM, consignmentPackagePM);
                        if(consignmentPackagePM.SequenceNumeric != null && consignmentPackagePM.SequenceNumeric.HasValue)
                        {
                            declarationConsignmentConsignmentItem.SequenceNumeric = 1;//consignmentPackagePM.SequenceNumeric.Value;
                            declarationConsignmentConsignmentItem.SequenceNumericSpecified = true;
                        }
                        declarationConsignmentConsignmentItemList.Add(declarationConsignmentConsignmentItem);

                        decimal packageQuantity = 0;
                        if (consignmentPackagePM.PackageQuantity.HasValue)
                        {
                            if (decimal.TryParse(consignmentPackagePM.PackageQuantity.ToString(), out packageQuantity))
                            {
                                totalPackageQuantity = totalPackageQuantity + packageQuantity;
                            }
                        }
                    }
                }
            }
            declarationConsignment.ConsignmentItem = declarationConsignmentConsignmentItemList.ToArray();
            declarationConsignment.TotalPackageQuantity = new ConsignmentTotalPackageQuantityType() { Value = totalPackageQuantity };

            List<DeclarationConsignmentAcceptancePlace> declarationConsignmentAcceptancePlaceList = new List<DeclarationConsignmentAcceptancePlace>();
            DeclarationConsignmentAcceptancePlace declarationConsignmentAcceptancePlace = new DeclarationConsignmentAcceptancePlace() { Name = new AcceptancePlaceNameTextType() { Value = _CourierMasterPM.OriginPortCode } };
            declarationConsignmentAcceptancePlaceList.Add(declarationConsignmentAcceptancePlace);
            declarationConsignment.AcceptancePlace = declarationConsignmentAcceptancePlaceList.ToArray();
            declarationConsignment.Consignee = GetConsignee();
            declarationConsignment.Consignor = GetConsignor();

            DecDangersContactPM decDangersContactPM =null;

            if (declarationPM.DecDangersContacts != null && declarationPM.DecDangersContacts.Count > 0)
            { decDangersContactPM = declarationPM.DecDangersContacts[0]; }

            if (decDangersContactPM != null)
            //DeclarationConsignmentUNDGContact
            {
                DeclarationConsignmentUNDGContact[] declarationConsignmentUNDGContacts = new DeclarationConsignmentUNDGContact[] {
                new DeclarationConsignmentUNDGContact
                {
                    Name = new UNDGContactNameTextType() { Value = decDangersContactPM.CompanyName },
                    Communication = new DeclarationConsignmentUNDGContactCommunication[]{
                 new DeclarationConsignmentUNDGContactCommunication {
                    ID =  new CommunicationIdentificationIDType (){Value =decDangersContactPM.CompanyCommNumber},
                    TypeID = new CommunicationTypeIDType (){ Value=decDangersContactPM.CompanyCommTypeCode} }
                },
                    Contact = new DeclarationConsignmentUNDGContactContact[] {
                    new DeclarationConsignmentUNDGContactContact{
                        Name = new ContactNameTextType { Value = _ForbiddenSignsUtil.ReplaceForbiddenChars(decDangersContactPM.ContactName, _forbiddenSigns) } ,
                        Communication =new DeclarationConsignmentUNDGContactContactCommunication[]
                    { new DeclarationConsignmentUNDGContactContactCommunication {
                                            TypeID = new CommunicationTypeIDType { Value = decDangersContactPM.ContactCommTypeCode },
                                            ID = new CommunicationIdentificationIDType { Value = decDangersContactPM.ContactCommNumber }

                    } }

                    } }
                }};

 

                declarationConsignment.UNDGContact = declarationConsignmentUNDGContacts;

            }


            List<DeclarationConsignmentFreight> declarationConsignmentFreightList = new List<DeclarationConsignmentFreight>();
            //DeclarationConsignmentFreight declarationConsignmentFreight = new DeclarationConsignmentFreight() { PaymentMethodCode = new FreightPaymentMethodCodeType() { Value = _DeclarationPM.SupplierInvoices.First().IncotermCode } };
            DeclarationConsignmentFreight declarationConsignmentFreight = new DeclarationConsignmentFreight() { PaymentMethodCode = new FreightPaymentMethodCodeType() { Value = _CourierMasterPM.WeightValueCode } };
            declarationConsignmentFreightList.Add(declarationConsignmentFreight);
            declarationConsignment.Freight = declarationConsignmentFreightList.ToArray();

            List<DeclarationConsignmentGoodsConsignedPlace> declarationConsignmentGoodsConsignedPlaceList = new List<DeclarationConsignmentGoodsConsignedPlace>();
            DeclarationConsignmentGoodsConsignedPlace declarationConsignmentGoodsConsignedPlace = new DeclarationConsignmentGoodsConsignedPlace() { ID = new GoodsConsignedPlaceIdentificationIDType() { Value = _CourierMasterPM.GatewayPortCode } };
            declarationConsignmentGoodsConsignedPlaceList.Add(declarationConsignmentGoodsConsignedPlace);
            declarationConsignment.GoodsConsignedPlace = declarationConsignmentGoodsConsignedPlaceList.ToArray();

            List<DeclarationConsignmentGoodsReceiptPlace> declarationConsignmentGoodsReceiptPlaceList = new List<DeclarationConsignmentGoodsReceiptPlace>();
            DeclarationConsignmentGoodsReceiptPlace declarationConsignmentGoodsReceiptPlace = new DeclarationConsignmentGoodsReceiptPlace() { ID = new GoodsReceiptPlaceIdentificationIDType() { Value = consignmentPM.StorageSiteCode } };
            declarationConsignmentGoodsReceiptPlaceList.Add(declarationConsignmentGoodsReceiptPlace);
            declarationConsignment.GoodsReceiptPlace = declarationConsignmentGoodsReceiptPlaceList.ToArray();

            List<DeclarationConsignmentGovernmentAgencyGoodsItem> declarationConsignmentGovernmentAgencyGoodsItemList = new List<DeclarationConsignmentGovernmentAgencyGoodsItem>();
            DeclarationConsignmentGovernmentAgencyGoodsItem declarationConsignmentGovernmentAgencyGoodsItem = new DeclarationConsignmentGovernmentAgencyGoodsItem();
            List<DeclarationConsignmentGovernmentAgencyGoodsItemAdditionalInformation> declarationConsignmentGovernmentAgencyGoodsItemAdditionalInformationList = new List<DeclarationConsignmentGovernmentAgencyGoodsItemAdditionalInformation>();

            if (!string.IsNullOrWhiteSpace(consignmentPM.ThirdCargoID))
            {
                DeclarationConsignmentGovernmentAgencyGoodsItemAdditionalInformation declarationConsignmentGovernmentAgencyGoodsItemAdditionalInformation = new DeclarationConsignmentGovernmentAgencyGoodsItemAdditionalInformation()
                {
                    //Content = new AdditionalInformationContentTextType() { Value = consignmentPM.ThirdCargoID },
                    Content = new AdditionalInformationContentTextType(),
                    StatementTypeCode = new AdditionalInformationStatementTypeCodeType() { Value = "20" }
                };
                DateTime? datetime = AmitalConvertUtil.GetUnifreightFormatedDate(consignmentPM.ThirdCargoID, "consignmentPM.ThirdCargoID");
                declarationConsignmentGovernmentAgencyGoodsItemAdditionalInformation.Content.Value = datetime.HasValue ? datetime.Value.ToString("ddMMyy") : "";

                declarationConsignmentGovernmentAgencyGoodsItemAdditionalInformationList.Add(declarationConsignmentGovernmentAgencyGoodsItemAdditionalInformation);
                declarationConsignmentGovernmentAgencyGoodsItem.AdditionalInformation = declarationConsignmentGovernmentAgencyGoodsItemAdditionalInformationList.ToArray();
                declarationConsignmentGovernmentAgencyGoodsItemList.Add(declarationConsignmentGovernmentAgencyGoodsItem);
            }
            declarationConsignment.GovernmentAgencyGoodsItem = declarationConsignmentGovernmentAgencyGoodsItemList.ToArray();

            List<DeclarationConsignmentTransportContractDocument> declarationConsignmentTransportContractDocumentList = new List<DeclarationConsignmentTransportContractDocument>();
            DeclarationConsignmentTransportContractDocument declarationConsignmentTransportContractDocument = new DeclarationConsignmentTransportContractDocument()
            {
                ID = new TransportContractDocumentIdentificationIDType() { Value = consignmentPM.ManifestNumber },
                TypeCode = new TransportContractDocumentTypeCodeType() { Value = "ILD" }
            };
            declarationConsignmentTransportContractDocumentList.Add(declarationConsignmentTransportContractDocument);
            declarationConsignmentTransportContractDocument = new DeclarationConsignmentTransportContractDocument()
            {
                ID = new TransportContractDocumentIdentificationIDType() { Value = _CourierMasterPM.MAWB },
                TypeCode = new TransportContractDocumentTypeCodeType() { Value = _CourierMasterPM.MAWBTypeCode }
            };
            if (!string.IsNullOrWhiteSpace(_CourierMasterPM.AirlinePrefix))
            {
                declarationConsignmentTransportContractDocument.ID.Value = _CourierMasterPM.AirlinePrefix + "-" + declarationConsignmentTransportContractDocument.ID.Value;
            }

            declarationConsignmentTransportContractDocumentList.Add(declarationConsignmentTransportContractDocument);
            declarationConsignment.TransportContractDocument = declarationConsignmentTransportContractDocumentList.ToArray();

            //List<DeclarationConsignmentUnloadingLocation> declarationConsignmentUnloadingLocationList = new List<DeclarationConsignmentUnloadingLocation>();
            //DeclarationConsignmentUnloadingLocation declarationConsignmentUnloadingLocation = new DeclarationConsignmentUnloadingLocation();
            //declarationConsignmentUnloadingLocation.ArrivalDateTime = consignmentPM.UnloadDate.HasValue ? DataTypeConvertorUtil.Convert(consignmentPM.UnloadDate.Value) : null;
            //declarationConsignmentUnloadingLocation.ID = new UnloadingLocationIdentificationIDType() { Value = consignmentPM.UnloadPortCode };
            //declarationConsignmentUnloadingLocationList.Add(declarationConsignmentUnloadingLocation);
            //declarationConsignment.UnloadingLocation = declarationConsignmentUnloadingLocationList.ToArray();

            return declarationConsignment;
        }

        private DeclarationConsignmentConsignee[] GetConsignee()
        {
         
            List<DeclarationConsignmentConsignee> declarationConsignmentConsigneeList = new List<DeclarationConsignmentConsignee>();
            DeclarationConsignmentConsignee declarationConsignmentConsignee = new DeclarationConsignmentConsignee();

          
            declarationConsignmentConsignee.Name = new ConsigneeNameTextType() { Value = _ForbiddenSignsUtil.ReplaceForbiddenChars(_DeclarationPM.ImporterName, _forbiddenSigns) };
            List<DeclarationConsignmentConsigneeAddress> declarationConsignmentConsigneeAddressList = new List<DeclarationConsignmentConsigneeAddress>();
            DeclarationConsignmentConsigneeAddress declarationConsignmentConsigneeAddress = new DeclarationConsignmentConsigneeAddress();
            if (!string.IsNullOrWhiteSpace(_DeclarationPM.ImporterAddress))
            {
                declarationConsignmentConsigneeAddress = new DeclarationConsignmentConsigneeAddress()
                {
                    Line = new AddressLineTextType() { Value = _ForbiddenSignsUtil.ReplaceForbiddenChars(_DeclarationPM.ImporterAddress, _forbiddenSigns)  }
                };
            }

            if (!string.IsNullOrEmpty(declarationConsignmentConsigneeAddress?.Line?.Value) && declarationConsignmentConsigneeAddress.Line.Value.Length > 70)
            {
                declarationConsignmentConsigneeAddress.Line.Value = declarationConsignmentConsigneeAddress.Line.Value.Substring(0, 70);
                declarationConsignmentConsigneeAddress.Line.Value = _ForbiddenSignsUtil.ReplaceForbiddenChars(declarationConsignmentConsigneeAddress.Line.Value, _forbiddenSigns);
            }

            declarationConsignmentConsigneeAddressList.Add(declarationConsignmentConsigneeAddress);
            declarationConsignmentConsignee.Address = declarationConsignmentConsigneeAddressList.ToArray();

            if (!string.IsNullOrWhiteSpace(_DeclarationPM.CasualImporterTel))
            {
                declarationConsignmentConsignee.Communication = new DeclarationConsignmentConsigneeCommunication[1] {
                    new DeclarationConsignmentConsigneeCommunication() {
                 ID  =  new CommunicationIdentificationIDType() {Value = _DeclarationPM.CasualImporterTel },
                TypeID= new CommunicationTypeIDType() { Value = "TE" } }

            };
            }

            //}
            declarationConsignmentConsigneeList.Add(declarationConsignmentConsignee);
            return declarationConsignmentConsigneeList.ToArray();
        }

        private DeclarationConsignmentConsignor[] GetConsignor()
        {
          
            List<DeclarationConsignmentConsignor> declarationConsignmentConsignorList = new List<DeclarationConsignmentConsignor>();
            DeclarationConsignmentConsignor declarationConsignmentConsignor = new DeclarationConsignmentConsignor();
            CustomsVendorPM customsVendorPM = null;
            if (_DeclarationPM.SupplierInvoices != null && _DeclarationPM.SupplierInvoices.Count() > 0)
            {
                if (!string.IsNullOrWhiteSpace(_DeclarationPM.SupplierInvoices.First().VendorId))
                {
                    _CustomsVendorQueryService = new CustomsVendorQueryService(_Tenant.Id);
                    customsVendorPM = _CustomsVendorQueryService.GetSingle(_DeclarationPM.SupplierInvoices.First().VendorId, false, false);
                }
            }
            if (customsVendorPM != null)
            {
                //declarationConsignmentConsignor.ID = new ConsignorIdentificationIDType() { Value = customsVendorPM.VendorNumber };
                declarationConsignmentConsignor.Name = new ConsignorNameTextType() { Value = _ForbiddenSignsUtil.ReplaceForbiddenChars(customsVendorPM.VendorName, _forbiddenSigns) };
 
                List<DeclarationConsignmentConsignorAddress> declarationConsignmentConsignorAddressList = new List<DeclarationConsignmentConsignorAddress>();
                DeclarationConsignmentConsignorAddress declarationConsignmentConsignorAddress = new DeclarationConsignmentConsignorAddress();

                declarationConsignmentConsignorAddress = new DeclarationConsignmentConsignorAddress()
                {
                    Line = new AddressLineTextType() { Value = customsVendorPM.MainAddressLine }
                };
                if (!string.IsNullOrWhiteSpace(customsVendorPM.CityName))
                {
                    declarationConsignmentConsignorAddress.Line.Value = declarationConsignmentConsignorAddress.Line.Value + " " + customsVendorPM.CityName;
                }
                if (!string.IsNullOrWhiteSpace(customsVendorPM.CountryCode))
                {
                    declarationConsignmentConsignorAddress.Line.Value = declarationConsignmentConsignorAddress.Line.Value + " " + customsVendorPM.CountryCode;

                }


                  if (!string.IsNullOrEmpty( declarationConsignmentConsignorAddress.Line.Value) && declarationConsignmentConsignorAddress.Line.Value.Length>70)
                    {
                    declarationConsignmentConsignorAddress.Line.Value = declarationConsignmentConsignorAddress.Line.Value.Substring(0, 70);
                    }
                declarationConsignmentConsignorAddress.Line.Value = _ForbiddenSignsUtil.ReplaceForbiddenChars(declarationConsignmentConsignorAddress.Line.Value, _forbiddenSigns);

                declarationConsignmentConsignorAddressList.Add(declarationConsignmentConsignorAddress);
                declarationConsignmentConsignor.Address = declarationConsignmentConsignorAddressList.ToArray();
            }
            else
            {
                declarationConsignmentConsignor.Name = new ConsignorNameTextType() { Value = _ForbiddenSignsUtil.ReplaceForbiddenChars(_DeclarationPM.CasualSupplierName, _forbiddenSigns) };
                List<DeclarationConsignmentConsignorAddress> declarationConsignmentConsignorAddressList = new List<DeclarationConsignmentConsignorAddress>();
                DeclarationConsignmentConsignorAddress declarationConsignmentConsignorAddress = new DeclarationConsignmentConsignorAddress();
                if (!string.IsNullOrWhiteSpace(_DeclarationPM.CasualSupplierAddress))
                {
                    declarationConsignmentConsignorAddress = new DeclarationConsignmentConsignorAddress()
                    {
                        Line = new AddressLineTextType() { Value = _DeclarationPM.CasualSupplierAddress }
                    };
                }

                if (!string.IsNullOrEmpty(declarationConsignmentConsignorAddress.Line.Value) && declarationConsignmentConsignorAddress.Line.Value.Length > 70)
                {
                    declarationConsignmentConsignorAddress.Line.Value = declarationConsignmentConsignorAddress.Line.Value.Substring(0, 70);
                }

                declarationConsignmentConsignorAddress.Line.Value = _ForbiddenSignsUtil.ReplaceForbiddenChars(declarationConsignmentConsignorAddress.Line.Value, _forbiddenSigns);

                declarationConsignmentConsignorAddressList.Add(declarationConsignmentConsignorAddress);
                declarationConsignmentConsignor.Address = declarationConsignmentConsignorAddressList.ToArray();
            }

            declarationConsignmentConsignorList.Add(declarationConsignmentConsignor);
            return declarationConsignmentConsignorList.ToArray();
        }

        private DeclarationConsignmentConsignmentItem BuildConsignmentItem(ConsignmentPM consignmentPM, ConsignmentPackagePM consignmentPackagePM)
        {
           

            DeclarationConsignmentConsignmentItem declarationConsignmentConsignmentItem = new DeclarationConsignmentConsignmentItem();

            ConsignmentPackDangerPM consignmentPackDangerPM= null;
            
            if (consignmentPackagePM.ConsignmentPackDangers != null && consignmentPackagePM.ConsignmentPackDangers.Count>0)
            { consignmentPackDangerPM=consignmentPackagePM.ConsignmentPackDangers[0];}
              

            declarationConsignmentConsignmentItem.GoodsStatusCode = new ConsignmentItemGoodsStatusCodeType() { Value = "N" };
            if(consignmentPackDangerPM!=null)
            {
             if (consignmentPackDangerPM.UNCode !="" || consignmentPackDangerPM.FlashpointTemperature !=""
                || consignmentPackDangerPM.StorageTemperature != ""|| consignmentPackDangerPM.DangerousGoodsPackingReqCode!="")
            {
                declarationConsignmentConsignmentItem.GoodsStatusCode.Value = "D";
            }

            }

 

            List<DeclarationConsignmentConsignmentItemCommodity> declarationConsignmentUnloadingLocationList = new List<DeclarationConsignmentConsignmentItemCommodity>();
            DeclarationConsignmentConsignmentItemCommodity declarationConsignmentUnloadingLocation = new DeclarationConsignmentConsignmentItemCommodity();

                if (consignmentPackDangerPM != null)
                {
                    declarationConsignmentUnloadingLocation = new DeclarationConsignmentConsignmentItemCommodity()
                    {
                        CargoDescription = new CommodityCargoDescriptionTextType() { Value = _ForbiddenSignsUtil.ReplaceForbiddenChars(consignmentPM.CargoDescription, _forbiddenSigns)},

                        Classification = new DeclarationConsignmentConsignmentItemCommodityClassification[]
                   { new DeclarationConsignmentConsignmentItemCommodityClassification {
                       ID = new ClassificationIdentificationIDType { Value = consignmentPackDangerPM.UNCode },
                       
                       IdentificationTypeCode = new ClassificationIdentificationTypeCodeType { Value = "SSO" }
                      
                   },
                    new DeclarationConsignmentConsignmentItemCommodityClassification {
                       ID = new ClassificationIdentificationIDType { Value =! string.IsNullOrEmpty(consignmentPackDangerPM.ClassificationFourDigit) ? consignmentPackDangerPM.ClassificationFourDigit:"" },

                       IdentificationTypeCode = new ClassificationIdentificationTypeCodeType { Value = "HS" }

                   }
                   }
                   ,
                        CommodityRelatedPackaging = new DeclarationConsignmentConsignmentItemCommodityCommodityRelatedPackaging[]
                   {
                        new DeclarationConsignmentConsignmentItemCommodityCommodityRelatedPackaging
                        {
                            DangerousGoodsPackingRequirementGroupCode = new CommodityRelatedPackagingDangerousGoodsPackingRequirementGroupCodeType
                            {
                                Value =    consignmentPackDangerPM.DangerousGoodsPackingReqCode
                            }
                        }
                   }
                   ,
                        Temperature = new DeclarationConsignmentConsignmentItemCommodityTemperature[]
                   { new DeclarationConsignmentConsignmentItemCommodityTemperature
                    {
                        FlashpointMeasure = new TemperatureFlashpointMeasureType
                        {
                            Value = Convert.ToDecimal(consignmentPackDangerPM.FlashpointTemperature)

                        } ,
                        StorageRequirementMeasure= new TemperatureStorageRequirementMeasureType
                        {
                            Value = Convert.ToDecimal(consignmentPackDangerPM.StorageTemperature)

                        }
                    }


                   }

                    };

                }
                else
                {
                    declarationConsignmentUnloadingLocation = new DeclarationConsignmentConsignmentItemCommodity()
                    {
                        CargoDescription = new CommodityCargoDescriptionTextType() { Value = _ForbiddenSignsUtil.ReplaceForbiddenChars(consignmentPM.CargoDescription, _forbiddenSigns)  },
                    };
                }
                declarationConsignmentUnloadingLocationList.Add(declarationConsignmentUnloadingLocation);
          
            declarationConsignmentConsignmentItem.Commodity = declarationConsignmentUnloadingLocationList.ToArray();

            decimal grossMassMeasure;
            if (consignmentPackagePM.GrossMassMeasure.HasValue)
            {
                if (decimal.TryParse(consignmentPackagePM.GrossMassMeasure.ToString(), out grossMassMeasure))
                {
                    List<DeclarationConsignmentConsignmentItemGoodsMeasure> declarationConsignmentConsignmentItemGoodsMeasureList = new List<DeclarationConsignmentConsignmentItemGoodsMeasure>();
                    declarationConsignmentConsignmentItemGoodsMeasureList.Add(new DeclarationConsignmentConsignmentItemGoodsMeasure()
                    {
                        GrossMassMeasure = new GoodsMeasureGrossMassMeasureType() { Value = grossMassMeasure }
                    });
                    declarationConsignmentConsignmentItem.GoodsMeasure = declarationConsignmentConsignmentItemGoodsMeasureList.ToArray();
                }
            }

            List<DeclarationConsignmentConsignmentItemGovernmentProcedure> declarationConsignmentConsignmentItemGovernmentProcedureList = new List<DeclarationConsignmentConsignmentItemGovernmentProcedure>();
            declarationConsignmentConsignmentItemGovernmentProcedureList.Add(new DeclarationConsignmentConsignmentItemGovernmentProcedure()
            {
                CurrentCode = new GovernmentProcedureCurrentCodeType() { Value = "4000000" }
            });
            declarationConsignmentConsignmentItem.GovernmentProcedure = declarationConsignmentConsignmentItemGovernmentProcedureList.ToArray();

            decimal quantity;
            if (consignmentPackagePM.PackageQuantity.HasValue)
            {
                if (decimal.TryParse(consignmentPackagePM.PackageQuantity.ToString(), out quantity))
                {
                    List<DeclarationConsignmentConsignmentItemPackaging> declarationConsignmentConsignmentItemPackagingList = new List<DeclarationConsignmentConsignmentItemPackaging>();
                    declarationConsignmentConsignmentItemPackagingList.Add(new DeclarationConsignmentConsignmentItemPackaging()
                    {
                        QuantityQuantity = new PackagingQuantityQuantityType() { Value = quantity },
                        TypeCode = new PackagingTypeCodeType() { Value = consignmentPackagePM.PackageTypeCode }
                    });
                    declarationConsignmentConsignmentItem.Packaging = declarationConsignmentConsignmentItemPackagingList.ToArray();
                }
            }

            return declarationConsignmentConsignmentItem;
        }

        public string MasterShipmentDataId
        {
            get { return _MasterShipmentDataId; }
            set { _MasterShipmentDataId = value; }
        }

        public override void PostGetRequest(MN_MSG1_MANIFEST customRequest, MANIFESTRequestRequestParams requestParams)
        {
        
            ICustomContext context = CustomContext.GetContext(requestParams.Tenant);
            var declarationQueryService = new DeclarationQueryService(context);
            DeclarationPM declarationPM = null;
            if (!string.IsNullOrEmpty(requestParams.DeclarationId))
            {
                declarationPM = declarationQueryService.GetSingle(requestParams.DeclarationId, false, false);
                if (declarationPM == null && !string.IsNullOrEmpty(requestParams.LoggingEntityId))
                {
                    declarationPM = declarationQueryService.GetSingle(requestParams.LoggingEntityId, false, false);
                }
            }

            if (declarationPM != null && declarationPM.IsCourierDeclaration)
            {
                DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
                DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
                DeclarationCourierStatusPM currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(declarationPM.Id, true, false);
                if (currentDeclarationCourierStatusPM == null)
                {
                    currentDeclarationCourierStatusPM = new DeclarationCourierStatusPM()
                    {
                        DeclarationId = declarationPM.Id,
                        Tenant = declarationPM.Tenant,
                        IsClosedForFollowUp = false,
                        IsCourierMissingClassification = false,
                        CargoDescription =  declarationPM.CargoDescription,
                        ImporterName =  declarationPM.CargoDescription ,
                    };
                    currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Insert;
                }
                else
                {
                    currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                }
                currentDeclarationCourierStatusPM.CourierManifestStatusCode = "I";
                declarationCourierStatusUpdateService.Update(currentDeclarationCourierStatusPM, true);
            }
        }
    }
}