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

namespace Logitude.Customs.BL.BL
{
    public class CalculateDeclarationCourierStatus
    {
        private DeclarationPM declarationPM;
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
            else if(!string.IsNullOrWhiteSpace(declarationId) && tenant > 0)
            {
                var context = CustomContext.GetContext(tenant);
                DeclarationQueryService declarationQueryService = new DeclarationQueryService(context);
                declarationQueryService.LoadSupplierInvoicesWithItems = false;
                this.declarationPM = declarationQueryService.GetSingle(declarationId, true, false);
            }
        }
        public void Update( Action<DeclarationCourierStatusPM> UPDATEDeclarationCourierStatusPM)
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
        public DeclarationCourierStatusPM CalcAll()
        {
            if (declarationPM == null) return null;
            if (declarationPM.IsCourierDeclaration)
            {
                var context = CustomContext.GetContext(declarationPM.Tenant);
                DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
                DeclarationCourierStatusPM myDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(declarationPM.Id, true, false);
                if (myDeclarationCourierStatusPM == null)
                {
                    myDeclarationCourierStatusPM = new DeclarationCourierStatusPM()
                    {
                        DeclarationId = declarationPM.Id,
                        Tenant = declarationPM.Tenant,
                        IsClosedForFollowUp = false,
                        IsCourierMissingClassification = false,
                        CourierDeclarationStatusCode = "",
                        ChangeSetOp = ChangeSetOperation.Insert,
                    };
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
                CalcDeclarationPendings(myDeclarationCourierStatusPM);

                return myDeclarationCourierStatusPM;

            }
            return null;
        }

        public void CalcDocumentStatusCode(DeclarationCourierStatusPM myDeclarationCourierStatusPM)
        {
            if (myDeclarationCourierStatusPM == null) return;
            //Set DocumentStatusCode
            if (string.IsNullOrWhiteSpace(myDeclarationCourierStatusPM.DocumentStatusCode))
            {
                myDeclarationCourierStatusPM.DocumentStatusCode = "M";
                return;
            }
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
                myDeclarationCourierStatusPM.DocumentStatusCode = "M";
            }

        }

        public void CalcSpecialActionStatus(DeclarationCourierStatusPM myDeclarationCourierStatusPM)
        {
            if (myDeclarationCourierStatusPM == null) return;

            //Set SpecialActionStatus
            DeclarationMamanSpecialActionRepository declarationMamanSpecialActionRepository = new DeclarationMamanSpecialActionRepository(myDeclarationCourierStatusPM.Tenant);
            List<DeclarationMamanSpecialAction> list = declarationMamanSpecialActionRepository.GetDeclarationMamanSpecialActionByDeclarationId(myDeclarationCourierStatusPM.DeclarationId, myDeclarationCourierStatusPM.Tenant);

            if(list != null && list.Count() > 0)
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
            if (myDeclarationCourierStatusPM == null) return;
            //Set HighLowValue
            string defValue = GetDefault("ISRAEL", "CGO_HIGH_VALUE", "NON", "NON");
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

        private string GetDefault(string DISTRID, string DEFID, string BRANCHID, string CARDID)
        {
            AmitalContext amitalContext = AmitalContext.GetContext(declarationPM.Tenant);
            var myGDFDATAQueryService = new GDFDATAQueryService(amitalContext);

            if (DISTRID == null || DEFID == null || BRANCHID == null || CARDID == null)
            {
                return ("");
            }

            GDFDATAPM myGDFDATAPM = myGDFDATAQueryService.GetSingle(DISTRID, DEFID, BRANCHID, CARDID, false, true);
            if (myGDFDATAPM == null)
            {
                return ("");
            }
            return (myGDFDATAPM.DEFDATA);
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
            if (declarationPM == null || myDeclarationCourierStatusPM == null) return;
            //Set CourierDeclarationStatusCode according to Declaration Message Required fields
            CustomsRequiredFieldErrors errorsForDeclaration = CustomsRequiredFieldsValidator.GetRequiredFieldErrorsForDeclaration(declarationPM.Id, declarationPM.Tenant, declarationPM);
            if (errorsForDeclaration != null && errorsForDeclaration.RequiredFields != null && errorsForDeclaration.RequiredFields.Count() > 0)
            {
                myDeclarationCourierStatusPM.CourierDeclarationStatusCode = "M";
            }
            else if (myDeclarationCourierStatusPM.TotalInvoiceAmountInUSD > 150 && string.IsNullOrEmpty(declarationPM.ImporterId) && string.IsNullOrEmpty(declarationPM.ImporterCode))
            {
                myDeclarationCourierStatusPM.CourierDeclarationStatusCode = "M";
            }
            else if (myDeclarationCourierStatusPM.DocumentStatusCode == "M")
            {
                myDeclarationCourierStatusPM.CourierDeclarationStatusCode = "M";
            }
            else
            {
                if (string.IsNullOrWhiteSpace(declarationPM.DeclarationStatusTypeCode) || declarationPM.IsChanged == true && myDeclarationCourierStatusPM.CourierDeclarationStatusCode == "V")
                {
                    myDeclarationCourierStatusPM.CourierDeclarationStatusCode = "R";
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(declarationPM.PaymentDate.ToString()))
                    {
                        myDeclarationCourierStatusPM.CourierDeclarationStatusCode = "V";
                    }
                    else
                    {
                        switch (declarationPM.DeclarationStatusTypeCode)
                        {
                            case "12":
                                myDeclarationCourierStatusPM.CourierDeclarationStatusCode = "X";
                                break;
                            case "11":
                            case "13":
                                if(declarationPM.IsChanged == true)
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
                    CustomsDocumentsTicketPM customsDocumentsTicketPM = customsDocumentsTicketPMList.Where(d => d.DocumentTypeCode == customDocumentTypePMItem.Code && d.DocumentsFilingId != null ).FirstOrDefault();
                    if (customsDocumentsTicketPM == null)
                    {
                        return true;
                    }
                }
            }

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
            CustomsDocumentsTicketPM customsDocumentsTicketPM = customsDocumentsTicketPMList.Where(d =>  d.DocumentsFilingId != null && d.DocumentStatusCode == "2").FirstOrDefault();
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
                if(myDeclarationCourierStatusPM.HighLowValue == "L")
                {
                    myDeclarationCourierStatusPM.FastIndividualProcessCode = "F";
                }
                else
                {
                    myDeclarationCourierStatusPM.FastIndividualProcessCode = "I";
                }
            }

        }

        public void CalcDeclarationPendings(DeclarationCourierStatusPM myDeclarationCourierStatusPM)
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
                if (declarationPendingPM_902 == null)
                {
                    CourierPendingReasonQueryService myCourierPendingReasonQueryService = new CourierPendingReasonQueryService(declarationPM.Tenant);
                    CourierPendingReasonPM courierPendingReasonPM = myCourierPendingReasonQueryService.GetSingleCourierPendingReasonByCode("902", declarationPM.Tenant);
                    if (courierPendingReasonPM != null && courierPendingReasonPM.Code == "902")
                    {
                        declarationPendingPM_902 = new DeclarationPendingPM();
                        declarationPendingPM_902.CourierPendingReasonCode = courierPendingReasonPM.Id;
                        declarationPendingPM_902.Status = "A";
                        declarationPendingPM_902.ChangeSetOp = ChangeSetOperation.Insert;
                        myDeclarationCourierStatusPM.DeclarationPendings.Add(declarationPendingPM_902);
                    }
                }
                else if (declarationPendingPM_902.Status != "A")
                {
                    declarationPendingPM_902.ChangeSetOp = ChangeSetOperation.Update;
                    declarationPendingPM_902.Status = "A";
                }
            }
            else if (declarationPendingPM_902 != null)
            {
                declarationPendingPM_902.ChangeSetOp = ChangeSetOperation.Update;
                declarationPendingPM_902.Status = "S";
                //LogMessagingUtil.Instance.AppendLine("Courier Pending Reason Code 900 Set as Solved");
            }
        }
    }
}
