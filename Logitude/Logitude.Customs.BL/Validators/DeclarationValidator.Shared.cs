//NEW CLASS         Yuval Chalup 03.11.2014 TASK-4238
using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;


namespace Logitude.Customs.BL.Validators
{

    public class DeclarationValidator //: IDeclarationValidator
    {
        private DeclarationPM _DeclarationPM;
        public List<String> ErrorCode { get; private set; }
        public bool ToUpdateWithPaymentDate { get; set; }

        public DeclarationValidator(DeclarationPM declarationPM)
        {
            _DeclarationPM = declarationPM;
            ErrorCode = new List<string>();

        }


        //Check if declaration was already paid
        public void PaymentDateCheck()
        {
            string errorMessage = "";
            //if (_DeclarationPM.PaymentDate.HasValue)
            if (_DeclarationPM != null)
            {
                if (_DeclarationPM.PaymentDate.HasValue && ToUpdateWithPaymentDate != true && _DeclarationPM.Direction != "E")
                {
                    errorMessage = "Customs.General.O.NoPaymentDate";
                    if (!string.IsNullOrWhiteSpace(errorMessage))
                    {
                        ErrorCode.Add(errorMessage);
                    }
                }
            }
        }

        //Check if Importer exists
        public void ImporterCheck()
        {
            if (_DeclarationPM != null)
            {
                if (string.IsNullOrWhiteSpace(_DeclarationPM.ImporterId))
                {
                    var errorMessage = "";
                    if (!string.IsNullOrWhiteSpace(_DeclarationPM.ImporterCode))
                    {
                        errorMessage = "Customs.General.O.ImporterCodeNoId";
                    }
                    else
                    {
                        errorMessage = "Customs.General.O.NoImporterId";
                    }

                    if (!string.IsNullOrWhiteSpace(errorMessage))
                    {
                        ErrorCode.Add(errorMessage);
                    }
                }
            }
        }
        public string ImportersCheck(string importerField, string importerCode, string importerId, string importerType,
            string importerName, string importerAddress, string importerPassportNumber, string importerPassCountryCode)
        {
            var errorMessage = "";
            if (_DeclarationPM != null)
            {
                switch (importerType)
                {
                    case "1":
                        if (string.IsNullOrWhiteSpace(importerId))
                        {
                            if (!string.IsNullOrWhiteSpace(importerCode))
                            {
                                //ALLREADY DONE INRAMALLA - (TASK 16825)
                                //search the code in Clients table 
                                //if (false)
                                //{
                                //    errorMessage = "יש לשלוף יבואן מהמכס לפני שליחה";
                                //}
                            }
                            else
                            {
                                //Check if ImporterName & ImporterAddrress has value
                                if (string.IsNullOrWhiteSpace(importerName) && string.IsNullOrWhiteSpace(importerAddress))
                                {
                                    errorMessage = "יש להזין נתוני יבואן " + importerField + " לפני שליחה";
                                }
                            }
                        }
                        break;
                    case "2":
                    case "3":
                        {

                            //Check That Both PassportCountry & PassportNumber has values
                            if (string.IsNullOrWhiteSpace(importerPassportNumber) || string.IsNullOrWhiteSpace(importerPassCountryCode))
                            {
                                errorMessage = "יש להזין נתוני יבואן " + importerField + " לפני שליחה";
                            }
                            break;
                        }
                    default:
                        {
                            if (string.IsNullOrWhiteSpace(importerId))
                            {
                                if (!string.IsNullOrWhiteSpace(importerCode))
                                {
                                    //errorMessage = "Customs.General.O.ImporterCodeNoId";
                                    errorMessage = "יש לשלוף לקוח מהמכס עבור יבואן " + importerField + " לפני שליחה";
                                }
                                else
                                {
                                    //errorMessage = "Customs.General.O.NoImporterId";
                                    errorMessage = "מספר יבואן " + importerField + " הוא שדה חובה";
                                }
                            }
                            break;

                        }
                }
            }
            return (errorMessage);
        }

        //Check constraints in progress
        public void ConstraintsInProgressCheck()
        {
            //Check if Declaration Paid and there waiting for constraint approval
            if (_DeclarationPM != null)
            {
                if (_DeclarationPM.DeclarationStatusTypeCode == "11")
                {
                    var errorMessage = "Customs.General.O.ConstraintsInProgress";
                    if (!string.IsNullOrWhiteSpace(errorMessage))
                    {
                        ErrorCode.Add(errorMessage);
                    }
                }
                //Check if there are constraints in progress
                if (false)//yaron
                {
                    if (_DeclarationPM.DeclarationStatusTypeCode == "13")
                    {
                        var errorMessage = "";

                        if (_DeclarationPM.DeclarationConstraints != null)
                        {
                            if (_DeclarationPM.DeclarationConstraints.Count > 0)
                            {
                                errorMessage = "Customs.General.O.ConstraintsInProgress2";
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(errorMessage))
                        {
                            ErrorCode.Add(errorMessage);
                        }
                    }
                }
            }

        }


        //Check if future payment was done
        public void FuturePaymentDoneCheck()
        {
            if (_DeclarationPM != null)
            {
                if (_DeclarationPM.DeclarationStatusTypeCode == "10")
                {
                    var errorMessage = "Customs.General.O.FuturePaymentDone";
                    if (!string.IsNullOrWhiteSpace(errorMessage))
                    {
                        ErrorCode.Add(errorMessage);
                    }
                }
            }
        }

        //An indication to send declaration again- in this case only Declaration Payment is allowed // Mirit 25/06/15 Task 14330
        public void SubmitDeclarationAgainDoneCheck()
        {
            if (_DeclarationPM.DeclarationStatusTypeCode == "14")
            {
                var errorMessage = "Customs.General.O.SubmitDeclarationAgain";
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    ErrorCode.Add(errorMessage);
                }
            }
        }

        //Check if future payment was done
        public void TaxationDateTimeCheck()
        {
            if (_DeclarationPM != null)
            {
                var errorMessage = "";
                if (!_DeclarationPM.TaxationDateTime.HasValue)
                {
                    errorMessage = "Customs.General.O.TaxationDateTimeNotToday";
                }
                else
                {
                    if ((_DeclarationPM.TaxationDateTime.Value.Date.Year != DateTime.Now.Date.Year) ||
                        (_DeclarationPM.TaxationDateTime.Value.Date.Month != DateTime.Now.Date.Month) ||
                        (_DeclarationPM.TaxationDateTime.Value.Date.Day != DateTime.Now.Date.Day))
                    {
                        errorMessage = "Customs.General.O.TaxationDateTimeNotToday";
                    }
                }
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    ErrorCode.Add(errorMessage);
                }
            }
        }

        //Check if future payment was done
        public void DocumetsUploadedCheck()
        {
            var errorMessage = "";
            //customDomainService = new customDomainService();
            List<CustomsDocumentPointerPM> customsDocumentPointerPMList = new List<CustomsDocumentPointerPM>();


            foreach (var customsDocumentPointerPM in customsDocumentPointerPMList)
            {
                List<CustomsDocumentPM> customsDocumentPMList = new List<CustomsDocumentPM>();

                foreach (var customsDocumentPM in customsDocumentPMList)
                {
                    if (string.IsNullOrWhiteSpace(customsDocumentPM.CustomsDocId))
                    {
                        errorMessage = "Customs.General.O.DocumetsUploaded";
                        break;
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                ErrorCode.Add(errorMessage);
            }
        }

        //Check if there is at least one Supplier Invoice AND each Supplier Invoice has at least one Item
        public void CheckSupplierInvoice()
        {
            var errorMessage = "";
            //Check if there is at least one Supplier Invoice
            if (_DeclarationPM.SupplierInvoices == null)
            {
                errorMessage = "Customs.General.O.NoSupplierInvoiceForDeclaration";
            }
            else
            {
                if (_DeclarationPM.SupplierInvoices.Count == 0)
                {
                    errorMessage = "Customs.General.O.NoSupplierInvoiceForDeclaration";
                }
                //If there is at least one Supplier Invoice
                else
                {
                    //Check that each Supplier Invoice has at least one Item
                    foreach (var supplierInvoices in _DeclarationPM.SupplierInvoices)
                    {
                        if (supplierInvoices.SupplierInvoiceItems == null)
                        {
                            errorMessage = "Customs.General.O.NoSupplierInvoiceItemForInvoice";
                        }
                        else
                        {
                            if (supplierInvoices.SupplierInvoiceItems.Count == 0)
                            {
                                errorMessage = "Customs.General.O.NoSupplierInvoiceItemForInvoice";
                            }
                        }
                    }
                }
            }


            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                ErrorCode.Add(errorMessage);
            }
        }

        //Check if there is at least one Package for each Consignment
        private void CheckConsignmentPackages()
        {
            var errorMessage = "";

            if (_DeclarationPM.Consignments == null)
            {
                return;
            }

            foreach (var consignments in _DeclarationPM.Consignments)
            {
                if (consignments.ConsignmentPackages == null)
                {
                    errorMessage = "Customs.General.O.NoConsignmentPackages";
                }
                else
                {
                    if (consignments.ConsignmentPackages.Count == 0)
                    {
                        errorMessage = "Customs.General.O.NoConsignmentPackages";
                    }
                }

                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    ErrorCode.Add(errorMessage);
                }
            }
        }

        //Validation Checks before sending Declaration to customs
        public void PreDeclarationSendChecks()
        {
            string errorMessage="";
            //CheckSupplierInvoice();
            PaymentDateCheck();
            //ImporterCheck();
            //Importer
            errorMessage = ImportersCheck("", _DeclarationPM.ImporterCode, _DeclarationPM.ImporterId, _DeclarationPM.ImporterTypeCode, _DeclarationPM.ImporterName, _DeclarationPM.ImporterAddress, _DeclarationPM.ImporterPassportNumber, _DeclarationPM.ImporterPassCountryCode);
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                ErrorCode.Add(errorMessage);
            }
            //Transfer Importer
            //errorMessage = ImportersCheck("מעביר", _DeclarationPM.TransferImporterCode, _DeclarationPM.TransferImporterId, _DeclarationPM.TransferImporterTypeCode, _DeclarationPM.TransferImporterName, _DeclarationPM.TransferImporterAddress, _DeclarationPM.TransferPassportNumber, _DeclarationPM.TransferImporterCountryCode);
            //if (!string.IsNullOrWhiteSpace(errorMessage))
            //{
            //    ErrorCode.Add(errorMessage);
            //}
            //Transfer Importer
            //errorMessage = ImportersCheck("זכאי", _DeclarationPM.EntitleImporterCode, _DeclarationPM.EntitleImporterId, _DeclarationPM.EntitleImporterTypeCode, _DeclarationPM.EntitleImporterName, _DeclarationPM.EntitleImporterAddress, _DeclarationPM.EntitlePassportNumber, _DeclarationPM.EntitleImporterCountryCode);
            //if (!string.IsNullOrWhiteSpace(errorMessage))
            //{
            //    ErrorCode.Add(errorMessage);
            //}

            //allready done by displayt only - pls check ConstraintsInProgressCheck();
            //allready done by displayt only - pls check FuturePaymentDoneCheck();
            CheckConsignmentPackages();
        }


        //<--- Yuval Chalup 18.11.2014 TASK-4240
        //Checks for opening Declaration view as 'Display Only'
        public void DeclarationViewDisplayOnlyChecks()
        {
            PaymentDateCheck();
            ConstraintsInProgressCheck();
            FuturePaymentDoneCheck();
            //SubmitDeclarationAgainDoneCheck(); // Mirit 25/06/15 Task 14330 + Remarked by Yuval Chalup 02.08.2015 TASK-15145
            CheckIsConvertedDeclaration(); // Mirit 02/12/15 Task 18508
            CheckIsCloseDeclaration();
        }

        private void CheckIsCloseDeclaration()
        {
            if (this._DeclarationPM != null)
            {
                if (this._DeclarationPM.IsClose && _DeclarationPM.Direction != "E")
                {
                    var errorMessage = "Customs.Declaration.O.Closed";
                    if (!string.IsNullOrWhiteSpace(errorMessage))
                    {
                        ErrorCode.Add(errorMessage);
                    }
                }
            }
        }

        //Yuval Chalup 18.11.2014 TASK-4240 --->


        //Check if it's a converted declaration (IsConvertedDeclaration=True)  // Mirit 02/12/15 Task 18508
        public void CheckIsConvertedDeclaration()
        {

            if (_DeclarationPM != null)
            {
                if (_DeclarationPM.IsConvertedDeclaration == true)
                {
                    var errorMessage = "Customs.General.O.IsConvertedDeclaration";
                    if (!string.IsNullOrWhiteSpace(errorMessage))
                    {
                        ErrorCode.Add(errorMessage);
                    }
                }
            }
        }




        //public interface IDeclarationValidator
        //{
        //    List<String> MustBeforeSend(DeclarationPM declarationPM);
        //}
    }
}
