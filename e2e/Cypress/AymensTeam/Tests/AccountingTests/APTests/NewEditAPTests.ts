import { Resolvers } from "../../../Resolvers/Resolvers";
import { NewAPScenarios } from '../../../Scenarios/AccountingScenarios/NewAPScenarios';
import { LoginComp } from "../../../../Login/Login.po";

describe('Accounts Payable', () => {
    let login: LoginComp = new LoginComp();
    let scenarios: NewAPScenarios = new NewAPScenarios();
    beforeEach(() => {
        Resolvers.MainMenuResolver.Selector('#GeneralMHAccounting').Select();
        Resolvers.MainMenuResolver.Selector('#PAYABLEAccounting').Select();
    });
 it('Test New Payment', () => {
        scenarios.RunScenario();  
    });
});