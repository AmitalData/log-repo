using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.EntityKeys;
using Unifreight.Data.AmitalModel.EntityPOCOs;


namespace Unifreight.Data.AmitalModel.Repsitories
{
    public class YCULTASKRepository : IRepository<YCULTASK>
    {
        private AmitalContext currentContext;
        public YCULTASKRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public YCULTASKRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public YCULTASK GetSingle(string TASKID)
        {
            return (from a in context.YCULTASKs
                    where a.TASKID == TASKID
                    select a).FirstOrDefault();
        }

        public IQueryable<YCULTASK> GetAll()
        {
            return from a in context.YCULTASKs
                   select a;
        }

        public void Add(YCULTASK entity)
        {
            context.YCULTASKs.Add(entity);
            SyncRecordCache.ClearCacheLasySyncByPrimaryNum(entity.PRIMARYNUM, entity.TENANT);
        }
        
        public void Remove(YCULTASK entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.YCULTASKs.Attach(entity);
            }
            //context.AddToYCULTASKs 
            context.YCULTASKs.Remove(entity);
            SyncRecordCache.ClearCacheLasySyncByPrimaryNum(entity.PRIMARYNUM, entity.TENANT);
        }

        public void Update(YCULTASK entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.YCULTASKs.Attach(entity); context.SetAsModified(entity);
            }
            SyncRecordCache.ClearCacheLasySyncByPrimaryNum(entity.PRIMARYNUM, entity.TENANT);
        }

        public List<YCULTASK> All()
        {
            return context.YCULTASKs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<YCULTASK> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public YCULTASK GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as YCULTASKKeys;
            return this.GetSingle(keys.TASKID);
        }

        public void GetStatisticWeekly(out int totalTasks, 
            //out int over30sectoanalyze, out int over30secfromlog2start, 
            out int problemTasks)
        {
            int over30sectoanalyze, over30secfromlog2start;
            totalTasks = over30sectoanalyze = over30secfromlog2start = problemTasks = -1;
            //select count(*) from ycultask where log_time > sysdate - 7

            //                select count(*) from ycultask where log_time > sysdate - 7 and PROCESS_END_TIME - PROCESS_START_TIME > 0.0003-- - over 30 sencods to analyze(from end-start)


            //select count(*) from ycultask where log_time > sysdate - 7 and PROCESS_START_TIME - LOG_TIME > 0.0002-- - from log to start

            //                                                                                                          select count(*) from ycultask where log_time > sysdate - 7 and status <> 'C'-- - Tasks with probl


            var weekAgo = DateTime.Now.Date.AddDays(-7);

            var qWeekAgo =
                (from tsk in context.YCULTASKs
                 where
                 tsk.LOGTIME >= weekAgo
                 select tsk);

            var qq1 = (
                 from a in qWeekAgo
                 group a by 1 into gWeek
                 select new
                 {

                     totalTasks = gWeek.Count(),

                     //over30sectoanalyze = gWeek.Count( r=> DbFunctions.DiffSeconds(r.PROCESSENDTIME ,r.PROCESSSTARTTIME)>25),

                     over30sectoanalyze = gWeek.Count(r => DbFunctions.AddMilliseconds(r.PROCESSENDTIME, 25000) > r.PROCESSSTARTTIME),
                     //over30secfromlog2start = gWeek.Count(r => DbFunctions.DiffSeconds(r.PROCESSSTARTTIME ,r.LOGTIME) > 25),

                     over30secfromlog2start = gWeek.Count(r => DbFunctions.AddMilliseconds(r.PROCESSSTARTTIME, 30000) > r.LOGTIME),

                     //problemTasks = gWeek.Count(r => r.STATUS.ToString() != "C")
                 }
                 );
            var qq = (
                 from a in qWeekAgo
                 group a by 1 into gWeek
                 select new
                 {

                     totalTasks = gWeek.Count(),

                     //over30sectoanalyze = gWeek.Count( r=> DbFunctions.DiffSeconds(r.PROCESSENDTIME ,r.PROCESSSTARTTIME)>25),

                     //over30sectoanalyze = gWeek.Count(r => DbFunctions.AddMilliseconds(r.PROCESSENDTIME, 25000) > r.PROCESSSTARTTIME),
                     //over30secfromlog2start = gWeek.Count(r => DbFunctions.DiffSeconds(r.PROCESSSTARTTIME ,r.LOGTIME) > 25),

                     //over30secfromlog2start = gWeek.Count(r => DbFunctions.AddMilliseconds(r.PROCESSSTARTTIME, 30000) > r.LOGTIME),

                     problemTasks = gWeek.Count(r => r.STATUS.ToString() != "C")
                 }
                 );
            //var list1 = qq1.FirstOrDefault();
            var list = qq.FirstOrDefault();
            if (list == null)
            {
                return;
            }
            totalTasks = list.totalTasks;
            //over30sectoanalyze = list.over30sectoanalyze;
            //over30secfromlog2start = list.over30secfromlog2start;
            problemTasks = list.problemTasks;

        }
    }
}
	 