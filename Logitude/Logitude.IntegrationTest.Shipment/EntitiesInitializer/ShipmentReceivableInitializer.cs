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
    public class ShipmentReceivableInitializer : IEntityInitializer
    {
        public object Create(EntityInitializerArguments args)
        {
            ShipmentReceivablePM entityPM = new ShipmentReceivablePM()
            {
                Quantity = args.ReceivableQuantity,
                UnitPrice = args.ReceivableUnitPrice,
                CurrencyId = args.CurrencyId,
                Rate = args.CurrencyRate,
                ProfitCurrencyExchangeRate = args.ProfitCurrencyRate,

                Tenant = IntegrationTestLoginParameters.Tenant,
                CreatedByUserId = IntegrationTestLoginParameters.LoginUserId,
                UpdateByUserId = IntegrationTestLoginParameters.LoginUserId,
                ShipmentReceivableLineStatusCode = "EMPT",
                ChargesTypeId = ShipmentVariables.ChargeTypeAFTId,
                MeasurementId = ShipmentVariables.MeasurmentGRWTId,
                VatTypeId = ShipmentVariables.VATTypeZeroId,
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
            };

            this.InitializeAmounts(entityPM);

            return entityPM;
        }

        private void InitializeAmounts(ShipmentReceivablePM entityPM)
        {
            if (entityPM.Quantity != null && entityPM.UnitPrice != null)
            {
                entityPM.ShipmentReceivableLineStatusCode = "OAMT";
                entityPM.TotalAmount = entityPM.Quantity * entityPM.UnitPrice;
                entityPM.TotalAmountLocal = entityPM.TotalAmount * entityPM.Rate;
                entityPM.AmountInProfitCurrency = entityPM.TotalAmountLocal / entityPM.ProfitCurrencyExchangeRate;
            }
        }
    }
}
