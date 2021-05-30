import {CustomsWizardComponent} from './Components/CustomsWizard/CustomsWizardComponent';
import {ArtemusWizardComponent} from './Components/ArtemusWizard/ArtemusWizardComponent';
import {SentToCustomComponent} from './Components/SentToCustomComponent/SentToCustomComponent';
import {SentToCustomLinkComponent} from './Components/SentToCustomComponent/SentToCustomLinkComponent'; 
import { ContainersStatusesSimulatorComponent } from './Components/ShipmentContainersStatuses/ContainersStatusesSimulatorComponent';

export const Components =
    [
        CustomsWizardComponent,
        SentToCustomComponent,
        SentToCustomLinkComponent,
        ArtemusWizardComponent,
        ContainersStatusesSimulatorComponent,
    ];


export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "CustomsWizardComponent": { myResult = CustomsWizardComponent; break; }
            case "SentToCustomComponent": { myResult = SentToCustomComponent; break; }
            case "SentToCustomLinkComponent": { myResult = SentToCustomLinkComponent; break; }
            case "ArtemusWizardComponent": { myResult = ArtemusWizardComponent; break; }
            case "ContainersStatusesSimulatorComponent": { myResult = ContainersStatusesSimulatorComponent; break; }
        }

        return myResult;
    }
}
