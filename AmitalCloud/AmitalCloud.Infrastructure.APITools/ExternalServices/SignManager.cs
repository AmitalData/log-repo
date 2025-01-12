//using API =AmitalCloud.Infrastructure.APITools.Interface;
using System;
using UnifreightIIG.Common.ClientSdk;
namespace AmitalCloud.Infrastructure.APITools.ExternalServices
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
    }
}
