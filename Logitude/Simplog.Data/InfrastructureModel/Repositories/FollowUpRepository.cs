using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class FollowUpRepository : IRepository<FollowUp>
    {
        IWebFreightContext webFreightContext;
        public FollowUpRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public FollowUpRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }
        public FollowUpRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public IQueryable<FollowUp> GetFollowUps(int tenant)
        {
            return (from record in context.FollowUps.Include("EventType").Include("OwnerUser.Contact") where record.Tenant == tenant select record);
        }

        public List<FollowUp> GetFollowUpsForDocOut(string docOutId,int tenant)
        {
            return (from record in context.FollowUps.Include("EventType").Include("OwnerUser.Contact")
                    where record.Tenant == tenant && record.InternalDocumentId == docOutId 
                    select record).ToList();
        }

        public List<FollowUp> GetFollowUpsForDocIn(string docInId, int tenant)
        {
            return (from record in context.FollowUps.Include("EventType").Include("OwnerUser.Contact")
                    where record.Tenant == tenant && record.DocumentsFilingId == docInId
                    select record).ToList();
        }

        public FollowUp GetSingleFollowUp(string id, int tenant)
        {
            return (from record in context.FollowUps.Include("EventType").Include("OwnerUser.Contact") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();

        }

        public IQueryable<FollowUp> GetFollowUpsThatHaveDateFileName(int tenant)
        {
            IQueryable<FollowUp> followups = (from a in context.FollowUps
                                              where a.Tenant == tenant  && !string.IsNullOrEmpty(a.DateFieldName)
                                              select a);
            return followups;
        }

        public List<FollowUp> GetFollowUpsByShipmentId(string shipmentId, int tenant)
        {
            List<FollowUp> followups = (from a in context.FollowUps.Include("EventType").Include("OwnerUser.Contact")
                                        where a.Tenant == tenant && a.ShipmentId == shipmentId
                                        select a).ToList();
            return followups;
        }

        public List<FollowUp> GetFollowUpsByEntityId(string entityId ,string objectTableName,int tenant)
        {
            List<FollowUp> followups = (from a in context.FollowUps
                                        where a.Tenant == tenant && ((a.ShipmentId == entityId && objectTableName == "Shipment") || (a.QuoteId == entityId && objectTableName == "Quote")) && !string.IsNullOrEmpty(a.DocumentTypeId)
                                        select a).ToList();
            return followups;
        }

        public List<FollowUp> GetFollowUpsByShipmentIdAndLeg(string shipmentId,string legType, int tenant)
        {
            List<FollowUp> followups = (from a in context.FollowUps.Include("EventType").Include("OwnerUser.Contact")
                                        where a.Tenant == tenant && a.ShipmentId == shipmentId&&a.LegType.Contains(legType)
                                          select a).ToList();
            return followups;
        }

        public List<FollowUp> GetFollowUpsByQuoteId(string quoteId, int tenant)
        {
            List<FollowUp> followups = (from a in context.FollowUps.Include("EventType").Include("OwnerUser.Contact")
                                        where a.Tenant == tenant && a.QuoteId == quoteId
                                        select a).ToList();
            return followups;
                  
        }

     



        public int GetFollowUpCount(string direction, string transportmode, string reference, string partner, int tenant, string cardId, string date)
        {
            int count = 0;
            if (date == "Today")
            {
                DateTime compareDate = DateTime.Now.Date;
                count = (from d in context.FollowUps
                         where d.Tenant == tenant && d.Shipment.Direction.Name.ToUpper() == direction.ToUpper() && d.Date != null && d.Date.Value == compareDate && (d.Shipment.TransportMode.Name.ToUpper().StartsWith(transportmode.ToUpper()) || d.Shipment.ShipperReference1.ToUpper() == reference.ToUpper()
                                         || d.Shipment.ShipperReference2.ToUpper() == reference.ToUpper()
                                         || d.Shipment.ConsigneeReference1.ToUpper() == reference.ToUpper()
                                         || d.Shipment.ConsigneeReference2.ToUpper() == reference.ToUpper()
                                         || d.Shipment.ShipmentNumber.ToUpper() == reference.ToUpper() || d.Shipment.ShipperCard.EnglishName.ToUpper().StartsWith(partner.ToUpper())
                                         || d.Shipment.ConsigneeCard.EnglishName.ToUpper().StartsWith(partner.ToUpper()))
                         select d).Count();
            }

            if (date == "Tomorrow")
            {
                DateTime compareDate = DateTime.Now.Date.AddDays(1);
                count = (from d in context.FollowUps
                         where d.Tenant == tenant && d.Shipment.Direction.Name.ToUpper() == direction.ToUpper() && d.Date != null && d.Date != null && d.Date.Value == compareDate && (d.Shipment.TransportMode.Name.ToUpper().StartsWith(transportmode.ToUpper()) || d.Shipment.ShipperReference1.ToUpper() == reference.ToUpper()
                                         || d.Shipment.ShipperReference2.ToUpper() == reference.ToUpper()
                                         || d.Shipment.ConsigneeReference1.ToUpper() == reference.ToUpper()
                                         || d.Shipment.ConsigneeReference2.ToUpper() == reference.ToUpper()
                                         || d.Shipment.ShipmentNumber.ToUpper() == reference.ToUpper() || d.Shipment.ShipperCard.EnglishName.ToUpper().StartsWith(partner.ToUpper())
                                         || d.Shipment.ConsigneeCard.EnglishName.ToUpper().StartsWith(partner.ToUpper()))
                         select d).Count();
            }

            if (date == "DueDate")
            {
                DateTime compareDate = DateTime.Now.Date;
                count = (from d in context.FollowUps
                         where d.Tenant == tenant && d.Shipment.Direction.Name.ToUpper() == direction.ToUpper() && d.Date != null && d.Date.Value <= compareDate && (d.Shipment.TransportMode.Name.ToUpper().StartsWith(transportmode.ToUpper()) || d.Shipment.ShipperReference1.ToUpper() == reference.ToUpper()
                                         || d.Shipment.ShipperReference2.ToUpper() == reference.ToUpper()
                                         || d.Shipment.ConsigneeReference1.ToUpper() == reference.ToUpper()
                                         || d.Shipment.ConsigneeReference2.ToUpper() == reference.ToUpper()
                                         || d.Shipment.ShipmentNumber.ToUpper() == reference.ToUpper() || d.Shipment.ShipperCard.EnglishName.ToUpper().StartsWith(partner.ToUpper())
                                         || d.Shipment.ConsigneeCard.EnglishName.ToUpper().StartsWith(partner.ToUpper()))
                         select d).Count();
            }

            if (date == "All")
            {

                count = (from d in context.FollowUps.Include("EventType").Include("OwnerUser.Contact")
                         where d.Tenant == tenant && d.Shipment.Direction.Name.ToUpper().StartsWith(direction.ToUpper())
                         select d).Count();

            }

            return count;

        }

        public IQueryable<FollowUp> GetFollowUpsByTenant(int tenant, string cardId)
        {
            IQueryable<FollowUp> followUps = null;

            if (!string.IsNullOrEmpty(cardId))
            {

                followUps = from a in context.FollowUps.Include("EventType").Include("OwnerUser.Contact")
                            where a.Tenant == tenant && (a.Shipment.AgentId == cardId || a.Shipment.ShipperId == cardId)
                            select a;
            }
            else
            {
                followUps = from a in context.FollowUps.Include("EventType").Include("OwnerUser.Contact")
                            where a.Tenant == tenant
                            select a;

            }

            return followUps;
        }

   
        public int GetFollowUpsCountForShipments(int tenant)
        {
            int count = (from a in context.FollowUps.Include("EventType").Include("OwnerUser.Contact")
                         where a.Tenant == tenant && a.ShipmentId != null&&a.Shipment.ShipmentLevelCode!="C"
                         select a).Count();
            return count;
        }

  
        public int GetFollowUpsCountForMasters(int tenant)
        {
            int count = (from a in context.FollowUps.Include("EventType").Include("OwnerUser.Contact")
                         where a.Tenant == tenant && a.ShipmentId != null && a.Shipment.ShipmentLevelCode != "H"
                         select a).Count();
            return count;
        }

  
        public int GetFollowUpsCountForQuotes(int tenant)
        {
            int count = (from a in context.FollowUps.Include("EventType").Include("OwnerUser.Contact")
                         where a.Tenant == tenant && a.QuoteId != null
                         select a).Count();
            return count;
        }





        public bool CheckIfFollowUpExist( string entityId , string objectTableName , string eventTypeId ,int tenant)
        {

            FollowUp followUp = (from a in context.FollowUps
                                 where a.Tenant == tenant && a.EventTypeId == eventTypeId && ((a.ShipmentId == entityId && objectTableName == "Shipment") || (a.QuoteId == entityId && objectTableName == "Quote"))
                                 select a).FirstOrDefault();

            if (followUp!=null) return true;
            else return false;
        }


        public IQueryable<string> GetDocumentTypeIdListsFromFollowUp(string entityId, string objectTableName, string eventTypeId, List<string> documentTypeIds, int tenant)
        {
            IQueryable<string> documentTypeIdLists = null;

            documentTypeIdLists = from a in context.FollowUps
                        where a.Tenant == tenant && a.EventTypeId == eventTypeId && ((a.ShipmentId == entityId && objectTableName == "Shipment") || (a.QuoteId == entityId && objectTableName == "Quote")) && documentTypeIds.Contains(a.DocumentTypeId)
                        select a.DocumentTypeId;


            return documentTypeIdLists;
        }





        public void Add(FollowUp entity)
        {
            context.FollowUps.Add(entity);
        }

        public void Remove(FollowUp entity)
        {
            try
            {
                context.FollowUps.Attach(entity);
            }
            catch { }
            context.FollowUps.Remove(entity);

        }

        public void Update(FollowUp entity)
        {
            try
            {

                context.FollowUps.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<FollowUp> All()
        {
            return context.FollowUps.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }




        public List<FollowUp> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public FollowUp GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}