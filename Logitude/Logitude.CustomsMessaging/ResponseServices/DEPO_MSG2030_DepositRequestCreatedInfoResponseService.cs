using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
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
using System.Threading.Tasks;
using UnifreightIIG.Common.MessageLib.Deposit;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DEPO_MSG2030_DepositRequestCreatedInfoResponseService : ResponseServiceBase<
        INF_MSG_GenericResponseData,
        DEPO_NG_2030_MSG1_DepositRequestCreatedInfoMsg,
        GenericRequestParams>
    {
        public int _MyTenant { get; set; }
        public ICustomContext _MyContext;
        public DepositPM _MyDepositPM ;
        public DeclarationPM _MyDeclarationPM;
        private ICommonDataContext _CommonContext;

        public override void Update(DEPO_NG_2030_MSG1_DepositRequestCreatedInfoMsg customResponse, GenericRequestParams requestParams)
        {
            //Analyze message 2030- Deposit Request (DCA)
            this._MyTenant = requestParams.Tenant;
            this._MyContext = CustomContext.GetContext(this._MyTenant);
            _CommonContext = CommonDataContext.GetContext(this._MyTenant);
            var depositQueryService = new DepositQueryService(this._MyContext);
            var depositUpdateService = new DepositUpdateService(this._MyContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            var tapagQueryService = new TapagQueryService(this._MyContext);
            var tapagUpdateService = new TapagUpdateService(this._MyContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            var tapagConnectionTableQueryService = new TapagConnectionTableQueryService(this._MyContext);
            
            //Check if Tapag file is already exist
            string requestFileNumber = string.Concat(customResponse.DepositRequestInfo.DepositRequestIdentifier.fileNumber, "-", customResponse.DepositRequestInfo.DepositRequestIdentifier.numeral);
            string tapagId = tapagConnectionTableQueryService.GetTapagIdByRequestFileNumber(requestFileNumber, this._MyTenant);

            if (!String.IsNullOrWhiteSpace(tapagId))
            {
                var depositId = depositQueryService.GetDepositIdByTapagNumber(tapagId,this._MyTenant);
                this._MyDepositPM = depositQueryService.GetSingle(depositId, true, false);

                if (depositId == null)
                {
                    this.MyResponseData = new INF_MSG_GenericResponseData()
                    {
                        Succeeded = true,
                        HasException = true,
                        UserMessage = "Can not find tapag file (not exist in connection table) " + "fileNumber=" + customResponse.DepositRequestInfo.DepositRequestIdentifier.fileNumber + "Numeral=" + customResponse.DepositRequestInfo.DepositRequestIdentifier.numeral,
                    };
                    LogMessagingUtil.Instance.AppendLine("Can not find tapag file (not exist in connection table) " + "fileNumber=" + customResponse.DepositRequestInfo.DepositRequestIdentifier.fileNumber + "Numeral=" + customResponse.DepositRequestInfo.DepositRequestIdentifier.numeral);
                    return;
                }
                if (this._MyDepositPM.IsClosed == true) // if tapag file is close, dont update
                {
                    this.MyResponseData = new INF_MSG_GenericResponseData()
                    {
                        ApplicationID = this._MyDepositPM.Id,
                        Succeeded = true,
                        UserMessage = null,
                    };

                    this.MyRequestSheetParam = new RequestSheetParam()
                    {
                        EntityId1 = this._MyDepositPM.Id,
                        ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Deposit"),
                        EntityId2 = this._MyDepositPM.TapagID,
                        ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Tapag"),
                        RequestDescription = "בקשה לפיקדון " + this._MyDepositPM.TapagNumber,
                    };
                    LogMessagingUtil.Instance.AppendLine("Tapag file is close " + this._MyDepositPM.TapagNumber);
                    return;
                }
                // update tapag record
               _MyDepositPM.ChangeSetOp = ChangeSetOperation.Update;
               DeleteDepositConditions();
            }
            else // Create a new Tapag record
            {
                _MyDepositPM = new DepositPM();
                _MyDepositPM.ChangeSetOp = ChangeSetOperation.Insert;
                //var newCounter = IdCounter.GetNumber("Dummy.CustomsTapagNumber", this._MyTenant);
                //_MyDepositPM.TapagNumber = newCounter.ToString(); // tapag counter
                _MyDepositPM.TapagNumber = CodeCounter.GetNumber("Customs.Tapag", this._MyTenant).ToString();
                _MyDepositPM.Tenant = this._MyTenant;
                _MyDepositPM.IsClosed = false;
                _MyDepositPM.CreateDate = DateTime.Now;
            }

            // Create new record in Deposit Table (and tapag table)
            UpdateDepositDetails(customResponse);
            depositUpdateService.Update(_MyDepositPM, true);

            //Create connection: 1. Tapag Connection Table 2. Payment Order connection table
            if (this._MyDepositPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                CreateConnectionTables(customResponse);
            }

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Deposit");
            this.MyRequestSheetParam.EntityId1 = this._MyDepositPM.Id;
            this.MyRequestSheetParam.RequestDescription = "בקשה לפיקדון " + customResponse.DepositRequestInfo.DepositRequestIdentifier.fileNumber;
            if (_MyDeclarationPM != null)
            {
                this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                this.MyRequestSheetParam.EntityId2 = _MyDeclarationPM.Id;
                this.MyRequestSheetParam.CustomFileNo = _MyDeclarationPM.CustomFileNo;
                if (this._MyDeclarationPM.IsConvertedDeclaration)
                {
                    MyRequestSheetParam.RequestDescription = string.Concat(MyRequestSheetParam.RequestDescription, "\n", this._MyDeclarationPM.UserNotes);
                }
            }

            this.MyResponseData = new INF_MSG_GenericResponseData()
            {
                ApplicationID = this._MyDepositPM.Id,
                Succeeded = true,
                HasException = false,
                UserMessage = "בקשה לפיקדון " + customResponse.DepositRequestInfo.DepositRequestIdentifier.fileNumber,
            };

        }

        private void DeleteDepositConditions()
        {
            if (this._MyDepositPM.DepositConditions == null)
            {
                return;
            }

            foreach (var conditionItem in this._MyDepositPM.DepositConditions)
            {
                conditionItem.ChangeSetOp = ChangeSetOperation.Delete;
                this._MyDepositPM.DeletedDepositConditions.Add(conditionItem);
            }
        }


        private void UpdateTapagDetails(DEPO_NG_2030_MSG1_DepositRequestCreatedInfoMsg customResponse)
        {
            var clientQueryService = new ClientQueryService(this._MyContext);
            var myDeclarationQueryService = new DeclarationQueryService(this._MyTenant);
            var myDeclarationConstraintQueryService = new DeclarationConstraintQueryService(this._MyTenant);

            string myDeclarationId = null;
            switch (customResponse.DepositRequestInfo.entityType)
            {
                case 1055: // Declaration Number 
                    myDeclarationId = myDeclarationQueryService.GetIdByDeclarationNumber(customResponse.DepositRequestInfo.entityNumber, this._MyTenant);
                    break;
                case 12240: // Constraint ID
                    myDeclarationId = myDeclarationConstraintQueryService.GetDeclarationConstraintsByConstraintId(customResponse.DepositRequestInfo.entityNumber, this._MyTenant);
                    break;
                case 11156: // Tapag = LeadingFileNumber
                    // ??
                    break;
                case 11118: // deposit Number // moran 15.9.16 - 22914
                    string declarationNumber = customResponse.DepositRequestInfo.entityNumber.Split('/')[0].ToString();
                    myDeclarationId = myDeclarationQueryService.GetIdByDeclarationNumber(declarationNumber, this._MyTenant);
                    break;

            }

            this._MyDepositPM.TapagTypeCode = "5";         
            if (!string.IsNullOrWhiteSpace(myDeclarationId))
            {
                _MyDeclarationPM = myDeclarationQueryService.GetSingle(myDeclarationId, false, false);
            }
            else if (customResponse.DepositRequestInfo.entityNumber.Substring(2, 2) == "98" || customResponse.DepositRequestInfo.entityNumber.Substring(2, 2) == "99")
            {
                DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(this._MyContext, new Dictionary<string, IContext>(), this._MyTenant);
                _MyDeclarationPM = declarationUpdateService.GetSertByConvertedDeclarationNumber(customResponse.DepositRequestInfo.entityNumber, this._MyTenant);
            }

            if (myDeclarationId != null)
            {
                this._MyDepositPM.CustomerId = _MyDeclarationPM.CustomerId;
                this._MyDepositPM.ImporterId = _MyDeclarationPM.ImporterId;//14561 
            }
            //this._MyDepositPM.ImporterId = clientQueryService.GetIdByCode(customResponse.DepositRequestInfo.PaymentDetails.externalID.ToString(), this._MyTenant);//14561 cancelled
            if (this._MyDepositPM.CustomerId == null && customResponse.DepositRequestInfo.CustomerIdentification.externalID != null)
            {
                // Get customerId by VatNumber (cards table)
                var cardRepository = new CardRepository(_CommonContext);
                Card card = cardRepository.GetSingleCardByVatNumber(customResponse.DepositRequestInfo.CustomerIdentification.externalID.ToString(), this._MyTenant);
                this._MyDepositPM.CustomerId = card.Id;
            }
            this._MyDepositPM.CustomsBranchCode = customResponse.DepositRequestInfo.customsHouse.ToString();
            this._MyDepositPM.ProfessionUnitTypeCode = customResponse.DepositRequestInfo.professionalUnitID.ToString();
            this._MyDepositPM.ValidityDate = customResponse.DepositRequestInfo.DepositValidityDate;
            if (customResponse.DepositRequestInfo.specializationTypeSpecified == true)
            {
                this._MyDepositPM.SpecializationTypeCode = customResponse.DepositRequestInfo.specializationType.ToString();
            }
            this._MyDepositPM.FollowDate = customResponse.DepositRequestInfo.requestValidity.AddDays(-7);
        }

        private void UpdateDepositDetails(DEPO_NG_2030_MSG1_DepositRequestCreatedInfoMsg customResponse)
        {
            // Update tapag details
            UpdateTapagDetails(customResponse);

            // Update deposit details
            this._MyDepositPM.Tenant = this._MyTenant;
            this._MyDepositPM.DepositEssenceTypeCode = customResponse.DepositRequestInfo.DepositEssenceType.ToString();
            this._MyDepositPM.DepositAmount = customResponse.DepositRequestInfo.DepositAmount;
            this._MyDepositPM.RequestValidityDate = customResponse.DepositRequestInfo.requestValidity;
            this._MyDepositPM.DepositValidityDate = customResponse.DepositRequestInfo.DepositValidityDate;
            if (customResponse.DepositRequestInfo.entityType > 0)
            {
                this._MyDepositPM.EntityTypeCode = customResponse.DepositRequestInfo.entityType.ToString();
            }
            this._MyDepositPM.EntityNumber = customResponse.DepositRequestInfo.entityNumber;
            this._MyDepositPM.PaymentNumber = customResponse.DepositRequestInfo.PaymentDetails.paymentID.ToString();
            if(customResponse.DepositRequestInfo.tradeMarkIDSpecified == true)
            {
                this._MyDepositPM.TradeMarkNumber = customResponse.DepositRequestInfo.tradeMarkID.ToString();
            }
            if(customResponse.DepositRequestInfo.lawyerIdSpecified == true)
            {
                this._MyDepositPM.LawyerNumber = customResponse.DepositRequestInfo.lawyerId.ToString();
            }
            this._MyDepositPM.VehicleChassisNumber = customResponse.DepositRequestInfo.vehicleChassisNumber;
            this._MyDepositPM.EngineNumber = customResponse.DepositRequestInfo.engineNumber;
            if (customResponse.DepositRequestInfo.birthDateSpecified == true)
            {
                this._MyDepositPM.BirthDate = customResponse.DepositRequestInfo.birthDate;
            }

            // Update deposit conditions details
            foreach (var depositCondition in customResponse.DepositConditions)
            {
                DepositConditionPM depositConditionPM = new DepositConditionPM();
                depositConditionPM.ChangeSetOp = ChangeSetOperation.Insert;
                depositConditionPM.Tenant = this._MyTenant;
                depositConditionPM.DepositConditionCode = depositCondition.DepositCondition.ToString();
                depositConditionPM.DepositAmount = depositCondition.DepositAmount;
                this._MyDepositPM.DepositConditions.Add(depositConditionPM);
            }
        }

        public override INF_MSG_GenericResponseData GetResponse(DEPO_NG_2030_MSG1_DepositRequestCreatedInfoMsg customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        private void CreateConnectionTables(DEPO_NG_2030_MSG1_DepositRequestCreatedInfoMsg customResponse)
        {
            // Update Tapag Connection Table
            // In Cases of <EntityType> = 1055 , <EntityNumber> will contain the DeclarationNumber , find the DelcarationId and update field DelcarationId in the connection table
            // In Cases of <EntityType> = 12240 , <EntityNumber> will contain the Constraint ID , find the DelcarationId using the DeclarationConstraints table and update field DelcarationId in the connection table
            var myDeclarationQueryService = new DeclarationQueryService(this._MyTenant);
            var myDeclarationConstraintQueryService = new DeclarationConstraintQueryService(this._MyTenant);
            var myTapagConnectionUpdateService = new TapagConnectionTableUpdateService(this._MyContext, new Dictionary<string, IContext>(), this._MyTenant);

            EventContextTagModel myInsertEventContextTagModel = new EventContextTagModel();
            switch (customResponse.DepositRequestInfo.entityType)
            {
                case 1055: // Declaration Number 
                    //Create Event "DPR" & Notification - Deposit Customs Request
                    myInsertEventContextTagModel.CallProccessID = EventContextTagModel.ProccessEnum.DEPO_MSG2030_DepositRequestCreatedInfoResponseServiceDeclaration;
                    myInsertEventContextTagModel.EventCode = "DPR";
                    myInsertEventContextTagModel.EventRemarks = "Deposit Customs Request";
                    break;
                case 12240: // Constraint ID
                    //Create Event "DPR" & Notification - Deposit Customs Request
                    myInsertEventContextTagModel.CallProccessID = EventContextTagModel.ProccessEnum.DEPO_MSG2030_DepositRequestCreatedInfoResponseServiceDeclaration;
                    myInsertEventContextTagModel.EventCode = "DPR";
                    myInsertEventContextTagModel.EventRemarks = "Deposit Customs Request";
                    break;
                case 11156: // Tapag = LeadingFileNumber
                    break;
            }

            if (_MyDeclarationPM != null && this._MyDepositPM.TapagID != null)
            {
                string declarationId = _MyDeclarationPM.Id;
                TapagConnectionTablePM tapagConnectionTablePM = new TapagConnectionTablePM();
                tapagConnectionTablePM.ChangeSetOp = ChangeSetOperation.Insert;
                tapagConnectionTablePM.Tenant = this._MyTenant;
                tapagConnectionTablePM.TapagId = this._MyDepositPM.TapagID;
                tapagConnectionTablePM.DeclarationId = declarationId;
                //tapagConnectionTablePM.CustomsTapagFile = customResponse.DepositRequestInfo.DepositRequestIdentifier.fileNumber;
                //tapagConnectionTablePM.CustomsNumeral = customResponse.DepositRequestInfo.DepositRequestIdentifier.numeral;
                tapagConnectionTablePM.RequestFileNumber = string.Concat(customResponse.DepositRequestInfo.DepositRequestIdentifier.fileNumber, "-", customResponse.DepositRequestInfo.DepositRequestIdentifier.numeral);
                tapagConnectionTablePM.CustomFileNo = _MyDeclarationPM.CustomFileNo;
                tapagConnectionTablePM.CurrentContextTag = myInsertEventContextTagModel;

                myTapagConnectionUpdateService.Update(tapagConnectionTablePM, true);
            }

            // Update PaymentOrder Connection Table
            // Search for an exsiting record in Payment Order Table using <paymentOrderID>, 
            // if found create a new record in Payment Order connection table
            var myPaymentOrderQueryService = new PaymentOrderQueryService(this._MyContext);
            var myPaymentOrderUpdateService = new PaymentOrderUpdateService(this._MyContext, new Dictionary<string, IContext>(), this._MyTenant);

            string paymentOrderId = myPaymentOrderQueryService.GetIdByPaymentNumber(customResponse.DepositRequestInfo.PaymentDetails.paymentID.ToString(), this._MyTenant);
            if (!string.IsNullOrWhiteSpace(paymentOrderId) && this._MyDeclarationPM != null)
            {
                PaymentOrderPM paymentOrderPM = myPaymentOrderQueryService.GetSingle(paymentOrderId, true, false);
                paymentOrderPM.ChangeSetOp = ChangeSetOperation.Update;

                PaymentOrderConnectionTablePM paymentOrderConnectionTablePM = paymentOrderPM.PaymentOrderConnectionTables.FirstOrDefault(si => si.ConnectedEntityId == _MyDeclarationPM.Id && si.PaymentOrderId == paymentOrderId);
                if (paymentOrderConnectionTablePM == null)
                {
                    PaymentOrderConnectionTablePM _paymentOrderConnectionTablePM = new PaymentOrderConnectionTablePM();
                    _paymentOrderConnectionTablePM.ChangeSetOp = ChangeSetOperation.Insert;
                    _paymentOrderConnectionTablePM.Tenant = this._MyTenant;
                    _paymentOrderConnectionTablePM.PaymentOrderId = paymentOrderId;
                    _paymentOrderConnectionTablePM.ConnectedEntityId = _MyDeclarationPM.Id;
                    _paymentOrderConnectionTablePM.ConnectedEntityCode = "D";
                    paymentOrderPM.PaymentOrderConnectionTables.Add(_paymentOrderConnectionTablePM);
                    myPaymentOrderUpdateService.Update(paymentOrderPM, true);
                }
               
            }
        }
    }
}
