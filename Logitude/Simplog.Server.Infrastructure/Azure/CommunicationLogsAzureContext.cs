using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Data.Services.Client;

namespace Simplog.Server.Infrastructure.Azure
{
    public class CommunicationLogsAzureContext //: TableServiceContext
    {
        public string TableName = "CommunicationLogsAzure";
        //TableServiceContext serviceContext = null;
        private TableEntity communicationLogsAzures;
        public CloudTable ErrorLogtable;

        public CommunicationLogsAzureContext() 
        {
            ErrorLogtable = StorageAcountDetails.TableClient.GetTableReference(TableName);
            ErrorLogtable.CreateIfNotExists();
            //serviceContext = StorageAcountDetails.TableClient.GetTableServiceContext();
        }


        public TableEntity CommunicationLogsAzureEntity
        {
            get { return communicationLogsAzures; }
        }

        public TableEntity CommunicationLogsAzures
        {
            get { return communicationLogsAzures; }
            set { communicationLogsAzures = value; }
        }

        public void Add(TableEntity entity)
        {
            TableOperation insertOperation = TableOperation.Insert(entity);
            ErrorLogtable.Execute(insertOperation);
            //serviceContext.AddObject(TableName, entity);
            //serviceContext.SaveChangesWithRetries();
        }

        public void Update(TableEntity entity)
        {
            TableOperation updateOperation = TableOperation.Merge(entity);
            ErrorLogtable.Execute(updateOperation);

            //serviceContext.AttachTo(TableName, entity, null);
            //serviceContext.UpdateObject(entity);
            //serviceContext.SaveChangesWithRetries(SaveChangesOptions.ReplaceOnUpdate);
        }
    }

}