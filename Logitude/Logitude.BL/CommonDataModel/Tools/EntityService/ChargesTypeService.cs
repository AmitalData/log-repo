using System.Collections.Generic;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools.CustomFields;
using System.Linq;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Data.InvoiceModel;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class ChargesTypeService
    {
        bool isNewEntity;
        private int tenant;
        public ChargesType Poco { get; set; }
        private ChargesTypePM entityPm;
        private ICommonDataContext objectContext;
        private ChargesTypeRepository entityRepository;
        private ChargeTypeAccountingRepository chargeTypeAccountingRepository;
        public ChargesTypeService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new ChargesTypeRepository(objectContext);
            this.chargeTypeAccountingRepository = new ChargeTypeAccountingRepository(objectContext);
        }

        private List<ChargeTypeAccountingPM> chargeTypeAccountingsChangeSet;
        public void SetChangeSet(List<ChargeTypeAccountingPM> chargeTypeAccountingsChangeSet)
        {
            this.chargeTypeAccountingsChangeSet = chargeTypeAccountingsChangeSet;
        }

        public void Create(ChargesTypePM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("ChargesType", tenant).ToString();
            this.Poco = new ChargesType();
            this.Poco.Id = this.entityPm.Id;

            ChargesTypeValidating.Validate(entityPM, objectContext, this.isNewEntity);

            if (!string.IsNullOrEmpty(entityPM.ChargesGroupCode))
            {
                FullChargesGroupId(entityPM);
            }
            if (!string.IsNullOrEmpty(entityPM.MeasurementCode))
            {
                FillMeasurementId(entityPM);
            }

            foreach (ChargeTypeAccountingPM item in entityPM.ChargeTypeAccountings)
            {
                this.CreateChargeTypeAccounting(item);
            }

            ChargesTypeTracing.Trace(entityPM, Poco, isNewEntity);
            ChargesTypeMapping.MapEntity(entityPM, Poco, isNewEntity);

            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

            new EntityCustomFieldService(new EntityCustomFieldServiceArgs() {ObjectTableName = "ChargesType", EntityId = entityPM.Id , Tenant = entityPM.Tenant , Type = "PM" , Entities = new List<ChargesTypePM> { entityPM }.Cast<object>().ToList() }).Update();

            TableLastUpdateClass.UpdateTableHistory(tenant, "ChargesType");
        }

        public void Update(ChargesTypePM entityPM, bool mapComposition = false)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleChargesType(entityPM.Id, entityPM.Tenant);

            ChargesTypeValidating.Validate(entityPM, objectContext, this.isNewEntity);

            if (mapComposition)
            {
                this.chargeTypeAccountingsChangeSet = entityPM.ChargeTypeAccountings;
            }

            this.UpdateChargeTypeAccountingsCollection();

            if (!string.IsNullOrEmpty(entityPM.ChargesGroupCode))
            {
                FullChargesGroupId(entityPM);
            }
            if (this.entityPm.PayableDebitGLAcountId != this.Poco.PayableDebitGLAcountId)
            {
                this.UpdatePayableDebitGLAccountInAPInvoiceLine();
            }
            ChargesTypeTracing.Trace(entityPM, Poco, isNewEntity);         
            ChargesTypeMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
            new EntityCustomFieldService(new EntityCustomFieldServiceArgs() { ObjectTableName = "ChargesType", EntityId = entityPM.Id, Tenant = entityPM.Tenant, Type = "PM", Entities = new List<ChargesTypePM> { entityPM }.Cast<object>().ToList() }).Update();

            TableLastUpdateClass.UpdateTableHistory(tenant, "ChargesType");
            
        }

        private void UpdateChargeTypeAccountingsCollection()
        {
            if (chargeTypeAccountingsChangeSet != null)
            {
                foreach (ChargeTypeAccountingPM itemPM in chargeTypeAccountingsChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateChargeTypeAccounting(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateChargeTypeAccounting(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteChargeTypeAccounting(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }

        private void UpdatePayableDebitGLAccountInAPInvoiceLine()
        {
            using (InvoiceContext invoiceContext = new InvoiceContext())
            {
                APInvoiceLineRepository apInvoiceLineRepository = new APInvoiceLineRepository(invoiceContext);
                APInvoiceQuery qpInvoiceQuery = new APInvoiceQuery();
                List<APInvoiceLine> apInvoiceLines = qpInvoiceQuery.GetAPInvoiceLinesByChargeTypeId(entityPm.Id, entityPm.Tenant).ToList();

                foreach (APInvoiceLine apInvoiceLine in apInvoiceLines)
                {
                    APInvoiceLine newApInvoiceLine = new APInvoiceLine
                    {
                        APInvoiceId = apInvoiceLine.APInvoiceId,
                        LineNumber = apInvoiceLine.LineNumber,
                        Tenant = apInvoiceLine.Tenant,
                        InvoiceCurrencyAmount = apInvoiceLine.InvoiceCurrencyAmount,
                        LocalCurrencyAmount = apInvoiceLine.LocalCurrencyAmount,
                        ProfitCurrencyAmount = apInvoiceLine.ProfitCurrencyAmount,
                        Notes = apInvoiceLine.Notes,
                        ChargesTypeId = apInvoiceLine.ChargesTypeId,
                        VatTypeId = apInvoiceLine.VatTypeId,
                        EntityId = apInvoiceLine.EntityId,
                        EntityPayableId = apInvoiceLine.EntityPayableId,
                        RefundAmount = apInvoiceLine.RefundAmount,
                        VatPercentage = apInvoiceLine.VatPercentage,
                        VatAmount = apInvoiceLine.VatAmount,
                        ForiegnCurrencyId = apInvoiceLine.ForiegnCurrencyId,
                        ForiegnCurrencyAmount = apInvoiceLine.ForiegnCurrencyAmount,
                        ForiegnExchangeRate = apInvoiceLine.ForiegnExchangeRate,
                        DebitAccount = apInvoiceLine.DebitAccount,
                        Description = apInvoiceLine.Description,
                        LocalDescription = apInvoiceLine.LocalDescription,
                        ChargeTypeGLAccountId = apInvoiceLine.ChargeTypeGLAccountId,
                        AuthorizedSignatory = apInvoiceLine.AuthorizedSignatory,
                        PrepaidCollectId = apInvoiceLine.PrepaidCollectId,
                        ContainerTypeId = apInvoiceLine.ContainerTypeId,
                        Quantity = apInvoiceLine.Quantity,
                        AutomaticLastUpdateDate = apInvoiceLine.AutomaticLastUpdateDate,
                        PayableDebitGLAcountId = entityPm.PayableDebitGLAcountId 
                    };

                    apInvoiceLineRepository.Update(newApInvoiceLine);
                }

                invoiceContext.SaveChanges();
            }
        }
        private void CreateChargeTypeAccounting(ChargeTypeAccountingPM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("ChargeTypeAccounting", tenant).ToString();
            itemPM.ChargeTypeId = entityPm.Id;
            itemPM.Tenant = tenant;

            ChargeTypeAccounting itemPoco = new ChargeTypeAccounting()
            {
                Id = itemPM.Id,
                ChargeTypeId = itemPM.ChargeTypeId,
                Tenant = itemPM.Tenant
            };

            ChargesTypeMapping.MapChargeTypeAccounting(itemPM, itemPoco, true);
            chargeTypeAccountingRepository.Add(itemPoco);              
        }
        private void UpdateChargeTypeAccounting(ChargeTypeAccountingPM itemPM)
        {
            ChargeTypeAccounting itemPoco = chargeTypeAccountingRepository.GetSingleChargeTypeAccountings(itemPM.Id, tenant);

            if (itemPoco != null)
            {
                ChargesTypeMapping.MapChargeTypeAccounting(itemPM, itemPoco, false);
                chargeTypeAccountingRepository.Update(itemPoco);
            }
        }
        private void DeleteChargeTypeAccounting(ChargeTypeAccountingPM itemPM)
        {
            ChargeTypeAccounting itemPoco = chargeTypeAccountingRepository.GetSingleChargeTypeAccountings(itemPM.Id, tenant);

            if (itemPoco != null)
            {
                chargeTypeAccountingRepository.Remove(itemPoco);
            }
        }

        private static void FullChargesGroupId(ChargesTypePM entityPM)
        {
            ChargesGroupQuery chargesGroupQuery = new ChargesGroupQuery(entityPM.Tenant);
            ChargesGroupPM chargesGroupPM = chargesGroupQuery.GetSingleChargesGroupPMByCode(entityPM.ChargesGroupCode, entityPM.Tenant);
            if (chargesGroupPM != null)
            {
                entityPM.ChargesGroupId = chargesGroupPM.Id;
            }
        }

        private static void FillMeasurementId(ChargesTypePM entityPM)
        {
            MeasurementQuery measurementQuery = new MeasurementQuery(entityPM.Tenant);
            MeasurementPM measurementPM = measurementQuery.GetSinglePMByCode(entityPM.MeasurementCode, entityPM.Tenant);
            if (measurementPM != null)
            {
                entityPM.MeasurementId = measurementPM.Id;
            }
        }
    }
}
