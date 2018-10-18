using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.MetaDataUpdate
{
    public partial class MetaDataUpdateClass
    {
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
        public TipRepository TipRepository { get; set; }
        public TenantSettingRepository TenantSettingRepository { get; set; }
        public QueryGroupRepository QueryGroupRepository { get; set; }
        public ObjectTableRuleRepository ObjectTableRuleRepository { get; set; }
        public ObjectTableRuleFieldRepository ObjectTableRuleFieldRepository { get; set; }
        public RuleConditionFieldRepository RuleConditionFieldRepository { get; set; }
        public EntityStatusRepository EntityStatusRepository { get; set; }
        public EventTypeRepository EventTypeRepository { get; set; }
        public MeasurementRepository MeasurementRepository { get; set; }
        public RankRepository RankRepository { get; set; }
        public EmailAlertSettingRepository EmailAlertSettingRepository { get; set; }
        public CreditCardTypeRepository CreditCardTypeRepository { get; set; }
        public MoveTypeRepository MoveTypeRepository { get; set; }
        public CounterDefinitionRepository CounterDefinitionRepository { get; set; }
        public CounterRepository CounterRepository { get; set; }
        public IndustryRepository IndustryRepository { get; set; }
        public InboundEmailRepository InboundEmailRepository { get; set; }
        public InboundEmailLineRepository InboundEmailLineRepository { get; set; }
    }
}