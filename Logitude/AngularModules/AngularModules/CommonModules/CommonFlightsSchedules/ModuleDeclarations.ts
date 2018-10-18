import {FVASimulatorComponent} from './Components/FlightsSchedules/FVASimulatorComponent';
import {FlightsSchedulesComponent} from './Components/FlightsSchedules/FlightsSchedulesComponent';
import {XMLFlightsSimulatorComponent} from './Components/FlightsSchedules/XMLFlightsSimulatorComponent';

export const Components =
    [
        FVASimulatorComponent,
        FlightsSchedulesComponent,
        XMLFlightsSimulatorComponent,
    ];


export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "FVASimulatorComponent": { myResult = FVASimulatorComponent; break; }
            case "FlightsSchedulesComponent": { myResult = FlightsSchedulesComponent; break; }
            case "XMLFlightsSimulatorComponent": { myResult = XMLFlightsSimulatorComponent; break; }
        }

        return myResult;
    }
}