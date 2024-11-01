//using AmitalCloud.Infrastructure.Data;
//using AmitalCloud.Infrastructure.Data.BaseClasses;
//using AmitalCloud.Infrastructure.Data.DataContracts;
//using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
//using AmitalCloud.Infrastructure.Data.Helpers;
//using AmitalCloud.Infrastructure.Data.Interfaces;
//using AmitalCloud.Infrastructure.Data.Repositories;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity.Validation;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Transactions;
//using System.Xml;
//using System.Xml.Serialization;
//namespace AmitalCloud.Infrastructure.Application.BaseClasses
//{
//    public abstract class BaseService<TEntityPOCO, TEntityKeys, TEntityPM, TEntityParentPM, TEntityParentKeys, TEntityList, TkeyType> : IService<TEntityPOCO, TEntityKeys, TEntityPM, TEntityParentPM, TEntityParentKeys, TEntityList, TkeyType>
//     where TEntityPOCO : class, new()
//     where TEntityPM : IEntityPM, new()
//     where TEntityKeys : IEntityKeyFields<TkeyType>, new()
//     where TEntityParentKeys : IEntityKeyFields<TkeyType>
//     where TEntityParentPM : IEntityPM
//     where TEntityList : class, new()
//    {
//        protected int Tenant;
//        protected IMapping<TEntityPM, TEntityPOCO, TEntityList> mapping;
//        protected TEntityPM EntityPM;
//        protected IRepository<TEntityPOCO, TkeyType> Repository;
//        protected IContext MainContext;
//        protected TEntityParentPM EntityParentPM;
//        protected TEntityKeys EntityKeys;
//        protected TEntityParentKeys EntityParentKeys;
//        private Dictionary<string, IContext> additionalContexts;
//        protected List<string> ErrorsList;
//        protected bool ThrowValidationException;
//        protected List<FieldChange> FieldChanges;
//        protected AuditLogRepository AuditLogRepository;
//        protected bool IsNewEntity;
//        protected Dictionary<string, IContext> AdditionalContexts
//        {
//            get { return additionalContexts; }
//            set { additionalContexts = value; }
//        }
//        protected TEntityPM OldEntityPM { get; set; }
//        protected TEntityPM ChangeTrackingEntityPM { get; set; }
//        protected string EntityChangeFieldXml { get; set; }
//        protected TEntityPOCO EntityPOCO { get; set; }
//        protected TEntityList EntityList { get; set; }
//        public bool DontAddTransaction { get; set; }//if you don't want to add a transaction to the update service. update()
//        //
//        #region Constructors
//        public BaseService()
//        {

//        }
//        public BaseService(IContext mainContext, int tenant)
//        {
//            this.Tenant = tenant;
//            this.MainContext = mainContext;
//        }
//        public BaseService(IContext mainContext, Dictionary<string, IContext> additionalContexts, int tenant)
//        {
//            this.Tenant = tenant;
//            this.AdditionalContexts = additionalContexts;
//            this.MainContext = mainContext;
//            this.ErrorsList = new List<string>();
//            this.ThrowValidationException = true;
//            FieldChanges = new List<FieldChange>();
//            AuditLogRepository = new AuditLogRepository(tenant);
//        }

//        #endregion
//        #region Public Methods

//        public TEntityList GetSingle(IEnumerable<KeyValuePair<string, string>> paramList)
//        {
//            var keys = new TEntityKeys();
//            keys.Initialize(paramList);
//            EntityList = new TEntityList();
//            mapping.POCOToList(Repository.GetSingle(keys), EntityList);
//            return EntityList;
//        }
//        public int GetListCount(QueryOperations queryOperations)
//        {
//            return GetListCount(queryOperations, new TreeFilterQueryArgs());
//        }
//        public int GetListCount(QueryOperations queryOperations, TreeFilterQueryArgs treeFilterQueryArgs)
//        {
//            return GetQuery(queryOperations, treeFilterQueryArgs).Count();
//        }
//        public List<TEntityList> GetList(QueryOperations queryOperations, int tenant)
//        {
//            return GetList(queryOperations, tenant, new TreeFilterQueryArgs());
//        }
//        public List<TEntityList> GetList(int tenant)
//        {
//            return GetList(new QueryOperations() { QueryFilterItems = new List<QueryFilterItem>(), PageIndex = 0, GetAll = true }, tenant);
//        }
//        public List<TEntityList> GetList(QueryOperations queryOperations, int tenant, TreeFilterQueryArgs treeFilterQueryArgs)
//        {
//            var query2 = GetQuery(queryOperations, treeFilterQueryArgs);
//            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
//            {
//                PropertyInfo propInfo = typeof(TEntityList).GetProperty(queryOperations.SortByColumnName);
//                List<ObjectField> AccountingInformationIdentifierObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("AccountingInformationIdentifier", tenant).ToList();

//                ObjectField objectField = (from a in AccountingInformationIdentifierObjectFields
//                                           where a.FieldName == queryOperations.SortByColumnName
//                                           select a).FirstOrDefault();

//                if (objectField != null)
//                {
//                    if (objectField.IsCustom)
//                    {
//                        query2 = sortClass.GetSorterQuery<TEntityList, string>(queryOperations, query2);
//                    }
//                    else
//                    {
//                        switch (objectField.DataTypeCode.ToLower())
//                        {
//                            case "ntext":
//                            case "text":
//                                {
//                                    query2 = sortClass.GetSorterQuery<TEntityList, string>(queryOperations, query2);
//                                    break;
//                                }
//                            case "sigdouble":
//                            case "double":
//                                {
//                                    query2 = sortClass.GetSorterQuery<TEntityList, double>(queryOperations, query2);
//                                    break;
//                                }
//                            case "date":
//                            case "datetime":
//                                {
//                                    query2 = sortClass.GetSorterQuery<TEntityList, DateTime>(queryOperations, query2);
//                                    break;
//                                }
//                            case "unsinteger":
//                            case "integer":
//                                {
//                                    query2 = sortClass.GetSorterQuery<TEntityList, int>(queryOperations, query2);
//                                    break;
//                                }
//                            case "boolean":
//                                {
//                                    query2 = sortClass.GetSorterQuery<TEntityList, bool>(queryOperations, query2);
//                                    break;
//                                }
//                            case "unsdecimal":
//                            case "decimal":
//                                {
//                                    query2 = sortClass.GetSorterQuery<TEntityList, decimal>(queryOperations, query2);
//                                    break;
//                                }
//                            default:
//                                {
//                                    query2 = query2.OrderBy(d => d.Code);
//                                    break;
//                                }
//                        }
//                    }
//                }
//            }
//            else
//            {
//                //query2 = query2.OrderBy(d => d.);
//            }
//            if (!queryOperations.GetAll)
//            {
//                query2 = query2.Skip(queryOperations.PageIndex);
//                query2 = query2.Take(queryOperations.PageSize);
//            }
//            return query2.ToList();


//        }

//        //




//        public List<TEntityPM> GetMulti(TEntityParentKeys entityParentKeys, bool getFromCache, bool getComposition = true)
//        {
//            List<TEntityPOCO> entityPOCOs = Repository.GetMulti(entityParentKeys);
//            List<TEntityPM> entityPMs = new List<TEntityPM>();
//            foreach (TEntityPOCO entityPOCO in entityPOCOs)
//            {
//                TEntityPM entityPM = new TEntityPM();
//                BaseEntityKeyFields<TkeyType> entityKeys = GetKeys(entityPOCO);
//                if (entityKeys != null && getComposition)
//                {
//                    GetComposition(entityKeys, entityPM);
//                }
//                mapping.CustomPOCOToPM(entityPM, entityPOCO);
//                mapping.POCOToPM(entityPM, entityPOCO);
//                entityPMs.Add(entityPM);
//            }
//            return entityPMs;
//        }
//        public TEntityPM GetEntityPM(TEntityPOCO entityPOCO, TEntityKeys entityKeys, bool getComposition = false)
//        {
//            var entityPM = new TEntityPM();
//            if (entityPOCO == null)
//            {
//                return default(TEntityPM);
//            }
//            mapping.POCOToPM(entityPM, entityPOCO);
//            if (getComposition)
//            {
//                GetComposition(entityKeys, entityPM);
//            }
//            mapping.CustomPOCOToPM(entityPM, entityPOCO);
//            return entityPM;
//        }
//        public virtual void GetComposition(TEntityKeys entityKeys, TEntityPM entityPM)
//        {
//        }
//        public virtual void InitializeSettings()
//        {
//        }
//        public void UpdateMulti(List<TEntityPM> entityPMList, List<TEntityPM> deletedEntityPMList, TEntityParentPM entityParentPM, bool commit)
//        {
//            try
//            {
//                //this.SetState(

//                EntityParentPM = entityParentPM;
//                if (EntityParentPM.ChangeSetOp == ChangeSetOperation.Insert)
//                {
//                    //SubmitChanges();
//                }

//                foreach (TEntityPM entityPM in entityPMList)
//                {
//                    Update(entityPM, commit);
//                }

//                foreach (TEntityPM entityPM in deletedEntityPMList)
//                {
//                    Update(entityPM, commit);
//                }
//            }
//            finally
//            {

//            }
//        }
//        public void Update(TEntityPM entityPM, bool commit, TimeSpan? transactionTimeout = null)
//        {
//            try
//            {
//                this.AddContext(entityPM);
//                if (Transaction.Current != null && Transaction.Current.IsolationLevel != IsolationLevel.Snapshot)
//                {
//                    PerformUpdate(entityPM, commit);
//                }
//                else
//                {
//                    using (TransactionScope scope = TransactionFactory.GetTransaction(transactionTimeout))//new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.Snapshot }))
//                    {
//                        PerformUpdate(entityPM, commit);
//                        scope.Complete();
//                    }
//                }

//            }
//            finally
//            {
//                TraceLoadTest(this.GetDebugTrace());
//                this.RemoveContext(entityPM);
//            }
//        }
//        public virtual void InitializeUpdateService()
//        {
//        }
//        public virtual void InitializeEntityPM(TEntityPM entityPM)
//        {
//        }
//        public string GetDebugTrace()
//        {
//            if (IsMainUpdateService())
//            {
//                return EntityUpdateServiceContext.Current.GetDebugTrace();
//            }
//            return null;
//        }
//        public void GetAncestorEntityUpdateService(out object AncestorEntityUpdateService)
//        {
//            AncestorEntityUpdateService = null;
//            try
//            {
//                AncestorEntityUpdateService = EntityUpdateServiceContext.Current.EntityUpdateService;
//            }
//            catch (Exception)
//            {

//                //throw;
//            }

//        }
//        public void GetAncestor(out object entityPOCO, out object entityPM, out object entityParentPM)

//        {
//            entityPOCO = null; entityPM = null; entityParentPM = null;
//            try
//            {
//                var entityUpdateService = //myobject 
//                EntityUpdateServiceContext.Current.EntityUpdateService as dynamic;
//                entityPOCO = entityUpdateService.EntityPOCO;
//                entityPM = entityUpdateService.EntityPM;
//            }
//            catch (Exception)
//            {

//                //throw;
//            }

//        }

//        #endregion
//        #region Protected Methods
//        protected TEntityPM GetSingle(TEntityKeys entityKeys, bool getComposition, bool getFromCache)
//        {
//            if (getFromCache && (CacheManager.CacheWrapper != null))
//            {
//                string entityKeyString = entityKeys.GetEntityPMName() + "_" + entityKeys.GetFullKey();
//                var cacheObj = CacheManager.CacheWrapper.Get(entityKeyString);
//                if (cacheObj != null)
//                {
//                    EntityPM = cacheObj as TEntityPM;
//                }
//                else
//                {
//                    EntityPOCO = Repository.GetSingle(entityKeys);
//                    if (EntityPOCO != null)
//                    {
//                        EntityPM = GetEntityPM(EntityPOCO, getComposition, entityKeys);
//                    }
//                    if (EntityPM != null)
//                    {
//                        CacheManager.CacheWrapper.Insert(entityKeyString, EntityPM);
//                    }
//                    else
//                    {
//                        CacheManager.CacheWrapper.Insert(entityKeyString, new NullCache());
//                    }
//                }
//            }
//            else
//            {
//                EntityPM = null;
//                EntityPOCO = Repository.GetSingle(entityKeys);
//                if (EntityPOCO != null)
//                {
//                    EntityPM = GetEntityPM(EntityPOCO, getComposition, entityKeys);
//                }
//                return EntityPM;
//            }
//            return EntityPM;
//        }
//        protected virtual void SubmitChanges()
//        {
//            try
//            {
//                MainContext.SaveChanges();
//            }
//            catch (DbEntityValidationException e)
//            {
//                throw;
//            }
//            foreach (IContext context in AdditionalContexts.Values)
//            {
//                context.SaveChanges();
//            }

//        }
//        protected virtual void OnCreating(TEntityPM entityPM, TEntityParentPM entityParentPM)
//        {
//        }
//        protected virtual void FillDefaultValuesOnCreate(TEntityPM entityPM)
//        {
//        }
//        protected virtual void FillDefaultValuesOnUpdate(TEntityPM entityPM)
//        {
//        }
//        protected virtual void UpdateComposition(TEntityPM entityPM)
//        {

//        }
//        protected virtual void OnUpdating(TEntityPM entityPM)
//        {

//        }
//        protected virtual void AfterUpdating(TEntityPM entityPM, TEntityParentPM entityParentPM)
//        {

//        }
//        protected virtual void UpdateCalculatedFields(TEntityPM entityPM, TEntityParentPM entityParentPM, TEntityPOCO entityPOCO)
//        {
//        }
//        protected virtual void Trace(TEntityPM entityPM, TEntityPOCO entityPOCO, string changesXml)
//        {
//        }
//        protected virtual void TraceLoadTest(string LoadTestLog)
//        {
//        }
//        protected virtual void OnUpdating(TEntityPM entityPM, TEntityPOCO entityPOCO)
//        {
//        }
//        protected virtual void CheckConcurrency(TEntityPM entityPM, TEntityPOCO entityPOCO)
//        {
//        }
//        protected virtual void Validate(TEntityPM entityPM)
//        {
//        }
//        protected virtual void AddContext(TEntityPM myTEntityPM)
//        {
//            if (EntityUpdateServiceContext.Current == null)
//            {
//                EntityUpdateServiceContext.Current = new CurrentDebug() { EntityUpdateService = this as object };
//                ///EntityUpdateServiceContext.Current = this as object;
//            }
//        }
//        protected void AddExternalTrace(string Trace)
//        {
//            if (IsMainUpdateService())
//            {
//                EntityUpdateServiceContext.Current.AddExternalTrace(Trace);
//            }
//        }


//        protected virtual IQueryable<TEntityPOCO> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<TEntityPOCO> iQueryable)
//        {
//            return iQueryable;
//        }
//        protected virtual IQueryable<TEntityPOCO> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<TEntityPOCO> iQueryable)
//        {
//            return iQueryable;
//        }




//        #endregion
//        #region Private Methods
//        private void AddStepTrace(string stepName)
//        {
//            if (IsMainUpdateService())
//            {
//                EntityUpdateServiceContext.Current.AddStepTrace(stepName);
//            }
//        }
//        private string GetChangesDetectedXml(TEntityPM changesTrackingEntityPM)
//        {
//            string xml = null;

//            if (changesTrackingEntityPM != null)
//            {
//                r root = new r();
//                root.cs = new List<c>();
//                foreach (NotifyPropertyChangeValues value in changesTrackingEntityPM.ChangedProperties)
//                {
//                    string oldValue = value.OldValue != null ? value.OldValue.ToString() : "";
//                    string newValue = value.NewValue != null ? value.NewValue.ToString() : "";

//                    if (value.PropertyType == "CustomFieldClass")
//                    {
//                        if (value.NewValue != null)
//                        {
//                            object newFieldValue = value.NewValue;
//                            if (newFieldValue.GetType() == typeof(CustomFieldClass))
//                            {
//                                CustomFieldClass classvalue = newFieldValue as CustomFieldClass;
//                                newValue = !string.IsNullOrEmpty(classvalue.Value) ? classvalue.Value : "";
//                            }
//                        }
//                        if (value.OldValue != null)
//                        {
//                            object oldFieldValue = value.OldValue;
//                            if (oldFieldValue.GetType() == typeof(CustomFieldClass))
//                            {
//                                CustomFieldClass classvalue = oldFieldValue as CustomFieldClass;
//                                oldValue = !string.IsNullOrEmpty(classvalue.Value) ? classvalue.Value : "";
//                            }
//                        }
//                    }

//                    oldValue = this.RemoveInvalidXmlChars(oldValue);
//                    newValue = this.RemoveInvalidXmlChars(newValue);

//                    c change = new c()
//                    {
//                        f = value.PropertyName,
//                        n = newValue,
//                        o = oldValue,
//                    };
//                    root.cs.Add(change);
//                }

//                xml = SerializeObjectToXml<r>(root);
//            }

//            return xml;
//        }
//        private string RemoveInvalidXmlChars(string text)
//        {
//            var validXmlChars = text.Where(ch => XmlConvert.IsXmlChar(ch)).ToArray();
//            return new string(validXmlChars);
//        }
//        private string SerializeObjectToXml<T>(T dataObject)
//        {
//            MemoryStream memstream = new MemoryStream();
//            XmlSerializer ser = new XmlSerializer(typeof(T));
//            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
//            ns.Add("", "http://www.champ.aero/GCCS/CargoXML");
//            XmlWriterSettings settings = new XmlWriterSettings()
//            {
//                Indent = true,
//                IndentChars = "",
//                OmitXmlDeclaration = true,
//                NewLineChars = "",
//                NewLineHandling = NewLineHandling.Replace,
//            };
//            XmlWriter writer = XmlTextWriter.Create(memstream, settings);
//            writer.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n");
//            ser.Serialize(writer, dataObject, ns);
//            memstream.Seek(0, SeekOrigin.Begin);
//            var reader = new StreamReader(memstream);
//            string content = reader.ReadToEnd();
//            return content;
//        }
//        private bool IsMainUpdateService()
//        {
//            return EntityUpdateServiceContext.Current != null && EntityUpdateServiceContext.Current.EntityUpdateService == this;
//        }
//        private void RemoveContext(TEntityPM myTEntityPM)
//        {
//            //if (EntityUpdateServiceContext.Current == this)
//            if (EntityUpdateServiceContext.Current != null && EntityUpdateServiceContext.Current.EntityUpdateService == this)
//            {
//                EntityUpdateServiceContext.Current.EntityUpdateService = null;
//                EntityUpdateServiceContext.Current = null;
//            }
//        }
//        private void PerformUpdate(TEntityPM entityPM, bool commit)
//        {
//            BaseEntityKeyFields<TkeyType> entityKeys = GetKeys(entityPM);
//            var iMapConvertFromBase64StringNVARCHARFields = mapping as IMappingEncodeBase64NVARCHARFields<TEntityPM>;
//            if (iMapConvertFromBase64StringNVARCHARFields != null)
//            {
//                iMapConvertFromBase64StringNVARCHARFields.EncodeBase64NVARCHARFields(entityPM);
//            }
//            this.EntityPM = entityPM;
//            if (entityPM.ChangeSetOp != Data.Enums.ChangeSetOperation.None)
//            {
//                switch (entityPM.ChangeSetOp)
//                {
//                    case Data.Enums.ChangeSetOperation.Insert:
//                        {
//                            EntityPOCO = new TEntityPOCO();
//                            FillDefaultValuesOnCreate(entityPM);
//                            OnCreating(EntityPM, EntityParentPM);
//                            break;
//                        }
//                    case Data.Enums.ChangeSetOperation.Update:
//                        {
//                            EntityPOCO = Repository.GetSingle(entityKeys);
//                            AddStepTrace("GetEntityPOCO");
//                            CheckConcurrency(entityPM, EntityPOCO);
//                            AddStepTrace("CheckConcurrency");
//                            OldEntityPM = new TEntityPM();
//                            ChangeTrackingEntityPM = new TEntityPM();
//                            mapping.POCOToPM(OldEntityPM, EntityPOCO);
//                            mapping.POCOToPM(ChangeTrackingEntityPM, EntityPOCO);
//                            mapping.PMToOldPM(entityPM, ChangeTrackingEntityPM);
//                            break;
//                        }
//                    case Data.Enums.ChangeSetOperation.Delete:
//                        {
//                            EntityPOCO = Repository.GetSingle(entityKeys);
//                            break;
//                        }
//                }
//                AddStepTrace("B4OnUpdating");
//                EntityChangeFieldXml = GetChangesDetectedXml(ChangeTrackingEntityPM);
//                OnUpdating(entityPM);
//                AddStepTrace("OnUpdating");
//                OnUpdating(entityPM, EntityPOCO);
//                AddStepTrace("OnUpdatingEntityPOCO");
//                FillDefaultValuesOnUpdate(entityPM);
//                AddStepTrace("FillDefaultValuesOnUpdate");
//                string changesXml = ""; //GetChangesDetectedXml(ChangeTrackingEntityPM); to be done later on.
//                Trace(entityPM, EntityPOCO, changesXml);
//                AddStepTrace("Trace");
//                Validate(entityPM);
//                AddStepTrace("Validate");
//                if (ErrorsList != null && ErrorsList.Count > 0)
//                {
//                    if (ThrowValidationException)
//                    {
//                        string errors = "";
//                        foreach (string error in ErrorsList)
//                        {
//                            errors = errors + Environment.NewLine + error;
//                        }
//                        throw new Exception(errors);
//                    }
//                    else
//                    {
//                        return;
//                    }
//                }
//                mapping.CustomPMToPOCO(EntityPM, EntityPOCO);
//                mapping.PMToPOCO(EntityPM, EntityPOCO);
//                AddStepTrace("Mapping");
//                switch (entityPM.ChangeSetOp)
//                {
//                    case Data.Enums.ChangeSetOperation.Insert:
//                        {
//                            Repository.Add(EntityPOCO);
//                            AddStepTrace("Insert");
//                            break;
//                        }
//                    case Data.Enums.ChangeSetOperation.Update:
//                        {
//                            Repository.Update(EntityPOCO);
//                            AddStepTrace("Update");
//                            break;
//                        }
//                    case Data.Enums.ChangeSetOperation.Delete:
//                        {
//                            Repository.Remove(EntityPOCO);
//                            AddStepTrace("Remove");
//                            break;
//                        }
//                }

//                UpdateComposition(entityPM);
//                AddStepTrace("UpdateComposition");
//                if (commit)
//                {
//                    SubmitChanges();
//                }
//                AfterUpdating(entityPM, EntityParentPM);
//                AddStepTrace("AfterUpdating");
//                UpdateCalculatedFields(entityPM, EntityParentPM, EntityPOCO);
//                AddStepTrace("UpdateCalculatedFields");
//            }
//            //scope.Complete();
//        }
//        private IQueryable<TEntityList> GetQuery(QueryOperations queryOperations, TreeFilterQueryArgs treeFilterQueryArgs)
//        {
//            GenericFilter filter = new GenericFilter();
//            GenericSort sortClass = new GenericSort();
//            IQueryable<TEntityPOCO> iQueryable = MainContext.GetActiveDbContext().Set<TEntityPOCO>();
//            iQueryable = ApplyCustomFilters(queryOperations, ApplyBusinessUnitFilters(queryOperations, iQueryable));
//            QueryOperations nonListQueryOperation = new QueryOperations();
//            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
//            QueryOperations listQueryOperation = new QueryOperations();
//            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();
//            iQueryable = filter.GetFilteredQuery<TEntityPOCO>(nonListQueryOperation, iQueryable);
//            var query2 = InjectionUtil.Instance.ApplyTreeFilter<TEntityList>(filter.GetFilteredQuery<TEntityList>(listQueryOperation, mapping.GetIqueryableList(iQueryable)), treeFilterQueryArgs);
//            return query2;
//        }


//        #endregion
//        #region Abstract Methods
//        protected abstract BaseEntityKeyFields<TkeyType> GetKeys(TEntityPOCO entityPOCO);
//        protected abstract BaseEntityKeyFields<TkeyType> GetKeys(TEntityPM entityPM);
//        #endregion
//    }

//    class EntityUpdateServiceContext
//    {
//        //[ThreadStatic]
//        //public static object Current = null;
//        [ThreadStatic]
//        public static CurrentDebug Current = null;

//    }
//    class CurrentDebug
//    {
//        public object EntityUpdateService = null;
//        private StringBuilder _sb;
//        private Stopwatch _sw;

//        public CurrentDebug()
//        {
//            _sb = new StringBuilder();
//            _sw = Stopwatch.StartNew();
//        }


//        internal void AddStepTrace(string stepName)
//        {
//            _sb.AppendLine(stepName + ":Took:" + _sw.ElapsedMilliseconds);
//            _sw.Restart();

//        }



//        internal void AddExternalTrace(string Trace)
//        {
//            _sb.AppendLine("ExternalTrace:" + Trace);
//        }

//        internal string GetDebugTrace()
//        {
//            return _sb.ToString();
//        }
//    }


//}
