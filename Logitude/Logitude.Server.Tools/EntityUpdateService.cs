using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.Server.Tools.Counters;
using System.Data.Entity;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Data.Entity.Validation;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.Server.Tools
{
    public abstract partial class EntityUpdateService <TEntityPOCO,TEntityPM,TEntityParentPM> 
        where TEntityPOCO : class , new() 
        where TEntityPM: EntityPM,new()
        where TEntityParentPM:EntityPM
    {
        protected  bool IsNewEntity;
        protected int Tenant;
        protected TEntityPOCO EntityPOCO { get; set; }
        protected TEntityPM OldEntityPM { get; set; }
        protected TEntityPM ChangeTrackingEntityPM { get; set; }
        protected string EntityChangeFieldXml { get; set; }
        private Dictionary<string, IContext> additionalContexts;
        protected Dictionary<string,IContext> AdditionalContexts
        {
            get { return additionalContexts; }
            set { additionalContexts = value; }
        }
        protected IMapping<TEntityPM, TEntityPOCO> Mapping;
        protected TEntityPM EntityPM;
        protected IRepository<TEntityPOCO> Repository;
        protected IContext MainContext;
        protected TEntityParentPM EntityParentPM;
        protected List<string> ErrorsList;
        protected bool ThrowValidationException;
        public EntityUpdateService()
        {

        }
     
        public  EntityUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
        {
            this.Tenant = tenant;
            this.AdditionalContexts = additionalContexts;
            this.MainContext = mainContext;
            this.ErrorsList = new List<string>();
            this.ThrowValidationException = true;
        }

        public void UpdateMulti(List<TEntityPM> entityPMList, List<TEntityPM> deletedEntityPMList, TEntityParentPM entityParentPM, bool commit)
        {
            try
            {
                //this.SetState(

                EntityParentPM = entityParentPM;
                if (EntityParentPM.ChangeSetOp == ChangeSetOperation.Insert)
                {
                    //SubmitChanges();
                }

                foreach (TEntityPM entityPM in entityPMList)
                {
                    Update(entityPM, commit);
                }

                foreach (TEntityPM entityPM in deletedEntityPMList)
                {
                    Update(entityPM, commit);
                }
            }
            finally
            {

            }
        }

        public void Update(TEntityPM entityPM, bool commit)
        {
            try
            {
                this.AddContext(entityPM);
                if (Transaction.Current != null && Transaction.Current.IsolationLevel != IsolationLevel.Snapshot)
                {
                    PerformUpdate(entityPM, commit);
                }
                else
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())//new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.Snapshot }))
                    {
                        PerformUpdate(entityPM, commit);
                        scope.Complete();
                    }
                }

            }
            finally
            {
                TraceLoadTest(this.GetDebugTrace());
                this.RemoveContext(entityPM);
            }
        }

        private void PerformUpdate(TEntityPM entityPM, bool commit)
        {
            EntityKeyFields entityKeys = GetKeys(entityPM);
            var iMapConvertFromBase64StringNVARCHARFields = Mapping as IMappingEncodeBase64NVARCHARFields<TEntityPM>;
            if (iMapConvertFromBase64StringNVARCHARFields != null)
            {
                iMapConvertFromBase64StringNVARCHARFields.EncodeBase64NVARCHARFields(entityPM);
            }
            this.EntityPM = entityPM;
            if (entityPM.ChangeSetOp != ChangeSetOperation.None)
            {
                switch (entityPM.ChangeSetOp)
                {
                    case ChangeSetOperation.Insert:
                        {
                            EntityPOCO = new TEntityPOCO();
                            OnCreating(EntityPM, EntityParentPM);
                            FillDefaultValuesOnCreate(entityPM);
                            break;
                        }
                    case ChangeSetOperation.Update:
                        {
                            
                            EntityPOCO = Repository.GetSingle(entityKeys);
                            AddStepTrace("GetEntityPOCO");
                            CheckConcurrency(entityPM, EntityPOCO);
                            AddStepTrace("CheckConcurrency");
                            OldEntityPM = new TEntityPM();
                            ChangeTrackingEntityPM = new TEntityPM();
                            Mapping.POCOToPM(OldEntityPM, EntityPOCO);
                            Mapping.POCOToPM(ChangeTrackingEntityPM, EntityPOCO);
                            Mapping.PMToOldPM(entityPM, ChangeTrackingEntityPM);
                            break;
                        }
                    case ChangeSetOperation.Delete:
                        {
                            EntityPOCO = Repository.GetSingle(entityKeys);

                            break;
                        }
                }
                AddStepTrace("B4OnUpdating");
                EntityChangeFieldXml = GetChangesDetectedXml(ChangeTrackingEntityPM);
                OnUpdating(entityPM);
                AddStepTrace("OnUpdating");
                OnUpdating(entityPM, EntityPOCO);
                AddStepTrace("OnUpdatingEntityPOCO");
                FillDefaultValuesOnUpdate(entityPM);
                AddStepTrace("FillDefaultValuesOnUpdate");
                string changesXml = ""; //GetChangesDetectedXml(ChangeTrackingEntityPM); to be done later on.

                Trace(entityPM, EntityPOCO, changesXml);
                AddStepTrace("Trace");
                Validate(entityPM);
                AddStepTrace("Validate");
                if (ErrorsList!=null && ErrorsList.Count > 0)
                {
                    if (ThrowValidationException)
                    {
                        string errors = "";
                        foreach (string error in ErrorsList)
                        {
                            errors = errors + Environment.NewLine + error;
                        }
                        throw new Exception(errors);
                    }
                    else
                    {
                        return;
                    }
                }
                Mapping.CustomPMToPOCO(EntityPM, EntityPOCO);
                Mapping.PMToPOCO(EntityPM, EntityPOCO);
                AddStepTrace("Mapping");
                switch (entityPM.ChangeSetOp)
                {
                    case ChangeSetOperation.Insert:
                        {
                            Repository.Add(EntityPOCO);
                            AddStepTrace("Insert");
                            break;
                        }
                    case ChangeSetOperation.Update:
                        {
                            Repository.Update(EntityPOCO);
                            AddStepTrace("Update");
                            break;
                        }
                    case ChangeSetOperation.Delete:
                        {
                            Repository.Remove(EntityPOCO);
                            AddStepTrace("Remove");
                            break;
                        }
                }
                
                UpdateComposition(entityPM);
                AddStepTrace("UpdateComposition");
                if (commit)
                {
                    SubmitChanges();
                }
                AfterUpdating(entityPM, EntityParentPM);
                AddStepTrace("AfterUpdating");
                UpdateCalculatedFields(entityPM, EntityParentPM, EntityPOCO);
                AddStepTrace("UpdateCalculatedFields");
            }
            //scope.Complete();
        }
        

        protected virtual void SubmitChanges()
        {
            try
            {
                MainContext.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                throw;
            }
            foreach (IContext context in AdditionalContexts.Values)
            {
                context.SaveChanges();
            }
            
        }

        protected virtual void OnCreating(TEntityPM entityPM, TEntityParentPM entityParentPM)
        {
        }

        protected virtual void FillDefaultValuesOnCreate(TEntityPM entityPM)
        {
        }

        protected virtual void FillDefaultValuesOnUpdate(TEntityPM entityPM)
        {
        }

        protected virtual void UpdateComposition(TEntityPM entityPM)
        {
 
        }

        protected virtual void OnUpdating(TEntityPM entityPM)
        {
               
        }

        protected virtual void AfterUpdating(TEntityPM entityPM, TEntityParentPM entityParentPM)
        {

        }
        protected virtual void UpdateCalculatedFields(TEntityPM entityPM, TEntityParentPM entityParentPM, TEntityPOCO entityPOCO)
        {

        }
        
        protected abstract EntityKeyFields GetKeys(TEntityPM entityPM);

        protected virtual void Trace(TEntityPM entityPM,TEntityPOCO entityPOCO,string changesXml)
        {
 
        }
        protected virtual void TraceLoadTest(string LoadTestLog)
        {

        }
        protected virtual void OnUpdating(TEntityPM entityPM,TEntityPOCO entityPOCO)
        {

        }

        private string GetChangesDetectedXml(TEntityPM changesTrackingEntityPM)
        {
            string xml = null;

            if (changesTrackingEntityPM != null)
            {
                r root = new r();
                root.cs = new List<c>();
                foreach (NotifyPropertyChangeValues value in changesTrackingEntityPM.ChangedProperties)
                {
                    string oldValue = value.OldValue != null ? value.OldValue.ToString() : "";
                    string newValue = value.NewValue != null ? value.NewValue.ToString() : "";

                    if (value.PropertyType == "CustomFieldClass")
                    {
                        object newFieldValue = value.NewValue;
                        if (newFieldValue.GetType() == typeof(CustomFieldClass))
                        {
                            CustomFieldClass classvalue = newFieldValue as CustomFieldClass;
                            newValue = !string.IsNullOrEmpty(classvalue.Value) ? classvalue.Value:"";
                        }
                
                        object oldFieldValue = value.OldValue;
                        if (oldFieldValue.GetType() == typeof(CustomFieldClass))
                        {
                            CustomFieldClass classvalue = oldFieldValue as CustomFieldClass;
                            oldValue = !string.IsNullOrEmpty(classvalue.Value) ? classvalue.Value : "";
                        }
                    }

                    oldValue = this.RemoveInvalidXmlChars(oldValue);
                    newValue = this.RemoveInvalidXmlChars(newValue);

                    c change = new c()
                    {
                        f = value.PropertyName,
                        n = newValue,
                        o = oldValue,
                    };
                    root.cs.Add(change);
                }

                xml = SerializeObjectToXml<r>(root); 
            }

            return xml;
        }
        private string RemoveInvalidXmlChars(string text)
        {
            var validXmlChars = text.Where(ch => XmlConvert.IsXmlChar(ch)).ToArray();
            return new string(validXmlChars);
        }


        private string SerializeObjectToXml<T>(T dataObject)
        {
            MemoryStream memstream = new MemoryStream();

            XmlSerializer ser = new XmlSerializer(typeof(T));

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "http://www.champ.aero/GCCS/CargoXML");
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "",
                OmitXmlDeclaration = true,
                NewLineChars = "",
                NewLineHandling = NewLineHandling.Replace,
            };

            XmlWriter writer = XmlTextWriter.Create(memstream, settings);
            writer.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n");
            ser.Serialize(writer, dataObject, ns);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            return content;
        }

        protected virtual void CheckConcurrency(TEntityPM entityPM, TEntityPOCO entityPOCO)
        {
            
        }
        


        public bool DontAddTransaction { get; set; }//if you don't want to add a transaction to the update service. update()

        protected virtual void Validate(TEntityPM entityPM)
        {
        }

        public virtual void InitializeUpdateService()
        {

        }

        public virtual void InitializeEntityPM(TEntityPM entityPM)
        {

        }
    }




}
