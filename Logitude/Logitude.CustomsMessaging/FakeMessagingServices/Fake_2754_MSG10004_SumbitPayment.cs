using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.RequestServices;
using UnifreightIIG.Common.ImportDeclarationServiceReference;
using DF_NG_2754_MSG10004_ImportDeclarationResponse = UnifreightIIG.Common.ImportDeclarationServiceReference.DF_NG_2754_MSG10004_ImportDeclarationResponse;

namespace Logitude.CustomsMessaging.FakeMessagingServices
{
    public class Fake_2754_MSG10004_SumbitPayment : Fake_ImportDeclaration_Response
    {
        public Fake_2754_MSG10004_SumbitPayment(GenericRequestParams requestParams) : base(requestParams) { }
    public ResponseHeader CallWS(out DF_NG_2754_MSG10004_ImportDeclarationResponse response)
        {
            UpdateDeclaration();
            UpdateStatus("5");
            response = fakeRespond;
            UpdateFakeResponseContentHeader();
            AddSign();
            AddResponseHeader();
            response.DeclarationPaymentDetails=AddPaymentDetails_2754();
            return _ResponseHeader;

        }
        public DF_NG_2754_MSG10004_ImportDeclarationResponseDeclarationPaymentDetails AddPaymentDetails_2754( )
        {
            // fill the right info
            var paymentDetails = new DF_NG_2754_MSG10004_ImportDeclarationResponseDeclarationPaymentDetails
            {
                PaymentOrderNumber = 455993853, // ???
                PaymentOrderStatus = 3
            };
            return paymentDetails;
        }
    }
}
