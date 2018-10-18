using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Logitude.CustomsMessaging.Helpers
{
    public static class CRSShrinkBlobStepsUtil
    {
        public const bool TO_SHRINK = true;

        static void ShrinkSignCustomRequestSignedByte(byte[] customRequestSignedByteArry, out byte[] ShrinkcustomRequestSignedByteArry)
        {
            /*
<?xml version="1.0" encoding="utf-8"?><gov.il:SignedRoot version="1.0.0" xmlns:gov.il="http://www.gov.il/xmldigsig/v_1_0_0"><gov.il:SigningAppInfo><gov.il:ApplicationName>Sign and Verify</gov.il:ApplicationName><gov.il:ApplicationVersion>2.0.0</gov.il:ApplicationVersion></gov.il:SigningAppInfo><gov.il:SignedObject Id="il-0caa22d7-67ec-439f-b231-5a37821b8732" MimeType="text/plain"><gov.il:SignedInfo Id="il-169c2d24-d34e-44cb-ae19-602f84b1377a"><gov.il:Data MimeType="text/plain" DataEncodingType="base64">XXXXXXXXXX
             */
            ShrinkcustomRequestSignedByteArry = null;
            var xDoc = XDocument.Parse(Encoding.UTF8.GetString(customRequestSignedByteArry));
            var Data = xDoc.Descendants().FirstOrDefault(ele => ele.Name.LocalName.Contains("Data"));
            if (Data != null)
            {
                Data.SetValue("Data has cleared AFTER SEND (ShrinkSignCustomRequestSignedByte)");
                //Data.Parent.SetElementValue("Data", "Data has cleared AFTER SEND (ShrinkSignCustomRequestSignedByte)");
                using (var stream = new MemoryStream())
                {
                    xDoc.Save(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    ShrinkcustomRequestSignedByteArry = stream.ToArray();
                }
            }
            else
            {

            }

        }
        public static void Try2ShrinkCustomResponseBlobFile<TCustomsResponse>(
            TCustomsResponse customsRequest, Action<TCustomsResponse> ShrinkCustomResponse, IUpdateBolb customsRequestsSheetServiceUpdateBolb)
            where TCustomsResponse : class, new()
        {

            if (ShrinkCustomResponse != null)
            {
                var sw = Stopwatch.StartNew();

                customsRequestsSheetServiceUpdateBolb.UpdateBolb(CustomsStepEnum.ReceivedCustomResponseCorrelation,
                    (memCustomsResponse) =>
                    {
                        if (customsRequest == null)
                        {
                            customsRequest = XmlGenericUtil<TCustomsResponse>.DeSerializeObject(Encoding.UTF8.GetString(memCustomsResponse.ToArray()));
                        }
                        //

                        ShrinkCustomResponse(customsRequest);
                        var memcustomsRequestShrink =
        XmlGenericUtil<TCustomsResponse>.MemoryStreamSerializeWithDefaultNamespace(customsRequest);
                        return memcustomsRequestShrink;
                    });

                LogMessagingUtil.Instance.AppendLine("Shrink:CustomRequest:took:" + sw.Elapsed.ToString());
            }
        }
        public static void Try2ShrinkCustomRequestBlobFile<TCustomsRequest>(
            TCustomsRequest customsRequest, Action<TCustomsRequest> ShrinkCustomRequest, IUpdateBolb customsRequestsSheetServiceUpdateBolb)
            where TCustomsRequest : class ,new()
        {
            if (!TO_SHRINK)
            {
                return;
            }
            //var ShrinkCustomRequest = this._RequestService.GetActionShrinkCustomRequest();
            if (ShrinkCustomRequest != null)
            {
                var sw = Stopwatch.StartNew();

                customsRequestsSheetServiceUpdateBolb.UpdateBolb(CustomsStepEnum.CustomRequest,
                    (memCustomsRequest) =>
                    {
                        if (customsRequest == null)
                        {
                            customsRequest = XmlGenericUtil<TCustomsRequest>.DeSerializeObject(Encoding.UTF8.GetString(memCustomsRequest.ToArray()));
                        }
                        //

                        ShrinkCustomRequest(customsRequest);
                        var memcustomsRequestShrink =
        XmlGenericUtil<TCustomsRequest>.MemoryStreamSerializeWithDefaultNamespace(customsRequest);
                        return memcustomsRequestShrink;
                    });

                LogMessagingUtil.Instance.AppendLine("Shrink:CustomRequest:took:" + sw.Elapsed.ToString());
            }
        }


        public static void Try2ShrinkCustomRequestSignBlobFile(IUpdateBolb customsRequestsSheetServiceUpdateBolb)
        {
            if (!TO_SHRINK)
            {
                return;
            }
            
            var sw = Stopwatch.StartNew();

            customsRequestsSheetServiceUpdateBolb.UpdateBolb(CustomsStepEnum.CustomRequestSign,
                (memCustomsRequest) =>
                {
                    byte[] ShrinkcustomRequestSignedByteArry = null;
                    //customsRequest = XmlGenericUtil<TCustomsRequest>.DeSerializeObject(customsRequestXml);
                    var customRequestSignedByteArry = memCustomsRequest.ToArray();
                    CRSShrinkBlobStepsUtil.ShrinkSignCustomRequestSignedByte(customRequestSignedByteArry, out ShrinkcustomRequestSignedByteArry);

                    return new MemoryStream(ShrinkcustomRequestSignedByteArry);
                });

            LogMessagingUtil.Instance.AppendLine("Shrink:CustomRequestSign:took:" + sw.Elapsed.ToString());

        }
    }
}
