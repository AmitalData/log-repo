using System.ComponentModel.DataAnnotations;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using System.Collections.Generic;

namespace Logitude.BL.Validators
{
    public class DocumentTypeClassLevelValidator
    {
        public static ValidationResult ValidateClass(DocumentTypePM documentType, ValidationContext context)
        {

            if (!string.IsNullOrEmpty(documentType.FileName))
            {
                string s = ('"').ToString();
                /// : * ? " < > 
                //char[] specialCharacterLists = { "", '1', '2' };
                List<string> speCharLists = new List<string>();
                speCharLists.Add("|");
                speCharLists.Add("/");
                speCharLists.Add("\\");
                speCharLists.Add(":");
                speCharLists.Add("*");
                speCharLists.Add("?");
                speCharLists.Add("<");
                speCharLists.Add(">");
                speCharLists.Add(('"').ToString());

                foreach (string specialCharacter in speCharLists)
                {
                   if(documentType.FileName.Contains(specialCharacter))
                    {
                        return new ValidationResult("FileName field can't contain any of the following characters:" + speCharLists[0] + speCharLists[1] + speCharLists[2] + speCharLists[3] + speCharLists[4] + speCharLists[5] + speCharLists[6] + speCharLists[7] + speCharLists[8] );
                    }
                }

                if (documentType.FileName.Contains(" "))
                {
                    return new ValidationResult("FileName field can't contain spaces");
                }



            }

       

            //\ /  *  " 

            if (documentType.IsDocOut)
            {
                if (documentType.TemplateFormatCode == null)
                {
                    return new ValidationResult(TextCodesTranslator.TranslateText("DocumentType.M.ChooseDocumentTypeFormat", documentType.Tenant));

                }
            }
            else
            {
                if (!documentType.IsDocOut && !documentType.IsDocIn)
                {
                    return new ValidationResult(TextCodesTranslator.TranslateText("DocumentType.M.EntityLevelvalidation", documentType.Tenant));
                }
            }

            if(!string.IsNullOrEmpty(documentType.ObjectTableName))
            {
                if (documentType.ObjectTableName != "Customs.Declaration" 
                    && documentType.ObjectTableName != "Shipment" 
                    && documentType.ObjectTableName != "Quote"
                    && documentType.ObjectTableName != "Master"
                    && documentType.ObjectTableName != "Customer" 
                    && documentType.ObjectTableName != "Opportunity" 
                    && documentType.ObjectTableName != "ARPayment" 
                    && documentType.ObjectTableName != "APPayment" 
                    && documentType.ObjectTableName != "ARInvoice" 
                    && documentType.ObjectTableName != "APInvoice" 
                    && documentType.ObjectTableName != "ShipmentPickUpDelivery" 
                    && documentType.ObjectTableName != "Ticket"
                    && documentType.ObjectTableName != "SharedLogistics"
                    && documentType.ObjectTableName != "LogitudeMessagesTransmissionLog"
                    && documentType.ObjectTableName != "Customs.CheckRepresentativeType"
                    && documentType.ObjectTableName != "Customs.Claim"
                    && documentType.ObjectTableName != "Journal"
                    && documentType.ObjectTableName != "BankDeposit"
                    && documentType.ObjectTableName != "Agent"
                    && documentType.ObjectTableName != "WarehouseEntry"
                    && documentType.ObjectTableName != "PaymentCheque"
                    && documentType.ObjectTableName != "TaxReport"
                    && documentType.ObjectTableName != "TaxDeductionReport"
                    && documentType.ObjectTableName != "WarehouseRelease"
                    && documentType.ObjectTableName != "Airline"
                    && documentType.ObjectTableName != "CustomAgent"
                    && documentType.ObjectTableName != "Participant"
                    && documentType.ObjectTableName != "ShippingAgent"
                    && documentType.ObjectTableName != "ShippingLine"
                    && documentType.ObjectTableName != "Trucker"
                    && documentType.ObjectTableName != "Vendor"
                    && documentType.ObjectTableName != "AccountingPartner"
                    && documentType.ObjectTableName != "Warehouse"
                    && documentType.ObjectTableName != "OpenFormatReport"
                    && documentType.ObjectTableName != "Occasion"
                    && documentType.ObjectTableName != "InterestReport")

                {

                    return new ValidationResult(TextCodesTranslator.TranslateText("DocumentType.M.TableNameDoesNotExist", documentType.Tenant));
               }
             }

            return null;
        }
    }
}
