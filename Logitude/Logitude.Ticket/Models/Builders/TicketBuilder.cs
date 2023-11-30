using Logitude.Base.Models.Partners;
using Logitude.Base.Models.UserTenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TicketTests.Models.Builders
{
    public class TicketBuilder
    {

        private TicketPM _ticket;

        public TicketBuilder()
        {
            this.Reset();
        }

        private void Reset()
        {
            _ticket = new TicketPM();
        }


        public TicketBuilder Subject(string subject)
        {
            _ticket.Subject = subject;
            return this;
        }

        public TicketBuilder TicketDescription(string ticketDescription)
        {
            _ticket.TicketDescription = ticketDescription;
            return this;
        }

        public TicketBuilder EntityType(string entityType)
        {
            _ticket.EntityType = entityType;
            return this;
        }

        public TicketBuilder MainClassificationId(string mainClassificationId)
        {
            _ticket.MainClassificationId = mainClassificationId;
            return this;
        }

        public TicketBuilder SeverityId(string severityId)
        {
            _ticket.SeverityId = severityId;
            return this;
        }

        public TicketBuilder EmployeeGroupId(string employeeGroupId)
        {
            _ticket.EmployeeGroupId = employeeGroupId;
            return this;
        }

        public TicketBuilder StageId(string stageId)
        {
            _ticket.StageId = stageId;
            return this;
        }

        public TicketPM Build()
        {
            TicketPM result = _ticket;
            this.Reset();
            return result;
        }

        public TicketBuilder WithModel(TicketPM ticket)
        {
            _ticket = ticket;
            return this;
        }

        public TicketBuilder WithDefualtValues()
        {
            _ticket = new TicketPM
            {
                Tenant = UserTenant.Tenant,
                CreatedByContactId = UserTenant.UserId,
                UpdatedByUserId = UserTenant.UserId,
                OwnerId = UserTenant.UserId,
                BusinessUnitId = UserTenant.BusinessUnitId,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CompanyId = PartnersData.CustomerId,
                ContactId = PartnersData.CustomerContactId
            };
            return this;
        }
    }
}
