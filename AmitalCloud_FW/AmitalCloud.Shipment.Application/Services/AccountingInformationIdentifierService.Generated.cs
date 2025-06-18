
//using AmitalCloud.Infrastructure.Application.BaseClasses;
//using AmitalCloud.Shipment.Data.Context;
//using AmitalCloud.Shipment.Data.EntityDataMappings;
//using AmitalCloud.Shipment.Data.Repositories;
//using AmitalCloud.Shipment.Domain.EntityLists;
//using AmitalCloud.Shipment.Domain.EntityPMs;
//using System.Collections.Generic;
//using System.Linq;
//using POCO = AmitalCloud.Shipment.Domain.EntityPOCOs;

//namespace AmitalCloud.Shipment.Application.Services
//{
//    internal class AccountingInformationIdentifierService  
//            : BaseService<POCO.AccountingInformationIdentifier, AccountingInformationIdentifierKeys, AccountingInformationIdentifierPM, AccountingInformationIdentifierPM, AccountingInformationIdentifierKeys, AccountingInformationIdentifierList>
//    {
//        #region EntityListQueryServices.AccountingInformationIdentifierListQueryService

//        public AccountingInformationIdentifierList GetSingle(string code)
//        {
//            IEnumerable<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
//            paramList.Append(new KeyValuePair<string, string>("code", code.ToString()));
//            return base.GetSingle(paramList);
//        }
//        public AccountingInformationIdentifierService(IShipmentContext context) => this.context = context;

//        #endregion

//        #region EntityQueryServices.AccountingInformationIdentifierQueryService
//        AccountingInformationIdentifierRepository repository;
//        IShipmentContext context;
//        public AccountingInformationIdentifierService(int tenant)
//        {
//            context = ShipmentContext.GetContext(tenant);
//            MainContext = context;
//            repository = new AccountingInformationIdentifierRepository(context);
//            Repository = repository;
//            mapping = new AccountingInformationIdentifierDataMapping();
//        }
//        public AccountingInformationIdentifierService(AccountingInformationIdentifierRepository repository)
//        {
//            this.repository = repository;
//            Repository = repository;
//            mapping = new AccountingInformationIdentifierDataMapping();
//        }
//        public AccountingInformationIdentifierService(IShipmentContext context)
//        {
//            this.repository = new AccountingInformationIdentifierRepository(context);
//            this.context = context;
//            MainContext = context;
//            Repository = repository;
//            mapping = new AccountingInformationIdentifierDataMapping();
//        }
//        public AccountingInformationIdentifierPM GetSingle(string code, bool getComposition, bool getFromCache)
//        {
//            EntityKeys = new AccountingInformationIdentifierKeys() { Code = code };
//            return base.GetSingle(EntityKeys, getComposition, getFromCache);
//        }
//        protected override EntityKeyFields GetKeys(POCO.AccountingInformationIdentifier entityPOCO)
//        {
//            AccountingInformationIdentifierKeys entityKeys = new AccountingInformationIdentifierKeys() { Code = entityPOCO.Code, };
//            return entityKeys;
//        }
//        #endregion
//        #region EntityUpdateServices.AccountingInformationIdentifierUpdateService

//        AccountingInformationIdentifierRepository entityRepository;
//        public AccountingInformationIdentifierService(IContext mainContext, Dictionary<string, IContext> additionalContexts, int tenant)
//            : base(mainContext, additionalContexts, tenant)
//        {
//            IShipmentContext context = mainContext as ShipmentContext;
//            context = context ?? mainContext as IShipmentContext; //Up line is A BUG -and i need it 4 Fakes
//            mapping = new AccountingInformationIdentifierDataMapping();
//            Repository = new AccountingInformationIdentifierRepository(context);
//        }
//        private IShipmentContext currentContext;
//        public AccountingInformationIdentifierService(int tenant)
//        {
//            currentContext = ShipmentContext.GetContext(tenant);
//        }
//        public AccountingInformationIdentifierService(IShipmentContext context)
//        {
//            currentContext = context;
//        }
//        protected override EntityKeyFields GetKeys(AccountingInformationIdentifierPM entityPM)
//        {
//            AccountingInformationIdentifierKeys entityKeys = new AccountingInformationIdentifierKeys() { Code = entityPM.Code };
//            return entityKeys;
//        }
//        protected override void FillDefaultValuesOnCreate(AccountingInformationIdentifierPM entityPM)
//        {

//        }
//        protected override void FillDefaultValuesOnUpdate(AccountingInformationIdentifierPM entityPM)
//        {

//        }

//        #endregion
//    }
//}
