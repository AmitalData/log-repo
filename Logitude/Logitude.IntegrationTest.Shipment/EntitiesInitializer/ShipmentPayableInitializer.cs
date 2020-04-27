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
                CurrencyId = args.CurrencyId,
                Rate = args.CurrencyRate,
                ProfitCurrencyExchangeRate = args.ProfitCurrencyRate,

                Tenant = IntegrationTestLoginParameters.Tenant,
                CreatedByUserId = IntegrationTestLoginParameters.LoginUserId,
                UpdateByUserId = IntegrationTestLoginParameters.LoginUserId,
                ShipmentPayableLineStatusCode = "EMPT",
                ShipmentPayableAmountTypeCode = "ACCU",
                ShipmentPayableAmountTypeName = "Accrual",
                ChargesTypeId = ShipmentVariables.ChargeTypeAFTId,
                MeasurementId = ShipmentVariables.MeasurmentGRWTId,
                VatTypeId = ShipmentVariables.VATTypeZeroId,
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
            if (entityPM.Quantity != null && entityPM.UnitPrice != null)
            {
                entityPM.ShipmentPayableLineStatusCode = "OAMT";
                entityPM.ExpectedAmount = entityPM.Quantity * entityPM.UnitPrice;
                entityPM.ExpectedAmountLocal = entityPM.ExpectedAmount * entityPM.Rate;
                entityPM.ExpectedAmountInProfitCurrency = entityPM.ExpectedAmountLocal / entityPM.ProfitCurrencyExchangeRate;

                entityPM.OpenAmount = entityPM.ExpectedAmount;
                entityPM.OpenAmountInLocalCurrency = entityPM.ExpectedAmountLocal;
                entityPM.OpenAmountInProfitCurrency = entityPM.ExpectedAmountInProfitCurrency;
            }
        }
    }
}
