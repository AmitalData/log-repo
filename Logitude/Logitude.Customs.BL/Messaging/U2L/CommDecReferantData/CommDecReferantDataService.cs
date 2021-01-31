//using Logitude.AmitalMessaging.Customs.
using Logitude.AmitalMessaging.Customs.CustomFile.DeclarationReferantData;
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.AmitalMessaging.Utils;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.Messaging.U2L.ImportDeclaration;
using Logitude.Customs.Data;
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

namespace Logitude.Customs.BL.Messaging.U2L.CommDecReferantData
{
    public class CommDecReferantDataService : UnifreightGenericService
    {
        private LOGIDECREFERANTDATA _LOGIDECREFERANTDATA;
        private LogitudeDeclarationReferantData _LogitudeDeclarationReferantData;
        private DeclarationReferantDataPM _DeclarationReferantDataPM;
        private ICustomContext _context;
        private DeclarationPM _MyDeclarationPM;

        private AmitalContext amitalContext;

        public const string UpsertActionConst = "Logitude.Customs.BL.Messaging.U2L.CommDecReferantData.CommDecReferantDataService.Upsert()";

        private Stopwatch _Stopwatch;

        public CommDecReferantDataService()
            : base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
            true
            )
        {

        }

        protected int ResolvedTenantLocal()
        {
            if (!string.IsNullOrWhiteSpace(_LOGIDECREFERANTDATA.LogitudeDeclarationReferantData[0].Tenant) && int.Parse(_LOGIDECREFERANTDATA.LogitudeDeclarationReferantData[0].Tenant) > 0)
            {
                return int.Parse(_LOGIDECREFERANTDATA.LogitudeDeclarationReferantData[0].Tenant);
            }

            return ResolvedTenant();
        }

        public override void ProccessGenericRequest(
              string xmlLOGIDECREFERANTDATA,
              ref string MoreParams,
              out string MessageOut)
        {
            MessageOut = "";
            _Stopwatch = Stopwatch.StartNew();
            MyCommunicationsParams.Subject = "CommDecReferantDataService ";

            DeserilazeObject(xmlLOGIDECREFERANTDATA);
            AppendLogLine("DeserilazeObject:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

            //CheckIntegrity();
            AppendLogLine("CheckIntegrity:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
            MyGenericResponseObj.Stage = "GetContext";
            MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.Success;
            _context = CustomContext.GetContext(ResolvedTenant());
            amitalContext = AmitalContext.GetContext(ResolvedTenant());


            ICustomContext dbContext = CustomContext.GetContext(ResolvedTenant());
            MyGenericResponseObj.Stage = "DeclarationReferantDataUpsert";

            try
            {
                var myQueryService = new DeclarationReferantDataQueryService(_context);
                var myDeclarationReferantDataUpdateService = new DeclarationReferantDataUpdateService(_context, new Dictionary<string, IContext>(), ResolvedTenantLocal());

                MyGenericResponseObj.Stage = "Check integrity ";
                ///must 

                if (String.IsNullOrWhiteSpace(_LogitudeDeclarationReferantData.Id))
                {
                    MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                    MyGenericResponseObj.Message = "Id is missing";
                    AppendLogLine(MyGenericResponseObj.Message);
                    return;
                }

                MyGenericResponseObj.Stage = "GetSingle";
                this._DeclarationReferantDataPM = myQueryService.GetSingle(_LogitudeDeclarationReferantData.Id, true, false);
                /// Exist
                if (_DeclarationReferantDataPM == null)
                {
                    var myDecQueryService = new DeclarationQueryService(_context);
                    this._MyDeclarationPM = myDecQueryService.GetSingle(_LogitudeDeclarationReferantData.Id, true, false);
                    if (this._MyDeclarationPM != null)
                    {
                        DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(dbContext, new Dictionary<string, IContext>(), ResolvedTenant());
                        if (!declarationUpdateService.CheckIfUpdatingAllowed(this._MyDeclarationPM))
                        {
                            AppendLogLine("Updating Not Allowed For Declaration " + this._MyDeclarationPM.CustomFileNo + Environment.NewLine + Logitude.Server.Tools.Helpers.LogMessagingUtil.Instance.ToString(1000));
                            return;
                        }
                    }
                    else
                    {
                        MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                        MyGenericResponseObj.Message = "No Declaration for Id " + _LogitudeDeclarationReferantData.Id;
                        AppendLogLine(MyGenericResponseObj.Message);
                        return;
                    }
                    this._DeclarationReferantDataPM = new Def.EntityPMs.DeclarationReferantDataPM();
                    this._DeclarationReferantDataPM.ChangeSetOp = ChangeSetOperation.Insert;
                    this._DeclarationReferantDataPM.DeclarationId = this._MyDeclarationPM.Id;
                }
                else
                {
                    this._DeclarationReferantDataPM.ChangeSetOp = ChangeSetOperation.Update;
                }
                if (_LogitudeDeclarationReferantData.QueueType == "Q2")
                {
                    _DeclarationReferantDataPM.ClassificationStatus = _LogitudeDeclarationReferantData.QueueStatus;
                    if (!string.IsNullOrWhiteSpace(_LogitudeDeclarationReferantData.QueueRemarks)) _DeclarationReferantDataPM.IsClassificationRemarks = true;
                }
                else if (_LogitudeDeclarationReferantData.QueueType == "Q3")
                {
                    _DeclarationReferantDataPM.ControllerStatus = _LogitudeDeclarationReferantData.QueueStatus;
                    if (!string.IsNullOrWhiteSpace(_LogitudeDeclarationReferantData.QueueRemarks)) _DeclarationReferantDataPM.IsControllerRemarks = true;
                }
                else if (_LogitudeDeclarationReferantData.QueueType == "Q9")
                {
                    _DeclarationReferantDataPM.CollectionOfMoneyStatus = _LogitudeDeclarationReferantData.QueueStatus;
                }
                if (!string.IsNullOrWhiteSpace(_LogitudeDeclarationReferantData.FollowUpStatus)) _DeclarationReferantDataPM.IsClosedForFollowUp = _LogitudeDeclarationReferantData.FollowUpStatus;

                if (!string.IsNullOrWhiteSpace(_LogitudeDeclarationReferantData.PreClassification))
                {
                    if(_LogitudeDeclarationReferantData.PreClassification == "D")
                    {
                        _DeclarationReferantDataPM.PreClassification = null;
                    }
                    else
                    {
                        _DeclarationReferantDataPM.PreClassification = _LogitudeDeclarationReferantData.PreClassification;
                    }
                }
                if (_DeclarationReferantDataPM.Tenant < 1) _DeclarationReferantDataPM.Tenant = ResolvedTenant();
                if (!string.IsNullOrWhiteSpace(_LogitudeDeclarationReferantData.ClassifiedUserId)) _DeclarationReferantDataPM.ClassifiedUserId = TranslateUser(_LogitudeDeclarationReferantData.ClassifiedUserId);
                if (!string.IsNullOrWhiteSpace(_LogitudeDeclarationReferantData.ControllerUserId)) _DeclarationReferantDataPM.ControllerUserId = TranslateUser(_LogitudeDeclarationReferantData.ControllerUserId);
                if (_LogitudeDeclarationReferantData.FileStatus == "OPT") _DeclarationReferantDataPM.NewFile = false;
                if (!string.IsNullOrWhiteSpace(_LogitudeDeclarationReferantData.LastStatusName)) _DeclarationReferantDataPM.LastStatusName = _LogitudeDeclarationReferantData.LastStatusName;
                var tempDate = AmitalConvertUtil.GetUnifreightFormatedDate(_LogitudeDeclarationReferantData.LastStatusDate, "_LogitudeDeclarationReferantData.LastStatusDate");
                if (!string.IsNullOrWhiteSpace(_LogitudeDeclarationReferantData.LastStatusDate) && tempDate.HasValue) _DeclarationReferantDataPM.LastStatusDate = tempDate.Value;
                if (string.IsNullOrWhiteSpace(_LogitudeDeclarationReferantData.OrderMoney) || (!string.IsNullOrWhiteSpace(_LogitudeDeclarationReferantData.OrderMoney) && _LogitudeDeclarationReferantData.OrderMoney.ToLower().Substring(0,1) != "t"))
                {
                    _DeclarationReferantDataPM.OrderMoney = false;

                }
                else
                {
                    _DeclarationReferantDataPM.OrderMoney = true;
                }

                myDeclarationReferantDataUpdateService.Update(this._DeclarationReferantDataPM, true);

                AppendLogLine("DeclarationReferantDataUpdate:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

                MyGenericResponseObj.Stage = "Done All ";
                MyGenericResponseObj.ApplicationId = this._DeclarationReferantDataPM.DeclarationId;
                MyCommunicationsParams.LoggingEntityId = MyGenericResponseObj.ApplicationId;
                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.Success;
            }
            catch (DbEntityValidationException ex)
            {
                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                var FormatedException = ExceptionFormatUtil.GetFormated(ex);
                AppendLogLine("ProccessRequest():Exception " + FormatedException.ToString() + Environment.NewLine + "---------------------------------------------");
                return;
            }
            catch (Exception e)
            {
                MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                AppendLogLine("ProccessRequest():Exception " + e.ToString() + Environment.NewLine + "---------------------------------------------");
                return;
            }
            if (!String.IsNullOrWhiteSpace(MyGenericResponseObj.StatusType.ToString()) && MyGenericResponseObj.StatusType != GenericResponseObj.StatusEnum.Success)
            {
                return;
            }

        }

        private string TranslateUser(string userId)
        {

            if (String.IsNullOrWhiteSpace(userId))
            {
                AppendLogLine("amitalReferentUserId is null");
                return null;
            }
            var repository = new UserRepository(ResolvedTenant());
            var myUserCard = repository.GetSingleUserByCode(userId, ResolvedTenant(), false);  //TODO: this function include all 
            if (myUserCard == null)
            {
                AppendLogLine("amitalReferentUserId = " + userId + " could not translate to Logitude Id");
                return null;
            }
            var cardId = myUserCard.Id;
            AppendLogLine("amitalReferentUserId = " + userId + " Translated to " + cardId);
            return cardId;
        }

        void DeserilazeObject(string xmlLOGIDECREFERANTDATA)
        {

            MyGenericResponseObj.Stage = "Initalize ProccessRequest";
            AppendLogLine("CommDecReferantDataService.ProccessRequest");

            AppendLogLine("Deserialize(DataIn1) ..");

            if (string.IsNullOrWhiteSpace(xmlLOGIDECREFERANTDATA))
            {
                throw new BusinessErrorException("DataIn1 is missing");
            }
            if (xmlLOGIDECREFERANTDATA.Length > 1000)
            {
                AppendLogLine("XmlIn=" + xmlLOGIDECREFERANTDATA.Substring(0, 1000));
                AppendLogLine(".Substring(0, 1000)");
            }
            else
            {
                AppendLogLine("XmlIn=" + xmlLOGIDECREFERANTDATA);
            }

            AppendLogLine("Tring DeserilazeObject");
            MyGenericResponseObj.Stage = "Trying DeserilazeObject";
            this._LOGIDECREFERANTDATA = XmlGenericUtil<LOGIDECREFERANTDATA>.DeSerializeObject(xmlLOGIDECREFERANTDATA);

            if (_LOGIDECREFERANTDATA.LogitudeDeclarationReferantData == null || _LOGIDECREFERANTDATA.LogitudeDeclarationReferantData.Length != 1)
            {
                throw new BusinessErrorException("_LOGIDECREFERANTDATA.CommDecReferantData.Length != 1");
            }
            this._LogitudeDeclarationReferantData = _LOGIDECREFERANTDATA.LogitudeDeclarationReferantData[0];
        }

        public override string GetAssemblyQualifiedName()
        {
            throw new NotImplementedException();
        }

        public override string GetExampleDataIn1()
        {
            var xml = "";
            var amitalObjExample = new LOGIDECREFERANTDATA();
            var myAmitalCommDecReferantData = new LogitudeDeclarationReferantData();

            myAmitalCommDecReferantData.Id = "1-1";
            myAmitalCommDecReferantData.PreClassification = "Test";
            myAmitalCommDecReferantData.FollowUpStatus = "CLS";
            myAmitalCommDecReferantData.QueueRemarks = "111";
            myAmitalCommDecReferantData.QueueStatus = "X";
            myAmitalCommDecReferantData.QueueType = "Q2";
            myAmitalCommDecReferantData.Tenant = "1";

            amitalObjExample.LogitudeDeclarationReferantData = new LogitudeDeclarationReferantData[] { myAmitalCommDecReferantData };

            xml = XmlGenericUtil<LOGIDECREFERANTDATA>.SerializeObject(amitalObjExample);

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


    }
}

