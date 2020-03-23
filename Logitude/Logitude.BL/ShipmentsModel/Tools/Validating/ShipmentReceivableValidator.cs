using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;

namespace Logitude.BL.ShipmentsModel.Tools.Validating
{
    public class ShipmentReceivableValidator
    {
        internal static void Validate(List<ShipmentReceivablePM> shipmentReceivables)
        {
            foreach(ShipmentReceivablePM item in shipmentReceivables)
            {
                ValidateEntityAmount(item);
                ValidateEntityLocalAmount(item);
                ValidateEntityProfitAmount(item);
            }
        }

        private static void ValidateEntityAmount(ShipmentReceivablePM item)
        {
            double? entityAmount = MethodHelper.Round(item.TotalAmount, 2);
            double? shouldBeAmount = MethodHelper.Round(item.Quantity * item.UnitPrice, 2);

            if (entityAmount != shouldBeAmount)
            {
                throw new ApplicationException("Invalid Shipment Receivable Amount");
            }
        }

        private static void ValidateEntityLocalAmount(ShipmentReceivablePM item)
        {
            double? entityAmount = MethodHelper.Round(item.TotalAmountLocal, 2);
            double? shouldBeAmount = MethodHelper.Round(item.TotalAmount * item.Rate, 2);

           
            if (entityAmount != shouldBeAmount)
            {
                throw new ApplicationException("Invalid Shipment Receivable Total Amount");
            }
        }

        private static void ValidateEntityProfitAmount(ShipmentReceivablePM item)
        {
            double? entityAmount = MethodHelper.Round(item.AmountInProfitCurrency, 2);
            double? shouldBeAmount = MethodHelper.Round(item.TotalAmountLocal / item.ProfitCurrencyExchangeRate, 2);

            if (entityAmount != shouldBeAmount)
            {
                throw new ApplicationException("Invalid Shipment Receivable Total Amount");
            }
        }
    }
}
