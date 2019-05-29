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

namespace Logitude.Customs.BL.BL
{
    public class CalculateDeclarationCourierStatus
    {
        private DeclarationPM declarationPM;
        public static void UpdateCourierDeclarationStatusCode(int Tenant, string DeclarationId)
        {
            var customContext = CustomContext.GetContext(Tenant);
            DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(customContext);
            DeclarationCourierStatusPM currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(DeclarationId, false, false);
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
            DeclarationCourierStatusPM currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(DeclarationId, false, false);
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
            DeclarationCourierStatusPM currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(this.declarationPM.Id, false, false);
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
                DeclarationCourierStatusPM myDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(declarationPM.Id, false, false);
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
                CalcCourierDeclarationStatusCode(myDeclarationCourierStatusPM);
                CalcCourierPaymentStatusCode(myDeclarationCourierStatusPM);
                CalcIsCourierMissingClassification(myDeclarationCourierStatusPM);
                CalcHighLowValue(myDeclarationCourierStatusPM);
                CalcDocumentStatusCode(myDeclarationCourierStatusPM);
                CalcSpecialActionStatus(myDeclarationCourierStatusPM);
                CalcFastIndividualProcess(myDeclarationCourierStatusPM);

                return myDeclarationCourierStatusPM;

            }
            return null;
        }

        public void CalcDocumentStatusCode(DeclarationCourierStatusPM myDeclarationCourierStatusPM)
        {
            if (myDeclarationCourierStatusPM == null) return;
            //Set DocumentStatusCode
            if (string.IsNullOrWhiteSpace(myDeclarationCourierStatusPM.DocumentStatusCode)) myDeclarationCourierStatusPM.DocumentStatusCode = "M";
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

        public void CalcTotalInvoiceAmountInUSD(DeclarationCourierStatusPM myDeclarationCourierStatusPM)
        {
            if (declarationPM == null || myDeclarationCourierStatusPM == null) return;
            //Set TotalInvoiceAmountInUSD - sum field InvoiceAmountInUSD from all SupplierInvoices
            myDeclarationCourierStatusPM.TotalInvoiceAmountInUSD = 0;
            if (declarationPM.SupplierInvoices != null && declarationPM.SupplierInvoices.Count() > 0)
            {
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
    }
}
