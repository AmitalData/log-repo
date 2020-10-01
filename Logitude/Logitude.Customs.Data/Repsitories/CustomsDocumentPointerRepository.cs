 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class CustomsDocumentPointerRepository:IRepository<CustomsDocumentPointer>
   {

        public CustomsDocumentPointerRepository()
        {
            (context as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 
        }
		public List<CustomsDocumentPointer> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }


        public List<CustomsDocumentPointer> GetCustomsDocumentPointerPMsByEntityIdAndChilds(string entityId, string childEntityId1,string childEntityId2,string childEntityId3, int tenant)
        {
            List<CustomsDocumentPointer> customsDocumentPointers;
            customsDocumentPointers = (from a in context.CustomsDocumentPointers
                                       where a.ParentEntityId == entityId||a.Child1EntityId==childEntityId1||a.Child2EntityId==childEntityId2||a.Child3EntityId==childEntityId3
                         select a).ToList();


            return customsDocumentPointers;
        }

        public List<CustomsDocumentPointer> GetParentDocumentPointer(string parentEntityId, string parentEntityCode, int tenant)
        {
            List<CustomsDocumentPointer> customsDocumentPointers;
            customsDocumentPointers = GetQParentDocumentPointer(parentEntityId, parentEntityCode, tenant)
                                       .ToList();

            return customsDocumentPointers;
        }

        public IQueryable<CustomsDocumentPointer> GetQParentDocumentPointer(string parentEntityId, string parentEntityCode, int tenant)
        {
           

            return (from a in context.CustomsDocumentPointers
                    where (a.ParentEntityId == parentEntityId && a.ParentEntityCode == parentEntityCode)
                    && a.Tenant == tenant
                    select a);
        }

        public List<CustomsDocumentPointer> GetCustomDocumentPointersForTicketId(string CustomsDocumentsTicketId, int tenant)
        {
            List<CustomsDocumentPointer> customsDocumentPointers;
            customsDocumentPointers = (from a in context.CustomsDocumentPointers
                                       where (a.CustomsDocumentsTicketId == CustomsDocumentsTicketId)
                                       && a.Tenant == tenant
                                       select a).ToList();

            return customsDocumentPointers;
        }

        public List<CustomsDocumentPointer> GetCustomDocumentPointersForCustomDocumentId(string customsDocumentId, int tenant)
        {
            List<string> ticketsIds = (from a in context.CustomsDocumentsTickets
                                       where a.DocumentsFilingId == customsDocumentId
                                       select a.Id).ToList();

            List<CustomsDocumentPointer> customsDocumentPointers;
            customsDocumentPointers = (from a in context.CustomsDocumentPointers
                                       where ticketsIds.Contains(a.CustomsDocumentsTicketId)
                                       && a.Tenant == tenant
                                       select a).ToList();

            return customsDocumentPointers;
        }

        public List<CustomsDocumentPointer> GetCustomDocumentPointersForMultipleTicketIds(List<string> customsDocumentsTicketIds, int tenant)
        {
            List<CustomsDocumentPointer> customsDocumentPointers;
            customsDocumentPointers = (from a in context.CustomsDocumentPointers
                                       where customsDocumentsTicketIds.Contains(a.CustomsDocumentsTicketId)
                                       && a.Tenant == tenant
                                       select a).ToList();

            return customsDocumentPointers;
        }

        public bool CheckForPointers(string parentEntityId, string child1EntityId, string child2EntityId, string child3EntityId, int tenant)
        {
            //string[] ids = child2EntityId.Split(',');
            bool exists = false;
            //if (ids.Count() > 1)
            //{
            //    exists = (from a in context.CustomsDocumentPointers
            //              where a.ParentEntityId == parentEntityId && a.Tenant == tenant && ((a.Child1EntityId != null ? a.Child1EntityId == child1EntityId : false) || (ids.Contains(a.Child2EntityId)) || (a.Child3EntityId != null ? a.Child3EntityId == child3EntityId : false))
            //              select a).Any();
            //}
            //else
            //{

                if (!string.IsNullOrEmpty(child3EntityId))
                {
                    exists = (from a in context.CustomsDocumentPointers
                              where a.ParentEntityId == parentEntityId && a.Tenant == tenant && a.Child1EntityId == child1EntityId && a.Child2EntityId == child2EntityId && a.Child3EntityId == child3EntityId
                              select a).Any();
                }
                else if ((!string.IsNullOrEmpty(child2EntityId)))
                {
                    exists = (from a in context.CustomsDocumentPointers
                              where a.ParentEntityId == parentEntityId && a.Tenant == tenant && a.Child1EntityId == child1EntityId && a.Child2EntityId == child2EntityId
                              select a).Any();
                }
                else if ((!string.IsNullOrEmpty(child1EntityId)))
                {
                    exists = (from a in context.CustomsDocumentPointers
                              where a.ParentEntityId == parentEntityId && a.Tenant == tenant && a.Child1EntityId == child1EntityId
                              select a).Any();
                }
                //if (!exists)
                //{
 
                //}

                //exists = (from a in context.CustomsDocumentPointers
                //          where a.ParentEntityId == parentEntityId && a.Tenant == tenant && ((a.Child1EntityId != null ? a.Child1EntityId == child1EntityId : false) || (a.Child2EntityId != null ? a.Child2EntityId == child2EntityId : false) || (a.Child3EntityId != null ? a.Child3EntityId == child3EntityId : false))
                //          select a).Any();
           // }
            return exists;
        }

        public bool CheckIfDocumentPointerExistsForConstraint(string child1EntityId, string child1EntityCode, int tenant)
        {
            bool exists = (from a in context.CustomsDocumentPointers
                           where a.Child1EntityId == child1EntityId && a.Child1EntityCode == child1EntityCode && a.Tenant == tenant
                           select a).Any();
            return exists;
        }

        public List<CustomsDocumentPointer> GetCustomsDocumentPointerPMsByRequiredDocID(string requiredDocID, int tenant)
        {
            List<CustomsDocumentPointer> customsDocumentPointers = new List<CustomsDocumentPointer>();
            //customsDocumentPointers = (from a in context.CustomsDocumentPointers.Include("CustomDocumentType")
            //                           where a.RequiredDocID == requiredDocID && a.Tenant == tenant
            //                           select a).ToList();
            //return customsDocumentPointers;

            List<string> ticketsIds = (from a in context.CustomsDocumentsTickets
                                       where a.RequestedCustomsDocId == requiredDocID
                                       select a.Id).ToList();

            customsDocumentPointers = (from a in context.CustomsDocumentPointers
                                       where ticketsIds.Contains(a.CustomsDocumentsTicketId)
                                       && a.Tenant == tenant
                                       select a).ToList();

            return customsDocumentPointers;
        }

        public IQueryable<CustomsDocumentPointer> GetCustomsDocumentPointerList(GetTicketsParams parameters, int tenant)
        {
            var q = GetAll(tenant);
            q = q.Where(a => a.ParentEntityCode == parameters.ParentEntityCode && a.ParentEntityId == parameters.ParentEntityId);
            //q = q.Where(a => a.Child1EntityCode == parameters.Child1EntityCode && a.Child1EntityId == parameters.Child1EntityId);
            //q = q.Where(a => a.Child2EntityCode == parameters.Child2EntityCode && a.Child2EntityId == parameters.Child2EntityId);
            //q = q.Where(a => a.Child3EntityCode == parameters.Child3EntityCode && a.Child3EntityId == parameters.Child3EntityId);

            /*if (!string.IsNullOrWhiteSpace(parameters.Child1EntityCode) && !string.IsNullOrWhiteSpace(parameters.Child1EntityId))
            {
                q = q.Where(a => a.Child1EntityCode == parameters.Child1EntityCode && a.Child1EntityId == parameters.Child1EntityId);
            }
            if (!string.IsNullOrWhiteSpace(parameters.Child2EntityCode) && !string.IsNullOrWhiteSpace(parameters.Child2EntityId))
            {
                q = q.Where(a => a.Child2EntityCode == parameters.Child2EntityCode && a.Child2EntityId == parameters.Child2EntityId);
            }
            if (!string.IsNullOrWhiteSpace(parameters.Child3EntityCode) && !string.IsNullOrWhiteSpace(parameters.Child3EntityId))
            {
                q = q.Where(a => a.Child3EntityCode == parameters.Child3EntityCode && a.Child3EntityId == parameters.Child3EntityId);
            }*/

            q = q.Where(a => ((a.Child1EntityCode ?? "_IsNull") == (parameters.Child1EntityCode ?? "_IsNull")) && ((a.Child1EntityId ?? "_IsNull") == (parameters.Child1EntityId ?? "_IsNull")));
            q = q.Where(a => ((a.Child2EntityCode ?? "_IsNull") == (parameters.Child2EntityCode ?? "_IsNull")) && ((a.Child2EntityId ?? "_IsNull") == (parameters.Child2EntityId ?? "_IsNull")));
            q = q.Where(a => ((a.Child3EntityCode ?? "_IsNull") == (parameters.Child3EntityCode ?? "_IsNull")) && ((a.Child3EntityId ?? "_IsNull") == (parameters.Child3EntityId ?? "_IsNull")));
            q = q.Distinct();

            return q;
        }

        public IQueryable<CustomsDocumentPointer> GetCustomsDocumentPointerListParentOnly(GetTicketsParams parameters, int tenant)
        {
            var q = GetAll(tenant);
            q = q.Where(a => a.ParentEntityCode == parameters.ParentEntityCode && a.ParentEntityId == parameters.ParentEntityId);
            q = q.Distinct();

            return q;
        }

        public IQueryable<CustomsDocumentPointer> GetCustomDocumentPoinersForItems(string parentEntityId, string invCounterKey, string itemsLineNumbers, int tenant)
        {
            string[] numbers = itemsLineNumbers.Split(',');
            IQueryable<CustomsDocumentPointer> pointers = (from a in context.CustomsDocumentPointers
                                                           where a.ParentEntityId == parentEntityId && a.Child1EntityId == invCounterKey && numbers.Contains(a.Child2EntityId) && a.Tenant == tenant
                                                           select a);
            return pointers;
        }
   }

}
   