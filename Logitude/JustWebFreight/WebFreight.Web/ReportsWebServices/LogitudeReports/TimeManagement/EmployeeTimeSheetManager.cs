using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.TimeManagement.Data;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.Data.Repositories;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.TimeManagement
{
    public class EmployeeTimeSheetManager
    {
        private int tenant;
        private DateTime? fromDate = null;
        private DateTime? toDate = null;
        private string employeeUsers = null;
        private int timeRequired = 9;
        public EmployeeTimeSheetManager(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;

            MemoryStream memoryStream = new MemoryStream(xmlFilters);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations myQueryOperations = (QueryOperations)xmlSerializer.Deserialize(memoryStream);

            QueryFilterItem filterItem_EmployeeUsers = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "Employees").FirstOrDefault();
            QueryFilterItem filterItem_TimeRequired = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "TimeRequired").FirstOrDefault();
            QueryFilterItem filterItem_FromDate = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();

            if (filterItem_FromDate != null)
            {
                if (filterItem_FromDate.FieldValue != null)
                {
                    fromDate = (DateTime)filterItem_FromDate.FieldValue;
                }
            }

            if (filterItem_ToDate != null)
            {
                if (filterItem_ToDate.FieldValue != null)
                {
                    toDate = (DateTime)filterItem_ToDate.FieldValue;
                }
            }

            if (filterItem_EmployeeUsers != null)
            {
                if (filterItem_EmployeeUsers.FieldValue != null)
                {
                    employeeUsers = filterItem_EmployeeUsers.FieldValue.ToString();
                }
            }

            if (filterItem_TimeRequired != null)
            {
                if (filterItem_TimeRequired.FieldValue != null)
                {
                    timeRequired = Convert.ToInt32(filterItem_TimeRequired.FieldValue);
                }
            }
        }

        public byte[] GetData()
        {
            EmployeeTimeSheetDataProvider myDataProvider = this.LoadDataProvider();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(EmployeeTimeSheetDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, myDataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);

            StreamReader streamReader = new StreamReader(memoryStream);
            string content = streamReader.ReadToEnd();
            byte[] bytearray = memoryStream.ToArray();
            return bytearray;
        }

        private EmployeeTimeSheetDataProvider LoadDataProvider()
        {
            EmployeeTimeSheetDataProvider myDataProvider = new EmployeeTimeSheetDataProvider()
            {
                EmployeeTimeSheetGroups = new List<EmployeeTimeSheetGroupData>(),
            };

            List<string> employeeUsersIdsList = this.GetSelectedUsersIds();
            Dictionary<string, string> employeeUserNames = this.GetEmployeeUserNames(employeeUsersIdsList);

            if (this.fromDate != null && this.toDate != null && employeeUsersIdsList != null && employeeUsersIdsList.Count > 0)
            {
                myDataProvider.FromDate = fromDate.Value;
                myDataProvider.ToDate = toDate.Value;
                myDataProvider.EmployeesUserNames = GetSelectedEmployeesNames(employeeUserNames);

                var dateList = Enumerable.Range(0, 1 + toDate.Value.Subtract(fromDate.Value).Days).Select(offset => fromDate.Value.AddDays(offset)).ToList();

                if (dateList.Count > 0)
                {
                    ITimeManagementContext iContext = TimeManagementContext.GetContext(tenant);

                    List<TMEmployeeTime> list_TMEmployeeTime =
                        (from a in iContext.TMEmployeeTimes
                         where
                         a.Tenant == tenant
                         && employeeUsersIdsList.Contains(a.EmployeeUserId)
                         && a.DateOfWork != null
                         && System.Data.Entity.DbFunctions.TruncateTime(a.DateOfWork) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate)
                         && System.Data.Entity.DbFunctions.TruncateTime(a.DateOfWork) <= System.Data.Entity.DbFunctions.TruncateTime(toDate)
                         select a).ToList();

                    List<TMOfficeHour> list_TMOfficeHour =
                        (from a in iContext.TMOfficeHours
                         where
                         !a.Inactive
                         && a.Tenant == tenant
                         && employeeUsersIdsList.Contains(a.UserId)
                         && a.WorkDate != null
                         && System.Data.Entity.DbFunctions.TruncateTime(a.WorkDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate)
                         && System.Data.Entity.DbFunctions.TruncateTime(a.WorkDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate)
                         select a).ToList();

                    var employeeUserGroups = from item in list_TMEmployeeTime
                                     group item by item.EmployeeUserId into g
                                     select new { EmployeeUserId = g.Key, Items = g };

                    foreach (var employeeItemGroup in employeeUserGroups)
                    {
                        EmployeeTimeSheetGroupData employeeTimeSheetGroup = new EmployeeTimeSheetGroupData();

                        employeeTimeSheetGroup.EmployeeTimeDaysOff = new List<EmployeeTimeDayOff>();
                        employeeTimeSheetGroup.EmployeeTimeSheetList = new List<EmployeeTimeSheetData>();
                        employeeTimeSheetGroup.EmployeeId = employeeItemGroup.EmployeeUserId;
                        employeeTimeSheetGroup.EmployeeName = employeeUserNames[employeeItemGroup.EmployeeUserId];

                        foreach (var dateItem in dateList)
                        {
                            EmployeeTimeSheetData itemRow = new EmployeeTimeSheetData()
                            {
                                EmployeeName = employeeTimeSheetGroup.EmployeeName,
                                DayOfWork = dateItem.ToString("dddd"),
                                DateOfWork = dateItem,
                                RequiredWorkHours = this.GetTimeFormatFromMinutes(timeRequired),
                                RequiredWorkMins = timeRequired,
                            };

                            if (itemRow.DayOfWork != null)
                            {
                                if (itemRow.DayOfWork.ToLower() == "friday" || itemRow.DayOfWork.ToLower() == "saturday")
                                {
                                    itemRow.RequiredWorkHours = "0";
                                    itemRow.RequiredWorkMins = 0;
                                }
                            }

                            List<TMOfficeHour> item_TMOfficeHour = list_TMOfficeHour.Where(a => a.WorkDate.Date == dateItem.Date && a.UserId == employeeItemGroup.EmployeeUserId).ToList();
                            List<TMEmployeeTime> item_TMEmployeeTime = employeeItemGroup.Items.Where(d => d.DateOfWork.Date == dateItem.Date).ToList();

                            double totalMinutesFromClock = 0;
                            foreach (var item in item_TMOfficeHour)
                            {
                                DateTime? entry = item.EntryTime != null ? item.EntryTime : item.RecordedEntryTime;
                                DateTime? exit = item.ExitTime != null ? item.ExitTime : item.RecordedExitTime;
                                if (entry != null && exit != null)
                                {
                                    totalMinutesFromClock += (exit.Value - entry.Value).TotalMinutes;
                                }
                            }

                            itemRow.MinutesFromClock = totalMinutesFromClock;
                            itemRow.MinutesFromOffice = item_TMEmployeeTime.Where(d => d.LocationCode == "O").Sum(s => s.TimeInMinutes);
                            itemRow.MinutesFromHome = item_TMEmployeeTime.Where(d => d.LocationCode == "H").Sum(s => s.TimeInMinutes);
                            itemRow.MinutesFromClient = item_TMEmployeeTime.Where(d => d.LocationCode == "C").Sum(s => s.TimeInMinutes);
                            itemRow.MinutesFromDayOff = item_TMEmployeeTime.Where(d => d.LocationCode == "D").Sum(s => s.TimeInMinutes);
                            itemRow.MinutesDifference = itemRow.MinutesFromOffice - itemRow.MinutesFromClock;
                            itemRow.MinutesTotalWork = itemRow.MinutesFromClock + itemRow.MinutesFromHome + itemRow.MinutesFromClient + itemRow.MinutesFromDayOff;
                            itemRow.MinutesOverTime = itemRow.MinutesTotalWork - (itemRow.RequiredWorkMins);

                            itemRow.TimeFromClock = this.GetTimeFormatFromMinutes(itemRow.MinutesFromClock);
                            itemRow.TimeFromOffice = this.GetTimeFormatFromMinutes(itemRow.MinutesFromOffice);
                            itemRow.TimeFromHome = this.GetTimeFormatFromMinutes(itemRow.MinutesFromHome);
                            itemRow.TimeFromClient = this.GetTimeFormatFromMinutes(itemRow.MinutesFromClient);
                            itemRow.TimeFromDayOff = this.GetTimeFormatFromMinutes(itemRow.MinutesFromDayOff);
                            itemRow.DifferenceTime = this.GetTimeFormatFromMinutes(itemRow.MinutesDifference);
                            itemRow.TotalWorkHrs = this.GetTimeFormatFromMinutes(itemRow.MinutesTotalWork);
                            itemRow.OverTime = this.GetTimeFormatFromMinutes(itemRow.MinutesOverTime);

                            employeeTimeSheetGroup.EmployeeTimeSheetList.Add(itemRow);
                        }

                        employeeTimeSheetGroup.Total_RequiredWorkHours = GetTimeFormatFromMinutes(employeeTimeSheetGroup.EmployeeTimeSheetList.Sum(a => a.RequiredWorkMins));
                        employeeTimeSheetGroup.Total_TimeFromClock = GetTimeFormatFromMinutes(employeeTimeSheetGroup.EmployeeTimeSheetList.Sum(a => a.MinutesFromClock));
                        employeeTimeSheetGroup.Total_TimeFromOffice = GetTimeFormatFromMinutes(employeeTimeSheetGroup.EmployeeTimeSheetList.Sum(a => a.MinutesFromOffice));
                        employeeTimeSheetGroup.Total_TimeFromHome = GetTimeFormatFromMinutes(employeeTimeSheetGroup.EmployeeTimeSheetList.Sum(a => a.MinutesFromHome));
                        employeeTimeSheetGroup.Total_TimeFromClient = GetTimeFormatFromMinutes(employeeTimeSheetGroup.EmployeeTimeSheetList.Sum(a => a.MinutesFromClient));
                        employeeTimeSheetGroup.Total_TimeFromDayOff = GetTimeFormatFromMinutes(employeeTimeSheetGroup.EmployeeTimeSheetList.Sum(a => a.MinutesFromDayOff));
                        employeeTimeSheetGroup.Total_DifferenceTime = GetTimeFormatFromMinutes(employeeTimeSheetGroup.EmployeeTimeSheetList.Sum(a => a.MinutesDifference));
                        employeeTimeSheetGroup.Total_TotalWorkHrs = GetTimeFormatFromMinutes(employeeTimeSheetGroup.EmployeeTimeSheetList.Sum(a => a.MinutesTotalWork));
                        employeeTimeSheetGroup.Total_OverTime = GetTimeFormatFromMinutes(employeeTimeSheetGroup.EmployeeTimeSheetList.Sum(a => a.MinutesOverTime));

                        employeeTimeSheetGroup.EmployeeTimeDaysOff = (from d in list_TMEmployeeTime
                                                                      where  d.EmployeeUserId == employeeTimeSheetGroup.EmployeeId 
                                                                      && d.LocationCode == "D"
                                                                      group d by d.ProjectId into g
                                                                      select new EmployeeTimeDayOff
                                                                      {
                                                                         ProjectId = g.Key,
                                                                         TimeInMinutes = g.Sum(s => s.TimeInMinutes),
                                                                      }).ToList();

                        foreach (EmployeeTimeDayOff item in employeeTimeSheetGroup.EmployeeTimeDaysOff)
                        {
                            item.TimeInHours = this.GetTimeFormatFromMinutes(item.TimeInMinutes);

                            if (item.ProjectId == null)
                            {
                                item.ProjectName = "No Project";
                            }

                            else
                            {
                                TMProject iProject = (from d in iContext.TMProjects where d.Id == item.ProjectId select d).FirstOrDefault();
                                if (iProject != null)
                                {
                                    item.ProjectName = iProject.Name;
                                }
                            }
                        }

                        myDataProvider.EmployeeTimeSheetGroups.Add(employeeTimeSheetGroup);
                    }

                }
            }

            return myDataProvider;
        }

        private string GetTimeFormatFromMinutes(double minutes)
        {
            string iResult = "";

            if (minutes != 0)
            {
                TimeSpan iTimeSpan = TimeSpan.FromMinutes(Math.Abs(minutes));

                iResult = (int)iTimeSpan.TotalHours + ":" + iTimeSpan.Minutes.ToString("00");

                if (minutes < 0)
                {
                    iResult = "- " + iResult;
                }
            }

            return iResult;
        }

        private List<string> GetSelectedUsersIds()
        {
            List<string> employeesUsersIdsList = new List<string>();
            if (!string.IsNullOrEmpty(employeeUsers))
            {
                employeeUsers = employeeUsers.Trim(',');
                employeesUsersIdsList = employeeUsers.Split(',').ToList() ;
            }

            return employeesUsersIdsList;
        }

        public Dictionary<string, string> GetEmployeeUserNames(List<string> employeeUsersIdsList)
        {
            Dictionary<string, string> employeeUserNames = new Dictionary<string, string>();
            ContactRepository contactRepository = new ContactRepository(tenant);
            IQueryable<Simplog.Data.CommonDataModel.EntityPOCOs.Contact> employeeContacts = contactRepository.GetContacts(employeeUsersIdsList, tenant);
            if (employeeContacts.ToList() == null)
            {
                return employeeUserNames;
            }
            foreach (var userContact in employeeContacts.ToList())
            {
                employeeUserNames.Add(userContact.Id, userContact.EnglishName);
            }
            return employeeUserNames;
        }

        private string GetSelectedEmployeesNames(Dictionary<string, string> employeeUserNames)
        {
            return string.Join(",",employeeUserNames.Values);
        }
    }
}