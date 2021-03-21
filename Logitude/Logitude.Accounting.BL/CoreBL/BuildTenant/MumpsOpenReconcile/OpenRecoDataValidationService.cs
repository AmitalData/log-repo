using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.BuildTenant.MumpsOpenReconcile
{
    public class OpenRecoDataValidationService
    {
        public StringBuilder Errors = new StringBuilder();
        public List<MMPSDataM> GoodRows = new List<MMPSDataM>();
        public HashSet<string> BadAccounts = new HashSet<string>();

        public void GetDataAndValid(string csvText)
        { 

            //var sb  =new StringBuilder();
            var list = csvText.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (var line in list)
            {
                var fields=line.Split(',').ToList();
                if (fields.Count!= 8)
                {
                    if (fields.Count > 0)
                    {
                        BadAccounts.Add(fields[0]);
                    }
                    LogIt($"error 8 field {line}");
                    continue;
                }
                try
                {
                    if (string.IsNullOrWhiteSpace(fields[6]))
                    {
                        LogIt($"error OriginAmount is null field {line}");
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(fields[7]))
                    {
                        LogIt($"error OpenAmount is null field {line}");
                        continue;
                    }
                    

                    var newRow = new MMPSDataM(line)
                    {
                        InternalNumber = fields[0],
                        ExternalNo = fields[1],
                        JLineNumber = int.Parse(fields[2]),
                        MyDate = fields[3],
                        Ref1 = fields[4],
                        Ref2 = fields[5],
                        OriginAmount = decimal.Parse(fields[6]),
                        OpenAmount = decimal.Parse(fields[7]),


                    };
                    if (string.IsNullOrWhiteSpace(newRow.InternalNumber))
                    {
                        LogIt($"error InternalNumber is null field {line}");
                        
                        continue;
                    }

                    if (newRow.JLineNumber<1)
                    {
                        LogIt($"JLineNumber is null field {line}");

                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(newRow.ExternalNo))
                    {
                        LogIt($"error ExternalNo is null field {line}");
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(newRow.Ref1))
                    {
                        //LogIt($"warning Ref1 is null field {line}");
                        //continue;
                    }
                    newRow.Ref1 = newRow.Ref1.Replace('~', ',');
                    newRow.Ref2 = newRow.Ref2.Replace('~', ',');
                    if (newRow.OriginAmount>=0)
                    {
                        if (newRow.OpenAmount<0)
                        {
                            LogIt($"error OriginAmount>=0 but newRow.OpenAmount<0 {line}");
                            continue;

                        }
                        if (newRow.OriginAmount <newRow.OpenAmount )
                        {
                            LogIt($"error OriginAmount>=0 but newRow.OriginAmount <newRow.OpenAmount {line}");
                            continue;

                        }

                    }
                    else//OriginAmount < 0
                    {
                        
                        if (newRow.OpenAmount >= 0)
                        {
                            LogIt($"error OriginAmount<0 but newRow.OpenAmount > 0{line}");
                            continue;

                        }
                        if ( newRow.OpenAmount< newRow.OriginAmount)
                        {
                            LogIt($"error OriginAmount>=0 but newRow.OpenAmount< newRow.OriginAmount {line}");
                            continue;

                        }
                    }
                    GoodRows.Add(newRow);

                }
                catch (Exception)
                {

                    LogIt($"error decimal  {line}");
                }
                

            }

        }

        private void LogIt(string mess)
        {
            LogMessagingUtil.Instance.AppendLine(mess);
            
        }
    }
    public class MMPSDataM
    {
        public string TheLine
        { get; internal set; }

        public MMPSDataM(string line)
        {
            this.TheLine = line;
        }

        public string InternalNumber { get; internal set; }
        public string ExternalNo { get; internal set; }
        public string MyDate { get; internal set; }
        public string Ref1 { get; internal set; }
        public string Ref2 { get; internal set; }
        public decimal OriginAmount { get; internal set; }
        public decimal OpenAmount { get; internal set; }


        public string RecordValidation { get; internal set; }
        public int JLineNumber { get; internal set; }
    }
}
