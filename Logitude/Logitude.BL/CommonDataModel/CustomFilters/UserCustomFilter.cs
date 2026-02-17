using System;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.BL.CommonDataModel.CustomFilters
{
    public class UserCustomFilter
    {
        private int tenant;
        public UserCustomFilter(int tenant)
        {
            this.tenant = tenant;
        }

        public IQueryable<User> GetFilteredQuery(QueryOperations operations, IQueryable<User> iQueryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "ActiveUsers")
                    {
                        iQueryableData = iQueryableData.Where(d => d.Contact.InActive == false);
                    }

                    if (item.FieldName == "InactiveUsers")
                    {
                        iQueryableData = iQueryableData.Where(d => d.Contact.InActive == true);
                    }

                    if (item.FieldName == "ActiveLicensed")
                    {
                        iQueryableData = iQueryableData.Where(d => d.LicencedUser == true && d.Contact.InActive == false);
                    }

                    if (item.FieldName == "ActiveNotLicensed")
                    {
                        iQueryableData = iQueryableData.Where(d => d.LicencedUser == false && d.Contact.InActive == false);
                    }

                    if (item.FieldName == "EmployeeGroupCustomFilter")
                    {
                        //var groupId = item.FieldValue.ToString();
                        //if (!string.IsNullOrEmpty(groupId))
                        //{
                        //    iQueryableData = iQueryableData.Where(d => d.GroupId != null && d.GroupId.Any(a => a == groupId));

                        //}
                    }

                    if (item.FieldName == "HasEmail")
                    {
                        iQueryableData = iQueryableData.Where(c => !string.IsNullOrEmpty(c.Contact.Email));
                    }

                    if (item.FieldName == "SearchEmailsWithout")
                    {
                        string value = item.FieldValue as string;
                        if (!string.IsNullOrEmpty(value))
                        {
                            string[] emails = value.Split(';');
                            if (emails.Count() > 0)
                            {
                                iQueryableData = iQueryableData.Where(c => !emails.Contains(c.Contact.Email));
                            }
                        }
                    }
                }
            }

            return iQueryableData;
        }
    }
}