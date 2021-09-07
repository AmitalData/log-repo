using FluentAssertions;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using Logitude.TimeManagementTests.Models;
using Logitude.TimeManagementTests.Models.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.TimeManagementTests.Services
{
    public class DataEntryServices
    {

        public TimeManagementAPIHelper CreateInstance(Table table)
        {
            dynamic dataTable = table.CreateDynamicInstance();
            return new TimeManagementAPIHelperBuilder()
                .WithDefualtValues()
                .LocationCode((string)dataTable.Location)
                .ItemsPM(GetTMEmployeeTime(dataTable))
                .Build();
        }
        private TMEmployeeTimePM GetTMEmployeeTime(dynamic dataTable)
        {
            return new TMEmployeeTimePMBuilder().WithDefualtValues()
                .LocationCode((string)dataTable.Location)
                .Description((string)dataTable.Description)
                .TimeInMinutes((int)dataTable.Minuts)
                .DateOfWork(DateTime.Now)
                .Build();
        }
        public TimeManagementAPIHelper UpdateInstance(Table table, TimeManagementAPIHelper timeManagementAPIHelper)
        {
            dynamic dataTable = table.CreateDynamicInstance();
            return new TimeManagementAPIHelperBuilder()
                .WithModel(timeManagementAPIHelper)
                .LocationCode((string)dataTable.Location)
                .WithItemsPM(GetUpdateItemPMs(dataTable, timeManagementAPIHelper.ItemsPM))
                .Build();
        }

        public TimeManagementAPIHelper Update(TimeManagementAPIHelper dataEntry)
        {
            var putDataEntry = APICaller.CallPut<TimeManagementAPIHelper>(dataEntry, Urls.TimeManagementDomainController, UserTenant.Token).Data;
            string path = Urls.GetDataEntryTimeSheetList(UserTenant.UserId, "A", DateTime.Now.AddDays(TimeManagementData.UpdatedDateNumber), DateTime.Now.AddDays(TimeManagementData.UpdatedDateNumber));
            var updatedDataEntry = APICaller.CallGet<TimeManagementAPIHelper>(path, UserTenant.Token).Data;
            return updatedDataEntry;
        }

        private List<TMEmployeeTimePM> GetUpdateItemPMs(dynamic dataTable, List<TMEmployeeTimePM> itemsPM)
        {
            var item = itemsPM.Where(e=>e.Id == TimeManagementData.DataEntryID).First();
            var updatedItem = new TMEmployeeTimePMBuilder().WithModel(item)
                .LocationCode((string)dataTable.Location)
                .Description((string)dataTable.Description)
                .TimeInMinutes((int)dataTable.Minuts)
                .DateOfWork(DateTime.Now.AddDays(TimeManagementData.UpdatedDateNumber))
                .ProjectId(TimeManagementData.UpdateProjectId)
                .sprintId(TimeManagementData.UpdateSprintId)
                .Build();
            return itemsPM;
        }


        public void Assert(TimeManagementAPIHelper dataEntry, TimeManagementAPIHelper updatedDataEntry)
        {
            var updatedItem = updatedDataEntry.ItemsPM.Where(e=>e.Id == TimeManagementData.DataEntryID).First();
            var item = dataEntry.ItemsPM.Where(e => e.Id == TimeManagementData.DataEntryID).First();
            updatedItem.LocationCode.Should().Be(item.LocationCode);
            updatedItem.Description.Should().Be(item.Description);
            updatedItem.SprintId.Should().Be(item.SprintId);
            updatedItem.ProjectId.Should().Be(item.ProjectId);
            //updatedItem.DateOfWork.Should().Be(item.DateOfWork);
            updatedItem.TimeInMinutes.Should().Be(item.TimeInMinutes);
        }
    }
}
