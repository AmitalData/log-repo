import { Resolvers } from "../../Resolvers/Resolvers";
import { Random } from "../../@e2e/core";


export class ReceivablesTab {

    public EntityNumber: string;

    public RunReceivablesTabScenarios() {
        this.EntityNumber = Random.GetRandomNumber();
        this.GoToReceivablesTab();
        this.AddReceivable(this.EntityNumber);
        this.CreateARInvoice(this.EntityNumber);
        this.FillInvoiceDetailes();
        this.GoToGeneralTabInARInvoice();
        this.GoToPaymentsTabInARInvoice(this.EntityNumber);
        this.BackToShipmentsTab();
    }
    private GoToReceivablesTab() {
        cy.get('#ShipmentTHReceivables').click();
     
    }

    private AddReceivable(EntityNumber: string) {
        Resolvers.ButtonResolver.Selector('#Add_4').Click();
     //   this.WaitLoaded('userviews/getsingle/?');
        Resolvers.LOVResolver.Selector('#ShipmentReceivable_ChargesTypeId').SelectFirst();
        Resolvers.TextBoxResolver.Selector('#ShipmentReceivable_TotalAmount').Type(EntityNumber);
     //   Resolvers.LOVResolver.Selector('#ShipmentReceivable_CurrencyId').SelectFirst();
     //   Resolvers.TextBoxResolver.Selector('#ShipmentReceivable_Quantity').Type('1');
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
      //  cy.get('#ARInvoiceBSaveAsDraft').click();
        Resolvers.ButtonResolver.Selector('#ARInvoiceBSaveAsDraft').Click();
        Resolvers.ButtonResolver.Selector('#ARInvoiceBApprove').Click();

       // cy.get('#ARInvoiceBApprove').click();
    }
    private BackToShipmentsTab() {
        Resolvers.ButtonResolver.Selector('#ARInvoiceBApprove').Click();
        Resolvers.ButtonResolver.Selector('#BackButton_1').Click();
    }
    private GoToGeneralTabInARInvoice() {
        cy.get('#ARInvoiceTHGeneral').click();
        Resolvers.LOVResolver.Selector('#ARInvoice_SalesmanUserId').SelectFirst();
        Resolvers.LOVResolver.Selector('#ARInvoice_Field1').SelectFirst();
        Resolvers.DatePickerResolver.Selector('#date_ARInvoice_PaidDate').Type('.');
    }
    private GoToPaymentsTabInARInvoice(EntityNumber: string) {
        cy.get('#ARInvoiceTHARPayments').click();
        Resolvers.LOVResolver.Selector('#ARPayment_AccountingPaymentMethodId').SelectFirst();
        Resolvers.TextBoxResolver.Selector('#ARPayment_AmountInPaymentCurrency').Type(EntityNumber);
        Resolvers.LOVResolver.Selector('#ARPayment_BranchId').SelectFirst();
        Resolvers.ButtonResolver.Selector('#ok-AddARPayment').Click();
        
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