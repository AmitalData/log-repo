using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnifreightIIG.Common.MessageLib.Gurntee;

namespace Logitude.CustomsMessaging.MessageAnalyzer
{
    public class GRNT_MSG15_createGurateeRequestInfoResponseService : ResponseServiceBase
        <INF_MSG_GenericResponseData, GRNT_MSG15_createGurateeRequestInfo, GenericRequestParams>
    {
        public GuaranteePM _MyGuaranteePM;
        public int _MyTenant { get; set; }
        public ICustomContext _MyContext;
        public DeclarationPM _MyDeclarationPM;

        public override void Update(GRNT_MSG15_createGurateeRequestInfo customResponse, GenericRequestParams requestParams)
        {
            try //Analyze Message 1812- Guarantee Creation Notification (DCA)
            {
                this._MyContext = CustomContext.GetContext(requestParams.Tenant);
                this._MyTenant = requestParams.Tenant;
                var declarationQueryService = new DeclarationQueryService(this._MyContext);
                DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(this._MyContext, new Dictionary<string, IContext>(), requestParams.Tenant);
                var guaranteeQueryService = new GuaranteeQueryService(this._MyContext);
                var tapagConnectionTableQueryService = new TapagConnectionTableQueryService(this._MyContext);
                var guaranteeUpdateService = new GuaranteeUpdateService(this._MyContext, new Dictionary<string, IContext>(), requestParams.Tenant);

                this.MyRequestSheetParam = new RequestSheetParam();
                this.MyRequestSheetParam.RequestDescription = "בקשה להמצאת ערבות ";

                if (customResponse.GuaranteeRequestInfo == null)
                {
                    this.MyResponseData = new INF_MSG_GenericResponseData()
                    {
                        Succeeded = true,
                        HasException = true,
                        UserMessage = "לא נמצאו נתונים",
                    };
                    LogMessagingUtil.Instance.AppendLine("Can not find Guarantee- GuaranteeRequestInfo is null");
                    return;
                }

                //Check if Tapag file is already exist
                _MyGuaranteePM = null;
                if (customResponse.GuaranteeRequestInfo.TPGIdentifier != null)
                {
                    //string tapagId = tapagConnectionTableQueryService.GetTapagIdByFileAndNumeral(customResponse.GuaranteeRequestInfo.TPGIdentifier.fileNumber, customResponse.GuaranteeRequestInfo.TPGIdentifier.numeral, requestParams.Tenant);
                    string requestFileNumber = string.Concat(customResponse.GuaranteeRequestInfo.TPGIdentifier.fileNumber, "-", customResponse.GuaranteeRequestInfo.TPGIdentifier.numeral);
                    string tapagId = tapagConnectionTableQueryService.GetTapagIdByRequestFileNumber(requestFileNumber, this._MyTenant);

                    this.MyRequestSheetParam = new RequestSheetParam();
                    this.MyRequestSheetParam.RequestDescription = "בקשה להמצאת ערבות " + customResponse.GuaranteeRequestInfo.guaranteeRequestNumber;

                    if (!String.IsNullOrWhiteSpace(tapagId))
                    {
                        var GuaranteePM = guaranteeQueryService.GetGuaranteeByTapagId(tapagId, this._MyTenant);
                        this._MyGuaranteePM = guaranteeQueryService.GetSingle(GuaranteePM.Id, true, false);

                        if (this._MyGuaranteePM == null)
                        {
                            this.MyResponseData = new INF_MSG_GenericResponseData()
                            {
                                Succeeded = true,
                                HasException = true,
                                UserMessage = "לא נמצא תיק תפג",
                            };
                            LogMessagingUtil.Instance.AppendLine("Can not find Guarantee By tapag file " + tapagId);
                            return;
                        }
                        if (this._MyGuaranteePM.IsClosed == true) // if tapag file is close, dont update
                        {
                            this.MyResponseData = new INF_MSG_GenericResponseData()
                            {
                                ApplicationID = this._MyGuaranteePM.Id,
                                Succeeded = true,
                                HasException = true,
                                UserMessage = "לא ניתן לעדכן בקשה סגורה",
                            };

                            this.MyRequestSheetParam.EntityId1 = this._MyGuaranteePM.Id;
                            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Guarantee");
                            this.MyRequestSheetParam.EntityId2 = this._MyGuaranteePM.TapagID;
                            this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Tapag");
                            this.MyRequestSheetParam.RequestDescription = "בקשה להמצאת ערבות " + this._MyGuaranteePM.GuaranteeRequestNumber;
                            LogMessagingUtil.Instance.AppendLine("Tapag file is close " + this._MyGuaranteePM.TapagNumber);
                            return;
                        }
                        else // update tapag record
                        {
                            _MyGuaranteePM.ChangeSetOp = ChangeSetOperation.Update;
                            EventContextTagModel myInsertEventContextTagModel = new EventContextTagModel()
                            {
                                CallProccessID = EventContextTagModel.ProccessEnum.GRNT_MSG15_createGurateeRequestInfoResponseServiceUpdate,
                                EventRemarks = "Update Guarantee",
                            };
                            _MyGuaranteePM.CurrentContextTag = myInsertEventContextTagModel;
                        }
                    }
                }
                if(_MyGuaranteePM == null) // Create a new Tapag record
                {
                    _MyGuaranteePM = new GuaranteePM();
                    _MyGuaranteePM.ChangeSetOp = ChangeSetOperation.Insert;
                    //var newCounter = IdCounter.GetNumber("Dummy.CustomsTapagNumber", this._MyTenant);// to check if not in updateservice
                    //_MyGuaranteePM.TapagNumber = newCounter.ToString(); // tapag counter
                    _MyGuaranteePM.TapagNumber = CodeCounter.GetNumber("Customs.Tapag", this._MyTenant).ToString();
                    _MyGuaranteePM.Tenant = this._MyTenant;
                    _MyGuaranteePM.IsClosed = false;
                    _MyGuaranteePM.CreateDate = DateTime.Now;

                    EventContextTagModel myInsertEventContextTagModel = new EventContextTagModel()
                    {
                        CallProccessID = EventContextTagModel.ProccessEnum.GRNT_MSG15_createGurateeRequestInfoResponseServiceNew,
                        EventRemarks = "Create New Guarantee",
                    };
                    _MyGuaranteePM.CurrentContextTag = myInsertEventContextTagModel;
                }

                // Create\Update record in Guarantee Table (and tapag table)
                UpdateGuaranteeDetails(customResponse);
                guaranteeUpdateService.Update(_MyGuaranteePM, true);

                //Create a record in Tapag Connection Table
                if (_MyGuaranteePM.ChangeSetOp == ChangeSetOperation.Insert)
                {
                    CreateConnectionTables(customResponse);
                }

                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Guarantee");
                this.MyRequestSheetParam.EntityId1 = this._MyGuaranteePM.Id;
                this.MyRequestSheetParam.RequestDescription = "בקשה להמצאת ערבות " + this._MyGuaranteePM.GuaranteeRequestNumber;
                if (this._MyDeclarationPM != null)
                {
                    this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                    this.MyRequestSheetParam.EntityId2 = _MyDeclarationPM.Id;
                    if (this._MyDeclarationPM.IsConvertedDeclaration)
                    {
                        MyRequestSheetParam.RequestDescription = string.Concat(MyRequestSheetParam.RequestDescription, "\n", this._MyDeclarationPM.UserNotes);
                    }
                }

                this.MyResponseData = new INF_MSG_GenericResponseData()
                {
                    ApplicationID = this._MyGuaranteePM.Id,
                    Succeeded = true,
                    HasException = false,
                    UserMessage = "בקשה להמצאת ערבות " + this._MyGuaranteePM.GuaranteeRequestNumber,
                };
            }
            catch (System.Exception ee)
            {
                ///MyResponseData = new INF_MSG_GenericResponseData() { HasException = true, ExceptionMessage = ee.ToString() };
                throw;
            }

        }

        private void UpdateTapagDetails(GRNT_MSG15_createGurateeRequestInfo customResponse)
        {
            var clientQueryService = new ClientQueryService(this._MyContext);
            var myDeclarationQueryService = new DeclarationQueryService(this._MyTenant);
            var myDeclarationConstraintQueryService = new DeclarationConstraintQueryService(this._MyTenant);
            var commonContext = CommonDataContext.GetContext(this._MyTenant);

            string myDeclarationId = null;
            switch (customResponse.GuaranteeRequestInfo.entityType)
            {
                case 1055: // Declaration Number 
                    myDeclarationId = myDeclarationQueryService.GetIdByDeclarationNumber(customResponse.GuaranteeRequestInfo.entityNumber, this._MyTenant);
                    break;
                case 12240: // Constraint ID
                    myDeclarationId = myDeclarationConstraintQueryService.GetDeclarationConstraintsByConstraintId(customResponse.GuaranteeRequestInfo.entityNumber, this._MyTenant);
                    break;
                case 11156: // Tapag = LeadingFileNumber
                    // ??
                    break;
            }

            if (!string.IsNullOrWhiteSpace(myDeclarationId))
            {
                _MyDeclarationPM = myDeclarationQueryService.GetSingle(myDeclarationId, false, false);               
            }
            else if (customResponse.GuaranteeRequestInfo.entityNumber.Substring(2, 2) == "98" || customResponse.GuaranteeRequestInfo.entityNumber.Substring(2, 2) == "99")
            {
                DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(this._MyContext, new Dictionary<string, IContext>(), this._MyTenant);
                _MyDeclarationPM = declarationUpdateService.GetSertByConvertedDeclarationNumber(customResponse.GuaranteeRequestInfo.entityNumber, this._MyTenant);
            }

            this._MyGuaranteePM.Tenant = this._MyTenant;
            this._MyGuaranteePM.TapagTypeCode = "6";

            if (_MyDeclarationPM != null)
            {
                this._MyGuaranteePM.CustomerId = _MyDeclarationPM.CustomerId;
                this._MyGuaranteePM.ConnectedDeclarationId = _MyDeclarationPM.Id;
            }
            
            if (customResponse.GuaranteeRequestInfo.CustomerIdentification.externalIDSpecified == true && customResponse.GuaranteeRequestInfo.CustomerIdentification.externalID != null)
            {
                this._MyGuaranteePM.ImporterId = clientQueryService.GetIdByCode(customResponse.GuaranteeRequestInfo.CustomerIdentification.externalID.ToString(), this._MyTenant,true);
                if (this._MyGuaranteePM.CustomerId == null)
                {
                    // Get customerId by VatNumber (cards table)
                    var cardRepository = new CardRepository(commonContext);
                    Card card = cardRepository.GetSingleCardByVatNumber(customResponse.GuaranteeRequestInfo.CustomerIdentification.externalID.ToString(), this._MyTenant);
                    if (card != null)
                    {
                        this._MyGuaranteePM.CustomerId = card.Id;
                    }
                }
            }

            this._MyGuaranteePM.CustomsBranchCode = customResponse.GuaranteeRequestInfo.customsHouse.ToString();
            this._MyGuaranteePM.ProfessionUnitTypeCode = customResponse.GuaranteeRequestInfo.professionalUnitID.ToString();
            this._MyGuaranteePM.ValidityDate = customResponse.GuaranteeRequestInfo.GuaranteeValidityDate;
            if (customResponse.GuaranteeRequestInfo.specializationTypeSpecified == true && customResponse.GuaranteeRequestInfo.specializationType != null) 
            {
                this._MyGuaranteePM.SpecializationTypeCode = customResponse.GuaranteeRequestInfo.specializationType.ToString();
            }
            this._MyGuaranteePM.FollowDate = customResponse.GuaranteeRequestInfo.requestValidity.AddDays(-7);
        }

        private void UpdateGuaranteeDetails(GRNT_MSG15_createGurateeRequestInfo customResponse)
        {
            // Update Tapag Details
            UpdateTapagDetails(customResponse);

            // Update Guarantee Details
            this._MyGuaranteePM.Tenant = this._MyTenant;
            this._MyGuaranteePM.GuaranteeRequestStatusCode = customResponse.GuaranteeRequestInfo.guaranteeRequestStatus.ToString();
            this._MyGuaranteePM.GuaranteeRequestNumber = customResponse.GuaranteeRequestInfo.guaranteeRequestNumber;
            this._MyGuaranteePM.NumeralRequest = customResponse.GuaranteeRequestInfo.numeralRequest.ToString();
            if (customResponse.GuaranteeRequestInfo.msgIDSpecified == true && customResponse.GuaranteeRequestInfo.msgID != null)
            {
                this._MyGuaranteePM.MsgID = customResponse.GuaranteeRequestInfo.msgID.ToString();
            }
            this._MyGuaranteePM.ClientActivityCode = customResponse.GuaranteeRequestInfo.clientActivity.ToString();
            if (customResponse.GuaranteeRequestInfo.entityTypeSpecified == true)
            {
                this._MyGuaranteePM.CustomEntityTypeCode = customResponse.GuaranteeRequestInfo.entityType.ToString();
            }
            this._MyGuaranteePM.CustomEntityNumber = customResponse.GuaranteeRequestInfo.entityNumber;
            this._MyGuaranteePM.RequestValidityDate = customResponse.GuaranteeRequestInfo.requestValidity;
            this._MyGuaranteePM.GuaranteeValidityDate = customResponse.GuaranteeRequestInfo.GuaranteeValidityDate;
            if (customResponse.GuaranteeRequestInfo.BrandIDSpecified == true)
            {
                this._MyGuaranteePM.BrandNumber = customResponse.GuaranteeRequestInfo.BrandID.ToString();
            }
            this._MyGuaranteePM.GuaranteeValidityDate = customResponse.GuaranteeRequestInfo.GuaranteeValidityDate;
            if (customResponse.GuaranteeRequestInfo.lawyerIdSpecified == true && customResponse.GuaranteeRequestInfo.lawyerId != null)
            {
                this._MyGuaranteePM.LawyerNumber = customResponse.GuaranteeRequestInfo.lawyerId.ToString();
            }
            this._MyGuaranteePM.BirthDate = customResponse.GuaranteeRequestInfo.birthDate;

            this._MyGuaranteePM.VehicleChassisNumber = customResponse.GuaranteeRequestInfo.vehicleChassisNumber; 
            this._MyGuaranteePM.EngineNumber = customResponse.GuaranteeRequestInfo.engineNumber;
            if (customResponse.GuaranteeChangeTermsRequestApprovalInfo != null)
            {
                this._MyGuaranteePM.UpdateDate = customResponse.GuaranteeChangeTermsRequestApprovalInfo.updateDate;
            }

            // Update Guarantee Conditions Details
            foreach (var guaranteeCondition in customResponse.GuaranteeConditions)
            {
                GuaranteeConditionPM guaranteeConditionPM = new GuaranteeConditionPM();
                guaranteeConditionPM.ChangeSetOp = ChangeSetOperation.Insert;
                guaranteeConditionPM.Tenant = this._MyTenant;
                guaranteeConditionPM.ReturnConditionCode = guaranteeCondition.guaranteeCondition.ToString();
                guaranteeConditionPM.GuaranteeAmount = guaranteeCondition.guaranteeAmount;
                this._MyGuaranteePM.GuaranteeConditions.Add(guaranteeConditionPM);
            }
            // Update Required Guarantee Types
            foreach (var guaranteeType in customResponse.GuaranteeRequiredTypes)
            {
                RequiredGuaranteeTypePM requiredGuaranteeTypePM = new RequiredGuaranteeTypePM();
                requiredGuaranteeTypePM.ChangeSetOp = ChangeSetOperation.Insert;
                requiredGuaranteeTypePM.Tenant = this._MyTenant;
                requiredGuaranteeTypePM.GuaranteeTypeCode = guaranteeType.guaranteeType.ToString();
                requiredGuaranteeTypePM.GuaranteeAmount = guaranteeType.guaranteeAmount;
                this._MyGuaranteePM.RequiredGuaranteeTypes.Add(requiredGuaranteeTypePM); 
            }
        }

        private void CreateConnectionTables(GRNT_MSG15_createGurateeRequestInfo customResponse)
        {
            // Update Tapag Connection Table
            // In Cases of <EntityType> = 1055 , <EntityNumber> will contain the DeclarationNumber , find the DelcarationId and update field DelcarationId in the connection table
            // In Cases of <EntityType> = 12240 , <EntityNumber> will contain the Constraint ID , find the DelcarationId using the DeclarationConstraints table and update field DelcarationId in the connection table
            var myDeclarationQueryService = new DeclarationQueryService(this._MyTenant);
            var myDeclarationConstraintQueryService = new DeclarationConstraintQueryService(this._MyTenant);
            var myTapagConnectionUpdateService = new TapagConnectionTableUpdateService(this._MyContext, new Dictionary<string, IContext>(), this._MyTenant);

            var remark = "Custom Guarantee Notification" + "\n" +
                "Message ID: " + customResponse.GuaranteeRequestInfo.msgID + "\n" +
                ", Guarantee request status: " + customResponse.GuaranteeRequestInfo.guaranteeRequestStatus + "\n" +
                ", Guarantee request number: " + customResponse.GuaranteeRequestInfo.guaranteeRequestNumber + "\n" +
                ", Guarantee validity: " + customResponse.GuaranteeRequestInfo.GuaranteeValidityDate + "\n" +
                ", Request validity: " + customResponse.GuaranteeRequestInfo.requestValidity + "\n" +
                ", Customer ID: " + customResponse.GuaranteeRequestInfo.CustomerIdentification.externalID + "\n" +
                ", Passport number: " + customResponse.GuaranteeRequestInfo.CustomerIdentification.passportNumber;
            if (customResponse.GuaranteeRequestInfo.TPGIdentifier != null)
            {
                remark = string.Concat(remark, ", Guarantee file number: ", customResponse.GuaranteeRequestInfo.TPGIdentifier.fileNumber);
            }

            EventContextTagModel myInsertEventContextTagModel = new EventContextTagModel();
            switch (customResponse.GuaranteeRequestInfo.entityType)
            {
                case 1055: // Declaration Number 
                    //Create Event "CGN" & Notification - Deposit Customs Request
                    myInsertEventContextTagModel.CallProccessID = EventContextTagModel.ProccessEnum.GRNT_MSG15_createGurateeRequestInfoResponseService;
                    myInsertEventContextTagModel.EventCode = "CGN";
                    myInsertEventContextTagModel.EventRemarks = remark;
                    break;
                case 12240: // Constraint ID
                    //Create Event "CGN" & Notification - Deposit Customs Request
                    myInsertEventContextTagModel.CallProccessID = EventContextTagModel.ProccessEnum.GRNT_MSG15_createGurateeRequestInfoResponseService;
                    myInsertEventContextTagModel.EventCode = "CGN";
                    myInsertEventContextTagModel.EventRemarks = remark;
                    break;
                case 11156: // Tapag = LeadingFileNumber
                    break;
            }

            if (_MyDeclarationPM != null && this._MyGuaranteePM.TapagID != null)
            {
                TapagConnectionTablePM tapagConnectionTablePM = new TapagConnectionTablePM();
                tapagConnectionTablePM.ChangeSetOp = ChangeSetOperation.Insert;
                tapagConnectionTablePM.Tenant = this._MyTenant;
                tapagConnectionTablePM.TapagId = this._MyGuaranteePM.TapagID;
                tapagConnectionTablePM.DeclarationId = _MyDeclarationPM.Id;
                if (customResponse.GuaranteeRequestInfo.TPGIdentifier != null)
                {
                    //tapagConnectionTablePM.CustomsTapagFile = customResponse.GuaranteeRequestInfo.TPGIdentifier.fileNumber;
                    //tapagConnectionTablePM.CustomsNumeral = customResponse.GuaranteeRequestInfo.TPGIdentifier.numeral;
                    tapagConnectionTablePM.RequestFileNumber = string.Concat(customResponse.GuaranteeRequestInfo.TPGIdentifier.fileNumber, "-", customResponse.GuaranteeRequestInfo.TPGIdentifier.numeral);
                }
                tapagConnectionTablePM.CustomFileNo = _MyDeclarationPM.CustomFileNo;
                tapagConnectionTablePM.CurrentContextTag = myInsertEventContextTagModel;
                
                myTapagConnectionUpdateService.Update(tapagConnectionTablePM, true);
            }
        }

        public override INF_MSG_GenericResponseData GetResponse(GRNT_MSG15_createGurateeRequestInfo customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }
    }
}


