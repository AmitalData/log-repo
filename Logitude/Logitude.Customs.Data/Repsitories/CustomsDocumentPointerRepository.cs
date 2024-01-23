 
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
using System.Runtime.Remoting.Contexts;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class CustomsDocumentPointerRepository:IRepository<CustomsDocumentPointer>
   {

        public CustomsDocumentPointerRepository()
        {
            //(context as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 
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
            (context as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            var res= (from a in context.CustomsDocumentPointers
                    where (a.ParentEntityId == parentEntityId && a.ParentEntityCode == parentEntityCode)
                    && a.Tenant == tenant
                    select a);
            return res;
        }

        public List<CustomsDocumentPointer> GetCustomDocumentPointersForTicketId(string CustomsDocumentsTicketId, int tenant)
        {
            (context as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 


            List<CustomsDocumentPointer> customsDocumentPointers;
            customsDocumentPointers = (from a in context.CustomsDocumentPointers
                                       where (a.CustomsDocumentsTicketId == CustomsDocumentsTicketId)
                                       && a.Tenant == tenant
                                       select a).ToList();

            return customsDocumentPointers;
        }

        public List<CustomsDocumentPointer> GetCustomDocumentPointersForCustomDocumentId(string customsDocumentId, int tenant)
        {
            (context as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            List<string> ticketsIds = (from a in context.CustomsDocumentsTickets
                                       where a.DocumentsFilingId == customsDocumentId && a.Tenant == tenant
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
                                       where a.RequestedCustomsDocId == requiredDocID // ??
                                       select a.Id).ToList();

            customsDocumentPointers = (from a in context.CustomsDocumentPointers
                                       where ticketsIds.Contains(a.CustomsDocumentsTicketId)
                                       && a.Tenant == tenant
                                       select a).ToList();

            return customsDocumentPointers;
        }

        public List<CustomsDocumentPointer> GetCustomsDocumentPointerList(GetTicketsParams parameters, int tenant)
        {
            //var q = GetAll(tenant);
            //q = q.Where(a => a.ParentEntityCode == parameters.ParentEntityCode && a.ParentEntityId == parameters.ParentEntityId);

            (context as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            var q = GetAll(tenant);
            q = q.Where(a => a.ParentEntityCode == parameters.ParentEntityCode && a.ParentEntityId == parameters.ParentEntityId);
            var list = q.ToList();
            q=list.AsQueryable<CustomsDocumentPointer>();

            //Devart.Data.Oracle.Entity.OracleFunctions.Trim()
            //System.Data.Entity.SqlServer.SqlFunctions.IsDate

            if(!string.IsNullOrEmpty(parameters.Child1EntityCode2))
                q = q.Where(a => (((a.Child1EntityCode ?? "_IsNull") == (parameters.Child1EntityCode ?? "_IsNull")) || ((a.Child1EntityCode ?? "_IsNull") == (parameters.Child1EntityCode2 ?? "_IsNull"))) && ((a.Child1EntityId ?? "_IsNull") == (parameters.Child1EntityId ?? "_IsNull")));
            else
                q = q.Where(a => ((a.Child1EntityCode ?? "_IsNull") == (parameters.Child1EntityCode ?? "_IsNull")) && ((a.Child1EntityId ?? "_IsNull") == (parameters.Child1EntityId ?? "_IsNull")));

            q = q.Where(a => ((a.Child2EntityCode ?? "_IsNull") == (parameters.Child2EntityCode ?? "_IsNull")) && ((a.Child2EntityId ?? "_IsNull") == (parameters.Child2EntityId ?? "_IsNull")));
            q = q.Where(a => ((a.Child3EntityCode ?? "_IsNull") == (parameters.Child3EntityCode ?? "_IsNull")) && ((a.Child3EntityId ?? "_IsNull") == (parameters.Child3EntityId ?? "_IsNull")));
            q = q.Distinct();
            list= q.ToList();
            //return list.AsQueryable<CustomsDocumentPointer>();
            return list;
        }



        

        public IQueryable<CustomsDocumentPointer> GetCustomsDocumentPointerListParentOnly(GetTicketsParams parameters, int tenant)
        {
            (context as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

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


		public IQueryable<CustomsDocumentPointer> GetCustomDocumentPoinersForClosingData(string parentEntityId,int tenant, string documentFilingId)
		{
			IQueryable<CustomsDocumentPointer> pointers = (from p in context.CustomsDocumentPointers

														   join t in context.CustomsDocumentsTickets

														   on p.CustomsDocumentsTicketId equals t.Id

														   where p.Tenant == tenant && t.DocumentsFilingId == documentFilingId && p.ParentEntityId == parentEntityId 

														   select p);
			return pointers;
		}
	}

}
   