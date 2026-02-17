
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

        public ReconcileExternalPagePM GetBankPageByPageNo(int pageNumber, string bankAccountId, int tenant)
        {
            ReconcileExternalPagePM myPM = null;
            var temp = repository.GetAll(tenant).Where(a => a.PageNo == pageNumber && a.BankAccountId == bankAccountId);
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

        public ReconcileExternalPagePM GetPrevPageNoByPageNo(int pageNumber, string bankAccountId, int tenant)
        {
            ReconcileExternalPagePM myPM = null;
            //&& a.IsApproved == true
            List<ReconcileExternalPage> approvedPagesOrdered = repository.GetAll(tenant).Where(a => a.BankAccountId == bankAccountId && a.StatusCode == "2").OrderBy(a => a.FromDate).ToList() ;

            var list = repository.GetAll(tenant);
            var list2 = list.Where(a => a.BankAccountId == bankAccountId && a.PageNo == pageNumber);
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

        public ReconcileExternalPagePM GetDraftPage(string bankAccountId, int tenant)
        {
            ReconcileExternalPagePM myPM = null;
            var temp = repository.GetAll(tenant).Where(a => a.StatusCode == "1" && a.BankAccountId == bankAccountId);
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

        public int GetLastPageNo(string bankAccountId, int tenant)
        {
            //
            var lastPage = (from a in context.ReconcileExternalPages
                            where a.BankAccountId == bankAccountId && a.Tenant == tenant
                            orderby a.PageNo descending
                            select a).FirstOrDefault();
            if (lastPage != null)
                return lastPage.PageNo;
            else
                return 0;

        }

    }

}
	 