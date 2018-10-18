using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WebFreight.Web.CustomWebServices.Contracts
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IGatewayService" in both code and config file together.
    [ServiceContract()]
    public interface IGatewayService
    {
        [OperationContract]
        string GetState();

        [OperationContract]
        void GetAllAssemblyQualifiedName(
                  out string AllAssemblyQualifiedName,
                  out string AllAssemblyDescription,
                  ref string MoreParams,
                  out string MessageOut
                  );


        [OperationContract]
        void GetExampleProcessRequest(
                  string AssemblyQualifiedName,
                  out string DataIn1,
                  out string DataIn2,
                  out string DataOut1,
                  out string DataOut2,
                  ref string MoreParams,
                  out string MessageOut
                  );




        [OperationContract]
        void ProccessBASE64Request(
                  string AssemblyQualifiedName,
                  string BASE64DataIn1,
                  string BASE64DataIn2,
            string BASE64DataIn3,
                  out string BASE64DataOut1,
                  out string BASE64DataOut2,
            out string BASE64DataOut3,
                  out string SUCCESS,
                  ref string MoreParams,
                  out string MessageOut
                  );
        [OperationContract]
        void ProccessRequest(
                  string AssemblyQualifiedName,
                  string DataIn1,
                  string DataIn2,
                  out string DataOut1,
                  out string DataOut2,
                  out string SUCCESS,
                  ref string MoreParams,
                  out string MessageOut
                  );
    }
}
