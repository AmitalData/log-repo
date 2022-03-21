using Profact.TimbraCFDI40;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.Tools.ExternalService.SATProfact40.Payment
{
    internal class Conceptos : SATComprobante
    {
        public static ComprobanteConcepto[] Get()
        {
            const decimal Quantity = 1;
            const string Description = "Pago";
            const decimal Amount = 0;
            const decimal UnitValue = 0;
            const string ProdServiceKey = "84111506";
            const string UnitKey = "ACT";
            const string ImpObject = "01"; //Not Include Tax

            List<ComprobanteConcepto> conceptosList = new List<ComprobanteConcepto>
            {
                new ComprobanteConcepto()
                {
                    Cantidad = Quantity,
                    Descripcion = Description,
                    Importe = Amount,
                    ValorUnitario = UnitValue,
                    ClaveProdServ = ProdServiceKey,
                    ClaveUnidad = UnitKey,
                    ObjetoImp = ImpObject,
                }
            };

            return conceptosList.ToArray();
        }
    }
}
