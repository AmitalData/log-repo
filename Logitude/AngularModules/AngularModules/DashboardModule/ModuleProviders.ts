import { AnalyticsFactsFieldsMetaDataListService } from './Services/StandardLists/AnalyticsFactsFieldsMetaDataListService';
import { AnalyticsFactsMetaDataListService } from './Services/StandardLists/AnalyticsFactsMetaDataListService';
import { AnalyticsFactsFieldsMetaDataPMExtendedService } from './Services/ExtendedPMs/AnalyticsFactsFieldsMetaDataExtendedService';
import { MeasureTypeListService } from './Services/StandardLists/MeasureTypeListService';
import { WidgetTypeListService } from './Services/StandardLists/WidgetTypeListService';
import { DashboardListService } from 'DashboardModule/Services/StandardLists/DashboardListService';
import { DashboardAnalyticsService } from './Services/DashboardAnalyticsService';

export class ModuleProviders {
    public static GetInstance(name: string) {

        var myResult: any = null;

        switch (name) {
            case "AnalyticsFactsFieldsMetaDataListService": { myResult = new AnalyticsFactsFieldsMetaDataListService(); break; }
            case "AnalyticsFactsMetaDataListService": { myResult = new AnalyticsFactsMetaDataListService(); break; }
            case "AnalyticsFactsFieldsMetaDataPMExtendedService": { myResult = new AnalyticsFactsFieldsMetaDataPMExtendedService(); break; }
            case "MeasureTypeListService": { myResult = new MeasureTypeListService(); break; }
            case "WidgetTypeListService": { myResult = new WidgetTypeListService(); break; }
            case "DashboardListService": { myResult = new DashboardListService(); break; }
            case "DashboardAnalyticsService": { myResult = new DashboardAnalyticsService(); break; }
        }

        return myResult;
    }
}
