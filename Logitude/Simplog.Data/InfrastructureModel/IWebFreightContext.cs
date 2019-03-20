using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

//using WebFreight.Web.QuoteModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel
{
    public interface IWebFreightContext : IContext
    {


        IDbSet<CounterLastNumber> CounterLastNumbers { get; }
        IDbSet<TransportMode> TransportModes { get; }
        IDbSet<Direction> Directions { get; }
        IDbSet<PartnerType> PartnerTypes { get; }
        IDbSet<PrepaidCollect> PrepaidCollects { get; }
        IDbSet<MoveType> MoveTypes { get; }
        IDbSet<SpecialService> SpecialServices { get; }
        IDbSet<FollowUp> FollowUps { get; }
        IDbSet<ValidationType> ValidationTypes { get; }
        IDbSet<FieldDataType> FieldDataTypes { get; }
        IDbSet<Translation> Translations { get; }
        IDbSet<TranslationHeader> TranslationHeaders { get; }
        IDbSet<TextCode> TextCodes { get; }
        IDbSet<ObjectTable> ObjectTables { get; }
        IDbSet<ObjectField> ObjectFields { get; }
        IDbSet<Screen> Screens { get; }
        IDbSet<ScreenField> ScreenFields { get; }
        IDbSet<TextCodeType> TextCodeTypes { get; }
        IDbSet<RatesTable> RatesTable { get; }
        IDbSet<EventType> EventType { get; }
        IDbSet<TraceEvent> TraceEvent { get; }
        IDbSet<Rank> Ranks { get; }
        IDbSet<Document> Documents { get; }
        IDbSet<Query> Queries { get; }
        IDbSet<AdvancedQueryFilter> AdvancedQueryFilters { get; }
        IDbSet<QueryColumn> QueryColumns { get; }
        IDbSet<DBIdCounter> DBIdCounters { get; }
        IDbSet<MenusTable> MenusTables { get; }
        IDbSet<MenuType> MenusTypes { get; }
        IDbSet<CategoryType> CategoryTypes { get; }
        IDbSet<CustomTable> CustomTables { get; }
        IDbSet<ObjectTableTab> ObjectTableTabs { get; }
        IDbSet<ObjectTableHelperControl> ObjectTableHelperControls { get; }
        IDbSet<GeneralLock> GeneralLocks { get; }
        IDbSet<IATACode> IATACodes { get; }
        IDbSet<ChargesGroup> ChargesGroups { get; }
        IDbSet<VolumeUnit> VolumeUnits { get; }
        IDbSet<EntityStatus> EntityStatus { get; }
        IDbSet<DescriptionOfGoods> DescriptionOfGoods { get; }
        IDbSet<MenuButton> MenuButtons { get; }
        IDbSet<MenuButtonGroup> MenuButtonGroups { get; }
        IDbSet<EntityLastActivity> EntityLastActivities { get; }
        IDbSet<CounterDefinition> CounterDefinitions { get; }
        IDbSet<CounterStat> CounterStats { get; }
        IDbSet<ObjectFieldValidation> ObjectFieldValidations { get; }
        IDbSet<RuleType> RuleTypes { get; }
        IDbSet<ObjectTableRule> ObjectTableRules { get; }
        IDbSet<ObjectTableRuleField> ObjectTableRuleFields { get; }
        IDbSet<QueryGroup> QueryGroups { get; }
        IDbSet<TriggerType> TriggerTypes { get; }
        IDbSet<RuleNotificationType> RuleNotificationTypes { get; }
        IDbSet<Counter> Counters { get; }
        IDbSet<TenantSetting> TenantSettings { get; }
        IDbSet<Tip> Tips { get; }
        IDbSet<TipsVisibility> TipsVisibilities { get; }
        IDbSet<ObjectFieldModification> ObjectFieldModifications { get; }
        IDbSet<ScreenModification> ScreenModifications { get; }
        IDbSet<ImageDetail> ImageDetails { get; }
        IDbSet<ImageLibrary> ImageLibrarys { get; }
        IDbSet<PermissionType> PermissionTypes { get; }
        IDbSet<ObjectTableType> ObjectTableTypes { get; }
        IDbSet<RuleConditionField> RuleConditionFields { get; }
        IDbSet<CustomPickList> CustomPickLists { get; }
        IDbSet<SharedLogisticsUpdate> SharedLogisticsUpdates { get; }
        IDbSet<SharedLogisticsUpdateStatus> SharedLogisticsUpdateStatus { get; }
        IDbSet<EntityLastUpdate> EntityLastUpdates { get; }
        IDbSet<EntityLastActivityType> EntityLastActivityTypes { get; }
        IDbSet<EventTypeCategory> EventTypeCategories { get; }
        IDbSet<EmailAlertSetting> EmailAlertSettings { get; }
        IDbSet<SharedLogisticsInvitationStatus> SharedLogisticsInvitationStatus { get; set; }
        IDbSet<ObjectTableLastUpdate> ObjectTableLastUpdates { get; }
        IDbSet<InboundEmail> InboundEmails { get; }
        IDbSet<InboundEmailLine> InboundEmailLines { get; }
        IDbSet<QueueDefinition> QueueDefinitions { get; }
        IDbSet<QueueMessage> QueueMessages { get; }
        IDbSet<BusinessHour> BusinessHours { get; }
        IDbSet<BusinessHoursHoliday> BusinessHoursHolidays { get; }
        IDbSet<APILogs> APILogs { get; }
        IDbSet<APILogsData> APILogsData { get; }
        IDbSet<QueueMessageMoreDetails> QueueMessageMoreDetails { get; }
        IDbSet<TasksScheduler> TasksSchedulers { get; }
        IDbSet<TaskSchedulerHistory> TaskSchedulerHistories { get; }
        IDbSet<DWObjectTable> DWObjectTables { get; }
        IDbSet<DWObjectField> DWObjectFields { get; }
        IDbSet<DWQuery> DWQueries { get;}
        IDbSet<DWSubQuery> DWSubQueries { get; }
        IDbSet<DWQueryColumn> DWQueryColumns { get; }
        IDbSet<DWQueryFilter> DWQueryFilters { get; }
        IDbSet<SharedUserQuery> SharedUserQueries { get; }
        IDbSet<DWCategories> DWCategories { get; }
        IDbSet<DWObjectFieldCategories> DWObjectFieldCategories { get; }
        IDbSet<SchedulerLogs> SchedulerLogs { get; }


        void SetAsModified(object entity);
        void DetectChanges();
        int SaveChanges();
    }
}
