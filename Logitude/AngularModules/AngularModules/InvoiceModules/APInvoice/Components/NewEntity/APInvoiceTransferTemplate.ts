import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {APInvoicePM} from '../../../../Invoice/EntityPMs/APInvoicePM';
import {APInvoiceLinePM} from '../../../../Invoice/EntityPMs/APInvoiceLinePM';
import {AppTool} from '../../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {CurrencyListService} from '../../../../Common/Services/StandardLists/CurrencyListService';
import {CurrencyList} from '../../../../Common/EntityLists/CurrencyList';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
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
import {APInvoicePMService} from '../../../../Invoice/Services/StandardPMs/APInvoicePMService';

@Component({   
    templateUrl: './APInvoiceTransferTemplate.html',
})

export class APInvoiceTransferTemplate extends BaseComponent {
    public EntityPM: APInvoicePM;
    public ObjectTableName: string = "APInvoice";
    public DataContext = this;
    public IsNew: boolean = false; 
    public EntityId: string;
    public ItemsSource: APInvoiceTransferLineArgs[] = [];
    public IsAutoUpdatingFields: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }
    InitTemplate(entity: APInvoicePM) {
        this.EntityPM = entity;
        this.BuildList();
    }
    SetWindowArgs(args: any) {
        if (args) {
            this.IsAutoUpdatingFields = true;
            this.EntityId = args.EntityId;
            this.IsNew = args.IsNewTemplate;
            this.GetSingleEntityPM();
        }
    }
    GetSingleEntityPM() {
        var service = new APInvoicePMService();
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

    public BuildList() {
        this.ItemsSource = [];
        this.ItemsSource.push(new APInvoiceTransferLineArgs(this.EntityPM, null, this, "BLTO"));
        this.ItemsSource.push(new APInvoiceTransferLineArgs(this.EntityPM, null, this, "CURR"));

        if (this.EntityPM.IsMultipleEntities) {
            //nothing
        }

        else {
            this.EntityPM.InvoiceLines.forEach(item => {
                this.ItemsSource.push(new APInvoiceTransferLineArgs(null, item, this, "Line"));
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
                var linePM: APInvoiceLinePM = this.EntityPM.InvoiceLines.filter(d => d.VatTypeId == item.VatTypeId && d.VatPercentage != null)[0];
                if (linePM != null) {

                    if (linePM.VatPercentage != 0) {
                        this.ItemsSource.push(new APInvoiceTransferLineArgs(null, linePM, this, "TAX"));
                    }
                }
            });
        }

        if (this.IsAutoUpdatingFields) {
            this.UpdateTransferData();
        }
    }

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

        else if (this.EntityPM && this.EntityPM.StatusCode == "LL") {
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
        if (this.EntityPM) {
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
        if (this.EntityPM && this.EntityPM.TransferStatusName != null) {
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

            var service = new APInvoicePMService();
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

export class APInvoiceTransferLineArgs extends BaseComponent {
    private invoicePM: APInvoicePM;
    private invoiceLinePM: APInvoiceLinePM;
    public Code: string;
    public ObjectTableName: string;
    public DataContext = this;
    public VatPercentage: number;
    public IsTransferredEnabled: boolean;
    constructor(entity: APInvoicePM, line: APInvoiceLinePM, public father: APInvoiceTransferTemplate, typeCode: string) {
        super();

        this.Code = typeCode;

        if (entity != null) {
            this.invoicePM = entity;
            this.ObjectTableName = "APInvoice";
        }

        if (line != null) {
            this.invoicePM = father.EntityPM;
            this.invoiceLinePM = line;
            this.ObjectTableName = "APInvoiceLine";
            this.VatPercentage = line.VatPercentage;
        }

        this.InitalizeServices();
        this.InitalizeProperties();
        this.SetUIProperties();

        this.GetEditingFieldName();
        this.GetDescriptionTitle();
        this.GetDescriptionValue();
        this.GetDescriptionHelp();
        this.GetDescriptionHelpVisibility();
        this.FillFieldsData();
    }

    SetUIProperties() {

        var isEnabled = true;

        if (this.father.TransferStatusCode == "TR") {
            isEnabled = false;
        }

        else if (this.invoicePM.StatusCode == "WA") {
            isEnabled = false;
        }

        this.IsTransferredEnabled = isEnabled;

        this.UIProperties.SetEnabled(this.EditingFieldName, this.ObjectTableName, this.IsTransferredEnabled);

        this.GetDescriptionHelpVisibility();
    }
    GetDescriptionHelpVisibility() {
        var result = false;

        if (AppTool.IsNullOrEmpty(this.EditingFieldValue)) {
            result = true;
        }

        this.DescriptionHelpVisibility = result;
    }

    private CardListService: CardListService;
    private CurrencyListService: CurrencyListService;
    private VatTypeListService: VatTypeListService;
    private ChargesTypeListService: ChargesTypeListService;
    private invoiceDomainService: InvoiceDomainService;
    InitalizeServices() {
        this.CardListService = new CardListService();
        this.CurrencyListService = new CurrencyListService();
        this.VatTypeListService = new VatTypeListService();
        this.ChargesTypeListService = new ChargesTypeListService();
        this.invoiceDomainService = new InvoiceDomainService();
    }

    public EditingFieldName: string;
    public DescriptionTitle: string = "";
    public DescriptionValue: string = "";
    public DescriptionHelp: string = "";
    public DescriptionHelpVisibility = false;
    InitalizeProperties() {
        switch (this.Code) {
            case "BLTO":
                {
                    this.EditingFieldName = "CreditAccount";
                    this.DescriptionTitle = "Vendor";
                    this.DescriptionValue = this.invoicePM.VendorName;
                    this.DescriptionHelp = "Please enter credit account";
                    break;
                }

            case "CURR":
                {
                    this.EditingFieldName = "AccountingExternalCode";
                    this.DescriptionTitle = "Invoice Currency";
                    this.DescriptionValue = this.invoicePM.InvoiceCurrencyCode;
                    this.DescriptionHelp = "Please enter external code for " + this.invoicePM.InvoiceCurrencyCode;
                    break;
                }

            case "Line":
                {
                    this.EditingFieldName = "DebitAccount";
                    this.DescriptionTitle = "Charge Type";
                    this.DescriptionValue = this.invoiceLinePM.Description;
                    this.DescriptionHelp = "Please enter debit account";
                    break;
                }

            case "TAX":
                {
                    this.EditingFieldName = "ExternalVATCard";
                    this.DescriptionTitle = "VAT Type Tax Code";
                    this.DescriptionValue = this.invoiceLinePM.VatTypeName;
                    this.DescriptionHelp = "Please enter external tax code";
                    break;
                }
        }
    }

    get EditingFieldValue() {
        var result = "";
        switch (this.Code) {
            case "BLTO":
                {
                    result = this.invoicePM.CreditAccount;
                    break;
                }

            case "CURR":
                {
                    result = this.invoicePM.AccountingExternalCode;
                    break;
                }

            case "Line":
                {
                    result = this.invoiceLinePM.DebitAccount;
                    break;
                }

            case "TAX":
                {
                    result = this.invoiceLinePM.ExternalVATCard;
                    break;
                }
        }

        return result;
    }
    set EditingFieldValue(value: string) {
        switch (this.Code) {
            case "BLTO":
                {
                    if (this.invoicePM.CreditAccount != value) {
                        this.invoicePM.CreditAccount = value;
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

            case "Line":
                {
                    if (this.invoiceLinePM.DebitAccount != value) {
                        this.invoiceLinePM.DebitAccount = value;

                        this.father.ItemsSource.filter(d => d.Code == this.Code && d.invoiceLinePM.ChargesTypeId == this.invoiceLinePM.ChargesTypeId).forEach(item => {
                            item.EditingFieldValue = value;
                        });
                    }

                    break;
                }

            case "TAX":
                {
                    if (this.invoiceLinePM.ExternalVATCard != value) {
                        this.invoiceLinePM.ExternalVATCard = value;

                        this.father.ItemsSource.filter(d => d.Code == this.Code && d.invoiceLinePM.VatTypeId == this.invoiceLinePM.VatTypeId).forEach(item => {
                            item.EditingFieldValue = value;
                        });
                    }

                    break;
                }
        }

        this.father.UpdateTransferData();
        this.GetDescriptionHelpVisibility();
    }

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
                                this.GetAccountingSettingData();
                            }
                            else {
                                this.GetVatTypeData();
                            }
                            break;
                        }
                    case "Line": {
                        this.GetChargesTypeData();
                        break;
                    }
                    case "PYTM":
                        {
                            this.GetPaymentTermData();
                            break;
                        }
                    case "MultipleLine":
                        {
                            break;
                        }

                    case "MultipleVAT":
                        {
                            break;
                        }

                    case "MultipleTAX":
                        {

                            break;
                        }
                    default: {
                       
                        break;
                    }
                }
            }
        }
    }
    private GetCardData() {
        this.CardListService.getSingle(this.invoicePM.VendorId).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var myResult: CardList = response.Result;
                if (myResult) {

                    if (myResult.AccountingVATSplit) {
                        this.GetCardDataBySplit();
                    }

                    else {
                        this.EditingFieldValue = myResult.PayablesAccountingCard;
                    }
                }
            }
        });
    }
    private GetCardDataBySplit() {
        this.invoiceDomainService.GetCardCurrenciesAccountingByCurrencyAndId(this.invoicePM.VendorId, this.invoicePM.InvoiceCurrencyId, true).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                this.EditingFieldValue = response.Result;
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
                        this.EditingFieldValue = charge.PayableDebitAccount;
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
                    this.EditingFieldValue = list.PayableDebitAccount;
                }
            }
        });
    }
    private GetAccountingSettingData() {
        this.EditingFieldValue = SessionLocator.AccountingSettingPM.PayableVATCard;
    }
           
    get CreditAccount() { return this.invoicePM.CreditAccount; }
    set CreditAccount(value: string) {
        if (this.invoicePM.CreditAccount != value) {
            this.invoicePM.CreditAccount = value;
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

    get DebitAccount() { return this.invoiceLinePM.DebitAccount; }
    set DebitAccount(value: string) {
        if (this.invoiceLinePM.DebitAccount != value) {
            this.invoiceLinePM.DebitAccount = value;
            this.father.UpdateTransferData();
            this.GetDescriptionHelpVisibility();

            this.father.ItemsSource.filter(d => d.Code == this.Code && d.invoiceLinePM.ChargesTypeId == this.invoiceLinePM.ChargesTypeId).forEach(item => {
                item.DebitAccount = value;
            });
        }
    }

    get ExternalVATCard() { return this.invoiceLinePM.ExternalVATCard; }
    set ExternalVATCard(value: string) {
        if (this.invoiceLinePM.ExternalVATCard != value) {
            this.invoiceLinePM.ExternalVATCard = value;
            this.father.UpdateTransferData();
            this.GetDescriptionHelpVisibility();

            this.father.ItemsSource.filter(d => d.Code == this.Code && d.invoiceLinePM.VatTypeId == this.invoiceLinePM.VatTypeId).forEach(item => {
                item.ExternalVATCard = value;
            });
        }
    }

    EditClicked() {
        var tableName = "";
        var entityId = "";
        var tabCode = "";
        var title = "";

        switch (this.Code) {
            case "BLTO":
                {
                    switch (this.invoicePM.VendorPartnerTypeId) {
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
                    entityId = this.invoicePM.VendorId;
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

            case "Line":
                {
                    tableName = "ChargesType";
                    title = "Edit Charges Type";
                    entityId = this.invoiceLinePM.ChargesTypeId;
                    tabCode = "CHAC";
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

                    switch (this.Code) {
                        case "BLTO": {
                            if (entity.AccountingVATSplit) {
                                this.GetCardDataBySplit();
                            }

                            else {
                                this.EditingFieldValue = entity.PayablesAccountingCard;
                            }

                            break;
                        }

                        case "CURR": {
                            this.EditingFieldValue = entity.AccountingExternalCode;
                            break;
                        }

                        case "Line": {
                            if (entity.AccountingVATSplit) {
                                var charge: ChargeTypeAccountingPM = entity.ChargeTypeAccountings.filter(d => d.VatTypeId == this.invoiceLinePM.VatTypeId)[0];
                                if (charge != null) {
                                    this.EditingFieldValue = charge.PayableDebitAccount;
                                }
                            }

                            else {
                                this.EditingFieldValue = entity.PayableDebitAccount;
                            }

                            break;
                        }

                        case "TAX": {
                            if (SessionLocator.AccountingSettingPM.AccountingSystemCode == "HV" || SessionLocator.AccountingSettingPM.AccountingSystemCode == "RH") {
                                this.EditingFieldValue = SessionLocator.AccountingSettingPM.PayableVATCard;
                            }

                            else {
                                this.EditingFieldValue = entity.PayablesExternalId;
                            }

                            break;
                        }
                    }                 
                }
            });
        });

        editWindow.ShowEditComponent(entityId, tableName, tabCode, true);
    }
}

export class InvoiceCodeNameClass {
    public VatTypeId: string;
}
