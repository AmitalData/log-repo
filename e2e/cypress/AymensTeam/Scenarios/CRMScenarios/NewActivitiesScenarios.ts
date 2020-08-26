import { Random } from "../../@e2e/core";
import { Resolvers } from "../../Resolvers/Resolvers";

export class NewActivitiesScenarios {
    private levelCode: string;
    private direction: string;
    private transportMode: string;
    private shipmentType: string;
    private objectTable: string;
    private isFCL: boolean = false;
    private isInlandDomestic: boolean = false;
    public EntityId: string;
    public EntityNumber: string;
    public RunScenario(levelCode:string) {

        this.OpenWizardWindow();
        this.FillGeneral();
        this.Save().then((subject: string) => {

        });
    }

    public WaitPromise() {
        return new Cypress.Promise((resolve, reject) => {
            return this.EntityNumber;
        });
    }

    private OpenWizardWindow() {
        let index: number = 0;

        switch (this.levelCode) {
            case "T": { index = 0; break; }
            case "P": { index = 1; break; }
            case "A": { index = 2; break; }
        }

        Resolvers.ToggleButtonResolver.Selector('#NEWACTIVITY').SelectByIndex(index);
        //Resolvers.ToggleButtonResolver.Selector("ToggleButton").Parent('OperationsComponent').SelectByIndex(0);

        Resolvers.WindowResolver.ShouldBeOpend();

    }
    private CancelWizardWindow() {
        Resolvers.ButtonResolver.Selector('Button').Text('Cancel').ThenConfirmButtonText("Don't Save").Click();
        Resolvers.WindowResolver.ShouldBeClosed();
    }
 
    private FillGeneral() {
       // if (this.levelCode == "T") {
            Resolvers.TextBoxResolver.Selector('#Activity_Subject').Type("test");
      //  }
    }
 
    Save() {

        return new Cypress.Promise((resolve, reject) => {

            cy.server();
            cy.route({
                method: 'POST',
                url: '**/activities',
                onResponse: (xhr) => {
                    expect(xhr.status).to.eq(200);
                }
            }).as('CreateTask')

            //  Resolvers.ButtonResolver.Selector('Button').Text('Ok').Click();
            cy.get('#Ok-AddActivity').click();
            cy.wait('@CreateTask');
            Resolvers.WindowResolver.ShouldBeClosed();
        });
    }

}