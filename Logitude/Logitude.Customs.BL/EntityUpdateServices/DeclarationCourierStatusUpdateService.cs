using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using Logitude.Customs.Data.EntityKeys;
using System.Configuration;
using System.Globalization;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.BL.Validators;
using Logitude.Customs.BL.EntityQueryServices;
using Unifreight.BL.EntityQueryServices;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.Data.AmitalModel;
using Logitude.Customs.BL.BL;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class DeclarationCourierStatusUpdateService //: EntityUpdateService<DeclarationCourierStatus, DeclarationCourierStatusPM, DeclarationPM>
    {
        protected override void OnCreating(DeclarationCourierStatusPM entityPM, EntityPM entityParentPM)
        {
            //if (entityParentPM != null)
            //{
            //    entityPM.DeclarationId = entityParentPM.Id;
            //    entityPM.Tenant = entityParentPM.Tenant;
            //}
        }

        protected override void OnUpdating(DeclarationCourierStatusPM entityPM, DeclarationCourierStatus entityPOCO)
        {
            var declarationCourierStatusRepository = new DeclarationCourierStatusRepository(entityPM.Tenant);
            declarationCourierStatusRepository.Lock_forUpdateNOWAIT(entityPM.DeclarationId);


            UpdateUnifreight(entityPM);

            base.OnUpdating(entityPM, entityPOCO);
        }


        protected override void UpdateComposition(DeclarationCourierStatusPM entityPM)
        {
            base.UpdateComposition(entityPM);
        }

        protected override void AfterUpdating(DeclarationCourierStatusPM entityPM, EntityPM entityParentPM)
        {
            LogMessagingUtil.Instance.AppendLine("DeclarationCourierStatusPM.DocumentStatusCode: " + entityPM.DocumentStatusCode);
        }

        public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationKeys entityKeyFields)
        {
            //(Repository as Logitude.Customs.Data.Repsitories.DeclarationCourierStatusRepository).FastDeleteMulti(entityKeyFields);
        }

        public DeclarationCourierStatusPM CalculateDeclarationCourierStatus(DeclarationPM declarationPM, bool isRequiredFieldHasChanged = false)
        {
            CalculateDeclarationCourierStatus calculateDeclarationCourierStatus = new CalculateDeclarationCourierStatus(declarationPM);
            return calculateDeclarationCourierStatus.CalcAll();

            if (declarationPM.IsCourierDeclaration)
            {
                //task 39471 added GetSingle here
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

                //Set TotalInvoiceAmountInUSD - sum field InvoiceAmountInUSD from all SupplierInvoices
                myDeclarationCourierStatusPM.TotalInvoiceAmountInUSD = 0;
                if (declarationPM.SupplierInvoices != null && declarationPM.SupplierInvoices.Count() > 0)
                {
                    myDeclarationCourierStatusPM.TotalInvoiceAmountInUSD = declarationPM.SupplierInvoices.Sum(r => r.InvoiceAmountInUSD);
                }

                //Set CourierDeclarationStatusCode according to Declaration Message Required fields
                CustomsRequiredFieldErrors errorsForDeclaration = CustomsRequiredFieldsValidator.GetRequiredFieldErrorsForDeclaration(declarationPM.Id, declarationPM.Tenant, declarationPM);
                if (errorsForDeclaration != null && errorsForDeclaration.RequiredFields != null && errorsForDeclaration.RequiredFields.Count() > 0)
                {
                    myDeclarationCourierStatusPM.CourierDeclarationStatusCode = "M";
                }
                else if(myDeclarationCourierStatusPM.TotalInvoiceAmountInUSD > 100 && string.IsNullOrEmpty(declarationPM.ImporterId) && string.IsNullOrEmpty(declarationPM.ImporterCode))
                {
                    myDeclarationCourierStatusPM.CourierDeclarationStatusCode = "M";
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(declarationPM.DeclarationStatusTypeCode) || declarationPM.IsChanged==true && myDeclarationCourierStatusPM.CourierDeclarationStatusCode == "V")//task 39471
                    {
                        myDeclarationCourierStatusPM.CourierDeclarationStatusCode = "R";
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(declarationPM.PaymentDate.ToString()))//task 40950
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
                                    myDeclarationCourierStatusPM.CourierDeclarationStatusCode = "V";
                                    break;
                                default:
                                    myDeclarationCourierStatusPM.CourierDeclarationStatusCode = "";
                                    break;
                            }
                        }
                    }
                }

                //Set CourierPaymentStatusCode
                if (declarationPM.PaymentDate == null)
                {
                    if (declarationPM.DeclarationStatusTypeCode == "13" && myDeclarationCourierStatusPM.CourierDeclarationStatusCode=="V")//Task 39471
                    {
                        myDeclarationCourierStatusPM.CourierPaymentStatusCode = "R";
                    }
                    else
                    {
                        myDeclarationCourierStatusPM.CourierPaymentStatusCode = "";//Task 39471
                    }
                }
                else
                {
                    switch (declarationPM.PaymentStatusCode)
                    {
                        case "":
                        case null://task 40951
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

                //Set IsCourierMissingClassification
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
                        Where(SI => SI.SupplierInvoiceItems == null || SI.SupplierInvoiceItems.Count()==0).ToList();
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
                else//task 40894
                {
                    myDeclarationCourierStatusPM.IsCourierMissingClassification = true;
                }

                //Set HighLowValue
                string defValue = GetDefault("ISRAEL", "CGO_HIGH_VALUE", "NON", "NON");
                decimal defaultAmount = 0;
                var boolvar = (decimal.TryParse(defValue, out defaultAmount));
                
                if(myDeclarationCourierStatusPM.TotalInvoiceAmountInUSD > defaultAmount)
                {
                    myDeclarationCourierStatusPM.HighLowValue = "H";
                }
                else
                {
                    myDeclarationCourierStatusPM.HighLowValue = "L";
                }

                if (string.IsNullOrWhiteSpace(myDeclarationCourierStatusPM.DocumentStatusCode)) myDeclarationCourierStatusPM.DocumentStatusCode = "M";

                return myDeclarationCourierStatusPM;

            }
            return null;
        }

        private string GetDefault(string DISTRID, string DEFID, string BRANCHID, string CARDID)
        {
            AmitalContext amitalContext = AmitalContext.GetContext(Tenant);
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
    }
}
