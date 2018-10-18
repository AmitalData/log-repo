using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class QuickbooksSyncRequestTicketQuery
    {
       public QuickbooksSyncRequestTicketRepository repository;
        public QuickbooksSyncRequestTicketQuery()
        {
            repository = new QuickbooksSyncRequestTicketRepository(); 
        }


        public QuickbooksSyncRequestTicketQuery(QuickbooksSyncRequestTicketRepository RequestTicketRepository)
        {
            repository = RequestTicketRepository;
        }

        public QuickbooksSyncRequestTicketQuery(int tenant)
        {
            repository = new QuickbooksSyncRequestTicketRepository(tenant);
        }

        public IQueryable<QuickbooksSyncRequestTicketPM> GetRequestTicketPMs()
        {
            return from a in repository.context.QuickbooksSyncRequestTickets
                   select new QuickbooksSyncRequestTicketPM()
                   {
                      Ticket = a.Ticket,
                      ExternalTablesRequestCount = a.ExternalTablesRequestCount,
                      Password = a.Password,
                      ReferenceNumber = a.ReferenceNumber,
                      RequestCount = a.RequestCount,
                      UserName = a.UserName,
                      Tenant = a.Tenant,
                      
                   };
        }

        public QuickbooksSyncRequestTicketPM GetSingleRequestTicketPM(string ticket)
        {
            return (from a in repository.context.QuickbooksSyncRequestTickets
                    where a.Ticket == ticket
                    select new QuickbooksSyncRequestTicketPM()
                    {
                        Ticket = a.Ticket,
                        ExternalTablesRequestCount = a.ExternalTablesRequestCount,
                        Password = a.Password,
                        ReferenceNumber = a.ReferenceNumber,
                        RequestCount = a.RequestCount,
                        UserName = a.UserName,
                        Tenant = a.Tenant,
                        IsCurrentInvoiceChecked = a.IsCurrentInvoiceChecked,
                    }).FirstOrDefault();
        }


        public QuickbooksSyncRequestTicketPM GetSingleRequestTicketPM(string ticket, int tenant)
        {
            return (from a in repository.context.QuickbooksSyncRequestTickets
                    where a.Ticket == ticket && a.Tenant == tenant
                    select new QuickbooksSyncRequestTicketPM()
                    {
                       Ticket = a.Ticket,
                      ExternalTablesRequestCount = a.ExternalTablesRequestCount,
                      Password = a.Password,
                      ReferenceNumber = a.ReferenceNumber,
                      RequestCount = a.RequestCount,
                      UserName = a.UserName,
                      Tenant = a.Tenant,
                    }).FirstOrDefault();
        }

        public IQueryable<QuickbooksSyncRequestTicketPM> GetRequestTicketPMsByTenant(int tenant)
        {
            IQueryable<QuickbooksSyncRequestTicketPM> RequestTickets = (from a in repository.context.QuickbooksSyncRequestTickets
                                              where a.Tenant == tenant
                                              select new QuickbooksSyncRequestTicketPM()
                                              {
                                                  Ticket = a.Ticket,
                                                  ExternalTablesRequestCount = a.ExternalTablesRequestCount,
                                                  Password = a.Password,
                                                  ReferenceNumber = a.ReferenceNumber,
                                                  RequestCount = a.RequestCount,
                                                  UserName = a.UserName,
                                                  Tenant = a.Tenant,
                                              });
            return RequestTickets;
        }



        public IQueryable<QuickbooksSyncRequestTicketList> GetIQueryableEntityList(IQueryable<QuickbooksSyncRequestTicket> iQueryable)
        {
            IQueryable<QuickbooksSyncRequestTicketList> result = from requestTicket in iQueryable
                                             select new QuickbooksSyncRequestTicketList()
                                             {
                                                 Ticket = requestTicket.Ticket,
                                                 Tenant = requestTicket.Tenant,
                                                 UserName = requestTicket.UserName,
                                                 ExternalTablesRequestCount = requestTicket.ExternalTablesRequestCount,
                                                 Password = requestTicket.Password,
                                                 ReferenceNumber = requestTicket.ReferenceNumber,
                                                 RequestCount = requestTicket.RequestCount,
                                                 
                                               
                                              
                                             };
            return result;
        }




    }
}