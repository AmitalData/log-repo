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
                .WithItems(GetUpdateItems(dataTable, timeManagementAPIHelper.Items))
                .Build();
        }



        private List<TMEmployeeTimePM> GetUpdateItemPMs(dynamic dataTable, List<TMEmployeeTimePM> itemsPM)
        {
            var item = itemsPM.Where(e=>e.Id == TimeManagementData.DataEntryID).First();
            var updatedItem = new TMEmployeeTimePMBuilder().WithModel(item)
                .LocationCode((string)dataTable.Location)
                .Description((string)dataTable.Description)
                .TimeInMinutes((int)dataTable.Minuts)
                .DateOfWork(DateTime.Now.AddHours(-1))
                .ProjectId(TimeManagementData.UpdateProjectId)
                .sprintId(TimeManagementData.UpdateSprintId)
                .Build();
            return new List<TMEmployeeTimePM>() { updatedItem };
        }
        private object GetUpdateItems(dynamic dataTable, List<TimeSheetItem> items)
        {
            var item = items.First();
            var updatedItem = new TimeSheetItemBuilder().WithModel(item)
                .LocationCode((string)dataTable.Location)
                .Description((string)dataTable.Description)
                .ProjectId(TimeManagementData.UpdateProjectId)
                .Days(GetDays((int)dataTable.Minuts, DateTime.Now.AddHours(-1)))
                .Build();
            return new List<TimeSheetItem>() { updatedItem };
        }

        private List<TimeSheetItemDay> GetDays(int minuts, DateTime dateTime)
        {
            return new List<TimeSheetItemDay>() { 
                new TimeSheetItemDay(){ Date = dateTime, Minuts = minuts}
            };
        }

        public void Assert(TimeManagementAPIHelper dataEntry, TimeManagementAPIHelper updatedDataEntry)
        {
            updatedDataEntry.LocationCode.Should().Be(dataEntry.LocationCode);
            var updatedItem = updatedDataEntry.ItemsPM.First();
            var item = dataEntry.ItemsPM.First();
            updatedItem.LocationCode.Should().Be(item.LocationCode);
            updatedItem.Description.Should().Be(item.Description);
            updatedItem.SprintId.Should().Be(item.SprintId);
            updatedItem.ProjectId.Should().Be(item.ProjectId);
            updatedItem.DateOfWork.Should().Be(item.DateOfWork);
            updatedItem.TimeInMinutes.Should().Be(item.TimeInMinutes);
        }
    }
}
