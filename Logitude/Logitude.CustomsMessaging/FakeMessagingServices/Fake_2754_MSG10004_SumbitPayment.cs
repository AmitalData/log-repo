using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.RequestServices;
using System;
using UnifreightIIG.Common.ImportDeclarationServiceReference;

namespace Logitude.CustomsMessaging.FakeMessagingServices
{
    public class Fake_2754_MSG10004_SumbitPayment : Fake_ImportDeclaration_Response
    {
        public ResponseHeader _header;
        public Declaration dec;

        public Fake_2754_MSG10004_SumbitPayment(GenericRequestParams requestParams) : base(requestParams) { }
    public ResponseHeader CallWS(out DF_NG_2754_MSG10004_ImportDeclarationResponse response, GenericRequestParams requestParams)
        {
            _header = new ResponseHeader();
            response = new DF_NG_2754_MSG10004_ImportDeclarationResponse();
            UpdateDeclaration(requestParams);
            AddResponseHeader();
            dec = new Declaration();
            CastDeclaration();
            response.Response = new Response
            {
                Declaration = dec
            };
            ResponseStatus status = new ResponseStatus
            {
                NameCode = new StatusNameCodeType() { Value = "5" },
                EffectiveDateTime = DateTime.Now.ToString()
            };
            response.Response.Status = status;
            response.DeclarationPaymentDetails = new DF_NG_2754_MSG10004_ImportDeclarationResponseDeclarationPaymentDetails()
            {
                PaymentOrderNumber= 455993853,
                PaymentOrderStatus = 3
            };
            _header.CorrelationId = Guid.NewGuid().ToString();
            _header.ExternalId = Guid.NewGuid().ToString();
            _header.Status = "Success";
            _header.ErrorDescription = "";
            _header.ErrorCode = "None";
            //response.DeclarationPaymentDetails=AddPaymentDetails_2754();
            return _header;

        }
        public DF_NG_2754_MSG10004_ImportDeclarationResponse AddPaymentDetails_2754( )
        {
            // fill the right info
            var paymentDetails = new DF_NG_2754_MSG10004_ImportDeclarationResponse
            {
              //  PaymentOrderNumber = 455993853, // ???
                //PaymentOrderStatus = 3
            };
            return paymentDetails;
        }
    }
}
