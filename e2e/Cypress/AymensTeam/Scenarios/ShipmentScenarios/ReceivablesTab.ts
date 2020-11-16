import { Resolvers } from "../../Resolvers/Resolvers";
import { Random } from "../../@e2e/core";


export class ReceivablesTab {

    public EntityNumber: string;
    public count: number;
    public RunReceivablesTabScenarios() {
        this.EntityNumber = Random.GetRandomNumber();
        this.count = 1;
        this.GoToReceivablesTab();
        this.AddReceivable(this.EntityNumber);
        this.CreateARInvoice(this.EntityNumber);
        this.FillInvoiceDetailes();
        this.GoToGeneralTabInARInvoice();
       // this.GoToPaymentsTabInARInvoice(this.EntityNumber);
        this.BackToShipmentsTab(this.count);
        this.AddReceivable("-" + this.EntityNumber);
        this.CreateCreditNote();
        this.FillInvoiceDetailes();
        this.GoToGeneralTabInARInvoice();
        this.count++;
        this.BackToShipmentsTab(this.count);
    }
    private GoToReceivablesTab() {
        cy.get('#ShipmentTHReceivables').click();
    }

    private AddReceivable(EntityNumber: string) {
        Resolvers.ButtonResolver.Selector('#Add').Click();
        Resolvers.LOVResolver.Selector('#ShipmentReceivable_ChargesTypeId').SelectFirst();
        Resolvers.TextBoxResolver.Selector('#ShipmentReceivable_TotalAmount').Type(EntityNumber);
        Resolvers.ButtonResolver.Selector('#Ok-AddReceivableBtn').Click();
    }
    private CreateARInvoice(EntityNumber: string) {
        cy.get('#CreateARInvoice').click();
        Resolvers.DatePickerResolver.Selector('#date_ARInvoice_DueDate').Type('.');
        Resolvers.ButtonResolver.Selector('#Ok-CreateARInvoice').Click();
    }
    private FillInvoiceDetailes() {
        Resolvers.LOVResolver.Selector('#ARInvoice_VatTypeId').Type('Zero');
        Resolvers.ButtonResolver.Selector('#VATApplyToAll').Click();
        Resolvers.ButtonResolver.Selector('#ARInvoiceBSaveAsDraft').Click();
    }
    private GoToGeneralTabInARInvoice() {
        cy.get('#ARInvoiceTHGeneral').click();
        Resolvers.LOVResolver.Selector('#ARInvoice_SalesmanUserId').SelectFirst();
        Resolvers.ButtonResolver.Selector('#ARInvoiceBSaveAsDraft').Click();
    }
    private GoToPaymentsTabInARInvoice(EntityNumber: string) {
        Resolvers.ButtonResolver.Selector('#ARInvoiceBApprove').Click();
        cy.get('#ARInvoiceTHARPayments').click();
        Resolvers.ButtonResolver.Selector('#New Payment').Click();
        Resolvers.LOVResolver.Selector('#ARPayment_AccountingPaymentMethodId').SelectFirst();
        Resolvers.TextBoxResolver.Selector('#ARPayment_AmountInPaymentCurrency').Type(EntityNumber);
        Resolvers.LOVResolver.Selector('#ARPayment_BranchId').SelectFirst();
        Resolvers.ButtonResolver.Selector('#ok-AddARPayment').Click();
        Resolvers.ButtonResolver.Selector('#ARPayment-SaveClose').Click();
        Resolvers.ButtonResolver.Selector('#Disconnect').Click();
        Resolvers.ButtonResolver.Selector('#Connect').Click();    
    }

    private BackToShipmentsTab(count:number) {
        Resolvers.ButtonResolver.Selector('#ARInvoiceBSaveAsDraft').Click();
        Resolvers.ButtonResolver.Selector('#ARInvoiceBApprove').Click();
        Resolvers.ButtonResolver.Selector('#BackButton_' + count).Click();
    }
    private BackToShipmentsTab15() {
        Resolvers.ButtonResolver.Selector('#ARInvoiceBSaveAsDraft').Click();
        Resolvers.ButtonResolver.Selector('#ARInvoiceBApprove').Click();
        Resolvers.ButtonResolver.Selector('#BackButton_15').Click();
    }
    
    private CreateCreditNote() {
        cy.get('#CreateCreditNote').click();
        Resolvers.DatePickerResolver.Selector('#date_ARInvoice_DueDate').Type('.');
        Resolvers.ButtonResolver.Selector('#Ok-CreateARInvoice').Click();
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