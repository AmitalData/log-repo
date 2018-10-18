using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.Azure.Repositories
{
    public class CommunicationLogsRepository //: IErrorLogsRepository<CommunicationLogsAzure>
    {
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ NOT USED REPOSITORY +++++++++++++++++++++++
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ Will be used later ++++++++++++++++++++++++

           private CommunicationLogsAzureContext tableContext = null;
        string TableName = "";
        //IErrorLogContext objectContext;
        // ErrorLogContext errorLogcontext;

        public CommunicationLogsRepository()
        {
            tableContext = new CommunicationLogsAzureContext();
            TableName = tableContext.TableName;
        }

        public CommunicationLogsRepository(CommunicationLogsAzureContext context)
        {
            tableContext = context;
            TableName = tableContext.TableName;
        }

        public CommunicationLogsAzure GetCommunicationLogs()
        {
            return null;
            //return (from entity in context.CreateQuery<CommunicationLogsAzure>(TableName).OrderBy(d => d.GMTCreateDateTime) select entity).FirstOrDefault();
            //return (from a in context.ErrorLogsEntity

            //        select a).OrderBy(d=>d.LogDateTime).FirstOrDefault();
            //return null;
        }

        public CommunicationLogsAzure GetSingleCommunicationLogs(string id)
        {
            return null;
            //return (from entity in context.CreateQuery<CommunicationLogsAzure>(TableName).Where(a => a.Id == id) select entity).FirstOrDefault();
            //return (from a in context.ErrorLogsEntity
            //        where a.RowKey == id
            //        select a).FirstOrDefault();
            //  return null;
        }


        public IQueryable<CommunicationLogsAzure> GetAllErrorLogs()
        {
            return null;
            //return (from entity in context.CreateQuery<CommunicationLogsAzure>(TableName) select entity);
            //IQueryable<ErrorLogs> errorlogs = context.ErrorLogsEntity;
            //return errorlogs;
            // return null;
        }

        public void Add(CommunicationLogsAzure entity)
        {
            context.Add(entity);
            //context.AddObject(TableName, entity);
            //context.SaveChangesWithRetries();
            //context.ErrorLogsEntity.Add(entity);

        }

        public void Remove(CommunicationLogsAzure entity)
        {
            //context.ErrorLogsEntity.Delete(entity);

        }

        public void Update(CommunicationLogsAzure entity)
        {
            context.Update(entity);
            //context.AttachTo(TableName, entity, null);
            //context.UpdateObject(entity);
            //context.SaveChangesWithRetries(SaveChangesOptions.ReplaceOnUpdate);
            //context.ErrorLogsEntity.Update(entity);
            //context.SetAsModified(entity);

        }

        public List<CommunicationLogsAzure> All()
        {
            return null;
            //return (from entity in context.CreateQuery<CommunicationLogsAzure>(TableName) select entity).ToList();
            //return context.ErrorLogsEntity.ToList();
            //return null;
        }

        public CommunicationLogsAzureContext context
        {
            get { return tableContext; }
        }

        //public void SubmitChanges()
        //{
        //    context.SaveChanges();
        //}
    }
}
