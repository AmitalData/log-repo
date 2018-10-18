using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simplog.Server.Infrastructure.Helpers;
using System.Collections.Generic;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.UnitTest.Utils;
using FakeItEasy;


namespace Logitude.UnitTest.Accounting.UniTests
{
    [TestClass]
    public class AccountingSettingResolverUnitTest
    {


        [TestInitialize]
        public void MyTestInitialize()
        {
            _Tenant = 1;
            _myDate = new DateTime(2016, 10, 20);
        }

        
        [TestMethod]
        public void ResolveVat_VatTypePercentagePM18_ReturnVAT1p18()
        {
            var percentagesQuery = new List<VatTypePercentagePM>()
                {
                    new  VatTypePercentagePM(){ Tenant=_Tenant , FromDate=_myDate.AddDays(-1)  , Percentage=18}
                     
                };
            
            

            var fake = A.Fake<AccountingSettingResolver>(opt => opt.CallsBaseMethods());
            A.CallTo(() => fake.GetAccountingVatList(_Tenant)).
                Returns(percentagesQuery);
            var vat = fake.ResolveVat(1, _myDate);
            Assert.AreEqual(vat, 1.18m);
        }


        

        //[Ignore("there is a problem with this test")]
        //[MethodName]_[StateUnderTest]_[ExpectedBehavior].
        //throw new Exception("No Vat definition for Document Date " + documentDate.ToShortDateString());
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ResolveVat_GetGoodValue_ThrowExceptionNoVatdefinitionforDocumentDate()
        {

            //CustomsWorkerRole.CustomsWorkerEntryPoint.StartStatic(false);
            

            
            var documentDateWithoutVat = _myDate.AddDays(-100);

            var percentagesQuery = new List<VatTypePercentagePM>()
                {
                    new  VatTypePercentagePM(){ Tenant=_Tenant , FromDate=_myDate.AddDays(-1)  , Percentage=18}
                     
                };
            var fake = A.Fake<AccountingSettingResolver>(opt => opt.CallsBaseMethods());
            A.CallTo(() => fake.GetAccountingVatList(_Tenant)).
                Returns(percentagesQuery);
            var vat = fake.ResolveVat(1, documentDateWithoutVat);
            Assert.AreEqual(vat, 1.18);
        }

     

       

        static int _Tenant { get; set; }

        static DateTime _myDate { get; set; }
    }
}
