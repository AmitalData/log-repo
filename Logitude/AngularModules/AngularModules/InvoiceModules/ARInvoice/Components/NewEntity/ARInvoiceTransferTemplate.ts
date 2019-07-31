import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ARInvoicePM} from '../../../../Invoice/EntityPMs/ARInvoicePM';
import {ARInvoiceLinePM} from '../../../../Invoice/EntityPMs/ARInvoiceLinePM';
import {AppTool} from '../../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {CurrencyListService} from '../../../../Common/Services/StandardLists/CurrencyListService';
import {CurrencyList} from '../../../../Common/EntityLists/CurrencyList';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {AccountingSystemListService} from '../../../../Common/Services/StandardLists/AccountingSystemListService';
import {AccountingSystemList} from '../../../../Common/EntityLists/AccountingSystemList';
import {VatTypeListService} from '../../../../Common/Services/StandardLists/VatTypeListService';
import {VatTypeList} from '../../../../Common/EntityLists/VatTypeList';
import {PaymentTermListService} from '../../../../Common/Services/StandardLists/PaymentTermListService';
import {PaymentTermList} from '../../../../Common/EntityLists/PaymentTermList';
import {ChargesTypeListService} from '../../../../Common/Services/StandardLists/ChargesTypeListService';
import {ChargesTypeList} from '../../../../Common/EntityLists/ChargesTypeList';
import {ChargeTypeAccountingPM} from '../../../../Common/EntityPMs/ChargeTypeAccountingPM';
import {ChargeTypeAccountingList} from '../../../../Common/EntityLists/ChargeTypeAccountingList';
import {InvoiceDomainService } from '../../../../Invoice/Services/InvoiceDomainService';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {ARInvoicePMService} from '../../../../Invoice/Services/StandardPMs/ARInvoicePMService';

@Component({
    moduleId: module.id,
    templateUrl: './ARInvoiceTransferTemplate.html',
})

export class ARInvoiceTransferTemplate extends BaseComponent {
    public EntityPM: ARInvoicePM;
    public ObjectTableName: string = "ARInvoice";
    public DataContext = this;
    public ItemsSource: ARInvoiceTransferLineArgs[] = [];
    public IsNew: boolean = false;
    public EntityId: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }
    InitTemplate(entity: ARInvoicePM) {
        this.EntityPM = entity;
        this.BuildList();
    }
    SetWindowArgs(args: any) {
        if (args) {
            this.EntityId = args.EntityId;
            this.IsNew = args.IsNewTemplate;
            this.GetSingleEntityPM();
        }
    }
    public GetSingleEntityPM() {
        var service = new ARInvoicePMService();
        service.get(this.EntityId).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var entityPM = response.Result;
                if (entityPM) {
                    this.EntityPM = entityPM;
                    this.BuildList();
                }
            }
        });
    }
    
    public RefreshData() {
        if (this.EntityPM != null) {
            this.BuildList();
        }
    }
    public BuildList() {
        this.ItemsSource = [];
        this.ItemsSource.push(new ARInvoiceTransferLineArgs(this.EntityPM, null, this, "BLTO"));
        this.ItemsSource.push(new ARInvoiceTransferLineArgs(this.EntityPM, null, this, "CURR"));
        this.EntityPM.InvoiceLines.forEach(item => {
            this.ItemsSource.push(new ARInvoiceTransferLineArgs(null, item, this, "Line"));
        });

        var listGrouped: InvoiceCodeNameClass[] = [];
        this.EntityPM.InvoiceLines.filter(f => f.VatTypeId != null).forEach(item => {
            var itemGrouped: InvoiceCodeNameClass = listGrouped.filter(f => f.VatTypeId == item.VatTypeId)[0];
            if (itemGrouped == null) {
                itemGrouped = new InvoiceCodeNameClass();
                itemGrouped.VatTypeId = item.VatTypeId;
                listGrouped.push(itemGrouped);
            }
        });
        listGrouped.forEach(item => {
            var linePM: ARInvoiceLinePM = this.EntityPM.InvoiceLines.filter(d => d.VatTypeId == item.VatTypeId && d.VatPercentage != null)[0];
            if (linePM != null) {
                if (linePM.VatPercentage != 0) {
                    this.ItemsSource.push(new ARInvoiceTransferLineArgs(null, linePM, this, "TAX"));
                }
            }
        });

        this.UpdateTransferData();
    }

    //UpdateTransferData
    UpdateTransferData() {
        if (this.EntityPM.TransferStatusCode != "TR" && this.EntityPM.TransferStatusCode != "IP" && this.EntityPM.TransferStatusCode != "ET") {
            var isReady = true;
            var isValid = this.ItemsSource.filter(d => AppTool.IsNullOrEmpty(d.EditingFieldValue))[0];
            if (isValid != null) {
                isReady = false;
            }
            if (this.EntityPM.TransferStatusCode != "BL") {
                this.TransferStatusCode = isReady ? "RD" : "NR";
            }
        }
    }

    //Properties
    get IsEditingEnabled() {
        var myResult = true;

        if (this.TransferStatusCode == "TR") {
            myResult = false;
        }

        else if (this.EntityPM != null && this.EntityPM.StatusCode == "LL") {
            myResult = false;
        }

        return myResult;
    }
    get SetBlockedButtonVisibility() {
        var result = false;
        if (this.TransferStatusCode != "TR") {
            if (this.TransferStatusCode != "BL") {
                result = true;
            }
        }
        return result;
    }
    get SetUnBlockedButtonVisibility() {
        var result = false;
        if (this.TransferStatusCode != "TR") {
            if (this.TransferStatusCode == "BL") {
                result = true;
            }
        }
        return result;
    }
    get TransferStatusCode() {
        if (this.EntityPM != null) {
            return this.EntityPM.TransferStatusCode;
        }
    }
    set TransferStatusCode(value: string) {
        if (this.EntityPM.TransferStatusCode != value) {
            this.EntityPM.TransferStatusCode = value;
            switch (value) {
                case "RD": { this.TransferStatusName = "Ready"; break; }
                case "NR": { this.TransferStatusName = "Not Ready"; break; }
                case "BL": { this.TransferStatusName = "Blocked"; break; }
                default: { this.TransferStatusName = "Transferred"; break; }
            }
        }
    }
    get IsReadyForTransfer() {
        return this.TransferStatusCode == "RD" ? true : false;
    }
    get TransferStatusName() {
        if (this.EntityPM != null && this.EntityPM.TransferStatusName != null) {
            return this.EntityPM.TransferStatusName;
        }

        else {
            return "Not Ready";
        }
    }
    set TransferStatusName(value: string) {
        if (this.EntityPM.TransferStatusName != value) {
            this.EntityPM.TransferStatusName = value;
        }
    }

    // Block Commands
    SetBlockedTransferClicked() {
        this.TransferStatusCode = "BL";
        this.UpdateTransferData();
    }
    SetUnBlockedTransferClicked() {
        this.TransferStatusCode = "NR";
        this.UpdateTransferData();
    }

    // Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    public ValidationErrorsList = [];
    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            var service = new ARInvoicePMService();
            service.update(this.EntityPM).subscribe((response: ServiceResponse) => {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                if (!response.HasError) {
                    this.CurrentSession.CloseCurrentWindow();
                }
                else {
                    this.ValidationErrorsList = response.ErrorsArray;
                }
            });
        }
    }
}

export class ARInvoiceTransferLineArgs extends BaseComponent {
    private invoicePM: ARInvoicePM;
    private invoiceLinePM: ARInvoiceLinePM;
    public Code: string;
    public ObjectTableName: string;
    public DataContext = this;
    public VatPercentage: number;

    constructor(entity: ARInvoicePM, line: ARInvoiceLinePM, public father: ARInvoiceTransferTemplate, typeCode: string) {
        super();
        this.InitalizeServices();
        if (entity != null) {
            this.invoicePM = entity;
            this.ObjectTableName = "ARInvoice";
        }

        if (line != null) {
            this.invoicePM = father.EntityPM;
            this.invoiceLinePM = line;
            this.ObjectTableName = "ARInvoiceLine";
            this.VatPercentage = line.VatPercentage;            
        }

        this.Code = typeCode;
        this.GetEditingFieldName();
        this.GetDescriptionTitle();
        this.GetDescriptionValue();
        this.GetDescriptionHelp();
        this.GetDescriptionHelpVisibility();
        this.SetUIProperties();
        this.FillFieldsData();
    }

    private CardListService: CardListService;
    private CurrencyListService: CurrencyListService;
    private VatTypeListService: VatTypeListService;
    private PaymentTermListService: PaymentTermListService;
    private ChargesTypeListService: ChargesTypeListService;
    private InvoiceDomainService: InvoiceDomainService;
    InitalizeServices() {
        this.CardListService = new CardListService();
        this.CurrencyListService = new CurrencyListService();
        this.VatTypeListService = new VatTypeListService();
        this.PaymentTermListService = new PaymentTermListService();
        this.ChargesTypeListService = new ChargesTypeListService();
        this.InvoiceDomainService = new InvoiceDomainService();
    }

    // FillFieldsData
    private FillFieldsData() {
        if (this.IsTransferredEnabled) {
            if (AppTool.IsNullOrEmpty(this.EditingFieldValue)) {
                switch (this.Code) {
                    case "BLTO":
                        {
                            this.GetBillToData();                            
                            break;
                        }

                    case "CURR":
                        {
                            this.GetCurrencyData();
                            break;
                        }

                    case "VAT":
                        {
                            this.GetVatTypeData();
                            break;
                        }
                    case "TAX":
                        {
                            if (SessionLocator.AccountingSettingPM.AccountingSystemCode == "HV" || SessionLocator.AccountingSettingPM.AccountingSystemCode == "RH") {
                                this.GetAccountingSystemData();
                            }
                            else {
                                this.GetVatTypeData();
                            }

                            break;
                        }
                    case "PYTM":
                        {
                            this.GetPaymentTermData();
                            break;
                        }
                    default: {
                        this.GetChargesTypeData();
                        break;
                    }
                }
            }
        }
    }
    private GetBillToData() {
        this.CardListService.getSingle(this.invoicePM.BillToId).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var card: CardList = response.Result;
                if (card != null) {
                    this.EditingFieldValue = card.ReceivablesAccountingCard;
                }
            }
        });
    }
    private GetCurrencyData() {
        this.CurrencyListService.getSingle(this.invoicePM.InvoiceCurrencyId).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var currency: CurrencyList = response.Result;
                if (currency != null) {
                    this.EditingFieldValue = currency.AccountingExternalCode;
                }
            }
        });
    }
    private GetVatTypeData() {
        this.VatTypeListService.getSingle(this.invoiceLinePM.VatTypeId).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var vat: VatTypeList = response.Result;
                if (vat != null) {
                    this.EditingFieldValue = vat.ExternalTAXItemId;
                }
            }
        });
    }
    private GetPaymentTermData() {
        this.PaymentTermListService.getSingle(this.invoicePM.PaymentTermId).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var payment: PaymentTermList = response.Result;
                if (payment != null) {
                    this.EditingFieldValue = payment.ExternalId;
                }
            }
        });
    }
    private GetChargesTypeData() {
        this.ChargesTypeListService.getSingle(this.invoiceLinePM.ChargesTypeId).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var charge: ChargesTypeList = response.Result;
                if (charge != null) {
                    if (charge.AccountingVATSplit) {
                        this.GetChargeTypeAccountingList();
                    }
                    else {
                        this.EditingFieldValue = charge.ReceivableCreditAccount;
                    }
                }
            }
        });
    }
    private GetChargeTypeAccountingList() {
        this.InvoiceDomainService.GetSingleChargeTypeAccountingList(this.invoiceLinePM.ChargesTypeId, this.invoiceLinePM.VatTypeId).subscribe((respo: ServiceResponse) => {
            if (!respo.HasError) {
                var list: ChargeTypeAccountingList = respo.Result;
                if (list != null) {
                    this.EditingFieldValue = list.ReceivableCreditAccount;
                }
            }
        });
    }
    private GetAccountingSystemData() {
        this.EditingFieldValue = SessionLocator.AccountingSettingPM.ReceivableVATCard;
    }

    // SetUIProperties
    private SetUIProperties() {
        this.UIProperties.SetEnabled(this.EditingFieldName, this.ObjectTableName, this.IsTransferredEnabled);
    }

    // Props
    public DescriptionTitle: string = "";
    private GetDescriptionTitle() {
        var result = "";
        switch (this.Code) {
            case "BLTO":
                {
                    result = "Bill To";
                    break;
                }
            case "CURR":
                {
                    result = "Invoice Currency";
                    break;
                }
            case "PYTM":
                {
                    result = "Payment Term";
                    break;
                }
            case "VAT":
                {
                    result = "VAT Type Item Code";
                    break;
                }
            case "TAX":
                {
                    result = "VAT Type Tax Code";
                    break;
                }
            default:
                {
                    result = "Charge Type";
                    break;
                }
        }
        this.DescriptionTitle = result;
    }

    public DescriptionValue: string = "";
    private GetDescriptionValue() {
        var result = "";
        switch (this.Code) {
            case "BLTO":
                {
                    result = this.invoicePM.BillToName;
                    break;
                }

            case "CURR":
                {
                    result = this.invoicePM.InvoiceCurrencyCode;
                    break;
                }

            case "PYTM":
                {
                    result = this.invoicePM.PaymentTermName;
                    break;
                }


            case "VAT":
                {
                    result = this.invoiceLinePM.VatTypeName;
                    break;
                }

            case "TAX":
                {
                    result = this.invoiceLinePM.VatTypeName;
                    break;
                }

            default:
                {
                    result = this.invoiceLinePM.Description;
                    break;
                }
        }

        this.DescriptionValue = result;
    }

    public DescriptionHelp: string = "";
    private GetDescriptionHelp() {
        var result = "";
        switch (this.Code) {
            case "BLTO":
                {
                    result = "Please enter debit account";
                    break;
                }

            case "CURR":
                {
                    result = "Please enter external code for " + this.invoicePM.InvoiceCurrencyCode;
                    break;
                }

            case "PYTM":
                {
                    result = "Please enter external payment term";
                    break;
                }


            case "VAT":
                {
                    result = "Please enter external vat card";
                    break;
                }

            case "TAX":
                {
                    result = "Please enter external tax code";
                    break;
                }

            default:
                {
                    result = "Please enter credit account";
                    break;
                }
        }
        this.DescriptionHelp = result;
    }

    public DescriptionHelpVisibility = false;
    GetDescriptionHelpVisibility() {
        var result = false;
        if (AppTool.IsNullOrEmpty(this.EditingFieldValue)) {
            result = true;
        }
        this.DescriptionHelpVisibility = result;
    }
    get IsTransferredEnabled() {
        var myResult = true;
        if (this.father.TransferStatusCode == "TR") {
            myResult = false;
        }
        else if (this.invoicePM.StatusCode == "DR" || this.invoicePM.StatusCode == "LL") {
            myResult = false;
        }
        return myResult;
    }

    public EditingFieldName: string;
    GetEditingFieldName() {
        var result = "";
        switch (this.Code) {
            case "BLTO":
                {
                    result = "DebitAccount";
                    break;
                }

            case "CURR":
                {
                    result = "AccountingExternalCode";
                    break;
                }

            case "PYTM":
                {
                    result = "PaymentTermExternalId";
                    break;
                }

            case "VAT":
                {
                    result = "ExternalTAXItemId";
                    break;
                }

            case "TAX":
                {
                    result = "ExternalVATCard";
                    break;
                }

            default:
                {
                    result = "CreditAccount";
                    break;
                }
        }
        this.EditingFieldName = result;
    }

    get EditingFieldValue()
    {
        var result = "";
        switch (this.Code) {
            case "BLTO":
                {
                    result = this.invoicePM.DebitAccount;
                    break;
                }

            case "CURR":
                {
                    result = this.invoicePM.AccountingExternalCode;
                    break;
                }

            case "PYTM":
                {
                    result = this.invoicePM.PaymentTermExternalId;
                    break;
                }


            case "VAT":
                {
                    result = this.invoiceLinePM.ExternalTAXItemId;
                    break;
                }

            case "TAX":
                {
                    result = this.invoiceLinePM.ExternalVATCard;
                    break;
                }

            default:
                {
                    result = this.invoiceLinePM.CreditAccount;
                    break;
                }
        }

        return result;
    }
    set EditingFieldValue(value:string)
    {
        switch (this.Code) {
            case "BLTO":
                {
                    if (this.invoicePM.DebitAccount != value) {
                        this.invoicePM.DebitAccount = value;
                    }
                    break;
                }
            case "CURR":
                {
                    if (this.invoicePM.AccountingExternalCode != value) {
                        this.invoicePM.AccountingExternalCode = value;
                    }
                    break;
                }
            case "PYTM":
                {
                    if (this.invoicePM.PaymentTermExternalId != value) {
                        this.invoicePM.PaymentTermExternalId = value;
                    }
                    break;
                }
            case "VAT":
                {
                    if (this.invoiceLinePM.ExternalTAXItemId != value) {
                        this.invoiceLinePM.ExternalTAXItemId = value;
                        this.father.EntityPM.InvoiceLines.filter(d => d.VatTypeId == this.invoiceLinePM.VatTypeId).forEach(item => {
                            item.ExternalTAXItemId = value;
                        });
                    }
                    break;
                }
            case "TAX":
                {
                    if (this.invoiceLinePM.ExternalVATCard != value) {
                        this.invoiceLinePM.ExternalVATCard = value;
                        this.father.EntityPM.InvoiceLines.filter(d => d.VatTypeId == this.invoiceLinePM.VatTypeId).forEach(item => {
                            item.ExternalVATCard = value;
                        }); 
                    }
                    break;
                }
            default:
                {
                    if (this.invoiceLinePM.CreditAccount != value) {
                        this.invoiceLinePM.CreditAccount = value;
                    }
                    break;
                }
        }
        this.father.UpdateTransferData();
        this.GetDescriptionHelpVisibility();
    }

    get DebitAccount() { return this.invoicePM.DebitAccount; }
    set DebitAccount(value: string) {
        if (this.invoicePM.DebitAccount != value) {
            this.invoicePM.DebitAccount = value;
            this.father.UpdateTransferData();
            this.GetDescriptionHelpVisibility();
        }
    }

    get AccountingExternalCode() { return this.invoicePM.AccountingExternalCode; }
    set AccountingExternalCode(value: string) {
        if (this.invoicePM.AccountingExternalCode != value) {
            this.invoicePM.AccountingExternalCode = value;
            this.father.UpdateTransferData();
            this.GetDescriptionHelpVisibility();
        }
    }

    get PaymentTermExternalId() { return this.invoicePM.PaymentTermExternalId; }
    set PaymentTermExternalId(value: string) {
        if (this.invoicePM.PaymentTermExternalId != value) {
            this.invoicePM.PaymentTermExternalId = value;
            this.father.UpdateTransferData();
            this.GetDescriptionHelpVisibility();
        }
    }

    get ExternalTAXItemId() { return this.invoiceLinePM.ExternalTAXItemId; }
    set ExternalTAXItemId(value: string) {
        if (this.invoiceLinePM.ExternalTAXItemId != value) {
            this.invoiceLinePM.ExternalTAXItemId = value;
            this.father.EntityPM.InvoiceLines.filter(d => d.VatTypeId == this.invoiceLinePM.VatTypeId).forEach(item => {
                item.ExternalTAXItemId = value;
                this.father.UpdateTransferData();
                this.GetDescriptionHelpVisibility();
            });
        }
    }

    get ExternalVATCard() { return this.invoiceLinePM.ExternalVATCard; }
    set ExternalVATCard(value: string) {
        if (this.invoiceLinePM.ExternalVATCard != value) {
            this.invoiceLinePM.ExternalVATCard = value;
            this.father.EntityPM.InvoiceLines.filter(d => d.VatTypeId == this.invoiceLinePM.VatTypeId).forEach(item => {
                item.ExternalVATCard = value;
                this.father.UpdateTransferData();
                this.GetDescriptionHelpVisibility();
            }); 
        }
    }

    get CreditAccount() { return this.invoiceLinePM.CreditAccount; }
    set CreditAccount(value: string) {
        if (this.invoiceLinePM.CreditAccount != value) {
            this.invoiceLinePM.CreditAccount = value;
            this.father.UpdateTransferData();
            this.GetDescriptionHelpVisibility();
        }
    }

    // Commands
    EditClicked() {
        var tableName = "";
        var entityId = "";
        var tabCode = "";
        var title = "";

        switch (this.Code) {
            case "BLTO":
                {
                    switch (this.invoicePM.BillToPartnerTypeId) {
                        case "CS":
                            {
                                tableName = "Customer";
                                tabCode = "CLAC";
                                break;
                            }

                        case "AG":
                            {
                                tableName = "Agent";
                                tabCode = "AGAC";
                                break;
                            }


                        case "CG":
                            {
                                tableName = "CustomAgent";
                                tabCode = "CUAC";
                                break;
                            }


                        case "AL":
                            {
                                tableName = "Airline";
                                tabCode = "ALAC";
                                break;
                            }

                        case "TR":
                            {
                                tableName = "Trucker";
                                tabCode = "TRAC";
                                break;
                            }

                        case "VD":
                            {
                                tableName = "Vendor";
                                tabCode = "VDAC";
                                break;
                            }

                        case "SG":
                            {
                                tableName = "ShippingAgent";
                                tabCode = "SAAC";
                                break;
                            }


                        case "SL":
                            {
                                tableName = "ShippingLine";
                                tabCode = "SLAC";
                                break;
                            }

                        case "WH":
                            {
                                tableName = "Warehouse";
                                tabCode = "WHAC";
                                break;
                            }
                    }
                    entityId = this.invoicePM.BillToId;
                    title = "Edit";
                    break;
                }

            case "CURR":
                {
                    tableName = "Currency";
                    title = "Edit Currency";
                    entityId = this.invoicePM.InvoiceCurrencyId;
                    tabCode = "CRAC";
                    break;
                }
            case "PYTM":
                {
                    tableName = "PaymentTerm";
                    title = "Edit Payment Term";
                    entityId = this.invoicePM.PaymentTermId;
                    tabCode = "PTAC";
                    break;
                }

            case "VAT":
                {
                    tableName = "VatType";
                    title = "Edit VAT Type";
                    entityId = this.invoiceLinePM.VatTypeId;
                    tabCode = "VTAC";
                    break;
                }

            case "TAX":
                {
                    tableName = "VatType";
                    title = "Edit VAT Type";
                    entityId = this.invoiceLinePM.VatTypeId;
                    tabCode = "VTAC";
                    break;
                }

            default:
                {
                    tableName = "ChargesType";
                    title = "Edit Charges Type";
                    entityId = this.invoiceLinePM.ChargesTypeId;
                    tabCode = "CHAC";
                    break;
                }
        }
        var editWindow: LogitudeWindow = new LogitudeWindow();
        editWindow.IsFillScreen = true;
        editWindow.ShowHeaderButtons = true;
        editWindow.Title = title;
        editWindow.IsEditComponent = true;
        editWindow.ComponentLoaded.subscribe(s => {
            editWindow.WindowClosed.subscribe(d => {
                var entity = s.EntityPM;
                if (entity != null) {
                    if (this.Code == "BLTO") {
                        this.EditingFieldValue = entity.ReceivablesAccountingCard;
                    }
                    else if (this.Code == "CURR") {
                        this.EditingFieldValue = entity.AccountingExternalCode;
                    }
                    else if (this.Code == "PYTM") {
                        this.EditingFieldValue = entity.ExternalId;
                    }
                    else if (this.Code == "VAT") {
                        this.EditingFieldValue = entity.ExternalVATCard;
                    }
                    else if (this.Code == "TAX") {
                        if (SessionLocator.AccountingSettingPM.AccountingSystemCode == "HV" || SessionLocator.AccountingSettingPM.AccountingSystemCode == "RH") {
                            this.EditingFieldValue = SessionLocator.AccountingSettingPM.ReceivableVATCard;
                        }
                        else {
                            this.EditingFieldValue = entity.ExternalVATCard;
                        }
                    }
                    else { // Charge Type
                        if (entity.AccountingVATSplit) {
                            this.father.ItemsSource.filter(d => d.Code == "Line").forEach(item => {
                                if (item.invoiceLinePM.ChargesTypeId == entity.Id) {
                                    var charge: ChargeTypeAccountingPM = entity.ChargeTypeAccountings.filter(d => d.VatTypeId == this.invoiceLinePM.VatTypeId)[0];
                                    if (charge != null) {
                                        item.EditingFieldValue = charge.ReceivableCreditAccount;
                                    }
                                }
                            });
                        }
                        else {

                            this.father.ItemsSource.filter(d => d.Code == "Line").forEach(item => {
                                if (item.invoiceLinePM.ChargesTypeId == entity.Id) {
                                    //if (this.father.isJournal) {
                                    item.EditingFieldValue = entity.ReceivableCreditAccount;
                                    // }

                                    //else {
                                    // item.EditingFieldValue = entity.ChargesTypeExternalCode;
                                    //}
                                }
                            });
                        }
                    }

                    this.father.UpdateTransferData();
                }
            });
        });

        editWindow.ShowEditComponent(entityId, tableName, tabCode, true);
    }
}
export class InvoiceCodeNameClass {
    public VatTypeId: string;
}
