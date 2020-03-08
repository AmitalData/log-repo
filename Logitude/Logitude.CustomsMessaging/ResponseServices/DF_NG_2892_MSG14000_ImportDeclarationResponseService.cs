using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Def.Messaging.Customs;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Common.Gen;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.ResponseServices.DeclarationErrorPointer;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnifreightIIG.Common.ImportDeclarationAmendmentServiceReference;
using UnifreightIIG.Common.MessageLib.Collateral;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer.DBWCO;
using Logitude.Customs.BL.BL;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
 
namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DF_NG_2892_MSG14000_ImportDeclarationResponseService :
        ResponseServiceBase<INF_MSG_GenericResponseData, INF_MSG_Generic, GenericRequestParams>
    {
        DeclarationPM _MyDeclarationPM;
        private bool _FastDelete;
        //private List<SupplierInvoiceItemsTaxesModPM> _SupplierInvoiceItemsTaxesModificationPMList;
        //public UnifreightIIG.Common.CommonIIGInterface.IResponseHeaderOrFault _ResponseHeaderExeption;
        public bool _IsSubmitDeclarationResponse { get; set; }
        public bool _IsRetrieveDeclarationResponse { get; set; }
        decimal? totGeneralTaxCalc = 0;
        decimal? totPurchaseCalc = 0;
        decimal? totVatCalc = 0;
        decimal? generalTax = 0;
        decimal? purchase = 0;
        decimal? vat = 0;

        DeclarationError _MyDeclarationError;
        decimal? _TotalBtlCoverageNISSum = 0;

        public override void OnRequestFail(INF_MSG_Generic customResponse, GenericRequestParams requestParams)
        {
            if (!String.IsNullOrWhiteSpace(requestParams.AppicationId))
            {
                CalculateDeclarationCourierStatus.UpdateCourierDeclarationStatusCode(requestParams.Tenant, requestParams.AppicationId);
            }
            base.OnRequestFail(customResponse, requestParams);
        }

        public override INF_MSG_GenericResponseData GetResponse(
            INF_MSG_Generic customResponse, GenericRequestParams requestParams)
        {

            /// itzik test     TestTrans(requestParams);
            return this.MyResponseData;
        }

        private void TestTrans(GenericRequestParams requestParams) /// itzik test 
        {
            DeclarationStatusRequestParams searchParams = new DeclarationStatusRequestParams()
            {
                LoggingEnabled = true,

                CustomFileNo = _MyDeclarationPM.CustomFileNo,
                DeclarationNumber = _MyDeclarationPM.DeclarationNumber,
                Tenant = requestParams.Tenant,
                RequestName = "Declaration Status Search",
                ResponseName = "Declaration Status Search",
                SuppressSplitWR = true
            };

            searchParams.RequestVIA = SendRequestVIA.WebServiceInteractive;
            var resData = Logitude.CustomsMessaging.MessagingServices.DF_NG_8250_Web01_DeclarationStatus_RequestMessagingService.SendInteractive(searchParams);
            if (!resData.Succeeded)
            {
                LogMessagingUtil.Instance.AppendLine("Sending Declaration Status Request Failed " + resData.CustomsRequestsSheetId + ", Message: " + resData.UserMessage);
                ///return;
            }
            else
            {
                LogMessagingUtil.Instance.AppendLine("Request Succeeded " + resData.CustomsRequestsSheetId);
            }

        }

        public override void Update(INF_MSG_Generic customResponse, GenericRequestParams requestParams)
        {
            var responseName = requestParams.ResponseName;
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myQueryService = new DeclarationQueryService(context);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            var mySupplierInvoiceItemsTaxUpdateService = new SupplierInvoiceItemsTaxUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            var mySupplierInvoiceItemVehicleModUpdateService = new SupplierInvoiceItemVehicleModUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant); // moran 20.10.15 - Task 17209 
            var mySupplierInvoiceItemModVehicleUpdateService = new SupplierInvoiceItemModVehicleUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant); // moran 24.11.15 - Task 17424 
            //var mySupplierInvoiceItemsTaxesModificationUpdateService = new SupplierInvoiceItemsTaxesModUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            var myDeclarationTaxUpdateService = new DeclarationTaxUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            var myDeclarationConstraintUpdateService = new DeclarationConstraintUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            var myDeclarationPaymentQueryService = new DeclarationPaymentQueryService(context);

            DeclarationErrorPointerService mydDclarationErrorPointerService = new DeclarationErrorPointerService();

            //var mySupplierInvioceItemCertificatUpdateService = new SupplierInvioceItemCertificatUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant); 

            this.MyResponseData = new INF_MSG_GenericResponseData();
 
            var swGetSingle = Stopwatch.StartNew();
            myQueryService.LoadSupplierInvoicesItemsParentsOnly = true;
            this._MyDeclarationPM = myQueryService.GetSingle(requestParams.AppicationId, true, false);
            LogMessagingUtil.Instance.AppendLine("myQueryService.GetSingle:Took:" + swGetSingle.ElapsedMilliseconds);

            if (this._MyDeclarationPM == null)
            {
                LogMessagingUtil.Instance.AppendLine("Can not find declaration" + requestParams.AppicationId);
                this.MyResponseData.ApplicationID = requestParams.AppicationId;
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.UserMessage = "Can not find declaration" + requestParams.AppicationId;
                return;
            }

 
             if (!string.IsNullOrWhiteSpace(this._MyDeclarationPM.Id))
            {
                if (this.MyRequestSheetParam == null)
                {
                    this.MyRequestSheetParam = new RequestSheetParam();
                }
                this.MyRequestSheetParam.CustomFileNo = this._MyDeclarationPM.CustomFileNo;
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                this.MyRequestSheetParam.EntityId1 = this._MyDeclarationPM.Id;
                this.MyRequestSheetParam.RequestDescription = "משוב לתיקון הצהרה  " + this._MyDeclarationPM.DeclarationNumber;
                 
            }
             float oldVersionId;
            float.TryParse(_MyDeclarationPM.VersionId, out oldVersionId);
            if (string.IsNullOrWhiteSpace(_MyDeclarationPM.VersionId))
            {
                oldVersionId = 0.1F;
            }
 
             if (_MyDeclarationPM.DepositionStatusCode == "R") _MyDeclarationPM.DepositionStatusCode = null;
            if (customResponse.ResponseContentHeader.Exception != null)
            {
                string userMessage = "";
                this._MyDeclarationPM.MarkAsChanged = false;
                var swErrosXml = Stopwatch.StartNew();

                foreach (var exception in customResponse.ResponseContentHeader.Exception)
                {
                    if (!string.IsNullOrWhiteSpace(userMessage))
                    {
                        userMessage = userMessage + @"
";
                    }
                    userMessage = userMessage + exception.ExeptionDescription;
                    //this._MyDeclarationPM.ErrosXml = mydDclarationErrorPointerService.AddDeclarationException(this._MyDeclarationPM.ErrosXml, "Buisness", exception, true);
                    LogMessagingUtil.Instance.AppendLine("ErrosXml:Took:" + swErrosXml.ElapsedMilliseconds);
                    switch (exception.ExeptionType)
                    {
                        case 2794:
                            {
                                this._MyDeclarationPM.DeclarationNumber = exception.ExceptionParms.FirstOrDefault();
                                if (!string.IsNullOrWhiteSpace(userMessage))
                                {
                                    userMessage = userMessage + @"
";
                                }
                                userMessage = userMessage + "עודכן מספר ההצהרה לפי רשומת הסוכן - יש לשדר את ההצהרה מחדש";
                                break;
                            }
                        case 1501:
                            {
                                UpdateUnifreightEvent("MPOA", requestParams.LoggingUserId);
                                userMessage = userMessage + "חסר יפוי כח";
                                break;
                            }
                        case 4589:
                            {
                                UpdateUnifreightEvent("MID", requestParams.LoggingUserId);
                                userMessage = userMessage + "חסר תצהיר יבואן";
                                if (string.IsNullOrWhiteSpace(_MyDeclarationPM.DepositionStatusCode)) _MyDeclarationPM.DepositionStatusCode = "R";
                                break;
                            }
                        case 2244:
                            {
                                UpdateUnifreightEvent("IDE", requestParams.LoggingUserId);
                                userMessage = userMessage + "תצהיר יבואן עומד לפוג";
                                break;
                            }
                    }
                }
                if (_IsSubmitDeclarationResponse != true) // only for sending declaration (2750)
                {
                    _MyDeclarationPM.IsChanged = true;
                }

                if (_MyDeclarationPM.UserNotes == "LoadTestOnProgress")
                {
                    _MyDeclarationPM.UserNotes = "LoadTest";
                }

                this._MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
                this._MyDeclarationPM.CurrentContextTag = Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.UpdateIIGExcptionConst;
                myDeclarationUpdateService.Update(this._MyDeclarationPM, true);

                if (this._MyDeclarationPM.IsCourierDeclaration) 
                {
 
                    var calculateDeclarationCourierStatus = new CalculateDeclarationCourierStatus(this._MyDeclarationPM);
                    calculateDeclarationCourierStatus.Update(
                        (currentDeclarationCourierStatusPM) =>
                        {

                            currentDeclarationCourierStatusPM.CourierDeclarationStatusCode = "X";
                        });
                }


                this.MyResponseData.ApplicationID = requestParams.AppicationId;
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.UserMessage = userMessage;
                this.MyResponseData.HasException = true;

                return;
            }









          
                //if (customResponse.ResponseContentHeader.Exception == null)
                //{
                //    string text = null;
                //    if (String.IsNullOrWhiteSpace(customResponse.ResponseContentHeader.Remark))
                //    {
                //        text = "No Declaration details in the Response " + requestParams.AppicationId;
                //        this.MyResponseData.UserMessage = text;
                //        LogMessagingUtil.Instance.AppendLine(text);
                //    }
                //    else
                //    {
                //        text = "No Declaration details in the Response " + customResponse.ResponseContentHeader.Remark + requestParams.AppicationId;
                //        this.MyResponseData.UserMessage = text;
                //        LogMessagingUtil.Instance.AppendLine(text);
                //    }
                //    this.MyResponseData.ApplicationID = requestParams.AppicationId; //Yuval Chalup 28.05.2015 TASK-13252+13509
                //    return;
                //}
   
            // if (customResponse != null)
            //{
            //    if (customResponse.Response != null)
            //    {
            //        if (customResponse.Response.Status != null)
            //        {
            //            if (customResponse.Response.Status.NameCode != null)
            //            {
            //                if (customResponse.Response.Status.NameCode.Value == "14")
            //                {
            //                    _MyDeclarationPM.PaymentDate = null;
            //                    LogMessagingUtil.Instance.AppendLine("Change Declaration Version From " + oldVersionId + "To " + _MyDeclarationPM.VersionId);
            //                    _MyDeclarationPM.CurrentContextTag = Logitude.Customs.BL.EntityUpdateServices.DeclarationUpdateService.UpdateUnifreightBillingConst;
            //                }
            //            }
            //        }
            //    }
            //}

        
 
            this.MyResponseData.UserMessage = "בקשה נשלחה בהצלחה";
            string declarationStatusTypeName = _MyDeclarationPM.DeclarationStatusTypeCode;
 

            MyResponseData.ApplicationID = requestParams.AppicationId;
            MyResponseData.Succeeded = true;
         
           
        }


        private void UpdateDepositionStatusCode()
        {
            if (_MyDeclarationError != null && _MyDeclarationError.Entitites != null && _MyDeclarationError.Entitites.Count > 0)
            {
                //Go over all the 'Entity'
                foreach (var entity in _MyDeclarationError.Entitites)
                {
                    if (entity.FieldErrors != null)
                    {
                        //Go over all the 'FieldErrors'
                        foreach (var fieldErrors in entity.FieldErrors)
                        {
                            //Get all 'FieldErrors' for the 'FieldError'
                            List<field> fieldList = (from a in entity.FieldErrors
                                                     where (a.Code == "4589")
                                                     select a).ToList();
                            if (fieldList.Count > 0)
                            {
                                if (string.IsNullOrWhiteSpace(_MyDeclarationPM.DepositionStatusCode)) _MyDeclarationPM.DepositionStatusCode = "R";
                                return;
                            }
                        }
                    }
                }
            }
            if (_MyDeclarationPM.DepositionStatusCode == "R") _MyDeclarationPM.DepositionStatusCode = null;
        }

        public void SendDeclarationPrint(DeclarationPM declarationPM, SendRequestVIA RequestVIA, GenericRequestParams requestParams) // moran 28.1.15 - Task 10005
        {

            LogMessagingUtil.Instance.AppendLine("SendDeclarationPrint");
            string decNum = declarationPM.DeclarationNumber;
            var decNumList = new List<string>();
            decNumList.Add(decNum);
            DF_NG_8302_Web03_DeclarationPrintRequestParams searchParams = new DF_NG_8302_Web03_DeclarationPrintRequestParams()
            {
                LoggingEnabled = true,
                CustomFileNo = declarationPM.CustomFileNo,
                DeclarationNumber = decNumList, //declarationPM.DeclarationNumber,
                Tenant = declarationPM.Tenant,
                RequestName = "Declaration Print (2750)",
                ResponseName = "Declaration Print (2750)",
                LoggingEntityId = declarationPM.Id,


                LoggingUserId = requestParams.LoggingUserId, //HD CALL#298426
            };
            
            searchParams.RequestVIA = RequestVIA; // SendRequestVIA.WebServiceBatch;
            var myRequestMessagingService = new DF_NG_8302_Web03_DeclarationPrintMessagingService();
            var resData = myRequestMessagingService.Send(searchParams);
            if (!resData.Succeeded)
            {
                LogMessagingUtil.Instance.AppendLine("Request Failed " + resData.CustomsRequestsSheetId + ", Message: " + resData.UserMessage);
                return;
            }
            LogMessagingUtil.Instance.AppendLine("Request Succeeded " + resData.CustomsRequestsSheetId);
        }

        //ITZIK+MIRT  private string GetErrosXmlFromResponseHeaderExeption()          {              return _ResponseHeaderExeption.ErrorDescription;         }

        private void DeleteDeclarationConstraints(DeclarationConstraintUpdateService myDeclarationConstraintUpdateService) // Delete old Constraints
        {
            if (this._MyDeclarationPM.DeclarationConstraints == null)
            {
                return;
            }

            foreach (var constraintItem in this._MyDeclarationPM.DeclarationConstraints)
            {
                constraintItem.ChangeSetOp = ChangeSetOperation.Delete;
                //myDeclarationConstraintUpdateService.Update(constraintItem, true);
                _MyDeclarationPM.DeletedDeclarationConstraints.Add(constraintItem);
            }
        }

        private List<DeclarationConstraintPM> BuildDeclarationConstraints(ResponseError[] responseError)
        {
            if (responseError == null)
            {
                return null;
            }
            //This section that stops analyzing when ConstraintID appears more than once in the response is CANCELLED - 
            //The new logic (below) analyze ONLY the first constraint of that ConstraintID
            //try
            //{
            //    responseError.Where(errorItem => errorItem.DMExtensions != null).ToList().ToDictionary(r => r.DMExtensions.ConstraintID.ToString());
            //}
            //catch (System.Exception)
            //{

            //    throw new System.Exception("הגיעה מספר אילוץ כפול, נא לפנות למלמ");
            //}

            var declarationConstraintPMList = new List<DeclarationConstraintPM>();
            foreach (var errorItem in responseError)
            {
                if (errorItem.DMExtensions != null)
                {
                    DeclarationConstraintPM declarationConstraintToCheckDistinct = null;
                    declarationConstraintToCheckDistinct = declarationConstraintPMList.Where(constraint => constraint.ConstraintNumber == errorItem.DMExtensions.ConstraintID.ToString()).FirstOrDefault();

                    if (declarationConstraintToCheckDistinct == null)
                    {
                        DeclarationConstraintPM declarationConstraint = null;
                        declarationConstraint = FindConstraintInList(errorItem.DMExtensions.ConstraintID.ToString(), _MyDeclarationPM.Tenant, _MyDeclarationPM.Id);

                        if (declarationConstraint == null)
                        {
                            declarationConstraint = new DeclarationConstraintPM();
                            declarationConstraint.ChangeSetOp = ChangeSetOperation.Insert;
                            declarationConstraint.ConstraintNumber = errorItem.DMExtensions.ConstraintID.ToString();
                            declarationConstraint.ConstraintTypeCode = errorItem.DMExtensions.ConstraintType.ToString();
                        }
                        else
                        {
                            declarationConstraint.ChangeSetOp = ChangeSetOperation.Update;
                            _MyDeclarationPM.DeletedDeclarationConstraints.Remove(declarationConstraint);
                        }

                        declarationConstraint.ConstraintStatusCode = errorItem.DMExtensions.ConstraintStatus.ToString();
                        declarationConstraintPMList.Add(declarationConstraint);
                    }
                }
            }
            return declarationConstraintPMList;
        }
        private List<DeclarationConstraintPM> BuildDeclarationConstraintsTEST(ResponseError[] responseError)
        {
            if (responseError == null)
            {
                return null;
            }

            var declarationConstraintPMList = new List<DeclarationConstraintPM>();

            try
            {
                responseError.Where(errorItem => errorItem.DMExtensions != null).ToList().ToDictionary(r => r.DMExtensions.ConstraintID.ToString());
            }
            catch (System.Exception)
            {

                throw new System.Exception("הגיעה מספר אילוץ כפול, נא לפנות למלמ");                
            }
            

            var myhshSet = new HashSet<string>();
            foreach (var item in responseError.Where(errorItem => errorItem.DMExtensions != null))
            {

                myhshSet.Add(item.DMExtensions.ConstraintID.ToString());
            }
            foreach (var hshSetKey in myhshSet)
            {
                ResponseError errorItem = null;
                errorItem = responseError.LastOrDefault(r => r.DMExtensions !=null && r.DMExtensions.ConstraintID.ToString() == hshSetKey);

                //if (errorItem.DMExtensions != null)
                {
                    DeclarationConstraintPM declarationConstraint = null;
                    declarationConstraint = FindConstraintInList(errorItem.DMExtensions.ConstraintID.ToString(), _MyDeclarationPM.Tenant, _MyDeclarationPM.Id);

                    if (declarationConstraint == null)
                    {
                        declarationConstraint = new DeclarationConstraintPM();
                        declarationConstraint.ChangeSetOp = ChangeSetOperation.Insert;
                        declarationConstraint.ConstraintNumber = errorItem.DMExtensions.ConstraintID.ToString();
                        declarationConstraint.ConstraintTypeCode = errorItem.DMExtensions.ConstraintType.ToString();
                    }
                    else
                    {
                        declarationConstraint.ChangeSetOp = ChangeSetOperation.Update;
                        _MyDeclarationPM.DeletedDeclarationConstraints.Remove(declarationConstraint);
                    }

                    declarationConstraint.ConstraintStatusCode = errorItem.DMExtensions.ConstraintStatus.ToString();
                    declarationConstraintPMList.Add(declarationConstraint);
                }
            }
            return declarationConstraintPMList;
        }

        private DeclarationConstraintPM FindConstraintInList(string constraintNumber, int tenant, string declarationID)
        {
            List<DeclarationConstraintPM> constraintList = (from a in _MyDeclarationPM.DeclarationConstraints
                                                            where a.ConstraintNumber == constraintNumber &&
                                                            a.Tenant == tenant && a.DeclarationID == declarationID
                                                            select a).ToList();

            if (constraintList.Count > 0)
            {
                return constraintList[0];
            }
            return null;
        }

        private void DeleteSupplierInvoiceItemsTaxes(SupplierInvoiceItemsTaxUpdateService mySupplierInvoiceItemsTaxUpdateService)
        {

            foreach (var si in _MyDeclarationPM.SupplierInvoices)
            {
                foreach (var supplierInvoiceItem in si.SupplierInvoiceItems)
                {
                    foreach (var item in supplierInvoiceItem.SupplierInvoiceItemTaxes)
                    {
                        item.ChangeSetOp = ChangeSetOperation.Delete;
                        mySupplierInvoiceItemsTaxUpdateService.Update(item, true);
                    }
                }
            }
        }

        private void DeleteDeclarationTaxes(DeclarationTaxUpdateService myDeclarationTaxUpdateService)
        {

            foreach (var item in _MyDeclarationPM.DeclarationTaxes)
            {
                item.ChangeSetOp = ChangeSetOperation.Delete;
                myDeclarationTaxUpdateService.Update(item, true);
            }
        }


        private void DeleteSupplierInvoiceItemsVehicleMods(SupplierInvoiceItemVehicleModUpdateService mySupplierInvoiceItemVehicleModUpdateService) // moran 20.10.15 - Task 17209
        {

            foreach (var si in _MyDeclarationPM.SupplierInvoices)
            {
                foreach (var supplierInvoiceItem in si.SupplierInvoiceItems)
                {
                    foreach (var vehicle in supplierInvoiceItem.SupplierInvoiceItemVehicles)
                    {
                        foreach (var vehicleMod in vehicle.SupplierInvoiceItemVehicleMods)
                        {
                            vehicleMod.ChangeSetOp = ChangeSetOperation.Delete;
                            mySupplierInvoiceItemVehicleModUpdateService.Update(vehicleMod, true);
                        }
                    }
                }
            }
        }


        private void DeleteSupplierInvoiceItemsModVehicles(SupplierInvoiceItemModVehicleUpdateService mySupplierInvoiceItemModVehicleUpdateService) // moran 24.11.15 - Task 17424
        {

            foreach (var si in _MyDeclarationPM.SupplierInvoices)
            {
                foreach (var supplierInvoiceItem in si.SupplierInvoiceItems)
                {
                    foreach (var modVehicle in supplierInvoiceItem.SupplierInvoiceItemModVehicles)
                    {
                        modVehicle.ChangeSetOp = ChangeSetOperation.Delete;
                        mySupplierInvoiceItemModVehicleUpdateService.Update(modVehicle, true);
                    }
                }
            }
        }

        private void DeleteSupplierInvioceItemCertificates(SupplierInvioceItemCertificatUpdateService mySupplierInvioceItemCertificatUpdateService)
        {
            foreach (var si in _MyDeclarationPM.SupplierInvoices)
            {
                foreach (var supplierInvoiceItem in si.SupplierInvoiceItems)
                {
                    foreach (var modCertificats in supplierInvoiceItem.SupplierInvioceItemCertificats)
                    {
                        modCertificats.ChangeSetOp = ChangeSetOperation.Delete;
                        mySupplierInvioceItemCertificatUpdateService.Update(modCertificats, true);
                    }
                }
            }
        }

        //private List<SupplierInvoicePM> GetSupplierInvoicesPM(
        //    INF_MSG_Generic customResponse)
        //{
        //    var supplierInvoicesPMList = new List<SupplierInvoicePM>();

        //    foreach (var goodsShipment in customResponse.Response.Declaration.GoodsShipment)
        //    {
        //        //var supplierInvoiceItemsTaxPM = new SupplierInvoiceItemsTaxPM();
        //        //var supplierInvoicePM = this._MyDeclarationPM.SupplierInvoices.FirstOrDefault(si => si.InvoiceNumber == goodsShipment.Invoice.ID.Value);
        //        var supplierInvoicePM = this._MyDeclarationPM.SupplierInvoices.FirstOrDefault(si => si.SequenceNumeric == goodsShipment.SequenceNumeric);

        //        if (supplierInvoicePM == null)
        //        {
        //            throw new System.Exception(
        //                "unable to find the supplierInvoicePM from goodsShipment.Invoice.ID.Value " + goodsShipment.Invoice.ID.Value);
        //        }
        //        var InvoiceCounterKey = supplierInvoicePM.InvoiceCounterKey;

        //        //Update supplier Valuation - Additional costs details from custom
        //        supplierInvoicePM.SupplierInvoiceModifications = GetSupplierInvoiceModifications(goodsShipment, ref supplierInvoicePM);
        //        //Update supplier items
        //        supplierInvoicePM.SupplierInvoiceItems = GetSupplierInvoiceItems(goodsShipment, ref supplierInvoicePM);

        //        if (goodsShipment.Invoice != null && goodsShipment.Invoice.DMExtensions != null && goodsShipment.Invoice.DMExtensions.RateNumeric != null)
        //        {
        //            supplierInvoicePM.ExchangeRate = goodsShipment.Invoice.DMExtensions.RateNumeric.Value;
        //        }

        //        supplierInvoicePM.ChangeSetOp = ChangeSetOperation.Update;
        //        supplierInvoicesPMList.Add(supplierInvoicePM);
        //    }

        //    return supplierInvoicesPMList;
        //}

        private List<SupplierInvoiceModificationPM> GetSupplierInvoiceModifications(DeclarationGoodsShipment goodsShipment, ref SupplierInvoicePM supplierInvoicePM)
        {
            // moran 26.5.15 - 13564 -->
            //var supplierInvoiceModificationPMList = new List<SupplierInvoiceModificationPM>();
            var supplierInvoiceModificationPMList = new List<SupplierInvoiceModificationPM>(supplierInvoicePM.SupplierInvoiceModifications);
            // moran 26.5.15 - 13564 <--
            if (goodsShipment.CustomsValuation == null)
            {
                return null;
            }

            //Check if there is a DECLARED Fee (I01) in message
            DeclarationGoodsShipmentCustomsValuation declarationGoodsShipmentCustomsValuation_I01 = goodsShipment.CustomsValuation.FirstOrDefault(rec => rec.ChargesTypeCode.Value == "I01");

            foreach (var valuationItem in goodsShipment.CustomsValuation)
            {
                if (valuationItem.ChargesTypeCode.Value != "67" && valuationItem.ChargesTypeCode.Value != "144")
                {
                    //If there is a DECLARED Fee (I01) in message:
                    //1 - Do NOT get the CALCULATED Fee (I02) from message
                    //2 - Delete the CALCULATED from DB
                    if (declarationGoodsShipmentCustomsValuation_I01 != null && valuationItem.ChargesTypeCode.Value == "I02")
                    {
                        var supplierInvoiceModificationPM = supplierInvoicePM.SupplierInvoiceModifications.FirstOrDefault(si => si.TypeCode == valuationItem.ChargesTypeCode.Value);
                        if (supplierInvoiceModificationPM != null)
                        {
                            //If exist delete
                            supplierInvoiceModificationPM.ChangeSetOp = ChangeSetOperation.Delete;
                            supplierInvoiceModificationPMList.Add(supplierInvoiceModificationPM);
                        }
                    }
                    else
                    {
                        var supplierInvoiceModificationPM = supplierInvoicePM.SupplierInvoiceModifications.FirstOrDefault(si => si.TypeCode == valuationItem.ChargesTypeCode.Value);
                        if (supplierInvoiceModificationPM != null)
                        {
                            //If exist update
                            supplierInvoiceModificationPM.ChangeSetOp = ChangeSetOperation.Update;
                        }
                        else
                        {
                            //Else create new SupplierInvoiceModification record
                            supplierInvoiceModificationPM = new SupplierInvoiceModificationPM();
                            supplierInvoiceModificationPM.ChangeSetOp = ChangeSetOperation.Insert;
                        }
                        supplierInvoiceModificationPM.TypeCode = valuationItem.ChargesTypeCode.Value;
                        supplierInvoiceModificationPM.CurrencyTypeCode = valuationItem.OtherChargeDeductionAmount.currencyID.ToString(); // TO CHECK? ENUM?
                        supplierInvoiceModificationPM.Amount = valuationItem.OtherChargeDeductionAmount.Value;
                        if (supplierInvoiceModificationPM.ChangeSetOp == ChangeSetOperation.Insert)
                        {
                            supplierInvoiceModificationPMList.Add(supplierInvoiceModificationPM);
                        }
                    }
                }
            }

            return supplierInvoiceModificationPMList;
        }
#if refreshWSDL20141230
        private SupplierInvoiceItemsModificationPM GetSupplierInvoiceItemsTaxesModifications(
            DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsValuationDeductionAdjustment 
            declarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsValuationDeductionAdjustment)
        {

            return null;
        }
#endif


        private List<SupplierInvoiceItemPM> GetSupplierInvoiceItems(
            DeclarationGoodsShipment goodsShipment,
            ref SupplierInvoicePM supplierInvoicePM)
        {
            var supplierInvoiceItemsPMList = new List<SupplierInvoiceItemPM>();
            if (supplierInvoicePM.IsAccumalated == true && supplierInvoicePM.SupplierInvoiceItems != null && supplierInvoicePM.SupplierInvoiceItems.Count > 0)
            {
                supplierInvoicePM.SupplierInvoiceItems.RemoveAll(rec => rec.IsParent != true);
            }
            foreach (var governmentAgencyGoodsItem in goodsShipment.GovernmentAgencyGoodsItem)
            {
                var supplierInvoiceItemPM = supplierInvoicePM.SupplierInvoiceItems.FirstOrDefault(si => si.SequenceNumeric == governmentAgencyGoodsItem.SequenceNumeric);
                //<--- Added by Yuval Chalup 26.05.2015 TASK-13473
                if (supplierInvoiceItemPM == null)
                {
                    if (governmentAgencyGoodsItem.Commodity != null)
                    {
                        if (governmentAgencyGoodsItem.Commodity.Classification != null)
                        {
                            if (governmentAgencyGoodsItem.Commodity.Classification.Count() > 0)
                            {
                                if (governmentAgencyGoodsItem.Commodity.Classification[0] != null)
                                {
                                    throw new System.Exception(
                                       "unable to find the supplierInvoiceItemPM from governmentAgencyGoodsItem.Commodity.Classification " + governmentAgencyGoodsItem.Commodity.Classification[0].ID.Value);
                                }
                            }
                        }
                    }
                    throw new System.Exception(
                       "unable to find the supplierInvoiceItemPM from governmentAgencyGoodsItem.SequenceNumeric " + governmentAgencyGoodsItem.SequenceNumeric);
                }
                //Added by Yuval Chalup 26.05.2015 TASK-13473 --->

                var supplierInvoiceItemsTaxPMList = new List<SupplierInvoiceItemsTaxPM>();
                //Update supplier item Valuation Adjustment - Commodity price adjustments
                supplierInvoiceItemPM.SupplierInvoiceItemsMods = GetSupplierInvoiceItemsModifications(governmentAgencyGoodsItem.ValuationAdjustment, supplierInvoiceItemPM);
                // moran 24.11.15 - Task 17424 -->
                supplierInvoiceItemPM.SupplierInvoiceItemModVehicles = GetSupplierInvoiceItemsModVehicles(governmentAgencyGoodsItem.DMExtensions.VehicleValuationAdjustment, supplierInvoiceItemPM);
                // moran 24.11.15 - Task 17424 <--

                supplierInvoiceItemPM.SupplierInvioceItemCertificats = GetSupplierInvioceItemCertificats(supplierInvoicePM, supplierInvoiceItemPM);

                if (governmentAgencyGoodsItem.Commodity == null)
                {
                    continue;
                }
                //TODO:DDDD

                if (governmentAgencyGoodsItem.Commodity.DutyTaxFee != null)
                {
                    foreach (var dutyTaxFee in governmentAgencyGoodsItem.Commodity.DutyTaxFee)
                    {
                        var supplierInvoiceItemsTaxPM = new SupplierInvoiceItemsTaxPM();
                        supplierInvoiceItemsTaxPM.ChangeSetOp = ChangeSetOperation.Insert;
                        //supplierInvoiceItemsTaxPM.DeclarationId = this._MyDeclarationPM.Id; //Removed by Yuval Chalup 26.05.2015 TASK-13473 (Move to SupplierInvoiceItemsTaxUpdateService.OnUpdating)
                        supplierInvoiceItemsTaxPM.Tenant = this._MyDeclarationPM.Tenant;
                        //supplierInvoiceItemsTaxPM.InvoiceCounterKey = supplierInvoicePM.InvoiceCounterKey; //Removed by Yuval Chalup 26.05.2015 TASK-13473 (Move to SupplierInvoiceItemsTaxUpdateService.OnUpdating)
                        //supplierInvoiceItemsTaxPM.LineNumber = supplierInvoiceItemPM.LineNumber; //Removed by Yuval Chalup 26.05.2015 TASK-13473 (Move to SupplierInvoiceItemsTaxUpdateService.OnUpdating)
                        supplierInvoiceItemsTaxPM.TaxTypeCode = dutyTaxFee.TypeCode.Value;
                        if (dutyTaxFee.DutyRegimeCode != null)
                        {
                            supplierInvoiceItemsTaxPM.TradeAgreementTypeCode = dutyTaxFee.DutyRegimeCode.Value;
                        }
                        supplierInvoiceItemsTaxPM.TaxRate = dutyTaxFee.TaxRate;
                        supplierInvoiceItemsTaxPM.TaxBaseAmount = dutyTaxFee.AdValoremTaxBaseAmount.Value;
                        supplierInvoiceItemsTaxPM.TaxAmount = dutyTaxFee.DMExtensions.CalculatedTax.Amount.Value;
                        supplierInvoiceItemsTaxPM.DeferedTaxAmount = dutyTaxFee.DMExtensions.CalculatedTax.DeferedTaxAmount.Value;
                        if (dutyTaxFee.DMExtensions.CalculatedTax.DefinedPerUnitMethod != null)
                        {
                            supplierInvoiceItemsTaxPM.DefinedPerUnitMeasure = dutyTaxFee.DMExtensions.CalculatedTax.DefinedPerUnitMethod.Value;
                        }
                        supplierInvoiceItemsTaxPM.AlternateRate = dutyTaxFee.DMExtensions.CalculatedTax.AlternateRate.Value;
                        if (dutyTaxFee.DMExtensions.CalculatedTax.AlternateDefinedPerUnitMeasure != null)
                        {
                            supplierInvoiceItemsTaxPM.AlternateDefinedPerUnitMeasure = dutyTaxFee.DMExtensions.CalculatedTax.AlternateDefinedPerUnitMeasure.Value;
                        }
                        if (dutyTaxFee.DMExtensions.CalculatedTax.DefinedPerUnitQuantity != null)
                        {
                            supplierInvoiceItemsTaxPM.DefinedPerUnitQuantity = dutyTaxFee.DMExtensions.CalculatedTax.DefinedPerUnitQuantity.Value;
                        }
                        if (dutyTaxFee.DMExtensions.CalculatedTax.AlternateDefinedPerUnitQuantity != null)
                        {
                            supplierInvoiceItemsTaxPM.AlternateDefinedPerUnitQuant = dutyTaxFee.DMExtensions.CalculatedTax.AlternateDefinedPerUnitQuantity.Value;
                        }
                        if (dutyTaxFee.DMExtensions.CalculatedTax.MeasurementUnitCode != null)
                        {
                            supplierInvoiceItemsTaxPM.MeasurementUnitCode = dutyTaxFee.DMExtensions.CalculatedTax.MeasurementUnitCode.Value;
                        }
                        if (dutyTaxFee.DMExtensions.CalculatedTax.AlternateMeasurementUnit != null)
                        {
                            supplierInvoiceItemsTaxPM.AlternateMeasurementUnitCode = dutyTaxFee.DMExtensions.CalculatedTax.AlternateMeasurementUnit.Value;
                        }
                        if (dutyTaxFee.DMExtensions.CalculatedTax.TradeLevyNumber != null)
                        {
                            supplierInvoiceItemsTaxPM.TradeLevyNumber = dutyTaxFee.DMExtensions.CalculatedTax.TradeLevyNumber.Value;
                        }
                        if (dutyTaxFee.DMExtensions.CalculatedTax.TotalBtlCoverageNIS != null)
                        {
                            supplierInvoiceItemsTaxPM.TotalBtlCoverageNIS = dutyTaxFee.DMExtensions.CalculatedTax.TotalBtlCoverageNIS.Value;
                            _TotalBtlCoverageNISSum = _TotalBtlCoverageNISSum + dutyTaxFee.DMExtensions.CalculatedTax.TotalBtlCoverageNIS.Value;
                        }
                        // moran 21.11.13 - Bug 2083 - change handle -->
                        //AddSupplierInvoiceItemsTaxesModificationPM(supplierInvoiceItemsTaxPM, governmentAgencyGoodsItem.Commodity);
                        //supplierInvoiceItemsTaxPM.SupplierInvoiceItemsTaxesModifications = GetSupplierInvoiceItemsTaxesModifications(governmentAgencyGoodsItem.Commodity.DMExtensions.ValuationDeductionAdjustment, supplierInvoiceItemsTaxPM);
                        // moran 21.11.13 - Bug 2083 - change handle <--
                        supplierInvoiceItemsTaxPMList.Add(supplierInvoiceItemsTaxPM);
                    }
                }

                supplierInvoiceItemPM.SupplierInvoiceItemTaxes = supplierInvoiceItemsTaxPMList;

                totGeneralTaxCalc = 0;
                totPurchaseCalc = 0;
                totVatCalc = 0;

                generalTax = 0;
                purchase = 0;
                vat = 0;

                foreach (var tax in supplierInvoiceItemPM.SupplierInvoiceItemTaxes)
                {
                    if (tax.TaxTypeCode == "1")
                    {
                        generalTax += tax.TaxAmount;
                    }
                    if (tax.TaxTypeCode == "16")
                    {
                        purchase += tax.TaxAmount;
                    }
                    if (tax.TaxTypeCode == "15")
                    {
                        vat += tax.TaxAmount;
                    }
                }

                // moran 6.10.15 - Task 17209 --> 
                supplierInvoiceItemPM.SupplierInvoiceItemVehicles = GetSupplierInvoiceItemsVehicles(governmentAgencyGoodsItem.DMExtensions.Vehicle, supplierInvoiceItemPM);
                // moran 6.10.15 - Task 17209 <--
                if (supplierInvoiceItemPM.SupplierInvoiceItemVehicles != null && supplierInvoiceItemPM.SupplierInvoiceItemVehicles.Count() > 0 && supplierInvoiceItemPM.SupplierInvoiceItemVehicles.LastOrDefault().SupplierInvoiceItemVehicleAdds != null && supplierInvoiceItemPM.SupplierInvoiceItemVehicles.LastOrDefault().SupplierInvoiceItemVehicleAdds.Count() > 0)
                {
                    decimal? diff = 0;
                    if (generalTax != totGeneralTaxCalc)
                    {
                        diff = generalTax - totGeneralTaxCalc;
                        supplierInvoiceItemPM.SupplierInvoiceItemVehicles.LastOrDefault().SupplierInvoiceItemVehicleAdds.LastOrDefault().ChassisTax += diff;
                    }
                    if (purchase != totPurchaseCalc)
                    {
                        diff = purchase - totPurchaseCalc;
                        supplierInvoiceItemPM.SupplierInvoiceItemVehicles.LastOrDefault().SupplierInvoiceItemVehicleAdds.LastOrDefault().ChassisPurchaseTax += diff;
                    }
                    if (vat != totVatCalc)
                    {
                        diff = vat - totVatCalc;
                        supplierInvoiceItemPM.SupplierInvoiceItemVehicles.LastOrDefault().SupplierInvoiceItemVehicleAdds.LastOrDefault().ChassisVat += diff;
                    }
                }
                supplierInvoiceItemPM.ChangeSetOp = ChangeSetOperation.Update;
                supplierInvoiceItemsPMList.Add(supplierInvoiceItemPM);
                var NewSupplierInvioceItemCertificats = supplierInvoiceItemPM.SupplierInvioceItemCertificats.Where(r => r.ChangeSetOp == ChangeSetOperation.Insert).ToList();
                if (supplierInvoicePM.IsAccumalated == true && supplierInvoiceItemPM.IsParent == true && NewSupplierInvioceItemCertificats != null && NewSupplierInvioceItemCertificats.Count > 0)
                {
                    AddNewCertificatesToChildItems(supplierInvoiceItemsPMList, supplierInvoiceItemPM, NewSupplierInvioceItemCertificats);
                }
            }
            
            return supplierInvoiceItemsPMList;
        }

        private void AddNewCertificatesToChildItems(List<SupplierInvoiceItemPM> supplierInvoiceItemsPMList, SupplierInvoiceItemPM supplierInvoiceItemParentPM, List<SupplierInvioceItemCertificatPM> newSupplierInvioceItemCertificats)
        {
            
            List<SupplierInvioceItemCertificatPM> newSupplierInvioceItemCertificatsForChild = newSupplierInvioceItemCertificats;
            var qs = new SupplierInvoiceItemQueryService(this._MyDeclarationPM.Tenant);
            SupplierInvioceItemCertificatQueryService supplierInvioceItemsCertificateQueryService = new SupplierInvioceItemCertificatQueryService(this._MyDeclarationPM.Tenant);
            List<SupplierInvoiceItemPM> supplierInvoiceItemsPMListforcert = qs.GetSupplierInvoiceItemsByParentFullPM(supplierInvoiceItemParentPM.DeclarationId, supplierInvoiceItemParentPM.CounterKey, supplierInvoiceItemParentPM.LineNumber, this._MyDeclarationPM.Tenant);
            foreach (var childSupplierInvoiceItemPM in supplierInvoiceItemsPMListforcert)
            {
                //newSupplierInvioceItemCertificatsForChild.ForEach(i => i.LineNumber = childSupplierInvoiceItemPM.LineNumber);
                childSupplierInvoiceItemPM.SupplierInvioceItemCertificats.AddRange(newSupplierInvioceItemCertificatsForChild);

                childSupplierInvoiceItemPM.ChangeSetOp = ChangeSetOperation.Update;
                supplierInvoiceItemsPMList.Add(childSupplierInvoiceItemPM);
            }
        }

        private List<SupplierInvioceItemCertificatPM> GetSupplierInvioceItemCertificats(SupplierInvoicePM supplierInvoicePM, SupplierInvoiceItemPM supplierInvoiceItemPM)
        {
            //if (supplierInvoiceItemPM.SupplierInvioceItemCertificats != null && supplierInvoiceItemPM.SupplierInvioceItemCertificats.Count() > 0)
            //{
            //    return supplierInvoiceItemPM.SupplierInvioceItemCertificats;
            //}

            //var supplierInvioceItemCertificatPMList = new List<SupplierInvioceItemCertificatPM>();
            var supplierInvioceItemCertificatPMList = supplierInvoiceItemPM.SupplierInvioceItemCertificats;


            List<string> certificateCodeListFromErrosXml = GetCertificateCodeListFromErrosXml("SupplierInvoice", supplierInvoicePM.SequenceNumeric.ToString(), "SupplierInvoiceItem", supplierInvoiceItemPM.SequenceNumeric.ToString());

            if (certificateCodeListFromErrosXml == null)
            {
                return supplierInvioceItemCertificatPMList;
                return null;
            }
            foreach (var certificateCodeFromErrosXml in certificateCodeListFromErrosXml)
            {
                //Check if the code exists current SupplierInvioceItemCertificats
                List<string> entityList = (from a in supplierInvoiceItemPM.SupplierInvioceItemCertificats
                                           where (a.ReqConfirmationTypeCode == certificateCodeFromErrosXml)
                                           select a.ReqConfirmationTypeCode).ToList();

                //If it does NOT exist - Add it to SupplierInvioceItemCertificat
                if (entityList.Count == 0)
                {
                    SupplierInvioceItemCertificatPM supplierInvioceItemCertificatPM = new SupplierInvioceItemCertificatPM();
                    supplierInvioceItemCertificatPM.ChangeSetOp = ChangeSetOperation.Insert;

                    supplierInvioceItemCertificatPM.Tenant = this._MyDeclarationPM.Tenant;
                    supplierInvioceItemCertificatPM.ReqConfirmationTypeCode = certificateCodeFromErrosXml;

                    supplierInvioceItemCertificatPMList.Add(supplierInvioceItemCertificatPM);
                }
            }

            return supplierInvioceItemCertificatPMList;
        }

        private List<string> GetCertificateCodeListFromErrosXml(string myChild1Type, string myChild1Sequence, string myChild2Type, string myChild2Sequence)
        {
            if (_MyDeclarationError == null)
            {
                return null;
            } 
            if (_MyDeclarationError.Entitites.Count == 0)
            {
                return null;
            }

            List<string> certificateCodeListFromErrosXml = new List<string>();

            //Get all 'Entity' for the SupplierInvoiceItem
            List<Entity> entityList = (from a in _MyDeclarationError.Entitites
                                        where (a.Child1Type == myChild1Type && a.Child1Sequence == myChild1Sequence
                                        && a.Child2Type == myChild2Type && a.Child2Sequence == myChild2Sequence)
                                        select a).ToList();

            if (entityList.Count >0) //Bug 23715: שליחת הצהרה- מתקבלת שגיאה שקשורה לאישורים
            //if (entityList.Count != null)
            {
                //Go over all the 'Entity'
                foreach (var entity in entityList)
                {
                    if (entity.FieldErrors != null)
                    {
                        //Go over all the 'FieldErrors'
                        foreach (var fieldErrors in entity.FieldErrors)
                        {
                            //Get all 'FieldErrors' for the 'FieldError'
                            List<field> fieldList = (from a in entity.FieldErrors
                                                     where (a.Code == "2592" && a.Fieldcode == "ClassificationCode")
                                                     select a).ToList();
                            //if (fieldList.Count != null)
                            if (fieldList.Count > 0)// Bug 23715: שליחת הצהרה- מתקבלת שגיאה שקשורה לאישורים
                            {
                                //Go over all the 'FieldErrors'
                                foreach (var field in fieldList)
                                {
                                    var messageError = field.MessageError;
                                    messageError = messageError.Substring(messageError.IndexOf("#") + 1, messageError.LastIndexOf("#") - messageError.IndexOf("#") - 1);

                                    string[] codes = messageError.Split(new string[] { ";" }, StringSplitOptions.None);
                                    for (int i = 0; i < codes.Length; i++)
                                    {
                                        var code = codes[i];
                                        var charList = new List<char>(); // moran 19.10.16 - Bug 23715 - update handle -->
                                        charList.Add(' ');
                                        charList.Add(',');
                                        code = code.Trim(charList.ToArray());
                                        if (!string.IsNullOrWhiteSpace(code))
                                        {
                                            if (code.Length <= 4)//&& code.Length==3 Bug 23715: שליחת הצהרה- מתקבלת שגיאה שקשורה לאישורים
                                            {
                                                var confirmationType = new ConfirmationTypeRepository(_MyDeclarationPM.Tenant);
                                                var myConfirmationType = confirmationType.GetSingle(code);
                                                if (myConfirmationType != null)
                                                {
                                                    certificateCodeListFromErrosXml.Add(code);
                                                }
                                                else
                                                {
                                                    LogMessagingUtil.Instance.AppendLine("Certificate " + code + " does not exist in DB.");
                                                }
                                            }
                                            else
                                            {
                                                LogMessagingUtil.Instance.AppendLine("Certificate " + code + " is too large.");
                                            }
                                        } // moran 19.10.16 - Bug 23715 - update handle <--
                                    }
                                }
                            }
                        }
                    }
                }
                return certificateCodeListFromErrosXml;
            }
            else
            {
                return null;
            }
        }


        private List<SupplierInvoiceItemModVehiclePM> GetSupplierInvoiceItemsModVehicles(DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsVehicleValuationAdjustment[] declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsVehicleValuationAdjustment, SupplierInvoiceItemPM supplierInvoiceItemPM)
        {// moran 24.11.15 - Task 17424
            // moran 6.12.15 - Task 19037 -->
            //var supplierInvoiceItemModVehiclePMList = new List<SupplierInvoiceItemModVehiclePM>(supplierInvoiceItemPM.SupplierInvoiceItemModVehicles);
            var supplierInvoiceItemModVehiclePMList = new List<SupplierInvoiceItemModVehiclePM>();
            // moran 6.12.15 - Task 19037 <--
            if (declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsVehicleValuationAdjustment == null)
            {
                return null;
            }

            foreach (var valuationAdjustmentItem in declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsVehicleValuationAdjustment)
            {

                //var supplierInvoiceItemModVehiclePM = supplierInvoiceItemPM.SupplierInvoiceItemModVehicles.FirstOrDefault(si => si.AdjustmentTypeCode == valuationAdjustmentItem.AdjustmentType.Value);
                //if (supplierInvoiceItemModVehiclePM != null)
                //{
                //If exist update
                //  supplierInvoiceItemModVehiclePM.ChangeSetOp = ChangeSetOperation.Update;
                //}
                //else
                //{
                //Else create new SupplierInvoiceModification record
                // supplierInvoiceItemModVehiclePM = new SupplierInvoiceItemModVehiclePM();
                SupplierInvoiceItemModVehiclePM supplierInvoiceItemModVehiclePM = new SupplierInvoiceItemModVehiclePM();
                supplierInvoiceItemModVehiclePM.ChangeSetOp = ChangeSetOperation.Insert;
                //}
                supplierInvoiceItemModVehiclePM.Tenant = this._MyDeclarationPM.Tenant;
                supplierInvoiceItemModVehiclePM.AdjustmentTypeCode = valuationAdjustmentItem.AdjustmentType.Value;
                supplierInvoiceItemModVehiclePM.DeductAmount = valuationAdjustmentItem.DeductAmount.Value;

                supplierInvoiceItemModVehiclePMList.Add(supplierInvoiceItemModVehiclePM);
            }

            return supplierInvoiceItemModVehiclePMList;
        }


#if refreshWSDL20141230
        // moran 21.11.13 - Bug 2083 - add GetSupplierInvoiceItemsTaxesModifications
        private List<SupplierInvoiceItemsTaxesModificationPM> GetSupplierInvoiceItemsTaxesModifications(DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsValuationDeductionAdjustment[] declarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsValuationDeductionAdjustment, SupplierInvoiceItemsTaxPM supplierInvoiceItemsTaxPM)
        {
            var supplierInvoiceItemsTaxesModificationPMList = new List<SupplierInvoiceItemsTaxesModificationPM>();
            if (declarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsValuationDeductionAdjustment == null)
            {
                return null;
            }

            foreach (var valuationDeductionAdjustment in declarationGoodsShipmentGovernmentAgencyGoodsItemCommodityDMExtensionsValuationDeductionAdjustment)
            {

                var supplierInvoiceItemsTaxesModificationPM = supplierInvoiceItemsTaxPM.SupplierInvoiceItemsTaxesModifications.FirstOrDefault(si => si.TypeCode == valuationDeductionAdjustment.ChargesTypeCode.Value);
                if (supplierInvoiceItemsTaxesModificationPM != null)
                {
                    //If exist update
                    supplierInvoiceItemsTaxesModificationPM.ChangeSetOp = ChangeSetOperation.Update;
                }
                else
                {
                    //Else create new SupplierInvoiceModification record
                    supplierInvoiceItemsTaxesModificationPM = new SupplierInvoiceItemsTaxesModificationPM();
                    supplierInvoiceItemsTaxesModificationPM.ChangeSetOp = ChangeSetOperation.Insert;
                }
                supplierInvoiceItemsTaxesModificationPM.TypeCode = valuationDeductionAdjustment.ChargesTypeCode.Value;
                supplierInvoiceItemsTaxesModificationPM.CurrencyTypeCode = valuationDeductionAdjustment.DeductAmount.currencyID.ToString();
                supplierInvoiceItemsTaxesModificationPM.Amount = valuationDeductionAdjustment.DeductAmount.Value;
                supplierInvoiceItemsTaxesModificationPM.LineNumber = supplierInvoiceItemsTaxPM.LineNumber;
                supplierInvoiceItemsTaxesModificationPM.TaxTypeCode = supplierInvoiceItemsTaxPM.TaxTypeCode;
                supplierInvoiceItemsTaxesModificationPM.Tenant = supplierInvoiceItemsTaxPM.Tenant;
                supplierInvoiceItemsTaxesModificationPM.DeclarationId = supplierInvoiceItemsTaxPM.DeclarationId;
                supplierInvoiceItemsTaxesModificationPMList.Add(supplierInvoiceItemsTaxesModificationPM);
            }

            return supplierInvoiceItemsTaxesModificationPMList;
        }

        
#endif

        // moran 6.10.15 - Task 17209 -->
        //private List<SupplierInvoiceItemVehiclePM> GetSupplierInvoiceItemsVehicles(DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification[] declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification, SupplierInvoiceItemPM supplierInvoiceItemPM)
        //{
        //    //var supplierInvoiceItemVehiclePMList = new List<SupplierInvoiceItemVehiclePM>(supplierInvoiceItemPM.SupplierInvoiceItemVehicles);
        //    var supplierInvoiceItemVehiclePMList = new List<SupplierInvoiceItemVehiclePM>();

        //    if (declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification == null || declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification.Count() < 1)
        //    {
        //        return null;
        //    }

        //    foreach (var vehicle in declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification)
        //    {

        //        var supplierInvoiceItemVehiclePM = supplierInvoiceItemPM.SupplierInvoiceItemVehicles.FirstOrDefault(si => si.VehicleTypeCode == vehicle.IDTypeCode.Value && (si.RichbitFileNumber == vehicle.ID.Value || si.VehicleChassisNumber == vehicle.ID.Value));
        //        if (supplierInvoiceItemVehiclePM == null)
        //        {

        //            throw new System.Exception(
        //               "unable to find the supplierInvoiceItemVehiclePM from vehicle ID " + vehicle.ID.Value + " and vehicle type " + vehicle.IDTypeCode.Value);
        //        }


        //        supplierInvoiceItemVehiclePM.ChangeSetOp = ChangeSetOperation.Update;

        //        supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleMods = GetSupplierInvoiceItemsVehicleMods(vehicle.VehicleValuationAdjustment, supplierInvoiceItemVehiclePM);
        //        // moran 21.3.16 - Task 20132 -->
        //        supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleAdds = GetSupplierInvoiceItemVehicleAdds(vehicle, supplierInvoiceItemVehiclePM, supplierInvoiceItemPM);
        //        // moran 21.3.16 - Task 20132 <--
        //        supplierInvoiceItemVehiclePMList.Add(supplierInvoiceItemVehiclePM); 
        //    }

        //    return supplierInvoiceItemVehiclePMList;
        //} // moran 6.10.15 - Task 17209 <--

        private List<SupplierInvoiceItemVehiclePM> GetSupplierInvoiceItemsVehicles(DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification[] declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification, SupplierInvoiceItemPM supplierInvoiceItemPM)
        {
            //var supplierInvoiceItemVehiclePMList = new List<SupplierInvoiceItemVehiclePM>(supplierInvoiceItemPM.SupplierInvoiceItemVehicles);
            var supplierInvoiceItemVehiclePMList = new List<SupplierInvoiceItemVehiclePM>();

            DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification vehicle = null;
            foreach (var supplierInvoiceItemVehiclePM in supplierInvoiceItemPM.SupplierInvoiceItemVehicles)
            {
                if (declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification == null || declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification.Count() < 1)
                {
                    vehicle = null;
                }
                else
                {
                    vehicle = declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification.FirstOrDefault(v => v.IDTypeCode.Value == supplierInvoiceItemVehiclePM.VehicleTypeCode && (v.ID.Value == supplierInvoiceItemVehiclePM.RichbitFileNumber || v.ID.Value == supplierInvoiceItemVehiclePM.VehicleChassisNumber));
                }
                supplierInvoiceItemVehiclePM.ChangeSetOp = ChangeSetOperation.Update;

                if (vehicle == null)
                {
                    supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleMods = GetSupplierInvoiceItemsVehicleMods(null, supplierInvoiceItemVehiclePM);
                }
                else
                {
                    supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleMods = GetSupplierInvoiceItemsVehicleMods(vehicle.VehicleValuationAdjustment, supplierInvoiceItemVehiclePM);
                }

                supplierInvoiceItemVehiclePMList.Add(supplierInvoiceItemVehiclePM);
            }


            vehicle = null;
            foreach (var supplierInvoiceItemVehiclePM in supplierInvoiceItemVehiclePMList)
            {
                if (declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification == null || declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification.Count() < 1)
                {
                    vehicle = null;
                }
                else
                {
                    vehicle = declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification.FirstOrDefault(v => v.IDTypeCode.Value == supplierInvoiceItemVehiclePM.VehicleTypeCode && (v.ID.Value == supplierInvoiceItemVehiclePM.RichbitFileNumber || v.ID.Value == supplierInvoiceItemVehiclePM.VehicleChassisNumber));
                }
                supplierInvoiceItemVehiclePM.ChangeSetOp = ChangeSetOperation.Update;

                supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleAdds = GetSupplierInvoiceItemVehicleAdds(vehicle, supplierInvoiceItemVehiclePM, supplierInvoiceItemPM, supplierInvoiceItemVehiclePMList);

            }

            return supplierInvoiceItemVehiclePMList;
        }


        private List<SupplierInvoiceItemVehicleModPM> GetSupplierInvoiceItemsVehicleMods(DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentificationVehicleValuationAdjustment[] declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentificationVehicleValuationAdjustment, SupplierInvoiceItemVehiclePM supplierInvoiceItemVehiclePM)
        { // moran 6.10.15 - Task 17209 -->
            //var supplierInvoiceItemVehicleModPMList = new List<SupplierInvoiceItemVehicleModPM>(supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleMods);
            var supplierInvoiceItemVehicleModPMList = new List<SupplierInvoiceItemVehicleModPM>();

            if (declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentificationVehicleValuationAdjustment == null || declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentificationVehicleValuationAdjustment.Count() < 1)
            {
                return null;
            }

            foreach (var vehicleMod in declarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentificationVehicleValuationAdjustment)
            {

                //var supplierInvoiceItemVehicleModPM = supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleMods.FirstOrDefault(si => si.AdjustmentTypeCode == vehicleMod.AdjustmentType.Value);

                //if (supplierInvoiceItemVehicleModPM != null)
                //{
                //If exist update
                //    supplierInvoiceItemVehicleModPM.ChangeSetOp = ChangeSetOperation.Update;
                //}
                //else
                //{
                //Else create new  record
                SupplierInvoiceItemVehicleModPM supplierInvoiceItemVehicleModPM = new SupplierInvoiceItemVehicleModPM();
                supplierInvoiceItemVehicleModPM.ChangeSetOp = ChangeSetOperation.Insert;
                //}

                supplierInvoiceItemVehicleModPM.Tenant = this._MyDeclarationPM.Tenant;
                supplierInvoiceItemVehicleModPM.AdjustmentTypeCode = vehicleMod.AdjustmentType.Value;
                supplierInvoiceItemVehicleModPM.DeductAmount = vehicleMod.DeductAmount.Value;

                supplierInvoiceItemVehicleModPMList.Add(supplierInvoiceItemVehicleModPM);
            }

            return supplierInvoiceItemVehicleModPMList;
        } // moran 6.10.15 - Task 17209 <--

        private List<SupplierInvoiceItemVehicleAddPM> GetSupplierInvoiceItemVehicleAdds(DeclarationGoodsShipmentGovernmentAgencyGoodsItemDMExtensionsProductIdentification vehicle, SupplierInvoiceItemVehiclePM supplierInvoiceItemVehiclePM, SupplierInvoiceItemPM supplierInvoiceItemPM, List<SupplierInvoiceItemVehiclePM> supplierInvoiceItemVehiclePMList = null)
        { // moran 21.3.16 - Task 20132 
            
            var supplierInvoiceItemVehicleAddPM = supplierInvoiceItemVehiclePM.SupplierInvoiceItemVehicleAdds.FirstOrDefault(si => si.DeclarationId == supplierInvoiceItemVehiclePM.DeclarationId && si.InvoiceItemLineNumber == supplierInvoiceItemVehiclePM.InvoiceItemLineNumber && si.LineNumber == supplierInvoiceItemVehiclePM.LineNumber);
            if (supplierInvoiceItemVehicleAddPM == null)
            {
                return null; // moran 24.4.16 - Task 21126
                throw new System.Exception(
                   "unable to find the supplierInvoiceItemVehicleAddPM from Declaration Id " + supplierInvoiceItemVehiclePM.DeclarationId + " and Supplier Invoice Item Line " + supplierInvoiceItemVehiclePM.InvoiceItemLineNumber + " and Supplier Invoice Item Vehicle Line " + supplierInvoiceItemVehiclePM.LineNumber);
            }


            supplierInvoiceItemVehicleAddPM.ChangeSetOp = ChangeSetOperation.Update;

            var supplierInvoiceItemVehicleAddPMList = new List<SupplierInvoiceItemVehicleAddPM>();

            decimal? allDeduction = 0;
            decimal? chassisDeduction = 0;

            if (supplierInvoiceItemVehiclePMList != null && supplierInvoiceItemVehiclePMList.Count() > 0)
            {
                foreach (var vehicleMod in supplierInvoiceItemVehiclePMList)
                {
                    foreach (var Deduction in vehicleMod.SupplierInvoiceItemVehicleMods)
                    {
                        if ((!string.IsNullOrWhiteSpace(vehicleMod.RichbitFileNumber) && vehicleMod.RichbitFileNumber == supplierInvoiceItemVehiclePM.RichbitFileNumber) || (!string.IsNullOrWhiteSpace(vehicleMod.VehicleChassisNumber) && vehicleMod.VehicleChassisNumber == supplierInvoiceItemVehiclePM.VehicleChassisNumber))
                        {
                            chassisDeduction += Deduction.DeductAmount;
                        }

                        allDeduction += Deduction.DeductAmount;

                    }
                }
            }

            supplierInvoiceItemVehicleAddPM.ChassisPurchaseTax = ((allDeduction + purchase) / supplierInvoiceItemPM.ItemPrice) * supplierInvoiceItemVehicleAddPM.VehicleValue - chassisDeduction;
            supplierInvoiceItemVehicleAddPM.ChassisTax = (generalTax / supplierInvoiceItemPM.ItemPrice) * supplierInvoiceItemVehicleAddPM.VehicleValue;
            supplierInvoiceItemVehicleAddPM.ChassisVat = (vat / supplierInvoiceItemPM.ItemPrice) * supplierInvoiceItemVehicleAddPM.VehicleValue;

            totGeneralTaxCalc += supplierInvoiceItemVehicleAddPM.ChassisTax;
            totPurchaseCalc += supplierInvoiceItemVehicleAddPM.ChassisPurchaseTax;
            totVatCalc += supplierInvoiceItemVehicleAddPM.ChassisVat;

            supplierInvoiceItemVehicleAddPMList.Add(supplierInvoiceItemVehicleAddPM);

            return supplierInvoiceItemVehicleAddPMList;
        }

        private List<SupplierInvoiceItemsModPM> GetSupplierInvoiceItemsModifications(DeclarationGoodsShipmentGovernmentAgencyGoodsItemValuationAdjustment[] declarationGoodsShipmentGovernmentAgencyGoodsItemValuationAdjustment, SupplierInvoiceItemPM supplierInvoiceItemsPM)
        {
            // moran 26.5.15 - 13564 -->
            //var supplierInvoiceItemsModificationPMList = new List<SupplierInvoiceItemsModificationPM>();
            var supplierInvoiceItemsModificationPMList = new List<SupplierInvoiceItemsModPM>(supplierInvoiceItemsPM.SupplierInvoiceItemsMods);
            // moran 26.5.15 - 13564 <--
            if (declarationGoodsShipmentGovernmentAgencyGoodsItemValuationAdjustment == null)
            {
                return null;
            }

            foreach (var valuationAdjustmentItem in declarationGoodsShipmentGovernmentAgencyGoodsItemValuationAdjustment)
            {

                var supplierInvoiceItemsModificationPM = supplierInvoiceItemsPM.SupplierInvoiceItemsMods.FirstOrDefault(si => si.TypeCode == valuationAdjustmentItem.AdditionCode.Value);
                if (supplierInvoiceItemsModificationPM != null)
                {
                    //If exist update
                    supplierInvoiceItemsModificationPM.ChangeSetOp = ChangeSetOperation.Update;
                }
                else
                {
                    //Else create new SupplierInvoiceModification record
                    supplierInvoiceItemsModificationPM = new SupplierInvoiceItemsModPM();
                    supplierInvoiceItemsModificationPM.ChangeSetOp = ChangeSetOperation.Insert;
                }
                supplierInvoiceItemsModificationPM.TypeCode = valuationAdjustmentItem.AdditionCode.Value;
                supplierInvoiceItemsModificationPM.CurrencyTypeCode = valuationAdjustmentItem.AmountAmount.currencyID.ToString();
                supplierInvoiceItemsModificationPM.Amount = valuationAdjustmentItem.AmountAmount.Value;

                supplierInvoiceItemsModificationPMList.Add(supplierInvoiceItemsModificationPM);
            }

            return supplierInvoiceItemsModificationPMList;
        }


        private void AddSupplierInvoiceItemsTaxesModificationPM(
            SupplierInvoiceItemsTaxPM supplierInvoiceItemsTaxPM,
            DeclarationGoodsShipmentGovernmentAgencyGoodsItemCommodity declarationGoodsShipmentGovernmentAgencyGoodsItemCommodity)
        {

            // return;
            //var currPM = //supplierInvoiceItemsTax.SupplierInvoiceItemsTaxesModifications[0] ;
            //        new SupplierInvoiceItemsTaxesModPM()
            //        {

            //            DeclarationId = supplierInvoiceItemsTaxPM.DeclarationId,

            //            InvoiceCounterKey = supplierInvoiceItemsTaxPM.InvoiceCounterKey,
            //            ChangeSetOp = ChangeSetOperation.Insert,
            //            Tenant = supplierInvoiceItemsTaxPM.Tenant,

            //            TaxTypeCode = supplierInvoiceItemsTaxPM.TaxTypeCode,
            //            TypeCode = "1",
            //            Amount = 0,
            //            LineNumber = 1,
            //            CurrencyTypeCode = "1"

            //        };
#if refreshWSDL20141230
            var valuationDeductionAdjustment = declarationGoodsShipmentGovernmentAgencyGoodsItemCommodity.DMExtensions.ValuationDeductionAdjustment.FirstOrDefault();
            if (valuationDeductionAdjustment != null)
            {
                currPM.Amount= valuationDeductionAdjustment.DeductAmount.Value ;
                currPM.CurrencyTypeCode = valuationDeductionAdjustment.DeductAmount.currencyID.ToString() ;//??
                currPM.LineNumber = 1;//??

                currPM.TypeCode = valuationDeductionAdjustment.ChargesTypeCode.Value;


            }

            
#endif
            //this._SupplierInvoiceItemsTaxesModificationPMList.Add(currPM);

            //declarationGoodsShipmentGovernmentAgencyGoodsItemCommodity.DMExtensions.DutyRegimeCode 
            //declarationGoodsShipmentGovernmentAgencyGoodsItemCommodity.DMExtensions.ValuationDeductionAdjustment[0].
            //myList.

            ///throw new NotImplementedException();
        }


        //private List<DeclarationTaxPM> GetDeclarationTaxesPM(DF_NG_2754_MSG10004_ImportDeclarationResponse customResponse)
        //{
        //    var declarationTaxPMList = new List<DeclarationTaxPM>();

        //    if (customResponse.Response.Declaration.DutyTaxFee == null)
        //    {
        //        return declarationTaxPMList;
        //    }

        //    foreach (var dutyTaxFee in customResponse.Response.Declaration.DutyTaxFee)
        //    {
        //        var declarationTaxPM = new DeclarationTaxPM();
        //        declarationTaxPM.ChangeSetOp = ChangeSetOperation.Insert;
        //        declarationTaxPM.DeclarationId = this._MyDeclarationPM.Id;
        //        declarationTaxPM.Tenant = this._MyDeclarationPM.Tenant;
        //        declarationTaxPM.TaxTypeCode = dutyTaxFee.TypeCode.Value;
        //        declarationTaxPM.TotalAmount = dutyTaxFee.DMExtensions.CalculatedTax.Amount.Value;
        //        declarationTaxPM.DeferredTaxAmount = dutyTaxFee.DMExtensions.CalculatedTax.DeferedTaxAmount.Value;
        //        declarationTaxPM.TaxBaseAmount = dutyTaxFee.AdValoremTaxBaseAmount.Value;

        //        declarationTaxPMList.Add(declarationTaxPM);
        //    }

        //    return declarationTaxPMList;
        //}

        private void UpdateUnifreightEvent(string eventCode, string loggingUserId)
        {
            switch (eventCode)
            {
                case "MPOA":
                    RaiseUnifreightEvent("MPOA", "MPOA", "");
                    break;
                case "MID":
                    RaiseUnifreightEvent("MID", "MID", "");
                    break;
                case "IDE":
                    RaiseUnifreightEvent("IDE", "IDE", "");
                    break;
            }

        }

        private void RaiseUnifreightEvent(string eventCode, string unifrieghtEvent, string eventRemarks)
        {
            try
            {
                string loggingUserId = AuthenticationUtil.ResolveUserId(_MyDeclarationPM.Tenant);

                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {

                    Tenant = _MyDeclarationPM.Tenant,
                    objectTableName = "Customs.Declaration",
                    EventCode = eventCode,
                    notes = eventRemarks,
                    CommunicationLoggingEntityReference = _MyDeclarationPM.DeclarationNumber,
                    EntityId = _MyDeclarationPM.Id,
                    UserId = loggingUserId,
                    CommunicationSubject = "Event from logitude",
                    MyUnifreightEventParam = new UnifreightEventParam()
                    {
                        Code = unifrieghtEvent,
                        Mode = UnifreightEventMode.@new,
                        EventDateTime = DateTime.Now,
                        Entname = "CFIFILEM",
                        PrimaryNum = _MyDeclarationPM.CustomFileNo,
                        EventRemarks = eventRemarks,
                    }
                };

                LogMessagingUtil.Instance.AppendLine("AmitalEventTracer.CreateTraceEvent: eventCode = " + eventCode + " CustomFileNo= " + _MyDeclarationPM.CustomFileNo + "   ");
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel, true);

            }
            catch (System.Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }

   

 
        //public override void Update(INF_MSG_Generic customResponse, GenericRequestParams requestParams)
        //{
        //    this.MyResponseData = new INF_MSG_GenericResponseData();
        //    this.MyResponseData.ApplicationID = requestParams.AppicationId;
        //    this.MyResponseData.Succeeded = true;
        //    this.MyResponseData.UserMessage = "Success";
        //    this.MyResponseData.HasException = true;

        //    return;
        //}

       
    }
}
