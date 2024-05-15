using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.CustomFilters;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.TreeFilterQuery;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;


namespace WebFreight.Web.Controllers.CommonDataModel.Extended
{
    public class CardExtendedController : ApiController
    {
        AuthenticationToken authToken;
        GLAccountCardsDataQueryService gLAccountCardsDataQueryService;
        List<CardList> connectedCards;
        GLAccountPM mainGLAccount;
        List<GLAccountPM> splitByCurrencyGLAccounts;
        CardPM card;
        
        public HttpResponseMessage GetDisconnectGLAccountFromCard(string Id, string partnerTypeId,string eventTypeCode)
        {
            try
            {
                authToken = GetAuthenticationToken();
                CheckContactFeature(partnerTypeId);

                 card = GetCardById(Id);
                if (card.GLAccountId != null)
                {
                    card.IsExcludeCard = true;
                    GLAccountPM glaccount = UpdateGLAccountFields(card);
                    SendHybridTask(glaccount);
                    UpdateCard(card);
                    if (card.PartnerTypeId == PartnerTypes.Customer || partnerTypeId == PartnerTypes.Vendor || partnerTypeId== PartnerTypes.AccountingPartner)
                        HandleGLAccountCardsData(glaccount);
                  //  DeleteGLAccountCardData(glaccount);
                    CreateEvents(Id, eventTypeCode);
                }
                return Request.CreateResponse(HttpStatusCode.OK, "Ok");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        string partnerObjectTableName;
         private void HandleGLAccountCardsData(GLAccountPM account)
        {
            connectedCards = GetGLAccountConnectedCards(account);
            if(connectedCards.Count == 0)
            {
                DeleteGLAccountCardData(account);
            }
            else if(connectedCards.Count>0)
            {
                GLAccountCardDataService accountCardDataService = new GLAccountCardDataService(card.Id, account.Id, card.Tenant);
                accountCardDataService.UpdateGLaccountCardsData();
               // UpdateGLAccountCardsData();
                //ApplyChangesOnGLAccounts();
            }
        }
        private List<CardList> GetGLAccountConnectedCards(GLAccountPM account)
        {
            CardQuery cardQuery = new CardQuery(account.Tenant);
            List<CardList> connectCards = cardQuery.GetCardPMsByGLAccountId(account.Id, account.Tenant);
            return connectCards;
        }
        private void DeleteGLAccountCardData(GLAccountPM gLAccount)
        {
             gLAccountCardsDataQueryService = new GLAccountCardsDataQueryService(gLAccount.Tenant);
            GLAccountCardsDataPM gLAccountCardsDataPM;
            if (glaccount.IsMultiCurrency == true)
            {
                gLAccountCardsDataPM = GetSingleGLAccountCardsData(glaccount.CardsDataId);
            }
           else
            {
                gLAccountCardsDataPM = GetGLAccountCardsDataForSingleCurrencyGLAccount();
            }
          
            if (gLAccountCardsDataPM != null)
            {
                ApplyChangesOnGLAccounts();
                UpdateGLAccountCardsData(gLAccountCardsDataPM);
              
            }

        }
      
        private void SaveGLAccountChanges(GLAccountPM accountPM)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(authToken.Tenant);
            GLAccountUpdateService gLAccountCardsDataUpdateService = new GLAccountUpdateService(accountingContext, new Dictionary<string, IContext>(), authToken.Tenant);
            gLAccountCardsDataUpdateService.Update(accountPM, true);
        }
       private void UpdateGLAcccount(GLAccountPM gLAccount)
        {
            gLAccount.CardsDataId = null;
            gLAccount.ChangeSetOp = ChangeSetOperation.Update;
            SaveGLAccountChanges(gLAccount);

        }
        private void ApplyChangesOnGLAccounts()
        {
            SetMainGLAccount();
           if(mainGLAccount != null) {
                UpdateGLAcccount(mainGLAccount);
                UpdateSplitByCurrencyAccounts(mainGLAccount);
            }
            else
            {
                UpdateGLAcccount(glaccount);
            }
                 
          
        }

        private void SetMainGLAccount()
        {
            if (glaccount.IsMultiCurrency == true)
            {
                mainGLAccount = glaccount;
            }
        }

        private void UpdateSplitByCurrencyAccounts(GLAccountPM accountPM)
        {
            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(accountPM.Tenant);
            List<GLAccountPM> gLAccounts = gLAccountQueryService.GetSplittedByCurrencyGLAccounts(accountPM.Id, accountPM.Tenant).ToList();
            foreach(GLAccountPM gLAccount in gLAccounts)
            {
                UpdateGLAcccount(gLAccount);
                SaveGLAccountChanges(gLAccount);
            }
        }

        private void UpdateGLAccountCardsData(GLAccountCardsDataPM gLAccountCardsDataPM)
        {
            gLAccountCardsDataPM.ChangeSetOp = ChangeSetOperation.Delete;
            IAccountingContext accountingContext = AccountingContext.GetContext(authToken.Tenant);
            GLAccountCardsDataUpdateService gLAccountCardsDataUpdateService = new GLAccountCardsDataUpdateService(accountingContext, new Dictionary<string, IContext>(), authToken.Tenant);
            gLAccountCardsDataUpdateService.Update(gLAccountCardsDataPM, true);
        }
        private GLAccountCardsDataPM GetGLAccountCardsDataForSingleCurrencyGLAccount()
        {
            GLAccountPM glacountToDeleteCardDataFrom;
            glacountToDeleteCardDataFrom = GetMainForSingleCurrencyGLAccount(glaccount);
            if (glacountToDeleteCardDataFrom != null)
            {
                mainGLAccount = glacountToDeleteCardDataFrom;
                return GetSingleGLAccountCardsData(mainGLAccount.CardsDataId);
            }
            return null;
        }
        private GLAccountCardsDataPM GetSingleGLAccountCardsData(string id)
        {
            return gLAccountCardsDataQueryService.GetSingle(id, false, false);
        }
        private GLAccountPM GetMainForSingleCurrencyGLAccount(GLAccountPM account)
        {

            GLAccountCurrencyPM gLAccountCurrency = GetSingleGLAccountCurrency(account); 
            if (gLAccountCurrency != null)
            {
                mainGLAccount= GetGLAccountById(gLAccountCurrency.MainGLAccountId);
                return mainGLAccount;
            }
            else return null;
           
        }
        private GLAccountCurrencyPM GetSingleGLAccountCurrency(GLAccountPM account)
        {
            GLAccountCurrencyQueryService gLAccountCurrencyQueryService = new GLAccountCurrencyQueryService(account.Tenant);
            return gLAccountCurrencyQueryService.GetEntityByGLAccountId(account.Id, account.Tenant);
        }
        private void CheckContactFeature(string partnerTypeId)
        {

            partnerObjectTableName = GetPartnerObjectTableName(partnerTypeId);
            partnerObjectTableName = partnerObjectTableName.Replace(" ", String.Empty);
            CheckObjectTableContactFeature(partnerObjectTableName);
        }

        private void CheckObjectTableContactFeature(string tableName)
        {
            SecurityUtility.CheckContactFeature(tableName, "UPDATE", authToken.Tenant);
        }

        private string GetPartnerObjectTableName(string id)
        {
            PartnerTypeQuery partnerTypeQuery = new PartnerTypeQuery(authToken.Tenant);
            PartnerTypePM partnerType = partnerTypeQuery.GetSinglePM(id);
            if (partnerType != null)
            {
                return partnerType.Name;
            }

            else return null;
        }

        private CardPM GetCardById(string id)
        {
            CardQuery cardQuery = new CardQuery(authToken.Tenant);
            return cardQuery.GetSinglePM(id, authToken.Tenant);
        }
        private AuthenticationToken GetAuthenticationToken()
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            return authToken;
        }

        private void UpdateCard(CardPM card)
        {
            card.GLAccountId = null;
            card.GLAccountDisplayNumber = null;
            ICommonDataContext commonContext = CommonDataContext.GetContext(authToken.Tenant);
            CardService cardService = new CardService(commonContext, authToken.Tenant);
            cardService.Update(card);

        }
        GLAccountPM glaccount;
        private GLAccountPM UpdateGLAccountFields(CardPM card)
        {
            glaccount = GetGLAccountById(card.GLAccountId);
            if (glaccount != null)
            {
              
                glaccount.PartnerTypeId = card.PartnerTypeId;
                glaccount.CardCode = card.Code;
            }
            return glaccount;
        }
        private void SendHybridTask(GLAccountPM gLAccount)
        {
            if (gLAccount != null)
            {
                IAccountingContext MyContext = AccountingContext.GetContext(authToken.Tenant);
                GLAccountUpdateService service = new GLAccountUpdateService(MyContext, new Dictionary<string, IContext>(), authToken.Tenant);
                service.SendHybridTask(gLAccount);
            }

        }
        private GLAccountPM GetGLAccountById(string id)
        {
            GLAccountQueryService gLAccountQuery = new GLAccountQueryService(authToken.Tenant);
            return gLAccountQuery.GetSingle(id, false, false);
        }
        private void CreateEvents(string cardId, string eventTypeCode)
        {
            CreateTraceEvent(glaccount.Id, "GLAccount", "DIST");
            CreateTraceEvent(cardId, partnerObjectTableName, eventTypeCode);
        }
        private void CreateTraceEvent(string id,string objecttableName,  string eventTypeCode)
        {
            ContactRepository contactRep = new ContactRepository(authToken.Tenant);
            string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(authToken.Tenant);
            Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, authToken.Tenant);

            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                EntityId = id,
                Tenant = authToken.Tenant,
                UserId = contact.Id,
                ObjectTableName = objecttableName,
                IsAddedManually = false,
                EventTypeCode = eventTypeCode,
                Notes = SetNotesForDisconnectGLAccountEvent(),
            });
        }

        private string SetNotesForDisconnectGLAccountEvent()
        {
            return string.Concat("Internal number: ", glaccount.InternalNumber , "\n Local name: " , glaccount.LocalName);
        }

        public HttpResponseMessage GetAllConnectedPartnersByGLAccountId(string glAccountId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("GLAccount", "READ", tenant);

                IAccountingContext MyContext = AccountingContext.GetContext(authToken.Tenant);
                GLAccountConnectedPartnerService gLAccountConnectedPartnerService = new GLAccountConnectedPartnerService(tenant);
                List<ShortPartnersDetails> connectedPartners= gLAccountConnectedPartnerService.GetAllConnectedPartnersByGLAccountId(glAccountId);


                return Request.CreateResponse(HttpStatusCode.OK, connectedPartners);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }



        public HttpResponseMessage GetByCompactFiltersShort([FromUri] ApiQueryFilters filters)
   {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;
                if (filters.Tenant != null)
                    tenant = filters.Tenant.Value;

                QueryOperations queryOperations = new QueryOperations()
                {
                    ObjectTableName = "Card",
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    QuerySection = "Cards",
                    SortByColumnName = filters.SortBy,
                    SortDirectin = filters.SortDirection,
                    GetAll = filters.GetAll,
                };

                List<ObjectField> CardObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Card", tenant);
                List<PropertyInfo> filterProperties = filters.GetType().GetProperties().ToList();
                for (int i = 1; i <= 10; i++)
                {
                    object filterNameProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Name")).GetValue(filters);
                    object filterValue1 = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Value")).GetValue(filters);
                    object filterOperatorProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Operator")).GetValue(filters);
                    object filterValue2 = null;

                    if (filterNameProp != null)
                    {
                        string filterName = filterNameProp.ToString();
                        string filterOperator = filterOperatorProp != null ? filterOperatorProp.ToString() : "Equals";
                        //if (filterValue1 != null && filterValue1.GetType() == typeof(string))
                        //{
                        //string[] values = filterValue1.ToString().Split(',');
                        //if (values.Count() > 1)
                        //{
                        //filterValue1 = values[0];
                        //filterValue2 = values[1];
                        //}
                        //}
                        //ToDo: Get object field by name and set the remained filter properties
                        ObjectField field = CardObjectFields.FirstOrDefault(f => f.FieldName == filterName);
                        if (field != null)
                        {
                            string valuestring1 = filterValue1 != null ? filterValue1.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filterValue2 != null ? filterValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            //queryOperations.SetFilter(filterName, value1, field.IsCustomFilter, filterOperator, value2, field.DisplayInList);
                            queryOperations.SetFilter(filterName, value1, field.IsCustomFilter, filterOperator, value2, field.DisplayInList, field.IsCustom, field.DataTypeCode, field.IsListFilter);
                        }
                        else
                            queryOperations.SetFilter(filterName, filterValue1, false, filterOperator, filterValue2, true);
                    }



                }

                if (!string.IsNullOrEmpty(filters.AdditionalFilters))
                {
                    JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                    var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);

                    foreach (QueryFilterItem filter in filters_list)
                    {
                        ObjectField field = CardObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
                        if (field != null)
                        {


                            string valuestring1 = filter.FieldValue != null ? filter.FieldValue.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filter.FieldValue2 != null ? filter.FieldValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            //queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList);
                            queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList, field.IsCustom, field.DataTypeCode, field.IsListFilter);
                        }
                        else
                        {
                            queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                        }
                    }
                }



                GenericFilter genericFilter = new GenericFilter();
                GenericSort sortClass = new GenericSort();

                TreeFilterQueryArgs treeFilterQueryArgs = new TreeFilterQueryArgs()
                {
                    AdditionalTreeFilter = filters.TreeFilters,
                    ObjectTableName = "Card",
                    ParentEntityId = filters.ParentEntityId,
                    ParentObjectTableName = filters.ParentObjectTableName,
                    Tenant = tenant,
                    ParentEntity = filters.ParentEntity
                };

                QueryFilterItem item = queryOperations.QueryFilterItems.Where(f => f.FieldName == "CompactSearchField" || f.FieldName == "CardSearchField").FirstOrDefault();

                queryOperations.QueryFilterItems.Remove(item);
                object compactSeachvalue = item != null ? item.FieldValue : null;

                ICommonDataContext MyContext = CommonDataContext.GetContext(tenant);
                CardRepository cardRepository = new CardRepository(MyContext);
                IQueryable<Card> entityPocos = cardRepository.GetCards(tenant);

                CardQuery cardQuery = new CardQuery(cardRepository);

                QueryOperations nonListQueryOperation = new QueryOperations();
                nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false && !d.IsListFilter).ToList();
                QueryOperations listQueryOperation = new QueryOperations();
                listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true || d.IsListFilter).ToList();

                CardCustomFilter customfilters = new CardCustomFilter(tenant);
                entityPocos = customfilters.GetFilteredQuery(queryOperations, entityPocos, true, MyContext);

                entityPocos = genericFilter.GetFilteredQuery<Card>(nonListQueryOperation, entityPocos);
                int skippedEntities = queryOperations.PageIndex;
                IQueryable<CardList> entityLists = cardQuery.GetIQueryableEntityListShort(entityPocos);

                entityLists = genericFilter.GetFilteredQuery<CardList>(listQueryOperation, entityLists);
                entityLists = new TreeFilterQueryService().Apply<CardList>(entityLists, treeFilterQueryArgs);

                ServiceResponse response = new ServiceResponse();
                if (compactSeachvalue != null)
                {
                    entityLists = CardCompactFilter.GetFilteredQuery(compactSeachvalue, queryOperations, genericFilter, entityLists, tenant);
                }
                else
                {
                    if (string.IsNullOrEmpty(queryOperations.SortByColumnName))
                    {
                        queryOperations.SortByColumnName = "EnglishName";
                    }
                    if (string.IsNullOrEmpty(queryOperations.SortDirectin))
                    {
                        queryOperations.SortDirectin = "Ascending";
                    }

                    entityLists = QuerySortClass.GetSortedQuery(queryOperations, entityLists, "Card", tenant);
                }

                response.Count = entityLists.Count();
                entityLists = entityLists.Take(queryOperations.PageSize);

                response.Result = entityLists;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }


    public static class PartnerTypes
    {

        public static string Customer = "CS";
        public static string Vendor = "VD";
        public static string AccountingPartner = "AC";

    }
}