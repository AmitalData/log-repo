using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.Validating;
using Logitude.BL.InfrastructureModel.Tools.TraceEvents;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class QueryService
    {

        bool isNewEntity;
        private int tenant;
        public Query Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private QueryPM entityPM;
        private IWebFreightContext objectContext;
        private QueryRepository entityRepository;
        private SharedUserQueryRepository sharedUserQueryRepository;
        public QueryService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new QueryRepository(objectContext);
            this.sharedUserQueryRepository = new SharedUserQueryRepository(objectContext);
        }

        public void Create(QueryPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("Query", tenant).ToString();
            this.entityPM.Code = this.entityPM.Id;
            this.Poco = new Query();
            this.Poco.Id = this.entityPM.Id;
            
            if (!string.IsNullOrEmpty(theEntityPm.NewViewName))
            {
                ObjectTableRepository tableRep = new ObjectTableRepository(tenant);
                ObjectTable table = tableRep.GetSingleObjectTable(theEntityPm.ObjectTableId,0,true);
                TextCodeRepository textCodeRep = new TextCodeRepository(objectContext);

                TextCode textCode = new TextCode()
                {
                    Id = IdCounter.GetNumber("TextCode", tenant).ToString(),
                    ObjectTableId = theEntityPm.ObjectTableId,
                    TextCodeTypeCode = "Q",
                    IsSpellChecked = false,
                    InActive = false,
                    Tenant = theEntityPm.Tenant,
                    DefaultText = theEntityPm.NewViewName,
                    Code = table.Name + ".Q." + theEntityPm.Id,
                };

                textCodeRep.Add(textCode);
                theEntityPm.NameTextCodeId = textCode.Id;
                theEntityPm.NameTextCodeCode = textCode.Code;

                string tenantCodesListName = "tenanttextcodes" + theEntityPm.Tenant;
                string zeroCodeslistName = "tenantzerotextcodes";

                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(tenantCodesListName) != null)
                    {
                        CacheManager.CacheWrapper.Invalidate(tenantCodesListName);
                    }
                    if (CacheManager.CacheWrapper.Get(zeroCodeslistName) != null)
                    {
                        CacheManager.CacheWrapper.Invalidate(zeroCodeslistName);
                    }
                }
            }

            foreach (SharedUserQueryPM itemPM in theEntityPm.SharedUserQueries)
            {
                this.CreateSharedUserQuery(itemPM);
            }

            QueryValidating.Validate(theEntityPm);
            QueryTracing.Trace(theEntityPm, Poco, isNewEntity);
            QueryMapping.MapEntity(theEntityPm, Poco, isNewEntity);

            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        private List<SharedUserQueryPM> sharedUserQueriesChangeSet;
        public void Update(QueryPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleQuery(theEntityPm.Id);
            
            QueryValidating.Validate(theEntityPm);

            this.sharedUserQueriesChangeSet = theEntityPm.SharedUserQueries;
            this.UpdateSharedUserQueriesCollection();

            QueryTracing.Trace(theEntityPm, Poco, isNewEntity);

            TextCodeRepository textCodeRep = new TextCodeRepository(objectContext);
            TextCode textCode = textCodeRep.GetSingleTextCodeByTenant(theEntityPm.NameTextCodeId, theEntityPm.Tenant);
            if (textCode != null && textCode.DefaultText != theEntityPm.NewViewName)
            {
                textCode.DefaultText = theEntityPm.NewViewName;
                textCode.DefaultTextPlural = theEntityPm.NewViewName;
                textCode.LocalDefaultText = theEntityPm.NewViewName;

                textCodeRep.Update(textCode);
                textCodeRep.SubmitChanges();

                string tenantCodesListName = "tenanttextcodes" + theEntityPm.Tenant;
                string zeroCodeslistName = "tenantzerotextcodes";

                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(tenantCodesListName) != null)
                    {
                        CacheManager.CacheWrapper.Invalidate(tenantCodesListName);
                    }
                    if (CacheManager.CacheWrapper.Get(zeroCodeslistName) != null)
                    {
                        CacheManager.CacheWrapper.Invalidate(zeroCodeslistName);
                    }
                }
            }

            QueryMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

        private void UpdateSharedUserQueriesCollection()
        {
            if (sharedUserQueriesChangeSet != null)
            {
                foreach (SharedUserQueryPM itemPM in sharedUserQueriesChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateSharedUserQuery(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateSharedUserQuery(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteSharedUserQuery(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }
        private void CreateSharedUserQuery(SharedUserQueryPM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("SharedUserQuery", tenant).ToString();
            itemPM.QueryId = this.entityPM.Id;
            itemPM.Tenant = tenant;           

            SharedUserQuery itemPoco = new SharedUserQuery()
            {
                Id = itemPM.Id,
                QueryId = itemPM.QueryId,
                Tenant = tenant
            };

            SharedUserQueryMapping.MapEntity(itemPM, itemPoco, true);
            sharedUserQueryRepository.Add(itemPoco);
        }
        private void UpdateSharedUserQuery(SharedUserQueryPM itemPM)
        {
            SharedUserQuery itemPoco = sharedUserQueryRepository.GetSingleSharedUserQuery(itemPM.Id, tenant);
            if (itemPoco != null)
            {
                SharedUserQueryMapping.MapEntity(itemPM, itemPoco, false);
                sharedUserQueryRepository.Update(itemPoco);
            }
        }
        private void DeleteSharedUserQuery(SharedUserQueryPM itemPM)
        {
            SharedUserQuery itemPoco = sharedUserQueryRepository.GetSingleSharedUserQuery(itemPM.Id, tenant);

            if (itemPoco != null)
            {
                sharedUserQueryRepository.Remove(itemPoco);
            }
        }
    }
}