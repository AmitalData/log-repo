using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityQueryServices;

namespace Logitude.Customs.BL.BL
{
    internal class CourierSchedulerService
    {
        private StringBuilder _stringBuilder;

        public DateTime? SendImmediate(int tenant, string declarationId, DateTime? date)
        {
            _stringBuilder = new StringBuilder();
            // if date === null  => SendImmediate
            if (date == null )
            {
                _stringBuilder.AppendLine("CourierSchedulerService: already send imm");
            }
            else
            {


                if (SendImmediateDueTimeRange(tenant))//Task 164013: בלדרות- תזמון העלאת מסמכים
                {
                    date = null;
                }
                else
                {
                    if (!String.IsNullOrWhiteSpace(declarationId))
                    {

                        if (SendImmediateDueArrivalDateB4Today(declarationId, tenant))
                        {
                            date = null;
                        }
                    }
                }

            }
            
            LogMessagingUtil.Instance.AppendLine(_stringBuilder.ToString());
            return date;
        }

        private bool SendImmediateDueArrivalDateB4Today(string declarationId, int tenant)
        {
            
            bool sendImmediate = false;
            try
            {

                var defValue = GDFDATAQueryService.GetDefault(tenant, "ISRAEL", "CGO_IMDOC", "NON", "NON");
                _stringBuilder.Append("|").Append("SendImmediateDueArrivalDateB4Today CGO_IMDOC = {defValue} ");
                if (defValue == "Y")
                {

                    var courierMasterQueryService = new CourierMasterQueryService(tenant);
                    var courierMasterPM = courierMasterQueryService.GetByDeclarationId(declarationId, tenant);
                    _stringBuilder.Append("|").Append($"declaration:{declarationId},courierMasterPM?.EstimatedArrivalDate =={courierMasterPM?.EstimatedArrivalDate} ");
                    if (courierMasterPM?.EstimatedArrivalDate != null)
                    {
                        ///CGO_IMDOC - שליחה מיידית של מסמכים עם תאריך הגעה קטן מהיום
                        if (DateTime.Now >= courierMasterPM?.EstimatedArrivalDate)
                        {

                            _stringBuilder.Append("|").Append("שליחה מיידית של מסמכים עם תאריך הגעה קטן מהיום!!!");
                            return true;
                        }
                    }
                }
            

            }
            finally
            {
                
                

            }

            return sendImmediate;


        }

        private bool SendImmediateDueTimeRange(int tenant)
        {


            ///CGO_TIMDOC - שליחה מידית בטווח שעות

            string defValue = GDFDATAQueryService.GetDefault(tenant, "ISRAEL", "CGO_TIMDOC", "NON", "NON");
            _stringBuilder.Append("|").Append("SendImmediateDueTimeRange.CGO_TIMDOC = {defValue} ");
            if (String.IsNullOrEmpty(defValue))
            {
                return false;
            }

            var fromTo = defValue.Split('-');
            if (fromTo.Length != 2)
            {
                _stringBuilder.Append("|").Append("SendImmediateDueTimeRange CGO_TIMDOC BAD Pattren!!! should be  '03:00-08:00' ");
                return false;
            }
            DateTime dateTimeStart;
            if (DateTime.TryParseExact(fromTo[0], "t", null, System.Globalization.DateTimeStyles.None, out dateTimeStart))
            {
                _stringBuilder.Append("|").Append("SendImmediateDueTimeRange CGO_TIMDOC bad pattren  !!! should be  '03:00-08:00' ");
                return false;

            }
            DateTime dateTimeEnd;
            if (DateTime.TryParseExact(fromTo[1], "t", null, System.Globalization.DateTimeStyles.None, out dateTimeEnd))
            {
                _stringBuilder.Append("|").Append("SendImmediateDueTimeRange CGO_TIMDOC bad pattren  !!! should be  '03:00-08:00' ");
                return false;

            }
            if (dateTimeStart < DateTime.Now && DateTime.Now < dateTimeEnd)
            {
                _stringBuilder.Append("|").Append($" שליחה מידית בטווח שעות    {dateTimeStart} < now:{DateTime.Now} < {dateTimeEnd} ");
                return true;
            }
            else
            {
                
                return false;
            }



        }

    }
}
