using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer.DBWCO
{
    public class LogitudePointerModel
    {
        public LogitudePointerModel(int Key, string WCOID, WCOErrorPointerModel.LogitudeEntityEnum LogitudeEntityID, string LogitudeFieldID)
        {
            this.Key =Key ;
            this.WCOID = WCOID;
            this.LogitudeEntity = LogitudeEntityID;
            this.LogitudeFieldID = LogitudeFieldID;
        }
        LogitudePointerModel()
        {

        }
        public int Key { get; set; } // A=1
        public string WCOID { get; set; }//E=5

        public WCOErrorPointerModel.LogitudeEntityEnum LogitudeEntity { get; set; } 
        public string LogitudeFieldID { get; set; } 
        

    }
}
