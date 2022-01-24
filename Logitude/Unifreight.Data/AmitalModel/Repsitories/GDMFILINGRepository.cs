
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.EntityKeys;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.Data.AmitalModel.Repsitories
{
    public class GDMFILINGRepository : IRepository<GDMFILING>
    {
        private AmitalContext currentContext;
        public GDMFILINGRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public GDMFILINGRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public GDMFILING GetSingle(string COMID)
        {
            return (from a in context.GDMFILINGs
                    where a.COMID == COMID
                    select a).FirstOrDefault();
        }

        public IQueryable<GDMFILING> GetAll()
        {
            return from a in context.GDMFILINGs
                   select a;
        }

        public void Add(GDMFILING entity)
        {
            context.GDMFILINGs.Add(entity);
        }

        public void Remove(GDMFILING entity)
        {

            AttachIfNot(entity);context.SetAsModified(entity); //context.GDMFILINGs.Attach(entity);
            //context.AddToGDMFILINGs 
            context.GDMFILINGs.Remove(entity);
        }

        public void Update(GDMFILING entity)
        {
            AttachIfNot(entity);context.SetAsModified(entity);

            //context.SetAsModified(entity);
        }
        void AttachIfNot(GDMFILING entity)
        {
            // if ((entity as EntityObject).EntityState == System.Data.EntityState.Unchanged)
            {
                context.GDMFILINGs.Attach(entity);
            }
        }

        public List<GDMFILING> All()
        {
            return context.GDMFILINGs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<GDMFILING> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public GDMFILING GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GDMFILINGKeys;
            return this.GetSingle(keys.COMID);
        }

        public List<GDMFILING> GetNotDeleted(List<string> commIds)
        {
            
            return this.GetAll()
                .Where(r => commIds.Contains(r.COMID))
                .Where(r => r.DELETED != "T").ToList();
        }
        public void GetStatisticWeekly(out int totalSplitedDocsLastMonth, out int logBoxDocuments)
        {
            logBoxDocuments =totalSplitedDocsLastMonth = -1;
            //var toDay = DateTime.Now.Date;//AddMonths(-1);
            //var qToday = (from a in context.GDMFILINGs
            //             where a.OPENDATE >= toDay
            //             select a);

            var week = DateTime.Now.Date.AddDays(-7); ;//AddMonths(-1);
            var qWeek = (from a in context.GDMFILINGs
                          where a.OPENDATE >= week
                         select a);
            var doGroupBy = true;
            if (doGroupBy)
            {
                DbContextBaseUtil.ToLog = false;
                var qGroupIt = (from a in qWeek
                                group a by 1 into groupBy1
                                select new
                                {
                                    totalSplitedDocsLastMonth = groupBy1.Count(a => !//String.IsNullOrWhiteSpace(a.PARENTCOMID)
                    (a.PARENTCOMID == null || a.PARENTCOMID.Trim() == string.Empty)),
                                    logBoxDocuments = groupBy1.Count(a => a.HASSIGN == "T" && a.SIGNMETADATA.Contains("Vat:"))
                                }
                               );
                var res=qGroupIt.FirstOrDefault();
                totalSplitedDocsLastMonth = res.totalSplitedDocsLastMonth;
                logBoxDocuments = res.logBoxDocuments;
            }
            else
            {
                var qRes =
                    (from a in qWeek
                     where !//String.IsNullOrWhiteSpace(a.PARENTCOMID)
                     (a.PARENTCOMID == null || a.PARENTCOMID.Trim() == string.Empty)
                     select a);
                var res = qRes.Count();
                totalSplitedDocsLastMonth = res;
            }
            

            var openReaderSingleResult = new OpenReaderSingleResult(this.currentContext);
            string UserId = openReaderSingleResult.GetSchemaUserId();
            var res1 = openReaderSingleResult.ExecuteReaderSingleResult<int>(
                $"select count(com_id) from {UserId}.gdmfiling where source ='LB' and OPEN_DATE> sysdate-1"
                ,
                (dataReader) =>
                {
                    Int32? val = null;
                    val = dataReader.GetInt32(0);
                    return val;

                });
            logBoxDocuments = res1.GetValueOrDefault();
        }

        
    }
}
	 