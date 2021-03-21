import { Resolvers } from "../../../Resolvers/Resolvers";
import { NewShipmentWizardScenarios } from '../../../Scenarios/ShipmentScenarios/NewShipmentWizardScenarios';
import { EditShipmentTabsScenarios } from '../../../Scenarios/ShipmentScenarios/EditShipmentTabsScenarios';

import { LoginComp } from "../../../../Login/Login.po";

describe('Operations', () => {
    let login: LoginComp = new LoginComp();
    let scenarios: NewShipmentWizardScenarios = new NewShipmentWizardScenarios();
    let editScenarios: EditShipmentTabsScenarios = new EditShipmentTabsScenarios();
    //let quoteActions: QuoteActions = new QuoteActions();
    var direction;
    var transportMode;
    var shipmentType;
    var levelCode;

    beforeEach(() => {
        Resolvers.MainMenuResolver.Selector('#GeneralMHOperations').Select();
        Resolvers.MainMenuResolver.Selector('#SHIP').Select();
    });

    it('Test New/Edit Shipment', () => {
        levelCode='D'
        direction = 'E';
        transportMode = 'A';
        shipmentType = '';
        scenarios.RunScenario(levelCode, transportMode, direction, shipmentType);
        editScenarios.RunEditTabsScenarios(levelCode, transportMode, direction, shipmentType);
        //quoteActions.RunQuoteActions();
    });
});