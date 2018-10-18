using System.Linq;
using System.ServiceModel.DomainServices.Hosting;
using Microsoft.ServiceModel.DomainServices.WindowsAzure;

using Simplog.Server.Infrastructure.Azure;
using WebFreight.Web.Helpers;
using Simplog.Data.Azure.Repositories;

namespace WebFreight.Web.Azure.AzureCommunicationLog
{
   // [EnableClientAccess]
    public class CommunicationLogsAzureDomainService
    {
        CommunicationLogsAzureContext context;
        string TableName = "";

        public CommunicationLogsAzureDomainService()
        {
            context = new CommunicationLogsAzureContext();
            TableName = context.TableName;
        }

        //public IQueryable<CommunicationLogsAzure> GetCommunicationLogsAzure()
        //{
        //    return (from entity in context.CreateQuery<CommunicationLogsAzure>(TableName) select entity);
        //    //return this.context.CommunicationLogsAzureEntity;
        //}

        public CommunicationLogsAzure GetSingleCommunicationLogsAzure(string id,int tenant)
        {
            var communicationlogQuery = (from entry in context.ErrorLogtable.CreateQuery<CommunicationLogsAzure>()
                                         where entry.Id == id && entry.Tenant == tenant
                                         select entry);

            return communicationlogQuery.FirstOrDefault();
            //return (from entity in context.CreateQuery<CommunicationLogsAzure>(TableName).Where(a => a.Id == id) select entity).FirstOrDefault();
            //return this.context.CommunicationLogsAzureEntity.Where(c => c.Id == id && c.Tenant == tenant).FirstOrDefault();
        }

        public void AddCommunicationLogsAzure(CommunicationLogsAzure entity)
        {
            context.Add(entity);
            //this.EntityContext.CommunicationLogsAzureEntity.Add(entity);
            //this.PersistChangeSet();
        }

        public void DeleteMyChildEntity(CommunicationLogsAzure entity)
        {
            //this.EntityContext.CommunicationLogsAzureEntity.Delete(entity);
            //this.PersistChangeSet();
        }

        public void UpdateMyChildEntity(CommunicationLogsAzure entity)
        {
            context.Update(entity);
        }

        public void SaveChanges()
        {
            //this.context.
        }

       
    }
}