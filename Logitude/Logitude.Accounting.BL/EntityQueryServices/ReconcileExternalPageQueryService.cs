
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
using Logitude.Accounting.Data.EntityListQueryServices;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.Accounting.BL.EntityQueryServices
{ 
   public partial class ReconcileExternalPageQueryService
   {
        public override void GetComposition(EntityKeyFields entityKeys, ReconcileExternalPagePM entityPM)
        {
            IAccountingContext context = MainContext as AccountingContext;
            ReconcileExternalPageKeys keys = entityKeys as ReconcileExternalPageKeys;
            ReconcileExternalPageLineQueryService lineQueryService = new ReconcileExternalPageLineQueryService(context);
            entityPM.ReconcileExternalPageLines = lineQueryService.GetMulti(keys, true);

            base.GetComposition(entityKeys, entityPM);
        }

        public ReconcileExternalPagePM GetPageByNumber(int pageNumber, string entityId, string objectTableName, int tenant)
        {
            ObjectTable objectTable = GetObjectTable(objectTableName,tenant);

            ReconcileExternalPagePM myPM = null;
            var temp = repository.GetAll(tenant)
                .Where(a => a.PageNo == pageNumber 
                && a.EntityId == entityId
                && a.ObjectTableId == objectTable.Id
                );
            if (temp != null)
            {
                ReconcileExternalPage MyPoco = temp.FirstOrDefault();
                ReconcileExternalPageKeys keys = new ReconcileExternalPageKeys();
                keys.Id = MyPoco.Id;
                myPM = GetEntityPM(MyPoco, true, keys);
                return myPM;
            }
            else
            {
                return null;
            }

        }
        private ObjectTable GetObjectTable(string tableName, int tenant)
        {
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = objectTableRepository.GetObjectTableByName(tableName, tenant, false);
            if (objectTable == null)
                throw new ApplicationException("No objectfield for " + tableName);
            return objectTable;
        }

        public ReconcileExternalPagePM GetPreviousPageByNumber(int pageNumber, string entityId, string objectTableName, int tenant)
        {
            ObjectTable objectTable = GetObjectTable(objectTableName, tenant);

            ReconcileExternalPagePM myPM = null;
            //&& a.IsApproved == true
            List<ReconcileExternalPage> approvedPagesOrdered = repository.GetAll(tenant)
                .Where(a =>
                 a.EntityId == entityId
                && a.ObjectTableId == objectTable.Id
                && a.StatusCode == "2").OrderBy(a => a.FromDate).ToList() ;

            var list = repository.GetAll(tenant);
            var list2 = list.Where(a =>
                 a.EntityId == entityId
                && a.ObjectTableId == objectTable.Id
                && a.PageNo == pageNumber);

            ReconcileExternalPage currentPage = list2.FirstOrDefault();

            int currentPageIndex = approvedPagesOrdered.IndexOf(currentPage);
            //if(currentPageIndex <= 0) return null; // its not found or first page, no prev pages

            if(currentPageIndex == 0)
                return null;
            else if (currentPageIndex < 0 && approvedPagesOrdered.Count > 0)
                currentPageIndex = (approvedPagesOrdered.Count); // last item
            else if (currentPageIndex < 0 && approvedPagesOrdered.Count <= 0) // by abdullah
                return null;

            ReconcileExternalPage prevPage = approvedPagesOrdered[currentPageIndex - 1];


            if (prevPage != null)
            {
                ReconcileExternalPage MyPoco = prevPage;
                ReconcileExternalPageKeys keys = new ReconcileExternalPageKeys();
                keys.Id = MyPoco.Id;
                myPM = GetEntityPM(MyPoco,true,keys);
                return myPM;
            }
            else
            {
                return null;
            }

        }

        public ReconcileExternalPagePM GetDraftPage(string entityId, string objectTableName, int tenant)
        {
            ObjectTable objectTable = GetObjectTable(objectTableName, tenant);

            ReconcileExternalPagePM myPM = null;
            var temp = repository.GetAll(tenant).Where(a => a.StatusCode == "1"
                && a.EntityId == entityId
                && a.ObjectTableId == objectTable.Id);
            if (temp != null)
            {
                ReconcileExternalPage MyPoco = temp.FirstOrDefault();
                myPM = GetEntityPM(MyPoco);
                return myPM;
            }
            else
            {
                return null;
            }

        }

        public int GetLastPageNumber(string entityId, string objectTableId, int tenant)
        {
            ReconcileExternalPage lastPage = (from a in context.ReconcileExternalPages
                            where
                                a.EntityId == entityId
                                && a.ObjectTableId == objectTableId
                                && a.Tenant == tenant
                            orderby a.PageNo descending
                            select a).FirstOrDefault();

            return lastPage != null ? lastPage.PageNo : 0;
        }
        public bool CheckLastApprovedPage(ReconcileExternalPagePM page, string objectTableName, int tenant)
        {
            //
            ReconcileExternalPagePM reconcileExternalPage = GetLastApprovedPage(page.EntityId, objectTableName, tenant); 

            if (reconcileExternalPage != null && (reconcileExternalPage.Id == page.Id))
            { return true; }
            else return false;
        }

        //public ReconcileExternalPagePM GetLastCancelledBankPage(string bankAccountId, int tenant)
        //{
        //    //
        //    ReconcileExternalPagePM ReconcileExternalPage= GetLastCancelledBankPage(bankAccountId, tenant);

        //    return ReconcileExternalPage;

        //}


        public ReconcileExternalPagePM GetLastApprovedPage(string entityId, string objectTableName, int tenant)
        {
            ObjectTable objectTable = GetObjectTable(objectTableName, tenant);

            ReconcileExternalPage reconcileExternalPage = (from a in context.ReconcileExternalPages
                                                           where
                                                                 a.EntityId == entityId
                                                                && a.ObjectTableId == objectTable.Id 
                                                                && a.Tenant == tenant 
                                                                && a.StatusCode == "2"
                                                           orderby a.PageNo descending
                                                           select a).FirstOrDefault();
            return GetEntityPM(reconcileExternalPage); ;
        }


        public bool CheckNextUnCancelledPage(string bankAccountId, string objectTableName, int pageNo, int tenant)
        {
            
                bool nextUncancelledBankPageExist = ChecNextUnCancelledPages(bankAccountId, objectTableName, pageNo,tenant );
                if (nextUncancelledBankPageExist)
                {
                    return false;

                }
                else
                {
                    return true;
                }
            
          
        }

        //private  List<ReconcileExternalPage> GetBankAccountBankPages(string  bankAccountId, int tenant)
        //{

        //    return (from a in context.ReconcileExternalPages
        //            where a.BankAccountId == bankAccountId && a.Tenant == tenant select a).ToList();


        //}
        //private  bool CheckPreviousBankPageStatus(List<ReconcileExternalPage> reconcileExternalPages, int pageNo)
        //{
        //    pageNo = pageNo - 1;
        //    return (from a in reconcileExternalPages 
        //            where a.StatusCode == "3"   && a.PageNo == pageNo
        //            select a).Any();
        //}

        private bool ChecNextUnCancelledPages(string entityId, string objectTableName, int pageNo, int tenant)
        {
            ObjectTable objectTable = GetObjectTable(objectTableName, tenant);

            return (from a in context.ReconcileExternalPages
                   where  
                   a.PageNo > pageNo 
                   && a.StatusCode != "3" 
                    && a.EntityId == entityId
                    && a.ObjectTableId == objectTable.Id && a.Tenant == tenant

             select a).Any();

        }
    }

}
	 