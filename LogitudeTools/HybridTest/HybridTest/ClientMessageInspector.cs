using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace HypredTest
{
    public class ClientMessageInspector : IClientMessageInspector
    {
        /// <summary>
        /// Enables inspection or modification of a message before a request message is sent to a service.
        /// </summary>
        /// <param name="request">The message to be sent to the service.</param>
        /// <param name="channel">The WCF client object channel.</param>
        /// <returns>
        /// The object that is returned as the <paramref name="correlationState " /> argument of
        /// the <see cref="M:System.ServiceModel.Dispatcher.IClientMessageInspector.AfterReceiveReply(System.ServiceModel.Channels.Message@,System.Object)" /> method.
        /// This is null if no correlation state is used.The best practice is to make this a <see cref="T:System.Guid" /> to ensure that no two
        /// <paramref name="correlationState" /> objects are the same.
        /// </returns>
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            HttpRequestMessageProperty httpRequestMessage;
            object httpRequestMessageObject;
            //if (request.Properties.TryGetValue(HttpRequestMessageProperty.Name, out httpRequestMessageObject))
            //{
            //    httpRequestMessage = httpRequestMessageObject as HttpRequestMessageProperty;
            //    if (string.IsNullOrEmpty(httpRequestMessage.Headers["User-Agent"]))
            //    {
            //        httpRequestMessage.Headers["User-Agent"] = Form1.Token;
            //    }
            //}
            //else
            //{
                httpRequestMessage = new HttpRequestMessageProperty();
                httpRequestMessage.Headers.Add("Token", Form1.Token);
                request.Headers.Add(MessageHeader.CreateHeader("Token", "", Form1.Token));
                //request.Properties..Add(HttpRequestMessageProperty.Name, httpRequestMessage);
            //}
            return null;
            //HttpRequestMessageProperty property = new HttpRequestMessageProperty();

            //property.Headers["User-Agent"] = "12121";
            //request.Properties.Add(HttpRequestMessageProperty.Name, property);

            //return null;
        }

        /// <summary>
        /// Enables inspection or modification of a message after a reply message is received but prior to passing it back to the client application.
        /// </summary>
        /// <param name="reply">The message to be transformed into types and handed back to the client application.</param>
        /// <param name="correlationState">Correlation state data.</param>
        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            // Nothing special here
        }
    }
}
