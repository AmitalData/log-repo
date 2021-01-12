import { Random } from "../../@e2e/core";
import { Resolvers } from "../../Resolvers/Resolvers";
import { NewEditActivitiesScenarios } from './NewEditActivitiesScenarios';


export class NewEditOpportunitiesScenarios {
    private OpportunityTypes: string;
    public EntityId: string;
    public EntityNumber: string;
    public RunScenario() {
        this.EntityNumber = Random.GetRandomNumber();
        this.OpenWizardWindow();
        this.OppotunityScenarios();
    }

    private OpenWizardWindow() {

        Resolvers.ButtonResolver.Selector('#NEWOPPORTUNITY').Click();
        Resolvers.WindowResolver.ShouldBeOpend();
    }

    private OppotunityScenarios() {
        this.CreateOpportunity(this.EntityNumber);
        this.SaveAndSearchOpportunity();
        this.EditOpportunity(this.EntityNumber);

    }
    private CreateOpportunity(EntityNumber: string) {
        Resolvers.LOVResolver.Selector('#Opportunity_OpportunityTypeId').SelectFirst();
        Resolvers.TextBoxResolver.Selector('#Opportunity_Subject').Type('Opportunity # : ' + EntityNumber);
        Resolvers.LOVResolver.Selector('#Opportunity_CustomerId').Type('Customer Activity');

    }

    private Save() {
        return new Cypress.Promise((resolve, reject) => {
            cy.server();
            cy.route({
                method: 'POST',
                url: '**/opportunities',
                onResponse: (xhr) => {
                    expect(xhr.status).to.eq(200);
                }
            }).as('CreateOpportunity');
            Resolvers.ButtonResolver.Selector('#Ok-AddOpportunity').Click();
            cy.wait('@CreateOpportunity').its('responseBody').then((json) => {
                resolve(json['Subject']);
            });
        });
    }
    private SaveAndSearchOpportunity() {
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
    private EditOpportunity(EntityNumber: string) {
        let scenarios: NewEditActivitiesScenarios = new NewEditActivitiesScenarios();
        Resolvers.DatePickerResolver.Selector('#date_Opportunity_StageDueDate').Type('.');
        Resolvers.LOVResolver.Selector("#Opportunity_RatingCode").SelectFirst();
        Resolvers.ButtonResolver.Selector('#AddPhoneCall').Click();
        scenarios.CreatePhoneCallWithoutCustomer(this.EntityNumber);
        scenarios.Save();
        Resolvers.ButtonResolver.Selector('#AddTask').Click();
        scenarios.CreateTask(this.EntityNumber);
        scenarios.Save();
        Resolvers.ButtonResolver.Selector('#AddAppointment').Click();
        scenarios.CreateAppoinment(this.EntityNumber);
        scenarios.Save();
        Resolvers.MainMenuResolver.Selector('#OpportunityTHGeneral').Select();
        Resolvers.LOVResolver.Selector("#Opportunity_ContactId").SelectFirst();
        Resolvers.LOVResolver.Selector("#Opportunity_OwnerId").SelectFirst();
        Resolvers.LOVResolver.Selector("#Opportunity_LeadSourceId").SelectFirst();
        Resolvers.LOVResolver.Selector("#Opportunity_LeadUserId").SelectFirst(); 
        Resolvers.LOVResolver.Selector("#Opportunity_LeadPartnerId").SelectFirst();
        //  this.WaitLoaded('CRMDomain/GetUpcomigActivities?');


        cy.get('#Opportunity-SaveClose').click();


        //  this.WaitLoaded('CRMDomain/GetUpcomigActivities?');
    }

    private WaitLoaded(urls: string) {
        cy.server();
        cy.route({
            method: 'GET',
            url: '**/' + urls + '**',
            onResponse: (xhr) => {
                expect(xhr.status).to.eq(200);
            }
        }).as('entityLoaded');
        //  cy.wait('@entityLoaded');
    }
}