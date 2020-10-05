import { Resolvers } from "../../Resolvers/Resolvers";


export class QuoteActions {

    public RunQuoteActions() {
        this.QuoteSave()
        this.CancelQuote();
        this.ReactivateQuote();
        //this.CopyQuote();
        this.QuoteAccepted();
        this.BuildShipmentFromQuote();
    }
    private QuoteSave() {

        cy.server();

        cy.route({
            method: 'PUT',
            url: '**/quotes',
            onResponse: (xhr) => {
                expect(xhr.status).to.eq(200);
            }
        }).as('SaveQuote')

        Resolvers.ButtonResolver.Selector('#Quote-Save').Click();

        cy.wait('@SaveQuote');

        //Resolvers.ButtonResolver.Selector('#Quote-Save').Click();
        //cy.get('#BusyIndicator_0').should('not.be.visible').then(() => {
        //})
    }
    private CancelQuote() {

        cy.server();

        cy.route({
            method: 'PUT',
            url: '**/quotes',
            onResponse: (xhr) => {
                expect(xhr.status).to.eq(200);
            }
        }).as('SaveQuote')

        Resolvers.ButtonResolver.IsMoreMenu().Selector('#QuoteBCancelQuote').Click();
        Resolvers.ButtonResolver.Selector('#QuoteBCancelQuote').Click();
        Resolvers.ButtonResolver.Selector('#ConfrimApproved').Click();

        cy.wait('@SaveQuote');



        //Resolvers.ButtonResolver.Selector('#MenuButtons').Click();
        //Resolvers.ButtonResolver.Selector('#QuoteBCancelQuote').Click();
        //Resolvers.ButtonResolver.Selector('#ConfrimApproved').Click();
        //cy.get('#BusyIndicator_0').should('not.be.visible');
        //Resolvers.WindowResolver.ShouldBeClosed();
        //this.WaitLoaded('addressviews/getsingle/?');
    }
    private ReactivateQuote() {
        cy.server();

        cy.route({
            method: 'PUT',
            url: '**/quotes',
            onResponse: (xhr) => {
                expect(xhr.status).to.eq(200);
            }
        }).as('SaveQuote')

        Resolvers.ButtonResolver.Selector('#MenuButtons').Click();
        Resolvers.ButtonResolver.Selector('#QuoteBReactivateQuote').Click();
        Resolvers.ButtonResolver.Selector('#ConfrimApproved').Click();

        cy.wait('@SaveQuote');

        //Resolvers.ButtonResolver.Selector('#MenuButtons').Click();
        //Resolvers.ButtonResolver.Selector('#QuoteBReactivateQuote').Click();
        //Resolvers.ButtonResolver.Selector('#ConfrimApproved').Click();
        //cy.get('#BusyIndicator_0').should('not.be.visible');
        //Resolvers.WindowResolver.ShouldBeClosed();
        //this.WaitLoaded('quotes');

    }
    private QuoteAccepted() {

        cy.server();

        cy.route({
            method: 'PUT',
            url: '**/quotes',
            onResponse: (xhr) => {
                expect(xhr.status).to.eq(200);
            }
        }).as('SaveQuote')

        Resolvers.ButtonResolver.Selector('#QuoteBAccept').Click();
        Resolvers.ButtonResolver.Selector('#ConfrimApproved').Click();

        cy.wait('@SaveQuote');


        //Resolvers.ButtonResolver.Selector('#QuoteBAccept_1').Click();
        //Resolvers.WindowResolver.ShouldBeOpend();
        //Resolvers.ButtonResolver.Selector('#ConfrimApproved').Click();
        //Resolvers.WindowResolver.ShouldBeClosed();
        //this.WaitLoaded('quotes');

    }
    private QuoteDeclined() {
        Resolvers.ButtonResolver.Selector('#QuoteBDecline').Click();
    }
    private CopyQuote() {
        Resolvers.ButtonResolver.Selector('#MenuButtons').Click();
        Resolvers.ButtonResolver.Selector('#QuoteBCopyQuote').Click();

        cy.get('logcheckbox')
            .find('.CheckBox')
            .within(() => {
                cy.get('#Quote_ChargesTypesCopyIsChecked').click({ force: true });
            });
        cy.get('logcheckbox')
            .find('.CheckBox')
            .within(() => {
                cy.get('#Quote_CopyCostIsChecked').click({ force: true });
            });
        cy.get('logcheckbox')
            .find('.CheckBox')
            .within(() => {
                cy.get('#Quote_CopySaleIsChecked').click({ force: true });
            });
        Resolvers.ButtonResolver.Selector('#CreateQuote').Click();
        cy.get('#BusyIndicator_0').should('not.be.visible');
        Resolvers.WindowResolver.ShouldBeClosed();
        //this.WaitLoaded('testyyyyyyy');
    }
    private BuildShipmentFromQuote() {


        Resolvers.ButtonResolver.Selector('#QuoteBBuildShipment').Click();

        cy.get("#ShipmentLevelRadio_0D").click({ force: true });
        Resolvers.ButtonResolver.Selector('#ShipmentCreatebtn').Click();


        //Resolvers.ButtonResolver.Selector('#QuoteBBuildShipment_1').Click();
        //cy.contains('button', 'Accept and Build').click();

    }
    private WaitLoaded(url: string) {
        cy.server();
        //cy.route({
        //    method: 'GET',
        //    url: '**/' + url + '**',
        //    onResponse: (xhr) => {
        //        expect(xhr.status).to.eq(200);
        //    }
        //}).as('entityLoaded');
        cy.route('**/' + url + '**').as('entityLoaded');
        // action
        cy.wait('@entityLoaded');
    }


    private Save() {
        cy.server();

        cy.route({
            method: 'PUT',
            url: '**/quotes',
            onResponse: (xhr) => {
                expect(xhr.status).to.eq(200);
            }
        }).as('SaveQuote')

        cy.get('#ShipmentCreatebtn').click();

        cy.wait('@SaveQuote');
    }

}