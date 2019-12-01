using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel.Description;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.HybridTest.WcfCallers;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class ContactTest
    {
        [TestMethod]
        public void Test_Contact_UPSERT()
        {
            ContactPM contactPM = new ContactPM()
            {
                EnglishName = HybridData.ContactCode,
                LocalName = "Hybrid Contact",
                Email = "HybridContact@logitudeworld.com",
                Password = "!H0",
                ExternalId = HybridData.ContactCode,
                Tenant = TestEnvironmentGlobalParameters.Tenant1,
            };
            Response serviceResponse = EntityWcfCaller.CallEntityUpsert(contactPM);
            Assert.IsFalse(serviceResponse.HasError, "Upsert Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNotNull(serviceResponse.Result, "Upsert Failed! " + serviceResponse.ErrorMessage);
            HybridData.ContactId = serviceResponse.Result;
        }

        [TestMethod]
        public void Test_Contact_GetContactPMByEmail()
        {
            if(HybridData.ContactId == null)
                Test_Contact_UPSERT();
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Contact",
                ServiceOperation = "GetContactPMByEmail",
                ServiceResponseIndex = 2,
                ServiceType = typeof(ContactPM),
                ServiceFilterType = null,
            };
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { "HybridContact@logitudeworld.com", TestEnvironmentGlobalParameters.Tenant1, serviceResponse };
            ContactPM contact = (ContactPM)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Contact PM By Email Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Contact PM By Email Failed! " + serviceResponse.Result);
            Assert.AreEqual(contact.EnglishName, HybridData.ContactCode, "Get Hybrid Contact From Contacts Failed!");
        }

        [TestMethod]
        public void Test_Contact_GetContactList()
        {
            if (HybridData.ContactId == null)
                Test_Contact_UPSERT();
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Contact",
                ServiceOperation = "GetContactList",
                ServiceResponseIndex = 2,
                ServiceType = typeof(ContactList),
                ServiceFilterType = typeof(ContactServiceReference.ContactApiFilters),
            };
            ContactServiceReference.ContactApiFilters filters = new ContactServiceReference.ContactApiFilters
            {
                Take = 10,
                ByCode = true,
                SearchFields = HybridData.ContactCode
            };
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { filters, TestEnvironmentGlobalParameters.Tenant1, serviceResponse };
            ContactList[] contacts = (ContactList[])WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get List Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get List Failed! " + serviceResponse.Result);
            Assert.AreEqual(contacts[0].EnglishName, HybridData.ContactCode, "Get Hybrid Contact From Contacts Failed!");
        }

        [TestMethod]
        public void Test_Contact_GetContactByExternalId()
        {
            if (HybridData.ContactId == null)
                Test_Contact_UPSERT();
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Contact",
                ServiceOperation = "GetContactByExternalId",
                ServiceResponseIndex = 2,
                ServiceType = typeof(ContactPM),
                ServiceFilterType = null,
            };
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { HybridData.ContactCode, TestEnvironmentGlobalParameters.Tenant1, serviceResponse };
            ContactPM contact = (ContactPM)WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            Assert.IsFalse(serviceResponse.HasError, "Get Contact By External Id Failed! " + serviceResponse.ErrorMessage);
            Assert.IsNull(serviceResponse.Result, "Get Contact By External Id Failed! " + serviceResponse.Result);
            Assert.AreEqual(contact.EnglishName, HybridData.ContactCode, "Get Hybrid Contact From Contacts Failed!");
        }
    }
}
