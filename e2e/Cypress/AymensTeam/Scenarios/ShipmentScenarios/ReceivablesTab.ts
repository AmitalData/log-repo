import { Resolvers } from "../../Resolvers/Resolvers";
import { Random } from "../../@e2e/core";


export class ReceivablesTab {

    public EntityNumber: string ="100";
    public count: number;
    public RunReceivablesTabScenarios() {
        this.count = 1;
        this.GoToReceivablesTab();
        this.AddARInvoice();
        this.AddCreditNoteInvoice();
        this.BackToSystem();
    }
    private AddARInvoice() {
        this.AddReceivable(this.EntityNumber);
        //this.CreateInvoice("ARInvoice");
        //this.FillInvoiceDetailes();
        //this.GoToPaymentsTabInARInvoice(this.EntityNumber);
        //cy.server();
        //cy.route({
        //    method: 'GET',
        //    url: '**/shipment/GetSingle?id=**',
        //    onResponse: (xhr) => {
        //        expect(xhr.status).to.eq(200);
        //    }
        //}).as('entityLoaded');
        //this.GoToGeneralTabInARInvoice();
        //this.BackToShipmentsTab(this.count);
        //cy.wait('@entityLoaded');
    }
    private AddCreditNoteInvoice() {
        this.AddReceivable("-" + this.EntityNumber);
        //this.CreateInvoice("CreditNote");
        //this.FillInvoiceDetailes();
        //this.count += 2;
        //cy.server();
        //cy.route({
        //    method: 'GET',
        //    url: '**/shipment/GetSingle?id=**',
        //    onResponse: (xhr) => {
        //        expect(xhr.status).to.eq(200);
        //    }
        //}).as('entityLoaded');
        //this.GoToGeneralTabInARInvoice();
        //this.BackToShipmentsTab(this.count);
        //cy.wait('@entityLoaded');
    }
    private GoToReceivablesTab() {
        cy.get('#ShipmentTHReceivables').click();
    }

    private AddReceivable(EntityNumber: string) {
        cy.get('#AddReceivable').click();
        Resolvers.LOVResolver.Selector('#ShipmentReceivable_ChargesTypeId').SelectFirst();
        Resolvers.LOVResolver.Selector('#ShipmentReceivable_CurrencyId').Type('NIS');
        Resolvers.TextBoxResolver.Selector('#ShipmentReceivable_TotalAmount').Type(EntityNumber);
        Resolvers.TextBoxResolver.Selector('#ShipmentReceivable_Notes').Type('test');
        Resolvers.ButtonResolver.Selector('#Ok-AddReceivableBtn').Click();
        Resolvers.ButtonResolver.Selector('#Shipment-Save').Click();
      //  this.WaitLoaded();
    }
    private CreateInvoice(name : string) {
        cy.get('#Create'+name).click();
        Resolvers.DatePickerResolver.Selector('#date_ARInvoice_DueDate').Type('.');
        Resolvers.ButtonResolver.Selector('#Ok-CreateARInvoice').Click();
    }

    private FillInvoiceDetailes() {
        Resolvers.LOVResolver.Selector('#ARInvoice_VatTypeId').Type('Zero');
        Resolvers.ButtonResolver.Selector('#VATApplyToAll').Click();
        cy.get('#ARInvoiceBApprove').click();
    }
    private GoToGeneralTabInARInvoice() {
        cy.get('#ARInvoiceTHGeneral').click();
        Resolvers.LOVResolver.Selector('#ARInvoice_SalesmanUserId').SelectFirst();
     //   cy.get('#ARInvoiceBApprove').click();

    }
    private GoToPaymentsTabInARInvoice(EntityNumber: string) {
     
        cy.get('#ARInvoiceTHARPayments').click();
        cy.contains('New Payment').click();
        Resolvers.LOVResolver.Selector('#ARPayment_AccountingPaymentMethodId').SelectFirst();
        Resolvers.TextBoxResolver.Selector('#ARPayment_AmountInPaymentCurrency').Type(EntityNumber);
        Resolvers.LOVResolver.Selector('#ARPayment_BranchId').SelectFirst();
        Resolvers.ButtonResolver.Selector('#ok-AddARPayment').Click();
        Resolvers.DatePickerResolver.Selector('#datepickerinputdiv_date_ARPayment_ValueDate').Type('.');
        Resolvers.ButtonResolver.Selector('#ARPayment-SaveClose').Click();
        Resolvers.ButtonResolver.Selector('#Disconnect').Click();
        Resolvers.ButtonResolver.Selector('#Connect').Click();
    }

    private BackToShipmentsTab(count: number) {
       // cy.get('#ARInvoiceBSaveAsDraft').click();
        Resolvers.ButtonResolver.Selector('#BackButton_' + count).Click();
       //   cy.get('.ConfirmWindow').should('be.visible')
       Resolvers.ButtonResolver.Selector('#ConfirmWindow_Yes_0').Click();  
        Resolvers.WindowResolver.ShouldBeClosed();
      
    }

    private BackToSystem() {

    }
    
    private WaitLoaded() {
        cy.server();
        cy.route({
            method: 'GET',
            url: '**/userviews/getsingle/?id**',
            onResponse: (xhr) => {
                expect(xhr.status).to.eq(200);
            }
        }).as('entitySaved');
        cy.wait('@entitySaved');
    }
}