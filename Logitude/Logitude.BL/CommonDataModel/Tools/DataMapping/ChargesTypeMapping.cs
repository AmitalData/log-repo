using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class ChargesTypeMapping
    {
        public static void MapEntity(ChargesTypePM entityPM, ChargesType poco, bool isNewEntity)
        {
            //ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);

            if (isNewEntity)
            {
                poco.Tenant = entityPM.Tenant;             
            }
            
            poco.AddedManually = entityPM.AddedManually;
            poco.Code = entityPM.Code;
            poco.MeasurementId = entityPM.MeasurementId;
            poco.InActive = entityPM.InActive;
            poco.LocalName = entityPM.LocalName;
            poco.EnglishName = entityPM.EnglishName;
            poco.Tenant = entityPM.Tenant;
            poco.AWBPrintDescription = entityPM.AWBPrintDescription;
            poco.ChargesGroupCode = entityPM.ChargesGroupCode;
            poco.ChargesGroupId = entityPM.ChargesGroupId;
            poco.IATACodeId = entityPM.IATACodeId;
            poco.Description = entityPM.Description;
            poco.IsAir = entityPM.IsAir;
            poco.IsOcean = entityPM.IsOcean;
            poco.IsInland = entityPM.IsInland;
            poco.IsAutoDisplayInConsolidation = entityPM.IsAutoDisplayInConsolidation;
            poco.IsAutoDisplayInShipment = entityPM.IsAutoDisplayInShipment;
            poco.IsPayable = entityPM.IsPayable;
            poco.IsReceivable = entityPM.IsReceivable;
            poco.VatTypeId = entityPM.VatTypeId;
            poco.DueTypeCode = entityPM.DueTypeCode;
            poco.IsAutoDisplayInQuote = entityPM.IsAutoDisplayInQuote;
            poco.ContainerMeasurementId = entityPM.ContainerMeasurementId;
            poco.ViewOrder = entityPM.ViewOrder;
            poco.PayableAccountId = entityPM.PayableAccountId;
            poco.ReceivableAccountId = entityPM.ReceivableAccountId;
            poco.ReceivablesChargesTypeExternalCode = entityPM.ReceivablesChargesTypeExternalCode;
            poco.PayablesChargesTypeExternalCode = entityPM.PayablesChargesTypeExternalCode;
            poco.ReceivableCreditAccount = entityPM.ReceivableCreditAccount;
            poco.PayableDebitAccount = entityPM.PayableDebitAccount;
            poco.AccountingVATSplit = entityPM.AccountingVATSplit;
            poco.PayableDebitGLAcountId = entityPM.PayableDebitGLAcountId;
            poco.ReceivableCreditGLAccountId = entityPM.ReceivableCreditGLAccountId;
            poco.IsAutoDisplayInCustoms = entityPM.IsAutoDisplayInCustoms;
            poco.IsCustoms = entityPM.IsCustoms;
            poco.IsBackToBack = entityPM.IsBackToBack;
            poco.SATExternalId = entityPM.SATExternalId;
            poco.IsExpense = entityPM.IsExpense;
            poco.IsDomestic = entityPM.IsDomestic;
            poco.IsDrop = entityPM.IsDrop;
            poco.IsImport = entityPM.IsImport;
            poco.IsExport = entityPM.IsExport;

            poco.ReceivablesDefaultCurrencyId = entityPM.ReceivablesDefaultCurrencyId;
            poco.PayablesDefaultCurrencyId = entityPM.PayablesDefaultCurrencyId;

            BuildSearchField(entityPM, poco);
        }

        internal static void MapChargeTypeAccounting(ChargeTypeAccountingPM itemPM, ChargeTypeAccounting itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Id = itemPM.Id;
                itemPoco.Tenant = itemPM.Tenant;
                itemPoco.ChargeTypeId = itemPM.ChargeTypeId;
            }

            itemPoco.VatTypeId = itemPM.VatTypeId;
            itemPoco.PayableDebitAccount = itemPM.PayableDebitAccount;
            itemPoco.ReceivableCreditAccount = itemPM.ReceivableCreditAccount;
            itemPoco.PayableDebitGLAcountId = itemPM.PayableDebitGLAcountId;
            itemPoco.ReceivableCreditGLAccountId = itemPM.ReceivableCreditGLAccountId;
        }

        private static void BuildSearchField(ChargesTypePM entityPM, ChargesType entityPoco)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Code);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.EnglishName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.LocalName);
           ChargesTypePM chargesType = SetChargesTypetGLAccountFields(entityPM);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ReceivableCreditGLAcountLocalName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ReceivableCreditGLAcountNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.PayableDebitGLAcountLocalName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.PayableDebitGLAcountNumber);
            entityPM.SearchFields = mySearchFields;
            entityPoco.SearchFields = mySearchFields;
        }

        private static ChargesTypePM SetChargesTypetGLAccountFields(ChargesTypePM chargesType)
        {
            IGLAccountQueryServiceExt glAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
            if (chargesType.ReceivableCreditGLAccountId != null) {
                string fieldsValues = glAccountQuery.GetGLAccountDisplayNoAndLocalName(chargesType.ReceivableCreditGLAccountId, chargesType.Tenant);
                string[] displayNoAndName = fieldsValues.Split(',');
                chargesType.ReceivableCreditGLAcountLocalName = displayNoAndName[1];
                chargesType.ReceivableCreditGLAcountNumber = displayNoAndName[0];
            }
            if (chargesType.PayableDebitGLAcountId != null)
            {
                string fieldsValues = glAccountQuery.GetGLAccountDisplayNoAndLocalName(chargesType.PayableDebitGLAcountId, chargesType.Tenant);
                string[] displayNoAndName = fieldsValues.Split(',');
                chargesType.PayableDebitGLAcountLocalName = displayNoAndName[1];
                chargesType.PayableDebitGLAcountNumber = displayNoAndName[0];
            }
            return chargesType;
        }
    }
}