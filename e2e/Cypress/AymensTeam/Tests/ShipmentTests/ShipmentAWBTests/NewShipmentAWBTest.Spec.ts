import { Resolvers } from "../../../Resolvers/Resolvers";
import { NewAWBScenarios } from '../../../Scenarios/ShipmentAWBScenarions/NewAWBScenarios';
import { LoginComp } from "../../../../Login/Login.po";

describe('Test New Shipment Wizard : ', () => {
    let login: LoginComp = new LoginComp();
    let scenarios: NewAWBScenarios = new NewAWBScenarios();

    var levelCode;

    beforeEach(() => {
        Resolvers.MainMenuResolver.Selector('#GeneralMHOperations').Select();
        Resolvers.MainMenuResolver.Selector('#SHIP').Select();
    });

    it('Create a Direct AWB shipment Wizard', () => {
        levelCode = 'D';
     
        scenarios.RunScenario(levelCode);
   
    });
});