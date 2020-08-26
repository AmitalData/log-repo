import { Resolvers } from "../../../Resolvers/Resolvers";
import { NewActivitiesScenarios } from '../../../Scenarios/CRMScenarios/NewActivitiesScenarios';
import { LoginComp } from "../../../../Login/Login.po";

describe('Activities', () => {
    let login: LoginComp = new LoginComp();
    let senarios: NewActivitiesScenarios = new NewActivitiesScenarios();


    beforeEach(() => {

        Resolvers.MainMenuResolver.Selector('#GeneralMHCRM').Select();
        Resolvers.MainMenuResolver.Selector('#CRMACT').Select();

    });

    it('Test New Activities', () => {
        
        senarios.RunScenario('T');
    

      //  Resolvers.ToggleButtonResolver.Selector('#NEWACTIVITY').SelectByIndex(0);
      //  Resolvers.WindowResolver.ShouldBeOpend();

   
        //scenarios.CreateWizardShipment('D', 'E', 'A');

        //scenarios.CreateWizardShipment('D', 'E', 'A');
    });
});