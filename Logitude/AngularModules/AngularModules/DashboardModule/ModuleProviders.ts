import { AnalyticsFactsFieldsMetaDataListService } from './Services/StandardLists/AnalyticsFactsFieldsMetaDataListService';
import { AnalyticsFactsMetaDataListService } from './Services/StandardLists/AnalyticsFactsMetaDataListService';

export class ModuleProviders {
    public static GetInstance(name: string) {

        var myResult: any = null;

        switch (name) {
            case "AnalyticsFactsFieldsMetaDataListService": { myResult = new AnalyticsFactsFieldsMetaDataListService(); break; }
            case "AnalyticsFactsMetaDataListService": { myResult = new AnalyticsFactsMetaDataListService(); break; }
        }

        return myResult;
    }
}
