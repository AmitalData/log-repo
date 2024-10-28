using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel;
using Logitude.Customs.Data;
using Logitude.Customs.BL.Validators;
using Unifreight.BL.EntityQueryServices;
using Unifreight.BL.EntityPMs.UGenerated;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Server.Tools.Helpers;
using System.Text.RegularExpressions;
using Logitude.Customs.Data.EntityMapping;

namespace Logitude.Customs.BL.BL
{
    public class CalculateDeclarationCourierStatus
    {
        private DeclarationPM declarationPM;
        public string ImporterCode;

        public static void UpdateCourierDeclarationStatusCode(int Tenant, string DeclarationId)
        {
            var customContext = CustomContext.GetContext(Tenant);
            DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(customContext);
            DeclarationCourierStatusPM currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(DeclarationId, true, false);
            if (currentDeclarationCourierStatusPM != null)
            {
                string prevVal = null;
                string currvVal = null;
                CalculateDeclarationCourierStatus calculateDeclarationCourierStatus = new CalculateDeclarationCourierStatus(null, DeclarationId, Tenant);
                prevVal = currentDeclarationCourierStatusPM.CourierDeclarationStatusCode;
                calculateDeclarationCourierStatus.CalcCourierDeclarationStatusCode(currentDeclarationCourierStatusPM);
                currvVal = currentDeclarationCourierStatusPM.CourierDeclarationStatusCode;

                if (prevVal != currvVal)
                {
                    DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(customContext, new Dictionary<string, IContext>(), Tenant);
                    currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                    declarationCourierStatusUpdateService.Update(currentDeclarationCourierStatusPM, true);
                }

            }
        }
        public static void UpdateCourierManifestStatusCode(int Tenant, string DeclarationId)
        {
            var customContext = CustomContext.GetContext(Tenant);
            DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(customContext);
            DeclarationCourierStatusPM currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(DeclarationId, true, false);
            if (currentDeclarationCourierStatusPM != null)
            {
                string prevVal = null;
                string currvVal = null;
                CalculateDeclarationCourierStatus calculateDeclarationCourierStatus = new CalculateDeclarationCourierStatus(null, DeclarationId, Tenant);
                prevVal = currentDeclarationCourierStatusPM.CourierManifestStatusCode;
                calculateDeclarationCourierStatus.CalcCourierManifestStatusCode(currentDeclarationCourierStatusPM);
                currvVal = currentDeclarationCourierStatusPM.CourierManifestStatusCode;

                if (prevVal != currvVal)
                {
                    DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(customContext, new Dictionary<string, IContext>(), Tenant);
                    currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                    declarationCourierStatusUpdateService.Update(currentDeclarationCourierStatusPM, true);
                }
            }
        }

        public CalculateDeclarationCourierStatus(DeclarationPM declarationPM, string declarationId = null, int tenant = 0)
        {
            if (declarationPM != null)
            {
                this.declarationPM = declarationPM;
            }
            else if (!string.IsNullOrWhiteSpace(declarationId) && tenant > 0)
            {
                var context = CustomContext.GetContext(tenant);
                DeclarationQueryService declarationQueryService = new DeclarationQueryService(context);
                declarationQueryService.LoadSupplierInvoicesWithItems = false;
                this.declarationPM = declarationQueryService.GetSingle(declarationId, true, false);
            }
        }
        public void Update(Action<DeclarationCourierStatusPM> UPDATEDeclarationCourierStatusPM)
        {
            var context = CustomContext.GetContext(this.declarationPM.Tenant);
            DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
            DeclarationCourierStatusPM currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(this.declarationPM.Id, true, false);
            if (currentDeclarationCourierStatusPM != null)
            {

                //currentDeclarationCourierStatusPM.CourierDeclarationStatusCode = "X";
                UPDATEDeclarationCourierStatusPM(currentDeclarationCourierStatusPM);
                var declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(context, new Dictionary<string, IContext>(), this.declarationPM.Tenant);
                currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                declarationCourierStatusUpdateService.Update(currentDeclarationCourierStatusPM, true);

            }
        }
        public DeclarationCourierStatusPM CalcAll(DeclarationCourierStatusPM _MyDeclarationCourierStatusPM = null)
        {
            LogMessagingUtil.Instance.AppendLine("CalcAll()");
            if (declarationPM == null) return null;
            if (declarationPM.IsCourierDeclaration)
            {
                var context = CustomContext.GetContext(declarationPM.Tenant);
                DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
                DeclarationCourierStatusPM myDeclarationCourierStatusPM = _MyDeclarationCourierStatusPM!= null? _MyDeclarationCourierStatusPM : declarationCourierStatusQueryService.GetSingle(declarationPM.Id, true, false);
                if (myDeclarationCourierStatusPM == null)
                {
                    myDeclarationCourierStatusPM =
                        declarationPM?.MyEcomInsert?.MyDeclarationCourierStatusPM 
                        ??new DeclarationCourierStatusPM()
                    {
                        DeclarationId = declarationPM.Id,
                        Tenant = declarationPM.Tenant,
                        IsClosedForFollowUp = false,
                        IsCourierMissingClassification = false,
                        CourierDeclarationStatusCode = "",
                        ChangeSetOp = ChangeSetOperation.Insert,
                    };
                    myDeclarationCourierStatusPM.DeclarationId = declarationPM.Id;
                    myDeclarationCourierStatusPM.IsClosedForFollowUp = false;
                    myDeclarationCourierStatusPM.IsClosedForFollowUp = false;
                    myDeclarationCourierStatusPM.IsCourierMissingClassification = false;
                    myDeclarationCourierStatusPM.CourierDeclarationStatusCode = "";
                }
                else
                {
                    myDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                }

                CalcCourierManifestStatusCode(myDeclarationCourierStatusPM);
                CalcTotalInvoiceAmountInUSD(myDeclarationCourierStatusPM);
                CalcDocumentStatusCode(myDeclarationCourierStatusPM);
                CalcCourierDeclarationStatusCode(myDeclarationCourierStatusPM);
                CalcCourierPaymentStatusCode(myDeclarationCourierStatusPM);
                CalcIsCourierMissingClassification(myDeclarationCourierStatusPM);
               CalcHighLowValue(myDeclarationCourierStatusPM);
                CalcSpecialActionStatus(myDeclarationCourierStatusPM);
                CalcFastIndividualProcess(myDeclarationCourierStatusPM);
                CalcDeclarationPendings902(myDeclarationCourierStatusPM);

                    CalcDeclarationPendings906(myDeclarationCourierStatusPM);
                CalcDeclarationPendings908(myDeclarationCourierStatusPM);

               LogMessagingUtil.Instance.AppendLine("CalcAll()->903");

                var updateDeclarationPending903InvalidPhoneNumberService = new UpdateDeclarationPending903InvalidPhoneNumberService(declarationPM);
                LogMessagingUtil.Instance.AppendLine("CalcAll->calc903");
                    updateDeclarationPending903InvalidPhoneNumberService.Calc(myDeclarationCourierStatusPM);

               

                return myDeclarationCourierStatusPM;

            }
            return null;
        }


        public void CalcDocumentStatusCode(DeclarationCourierStatusPM myDeclarationCourierStatusPM)
        {
            LogMessagingUtil.Instance.AppendLine("CalcDocumentStatusCode()");
            if (myDeclarationCourierStatusPM == null) return;
            //Set DocumentStatusCode
            if (string.IsNullOrWhiteSpace(myDeclarationCourierStatusPM.DocumentStatusCode))
            {
                LogMessagingUtil.Instance.AppendLine("string.IsNullOrWhiteSpace(myDeclarationCourierStatusPM.DocumentStatusCode)");

                myDeclarationCourierStatusPM.DocumentStatusCode = "M";
                return;
            }
            LogMessagingUtil.Instance.AppendLine("!string.IsNullOrWhiteSpace(myDeclarationCourierStatusPM.DocumentStatusCode)");

            var customContext = CustomContext.GetContext(myDeclarationCourierStatusPM.Tenant);
            CustomsDocumentsTicketQueryService myCustomsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(customContext);
            List<CustomsDocumentsTicketPM> customsDocumentsTicketPMList = myCustomsDocumentsTicketQueryService.GetCustomsDocumentsTicketPMsByEntityIdAndChilds(declarationPM.Id, "", "", "", myDeclarationCourierStatusPM.Tenant, "Declaration");

            /*else*/
            if (IsDocumentError(myDeclarationCourierStatusPM, customsDocumentsTicketPMList))
            {
                myDeclarationCourierStatusPM.DocumentStatusCode = "X";

            }
            else if (IsDocumentMissing(myDeclarationCourierStatusPM, customsDocumentsTicketPMList))
            {
                LogMessagingUtil.Instance.AppendLine("IsDocumentMissing");

                myDeclarationCourierStatusPM.DocumentStatusCode = "M";
            }

        }
        public void CalcMissingDocumentStatusCode(DeclarationCourierStatusPM myDeclarationCourierStatusPM)
        {
            if (myDeclarationCourierStatusPM == null) return;
            
            var customContext = CustomContext.GetContext(myDeclarationCourierStatusPM.Tenant);
            CustomsDocumentsTicketQueryService myCustomsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(customContext);
            List<CustomsDocumentsTicketPM> customsDocumentsTicketPMList = myCustomsDocumentsTicketQueryService.GetCustomsDocumentsTicketPMsByEntityIdAndChilds(declarationPM.Id, "", "", "", myDeclarationCourierStatusPM.Tenant, "Declaration");
            CustomDocumentTypeQueryService docTypeQuery = new CustomDocumentTypeQueryService(customContext);
               
            var existCustomsDocumentTypes = customsDocumentsTicketPMList.Where(d => 
            (d.DocumentTypeCode == "380" || d.DocumentTypeCode == "ILD") && d.DocumentsFilingId != null)
                .Select(x=>x.DocumentTypeCode).Distinct().ToList();

            if (existCustomsDocumentTypes == null || existCustomsDocumentTypes.Count == 0)
            {
                myDeclarationCourierStatusPM.MissedDocumentStatusCode = null;//חסר שניהם
            }
            else
            {
                if (existCustomsDocumentTypes.Count == 1)
                    myDeclarationCourierStatusPM.MissedDocumentStatusCode = existCustomsDocumentTypes.Contains("380") ? "C" : "I";
                else
                {
                    myDeclarationCourierStatusPM.MissedDocumentStatusCode = "V";

                }
            }
        }

        public void CalcSpecialActionStatus(DeclarationCourierStatusPM myDeclarationCourierStatusPM)
        {
            if (myDeclarationCourierStatusPM == null) return;

            //Set SpecialActionStatus
            DeclarationMamanSpecialActionRepository declarationMamanSpecialActionRepository = new DeclarationMamanSpecialActionRepository(myDeclarationCourierStatusPM.Tenant);
            List<DeclarationMamanSpecialAction> list = declarationMamanSpecialActionRepository.GetDeclarationMamanSpecialActionByDeclarationId(myDeclarationCourierStatusPM.DeclarationId, myDeclarationCourierStatusPM.Tenant);

            if (list != null && list.Count() > 0)
            {
                int isError = list.Where(SA => SA.MamanSpecialActionStatusCode == "2").ToList().Count();
                if (isError > 0)
                {
                    myDeclarationCourierStatusPM.SpecialActionStatus = "X";
                }
                else
                {
                    myDeclarationCourierStatusPM.SpecialActionStatus = list.All(SA => SA.MamanSpecialActionStatusCode == "1") ? "V" : null;
                }
            }
        }

        public void CalcHighLowValue(DeclarationCourierStatusPM myDeclarationCourierStatusPM)
        {
            var setting = CustomsSettingQueryService.GetSettingByTenant(myDeclarationCourierStatusPM.Tenant);

          

                    if (myDeclarationCourierStatusPM == null) return;
                //Set HighLowValue
                DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(myDeclarationCourierStatusPM.Tenant);

                string defValue = defaultValueQueryService.GetDefault("ISRAEL", "CGO_HIGH_VALUE", "NON", "NON", myDeclarationCourierStatusPM.Tenant);
                    decimal defaultAmount = 0;
                    var boolvar = (decimal.TryParse(defValue, out defaultAmount));

                    if (myDeclarationCourierStatusPM.TotalInvoiceAmountInUSD > defaultAmount)
                    {
                        myDeclarationCourierStatusPM.HighLowValue = "H";
                    }
                    else
                    {
                        myDeclarationCourierStatusPM.HighLowValue = "L";
                    }
               
        }

        

        public void CalcIsCourierMissingClassification(DeclarationCourierStatusPM myDeclarationCourierStatusPM)
        {
            if (declarationPM == null || myDeclarationCourierStatusPM == null) return;
            //Set IsCourierMissingClassification
            if (declarationPM.PaymentDate.HasValue)
            {
                myDeclarationCourierStatusPM.IsCourierMissingClassification = false;
            }
            else
            {
                if (declarationPM.SupplierInvoices != null && declarationPM.SupplierInvoices.Count() > 0)
                {

                    List<SupplierInvoicePM> emptyClassificationCodeList = declarationPM.SupplierInvoices.
                        Where(SI => SI.SupplierInvoiceItems != null && SI.SupplierInvoiceItems.Any(u => string.IsNullOrWhiteSpace(u.ClassificationCode))).ToList();

                    if (emptyClassificationCodeList != null && emptyClassificationCodeList.Count() > 0)
                    {
                        myDeclarationCourierStatusPM.IsCourierMissingClassification = true;
                    }
                    else
                    {
                        List<SupplierInvoicePM> emptyItemsList = declarationPM.SupplierInvoices.
                        Where(SI => SI.SupplierInvoiceItems == null || SI.SupplierInvoiceItems.Count() == 0).ToList();
                        if (emptyItemsList != null && emptyItemsList.Count() > 0)
                        {
                            myDeclarationCourierStatusPM.IsCourierMissingClassification = true;
                        }
                        else
                        {
                            myDeclarationCourierStatusPM.IsCourierMissingClassification = false;
                        }
                    }
                }
                else
                {
                    myDeclarationCourierStatusPM.IsCourierMissingClassification = true;
                }
            }
        }

        public void CalcCourierPaymentStatusCode(DeclarationCourierStatusPM myDeclarationCourierStatusPM)
        {
            if (declarationPM == null || myDeclarationCourierStatusPM == null || (myDeclarationCourierStatusPM != null && myDeclarationCourierStatusPM.CourierPaymentStatusCode == "P")) return;
            //Set CourierPaymentStatusCode
            if (declarationPM.PaymentDate == null)
            {
                if (declarationPM.DeclarationStatusTypeCode == "13" && myDeclarationCourierStatusPM.CourierDeclarationStatusCode == "V")
                {
                    myDeclarationCourierStatusPM.CourierPaymentStatusCode = "R";
                }
                else
                {
                    myDeclarationCourierStatusPM.CourierPaymentStatusCode = "";
                }
            }
            else
            {
                switch (declarationPM.PaymentStatusCode)
                {
                    case "":
                    case null:
                        myDeclarationCourierStatusPM.CourierPaymentStatusCode = "O";
                        break;
                    case "3":
                        myDeclarationCourierStatusPM.CourierPaymentStatusCode = "P";
                        break;
                    default:
                        myDeclarationCourierStatusPM.CourierPaymentStatusCode = "";
                        break;
                }
            }
        }

        public void CalcCourierDeclarationStatusCode(DeclarationCourierStatusPM myDeclarationCourierStatusPM)
        {
            LogMessagingUtil.Instance.AppendLine("CalcCourierDeclarationStatusCode()");
            if (declarationPM == null || myDeclarationCourierStatusPM == null) return;
            //Set CourierDeclarationStatusCode according to Declaration Message Required fields
            CustomsRequiredFieldErrors errorsForDeclaration = CustomsRequiredFieldsValidator.GetRequiredFieldErrorsForDeclaration(declarationPM.Id, declarationPM.Tenant, declarationPM);
            if (errorsForDeclaration != null && errorsForDeclaration.RequiredFields != null && errorsForDeclaration.RequiredFields.Count() > 0)
            {
                LogMessagingUtil.Instance.AppendLine("RequiredFields missed, CourierDeclarationStatusCode = M");
                myDeclarationCourierStatusPM.CourierDeclarationStatusCode = "M";
            }
            else if (myDeclarationCourierStatusPM.TotalInvoiceAmountInUSD > 150 && string.IsNullOrEmpty(declarationPM.ImporterId) && string.IsNullOrEmpty(declarationPM.ImporterCode))
            {
                LogMessagingUtil.Instance.AppendLine("TotalInvoiceAmountInUSD > 150, CourierDeclarationStatusCode = M");
                myDeclarationCourierStatusPM.CourierDeclarationStatusCode = "M";
            }
            else if (myDeclarationCourierStatusPM.DocumentStatusCode == "M")
            {
                LogMessagingUtil.Instance.AppendLine("DocumentStatusCode is M so CourierDeclarationStatusCode = M");
                myDeclarationCourierStatusPM.CourierDeclarationStatusCode = "M";
            }
            else
            {
                if (string.IsNullOrWhiteSpace(declarationPM.DeclarationStatusTypeCode) || declarationPM.IsChanged == true && myDeclarationCourierStatusPM.CourierDeclarationStatusCode == "V")
                {
                    LogMessagingUtil.Instance.AppendLine("CourierDeclarationStatusCode set to R");
                    myDeclarationCourierStatusPM.CourierDeclarationStatusCode = "R";
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(declarationPM.PaymentDate.ToString()))
                    {
                        LogMessagingUtil.Instance.AppendLine("CourierDeclarationStatusCode set to V");
                        myDeclarationCourierStatusPM.CourierDeclarationStatusCode = "V";
                    }
                    else
                    {
                        if (ImporterCode == null)
                        { // importercode from u2l changed 

                            switch (declarationPM.DeclarationStatusTypeCode)
                            {
                                case "12":
                                    myDeclarationCourierStatusPM.CourierDeclarationStatusCode = "X";
                                    break;
                                case "11":
                                case "13":
                                    if (declarationPM.IsChanged == true)
                                    {
                                        myDeclarationCourierStatusPM.CourierDeclarationStatusCode = "R";
                                    }
                                    else
                                    {
                                        myDeclarationCourierStatusPM.CourierDeclarationStatusCode = "V";
                                    }
                                    break;
                                default:
                                    myDeclarationCourierStatusPM.CourierDeclarationStatusCode = "";
                                    break;
                            }
                            LogMessagingUtil.Instance.AppendLine("DeclarationStatusTypeCode is " + declarationPM.DeclarationStatusTypeCode +
                                ",CourierDeclarationStatusCode set to " + myDeclarationCourierStatusPM.CourierDeclarationStatusCode);
                        }
                        else
                        {
                            if (declarationPM.IsChanged == false && myDeclarationCourierStatusPM.CourierDeclarationStatusCode == "V")
                            {

                            }
                            else
                            {
                                LogMessagingUtil.Instance.AppendLine("DeclarationStatusTypeCode is " + declarationPM.DeclarationStatusTypeCode +
                           ",CourierDeclarationStatusCode set to " + myDeclarationCourierStatusPM.CourierDeclarationStatusCode);
                                myDeclarationCourierStatusPM.CourierDeclarationStatusCode = "R";
                            }
                        }
                    }
                }
            }
        }

        public Boolean IsDocumentMissing(DeclarationCourierStatusPM myDeclarationCourierStatusPM, List<CustomsDocumentsTicketPM> customsDocumentsTicketPMList)
        {
            var customContext = CustomContext.GetContext(myDeclarationCourierStatusPM.Tenant);
            //CustomsDocumentsTicketQueryService myCustomsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(customContext);
            CustomDocumentTypeQueryService docTypeQuery = new CustomDocumentTypeQueryService(customContext);

            //List<CustomsDocumentsTicketPM> customsDocumentsTicketPMList = myCustomsDocumentsTicketQueryService.GetCustomsDocumentsTicketPMsByEntityIdAndChilds(declarationPM.Id, "", "", "", myDeclarationCourierStatusPM.Tenant, "Declaration");

            List<CustomDocumentTypePM> CustomDocumentTypePMList = docTypeQuery.GetMandatoryCustomDocumentTypesForCourier(declarationPM.Tenant);
            if (CustomDocumentTypePMList != null)
            {
                foreach (CustomDocumentTypePM customDocumentTypePMItem in CustomDocumentTypePMList)
                {
                    LogMessagingUtil.Instance.AppendLine("customDocumentTypePMItem?.code" + customDocumentTypePMItem?.Code);

                    CustomsDocumentsTicketPM customsDocumentsTicketPM = customsDocumentsTicketPMList.Where(d => d.DocumentTypeCode == customDocumentTypePMItem.Code && d.DocumentsFilingId != null).FirstOrDefault();
                    LogMessagingUtil.Instance.AppendLine("customsDocumentsTicketPM?.DocumentsFilingId" + customsDocumentsTicketPM?.DocumentsFilingId);
                    LogMessagingUtil.Instance.AppendLine("customsDocumentsTicketPM?.DocumentStatusCode" + customsDocumentsTicketPM?.DocumentStatusCode);
                    LogMessagingUtil.Instance.AppendLine("customsDocumentsTicketPM?.DocumentTypeCode" + customsDocumentsTicketPM?.DocumentTypeCode);

                    if (customsDocumentsTicketPM == null)
                    {
                        LogMessagingUtil.Instance.AppendLine("customsDocumentsTicketPM == null");

                        return true;
                    }
                    if (string.IsNullOrWhiteSpace(customsDocumentsTicketPM.CustomsDocId))// is missing or  not sent yet  !!
                    {

                        LogMessagingUtil.Instance.AppendLine("string.IsNullOrWhiteSpace(customsDocumentsTicketPM.CustomsDocId)");

                        return true;
                    }
                }
            }
            LogMessagingUtil.Instance.AppendLine(" return false;");
            return false;
        }

        public Boolean IsDocumentError(DeclarationCourierStatusPM myDeclarationCourierStatusPM, List<CustomsDocumentsTicketPM> customsDocumentsTicketPMList)
        {
            //var customContext = CustomContext.GetContext(myDeclarationCourierStatusPM.Tenant);
            //CustomsDocumentsTicketQueryService myCustomsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(customContext);
            //CustomDocumentTypeQueryService docTypeQuery = new CustomDocumentTypeQueryService(customContext);

            //List<CustomsDocumentsTicketPM> customsDocumentsTicketPMList = myCustomsDocumentsTicketQueryService.GetCustomsDocumentsTicketPMsByEntityIdAndChilds(declarationPM.Id, "", "", "", myDeclarationCourierStatusPM.Tenant, "Declaration");

            //List<CustomDocumentTypePM> CustomDocumentTypePMList = docTypeQuery.GetMandatoryCustomDocumentTypesForCourier(declarationPM.Tenant);
            //if (CustomDocumentTypePMList != null)
            //{
            //    foreach (CustomDocumentTypePM customDocumentTypePMItem in CustomDocumentTypePMList) d.DocumentTypeCode == customDocumentTypePMItem.Code &&
            //    {
            CustomsDocumentsTicketPM customsDocumentsTicketPM = customsDocumentsTicketPMList.Where(d => d.DocumentsFilingId != null && d.DocumentStatusCode == "2").FirstOrDefault();
            if (customsDocumentsTicketPM != null)
            {
                return true;
            }
            //    }
            //}

            return false;
        }

        public void CalcTotalInvoiceAmountInUSD(DeclarationCourierStatusPM myDeclarationCourierStatusPM)
        {
            if (declarationPM == null || myDeclarationCourierStatusPM == null) return;
            //Set TotalInvoiceAmountInUSD - sum field InvoiceAmountInUSD from all SupplierInvoices

            if (declarationPM.SupplierInvoices != null && declarationPM.SupplierInvoices.Count() > 0)
            {
                myDeclarationCourierStatusPM.TotalInvoiceAmountInUSD = 0;
                myDeclarationCourierStatusPM.TotalInvoiceAmountInUSD = declarationPM.SupplierInvoices.Sum(r => r.InvoiceAmountInUSD);
            }
        }

        public void CalcCourierManifestStatusCode(DeclarationCourierStatusPM myDeclarationCourierStatusPM)
        {
            LogMessagingUtil.Instance.AppendLine("CalcCourierManifestStatusCode()");
            if (declarationPM == null || myDeclarationCourierStatusPM == null) return;
            //Set CourierManifestStatusCode according to Manifest Message Required fields
            CustomsRequiredFieldErrors errorsForCourierDeclaration = CustomsRequiredFieldsValidator.GetRequiredFieldErrorsForCourierDeclaration(declarationPM.Id, declarationPM.Tenant, declarationPM);
            if (errorsForCourierDeclaration != null && errorsForCourierDeclaration.RequiredFields != null && errorsForCourierDeclaration.RequiredFields.Count() > 0)
            {
                myDeclarationCourierStatusPM.CourierManifestStatusCode = "M";
            }
            else
            {
                if (string.IsNullOrWhiteSpace(declarationPM.ManifestCargoStatusCode))
                {
                    myDeclarationCourierStatusPM.CourierManifestStatusCode = "R";
                }
                else
                {
                    switch (declarationPM.ManifestCargoStatusCode)
                    {
                        case "1":
                            myDeclarationCourierStatusPM.CourierManifestStatusCode = "V";
                            break;
                        case "2":
                        case "3":
                            myDeclarationCourierStatusPM.CourierManifestStatusCode = "X";
                            break;


                        case "4":// sana + eitan (+itzik !!) ----> R - While Send Failed !!-- Ready 2 send (again)
                            myDeclarationCourierStatusPM.CourierManifestStatusCode = "R";
                            break;
                        default:
                            myDeclarationCourierStatusPM.CourierManifestStatusCode = "";
                            break;
                    }
                }
            }
        }

        public void CalcFastIndividualProcess(DeclarationCourierStatusPM myDeclarationCourierStatusPM)
        {
            if (myDeclarationCourierStatusPM == null) return;

            //Set CalcFastIndividualProcess
            if (!string.IsNullOrEmpty(myDeclarationCourierStatusPM.ManualProcessCode))
            {
                myDeclarationCourierStatusPM.FastIndividualProcessCode = myDeclarationCourierStatusPM.ManualProcessCode;
            }
            else
            {
                if (myDeclarationCourierStatusPM.HighLowValue == "L")
                {
                    myDeclarationCourierStatusPM.FastIndividualProcessCode = "F";
                }
                else
                {
                    myDeclarationCourierStatusPM.FastIndividualProcessCode = "I";
                }
            }

        }

        public void CalcDeclarationPendings902(DeclarationCourierStatusPM myDeclarationCourierStatusPM)
        {
            if (declarationPM == null || myDeclarationCourierStatusPM == null) return;

            DeclarationPendingPM declarationPendingPM_902 = null;
            if (myDeclarationCourierStatusPM.DeclarationPendings != null && myDeclarationCourierStatusPM.DeclarationPendings.Count() > 0)
            {
                declarationPendingPM_902 = myDeclarationCourierStatusPM.DeclarationPendings.Where(r => r.CourierPendingReasonCode == "902").FirstOrDefault();
            }

     

                if (myDeclarationCourierStatusPM.TotalInvoiceAmountInUSD > 150 && string.IsNullOrEmpty(declarationPM.ImporterId) && string.IsNullOrEmpty(declarationPM.ImporterCode))
            {
                // Set Pending 902- Missing ID
                // LogMessagingUtil.Instance.AppendLine("Set Courier Pending Reason Code 900");
                CourierPendingReasonRepository courierPendingReasonRepositoryRepository = new CourierPendingReasonRepository(declarationPM.Tenant);
                Boolean isActive = courierPendingReasonRepositoryRepository.IsActive("902", myDeclarationCourierStatusPM.Tenant);
                if (isActive)
                {
                    if (declarationPendingPM_902 == null)
                {

                        declarationPendingPM_902 = new DeclarationPendingPM();
                        declarationPendingPM_902.CourierPendingReasonCode = "902";
                        declarationPendingPM_902.Status = "A";
                        declarationPendingPM_902.ChangeSetOp = ChangeSetOperation.Insert;
                        myDeclarationCourierStatusPM.DeclarationPendings.Add(declarationPendingPM_902);
                    }

                else if (declarationPendingPM_902.Status != "A")
                {
                    declarationPendingPM_902.ChangeSetOp = ChangeSetOperation.Update;
                    declarationPendingPM_902.Status = "A";
                }
                    }
            }
            else if (declarationPendingPM_902 != null)
            {
                declarationPendingPM_902.ChangeSetOp = ChangeSetOperation.Update;
                declarationPendingPM_902.Status = "S";
                //LogMessagingUtil.Instance.AppendLine("Courier Pending Reason Code 900 Set as Solved");
            }
        }
        public string DeleteSpaces(string inputString)
        {
            return Regex.Replace(inputString, @"\s+", "");
        }
        public void CalcDeclarationPendings908(DeclarationCourierStatusPM myDeclarationCourierStatusPM)
        {
            if (declarationPM == null || myDeclarationCourierStatusPM == null) return;
            string clientFullName=null;
            if (!string.IsNullOrEmpty(declarationPM.ImporterId)){
                ClientQueryService clientQueryService = new ClientQueryService(myDeclarationCourierStatusPM.Tenant);
                var clientId = clientQueryService.GetSingle(declarationPM.ImporterId,false,false);
                if(clientId != null && !string.IsNullOrWhiteSpace(clientId.FullName))
                {
                    clientFullName = DeleteSpaces(clientId.FullName);
                }
            }
            if (myDeclarationCourierStatusPM.TotalInvoiceAmountInUSD >= 1000 && (string.IsNullOrEmpty(declarationPM.ImporterId) || (!string.IsNullOrEmpty(declarationPM.ImporterId) && clientFullName.Contains("ישלשלוףלקוח"))))
            {
                DeclarationPendingPM declarationPendingPM_908 = null;
                if (myDeclarationCourierStatusPM.DeclarationPendings != null && myDeclarationCourierStatusPM.DeclarationPendings.Count() > 0)
                {
                    declarationPendingPM_908 = myDeclarationCourierStatusPM.DeclarationPendings.Where(r => r.CourierPendingReasonCode == "908").FirstOrDefault();
                }
                CourierPendingReasonRepository courierPendingReasonRepositoryRepository = new CourierPendingReasonRepository(declarationPM.Tenant);
                Boolean isActive = courierPendingReasonRepositoryRepository.IsActive("908", myDeclarationCourierStatusPM.Tenant);
                if (isActive)
                {
                    if (declarationPendingPM_908 == null)
                    {

                        declarationPendingPM_908 = new DeclarationPendingPM();
                        declarationPendingPM_908.CourierPendingReasonCode = "908";
                        declarationPendingPM_908.Status = "A";
                        declarationPendingPM_908.ChangeSetOp = ChangeSetOperation.Insert;
                        myDeclarationCourierStatusPM.DeclarationPendings.Add(declarationPendingPM_908);
                        if (myDeclarationCourierStatusPM.ChangeSetOp == ChangeSetOperation.None)
                        {
                            myDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                        }
                    }

                    else if (declarationPendingPM_908.Status != "A")
                    {
                        declarationPendingPM_908.ChangeSetOp = ChangeSetOperation.Update;
                        declarationPendingPM_908.Status = "A";
                        if (myDeclarationCourierStatusPM.ChangeSetOp == ChangeSetOperation.None)
                        {
                            myDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                        }
                    }
                }


            }
        }

        public void CalcDeclarationPendings906(DeclarationCourierStatusPM myDeclarationCourierStatusPM)
        {
            if (declarationPM == null || myDeclarationCourierStatusPM == null) return;
            DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(myDeclarationCourierStatusPM.Tenant);

            string defValue = defaultValueQueryService.GetDefault("ISRAEL", "CGO_CUST_CAS", "NON", "NON", myDeclarationCourierStatusPM.Tenant);
            if (!string.IsNullOrEmpty(defValue) && !string.IsNullOrEmpty(declarationPM.CustomerCode) && declarationPM.CustomerCode != defValue)
            {
                DeclarationPendingPM declarationPendingPM_906 = null;
                if (myDeclarationCourierStatusPM.DeclarationPendings != null && myDeclarationCourierStatusPM.DeclarationPendings.Count() > 0)
                {
                    declarationPendingPM_906 = myDeclarationCourierStatusPM.DeclarationPendings.Where(r => r.CourierPendingReasonCode == "906").FirstOrDefault();
                    if(declarationPendingPM_906?.DeclarationID != null) NetCommonHelper.Logger.DevLog.Instance.WriteDebug("906 penidng found "+ "decId: " + declarationPendingPM_906?.DeclarationID + "STACK: " + Environment.StackTrace);
                }




                CourierPendingReasonRepository courierPendingReasonRepositoryRepository = new CourierPendingReasonRepository(declarationPM.Tenant);
                Boolean isActive = courierPendingReasonRepositoryRepository.IsActive("906", myDeclarationCourierStatusPM.Tenant);
                if (declarationPendingPM_906?.DeclarationID != null) NetCommonHelper.Logger.DevLog.Instance.WriteDebug("906 penidng isActive " + isActive + " decId: " + declarationPendingPM_906?.DeclarationID + "STACK: " + Environment.StackTrace);

                if (isActive)
                {
                    if (declarationPendingPM_906 == null)
                    {

                        declarationPendingPM_906 = new DeclarationPendingPM();
                        declarationPendingPM_906.CourierPendingReasonCode = "906";
                        declarationPendingPM_906.Status = "A";
                        declarationPendingPM_906.ChangeSetOp = ChangeSetOperation.Insert;
                        myDeclarationCourierStatusPM.DeclarationPendings.Add(declarationPendingPM_906);
                        if (myDeclarationCourierStatusPM.ChangeSetOp == ChangeSetOperation.None)
                        {
                            myDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                            NetCommonHelper.Logger.DevLog.Instance.WriteDebug("906 pending update. STACK: " + Environment.StackTrace);

                        }
                    }

                    else if (declarationPendingPM_906.Status != "A")
                    {
                        declarationPendingPM_906.ChangeSetOp = ChangeSetOperation.Update;
                        declarationPendingPM_906.Status = "A";
                        if (myDeclarationCourierStatusPM.ChangeSetOp == ChangeSetOperation.None)
                        {
                            myDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                        }
                    }
                }


            }
        }
 
    }


    public class UpdateDeclarationPending903InvalidPhoneNumberService
    {
        private DeclarationPM declarationPM;

        public UpdateDeclarationPending903InvalidPhoneNumberService(DeclarationPM declarationPM)
        {
            this.declarationPM = declarationPM;
        }
        public void Calc(DeclarationCourierStatusPM myDeclarationCourierStatusPM)
        {

            LogMessagingUtil.Instance.AppendLine("Calc() 903");

            if (declarationPM == null || myDeclarationCourierStatusPM == null) return;
            string courierReasonCode = "903";
            if (myDeclarationCourierStatusPM == null)
            {
                return;//not courier 
            }
            var declarationPending903PM = myDeclarationCourierStatusPM.DeclarationPendings.Where(r => r.DeclarationID == declarationPM.Id && r.CourierPendingReasonCode == courierReasonCode).FirstOrDefault();
            LogMessagingUtil.Instance.AppendLine("declarationPending903PM?"+ declarationPending903PM==null?"yes":"no");
            bool valid = false;
            if (string.IsNullOrWhiteSpace(declarationPM.CasualImporterTel))
            {
                valid = true;
            }
            else if (declarationPM.CasualImporterTel.StartsWith("9725") && declarationPM.CasualImporterTel.Length == 12)
            {
                valid = true;
            }
            else if (declarationPM.CasualImporterTel.StartsWith("05") && declarationPM.CasualImporterTel.Length == 10)
            {
                valid = true;
            }
            LogMessagingUtil.Instance.AppendLine("valid 903:" + valid);

            if (valid)
            {
                if (declarationPending903PM != null && declarationPending903PM.Status == "A")
                {
                    LogMessagingUtil.Instance.AppendLine("declarationPending903PM.Status == a 903");

                    //UPDATE to solve
                    declarationPending903PM.Status = "S";
                    declarationPending903PM.ChangeSetOp = ChangeSetOperation.Update;
                    if (myDeclarationCourierStatusPM.ChangeSetOp == ChangeSetOperation.None)
                    {
                        LogMessagingUtil.Instance.AppendLine("myDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update 903");

                        myDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                    }

                }

            }
            else//invalid 
            {
                CourierPendingReasonRepository courierPendingReasonRepositoryRepository = new CourierPendingReasonRepository(declarationPM.Tenant);
                Boolean isActive = courierPendingReasonRepositoryRepository.IsActive(courierReasonCode, myDeclarationCourierStatusPM.Tenant);
                LogMessagingUtil.Instance.AppendLine("isActive 903"+ isActive);

                if (isActive)
                {
                    if (declarationPending903PM == null)
                { 
                LogMessagingUtil.Instance.AppendLine("declarationPending903PM == null)");

                        declarationPending903PM = new DeclarationPendingPM()
                        {
                            ChangeSetOp = ChangeSetOperation.Insert,
                            DeclarationID = declarationPM.Id,
                            Tenant = declarationPM.Tenant,
                            CourierPendingReasonCode = courierReasonCode,
                            Status = "A",
                        };
                        LogMessagingUtil.Instance.AppendLine(" myDeclarationCourierStatusPM.DeclarationPendings.Add(declarationPending903PM);");

                        myDeclarationCourierStatusPM.DeclarationPendings.Add(declarationPending903PM);
                        if (myDeclarationCourierStatusPM.ChangeSetOp == ChangeSetOperation.None)
                        {
                            myDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                        }
                        LogMessagingUtil.Instance.AppendLine("myDeclarationCourierStatusPM.ChangeSetO" + myDeclarationCourierStatusPM.ChangeSetOp);
                    }


                 
                else
                    {
                        LogMessagingUtil.Instance.AppendLine("isActive! 903");

                        if (declarationPending903PM.Status == "S")
                    {
                            LogMessagingUtil.Instance.AppendLine("declarationPending903PM.Status == s");

                            declarationPending903PM.Status = "A";
                        declarationPending903PM.ChangeSetOp = ChangeSetOperation.Update;
                        if (myDeclarationCourierStatusPM.ChangeSetOp == ChangeSetOperation.None)
                        {
                            myDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                        }


                    }
                }
}
                
            }

        }
    }


    public class UpdateDeclarationPending904ExceededGrossMassMeasureService
    {
        private DeclarationPM declarationPM;

        public UpdateDeclarationPending904ExceededGrossMassMeasureService(DeclarationPM declarationPM)
        {
            this.declarationPM = declarationPM;
        }
        public void Calc(DeclarationCourierStatusPM myDeclarationCourierStatusPM)
        {
            if (declarationPM == null || myDeclarationCourierStatusPM == null) return;
            string courierReasonCode = "904";
            if (myDeclarationCourierStatusPM == null)
            {
                return;//not courier 
            }
            var declarationPending904PM = myDeclarationCourierStatusPM.DeclarationPendings.Where(r => r.DeclarationID == declarationPM.Id && r.CourierPendingReasonCode == courierReasonCode).FirstOrDefault();
            DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(myDeclarationCourierStatusPM.Tenant);

            string defValue = defaultValueQueryService.GetDefault("ISRAEL", "CGO_PENDING_WGT", "NON", "NON", myDeclarationCourierStatusPM.Tenant);
            decimal defaultAmount = 0;
            var boolvar = (decimal.TryParse(defValue, out defaultAmount));

            if (!boolvar)
            {
                return;
            }

            ConsignmentPackageRepository consignmentPackageRepository = new ConsignmentPackageRepository(declarationPM.Tenant);
            List<ConsignmentPackage> listConPackages = consignmentPackageRepository.GetConsignmentPackagesFilterByMeasureQualifierCode(declarationPM.Id);

            var sumGross = listConPackages.Sum(c => c.GrossMassMeasure);


            if (sumGross < defaultAmount)
            {
                if (declarationPending904PM != null && declarationPending904PM.Status == "A")
                {
                    //UPDATE to solve
                    declarationPending904PM.Status = "S";
                    declarationPending904PM.ChangeSetOp = ChangeSetOperation.Update;
                    if (myDeclarationCourierStatusPM.ChangeSetOp == ChangeSetOperation.None)
                    {
                        myDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                    }

                }

            }
            else//invalid 
            {
                CourierPendingReasonRepository courierPendingReasonRepositoryRepository = new CourierPendingReasonRepository(declarationPM.Tenant);
                Boolean isActive = courierPendingReasonRepositoryRepository.IsActive(courierReasonCode, myDeclarationCourierStatusPM.Tenant);
                if (isActive)
                {
                    if (declarationPending904PM == null)
                    {

                        declarationPending904PM = new DeclarationPendingPM()
                        {
                            ChangeSetOp = ChangeSetOperation.Insert,
                            DeclarationID = declarationPM.Id,
                            Tenant = declarationPM.Tenant,
                            CourierPendingReasonCode = courierReasonCode,
                            Status = "A",
                        };

                        myDeclarationCourierStatusPM.DeclarationPendings.Add(declarationPending904PM);
                        if (myDeclarationCourierStatusPM.ChangeSetOp == ChangeSetOperation.None)
                        {
                            myDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                        }
                    }



                    else
                    {
                        if (declarationPending904PM.Status == "S")
                        {
                            declarationPending904PM.Status = "A";
                            declarationPending904PM.ChangeSetOp = ChangeSetOperation.Update;
                            if (myDeclarationCourierStatusPM.ChangeSetOp == ChangeSetOperation.None)
                            {
                                myDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                            }


                        }
                    }
                }

            }

        }
        
    }
    public class UpdateDeclarationPending907ExceedingTheQuantityOfGoodsService
    {
        private DeclarationPM declarationPM;

        public UpdateDeclarationPending907ExceedingTheQuantityOfGoodsService(DeclarationPM declarationPM)
        {
            this.declarationPM = declarationPM;
        }
        public void Calc(DeclarationCourierStatusPM myDeclarationCourierStatusPM)
        {
            if (declarationPM == null || myDeclarationCourierStatusPM == null) return;
            string courierReasonCode = "907";
            if (myDeclarationCourierStatusPM == null)
            {
                return;//not courier 
            }
            DeclarationPendingPM declarationPendingPM_907 = null;
            if (myDeclarationCourierStatusPM.DeclarationPendings != null && myDeclarationCourierStatusPM.DeclarationPendings.Count() > 0)
            {
                declarationPendingPM_907 = myDeclarationCourierStatusPM.DeclarationPendings.Where(r => r.DeclarationID == declarationPM.Id && r.CourierPendingReasonCode == courierReasonCode).FirstOrDefault();
            }

            DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(declarationPM.Tenant);
            string defValue = defaultValueQueryService.GetDefault("ISRAEL", "CGO_PND_QTY_VAL", "NON", "NON", declarationPM.Tenant);
            decimal defaultAmount = 0;
            var boolvar = (decimal.TryParse(defValue, out defaultAmount));

            if (!boolvar)
            {
                return;
            }

            SupplierInvoiceItemRepository supplierInvoiceItemRepository = new SupplierInvoiceItemRepository(declarationPM.Tenant);
            List<SupplierInvoiceItem> listConPackages = supplierInvoiceItemRepository.GetPreferenceDocumentNumberSupplierInvoiceItemByDeclarationId(declarationPM.Id, declarationPM.Tenant);

            var sumInvoiceQuantity = listConPackages.Sum(c => c.InvoiceQuantity);

            if (sumInvoiceQuantity >= defaultAmount)
            {




                CourierPendingReasonRepository courierPendingReasonRepositoryRepository = new CourierPendingReasonRepository(declarationPM.Tenant);
                Boolean isActive = courierPendingReasonRepositoryRepository.IsActive("907", myDeclarationCourierStatusPM.Tenant);
                if (isActive)
                {
                    if (declarationPendingPM_907 == null)
                    {

                        declarationPendingPM_907 = new DeclarationPendingPM();
                        declarationPendingPM_907.CourierPendingReasonCode = "907";
                        declarationPendingPM_907.Status = "A";
                        declarationPendingPM_907.ChangeSetOp = ChangeSetOperation.Insert;
                        myDeclarationCourierStatusPM.DeclarationPendings.Add(declarationPendingPM_907);
                        if (myDeclarationCourierStatusPM.ChangeSetOp == ChangeSetOperation.None)
                        {
                            myDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                        }
                    }

                    else if (declarationPendingPM_907.Status != "A")
                    {
                        declarationPendingPM_907.ChangeSetOp = ChangeSetOperation.Update;
                        declarationPendingPM_907.Status = "A";
                        if (myDeclarationCourierStatusPM.ChangeSetOp == ChangeSetOperation.None)
                        {
                            myDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                        }
                    }
                }

            }



        }

        
    }
}
