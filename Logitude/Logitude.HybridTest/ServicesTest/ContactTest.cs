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
        private static EntityWcfCaller entityWcfCaller = new EntityWcfCaller();
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
                Tenant = EnvironmentGlobalParams.MainTenant,
            };
            ServiceOutcome serviceOutcome = entityWcfCaller.CallEntityUpsert(contactPM);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNotNull(serviceOutcome.Response.Result, "Upsert Failed! " + serviceOutcome.Response.ErrorMessage);
        }

        [TestMethod]
        public void Test_Contact_GetContactPMByEmail()
        {
            //Test_Contact_UPSERT();
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Contact",
                ServiceOperation = "GetContactPMByEmail",
                ServiceResponseIndex = 2,
                ServiceType = typeof(ContactPM),
                ServiceFilterType = null,
            };
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { "HybridContact@logitudeworld.com", EnvironmentGlobalParams.MainTenant, serviceResponse };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

            ContactPM contact = (ContactPM)serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get Contact PM By Email Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get Contact PM By Email Failed! " + serviceOutcome.Response.Result);
            Assert.AreEqual(contact.EnglishName, HybridData.ContactCode, "Get Hybrid Contact From Contacts Failed!");
        }

        [TestMethod]
        public void Test_Contact_GetContactList()
        {
            //Test_Contact_UPSERT();
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
            object[] serviceParameters = new object[] { filters, EnvironmentGlobalParams.MainTenant, serviceResponse };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

            ContactList[] contacts = (ContactList[])serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get List Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get List Failed! " + serviceOutcome.Response.Result);
            Assert.AreEqual(contacts[0].EnglishName, HybridData.ContactCode, "Get Hybrid Contact From Contacts Failed!");
        }

        [TestMethod]
        public void Test_Contact_GetContactByExternalId()
        {
            //Test_Contact_UPSERT();
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "Contact",
                ServiceOperation = "GetContactByExternalId",
                ServiceResponseIndex = 2,
                ServiceType = typeof(ContactPM),
                ServiceFilterType = null,
            };
            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { HybridData.ContactCode, EnvironmentGlobalParams.MainTenant, serviceResponse };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

            ContactPM contact = (ContactPM)serviceOutcome.Result;
            Assert.IsFalse(serviceOutcome.Response.HasError, "Get Contact By External Id Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNull(serviceOutcome.Response.Result, "Get Contact By External Id Failed! " + serviceOutcome.Response.Result);
            Assert.AreEqual(contact.EnglishName, HybridData.ContactCode, "Get Hybrid Contact From Contacts Failed!");
        }
    }
}
