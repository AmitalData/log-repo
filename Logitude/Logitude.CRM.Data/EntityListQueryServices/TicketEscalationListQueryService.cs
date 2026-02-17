using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityLists;

namespace Logitude.CRM.Data.EntityListQueryServices
{

    public partial class TicketEscalationListQueryService
    {
        private IQueryable<TicketEscalationList> GetIqueryableList(IQueryable<TicketEscalation> iQueryable)
        {
            IQueryable<TicketEscalationList> query = (from a in iQueryable
                                                      select new TicketEscalationList()
                                                      {

                                                          Id = a.Id,

                                                          Tenant = a.Tenant,

                                                          CreateDate = a.CreateDate,

                                                          TicketId = a.TicketId,

                                                          LineNumber = a.LineNumber,

                                                          EscalationFor = a.EscalationFor,

                                                          Recepients = a.Recepients,

                                                          IsClose = a.IsClose,

                                                          IsSLAViolated = a.IsSLAViolated,

                                                          DueDate = a.DueDate,

                                                          CloseDate = a.CloseDate,

                                                          EscalationForName = a.EscalationFor == "FR" ? "First Response" : "Resolve Within",

                                                          UpdateDate = a.UpdateDate,
                                                      });
            return query;
        }

        private IQueryable<TicketEscalation> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<TicketEscalation> iQueryable, int tenant)
        {
            return iQueryable;
        }

        private IQueryable<TicketEscalation> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<TicketEscalation> iQueryable, int tenant)
        {
            return iQueryable;
        }

        public List<TicketEscalationList> GetTicketEscalationListByTicketId(string ticketId, int tenant)
        {
            IQueryable<TicketEscalationList> query = (from a in context.TicketEscalations.Include("CreatedByContact")
                                                      where a.Tenant == tenant && a.TicketId == ticketId

                                                    select new TicketEscalationList()
                                                    {
                                                        Id = a.Id,

                                                        Tenant = a.Tenant,

                                                        CreateDate = a.CreateDate,

                                                        TicketId = a.TicketId,

                                                        LineNumber = a.LineNumber,

                                                        EscalationFor = a.EscalationFor,

                                                        Recepients = a.Recepients,

                                                        IsClose = a.IsClose,

                                                        IsSLAViolated = a.IsSLAViolated,

                                                        DueDate = a.DueDate,

                                                        CloseDate = a.CloseDate,

                                                        EscalationForName = a.EscalationFor == "FR" ? "First Response" : "Resolve Within",

                                                        UpdateDate = a.UpdateDate,

                                                       
                                                    });
            return query.ToList();
        }
    }
}
