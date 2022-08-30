import { AnalyticsFactsFieldsMetaDataListService } from './Services/StandardLists/AnalyticsFactsFieldsMetaDataListService';
import { AnalyticsFactsMetaDataListService } from './Services/StandardLists/AnalyticsFactsMetaDataListService';
import { AnalyticsFactsFieldsMetaDataPMExtendedService } from './Services/ExtendedPMs/AnalyticsFactsFieldsMetaDataExtendedService';

export class ModuleProviders {
    public static GetInstance(name: string) {

        var myResult: any = null;

        switch (name) {
            case "AnalyticsFactsFieldsMetaDataListService": { myResult = new AnalyticsFactsFieldsMetaDataListService(); break; }
            case "AnalyticsFactsMetaDataListService": { myResult = new AnalyticsFactsMetaDataListService(); break; }
            case "AnalyticsFactsFieldsMetaDataPMExtendedService": { myResult = new AnalyticsFactsFieldsMetaDataPMExtendedService(); break; }
        }

        return myResult;
    }
}
