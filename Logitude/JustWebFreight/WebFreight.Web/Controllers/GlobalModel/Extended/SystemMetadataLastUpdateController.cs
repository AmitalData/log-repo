using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.GlobalModel
{
    public class SystemMetadataLastUpdateController : ApiController
    {
        public HttpResponseMessage GetSystemMetadataLastUpdates(int tenant)
        {
            try
            {
                MetaDataLastUpdateDates metadata = new MetaDataLastUpdateDates()
                {
                    Id = 1,
                };

                using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
                {
                    SystemMetadataLastUpdateRepository rep = new SystemMetadataLastUpdateRepository();
                    SystemMetadataLastUpdate update = rep.GetSingleSystemMetadataLastUpdate("1");

                    metadata.ObjectFieldsSystemUpdateDateGMT = (update != null ? update.ObjectFieldsUpdateDateGMT : DateTime.UtcNow);
                    metadata.TranslationsSystemUpdateDateGMT = (update != null ? update.TranslationsUpdateDateGMT : DateTime.UtcNow);

                    scope.Complete();
                }

                IWebFreightContext ObjectContext = WebFreightContext.GetContext(tenant);
                ObjectFieldRepository objectFieldsRepository = new ObjectFieldRepository(ObjectContext);
                TranslationRepository translationRepository = new Simplog.Data.InfrastructureModel.Repositories.TranslationRepository(ObjectContext);

                ObjectFieldModification mod = objectFieldsRepository.GetLastObjectFieldModificationByTenant(tenant);
                metadata.ObjectFieldsTenantUpdateDateGMT = (mod != null ? mod.UpdateDateGMT.Value : new DateTime(2015, 1, 1));

                Translation translation = translationRepository.GetLastTranslationsByTenant(tenant);
                metadata.TranslationsTenantUpdateDateGMT = (translation != null ? translation.UpdateDateGMT.Value : new DateTime(2015, 1, 1));

                return Request.CreateResponse(HttpStatusCode.OK, metadata);


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }
    }
}