using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;


namespace Simplog.Data.CommonDataModel.Repositories
{
    
    public class CommunicationLogStepRepository:IRepository<CommunicationLogStep>
    {
        ICommonDataContext commonDataContext;

        public CommunicationLogStepRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CommunicationLogStepRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CommunicationLogStepRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<CommunicationLogStep> GetCommunicationSteps(int tenant)
        {
            return (from record in context.CommunicationLogSteps where record.Tenant == tenant select record);
        }

      


        public CommunicationLogStep CommunicationLogStep(string id, int StepNumber, int tenant)
        {
           return (from a in context.CommunicationLogSteps 
                       .Include("CommunicationStatusType")
                       .Include("Document")
                   where a.CommunicationLogId == id && a.StepNumber == StepNumber
                    select a).FirstOrDefault();

        }


        public IQueryable<CommunicationLogStep> GetCommunicationStepsForCommLog(string comLogId, int tenant)
        {
            return (from a in context.CommunicationLogSteps.Include("CommunicationStatusType") .Include("Document")
                    where a.Tenant == tenant && a.CommunicationLogId == comLogId
                    select a);
        }
       

        public void Add(CommunicationLogStep entity)
        {
            CompressUpSert(entity);
            context.CommunicationLogSteps.Add(entity);
        }

        private static void CompressUpSert(CommunicationLogStep entity)
        {
            if (!entity.IsLogCompress && !String.IsNullOrWhiteSpace(entity.Log))
            {

                entity.Log = InjectionUtil.Instance.CompressText(entity.Log);
                entity.IsLogCompress = true;
            }
        }

        public void Remove(CommunicationLogStep entity)
        {
            context.CommunicationLogSteps.Attach(entity);
            context.CommunicationLogSteps.Remove(entity);
        }

        public void Update(CommunicationLogStep entity)
        {
            try
            {
                CompressUpSert(entity);

                context.CommunicationLogSteps.Attach(entity);
               
            }
            catch
            { }

            context.SetAsModified(entity);
        }

        public List<CommunicationLogStep> All()
        {
            return context.CommunicationLogSteps.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CommunicationLogStep> GetMultiCommunicationLog(string id, int tenant)
        {
            return (from a in context.CommunicationLogSteps
                       .Include("CommunicationStatusType")
                       .Include("Document")
                    where a.CommunicationLogId == id
                    orderby a.StepNumber //MUST !!!
                    select a).ToList();

        }
        public List<CommunicationLogStep> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CommunicationLog GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }


        EntityPOCOs.CommunicationLogStep Server.Infrastructure.IRepository<EntityPOCOs.CommunicationLogStep>.GetSingle(Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<CommunicationLogStep> GetQMultiCommunicationLog(string id, int tenant)
        {
            return (from a in context.CommunicationLogSteps
                       .Include("CommunicationStatusType")
                       .Include("Document")
                    where a.CommunicationLogId == id
                    orderby a.StepNumber //MUST !!!
                    select a);

        }
        public IQueryable<string> Get104921()
        {
            //25 - נובמבר - 2019
            //104921 ==select  count(*) from CommunicationLogs where createdate_> sysdate -10
            DateTime sdateTime = new DateTime(2019, 11, 25);
            DateTime edateTime = new DateTime(2019, 12, 05);
            var q=
            context.CommunicationLogs
                .Where(r => r.CreateDate > sdateTime)
                .Where(r => r.CreateDate <= edateTime)
                ///..Take(1000*200)
                //104921 ==select  count(*) from CommunicationLogs where createdate_> sysdate -10
                .Select(r=>r.Id);
            return q;
        }
    }
}


