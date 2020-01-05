using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.InfrastructureModel
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Core.EntityClient;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using System.Transactions;
    using System.Web;
    using System.Xml.Serialization;
    using WebFreight.Web.Azure;
    using Simplog.Server.Infrastructure.Helpers;
    using System.Threading;
    using Logitude.SystemLogs;
    using Logitude.Server.Tools.Counters;
    using Simplog.Server.Infrastructure;
    using Simplog.Server.Infrastructure.DataContracts;
    using WebFreight.Web.GlobalModel;
    using Simplog.Global.Data.GlobalModel.EntityPOCOs;
    using System.Data.Entity.Validation;
    using Logitude.BL.InfrastructureModel.EntityQueries;
    using Logitude.BL.CommonDataModel.EntityQueries;
    using Logitude.BL.InfrastructureModel.EntityPMs;
    using Logitude.BL.CommonDataModel.EntityPMs;
    using Logitude.BL.InfrastructureModel.Tools.EntityService;
    using Logitude.BL.InfrastructureModel.EntityLists;
    using Simplog.Global.Data.GlobalModel.Repositories;
    using Logitude.BL.Helpers;

    // TODO: Create methods containing your application logic.
    // new comment
   // [RequiresAuthentication]
    [EnableClientAccess()]                
    public class GeneralDomainService : LogitudeDomainService
    {
        private UserData currentUser;
        public int LastTenantEntered { get; set; }
        public IWebFreightContext ObjectContext { get; set; }
        public ObjectTableRepository ObjectTableRepository { get; set; }
        public ObjectFieldRepository ObjectFieldsRepository { get; set; }
        public TextCodeRepository TextCodeRepository { get; set; }
        public TranslationRepository TranslationRepository { get; set; }
        public TranslationHeaderRepository TranslationHeaderRepository { get; set; }
        public DataTypeRepository DataTypeRepository { get; set; }
        public ScreensRepository ScreensRepository { get; set; }
        public ScreenFieldsRepository ScreenFieldsRepository { get; set; }
        public TextCodeTypesRepository TextCodeTypesRepository { get; set; }
        public FieldDataTypesRepository FieldDataTypesRepository { get; set; }
        public QueryRepository QueriesRepository { get; set; }
        public QueryColumnRepository QueryColumnsRepository { get; set; }        
        public CustomTableRepository CustomTablesRepository { get; set; }
        public MenusTableRepository MenusTablesRepository { get; set; }
        public MenuTypeRepository MenuTypesRepository { get; set; }
        public CategoryTypeRepository CategoryTypesRepository { get; set; }
        public AdvancedQueryFilterRepository AdvancedQueryFiltersRepository { get; set; }
        public ObjectTableTabRepository ObjectTableTabsRepository { get; set; }
        public ObjectTableHelperControlRepository ObjectTableHelperControlsRepository { get; set; }
        public MenuButtonGroupRepository MenuButtonGroupRepository { get; set; }
        public MenuButtonRepository MenuButtonRepository { get; set; }
        public ObjectFieldValidationRepository ObjectFieldValidationRepository { get; set; }
      

        ObjectTableRuleRepository objectTableRuleRepository;
        public ObjectTableRuleRepository ObjectTableRuleRepository
        {
            get { return objectTableRuleRepository; }
            set { objectTableRuleRepository = value; }
        }

        ObjectTableRuleFieldRepository objectTableRuleFieldRepository;
        public ObjectTableRuleFieldRepository ObjectTableRuleFieldRepository
        {
            get { return objectTableRuleFieldRepository; }
            set { objectTableRuleFieldRepository = value; }
        }

        RuleTypeRepository ruleTypeRepository;
        public RuleTypeRepository RuleTypeRepository
        {
            get { return ruleTypeRepository; }
            set { ruleTypeRepository = value; }
        }

        TipRepository tipRepository;
        public TipRepository TipRepository
        {
            get { return tipRepository; }
            set { tipRepository = value; }
        }

        TipsVisibilityRepository tipsVisibilityRepository;
        public TipsVisibilityRepository TipsVisibilityRepository
        {
            get { return tipsVisibilityRepository; }
            set { tipsVisibilityRepository = value; }
        }

        TriggerTypeRepository triggerTypeRepository;
        public TriggerTypeRepository TriggerTypeRepository
        {
            get { return triggerTypeRepository; }
            set { triggerTypeRepository = value; }
        }

        RuleNotificationTypeRepository ruleNotificationTypeRepository;
        public RuleNotificationTypeRepository RuleNotificationTypeRepository
        {
            get { return ruleNotificationTypeRepository; }
            set { ruleNotificationTypeRepository = value; }
        }

        CounterStatRepository counterStatRepository;
        public CounterStatRepository CounterStatRepository
        {
            get { return counterStatRepository; }
            set { counterStatRepository = value; }
        }

        CounterDefinitionRepository counterDefinitionRepository;
        public CounterDefinitionRepository CounterDefinitionRepository
        {
            get { return counterDefinitionRepository; }
            set { counterDefinitionRepository = value; }
        }

        CounterRepository counterRepository;
        public CounterRepository CounterRepository
        {
            get { return counterRepository; }
            set { counterRepository = value; }
        }

        TenantSettingRepository tenantSettingRepository;
        public TenantSettingRepository TenantSettingRepository
        {
            get { return tenantSettingRepository; }
            set { tenantSettingRepository = value; }
        }

        RuleConditionFieldRepository ruleConditionFieldRepository;
        public RuleConditionFieldRepository RuleConditionFieldRepository
        {
            get { return ruleConditionFieldRepository; }
            set { ruleConditionFieldRepository = value; }
        }
         
        public QueryGroupRepository QueryGroupRepository { get; set; }



        private AdvancedQueryFilterQuery advancedQueryFilterQuery;
        private CounterDefinitionQuery counterDefinitionQuery;
        private CounterQuery counterQuery;
        private MenuButtonGroupQuery menuButtonGroupQuery;
        private MenusTableQuery menusTableQuery;
        private ObjectFieldQuery objectFieldsQuery;
        private ObjectTableRuleFieldQuery objectTableRuleFieldQuery;
        private ObjectTableRuleQuery objectTableRuleQuery;
        private ObjectTableTabQuery objectTableTabQuery;
        private QueryColumnQuery queryColumnQuery;
        private QueryQuery queryQuery;
        private ScreenFieldsQuery screenFieldsQuery;
        private ScreensQuery screensQuery;
        private TenantSettingQuery tenantSettingQuery;
        private TextCodeQuery textCodeQuery;
        private TipQuery tipQuery;
        private TipsVisibilityQuery tipsVisibilityQuery;



        public GeneralDomainService(UserData currentuser)
        {
            //CurrentUser = _currentuser;
            //ObjectContext = new WebFreightContext();
            //InitializeRepositories();
        }

        public GeneralDomainService()
        {
            //ObjectContext = new WebFreightContext();
            //InitializeRepositories();
        }

        public GeneralDomainService(IWebFreightContext context)
        {
            ObjectTableRepository = new ObjectTableRepository(context);
            ObjectFieldsRepository = new ObjectFieldRepository(context);
            TextCodeRepository = new TextCodeRepository(context);
            TranslationRepository = new TranslationRepository(context);
            TranslationHeaderRepository = new TranslationHeaderRepository(context);
            DataTypeRepository = new DataTypeRepository(context);
            ScreensRepository = new ScreensRepository(context);
            ScreenFieldsRepository = new ScreenFieldsRepository(context);
            TextCodeTypesRepository = new TextCodeTypesRepository(context);
            FieldDataTypesRepository = new FieldDataTypesRepository(context);
            QueriesRepository = new QueryRepository(context);
            QueryColumnsRepository = new QueryColumnRepository(context);            
            CustomTablesRepository = new CustomTableRepository(context);
            MenusTablesRepository = new MenusTableRepository(context);
            MenuTypesRepository = new MenuTypeRepository(context);
            CategoryTypesRepository = new CategoryTypeRepository(context);
            AdvancedQueryFiltersRepository = new AdvancedQueryFilterRepository(context);
            ObjectTableTabsRepository = new ObjectTableTabRepository(context);
            ObjectTableHelperControlsRepository = new ObjectTableHelperControlRepository(context);
            MenuButtonGroupRepository = new MenuButtonGroupRepository(context);
            MenuButtonRepository = new MenuButtonRepository(context);
            CounterDefinitionRepository = new CounterDefinitionRepository(context);
            ObjectFieldValidationRepository = new ObjectFieldValidationRepository(context);
            ObjectTableRuleRepository = new ObjectTableRuleRepository(context);
            ObjectTableRuleFieldRepository = new ObjectTableRuleFieldRepository(context);
            RuleTypeRepository = new RuleTypeRepository(context);
            QueryGroupRepository = new QueryGroupRepository(context);
            TriggerTypeRepository = new TriggerTypeRepository(context);
            RuleNotificationTypeRepository = new RuleNotificationTypeRepository(context);
            CounterRepository = new CounterRepository(context);
            TenantSettingRepository = new TenantSettingRepository(context);
            CounterStatRepository = new CounterStatRepository(context);
            TipRepository = new TipRepository(context);
            TipsVisibilityRepository = new TipsVisibilityRepository(context);

            RuleConditionFieldRepository = new RuleConditionFieldRepository(context);
        }

        private void InitializeRepositories()
        {
            ObjectTableRepository = new ObjectTableRepository(ObjectContext);
            ObjectFieldsRepository = new ObjectFieldRepository(ObjectContext);
            TextCodeRepository = new TextCodeRepository(ObjectContext);
            TranslationRepository = new TranslationRepository(ObjectContext);
            TranslationHeaderRepository = new TranslationHeaderRepository(ObjectContext);
            DataTypeRepository = new DataTypeRepository(ObjectContext);
            ScreensRepository = new ScreensRepository(ObjectContext);
            ScreenFieldsRepository = new ScreenFieldsRepository(ObjectContext);
            TextCodeTypesRepository = new TextCodeTypesRepository(ObjectContext);
            FieldDataTypesRepository = new FieldDataTypesRepository(ObjectContext);
            QueriesRepository = new QueryRepository(ObjectContext);
            QueryColumnsRepository = new QueryColumnRepository(ObjectContext);            
            CustomTablesRepository = new CustomTableRepository(ObjectContext);
            MenusTablesRepository = new MenusTableRepository(ObjectContext);
            MenuTypesRepository = new MenuTypeRepository(ObjectContext);
            CategoryTypesRepository = new CategoryTypeRepository(ObjectContext);
            AdvancedQueryFiltersRepository = new AdvancedQueryFilterRepository(ObjectContext);
            ObjectTableTabsRepository = new ObjectTableTabRepository(ObjectContext);
            ObjectTableHelperControlsRepository = new ObjectTableHelperControlRepository(ObjectContext);
            MenuButtonGroupRepository = new MenuButtonGroupRepository(ObjectContext);
            MenuButtonRepository = new MenuButtonRepository(ObjectContext);
            CounterDefinitionRepository = new CounterDefinitionRepository(ObjectContext);
            ObjectFieldValidationRepository = new ObjectFieldValidationRepository(ObjectContext);
            ObjectTableRuleRepository = new ObjectTableRuleRepository(ObjectContext);
            ObjectTableRuleFieldRepository = new ObjectTableRuleFieldRepository(ObjectContext);
            RuleTypeRepository = new RuleTypeRepository(ObjectContext);
            QueryGroupRepository = new QueryGroupRepository(ObjectContext);
            TriggerTypeRepository = new TriggerTypeRepository(ObjectContext);
            RuleNotificationTypeRepository = new RuleNotificationTypeRepository(ObjectContext);
            CounterRepository = new CounterRepository(ObjectContext);
            TenantSettingRepository = new TenantSettingRepository(ObjectContext);
            CounterStatRepository = new CounterStatRepository(ObjectContext);
            TipRepository = new TipRepository(ObjectContext);
            TipsVisibilityRepository = new TipsVisibilityRepository(ObjectContext);

            RuleConditionFieldRepository = new RuleConditionFieldRepository(ObjectContext);
        }

        #region Get,insert,update ,delete domain service methods

        #region Tips
        public List<TipPM> GetTips(int tenant)
        {
            TipRepository = new TipRepository(tenant);
            //this.ChangeConnectionString(tenant);
            tipQuery = new TipQuery(TipRepository);
            return tipQuery.GetTipsPMs();
        }

        public void InsertTip(TipPM newEntity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(newEntity.Tenant);
            }
            TipService service = new TipService(ObjectContext, newEntity.Tenant);
            service.Create(newEntity);

            //TipRepository = new TipRepository(ObjectContext);
            //tipQuery = new TipQuery(TipRepository);
            //bool exists = tipQuery.GetSingleTipPM(newEntity.Code, newEntity.Tenant) != null ? true : false;
            
            //if (!exists)
            //{
            //    Tip newTip = new Tip();
            //    MapTipPMTip(newEntity, newTip);
            //    TipRepository.Add(newTip);
            //}
            //else
            //{
            //    throw new Exception("This Entity Already exists!");
            //}
        }

        //public void MapTipPMTip(TipPM tipPM, Tip tip)
        //{
        //    tip.Code = tipPM.Code;
        //    tip.ShortTextCode = tipPM.ShortTextCodeId;
        //    tip.Tenant = tipPM.Tenant;
        //    tip.VisibilityDefaultValue = tipPM.VisibilityDefaultValue;
        //    tip.ObjectTableId = tipPM.ObjectTableId;  
        //}

        public void UpdateTip(TipPM currentEntity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(currentEntity.Tenant);
            }

            TipService service = new TipService(ObjectContext, currentEntity.Tenant);
            service.Update(currentEntity);

            //TipRepository = new TipRepository(ObjectContext);
            //Tip tip = TipRepository.GetSingleTip(currentEntity.Code, currentEntity.Tenant);

            //MapTipPMTip(currentEntity, tip);
            //TipRepository.Update(tip);            
        }

        public void DeleteTip(TipPM entity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            TipRepository = new TipRepository(ObjectContext);
            Tip tip = TipRepository.GetSingleTip(entity.Code, entity.Tenant);
            if (tip != null)
            {
                TipRepository.Remove(tip);
            }
        }
        #endregion

        #region TipVisibilities
        public List<TipsVisibilityPM> GetTipVisibilities(int tenant,string userId)
        {
            TipsVisibilityRepository = new TipsVisibilityRepository(tenant);
            //this.ChangeConnectionString(tenant);
            tipsVisibilityQuery = new TipsVisibilityQuery(TipsVisibilityRepository);
            return tipsVisibilityQuery.GetTipsVisibilities(tenant, userId);
        }

        public void InsertTipsVisibility(TipsVisibilityPM newEntity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(newEntity.Tenant);
            }
            TipsVisibilityService service = new TipsVisibilityService(ObjectContext, newEntity.Tenant);          

            TipsVisibilityRepository = new TipsVisibilityRepository(ObjectContext);
            bool exists = TipsVisibilityRepository.GetSingleTipsVisibility(newEntity.Id, newEntity.Tenant) != null ? true : false;

            if (!exists)
            {
                service.Create(newEntity);

            }
            else
            {
                throw new Exception("This Entity Already exists!");
            }
        }

        public void MapTipsVisibilityPMTipVisibility(TipsVisibilityPM tipPM, TipsVisibility tip)
        {
            tip.IsVisible = tipPM.IsVisible;
            tip.UserId = tipPM.UserId;
            tip.Tenant = tipPM.Tenant;
            tip.TipCode = tipPM.TipCode;
        }

        public void UpdateTipsVisibility(TipsVisibilityPM currentEntity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(currentEntity.Tenant);
            }

            TipsVisibilityService service = new TipsVisibilityService(ObjectContext, currentEntity.Tenant);
            service.Update(currentEntity);

            //TipsVisibilityRepository = new TipsVisibilityRepository(ObjectContext);
            //TipsVisibility tip = TipsVisibilityRepository.GetSingleTipsVisibility(currentEntity.Id, currentEntity.Tenant);

            //MapTipsVisibilityPMTipVisibility(currentEntity, tip);
            //TipsVisibilityRepository.Update(tip);
        }

        public void DeleteTipsVisibility(TipsVisibilityPM entity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            TipsVisibilityRepository = new TipsVisibilityRepository(ObjectContext);
            TipsVisibility tip = TipsVisibilityRepository.GetSingleTipsVisibility(entity.Id, entity.Tenant);

            if (tip != null)
            {
                TipsVisibilityRepository.Remove(tip);
            }
        }
        #endregion

        #region RuleNotificationTypes
        public IQueryable<RuleNotificationType> GetRuleNotificationTypes(int tenant)
        {
            RuleNotificationTypeRepository = new RuleNotificationTypeRepository(tenant);
            return RuleNotificationTypeRepository.GetRuleNotificationTypes();
        }

        public void InsertRuleNotificationType(RuleNotificationType newEntity)
        {
            bool exists = RuleNotificationTypeRepository.GetRuleNotificationTypeByCode(newEntity.Code) != null ? true : false;
            
            if (!exists)
            {
                RuleNotificationTypeRepository.Add(newEntity);
            }
            else
            {
                throw new Exception("This Entity Already exists!");
            }
        }

        public void UpdateRuleNotificationType(RuleNotificationType currentEntity)
        {
            bool exists = RuleNotificationTypeRepository.GetRuleNotificationTypes().Where(r => r.Code == currentEntity.Code && r.Name != currentEntity.Name).Any();
            if (!exists)
            {
                RuleNotificationTypeRepository.Update(currentEntity);
            }
            else
            {
                throw new Exception("This Entity Already exists!");
            }
        }

        public void DeleteRuleNotificationType(RuleNotificationType entity)
        {
            RuleNotificationTypeRepository.Remove(entity);
        }
        #endregion

        #region TriggerTypes
        public IQueryable<TriggerType> GetTriggerTypes(int tenant)
        {
            TriggerTypeRepository = new TriggerTypeRepository(tenant);
            return TriggerTypeRepository.GetTriggerTypes();
        }

        public void InsertTriggerType(TriggerType newEntity)
        {
            bool exists = TriggerTypeRepository.GetTriggerTypeByCode(newEntity.Code) != null ? true : false;
            
            if (!exists)
            {
                TriggerTypeRepository.Add(newEntity);
            }
            else
            {                
                throw new Exception("This Entity Already exists!");
            }
        }

        public void UpdateTriggerType(TriggerType currentEntity)
        {
            bool exists = TriggerTypeRepository.GetTriggerTypes().Where(r => r.Code == currentEntity.Code && r.Name != currentEntity.Name).Any();
            if (!exists)
            {
                TriggerTypeRepository.Update(currentEntity);
            }
            else
            {
                throw new Exception("This Entity Already exists!");
            }
        }

        public void DeleteTriggerType(TriggerType entity)
        {
            TriggerTypeRepository.Remove(entity);
        }
        #endregion

        #region ObjectTableRules
        public IQueryable<ObjectTableRule> GetObjectTableRules()
        {
            //this.ChangeConnectionString(tenant);
            ObjectTableRuleRepository = new ObjectTableRuleRepository(0);
            return ObjectTableRuleRepository.GetObjectTableRules(0);
        }

        public List<ObjectTableRulePM> GetObjectTableRulePMsByTenant(int tenant)
        {
            ObjectTableRuleRepository = new ObjectTableRuleRepository(tenant);
            //this.ChangeConnectionString(tenant);
            objectTableRuleQuery = new ObjectTableRuleQuery(ObjectTableRuleRepository);
            return objectTableRuleQuery.GetObjectTableRulePMsByTenant(tenant);
        }

        public void InsertObjectTableRule(ObjectTableRulePM newEntity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(newEntity.Tenant);
            }
            ObjectTableRuleService service = new ObjectTableRuleService(ObjectContext, newEntity.Tenant);
            service.Create(newEntity);


            //ObjectTableRuleRepository = new ObjectTableRuleRepository(ObjectContext);
            //RuleTypeRepository = new RuleTypeRepository(ObjectContext);
            //RuleConditionFieldRepository = new RuleConditionFieldRepository(ObjectContext);

            //ObjectTableRule newObjectTableRule = new ObjectTableRule();
            //newObjectTableRule.Id = IdCounter.GetNumber("ObjectTableRule", newEntity.Tenant).ToString();
            //newEntity.Id = newObjectTableRule.Id;
            //newEntity.RuleTypeName = RuleTypeRepository.GetRuleTypeByCode(newEntity.RuleTypeCode).Name;
            //MapObjectTableRulePMObjectTableRule(newEntity, newObjectTableRule);
            //if (newObjectTableRule.RuleCode == null)
            //{
            //    newObjectTableRule.RuleCode = newObjectTableRule.Id;
            //}


            //objectTableRuleQuery = new ObjectTableRuleQuery(ObjectTableRuleRepository);

            //if (objectTableRuleQuery.GetObjectTableRulePMsByTenant(newEntity.Tenant).Where(r => r.RuleCode == newObjectTableRule.RuleCode).FirstOrDefault() == null)
            //{
            //    ObjectTableRuleRepository.Add(newObjectTableRule);
            //    ObjectTableRuleRepository.SubmitChanges();

            //    if (newEntity.RuleConditionFields != null)
            //    {
            //        foreach (RuleConditionFieldPM condFieldPM in newEntity.RuleConditionFields)
            //        {
            //            RuleConditionField condField = new RuleConditionField();
            //            condField.Id = IdCounter.GetNumber("RuleConditionField", newEntity.Tenant).ToString();
            //            condFieldPM.Id = condField.Id;
            //            condFieldPM.ObjectTableRuleId = newObjectTableRule.Id;
            //            MapRuleConditionFieldPMRuleConditionField(condFieldPM, condField);
            //            RuleConditionFieldRepository.Add(condField);
            //        }
            //    }
            //}
            //else
            //{
            //    string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", newEntity.Tenant);
            //    msg = msg.Replace("%Entity", "Rule");
            //    throw new Exception(msg);
            //}  
        }

        public void UpdateObjectTableRule(ObjectTableRulePM currentEntity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(currentEntity.Tenant);
            }

           

            //ObjectTableRuleRepository = new ObjectTableRuleRepository(ObjectContext);
            //RuleTypeRepository = new RuleTypeRepository(ObjectContext);
            //RuleConditionFieldRepository = new RuleConditionFieldRepository(ObjectContext);

            //ObjectTableRule objectTableRule = ObjectTableRuleRepository.GetSingleObjectTableRule(currentEntity.Id, currentEntity.Tenant);

            //string objectRulesListName = objectTableRule.ObjectTable.Name + "DuplicationRules" + currentEntity.Tenant;
            //if (CacheManager.CacheWrapper.Get(objectRulesListName) != null)
            //{
            //    CacheManager.CacheWrapper.Remove(objectRulesListName);
            //}

            //MapObjectTableRulePMObjectTableRule(currentEntity, objectTableRule);
            //bool replicated = ObjectTableRuleRepository.GetObjectTableRules(currentEntity.Tenant).Where(r => r.RuleCode == currentEntity.RuleCode && r.Id != currentEntity.Id).Any();
            //if (!replicated)
            //{
            //    ObjectTableRuleRepository.Update(objectTableRule);
            //}
            //else
            //{
            //    string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", currentEntity.Tenant);
            //    msg = msg.Replace("%Entity", "Rule");
            //    throw new Exception(msg);
            //}

            List<RuleConditionFieldPM> ruleCondetionFieldChangeSet = ChangeSet.GetAssociatedChanges(currentEntity, d => d.RuleConditionFields).Cast<RuleConditionFieldPM>().ToList();

            foreach (RuleConditionFieldPM r in ruleCondetionFieldChangeSet)
            {

                string listName = "ruleconditionfieldstenant" + r.Tenant;

                if (CacheManager.CacheWrapper.Get(listName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(listName);
                }


                ChangeOperation op = ChangeSet.GetChangeOperation(r);

                switch (op)
                {
                    case ChangeOperation.Insert:
                        {
                            r.ChangeSetOp = ChangeSetOperation.Insert;
                            //RuleConditionField condField = new RuleConditionField();
                            //condField.Id = IdCounter.GetNumber("RuleConditionField", condFieldPM.Tenant).ToString();
                            //condFieldPM.Id = condField.Id;
                            //MapRuleConditionFieldPMRuleConditionField(condFieldPM, condField);
                            //RuleConditionFieldRepository.Add(condField);
                            //RuleConditionFieldRepository.SubmitChanges();
                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            r.ChangeSetOp = ChangeSetOperation.Delete;

                            //RuleConditionField condField = RuleConditionFieldRepository.GetSingleRuleConditionField(condFieldPM.Id, condFieldPM.Tenant);
                            //RuleConditionFieldRepository.Remove(condField);
                            //RuleConditionFieldRepository.SubmitChanges();

                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            r.ChangeSetOp = ChangeSetOperation.Update;

                            //RuleConditionField condField = RuleConditionFieldRepository.GetSingleRuleConditionField(condFieldPM.Id, condFieldPM.Tenant);
                            //MapRuleConditionFieldPMRuleConditionField(condFieldPM, condField);
                            //RuleConditionFieldRepository.Update(condField);
                            //RuleConditionFieldRepository.SubmitChanges();

                            break;
                        }
                    case ChangeOperation.None:
                        {
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }

            ObjectTableRuleService service = new ObjectTableRuleService(ObjectContext, currentEntity.Tenant);
            service.Update(currentEntity , ruleCondetionFieldChangeSet);
        }

        public void DeleteObjectTableRule(ObjectTableRulePM entity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(entity.Tenant);
            }

            ObjectTableRuleRepository = new ObjectTableRuleRepository(ObjectContext);
            ObjectTableRule objectTableRule = ObjectTableRuleRepository.GetSingleObjectTableRule(entity.Id, entity.Tenant);
            ObjectTableRuleRepository.Remove(objectTableRule);
        }

        void MapObjectTableRulePMObjectTableRule(ObjectTableRulePM rulePM, ObjectTableRule rule)
        {
            rule.Tenant = rulePM.Tenant;
            rule.RuleCode = rulePM.RuleCode;
            rule.RuleTypeCode = rulePM.RuleTypeCode;
            rule.Name = rulePM.Name;
            rule.ObjectTableId = rulePM.ObjectTableId;
            rule.OutputMessage = rulePM.OutputMessage;
            rule.SystemLevel = rulePM.SystemLevel;
            rule.InActive = rulePM.InActive;
            rule.Condition = rulePM.Condition;
            rule.TriggerTypeCode = rulePM.TriggerTypeCode;
            rule.TriggerFieldId = rulePM.TriggerFieldId;
            rule.ActiveForUpdate = rulePM.ActiveForUpdate;
            rule.ActiveForNew = rulePM.ActiveForNew;
            rule.RuleNotificationTypeCode = rulePM.RuleNotificationTypeCode;
            rule.Internal = rulePM.Internal;
            rule.AdvancedCondition = rulePM.AdvancedCondition;
        }
        #endregion
 
        #region ObjectTableRuleFields
        public IQueryable<ObjectTableRuleField> GetObjectTableRuleFields()
        {
            ObjectTableRuleFieldRepository = new ObjectTableRuleFieldRepository(0);
            //this.ChangeConnectionString(tenant);
            return ObjectTableRuleFieldRepository.GetObjectTableRuleFields(0);
        }

        public IQueryable<ObjectTableRuleFieldPM> GetObjectTableRuleFieldPMsByTenant(int tenant)
        {
            ObjectTableRuleFieldRepository = new ObjectTableRuleFieldRepository(tenant);
            //this.ChangeConnectionString(tenant);
            objectTableRuleFieldQuery = new ObjectTableRuleFieldQuery(ObjectTableRuleFieldRepository);
            return objectTableRuleFieldQuery.GetObjectTableRuleFieldPMsByTenant(tenant);
        }

        public void InsertObjectTableRuleField(ObjectTableRuleFieldPM newEntity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(newEntity.Tenant);
            }
            ObjectTableRuleFieldService service = new ObjectTableRuleFieldService(ObjectContext , newEntity.Tenant);
            service.Create(newEntity);

            //ObjectTableRuleFieldRepository = new ObjectTableRuleFieldRepository(ObjectContext);
            //ObjectTableRuleField newObjectTableRuleField = new ObjectTableRuleField();
            //newObjectTableRuleField.Id = IdCounter.GetNumber("ObjectTableRuleField", newEntity.Tenant).ToString();
            //newEntity.Id = newObjectTableRuleField.Id;
            //MapObjectTableRuleFieldPMObjectTableRuleField(newEntity, newObjectTableRuleField);
            //ObjectTableRuleFieldRepository.Add(newObjectTableRuleField);
        }

        public void UpdateObjectTableRuleField(ObjectTableRuleFieldPM currentEntity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(currentEntity.Tenant);
            }
            ObjectTableRuleFieldService service = new ObjectTableRuleFieldService(ObjectContext, currentEntity.Tenant);
            service.Update(currentEntity);

           // ObjectTableRuleFieldRepository = new ObjectTableRuleFieldRepository(ObjectContext);
           // ObjectTableRuleField objectTableRuleField = ObjectTableRuleFieldRepository.GetSingleObjectTableRuleField(currentEntity.Id, currentEntity.Tenant);
            
           //string rulesFieldsListName = "RuleFields" + objectTableRuleField.Tenant;
           //if (CacheManager.CacheWrapper.Get(rulesFieldsListName) != null)
           //{
           //    CacheManager.CacheWrapper.Remove(rulesFieldsListName);
           //}

         
           // MapObjectTableRuleFieldPMObjectTableRuleField(currentEntity, objectTableRuleField);
           // ObjectTableRuleFieldRepository.Update(objectTableRuleField);
        }

        public void DeleteObjectTableRulField(ObjectTableRuleFieldPM entity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(entity.Tenant);
            }

            ObjectTableRuleFieldRepository = new ObjectTableRuleFieldRepository(ObjectContext);


            ObjectTableRuleField objectTableRuleField = ObjectTableRuleFieldRepository.GetSingleObjectTableRuleField(entity.Id, entity.Tenant);
            ObjectTableRuleFieldRepository.Remove(objectTableRuleField);
        }

        void MapObjectTableRuleFieldPMObjectTableRuleField(ObjectTableRuleFieldPM ruleFieldPM, ObjectTableRuleField ruleField)
        {
            ruleField.Tenant = ruleFieldPM.Tenant;
            ruleField.ObjectFieldId = ruleFieldPM.ObjectFieldId;
            ruleField.ObjectTableRuleId = ruleFieldPM.ObjectTableRuleId;
            ruleField.SystemLevel = ruleFieldPM.SystemLevel;
            ruleField.Expression = ruleFieldPM.Expression;
            ruleField.RuleNotificationTypeCode = ruleFieldPM.RuleNotificationTypeCode;
            ruleField.ObjectFieldCode = ruleFieldPM.ObjectFieldCode;
        }
        #endregion

        //#region RuleConditionFields


        //public IQueryable<RuleConditionFieldPM> GetRuleConditionFields()
        //{
        //    RuleConditionFieldRepository = new RuleConditionFieldRepository(0);
        //    this.ChangeConnectionString(0);
        //    return RuleConditionFieldRepository.GetRuleConditionFieldPMsByTenant(0);
        //}



        //public IQueryable<RuleConditionFieldPM> GetRuleConditionFieldPMsByTenant(int tenant)
        //{
        //    RuleConditionFieldRepository = new RuleConditionFieldRepository(tenant);
        //    this.ChangeConnectionString(tenant);
        //    return RuleConditionFieldRepository.GetRuleConditionFieldPMsByTenant(tenant);
        //}



        //public IQueryable<RuleConditionFieldPM> GetRuleConditionFieldPMsByRuleId(int tenant, string ruleId)
        //{
        //    RuleConditionFieldRepository = new RuleConditionFieldRepository(tenant);
        //    this.ChangeConnectionString(tenant);
        //    return RuleConditionFieldRepository.GetRuleConditionFieldsByRuleId(tenant, ruleId);
        //}

        public void MapRuleConditionFieldPMRuleConditionField(RuleConditionFieldPM ruleConditionFieldPM, RuleConditionField ruleConditionField)
        {
            ruleConditionField.Tenant = ruleConditionFieldPM.Tenant;
            ruleConditionField.ObjectFieldId = ruleConditionFieldPM.ObjectFieldId;
            ruleConditionField.ObjectFieldCode = ruleConditionFieldPM.ObjectFieldCode;
            ruleConditionField.ObjectTableRuleId = ruleConditionFieldPM.ObjectTableRuleId;
            ruleConditionField.Operator = ruleConditionFieldPM.Operator;
            ruleConditionField.Value = ruleConditionFieldPM.Value;


        }

        public void InsertRuleConditionField(RuleConditionFieldPM entity)
        {
            //RuleConditionFieldRepository = new RuleConditionFieldRepository(entity.Tenant);
            //this.ChangeConnectionString(entity.Tenant);
            //entity.Id = IdCounter.GetNumber("RuleConditionField").ToString();
            //RuleConditionField newRuleConditionField = new RuleConditionField();
            //newRuleConditionField.Id = entity.Id;
            //MapRuleConditionFieldPMRuleConditionField(entity, newRuleConditionField);
            //RuleConditionFieldRepository.Add(newRuleConditionField);
        }

        public void UpdateRuleConditionField(RuleConditionFieldPM entity)
        {
            //RuleConditionFieldRepository = new RuleConditionFieldRepository(entity.Tenant);
            //this.ChangeConnectionString(entity.Tenant);
            //RuleConditionField ruleConditionField = RuleConditionFieldRepository.GetSingleRuleConditionField(entity.Id);
            //MapRuleConditionFieldPMRuleConditionField(entity, ruleConditionField);
            //RuleConditionFieldRepository.Update(ruleConditionField);
        }

        public void DeleteRuleConditionField(RuleConditionFieldPM entity)
        {
            //RuleConditionFieldRepository = new RuleConditionFieldRepository(entity.Tenant);
            //this.ChangeConnectionString(entity.Tenant);
            //RuleConditionField ruleConditionField = RuleConditionFieldRepository.GetSingleRuleConditionField(entity.Id);
            //if (ruleConditionField != null)
            //{
            //    RuleConditionFieldRepository.Remove(ruleConditionField);
            //}
        }
        //#endregion


        #region RuleTypes
        public IQueryable<RuleType> GetRuleTypes(int tenant)
        {
            RuleTypeRepository = new RuleTypeRepository(tenant);
            
            return RuleTypeRepository.GetRuleTypes();
        }

        public void InsertRuleType(RuleType newEntity)
        {
            bool exists = RuleTypeRepository.GetRuleTypeByCode(newEntity.Code) != null ? true : false;
            
            if (!exists)
            {
                RuleTypeRepository.Add(newEntity);
            }
            else
            {                
                throw new Exception("This Entity Already exists!");
            }             
        }

        public void UpdateRuleType(RuleType currentEntity)
        {
            bool exists = RuleTypeRepository.GetRuleTypes().Where(r => r.Code == currentEntity.Code && r.Name != currentEntity.Name).Any();
            if (!exists)
            {
                RuleTypeRepository.Update(currentEntity);
            }
            else
            {
                throw new Exception("This Entity Already exists!");
            }
        }

        public void DeleteRuleType(RuleType entity)
        {
            RuleTypeRepository.Remove(entity);
        }
        #endregion

        #region CounterDefinitions
        public IQueryable<CounterDefinitionPM> GetCounterDefinitions()
        {
           counterDefinitionQuery = new CounterDefinitionQuery(0);
            this.ChangeConnectionString(0);
            return counterDefinitionQuery.GetCounterDefinitionsByTenant(0);
        }

        public IQueryable<CounterDefinitionPM> GetCounterDefinitionsByTenant(int tenant)
        {
            counterDefinitionQuery = new CounterDefinitionQuery(tenant);
            this.ChangeConnectionString(tenant);
            return counterDefinitionQuery.GetCounterDefinitionsByTenant(tenant);
        }
        [Invoke]
        public bool CheckIfUsed(string counterId, int tenant)
        {
            counterDefinitionQuery = new CounterDefinitionQuery(tenant);
            this.ChangeConnectionString(tenant);
            string email = SecurityUtility.GetAuthenticatedUser();
            UserQuery userQuery = new UserQuery(tenant);
            UserPM user = userQuery.GetSingleUserByEmailOrIdAndTenantOrTenantZero(null,email, tenant);

            if (user.IsCustomerCare)
            {
                return false;
            }
            else
            {
            return counterDefinitionQuery.CheckIfUsed(counterId, tenant);
        }
        }

        public CounterDefinitionPM GetCounterDefinitionsByCounterId(string counterId, int tenant)
        {
            counterDefinitionQuery = new CounterDefinitionQuery(tenant);
            this.ChangeConnectionString(tenant);
            return counterDefinitionQuery.GetCounterDefinitionsByCounterId(counterId, tenant).FirstOrDefault();
        }

        public void InsertCounterDefinition(CounterDefinitionPM entity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(entity.Tenant);
            }

            CounterDefinitionService service = new CounterDefinitionService(ObjectContext, ServiceContext.User.Identity.Name, entity.Tenant);
            service.Create(entity);
        }

        public void UpdateCounterDefinition(CounterDefinitionPM currentEntity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(currentEntity.Tenant);
            }

            CounterDefinitionService service = new CounterDefinitionService(ObjectContext, ServiceContext.User.Identity.Name, currentEntity.Tenant);
            service.Update(currentEntity);
        }

        public void DeleteCounterDefinition(CounterDefinitionPM entity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            CounterDefinitionRepository = new CounterDefinitionRepository(ObjectContext);
            this.ChangeConnectionString(entity.Tenant);
            CounterDefinition counterDefinition = CounterDefinitionRepository.GetSingleCounterDefinition(entity.Id, entity.Tenant);
            CounterDefinitionRepository.Remove(counterDefinition);
        }
        #endregion

        #region Counters
        public IQueryable<CounterPM> GetCounters()
        {
            counterQuery = new CounterQuery(0);
            this.ChangeConnectionString(0);
            return counterQuery.GetCountersByTenant(0);
        }

        public IQueryable<CounterPM> GetCountersByTenant(int tenant)
        {
            counterQuery = new CounterQuery(tenant);
            this.ChangeConnectionString(tenant);
            return counterQuery.GetCountersByTenant(tenant);
        }

        public CounterPM GetObjectTableCounter(string objectTableId, int tenant)
        {
            counterQuery = new CounterQuery(tenant);
            this.ChangeConnectionString(tenant);
            return counterQuery.GetObjectTableCounters(objectTableId, tenant).FirstOrDefault();
        }

        public void InsertCounter(CounterPM entity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(entity.Tenant);
            }

            CounterService service = new CounterService(ObjectContext , entity.Tenant);
            service.Create(entity);
        }

        public void UpdateCounter(CounterPM currentEntity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(currentEntity.Tenant);
            }

            CounterService service = new CounterService(ObjectContext , currentEntity.Tenant);
            service.Update(currentEntity);
        }

        public void DeleteCounter(CounterPM entity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(entity.Tenant);
            }

            CounterRepository = new CounterRepository(ObjectContext);
            this.ChangeConnectionString(entity.Tenant);
            Counter counter = CounterRepository.GetSingleCounter(entity.Id, entity.Tenant);
            CounterRepository.Remove(counter);
        }

        public void MapCounterPMCounter(CounterPM counterPM, Counter counter)
        {
            counter.Name = counterPM.Name;
            counter.ObjectTableId = counterPM.ObjectTableId;
            counter.Tenant = counterPM.Tenant;
            counter.Code = counterPM.Code;  
        }
        #endregion

        #region TenantSettings
        public IQueryable<TenantSettingPM> GetTenantSettings()
        {
            this.ChangeConnectionString(0);
            tenantSettingQuery = new TenantSettingQuery(TenantSettingRepository);
            return tenantSettingQuery.GetTenantSettingsByTenant(0);
        }

        public IQueryable<TenantSettingPM> GetTenantSettingsByTenant(int tenant)
        {
            TenantSettingRepository = new TenantSettingRepository(tenant);
            this.ChangeConnectionString(tenant);
            tenantSettingQuery = new TenantSettingQuery(TenantSettingRepository);
            return tenantSettingQuery.GetTenantSettingsByTenant(tenant);
        }

        public TenantSettingPM GetObjectTableTenantSettings(string objectTableId, int tenant)
        {
            TenantSettingRepository = new TenantSettingRepository(tenant);
            this.ChangeConnectionString(tenant);
            tenantSettingQuery = new TenantSettingQuery(TenantSettingRepository);
            return tenantSettingQuery.GetTenantSettingsByObjectTableId(objectTableId, tenant).FirstOrDefault();
        }

        public void InsertTenantSetting(TenantSettingPM entity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(entity.Tenant);
            }

            TenantSettingService service = new TenantSettingService(ObjectContext, entity.Tenant);
            service.Create(entity);
        }

        public void UpdateTenantSetting(TenantSettingPM currentEntity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(currentEntity.Tenant);
            }

            TenantSettingService service = new TenantSettingService(ObjectContext, currentEntity.Tenant);
            service.Update(currentEntity);
        }

        public void DeleteTenantSetting(TenantSettingPM entity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            TenantSettingRepository = new TenantSettingRepository(ObjectContext);
            this.ChangeConnectionString(entity.Tenant);
            TenantSetting tenantSetting = TenantSettingRepository.GetSingleTenantSetting(entity.Id, entity.Tenant);
            TenantSettingRepository.Remove(tenantSetting);
        }
        #endregion

        #region ObjectTables
        public IQueryable<ObjectTablePM> GetObjectTables()
        {
            ObjectTableRepository = new ObjectTableRepository(0);
            ObjectTableQuery objectTabelQuery = new ObjectTableQuery(ObjectTableRepository);
            this.ChangeConnectionString(0);
            return objectTabelQuery.GetObjectPMsByTenant(0);
        }

        public List<ObjectTablePM> GetSomeObjectTables()
        {
            ObjectTableRepository = new ObjectTableRepository(0);
            ObjectTableQuery objectTableQuery = new ObjectTableQuery(ObjectTableRepository);
            return objectTableQuery.GetSomeObjectTables(0);
        }

        [Invoke]
        public DateTime? GetLastTableUpdateDate(int tenant)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(tenant);
            }

            ObjectTableLastUpdateRepository tableLastUpdateRepository = new ObjectTableLastUpdateRepository(ObjectContext);
            ObjectTableLastUpdateQuery objectTabelQuery = new ObjectTableLastUpdateQuery(tableLastUpdateRepository);
            ObjectTableLastUpdatePM lastupdate = objectTabelQuery.GetLastUpdatedTable(tenant);
            DateTime? lastDate = null;
            if (lastupdate == null)
            {
                lastDate = DateTime.UtcNow;
            }
            else
            {
                lastDate = lastupdate.LastUpdateDate;
            }

            return lastDate;
        }

        public MetaDataLastUpdateDates GetSystemMetadataLastUpdates(int tenant)
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

            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(tenant);
            }


            ObjectFieldsRepository = new ObjectFieldRepository(ObjectContext);
            TranslationRepository = new Simplog.Data.InfrastructureModel.Repositories.TranslationRepository(ObjectContext);

            ObjectFieldModification mod = ObjectFieldsRepository.GetLastObjectFieldModificationByTenant(tenant);
            metadata.ObjectFieldsTenantUpdateDateGMT = (mod != null ? mod.UpdateDateGMT.Value : new DateTime(2015,1,1));

            Translation translation = TranslationRepository.GetLastTranslationsByTenant(tenant);
            metadata.TranslationsTenantUpdateDateGMT = (translation != null ? translation.UpdateDateGMT.Value : new DateTime(2015, 1, 1));
           

            return metadata;

        }
        

        [RequiresAuthentication]
        public List<ObjectTableLastUpdatePM> GetLastUpdatedTables(int tenant, DateTime sinceDate,string clientEmail)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(tenant);
            }

            ObjectTableLastUpdateRepository tableLastUpdateRepository = new ObjectTableLastUpdateRepository(ObjectContext);
            ObjectTableLastUpdateQuery objectTabelQuery = new ObjectTableLastUpdateQuery(tableLastUpdateRepository);
            try
            {
                this.ChangeConnectionString(tenant);
                List<ObjectTableLastUpdatePM> list = objectTabelQuery.GetLastUpdatedTables(tenant, sinceDate);
                return list;
            }
            catch (Exception ex)
            {
                string errorMessage;
                errorMessage = ex.Message;

                if (ex.InnerException != null)
                {
                    errorMessage += Environment.NewLine + ex.InnerException.Message;
                }
                errorMessage += Environment.NewLine + ex.ToString();
                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                }
                AzureLog.SaveLogsInStorage(errorMessage, "E", DateTime.Now, ex.Message, ex.StackTrace, tenant, ServiceContext.User != null ? ServiceContext.User.Identity.Name : "", ServiceContext.User != null ? ServiceContext.User.Identity.Name : "",ip);
                throw new DomainException(ex.Message);
            }
        }

        public IQueryable<ObjectTablePM> GetObjectTablesByTenant(int tenant)
        {
            ObjectTableRepository = new ObjectTableRepository(tenant);
            this.ChangeConnectionString(tenant);
            ObjectTableQuery objectTabelQuery = new ObjectTableQuery(ObjectTableRepository);
            IQueryable<ObjectTablePM> result = objectTabelQuery.GetObjectPMsByTenant(tenant).OrderBy(o => o.Name);
             
            //foreach (ObjectTablePM objectTable in result)
            //{
            //    if (objectTable.Name == "Shipment")
            //    {
            //    }
            //    objectTable.ObjectTableRules = ObjectTableRuleRepository.GetObjectTableRulePMsByObjectTableId(objectTable.Id, objectTable.Tenant);
            //    //foreach (ObjectTableRulePM rule in objectTable.ObjectTableRules)
            //    //{
            //    //    rule.ObjectTableRuleFields = ObjectTableRuleFieldRepository.GetObjectTableRuleFieldPMsByObjectTableRuleId(rule.Id, rule.Tenant).ToList();
            //    //}
            //}
            return result;
        }

        public ObjectTablePM GetObjectTableIDByName(string name, int tenant)
        {
            //ObjectTableRepository = new ObjectTabelRepository(tenant);
            this.ChangeConnectionString(tenant);
            return ObjectTableQuery.GetObjectTableByCode(name, tenant);
        }

        public ObjectTablePM GetSingleObjectTable(string id, int tenant)
        {
            ObjectTableRepository = new ObjectTableRepository(tenant);
            ObjectTableQuery objectTabelQuery = new ObjectTableQuery(ObjectTableRepository);
            return objectTabelQuery.GetObjectTablePMById(id, tenant);
        }

        public ObjectTableList GetSingleObjectTableList(string id, int tenant)
        {
            ObjectTableRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = ObjectTableRepository.GetObjectTableById(id, tenant);

            ObjectTableList objectTableList = new ObjectTableList()
            {
                Name = objectTable.Name,
                AutoCompleteBox1 = objectTable.LookUp1,
                AutoCompleteBox2 = objectTable.LookUp2,
                AutoCompleteSearchWindow = objectTable.AutoCompleteSearchWindow,
                DependencyFilter1 = objectTable.DependencyFilter1,
                DependencyFilter2 = objectTable.DependencyFilter2,
                HeaderScreenId = objectTable.HeaderScreenId,
                Id = objectTable.Id,
                IsClosed = objectTable.IsClosed,
                IsNewWizard = objectTable.IsNewWizard,
                KeyPropertyPath = objectTable.KeyPropertyPath,
                NewWizardControlName = objectTable.NewWizardControlName,
                Tenant = objectTable.Tenant,
                EnableAddFromLOV = objectTable.EnableEditFromLOV,
                EnableEditFromLOV = objectTable.EnableEditFromLOV,
                HasCounter = objectTable.HasCounter,
                SearchFields=objectTable.SearchFields,
                DescriptionTextCodeId=objectTable.DescriptionTextCodeId,
                DescriptionTextCodeCode=objectTable.DescriptionTextCodeCode,
                AllowCustomFields = objectTable.AllowCustomFields,
                MaxNumberOfCustomFields = objectTable.MaxNumberOfCustomFields,
                DBTableName = objectTable.DBTableName,
                HasDocuments = objectTable.HasDocuments,
                //IsOperational = objectTable.IsOperational,
            };
            return objectTableList;
        }

        public IQueryable<ObjectTableList> GetObjectTableLists(int tenant)
        {
            ObjectTableRepository = new ObjectTableRepository(tenant);
            IQueryable<ObjectTable> objectTables = ObjectTableRepository.GetObjectsByTenantOrTenantZero(tenant);

            var query2 = from objectTable in objectTables
                         select new ObjectTableList()
                         {
                             Name = objectTable.Name,
                             AutoCompleteBox1 = objectTable.LookUp1,
                             AutoCompleteBox2 = objectTable.LookUp2,
                             AutoCompleteSearchWindow = objectTable.AutoCompleteSearchWindow,
                             DependencyFilter1 = objectTable.DependencyFilter1,
                             DependencyFilter2 = objectTable.DependencyFilter2,
                             HeaderScreenId = objectTable.HeaderScreenId,
                             Id = objectTable.Id,
                             IsClosed = objectTable.IsClosed,
                             IsNewWizard = objectTable.IsNewWizard,
                             KeyPropertyPath = objectTable.KeyPropertyPath,
                             NewWizardControlName = objectTable.NewWizardControlName,
                             Tenant = objectTable.Tenant,
                             EnableAddFromLOV = objectTable.EnableEditFromLOV,
                             EnableEditFromLOV = objectTable.EnableEditFromLOV,
                             HasCounter = objectTable.HasCounter,
                             SearchFields = objectTable.SearchFields,
                             DescriptionTextCodeId = objectTable.DescriptionTextCodeId,
                             DescriptionTextCodeCode = objectTable.DescriptionTextCodeCode,
                             AllowCustomFields = objectTable.AllowCustomFields,
                             MaxNumberOfCustomFields = objectTable.MaxNumberOfCustomFields,
                             DBTableName = objectTable.DBTableName,
                             HasDocuments = objectTable.HasDocuments,
                             //IsOperational = objectTable.IsOperational,
                         };
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ObjectTableList> GetObjectTableFilters(byte[] xmlFilters, int tenant)
        {
            ObjectTableRepository = new ObjectTableRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<ObjectTable> objectTables = ObjectTableRepository.GetObjectsByTenant(tenant);
           
            //agents = customfilters.GetFilteredQuery(queryOperations, agents);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            objectTables = filter.GetFilteredQuery<ObjectTable>(nonListQueryOperation, objectTables);
            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            var query2 = from objectTable in objectTables
                         select new ObjectTableList()
                         {
                             Name = objectTable.Name,
                             AutoCompleteBox1 = objectTable.LookUp1,
                             AutoCompleteBox2 = objectTable.LookUp2,
                             AutoCompleteSearchWindow = objectTable.AutoCompleteSearchWindow,
                             DependencyFilter1 = objectTable.DependencyFilter1,
                             DependencyFilter2 = objectTable.DependencyFilter2,
                             HeaderScreenId = objectTable.HeaderScreenId,
                             Id = objectTable.Id,
                             IsClosed = objectTable.IsClosed,
                             IsNewWizard = objectTable.IsNewWizard,
                             KeyPropertyPath = objectTable.KeyPropertyPath,
                             NewWizardControlName = objectTable.NewWizardControlName,
                             Tenant = objectTable.Tenant,
                             EnableAddFromLOV = objectTable.EnableEditFromLOV,
                             EnableEditFromLOV = objectTable.EnableEditFromLOV,
                             HasCounter = objectTable.HasCounter,
                             SearchFields = objectTable.SearchFields,
                             DescriptionTextCodeId = objectTable.DescriptionTextCodeId,
                             DescriptionTextCodeCode = objectTable.DescriptionTextCodeCode,
                             AllowCustomFields = objectTable.AllowCustomFields,
                             MaxNumberOfCustomFields = objectTable.MaxNumberOfCustomFields,
                             DBTableName = objectTable.DBTableName,
                             HasDocuments = objectTable.HasDocuments,
                             //IsOperational = objectTable.IsOperational,
                         };
            query2 = filter.GetFilteredQuery<ObjectTableList>(listQueryOperation, query2);
            //-------------------------------------------------------------------------------
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ObjectTableList).GetProperty(queryOperations.SortByColumnName);
                switch (propInfo.PropertyType.Name.ToLower())
                {
                    case "string":
                        {
                            query2 = sortClass.GetSorterQuery<ObjectTableList, string>(queryOperations, query2);
                            break;
                        }
                    case "double":
                        {
                            query2 = sortClass.GetSorterQuery<ObjectTableList, double>(queryOperations, query2);
                            break;
                        }
                    case "datetime":
                        {
                            query2 = sortClass.GetSorterQuery<ObjectTableList, DateTime>(queryOperations, query2);
                            break;
                        }
                    case "int":
                        {
                            query2 = sortClass.GetSorterQuery<ObjectTableList, int>(queryOperations, query2);
                            break;
                        }
                    case "boolean":
                        {
                            query2 = sortClass.GetSorterQuery<ObjectTableList, bool>(queryOperations, query2);
                            break;
                        }
                    default:
                        {
                            query2 = query2.OrderByDescending(d => d.Name);
                            break;
                        }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.Name);
            }
            //--------------------------------------------------------------------------------------------------

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);

            return query2;
        }
       
        public int GetObjectTableFiltersCount(byte[] xmlFilters, int tenant)
        {
            ObjectTableRepository = new ObjectTableRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            //PortCustomFilter customfilters = new PortCustomFilter();
            IQueryable<ObjectTable> objectTables = ObjectTableRepository.GetObjectsByTenant(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            objectTables = filter.GetFilteredQuery<ObjectTable>(nonListQueryOperation, objectTables);

            var query2 = from objectTable in objectTables
                         select new ObjectTableList()
                         {
                             Name = objectTable.Name,
                             AutoCompleteBox1 = objectTable.LookUp1,
                             AutoCompleteBox2 = objectTable.LookUp2,
                             AutoCompleteSearchWindow = objectTable.AutoCompleteSearchWindow,
                             DependencyFilter1 = objectTable.DependencyFilter1,
                             DependencyFilter2 = objectTable.DependencyFilter2,
                             HeaderScreenId = objectTable.HeaderScreenId,
                             Id = objectTable.Id,
                             IsClosed = objectTable.IsClosed,
                             IsNewWizard = objectTable.IsNewWizard,
                             KeyPropertyPath = objectTable.KeyPropertyPath,
                             NewWizardControlName = objectTable.NewWizardControlName,
                             Tenant = objectTable.Tenant,
                             EnableAddFromLOV = objectTable.EnableEditFromLOV,
                             EnableEditFromLOV = objectTable.EnableEditFromLOV,
                             HasCounter = objectTable.HasCounter,
                             SearchFields = objectTable.SearchFields,
                             DescriptionTextCodeId = objectTable.DescriptionTextCodeId,
                             DescriptionTextCodeCode = objectTable.DescriptionTextCodeCode,
                             AllowCustomFields = objectTable.AllowCustomFields,
                             MaxNumberOfCustomFields = objectTable.MaxNumberOfCustomFields,
                             DBTableName = objectTable.DBTableName,
                             HasDocuments = objectTable.HasDocuments,
                             //IsOperational = objectTable.IsOperational,
                         };
            query2 = filter.GetFilteredQuery<ObjectTableList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public void InsertObjectTable(ObjectTablePM objectTable)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(objectTable.Tenant);
            }
            ObjectTableService service = new ObjectTableService(ObjectContext, objectTable.Tenant);
            service.Create(objectTable);

            //ObjectTableRepository = new ObjectTabelRepository(ObjectContext);
            //this.ChangeConnectionString(objectTable.Tenant);
            //objectTable.Id = IdCounter.GetNumber("ObjectTable", objectTable.Tenant).ToString();
            //objectTable.LastUpdateDate = TenantServerConfigration.GetCurrentDateTime(objectTable.Tenant);
            //ObjectTable newObjectTable = new ObjectTable();
            //newObjectTable.Id = objectTable.Id;
            //newObjectTable.LastUpdateDate = objectTable.LastUpdateDate;
            //MapObjectTablePMObjectTable(objectTable, newObjectTable);
            //ObjectTableRepository.Add(newObjectTable);
        }

        public void UpdateObjectTable(ObjectTablePM currentObjectTable)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(currentObjectTable.Tenant);
            }
            ObjectTableService service = new ObjectTableService(ObjectContext, currentObjectTable.Tenant);
            service.Update(currentObjectTable);

            //ObjectTableRepository = new ObjectTabelRepository(ObjectContext);
            //this.ChangeConnectionString(currentObjectTable.Tenant);
            //ObjectTable entity = ObjectTableRepository.GetSingleObjectTable(currentObjectTable.Id, currentObjectTable.Tenant,false);
            //MapObjectTablePMObjectTable(currentObjectTable, entity);

           
            //ObjectTableRepository.Update(entity);
        }  

        public void DeleteObjectTable(ObjectTablePM objectTable)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(objectTable.Tenant);
            }
            ObjectTableRepository = new ObjectTableRepository(ObjectContext);
            this.ChangeConnectionString(objectTable.Tenant);
            ObjectTable entity = ObjectTableRepository.GetSingleObjectTable(objectTable.Id,objectTable.Tenant,false);
            ObjectTableRepository.Remove(entity);
        }
        #endregion

        #region ObjectFields
        public List<ObjectFieldPM> GetObjectFields()
        {
            ObjectFieldsRepository = new ObjectFieldRepository(0);
            this.ChangeConnectionString(0);
            objectFieldsQuery = new ObjectFieldQuery(ObjectFieldsRepository);
            return objectFieldsQuery.GetObjectFieldPMsByTenant(0, 0).ToList();
        }

        public List<ObjectFieldPM> GetObjectFieldsForObject(string objectName, int tenant)
        {
            ObjectFieldsRepository = new ObjectFieldRepository(tenant);
            this.ChangeConnectionString(tenant);
            objectFieldsQuery = new ObjectFieldQuery(ObjectFieldsRepository);
            return objectFieldsQuery.GetObjectFieldPMsByTenant(tenant, tenant).Where(of => of.ObjectTableName == objectName).ToList();
        }


        public List<ObjectFieldPM> GetObjectFieldsAllowedinAutomationConditionsByObjectTableId(string objecttableId, int tenant)
        {
            ObjectFieldsRepository = new ObjectFieldRepository(tenant);
            this.ChangeConnectionString(tenant);
            objectFieldsQuery = new ObjectFieldQuery(ObjectFieldsRepository);
            return objectFieldsQuery.GetObjectFieldsAllowedinAutomationConditionsPMsByObjectTableId(objecttableId, tenant);
        }



        public List<ObjectFieldPM> GetDisplayOnLookUpObjectFieldsForObject(string objectName, int tenant)
        {
            ObjectFieldsRepository = new ObjectFieldRepository(tenant);
            this.ChangeConnectionString(tenant);
            objectFieldsQuery = new ObjectFieldQuery(ObjectFieldsRepository);
            return objectFieldsQuery.GetObjectFieldPMsByTenant(tenant, tenant).Where(of => of.ObjectTableName == objectName && of.Tenant == tenant && of.DisplayOnLookUp == true).OrderBy(o => o.FieldName).ToList();
        }

        public List<ObjectFieldPM> GetListObjectFieldsForObject(string objectName, int tenant)
        {
            ObjectFieldsRepository = new ObjectFieldRepository(tenant);
            this.ChangeConnectionString(tenant);
            objectFieldsQuery = new ObjectFieldQuery(ObjectFieldsRepository);
            return objectFieldsQuery.GetObjectFieldPMsByTenant(tenant, tenant).Where(of => of.ObjectTableName == objectName && of.Tenant == tenant && of.DisplayInList == true).OrderBy(o => o.FieldName).ToList();
        }

        public List<ObjectFieldPM> GetCustomFieldsByTableId(string tableId, int tenant)
        {
            ObjectFieldsRepository = new ObjectFieldRepository(tenant);
            this.ChangeConnectionString(tenant);
            objectFieldsQuery = new ObjectFieldQuery(ObjectFieldsRepository);
            return objectFieldsQuery.GetCustomFieldsBytableID(tableId, tenant, tenant).ToList();
        }

        public ObjectFieldPM GetCustomFieldsByFieldId(string fieldId, int tenant)
        {
            ObjectFieldsRepository = new ObjectFieldRepository(tenant);
            this.ChangeConnectionString(tenant);
            objectFieldsQuery = new ObjectFieldQuery(ObjectFieldsRepository);
            return objectFieldsQuery.GetCustomFieldsByFieldId(fieldId, tenant);
        }

        public List<ObjectFieldPM> GetStandardFieldsForTableID(string tableId, int tenant)
        {
            ObjectFieldsRepository = new ObjectFieldRepository(tenant);
            this.ChangeConnectionString(tenant);
            objectFieldsQuery = new ObjectFieldQuery(ObjectFieldsRepository);
            return objectFieldsQuery.GetStandardFieldsFortableID(tableId, tenant, tenant).ToList();
            
            //List<ObjectField> AllList = ObjectFieldsRepository.GetObjectFields().Where(of => of.ObjectTableId == TableID && of.IsCustom == false && of.Tenant == tenant).ToList<ObjectField>();
            //List<ObjectField> CopyList = ObjectFieldsRepository.GetObjectFields().Where(of => of.ObjectTableId == TableID && of.IsCustom == false && of.IsOverridden == true).ToList<ObjectField>();
            //foreach (ObjectField copyField in CopyList)
            //{
            //    ObjectField ToBeRemoved = AllList.Where(f => f.FieldName == copyField.FieldName && f.IsOverridden == false).FirstOrDefault();
            //    if (ToBeRemoved != null) AllList.Remove(ToBeRemoved);
            //}

            //return AllList.OrderBy(O => O.FieldName).AsQueryable<ObjectField>();
            //return ObjectFieldsRepository.GetObjectFields().Where(of => of.ObjectTableId == TableID && of.IsCustom == false);
        }

        public ObjectFieldPM GetStandardFieldsByFieldId(string fieldId, int tenant)
        {
            ObjectFieldsRepository = new ObjectFieldRepository(tenant);
            this.ChangeConnectionString(tenant);
            objectFieldsQuery = new ObjectFieldQuery(ObjectFieldsRepository);
            return objectFieldsQuery.GetStandardFieldsByFieldId(fieldId, tenant);
        }

        public List<ObjectFieldPM> GetFilterObjectFields(int tenant)
        {
            ObjectFieldsRepository = new ObjectFieldRepository(tenant);
            this.ChangeConnectionString(tenant);
            objectFieldsQuery = new ObjectFieldQuery(ObjectFieldsRepository);
            return objectFieldsQuery.GetFilteredObjectFields(tenant, tenant).ToList();
        }

        public List<ObjectFieldPM> GetAdvanceFilterObjectFields(int tenant, string queryId)
        {
            ObjectFieldsRepository = new ObjectFieldRepository(tenant);
            this.ChangeConnectionString(tenant);
            objectFieldsQuery = new ObjectFieldQuery(ObjectFieldsRepository);
            return objectFieldsQuery.GetAdvanceFilteredObjectFields(tenant, queryId, tenant).ToList();
        }

        //public List<ObjectFieldPM> GetFixedFilterObjectFields(int tenant, string queryId)
        //{
        //    this.ChangeConnectionString(tenant);
        //    return ObjectFieldsRepository.GetFixedFilteredObjectFields(tenant, queryId).ToList();
        //}

        //[Query(HasSideEffects = true)]
        //public List<ObjectFieldPM> GetObjectFieldsByTenant(int tenant)
        //{
        //   // System.Threading.Thread.Sleep(new TimeSpan(0, 1, 30));
        //    this.ChangeConnectionString(tenant);
        //    List<ObjectFieldPM> result = ObjectFieldsRepository.GetObjectFieldPMsByTenant(tenant).ToList();//.Where(o => o.ObjectTableId == "1-163" || o.ObjectTableId == "1-143")

        //    //throw new Exception("Server returned timeout exception frfgfg retgtrtr tgrrgrtgtr rtgrgrtgr trggtrgrtg rtgrtgrtg gtrgrg rtgtrgtrg trgrtgrtg tgr tgrgrtg rtgrtg");



        //    //foreach (ObjectFieldPM objectField in result)
        //    //{

        //    //    objectField.ObjectFieldValidations = ObjectFieldValidationRepository.GetObjectFieldValidationPMsByObjectFieldId(objectField.Id, objectField.Tenant).ToList();
        //    //}

        //    //ObjectFieldPM pm = result.Where(o => o.FieldName == "ShipperId").FirstOrDefault();


        //    return result;
        //    //List<ObjectField> Zerolist = ObjectFieldsRepository.GetObjectFields().Where(o => o.Tenant == 0).ToList();
        //    //List<ObjectField> Tenantlist = ObjectFieldsRepository.GetObjectFields().Where(d => d.Tenant == tenant).ToList();

        //    //foreach (ObjectField objectField in Tenantlist)
        //    //{
        //    //    ObjectField rem = Zerolist.Where(o => o.FieldName == objectField.FieldName).FirstOrDefault();
        //    //    if (rem != null)
        //    //    {
        //    //        Zerolist.Remove(rem);
        //    //        Zerolist.Add(objectField);
        //    //    }
        //    //}

        //    //return Zerolist;
        //    //return ObjectFieldsRepository.GetObjectFields().Where(d=>d.Tenant==tenant||d.Tenant==0).OrderByDescending(d=>d.IsOverridden);
        //}

        public int GetAllObjectFieldsCount(int tenant)
        {
            //Thread.Sleep(100000);
            int count = ObjectFieldQuery.GetTenantObjectFieldsWithTenantZero(tenant).Count();
            return count;
        } 

        public List<ObjectFieldPM> GetAllObjectFieldsForTenant(int tenant,int skip,int take)
        {
            //ObjectFieldsRepository = new ObjectFieldsRepository(tenant);    
            //this.ChangeConnectionString(tenant);
            //objectFieldsQuery = new ObjectFieldsQuery(ObjectFieldsRepository);
            //Thread.Sleep(100000);
           
            List<ObjectFieldPM> result = ObjectFieldQuery.GetObjectFieldsByTenantStep(tenant,skip,take);

            return result;            
        }

        //public List<ObjectFieldPM> GetMasterObjectFieldsByTenant(int tenant)
        //{
        //    ObjectFieldsRepository = new ObjectFieldsRepository(tenant);
        //    // System.Threading.Thread.Sleep(new TimeSpan(0, 1, 30));
        //    this.ChangeConnectionString(tenant);
        //    objectFieldsQuery = new ObjectFieldsQuery(ObjectFieldsRepository);
        //    List<ObjectFieldPM> result = objectFieldsQuery.GetMasterObjectFieldPMsByTenant(tenant).ToList();

        //    return result;           
        //}

        //public List<ObjectFieldPM> GetQuoteObjectFieldsByTenant(int tenant)
        //{
        //    ObjectFieldsRepository = new ObjectFieldsRepository(tenant);
        //    // System.Threading.Thread.Sleep(new TimeSpan(0, 1, 30));
        //    this.ChangeConnectionString(tenant);
        //    objectFieldsQuery = new ObjectFieldsQuery(ObjectFieldsRepository);
        //    List<ObjectFieldPM> result = objectFieldsQuery.GetQuoteObjectFieldPMsByTenant(tenant).ToList();

        //    return result;
        //}

        //public List<ObjectFieldPM> GetInvoiceObjectFieldsByTenant(int tenant)
        //{
        //    ObjectFieldsRepository = new ObjectFieldsRepository(tenant);
        //    // System.Threading.Thread.Sleep(new TimeSpan(0, 1, 30));
        //    this.ChangeConnectionString(tenant);
        //    objectFieldsQuery = new ObjectFieldsQuery(ObjectFieldsRepository);
        //    List<ObjectFieldPM> result = objectFieldsQuery.GetInvoiceObjectFieldPMsByTenant(tenant).ToList();

        //    return result;
        //}

        //public List<ObjectFieldPM> GetFirstOtherObjectFieldsByTenant(int tenant)
        //{
        //    ObjectFieldsRepository = new ObjectFieldsRepository(tenant);
        //    // System.Threading.Thread.Sleep(new TimeSpan(0, 1, 30));
        //    this.ChangeConnectionString(tenant);
        //    objectFieldsQuery = new ObjectFieldsQuery(ObjectFieldsRepository);
        //    List<ObjectFieldPM> result = objectFieldsQuery.GetFirstOtherObjectFieldPMsByTenant(tenant).ToList();

        //    return result;
        //}

        //public List<ObjectFieldPM> GetSecondOtherObjectFieldsByTenant(int tenant)
        //{
        //    ObjectFieldsRepository = new ObjectFieldsRepository(tenant);
        //    // System.Threading.Thread.Sleep(new TimeSpan(0, 1, 30));
        //    this.ChangeConnectionString(tenant);
        //    objectFieldsQuery = new ObjectFieldsQuery(ObjectFieldsRepository);
        //    List<ObjectFieldPM> result = objectFieldsQuery.GetSecondOtherObjectFieldPMsByTenant(tenant).ToList();

        //    return result;
        //}

        //public void MapObjectFieldPMObjectField(ObjectFieldPM objectFieldPM, ObjectField objectField, ObjectFieldModification objectFieldModification)
        //{
        //    objectField.AutomaticField = objectFieldPM.AutomaticField;
        //    objectField.CanFilter = objectFieldPM.CanFilter;
        //    objectField.ConverterName = objectFieldPM.ConverterName;
        //    objectField.DataTemplateName = objectFieldPM.DataTemplateName;
        //    objectField.DataTypeCode = objectFieldPM.DataTypeCode;
        //    objectField.DependencyFilter1Type = objectFieldPM.DependencyFilter1Type;
        //    objectField.DependencyFilter1Value = objectFieldPM.DependencyFilter1Value;
        //    objectField.DependencyFilter2Type = objectFieldPM.DependencyFilter2Type;
        //    objectField.DependencyFilter2Value = objectFieldPM.DependencyFilter2Value;
        //    objectField.DisplayInList = objectFieldPM.DisplayInList;
        //    objectField.DisplayInLookUpIndex = objectFieldPM.DisplayInLookUpIndex;
        //    objectField.DisplayInSearchWindowFilters = objectFieldPM.DisplayInSearchWindowFilters;
        //    objectField.DisplayInSearchWindowFiltersIndex = objectFieldPM.DisplayInSearchWindowFiltersIndex;
        //    objectField.DisplayInSearchWindowList = objectFieldPM.DisplayInSearchWindowList;
        //    objectField.DisplayInSearchWindowListIndex = objectFieldPM.DisplayInSearchWindowListIndex;
        //    objectField.DisplayOnLookUp = objectFieldPM.DisplayOnLookUp;
        //    objectField.DisplayOnly = objectFieldPM.DisplayOnly;
        //    objectField.FullNameTextCodeId = objectFieldPM.FullNameTextCodeId;
        //    objectField.FieldName = objectFieldPM.FieldName;
        //    objectField.ShortNameTextCodeId = objectFieldPM.ShortNameTextCodeId;
        //    objectField.HelpTextCodeId = objectFieldPM.HelpTextCodeId;
        //    objectField.AgentPermissionTypeCode = objectFieldPM.AgentPermissionTypeCode;
        //    objectField.CustomerPermissionTypeCode = objectFieldPM.CustomerPermissionTypeCode;
        //    objectField.IsCustomFilter = objectFieldPM.IsCustomFilter;
        //    objectField.IsMulti = objectFieldPM.IsMulti;
        //    objectField.IsTimeFrameFilter = objectFieldPM.IsTimeFrameFilter;
        //    objectField.ListTextCodeId = objectFieldPM.ListTextCodeId;
        //    objectField.ListPropertyPath = objectFieldPM.ListPropertyPath;
        //    objectField.LookUpControlName = objectFieldPM.LookUpControlName;
        //    objectField.LookUpTableId = objectFieldPM.LookUpTableId;
        //    objectField.MultiLine = objectFieldPM.MultiLine;
        //    objectField.MultiTableId = objectFieldPM.MultiTableId;
        //    objectField.ObjectTableId = objectFieldPM.ObjectTableId;
        //    objectField.Operator = objectFieldPM.Operator;
        //    objectField.PMPropertyPath = objectFieldPM.PMPropertyPath;
        //    objectField.SystemMaxLength = objectFieldPM.SystemMaxLength;
        //    objectField.SystemRequired = objectFieldPM.SystemRequired;
        //    objectField.Tenant = objectFieldPM.Tenant;
        //    objectField.UniqueField = objectFieldPM.UniqueField;
        //    objectField.ValidForQuerySection1 = objectFieldPM.ValidForQuerySection1;
        //    objectField.ValidForQuerySection2 = objectFieldPM.ValidForQuerySection2;
        //    objectField.IsRestrictable = objectFieldPM.IsRestrictable;
        //    objectField.DisplayInEntityVariables = objectFieldPM.DisplayInEntityVariables;
        //    objectField.TextCase = objectFieldPM.TextCase;
        //    objectField.DigitsAfterPoint = objectFieldPM.DigitsAfterPoint;
        //    objectField.SearchFields = objectFieldPM.FieldName + "," + objectFieldPM.ObjectTableName + "," + objectFieldPM.FullNameTextCodeDefaultText + "," + objectFieldPM.PMPropertyPath + "," + objectFieldPM.ListPropertyPath + "," + objectFieldPM.ListTextCodeDefaultText + "," + objectFieldPM.MinLength + "," + objectFieldPM.MaxLength;
        //    objectField.DisplayInLookupColumnSize = objectFieldPM.DisplayInLookupColumnSize;
        //    objectField.ColumnHeaderTemplateName = objectFieldPM.ColumnHeaderTemplateName;
        //    objectField.ControlField1 = objectFieldPM.ControlField1;
        //    objectField.ControlField2 = objectFieldPM.ControlField2;
        //    objectField.CustomPickListCode = objectFieldPM.CustomPickListCode;
        //    if (objectFieldModification != null)
        //    {
        //        objectFieldModification.IsRequired = objectFieldPM.IsRequiered;
        //        objectFieldModification.MaxLength = objectFieldPM.MaxLength;
        //        objectFieldModification.MinLength = objectFieldPM.MinLength;
        //    }
        //    else
        //    {
        //        objectField.IsRequiered = objectFieldPM.IsRequiered;
        //        objectField.MaxLength = objectFieldPM.MaxLength;
        //        objectField.MinLength = objectFieldPM.MinLength;
        //    }
        //}

        public void InsertObjectField(ObjectFieldPM objectField)
        {
            
             if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(objectField.Tenant);
            }

         

            string tenantCodesListName = "tenanttextcodes" + objectField.Tenant;
            string tenantFieldsListName = "tenantobjectfields" + objectField.Tenant;
            //string zeroCodeslistName = "tenantzerotextcodes";
            //string zeroFieldslistName = "tenantzeroobjectfields";

            ObjectTableRepository rep = new ObjectTableRepository(ObjectContext);
            ObjectTable table = rep.GetObjectTableById(objectField.ObjectTableId, objectField.Tenant);

           

            if (HttpContext.Current != null)
            {
                if (CacheManager.CacheWrapper.Get(tenantCodesListName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(tenantCodesListName);
                }

                if (CacheManager.CacheWrapper.Get(tenantFieldsListName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(tenantFieldsListName);
                }

                if (table != null)
                {
                    string tenantListName = "tabletenantobjectfields" + table.Name + objectField.Tenant;
                    if (CacheManager.CacheWrapper.Get(tenantListName) != null)
                    {
                        CacheManager.CacheWrapper.Invalidate(tenantListName);
                    }
                }

                //if (CacheManager.CacheWrapper.Get(zeroCodeslistName) != null)
                //{
                //    CacheManager.CacheWrapper.Invalidate(zeroCodeslistName);
                //}

                //if (CacheManager.CacheWrapper.Get(zeroFieldslistName) != null)
                //{
                //    CacheManager.CacheWrapper.Invalidate(zeroFieldslistName);
                //}
            }
           
            ObjectFieldService service = new ObjectFieldService(ObjectContext, objectField.Tenant);
            service.Create(objectField);

            //ObjectFieldsRepository = new ObjectFieldsRepository(ObjectContext);
            //TextCodeRepository = new TextCodeRepository(ObjectContext);
            //ObjectTableRepository = new ObjectTabelRepository(ObjectContext);
            //ObjectFieldValidationRepository = new ObjectFieldValidationRepository(ObjectContext);

            //this.ChangeConnectionString(objectField.Tenant);
            //if (objectField.IsCustom)
            //{
            //    string tableId = objectField.ObjectTableId;
            //    int tenant = objectField.Tenant;
            //    List<ObjectField> list = ObjectFieldsRepository.GetObjectFieldsByTenant(tenant).Where(o => o.ObjectTableId == tableId && o.IsCustom == true).ToList<ObjectField>();

            //    int count = 0;
            //    if (list != null) count = list.Count;
            //    if (count < 10)
            //    {
            //        TextCode newCustomFieldTextCode = new TextCode();
            //        ObjectTable ob = ObjectTableRepository.GetObjects().Where(o => o.Id == objectField.ObjectTableId).FirstOrDefault();

            //        newCustomFieldTextCode.Code = ob.Name + ".Field" + (count + 1).ToString();
            //        newCustomFieldTextCode.Tenant = objectField.Tenant;
            //        newCustomFieldTextCode.TextCodeTypeCode = "F";
            //        newCustomFieldTextCode.ObjectTableId = objectField.ObjectTableId;
            //        newCustomFieldTextCode.DefaultText = objectField.FullNameTextCodeId;
            //        newCustomFieldTextCode.Id = IdCounter.GetNumber("TextCode", objectField.Tenant).ToString();
            //        TextCodeRepository.Add(newCustomFieldTextCode);
            //        objectField.FullNameTextCodeId = newCustomFieldTextCode.Id;

            //        if (!string.IsNullOrEmpty(objectField.HelpTextCodeId))
            //        {
            //            TextCode newHelpTextCode = new TextCode();
            //            newHelpTextCode.TextCodeTypeCode = "H";
            //            newHelpTextCode.ObjectTableId = objectField.ObjectTableId;
            //            newHelpTextCode.DefaultText = objectField.HelpTextCodeId;
            //            newHelpTextCode.Id = IdCounter.GetNumber("TextCode", objectField.Tenant).ToString();
            //            newHelpTextCode.Tenant = objectField.Tenant;
            //            newHelpTextCode.Code = ob.Name + ".Field" + (count + 1).ToString() + ".HelpText";
            //            TextCodeRepository.Add(newHelpTextCode);
            //            objectField.HelpTextCodeId = newHelpTextCode.Id;
            //            objectField.HelpTextTextCodeCode = newHelpTextCode.Code;
            //            objectField.HelpTextCodeDefaultText = newHelpTextCode.DefaultText;
            //        }

            //        if (!string.IsNullOrEmpty(objectField.ListTextCodeId) && objectField.DisplayInList)
            //        {
            //            TextCode listFieldLableTextCode = new TextCode();
            //            listFieldLableTextCode.Code = ob.Name + ".Field" + (count + 1).ToString() + "ListLable";
            //            listFieldLableTextCode.DefaultText = objectField.ListTextCodeId;
            //            listFieldLableTextCode.Id = IdCounter.GetNumber("TextCode", objectField.Tenant).ToString();
            //            listFieldLableTextCode.ObjectTableId = objectField.ObjectTableId;
            //            listFieldLableTextCode.Tenant = objectField.Tenant;
            //            listFieldLableTextCode.TextCodeTypeCode = "CH";
            //            TextCodeRepository.Add(listFieldLableTextCode);
            //            objectField.ListTextCodeId = listFieldLableTextCode.Id;
            //            objectField.ListTextCodeCode = listFieldLableTextCode.Code;
            //            objectField.ListTextCodeDefaultText = listFieldLableTextCode.DefaultText;
            //        }

            //        objectField.FullNameTextCodeCode = newCustomFieldTextCode.Code;
            //        objectField.FullNameTextCodeDefaultText = newCustomFieldTextCode.DefaultText;

            //        string fieldname = "Field" + (count + 1).ToString();
            //        objectField.FieldName = fieldname;
            //        objectField.PMPropertyPath = fieldname;
            //        objectField.ListPropertyPath = fieldname;
            //        objectField.DisplayInList = true;
            //    }

            //    else if (count == 10)
            //    {
            //        return;
            //    }

            //    objectField.Id = IdCounter.GetNumber("ObjectField", objectField.Tenant).ToString();
            //    ObjectField newObjectField = new ObjectField();
            //    newObjectField.Id = objectField.Id;
            //    MapObjectFieldPMObjectField(objectField, newObjectField, null);

            //    if (objectField.ObjectFieldValidations != null)
            //    {
            //        foreach (ObjectFieldValidationPM fieldValidation in objectField.ObjectFieldValidations)
            //        {
            //            ObjectFieldValidation newFieldValidation = new ObjectFieldValidation()
            //            {
            //                Id = IdCounter.GetNumber("ObjectFieldValidation", objectField.Tenant).ToString(),
            //                ObjectFieldId = objectField.Id,
            //                Tenant = fieldValidation.Tenant,
            //                ValidationExpression = fieldValidation.ValidationExpression,
            //                ErrorMessage = fieldValidation.ErrorMessage,
            //                ValidationOrder = fieldValidation.ValidationOrder,
            //                Condition = fieldValidation.Condition,
            //                Code = fieldValidation.Code,
            //            };

            //            fieldValidation.Id = newFieldValidation.Id;
            //            fieldValidation.ObjectFieldId = newFieldValidation.ObjectFieldId;
            //            ObjectFieldValidationRepository.Add(newFieldValidation);
            //        }
            //    }

            //    newObjectField.IsCustom = true;
            //    ObjectFieldsRepository.Add(newObjectField);
            //}

            //else
            //{
            //    objectField.Id = IdCounter.GetNumber("ObjectField", objectField.Tenant).ToString();
            //    ObjectField newObjectField = new ObjectField();
            //    newObjectField.Id = objectField.Id;
            //    MapObjectFieldPMObjectField(objectField, newObjectField, null);

            //    if (objectField.ObjectFieldValidations != null)
            //    {
            //        foreach (ObjectFieldValidationPM fieldValidation in objectField.ObjectFieldValidations)
            //        {
            //            ObjectFieldValidation newFieldValidation = new ObjectFieldValidation()
            //            {
            //                Id = IdCounter.GetNumber("ObjectFieldValidation", objectField.Tenant).ToString(),
            //                ObjectFieldId = objectField.Id,
            //                Tenant = fieldValidation.Tenant,
            //                ValidationExpression = fieldValidation.ValidationExpression,
            //                ErrorMessage = fieldValidation.ErrorMessage,
            //                ValidationOrder = fieldValidation.ValidationOrder,
            //                Condition = fieldValidation.Condition,
            //                Code = fieldValidation.Code,
            //            };

            //            fieldValidation.Id = newFieldValidation.Id;
            //            fieldValidation.ObjectFieldId = newFieldValidation.ObjectFieldId;
            //            ObjectFieldValidationRepository.Add(newFieldValidation);
            //        }
            //    }
            //    ObjectFieldsRepository.Add(newObjectField);
            //} 
        }

        //public void MakeCopyObjectField(ObjectField newCopyField)
        //{            
        //    TextCode OldTextCode = TextCodeRepository.GetTextCodes().Where(T => T.Id == newCopyField.Id).FirstOrDefault();
        //    TextCode newCopyOTextCode = new TextCode();
        //    newCopyOTextCode.Code = newCopyField.ObjectTable.Name;
        //    newCopyOTextCode.Tenant = newCopyField.Tenant;
        //    newCopyOTextCode.TextCodeTypeCode = "F";
        //    newCopyOTextCode.ObjectTableId = newCopyField.ObjectTableId;
        //    newCopyOTextCode.DefaultText = OldTextCode.DefaultText;
        //    newCopyOTextCode.Id = IdCounter.GetNumber().ToString();
        //    TextCodeRepository.Add(newCopyOTextCode);

        //    newCopyField.Id = IdCounter.GetNumber().ToString();
        //    newCopyField.FieldLable = newCopyField.Id;
        //    newCopyField.HelpText = newCopyField.Id;
        //}

        public void UpdateObjectField(ObjectFieldPM objectField)
        {
            string tenantCodesListName = "tenanttextcodes" + objectField.Tenant;
            string tenantFieldsListName = "tenantobjectfields" + objectField.Tenant;
            string zeroCodeslistName = "tenantzerotextcodes";
            string zeroFieldslistName = "tenantzeroobjectfields";

            string zeroAutomationConditionsObjectFieldslistName = "tabletenantzeroAutomationConditionsObjectFields" + objectField.ObjectTableId;
            string tenantAutomationConditionsObjectFieldListName = "tabletenantAutomationConditionsObjectFields" + objectField.ObjectTableId;

       

            if (HttpContext.Current != null)
            {
                if (CacheManager.CacheWrapper.Get(tenantCodesListName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(tenantCodesListName);
                }

                if (CacheManager.CacheWrapper.Get(tenantFieldsListName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(tenantFieldsListName);
                }

                if (CacheManager.CacheWrapper.Get(zeroCodeslistName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(zeroCodeslistName);
                }

                if (CacheManager.CacheWrapper.Get(zeroFieldslistName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(zeroFieldslistName);
                }



                if (CacheManager.CacheWrapper.Get(zeroAutomationConditionsObjectFieldslistName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(zeroAutomationConditionsObjectFieldslistName);
                }


                if (CacheManager.CacheWrapper.Get(tenantAutomationConditionsObjectFieldListName) != null)
                {
                    CacheManager.CacheWrapper.Invalidate(tenantAutomationConditionsObjectFieldListName);
                }


            }
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(objectField.Tenant);
            }

            ObjectFieldsRepository = new ObjectFieldRepository(ObjectContext);
            TextCodeRepository = new TextCodeRepository(ObjectContext);
            ObjectTableRepository = new ObjectTableRepository(ObjectContext);
            ObjectFieldValidationRepository = new ObjectFieldValidationRepository(ObjectContext);

            this.ChangeConnectionString(objectField.Tenant);

            CacheManager.CacheWrapper.Invalidate("shipmentobjectfields");
            CacheManager.CacheWrapper.Invalidate("masterobjectfields");
            CacheManager.CacheWrapper.Invalidate("invoiceobjectfields");
            CacheManager.CacheWrapper.Invalidate("quoteobjectfields");
            CacheManager.CacheWrapper.Invalidate("firstotherobjectfields");
            CacheManager.CacheWrapper.Invalidate("secondotherobjectfields");

            //string objectFieldsListName = objectField.ObjectTableName.ToLower() + "customobjectfields" + objectField.Tenant;
            //CacheManager.CacheWrapper.Remove(objectFieldsListName);

            //if (!string.IsNullOrEmpty(objectField.FullNameTextCodeId))
            //{
            //    TextCode textCode = TextCodeRepository.GetTextCodes().Where(o => o.Id == objectField.FullNameTextCodeId).FirstOrDefault();
            //    if (textCode != null)
            //    {
            //        if (textCode.DefaultText != objectField.FullNameTextCodeDefaultText)
            //        {
            //            textCode.DefaultText = objectField.FullNameTextCodeDefaultText;
            //        }
            //    }
            //}

            //if (!string.IsNullOrEmpty(objectField.HelpTextCodeId))
            //{
            //    TextCode helpTextCode = TextCodeRepository.GetTextCodes().Where(o => o.Id == objectField.HelpTextCodeId).FirstOrDefault();
            //    if (helpTextCode == null)
            //    {
            //        ObjectTable ob = ObjectTableRepository.GetSingleObjectTable(objectField.ObjectTableId, objectField.Tenant, true);

            //        TextCode newHelpTextCode = new TextCode();
            //        newHelpTextCode.TextCodeTypeCode = "H";
            //        newHelpTextCode.ObjectTableId = objectField.ObjectTableId;
            //        newHelpTextCode.DefaultText = objectField.HelpTextCodeId;
            //        newHelpTextCode.Id = IdCounter.GetNumber("TextCode", objectField.Tenant).ToString();
            //        newHelpTextCode.Tenant = objectField.Tenant;
            //        newHelpTextCode.Code = ob.Name + "." + objectField.FieldName + ".HelpText";
            //        TextCodeRepository.Add(newHelpTextCode);
            //        objectField.HelpTextCodeId = newHelpTextCode.Id;
            //    }

            //    else
            //    {
            //        if (helpTextCode.DefaultText != objectField.HelpTextCodeDefaultText)
            //        {
            //            helpTextCode.DefaultText = objectField.HelpTextCodeDefaultText;
            //        }
            //    }
            //}


         //   ObjectField entity = ObjectFieldsRepository.GetSingleObjectField(objectField.Id);

         //   ObjectFieldModification mod = ObjectFieldsRepository.GetObjectFieldModificationByObjectField(objectField.Id, objectField.UserTenant);
         //   if ((objectField.IsRequiered != entity.IsRequiered) || (objectField.MinLength != entity.MinLength) || (objectField.MaxLength != entity.MaxLength))
         //   {
         //       if (mod == null && entity.Tenant == 0)
         //       {
         //           mod = new ObjectFieldModification() { Id = IdCounter.GetNumber("ObjectFieldModification", objectField.Tenant), ObjectFieldId = entity.Id, IsRequired = objectField.IsRequiered, MaxLength = objectField.MaxLength, MinLength = objectField.MinLength, Tenant = objectField.UserTenant };
         //           this.ObjectContext.ObjectFieldModifications.Add(mod);
         //       }
         //   }

         ////   MapObjectFieldPMObjectField(objectField, entity, mod);

            #region ObjectFieldValidations
            List<ObjectFieldValidationPM> objectFieldValidationChangeSet = ChangeSet.GetAssociatedChanges(objectField, d => d.ObjectFieldValidations).Cast<ObjectFieldValidationPM>().ToList();

            foreach (ObjectFieldValidationPM r in objectFieldValidationChangeSet)
            {


                switch (ChangeSet.GetChangeOperation(r))
                {

                    case ChangeOperation.Insert:
                        {
                            r.ChangeSetOp = ChangeSetOperation.Insert; break;
                            //r.Id = IdCounter.GetNumber("ObjectFieldValidation", objectField.Tenant).ToString();
                            //ObjectFieldValidation newObjectFieldValidation = new ObjectFieldValidation();
                            //newObjectFieldValidation.Id = r.Id;
                            //MapObjectFieldValidationPMObjectFieldValidation(r, newObjectFieldValidation);
                            //ObjectFieldValidationRepository.Add(newObjectFieldValidation);
                            //break;
                        }
                    case ChangeOperation.Update:
                        {
                            r.ChangeSetOp = ChangeSetOperation.Insert; break;
                            //ObjectFieldValidation objectFieldValidation = ObjectFieldValidationRepository.GetSingleObjectFieldValidation(r.Id, objectField.Tenant);
                            //MapObjectFieldValidationPMObjectFieldValidation(r, objectFieldValidation);
                            //ObjectFieldValidationRepository.Update(objectFieldValidation);

                        }
                    case ChangeOperation.Delete:
                        {
                            r.ChangeSetOp = ChangeSetOperation.Delete; break;
                            //ObjectFieldValidation objectFieldValidation = ObjectFieldValidationRepository.GetSingleObjectFieldValidation(r.Id, objectField.Tenant);
                            //ObjectFieldValidationRepository.Remove(objectFieldValidation);
                            //break;
                        }
                    case ChangeOperation.None:
                        {
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            # endregion

            ObjectFieldService service = new ObjectFieldService(ObjectContext, objectField.Tenant);
            service.Update(objectField, objectFieldValidationChangeSet);


           // ObjectFieldsRepository.Update(entity);
        }

        //public void MapObjectFieldValidationPMObjectFieldValidation(ObjectFieldValidationPM objectFieldValidationPM, ObjectFieldValidation objectFieldValidation)
        //{
        //    objectFieldValidation.ObjectFieldId = objectFieldValidationPM.ObjectFieldId;
        //    objectFieldValidation.Tenant = objectFieldValidationPM.Tenant;
        //    objectFieldValidation.ValidationExpression = objectFieldValidationPM.ValidationExpression;
        //    objectFieldValidation.ErrorMessage = objectFieldValidationPM.ErrorMessage;
        //    objectFieldValidation.ValidationOrder = objectFieldValidationPM.ValidationOrder;
        //    objectFieldValidation.Condition = objectFieldValidationPM.Condition;
        //    objectFieldValidation.Code=  objectFieldValidationPM.Code;
        //}

        public void DeleteObjectField(ObjectFieldPM objectField)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(objectField.Tenant);
            }
            ObjectFieldsRepository = new ObjectFieldRepository(ObjectContext);
            this.ChangeConnectionString(objectField.Tenant);
            ObjectField entity = ObjectFieldsRepository.GetSingleObjectField(objectField.Id);
            ObjectFieldsRepository.Remove(entity);
        }
        #endregion

        #region TextCodes

        public int GetAllTextCodesCount(int tenant)
        {
            int count = TextCodeRepository.GetTenantTextCodesWithTenantZero(tenant).Count();
            return count;
        }

        public IQueryable<TextCodePM> GetTextCodes()
        {
            TextCodeRepository = new TextCodeRepository(0);
            this.ChangeConnectionString(0);
            textCodeQuery = new TextCodeQuery(TextCodeRepository);
            return textCodeQuery.GetTextCodePMsByTenant(0);
        }

        public IQueryable<TextCodePM> GetTextCodesByTenant(int tenant)
        {
            TextCodeRepository = new TextCodeRepository(tenant);
            this.ChangeConnectionString(tenant);
            textCodeQuery = new TextCodeQuery(TextCodeRepository);
            return textCodeQuery.GetTextCodePMsByTenant(tenant).Where(t => t.Tenant == tenant);
        }

        public IQueryable<TextCodePM> GetTabels(int tenant)
        {
            TextCodeRepository = new TextCodeRepository(tenant);
            //this.ChangeConnectionString(tenant);
            textCodeQuery = new TextCodeQuery(TextCodeRepository);
            return textCodeQuery.GetTextCodePMsByTenant(tenant).Where(T => T.TextCodeTypeCode == "T");
        }

        [InvokeAttribute]
        public int GetTextCodesCountForDefaultTranslation(int tenant, string objectTableId, string isSpellCheckedCode, string textCodeTypeCode, DateTime? selectedCheckDate, string checkDateFiler,string searchText)
        {
            TextCodeRepository = new TextCodeRepository(tenant);
            return TextCodeRepository.GetTextCodesCountForDefaultTranslation(tenant, objectTableId, isSpellCheckedCode, textCodeTypeCode, selectedCheckDate, checkDateFiler, searchText);
        }

        //public IQueryable<TextCodePM> GetTextCodesForDefaultTranslation(int tenant,string objectTableId, int skipDigit, int takeDigit)
        //{
        //    return TextCodeRepository.GetTextCodesForDefaultTranslation(tenant, objectTableId, skipDigit, takeDigit);
        //}

        public List<TextCodePM> GetFilteredTextCodesForDefaultTranslation(int tenant, string objectTableId, string isSpellCheckedCode, string textCodeTypeCode, DateTime? selectedCheckDate, string checkDateFiler, string searchText, int skipDigit, int takeDigit)
        {
            TextCodeRepository = new TextCodeRepository(tenant);
            textCodeQuery = new TextCodeQuery(TextCodeRepository);
            return textCodeQuery.GetFilteredTextCodesForDefaultTranslation(tenant, objectTableId, isSpellCheckedCode, textCodeTypeCode, selectedCheckDate, checkDateFiler, searchText, skipDigit, takeDigit);
        }
        
        //public void MapTextCodePMTextCode(TextCodePM textCodePM, TextCode textCode)
        //{
        //    textCode.Code = textCodePM.Code;
        //    textCode.DefaultText = textCodePM.DefaultText;
        //    textCode.DefaultTextPlural = textCodePM.DefaultTextPlural;
        //    textCode.ObjectTableId = textCodePM.ObjectTableId;           
        //    textCode.Tenant = textCodePM.Tenant;
        //    textCode.TextCodeTypeCode = textCodePM.TextCodeTypeCode;
        //    textCode.IsSpellChecked = textCodePM.IsSpellChecked;
        //    textCode.SpellCheckedByUserId = textCodePM.SpellCheckedByUserId;
        //    textCode.SpellCheckDate = textCodePM.SpellCheckDate;
        //    textCode.InActive = textCodePM.InActive;
        //}

        public void InsertTextCode(TextCodePM textCode)
        {
            string tenantCodesListName = "tenanttextcodes" + textCode.Tenant;
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

            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(textCode.Tenant);
            }
            TextCodeService service = new TextCodeService(ObjectContext , textCode.Tenant);
            service.Create(textCode);

            //TextCodeRepository = new TextCodeRepository(ObjectContext);
            //this.ChangeConnectionString(textCode.Tenant);
            //if (textCode.DefaultText == null)
            //{ 
            //}
            //textCode.Id = IdCounter.GetNumber("TextCode", textCode.Tenant).ToString();

            //TextCode newTextCode = new TextCode();
            //newTextCode.Id = textCode.Id;
            //MapTextCodePMTextCode(textCode, newTextCode);
            //TextCodeRepository.Add(newTextCode);
        }

        public void UpdateTextCode(TextCodePM currentTextCode)
        {
            string tenantCodesListName = "tenanttextcodes" + currentTextCode.Tenant;
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
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(currentTextCode.Tenant);
            }
         
            TextCodeService service = new TextCodeService(ObjectContext, currentTextCode.Tenant);
            service.Update(currentTextCode);

            //TextCodeRepository = new TextCodeRepository(ObjectContext);
            //this.ChangeConnectionString(currentTextCode.Tenant);
            //TextCode entity = TextCodeRepository.GetSingleTextCode(currentTextCode.Id);
            //MapTextCodePMTextCode(currentTextCode, entity);
            //TextCodeRepository.Update(entity);
        }

        public string GetTextCodeIdByCode(string code, int tenant)
        {
            TextCodeRepository textCodeRepository = new TextCodeRepository(tenant);
            string textCodeId = textCodeRepository.GetSingleTextCodeByCode(code, tenant);
            return textCodeId;
        }

        public void DeleteTextCode(TextCodePM textCode)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(textCode.Tenant);
            }
            TextCodeRepository = new TextCodeRepository(ObjectContext);
            this.ChangeConnectionString(textCode.Tenant);
            TextCode entity = TextCodeRepository.GetSingleTextCode(textCode.Id);
            TextCodeRepository.Remove(entity);
        }
        #endregion

        #region Translations
        public List<Translation> GetTranslations(int tenant)
        {
            TranslationRepository = new TranslationRepository(tenant);
            this.ChangeConnectionString(tenant);
            return TranslationRepository.GetTranslationsByTenant(tenant);
        }

        public List<Translation> GetTranslationForTextCode(string txtCode, int tenant)
        {
            TranslationRepository = new TranslationRepository(tenant);
            this.ChangeConnectionString(tenant);
            return TranslationRepository.GetTranslationsByTenant(tenant).Where(t=>t.TextCode.Code == txtCode).ToList();
        }


        #region Translations Methods

        public FieldsTranslations GetSingleFieldTranslationForTextCodeId(string Id, int tenant)
        {
            string email = SecurityUtility.GetAuthenticatedUser();
            Contact contact = null;
            if (!string.IsNullOrEmpty(email))
            {
                ContactRepository contactrep = new ContactRepository(tenant);
                contact = contactrep.GetSingleContactByEmail(email, tenant);
            }

            ObjectTableRepository obTableRepository = new ObjectTableRepository(tenant);
            TranslationRepository = new TranslationRepository(tenant);
            this.ChangeConnectionString(tenant);

            TextCodeRepository = new TextCodeRepository(tenant);


            Dictionary<string, Translation> defaultTranslationsDictionary = new Dictionary<string, Translation>();
            List<Translation> defaultTranslationsList = TranslationRepository.GetTranslationsByTenant(tenant).Where(t => t.TextCode.Id == Id).ToList();
            foreach (Translation translation in defaultTranslationsList)
            {
                TextCode textcode = TextCodeRepository.GetTenantTextCodesWithTenantZero(translation.Tenant).Where(t => t.Code == translation.TextCodeCode).FirstOrDefault();
                defaultTranslationsDictionary.Add(textcode.Code, translation);
            }

            TextCode defaultTextCode = TextCodeRepository.GetSingleTextCode(Id);
            Translation defaultTranslation = null;
            if (defaultTranslationsDictionary.Count != 0)
            {
                if (defaultTranslationsDictionary.ContainsKey(defaultTextCode.Code))
                {
                    defaultTranslation = defaultTranslationsDictionary[defaultTextCode.Code];
                }
            }

            FieldsTranslations ft = new FieldsTranslations();
            if (defaultTranslation != null)
            {
                ft.IsTranslated = true;
                ft.TranslateDate = defaultTranslation.TranslateDate;
                ft.TranslatedText = defaultTranslation.TranslatedText;
                ft.TranslatedByUserId = defaultTranslation.TranslatedByUserId;
                ft.TranslatedTextPlural = defaultTranslation.TranslatedTextPlural;
            }
            else
            {


                if (contact.DontShowLocalLabels)
                {
                    ft.TranslatedText = defaultTextCode.DefaultText;
                    ft.TranslatedTextPlural = defaultTextCode.DefaultTextPlural;
                }
                else
                {
                    ft.TranslatedText = !string.IsNullOrEmpty(defaultTextCode.LocalDefaultText) ? defaultTextCode.LocalDefaultText : defaultTextCode.DefaultText;
                    ft.TranslatedTextPlural = defaultTextCode.DefaultTextPlural;
                }
            }


            ft.Tenant = defaultTextCode.Tenant;//tc.ObjectTable.Tenant;
            ft.DefaultText = defaultTextCode.DefaultText;
            ft.DefaultTextPlural = defaultTextCode.DefaultTextPlural;
            ft.Code = defaultTextCode.Code;
            ft.TextCodeId = defaultTextCode.Id;
            ft.TextCodeCode = defaultTextCode.Code;
            ft.TranslationTenent = tenant;
            //ft.TranslationLanguageCode = translationLanguageCode;
            ft.TypeCode = defaultTextCode.TextCodeTypeCode;
            ft.ObjectTableID = defaultTextCode.ObjectTableId;

            ObjectTablePM obTablePM = ObjectTableQuery.GetSingleObjectTableById(defaultTextCode.ObjectTableId, defaultTextCode.Tenant);
            if (obTablePM != null)
            {
                ft.ObjectTableName = obTablePM.Name;
            }
            else
            {
                ObjectTable obTable = obTableRepository.GetSingleObjectTable(defaultTextCode.ObjectTableId, defaultTextCode.Tenant, false);
                if (obTable != null)
                {
                    ft.ObjectTableName = obTable.Name;
                }
            }




            return ft;
        }

        public FieldsTranslations GetSingleFieldTranslationForTextCode(string txtCode, int tenant)
        {
            string email = SecurityUtility.GetAuthenticatedUser();
            Contact contact = null;
            if (!string.IsNullOrEmpty(email))
            {
                ContactRepository contactrep = new ContactRepository(tenant);
                contact = contactrep.GetSingleContactByEmail(email, tenant);
            }

            ObjectTableRepository obTableRepository = new ObjectTableRepository(tenant);
            TranslationRepository = new TranslationRepository(tenant);
            this.ChangeConnectionString(tenant);

            TextCodeRepository = new TextCodeRepository(tenant);
          

             Dictionary<string, Translation> defaultTranslationsDictionary = new Dictionary<string, Translation>();
             List<Translation> defaultTranslationsList = TranslationRepository.GetTranslationsByTenant(tenant).Where(t => t.TextCode.Code == txtCode).ToList();
             foreach (Translation translation in defaultTranslationsList)
             {
                 TextCode textcode = TextCodeRepository.GetTenantTextCodesWithTenantZero(translation.Tenant).Where(t => t.Code == translation.TextCodeCode).FirstOrDefault();
                 defaultTranslationsDictionary.Add(textcode.Code, translation);
             }

            TextCode defaultTextCode = TextCodeRepository.GetTextCodeByTenantAndCode(txtCode, tenant);
            Translation defaultTranslation = null;
            if (defaultTranslationsDictionary.Count != 0)
            {
                if (defaultTranslationsDictionary.ContainsKey(defaultTextCode.Code))
                {
                    defaultTranslation = defaultTranslationsDictionary[defaultTextCode.Code];
                }
            }

            FieldsTranslations ft = new FieldsTranslations();
            if (defaultTranslation != null)
            {
                ft.IsTranslated = true;
                ft.TranslateDate = defaultTranslation.TranslateDate;
                ft.TranslatedText = defaultTranslation.TranslatedText;
                ft.TranslatedByUserId = defaultTranslation.TranslatedByUserId;
                ft.TranslatedTextPlural = defaultTranslation.TranslatedTextPlural;
            }
            else
            {


                if (contact.DontShowLocalLabels)
                {
                    ft.TranslatedText = defaultTextCode.DefaultText;
                    ft.TranslatedTextPlural = defaultTextCode.DefaultTextPlural;
                }
                else
                {
                    ft.TranslatedText = !string.IsNullOrEmpty(defaultTextCode.LocalDefaultText) ? defaultTextCode.LocalDefaultText : defaultTextCode.DefaultText;
                    ft.TranslatedTextPlural = defaultTextCode.DefaultTextPlural;
                }
            }


            ft.Tenant = defaultTextCode.Tenant;//tc.ObjectTable.Tenant;
            ft.DefaultText = defaultTextCode.DefaultText;
            ft.DefaultTextPlural = defaultTextCode.DefaultTextPlural;
            ft.Code = defaultTextCode.Code;
            ft.TextCodeId = defaultTextCode.Id;
            ft.TextCodeCode = defaultTextCode.Code;
            ft.TranslationTenent = tenant;
            //ft.TranslationLanguageCode = translationLanguageCode;
            ft.TypeCode = defaultTextCode.TextCodeTypeCode;
            ft.ObjectTableID = defaultTextCode.ObjectTableId;

            ObjectTablePM obTablePM = ObjectTableQuery.GetSingleObjectTableById(defaultTextCode.ObjectTableId, defaultTextCode.Tenant);
            if (obTablePM != null)
            {
                ft.ObjectTableName = obTablePM.Name;
            }
            else
            {
                ObjectTable obTable = obTableRepository.GetSingleObjectTable(defaultTextCode.ObjectTableId, defaultTextCode.Tenant, false);
                if (obTable != null)
                {
                    ft.ObjectTableName = obTable.Name;
                }
            }


             

             return ft;
        }
        //-- 01
        public List<FieldsTranslations> GetAllFieldsTranslations(int translationTenant, string translationLanguageCode,int skip,int take)
        {
            string email = SecurityUtility.GetAuthenticatedUser();
            Contact contact = null;
            if (!string.IsNullOrEmpty(email))
            {
                ContactRepository contactrep = new ContactRepository(translationTenant);
                contact = contactrep.GetSingleContactByEmail(email, translationTenant);
            }
            TranslationRepository = new TranslationRepository(translationTenant);
            TextCodeRepository = new TextCodeRepository(translationTenant);
            ObjectTableRepository obTableRepository = new ObjectTableRepository(translationTenant);
            this.ChangeConnectionString(translationTenant);
            //GeneralDomainService defaultDomain = new GeneralDomainService();
            List<FieldsTranslations> fieldsTranslationList = new List<FieldsTranslations>();
            List<TextCode> textCodesList = TextCodeRepository.GetTextCodesByTenantStep(translationTenant,skip,take).Where(d => d.InActive == false).ToList<TextCode>();
            Dictionary<string, Translation> translationsDictionary = new Dictionary<string, Translation>();
            Dictionary<string, Translation> defaultTranslationsDictionary = new Dictionary<string, Translation>();
            //TranslationHeaderRepository translationHeaderRepository = new Simplog.Data.InfrastructureModel.Repositories.TranslationHeaderRepository(translationTenant);
            //List<TranslationHeader> translationHeaders = translationHeaderRepository.GetTranslationHeaders().ToList();
            List<Translation> translationsList = TranslationRepository.GetTranslationsByTenant(translationTenant).Where(w => w.Tenant == translationTenant && w.TranslationHeaderCode == translationLanguageCode).ToList<Translation>();

            List<Translation> defaultTranslationsList = TranslationRepository.GetTranslationsWithoutESByTenant(0).Where(d => d.TranslationHeaderCode == translationLanguageCode && d.Tenant == 0).ToList(); //TranslationRepository.GetTranslations().Where(w => w.TranslationHeader.Tenant == 0 && w.TranslationHeader.Description == TranslationLanguage).ToList<Translation>();

            foreach (Translation translation in translationsList)
            {
                TextCode textcode = TextCodeRepository.GetTenantTextCodesWithTenantZero(translation.Tenant).Where(t => t.Code == translation.TextCodeCode).FirstOrDefault();
                translationsDictionary.Add(textcode.Code, translation);
            }

            foreach (Translation translation in defaultTranslationsList)
            {
                TextCode textcode = TextCodeRepository.GetTenantTextCodesWithTenantZero(translation.Tenant).Where(t => t.Code == translation.TextCodeCode).FirstOrDefault();
                defaultTranslationsDictionary.Add(textcode.Code, translation);
            }

            foreach (TextCode tc in textCodesList)
            {
                Translation translaion = null;
                if (translationsDictionary.Count != 0)
                {
                    if (translationsDictionary.ContainsKey(tc.Code))
                    {
                        translaion = translationsDictionary[tc.Code];
                    }
                }

                FieldsTranslations ft = new FieldsTranslations();

                if (translaion != null)
                {
                    ft.IsTranslated = true;
                    ft.TranslateDate = translaion.TranslateDate;
                    ft.TranslatedText = translaion.TranslatedText;
                    ft.TranslatedByUserId = translaion.TranslatedByUserId;
                    ft.TranslatedTextPlural = translaion.TranslatedTextPlural;
                }

                else
                {
                    Translation defaultTranslation = null;
                    if (defaultTranslationsDictionary.Count != 0)
                    {
                        if (defaultTranslationsDictionary.ContainsKey(tc.Code))
                        {
                            defaultTranslation = defaultTranslationsDictionary[tc.Code];
                        }
                    }

                    if (defaultTranslation != null)
                    {
                        ft.IsTranslated = true;
                        ft.TranslateDate = defaultTranslation.TranslateDate;
                        ft.TranslatedText = defaultTranslation.TranslatedText;
                        ft.TranslatedByUserId = defaultTranslation.TranslatedByUserId;
                        ft.TranslatedTextPlural = defaultTranslation.TranslatedTextPlural;
                    }
                    else
                    {
                       

                        if (contact.DontShowLocalLabels)
                        {
                            ft.TranslatedText = tc.DefaultText;
                            ft.TranslatedTextPlural = tc.DefaultTextPlural;
                        }
                        else
                        {
                            ft.TranslatedText = !string.IsNullOrEmpty(tc.LocalDefaultText) ? tc.LocalDefaultText : tc.DefaultText;
                            ft.TranslatedTextPlural = tc.DefaultTextPlural;
                        }
                    }
                }
                ft.Tenant = tc.Tenant;//tc.ObjectTable.Tenant;
                ft.DefaultText = tc.DefaultText;
                ft.DefaultTextPlural = tc.DefaultTextPlural;
                ft.Code = tc.Code;
                ft.TextCodeId = tc.Id;
                ft.TextCodeCode = tc.Code;
                ft.TranslationTenent = translationTenant;
                ft.TranslationLanguageCode = translationLanguageCode;
                ft.TypeCode = tc.TextCodeTypeCode;
                ft.ObjectTableID = tc.ObjectTableId;

                ObjectTablePM obTablePM = ObjectTableQuery.GetSingleObjectTableById(tc.ObjectTableId, tc.Tenant);
                if (obTablePM != null)
                {
                    ft.ObjectTableName = obTablePM.Name;
                }
                else 
                {
                    ObjectTable obTable = obTableRepository.GetSingleObjectTable(tc.ObjectTableId, tc.Tenant, false);
                    if (obTable != null)
                    {
                        ft.ObjectTableName = obTable.Name;
                    }
                }

                fieldsTranslationList.Add(ft);
            }

            return fieldsTranslationList;
        }

        // Test for controller
        public TranslationArgs GetAllFieldsTranslationsArgs(int translationTenant, string translationLanguageCode, int skip, int take)
        {
            string email = SecurityUtility.GetAuthenticatedUser();
            Contact contact = null;
            if (!string.IsNullOrEmpty(email))
            {
                ContactRepository contactrep = new ContactRepository(translationTenant);
                contact = contactrep.GetSingleContactByEmail(email, translationTenant);
            }
            TranslationRepository = new TranslationRepository(translationTenant);
            TextCodeRepository = new TextCodeRepository(translationTenant);
            ObjectTableRepository obTableRepository = new ObjectTableRepository(translationTenant);
            this.ChangeConnectionString(translationTenant);
            List<FieldsTranslations> fieldsTranslationList = new List<FieldsTranslations>();
            List<TextCode> textCodesList = TextCodeRepository.GetTextCodesByTenantStep(translationTenant, skip, take).Where(d => d.InActive == false).OrderBy(o => o.Code).ToList<TextCode>();
            Dictionary<string, Translation> translationsDictionary = new Dictionary<string, Translation>();
            Dictionary<string, Translation> defaultTranslationsDictionary = new Dictionary<string, Translation>();
            List<Translation> translationsList = TranslationRepository.GetTranslationsByTenant(translationTenant).Where(w => w.Tenant == translationTenant && w.TranslationHeaderCode == translationLanguageCode).ToList<Translation>();

            List<Translation> defaultTranslationsList = TranslationRepository.GetTranslationsWithoutESByTenant(0).Where(d => d.TranslationHeaderCode == translationLanguageCode && d.Tenant == 0).ToList(); //TranslationRepository.GetTranslations().Where(w => w.TranslationHeader.Tenant == 0 && w.TranslationHeader.Description == TranslationLanguage).ToList<Translation>();
            var translationArgs = new TranslationArgs();
            translationArgs.CountAll = TextCodeRepository.GetTextCodesByTenantCount(translationTenant);

            foreach (Translation translation in translationsList)
            {
                TextCode textcode = TextCodeRepository.GetTenantTextCodesWithTenantZero(translation.Tenant).Where(t => t.Code == translation.TextCodeCode).FirstOrDefault();
                translationsDictionary.Add(textcode.Code, translation);
            }

            foreach (Translation translation in defaultTranslationsList)
            {
                TextCode textcode = TextCodeRepository.GetTenantTextCodesWithTenantZero(translation.Tenant).Where(t => t.Code == translation.TextCodeCode).FirstOrDefault();
                defaultTranslationsDictionary.Add(textcode.Code, translation);
            }

            foreach (TextCode tc in textCodesList)
            {
                Translation translaion = null;
                if (translationsDictionary.Count != 0)
                {
                    if (translationsDictionary.ContainsKey(tc.Code))
                    {
                        translaion = translationsDictionary[tc.Code];
                    }
                }

                FieldsTranslations ft = new FieldsTranslations();

                if (translaion != null)
                {
                    ft.IsTranslated = true;
                    ft.TranslateDate = translaion.TranslateDate;
                    ft.TranslatedText = translaion.TranslatedText;
                    ft.TranslatedByUserId = translaion.TranslatedByUserId;
                    ft.TranslatedTextPlural = translaion.TranslatedTextPlural;
                }

                else
                {
                    Translation defaultTranslation = null;
                    if (defaultTranslationsDictionary.Count != 0)
                    {
                        if (defaultTranslationsDictionary.ContainsKey(tc.Code))
                        {
                            defaultTranslation = defaultTranslationsDictionary[tc.Code];
                        }
                    }

                    if (defaultTranslation != null)
                    {
                        ft.IsTranslated = true;
                        ft.TranslateDate = defaultTranslation.TranslateDate;
                        ft.TranslatedText = defaultTranslation.TranslatedText;
                        ft.TranslatedByUserId = defaultTranslation.TranslatedByUserId;
                        ft.TranslatedTextPlural = defaultTranslation.TranslatedTextPlural;
                    }
                    else
                    {


                        if (contact.DontShowLocalLabels)
                        {
                            ft.TranslatedText = tc.DefaultText;
                            ft.TranslatedTextPlural = tc.DefaultTextPlural;
                        }
                        else
                        {
                            ft.TranslatedText = !string.IsNullOrEmpty(tc.LocalDefaultText) ? tc.LocalDefaultText : tc.DefaultText;
                            ft.TranslatedTextPlural = tc.DefaultTextPlural;
                        }
                    }
                }
                ft.Tenant = tc.Tenant;//tc.ObjectTable.Tenant;
                ft.DefaultText = tc.DefaultText;
                ft.DefaultTextPlural = tc.DefaultTextPlural;
                ft.Code = tc.Code;
                ft.TextCodeId = tc.Id;
                ft.TextCodeCode = tc.Code;
                ft.TranslationTenent = translationTenant;
                ft.TranslationLanguageCode = translationLanguageCode;
                ft.TypeCode = tc.TextCodeTypeCode;
                ft.ObjectTableID = tc.ObjectTableId;

                ObjectTablePM obTablePM = ObjectTableQuery.GetSingleObjectTableById(tc.ObjectTableId, tc.Tenant);
                if (obTablePM != null)
                {
                    ft.ObjectTableName = obTablePM.Name;
                }
                else
                {
                    ObjectTable obTable = obTableRepository.GetSingleObjectTable(tc.ObjectTableId, tc.Tenant, false);
                    if (obTable != null)
                    {
                        ft.ObjectTableName = obTable.Name;
                    }
                }

                fieldsTranslationList.Add(ft);
            }

            translationArgs.FieldsTranslations = fieldsTranslationList;
            return translationArgs;
        }
        ////-- 02
        //public List<FieldsTranslations> GetMasterFieldsTranslations(int translationTenant, string translationLanguageCode)
        //{
        //    TranslationRepository = new TranslationRepository(translationTenant);
        //    TextCodeRepository = new TextCodeRepository(translationTenant);
        //    this.ChangeConnectionString(translationTenant);
        //    GeneralDomainService defaultDomain = new GeneralDomainService();
        //    List<FieldsTranslations> fieldsTranslationList = new List<FieldsTranslations>();
        //    List<TextCode> textCodesList = TextCodeRepository.GetMasterTextCodesByTenant(translationTenant).Where(d => d.InActive == false).ToList<TextCode>();
        //    Dictionary<string, Translation> translationsDictionary = new Dictionary<string, Translation>();
        //    Dictionary<string, Translation> defaultTranslationsDictionary = new Dictionary<string, Translation>();
        //    List<Translation> translationsList = TranslationRepository.GetTranslationsByTenant(translationTenant).Where(w => w.Tenant == translationTenant && w.TranslationHeader.Code == translationLanguageCode).ToList<Translation>();

        //    List<Translation> defaultTranslationsList = defaultDomain.GetTranslations(0).Where(d => d.TranslationHeader.Code == translationLanguageCode && d.Tenant == 0).ToList(); //TranslationRepository.GetTranslations().Where(w => w.TranslationHeader.Tenant == 0 && w.TranslationHeader.Description == TranslationLanguage).ToList<Translation>();

        //    foreach (Translation translation in translationsList)
        //    {
        //        translationsDictionary.Add(translation.TextCode.Code, translation);
        //    }

        //    foreach (Translation translation in defaultTranslationsList)
        //    {
        //        defaultTranslationsDictionary.Add(translation.TextCode.Code, translation);
        //    }

        //    foreach (TextCode tc in textCodesList)
        //    {
        //        Translation translaion = null;
        //        if (translationsDictionary.Count != 0)
        //        {
        //            if (translationsDictionary.ContainsKey(tc.Code))
        //            {
        //                translaion = translationsDictionary[tc.Code];
        //            }
        //        }

        //        //Translation translaion = (from t in translationsList
        //        //                          where t.TextCode.Code == tc.Code
        //        //                          select t).FirstOrDefault();
        //        //TranslationRepository.GetTranslations().Where(t => t.TextCode.Code == tc.Code && t.TranslationHeader.Tenant == translationTenant).FirstOrDefault();

        //        FieldsTranslations ft = new FieldsTranslations();

        //        //tc
        //        //translaion

        //        if (translaion != null)
        //        {
        //            ft.IsTranslated = true;
        //            ft.TranslateDate = translaion.TranslateDate;
        //            ft.TranslatedText = translaion.TranslatedText;
        //            ft.TranslatedByUserId = translaion.TranslatedByUserId;
        //            ft.TranslatedTextPlural = translaion.TranslatedTextPlural;
        //        }

        //        else
        //        {
        //            Translation defaultTranslation = null;
        //            if (defaultTranslationsDictionary.Count != 0)
        //            {
        //                if (defaultTranslationsDictionary.ContainsKey(tc.Code))
        //                {
        //                    defaultTranslation = defaultTranslationsDictionary[tc.Code];
        //                }
        //            }
        //            //(from a in defaultTranslationsList
        //            //where a.TextCode.Code == tc.Code
        //            //select a).FirstOrDefault();
        //            if (defaultTranslation != null)
        //            {
        //                ft.IsTranslated = true;
        //                ft.TranslateDate = defaultTranslation.TranslateDate;
        //                ft.TranslatedText = defaultTranslation.TranslatedText;
        //                ft.TranslatedByUserId = defaultTranslation.TranslatedByUserId;
        //                ft.TranslatedTextPlural = defaultTranslation.TranslatedTextPlural;
        //            }
        //            else
        //            {
        //                ft.TranslatedText = tc.DefaultText;
        //                ft.TranslatedTextPlural = tc.DefaultTextPlural;
        //            }
        //        }


        //        ft.Tenant = tc.Tenant; //tc.ObjectTable.Tenant;
        //        ft.DefaultText = tc.DefaultText;
        //        ft.DefaultTextPlural = tc.DefaultTextPlural;
        //        ft.Code = tc.Code;
        //        ft.TextCodeId = tc.Id;
        //        ft.TranslationTenent = translationTenant;
        //        ft.TranslationLanguageCode = translationLanguageCode;
        //        ft.TypeCode = tc.TextCodeTypeCode;
        //        ft.ObjectTableID = tc.ObjectTableId;
        //        ft.ObjectTableName = ObjectTabelQuery.GetSingleObjectTableById(tc.ObjectTableId, tc.Tenant).Name;//tc.ObjectTable.Name;//ObjectTableRepository.GetObjectsByTenant(tc.).Where(o => o.Id == ft.ObjectTableID).FirstOrDefault().Name;
        //        fieldsTranslationList.Add(ft);
        //    }


        //    return fieldsTranslationList;
        //}

        ////-- 03
        //public List<FieldsTranslations> GetQuoteFieldsTranslations(int translationTenant, string translationLanguageCode)
        //{
        //    TranslationRepository = new TranslationRepository(translationTenant);
        //    TextCodeRepository = new TextCodeRepository(translationTenant);
        //    this.ChangeConnectionString(translationTenant);
        //    GeneralDomainService defaultDomain = new GeneralDomainService();
        //    List<FieldsTranslations> fieldsTranslationList = new List<FieldsTranslations>();
        //    List<TextCode> textCodesList = TextCodeRepository.GetQuoteTextCodesByTenant(translationTenant).Where(d => d.InActive == false).ToList<TextCode>();
        //    Dictionary<string, Translation> translationsDictionary = new Dictionary<string, Translation>();
        //    Dictionary<string, Translation> defaultTranslationsDictionary = new Dictionary<string, Translation>();
        //    List<Translation> translationsList = TranslationRepository.GetTranslationsByTenant(translationTenant).Where(w => w.Tenant == translationTenant && w.TranslationHeader.Code == translationLanguageCode).ToList<Translation>();

        //    List<Translation> defaultTranslationsList = defaultDomain.GetTranslations(0).Where(d => d.TranslationHeader.Code == translationLanguageCode && d.Tenant == 0).ToList(); //TranslationRepository.GetTranslations().Where(w => w.TranslationHeader.Tenant == 0 && w.TranslationHeader.Description == TranslationLanguage).ToList<Translation>();

        //    foreach (Translation translation in translationsList)
        //    {
        //        translationsDictionary.Add(translation.TextCode.Code, translation);
        //    }

        //    foreach (Translation translation in defaultTranslationsList)
        //    {
        //        defaultTranslationsDictionary.Add(translation.TextCode.Code, translation);
        //    }


        //    foreach (TextCode tc in textCodesList)
        //    {
        //        Translation translaion = null;
        //        if (translationsDictionary.Count != 0)
        //        {
        //            if (translationsDictionary.ContainsKey(tc.Code))
        //            {
        //                translaion = translationsDictionary[tc.Code];
        //            }
        //        }

        //        //Translation translaion = (from t in translationsList
        //        //                          where t.TextCode.Code == tc.Code
        //        //                          select t).FirstOrDefault();

        //        //TranslationRepository.GetTranslations().Where(t => t.TextCode.Code == tc.Code && t.TranslationHeader.Tenant == translationTenant).FirstOrDefault();

        //        FieldsTranslations ft = new FieldsTranslations();

        //        //tc
        //        //translaion

        //        if (translaion != null)
        //        {
        //            ft.IsTranslated = true;
        //            ft.TranslateDate = translaion.TranslateDate;
        //            ft.TranslatedText = translaion.TranslatedText;
        //            ft.TranslatedByUserId = translaion.TranslatedByUserId;
        //            ft.TranslatedTextPlural = translaion.TranslatedTextPlural;
        //        }

        //        else
        //        {
        //            Translation defaultTranslation = null;
        //            if (defaultTranslationsDictionary.Count != 0)
        //            {
        //                if (defaultTranslationsDictionary.ContainsKey(tc.Code))
        //                {
        //                    defaultTranslation = defaultTranslationsDictionary[tc.Code];
        //                }
        //            }
        //            //(from a in defaultTranslationsList
        //            //where a.TextCode.Code == tc.Code
        //            //select a).FirstOrDefault();
        //            if (defaultTranslation != null)
        //            {
        //                ft.IsTranslated = true;
        //                ft.TranslateDate = defaultTranslation.TranslateDate;
        //                ft.TranslatedText = defaultTranslation.TranslatedText;
        //                ft.TranslatedByUserId = defaultTranslation.TranslatedByUserId;
        //                ft.TranslatedTextPlural = defaultTranslation.TranslatedTextPlural;
        //            }
        //            else
        //            {
        //                ft.TranslatedText = tc.DefaultText;
        //                ft.TranslatedTextPlural = tc.DefaultTextPlural;
        //            }
        //        }


        //        ft.Tenant = tc.Tenant; //tc.ObjectTable.Tenant;
        //        ft.DefaultText = tc.DefaultText;
        //        ft.DefaultTextPlural = tc.DefaultTextPlural;
        //        ft.Code = tc.Code;
        //        ft.TextCodeId = tc.Id;
        //        ft.TranslationTenent = translationTenant;
        //        ft.TranslationLanguageCode = translationLanguageCode;
        //        ft.TypeCode = tc.TextCodeTypeCode;
        //        ft.ObjectTableID = tc.ObjectTableId;
        //        ft.ObjectTableName = ObjectTabelQuery.GetSingleObjectTableById(tc.ObjectTableId, tc.Tenant).Name;//tc.ObjectTable.Name;//ObjectTableRepository.GetObjectsByTenant(tc.).Where(o => o.Id == ft.ObjectTableID).FirstOrDefault().Name;
        //        fieldsTranslationList.Add(ft);
        //    }


        //    return fieldsTranslationList;
        //}

        ////-- 04
        //public List<FieldsTranslations> GetInvoiceFieldsTranslations(int translationTenant, string translationLanguageCode)
        //{

        //    //throw new Exception("failed!");
        //    TranslationRepository = new TranslationRepository(translationTenant);
        //    TextCodeRepository = new TextCodeRepository(translationTenant);
        //    this.ChangeConnectionString(translationTenant);
        //    GeneralDomainService defaultDomain = new GeneralDomainService();
        //    List<FieldsTranslations> fieldsTranslationList = new List<FieldsTranslations>();
        //    List<TextCode> textCodesList = TextCodeRepository.GetInvoiceTextCodesByTenant(translationTenant).Where(d => d.InActive == false).ToList<TextCode>();
        //    Dictionary<string, Translation> translationsDictionary = new Dictionary<string, Translation>();
        //    Dictionary<string, Translation> defaultTranslationsDictionary = new Dictionary<string, Translation>();
        //    List<Translation> translationsList = TranslationRepository.GetTranslationsByTenant(translationTenant).Where(w => w.Tenant == translationTenant && w.TranslationHeader.Code == translationLanguageCode).ToList<Translation>();

        //    List<Translation> defaultTranslationsList = defaultDomain.GetTranslations(0).Where(d => d.TranslationHeader.Code == translationLanguageCode && d.Tenant == 0).ToList(); //TranslationRepository.GetTranslations().Where(w => w.TranslationHeader.Tenant == 0 && w.TranslationHeader.Description == TranslationLanguage).ToList<Translation>();

        //    foreach (Translation translation in translationsList)
        //    {
        //        translationsDictionary.Add(translation.TextCode.Code, translation);
        //    }

        //    foreach (Translation translation in defaultTranslationsList)
        //    {
        //        defaultTranslationsDictionary.Add(translation.TextCode.Code, translation);
        //    }


        //    foreach (TextCode tc in textCodesList)
        //    {
        //        Translation translaion = null;
        //        if (translationsDictionary.Count != 0)
        //        {
        //            if (translationsDictionary.ContainsKey(tc.Code))
        //            {
        //                translaion = translationsDictionary[tc.Code];
        //            }
        //        }

        //        //Translation translaion = (from t in translationsList
        //        //                          where t.TextCode.Code == tc.Code
        //        //                          select t).FirstOrDefault();

        //        //TranslationRepository.GetTranslations().Where(t => t.TextCode.Code == tc.Code && t.TranslationHeader.Tenant == translationTenant).FirstOrDefault();

        //        FieldsTranslations ft = new FieldsTranslations();

        //        //tc
        //        //translaion

        //        if (translaion != null)
        //        {
        //            ft.IsTranslated = true;
        //            ft.TranslateDate = translaion.TranslateDate;
        //            ft.TranslatedText = translaion.TranslatedText;
        //            ft.TranslatedByUserId = translaion.TranslatedByUserId;
        //            ft.TranslatedTextPlural = translaion.TranslatedTextPlural;
        //        }

        //        else
        //        {
        //            Translation defaultTranslation = null;
        //            if (defaultTranslationsDictionary.Count != 0)
        //            {
        //                if (defaultTranslationsDictionary.ContainsKey(tc.Code))
        //                {
        //                    defaultTranslation = defaultTranslationsDictionary[tc.Code];
        //                }
        //            }
        //            //(from a in defaultTranslationsList
        //            //where a.TextCode.Code == tc.Code
        //            //select a).FirstOrDefault();
        //            if (defaultTranslation != null)
        //            {
        //                ft.IsTranslated = true;
        //                ft.TranslateDate = defaultTranslation.TranslateDate;
        //                ft.TranslatedText = defaultTranslation.TranslatedText;
        //                ft.TranslatedByUserId = defaultTranslation.TranslatedByUserId;
        //                ft.TranslatedTextPlural = defaultTranslation.TranslatedTextPlural;
        //            }
        //            else
        //            {
        //                ft.TranslatedText = tc.DefaultText;
        //                ft.TranslatedTextPlural = tc.DefaultTextPlural;
        //            }
        //        }


        //        ft.Tenant = tc.Tenant;//tc.ObjectTable.Tenant;
        //        ft.DefaultText = tc.DefaultText;
        //        ft.DefaultTextPlural = tc.DefaultTextPlural;
        //        ft.Code = tc.Code;
        //        ft.TextCodeId = tc.Id;
        //        ft.TranslationTenent = translationTenant;
        //        ft.TranslationLanguageCode = translationLanguageCode;
        //        ft.TypeCode = tc.TextCodeTypeCode;
        //        ft.ObjectTableID = tc.ObjectTableId;
        //        ft.ObjectTableName = ObjectTabelQuery.GetSingleObjectTableById(tc.ObjectTableId, tc.Tenant).Name; //tc.ObjectTable.Name;//ObjectTableRepository.GetObjectsByTenant(tc.).Where(o => o.Id == ft.ObjectTableID).FirstOrDefault().Name;
        //        fieldsTranslationList.Add(ft);
        //    }


        //    return fieldsTranslationList;
        //}

        ////-- 05
        //public List<FieldsTranslations> GetFirstCallOtherTablesFieldsTranslations(int translationTenant, string translationLanguageCode)
        //{
        //    TranslationRepository = new TranslationRepository(translationTenant);
        //    TextCodeRepository = new TextCodeRepository(translationTenant);
        //    this.ChangeConnectionString(translationTenant);
        //    GeneralDomainService defaultDomain = new GeneralDomainService();
        //    List<FieldsTranslations> fieldsTranslationList = new List<FieldsTranslations>();
        //    List<TextCode> textCodesList = TextCodeRepository.GetOtherTablesTextCodesByTenant(translationTenant, 0).Where(d => d.InActive == false).ToList<TextCode>();
        //    Dictionary<string, Translation> translationsDictionary = new Dictionary<string, Translation>();
        //    Dictionary<string, Translation> defaultTranslationsDictionary = new Dictionary<string, Translation>();
        //    List<Translation> translationsList = TranslationRepository.GetTranslationsByTenant(translationTenant).Where(w => w.Tenant == translationTenant && w.TranslationHeader.Code == translationLanguageCode).ToList<Translation>();

        //    List<Translation> defaultTranslationsList = defaultDomain.GetTranslations(0).Where(d => d.TranslationHeader.Code == translationLanguageCode && d.Tenant == 0).ToList(); //TranslationRepository.GetTranslations().Where(w => w.TranslationHeader.Tenant == 0 && w.TranslationHeader.Description == TranslationLanguage).ToList<Translation>();

        //    foreach (Translation translation in translationsList)
        //    {
        //        translationsDictionary.Add(translation.TextCode.Code, translation);
        //    }

        //    foreach (Translation translation in defaultTranslationsList)
        //    {
        //        defaultTranslationsDictionary.Add(translation.TextCode.Code, translation);
        //    }


        //    foreach (TextCode tc in textCodesList)
        //    {
        //        Translation translaion = null;
        //        if (translationsDictionary.Count != 0)
        //        {
        //            if (translationsDictionary.ContainsKey(tc.Code))
        //            {
        //                translaion = translationsDictionary[tc.Code];
        //            }
        //        }

        //        //Translation translaion = (from t in translationsList
        //        //                          where t.TextCode.Code == tc.Code
        //        //                          select t).FirstOrDefault();

        //        //TranslationRepository.GetTranslations().Where(t => t.TextCode.Code == tc.Code && t.TranslationHeader.Tenant == translationTenant).FirstOrDefault();

        //        FieldsTranslations ft = new FieldsTranslations();

        //        //tc
        //        //translaion

        //        if (translaion != null)
        //        {
        //            ft.IsTranslated = true;
        //            ft.TranslateDate = translaion.TranslateDate;
        //            ft.TranslatedText = translaion.TranslatedText;
        //            ft.TranslatedByUserId = translaion.TranslatedByUserId;
        //            ft.TranslatedTextPlural = translaion.TranslatedTextPlural;
        //        }

        //        else
        //        {
        //            Translation defaultTranslation = null;
        //            if (defaultTranslationsDictionary.Count != 0)
        //            {
        //                if (defaultTranslationsDictionary.ContainsKey(tc.Code))
        //                {
        //                    defaultTranslation = defaultTranslationsDictionary[tc.Code];
        //                }
        //            }
        //            //(from a in defaultTranslationsList
        //            //where a.TextCode.Code == tc.Code
        //            //select a).FirstOrDefault();
        //            if (defaultTranslation != null)
        //            {
        //                ft.IsTranslated = true;
        //                ft.TranslateDate = defaultTranslation.TranslateDate;
        //                ft.TranslatedText = defaultTranslation.TranslatedText;
        //                ft.TranslatedByUserId = defaultTranslation.TranslatedByUserId;
        //                ft.TranslatedTextPlural = defaultTranslation.TranslatedTextPlural;
        //            }
        //            else
        //            {
        //                ft.TranslatedText = tc.DefaultText;
        //                ft.TranslatedTextPlural = tc.DefaultTextPlural;
        //            }
        //        }


        //        ft.Tenant = tc.Tenant;//tc.ObjectTable.Tenant;
        //        ft.DefaultText = tc.DefaultText;
        //        ft.DefaultTextPlural = tc.DefaultTextPlural;
        //        ft.Code = tc.Code;
        //        ft.TextCodeId = tc.Id;
        //        ft.TranslationTenent = translationTenant;
        //        ft.TranslationLanguageCode = translationLanguageCode;
        //        ft.TypeCode = tc.TextCodeTypeCode;
        //        ft.ObjectTableID = tc.ObjectTableId;
        //        ft.ObjectTableName = ObjectTabelQuery.GetSingleObjectTableById(tc.ObjectTableId, tc.Tenant).Name;//tc.ObjectTable.Name;//ObjectTableRepository.GetObjectsByTenant(tc.).Where(o => o.Id == ft.ObjectTableID).FirstOrDefault().Name;
        //        fieldsTranslationList.Add(ft);
        //    }

        //    //List<FieldsTranslations> firstHalf = fieldsTranslationList.Take(fieldsTranslationList.Count / 2).OrderBy(f => f.Code).ToList();
        //    return fieldsTranslationList;
        //}

        ////-- 06
        //public List<FieldsTranslations> GetSecondCallOtherTablesFieldsTranslations(int translationTenant, string translationLanguageCode)
        //{
        //    TranslationRepository = new TranslationRepository(translationTenant);
        //    TextCodeRepository = new TextCodeRepository(translationTenant);
        //    this.ChangeConnectionString(translationTenant);
        //    GeneralDomainService defaultDomain = new GeneralDomainService();
        //    List<FieldsTranslations> fieldsTranslationList = new List<FieldsTranslations>();
        //    List<TextCode> textCodesList = TextCodeRepository.GetOtherTablesTextCodesByTenant(translationTenant, 1).Where(d => d.InActive == false).ToList<TextCode>();
        //    Dictionary<string, Translation> translationsDictionary = new Dictionary<string, Translation>();
        //    Dictionary<string, Translation> defaultTranslationsDictionary = new Dictionary<string, Translation>();
        //    List<Translation> translationsList = TranslationRepository.GetTranslationsByTenant(translationTenant).Where(w => w.Tenant == translationTenant && w.TranslationHeader.Code == translationLanguageCode).ToList<Translation>();

        //    List<Translation> defaultTranslationsList = defaultDomain.GetTranslations(0).Where(d => d.TranslationHeader.Code == translationLanguageCode && d.Tenant == 0).ToList(); //TranslationRepository.GetTranslations().Where(w => w.TranslationHeader.Tenant == 0 && w.TranslationHeader.Description == TranslationLanguage).ToList<Translation>();

        //    foreach (Translation translation in translationsList)
        //    {
        //        translationsDictionary.Add(translation.TextCode.Code, translation);
        //    }

        //    foreach (Translation translation in defaultTranslationsList)
        //    {
        //        defaultTranslationsDictionary.Add(translation.TextCode.Code, translation);
        //    }


        //    foreach (TextCode tc in textCodesList)
        //    {
        //        Translation translaion = null;
        //        if (translationsDictionary.Count != 0)
        //        {
        //            if (translationsDictionary.ContainsKey(tc.Code))
        //            {
        //                translaion = translationsDictionary[tc.Code];
        //            }
        //        }

        //        //Translation translaion = (from t in translationsList
        //        //                          where t.TextCode.Code == tc.Code
        //        //                          select t).FirstOrDefault();

        //        //TranslationRepository.GetTranslations().Where(t => t.TextCode.Code == tc.Code && t.TranslationHeader.Tenant == translationTenant).FirstOrDefault();

        //        FieldsTranslations ft = new FieldsTranslations();

        //        //tc
        //        //translaion

        //        if (translaion != null)
        //        {
        //            ft.IsTranslated = true;
        //            ft.TranslateDate = translaion.TranslateDate;
        //            ft.TranslatedText = translaion.TranslatedText;
        //            ft.TranslatedByUserId = translaion.TranslatedByUserId;
        //            ft.TranslatedTextPlural = translaion.TranslatedTextPlural;
        //        }

        //        else
        //        {
        //            Translation defaultTranslation = null;
        //            if (defaultTranslationsDictionary.Count != 0)
        //            {
        //                if (defaultTranslationsDictionary.ContainsKey(tc.Code))
        //                {
        //                    defaultTranslation = defaultTranslationsDictionary[tc.Code];
        //                }
        //            }
        //            //(from a in defaultTranslationsList
        //            //where a.TextCode.Code == tc.Code
        //            //select a).FirstOrDefault();
        //            if (defaultTranslation != null)
        //            {
        //                ft.IsTranslated = true;
        //                ft.TranslateDate = defaultTranslation.TranslateDate;
        //                ft.TranslatedText = defaultTranslation.TranslatedText;
        //                ft.TranslatedByUserId = defaultTranslation.TranslatedByUserId;
        //                ft.TranslatedTextPlural = defaultTranslation.TranslatedTextPlural;
        //            }
        //            else
        //            {
        //                ft.TranslatedText = tc.DefaultText;
        //                ft.TranslatedTextPlural = tc.DefaultTextPlural;
        //            }
        //        }


        //        ft.Tenant = tc.Tenant;//tc.ObjectTable.Tenant;
        //        ft.DefaultText = tc.DefaultText;
        //        ft.DefaultTextPlural = tc.DefaultTextPlural;
        //        ft.Code = tc.Code;
        //        ft.TextCodeId = tc.Id;
        //        ft.TranslationTenent = translationTenant;
        //        ft.TranslationLanguageCode = translationLanguageCode;
        //        ft.TypeCode = tc.TextCodeTypeCode;
        //        ft.ObjectTableID = tc.ObjectTableId;
        //        ft.ObjectTableName = ObjectTabelQuery.GetSingleObjectTableById(tc.ObjectTableId, tc.Tenant).Name;//tc.ObjectTable.Name;//ObjectTableRepository.GetObjectsByTenant(tc.).Where(o => o.Id == ft.ObjectTableID).FirstOrDefault().Name;
        //        fieldsTranslationList.Add(ft);
        //    }

        //    //List<FieldsTranslations> secondHalf = fieldsTranslationList.Skip(fieldsTranslationList.Count / 2).OrderBy(f => f.Code).ToList();
        //    return fieldsTranslationList;
        //}

        //public List<FieldsTranslations> GetThirdCallOtherTablesFieldsTranslations(int translationTenant, string translationLanguageCode)
        //{
        //    TranslationRepository = new TranslationRepository(translationTenant);
        //    TextCodeRepository = new TextCodeRepository(translationTenant);
        //    this.ChangeConnectionString(translationTenant);
        //    GeneralDomainService defaultDomain = new GeneralDomainService();
        //    List<FieldsTranslations> fieldsTranslationList = new List<FieldsTranslations>();
        //    List<TextCode> textCodesList = TextCodeRepository.GetOtherTablesTextCodesByTenant(translationTenant, 2).Where(d => d.InActive == false).ToList<TextCode>();
        //    Dictionary<string, Translation> translationsDictionary = new Dictionary<string, Translation>();
        //    Dictionary<string, Translation> defaultTranslationsDictionary = new Dictionary<string, Translation>();
        //    List<Translation> translationsList = TranslationRepository.GetTranslationsByTenant(translationTenant).Where(w => w.Tenant == translationTenant && w.TranslationHeader.Code == translationLanguageCode).ToList<Translation>();

        //    List<Translation> defaultTranslationsList = defaultDomain.GetTranslations(0).Where(d => d.TranslationHeader.Code == translationLanguageCode && d.Tenant == 0).ToList(); //TranslationRepository.GetTranslations().Where(w => w.TranslationHeader.Tenant == 0 && w.TranslationHeader.Description == TranslationLanguage).ToList<Translation>();

        //    foreach (Translation translation in translationsList)
        //    {
        //        translationsDictionary.Add(translation.TextCode.Code, translation);
        //    }

        //    foreach (Translation translation in defaultTranslationsList)
        //    {
        //        defaultTranslationsDictionary.Add(translation.TextCode.Code, translation);
        //    }


        //    foreach (TextCode tc in textCodesList)
        //    {
        //        Translation translaion = null;
        //        if (translationsDictionary.Count != 0)
        //        {
        //            if (translationsDictionary.ContainsKey(tc.Code))
        //            {
        //                translaion = translationsDictionary[tc.Code];
        //            }
        //        }

        //        //Translation translaion = (from t in translationsList
        //        //                          where t.TextCode.Code == tc.Code
        //        //                          select t).FirstOrDefault();

        //        //TranslationRepository.GetTranslations().Where(t => t.TextCode.Code == tc.Code && t.TranslationHeader.Tenant == translationTenant).FirstOrDefault();

        //        FieldsTranslations ft = new FieldsTranslations();

        //        //tc
        //        //translaion

        //        if (translaion != null)
        //        {
        //            ft.IsTranslated = true;
        //            ft.TranslateDate = translaion.TranslateDate;
        //            ft.TranslatedText = translaion.TranslatedText;
        //            ft.TranslatedByUserId = translaion.TranslatedByUserId;
        //            ft.TranslatedTextPlural = translaion.TranslatedTextPlural;
        //        }

        //        else
        //        {
        //            Translation defaultTranslation = null;
        //            if (defaultTranslationsDictionary.Count != 0)
        //            {
        //                if (defaultTranslationsDictionary.ContainsKey(tc.Code))
        //                {
        //                    defaultTranslation = defaultTranslationsDictionary[tc.Code];
        //                }
        //            }
        //            //(from a in defaultTranslationsList
        //            //where a.TextCode.Code == tc.Code
        //            //select a).FirstOrDefault();
        //            if (defaultTranslation != null)
        //            {
        //                ft.IsTranslated = true;
        //                ft.TranslateDate = defaultTranslation.TranslateDate;
        //                ft.TranslatedText = defaultTranslation.TranslatedText;
        //                ft.TranslatedByUserId = defaultTranslation.TranslatedByUserId;
        //                ft.TranslatedTextPlural = defaultTranslation.TranslatedTextPlural;
        //            }
        //            else
        //            {
        //                ft.TranslatedText = tc.DefaultText;
        //                ft.TranslatedTextPlural = tc.DefaultTextPlural;
        //            }
        //        }


        //        ft.Tenant = tc.Tenant;//tc.ObjectTable.Tenant;
        //        ft.DefaultText = tc.DefaultText;
        //        ft.DefaultTextPlural = tc.DefaultTextPlural;
        //        ft.Code = tc.Code;
        //        ft.TextCodeId = tc.Id;
        //        ft.TranslationTenent = translationTenant;
        //        ft.TranslationLanguageCode = translationLanguageCode;
        //        ft.TypeCode = tc.TextCodeTypeCode;
        //        ft.ObjectTableID = tc.ObjectTableId;
        //        ft.ObjectTableName = ObjectTabelQuery.GetSingleObjectTableById(tc.ObjectTableId, tc.Tenant).Name;//tc.ObjectTable.Name;//ObjectTableRepository.GetObjectsByTenant(tc.).Where(o => o.Id == ft.ObjectTableID).FirstOrDefault().Name;
        //        fieldsTranslationList.Add(ft);
        //    }

        //    //List<FieldsTranslations> secondHalf = fieldsTranslationList.Skip(fieldsTranslationList.Count / 2).OrderBy(f => f.Code).ToList();
        //    return fieldsTranslationList;
        //}

        //public List<FieldsTranslations> GetForthCallOtherTablesFieldsTranslations(int translationTenant, string translationLanguageCode)
        //{
        //    TranslationRepository = new TranslationRepository(translationTenant);
        //    TextCodeRepository = new TextCodeRepository(translationTenant);
        //    this.ChangeConnectionString(translationTenant);
        //    GeneralDomainService defaultDomain = new GeneralDomainService();
        //    List<FieldsTranslations> fieldsTranslationList = new List<FieldsTranslations>();
        //    List<TextCode> textCodesList = TextCodeRepository.GetOtherTablesTextCodesByTenant(translationTenant, 3).Where(d => d.InActive == false).ToList<TextCode>();
        //    Dictionary<string, Translation> translationsDictionary = new Dictionary<string, Translation>();
        //    Dictionary<string, Translation> defaultTranslationsDictionary = new Dictionary<string, Translation>();
        //    List<Translation> translationsList = TranslationRepository.GetTranslationsByTenant(translationTenant).Where(w => w.Tenant == translationTenant && w.TranslationHeader.Code == translationLanguageCode).ToList<Translation>();

        //    List<Translation> defaultTranslationsList = defaultDomain.GetTranslations(0).Where(d => d.TranslationHeader.Code == translationLanguageCode && d.Tenant == 0).ToList(); //TranslationRepository.GetTranslations().Where(w => w.TranslationHeader.Tenant == 0 && w.TranslationHeader.Description == TranslationLanguage).ToList<Translation>();

        //    foreach (Translation translation in translationsList)
        //    {
        //        translationsDictionary.Add(translation.TextCode.Code, translation);
        //    }

        //    foreach (Translation translation in defaultTranslationsList)
        //    {
        //        defaultTranslationsDictionary.Add(translation.TextCode.Code, translation);
        //    }


        //    foreach (TextCode tc in textCodesList)
        //    {
        //        Translation translaion = null;
        //        if (translationsDictionary.Count != 0)
        //        {
        //            if (translationsDictionary.ContainsKey(tc.Code))
        //            {
        //                translaion = translationsDictionary[tc.Code];
        //            }
        //        }

        //        //Translation translaion = (from t in translationsList
        //        //                          where t.TextCode.Code == tc.Code
        //        //                          select t).FirstOrDefault();

        //        //TranslationRepository.GetTranslations().Where(t => t.TextCode.Code == tc.Code && t.TranslationHeader.Tenant == translationTenant).FirstOrDefault();

        //        FieldsTranslations ft = new FieldsTranslations();

        //        //tc
        //        //translaion

        //        if (translaion != null)
        //        {
        //            ft.IsTranslated = true;
        //            ft.TranslateDate = translaion.TranslateDate;
        //            ft.TranslatedText = translaion.TranslatedText;
        //            ft.TranslatedByUserId = translaion.TranslatedByUserId;
        //            ft.TranslatedTextPlural = translaion.TranslatedTextPlural;
        //        }

        //        else
        //        {
        //            Translation defaultTranslation = null;
        //            if (defaultTranslationsDictionary.Count != 0)
        //            {
        //                if (defaultTranslationsDictionary.ContainsKey(tc.Code))
        //                {
        //                    defaultTranslation = defaultTranslationsDictionary[tc.Code];
        //                }
        //            }
        //            //(from a in defaultTranslationsList
        //            //where a.TextCode.Code == tc.Code
        //            //select a).FirstOrDefault();
        //            if (defaultTranslation != null)
        //            {
        //                ft.IsTranslated = true;
        //                ft.TranslateDate = defaultTranslation.TranslateDate;
        //                ft.TranslatedText = defaultTranslation.TranslatedText;
        //                ft.TranslatedByUserId = defaultTranslation.TranslatedByUserId;
        //                ft.TranslatedTextPlural = defaultTranslation.TranslatedTextPlural;
        //            }
        //            else
        //            {
        //                ft.TranslatedText = tc.DefaultText;
        //                ft.TranslatedTextPlural = tc.DefaultTextPlural;
        //            }
        //        }


        //        ft.Tenant = tc.Tenant;// tc.ObjectTable.Tenant;
        //        ft.DefaultText = tc.DefaultText;
        //        ft.DefaultTextPlural = tc.DefaultTextPlural;
        //        ft.Code = tc.Code;
        //        ft.TextCodeId = tc.Id;
        //        ft.TranslationTenent = translationTenant;
        //        ft.TranslationLanguageCode = translationLanguageCode;
        //        ft.TypeCode = tc.TextCodeTypeCode;
        //        ft.ObjectTableID = tc.ObjectTableId;
        //        ft.ObjectTableName = ObjectTabelQuery.GetSingleObjectTableById(tc.ObjectTableId, tc.Tenant).Name;//tc.ObjectTable.Name;//ObjectTableRepository.GetObjectsByTenant(tc.).Where(o => o.Id == ft.ObjectTableID).FirstOrDefault().Name;
        //        fieldsTranslationList.Add(ft);
        //    }

        //    //List<FieldsTranslations> secondHalf = fieldsTranslationList.Skip(fieldsTranslationList.Count / 2).OrderBy(f => f.Code).ToList();
        //    return fieldsTranslationList;
        //}

        //-- 07

        #endregion

        public List<FieldsTranslations> GetTranslationsForExport(int translationTenant)
        {
            TranslationRepository = new TranslationRepository(translationTenant);
            TextCodeRepository = new TextCodeRepository(translationTenant);
            this.ChangeConnectionString(translationTenant);
            List<FieldsTranslations> fieldsTranslationList = new List<FieldsTranslations>();
            List<TextCode> textCodesList = TextCodeRepository.GetTextCodesByTenant(translationTenant).Where(d => d.InActive == false).ToList<TextCode>();
            List<Translation> translationsList = TranslationRepository.GetTranslationsByTenant(translationTenant).ToList<Translation>();

            foreach (TextCode tc in textCodesList)
            {
                Translation translaion = (from t in translationsList
                                          where t.TextCode.Code == tc.Code
                                          select t).FirstOrDefault();

                FieldsTranslations ft = new FieldsTranslations();

                if (translaion != null)
                {
                    ft.IsTranslated = true;
                    ft.TranslateDate = translaion.TranslateDate;
                    ft.TranslatedText = translaion.TranslatedText;
                    ft.TranslatedByUserId = translaion.TranslatedByUserId;
                    ft.TranslatedTextPlural = translaion.TranslatedTextPlural;

                    ft.DefaultText = tc.DefaultText;
                    ft.DefaultTextPlural = tc.DefaultTextPlural;
                    ft.Code = tc.Code;
                    ft.TextCodeId = tc.Id;
                    ft.TextCodeCode = tc.Code;
                    ft.TranslationTenent = translationTenant;
                    ft.TypeCode = tc.TextCodeTypeCode;
                    ft.ObjectTableName = tc.ObjectTable.Name;
                    ft.ObjectTableID = tc.ObjectTableId;
                    ft.TranslationLanguageCode = translaion.TranslationHeaderCode;

                    fieldsTranslationList.Add(ft);
                }
                // else
                // {
                //     ft.TranslatedText = tc.DefaultText;
                //     ft.TranslatedTextPlural = tc.DefaultTextPlural;
                // }

                //// ft.Tenant = tc.ObjectTable.Tenant;
                // ft.DefaultText = tc.DefaultText;
                // ft.DefaultTextPlural = tc.DefaultTextPlural;
                // ft.Code = tc.Code;
                // ft.TextCodeId = tc.Id;
                // ft.TranslationTenent = translationTenant;
                // ft.TypeCode = tc.TextCodeTypeCode;
                // ft.ObjectTableID = tc.ObjectTableId;
                // fieldsTranslationList.Add(ft);
            }

            return fieldsTranslationList.OrderBy(f => f.Code).ToList();
        }
        //-- 08
        public List<FieldsTranslations> GetTranslationsByParam(int translationTenant, string typeCode, string tableId, string translationLanguageCode)
        {
            TextCodeRepository = new TextCodeRepository(translationTenant);
            TranslationRepository = new TranslationRepository(translationTenant);
            this.ChangeConnectionString(translationTenant);
            List<FieldsTranslations> fieldsTranslationList = new List<FieldsTranslations>();
            List<TextCode> textCodesList;
            GeneralDomainService defaultDomain = new GeneralDomainService();

            if (!string.IsNullOrEmpty(tableId) && !string.IsNullOrEmpty(typeCode))
            {
                textCodesList = TextCodeRepository.GetTextCodesByTenant(translationTenant).Where(t => t.TextCodeTypeCode == typeCode && t.ObjectTableId == tableId).ToList<TextCode>();
            }

            else if (string.IsNullOrEmpty(tableId) && !string.IsNullOrEmpty(typeCode))
            {
                textCodesList = TextCodeRepository.GetTextCodesByTenant(translationTenant).Where(t => t.TextCodeTypeCode == typeCode).ToList<TextCode>();
            }

            else if (!string.IsNullOrEmpty(tableId) && string.IsNullOrEmpty(typeCode))
            {
                textCodesList = TextCodeRepository.GetTextCodesByTenant(translationTenant).Where(t => t.ObjectTableId == tableId).ToList<TextCode>();
            }

            else
            {
                throw new ApplicationException("Error Loading Translations");
            }

            //if (typeCode == "F")
            //{
            //    textCodesList = textCodesList.Select(r=>r.ObjectTable.ObjectFields.Where(o=>o.IsCustom == false &&o.FieldLable == r.Code)))
            //}
            // GeneralDomainService defaultDomain = new GeneralDomainService();
            List<Translation> translaionList = TranslationRepository.GetTranslationsByTenant(translationTenant).Where(t => t.TranslationHeaderCode == translationLanguageCode).ToList();
            List<Translation> defaultTranslationsList = defaultDomain.GetTranslations(0).Where(d => d.TranslationHeaderCode == translationLanguageCode).ToList();//TranslationRepository.GetTranslations().Where(w => w.TranslationHeader.Tenant == 0 && w.TranslationHeader.Description == translationLanguage).ToList<Translation>();
            foreach (TextCode tc in textCodesList)
            {
                Translation translaion = translaionList.Where(t => t.TextCodeCode == tc.Code).FirstOrDefault();
                //Translation translaion = translaionList.Where(t => t.TextCode.Code == tc.Code && tc.TextCodeTypeCode == typeCode).FirstOrDefault();

                FieldsTranslations ft = new FieldsTranslations();

                if (translaion != null)
                {
                    ft.IsTranslated = true;
                    ft.TranslateDate = translaion.TranslateDate;
                    ft.TranslatedText = translaion.TranslatedText;
                    ft.TranslatedByUserId = translaion.TranslatedByUserId;
                    ft.TranslatedTextPlural = translaion.TranslatedTextPlural;
                }

                else
                {
                    Translation defaultTranslation = (from a in defaultTranslationsList
                                                      where a.TextCode.Code == tc.Code
                                                      select a).FirstOrDefault();
                    if (defaultTranslation != null)
                    {
                        ft.IsTranslated = true;
                        ft.TranslateDate = defaultTranslation.TranslateDate;
                        ft.TranslatedText = defaultTranslation.TranslatedText;
                        ft.TranslatedByUserId = defaultTranslation.TranslatedByUserId;
                        ft.TranslatedTextPlural = defaultTranslation.TranslatedTextPlural;
                    }

                    else
                    {
                        ft.TranslatedText = tc.DefaultText;
                        ft.TranslatedTextPlural = tc.DefaultTextPlural;
                    }
                    //ft.TranslatedText = tc.DefaultText;
                    //ft.TranslatedTextPlural = tc.DefaultTextPlural;
                }

                ft.Tenant = tc.Tenant;//tc.ObjectTable.Tenant;
                ft.DefaultText = tc.DefaultText;
                ft.DefaultTextPlural = tc.DefaultTextPlural;
                ft.Code = tc.Code;
                ft.TextCodeId = tc.Id;
                ft.TextCodeCode = tc.Code;
                ft.TypeCode = tc.TextCodeTypeCode;
                ft.ObjectTableID = tc.ObjectTableId;
                ft.TranslationTenent = translationTenant;
                ft.TranslationLanguageCode = translationLanguageCode;
                ft.ObjectTableName = tc.ObjectTable.Name;
                ft.ObjectTableTypeCode = tc.ObjectTable.ObjectTableTypeCode;
                fieldsTranslationList.Add(ft);
            }

            return fieldsTranslationList.OrderBy(f => f.Code).ToList();
        }

       // List<FieldsTranslations> NewFieldsTranslationsList = null;
        string[] lineOfStrings;
        string[] thisLine;
        [Invoke]
        public void GetStringOfTranslation(string linesOfStrings, int currentTenant, string translationLanguageCode)
        {
            this.ChangeConnectionString(currentTenant);
            lineOfStrings = linesOfStrings.Split('\n');

            foreach (string line in lineOfStrings)
            {
                if (line != "\r" && line != "")
                {
                    thisLine = line.Split(',');
                    FieldsTranslations newFieldTranslation = new FieldsTranslations();
                    newFieldTranslation.Code = thisLine[0];
                    newFieldTranslation.DefaultText = thisLine[1];
                    newFieldTranslation.DefaultTextPlural = thisLine[2];
                    newFieldTranslation.ObjectTableName = thisLine[3];
                   // NewFieldTranslation.TextCodeId = ThisLine[4];
                    newFieldTranslation.TranslatedText = thisLine[4];
                    newFieldTranslation.TranslatedTextPlural = thisLine[5];
                    newFieldTranslation.TranslationTenent = currentTenant;//Convert.ToInt32(ThisLine[6]);
                    newFieldTranslation.TypeCode = thisLine[7];
                    newFieldTranslation.TranslationLanguageCode = translationLanguageCode;
                  //  NewFieldTranslation.TranslatedText = ThisLine[8];
                    UpdateFieldTranslation(newFieldTranslation);
                  //  NewFieldsTranslationsList = new List<FieldsTranslations>();
                  //  NewFieldsTranslationsList.Add(NewFieldTranslation);
                }
            }        
        }

        public void UpdateFieldTranslation(FieldsTranslations currentFieldTranslation)
        {
            this.ChangeConnectionString(currentFieldTranslation.TranslationTenent);

            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(currentFieldTranslation.Tenant);
            }

            TranslationHeaderRepository = new TranslationHeaderRepository(currentFieldTranslation.Tenant);

            string email = SecurityUtility.GetAuthenticatedUser();
            Contact contact = null;
            if (!string.IsNullOrEmpty(email))
            {
                ContactRepository contactrep = new ContactRepository(currentFieldTranslation.Tenant);
                contact = contactrep.GetSingleContactByEmail(email, currentFieldTranslation.TranslationTenent);
            }

            TextCodeRepository textCodeRepository = new TextCodeRepository(ObjectContext);
            TextCode textCode = textCodeRepository.GetSingleTextCodeByCode(currentFieldTranslation.TextCodeCode);

            TextCodeRepository = new TextCodeRepository(ObjectContext);
            TranslationRepository = new TranslationRepository(ObjectContext);
            Translation currentTranslation = TranslationRepository.GetTranslationsByTenant(currentFieldTranslation.TranslationTenent).Where(t => t.TextCode.Code == currentFieldTranslation.Code && t.TranslationHeaderCode == currentFieldTranslation.TranslationLanguageCode).FirstOrDefault();

            if (currentTranslation != null)
            {
                currentTranslation.UpdateDateGMT = DateTime.UtcNow;
                bool removeTranslation = false;
                if (currentFieldTranslation.TypeCode != "H")
                {
                    if (string.IsNullOrEmpty(currentFieldTranslation.TranslatedText))
                    {
                        removeTranslation = true;
                    }

                    else if (textCode != null)
                    {
                        if (textCode.DefaultText == currentFieldTranslation.TranslatedText && textCode.DefaultTextPlural == currentFieldTranslation.TranslatedTextPlural)
                        {
                            removeTranslation = true;
                        }
                    }
                }

                if (removeTranslation)
                {
                    TranslationRepository.Remove(currentTranslation);
                }

                else
                {
                    currentTranslation.TranslatedText = currentFieldTranslation.TranslatedText;
                    currentTranslation.TranslatedTextPlural = currentFieldTranslation.TranslatedTextPlural;
                    currentTranslation.TranslateDate = TenantServerConfigration.GetCurrentDateTime(currentFieldTranslation.TranslationTenent).Date;
                    currentTranslation.TranslatedByUserId = contact != null ? contact.Id : null;//currentFieldTranslation.TranslatedByUserId;
                    TranslationRepository.Update(currentTranslation);
                }

                TranslationRepository.SubmitChanges();
            }

            else
            {
                bool addNew = true;

                if (textCode != null)
                {
                    if (textCode.DefaultText == currentFieldTranslation.TranslatedText && textCode.DefaultTextPlural == currentFieldTranslation.TranslatedTextPlural)
                    {
                        addNew = false;
                    }
                }

                if (addNew)
                {
                    Translation newTranslaion = new Translation();
                    newTranslaion.UpdateDateGMT = DateTime.UtcNow;
                    newTranslaion.Id = IdCounter.GetNumber("Translation", currentFieldTranslation.TranslationTenent).ToString();
                    newTranslaion.Tenant = currentFieldTranslation.TranslationTenent;
                    newTranslaion.TranslateDate = TenantServerConfigration.GetCurrentDateTime(currentFieldTranslation.TranslationTenent).Date;
                    newTranslaion.TranslatedByUserId = contact != null ? contact.Id : null; //currentFieldTranslation.TranslatedByUserId;
                    newTranslaion.TextCode = TextCodeRepository.GetTextCodesByTenant(currentFieldTranslation.TranslationTenent).Where(t => t.Code == currentFieldTranslation.Code).FirstOrDefault();

                    if (newTranslaion.TextCode != null)
                    {
                        newTranslaion.TextCodeId = newTranslaion.TextCode.Id;
                        newTranslaion.TextCodeCode = newTranslaion.TextCode.Code;
                        newTranslaion.TranslatedText = currentFieldTranslation.TranslatedText;
                        newTranslaion.TranslatedTextPlural = currentFieldTranslation.TranslatedTextPlural;

                        TranslationHeader translationHeader = TranslationHeaderRepository.GetTranslationHeadersByTenant(0).Where(h => h.Code == currentFieldTranslation.TranslationLanguageCode).FirstOrDefault();
                        if (translationHeader != null)
                        {
                            newTranslaion.TranslationHeaderCode = translationHeader.Code;
                        }

                        TranslationRepository.Add(newTranslaion);
                        TranslationRepository.SubmitChanges();
                    }
                }
            }

            TableLastUpdateClass.UpdateSystemMetaDataHistory(false, true);
        }

        public void InsertTranslation(Translation translation)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(translation.Tenant);
            }
            TranslationRepository = new TranslationRepository(ObjectContext);
            this.ChangeConnectionString(translation.Tenant);
            translation.Id = IdCounter.GetNumber("Translation", translation.Tenant).ToString();
            TranslationRepository.Add(translation);

            TableLastUpdateClass.UpdateSystemMetaDataHistory(false, true);
        }

        public void UpdateTranslation(Translation currentTranslation)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(currentTranslation.Tenant);
            }
            TranslationRepository = new TranslationRepository(ObjectContext);
            this.ChangeConnectionString(currentTranslation.Tenant);
            TranslationRepository.Update(currentTranslation);

            TableLastUpdateClass.UpdateSystemMetaDataHistory(false, true);
        }

        public void DeleteTextCode(Translation translation)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(translation.Tenant);
            }
            TranslationRepository = new TranslationRepository(ObjectContext);
            this.ChangeConnectionString(translation.Tenant);
            TranslationRepository.Remove(translation);
        }

        public List<FieldsTranslations> GetAllFieldsTranslationsByFilters(int tenant, string language, string objectTableId, string textCodeType)
        {
            string email = SecurityUtility.GetAuthenticatedUser();
            Contact contact = null;
            if (!string.IsNullOrEmpty(email))
            {
                ContactRepository contactrep = new ContactRepository(tenant);
                contact = contactrep.GetSingleContactByEmail(email, tenant);
            }

            TranslationRepository = new TranslationRepository(tenant);
            TextCodeRepository = new TextCodeRepository(tenant);
            ObjectTableRepository obTableRepository = new ObjectTableRepository(tenant);
            
            List<FieldsTranslations> fieldsTranslationList = new List<FieldsTranslations>();
            List<TextCode> textCodesList = TextCodeRepository.GetTenantTextCodesWithTenantZero(tenant).Where(d => d.InActive == false).ToList<TextCode>();
            Dictionary<string, Translation> translationsDictionary = new Dictionary<string, Translation>();
            Dictionary<string, Translation> defaultTranslationsDictionary = new Dictionary<string, Translation>();
            
            List<Translation> translationsList = TranslationRepository.GetTranslationsByTenant(tenant).Where(w => w.Tenant == tenant && w.TranslationHeaderCode == language).ToList<Translation>();

            List<Translation> defaultTranslationsList = TranslationRepository.GetTranslationsWithoutESByTenant(0).Where(d => d.TranslationHeaderCode == language && d.Tenant == 0).ToList();

            foreach (Translation translation in translationsList)
            {
                TextCode textcode = TextCodeRepository.GetTenantTextCodesWithTenantZero(translation.Tenant).Where(t => t.Code == translation.TextCodeCode).FirstOrDefault();
                translationsDictionary.Add(textcode.Code, translation);
            }

            foreach (Translation translation in defaultTranslationsList)
            {
                TextCode textcode = TextCodeRepository.GetTenantTextCodesWithTenantZero(translation.Tenant).Where(t => t.Code == translation.TextCodeCode).FirstOrDefault();
                defaultTranslationsDictionary.Add(textcode.Code, translation);
            }

            foreach (TextCode tc in textCodesList)
            {
                Translation translaion = null;
                if (translationsDictionary.Count != 0)
                {
                    if (translationsDictionary.ContainsKey(tc.Code))
                    {
                        translaion = translationsDictionary[tc.Code];
                    }
                }

                FieldsTranslations ft = new FieldsTranslations();

                if (translaion != null)
                {
                    ft.IsTranslated = true;
                    ft.TranslateDate = translaion.TranslateDate;
                    ft.TranslatedText = translaion.TranslatedText;
                    ft.TranslatedByUserId = translaion.TranslatedByUserId;
                    ft.TranslatedTextPlural = translaion.TranslatedTextPlural;
                }

                else
                {
                    Translation defaultTranslation = null;
                    if (defaultTranslationsDictionary.Count != 0)
                    {
                        if (defaultTranslationsDictionary.ContainsKey(tc.Code))
                        {
                            defaultTranslation = defaultTranslationsDictionary[tc.Code];
                        }
                    }

                    if (defaultTranslation != null)
                    {
                        ft.IsTranslated = true;
                        ft.TranslateDate = defaultTranslation.TranslateDate;
                        ft.TranslatedText = defaultTranslation.TranslatedText;
                        ft.TranslatedByUserId = defaultTranslation.TranslatedByUserId;
                        ft.TranslatedTextPlural = defaultTranslation.TranslatedTextPlural;
                    }
                    else
                    {


                        if (contact.DontShowLocalLabels)
                        {
                            ft.TranslatedText = tc.DefaultText;
                            ft.TranslatedTextPlural = tc.DefaultTextPlural;
                        }
                        else
                        {
                            ft.TranslatedText = !string.IsNullOrEmpty(tc.LocalDefaultText) ? tc.LocalDefaultText : tc.DefaultText;
                            ft.TranslatedTextPlural = tc.DefaultTextPlural;
                        }
                    }
                }
                ft.Tenant = tc.Tenant;
                ft.DefaultText = tc.DefaultText;
                ft.DefaultTextPlural = tc.DefaultTextPlural;
                ft.Code = tc.Code;
                ft.TextCodeId = tc.Id;
                ft.TextCodeCode = tc.Code;
                ft.TranslationTenent = tenant;
                ft.TranslationLanguageCode = language;
                ft.TypeCode = tc.TextCodeTypeCode;
                ft.ObjectTableID = tc.ObjectTableId;

                ObjectTablePM obTablePM = ObjectTableQuery.GetSingleObjectTableById(tc.ObjectTableId, tc.Tenant);
                if (obTablePM != null)
                {
                    ft.ObjectTableName = obTablePM.Name;
                }
                else
                {
                    ObjectTable obTable = obTableRepository.GetSingleObjectTable(tc.ObjectTableId, tc.Tenant, false);
                    if (obTable != null)
                    {
                        ft.ObjectTableName = obTable.Name;
                    }
                }

                fieldsTranslationList.Add(ft);
            }

            if(!string.IsNullOrEmpty(objectTableId))
            {
                fieldsTranslationList = fieldsTranslationList.Where(d => d.ObjectTableID == objectTableId).ToList();
            }

            if (!string.IsNullOrEmpty(textCodeType))
            {
                fieldsTranslationList = fieldsTranslationList.Where(d => d.TypeCode == textCodeType).ToList();
            }
            
            return fieldsTranslationList;
        }
        #endregion

        #region TranslationHeaders
        public IQueryable<TranslationHeader> GetTranslationHeaders()
        {
            TranslationHeaderRepository = new TranslationHeaderRepository(0);
            this.ChangeConnectionString(0);
            return TranslationHeaderRepository.GetTranslationHeadersByTenant(0);
        }

        public IQueryable<TranslationHeader> GetTranslationHeadersByTenant(int tenant)
        {
            TranslationHeaderRepository = new TranslationHeaderRepository(tenant);
            this.ChangeConnectionString(tenant);
            return TranslationHeaderRepository.GetTranslationHeadersByTenant(tenant);
        }

        public void InsertTranslationHeader(TranslationHeader translationHeader)
        {
            translationHeader.Code = translationHeader.Code;//IdCounter.GetNumber("TranslationHeader").ToString();
            TranslationHeaderRepository.Add(translationHeader);
        }

        public void UpdateTranslationHeader(TranslationHeader currentTranslationHeader)
        {
            
            TranslationHeaderRepository.Update(currentTranslationHeader);
        }

        public void DeleteTranslationHeader(TranslationHeader translationHeader)
        {
            
            TranslationHeaderRepository.Remove(translationHeader);
        }
        #endregion

        #region FieldDataTypeDataTypes
        public IQueryable<FieldDataType> GetFieldDataTypes(int tenant)
        {
            DataTypeRepository = new DataTypeRepository(tenant);
            this.ChangeConnectionString(tenant);
            return DataTypeRepository.GetDataTypes();
        }

        public void InsertFieldDataType(FieldDataType dataType)
        {
            DataTypeRepository.Add(dataType);
        }

        public void UpdateFieldDataType(FieldDataType currentDataType)
        {
            DataTypeRepository.Update(currentDataType);
        }

        public void DeleteFieldDataType(FieldDataType dataType)
        {
            DataTypeRepository.Remove(dataType);
        }
        #endregion

        #region Screens
        public List<ScreenPM> GetScreens()
        {
            ScreensRepository = new ScreensRepository(0);
            this.ChangeConnectionString(0);
            screensQuery = new ScreensQuery(ScreensRepository);
            return screensQuery.GetScreenPMsByTenant(0);
        }

        public List<ScreenPM> GetScreensByTenant(int tenant)
        {
            ScreensRepository = new ScreensRepository(tenant);
            this.ChangeConnectionString(tenant);
            screensQuery = new ScreensQuery(ScreensRepository);
            return screensQuery.GetScreenPMsByTenant(tenant);
        }

        //public void MapScreenPMScreen(ScreenPM screenPM, Screen screen,ScreenModification screenModification)
        //{
        //    screen.Code = screenPM.Code;

        //    screen.IsReadOnly = screenPM.IsReadOnly;
           
        //    screen.ObjectTableId = screenPM.ObjectTableId;
        //    screen.Name = screenPM.Name;
        //    if (screenModification != null)
        //    {
        //        screenModification.NumberOfColumns = screenPM.NumberOfColumns;
        //        screenModification.NumberOfRows = screenPM.NumberOfRows;
        //    }
        //    else
        //    {
        //        screen.NumberOfColumns = screenPM.NumberOfColumns;
        //        screen.NumberOfRows = screenPM.NumberOfRows;
        //    }
        //    screen.Tenant = screenPM.Tenant;
        //}

        public void InsertScreen(ScreenPM screen)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(screen.Tenant);
            }
            ScreenService service = new ScreenService(ObjectContext , screen.Tenant);
            service.Create(screen);

            //ScreensRepository = new ScreensRepository(ObjectContext);
            //this.ChangeConnectionString(screen.Tenant);
            //screen.Id = IdCounter.GetNumber("Screen", screen.Tenant).ToString();
            //Screen newScreen = new Screen();
            //newScreen.Id = screen.Id;
            //MapScreenPMScreen(screen, newScreen,null);
            //ScreensRepository.Add(newScreen);
        }

        public void UpdateScreen(ScreenPM currentScreen)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(currentScreen.Tenant);
            }
            ScreenService service = new ScreenService(ObjectContext, currentScreen.Tenant);
            service.Update(currentScreen);

            //ScreensRepository = new ScreensRepository(ObjectContext);
            //this.ChangeConnectionString(currentScreen.Tenant);
            //Screen entity = ScreensRepository.GetSingleScreen(currentScreen.Id);
            //ScreenModification mod = ScreensRepository.GetScreenModificationByScreen(currentScreen.Id, currentScreen.UserTenant);
            //if ((entity.NumberOfColumns != currentScreen.NumberOfColumns) || (entity.NumberOfRows != currentScreen.NumberOfRows))
            //{
            //    if (mod == null)
            //    {
            //        mod = new ScreenModification() { Tenant = currentScreen.UserTenant, ScreenId = currentScreen.Id, Id = IdCounter.GetNumber("ScreenModification", currentScreen.Tenant), };
            //        this.ObjectContext.ScreenModifications.Add(mod);
            //    }
            //}
            //MapScreenPMScreen(currentScreen, entity, mod);
            //ScreensRepository.Update(entity);
        }

        public void DeleteScreen(ScreenPM screen)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(screen.Tenant);
            }
            ScreensRepository = new ScreensRepository(ObjectContext);
            this.ChangeConnectionString(screen.Tenant);
            Screen entity = ScreensRepository.GetSingleScreen(screen.Id);
            ScreensRepository.Remove(entity);
        }
        #endregion

        #region ScreenFields
        public List<ScreenFieldPM> GetScreenFields()
        {
            ScreenFieldsRepository = new ScreenFieldsRepository(0);
            this.ChangeConnectionString(0);
            screenFieldsQuery = new ScreenFieldsQuery(ScreenFieldsRepository);
            return screenFieldsQuery.GetScreenFieldPMsByTenant(0);
        }

        public List<ScreenFieldPM> GetScreenFieldsByTenant(int tenant)
        {
            ScreenFieldsRepository = new ScreenFieldsRepository(tenant);
            this.ChangeConnectionString(tenant);
            screenFieldsQuery = new ScreenFieldsQuery(ScreenFieldsRepository);
            return screenFieldsQuery.GetScreenFieldPMsByTenant(tenant);
        }

        public List<ScreenFieldPM> GetScreenFieldsForScreen(string screenCode, int tenant)
        {
            ScreenFieldsRepository = new ScreenFieldsRepository(tenant);
            this.ChangeConnectionString(tenant);
            screenFieldsQuery = new ScreenFieldsQuery(ScreenFieldsRepository);
            return screenFieldsQuery.GetScreenFieldPMsByTenant(tenant).Where(s => s.Tenant == tenant && s.ScreenCode == screenCode).ToList();
        }

        //public void MapScreenFieldPMScreenField(ScreenFieldPM screenFieldPM, ScreenField screenField)
        //{
        //    screenField.Column = screenFieldPM.Column;
        //    screenField.ObjectFieldId = screenFieldPM.ObjectFieldId;           
        //    screenField.Row = screenFieldPM.Row;
        //    screenField.ScreenId = screenFieldPM.ScreenId;
        //    screenField.Tenant = screenFieldPM.Tenant;
        //}

        public void InsertScreenField(ScreenFieldPM screenField)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(screenField.Tenant);
            }
            ScreenFieldService service = new ScreenFieldService(ObjectContext, screenField.Tenant);
            service.Create(screenField);

            //ScreenFieldsRepository = new ScreenFieldsRepository(ObjectContext);
            //this.ChangeConnectionString(screenField.Tenant);
            //screenField.Id = IdCounter.GetNumber("ScreenField", screenField.Tenant).ToString();
            //ScreenField newScreenField = new ScreenField();
            //newScreenField.Id = screenField.Id;
            //MapScreenFieldPMScreenField(screenField, newScreenField);
            //ScreenFieldsRepository.Add(newScreenField);
        }

        public void UpdateScreenField(ScreenFieldPM currentScreenField)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(currentScreenField.Tenant);
            }
            ScreenFieldService service = new ScreenFieldService(ObjectContext, currentScreenField.Tenant);
            service.Update(currentScreenField);

            //ScreenFieldsRepository = new ScreenFieldsRepository(ObjectContext);
            //this.ChangeConnectionString(currentScreenField.Tenant);
            //ScreenField entity = ScreenFieldsRepository.GetSingleScreenField(currentScreenField.Id);
            //MapScreenFieldPMScreenField(currentScreenField, entity);
            //ScreenFieldsRepository.Update(entity);
        }

        public void DeleteScreenField(ScreenFieldPM screenField)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(screenField.Tenant);
            }

            ScreenFieldsRepository = new ScreenFieldsRepository(ObjectContext);
            this.ChangeConnectionString(screenField.Tenant);
            ScreenField entity = ScreenFieldsRepository.GetSingleScreenField(screenField.Id);
            ScreenFieldsRepository.Remove(entity);
            ScreenFieldsRepository.SubmitChanges();
        }
        #endregion

        #region TextCodeTypes
        public IQueryable<TextCodeType> GetTextCodeTypes(int tenant)
        {
            TextCodeTypesRepository = new TextCodeTypesRepository(tenant);
            this.ChangeConnectionString(tenant);
            return TextCodeTypesRepository.GetTextCodeTypes();
        }

        public void InsertTextCodeType(TextCodeType textCodeType)
        {
            TextCodeTypesRepository.Add(textCodeType);
        }

        public void UpdateTextCodeType(TextCodeType currentTextCodeType)
        {
            TextCodeTypesRepository.Update(currentTextCodeType);
        }

        public void DeleteTextCodeType(TextCodeType textCodeType)
        {
            TextCodeTypesRepository.Remove(textCodeType);
        }
        #endregion

        #region Queries
        public IQueryable<QueryPM> GetQueries()
        {
            QueriesRepository = new QueryRepository(0);
            this.ChangeConnectionString(0);
            queryQuery = new QueryQuery(QueriesRepository);
            return queryQuery.GetQueryPMsByTenant(0);
        }

        public IQueryable<QueryPM> GetQueriesByTenant(int tenant)
        {
            QueriesRepository = new QueryRepository(tenant);
            this.ChangeConnectionString(tenant);
            queryQuery = new QueryQuery(QueriesRepository);
            return queryQuery.GetQueryPMsByTenant(tenant);
        }

        public QueryPM GetQueryByNameTenant(int tenant, string name)
        {
            QueriesRepository = new QueryRepository(tenant);
            this.ChangeConnectionString(tenant);
            queryQuery = new QueryQuery(QueriesRepository);
            return queryQuery.GetQueryByNameTenant(tenant, name);
        }

        public List<QueryPM> GetQueriesByTenantAndUser(int tenant, string userId)
        {
            QueriesRepository = new QueryRepository(tenant);
            this.ChangeConnectionString(tenant);
            queryQuery = new QueryQuery(QueriesRepository);
            return queryQuery.GetQueries(tenant, userId); 
        }

        //====================================================================
        public QueryPM GetSingleQuery(string id, int tenant)
        {
            QueriesRepository = new QueryRepository(tenant);
            //this.ChangeConnectionString(tenant);
            queryQuery = new QueryQuery(QueriesRepository);
            return queryQuery.GetSingleQueryPM(id);
        }

        public QueryPM GetQueryById(string id, int tenant)
        {
            QueriesRepository = new QueryRepository(tenant);
            queryQuery = new QueryQuery(QueriesRepository);
            QueryPM query = queryQuery.GetSingleQueryPM(id);
            return query;
        }

        public QueryList GetSingleQueryList(string id, int tenant)
        {
            QueriesRepository = new QueryRepository(tenant);
            Query query = QueriesRepository.GetSingleQuery(id);
            QueryList queryList = new QueryList()
            {
                Id = query.Id,
                Code = query.Code,
                DisplayCount = query.DisplayCount,
                IndexOrder = query.IndexOrder,
                QuerySection = query.QuerySection,
                ObjectTableId = query.ObjectTableId,
                ObjectTableName = query.ObjectTable.Name,
                OriginalQueryId = query.OriginalQueryId,
                SystemLevel = query.SystemLevel,
                Tenant = query.Tenant,
                TenantLevel = query.TenantLevel,
                UserId = query.UserId,
                IsHiddenFromView = query.IsHiddenFromView,
                IsNewFromTenantZeroOnly = query.IsNewFromTenantZeroOnly,
            };
            return queryList;
        }

        public IQueryable<QueryList> GetQueryLists(int tenant)
        {
            QueriesRepository = new QueryRepository(tenant);
            IQueryable<Query> queries = QueriesRepository.GetQueriesByTenant(tenant);
            var query2 = from a in queries
                         select new QueryList()
                         {
                             Id = a.Id,
                             Code = a.Code,
                             DisplayCount = a.DisplayCount,
                             IndexOrder = a.IndexOrder,
                             QuerySection = a.QuerySection,
                             ObjectTableId = a.ObjectTableId,
                             ObjectTableName = a.ObjectTable.Name,
                             OriginalQueryId = a.OriginalQueryId,
                             SystemLevel = a.SystemLevel,
                             Tenant = a.Tenant,
                             TenantLevel = a.TenantLevel,
                             UserId = a.UserId,
                             IsHiddenFromView = a.IsHiddenFromView,
                             IsNewFromTenantZeroOnly = a.IsNewFromTenantZeroOnly,
                         };
            return query2;
        }

        public void InsertQuery(QueryPM entity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(entity.Tenant);
            }

            QueryService service = new QueryService(ObjectContext, entity.Tenant);
            service.Create(entity);
        }

        public void UpdateQuery(QueryPM entity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(entity.Tenant);
            }

            QueryService service = new QueryService(ObjectContext, entity.Tenant);
            service.Update(entity);
        }

        public void DeleteQuery(QueryPM entity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            QueriesRepository = new QueryRepository(ObjectContext);
            QueryColumnsRepository = new QueryColumnRepository(ObjectContext);

            this.ChangeConnectionString(entity.Tenant);
            Query query = QueriesRepository.GetSingleQuery(entity.Id);
           
            List<Query> coppiedQueries = QueriesRepository.GetQueriesByOrigionalQueryTenant(query.Id, query.Tenant).ToList();

            foreach (var q in coppiedQueries)
                q.OriginalQueryId = null;

            QueriesRepository.Remove(query);
            QueriesRepository.SubmitChanges();
        }
        #endregion      

        #region AdvancedQueryFilters
        //[Invoke]
        //public decimal RemoveAdvanceQueryFilter(ObjectField objectField,int tenant,string queryId)
        //{
        //    AdvancedQueryFiltersRepository = new AdvancedQueryFilterRepository(tenant);
        //    this.AdvancedQueryFiltersRepository.RemoveFilter(objectField, tenant,queryId);
        //    return 1;
        //}

        public IQueryable<AdvancedQueryFilterPM> GetAdvancedQueryFilter()
        {
            advancedQueryFilterQuery = new AdvancedQueryFilterQuery(0);
            this.ChangeConnectionString(0);
            return advancedQueryFilterQuery.GetAdvancedQueryFilterPMsByTenant(0);
        }

        public IQueryable<AdvancedQueryFilterPM> GetPredefinedQueryFilters(int tenant)
        {
            advancedQueryFilterQuery = new AdvancedQueryFilterQuery(tenant);
            this.ChangeConnectionString(0);
            return advancedQueryFilterQuery.GetPredefinedQueryFilters(tenant);
        }

        public IQueryable<AdvancedQueryFilterPM> GetAdvancedQueryFiltersByTenant(int tenant,string userid)
        {
            advancedQueryFilterQuery = new AdvancedQueryFilterQuery(tenant); ;
            this.ChangeConnectionString(tenant);
            return advancedQueryFilterQuery.GetAdvancedQueryFilterPMsByTenantAndUser(tenant, userid);
        }

        public IQueryable<AdvancedQueryFilterPM> GetAdvancedQueryFiltersByTenantAndNoUser(int tenant)
        {
            advancedQueryFilterQuery = new AdvancedQueryFilterQuery(tenant);
            this.ChangeConnectionString(tenant);
            return advancedQueryFilterQuery.GetAdvancedQueryFiltersByTenantAndNoUser(tenant);
        }

        public IQueryable<AdvancedQueryFilterPM> GetAdvancedQueryFiltersByQueryId(int tenant, string queryId)
        {
            advancedQueryFilterQuery = new AdvancedQueryFilterQuery(tenant);
            this.ChangeConnectionString(tenant);
            return advancedQueryFilterQuery.GetAdvancedQueryFiltersByQueryId(tenant, queryId);
        }

        //public void MapAdvancedQueryFilterPMAdvancedQueryFilter(AdvancedQueryFilterPM advancedQueryFilterPM, AdvancedQueryFilter advancedQueryFilter)
        //{    
        //    advancedQueryFilter.IndexOrder = advancedQueryFilterPM.IndexOrder;            
        //    advancedQueryFilter.IsPredefined = advancedQueryFilterPM.IsPredefined;
        //    advancedQueryFilter.ObjectFieldId = advancedQueryFilterPM.ObjectFieldId;           
        //    advancedQueryFilter.Operator = advancedQueryFilterPM.Operator;
        //    advancedQueryFilter.PredefinedValue = advancedQueryFilterPM.PredefinedValue;
        //    advancedQueryFilter.PredefinedValue2 = advancedQueryFilterPM.PredefinedValue2;        
        //    advancedQueryFilter.QueryId = advancedQueryFilterPM.QueryId;
        //    advancedQueryFilter.Tenant = advancedQueryFilterPM.Tenant;
        //    advancedQueryFilter.UserId = advancedQueryFilterPM.UserId;           
        //}

        public void InsertAdvancedQueryFilter(AdvancedQueryFilterPM entity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            AdvancedQueryFilterService service = new AdvancedQueryFilterService(ObjectContext, entity.Tenant);
            service.Create(entity);

            //AdvancedQueryFiltersRepository = new AdvancedQueryFilterRepository(ObjectContext);
            //this.ChangeConnectionString(entity.Tenant);
            //entity.Id = IdCounter.GetNumber("AdvancedQueryFilter", entity.Tenant).ToString();
            //AdvancedQueryFilter newAdvencedQueryFilter = new AdvancedQueryFilter();
            //newAdvencedQueryFilter.Id = entity.Id;
            //MapAdvancedQueryFilterPMAdvancedQueryFilter(entity, newAdvencedQueryFilter);
            //AdvancedQueryFiltersRepository.Add(newAdvencedQueryFilter);
        }

        public void UpdateAdvancedQueryFilter(AdvancedQueryFilterPM entity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            AdvancedQueryFilterService service = new AdvancedQueryFilterService(ObjectContext, entity.Tenant);
            service.Update(entity);

            //AdvancedQueryFiltersRepository = new AdvancedQueryFilterRepository(ObjectContext);
            //this.ChangeConnectionString(entity.Tenant);
            //AdvancedQueryFilter advancedQueryFilter = AdvancedQueryFiltersRepository.GetSingleAdvancedQueryfilter(entity.Id);
            //MapAdvancedQueryFilterPMAdvancedQueryFilter(entity, advancedQueryFilter);
            //AdvancedQueryFiltersRepository.Update(advancedQueryFilter);
        }

        public void DeleteAdvancedQueryFilter(AdvancedQueryFilterPM entity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            AdvancedQueryFiltersRepository = new AdvancedQueryFilterRepository(ObjectContext);
            this.ChangeConnectionString(entity.Tenant);
            AdvancedQueryFilter advancedQueryFilter = AdvancedQueryFiltersRepository.GetSingleAdvancedQueryfilter(entity.Id);
            if (advancedQueryFilter != null)
            {
                AdvancedQueryFiltersRepository.Remove(advancedQueryFilter);
            }
            AdvancedQueryFiltersRepository.SubmitChanges();

        }
        #endregion

        #region QueryColumns
        public IQueryable<QueryColumnPM> GetQueryColumns()
        {
            QueryColumnsRepository = new QueryColumnRepository(0);
            this.ChangeConnectionString(0);
            queryColumnQuery = new QueryColumnQuery(QueryColumnsRepository);
            return queryColumnQuery.GetQueryColumnPMsByTenant(0);
        }

        public IQueryable<QueryColumnPM> GetQueryColumnsByTenant(int tenant)
        {
            QueryColumnsRepository = new QueryColumnRepository(tenant);
            this.ChangeConnectionString(tenant);
            queryColumnQuery = new QueryColumnQuery(QueryColumnsRepository);
            return queryColumnQuery.GetQueryColumnPMsByTenant(tenant);
        }

        public IQueryable<QueryColumnPM> GetQueryColumnsByTenantAndNoUser(int tenant)
        {
            QueryColumnsRepository = new QueryColumnRepository(tenant);
            this.ChangeConnectionString(tenant);
            queryColumnQuery = new QueryColumnQuery(QueryColumnsRepository);
            return queryColumnQuery.GetQueryColumnsByTenantAndNoUser(tenant);
        }

        public IQueryable<QueryColumnPM> GetQueryColumnsByTenantAndUser(int tenant, string userId)
        {
            QueryColumnsRepository = new QueryColumnRepository(tenant);
            this.ChangeConnectionString(tenant);
            queryColumnQuery = new QueryColumnQuery(QueryColumnsRepository);
            return queryColumnQuery.GetQueryColumnsByTenantAndUser(tenant, userId);
        }

        public List<QueryColumnPM> GetQueryColumnsWithTenantZero(int tenant, string userId)
        {
            QueryColumnsRepository = new QueryColumnRepository(tenant);
            this.ChangeConnectionString(tenant);
            queryColumnQuery = new QueryColumnQuery(QueryColumnsRepository);
            return queryColumnQuery.GetQueryColumns(tenant, userId);
        }

        public IQueryable<QueryColumnPM> GetQueryColumnsByQueryIdAndUser(int tenant, string userId,string queryId)
        {
            QueryColumnsRepository = new QueryColumnRepository(tenant);
            this.ChangeConnectionString(tenant);
            queryColumnQuery = new QueryColumnQuery(QueryColumnsRepository);
            return queryColumnQuery.GetQueryColumnsByQueryIdAndUser(tenant, userId, queryId);
        }

        public IQueryable<QueryColumnPM> GetQueryColumnsByQueryTenant(int tenant, string queryName)
        {
            QueryColumnsRepository = new QueryColumnRepository(tenant);
            this.ChangeConnectionString(tenant);
            queryColumnQuery = new QueryColumnQuery(QueryColumnsRepository);
            return queryColumnQuery.GetQueryColumnsByQueryTenant(tenant, queryName); 
        }

        //public void MapQueryColumnPMQueryColumn(QueryColumnPM queryColumnPM, QueryColumn queryColumn)
        //{
        //    queryColumn.ColumnWidth = queryColumnPM.ColumnWidth;
        //    queryColumn.IndexOrder = queryColumnPM.IndexOrder;
        //    queryColumn.ObjectFieldId = queryColumnPM.ObjectFieldId;            
        //    queryColumn.QueryId = queryColumnPM.QueryId;
        //    queryColumn.Tenant = queryColumnPM.Tenant;
        //    queryColumn.UserId = queryColumnPM.UserId;
           
        //}

        public void InsertQueryColumn(QueryColumnPM entity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            QueryColumnService service = new QueryColumnService(ObjectContext, entity.Tenant);
            service.Create(entity);

            //QueryColumnsRepository = new QueryColumnRepository(ObjectContext);
            //this.ChangeConnectionString(entity.Tenant);
            //entity.Id = IdCounter.GetNumber("QueryColumn", entity.Tenant).ToString();
            //QueryColumn newQueryColumn = new QueryColumn();
            //newQueryColumn.Id = entity.Id;
            //MapQueryColumnPMQueryColumn(entity, newQueryColumn);
            //QueryColumnsRepository.Add(newQueryColumn);
        }

        public void UpdateQueryColumn(QueryColumnPM entity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            if (entity.Tenant == 0)
            {

            }
            QueryColumnService service = new QueryColumnService(ObjectContext, entity.Tenant);
            service.Update(entity);

            //QueryColumnsRepository = new QueryColumnRepository(ObjectContext);
            //this.ChangeConnectionString(entity.Tenant);
            //QueryColumn queryColumn = QueryColumnsRepository.GetSingleQueryColumn(entity.Id,entity.Tenant);
            //if (queryColumn != null)
            //{
            //    MapQueryColumnPMQueryColumn(entity, queryColumn);
            //    QueryColumnsRepository.Update(queryColumn);
            //}

       }

        public void DeleteQueryColumn(QueryColumnPM entity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            QueryColumnsRepository = new QueryColumnRepository(ObjectContext);
            this.ChangeConnectionString(entity.Tenant);
            QueryColumn queryColumn = QueryColumnsRepository.GetSingleQueryColumn(entity.Id, entity.Tenant);
            QueryColumnsRepository.Remove(queryColumn);
            QueryColumnsRepository.SubmitChanges(); ;
        }
        #endregion

        #region CustomTables
        public IQueryable<CustomTable> GetCustomTablesByTenant(int tenant)
        {
            CustomTablesRepository = new CustomTableRepository(tenant);
            this.ChangeConnectionString(tenant);
            return CustomTablesRepository.GetCustomTablesByTenant(tenant);
        }

        public IQueryable<CustomTable> GetCustomTablesByTenantAndObjectTable(string objectTableId, int tenant)
        {
            CustomTablesRepository = new CustomTableRepository(tenant);
            this.ChangeConnectionString(tenant);
            return CustomTablesRepository.GetCustomTablesByTenantAndObjectTable(objectTableId, tenant);
        }

        //public void InsertCustomTable(CustomTablePM entity)
        //{
        //    if (ObjectContext == null)
        //    {
        //        ObjectContext = WebFreightContext.GetContext(entity.Tenant);
        //    }
        //    //CustomTableService service = new CustomTableService(ObjectContext, entity.Tenant);
        //    //service.Create(entity);

        //    CustomTablesRepository = new CustomTableRepository(ObjectContext);
        //    this.ChangeConnectionString(entity.Tenant);
        //    entity.Id = IdCounter.GetNumber("CustomTable", entity.Tenant).ToString();

        //    //if (entity.CustomField == null)
        //    //{
        //    //    CustomField newCustomfield = new CustomField();
        //    //    newCustomfield.Id = entity.Id;
        //    //    newCustomfield.ObjectType = "CtmTable";
        //    //    entity.CustomField = newCustomfield;

        //    //    CustomFieldsRepository.Add(newCustomfield);

        //    //}

        // //   CustomTablesRepository.Add(customtable);
        //}

        //public void UpdateCustomTable(CustomTablePM currentEntity)
        //{
        //    if (ObjectContext == null)
        //    {
        //        ObjectContext = WebFreightContext.GetContext(currentEntity.Tenant);
        //    }
        //    //CustomTableService service = new CustomTableService(ObjectContext, currentEntity.Tenant);
        //    //service.Update(currentEntity);

        //    CustomTablesRepository = new CustomTableRepository(ObjectContext);
        //    this.ChangeConnectionString(currentEntity.Tenant);
        //   // CustomTablesRepository.Update(currentEntity);
        //}

        public void DeleteCustomTable(CustomTable entity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            CustomTablesRepository = new CustomTableRepository(ObjectContext);
            this.ChangeConnectionString(entity.Tenant);
            CustomTablesRepository.Remove(entity);
        }
        #endregion 

        #region MenuTypes
        public IQueryable<MenuType> GetMenuTypes(int tenant)
        {
            MenuTypesRepository = new MenuTypeRepository(tenant);
            this.ChangeConnectionString(tenant);            
            return MenuTypesRepository.GetMenuTypes();
        }

        public void InsertMenuType(MenuType entity)
        {
            MenuTypesRepository.Add(entity);
        }

        public void UpdateMenuType(MenuType currentEntity)
        {
            MenuTypesRepository.Update(currentEntity);
        }

        public void DeleteMenuType(MenuType entity)
        {
            MenuTypesRepository.Remove(entity);
        }
        #endregion

        #region CategoryTypes
        public IQueryable<CategoryType> GetCategoryTypes()
        {            
            return CategoryTypesRepository.GetCategoryTypes();
        }

        public void InsertCategoryType(CategoryType entity)
        {
            CategoryTypesRepository.Add(entity);
        }

        public void UpdateCategoryType(CategoryType currentEntity)
        {
            CategoryTypesRepository.Update(currentEntity);
        }

        public void DeleteCategoryType(CategoryType entity)
        {
            CategoryTypesRepository.Remove(entity);
        }
        #endregion

        #region MenusTables
        public IQueryable<MenusTablePM> GetMenusTableByTenant(int tenant)
        {
            MenusTablesRepository = new MenusTableRepository(tenant);
            this.ChangeConnectionString(tenant);
            menusTableQuery= new MenusTableQuery(MenusTablesRepository);
            return menusTableQuery.GetMenusTablePMsByTenant(tenant);
        }

        public IQueryable<MenusTablePM> GetMainMenuList(int tenant)
        {
            MenusTablesRepository = new MenusTableRepository(tenant);
            this.ChangeConnectionString(tenant);
            menusTableQuery = new MenusTableQuery(MenusTablesRepository);
            return menusTableQuery.GetMainMenuList(tenant);
        }

        public IQueryable<MenusTablePM> GetMaintenanceMenusList(int tenant)
        {
            MenusTablesRepository = new MenusTableRepository(tenant);
            this.ChangeConnectionString(tenant);
            menusTableQuery = new MenusTableQuery(MenusTablesRepository);
            return menusTableQuery.GetMaintenanceMenusList(tenant);
        }

        //public void MapMenusTablePMMenusTable(MenusTablePM menusTablePM, MenusTable menusTable)
        //{
        //    menusTable.CategoryTypeCode = menusTablePM.CategoryTypeCode;
        //    menusTable.Icon = menusTablePM.Icon;
        //    menusTable.IndexOfOrder = menusTablePM.IndexOfOrder;
        //    menusTable.MenuTypeCode = menusTablePM.MenuTypeCode;
        //    menusTable.ObjectTableId = menusTablePM.ObjectTableId;
        //    menusTable.Tenant = menusTablePM.Tenant;
        //    menusTable.TextCode = menusTablePM.TextCode;
        //    menusTable.UserControlName = menusTablePM.UserControlName;
        //    menusTable.FeatureId = menusTablePM.FeatureId;
        //    menusTable.Code = menusTablePM.Code;                      
        //}

        public void InsertMenusTable(MenusTablePM entity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            MenusTableService service = new MenusTableService(ObjectContext , entity.Tenant);
            service.Create(entity);

            //MenusTablesRepository = new MenusTableRepository(ObjectContext);
            //this.ChangeConnectionString(entity.Tenant);
            //entity.Id = IdCounter.GetNumber("MenusTable", entity.Tenant).ToString();

            //MenusTable newMenusTable = new MenusTable();

            //newMenusTable.Id = entity.Id;
            //MapMenusTablePMMenusTable(entity, newMenusTable);
            
            //if (entity.IndexOfOrder == 0 || entity.IndexOfOrder == null)
            //{
            //    int index = MenusTablesRepository.GetCount(entity.Tenant, entity.MenuTypeCode, entity.CategoryTypeCode);
            //    entity.IndexOfOrder = index;
            //}
            //MenusTablesRepository.Add(newMenusTable);
        }

        public void UpdateMenusTable(MenusTablePM currentEntity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(currentEntity.Tenant);
            }
            MenusTableService service = new MenusTableService(ObjectContext, currentEntity.Tenant);
            service.Update(currentEntity);

            //MenusTablesRepository = new MenusTableRepository(ObjectContext);
            //this.ChangeConnectionString(currentEntity.Tenant);
            //MenusTable menusTable = MenusTablesRepository.GetSingleMenusTable(currentEntity.Id);
            //MapMenusTablePMMenusTable(currentEntity, menusTable);
            //MenusTablesRepository.Update(menusTable);
        }

        public void DeleteMenusTable(MenusTablePM entity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            MenusTablesRepository = new MenusTableRepository(ObjectContext);
            this.ChangeConnectionString(entity.Tenant);
            MenusTable menusTable = MenusTablesRepository.GetSingleMenusTable(entity.Id);
            MenusTablesRepository.Remove(menusTable);
        }
        #endregion 

        #region ObjectTableTabs
        public IQueryable<ObjectTableTabPM> GetObjectTableTabsByTenant(int tenant)
        {
            ObjectTableTabsRepository = new ObjectTableTabRepository(tenant);
            this.ChangeConnectionString(tenant);
            objectTableTabQuery = new ObjectTableTabQuery(ObjectTableTabsRepository);
            return objectTableTabQuery.GetObjectTableTabPMsByTenant(tenant);
        }

        public IQueryable<ObjectTableTabPM> GetObjectTableTabsByTenantAndObjectTable(string objectTableId, int tenant)
        {
            ObjectTableTabsRepository = new ObjectTableTabRepository(tenant);
            this.ChangeConnectionString(tenant);
            objectTableTabQuery = new ObjectTableTabQuery(ObjectTableTabsRepository);
            return objectTableTabQuery.GetObjectTableTabsByTenantAndObjectTable(objectTableId, tenant);
        }

        //public void MapObjectTableTabPMObjectTableTab(ObjectTableTabPM objectTableTabPM, ObjectTableTab objectTableTab)
        //{
        //    objectTableTab.ControlPath = objectTableTabPM.ControlPath;
        //    objectTableTab.IndexOrder = objectTableTabPM.IndexOrder;
        //    objectTableTab.ObjectTableId = objectTableTabPM.ObjectTableId;
        //    objectTableTab.TabNameTextCodeId = objectTableTabPM.TabNameTextCodeId;
        //    objectTableTab.Tenant = objectTableTabPM.Tenant;
        //    objectTableTab.Code = objectTableTabPM.Code;
        //    objectTableTab.FeatureId = objectTableTabPM.FeatureId;
        //}

        public void InsertObjectTableTab(ObjectTableTabPM entity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            ObjectTableTabService service = new ObjectTableTabService(ObjectContext, entity.Tenant);
            service.Create(entity);

            //ObjectTableTabsRepository = new ObjectTableTabRepository(ObjectContext);
            //this.ChangeConnectionString(entity.Tenant);
            //entity.Id = IdCounter.GetNumber("ObjectTableTab", entity.Tenant).ToString();
            //ObjectTableTab newObjectTableTab = new ObjectTableTab();
            //newObjectTableTab.Id = entity.Id;
            //MapObjectTableTabPMObjectTableTab(entity, newObjectTableTab);
            //ObjectTableTabsRepository.Add(newObjectTableTab);
        }

        public void UpdateObjectTableTab(ObjectTableTabPM currentEntity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(currentEntity.Tenant);
            }
            ObjectTableTabService service = new ObjectTableTabService(ObjectContext, currentEntity.Tenant);
            service.Update(currentEntity);

            //ObjectTableTabsRepository = new ObjectTableTabRepository(ObjectContext);
            //this.ChangeConnectionString(currentEntity.Tenant);
            //ObjectTableTab objectTableTab = ObjectTableTabsRepository.GetSingleObjectTableTab(currentEntity.Id);
            //MapObjectTableTabPMObjectTableTab(currentEntity, objectTableTab);
            //ObjectTableTabsRepository.Update(objectTableTab);
        }

        public void DeleteObjectTableTab(ObjectTableTabPM entity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            ObjectTableTabsRepository = new ObjectTableTabRepository(ObjectContext);
            this.ChangeConnectionString(entity.Tenant);
            ObjectTableTab objectTableTab = ObjectTableTabsRepository.GetSingleObjectTableTab(entity.Id);
            ObjectTableTabsRepository.Remove(objectTableTab);
        }
        #endregion 

        #region MenuButtonGroups
        public List<MenuButtonGroupPM> GetMenuButtonGroupsByTenant(int tenant)
        {
            MenuButtonGroupRepository = new MenuButtonGroupRepository(tenant);
            this.ChangeConnectionString(tenant);
            menuButtonGroupQuery = new MenuButtonGroupQuery(MenuButtonGroupRepository);

            return menuButtonGroupQuery.GetMenuButtonGroupPMsByTenant(tenant);
        }

        public List<MenuButtonGroupPM> GetMenuButtonGroupsByObjectTable(string objectTableId, int tenant)
        {
            MenuButtonGroupRepository = new MenuButtonGroupRepository(tenant);
            this.ChangeConnectionString(tenant);
            menuButtonGroupQuery = new MenuButtonGroupQuery(MenuButtonGroupRepository);
            return menuButtonGroupQuery.GetMenuButtonGroupsByObjectTable(objectTableId, tenant);
        }

        public MenuButtonGroupPM GetSingleMenuButtonGroupByObjectTable(string objectTableId, int tenant)
        {

           

            MenuButtonGroupRepository = new MenuButtonGroupRepository(tenant);
            this.ChangeConnectionString(tenant);
            menuButtonGroupQuery = new MenuButtonGroupQuery(MenuButtonGroupRepository);
            return menuButtonGroupQuery.GetSingleMenuButtonGroupPMByObjectTable(objectTableId, tenant);
        }

        //public void MapMenuButtonGroupPMMenuButtonGroup(MenuButtonGroupPM menuButtonGroupPM, MenuButtonGroup menuButtonGroup)
        //{
        //    menuButtonGroup.MenuButtonGroupType = menuButtonGroupPM.MenuButtonGroupType;
        //    menuButtonGroup.Name = menuButtonGroupPM.Name;
        //    menuButtonGroup.ObjectTableId = menuButtonGroupPM.ObjectTableId;
        //    menuButtonGroup.Tenant = menuButtonGroupPM.Tenant;
        //}

        public void InsertMenuButtonGroup(MenuButtonGroupPM entity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            MenuButtonGroupService service = new MenuButtonGroupService(ObjectContext , entity.Tenant);
            service.Create(entity);

            //MenuButtonGroupRepository = new MenuButtonGroupRepository(ObjectContext);
            //MenuButtonRepository = new MenuButtonRepository(ObjectContext);
            //this.ChangeConnectionString(entity.Tenant);
            //entity.Id = IdCounter.GetNumber("MenuButtonGroup", entity.Tenant).ToString();
            //MenuButtonGroup newMenuButtonGroup = new MenuButtonGroup();
            //newMenuButtonGroup.Id = entity.Id;
            //if (entity.MenuButtons!=null)
            //{
            //    foreach (MenuButtonPM button in entity.MenuButtons)
            //    {
            //        MenuButton newButton = new MenuButton();
            //        newButton.Id = IdCounter.GetNumber("MenuButton", entity.Tenant).ToString();
            //        button.Id = newButton.Id;
            //        newButton.MenuButtonGroupId = entity.Id;
            //        MapMenuButtonPMMenuButton(button, newButton);
            //        MenuButtonRepository.Add(newButton);
            //    }
            //}
            //MapMenuButtonGroupPMMenuButtonGroup(entity, newMenuButtonGroup);
            //MenuButtonGroupRepository.Add(newMenuButtonGroup);
        }

        //public void MapMenuButtonPMMenuButton(MenuButtonPM menuButtonPM, MenuButton menuButton)
        //{
        //    menuButton.EventCode = menuButtonPM.EventCode;
        //    menuButton.Index = menuButtonPM.Index;
        //    menuButton.IsActive = menuButtonPM.IsActive;
        //    menuButton.LabelTextCodeId = menuButtonPM.LabelTextCodeId;
        //    menuButton.MenuButtonGroupId = menuButtonPM.MenuButtonGroupId;
        //    menuButton.ParentMenuButtonId = menuButtonPM.ParentMenuButtonId;
        //    menuButton.Tenant = menuButtonPM.Tenant;
        //    menuButton.MenuButtonType = menuButtonPM.MenuButtonType;
        //    menuButton.DropDownControl = menuButtonPM.DropDownControl;
        //    menuButton.Style = menuButtonPM.Style;
        //}

        public void UpdateMenuButtonGroup(MenuButtonGroupPM currentEntity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(currentEntity.Tenant);
            }
            
            MenuButtonGroupRepository = new MenuButtonGroupRepository(ObjectContext);
            MenuButtonRepository = new MenuButtonRepository(ObjectContext);

            MenuButtonGroup menuButtonGroup = MenuButtonGroupRepository.GetSingleMenuButtonGroup(currentEntity.Id);
            List<MenuButtonPM> menuButtonChangeSet = ChangeSet.GetAssociatedChanges(currentEntity, d => d.MenuButtons).Cast<MenuButtonPM>().ToList();

            foreach (MenuButtonPM r in menuButtonChangeSet)
            {
                ChangeOperation op = ChangeSet.GetChangeOperation(r);
                switch (op)
                {
                    case ChangeOperation.Insert:
                        {
                            r.ChangeSetOp = ChangeSetOperation.Insert;
                            //r.Id = IdCounter.GetNumber("MenuButton", currentEntity.Tenant).ToString();
                            //MenuButton newMenuButton = new MenuButton();
                            //newMenuButton.Id = r.Id;
                            //MapMenuButtonPMMenuButton(r, newMenuButton);
                            //MenuButtonRepository.Add(newMenuButton);
                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            r.ChangeSetOp = ChangeSetOperation.Update;
                            //MenuButton menuButton = MenuButtonRepository.GetSingleMenuButton(r.Id);
                            //MapMenuButtonPMMenuButton(r, menuButton);
                            //MenuButtonRepository.Update(menuButton);
                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            r.ChangeSetOp = ChangeSetOperation.Delete;
                            //MenuButton menuButton = MenuButtonRepository.GetSingleMenuButton(r.Id);
                            //MenuButtonRepository.Remove(menuButton);

                            break;
                        }
                    case ChangeOperation.None:
                        {

                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }

            MenuButtonGroupService service = new MenuButtonGroupService(ObjectContext, currentEntity.Tenant);
            service.Update(currentEntity , menuButtonChangeSet);

            //this.ChangeConnectionString(currentEntity.Tenant);
            //MapMenuButtonGroupPMMenuButtonGroup(currentEntity, menuButtonGroup);
            //MenuButtonGroupRepository.Update(menuButtonGroup);
        }

        public void DeleteMenuButtonGroup(MenuButtonGroupPM entity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            MenuButtonGroupRepository = new MenuButtonGroupRepository(ObjectContext);
            this.ChangeConnectionString(entity.Tenant);
            MenuButtonGroup menuButtonGroup = MenuButtonGroupRepository.GetSingleMenuButtonGroup(entity.Id);
            MenuButtonGroupRepository.Remove(menuButtonGroup);
        }
        #endregion 

        //#region MenuButtons
        //public IQueryable<MenuButton> GetMenuButtonsByTenant(int tenant)
        //{
        //    this.ChangeConnectionString(tenant);
        //    return MenuButtonRepository.GetMenuButtonsByTenant(tenant);
        //}


        //public IQueryable<MenuButton> GetMenuButtonsByMenuButtonGroup(string ObjectTableId, int tenant)
        //{
        //    this.ChangeConnectionString(tenant);
        //    return MenuButtonRepository.GetMenuButtonsByMenuButtonGroup(ObjectTableId, tenant);
        //}

        //public void InsertMenuButton(MenuButton entity)
        //{
        //    this.ChangeConnectionString(entity.Tenant);
        //    entity.Id = IdCounter.GetNumber().ToString();
        //    MenuButtonRepository.Add(entity);
        //}



        //public void UpdateMenuButton(MenuButton currentEntity)
        //{
        //    this.ChangeConnectionString(currentEntity.Tenant);
        //    MenuButtonRepository.Update(currentEntity);
        //}

        //public void DeleteMenuButton(MenuButton entity)
        //{
        //    this.ChangeConnectionString(entity.Tenant);
        //    MenuButtonRepository.Remove(entity);
        //}


        //#endregion 

        #region ObjectTableHelperControls
        public IQueryable<ObjectTableHelperControlPM> GetObjectTableHelperControlsByTenant(int tenant)
        {
            ObjectTableHelperControlsRepository = new ObjectTableHelperControlRepository(tenant);
            this.ChangeConnectionString(tenant);
            ObjectTableHelperControlQuery objectTableHelperControlQuery = new ObjectTableHelperControlQuery(ObjectTableHelperControlsRepository);
            return objectTableHelperControlQuery.GetObjectTableHelperControlPMsByTenant(tenant);
        }

        public IQueryable<ObjectTableHelperControlPM> GetObjectTableHelperControlsByTenantAndObjectTable(string objectTableId, int tenant)
        {
            ObjectTableHelperControlsRepository = new ObjectTableHelperControlRepository(tenant);
            this.ChangeConnectionString(tenant);
            ObjectTableHelperControlQuery objectTableHelperControlQuery = new ObjectTableHelperControlQuery(ObjectTableHelperControlsRepository);

            return objectTableHelperControlQuery.GetObjectTableHelperControlsByTenantAndObjectTable(objectTableId, tenant);
        }

        //public void MapObjectTableHelperControlPMObjectTableHelperControl(ObjectTableHelperControlPM objectTableHelperControlPM, ObjectTableHelperControl objectTableHelperControl)
        //{
        //    objectTableHelperControl.ControlPath = objectTableHelperControlPM.ControlPath;
        //    objectTableHelperControl.ObjectTableId = objectTableHelperControlPM.ObjectTableId;
        //    objectTableHelperControl.Tenant = objectTableHelperControlPM.Tenant;
        //    objectTableHelperControl.Code = objectTableHelperControlPM.Code;
        //    objectTableHelperControl.FeatureId = objectTableHelperControlPM.FeatureId;
        //}

        public void InsertObjectTableHelperControl(ObjectTableHelperControlPM entity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            ObjectTableHelperControlService service = new ObjectTableHelperControlService(ObjectContext , entity.Tenant);
            service.Create(entity);

            //ObjectTableHelperControlsRepository = new ObjectTableHelperControlRepository(ObjectContext);
            //this.ChangeConnectionString(entity.Tenant);
            //entity.Id = IdCounter.GetNumber("ObjectTableHelperControl", entity.Tenant).ToString();
            //ObjectTableHelperControl newhelperControl = new ObjectTableHelperControl();
            //newhelperControl.Id = entity.Id;
            //MapObjectTableHelperControlPMObjectTableHelperControl(entity, newhelperControl);
            //ObjectTableHelperControlsRepository.Add(newhelperControl);
        }

        public void UpdateObjectTableHelperControl(ObjectTableHelperControlPM currentEntity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(currentEntity.Tenant);
            }
            ObjectTableHelperControlService service = new ObjectTableHelperControlService(ObjectContext, currentEntity.Tenant);
            service.Update(currentEntity);

            //ObjectTableHelperControlsRepository = new ObjectTableHelperControlRepository(ObjectContext);
            //this.ChangeConnectionString(currentEntity.Tenant);
            //ObjectTableHelperControl helperControl = ObjectTableHelperControlsRepository.GetSingleObjectTableHelperControl(currentEntity.Id);
            //MapObjectTableHelperControlPMObjectTableHelperControl(currentEntity, helperControl);
            //ObjectTableHelperControlsRepository.Update(helperControl);
        }

        public void DeleteObjectTableHelperControl(ObjectTableHelperControlPM entity)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(entity.Tenant);
            }
            ObjectTableHelperControlsRepository = new ObjectTableHelperControlRepository(ObjectContext);
            this.ChangeConnectionString(entity.Tenant);
            ObjectTableHelperControl helperControl = ObjectTableHelperControlsRepository.GetSingleObjectTableHelperControl(entity.Id);
            ObjectTableHelperControlsRepository.Remove(helperControl);
        }
        #endregion 
        #endregion

       
        //private void CreateQuickFilters()
        //{
            
        //}

        private void ChangeConnectionString(int tenant)
        {
            //if (LastTenantEntered != tenant)
            //{
            //    GlobalTenantRepository globalTenantRepository = new GlobalTenantRepository();
            //    GlobalTenant GTenant = globalTenantRepository.GetGlobalTenantsByTenant(tenant);
            //    if (GTenant != null)
            //    {
            //       // ConnectionString = GTenant.GlobalDB.DBConnection;
            //        //ObjectContext.Connection.ChangeDatabase(GTenant.GlobalDB.Id);

            //        //this.ObjectContext.Connection.Close();
            //        this.ObjectContext.Connection.ConnectionString = BuildConnectionString( GTenant.GlobalDB.DBConnection);
            //        LastTenantEntered = tenant;
            //        //this.ObjectContext.Connection.Open();

            //    }
            //}

        }

        private string BuildConnectionString(string dbConnectionInfo)
        {
            string theEntityBuilder;

           // if (!WebFreightEntryPoint.InAzure)
           // {
                string[] information = dbConnectionInfo.Split(',');
                string dbName = information[0];
                string userName = information[1];
                string pass = information[2];
                SqlConnectionStringBuilder sqlBuilder =
                   new SqlConnectionStringBuilder();

                // Set the properties for the data source.
                sqlBuilder.DataSource = ".";
                sqlBuilder.InitialCatalog = dbName;
                sqlBuilder.IntegratedSecurity = false;
                sqlBuilder.UserID = userName;
                sqlBuilder.Password = pass;

                // Build the SqlConnection connection string.
                string providerString = sqlBuilder.ToString();

                // Initialize the EntityConnectionStringBuilder.
                EntityConnectionStringBuilder entityBuilder =
                    new EntityConnectionStringBuilder();

                //Set the provider name.
                entityBuilder.Provider = "System.Data.SqlClient";

                // Set the provider-specific connection string.
                entityBuilder.ProviderConnectionString = providerString;

                // Set the Metadata location.
                entityBuilder.Metadata = string.Format(@"res://*/{0}.csdl|res://*/{0}.ssdl|res://*/{0}.msl",
                    "WebFreightModel");

                theEntityBuilder = entityBuilder.ToString();
          //  }
            //else
            //{
            //    string[] information = DBConnectionInfo.Split(',');
            //    string dbName = information[0];
            //    string userName = information[1];
            //    string pass = information[2];
            //    SqlConnectionStringBuilder sqlBuilder =
            //       new SqlConnectionStringBuilder();

            //    // Set the properties for the data source.
            //    //sqlBuilder.DataSource = "Server=tcp:z0n0c08sao.database.windows.net";
            //    //sqlBuilder.InitialCatalog = dbName;
            //    //sqlBuilder.UserID = userName;
            //    //sqlBuilder.Password = pass;
            //    sqlBuilder.ConnectionString = "Server=tcp:z0n0c08sao.database.windows.net;Database=CTP2Main;User ID=simplog@z0n0c08sao;Password=Saas256!@;Trusted_Connection=False;Encrypt=True;MultipleActiveResultSets=True;";
            //    //sqlBuilder.Encrypt = true;
            //    //sqlBuilder.MultipleActiveResultSets = true;
            //    // Build the SqlConnection connection string.
            //    string providerString = sqlBuilder.ToString();

            //    // Initialize the EntityConnectionStringBuilder.
            //    EntityConnectionStringBuilder entityBuilder =
            //        new EntityConnectionStringBuilder();

            //    //Set the provider name.
            //    entityBuilder.Provider = "System.Data.SqlClient";

            //    // Set the provider-specific connection string.
            //    entityBuilder.ProviderConnectionString = providerString;

            //    // Set the Metadata location.
            //    entityBuilder.Metadata = string.Format(@"res://*/{0}.csdl|res://*/{0}.ssdl|res://*/{0}.msl",
            //        "WebFreightModel");

            //    EntityBuilder = entityBuilder.ToString();

           // }

            return theEntityBuilder;
        }

        [Invoke]
        public bool CheckCurrenctUserValidity(string clientEmail, int tenant)
        {
            if (ObjectContext == null)
            {
                ObjectContext = WebFreightContext.GetContext(tenant);
            }

            if (HttpContext.Current != null)
            {
                if (string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
                    return false;
                }
            }
             
            string authEmail = SecurityUtility.GetAuthenticatedUser();
            if (!authEmail.Trim().ToLower().Equals(clientEmail.Trim().ToLower()))
            {
                throw new Exception("Sorry! this user is not the last signed user!");
            }

            bool isBlocking;
            using (
                TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
            {
                IGlobalContext globalcontext = GlobalContext.GetContext();
                //string connection = globalcontext.GetCurrentConnection();
                //if (connection.Contains("Main"))
                //{ }

                isBlocking = (from a in globalcontext.GlobalDBs select a).FirstOrDefault().IsBlocking;
                GlobalContactRepository repository = new GlobalContactRepository(globalcontext);
                GlobalContact contact = repository.GetGlobalContactByEmailAndTenant(authEmail,tenant);
                if (contact != null && contact.InActive)
                {
                    scope.Complete();
                    return false;
                }
                scope.Complete();
            }
            bool isIpAuthenticated = true;
            string ipstring = LogitudeSettings.CustomerCareIP;//System.Configuration.ConfigurationManager.AppSettings.Get("CustomerCareIP");
            string[] authenticatedIPs = ipstring.Split(',');

            string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
            if (string.IsNullOrEmpty(currentIP))
            {
                currentIP = HttpContext.Current.Request.UserHostAddress;
            }

            if (!authenticatedIPs.Contains(currentIP))
            {
                isIpAuthenticated = false;
            }

            if (isBlocking)
            {
                if (!isIpAuthenticated)
                {
                    throw new Exception("System Upgrading");
                }
            }

            return true;
        }

        //protected override bool PersistChangeSet()
        //{
        //    ObjectContext.SaveChanges();
        //    return base.PersistChangeSet();
        //}

        //protected override bool PersistChangeSet(ChangeSet changeSet)
        //{
           
        //    return base.PersistChangeSet(changeSet);
        //}

        public List<Translation> GetGeneratedTranslations()
        {
            //Translation t1 = new Translation() {Id= "1", TextCodeCode = "Code1" , TranslatedText = "Translation1" };
            //Translation t2 = new Translation() { Id = "2", TextCodeCode = "Code2", TranslatedText = "Translation2" };
            //Translation t3 = new Translation() { Id = "3", TextCodeCode = "Code3", TranslatedText = "Translation3" };
            //Translation t4 = new Translation() { Id = "4", TextCodeCode = "Code4", TranslatedText = "Translation4" };
            //Translation t5 = new Translation() { Id = "5", TextCodeCode = "Code5", TranslatedText = "Translation5" };

            List<Translation> list = new List<Translation>();
            //list.Add(t1);
            //list.Add(t2);
            //list.Add(t3);
            //list.Add(t4);
            //list.Add(t5);
            return list;
        }

        //protected override void OnError(DomainServiceErrorInfo errorInfo)
            //{
        //    base.OnError(errorInfo);
        //    //string errorMessage;
        //    //errorMessage = errorInfo.Error.Message;

        //    //if (errorInfo.Error.InnerException != null)
        //    //{
        //    //    errorMessage += Environment.NewLine + errorInfo.Error.InnerException.Message;
        //    //}
        //    //errorMessage += Environment.NewLine + errorInfo.ToString();
        //    //if (!string.IsNullOrEmpty(errorInfo.Error.StackTrace))
        //    //{
        //    //    errorMessage += Environment.NewLine + errorInfo.Error.StackTrace;
        //    //}

        //    //ExceptionHandler.HandleException(errorInfo.Error, DateTime.Now, 0, "", "General Domain Service", "OnError()");

        //    //AzureLog.SaveLogsInStorage(errorMessage, "E", DateTime.Now, errorInfo.Error.Message, errorInfo.Error.StackTrace, 0, ServiceContext.User != null ? ServiceContext.User.Identity.Name : "", ServiceContext.User != null ? ServiceContext.User.Identity.Name : "");
        //    //throw errorInfo.Error;
        //}

        //public override bool Submit(ChangeSet changeSet)
            //{

        //    //using (var tx = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
        //    //{
        //    // call to base to process the changeset within
        //    // our transaction scope
        //    // if (!WebFreightEntryPoint.UsingAzure)
        //    // {
        //    using (TransactionScope scope = TransactionFactory.GetTransaction())
        //    {
        //        bool f = true;
        //        try
        //        {
        //            f = base.Submit(changeSet);
        //        }
        //        catch (DbEntityValidationException ex)
        //        {
        //            foreach (var eve in ex.EntityValidationErrors)
        //            {
        //                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //                foreach (var ve in eve.ValidationErrors)
        //        {
        //                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                        ve.PropertyName, ve.ErrorMessage);
        //                }
        //            }


        //            scope.Dispose();
        //            throw new DomainException(ex.Message);
        //        }
        //        // complete the transaction
        //        scope.Complete();

        //        return f;
            //    }
        //    // }
        //    //else
        //    //{
        //    //    bool f = true;
        //    //    try
        //    //    {
        //    //        f = base.Submit(changeSet);
        //    //    }
        //    //    catch (OptimisticConcurrencyException ex)
        //    //    {

        //    //    }
        //    //    // complete the transaction

        //    //    return f;
        //    //}

            //}
    }

    public class TranslationArgs
    {
        public List<FieldsTranslations> FieldsTranslations { get; set; }
        public int CountAll { get; set; }
    }
}




