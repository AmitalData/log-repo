using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel;
using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.BL.CommonDataModel.EntityPMs;
using System.ServiceModel.DomainServices.Server;
using Logitude.Server.Tools.Counters;
using Logitude.BL.InvoiceModel.Tools.DataMapping;
namespace Logitude.BL.InvoiceModel.Tools.EntityService
{
    public class QuickbooksSyncRequestTicketService
    {
        bool isNewEntity;
        private int tenant;
        public QuickbooksSyncRequestTicket Poco { get; set; }

        public IInvoiceContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private QuickbooksSyncRequestTicketPM entityPM;
        private IInvoiceContext objectContext;
        private QuickbooksSyncRequestTicketRepository entityRepository;
        public QuickbooksSyncRequestTicketService(IInvoiceContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new QuickbooksSyncRequestTicketRepository(objectContext);
        }

        public QuickbooksSyncRequestTicketService(IInvoiceContext objectContext)
        {
           
            this.ObjectContext = objectContext;
            this.entityRepository = new QuickbooksSyncRequestTicketRepository(objectContext);
        }


        public void Create(QuickbooksSyncRequestTicketPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Ticket =theEntityPm.Ticket;
            this.Poco = new QuickbooksSyncRequestTicket();
            this.Poco.Ticket = this.entityPM.Ticket;

            QuickbooksSyncRequestTicketMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(QuickbooksSyncRequestTicketPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleRequestTicketByTicket(theEntityPm.Ticket);
            if (Poco != null)
            {
                QuickbooksSyncRequestTicketMapping.MapEntity(entityPM, Poco, isNewEntity);
                entityRepository.Update(Poco);
                entityRepository.SubmitChanges();
            }
            
        }

        public void Remove(QuickbooksSyncRequestTicketPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleRequestTicketByTicket(theEntityPm.Ticket);
            entityRepository.Remove(Poco);
            entityRepository.SubmitChanges();

        }


    }
}