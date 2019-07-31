import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ARPaymentPM} from '../../../../Invoice/EntityPMs/ARPaymentPM';
import {AppTool} from '../../../../Infrastructure/Tools'; 
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {CurrencyListService} from '../../../../Common/Services/StandardLists/CurrencyListService';
import {CurrencyList} from '../../../../Common/EntityLists/CurrencyList';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator'; 
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {ARPaymentPMService} from '../../../../Invoice/Services/StandardPMs/ARPaymentPMService';

@Component({
    moduleId: module.id,
    templateUrl: './ARPaymentTransferTemplate.html',
})

export class ARPaymentTransferTemplate extends BaseComponent {
    public EntityPM: ARPaymentPM;
    public ObjectTableName: string = "ARPayment";
    public DataContext = this;
    public ItemsSource: ARTransferLineArgs[] = [];
    private IsTheFirstTime: boolean = true;
    public IsNew: boolean = false;
    public EntityId: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }
    InitTemplate(entity: ARPaymentPM) {
        this.EntityPM = entity;
        this.FillFieldsData();
    }
    SetWindowArgs(args: any) {
        if (args) {
            this.EntityId = args.EntityId;
            this.IsNew = args.IsNewTemplate;
            this.GetSingleEntityPM();
        }
    }
    GetSingleEntityPM() {
        var service = new ARPaymentPMService();
        service.get(this.EntityId).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var entityPM = response.Result;
                if (entityPM) {
                    this.EntityPM = entityPM;
                    this.FillFieldsData();
                }
            }
        });
    }

    private isBillToFinished = false;
    private isCurrencyFinished = false;
    public BuildList() {
        if (this.isBillToFinished && this.isCurrencyFinished) {
            this.ItemsSource = [];
            this.ItemsSource.push(new ARTransferLineArgs(this.EntityPM, this, "BLTO"));
            this.ItemsSource.push(new ARTransferLineArgs(this.EntityPM, this, "CURR"));
            this.UpdateTransferData();
        }
    }
    public CurrencyExternalId: string = ""; 
    public BillToExternalId: string = ""; 
    private FillFieldsData() {
        this.GetCurrencyData();
        this.GetBillToData();
    }
    private GetCurrencyData() {
        var currencyListService = new CurrencyListService();
        currencyListService.getSingle(this.EntityPM.PaymentCurrencyId).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var currency: CurrencyList = response.Result;
                if (currency != null) {
                    this.CurrencyExternalId = currency.AccountingExternalCode;
                }
                this.isCurrencyFinished = true;
                this.BuildList();
            }
        });
    }
    private GetBillToData() {
        var cardListService = new CardListService();
        cardListService.getSingle(this.EntityPM.BillToId).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var card: CardList = response.Result;
                if (card != null) {
                    this.BillToExternalId = card.ReceivablesAccountingCard;
                }
                this.isBillToFinished = true;
                this.BuildList();
            }
        });
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
    get IsEditingEnabled()
    {
        var myResult = true;

        if (this.TransferStatusCode == "TR") {
            myResult = false;
        }

        else if (this.EntityPM && this.EntityPM.StatusCode == "LL") {
            myResult = false;
        }

        return myResult;
    }
    get SetBlockedButtonVisibility()
    {
        var result = false;
        if (this.TransferStatusCode != "TR") {
            if (this.TransferStatusCode != "BL") {
                result = true;
            }
        }
        return result;
    }
    get SetUnBlockedButtonVisibility()
    {
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
            if (this.IsTheFirstTime && !this.IsNew) {
                this.IsTheFirstTime = false;
                if (this.CurrentSession.CurrentEditComponent != null) {
                    var confirmWindow = new ConfirmWindow();
                    confirmWindow.Width = 450;
                    confirmWindow.Show("Accounting details needed for transfer is updated, update the transfer status ?");
                    confirmWindow.WindowClosed.subscribe((event: any) => {
                        if (confirmWindow.Yes) {
                            this.CurrentSession.CurrentEditComponent.SaveChanges();
                        }
                    });
                }
            }
        }
    }
    get IsReadyForTransfer() {
        return this.TransferStatusCode == "RD" ? true : false;
    } 
    get TransferStatusName()
    {
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
            var service = new ARPaymentPMService();
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

export class ARTransferLineArgs extends BaseComponent {
    public EntityPM: ARPaymentPM;
    public Code: string;
    public ObjectTableName: string = "ARPayment";
    public DataContext = this;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(entity: ARPaymentPM, public father: ARPaymentTransferTemplate, typeCode: string) {
        super();
        this.EntityPM = entity;
        this.Code = typeCode;
        this.GetDescriptionTitle();
        this.GetDescriptionValue();
        this.GetDescriptionHelp();
        this.GetDescriptionHelpVisibility();
    }

    // Props
    public DescriptionTitle: string ="";
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
                    result = "Payment Currency";
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
                    result = this.EntityPM.BillToName;
                    break;
                }

            case "CURR":
                {
                    result = this.EntityPM.PaymentCurrencyCode;
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
                    result = "Please enter bill to account";
                    break;
                }
            case "CURR":
                {
                    result = "Please enter external code for " + this.EntityPM.PaymentCurrencyCode;
                    break;
                }
        }
        this.DescriptionHelp = result;
    }

    public DescriptionHelpVisibility = false;
    GetDescriptionHelpVisibility()
    {
        var result = false;
        if (AppTool.IsNullOrEmpty(this.EditingFieldValue)) {
            result = true;
        }
        this.DescriptionHelpVisibility = result;
    }
    get IsTransferredEnabled()
    {
        var myResult = true;
        if (this.father.TransferStatusCode == "TR") {
            myResult = false;
        }
        else if (this.EntityPM.StatusCode == "DR" || this.EntityPM.StatusCode == "LL") {
            myResult = false;
        }
        return myResult;
    }

    get EditingFieldValue() {
        var result = "";
        switch (this.Code) {
            case "BLTO":
                {
                    result = this.father.BillToExternalId;
                    break;
                }

            case "CURR":
                {
                    result = this.father.CurrencyExternalId;
                    break;
                }
        }
        return result;
    }
    set EditingFieldValue(value: string) {
        switch (this.Code) {
            case "BLTO":
                {
                    if (this.father.BillToExternalId != value) {
                        this.father.BillToExternalId = value;
                    }
                    break;
                }

            case "CURR":
                {
                    if (this.father.CurrencyExternalId != value) {
                        this.father.CurrencyExternalId = value;
                    }
                    break;
                }
        }

        this.father.UpdateTransferData();
        this.GetDescriptionHelpVisibility();
    }

    // Commands
    EditClicked() {
        switch (this.Code) {
            case "BLTO":
                {
                    var tableName = "";
                    var entityId = "";
                    var tabCode = "";
                    var title = "";

                    switch (this.EntityPM.BillToPartnerTypeId) {
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
                    entityId = this.EntityPM.BillToId;
                    title = "Edit";
                    break;
                }

            case "CURR":
                {
                    tableName = "Currency"; 
                    title = "Edit Currency";
                    entityId = this.EntityPM.PaymentCurrencyId;
                    tabCode = "CRAC";
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

                    this.father.UpdateTransferData();
                }
            });
        });        

        editWindow.ShowEditComponent(entityId, tableName, tabCode, true);
    }
}
