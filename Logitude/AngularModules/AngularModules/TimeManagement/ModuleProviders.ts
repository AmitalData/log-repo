import {TMEmployeeTimeListService} from './Services/StandardLists/TMEmployeeTimeListService';
import {TMProjectListService} from './Services/StandardLists/TMProjectListService';
import {TMLocationListService} from './Services/StandardLists/TMLocationListService';
import {TMOfficeHourListService} from './Services/StandardLists/TMOfficeHourListService';
import {TMBudgetListService} from './Services/StandardLists/TMBudgetListService';
import {TMProjectCategoryListService} from './Services/StandardLists/TMProjectCategoryListService';

import {TMEmployeeTimePMService} from './Services/StandardPMs/TMEmployeeTimePMService';
import {TMOfficeHourPMService} from './Services/StandardPMs/TMOfficeHourPMService';
import {TMProjectPMService} from './Services/StandardPMs/TMProjectPMService';
import {TMBudgetPMService} from './Services/StandardPMs/TMBudgetPMService';
import {TMProjectCategoryPMService} from './Services/StandardPMs/TMProjectCategoryPMService';
import {SprintListService} from './Services/StandardLists/SprintListService';
import {SprintPMService} from './Services/StandardPMs/SprintPMService';






export class ModuleProviders {
    public static GetInstance(name: string) {
        var myResult: any = null;
        switch (name) {
            case "TMEmployeeTimeListService": { myResult = new TMEmployeeTimeListService(); break; }
            case "TMProjectListService": { myResult = new TMProjectListService(); break; }
            case "TMLocationListService": { myResult = new TMLocationListService(); break; }
            case "TMOfficeHourListService": { myResult = new TMOfficeHourListService(); break; }
            case "TMBudgetListService": { myResult = new TMBudgetListService(); break; }
            case "TMProjectCategoryListService": { myResult = new TMProjectCategoryListService(); break; }

            case "TMEmployeeTimePMService": { myResult = new TMEmployeeTimePMService(); break; }
            case "TMOfficeHourPMService": { myResult = new TMOfficeHourPMService(); break; }
            case "TMProjectPMService": { myResult = new TMProjectPMService(); break; }
            case "TMBudgetPMService": { myResult = new TMBudgetPMService(); break; }
            case "TMProjectCategoryPMService": { myResult = new TMProjectCategoryPMService(); break; }

            case "SprintListService": { myResult = new SprintListService(); break; }
            case "SprintPMService": { myResult = new SprintPMService(); break; }

        }
        return myResult;
    }
}
