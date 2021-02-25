using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.BuildTenant.MumpsOpenReconcile
{
    public class OpenRecoDataValidation
    {
        public StringBuilder Errors = new StringBuilder();
        public List<MMPSDataM> GoodRows = new List<MMPSDataM>();
        public void GetDataAndValid()
        {
            var m = new MumpsData();
            //var sb  =new StringBuilder();
            var list =m.GetData().Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (var line in list)
            {
                var fields=line.Split(',').ToList();
                if (fields.Count!= 7)
                {
                    Errors.AppendLine($"error 7 field {line}");
                    continue;
                }
                try
                {
                    GoodRows.Add(new MMPSDataM()
                    {
                        InternalNumber = fields[0],
                        ExternalNo = fields[1],
                        MyDate = fields[2],
                        Ref1 = fields[3],
                        Ref2 = fields[4],
                        OriginAmount = decimal.Parse(fields[5]),
                        OpenAmount = decimal.Parse(fields[6]),


                    });
                }
                catch (Exception)
                {

                    Errors.AppendLine($"error decimal  {line}");
                }
                

            }

        }
    }
    public class MMPSDataM
    {
        public string InternalNumber { get; internal set; }
        public string ExternalNo { get; internal set; }
        public string MyDate { get; internal set; }
        public string Ref1 { get; internal set; }
        public string Ref2 { get; internal set; }
        public decimal OriginAmount { get; internal set; }
        public decimal OpenAmount { get; internal set; }
    }
}
