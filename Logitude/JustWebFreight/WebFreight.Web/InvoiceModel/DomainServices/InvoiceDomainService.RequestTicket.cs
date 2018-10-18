using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.InvoiceModel.DomainServices
{
    public partial class InvoiceDomainService
    {

        public QuickbooksSyncRequestTicketPM GetSingleRequestTicketPM(string ticket, int tenant)
        {

            SecurityUtility.AuthenticationOnTenant(tenant);
          

            requestTicketQuery = new QuickbooksSyncRequestTicketQuery(tenant);
            return requestTicketQuery.GetSingleRequestTicketPM(ticket, tenant);
        }

        public QuickbooksSyncRequestTicketPM GetRequestTicketPM(string ticket, int tenant)
        {

            SecurityUtility.AuthenticationOnTenant(tenant);


            requestTicketQuery = new QuickbooksSyncRequestTicketQuery(tenant);
            return requestTicketQuery.GetSingleRequestTicketPM(ticket);
        }


        public void UpdateRequestTicketList(QuickbooksSyncRequestTicketList currentEntity)
        {

        }

        public QuickbooksSyncRequestTicketList GetSingleRequestTicketList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
        

            QuickbooksSyncRequestTicketList entityList = null;
            requestTicketRepository = new QuickbooksSyncRequestTicketRepository(tenant);
            requestTicketQuery = new QuickbooksSyncRequestTicketQuery(requestTicketRepository);
            QuickbooksSyncRequestTicket entity = requestTicketRepository.GetSingleRequestTicket(id, tenant);

            if (entity != null)
            {
                List<QuickbooksSyncRequestTicket> SingleEntityList = new List<QuickbooksSyncRequestTicket>();
                SingleEntityList.Add(entity);

                IQueryable<QuickbooksSyncRequestTicket> iQueryable = SingleEntityList.AsQueryable();
                IQueryable<QuickbooksSyncRequestTicketList> iQueryableEntityList = requestTicketQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }


        public IQueryable<QuickbooksSyncRequestTicketList> GetRequestTicketLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            requestTicketRepository = new QuickbooksSyncRequestTicketRepository(tenant);
            requestTicketQuery = new QuickbooksSyncRequestTicketQuery(requestTicketRepository);
            IQueryable<QuickbooksSyncRequestTicket> RequestTickets = requestTicketRepository.GetRequestTicketsByTenant(tenant);

            IQueryable<QuickbooksSyncRequestTicketList> query2 = requestTicketQuery.GetIQueryableEntityList(RequestTickets);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<QuickbooksSyncRequestTicketList> GetRequestTicketFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            requestTicketRepository = new QuickbooksSyncRequestTicketRepository(tenant);
            requestTicketQuery = new QuickbooksSyncRequestTicketQuery(requestTicketRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<QuickbooksSyncRequestTicket> RequestTickets = requestTicketRepository.GetRequestTicketsByTenant(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            RequestTickets = filter.GetFilteredQuery<QuickbooksSyncRequestTicket>(nonListQueryOperation, RequestTickets);

            int skippedRequestTickets = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            IQueryable<QuickbooksSyncRequestTicketList> query2 = requestTicketQuery.GetIQueryableEntityList(RequestTickets);

            query2 = filter.GetFilteredQuery<QuickbooksSyncRequestTicketList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(QuickbooksSyncRequestTicketList).GetProperty(queryOperations.SortByColumnName);

                switch (propInfo.PropertyType.Name.ToLower())
                {
                    case "string":
                        {
                            query2 = sortClass.GetSorterQuery<QuickbooksSyncRequestTicketList, string>(queryOperations, query2);
                            break;
                        }
                    case "double":
                        {
                            query2 = sortClass.GetSorterQuery<QuickbooksSyncRequestTicketList, double>(queryOperations, query2);
                            break;
                        }
                    case "datetime":
                        {
                            query2 = sortClass.GetSorterQuery<QuickbooksSyncRequestTicketList, DateTime>(queryOperations, query2);
                            break;
                        }
                    case "int":
                        {
                            query2 = sortClass.GetSorterQuery<QuickbooksSyncRequestTicketList, int>(queryOperations, query2);
                            break;
                        }
                    default:
                        {
                            query2 = query2.OrderByDescending(d => d.ReferenceNumber);
                            break;
                        }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.ReferenceNumber);
            }

            //--------------------------------------------------------------------------------------------------

            query2 = query2.Skip(skippedRequestTickets);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetRequestTicketFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<QuickbooksSyncRequestTicket> RequestTickets = requestTicketRepository.GetRequestTicketsByTenant(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            RequestTickets = filter.GetFilteredQuery<QuickbooksSyncRequestTicket>(nonListQueryOperation, RequestTickets);
            requestTicketQuery = new QuickbooksSyncRequestTicketQuery(requestTicketRepository);
            IQueryable<QuickbooksSyncRequestTicketList> query2 = requestTicketQuery.GetIQueryableEntityList(RequestTickets);

            query2 = filter.GetFilteredQuery<QuickbooksSyncRequestTicketList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }



    }
}