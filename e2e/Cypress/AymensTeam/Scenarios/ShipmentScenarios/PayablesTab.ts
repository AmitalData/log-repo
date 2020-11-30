import { Resolvers } from "../../Resolvers/Resolvers";
import { Random } from "../../@e2e/core";

export class PayablesTab {
    public EntityNumber: string;

    public RunPayablesTabScenarios() {
        this.EntityNumber = Random.GetRandomNumber();
        this.GoToPayablesTab();
        this.AddPayable();
        this.ReceiveInvoice(this.EntityNumber);
        this.FillInvoiceDetailes(this.EntityNumber);
       // this.GoToGeneralTabInAPInvoice();
        this.SaveAP();
        this.ApproveAP();
        this.BackToShipmentsTab();
    }
    private GoToPayablesTab() {
        cy.get('#ShipmentTHPayables').click();
    }
    private AddPayable() {
        cy.get('#AddPayable').click();
        Resolvers.LOVResolver.Selector('#ShipmentPayable_ChargesTypeId').SelectFirst();
        Resolvers.LOVResolver.Selector('#ShipmentPayable_CurrencyId').Type('NIS');
        Resolvers.TextBoxResolver.Selector('#ShipmentPayable_ExpectedAmount').Type('100');
        Resolvers.ButtonResolver.Selector('#Ok-AddPayableBtn').Click();
    }
    private ReceiveInvoice(EntityNumber: string) {
        cy.get('#ReceiveInvoice').click();
        Resolvers.LOVResolver.Selector('#APInvoice_VendorId').Type('AR');
        Resolvers.TextBoxResolver.Selector('#APInvoice_InvoiceNumber').Type('Invoice # : ' + EntityNumber);
        Resolvers.TextBoxResolver.Selector('#APInvoice_AmountInInvoiceCurrency').Type('100');
        Resolvers.LOVResolver.Selector('#APInvoice_InvoiceCurrencyId').Type('NIS');
        Resolvers.DatePickerResolver.Selector('#date_APInvoice_InvoiceDate').Type('.');
        Resolvers.LOVResolver.Selector('#APInvoice_PaymentTermId').SelectFirst();
        Resolvers.ButtonResolver.Selector('#Ok-CreateAPInvoice').Click();
    }
    private FillInvoiceDetailes(EntityNumber: string) {
     //   Resolvers.ButtonResolver.Selector('#AddInvoiceLine').Click();
     //   Resolvers.LOVResolver.Selector('#APInvoiceLine_ChargesTypeId').SelectFirst();
     
        Resolvers.TextBoxResolver.Selector('#APInvoiceLine_InvoiceCurrencyAmount').Type('100');
        Resolvers.LOVResolver.Selector('#APInvoice_VatTypeId').Type('Zero');
        Resolvers.ButtonResolver.Selector('#VATApplyToAll').Click();
        
     //   Resolvers.ButtonResolver.Selector('#Ok-AddInvoiceLine').Click();
        //cy.get('#APInvoiceBSave').click();
        //cy.get('#APInvoiceBApprove').click();
    }
    private BackToShipmentsTab() {
        Resolvers.ButtonResolver.Selector('#BackButton_1').Click();
    }
    private SaveAP() {
        cy.get('#APInvoiceBSave').click();
        cy.server();
        cy.route({
            method: 'GET',
            url: '**/GetInvoiceOpenAmountPayables?**',
            onResponse: (xhr) => {
                expect(xhr.status).to.eq(200);
            }
        }).as('SaveInvoice');
        cy.wait('@SaveInvoice');
    }
    private ApproveAP() {
   
    cy.server();
    cy.route({
        method: 'GET',
        url: '**/GetValidateInvoiceDate?**',
        onResponse: (xhr) => {
            expect(xhr.status).to.eq(200);
        }
        }).as('ApproveInvoice');
        cy.get('#APInvoiceBApprove').click();
        cy.wait('@ApproveInvoice');       
    }
    private GoToGeneralTabInAPInvoice() {
        cy.get('#APInvoiceTHGeneral').click();
        Resolvers.LOVResolver.Selector('#APInvoice_BranchId').SelectFirst();
    }
}