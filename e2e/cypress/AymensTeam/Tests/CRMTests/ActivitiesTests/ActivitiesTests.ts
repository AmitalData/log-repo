import { Resolvers } from "../../../Resolvers/Resolvers";
import { ShipmentScenarios } from '../../../Scenarios/ShipmentScenarios';
import { NewActivitiesScenarios } from '../../../Scenarios/CRMScenarios/NewActivitiesScenarios';

import { LoginComp } from "../../../../Login/Login.po";

describe('Activities', () => {
    let login: LoginComp = new LoginComp();
    let scenarios: NewShipmentWizardScenarios = new NewShipmentWizardScenarios();
    //let scenarios: ShipmentScenarios = new ShipmentScenarios();


    beforeEach(() => {

        Resolvers.MainMenuResolver.Selector('#GeneralMHOperations').Select();
        Resolvers.MainMenuResolver.Selector('#SHIP').Select();

    });

    it('Test New Shipment Wizard', () => {
        scenarios.RunScenario('D', 'A', 'E');

        //scenarios.CreateWizardShipment('D', 'E', 'A');

        //scenarios.CreateWizardShipment('D', 'E', 'A');
    });
});