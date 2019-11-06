using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Microsoft.Practices.Unity;
using System.Text;

namespace WebFreight.Web.Helpers
{
    public class BluesnapExecutionService
    {
        private DocumentRepository documentRepository;
        private int tenant;
        private string documentId;
        public BluesnapExecutionService(int tenant) {
            this.tenant = tenant;
            this.documentRepository = new DocumentRepository(tenant);
        }

        public void SaveBluesnapTransaction(string file, string type,DateTime? TransactionDate)
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                this.SaveDocument(file);

                using (TransactionScope globalScope = TransactionFactory.GetNewTransaction())
                {
                    BluesnapTransactionRepository bluesnapTransactionRepository = new BluesnapTransactionRepository();
                    BluesnapTransaction transaction = new BluesnapTransaction()
                    {
                        CreateDate = DateTime.Now,
                        DocumentId = documentId,
                        LogitudeAmital = type,
                        Tenant = Convert.ToInt32(tenant),
                        Id = IdCounter.GetNumber("BluesnapTransaction", tenant),
                        TransactionDate = TransactionDate,
                        
                    };

                    bluesnapTransactionRepository.Add(transaction);
                    bluesnapTransactionRepository.SubmitChanges();
                    globalScope.Complete();
                }

                scope.Complete();

            }
        }

        public int Tenant
        {
            get { return tenant; }

            set { tenant = value; }

        }

        private void SaveDocument(string file) {            
            Document document = new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "xml",
                FileSize = file.Length,
                Tenant = Convert.ToInt32(tenant),
                Id = IdCounter.GetNumber("Document", tenant),
                HasFile = true,
                Folder = "bluesnap",
                
                
            };




            documentId = document.Id;
            documentRepository.Add(document);
            documentRepository.SubmitChanges();
            byte[] myByteArray = Encoding.ASCII.GetBytes(file);

            Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
            {
                FileName = documentId,
                FolderName = "bluesnap",
                Extension = "xml",
                Tenant = tenant,
                FileSize = myByteArray.Length,
            };

            Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            storageservice.Write(myByteArray, fileInfo);

        }

        public int GetUserTenantByEmail(string email)
        {
            GlobalContactRepository globalContactRepository = new GlobalContactRepository();
            GlobalContact globalContact = globalContactRepository.GetContactByEmail(email).FirstOrDefault();
            if (globalContact != null)
            {
                return globalContact.GlobalTenantId;
            }
            return 0;
        }


    }
}