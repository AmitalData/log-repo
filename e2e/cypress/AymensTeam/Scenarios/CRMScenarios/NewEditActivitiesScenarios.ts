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
        this.ActivityScenarios();
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
 
    private ActivityScenarios() {
        if (this.ActivitTypes == "T") {
            this.CreateTask(this.EntityNumber);
            this.SaveAndSearchActivity();
            this.EditTask(this.EntityNumber);
        }
        if (this.ActivitTypes == "P") {
            this.CreatePhoneCall(this.EntityNumber);
            this.SaveAndSearchActivity();
            this.EditPhoneCall(this.EntityNumber);
        }
        if (this.ActivitTypes == "A") {
            this.CreateAppoinment(this.EntityNumber);
            this.SaveAndSearchActivity();
            this.EditAppoinment(this.EntityNumber);
        }

    }
    private CreateTask(EntityNumber: string) {
        Resolvers.TextBoxResolver.Selector('#Activity_Subject').Type('Task # : ' + EntityNumber);
        Resolvers.TextBoxResolver.Selector('#Activity_Description').Type('Description for task # : ' + EntityNumber);
        Resolvers.LOVResolver.Selector('#Activity_PriorityCode').SelectFirst();
    }
    private CreatePhoneCall(EntityNumber: string) {
        Resolvers.LOVResolver.Selector('#Activity_CustomerId').Type('Customer Activity');
        Resolvers.LOVResolver.Selector('#Activity_CallWithId').SelectFirst();

        Resolvers.TextBoxResolver.Selector('#Activity_Subject').Type('PhoneCall # : ' + EntityNumber);
        Resolvers.TextBoxResolver.Selector('#Activity_Description').Type('Description for PhoneCall # : ' + EntityNumber);
        Resolvers.LOVResolver.Selector('#Activity_PriorityCode').SelectFirst();
    }
    private CreateAppoinment(EntityNumber: string) {
        Resolvers.TextBoxResolver.Selector('#Activity_Subject').Type('Appoinment # : ' + EntityNumber);
        Resolvers.TextBoxResolver.Selector('#Activity_Description').Type('Description for Appoinment # : ' + EntityNumber);
        Resolvers.LOVResolver.Selector('#Activity_PriorityCode').SelectFirst();
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
            }).as('CreateTask');
            Resolvers.ButtonResolver.Selector('#Ok-AddActivity').Click();
            cy.wait('@CreateTask').its('responseBody').then((json) => {
                resolve(json['Subject']);
            });
        });
    }
    SaveAndSearchActivity() {
        this.Save().then((entityNumber: string) => {
            cy.get('quicksearchtextbox')
                .find('.LogitudeQuickSearchTextBox')
                .within(() => {
                    cy.get('input').type(entityNumber).then(() => {
                            cy.get('ul > li').eq(0).click({ force: true });
                    });
                });
        });
    }
    EditAppoinment(EntityNumber: string) {
        let today = new Date().toLocaleDateString();
        Resolvers.DatePickerResolver.Selector('#date_Activity_StartDateTime').Type(today);
        Resolvers.DatePickerResolver.Selector('#time_Activity_StartDateTime').Type('10:00 AM');
        Resolvers.LOVResolver.Selector("#Activity_CustomerId").SelectFirst();
        Resolvers.TextBoxResolver.Selector('#Activity_Description').Type('Edit Appointment # : ' + EntityNumber);
        cy.get('#Activity-SaveClose').click();
    }
    EditPhoneCall(EntityNumber: string) {
        let today = new Date().toLocaleDateString();
        Resolvers.DatePickerResolver.Selector('#date_Activity_DueDate').Type(today);
        Resolvers.DatePickerResolver.Selector('#time_Activity_DueDate').Type('10:00 AM');
        Resolvers.LOVResolver.Selector("#Activity_PriorityCode").SelectFirst();
        Resolvers.TextBoxResolver.Selector('#Activity_Description').Type('Edit PhoneCall # : ' + EntityNumber);
        cy.get('#Activity-SaveClose').click();
    }
    EditTask(EntityNumber: string) {   
        let today = new Date().toLocaleDateString();
        Resolvers.DatePickerResolver.Selector('#date_Activity_StartDateTime').Type(today);
        Resolvers.DatePickerResolver.Selector('#time_Activity_StartDateTime').Type('10:00 AM');
        Resolvers.LOVResolver.Selector("#Activity_CustomerId").SelectFirst();
        Resolvers.TextBoxResolver.Selector('#Activity_Description').Type('Edit Task # : ' + EntityNumber);
        cy.get('#Activity-SaveClose').click();
    }
}