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
        private string employeeUserId = null;
        private double? timeRequired = 9;
        public EmployeeTimeSheetManager(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;

            MemoryStream memoryStream = new MemoryStream(xmlFilters);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations myQueryOperations = (QueryOperations)xmlSerializer.Deserialize(memoryStream);

            QueryFilterItem filterItem_EmployeeUserId = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "EmployeeUserId").FirstOrDefault();
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

            if (filterItem_EmployeeUserId != null)
            {
                if (filterItem_EmployeeUserId.FieldValue != null)
                {
                    employeeUserId = filterItem_EmployeeUserId.FieldValue.ToString();
                }
            }

            if (filterItem_TimeRequired != null)
            {
                if (filterItem_TimeRequired.FieldValue != null)
                {
                    timeRequired = Convert.ToDouble(filterItem_TimeRequired.FieldValue);
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
                EmployeeTimeSheetList = new List<EmployeeTimeSheetData>()
            };

            if(this.fromDate != null && this.toDate != null && this.employeeUserId != null)
            {
                myDataProvider.FromDate = fromDate.Value;
                myDataProvider.ToDate = toDate.Value;
                myDataProvider.EmployeeUserId = employeeUserId;

                Simplog.Data.CommonDataModel.EntityPOCOs.Contact iContact = ContactRepository.GetSingleContact(employeeUserId, tenant, true);
                if(iContact != null)
                {
                    myDataProvider.EmployeeUserName = iContact.EnglishName;
                }

                var dateList = Enumerable.Range(0, 1 + toDate.Value.Subtract(fromDate.Value).Days).Select(offset => fromDate.Value.AddDays(offset)).ToList();

                if (dateList.Count > 0)
                {
                    ITimeManagementContext iContext = TimeManagementContext.GetContext(tenant);

                    List<TMEmployeeTime> list_TMEmployeeTime =
                        (from a in iContext.TMEmployeeTimes
                         where
                         a.Tenant == tenant
                         && a.EmployeeUserId == employeeUserId
                         && a.DateOfWork != null
                         && System.Data.Entity.DbFunctions.TruncateTime(a.DateOfWork) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate)
                         && System.Data.Entity.DbFunctions.TruncateTime(a.DateOfWork) <= System.Data.Entity.DbFunctions.TruncateTime(toDate)
                         select a).ToList();

                    List<TMOfficeHour> list_TMOfficeHour =
                        (from a in iContext.TMOfficeHours
                         where
                         !a.Inactive
                         && a.Tenant == tenant
                         && a.UserId == employeeUserId
                         && a.WorkDate != null
                         && System.Data.Entity.DbFunctions.TruncateTime(a.WorkDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate)
                         && System.Data.Entity.DbFunctions.TruncateTime(a.WorkDate) <= System.Data.Entity.DbFunctions.TruncateTime(toDate)
                         select a).ToList();

                    foreach (var dateItem in dateList)
                    {
                        EmployeeTimeSheetData itemRow = new EmployeeTimeSheetData()
                        {
                            EmployeeName = myDataProvider.EmployeeUserName,
                            DayOfWork = dateItem.ToString("dddd"),
                            DateOfWork = dateItem,
                            RequiredWorkHours = timeRequired
                        };

                        if (itemRow.DayOfWork != null)
                        {
                            if (itemRow.DayOfWork.ToLower() == "friday" || itemRow.DayOfWork.ToLower() == "saturday")
                            {
                                itemRow.RequiredWorkHours = 0;
                            }
                        }

                        List<TMOfficeHour> item_TMOfficeHour = list_TMOfficeHour.Where(a => a.WorkDate.Date == dateItem.Date && a.UserId == employeeUserId).ToList();
                        List<TMEmployeeTime> item_TMEmployeeTime = list_TMEmployeeTime.Where(d => d.DateOfWork.Date == dateItem.Date && d.EmployeeUserId == employeeUserId).ToList();

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
                        itemRow.MinutesDifference = itemRow.MinutesFromOffice - itemRow.MinutesFromClock;
                        itemRow.MinutesTotalWork = itemRow.MinutesFromOffice + itemRow.MinutesFromHome + itemRow.MinutesFromClient;
                        itemRow.MinutesOverTime = itemRow.MinutesTotalWork - (itemRow.RequiredWorkHours.Value * 60);

                        itemRow.TimeFromClock = this.GetTimeFormatFromMinutes(itemRow.MinutesFromClock);
                        itemRow.TimeFromOffice = this.GetTimeFormatFromMinutes(itemRow.MinutesFromOffice);
                        itemRow.TimeFromHome = this.GetTimeFormatFromMinutes(itemRow.MinutesFromHome);
                        itemRow.TimeFromClient = this.GetTimeFormatFromMinutes(itemRow.MinutesFromClient);
                        itemRow.DifferenceTime = this.GetTimeFormatFromMinutes(itemRow.MinutesDifference);
                        itemRow.TotalWorkHrs = this.GetTimeFormatFromMinutes(itemRow.MinutesTotalWork);
                        itemRow.OverTime = this.GetTimeFormatFromMinutes(itemRow.MinutesOverTime);

                        myDataProvider.EmployeeTimeSheetList.Add(itemRow);
                    }
                }
            }

            myDataProvider.Total_RequiredWorkHours = myDataProvider.EmployeeTimeSheetList.Sum(a => a.RequiredWorkHours);
            myDataProvider.Total_TimeFromClock = GetTimeFormatFromMinutes(myDataProvider.EmployeeTimeSheetList.Sum(a => a.MinutesFromClock));
            myDataProvider.Total_TimeFromOffice = GetTimeFormatFromMinutes(myDataProvider.EmployeeTimeSheetList.Sum(a => a.MinutesFromOffice));
            myDataProvider.Total_TimeFromHome = GetTimeFormatFromMinutes(myDataProvider.EmployeeTimeSheetList.Sum(a => a.MinutesFromHome));
            myDataProvider.Total_TimeFromClient = GetTimeFormatFromMinutes(myDataProvider.EmployeeTimeSheetList.Sum(a => a.MinutesFromClient));
            myDataProvider.Total_DifferenceTime = GetTimeFormatFromMinutes(myDataProvider.EmployeeTimeSheetList.Sum(a => a.MinutesDifference));
            myDataProvider.Total_TotalWorkHrs = GetTimeFormatFromMinutes(myDataProvider.EmployeeTimeSheetList.Sum(a => a.MinutesTotalWork));
            myDataProvider.Total_OverTime = GetTimeFormatFromMinutes(myDataProvider.EmployeeTimeSheetList.Sum(a => a.MinutesOverTime));

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
    }
}