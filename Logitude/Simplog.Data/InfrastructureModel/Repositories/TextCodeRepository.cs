using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using System.Transactions;
using Simplog.Server.Infrastructure;
using System.Text;

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

        public IQueryable<TextCode> GetTextCodesByTenantForCustomization(int tenant)
        {
            IQueryable<TextCode> textcodes = from a in context.TextCodes
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

        public List<TextCode> GetDigitalTextCodesByTenantAndObjectTable(int tenant, string objectTableName)
        {
            var allowedTextCodesTypes = new List<string> { "F", "QC", "CH", "TH", "G" , "O"};

            List<TextCode> textcodes = context.TextCodes
                                              .Where(a => (a.Tenant == tenant || a.Tenant == 0) 
                                                           && (a.ObjectTable.Name.Equals(objectTableName) 
                                                                || (a.ObjectTable
                                                                    .Name
                                                                    .Equals("general", StringComparison.InvariantCultureIgnoreCase)
                                                                    && a.Code.StartsWith(objectTableName))) 
                                                           && allowedTextCodesTypes.Contains(a.TextCodeTypeCode))
                                              .ToList();
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
                        IWebFreightContext context = WebFreightContext.GetContext(tenant);
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

		public TextCode GetSingleTextCodeByCode(string code, bool fromCache = false)
		{
			string codeName = "GetSingleTextCodeByCode" + code;
			TextCode textCode = new TextCode();
			if (fromCache)
			{
				if (CacheManager.CacheWrapper.Get(codeName) == null)
				{
					textCode = (from a in context.TextCodes.Include("ObjectTable").Include("SpellCheckedByUser")
								where a.Code == code
								select a).FirstOrDefault();

					CacheManager.CacheWrapper.Insert(codeName, textCode, null);
				}
				else
				{
					textCode = (TextCode)CacheManager.CacheWrapper.Get(codeName);
				}
				return textCode;
			}
			else
			{
				return (from a in context.TextCodes.Include("ObjectTable").Include("SpellCheckedByUser")
						where a.Code == code
						select a).FirstOrDefault();
			}

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
            entity.LocalDefaultText = TryConvertFromBase64(entity.LocalDefaultText);

            context.TextCodes.Add(entity);
        }

        public void Remove(TextCode entity)
        {
            context.TextCodes.Attach(entity);
            context.TextCodes.Remove(entity);
        }

        public void Update(TextCode entity)
        {
            entity.LocalDefaultText = TryConvertFromBase64(entity.LocalDefaultText);

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
        public static string TryConvertFromBase64(string input)
        {
            try
            {
                if (input == null)
                {
                    return null;
                }
                if (input.StartsWith("BS64:") || input.StartsWith("\"BS64:"))
                {

                    return ConvertFromBase64(input);


                }
                return input;

            }
            catch (FormatException)
            {
                return input;
            }
        }

        private static string ConvertFromBase64(string input)
        {
            string substringToRemove = "\"";
            string backUp = input;
            try
            {
                input = input.Trim('\"');
                input = input.Substring(5);//REMOVE BS64:
                byte[] data = Convert.FromBase64String(input);
                string decodedString = Encoding.UTF8.GetString(data);
                decodedString = decodedString.Trim('\"');
                decodedString = decodedString.Replace("\\\"", "\"").Replace("\\\\", "\\");
                return decodedString;

            }
            catch (FormatException)
            {
                return backUp;
            }

        }

    }
}
