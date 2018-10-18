using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Models
{
    //[DataContract]
    public class AmitalStandardCommunicationModel
    {
        public AmitalStandardCommunicationModel(OperationMethod UnifaceMethodType, string UnifaceComponentName, string UnifaceOperation)
        {
            this.UnifaceMethodType = UnifaceMethodType;
            this.UnifaceComponentName = UnifaceComponentName;
            this.UnifaceOperation = UnifaceOperation;
            if (this.UnifaceMethodType == OperationMethod.AnalyzeStandard)
            {
                if (string.IsNullOrWhiteSpace(this.UnifaceComponentName) && string.IsNullOrWhiteSpace(this.UnifaceOperation))
                {
                    //good
                }
                else
                {
                    //never mind
                }

            }
            else
            {
                if (string.IsNullOrWhiteSpace(this.UnifaceComponentName) )
                {
                    throw new  ArgumentNullException("UnifaceComponentName"); 
                }
                if(string.IsNullOrWhiteSpace(this.UnifaceOperation))
                {
                    throw new ArgumentNullException("UnifaceOperation"); 
                }
            }

        }
        AmitalStandardCommunicationModel()
        {

        }
        public enum OperationMethod :int
        {
            AnalyzeStandard=0,//default
            DataAccess=1
        }

        

        public OperationMethod UnifaceMethodType { get; private set; }
        public string UnifaceComponentName { get; private set; }
        public string UnifaceOperation { get; private  set; }


        public string communicationLogId { get; set; }
        

    }
}
