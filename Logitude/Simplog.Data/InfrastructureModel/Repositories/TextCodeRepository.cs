using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using System.Transactions;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class TextCodeRepository:IRepository<TextCode>
    {
          IWebFreightContext webFreightContext;
        public TextCodeRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }
        public TextCodeRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public TextCodeRepository()
        {
               webFreightContext=new WebFreightContext(); 
        }
        public TextCode GetTextCodeByTenantAndCode(string code, int tenant)
        {
            TextCode textCode = (from a in context.TextCodes.Include("ObjectTable").Include("SpellCheckedByUser")
                                 where a.Code == code && a.Tenant == tenant
                                 select a).FirstOrDefault();
            if (textCode == null)
            {
                textCode = (from a in context.TextCodes.Include("ObjectTable").Include("SpellCheckedByUser")
                            where a.Code == code && a.Tenant == 0
                            select a).FirstOrDefault();
            }

            return textCode;
        }

        

        public IQueryable<TextCode> GetTextCodes()
        {
            return context.TextCodes;
        }

        public IQueryable<TextCode> GetTextCodesByTenant(int tenant)
        {
            IQueryable<TextCode> textcodes = from a in context.TextCodes.Include("ObjectTable").Include("SpellCheckedByUser")
                                             where (a.Tenant == tenant || a.Tenant == 0)
                                             select a;

            return textcodes;
        }


        public List<TextCode> GetTextCodesByTenantAndObjectTable(int tenant, string objectTableName)
        {

            ObjectTable table = context.ObjectTables.Where(t => t.Name == objectTableName).FirstOrDefault();
            List<TextCode> textcodes = (from a in context.TextCodes.Include("ObjectTable").Include("SpellCheckedByUser")
                                        where (a.Tenant == tenant || a.Tenant == 0) && a.ObjectTableId == table.Id && a.InActive == false
                                             select a).ToList();

            return textcodes;
        }

   
        public static List<TextCode> GetTextCodesByTenantStep(int tenant,int skip,int take)
        {
            List<TextCode> result = new List<TextCode>();
             
            List<TextCode> allTextCodes = new List<TextCode>();

            allTextCodes = GetTenantTextCodesWithTenantZero(tenant);
             
            result = allTextCodes.Skip(skip).Take(take).ToList();


            return result;
        }


        public static List<TextCode> GetTenantTextCodesWithTenantZero(int tenant)
        {

            string listName = "tenantzerotextcodes";
            string tenantListName = "tenanttextcodes" + tenant;
            List<TextCode> result = new List<TextCode>();

            List<TextCode> currentTenantTextCodes = new List<TextCode>();
            List<TextCode> zeroTenantTextCodes = new List<TextCode>();

                  

            if (tenant != 0)
            {
               
                    if (CacheManager.CacheWrapper.Get(tenantListName) == null)
                    {
                        using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                        {
                            IWebFreightContext context = WebFreightContext.GetContext(tenant);
                            currentTenantTextCodes = (from a in context.TextCodes//.Include("ObjectTable")//.Include("SpellCheckedByUser")
                                                      where a.Tenant == tenant && a.InActive == false
                                                      select a).ToList();
                            scope.Complete();
                        }

                        CacheManager.CacheWrapper.Insert(tenantListName, currentTenantTextCodes, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                    }
                    else
                    {
                        currentTenantTextCodes = (List<TextCode>)CacheManager.CacheWrapper.Get(tenantListName);
                    }
                
          
            }

           
                if (CacheManager.CacheWrapper.Get(listName) == null)
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction(new TimeSpan(2, 0, 0)))//new TransactionScope(TransactionScopeOption.RequiresNew,new TimeSpan(2,0,0)))
                    {
                        IWebFreightContext context = WebFreightContext.GetContext(0);
                        zeroTenantTextCodes = (from a in context.TextCodes//.Include("ObjectTable")//.Include("SpellCheckedByUser")
                                                  where a.Tenant == 0 && a.InActive == false
                                                  select a).ToList();

                        scope.Complete();
                    }

                   
                    CacheManager.CacheWrapper.Insert(listName, zeroTenantTextCodes, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                else
                {
                    zeroTenantTextCodes = (List<TextCode>)CacheManager.CacheWrapper.Get(listName);
                }
            
           

            result = zeroTenantTextCodes.Concat(currentTenantTextCodes).ToList();


            return result;
           
        }

      
        public IQueryable<TextCode> GetActiveTextCodesByTenant(int tenant)
        {
            IQueryable<TextCode> textcodes = from a in context.TextCodes.Include("ObjectTable").Include("SpellCheckedByUser")
                                             where a.Tenant == tenant && !a.InActive
                                             select a;

            return textcodes;
        }

        public TextCode GetSingleTextCode(string id)
        {
            return (from a in context.TextCodes.Include("ObjectTable").Include("SpellCheckedByUser")
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public TextCode GetSingleTextCodeByCode(string code)
        {
            return (from a in context.TextCodes.Include("ObjectTable").Include("SpellCheckedByUser")
                    where a.Code == code
                    select a).FirstOrDefault();
        }

        public TextCode GetSingleTextCodeByTenant(string code, int tenant)
        {
            return (from a in context.TextCodes.Include("ObjectTable").Include("SpellCheckedByUser")
                    where a.Code == code && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public string GetSingleTextCodeByCode(string code, int tenant)
        {
            return (from a in context.TextCodes.Include("ObjectTable").Include("SpellCheckedByUser")
                    where a.Code == code && a.Tenant == tenant
                    select a.Id).FirstOrDefault();
        }


        public int GetTextCodesCountForDefaultTranslation(int tenant, string objectTableId, string isSpellCheckedCode, string textCodeTypeCode, DateTime? selectedCheckDate, string checkDateFiler, string searchText)
        {
            int result = 0;
            List<TextCode> data = context.TextCodes.Where(d => d.Tenant == tenant && !d.InActive).ToList();
            List<TextCode> filteredListTable = string.IsNullOrEmpty(objectTableId) ? data : data.Where(d => d.ObjectTableId == objectTableId).ToList();
            List<TextCode> filteredListSearch = string.IsNullOrEmpty(searchText) ? filteredListTable : filteredListTable.Where(d => d.Code.ToUpper().Contains(searchText.ToUpper()) || (!string.IsNullOrEmpty(d.DefaultText) && d.DefaultText.ToUpper().Contains(searchText.ToUpper())) || (!string.IsNullOrEmpty(d.LocalDefaultText) && d.LocalDefaultText.ToUpper().Contains(searchText.ToUpper()))).ToList();
            List<TextCode> filteredListType = (textCodeTypeCode == "All") ? filteredListSearch : filteredListSearch.Where(d => d.TextCodeTypeCode == textCodeTypeCode).ToList();           
            List<TextCode> filteredListDate = new List<TextCode>();
            List<TextCode> filteredListChecked = new List<TextCode>();

            if (selectedCheckDate == null || checkDateFiler == "None")
            {
                filteredListDate = filteredListType;
            }

            if (selectedCheckDate != null)
            {
                if (checkDateFiler == "Equals") { filteredListDate = filteredListType.Where(d => d.SpellCheckDate == selectedCheckDate).ToList(); }
                else if (checkDateFiler == "Bigger") { filteredListDate = filteredListType.Where(d => d.SpellCheckDate > selectedCheckDate).ToList(); }
                else if (checkDateFiler == "Less") { filteredListDate = filteredListType.Where(d => d.SpellCheckDate < selectedCheckDate).ToList(); }
            }

            if (isSpellCheckedCode == "None")
            {
                filteredListChecked = filteredListDate;
            }

            else if (isSpellCheckedCode != "None")
            {
                if (isSpellCheckedCode == "True")
                {
                    filteredListChecked = filteredListDate.Where(d => d.IsSpellChecked == true).ToList();
                }

                else
                {
                    filteredListChecked = filteredListDate.Where(d => d.IsSpellChecked == false).ToList();
                }
            }

            if (filteredListChecked != null && filteredListChecked.Count > 0)
            {
                result = filteredListChecked.Count;
            }

            return result;
        }

     
        public void Add(TextCode entity)
        {
            context.TextCodes.Add(entity);
        }

        public void Remove(TextCode entity)
        {
            context.TextCodes.Attach(entity);
            context.TextCodes.Remove(entity);
        }

        public void Update(TextCode entity)
        {
            try { context.TextCodes.Attach(entity); }
            catch { }
            context.SetAsModified(entity);
        }

        public List<TextCode> All()
        {
            return context.TextCodes.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }






        public List<TextCode> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public TextCode GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public int GetTextCodesByTenantCount(int tenant)
        {
            var textcodes = (from a in context.TextCodes
                            where (a.Tenant == tenant || a.Tenant == 0) && a.TextCodeTypeCode != "T" && !a.InActive 
                            select a).ToList().Count();

            return textcodes;
        }
    }
}
