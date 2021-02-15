using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.Helpers;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnifreightIIG.Common.MessageLib.DeclarationDeal;
using UnifreightIIG.Common.MessageLib.Deficit;
using Logitude.Customs.BL.Models;
using Attachment = UnifreightIIG.Common.MessageLib.Deficit.Attachment;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DE_NG_280_MSG11_DebtNotificationMessageResponseService:
        ResponseServiceBase<INF_MSG_GenericResponseData, DE_NG_280_MSG11_DebtNotificationMessage, GenericRequestParams>
    {
        private int _MyDeficitTenant;
        private ICustomContext myDbContext;
        public DeficitPM _MyDeficitPM;
        private DeclarationPM _MyDeclarationPM;

        public override INF_MSG_GenericResponseData GetResponse(DE_NG_280_MSG11_DebtNotificationMessage customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override Action<DE_NG_280_MSG11_DebtNotificationMessage> GetActionShrinkCustomResponse()
        {
            return (customResponse) =>
            {

                if (customResponse == null) return;
                if (customResponse.Attachment == null) return;
                 customResponse.Attachment.ToList().ForEach( attachment=>{

                     var MD5Hash = MD5HashUtil.GetMD5Hash(attachment.content);
                     attachment.content = System.Text.UTF8Encoding.UTF8.GetBytes(MD5Hash);
                 }) ;


                


            };
        }

        public override void Update(DE_NG_280_MSG11_DebtNotificationMessage customResponse, GenericRequestParams requestParams)
        {
            //Analyze message 280- Debt Notification
            this._MyDeficitTenant = requestParams.Tenant;
            this.myDbContext = CustomContext.GetContext(this._MyDeficitTenant);

            var deficitQueryService = new DeficitQueryService(this.myDbContext);
            var deficitUpdateService = new DeficitUpdateService(this.myDbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            var tapagQueryService = new TapagQueryService(this.myDbContext);
            var tapagUpdateService = new TapagUpdateService(this.myDbContext, new Dictionary<string, IContext>(), requestParams.Tenant);

            var id = deficitQueryService.GetIdByDebtNotificationNumber(customResponse.DebtNotificationMessag.debtNotificationID.ToString(), requestParams.Tenant);
            if (!String.IsNullOrWhiteSpace(id))
            {
                this.MyResponseData = new INF_MSG_GenericResponseData();
                this.MyResponseData.Succeeded = false;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = "בקשה חוזרת להודעת חיוב " + customResponse.DebtNotificationMessag.debtNotificationID + " (לא נותח)";

                this.MyRequestSheetParam = new RequestSheetParam();
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Deficit");
                this.MyRequestSheetParam.EntityId1 = id;
                this.MyRequestSheetParam.RequestDescription = "בקשה חוזרת להודעת חיוב " + customResponse.DebtNotificationMessag.debtNotificationID;
                string declarationId = GetTapagConnection(customResponse.DeficitFile.FirstOrDefault().TPGIdentifier.fileNumber, customResponse.DeficitFile.FirstOrDefault().TPGIdentifier.numeral);
                if (!string.IsNullOrWhiteSpace(declarationId))
                {
                    this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                    this.MyRequestSheetParam.EntityId2 = declarationId;
                }
                return;
            }
           
            // Create new record in Deficit Table
            GetDeficitDetails(customResponse);
            deficitUpdateService.Update(this._MyDeficitPM, true);

            //Create connection: 1. Tapag Connection Table 2. Tapag Connected File Paragraph Types
            CreateConnectionTables(customResponse);

            //Add Document Billing notification - Contains the agent's relative debt
            AnalyzeDeficitDocument(customResponse.Attachment, requestParams);

            requestParams.LoggingEntityId = this._MyDeficitPM.Id;
            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyResponseData.ApplicationID = this._MyDeficitPM.Id;
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;

            this.MyRequestSheetParam = new RequestSheetParam();
            this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Deficit");
            this.MyRequestSheetParam.EntityId1 = this._MyDeficitPM.Id;
            if (this._MyDeclarationPM != null)
            {
                this.MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                this.MyRequestSheetParam.EntityId2 = this._MyDeclarationPM.Id;
                this.MyRequestSheetParam.CustomFileNo = this._MyDeclarationPM.CustomFileNo;
            }
            this.MyRequestSheetParam.RequestDescription = "הודעת חיוב " + this._MyDeficitPM.DebtNotificationNumber;
            if (this._MyDeclarationPM != null && this._MyDeclarationPM.IsConvertedDeclaration)
            {
                MyRequestSheetParam.RequestDescription = string.Concat(MyRequestSheetParam.RequestDescription, "\n", this._MyDeclarationPM.UserNotes);
            }
        }

        private void GetTapagDetails(DE_NG_280_MSG11_DebtNotificationMessage customResponse)
        {
            var clientQueryService = new ClientQueryService(this.myDbContext);
            var myDeclarationQueryService = new DeclarationQueryService(this._MyDeficitTenant);

            /*string declarationId = myDeclarationQueryService.GetIdByDeclarationNumber(customResponse.DeficitFile.FirstOrDefault().TPGIdentifier.fileNumber, this._MyDeficitTenant);
            if (string.IsNullOrWhiteSpace(declarationId))
            {
                LogMessagingUtil.Instance.AppendLine("DebtNotificationMessage: DeclarationID= " + customResponse.DeficitFile.FirstOrDefault().TPGIdentifier.fileNumber + " But declarationRep.GetSingleDeclarationByNumber return null ");
            }
            else
            {
                LogMessagingUtil.Instance.AppendLine("DebtNotificationMessage: DeclarationID= " + customResponse.DeficitFile.FirstOrDefault().TPGIdentifier.fileNumber);
                _MyDeclarationPM = myDeclarationQueryService.GetSingle(declarationId, false, false);
            }*/

            DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(this.myDbContext, new Dictionary<string, IContext>(), this._MyDeficitTenant);
            _MyDeclarationPM = declarationUpdateService.GetSertByConvertedDeclarationNumber(customResponse.DeficitFile.FirstOrDefault().TPGIdentifier.fileNumber, this._MyDeficitTenant);
            if (_MyDeclarationPM == null || string.IsNullOrWhiteSpace(_MyDeclarationPM.Id))
            {
                LogMessagingUtil.Instance.AppendLine("DebtNotificationMessage: DeclarationID= " + customResponse.DeficitFile.FirstOrDefault().TPGIdentifier.fileNumber + " But declarationRep.GetSingleDeclarationByNumber return null ");
            }
            else
            {
                LogMessagingUtil.Instance.AppendLine("DebtNotificationMessage: DeclarationID= " + customResponse.DeficitFile.FirstOrDefault().TPGIdentifier.fileNumber);
            }

            //var newCounter = IdCounter.GetNumber("Dummy.CustomsTapagNumber", this._MyDeficitTenant);
            //this._MyDeficitPM.TapagNumber = newCounter.ToString(); // tapag counter
            this._MyDeficitPM.TapagNumber = CodeCounter.GetNumber("Customs.Tapag", this._MyDeficitTenant).ToString();
            this._MyDeficitPM.TapagTypeCode = "1";
            this._MyDeficitPM.LeadingFileNumber = customResponse.DebtNotificationMessag.leadingFileNumber.ToString();
            var clientId = clientQueryService.GetIdByCode(customResponse.DebtNotificationMessag.externalID.ToString(), this._MyDeficitTenant,true);
            if (!String.IsNullOrWhiteSpace(clientId))
            {
                //this._MyDeficitPM.CustomerId = clientId;
                this._MyDeficitPM.ImporterId = clientId;
            }
            if (this._MyDeclarationPM != null && !String.IsNullOrWhiteSpace(_MyDeclarationPM.CustomerId))
            {
                this._MyDeficitPM.CustomerId = _MyDeclarationPM.CustomerId;
            }
            this._MyDeficitPM.CustomsBranchCode = customResponse.DebtNotificationMessag.customOfficeNumber.ToString();
            this._MyDeficitPM.ProfessionUnitTypeCode = customResponse.DebtNotificationMessag.unitCode.ToString();
            this._MyDeficitPM.CreateDate = DateTime.Now;
            this._MyDeficitPM.ValidityDate = customResponse.DebtNotificationMessag.validityDateTo;
            this._MyDeficitPM.FollowDate = customResponse.DebtNotificationMessag.validityDateTo.AddDays(-7);
        }

        private void GetDeficitDetails(DE_NG_280_MSG11_DebtNotificationMessage customResponse)
        {
            this._MyDeficitPM = new DeficitPM();
            var deficitUpdateService = new DeficitUpdateService(this.myDbContext, new Dictionary<string, IContext>(), this._MyDeficitTenant);

            this._MyDeficitPM.ChangeSetOp = ChangeSetOperation.Insert;
            this._MyDeficitPM.Tenant = this._MyDeficitTenant;
            //deficitPM.TapagFileID = tapagPM.id; // will be update while creating deficit counter (the new Tapag Records Id)
            this._MyDeficitPM.NotificationTypeCode = customResponse.DebtNotificationMessag.notificationType.ToString();
            this._MyDeficitPM.DebtNotificationNumber = customResponse.DebtNotificationMessag.debtNotificationID.ToString();
            this._MyDeficitPM.ProductionDate = customResponse.DebtNotificationMessag.prodactionDate;
            this._MyDeficitPM.DebtNotificationReason = customResponse.DebtNotificationMessag.debtNotificationReason;
            this._MyDeficitPM.RealesGoodsDescription = customResponse.DebtNotificationMessag.realesGoodsDescription;
            this._MyDeficitPM.PaymentOrderNumber = customResponse.DebtNotificationMessag.paymentOrderID.ToString();
            this._MyDeficitPM.ValidityDateTo = customResponse.DebtNotificationMessag.validityDateTo;
            this._MyDeficitPM.LeadingFileNumber = customResponse.DebtNotificationMessag.leadingFileNumber.ToString();

            // Create new record in Tapag Table
            GetTapagDetails(customResponse);
        }

        private void CreateConnectionTables(DE_NG_280_MSG11_DebtNotificationMessage customResponse)
        {
            var myDeclarationQueryService = new DeclarationQueryService(this._MyDeficitTenant);
            var tapagConnectionUpdateService = new TapagConnectionTableUpdateService(this.myDbContext, new Dictionary<string, IContext>(), this._MyDeficitTenant);
            var deficitParagraphTypeUpdateService = new DeficitConnFileParagraphTypeUpdateService(this.myDbContext, new Dictionary<string, IContext>(), this._MyDeficitTenant);

            LogMessagingUtil.Instance.AppendLine("DebtNotificationMessage: CreateConnectionTables- TapagConnectionTable and DeficitParagraphType");

            foreach (var deficitItem in customResponse.DeficitFile)
            {
                if (this._MyDeclarationPM != null && !string.IsNullOrEmpty(_MyDeclarationPM.Id))
                {
                    var declarationId = _MyDeclarationPM.Id;
                    if (!_MyDeclarationPM.DeclarationNumber.Equals(deficitItem.TPGIdentifier.fileNumber))
                    {
                        declarationId = myDeclarationQueryService.GetIdByDeclarationNumber(deficitItem.TPGIdentifier.fileNumber, this._MyDeficitTenant);
                    }
                    if (!string.IsNullOrEmpty(declarationId) && !string.IsNullOrEmpty(this._MyDeficitPM.TapagId))
                    {
                        var tapagConnectionTablePM = new TapagConnectionTablePM();
                        tapagConnectionTablePM.ChangeSetOp = ChangeSetOperation.Insert;
                        tapagConnectionTablePM.TapagId = this._MyDeficitPM.TapagId;
                        tapagConnectionTablePM.DeclarationId = declarationId;
                        tapagConnectionTablePM.Tenant = this._MyDeficitTenant;
                        tapagConnectionTablePM.CustomsTapagFile = deficitItem.TPGIdentifier.fileNumber;
                        tapagConnectionTablePM.CustomsNumeral = deficitItem.TPGIdentifier.numeral;

                        tapagConnectionUpdateService.Update(tapagConnectionTablePM, true);

                        List<string> paragraphTypeList = new List<string>();
                        foreach (var paragraphItem in deficitItem.DebtAmount)
                        {
                            if (!paragraphTypeList.Contains(paragraphItem.ParagraphType.ToString()))
                            {
                                var deficitParagraphTypePM = new DeficitConnFileParagraphTypePM();
                                deficitParagraphTypePM.ChangeSetOp = ChangeSetOperation.Insert;
                                deficitParagraphTypePM.DeficitId = this._MyDeficitPM.Id;
                                deficitParagraphTypePM.Tenant = this._MyDeficitTenant;
                                deficitParagraphTypePM.DeclarationId = declarationId;
                                deficitParagraphTypePM.ParagraphTypeCode = paragraphItem.ParagraphType.ToString();
                                deficitParagraphTypePM.Amount = paragraphItem.amount;
                                paragraphTypeList.Add(deficitParagraphTypePM.ParagraphTypeCode);

                                deficitParagraphTypeUpdateService.Update(deficitParagraphTypePM, true);
                            }
                        }
                    }
                }
            }

            var paymentOrderQueryService = new PaymentOrderQueryService(this._MyDeficitTenant);
            var myPaymentOrderUpdateService = new PaymentOrderUpdateService(this.myDbContext, new Dictionary<string, IContext>(), this._MyDeficitTenant);

            var paymentOrderId = paymentOrderQueryService.GetIdByPaymentNumber(this._MyDeficitPM.PaymentOrderNumber, this._MyDeficitTenant);
            if (!string.IsNullOrWhiteSpace(paymentOrderId) && this._MyDeclarationPM != null)
            {
                PaymentOrderPM paymentOrderPM = paymentOrderQueryService.GetSingle(paymentOrderId, true, false);
                paymentOrderPM.ChangeSetOp = ChangeSetOperation.Update;
                if (paymentOrderPM.CustomerId == null)
                {
                    paymentOrderPM.CustomerId = this._MyDeficitPM.CustomerId;
                }
                PaymentOrderConnectionTablePM paymentOrderConnectionTablePM = paymentOrderPM.PaymentOrderConnectionTables.FirstOrDefault(si => si.ConnectedEntityId == _MyDeclarationPM.Id && si.PaymentOrderId == paymentOrderId);
                if (paymentOrderConnectionTablePM == null)
                {
                    paymentOrderConnectionTablePM = new PaymentOrderConnectionTablePM();
                    paymentOrderConnectionTablePM.ChangeSetOp = ChangeSetOperation.Insert;
                    paymentOrderConnectionTablePM.Tenant = this._MyDeficitTenant;
                    paymentOrderConnectionTablePM.PaymentOrderId = paymentOrderId;
                    //paymentOrderConnectionTablePM.ConnectedEntityCode = "T";
                    //paymentOrderConnectionTablePM.ConnectedEntityId = this._MyDeficitPM.TapagId;
                    paymentOrderConnectionTablePM.ConnectedEntityCode = "D";
                    paymentOrderConnectionTablePM.ConnectedEntityId = _MyDeclarationPM.Id;
                    paymentOrderPM.PaymentOrderConnectionTables.Add(paymentOrderConnectionTablePM);
                }

                var fUStatusRemarks = "\n" + "סטטוס הוראה: " + paymentOrderPM.PaymentStatusName;
                if (paymentOrderPM.LastPayDate != null)
                {
                    fUStatusRemarks += "\n" + "תאריך אחרון לתשלום: " + ((DateTime)paymentOrderPM.LastPayDate).Date.ToString("dd/MM/yyyy");
                }
                fUStatusRemarks += "\n" + "סכום לתשלום: " + paymentOrderPM.TotalSumToPay
                                 + "\n" + "התהליך היוצר: " + paymentOrderPM.PaymentProcessName;
                var myInsertEventContextTagModel = new EventContextTagModel() // Indication to Create Unifreight Status "POR"
                {
                    CallProccessID = EventContextTagModel.ProccessEnum.TSH_MSG2_PaymentOrderReplyResponseServiceCreate,
                    EventCode = "POR",
                    EventRemarks = "הוראת תשלום " + paymentOrderPM.PaymentNumber + " נוצרה",
                    FUStatusRemarks = "הוראת תשלום " + paymentOrderPM.PaymentNumber + " נוצרה" + fUStatusRemarks,
                };
                paymentOrderPM.CurrentContextTag = myInsertEventContextTagModel;

                myPaymentOrderUpdateService.Update(paymentOrderPM, true);
            }
        }

        private void AnalyzeDeficitDocument(Attachment[] attachment, GenericRequestParams requestParams)
        {
            ICommonDataContext dataContext = CommonDataContext.GetContext(requestParams.Tenant);
            var documentsFilingService = new UnifreightDocumentsFilingService(dataContext, requestParams.Tenant, new CustomDocumentsFilingParams() { MainInterfaceCode = "280" });
            var documentTypeQuery = new DocumentTypeQuery(requestParams.Tenant);
            var documentsFilingQuery = new DocumentsFilingQuery(requestParams.Tenant);
            DocumentsFilingPM documentsFilingPM = null;

            if (attachment == null)//| _MyDeclarationPM == null
            {
                return;
            }

            foreach (var attachmentItem in attachment)
            {
                //Check if file already exists
                string objectTableId = "";
                string entityId = "";
                string childEntityId = "";
                if (_MyDeclarationPM != null) // If connected to Declaration
                {
                    objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                    entityId = _MyDeclarationPM.Id;
                    childEntityId = _MyDeficitPM.Id;
                }
                else
                {
                    objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Deficit");
                    entityId = _MyDeficitPM.Id;
                }
                var documentType = documentTypeQuery.GetSinglePMByCodeAndTenant("DEF", requestParams.Tenant);
                var documentsFilingPMList = documentsFilingQuery.GetDocumentsFilingPMsByEntityIdAndObjectTable(entityId, childEntityId, objectTableId, "I", requestParams.Tenant);

                foreach (var documentItem in documentsFilingPMList)
                {
                    if (documentItem.DocumentTypeId == documentType.Id)
                    {
                        documentsFilingPM = documentItem;
                        break;
                    }
                }

                if (documentsFilingPM == null)
                {
                    CreatePaymentDocument(attachmentItem, requestParams);
                }
                else
                {
                    UpdatePaymentDocument(documentsFilingPM, attachmentItem, requestParams);
                }
            }
        }

        private void UpdatePaymentDocument(DocumentsFilingPM documentsFilingPM, Attachment attachment, GenericRequestParams requestParams)
        {
            ICommonDataContext dataContext = CommonDataContext.GetContext(requestParams.Tenant);
            var documentsFilingService = new UnifreightDocumentsFilingService(dataContext, requestParams.Tenant, new CustomDocumentsFilingParams() { MainInterfaceCode = "280" });
            var documentTypeQuery = new DocumentTypeQuery(requestParams.Tenant);
            var documentsFilingQuery = new DocumentsFilingQuery(requestParams.Tenant);
            string logMessage = "";

            documentsFilingService.Update(documentsFilingPM, attachment.content,requestParams.LoggingUserId);
            if (this._MyDeclarationPM != null)
            {
                logMessage = " -For declaration " + _MyDeclarationPM.DeclarationNumber;
            }
            LogMessagingUtil.Instance.AppendLine("File document " + documentsFilingPM.Code + logMessage);
        }

        private void CreatePaymentDocument(Attachment attachment, GenericRequestParams requestParams)
        {
            ICommonDataContext dataContext = CommonDataContext.GetContext(requestParams.Tenant);
            var documentsFilingService = new UnifreightDocumentsFilingService(dataContext, requestParams.Tenant, new CustomDocumentsFilingParams() { MainInterfaceCode = "280" });
            var documentTypeQuery = new DocumentTypeQuery(requestParams.Tenant);
            string logMessage = "";

            var documentsFilingPM = new DocumentsFilingPM();
            documentsFilingPM.Tenant = requestParams.Tenant;
            var documentType = documentTypeQuery.GetSinglePMByCodeAndTenant("DEF", requestParams.Tenant);
            documentsFilingPM.DocumentTypeId = documentType.Id;
            documentsFilingPM.Name = "הודעת חיוב " + this._MyDeficitPM.DebtNotificationNumber;
            if (_MyDeclarationPM != null)
            {
                documentsFilingPM.EntityId = _MyDeclarationPM.Id;
                documentsFilingPM.ObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                documentsFilingPM.ChildEntityId = _MyDeficitPM.Id;
                documentsFilingPM.ChildObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Deficit");
                documentsFilingPM.ExternalEntityReference = _MyDeclarationPM.CustomFileNo;
                logMessage = " -For declaration " + _MyDeclarationPM.DeclarationNumber;
            }
            else
            {
                documentsFilingPM.ChildEntityId = _MyDeficitPM.Id;
                documentsFilingPM.ChildObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Deficit");
                logMessage = " -For deficit " + _MyDeficitPM.DebtNotificationNumber;
            }
            documentsFilingPM.ChildEntityReference = _MyDeficitPM.PaymentOrderNumber;
            documentsFilingPM.CreatedByUserId = requestParams.LoggingUserId;
            documentsFilingPM.OwnerId = requestParams.LoggingUserId;
            documentsFilingPM.UpdatedByUserId = requestParams.LoggingUserId;
            documentsFilingPM.ReceivedByUserId = requestParams.LoggingUserId;
            documentsFilingPM.DirectionCode = "I";
            documentsFilingPM.Description = "הודעת חיוב " + this._MyDeficitPM.DebtNotificationNumber;
            documentsFilingPM.ExternalEntityName = "CFIFILEM";
            documentsFilingPM.FileExtension = "PDF";

            documentsFilingService.Create(documentsFilingPM, attachment.content,requestParams.LoggingUserId);
            LogMessagingUtil.Instance.AppendLine("File document " + documentsFilingPM.Code + logMessage);
        }

        private string GetTapagConnection(string fileNumber, int numeral)
        {
            var tapagConnectionTableQueryService = new TapagConnectionTableQueryService(this._MyDeficitTenant);
            TapagConnectionTablePM tapagConnectionTablePM = tapagConnectionTableQueryService.GetTapagConnectionByFileAndNumeral(fileNumber, numeral,this._MyDeficitTenant);
            if(tapagConnectionTablePM != null)
            {
                return tapagConnectionTablePM.DeclarationId;
            }
            return null;
        }
    }
}
