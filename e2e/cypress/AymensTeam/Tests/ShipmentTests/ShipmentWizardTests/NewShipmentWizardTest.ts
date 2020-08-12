import { Resolvers } from "../../../Resolvers/Resolvers";
import { ShipmentScenarios } from '../../../Scenarios/ShipmentScenarios';
import { NewShipmentWizardScenarios } from '../../../Scenarios/ShipmentScenarios/NewShipmentWizardScenarios';

import { LoginComp } from "../../../../Login/Login.po";

describe('Operations', () => {
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