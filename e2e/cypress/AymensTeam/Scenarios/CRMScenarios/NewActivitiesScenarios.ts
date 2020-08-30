import { Random } from "../../@e2e/core";
import { Resolvers } from "../../Resolvers/Resolvers";

export class NewActivitiesScenarios {
    private ActivitTypes : string;
   // private objectTable: string;
    public EntityId: string;
    public EntityNumber: string;
    public RunScenario(ActivitTypes: string) {
        this.ActivitTypes = ActivitTypes;
        this.EntityNumber = Random.GetRandomNumber();
   //     this.objectTable = "Activity";
        this.OpenWizardWindow();
        this.FillActivitTypes();
     //   this.Save().then((subject: string) => {
       // });
    }


    private OpenWizardWindow() {
        let index: number = 0;
 
        switch (this.ActivitTypes) {
            case "T": { index = 0; break; }
            case "P": { index = 1; break; }
            case "A": { index = 2; break; }
        }

        Resolvers.ToggleButtonResolver.Selector('#NEWACTIVITY').SelectByIndex(index);
        Resolvers.WindowResolver.ShouldBeOpend();

    }
 
 
    private FillActivitTypes() {
        if (this.ActivitTypes == "T") {
            this.CreateTask();
        }
        if (this.ActivitTypes == "P") {
            this.CreatePhoneCall();
        }
        if (this.ActivitTypes == "A") {
            this.CreateAppoinment();
        }

    }
    private CreateTask() {
      //  Resolvers.TextBoxResolver.Selector('#Activity_Subject').Type(this.EntityNumber);
        cy.get('#Activity_Subject').type(this.EntityNumber).then(() => {
            this.Save();
        })
    }
    private CreatePhoneCall() {
      //  Resolvers.LOVResolver.ObjectTable(this.objectTable).ObjectField("CallWithId").SelectFirst();
      // Resolvers.TextBoxResolver.Selector('#Activity_CallWithId').Type(Random.GetRandomNumber());
        Resolvers.LOVResolver.Selector('#Activity_CallWithId').SelectFirst();
        cy.get('#Activity_Subject').type(this.EntityNumber).then(() => {
            this.Save();
        })
    }
    private CreateAppoinment() {
       // Resolvers.TextBoxResolver.Selector('#Activity_Subject').Type("test app");
        cy.get('#Activity_Subject').type(this.EntityNumber).then(() => {
            this.Save();
        })
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