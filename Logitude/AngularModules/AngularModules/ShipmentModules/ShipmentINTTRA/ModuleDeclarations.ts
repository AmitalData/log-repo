import {WizardComponent} from './Components/Wizard/WizardComponent';
import {SimulatorComponent} from './Components/Wizard/SimulatorComponent';
import {INTTRASettingsComponent} from './Components/Maintenance/INTTRASettingsComponent';
import {INTTRACommunicationSettingsComponent} from './Components/Maintenance/INTTRACommunicationSettingsComponent';
import { SimulatorBookingComponent } from './Components/Wizard/SimulatorBookingComponent';
import { SimulatorBookingLoadComponent } from './Components/Wizard/SimulatorBookingLoadComponent';

export const Components =
    [
        WizardComponent,
        SimulatorComponent,
        INTTRASettingsComponent,
        INTTRACommunicationSettingsComponent,
        SimulatorBookingComponent,
        SimulatorBookingLoadComponent
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "WizardComponent": { myResult = WizardComponent; break; }
            case "SimulatorComponent": { myResult = SimulatorComponent; break; }
            case "INTTRASettingsComponent": { myResult = INTTRASettingsComponent; break; }
            case "INTTRACommunicationSettingsComponent": { myResult = INTTRACommunicationSettingsComponent; break; }
            case "SimulatorBookingComponent": { myResult = SimulatorBookingComponent; break; }
            case "SimulatorBookingLoadComponent": { myResult = SimulatorBookingLoadComponent; break; }
        }
        return myResult;
    }
}
