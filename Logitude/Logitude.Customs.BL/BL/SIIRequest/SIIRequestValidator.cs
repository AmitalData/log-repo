using Logitude.Customs.BL.BL.SIIRequest;
using Logitude.Customs.Data.DataContracts.SIIRequest;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

internal static class SIIRequestValidator
{
    internal const string RequiredFieldsTextCode = "Customs.SIIRequest.O.RequiredFields";
    public static void Validate(ReleaseRequestApiDto dto, int tenant)
                         
    {
        var errors = new List<string>();

        /* -------------------------------------------------  
         *  ReleaseRequestForm  (always-required fields)  
         * ------------------------------------------------- */
        var f = dto.ReleaseRequestForm;
        Check(f.FormApplicationId, "formApplicationId", errors);
        Check(f.ImporterNumber, "importerNumber", errors);
        Check(f.ImporterEmail, "importerEmail", errors);
        Check(f.ApplicantIdNumber, "applicantIdNumber", errors);
        Check(f.ApplicantFullName, "applicantFullName", errors);
        Check(f.CustomsAgentRegisteredNumber, "customsAgentRegisteredNumber", errors);
        Check(f.CustomsAgentName, "customsAgentName", errors);
        Check(f.DeliveryArrivalDate, "deliveryArrivalDate", errors);
        Check(f.BillOfLadingId, "billOfLadingId", errors);
        CheckIndex(f.FormAttachmentIndex, "formAttachmentIndex", errors);
        Check(f.ImportCountry?.AlphaCode, "importCountry", errors);
        Check(f.DestinationPort?.Id, "destinationPort", errors);
        Check(f.WarehouseLocationName, "warehouseLocationName", errors);
        Check(f.WarehouseSettlement?.Id, "warehouseSettlement", errors);
        Check(f.ContactPersonFirstName, "contactPersonFirstName", errors);
        Check(f.ContactPersonLastName, "contactPersonLastName", errors);
        Check(f.ContactPersonPhone, "contactPersonPhone", errors);
        Check(f.ContactPersonEmail, "contactPersonEmail", errors);
        Check(f.IsNumericCountryCode, "isNumericCountryCode", errors);


        foreach (var (line, i) in dto.ReleaseRequestForm.ReleaseRequestLinesForm.Select((l, idx) => (l, idx + 1)))
        {
            string p = $"line[{i}]";
            Check(line.LineSerialNumber, $"{p}.lineSerialNumber", errors);
            Check(line.CustomsItem, $"{p}.customsItem", errors);
            Check(line.OriginCountry?.AlphaCode, $"{p}.originCountry", errors);
            Check(line.ModelCode, $"{p}.modelCode", errors);
            Check(line.ModelDescription, $"{p}.modelDescription", errors);
            Check(line.SupplierInvoiceNumber, $"{p}.supplierInvoiceNumber", errors);
            Check(line.SupplierInvoiceDate, $"{p}.supplierInvoiceDate", errors);
            CheckIndexList(line.FormAttachmentIndexes, $"{p}.formAttachmentIndexes", errors);
            Check(line.IsDutchGroup1Requested, $"{p}.isDutchGroup1Requested", errors);

            bool hasProductFile = !string.IsNullOrWhiteSpace(line.ProductFileNumber);

            if (hasProductFile)
            {
                Check(line.ProductFileNumber, $"{p}.productFileNumber", errors);
                Check(line.QuantityToRelease, $"{p}.quantityToRelease", errors);
                Check(line.SiiUnitCode, $"{p}.siiUnitCode", errors);
            }
            else
            {
                Check(line.ProductCode, $"{p}.productCode", errors);
            }

            bool declaredMismatch = line.QuantityByDecaredUnit == null ^ string.IsNullOrWhiteSpace(line.DeclaredUnitCode);
            if (declaredMismatch)
                errors.Add($"{p}.quantityByDeclaredUnit/declaredUnitCode must both be supplied or both empty");
        }


        foreach (var (att, i) in dto.FormAttachments.Select((a, idx) => (a, idx)))
        {
            string p = $"attachment[{i}]";
            CheckIndex(att.FormAttachmentIndex, $"{p}.formAttachmentIndex", errors);
        }

        if (errors.Count > 0)
        {
            var prefix = Translate(RequiredFieldsTextCode, tenant);
            throw new InvalidOperationException(
                $"{prefix}: {string.Join("; ", errors)}");
        }


    }
    private static string Translate(string code, int tenant)
    {
        if (SIIRequestApiRequestFactory.OverrideITextCodeTranslator != null)
            return SIIRequestApiRequestFactory.OverrideITextCodeTranslator.Translate(code, tenant);

        bool useLocal = true;
        var contact = SIIRequestApiRequestFactory.GetLoggedContact(tenant);
        if (contact != null) useLocal = !contact.DontShowLocal;

        var txt = TranslateTextsClass.Translate(code, tenant, useLocal);
        return string.IsNullOrWhiteSpace(txt) ? $"$Text({code})" : txt;
    }


    private static void Check(object value, string name, IList<string> errs)
    {
        switch (value)
        {
            case null:
                errs.Add(name);
                break;
            case string s when string.IsNullOrWhiteSpace(s):
                errs.Add(name);
                break;
        }
    }
    private static void CheckIndex(int idx, string name, IList<string> errs)
    {
        if (idx < 0) errs.Add(name);
    }
    private static void CheckIndexList(ICollection<int> list, string name, IList<string> errs)
    {
        if (list == null || list.Count == 0) errs.Add(name);
    }
}

internal enum ValidationScenario
{
    WithProductFile,   // ”תיק מוצר חובה“  
    AlphaNoProduct     // ”לקוח אלפא“ – product file missing   
}
