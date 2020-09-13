using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.Customs.PerformanceLogger
{
    public class PerformanceQueue
    {
        static PerformanceQueue _Instance;

        public static PerformanceQueue Instance
        {
            get { return PerformanceQueue._Instance = PerformanceQueue._Instance ?? new PerformanceQueue(); }

        }

        List<PerformanceM> _MyQueuePerformanceM;
       

        object MyObj = new Hashtable();
        
      PerformanceQueue()
        {
            
            _MyQueuePerformanceM = new List<PerformanceM>();
            _LastTimeWriteCSV = DateTime.Now;

        }
        DateTime _LastTimeWriteCSV;
        public void Add(PerformanceM newQue)
        {

           


            lock ((this._MyQueuePerformanceM as ICollection).SyncRoot)
            {
               
               
                _MyQueuePerformanceM.Add(newQue);
                if (DateTime.Now.Subtract(_LastTimeWriteCSV) > TimeSpan.FromMinutes(5))
                {
                    _LastTimeWriteCSV = DateTime.Now;
                    WriteCSV();
                    _MyQueuePerformanceM.Clear();
                }
            }

            
        }

        private void WriteCSV()
        {
            try
            {


                string PerformanceLoggerCSVPath = ConfigurationManager.AppSettings.Get("PerformanceLoggerCSVPath");

                if (!string.IsNullOrWhiteSpace(PerformanceLoggerCSVPath))
                {
                    String lastDBResponseTime=string.Empty;
                    var res = _MyQueuePerformanceM.Select(r =>
                    {
                        if (!String.IsNullOrWhiteSpace(r.DBResponseTime))
                        {
                            lastDBResponseTime = r.DBResponseTime;
                        }
                        var line = new StringBuilder();
                        line
                        .Append(r.ServerName).Append("~")
                        .Append(r.PID).Append("~")
                        .Append(r.ThreadId).Append("~")
                        .Append(r.QueueDefinitionCode).Append("~")
                        .Append(r.InterfaceTypeCode).Append("~")
                        //.Append(r.RequestCreateDate).Append("~")
                        .Append(r.RequestStartDate).Append("~")
                        .Append(r.RequestEndDate).Append("~")
                        .Append(r.RequestDiff).Append("~")
                        .Append(r.QueueStartDate).Append("~")
                        .Append(r.QueueEndDate).Append("~")
                        .Append(r.QueueDiff).Append("~")
                        .Append(r.ServerCPU).Append("~")
                        .Append(r.RequestSheetID).Append("~")
                        .Append(lastDBResponseTime).Append("~")//.Append(r.DBResponseTime).Append("~")
                        //.Append(r.QueueReceiveDate).Append("~")
                        .Append(r.QueueSuccessComplete).Append("~");

                        line.Replace('"', " "[0]);
                        line.Replace(',', " "[0]);
                        line.Replace(Environment.NewLine, "");
                        line.Replace('~', ',');
                        return line.ToString();
                    }
                    );
                    int dayPart =DateTime.Now.Hour / 4;
                    string fileName = DateTime.Today.Year + "." + DateTime.Today.Month + "." + DateTime.Today.Day + "." + dayPart;
                    string fullPath = Path.Combine(PerformanceLoggerCSVPath, "PerformanceLogger" + fileName + ".csv");
                    if (!File.Exists(fullPath))
                    {
                        var hdr = new StringBuilder();
                        hdr
                       .Append("ServerName").Append(",")
                       .Append("PID").Append(",")
                       .Append("ThreadId").Append(",")
                       .Append("QueueDefinitionCode").Append(",")
                       .Append("InterfaceTypeCode").Append(",")
                       //.Append("RequestCreateDate").Append(",")
                       .Append("RequestStartDate").Append(",")
                       .Append("RequestEndDate").Append(",")
                       .Append("RequestDiff").Append(",")
                       .Append("QueueStartDate").Append(",")
                       .Append("QueueEndDate").Append(",")
                       .Append("QueueDiff").Append(",")
                       .Append("ServerCPU").Append(",")
                       .Append("RequestSheetID").Append(",")
                       .Append("DBResponseTime").Append(",")
                       //.Append("QueueReceiveDate").Append(",")
                       .Append("QueueSuccessComplete").AppendLine();


                        File.AppendAllText(fullPath, hdr.ToString());
                    }
                    File.AppendAllLines(fullPath, res);
                    
                }
            }
            catch (Exception)
            {

                //throw;
            }
        }



        public bool Remove(int tenant, String CustomsRequestsSheetId)
        {
            //StartAt = DateTime.Now;
            lock ((this._MyQueuePerformanceM as ICollection).SyncRoot)
            {
                
            }
            return false;
        }




    }
}
