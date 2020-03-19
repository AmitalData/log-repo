using System;
using System.Collections.Generic;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.Server.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.HybridTest.ServicesTest
{
    [TestClass]
    public class CommTaskTest
    {
        [TestMethod]
        public void Test_CommTask_()
        {
            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "InsertCommTask",
                ServiceOperation = "InsertTask",
                ServiceType = typeof(QueueTask),
            };
            QueueTask[] tasks = new QueueTask[1];
            tasks[0] = new QueueTask() {
                Action = "CommUpdate",
                Parameters = new List<Parameter>()
                {
                    new Parameter { Name = "1", Value = "1"},
                    new Parameter { Name = "2", Value = "2"},
                }
            };

            object[] serviceParameters = new object[] { EnvironmentGlobalParams.MainTenant, EnvironmentGlobalParams.SecondaryTenant, 1, "CommUpdate", tasks };
            ServiceOutcome serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);
            Assert.IsFalse(serviceOutcome.Response.HasError, "Insert CommTask Failed! " + serviceOutcome.Response.ErrorMessage);
            Assert.IsNotNull(serviceOutcome.Response.Result, "Insert CommTask Failed! " + serviceOutcome.Response.Result);

            serviceProperties = new InvokedProperties
            {
                ServiceName = "ExternalTasksQueue",
                ServiceOperation = "GetTaskFromQueue",
                SecondaryToken = EnvironmentGlobalParams.SecondaryTenantToken,
            };

            serviceParameters = new object[] { EnvironmentGlobalParams.SecondaryTenant, 1 };
            serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);

            Assert.IsNotNull(serviceOutcome.Result, "Get Task From Queue Failed! ");
            string[] communicationLogId = serviceOutcome.Result.ToString().Split(new string[] { "CommunicationLogId=\"" }, StringSplitOptions.None);
            communicationLogId = communicationLogId[1].Split('\"');

            Assert.IsNotNull(communicationLogId[0], "Get Task From Queue Failed! ");
            //serviceProperties = new InvokedProperties
            //{
            //    ServiceName = "ExternalTasksQueue",
            //    ServiceOperation = "MarkTaskAsDone",
            //    SecondaryToken = EnvironmentGlobalParams.SecondaryTenantToken,
            //};

            //serviceParameters = new object[] { communicationLogId[0], EnvironmentGlobalParams.SecondaryTenant, 1 };
            //serviceOutcome = WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters);


        }        
    }
}