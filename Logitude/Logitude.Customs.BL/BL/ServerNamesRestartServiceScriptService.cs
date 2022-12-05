using Logitude.Customs.Data.Repsitories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.BL
{
    public class ServerNamesRestartServiceScriptService
    {
        public string GetScript()
        {
            string template= @"
cls 
sc \\iigtest  stop AmitalCustomsWindowsServiceCustomsEXP
rem timeout 60 
ping 127.0.0.1 -n 10
rem enshure all services stoped
sc \\iigtest  stop AmitalCustomsWindowsServiceCustomsEXP
ping 127.0.0.1 -n 60 
rem Starting....
sc \\iigtest start AmitalCustomsWindowsServiceCustomsEXP
rem enshure all services started  
ping 127.0.0.1 -n 10 
sc \\iigtest start AmitalCustomsWindowsServiceCustomsEXP
pause
";

            StringBuilder stringBuilder  = new StringBuilder();
            try
            {
                var serversNameRepository = new ServersNameRepository(0);
                var list = serversNameRepository.All().ToList();//NOCACHE!!fast!!  
                if (!list.Any())
                {
                    return "REM serversName  not defined ";
                }
                var validServices = list
                    .Where(r => !String.IsNullOrWhiteSpace(r.ServerName))
                    .Where(r => !String.IsNullOrWhiteSpace(r.ServiceName))
                    .OrderBy(r => r.ServiceName).ThenBy(r=>r.ServiceName)
                    .ToList();
                ;
                if (list.Count != validServices.Count)
                {
                    stringBuilder.AppendLine($"REM Only {validServices.Count} valid validService  from  {list.Count()} records");
                }

                if (!validServices.Any())
                {
                    return stringBuilder.ToString();
                }


                stringBuilder.AppendLine("cls");

                var stopStartEnum = StopStartEnum.stop;
                stringBuilder.AppendLine("rem stop services....");
                SC_StopStart(stringBuilder, validServices, stopStartEnum);

                int Seconds = 15;
                WaitSeconds(stringBuilder, Seconds);

                stringBuilder.AppendLine("rem reenshure all services stoped");
                stopStartEnum = StopStartEnum.stop;
                SC_StopStart(stringBuilder, validServices, stopStartEnum);
                
                Seconds = 15;
                WaitSeconds(stringBuilder, Seconds);


                stopStartEnum = StopStartEnum.start;
                stringBuilder.AppendLine("rem Starting...");
                SC_StopStart(stringBuilder, validServices, stopStartEnum);


                Seconds = 10;
                WaitSeconds(stringBuilder, Seconds);


                stopStartEnum = StopStartEnum.start;
                stringBuilder.AppendLine("rem enshure all services started  ");
                SC_StopStart(stringBuilder, validServices, stopStartEnum);
                stringBuilder.AppendLine("pause");
                stringBuilder.AppendLine();

            }
            catch (Exception e )
            {

                stringBuilder.Clear();
                stringBuilder.AppendLine(e.ToString());
            }
            return stringBuilder.ToString();





        }


        private static void WaitSeconds(StringBuilder stringBuilder, int sec)
        {
            stringBuilder.AppendLine($"ping 127.0.0.1 -n {sec}");
        }

        private static void SC_StopStart(StringBuilder stringBuilder, List<Data.EntityPOCOs.ServersName> validServices, StopStartEnum stopStartEnum)
        {
            foreach (var item in validServices)
            {
                stringBuilder
                    .AppendLine($"sc \\\\{item.ServerName}  {stopStartEnum} {item.ServiceName} ");
            }
        }
    }
    enum StopStartEnum
    {
        stop,
        start
    }
}
