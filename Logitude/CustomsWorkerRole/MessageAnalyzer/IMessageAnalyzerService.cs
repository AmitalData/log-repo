using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomsWorkerRole.MessageAnalyzer
{
    public interface IMessageAnalyzerService
    {
        
        //void LoadXML(string fileContents);
        //void Validate();
        string Xml { get; set; }
        Logitude.CustomsMessaging.ResponseData.INF_MSG_GenericResponseData ResponseData { get; }

        void Analyze(string dcaResponseMessage);

        
        //string ExceptionMessage { get; set; }

        //int Tenant { get; set; }
        int ResolveTenant();

        string GetObjectTableName();
    }
}
