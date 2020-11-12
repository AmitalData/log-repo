import { Resolvers } from "../../Resolvers/Resolvers";
import { Random } from "../../@e2e/core";

export class PayablesTab {
    public EntityNumber: string;

    public RunPayablesTabScenarios() {
        this.EntityNumber = Random.GetRandomNumber();
        this.GoToPayablesTab();
        this.ReceiveInvoice(this.EntityNumber);
        this.FillInvoiceDetailes(this.EntityNumber);
        this.GoToGeneralTabInAPInvoice();
        this.SaveAP();
        this.BackToShipmentsTab();
    }
    private GoToPayablesTab() {
        cy.get('#ShipmentTHPayables').click();
    }
   
    private ReceiveInvoice(EntityNumber: string) {
        cy.get('#ReceiveInvoice').click();
        Resolvers.LOVResolver.Selector('#APInvoice_VendorId').Type('Automation Test');
        Resolvers.TextBoxResolver.Selector('#APInvoice_InvoiceNumber').Type('Invoice # : ' + EntityNumber);
        Resolvers.TextBoxResolver.Selector('#APInvoice_AmountInInvoiceCurrency').Type('100');
        Resolvers.LOVResolver.Selector('#APInvoice_InvoiceCurrencyId').Type('EUR');
        Resolvers.DatePickerResolver.Selector('#date_APInvoice_InvoiceDate').Type('.');
        Resolvers.LOVResolver.Selector('#APInvoice_PaymentTermId').SelectFirst();
        Resolvers.ButtonResolver.Selector('#Ok-CreateAPInvoice').Click();
    }
    private FillInvoiceDetailes(EntityNumber: string) {
        Resolvers.ButtonResolver.Selector('#AddInvoiceLine').Click();
        Resolvers.LOVResolver.Selector('#APInvoiceLine_ChargesTypeId').SelectFirst();
        Resolvers.LOVResolver.Selector('#APInvoiceLine_VatTypeId').Type('Zero');
        Resolvers.TextBoxResolver.Selector('#APInvoiceLine_InvoiceCurrencyAmount').Type('100');
        Resolvers.ButtonResolver.Selector('#Ok-AddInvoiceLine').Click();
        //cy.get('#APInvoiceBSave').click();
        //cy.get('#APInvoiceBApprove').click();
    }
    private BackToShipmentsTab() {
        Resolvers.ButtonResolver.Selector('#BackButton_1').Click();

    }
    private SaveAP() {
        cy.get('#APInvoiceBSave').click();
        cy.get('#APInvoiceBApprove').click();
    }
    private GoToGeneralTabInAPInvoice() {
        cy.get('#APInvoiceTHGeneral').click();
        Resolvers.LOVResolver.Selector('#APInvoice_BranchId').SelectFirst();
    }
}