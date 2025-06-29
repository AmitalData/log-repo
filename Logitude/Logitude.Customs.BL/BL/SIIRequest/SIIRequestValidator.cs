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
         *  ReleaseRequestForm  (always‐required fields)  
         * ------------------------------------------------- */
        var f = dto.releaseRequestForm;
        Check(f.formApplicationId, "formApplicationId", errors);
        Check(f.importerNumber, "importerNumber", errors);
        Check(f.importerEmail, "importerEmail", errors);
        Check(f.applicantIdNumber, "applicantIdNumber", errors);
        Check(f.applicantFullName, "applicantFullName", errors);
        Check(f.customsAgentRegisteredNumber, "customsAgentRegisteredNumber", errors);
        Check(f.customsAgentName, "customsAgentName", errors);
        Check(f.deliveryArrivalDate, "deliveryArrivalDate", errors);
        Check(f.billOfLadingId, "billOfLadingId", errors);
        CheckIndex(f.formAttachmentIndex, "formAttachmentIndex", errors);
        Check(f.importCountry?.alphaCode, "importCountry", errors);
        Check(f.destinationPort?.id, "destinationPort", errors);
        Check(f.warehouseLocationName, "warehouseLocationName", errors);
        Check(f.warehouseSettlement?.id, "warehouseSettlement", errors);
        Check(f.contactPersonFirstName, "contactPersonFirstName", errors);
        Check(f.contactPersonLastName, "contactPersonLastName", errors);
        Check(f.contactPersonPhone, "contactPersonPhone", errors);
        Check(f.contactPersonEmail, "contactPersonEmail", errors);
        Check(f.isNumericCountryCode, "isNumericCountryCode", errors);

        /* -------------------------------------------------  
         *  ReleaseRequestLinesForm  
         * ------------------------------------------------- */
        foreach (var (line, i) in dto.releaseRequestForm.releaseRequestLinesForm
                                         .Select((l, idx) => (l, idx + 1)))
        {
            string p = $"line[{i}]";
            Check(line.lineSerialNumber, $"{p}.lineSerialNumber", errors);
            Check(line.customsItem, $"{p}.customsItem", errors);
            Check(line.originCountry?.alphaCode, $"{p}.originCountry", errors);
            Check(line.modelCode, $"{p}.modelCode", errors);
            Check(line.modelDescription, $"{p}.modelDescription", errors);
            Check(line.supplierInvoiceNumber, $"{p}.supplierInvoiceNumber", errors);
            Check(line.supplierInvoiceDate, $"{p}.supplierInvoiceDate", errors);
            CheckIndexList(line.formAttachmentIndexes, $"{p}.formAttachmentIndexes", errors);
            Check(line.isDutchGroup1Requested, $"{p}.isDutchGroup1Requested", errors);

            bool hasProductFile = !string.IsNullOrWhiteSpace(line.productFileNumber);
            if (hasProductFile)
            {
                Check(line.productFileNumber, $"{p}.productFileNumber", errors);
                Check(line.quantityToRelease, $"{p}.quantityToRelease", errors);
                Check(line.siiUnitCode, $"{p}.siiUnitCode", errors);
            }
            else
            {
                Check(line.productCode, $"{p}.productCode", errors);
            }

            bool declaredMismatch = line.quantityByDecaredUnit == null
                                    ^ string.IsNullOrWhiteSpace(line.declaredUnitCode);
            if (declaredMismatch)
                errors.Add($"{p}.quantityByDeclaredUnit/declaredUnitCode must both be supplied or both empty");
        }

        /* -------------------------------------------------  
         *  FormAttachments  
         * ------------------------------------------------- */
        foreach (var (att, i) in dto.formAttachments.Select((a, idx) => (a, idx)))
        {
            string p = $"attachment[{i}]";
            CheckIndex(att.formAttachmentIndex, $"{p}.formAttachmentIndex", errors);
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
            return SIIRequestApiRequestFactory
                   .OverrideITextCodeTranslator
                   .Translate(code, tenant);

        bool useLocal = true;
        var contact = SIIRequestApiRequestFactory.GetLoggedContact(tenant);
        if (contact != null) useLocal = !contact.DontShowLocal;

        var txt = TranslateTextsClass.Translate(code, tenant, useLocal);
        return string.IsNullOrWhiteSpace(txt)
            ? $"$Text({code})"
            : txt;
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
