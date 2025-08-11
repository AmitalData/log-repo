import {BusinessHoursHolidayListService} from './Services/StandardLists/BusinessHoursHolidayListService';
import {ChargesGroupListService} from './Services/StandardLists/ChargesGroupListService';
import {CustomPickListListService} from './Services/StandardLists/CustomPickListListService';
import {DescriptionOfGoodsListService} from './Services/StandardLists/DescriptionOfGoodsListService';
import {DirectionListService} from './Services/StandardLists/DirectionListService';
import {EntityStatusListService} from './Services/StandardLists/EntityStatusListService';
import {EventTypeCategoryListService} from './Services/StandardLists/EventTypeCategoryListService';
import {EventTypeListService} from './Services/StandardLists/EventTypeListService';
import {IATACodeListService} from './Services/StandardLists/IATACodeListService';
import {MoveTypeListService} from './Services/StandardLists/MoveTypeListService';
import {ObjectTableListService} from './Services/StandardLists/ObjectTableListService';
import {PrepaidCollectListService} from './Services/StandardLists/PrepaidCollectListService';
import {RatesTableListService} from './Services/StandardLists/RatesTableListService';
import {ObjectFieldListService} from './Services/StandardLists/ObjectFieldListService';
//import {SharedLogisticsInvitationStatusListService} from './Services/StandardLists/SharedLogisticsInvitationStatusListService';
import {TenantManagementListService} from './Services/StandardLists/TenantManagementListService';
import {TransportModeListService} from './Services/StandardLists/TransportModeListService';
import {VolumeUnitListService} from './Services/StandardLists/VolumeUnitListService';
import {AdvancedQueryFiltersPMService} from './Services/StandardPMs/AdvancedQueryFiltersPMService';
import {BusinessHoursHolidayPMService} from './Services/StandardPMs/BusinessHoursHolidayPMService';
import {ChargesGroupPMService} from './Services/StandardPMs/ChargesGroupPMService';
import { CustomPickListPMService } from './Services/StandardPMs/CustomPickListPMService';
import { BluesnapContractPMService } from './Services/StandardPMs/BluesnapContractPMService';
import { QuoteChargesGroupListService } from './Services/StandardLists/QuoteChargesGroupListService';
import { QuoteChargesGroupPMService } from './Services/StandardPMs/QuoteChargesGroupPMService';

import {EntityStatusPMService} from './Services/StandardPMs/EntityStatusPMService';
//import {EventTypeCategoryPMService} from './Services/StandardPMs/EventTypeCategoryPMService';
import {EventTypePMService} from './Services/StandardPMs/EventTypePMService';
import {GeneralEntitiesService} from './Services/StandardPMs/GeneralEntitiesService';
//import {IATACodePMService} from './Services/StandardPMs/IATACodePMService';
import {MoveTypePMService} from './Services/StandardPMs/MoveTypePMService';
import {ObjectFieldPMService} from './Services/StandardPMs/ObjectFieldPMService';
//import {ObjectTablePMService} from './Services/StandardPMs/ObjectTablePMService';
//import {PrepaidCollectPMService} from './Services/StandardPMs/PrepaidCollectPMService';
import {QueriesPMService} from './Services/StandardPMs/QueriesPMService';
import {QueryColumnsPMService} from './Services/StandardPMs/QueryColumnsPMService';
import {RatesTablePMService} from './Services/StandardPMs/RatesTablePMService';
import {ScreenSectionPMService} from './Services/StandardPMs/ScreenSectionPMService';
//import {SharedLogisticsInvitationStatusPMService} from './Services/StandardPMs/SharedLogisticsInvitationStatusPMService';
import {TenantManagementPMService} from './Services/StandardPMs/TenantManagementPMService';
import {TextCodePMService} from './Services/StandardPMs/TextCodePMService';
import {TraceEventPMService} from './Services/StandardPMs/TraceEventPMService';
//import {TransportModePMService} from './Services/StandardPMs/TransportModePMService';
//import {VolumeUnitPMService} from './Services/StandardPMs/VolumeUnitPMService';
import {APILogsListService} from './Services/StandardLists/APILogsListService';
import {APILogsPMService} from './Services/StandardPMs/APILogsPMService';
import { EmailAlertSettingPMService } from './Services/ExtendedPMs/EmailAlertSettingPMService';
import { BluesnapContractListService } from './Services/StandardLists/BluesnapContractListService';
import { BluesnapContractTypeListService } from './Services/StandardLists/BluesnapContractTypeListService';
import {BusinessHourListService} from './Services/StandardLists/BusinessHourListService';
import {TenantTypeListService} from './Services/StandardLists/TenantTypeListService';
import {PaymentChannelListService} from './Services/StandardLists/PaymentChannelListService';
import {PaymentMethodListService} from './Services/StandardLists/PaymentMethodListService';
import {RecurringPeriodListService} from './Services/StandardLists/RecurringPeriodListService';
import {TenantManagmentPrivateLabelsListService} from './Services/StandardLists/TenantManagmentPrivateLabelsListService';
import {PaymentCurrencyListService} from './Services/StandardLists/PaymentCurrencyListService';
import {AWBMessagesCCSTypeListService} from './Services/StandardLists/AWBMessagesCCSTypeListService';
import {AnalyzeQueueListService} from './Services/StandardLists/AnalyzeQueueListService';
import {AnalyzeQueuePMService} from './Services/StandardPMs/AnalyzeQueuePMService';
import {ApiCredintialsListService} from './Services/StandardLists/ApiCredintialsListService';
import { ErrorLogPMFileLoggerService } from './Services/ExtendedPMs/ErrorLogPMFileLoggerService';
import {BusinessRoleListService} from './Services/StandardLists/BusinessRoleListService';
import {InboundEmailListService} from './Services/StandardLists/InboundEmailListService';
import {InboundEmailPMService} from './Services/StandardPMs/InboundEmailPMService';
import {ErrorLogListService} from './Services/StandardLists/ErrorLogListService';
import {ErrorLogPMService} from './Services/StandardPMs/ErrorLogPMService';
import {BusinessRolePMService} from './Services/StandardPMs/BusinessRolePMService';
import { BatchTaskExecutionListService } from './Services/StandardLists/BatchTaskExecutionListService';
import { BatchTaskExecutionPMService } from './Services/StandardPMs/BatchTaskExecutionPMService';

import { AnalyzeQueueMenuButtonsHandler } from './Components/MenuButtons/AnalyzeQueueMenuButtonsHandler';
import { BatchTaskExecutionMenuButtonsHandler } from './Components/MenuButtons/BatchTaskExecutionMenuButtonsHandler';
import {TenantManagementMenuButtonsHandler} from './Components/MenuButtons/TenantManagementMenuButtonsHandler';
import {DeploymentPackageMenuButtonsHandler} from './Components/MenuButtons/DeploymentPackageMenuButtonsHandler';
//
import {BusinessProcessQueueListService} from  './Services/StandardLists/BusinessProcessQueueListService';
import {BusinessProcessQueuePMService} from  './Services/StandardPMs/BusinessProcessQueuePMService';
import {TeamListService} from  './Services/StandardLists/TeamListService';
import {TeamPMService} from './Services/StandardPMs/TeamPMService';
import { DWQueryBuilderService } from './Services/ExtendedPMs/DWQueryBuilderService';

import { BIReportFolderListService } from './Services/StandardLists/BIReportFolderListService';
import { BIReportListService } from './Services/StandardLists/BIReportListService';
import { BIReportPMService } from './Services/StandardPMs/BIReportPMService';
import { BIReportsTypeListService } from './Services/StandardLists/BIReportsTypeListService';
import { BIReportExtendedListService } from './Services/ExtendedLists/BIReportExtendedListService';
import { LastRunDetailExtendedPMService } from './Services/ExtendedPMs/LastRunDetailExtendedPMService';
import { DWObjectTableExtendedListService } from './Services/ExtendedLists/DWObjectTableExtendedListService';
import { WebhookKeysListService } from './Services/StandardLists/WebhookKeysListService';
import { BIReportFolderExtendedListService } from './Services/ExtendedLists/BIReportFolderExtendedListService';
import { AdditionalCurrencyRateListService } from './Services/StandardLists/AdditionalCurrencyRateListService';
import { AdditionalCurrencyRatePMService } from './Services/StandardPMs/AdditionalCurrencyRatePMService';

import { ToggleListService } from './Services/StandardLists/ToggleListService';
import { FeatureToggleListService } from './Services/StandardLists/FeatureToggleListService';
import { FeatureTogglePMService } from './Services/StandardPMs/FeatureTogglePMService';
import { TaskSchedulerHistoryListService } from './Services/StandardLists/TaskSchedulerHistoryListService';
import { SchedulerProcedureListService } from './Services/StandardLists/SchedulerProcedureListService';
import { TasksSchedulerListService } from './Services/StandardLists/TasksSchedulerListService';
import { BluesnapTransactionListService } from './Services/StandardLists/BluesnapTransactionListService';
import { PriceStepListService } from './Services/StandardLists/PriceStepListService';
import { PriceStepPMService } from './Services/StandardPMs/PriceStepPMService';
import { HelpResourceListService } from './Services/StandardLists/HelpResourceListService';
import { HelpResourcePMService } from './Services/StandardPMs/HelpResourcePMService';
import { MultiEntityUpdateLogPMService } from './Services/StandardPMs/MultiEntityUpdateLogPMService';
import { EntityStatusTypeListService } from './Services/StandardLists/EntityStatusTypeListService';

import { ImageLibraryListService } from './Services/StandardLists/ImageLibraryListService';
import { ImageLibraryPMService } from './Services/StandardPMs/ImageLibraryPMService';
import { TableTabService } from './Services/ExtendedPMs/TableTabService';
import { ScreenSectionListService } from './Services/StandardLists/ScreenSectionListService';
import { ScreenListService } from './Services/StandardLists/ScreenListService';
import { ScreenPMService } from './Services/StandardPMs/ScreenPMService';
import { DigitalPortalScreenListService } from './Services/StandardLists/DigitalPortalScreenListService';
import { DigitalPreDefinedComponentListService } from './Services/StandardLists/DigitalPreDefinedComponentListService';
import { DeploymentPackagePMService } from './Services/StandardPMs/DeploymentPackagePMService';
import { DeploymentPackageListService } from './Services/StandardLists/DeploymentPackageListService';
import { CustomFieldsMainObjectPMService } from './Services/StandardPMs/CustomFieldsMainObjectsPMService';
import { CustomFieldsMainObjectListService } from './Services/StandardLists/CustomFieldsMainObjectListService';
import { DigitalCustomizationService } from './Services/WebServices/DigitalCustomizationService';
import { ReferenceCustomObjectPMService } from './Services/StandardPMs/ReferenceCustomObjectPMService';
import { ReferenceCustomObjectListService } from './Services/StandardLists/ReferenceCustomObjectListService';
import { DataCustomObjectPMService } from './Services/StandardPMs/DataCustomObjectPMService';
import {ObjectTablePMExtendedService} from './Services/ExtendedPMs/ObjectTablePMExtendedService';
import { DataCustomObjectListService } from './Services/StandardLists/DataCustomObjectListService';
import { ReferantTeamListService } from '../Customs/Services/StandardLists/ReferantTeamListService';
import { DeploymentPackageExecutionLogPM } from './EntityPMs/DeploymentPackageExecutionLogPM';
import { DeploymentPackageExecutionLogList } from './EntityLists/DeploymentPackageExecutionLogList';
import { DeploymentPackageExecutionLogPMService } from './Services/StandardPMs/DeploymentPackageExecutionLogPMService';
import { DeploymentPackageExecutionLogListService } from './Services/StandardLists/DeploymentPackageExecutionLogListService';
import { DeploymentPackageExecutionLogListExtendedService } from './Services/ExtendedLists/DeploymentPackageExecutionLogListExtendedService';
import { SchedulerReportMessageTemplateService } from './Components/StimulsoftComponent/Services/SchedulerReportMessageTemplateService';
import { DefaultAndConfigurationListService } from './Services/StandardLists/DefaultAndConfigurationListService';
import { DefaultAndConfigurationPMService } from './Services/StandardPMs/DefaultAndConfigurationPMService';
import { DefaultAndConfigurationKeyListService } from './Services/StandardLists/DefaultAndConfigurationKeyListService';
import { GeneralLockListService } from './Services/StandardLists/GeneralLockListService';
import { ReportService } from 'Common/Services/ExtendedLists/ReportService';

export class ModuleProviders {

    public static GetInstance(name: string) {

        var myResult: any = null;

        switch (name) {
            case "BIReportPMService": { myResult = new BIReportPMService(); break; }
            case "BIReportsTypeListService": { myResult = new BIReportsTypeListService(); break; }
            case "BIReportListService": { myResult = new BIReportListService(); break; }
            case "BIReportFolderListService": { myResult = new BIReportFolderListService(); break; }
            case "BusinessHoursHolidayListService": { myResult = new BusinessHoursHolidayListService(); break; }
            case "ChargesGroupListService": { myResult = new ChargesGroupListService(); break; }
            case "CustomPickListListService": { myResult = new CustomPickListListService(); break; }
            case "DescriptionOfGoodsListService": { myResult = new DescriptionOfGoodsListService(); break; }
            case "DirectionListService": { myResult = new DirectionListService(); break; }
            case "EntityStatusListService": { myResult = new EntityStatusListService(); break; }
            case "EventTypeCategoryListService": { myResult = new EventTypeCategoryListService(); break; }
            case "EventTypeListService": { myResult = new EventTypeListService(); break; }
            case "IATACodeListService": { myResult = new IATACodeListService(); break; }
            case "MoveTypeListService": { myResult = new MoveTypeListService(); break; }
            case "ObjectTableListService": { myResult = new ObjectTableListService(); break; }
            case "PrepaidCollectListService": { myResult = new PrepaidCollectListService(); break; }
            case "RatesTableListService": { myResult = new RatesTableListService(); break; }
            case "ObjectFieldListService": { myResult = new ObjectFieldListService(); break; }
            case "QuoteChargesGroupListService": { myResult = new QuoteChargesGroupListService(); break; }
            case "QuoteChargesGroupPMService": { myResult = new QuoteChargesGroupPMService(); break; }

            //case "SharedLogisticsInvitationStatusListService": { myResult = new SharedLogisticsInvitationStatusListService(); break; }
            case "TenantManagementListService": { myResult = new TenantManagementListService(); break; }
            case "TransportModeListService": { myResult = new TransportModeListService(); break; }
            case "VolumeUnitListService": { myResult = new VolumeUnitListService(); break; }
            case "AdvancedQueryFiltersPMService": { myResult = new AdvancedQueryFiltersPMService(); break; }
            case "BusinessHoursHolidayPMService": { myResult = new BusinessHoursHolidayPMService(); break; }
            case "ChargesGroupPMService": { myResult = new ChargesGroupPMService(); break; }
            case "CustomPickListPMService": { myResult = new CustomPickListPMService(); break; }
            case "EntityStatusPMService": { myResult = new EntityStatusPMService(); break; }
            //case "EventTypeCategoryPMService": { myResult = new EventTypeCategoryPMService(); break; }
            case "EventTypePMService": { myResult = new EventTypePMService(); break; }
            case "GeneralEntitiesService": { myResult = new GeneralEntitiesService(); break; }
            //case "IATACodePMService": { myResult = new IATACodePMService(); break; }
            case "MoveTypePMService": { myResult = new MoveTypePMService(); break; }
            case "ObjectFieldPMService": { myResult = new ObjectFieldPMService(); break; }
            //case "ObjectTablePMService": { myResult = new ObjectTablePMService(); break; }
            //case "PrepaidCollectPMService": { myResult = new PrepaidCollectPMService(); break; }
            case "QueriesPMService": { myResult = new QueriesPMService(); break; }
            case "QueryColumnsPMService": { myResult = new QueryColumnsPMService(); break; }
            case "RatesTablePMService": { myResult = new RatesTablePMService(); break; }
            case "ScreenSectionPMService": { myResult = new ScreenSectionPMService(); break; }
            //case "SharedLogisticsInvitationStatusPMService": { myResult = new SharedLogisticsInvitationStatusPMService(); break; }
            case "TenantManagementPMService": { myResult = new TenantManagementPMService(); break; }
            case "TextCodePMService": { myResult = new TextCodePMService(); break; }
            case "TraceEventPMService": { myResult = new TraceEventPMService(); break; }
            //case "TransportModePMService": { myResult = new TransportModePMService(); break; }
            //case "VolumeUnitPMService": { myResult = new VolumeUnitPMService(); break; }
            case "APILogsListService": { myResult = new APILogsListService(); break; }
            case "APILogsPMService": { myResult = new APILogsPMService(); break; }
            case "EmailAlertSettingPMService": { myResult = new EmailAlertSettingPMService(); break; }
            case "BluesnapContractListService": { myResult = new BluesnapContractListService(); break; }
            case "BluesnapContractTypeListService": { myResult = new BluesnapContractTypeListService(); break; }
            case "BluesnapContractPMService": { myResult = new BluesnapContractPMService(); break; }  
            case "ObjectTablePMExtendedService": { myResult = new ObjectTablePMExtendedService(); break; }   
            case "ReportService": { myResult = new ReportService(); break; }

            case "TenantTypeListService": { myResult = new TenantTypeListService(); break; }
            case "BusinessHourListService": { myResult = new BusinessHourListService(); break; }
            case "PaymentChannelListService": { myResult = new PaymentChannelListService(); break; }
            case "InboundEmailListService": { myResult = new InboundEmailListService(); break; }
            case "InboundEmailPMService": { myResult = new InboundEmailPMService(); break; }
            case "PaymentMethodListService": { myResult = new PaymentMethodListService(); break; }
            case "RecurringPeriodListService": { myResult = new RecurringPeriodListService(); break; }
            case "TenantManagmentPrivateLabelsListService": { myResult = new TenantManagmentPrivateLabelsListService(); break; }
            case "PaymentCurrencyListService": { myResult = new PaymentCurrencyListService(); break; }
            case "AWBMessagesCCSTypeListService": { myResult = new AWBMessagesCCSTypeListService(); break; }
            case "AnalyzeQueueListService": { myResult = new AnalyzeQueueListService(); break; }
            case "AnalyzeQueuePMService": { myResult = new AnalyzeQueuePMService(); break; }
            case "ErrorLogListService": { myResult = new ErrorLogListService(); break; }
            case "BatchTaskExecutionListService": { myResult = new BatchTaskExecutionListService(); break; }
            case "ErrorLogPMService": { myResult = new ErrorLogPMService(); break; }
            case "ApiCredintialsListService": { myResult = new ApiCredintialsListService(); break; }
            case "AnalyzeQueueMenuButtonsHandler": { myResult = new AnalyzeQueueMenuButtonsHandler(); break; }
            case "BatchTaskExecutionMenuButtonsHandler": { myResult = new BatchTaskExecutionMenuButtonsHandler(); break; }
            case "TenantManagementMenuButtonsHandler": { myResult = new TenantManagementMenuButtonsHandler(); break; }
            case "DeploymentPackageMenuButtonsHandler": { myResult = new DeploymentPackageMenuButtonsHandler(); break; }
            case "ErrorLogPMFileLoggerService": { myResult = new ErrorLogPMFileLoggerService(); break; }
            case "BusinessProcessQueueListService": { myResult = new BusinessProcessQueueListService(); break; }
            case "BusinessProcessQueuePMService": { myResult = new BusinessProcessQueuePMService(); break; }
            case "BusinessRoleListService": { myResult = new BusinessRoleListService(); break; }
            case "BIReportExtendedListService": { myResult = new BIReportExtendedListService(); break; }
            case "LastRunDetailExtendedPMService": { myResult = new LastRunDetailExtendedPMService(); break; }
            case "AdditionalCurrencyRatePMService": { myResult = new AdditionalCurrencyRatePMService(); break; }
            case "DWObjectTableExtendedListService": { myResult = new DWObjectTableExtendedListService(); break; }
            case "BusinessRolePMService": { myResult = new BusinessRolePMService(); break; }
            case "TeamListService": { myResult = new TeamListService(); break; }
            case "TeamPMService": { myResult = new TeamPMService(); break; }
            case "BatchTaskExecutionPMService": { myResult = new BatchTaskExecutionPMService(); break; }
            case "DWQueryBuilderService": { myResult = new DWQueryBuilderService(); break; }
            case "WebhookKeysListService": { myResult = new WebhookKeysListService(); break; }
            case "FeatureToggleListService": { myResult = new FeatureToggleListService(); break; }
            case "FeatureTogglePMService": { myResult = new FeatureTogglePMService(); break; }
            case "ToggleListService": { myResult = new ToggleListService(); break; }
            case "TaskSchedulerHistoryListService": { myResult = new TaskSchedulerHistoryListService(); break; }
            case "SchedulerProcedureListService": { myResult = new SchedulerProcedureListService(); break; }
            case "TasksSchedulerListService": { myResult = new TasksSchedulerListService(); break; }
            case "BluesnapTransactionListService": { myResult = new BluesnapTransactionListService(); break; }
            case "PriceStepListService": { myResult = new PriceStepListService(); break; }
            case "PriceStepPMService": { myResult = new PriceStepPMService(); break; }
            case "HelpResourceListService": { myResult = new HelpResourceListService(); break; }
            case "HelpResourcePMService": { myResult = new HelpResourcePMService(); break; }
            case "BIReportFolderExtendedListService": { myResult = new BIReportFolderExtendedListService(); break; }
            case "AdditionalCurrencyRateListService": { myResult = new AdditionalCurrencyRateListService(); break; }
            case "MultiEntityUpdateLogPMService": { myResult = new MultiEntityUpdateLogPMService(); break; }
            case "EntityStatusTypeListService": { myResult = new EntityStatusTypeListService(); break; }
            case "ImageLibraryListService": { myResult = new ImageLibraryListService(); break; }
            case "ImageLibraryPMService": { myResult = new ImageLibraryPMService(); break; }
            case "TableTabService": { myResult = new TableTabService(); break; }
            case "ScreenSectionListService": { myResult = new ScreenSectionListService(); break; }
            case "ScreenListService": { myResult = new ScreenListService(); break; }
            case "ScreenPMService": { myResult = new ScreenPMService(); break; }
            case "DigitalPortalScreenListService": { myResult = new DigitalPortalScreenListService(); break; }
            case "DigitalPreDefinedComponentListService": { myResult = new DigitalPreDefinedComponentListService(); break; }
            case "DeploymentPackagePMService": { myResult = new DeploymentPackagePMService(); break; }
            case "DeploymentPackageListService": { myResult = new DeploymentPackageListService(); break; }
            case "CustomFieldsMainObjectPMService": { myResult = new CustomFieldsMainObjectPMService(); break; }
            case "CustomFieldsMainObjectListService": { myResult = new CustomFieldsMainObjectListService(); break; }
            case "DigitalCustomizationService": { myResult = new DigitalCustomizationService(); break; }
            case "DataCustomObjectPMService": { myResult = new DataCustomObjectPMService(); break; }
            case "DataCustomObjectListService": { myResult = new DataCustomObjectListService(); break; }
            case "ReferenceCustomObjectPMService": { myResult = new ReferenceCustomObjectPMService(); break; }
            case "ReferenceCustomObjectListService": { myResult = new ReferenceCustomObjectListService(); break; }
            case "DeploymentPackageExecutionLogPMService": { myResult = new DeploymentPackageExecutionLogPMService(); break; }
            case "DeploymentPackageExecutionLogListService": { myResult = new DeploymentPackageExecutionLogListService(); break; }
            case "DeploymentPackageExecutionLogListExtendedService": { myResult = new DeploymentPackageExecutionLogListExtendedService(); break; }
            case "SchedulerReportMessageTemplateService": { myResult = new SchedulerReportMessageTemplateService(); break;}
            case "DefaultAndConfigurationListService": { myResult = new DefaultAndConfigurationListService(); break; }
            case "DefaultAndConfigurationPMService": { myResult = new DefaultAndConfigurationPMService(); break; }
            case "DefaultAndConfigurationKeyListService": { myResult = new DefaultAndConfigurationKeyListService(); break; }
            case "GeneralLockListService": { myResult = new GeneralLockListService(); break; }

        }

        return myResult;
    }
}
