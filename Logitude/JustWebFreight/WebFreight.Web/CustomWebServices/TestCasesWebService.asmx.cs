using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;
using WebFreight.Web.CustomWebServices.BL;

namespace WebFreight.Web.CustomWebServices
{
    /// <summary>
    /// Summary description for TestCasesWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TestCasesWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public byte[] GetAllTestCases()
        {
            List<TestCase> testList = new List<TestCase>() 
            {
                #region Send Declaration
                new TestCase(){ IndexOrder=0, Code="Real Logic", Description="This case operates the real logic.",Type="webservice",Group="declaration1"},
                new TestCase(){ IndexOrder=1, Code="In Background", Description="This case to test how the UI responds to the successfully send of the declaration and that it will continue to work in background.",Type="webservice",Group="declaration1"},
                new TestCase(){ IndexOrder=2, Code="Send Succeeded", Description="This case to test how the UI responds to the successfull response of send declaration.",Type="webservice",Group="declaration1"},
                new TestCase(){ IndexOrder=3, Code="Send Failed", Description="This case to test how the UI responds to the failure response of send declaration.",Type="webservice",Group="declaration1"},
                 
                #endregion

                #region physical check 1
                new TestCase(){ IndexOrder=0, Code="Real Logic", Description="This case operates the real logic.",Type="webservice",Group="physicalcheck1"},
                new TestCase(){ IndexOrder=1, Code="Available Time List Succeeded", Description="This case to test how the UI responds to the successfull response of available time list.",Type="webservice",Group="physicalcheck1"},
                new TestCase(){ IndexOrder=2, Code="Available Time List Fail", Description="This case to test how the UI responds to the failure response of available time list.",Type="webservice",Group="physicalcheck1"},
                new TestCase(){ IndexOrder=3, Code="Choose Automatic Time", Description="This case to test how the UI responds to the successfull response of choosing Automatic time.",Type="customservice",Group="physicalcheck1"},
                new TestCase(){ IndexOrder=4, Code="Choose Automatic Time Fail", Description="This case to test how the UI responds to the failure response of choosing Automatic time.",Type="customservice",Group="physicalcheck1"},
                #endregion

                #region physical check 2
                new TestCase(){ IndexOrder=0, Code="Real Logic", Description="This case operates the real logic.",Type="webservice",Group="physicalcheck2"},
                new TestCase(){ IndexOrder=1, Code="Choose Specific Time", Description="This case to test how the UI responds to the successfull response of choosing specific time.",Type="webservice",Group="physicalcheck2"},
                new TestCase(){ IndexOrder=2, Code="Choose Specific Time Fail", Description="This case to test how the UI responds to the failure response of choosing specific time.",Type="webservice",Group="physicalcheck2"},
                #endregion

                #region decalaration payment
                new TestCase(){ IndexOrder=0, Code="Real Logic", Description="This case operates the real logic.",Type="webservice",Group="declarationpayment"},
                new TestCase(){ IndexOrder=1, Code="Send Succeeded", Description="This case to test how the UI responds to the successfull response of send declaration payment.",Type="webservice",Group="declarationpayment"},
                new TestCase(){ IndexOrder=2, Code="Send Failed", Description="This case to test how the UI responds to the failure response of send declarationpayment.",Type="webservice",Group="declarationpayment"},
                #endregion

                #region search vendor
                new TestCase(){ IndexOrder=0, Code="Real Logic", Description="This case operates the real logic.",Type="webservice",Group="searchvendor"},
                new TestCase(){ IndexOrder=1, Code="Return a list regarding search values", Description="This case to test how the UI responds to the successfull response of searching vendors regarding search values.",Type="webservice",Group="searchvendor"},
                new TestCase(){ IndexOrder=2, Code="Return a list regarding search values and different result count", Description="This case to test how the UI responds to the failure response of searching operation.",Type="webservice",Group="searchvendor"},
                new TestCase(){ IndexOrder=3, Code="Return a list neglecting search values", Description="This case to test how the UI responds to the successfull response of searching vendors neglecting search values.",Type="webservice",Group="searchvendor"},
                new TestCase(){ IndexOrder=4, Code="Return an empty list", Description="This case to test how the UI responds to the successfull response of returning an empty list.",Type="webservice",Group="searchvendor"},
                new TestCase(){ IndexOrder=5, Code="Search failed", Description="This case to test how the UI responds to the failure response of searching operation.",Type="webservice",Group="searchvendor"},
                #endregion

                #region new vendor
                new TestCase(){ IndexOrder=0, Code="Real Logic", Description="This case operates the real logic.",Type="webservice",Group="neweditvendor"},
                new TestCase(){ IndexOrder=1, Code="Send Succeeded", Description="This case to test how the UI responds to the successfull response of add edit vendor.",Type="webservice",Group="neweditvendor"},
                new TestCase(){ IndexOrder=2, Code="Send Failed", Description="This case to test how the UI responds to the failure response of add edit vendor.",Type="webservice",Group="neweditvendor"},
                  new TestCase(){ IndexOrder=3, Code="Send Succeeded With Warning", Description="This case returns a warning of an existing client and waits the response of the user.",Type="webservice",Group="neweditvendor"},
                #endregion

                #region new vendor communication
                new TestCase(){ IndexOrder=0, Code="Real Logic", Description="This case operates the real logic.",Type="webservice",Group="newvendorcommunication"},
                new TestCase(){ IndexOrder=1, Code="Send Succeeded", Description="This case to test how the UI responds to the successfull response of creating a new vendor communication.",Type="webservice",Group="newvendorcommunication"},
                new TestCase(){ IndexOrder=2, Code="Send Failed", Description="This case to test how the UI responds to the failure response of creating a new vendor communication.",Type="webservice",Group="newvendorcommunication"},
                #endregion

                #region search client
                new TestCase(){ IndexOrder=0, Code="Real Logic", Description="This case operates the real logic.",Type="webservice",Group="searchclient"},
                new TestCase(){ IndexOrder=1, Code="Send Succeeded", Description="This case to test how the UI responds to the successfull response of searching a client.",Type="webservice",Group="searchclient"},
                new TestCase(){ IndexOrder=2, Code="Send Succeeded With Can Continue", Description="This case to test how the UI responds to the successfull response of searching a client with the ability to build a new client.",Type="webservice",Group="searchclient"},
                new TestCase(){ IndexOrder=3, Code="Send Failed", Description="This case to test how the UI responds to the failure response of add edit client.",Type="webservice",Group="searchclient"},

                
                #endregion

                #region Send payment order
                new TestCase(){ IndexOrder=0, Code="Real Logic", Description="This case operates the real logic.",Type="webservice",Group="paymentorder"},
                new TestCase(){ IndexOrder=1, Code="Send Succeeded", Description="This case to test how the UI responds to the successfull response of send payment order.",Type="webservice",Group="paymentorder"},
                new TestCase(){ IndexOrder=2, Code="Send Failed", Description="This case to test how the UI responds to the failure response of send payment order.",Type="webservice",Group="paymentorder"},
                #endregion

                #region send declaration status
                new TestCase(){ IndexOrder=0, Code="Real Logic", Description="This case operates the real logic.",Type="webservice",Group="senddeclarationstatus"},
                new TestCase(){ IndexOrder=1, Code="Search Succeeded", Description="This case to test how the UI responds to the successfull response of search declaration status by number or cargo.",Type="webservice",Group="senddeclarationstatus"},
                new TestCase(){ IndexOrder=2, Code="Search Failed", Description="This case to test how the UI responds to the failure response of search declaration status by number or cargo.",Type="webservice",Group="senddeclarationstatus"},
                #endregion

                #region send declaration constriant
                new TestCase(){ IndexOrder=0, Code="Real Logic", Description="This case operates the real logic.",Type="webservice",Group="senddeclarationconstraint"},
                new TestCase(){ IndexOrder=1, Code="Send Succeeded", Description="This case to test how the UI responds to the successfull response of send declaration constraint.",Type="webservice",Group="senddeclarationconstraint"},
                new TestCase(){ IndexOrder=2, Code="Send Failed", Description="This case to test how the UI responds to the failure response of send declaration constraint.",Type="webservice",Group="senddeclarationconstraint"},
                #endregion

                #region send collateral Answers
                new TestCase(){ IndexOrder=0, Code="Real Logic", Description="This case operates the real logic.",Type="webservice",Group="sendcollateralanswers"},
                new TestCase(){ IndexOrder=1, Code="Send Succeeded", Description="This case to test how the UI responds to the successfull response of send collateral response.",Type="webservice",Group="sendcollateralanswers"},
                new TestCase(){ IndexOrder=2, Code="Send Failed", Description="This case to test how the UI responds to the failure response of send collateral response.",Type="webservice",Group="sendcollateralanswers"},
                #endregion

                #region New payment order
                new TestCase(){ IndexOrder=0, Code="Real Logic", Description="This case operates the real logic.",Type="webservice",Group="newpaymentorder"},
                new TestCase(){ IndexOrder=1, Code="Send Succeeded", Description="This case to test how the UI responds to the successfull response of send payment order.",Type="webservice",Group="newpaymentorder"},
                new TestCase(){ IndexOrder=2, Code="Send Failed", Description="This case to test how the UI responds to the failure response of send payment order.",Type="webservice",Group="newpaymentorder"},
                #endregion

                #region Send currency rate
                new TestCase(){ IndexOrder=0, Code="Real Logic", Description="This case operates the real logic.",Type="webservice",Group="sendcurrencyrate"},
                new TestCase(){ IndexOrder=1, Code="Send Succeeded", Description="This case to test how the UI responds to the successfull response of send currency rate.",Type="webservice",Group="sendcurrencyrate"},
                new TestCase(){ IndexOrder=2, Code="Send Failed", Description="This case to test how the UI responds to the failure response of send currency rate.",Type="webservice",Group="sendcurrencyrate"},
                #endregion

                #region Send new file request
                new TestCase(){ IndexOrder=0, Code="Real Logic", Description="This case operates the real logic.",Type="webservice",Group="sendrequestfile"},
                new TestCase(){ IndexOrder=1, Code="Send Succeeded", Description="This case to test how the UI responds to the successfull response of send  request file.",Type="webservice",Group="sendrequestfile"},
                new TestCase(){ IndexOrder=2, Code="Send Failed", Description="This case to test how the UI responds to the failure response of send  request file.",Type="webservice",Group="sendrequestfile"},
                #endregion

                #region Refresh Consignment
                new TestCase(){ IndexOrder=0, Code="Real Logic", Description="This case operates the real logic.",Type="webservice",Group="consignment"},
                new TestCase(){ IndexOrder=1, Code="Send Succeeded", Description="This case to test how the UI responds to the successfull response of refresh consignment.",Type="webservice",Group="consignment"},
                new TestCase(){ IndexOrder=2, Code="Send Failed", Description="This case to test how the UI responds to the failure response of refresh consignment.",Type="webservice",Group="consignment"},
                 
                #endregion

                #region Update vehicle
                new TestCase(){ IndexOrder=0, Code="Real Logic", Description="This case operates the real logic.",Type="webservice",Group="updateVehicle"},
                new TestCase(){ IndexOrder=1, Code="Send Succeeded", Description="This case to test how the UI responds to the successfull response of sending vehicle.",Type="webservice",Group="updateVehicle"},
                new TestCase(){ IndexOrder=2, Code="Send Failed", Description="This case to test how the UI responds to the failure response of sending  vehicle",Type="webservice",Group="updateVehicle"},

                
                #endregion

                #region send notification reply
                new TestCase(){ IndexOrder=0, Code="Real Logic", Description="This case operates the real logic.",Type="webservice",Group="sendnotification"},
                new TestCase(){ IndexOrder=1, Code="Send Succeeded", Description="This case to test how the UI responds to the successfull response of sending notification reply.",Type="webservice",Group="sendnotification"},
                new TestCase(){ IndexOrder=2, Code="Send Failed", Description="This case to test how the UI responds to the failure response of sending  notification reply",Type="webservice",Group="sendnotification"},

                
                #endregion

                #region new Vehicle
                new TestCase(){ IndexOrder=0, Code="Real Logic", Description="This case operates the real logic.",Type="webservice",Group="newVehicle"},
                new TestCase(){ IndexOrder=1, Code="Send Succeeded", Description="This case to test how the UI responds to the successfull response of sending vehicle.",Type="webservice",Group="newVehicle"},
                new TestCase(){ IndexOrder=2, Code="Send Failed", Description="This case to test how the UI responds to the failure response of sending  vehicle",Type="webservice",Group="newVehicle"},

                
                #endregion


                


            };


            TestCaseEnvelope envelope = new TestCaseEnvelope();
            envelope.TestCases = testList;
            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(TestCaseEnvelope));
            ser.Serialize(memstream, envelope);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
           
        }
    }
}
