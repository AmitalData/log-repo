using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientSdk;

namespace Logitude.Server.Tools.ExternalServices
{
    public class SignManager 
    {


        public byte[] SignBytes(int tenant, byte[] bytesSerilazeObject)
        {
            byte[] bytesSignedSerilazeObject = null;

            var file = Guid.NewGuid().ToString();

            string ErrorOccurred = "";
            string MoreParamsOut = "";
            string Message = "";


            FactoryServicePool<ISignService>.Use(ServiceTypeEnum.Sign, tenant,
                prxy =>
                {
                    
                    
                    prxy.CreateSignedDoc(file + ".xml", bytesSerilazeObject, file + ".Signed", "MEHES", out bytesSignedSerilazeObject,
                                out ErrorOccurred, out MoreParamsOut, out Message);
                }
            );

            if (ErrorOccurred != "0")
            {
                throw new Exception(Message);
            }

            return bytesSignedSerilazeObject;
        }
#if false
        byte[] SignItOld(byte[] bytesSerilazeObject)
        {
            var sw = Stopwatch.StartNew();
            byte[] bytesSignedSerilazeObject = null;
            try
            {

            
                var file = Guid.NewGuid().ToString();

                string ErrorOccurred = "";
                string MoreParamsOut = "";
                string Message = "";
                var unifreightSignServiceAddress = @"http://itzik7:5058/Unifreight/SignService/basic";
                LogMessagingUtil.Instance.AppendLine("Start UnifreightSignServiceAddress =" + unifreightSignServiceAddress);
                using (var unifreightSignService = new UnifreightSignService(unifreightSignServiceAddress))
                {
                    var client = unifreightSignService.GetChannel<ISignService>();
                    client.CreateSignedDoc(file + ".xml", bytesSerilazeObject, file + ".Signed", "MEHES", out bytesSignedSerilazeObject,
                        out ErrorOccurred, out MoreParamsOut, out Message);
                }
                LogMessagingUtil.Instance.AppendLine("Sign took:" + sw.Elapsed.ToString());
            }
            catch (Exception ex)
            {
                var msg = "Sign service failed " + Environment.NewLine + ex.Message;
                var field = ex.GetType().GetField("_message", BindingFlags.Instance | BindingFlags.NonPublic);
                if (field != null)
                {
                    field.SetValue(ex, msg);
                }
                throw ex;

                //throw new Exception("Sign service failed ", e);
            }
            return bytesSignedSerilazeObject;

        }
#endif



    }
}
