import { Random } from "../../@e2e/core";
import { Resolvers } from "../../Resolvers/Resolvers";

export class NewEditActivitiesScenarios {
    private ActivitTypes : string;
    public EntityId: string;
    public EntityNumber: string;
    public RunScenario(ActivitTypes: string) {
        this.ActivitTypes = ActivitTypes;
        this.EntityNumber = Random.GetRandomNumber();
        this.OpenWizardWindow();
        this.FillActivitTypes();
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
            this.CreateTask(this.EntityNumber);
        }
        if (this.ActivitTypes == "P") {
            this.CreatePhoneCall(this.EntityNumber);
        }
        if (this.ActivitTypes == "A") {
            this.CreateAppoinment(this.EntityNumber);
        }

    }
    private CreateTask(TaskNumber: string) {
        cy.get('#Activity_Subject').type(TaskNumber, { force: true }).then(() => {
            Resolvers.LOVResolver.Selector('#Activity_PriorityCode').SelectFirst();
            this.Save();
        })
        this.SearchAboutActivity(TaskNumber);
        this.EditTask(TaskNumber);
    }
    private CreatePhoneCall(TaskNumber: string) {
        Resolvers.LOVResolver.Selector('#Activity_CallWithId').SelectFirst();
        cy.get('#Activity_Subject').type(TaskNumber, { force: true }).then(() => {
        Resolvers.LOVResolver.Selector('#Activity_PriorityCode').SelectFirst();
            this.Save();
        })
        this.SearchAboutActivity(TaskNumber);
        this.EditPhoneCall(TaskNumber);
    }
    private CreateAppoinment(TaskNumber: string) {
        cy.get('#Activity_Subject').type(TaskNumber, { force: true }).then(() => {
            Resolvers.LOVResolver.Selector('#Activity_PriorityCode').SelectFirst();
            this.Save();  
        })
        this.SearchAboutActivity(TaskNumber);
        this.EditAppoinment(TaskNumber);
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
            cy.get('#Ok-AddActivity').click();
            cy.wait('@CreateTask');
            Resolvers.WindowResolver.ShouldBeClosed();
        });
    }
    SearchAboutActivity(TaskNumber: string) {
        cy.get('quicksearchtextbox')
            .find('.LogitudeQuickSearchTextBox')
            .eq(0)
            .within(() => {
                cy.get('input').type(TaskNumber).then(() => {
                    cy.get('ul > li').eq(0).click({ force: true });
                });
            });
    }
    EditAppoinment(TaskNumber: string) {
        let today = new Date().toLocaleDateString();
        Resolvers.DatePickerResolver.Selector('#date_Activity_StartDateTime').Type(today);
        Resolvers.DatePickerResolver.Selector('#time_Activity_StartDateTime').Type('10:00 AM');
        Resolvers.LOVResolver.Selector("#Activity_CustomerId").SelectFirst();
        Resolvers.TextBoxResolver.Selector('#Activity_Description').Type(TaskNumber);
        cy.get('#Activity-SaveClose').click();
    }
    EditPhoneCall(TaskNumber: string) {
        let today = new Date().toLocaleDateString();
        Resolvers.DatePickerResolver.Selector('#date_Activity_DueDate').Type(today);
        Resolvers.DatePickerResolver.Selector('#time_Activity_DueDate').Type('10:00 AM');
        Resolvers.LOVResolver.Selector("#Activity_PriorityCode").SelectFirst();
        Resolvers.TextBoxResolver.Selector('#Activity_Description').Type(TaskNumber);
        cy.get('#Activity-SaveClose').click();
    }
    EditTask(TaskNumber: string) {   
        let today = new Date().toLocaleDateString();
        Resolvers.DatePickerResolver.Selector('#date_Activity_StartDateTime').Type(today);
        Resolvers.DatePickerResolver.Selector('#time_Activity_StartDateTime').Type('10:00 AM');
        Resolvers.LOVResolver.Selector("#Activity_CustomerId").SelectFirst();
        Resolvers.TextBoxResolver.Selector('#Activity_Description').Type(TaskNumber);
        cy.get('#Activity-SaveClose').click();
    }
}