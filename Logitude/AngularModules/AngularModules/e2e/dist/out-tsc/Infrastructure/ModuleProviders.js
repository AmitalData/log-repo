"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var BusinessHoursHolidayListService_1 = require("./Services/StandardLists/BusinessHoursHolidayListService");
var ChargesGroupListService_1 = require("./Services/StandardLists/ChargesGroupListService");
var CustomPickListListService_1 = require("./Services/StandardLists/CustomPickListListService");
var DescriptionOfGoodsListService_1 = require("./Services/StandardLists/DescriptionOfGoodsListService");
var DirectionListService_1 = require("./Services/StandardLists/DirectionListService");
var EntityStatusListService_1 = require("./Services/StandardLists/EntityStatusListService");
var EventTypeCategoryListService_1 = require("./Services/StandardLists/EventTypeCategoryListService");
var EventTypeListService_1 = require("./Services/StandardLists/EventTypeListService");
var IATACodeListService_1 = require("./Services/StandardLists/IATACodeListService");
var MoveTypeListService_1 = require("./Services/StandardLists/MoveTypeListService");
var ObjectTableListService_1 = require("./Services/StandardLists/ObjectTableListService");
var PrepaidCollectListService_1 = require("./Services/StandardLists/PrepaidCollectListService");
var RatesTableListService_1 = require("./Services/StandardLists/RatesTableListService");
var ObjectFieldListService_1 = require("./Services/StandardLists/ObjectFieldListService");
//import {SharedLogisticsInvitationStatusListService} from './Services/StandardLists/SharedLogisticsInvitationStatusListService';
var TenantManagementListService_1 = require("./Services/StandardLists/TenantManagementListService");
var TransportModeListService_1 = require("./Services/StandardLists/TransportModeListService");
var VolumeUnitListService_1 = require("./Services/StandardLists/VolumeUnitListService");
var AdvancedQueryFiltersPMService_1 = require("./Services/StandardPMs/AdvancedQueryFiltersPMService");
var BusinessHoursHolidayPMService_1 = require("./Services/StandardPMs/BusinessHoursHolidayPMService");
var ChargesGroupPMService_1 = require("./Services/StandardPMs/ChargesGroupPMService");
var CustomPickListPMService_1 = require("./Services/StandardPMs/CustomPickListPMService");
var BluesnapContractPMService_1 = require("./Services/StandardPMs/BluesnapContractPMService");
//import {EntityStatusPMService} from './Services/StandardPMs/EntityStatusPMService';
//import {EventTypeCategoryPMService} from './Services/StandardPMs/EventTypeCategoryPMService';
var EventTypePMService_1 = require("./Services/StandardPMs/EventTypePMService");
var GeneralEntitiesService_1 = require("./Services/StandardPMs/GeneralEntitiesService");
//import {IATACodePMService} from './Services/StandardPMs/IATACodePMService';
var MoveTypePMService_1 = require("./Services/StandardPMs/MoveTypePMService");
var ObjectFieldPMService_1 = require("./Services/StandardPMs/ObjectFieldPMService");
//import {ObjectTablePMService} from './Services/StandardPMs/ObjectTablePMService';
//import {PrepaidCollectPMService} from './Services/StandardPMs/PrepaidCollectPMService';
var QueriesPMService_1 = require("./Services/StandardPMs/QueriesPMService");
var QueryColumnsPMService_1 = require("./Services/StandardPMs/QueryColumnsPMService");
var RatesTablePMService_1 = require("./Services/StandardPMs/RatesTablePMService");
//import {SharedLogisticsInvitationStatusPMService} from './Services/StandardPMs/SharedLogisticsInvitationStatusPMService';
var TenantManagementPMService_1 = require("./Services/StandardPMs/TenantManagementPMService");
var TextCodePMService_1 = require("./Services/StandardPMs/TextCodePMService");
var TraceEventPMService_1 = require("./Services/StandardPMs/TraceEventPMService");
//import {TransportModePMService} from './Services/StandardPMs/TransportModePMService';
//import {VolumeUnitPMService} from './Services/StandardPMs/VolumeUnitPMService';
var APILogsListService_1 = require("./Services/StandardLists/APILogsListService");
var APILogsPMService_1 = require("./Services/StandardPMs/APILogsPMService");
var EmailAlertSettingPMService_1 = require("./Services/ExtendedPMs/EmailAlertSettingPMService");
var BluesnapContractListService_1 = require("./Services/StandardLists/BluesnapContractListService");
var BluesnapContractTypeListService_1 = require("./Services/StandardLists/BluesnapContractTypeListService");
var BusinessHourListService_1 = require("./Services/StandardLists/BusinessHourListService");
var TenantTypeListService_1 = require("./Services/StandardLists/TenantTypeListService");
var PaymentChannelListService_1 = require("./Services/StandardLists/PaymentChannelListService");
var PaymentMethodListService_1 = require("./Services/StandardLists/PaymentMethodListService");
var RecurringPeriodListService_1 = require("./Services/StandardLists/RecurringPeriodListService");
var TenantManagmentPrivateLabelsListService_1 = require("./Services/StandardLists/TenantManagmentPrivateLabelsListService");
var PaymentCurrencyListService_1 = require("./Services/StandardLists/PaymentCurrencyListService");
var AWBMessagesCCSTypeListService_1 = require("./Services/StandardLists/AWBMessagesCCSTypeListService");
var AnalyzeQueueListService_1 = require("./Services/StandardLists/AnalyzeQueueListService");
var AnalyzeQueuePMService_1 = require("./Services/StandardPMs/AnalyzeQueuePMService");
var ApiCredintialsListService_1 = require("./Services/StandardLists/ApiCredintialsListService");
var ErrorLogPMFileLoggerService_1 = require("./Services/ExtendedPMs/ErrorLogPMFileLoggerService");
var BusinessRoleListService_1 = require("./Services/StandardLists/BusinessRoleListService");
var InboundEmailListService_1 = require("./Services/StandardLists/InboundEmailListService");
var InboundEmailPMService_1 = require("./Services/StandardPMs/InboundEmailPMService");
var ErrorLogListService_1 = require("./Services/StandardLists/ErrorLogListService");
var ErrorLogPMService_1 = require("./Services/StandardPMs/ErrorLogPMService");
var BusinessRolePMService_1 = require("./Services/StandardPMs/BusinessRolePMService");
var BatchTaskExecutionListService_1 = require("./Services/StandardLists/BatchTaskExecutionListService");
var BatchTaskExecutionPMService_1 = require("./Services/StandardPMs/BatchTaskExecutionPMService");
var AnalyzeQueueMenuButtonsHandler_1 = require("./Components/MenuButtons/AnalyzeQueueMenuButtonsHandler");
var TenantManagementMenuButtonsHandler_1 = require("./Components/MenuButtons/TenantManagementMenuButtonsHandler");
//
var BusinessProcessQueueListService_1 = require("./Services/StandardLists/BusinessProcessQueueListService");
var BusinessProcessQueuePMService_1 = require("./Services/StandardPMs/BusinessProcessQueuePMService");
var TeamListService_1 = require("./Services/StandardLists/TeamListService");
var TeamPMService_1 = require("./Services/StandardPMs/TeamPMService");
var DWQueryBuilderService_1 = require("./Services/ExtendedPMs/DWQueryBuilderService");
var BIReportFolderListService_1 = require("./Services/StandardLists/BIReportFolderListService");
var BIReportListService_1 = require("./Services/StandardLists/BIReportListService");
var BIReportPMService_1 = require("./Services/StandardPMs/BIReportPMService");
var BIReportsTypeListService_1 = require("./Services/StandardLists/BIReportsTypeListService");
var WebhookKeysListService_1 = require("./Services/StandardLists/WebhookKeysListService");
var ToggleListService_1 = require("./Services/StandardLists/ToggleListService");
var FeatureToggleListService_1 = require("./Services/StandardLists/FeatureToggleListService");
var FeatureTogglePMService_1 = require("./Services/StandardPMs/FeatureTogglePMService");
var TaskSchedulerHistoryListService_1 = require("./Services/StandardLists/TaskSchedulerHistoryListService");
var ModuleProviders = /** @class */ (function () {
    function ModuleProviders() {
    }
    ModuleProviders.GetInstance = function (name) {
        var myResult = null;
        switch (name) {
            case "BIReportPMService": {
                myResult = new BIReportPMService_1.BIReportPMService();
                break;
            }
            case "BIReportsTypeListService": {
                myResult = new BIReportsTypeListService_1.BIReportsTypeListService();
                break;
            }
            case "BIReportListService": {
                myResult = new BIReportListService_1.BIReportListService();
                break;
            }
            case "BIReportFolderListService": {
                myResult = new BIReportFolderListService_1.BIReportFolderListService();
                break;
            }
            case "BusinessHoursHolidayListService": {
                myResult = new BusinessHoursHolidayListService_1.BusinessHoursHolidayListService();
                break;
            }
            case "ChargesGroupListService": {
                myResult = new ChargesGroupListService_1.ChargesGroupListService();
                break;
            }
            case "CustomPickListListService": {
                myResult = new CustomPickListListService_1.CustomPickListListService();
                break;
            }
            case "DescriptionOfGoodsListService": {
                myResult = new DescriptionOfGoodsListService_1.DescriptionOfGoodsListService();
                break;
            }
            case "DirectionListService": {
                myResult = new DirectionListService_1.DirectionListService();
                break;
            }
            case "EntityStatusListService": {
                myResult = new EntityStatusListService_1.EntityStatusListService();
                break;
            }
            case "EventTypeCategoryListService": {
                myResult = new EventTypeCategoryListService_1.EventTypeCategoryListService();
                break;
            }
            case "EventTypeListService": {
                myResult = new EventTypeListService_1.EventTypeListService();
                break;
            }
            case "IATACodeListService": {
                myResult = new IATACodeListService_1.IATACodeListService();
                break;
            }
            case "MoveTypeListService": {
                myResult = new MoveTypeListService_1.MoveTypeListService();
                break;
            }
            case "ObjectTableListService": {
                myResult = new ObjectTableListService_1.ObjectTableListService();
                break;
            }
            case "PrepaidCollectListService": {
                myResult = new PrepaidCollectListService_1.PrepaidCollectListService();
                break;
            }
            case "RatesTableListService": {
                myResult = new RatesTableListService_1.RatesTableListService();
                break;
            }
            case "ObjectFieldListService": {
                myResult = new ObjectFieldListService_1.ObjectFieldListService();
                break;
            }
            //case "SharedLogisticsInvitationStatusListService": { myResult = new SharedLogisticsInvitationStatusListService(); break; }
            case "TenantManagementListService": {
                myResult = new TenantManagementListService_1.TenantManagementListService();
                break;
            }
            case "TransportModeListService": {
                myResult = new TransportModeListService_1.TransportModeListService();
                break;
            }
            case "VolumeUnitListService": {
                myResult = new VolumeUnitListService_1.VolumeUnitListService();
                break;
            }
            case "AdvancedQueryFiltersPMService": {
                myResult = new AdvancedQueryFiltersPMService_1.AdvancedQueryFiltersPMService();
                break;
            }
            case "BusinessHoursHolidayPMService": {
                myResult = new BusinessHoursHolidayPMService_1.BusinessHoursHolidayPMService();
                break;
            }
            case "ChargesGroupPMService": {
                myResult = new ChargesGroupPMService_1.ChargesGroupPMService();
                break;
            }
            case "CustomPickListPMService": {
                myResult = new CustomPickListPMService_1.CustomPickListPMService();
                break;
            }
            //case "EntityStatusPMService": { myResult = new EntityStatusPMService(); break; }
            //case "EventTypeCategoryPMService": { myResult = new EventTypeCategoryPMService(); break; }
            case "EventTypePMService": {
                myResult = new EventTypePMService_1.EventTypePMService();
                break;
            }
            case "GeneralEntitiesService": {
                myResult = new GeneralEntitiesService_1.GeneralEntitiesService();
                break;
            }
            //case "IATACodePMService": { myResult = new IATACodePMService(); break; }
            case "MoveTypePMService": {
                myResult = new MoveTypePMService_1.MoveTypePMService();
                break;
            }
            case "ObjectFieldPMService": {
                myResult = new ObjectFieldPMService_1.ObjectFieldPMService();
                break;
            }
            //case "ObjectTablePMService": { myResult = new ObjectTablePMService(); break; }
            //case "PrepaidCollectPMService": { myResult = new PrepaidCollectPMService(); break; }
            case "QueriesPMService": {
                myResult = new QueriesPMService_1.QueriesPMService();
                break;
            }
            case "QueryColumnsPMService": {
                myResult = new QueryColumnsPMService_1.QueryColumnsPMService();
                break;
            }
            case "RatesTablePMService": {
                myResult = new RatesTablePMService_1.RatesTablePMService();
                break;
            }
            //case "SharedLogisticsInvitationStatusPMService": { myResult = new SharedLogisticsInvitationStatusPMService(); break; }
            case "TenantManagementPMService": {
                myResult = new TenantManagementPMService_1.TenantManagementPMService();
                break;
            }
            case "TextCodePMService": {
                myResult = new TextCodePMService_1.TextCodePMService();
                break;
            }
            case "TraceEventPMService": {
                myResult = new TraceEventPMService_1.TraceEventPMService();
                break;
            }
            //case "TransportModePMService": { myResult = new TransportModePMService(); break; }
            //case "VolumeUnitPMService": { myResult = new VolumeUnitPMService(); break; }
            case "APILogsListService": {
                myResult = new APILogsListService_1.APILogsListService();
                break;
            }
            case "APILogsPMService": {
                myResult = new APILogsPMService_1.APILogsPMService();
                break;
            }
            case "EmailAlertSettingPMService": {
                myResult = new EmailAlertSettingPMService_1.EmailAlertSettingPMService();
                break;
            }
            case "BluesnapContractListService": {
                myResult = new BluesnapContractListService_1.BluesnapContractListService();
                break;
            }
            case "BluesnapContractTypeListService": {
                myResult = new BluesnapContractTypeListService_1.BluesnapContractTypeListService();
                break;
            }
            case "BluesnapContractPMService": {
                myResult = new BluesnapContractPMService_1.BluesnapContractPMService();
                break;
            }
            case "TenantTypeListService": {
                myResult = new TenantTypeListService_1.TenantTypeListService();
                break;
            }
            case "BusinessHourListService": {
                myResult = new BusinessHourListService_1.BusinessHourListService();
                break;
            }
            case "PaymentChannelListService": {
                myResult = new PaymentChannelListService_1.PaymentChannelListService();
                break;
            }
            case "InboundEmailListService": {
                myResult = new InboundEmailListService_1.InboundEmailListService();
                break;
            }
            case "InboundEmailPMService": {
                myResult = new InboundEmailPMService_1.InboundEmailPMService();
                break;
            }
            case "PaymentMethodListService": {
                myResult = new PaymentMethodListService_1.PaymentMethodListService();
                break;
            }
            case "RecurringPeriodListService": {
                myResult = new RecurringPeriodListService_1.RecurringPeriodListService();
                break;
            }
            case "TenantManagmentPrivateLabelsListService": {
                myResult = new TenantManagmentPrivateLabelsListService_1.TenantManagmentPrivateLabelsListService();
                break;
            }
            case "PaymentCurrencyListService": {
                myResult = new PaymentCurrencyListService_1.PaymentCurrencyListService();
                break;
            }
            case "AWBMessagesCCSTypeListService": {
                myResult = new AWBMessagesCCSTypeListService_1.AWBMessagesCCSTypeListService();
                break;
            }
            case "AnalyzeQueueListService": {
                myResult = new AnalyzeQueueListService_1.AnalyzeQueueListService();
                break;
            }
            case "AnalyzeQueuePMService": {
                myResult = new AnalyzeQueuePMService_1.AnalyzeQueuePMService();
                break;
            }
            case "ErrorLogListService": {
                myResult = new ErrorLogListService_1.ErrorLogListService();
                break;
            }
            case "BatchTaskExecutionListService": {
                myResult = new BatchTaskExecutionListService_1.BatchTaskExecutionListService();
                break;
            }
            case "ErrorLogPMService": {
                myResult = new ErrorLogPMService_1.ErrorLogPMService();
                break;
            }
            case "ApiCredintialsListService": {
                myResult = new ApiCredintialsListService_1.ApiCredintialsListService();
                break;
            }
            case "AnalyzeQueueMenuButtonsHandler": {
                myResult = new AnalyzeQueueMenuButtonsHandler_1.AnalyzeQueueMenuButtonsHandler();
                break;
            }
            case "TenantManagementMenuButtonsHandler": {
                myResult = new TenantManagementMenuButtonsHandler_1.TenantManagementMenuButtonsHandler();
                break;
            }
            case "ErrorLogPMFileLoggerService": {
                myResult = new ErrorLogPMFileLoggerService_1.ErrorLogPMFileLoggerService();
                break;
            }
            case "BusinessProcessQueueListService": {
                myResult = new BusinessProcessQueueListService_1.BusinessProcessQueueListService();
                break;
            }
            case "BusinessProcessQueuePMService": {
                myResult = new BusinessProcessQueuePMService_1.BusinessProcessQueuePMService();
                break;
            }
            case "BusinessRoleListService": {
                myResult = new BusinessRoleListService_1.BusinessRoleListService();
                break;
            }
            case "BusinessRolePMService": {
                myResult = new BusinessRolePMService_1.BusinessRolePMService();
                break;
            }
            case "TeamListService": {
                myResult = new TeamListService_1.TeamListService();
                break;
            }
            case "TeamPMService": {
                myResult = new TeamPMService_1.TeamPMService();
                break;
            }
            case "BatchTaskExecutionPMService": {
                myResult = new BatchTaskExecutionPMService_1.BatchTaskExecutionPMService();
                break;
            }
            case "DWQueryBuilderService": {
                myResult = new DWQueryBuilderService_1.DWQueryBuilderService();
                break;
            }
            case "WebhookKeysListService": {
                myResult = new WebhookKeysListService_1.WebhookKeysListService();
                break;
            }
            case "FeatureToggleListService": {
                myResult = new FeatureToggleListService_1.FeatureToggleListService();
                break;
            }
            case "FeatureTogglePMService": {
                myResult = new FeatureTogglePMService_1.FeatureTogglePMService();
                break;
            }
            case "ToggleListService": {
                myResult = new ToggleListService_1.ToggleListService();
                break;
            }
            case "TaskSchedulerHistoryListService": {
                myResult = new TaskSchedulerHistoryListService_1.TaskSchedulerHistoryListService();
                break;
            }
        }
        return myResult;
    };
    return ModuleProviders;
}());
exports.ModuleProviders = ModuleProviders;
//# sourceMappingURL=ModuleProviders.js.map