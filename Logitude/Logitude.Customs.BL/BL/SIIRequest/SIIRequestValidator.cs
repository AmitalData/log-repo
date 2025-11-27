using Logitude.Customs.BL.BL.SIIRequest;
using Logitude.Customs.Data.DataContracts.SIIRequest;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

internal static class SIIRequestValidator
{
    internal const string RequiredFieldsTextCode = "Customs.SIIRequest.O.RequiredFields";
    internal const string LineCode = "Customs.SIIRequest.O.Line";
    internal const string AttachmentCode = "Customs.SIIRequest.O.Attachment";

    public static void Validate(ReleaseRequestApiDto dto, int tenant)
    {
        var errors = new List<string>();


        var f = dto.releaseRequestForm;
        Check(f.formApplicationId, "formApplicationId", errors);
        Check(f.importerNumber, "importerNumber", errors);
        Check(f.importerEmail, "importerEmail", errors);
        Check(f.applicantIdNumber, "applicantIdNumber", errors);

        if (f.applicantSystemId <= 0)
            errors.Add("applicantSystemId");

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

        foreach (var (line, i) in dto.releaseRequestForm.releaseRequestLinesForm
                                         .Select((l, idx) => (l, idx + 1)))
        {
            string p = $"line[{line.UiLineNumber}]";

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
                Check(line.quantityByDeclaredUnit, $"{p}.quantityByDeclaredUnit", errors);
                Check(line.declaredUnitCode, $"{p}.declaredUnitCode", errors);
            }

        }


        foreach (var (att, i) in dto.formAttachments.Select((a, idx) => (a, idx)))
        {
            string p = $"attachment[{i}]";
            CheckIndex(att.formAttachmentIndex, $"{p}.formAttachmentIndex", errors);
        }

        string lineLabel = Translate(LineCode, tenant);
        string attachmentLabel = Translate(AttachmentCode, tenant);

        if (errors.Count > 0)
        {
            var translatedErrors = errors.Select(raw =>
            {
                var m = Regex.Match(raw, @"^(?<type>line|attachment)\[(?<idx>\d+)\]\.(?<field>.+)$");
                if (m.Success)
                {
                    string context = m.Groups["type"].Value == "line"
                                     ? $"{lineLabel} {m.Groups["idx"].Value} – "
                                     : $"{attachmentLabel} {m.Groups["idx"].Value} – ";

                    string fieldKey = m.Groups["field"].Value;
                    string hebrew = Translate($"Customs.SIIRequest.O.{fieldKey}", tenant);

                    return context + hebrew;
                }


                return Translate($"Customs.SIIRequest.O.{raw}", tenant);
            });

            var prefix = Translate(RequiredFieldsTextCode, tenant);

            throw new InvalidOperationException(
                prefix + ":" + Environment.NewLine +
                "• " + string.Join(Environment.NewLine + "• ", translatedErrors));
        }
    }

    public static string Translate(string code, int tenant)
    {
        if (SIIRequestApiRequestFactory.OverrideITextCodeTranslator != null)
            return SIIRequestApiRequestFactory.OverrideITextCodeTranslator.Translate(code, tenant);

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
