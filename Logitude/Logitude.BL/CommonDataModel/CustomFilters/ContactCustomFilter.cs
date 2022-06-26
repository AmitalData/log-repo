using System;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;

namespace Logitude.BL.CommonDataModel.CustomFilters
{
    public class ContactCustomFilter
    {
        int tenant;       
        public ContactCustomFilter(int tenant)
        {
            this.tenant = tenant;
        }

        public IQueryable<Contact> GetFilteredQuery(QueryOperations operations, IQueryable<Contact> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "ContactCustomFilter")
                    {
                        string value = item.FieldValue as string;

                        if (queryableData.Count() != 0)
                        {
                            if (value == "SearchText")
                            {
                                queryableData = queryableData.Where(d => !string.IsNullOrEmpty(d.Email));
                            }

                            else
                            {
                                queryableData = queryableData.Where(d =>
                                    (!string.IsNullOrEmpty(d.Email)
                                    &&
                                    (d.Email.ToUpper().Contains(value.ToUpper()) || d.EnglishName.ToUpper().StartsWith(value.ToUpper())))
                                    );

                            }
                        }
                    }

                    if (item.FieldName == "CardId" || item.FieldName == "CardCode")
                    {
                        string value = item.FieldValue as string;
                        List<string> cardIds = GetCardIdsForList(item);

                        CardContactRepository repositry = new CardContactRepository(tenant);
                        if (item.Operator == "InListExact")
                        {
                            queryableData = repositry.GetContactsByCardIds(cardIds);
                        }
                        else
                        {
                            queryableData = repositry.GetContactsByCardId(value);
                        }
                    }

                    if (item.FieldName == "ContactEmailAndNames")
                    {
                        string value = item.FieldValue as string;
                        if (queryableData.Count() != 0)
                        {
                            queryableData = queryableData.Where(d =>
                                d.Email.ToUpper().Contains(value.ToUpper())
                                ||
                                d.EnglishName.ToUpper().Contains(value.ToUpper())
                                ||
                                d.LocalName.ToUpper().Contains(value.ToUpper())
                                );
                        }
                    }

                    if (item.FieldName == "UpcomingDates")
                    {
                        string tString = item.FieldValue as string;

                        DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                        int currentDayOfYear = todayDate.Value.DayOfYear;
                        int day1 = currentDayOfYear - 5;
                        int day2 = currentDayOfYear + 5;

                        if (day1 < 1)
                        {
                            day1 = 1;
                        }

                        if (day2 < 1)
                        {
                            day2 = 365;
                        }

                        queryableData = queryableData.Where(c => (c.BirthdayReminder && c.BirthDayOfYear >= day1 && c.BirthDayOfYear <= day2));
                    }

                    if (item.FieldName == "HasNoReminders")
                    {
                        queryableData = queryableData.Where(c => !c.BirthdayReminder);

                    }

                    if (item.FieldName == "UpcomingBirthdaysFilter")
                    {
                        int start = 0;
                        int end = 0;

                        if (item.FieldValue != null)
                        {
                            start = Int32.Parse(item.FieldValue.ToString());
                        }

                        if (item.FieldValue2 != null)
                        {
                            end = Int32.Parse(item.FieldValue2.ToString());
                        }

                        DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                        int currentDayOfYear = todayDate.Value.DayOfYear;
                        int thisYear = todayDate.Value.Year;

                        int day1 = currentDayOfYear + start;
                        int day2 = currentDayOfYear + end;

                        if (day1 <= 0 || day1 > 365)
                        {
                            day1 = 1;
                        }

                        if (day2 <= 0 || day2 > 365)
                        {
                            day2 = 365;
                        }

                        queryableData = from a in queryableData
                                        where a.UserType == "R"
                                        && a.BirthdayReminder
                                        && a.BirthDayOfYear >= day1
                                        && a.BirthDayOfYear <= day2
                                        && (a.DoneDate == null || a.DoneDate.Value.Year != thisYear)
                                        select a;
                    }

                    if (item.FieldName == "ContactIdCustomFilter")
                    {
                        string value = item.FieldValue as string;
                        if (!string.IsNullOrEmpty(value))
                        {
                            ContactRepository contactRepo = new ContactRepository(tenant);
                            UserRepository userRepo = new UserRepository(tenant);

                            List<string> usersIds = new List<string>();
                            List<User> users = userRepo.GetUsers(tenant).ToList();

                            if (users != null && users.Count() > 0)
                            {
                                usersIds = users.Select(a => a.Id).ToList();
                                queryableData = contactRepo.GetContacts(usersIds, tenant);
                            }
                        }
                    }

                    if (item.FieldName == "HasEmail")
                    {
                        queryableData = queryableData.Where(c => !string.IsNullOrEmpty(c.Email));
                    }

                    if (item.FieldName == "SearchEmailsWithout")
                    {
                        string value = item.FieldValue as string;
                        if (!string.IsNullOrEmpty(value))
                        {
                            string[] emails = value.Split(';');
                            if (emails.Count() > 0)
                            {
                                queryableData = queryableData.Where(c => !emails.Contains(c.Email));
                            }
                        }
                    }

                    if (item.FieldName == "Occasion_ContactsQuery")
                    {
                        string value = item.FieldValue as string;
                        if (!string.IsNullOrEmpty(value))
                        {
                            value = value.TrimEnd(',');
                            var contactIds = value.Split(',');
                            if (contactIds.Count() > 0)
                            {
                                queryableData = queryableData.Where(c => contactIds.Contains(c.Id));
                            }
                        }
                    }

                }
            }

            return queryableData;
        }

        private List<string> GetCardIdsForList(QueryFilterItem item)
        {
            string value = item.FieldValue as string;

            if (item.FieldName != "CardCode")
            {
                return value.Split(',').ToList();
            }

            PartnerTypeRepository partnerTypeRepository = new PartnerTypeRepository(tenant);
            string partnerTypeId = partnerTypeRepository.GetSinglePartnerTypeByName(item.FieldValue2?.ToString())?.Id;
            CardRepository cardRepository = new CardRepository(tenant);
            return cardRepository.GetActiveCardsIdsByCodes(value.Split(new Char[] { ';', ',' })?.ToList(), partnerTypeId, tenant);
        }
    }
}
