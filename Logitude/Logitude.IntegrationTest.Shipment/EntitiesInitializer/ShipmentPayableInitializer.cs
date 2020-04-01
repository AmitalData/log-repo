using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Core.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Shipment.EntitiesInitializer
{
    public class ShipmentPayableInitializer : IEntityInitializer
    {
        public object Create(EntityInitializerArguments args)
        {
            ShipmentPayablePM entityPM = new ShipmentPayablePM()
            {
                Quantity = args.PayableQuantity,
                UnitPrice = args.PayableUnitPrice,
                Tenant = IntegrationTestLoginParameters.Tenant,
                CreatedByUserId = IntegrationTestLoginParameters.LoginUserId,
                UpdateByUserId = IntegrationTestLoginParameters.LoginUserId,
                ShipmentPayableLineStatusCode = "EMPT",
                ShipmentPayableAmountTypeCode = "ACCU",
                ShipmentPayableAmountTypeName = "Accrual",
                ChargesTypeId = ShipmentVariables.ChargeTypeAFTId,
                MeasurementId = ShipmentVariables.MeasurmentGRWTId,
                VatTypeId = ShipmentVariables.VATTypeZeroId,
                CurrencyId = CorePreparationVariables.ProfitCurrencyId,
                Rate = CorePreparationVariables.ProfitCurrencyRate,
                ProfitCurrencyExchangeRate = CorePreparationVariables.ProfitCurrencyRate,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
            };

            this.InitializeAmounts(entityPM);
       
            //newItem.ShipmentId = this.EntityPM.Id;
            //newItem.ShipmentNumber = this.EntityPM.ShipmentNumber;
            //newItem.CreateDate = DateTool.GetCurrentDateAsUtc();
            //newItem.UpdateDate = DateTool.GetCurrentDateAsUtc();

            return entityPM;
        }

        private void InitializeAmounts(ShipmentPayablePM entityPM)
        {
            // Old
            //ShipmentPayable.OpenAmount = ShipmentPayable.Quantity * ShipmentPayable.UnitPrice;
            //ShipmentPayable.OpenAmountInLocalCurrency = ShipmentPayable.OpenAmount * CorePreparationVariables.ProfitCurrencyRate;
            //ShipmentPayable.OpenAmountInProfitCurrency = ShipmentPayable.OpenAmountInLocalCurrency / CorePreparationVariables.ProfitCurrencyRate;


            if (entityPM.Quantity != null && entityPM.UnitPrice != null)
            {
                entityPM.ShipmentPayableLineStatusCode = "OAMT";
                entityPM.ExpectedAmount = entityPM.Quantity * entityPM.UnitPrice;
                entityPM.ExpectedAmountLocal = entityPM.ExpectedAmount * entityPM.Rate;
                entityPM.ExpectedAmountInProfitCurrency = entityPM.ExpectedAmountLocal / entityPM.ProfitCurrencyExchangeRate;
            }
        }
    }
}
