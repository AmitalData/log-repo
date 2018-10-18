using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using System.Web;

namespace WebFreight.Web.MetaDataUpdate.SendBox
{
    public class DocumentTypeUpdateClass
    {
        public static void UpdateDataForTenant(int tenant, string message)
        {

            if (tenant == 0)
            {
                throw new Exception("not for zero");

            }
            IWebFreightContext context = WebFreightContext.GetContext(tenant);
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);


            #region Repositories definitions


            ObjectTableRepository objectTableRepository = new ObjectTableRepository(context);
            DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(commonContext);
            DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(documentTypeRepository);
            DocumentTypeCopyRepository documentTypeCopyRepository = new DocumentTypeCopyRepository(commonContext);
            DocumentTypeTemplateRepository documentTypeTemplateRepository = new DocumentTypeTemplateRepository(commonContext);
            DocumentTypeCustomFieldRepository documentTypeCustomFieldRepository = new DocumentTypeCustomFieldRepository(commonContext);

            ObjectFieldRepository objectFieldsRepository = new ObjectFieldRepository(context);

            #endregion

            #region Dictionaries and lists



            Dictionary<string, ObjectTable> tenantZeroObjectTables = objectTableRepository.GetObjectsByTenant(0).ToDictionary(d => d.Name, a => a);
            Dictionary<string, DocumentTypePM> tenantZeroDocumentTypes = documentTypeQuery.GetDocumentTypePMsByTenant(0).ToDictionary(d => d.Code + d.ObjectTableId, a => a);
            Dictionary<string, DocumentType> currentTenantDocumentTypes = documentTypeRepository.GetDocumentTypes(tenant).ToDictionary(d => d.Code + d.ObjectTableId, a => a);
            List<DocumentTypeCustomField> tenantZeroCustomFields = documentTypeCustomFieldRepository.GetDocumentTypeCustomFields(0).ToList();



            #endregion



            using (TransactionScope scop = TransactionFactory.GetNewTransaction(new TimeSpan(2, 5, 0)))//new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(2, 5, 0)))
            {
                #region update methods

                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();


                TenantsUpdateClass.UpdateDocumentTypes(tenant, documentTypeRepository, documentTypeCopyRepository, tenantZeroDocumentTypes, currentTenantDocumentTypes,
                     documentTypeCustomFieldRepository, tenantZeroCustomFields,
                    documentTypeTemplateRepository);


                scop.Complete();

                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                AzureLog.SaveLogsInStorage("Update Tenant " + tenant + "Elapsed Time :" + ts.ToString(), "P", DateTime.Now, "", "", tenant, null, null, null);

                #endregion
            }

        }
    }
}

