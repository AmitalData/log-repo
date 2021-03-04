import * as gr from '../../../Base/cypress/Actions/GenerateRandoms';
import { AccountingSelectors } from "../selectors/Selectors";
import { BaseSelectors } from "../../../Base/cypress/selectors/BaseSelectors";
import { RequestAliases } from "../../../Base/cypress/constants/RequestAliases";
import { ShipmentSelectors } from "../../../Shipment/cypress/selectors/Selectors";
import { APInvoiceDetails } from "cypress/models/APInvoiceDetails";
import * as BaseAssertion from '../../../Base/cypress/actions/Assertion';
import { AccountingURLs } from '../constants/URLs';
import { PayableDetails } from '../../../Shipment/cypress/models/PayableDetails';
import { ARInvoiceDetails } from 'cypress/models/ARInvoiceDetails';
import { APPaymentDetails } from 'cypress/models/APPaymentDetails';
import { ARPaymentDetails } from 'cypress/models/ARPaymentDetails';
import { BaseURLs } from '../../../Base/cypress/constants/URLs';
import { QuickSearchDetails } from '../../../Base/cypress/models/QuickSearchDetails';
import { RestAPI } from '../../../Base/cypress/constants/RestAPI'
import * as BaseActions from '../../../Base/cypress/actions/Actions';
import { intersection } from 'cypress/types/lodash';

export function NavigatesToAccountingMenu() {
    cy.Click(BaseSelectors.AccountingMenu, null)
}
export function NavigatesToAccountingSettings() {
    NavigatesToAccountingMenu();
    cy.Click(AccountingSelectors.AccountingSettings, null);

}
export function NavigatesToAccountingTransfer() {
    NavigatesToAccountingMenu();
    cy.Click(AccountingSelectors.AccountingTransfer, null)
}
export function NavigatesToNotReadyARInvoices() {
    NavigatesToAccountingTransfer()
    cy.Click("#ARInvoicesTransfersection", AccountingSelectors.ContainNotReadyInvoices)
}
export function NavigatesToNewTransferARInvoices() {
    NavigatesToAccountingTransfer()
    cy.Click("#ARInvoicesTransfersection", AccountingSelectors.ContainNewTransfer)
}
export function changeAccountingsSystem(AccountingsSystem: string, ExternalTransmissionType?: string) {
    NavigatesToAccountingSettings()
    cy.Click(BaseSelectors.buttonspan, AccountingSelectors.ContainAccountingSystem, true)
    cy.SelectDropDownListItem(AccountingSelectors.LogLovAccountingSettingAccountingSystemCode, AccountingsSystem)
    //cy.FillLogLov(AccountingSelectors.AccountingSystemType, AccountingsSystem, true)
    if (AccountingsSystem != AccountingSelectors.ContainNone) {
        cy.SelectCheckBox(AccountingSelectors.IsARInvoicesTransferEnabled)
        cy.SelectCheckBox(AccountingSelectors.IsAPInvoicesTransferEnabled)
        cy.SelectCheckBox(AccountingSelectors.IsARPaymentsTransferEnabled)
        cy.SelectCheckBox(AccountingSelectors.IsAPPaymentsTransferEnabled)
        ExternalTransmission(ExternalTransmissionType);
    }
    cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
}
export function ExternalTransmission(ExternalTransmissionType: string) {
    cy.Click(BaseSelectors.ComboBoxLast, null)
    cy.get(BaseSelectors.FillParentClass).find(BaseSelectors.TextTrimming).contains(ExternalTransmissionType).click()
}
//#region CustomInvoices
export function NewCustomsCreditNoteARInvoice() {
    cy.Click(BaseSelectors.ToggleButtonClass, AccountingSelectors.ContainsCustoms, true);
    cy.Click(AccountingSelectors.CreateCustomsCreditNote, null, false);
}
export function NewCustomsARInvoice() {
    cy.Click(BaseSelectors.ToggleButtonClass, AccountingSelectors.ContainsCustoms, true);
    cy.Click(AccountingSelectors.CreateCustomsARInvoice, null, false);
}
//#endregion
export function NavigatesToAccountsPayablesWorkspace() {
    NavigatesToAccountingMenu();
    cy.Click(AccountingSelectors.PayableAccountingTab, null)
}
export function NavigatesToAccountsReceivableWorkspace() {
    NavigatesToAccountingMenu();
    cy.Click(AccountingSelectors.ReceivableAccounting, null)
}
//#region APInvoice
export function FillAPInvoiceDetails(aPInvoiceDetails: APInvoiceDetails, multiple = false, havePayableVendor?: string) {
    var generatedInvoiceNumber = "AP" + gr.GenerateRandomNumber(10000, 99999).toString();
    cy.FillLogLov(AccountingSelectors.APInvoiceVendor, aPInvoiceDetails.Vendor, false);
    cy.FillLogTextBox(AccountingSelectors.APInvoiceInvoiceNumber, generatedInvoiceNumber)
    cy.FillLogTextBox(AccountingSelectors.APInvoiceAmountInInvoice, aPInvoiceDetails.InvoiceAmount.toString());
    cy.FillLogLov(AccountingSelectors.APInvoiceInvoiceCurrency, aPInvoiceDetails.InvoiceCurrency, true);
    cy.FillLogTextBox(AccountingSelectors.APInvoiceInvoiceExchangeRate, aPInvoiceDetails.InvoiceExchangeRate.toString());
    cy.FillDate(AccountingSelectors.APInvoiceInvoiceDate, aPInvoiceDetails.InvoiceDate)
    cy.FillLogLov(AccountingSelectors.APInvoicePaymentTerm, aPInvoiceDetails.PaymentTerms, true);
    cy.FillDate(AccountingSelectors.APInvoiceDueDate, aPInvoiceDetails.DueDate)
    cy.FillLogTextBox(AccountingSelectors.APInvoiceVATNumber, aPInvoiceDetails.VatNo.toString())
    cy.FillLogLov(AccountingSelectors.APInvoiceBranch, aPInvoiceDetails.Branch, true)
    cy.Click(AccountingSelectors.OkCreateAPInvoiceButton, null);
    if (!multiple) {
        if (!havePayableVendor) {
            cy.Click(BaseSelectors.CheckBoxLine, null)
        }
        cy.FillLogLov(AccountingSelectors.APInvoiceVatType, aPInvoiceDetails.VATType, true)
        cy.Click(AccountingSelectors.VatTypeApplyToAll, null)
    }
}
export function ReceiveAPInvoice() {
    cy.DefineRequestWait(RestAPI.POST, AccountingURLs.APInvoices, RequestAliases.APInvoicesRequest)
    cy.DefineRequestWait(RestAPI.POST, AccountingURLs.InvoiceDomain, RequestAliases.InvoiceDomain)
    cy.Click(AccountingSelectors.APInvoiceSaveButton, null)
    BaseAssertion.AssertStatusCode(RequestAliases.InvoiceDomain, 200).then((interception) => {
        if (interception.response.body) {
            ClickOnSaveOnConfirmWindow()
        }
    })
}

export function SaveAPInvoice() {
    cy.DefineRequestWait(RestAPI.POST, AccountingURLs.InvoiceDomain, RequestAliases.InvoiceDomain)
    cy.Click(AccountingSelectors.APInvoiceSaveButton, null)
}
export function APApproveInvoice() {
    cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.APInvoices, RequestAliases.APInvoicesRequest)
    cy.DefineRequestWait(RestAPI.POST, AccountingURLs.InvoiceDomain, RequestAliases.InvoiceDomain)
    cy.Click(AccountingSelectors.APInvoiceApproveButton, null)
    BaseAssertion.AssertStatusCode(RequestAliases.InvoiceDomain, 200).then((interception) => {
        if (interception.response.body) {
            ClickOnSaveOnConfirmWindow()
        }
    })
}
export function APInvoiceCancelApproval() {
    cy.Click(BaseSelectors.MoreList, null, true)
    cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.APInvoices, RequestAliases.APInvoicesRequest)
    cy.Click(AccountingSelectors.APInvoiceCancelApprovalButton, null)
}

export function VoidAPInvoice() {
    cy.Click(BaseSelectors.MoreList, null, true)
    cy.Click(AccountingSelectors.APInvoiceVoidButton, null)
    cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.APInvoices, RequestAliases.APInvoicesRequest)
    cy.Click(ShipmentSelectors.ConfirmWindowYes, null);
}

//#endregion
//#region ARInvoice
export function FillARInvoiceDetails(aRInvoiceDetails: ARInvoiceDetails) {
    // cy.get(".ComboBox").click();
    // cy.get(".FillParent").find(".TextTrimming").contains("Customer").click()
    if (aRInvoiceDetails.Partner) {
        cy.FillLogLov(AccountingSelectors.ARInvoicePartner, aRInvoiceDetails.Partner, false)
    }
    cy.FillLogLov(AccountingSelectors.ARInvoiceInvoiceCurrency, aRInvoiceDetails.InvoiceCurrency, true)
    cy.FillLogTextBox(AccountingSelectors.ARInvoiceExchangeRate, aRInvoiceDetails.InvoiceExchangeRate.toString());
    cy.FillDate(AccountingSelectors.ARInvoiceInvoiceDate, aRInvoiceDetails.InvoiceDate)
    cy.FillLogLov(AccountingSelectors.ARInvoicePaymentTerm, aRInvoiceDetails.PaymentTerms, true)
    cy.FillDate(AccountingSelectors.ARInvoiceDueDate, aRInvoiceDetails.DueDate)
    cy.FillLogTextBox(AccountingSelectors.ARInvoiceVatNumber, aRInvoiceDetails.VATNo)
    cy.FillLogLov(AccountingSelectors.ARInvoiceBranch, aRInvoiceDetails.Branch, true)
    if (aRInvoiceDetails.Partner) {
        cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK)
        cy.Click(BaseSelectors.CheckBoxLine, null)
    }
    else {
        cy.Click(AccountingSelectors.OkCreateARInvoiceButton, null);
        cy.FillLogLov(AccountingSelectors.ARInvoiceVatType, aRInvoiceDetails.VATType, true)
        cy.DefineRequestWait(RestAPI.GET, AccountingURLs.VatTypePercentageCall, RequestAliases.GetVatTypePercentage)
        cy.Click(AccountingSelectors.VatTypeApplyToAll, null)
        cy.wait('@' + RequestAliases.GetVatTypePercentage)
    }
}

export function CreateARInvoice(Constituent?: boolean) {
    cy.DefineRequestWait(RestAPI.POST, AccountingURLs.ARInvoices, RequestAliases.ARInvoicesRequest)
    if (Constituent) {
        cy.Click(AccountingSelectors.GeneralSave, null)

    }
    else {
        cy.Click(AccountingSelectors.ARInvoiceSaveButton, null)

    }
}
export function ARApproveInvoice() {
    cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.ARInvoices, RequestAliases.ARInvoicesRequest)
    cy.Click(AccountingSelectors.ARInvoiceApproveButton, null)
}

export function SetAsSentARInvoice() {
    cy.Click(BaseSelectors.MoreList, null, true)
    cy.Click(AccountingSelectors.ARInvoiceSetAsSentButton, null)
    cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.ARInvoices, RequestAliases.ARInvoicesRequest)
    cy.Click("button", "Confirm");
}

export function VoidARInvoice() {
    cy.Click(BaseSelectors.MoreList, null, true)
    cy.Click(AccountingSelectors.ARInvoiceVoidButton, null)
    cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.ARInvoices, RequestAliases.ARInvoicesRequest)
    cy.Click(ShipmentSelectors.ConfirmWindowYes, null);
}

export function CancelDraftARInvoice() {
    cy.Click(BaseSelectors.MoreList, null, true)
    cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.ARInvoices, RequestAliases.ARInvoicesRequest)
    cy.Click(AccountingSelectors.ARInvoiceCancelDraftButton, null)
    cy.Click(ShipmentSelectors.ConfirmWindowYes, null)
}
//#endregion
//#region Add Two Shipment Lines And Edit Amount
export function AddTwoShipmentLinesAndEditAmount(shipmentNumbers: string[], VATType: string, payableDetails: PayableDetails) {
    const amount = CalculateAmount(payableDetails.Quantity, payableDetails.UnitPrice);
    for (let i = 0; i < shipmentNumbers.length; i++) {
        var quickSearchDetails = {
            Selector: ShipmentSelectors.ShipmentSearchBar,
            Parent: ShipmentSelectors.ShipmentSearchParent,
            ParentClass: ShipmentSelectors.ShipmentSearchParentClass,
            WaitURL: BaseURLs.GetQuickSearch,
            Value: shipmentNumbers[i]
        } as QuickSearchDetails;
        cy.SelectQuickSearchFirstElement(quickSearchDetails);

        cy.Click(AccountingSelectors.EditShipmentLineIcon(shipmentNumbers[i]), null, true)
        cy.FillLogTextBox(AccountingSelectors.APInvoiceLineForiegnCurrencyAmount, amount.toString());
        cy.FillLogLov(AccountingSelectors.APInvoiceVatType, VATType, true);
        cy.Click(BaseSelectors.Button, BaseSelectors.ContainsApplytoall);
        cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.InvoiceDomain, RequestAliases.APInvoicesRequest);
        cy.Click(BaseSelectors.RedButton, BaseSelectors.ContainsOK);
        BaseAssertion.AssertStatusCode(RequestAliases.APInvoicesRequest, 200);
    }
    const totalAmount = amount * shipmentNumbers.length;
    cy.FillLogTextBox(AccountingSelectors.APInvoiceAmountInInvoiceCurrency, totalAmount.toString());
}
//#endregion
//#region CalculateAmount
function CalculateAmount(Quantity: number, UnitPrice: number): number {
    return Quantity * UnitPrice;
}
//#endregion
//#region ConsolidationInvoice
export function FillconsolidationInvoiceDetails(ARInvoiceData: ARInvoiceDetails) {
    NavigatesToAccountsReceivableWorkspace()
    cy.Click(BaseSelectors.ToggleIcon, null)
    cy.Click(BaseSelectors.button, "New consolidation invoice")
    FillARInvoiceDetails(ARInvoiceData)

}
export function ApproveConsilidationInvoice() {
    //cy.DefineRequestWait(RestAPI.POST, AccountingURLs.ConsilidationInvoiceDomain, RequestAliases.ConsilidationInvoiceDomain)
    cy.DefineRequestWait(RestAPI.GET, AccountingURLs.ARInvoicesGetSingle, RequestAliases.ARInvoicesRequest)

    cy.Click(AccountingSelectors.ARInvoiceApproveButton, null)
}
//#endregion


//#region APPayment
export function FillAPPayment(aPPaymentDetails: APPaymentDetails, invoiceNumber: string) {
    NavigatesToAccountingMenu();
    cy.Click(AccountingSelectors.PayableAccountingTab, null)
    cy.Click(AccountingSelectors.NewAPPayment, null)
    cy.FillLogLov(AccountingSelectors.APPaymentVendor, aPPaymentDetails.Vendor, false)
    cy.FillLogLov(AccountingSelectors.APPaymentMethod, aPPaymentDetails.PaymentMethod, true)
    cy.FillLogTextBox(AccountingSelectors.APPaymentAmount, aPPaymentDetails.PaymentAmount.toString())
    cy.FillLogLov(AccountingSelectors.APPaymentCurrency, aPPaymentDetails.PaymentCurrency, true)
    cy.FillLogTextBox(AccountingSelectors.APPaymentCurrencyExchangeRate, aPPaymentDetails.Rate.toString())
    cy.FillDate(AccountingSelectors.APPaymentRegisterDate, aPPaymentDetails.RegisterDate)
    cy.FillLogLov(AccountingSelectors.APPaymentBranch, aPPaymentDetails.Branch, true)
    cy.DefineRequestWait(RestAPI.GET, AccountingURLs.APInvoiceViewsGetByFilters, RequestAliases.APInvoiceView)
    cy.FillLogTextBox(BaseSelectors.SearchField, invoiceNumber);
    BaseAssertion.AssertStatusCode(RequestAliases.APInvoiceView, 200)
    cy.Click(BaseSelectors.CheckBoxLine, null)

}

export function SaveAPPayment() {
    cy.DefineRequestWait(RestAPI.POST, AccountingURLs.APPayments, RequestAliases.APPayments)
    cy.Click(AccountingSelectors.APPaymentSaveButton, null)
}

export function ApproveAPPayment() {
    cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.APPayments, RequestAliases.APPayments)
    cy.Click(AccountingSelectors.APPaymentApproveButton, null)
}

export function PayAPInvoice() {
    SaveAPPayment()
    ApproveAPPayment()

}
//#endregion
//#region ARPayment
export function FillARPaymentDetails(aRPaymentDetails: ARPaymentDetails) {
    if (aRPaymentDetails.Partner) {
        cy.FillLogLov(AccountingSelectors.ARPaymentPartner, aRPaymentDetails.Partner, false)
    }
    cy.FillLogLov(AccountingSelectors.ARPaymentPaymentMethod, aRPaymentDetails.PaymentMethod, true)
    cy.FillLogTextBox(AccountingSelectors.ARPaymentAmount, aRPaymentDetails.PaymentAmount)
    cy.Click(AccountingSelectors.OkAddARPayment, null)
}

export function SaveARPayment() {
    cy.DefineRequestWait(RestAPI.POST, AccountingURLs.ARPayments, RequestAliases.ARPayments)
    cy.Click(AccountingSelectors.ARPaymentSave, null)
}

export function ApproveARPayment() {
    cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.ARPayments, RequestAliases.ARPayments)
    cy.Click(AccountingSelectors.ARPaymentBApprove, null)
}

export function PayARInvoice() {
    SaveARPayment()
    BaseAssertion.AssertStatusCode(RequestAliases.ARPayments, 200)
    ApproveARPayment()
}

export function NewARPaymentFromAccounting(aRPaymentDetails: ARPaymentDetails, invoiceNumber: string) {
    NavigatesToAccountsReceivableWorkspace()
    cy.Click(AccountingSelectors.QueryLink, AccountingSelectors.NewPayment)
    FillARPaymentDetails(aRPaymentDetails)
    cy.DefineRequestWait(RestAPI.GET, AccountingURLs.ARInvoiceViewsGetByFilters, RequestAliases.ARInvoiceviews)
    cy.FillLogTextBox(BaseSelectors.SearchField, invoiceNumber);
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoiceviews, 200)
    cy.wait(10000)
    cy.Click(BaseSelectors.CheckBoxLine, null)

}
//#endregion
//#region navigates to draft invoice
export function NavigatesToDraftInvoice(draftConsolidationInvoiceNumber: string) {
    NavigatesToAccountsReceivableWorkspace();
    cy.DefineRequestWait(RestAPI.GET, AccountingURLs.ARInvoiceViewsGetByFilters, RequestAliases.ARInvoiceviews);
    cy.DefineRequestWait(RestAPI.GET, AccountingURLs.ARInvoiceViewsGetByFilters, RequestAliases.ARInvoiceviews);
    cy.DefineRequestWait(RestAPI.GET, AccountingURLs.ARInvoiceViewsGetByFilters, RequestAliases.ARInvoiceviews);
    cy.Click(AccountingSelectors.QueryLink, AccountingSelectors.ContainDraftInvoices);
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoiceviews, 200);
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoiceviews, 200);
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoiceviews, 200);
    cy.DefineRequestWait(RestAPI.GET, AccountingURLs.ARInvoiceViewsGetByFilters, RequestAliases.ARInvoiceviews);
    cy.DefineRequestWait(RestAPI.GET, AccountingURLs.ARInvoiceViewsGetByFilters, RequestAliases.ARInvoiceviews);
    cy.FillLogTextBox(BaseSelectors.SearchField, draftConsolidationInvoiceNumber);
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoiceviews, 200);
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoiceviews, 200);
    cy.DefineRequestWait(RestAPI.GET, AccountingURLs.ARInvoicesGetSingle, RequestAliases.ARInvoicesRequest);
    cy.DefineRequestWait(RestAPI.GET, AccountingURLs.ARInvoiceViewsGetByFilters, RequestAliases.ARInvoiceviews);
    cy.DefineRequestWait(RestAPI.GET, AccountingURLs.ARInvoiceViewsGetByFilters, RequestAliases.ARInvoiceviews);
    cy.Click(BaseSelectors.ListItem, null);

    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoicesRequest, 200);
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoiceviews, 200);
    BaseAssertion.AssertStatusCode(RequestAliases.ARInvoiceviews, 200);
}
//#endregion
export function AddSecondInvoiceToConsolidation(ARInvoiceNumber: string) {
    cy.DefineRequestWait(RestAPI.PUT, AccountingURLs.ARInvoices, RequestAliases.ARInvoicesPutRequest);
    cy.get("[data-cy='CheckBox" + ARInvoiceNumber + "']").click();
}
export function AssertTransferStatus(TransferStatus: string) {
    cy.get(BaseSelectors.HeaderScreen).eq(1)
        .should("contain", TransferStatus)
}
export function AssertTransferError(ARInvoiceNumber: string, ErrorMessage: string) {
    cy.get(BaseSelectors.DivListItem).find(BaseSelectors.TextTrimming).contains(ARInvoiceNumber)
        .parents(BaseSelectors.RowClass).find(BaseSelectors.TextTrimming)
        .contains(ErrorMessage)
}
export function FillExternalID(NotReadyValue: string, ExternalID: string) {
    let EntitySelector: string;
    let ExternalIdSelectorTextBox: string
    let SaveCloseButton: string;
    let Edit: string
    if (NotReadyValue == BaseSelectors.TestCompany) {
        EntitySelector = AccountingSelectors.LogLovARInvoicePartnerId;
        ExternalIdSelectorTextBox = BaseSelectors.CustomerReceivablesAccountingCard;
        SaveCloseButton = BaseSelectors.CustomerSaveClose
        Edit = AccountingSelectors.EditBillTo
    }
    else if (NotReadyValue == BaseSelectors.EUR) {
        EntitySelector = ShipmentSelectors.LogLovShipmentReceivableCurrencyId;
        ExternalIdSelectorTextBox = BaseSelectors.CurrencyAccountingExternalCode
        SaveCloseButton = BaseSelectors.CurrencySaveClose
        Edit = AccountingSelectors.EditInvoiceCurrency
    }
    else if (NotReadyValue == BaseSelectors.AirFreight) {
        EntitySelector = ShipmentSelectors.LogLovShipmentReceivableChargesTypeId;
        ExternalIdSelectorTextBox = BaseSelectors.ChargesTypeReceivableCreditAccount
        SaveCloseButton = BaseSelectors.ChargesTypeSaveClose
        Edit = AccountingSelectors.EditChargeType
    }
    cy.Click(Edit, null, true)
    cy.Click(BaseSelectors.LogitudeWindow, BaseSelectors.ContainsAccounting);
    cy.FillLogTextBox(ExternalIdSelectorTextBox, ExternalID)
    cy.Click(SaveCloseButton, null);
    BaseAssertion.AssertElementNotExist(SaveCloseButton)
}
export function ARInvoiceSearch(ARInvoiceNumber: string) {
    cy.DefineRequestWait(RestAPI.POST, AccountingURLs.PerformancelogsPostLogsList, RequestAliases.performancelogs);
    cy.FillLogTextBox(BaseSelectors.SearchField,ARInvoiceNumber)
    BaseAssertion.AssertStatusCode(RequestAliases.performancelogs, 200);
}
export function ARInvoiceSearchInTransferScreen(ARInvoiceNumber: string) {
    cy.DefineRequestWait(RestAPI.POST, AccountingURLs.PerformancelogsPostLogsList, RequestAliases.performancelogs);
    cy.FillLogTextBox(BaseSelectors.NullSearch,ARInvoiceNumber)
    BaseAssertion.AssertStatusCode(RequestAliases.performancelogs, 200);
}
export function ClickOnRowDependingOnARInvoiceNumber(ARInvoiceNumber: string) {
    BaseActions.ClickOnRowDependingOnValue(ARInvoiceNumber)
}
export function ExportARInvoice(ARInvoiceNumber: string) {
    cy.get(AccountingSelectors.TransferCheckBox(ARInvoiceNumber)).find(BaseSelectors.label).click({force:true});
    cy.Click(BaseSelectors.RedButton, AccountingSelectors.ContainExport)
}
export function CloseExportingInvoiceTransferWindow() {
    cy.get(BaseSelectors.ColorGreenClass).contains(AccountingSelectors.ContainTransferredSuccessfully)
        .parents(BaseSelectors.LogitudeWindow)
        .find(BaseSelectors.button).contains(BaseSelectors.ContainClose).click()
}
export function AssertTransferredInvoice() {
    cy.get(BaseSelectors.ColorGreenClass).should('contain.text', AccountingSelectors.ContainTransferredSuccessfully)
}
function ClickOnSaveOnConfirmWindow() {
    cy.get(BaseSelectors.ConfirmWindow).find(BaseSelectors.RedButton).contains(BaseSelectors.ContainSave).click()

}