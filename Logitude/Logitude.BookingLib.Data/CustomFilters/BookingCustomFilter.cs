using Logitude.BookingLib.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BookingLib.Data.CustomFilters
{
    public class BookingCustomFilter
    {
        public static IQueryable<Booking> GetFilteredQuery(QueryOperations operations, IQueryable<Booking> queryableData, int tenant)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;
            bool showIsCancelled = false;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    switch (item.FieldName)
                    {
                        case "CreatedBookings":
                            {
                                queryableData = queryableData.Where(d => d.BookingStatusCode == "CRT");
                                break;
                            }

                        case "WaitingBookings":
                            {
                                queryableData = queryableData.Where(d => d.WaitingForResponse);
                                break;
                            }

                        case "ConfirmedBookings":
                            {
                                queryableData = queryableData.Where(d => d.BookingStatusCode == "CNF");
                                break;
                            }

                        case "RejectedBookings":
                            {
                                queryableData = queryableData.Where(d => d.HasErrors);
                                break;
                            }

                        case "ProgressBookings":
                            {
                                queryableData = queryableData.Where(d => d.BookingStatusCode != "AWB");
                                break;
                            }

                        case "CancelledBookings":
                            {
                                showIsCancelled = true;
                                queryableData = queryableData.Where(d => d.IsCancelled);
                                break;
                            }
                    }
                }
            }

            if (showIsCancelled)
            {
                queryableData = queryableData.Where(d => d.IsCancelled == true);
            }

            else
            {
                queryableData = queryableData.Where(d => d.IsCancelled == false);
            }

            return queryableData;
        }
    }
}
