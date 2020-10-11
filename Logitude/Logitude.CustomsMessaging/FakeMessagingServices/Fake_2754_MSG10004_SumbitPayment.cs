using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.RequestServices;
using System.Xml.Serialization;
using UnifreightIIG.Common.ImportDeclarationServiceReference;
using DF_NG_2754_MSG10004_ImportDeclarationResponse = UnifreightIIG.Common.ImportDeclarationSubmitRequestServiceReference.DF_NG_2754_MSG10004_ImportDeclarationResponse;

namespace Logitude.CustomsMessaging.FakeMessagingServices
{
    public class Fake_2754_MSG10004_SumbitPayment : Fake_ImportDeclaration_Response
    {
        public Fake_2754_MSG10004_SumbitPayment(GenericRequestParams requestParams) : base(requestParams) { }
    public ResponseHeader CallWS(out DF_NG_2754_MSG10004_ImportDeclarationResponse response, GenericRequestParams requestParams)
        {
            UpdateDeclaration(requestParams);
            UpdateStatus("5");
            response = cast(fakeRespond);
            UpdateFakeResponseContentHeader();
            AddSign();
            AddResponseHeader();
            response.DeclarationPaymentDetails=AddPaymentDetails_2754();
            response.ResponseContentHeader = new UnifreightIIG.Common.ImportDeclarationSubmitRequestServiceReference.ResponseContentHeader()
            { ApplicationID=0};

            return _ResponseHeader;

        }

        public DF_NG_2754_MSG10004_ImportDeclarationResponse cast(UnifreightIIG.Common.ImportDeclarationServiceReference.DF_NG_2754_MSG10004_ImportDeclarationResponse customResponse)
        {


            string DeclarationString;
            using (var stringwriter = new System.IO.StringWriter())
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

                //ns.Add("q", "http://malam.com/customs/DealFile/Declaration/DF_MSG10000_ImportDeclaration");
                var serializer = new XmlSerializer(customResponse.GetType());
                serializer.Serialize(stringwriter, customResponse, namespaces: ns);
                DeclarationString = stringwriter.ToString();
            }
            //DeclarationString = DeclarationString.Replace("xmlns:q", "xmlns");
            //DeclarationString = DeclarationString.Replace("<q:", "<");
            //DeclarationString = DeclarationString.Replace("</q:", "</");

            using (var stringReader = new System.IO.StringReader(DeclarationString))
            {
                var serializer = new XmlSerializer(typeof(DF_NG_2754_MSG10004_ImportDeclarationResponse));

                try
                {
                    return serializer.Deserialize(stringReader) as DF_NG_2754_MSG10004_ImportDeclarationResponse;


                }
                catch (System.Exception ex)
                {
                    return null;
                }

            }


        }


        public UnifreightIIG.Common.ImportDeclarationSubmitRequestServiceReference.DF_NG_2754_MSG10004_ImportDeclarationResponseDeclarationPaymentDetails AddPaymentDetails_2754( )
        {
            // fill the right info
            var paymentDetails = new UnifreightIIG.Common.ImportDeclarationSubmitRequestServiceReference.DF_NG_2754_MSG10004_ImportDeclarationResponseDeclarationPaymentDetails
            {
                PaymentOrderNumber = 455993853, // ???
                PaymentOrderStatus = 3
            };
            return paymentDetails;
        }
    }
}
