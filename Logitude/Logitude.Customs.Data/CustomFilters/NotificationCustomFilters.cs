using Logitude.Customs.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Data.CustomFilters
{
    public class NotificationCustomFilters
    {

        public IQueryable<Notification> GetFilteredQuery(QueryOperations operations, IQueryable<Notification> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;



            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {


                    if (item.FieldName == "Dates")
                    {
                        DateTime? value = item.FieldValue as DateTime?;
                        DateTime? value2 = item.FieldValue2 as DateTime?;
                        DateTime? value3 = item.FieldValue3 as DateTime?;
                        if (queryableData.Count() != 0)
                        {
                            if (value != null && value2 != null)
                            {
                                queryableData = queryableData.Where(d => (d.CreateDate >= value && d.CreateDate <= value2) || (d.DueDate != null && (d.DueDate <= value3 || d.DueDate == null)));
                            }

                            else if (value != null && value2 == null)
                            {
                                queryableData = queryableData.Where(d => (d.CreateDate >= value) || (d.DueDate <= value3));
                            }

                            else  if (value2 != null && value == null)
                            {
                                queryableData = queryableData.Where(d => (d.CreateDate <= value2) || (d.DueDate <= value3));
                            }





                            else if (value == null && value2 == null && value3 != null)
                            
                            {

                                queryableData = queryableData.Where(d => d.DueDate <= value3);
                            }


                        }
                    }

                    if (item.FieldName == "Declaration")
                    {
                        string value = item.FieldValue as string;
                        string value2 = item.FieldValue2 as string;
                    
                        if (queryableData.Count() != 0)
                        {
                          
                          queryableData = queryableData.Where(d => (d.Reference1Number == value2 || d.EntityId == value) );
                           

                        }
                    }
                    }

                }
           

            return queryableData;


        }
    }
}
