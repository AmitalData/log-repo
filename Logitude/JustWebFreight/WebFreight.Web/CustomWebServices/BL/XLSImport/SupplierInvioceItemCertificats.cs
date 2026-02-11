using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.Repsitories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.CustomWebServices.BL.XLSImport
{
    public class SupplierInvioceItemCertificats
    {
        public List<CertificateFromFile> fromFile = new List<CertificateFromFile>();
        public List<CertificateErrorView> errors = new List<CertificateErrorView>();


        internal List<CertificateErrorView> RecallSuppliersFromFileRequest(string key, int tenant, string decodedString, string clientID)
        {
            ReadDataFromCsvFile(decodedString);
            AnalayzeData(tenant, clientID);
            return errors;
        }
        public void AnalayzeData(int tenant, string clientID)
        {
            foreach (CertificateFromFile item in fromFile)
            {
                foreach (ModelCodeAndConfirmatioNCode model in item.ModelCodeAndConfirmatioNCodeList)
                {
                    UpdateDB(item, model, tenant, clientID);
                }
            }

        }
        public void UpdateDB(CertificateFromFile item, ModelCodeAndConfirmatioNCode model, int tenant, string clientID)
        {
            SupplierInvoiceRepository supplierInvoiceRepository = new SupplierInvoiceRepository(tenant);
            DeclarationRepository delcarationRepository = new DeclarationRepository(tenant);
            SupplierInvoiceItemQueryService supplierInvoiceItemRepository = new SupplierInvoiceItemQueryService(tenant);
            ICustomContext MyContext = CustomContext.GetContext(tenant);
            SupplierInvioceItemCertificatUpdateService updateService = new SupplierInvioceItemCertificatUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
            SupplierInvioceItemCertificatRepository repo = new SupplierInvioceItemCertificatRepository(tenant);
            Boolean foundInvoiceItem = false;
            Boolean LockedDec = false;
            var list = supplierInvoiceRepository.GetDeclarationIdfromInvoiceNumber(item.SupplierItemInvoice, tenant);
            var decList = delcarationRepository.GetDeclarationsByIdAndClientID(list, clientID);
            if (decList.Count == 0)
            {
                AddErrors("", model.RowNumber, "", item.SupplierItemInvoice, model.ModelCode, "	הצהרה ו/או מס' חשבון ספק לא אותר");
            }
            foundInvoiceItem = false;
            foreach (var dec in decList)
            {
                LockedDec = false;
                var invoiceItems = supplierInvoiceItemRepository.GetSupplierInvoiceItemByInvoiceNumber(tenant, dec.Id, model.ModelCode);
                foreach (SupplierInvoiceItemPM invoiceItem in invoiceItems)
                {
                    if (model.ConfirmationCode == "")
                    {
                        AddErrors(dec.DeclarationNumber, model.RowNumber, dec.CustomFileNo, item.SupplierItemInvoice, model.ModelCode, "מס' אישור לא אותר בעמודה J  באקסל");
                    }
                    foundInvoiceItem = true;
                    if (dec.PaymentDate != null)
                    {
                        AddErrors(dec.DeclarationNumber, model.RowNumber, dec.CustomFileNo, item.SupplierItemInvoice, model.ModelCode, "הצהרה שולמה");
                        continue;
                    }
                    if (dec.DeclarationStatusTypeCode == "1")
                    {
                        AddErrors(dec.DeclarationNumber, model.RowNumber, dec.CustomFileNo, item.SupplierItemInvoice, model.ModelCode, "הצהרה בוטלה");
                        continue;
                    }
                    long lCUSTOMFILENO;
                    if (!long.TryParse(dec.CustomFileNo, out lCUSTOMFILENO))
                    {
                        throw new BusinessErrorException("_DirtyDeclarationPaymentPM.DeclarationId could not convert to long ");
                    }
                    var myCCUFILEMRepository = new CCUFILEMRepository(dec.Tenant);
                    var ccufilem = myCCUFILEMRepository.GetFILENOByCUSTOMFILENO(lCUSTOMFILENO,dec.Tenant);
                    var myCCUQUELOCKRepository = new CCUQUELOCKRepository(dec.Tenant);
                    try
                    {
                        var cculock = myCCUQUELOCKRepository.GetSingleGeneralLockNOWAIT("CCUFILEM", ccufilem.ToString());
                    }
                    catch (System.Exception)
                    {
                        AddErrors(dec.DeclarationNumber, model.RowNumber, dec.CustomFileNo, item.SupplierItemInvoice, model.ModelCode, "התיק נעול על ידי משתמש אחר");
                        LockedDec = true;
                    }

                    if (!LockedDec && model.ConfirmationCode != "" && !repo.IsExist(dec.Id, invoiceItem.CounterKey, invoiceItem.LineNumber, tenant, "2402", model.ConfirmationCode, model.RequestNumber))
                    {
                        updateService.InsertSupplierInvioceItemCertificatByCsvFile(model.ConfirmationCode, model.RequestNumber, tenant, dec.Id, invoiceItem.LineNumber, invoiceItem.CounterKey, invoiceItem);
                    }
                }
            }
            if (!foundInvoiceItem)
            {
                if (item.SupplierItemInvoice != "" && item.SupplierItemInvoice != null)
                {
                    AddErrors("", model.RowNumber, "", item.SupplierItemInvoice, model.ModelCode, "פרט מכס לא אותר");
                    if (model.ConfirmationCode == "")
                    {
                        AddErrors("", model.RowNumber, "", item.SupplierItemInvoice, model.ModelCode, "מס' אישור לא אותר בעמודה J  באקסל");
                    }
                }
            }

        }
        public void ReadDataFromCsvFile(string decodedString)
        {
            var lines = decodedString.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> results = new List<string>();
            foreach (string line in lines)
            {
                results.AddRange(Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))"));
            }
            Boolean CodeExist = false;
            for (int i = 10; i < results.Count;) // the excel has 10 cols  , Excel Analayze
            {
                CodeExist = false;
                string SupplierItemInvoice = results[i + 8];
                foreach (var item in fromFile) // check if code exist in list already
                {
                     if (item.SupplierItemInvoice == SupplierItemInvoice)
                    {
                        CodeExist = true;
                        item.ModelCodeAndConfirmatioNCodeList.Add(new ModelCodeAndConfirmatioNCode
                        {
                            RequestNumber = results[i + 2],
                            RowNumber = results[i + 3],
                            ModelCode = PadLeftOnModelCdoe(results[i + 5]),
                            ConfirmationCode = Regex.Replace(results[i + 9], @"[^\d]", ""),
                        });
                        break;
                    }
                }
                if (!CodeExist)
                {
                    CertificateFromFile row = new CertificateFromFile();
                    row.SupplierItemInvoice = SupplierItemInvoice;
                    row.ModelCodeAndConfirmatioNCodeList = new List<ModelCodeAndConfirmatioNCode>
                    {
                        new ModelCodeAndConfirmatioNCode
                        {
                            RequestNumber=results[i+2],
                            RowNumber=results[i+3],
                            ModelCode=PadLeftOnModelCdoe(results[i + 5]),
                            ConfirmationCode = Regex.Replace(results[i + 9], @"[^\d]", ""),
                        }
                    };
                    fromFile.Add(row);
                }
                i += 10;
            }

        }
         public string PadLeftOnModelCdoe(string code)
        {
            if (Regex.IsMatch(code , @"^\d+$"))
            {
                if (code.Length < 8)
                {
                    int value = Convert.ToInt32(code);
                    var modelCode = value.ToString("D8");
                    return modelCode;
                }
            }
            return code;
        }
        public void AddErrors(string decID, string excelRow, string customFileNr, string supplierItemInvoice, string modelCode, string error)
        {
            errors.Add(new CertificateErrorView()
            {
                ExcelRow = excelRow,
                DeclarationId = decID,
                CustomfileNr = customFileNr,
                SupplierItemInvoice = supplierItemInvoice,
                Model = modelCode,
                Errors = error,
            });
        }
        public List<CertificateErrorView> getErrorsList()
        {
            return errors;
        }
        public byte[] ExportErrors(int tenant, List<CertificateErrorView> errorsList)
        {
            DataTable dt = null;
            var settingCol = new BITabularViewSettings()
            {
                Columns = new List<Column>()
            };
            dt = new DataTable("Supplier Invioce Item Certificat Errors");

            settingCol.Columns.Add(new Column() { Index = 1, Code = "ExcelRow", Name = "ExcelRow", DataTypeCode = "String", Width = 150, });
            dt.Columns.Add(new DataColumn() { Caption = /*"ExcelRow"*/"שורה בבקשה", ColumnName = "ExcelRow", DataType = System.Type.GetType("System.String"), });

            settingCol.Columns.Add(new Column() { Index = 2, Code = "CustomfileNr", Name = "CustomfileNr", DataTypeCode = "String", Width = 150, });
            dt.Columns.Add(new DataColumn() { Caption = /*"CustomfileNr"*/"מס' תיק", ColumnName = "CustomfileNr", DataType = System.Type.GetType("System.String"), });

            settingCol.Columns.Add(new Column() { Index = 3, Code = "DeclarationId", Name = "DeclarationId", DataTypeCode = "String", Width = 150, });
            dt.Columns.Add(new DataColumn() { Caption = /*"DeclarationId"*/"מס' הצהרה", ColumnName = "DeclarationId", DataType = System.Type.GetType("System.String"), });

            settingCol.Columns.Add(new Column() { Index = 4, Code = "SupplierItemInvoice", Name = "SupplierItemInvoice", DataTypeCode = "String", Width = 150, });
            dt.Columns.Add(new DataColumn() { Caption = /*"SupplierItemInvoice"*/"מס' חשבון ספק", ColumnName = "SupplierItemInvoice", DataType = System.Type.GetType("System.String"), });

            settingCol.Columns.Add(new Column() { Index = 5, Code = "Model", Name = "Model", DataTypeCode = "String", Width = 150, });
            dt.Columns.Add(new DataColumn() { Caption = /*"Model"*/"דגם", ColumnName = "Model", DataType = System.Type.GetType("System.String"), });

            settingCol.Columns.Add(new Column() { Index = 6, Code = "Errors", Name = "Errors", DataTypeCode = "String", Width = 150, });
            dt.Columns.Add(new DataColumn() { Caption = /*"Errors"*/"הודעת שגיאה", ColumnName = "Errors", DataType = System.Type.GetType("System.String"), });
            errorsList.ForEach(r =>
            {
                var newrow = dt.NewRow();
                newrow[0] = r.ExcelRow;
                newrow[1] = r.CustomfileNr;
                newrow[2] = r.DeclarationId;
                newrow[3] = r.SupplierItemInvoice;
                newrow[4] = r.Model;
                newrow[5] = r.Errors;
                dt.Rows.Add(newrow);
            });
            var xls = new ExportToExcelHelper();
            var res = xls.ExportDataTableToExcel(dt, tenant, settingCol);
            return res;
        }
    }
    public class CertificateErrorView
    {
        public string ExcelRow;
        public string CustomfileNr;
        public string DeclarationId;
        public string SupplierItemInvoice;
        public string Model;
        public string Errors;
    }
    public class CertificateFromFile
    {
        public string SupplierItemInvoice;
        public List<ModelCodeAndConfirmatioNCode> ModelCodeAndConfirmatioNCodeList;
    }
    public class ModelCodeAndConfirmatioNCode
    {
        public string ModelCode;
        public string ConfirmationCode;
        public string RequestNumber;
        public string RowNumber;
    }

}