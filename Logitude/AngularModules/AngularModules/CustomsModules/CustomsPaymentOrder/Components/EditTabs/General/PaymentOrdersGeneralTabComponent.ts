import {Component}  from '@angular/core';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {AppTool, ArrayTool} from '../../../../../Infrastructure/Tools';
import {FeatureLocator} from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {PaymentOrderPM} from '../../../../../Customs/EntityPMs/PaymentOrderPM';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { PaymentOrderConnectionTableExtendedPMService } from '../../../../../Customs/Services/ExtendedPMs/PaymentOrderConnectionTableExtendedPMService';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { CustomsSettingListService } from '../../../../../Customs/Services/StandardLists/CustomsSettingListService';
import { ClientList } from '../../../../../Customs/EntityLists/ClientList';
import { CustomBankList } from '../../../../../Customs/EntityLists/CustomBankList';
import { PaymentOrderMethodPM } from '../../../../../Customs/EntityPMs/PaymentOrderMethodPM';
import { PaymentOrderProtestReasonPM } from '../../../../../Customs/EntityPMs/PaymentOrderProtestReasonPM';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { DeclarationWebService } from '../../../../../Customs/Services/WebServices/DeclarationWebService';
import { DeclarationExtendedListService } from '../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { CustomBankListService } from '../../../../../Customs/Services/StandardLists/CustomBankListService';
import { CustomBankCardExtendedPMService } from '../../../../../Customs/Services/ExtendedPMs/CustomBankCardExtendedPMService';
import { IIGGeneralMessagesService } from '../../../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { CustomBanksCardPM } from '../../../../../Customs/EntityPMs/CustomBanksCardPM';
//import { EditPaymentOrderComponent } from '../../../Declaration/EditTabs/PaymentOrder/EditPaymentOrderComponent';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: './PaymentOrdersGeneralTabComponent.html',
})

export class PaymentOrdersGeneralTabComponent extends BaseComponent {
    public DataContext: PaymentOrdersGeneralTabComponent = this;
    public EntityPM: PaymentOrderPM = new PaymentOrderPM();
    public ObjectTableName: string = "Customs.PaymentOrder";
    public AccountingSelectionFilterList: CodeNameClass[] = [];

    public LinesList: ObservableCollection;
    public MethodsList: ObservableCollection;
    public ProtestsList: ObservableCollection;
    public banksList: CustomBankList[] = [];

    private isControlEnabled: boolean = true;
    private isErrorMessageVisibility: boolean = false;
    private isByAccountingCustomFile: boolean = true;
    private isByPaymentOrderAccCard: boolean = false;
    private showMethodsColumnFooters: boolean = false;
    private showProtestsColumnFooters: boolean = false;
    private isFromBuildAccounting: boolean = false;
    public isTranslationLoaded: boolean = false;
    //private paymentOrderAccCard: string;
    private customFilesList: string[] = [];
    private currentEditComponentId: string;
    public AmountInDisputeTotal: number = 0;

    public declarationWebService: DeclarationWebService = new DeclarationWebService;
    private declarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    private paymentOrderConnectionTableExtendedPMService: PaymentOrderConnectionTableExtendedPMService = new PaymentOrderConnectionTableExtendedPMService;
    public customsSettingListService: CustomsSettingListService = new CustomsSettingListService();

    imgNgStyle = "";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private EntityResourceService: EntityResourceService) {
        super();
        this.LinesList = new ObservableCollection([]);
        this.MethodsList = new ObservableCollection([]);
        this.ProtestsList = new ObservableCollection([]);
        this.banksList = [];
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        this.CurrentSession.StartBusyIndicator("");
        this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe(response => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrderLine").subscribe(response => {
                this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrderMethod").subscribe(response => {
                    this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrderProtestReason").subscribe(response => {
                        this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsSetting").subscribe(response => {
                            this.EntityResourceService.getEntityResourceByTableName("Customs.DeclarationPaymentMethod").subscribe(response => {
                            if (this.entityArgs.EntityPM != null) {
                                this.EntityPM = this.entityArgs.EntityPM;
                                this.ObjectTableName = this.entityArgs.ObjectTableName;
                                this.InitPaymentOrderScreen();
                            }
                            this.Listen();
                            this.isTranslationLoaded = true;
                            this.CurrentSession.StopBusyIndicator();
                            });
                        });
                    });
                });
            });
        });

    }

    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            this.currentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.EntityPM.CustomerChanged = false;
                    }
                })
            );

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.DisplayOnlyCheck();
                        this.BuildPaymentOrderMethods();
                    }
                })
            );

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.currentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "POGN") {

                        }
                    }
                })
            );
        }
    }

    InitPaymentOrderScreen() {

        this.UIProperties.SetEnabled("PaymentOrderTypeCode", "Customs.PaymentOrder", false);
        this.UIProperties.SetEnabled("CustomerActivityTypeCode", "Customs.PaymentOrder", false);
        this.UIProperties.SetEnabled("ImporterId", "Customs.PaymentOrder", false);
        this.UIProperties.SetEnabled("TotalSumToPay", "Customs.PaymentOrder", false);
        this.UIProperties.SetEnabled("PaymentProcessCode", "Customs.PaymentOrder", false);
        this.UIProperties.SetEnabled("PaymentOrderLeftAmount", "Customs.PaymentOrder", false);

        if (this.EntityPM.PaymentOrderLines) {
            this.EntityPM.PaymentOrderLines.forEach((itemLine) => {
                this.LinesList.Insert(itemLine);
            });
        }
        this.BuildPaymentOrderMethods();

        if (this.EntityPM.PaymentOrderProtestReasons) {
            this.EntityPM.PaymentOrderProtestReasons.forEach((itemLine) => {
                if (itemLine.AmountInDispute != null) {
                    let amount: number = itemLine.AmountInDispute;
                    this.AmountInDisputeTotal = this.AmountInDisputeTotal + Number(amount);
                }
                this.ProtestsList.Insert(itemLine);
            });
            if ((this.ProtestsList.Length * 27) + 27 < 123) {
                this.FooterProtests = (this.ProtestsList.Length * 27) + 27;
            }
            else {
                this.FooterProtests = 123;
            }
        }

        this.BuildAccountingSelectionGroupFilterList();
        this.BuildAccountingCustomFilesList();
        this.DisplayOnlyCheck();
    }

    InitTab(entityPM: PaymentOrderPM, /*parent: EditPaymentOrderComponent,*/ isDisplayOnly: boolean) {

        this.EntityPM = entityPM;
        this.InitPaymentOrderScreen();
        //this.Parent = parent;
        //this.IsDisplayOnly = isDisplayOnly;
    }

    BuildPaymentOrderMethods() {
        this.MethodsList = new ObservableCollection([]);
        this.MethodItemsAmountTotal = 0;

        if (this.EntityPM.PaymentOrderMethods) {
            for (let item of this.EntityPM.PaymentOrderMethods) {
                if (item.Amount != null) {
                    let amount: number = item.Amount;
                    this.MethodItemsAmountTotal = this.MethodItemsAmountTotal + Number(amount);
                }
                this.MethodsList.Insert(new PaymentMethodModel(item, this));

                if ((this.MethodsList.Length * 27) + 27 < 123) {
                    this.FooterMethods = (this.MethodsList.Length * 27) + 27;
                }
                else {
                    this.FooterMethods = 123;
                }
            }
        }
    }

    DisplayOnlyCheck() {
        if (this.EntityPM.IsClosed) {
            this.IsControlEnabled = false;
            this.IsErrorMessageVisibility = true;
            this.SetScreenFieldsEditability();
        }
        else {
            this.IsControlEnabled = true;
            this.IsErrorMessageVisibility = false;
        }
    }

    SetScreenFieldsEditability() {
        this.UIProperties.SetEnabled("AccountingCustomFile", null, false);
        this.UIProperties.SetEnabled("ImporterId", "Customs.PaymentOrder", false);
        this.UIProperties.SetEnabled("PaymentOrderTypeCode", "Customs.PaymentOrder", false);
        this.UIProperties.SetEnabled("CustomerActivityTypeCode", "Customs.PaymentOrder", false);
        this.UIProperties.SetEnabled("TotalSumToPay", "Customs.PaymentOrder", false);
        this.UIProperties.SetEnabled("PaymentProcessCode", "Customs.PaymentOrder", false);
        this.UIProperties.SetEnabled("PaymentOrderLeftAmount", "Customs.PaymentOrder", false);
        this.UIProperties.SetEnabled("CustomerId", "Customs.PaymentOrder", false);
    }

    // log tab
    selectedTab: LogTab;
    public get SelectedTab() { return this.selectedTab; }
    public set SelectedTab(tab: LogTab) {
        this.selectedTab = tab;
    }

    public SetTabArgs(args: any, valdationErrorList: any[] = null) {
        this.EntityPM = args.EntityPM;
        console.log("EntityPM", this.EntityPM);
    }

    public get PaymentOrderTypeCode() { return this.EntityPM.PaymentOrderTypeCode; }
    public set PaymentOrderTypeCode(newValue: string) { this.EntityPM.PaymentOrderTypeCode = newValue; } 

    public get CustomerActivityTypeCode() { return this.EntityPM.CustomerActivityTypeCode; }
    public set CustomerActivityTypeCode(newValue: string) { this.EntityPM.CustomerActivityTypeCode = newValue; } 

    public get ImporterId() { return this.EntityPM.ImporterId; }
    public set ImporterId(newValue: string) { this.EntityPM.ImporterId = newValue; }

    public get TotalSumToPay() { return this.EntityPM.TotalSumToPay; }
    public set TotalSumToPay(newValue: number) { this.EntityPM.TotalSumToPay = newValue; }

    public get PaymentProcessCode() { return this.EntityPM.PaymentProcessCode; }
    public set PaymentProcessCode(newValue: string) { this.EntityPM.PaymentProcessCode = newValue; }

    public get PaymentOrderSelectedLabel() { return this.EntityPM.PaymentOrderSelectedLabel; }
    public set PaymentOrderSelectedLabel(newValue: string) { this.EntityPM.PaymentOrderSelectedLabel = newValue; }

    public get PaymentOrderLeftAmount() { return this.EntityPM.PaymentOrderLeftAmount; }
    public set PaymentOrderLeftAmount(newValue: number) { this.EntityPM.PaymentOrderLeftAmount = newValue; }

    public get AccountingCustomFile() { return this.EntityPM.AccountingCustomFile; }
    public set AccountingCustomFile(newValue: string) { this.EntityPM.AccountingCustomFile = newValue; }

    public get Reason() { return this.EntityPM.Reason; }
    public set Reason(newValue: string) { this.EntityPM.Reason = newValue; }

    public get InternalNotes() { return this.EntityPM.InternalNotes; }
    public set InternalNotes(newValue: string) { this.EntityPM.InternalNotes = newValue; }

    public get CustomerId() { return this.EntityPM.CustomerId; }
    public set CustomerId(newValue: string) {
        this.EntityPM.CustomerId = newValue;
        this.EntityPM.CustomerChanged = true;
    }

    public get IsControlEnabled() { return this.isControlEnabled; }
    public set IsControlEnabled(newValue: boolean) { this.isControlEnabled = newValue; }

    public get IsErrorMessageVisibility() { return this.isErrorMessageVisibility; }
    public set IsErrorMessageVisibility(newValue: boolean) { this.isErrorMessageVisibility = newValue; }

    public get IsByAccountingCustomFile() { return this.isByAccountingCustomFile; }
    public set IsByAccountingCustomFile(newValue: boolean) { this.isByAccountingCustomFile = newValue; }

    public get IsByPaymentOrderAccCard() { return this.isByPaymentOrderAccCard; }
    public set IsByPaymentOrderAccCard(newValue: boolean) { this.isByPaymentOrderAccCard = newValue; }

    //public get PaymentOrderAccCard() { return this.paymentOrderAccCard; }
    //public set PaymentOrderAccCard(newValue: string) { this.paymentOrderAccCard = newValue; }

    public get ShowMethodsColumnFooters() { return this.showMethodsColumnFooters; }
    public set ShowMethodsColumnFooters(newValue: boolean) { this.showMethodsColumnFooters = newValue; }

    public get ShowProtestsColumnFooters() { return this.showProtestsColumnFooters; }
    public set ShowProtestsColumnFooters(newValue: boolean) { this.showProtestsColumnFooters = newValue; }

    public CheckAndCalcDeclrationByAccountingCustomFile() {
        if (this.PaymentOrderSelectedLabel == "PaymentOrderAccCard") {
            return (true);
        }

        if (AppTool.IsNullOrEmpty(this.AccountingCustomFile)) {
            this.MessageAccountingCardWindow(TextCodeTranslator.Translate("Customs.PaymentOrder.O.AccountingCustomFileMissing"));
            return (false);
        }

        if (this.customFilesList.length > 0) {
            if (this.customFilesList.indexOf(this.AccountingCustomFile) < 0) {
                this.MessageAccountingCardWindow("יש לבחור תיק עמילות מהרשימה!");
                return (false);
            }
        }
    }


    //#region AccountingCustomFile
    private BuildAccountingSelectionGroupFilterList() {
        this.AccountingSelectionFilterList = [];
        this.isFromBuildAccounting = true;

        var myAccountingCustomFileItem: CodeNameClass = new CodeNameClass();
        myAccountingCustomFileItem.Code = "0"; // "AccountingCustomFile"
        myAccountingCustomFileItem.Name = TextCodeTranslator.Translate("Customs.PaymentOrder.F.AccountingCustomFile");
        this.AccountingSelectionFilterList.push(myAccountingCustomFileItem);

        var myPaymentOrderAccCardItem: CodeNameClass = new CodeNameClass();
        myPaymentOrderAccCardItem.Code = "1"; // "PaymentOrderAccCard"
        myPaymentOrderAccCardItem.Name = TextCodeTranslator.Translate("Customs.CustomsSetting.F.PaymentOrderAccCard");
        this.AccountingSelectionFilterList.push(myPaymentOrderAccCardItem);

        if (this.PaymentOrderSelectedLabel == "PaymentOrderAccCard") {
            this.SelectedAccountingFilter = myPaymentOrderAccCardItem;
        }
        else { // AccountingCustomFile
            this.SelectedAccountingFilter = myAccountingCustomFileItem;
        }
        this.isFromBuildAccounting = false;
    }

    private selectedAccountingFilter: CodeNameClass;
    get SelectedAccountingFilter() {
        return this.selectedAccountingFilter;
    }
    set SelectedAccountingFilter(newValue: CodeNameClass) {
        if (this.selectedAccountingFilter != newValue) {
            this.selectedAccountingFilter = newValue;
        }

        this.UIProperties.SetEnabled("AccountingCustomFile", this.ObjectTableName, true);
        if (newValue.Code == "1") {
            this.PaymentOrderSelectedLabel = "PaymentOrderAccCard";

            this.customsSettingListService.getSingleFromCache(SessionLocator.Tenant.toString())
                .subscribe((customsSettingList: any) => {
                    if (customsSettingList != null) {
                        if (!AppTool.IsNullOrEmpty(customsSettingList.Result)) {
                            this.AccountingCustomFile = customsSettingList.Result.PaymentOrderAccCard;
                            this.UIProperties.SetEnabled("AccountingCustomFile", this.ObjectTableName, false);
                        }
                    else {
                            this.MessageAccountingCardWindow(TextCodeTranslator.Translate("Customs.PaymentOrder.O.NoAccountingCard"));                        
                    }
                }});

        }
        else if (this.isFromBuildAccounting == false) {
            this.PaymentOrderSelectedLabel = "AccountingCustomFile";
            this.AccountingCustomFile = "";
            if (this.customFilesList != null && this.customFilesList.length == 1) {
                this.AccountingCustomFile = this.customFilesList[0];
            }
        }
    }

    public AccountingCustomFileLostFocusMethod(item) {
        if (this.PaymentOrderSelectedLabel == "PaymentOrderAccCard") {
            return (true);
        }

        var errorMessage = "";
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];

        if (AppTool.IsNullOrEmpty(this.AccountingCustomFile)) {
            return;
        }

        if (this.customFilesList != null && this.customFilesList.length > 0) {
            if (!(this.customFilesList.indexOf(this.AccountingCustomFile) > -1)) {
                errorMessage = "יש לבחור תיק עמילות מהרשימה!";
                this.MessageAccountingCardWindow(errorMessage);
                this.CurrentSession.CurrentEditComponent.ValidationErrorsList.push(errorMessage);
                return;
            }
        }
        else {
            this.declarationExtendedListService.GetDeclarationByCustomFileNoAndCCU(this.AccountingCustomFile)
                .subscribe((myResponse: ServiceResponse) => {
                    if (myResponse.Result == null || (myResponse.Result != null && AppTool.IsNullOrEmpty(myResponse.Result.Id))) {
                        errorMessage = TextCodeTranslator.Translate("Customs.Declaration.O.Didntfindcustomfile");
                        this.MessageAccountingCardWindow(errorMessage);
                        this.CurrentSession.CurrentEditComponent.ValidationErrorsList.push(errorMessage);
                        this.AccountingCustomFile = "";
                        return;
                    }
                    else {
                        this.PaymentOrderSelectedLabel = "AccountingCustomFile";
                    }
                });
        }
    }

    MessageAccountingCardWindow(message: string) {
        var messageWindow = new MessageWindow();
        messageWindow.Width = 300;
        messageWindow.Height = 150;
        messageWindow.Show(message);
    }

    OpenCustomFilesScreen() {

        if (!this.IsControlEnabled) {
            return;
        }

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 300;
        logitudeWindow.Height = 400;
        logitudeWindow.IsShowCloseButton = true;
        logitudeWindow.Title = "תיק עמילות לחיוב";
        logitudeWindow.WindowArgs = this.customFilesList;
        logitudeWindow.WindowClosed.subscribe(($event: any) => this.OnCustomFilesScreenWindowClosed($event));
      logitudeWindow.Show('./CustomsModules/CustomsPaymentOrder/Components/EditTabs/General/AccountingCustomFilesComponent');
    }

    OnCustomFilesScreenWindowClosed(arg: any) {
        if (!AppTool.IsNullOrEmpty(arg)) {
            this.AccountingCustomFile = arg;
            this.IsByAccountingCustomFile = true;
        }
    }

    private BuildAccountingCustomFilesList() {
        this.paymentOrderConnectionTableExtendedPMService.GetAccountingCustomFileNumbers(this.EntityPM.Id, SessionLocator.Tenant)
            .subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                this.BuildAccountingCustomFilesListOp_Completed(myResponse, false);
            });
    }
    private BuildAccountingCustomFilesListOp_Completed(myResponse: ServiceResponse, sourceIsCostomFile: boolean) 
    {      
        this.customFilesList = [];
        if (myResponse.Result != null) {
            myResponse.Result.forEach((item) => {
                this.customFilesList.push(item);
            });
        }

        if (this.EntityPM != null) {
            if (AppTool.IsNullOrEmpty(this.AccountingCustomFile) && this.customFilesList.length == 1) {
                //LoadOperation getFileOp = declarationContext.Load(declarationContext.GetSingleDeclarationByCustomFileNoQuery(entityPM.AccountingCustomFile, TenantContext.Current.Id), LoadBehavior.RefreshCurrent, true);
                //getFileOp.Completed += AccountingCustomFileLostFocus_Completed;
                this.AccountingCustomFile = this.customFilesList[0];
            }
        }
    }
    //#endregion

    //#region PaymentOrderMethods
    FooterMethods: number = 0;

    private methodItemsAmountTotal: number = 0;
    get MethodItemsAmountTotal() { return this.methodItemsAmountTotal; }
    set MethodItemsAmountTotal(newValue: number) {
        if (this.methodItemsAmountTotal != newValue) {
            this.methodItemsAmountTotal = newValue;
        }
    }

    CalcMethodItemsAmountTotal() {
        this.MethodItemsAmountTotal = 0;
        if (!AppTool.IsNullOrEmpty(this.MethodsList)) {
            this.MethodsList.Collection.forEach((method) => {
                if (method.Amount != null) {
                    let amount: number = method.Amount;
                    this.MethodItemsAmountTotal = this.MethodItemsAmountTotal + Number(amount);
                }
            });
        }
    }

    DeleteMethodsList(item: PaymentMethodModel) {
        if (!this.IsControlEnabled) return;

        if (!AppTool.IsNullOrEmpty(item)) {
            this.MethodsList.Remove(item);
            if ((this.MethodsList.Length * 27) + 27 < 123) {
                this.FooterMethods = (this.MethodsList.Length * 27) + 27;
            }
            else {
                this.FooterMethods = 123;
            }
            if (item.Amount != null) {
                let amount: number = item.Amount;
                this.MethodItemsAmountTotal = this.MethodItemsAmountTotal - Number(amount);
            }
            this.EntityPM.RemovePaymentOrderMethod(item.methodPM);
        }
    } 

    AddPaymentOrderMethod() {
        if (!this.IsControlEnabled) return;

        var newPaymentOrderMethodPM = new PaymentOrderMethodPM(this.EntityPM);
        newPaymentOrderMethodPM.PaymentOrderId = this.EntityPM.Id;
        newPaymentOrderMethodPM.Tenant = this.EntityPM.Tenant;
        newPaymentOrderMethodPM.Line = (ArrayTool.Max(this.EntityPM.PaymentOrderMethods, "Line") + 1);

        this.EntityPM.AddPaymentOrderMethod(newPaymentOrderMethodPM);
        var itemModel = new PaymentMethodModel(newPaymentOrderMethodPM, this);
        this.MethodsList.Insert(itemModel);

        if ((this.MethodsList.Length * 27) + 27 < 123) {
            this.FooterMethods = (this.MethodsList.Length * 27) + 27;
        }
        else {
            this.FooterMethods = 123;
        }

        this.ShowMethodsColumnFooters = true;
    }

    //#endregion

    //#region PaymentOrderProtestReason
    FooterProtests: number = 0;

    CalcProtestItemsTotal(event: any) {
        let total: number = 0;
        this.AmountInDisputeTotal = 0;

        if (!AppTool.IsNullOrEmpty(this.ProtestsList)) {
            this.ProtestsList.Collection.forEach((item) => {
                if (item.AmountInDispute != null) {
                    total = item.AmountInDispute;
                    this.AmountInDisputeTotal = this.AmountInDisputeTotal + Number(total);
                }
            });
        }
    }

    DeleteProtestsList(item) {
        if (!this.IsControlEnabled) return;

        if (!AppTool.IsNullOrEmpty(item)) {
            this.ProtestsList.Remove(item);
            this.EntityPM.RemovePaymentOrderProtestReason(item);
            if (item.AmountInDispute != null) {
                let amount: number = item.AmountInDispute;
                this.AmountInDisputeTotal = this.AmountInDisputeTotal - Number(amount);
            }

            if ((this.ProtestsList.Length * 27) + 27 < 123) {
                this.FooterProtests = (this.ProtestsList.Length * 27) + 27;
            }
            else {
                this.FooterProtests = 123;
            }
        }
    }

    AddPaymentOrderProtestReason() {
        if (!this.IsControlEnabled) return;

        var newPaymentOrderProtestReasonPM = new PaymentOrderProtestReasonPM(this.EntityPM);
        newPaymentOrderProtestReasonPM.PaymentOrderId = this.EntityPM.Id;
        newPaymentOrderProtestReasonPM.Tenant = this.EntityPM.Tenant;
        newPaymentOrderProtestReasonPM.Line = (ArrayTool.Max(this.EntityPM.PaymentOrderProtestReasons, "Line") + 1);

        this.ProtestsList.Insert(newPaymentOrderProtestReasonPM)
        this.EntityPM.AddPaymentOrderProtestReason(newPaymentOrderProtestReasonPM);

        if ((this.ProtestsList.Length * 27) + 27 < 123) {
            this.FooterProtests = (this.ProtestsList.Length * 27) + 27;
        }
        else {
            this.FooterProtests = 123;
        }

        this.ShowProtestsColumnFooters = true;
    }

    SetProtestTypeLocalName(item, lookupEntity) {
        if (!AppTool.IsNullOrEmpty(lookupEntity)) {
            item.ProtestTypeName = lookupEntity.LocalName;
        }
    }

    //#endregion
}

class CodeNameClass {
    public Code: string
    public Name: string
}

export class PaymentMethodModel extends BaseComponent {
    public ObjectTableName = "Customs.PaymentOrderMethod";
    public DataContext = this;
    BanksList: CustomBankList[] = [];
    AgentBanks: CustomBankList[] = [];
    customBankListService: CustomBankListService = new CustomBankListService();
    customBankCardExtendedPMService: CustomBankCardExtendedPMService = new CustomBankCardExtendedPMService();

    constructor(public methodPM: PaymentOrderMethodPM, public parent: PaymentOrdersGeneralTabComponent) {
        super();
      
        if (methodPM.TypeCode == "1") {
            this.BanksList = [];
            this.AgentBanks = [];
            this.LoadBanks();
        }

        this.UIProperties.SetEnabled("PaymentMethodStatusCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("PaymentMethodStatusName", this.ObjectTableName, false);
    }

    public CalcAmountDifference(logCellTemplate) {
        var amount: number = 0;

        if (this.parent.MethodsList != null) {
            this.parent.MethodsList.Collection.forEach((method) => {
                if (method.Amount != null) {
                    amount = amount + Number(method.Amount);
                }
            });
        }
        if (this.parent.TotalSumToPay > amount) {
            this.Amount = this.parent.TotalSumToPay - amount;
        }
    }

    LoadBanks() {
        this.parent.declarationWebService.GetCustomBanksForCard(this.parent.EntityPM.CustomerId).subscribe((response: ServiceResponse) => {
            var result = response.Result;
            console.log("[Response] GetCustomBanksForCard: ", result);
            if (!AppTool.IsNullOrEmpty(result)) {
                this.BanksList = result;
                this.parent.customsSettingListService.getAll().subscribe((response: ServiceResponse) => {
                    var list = response.Result;

                    if (!AppTool.IsNullOrEmpty(list)) {
                        var customsSetting = list[0];
                        //if (this.BanksList.length == 0) {
                        //    this.customBankListService.getAllFromCache().subscribe((response: ServiceResponse) => {
                        //        if (response) {
                        //            if (!response.HasError) {
                        //                this.agentBanks = response.Result.filter(d => d.PayerTypeCode == "3" && !d.InActive);
                        //                if (this.agentBanks.length == 1) {
                        //                    this.InternalBankId = this.agentBanks[0].Id;
                        //                    this.SelectedBank = this.agentBanks[0];
                        //                    this.BanksList = this.agentBanks;

                        //                }
                        //            }
                        //        }
                        //    });
                        //}
                        //else {
                            if (customsSetting != null) {
                                if (customsSetting.BlockAgentBankForMasab) {
                                    if (this.BanksList.length > 0) {
                                        if (this.BanksList.length == 1) {
                                            if (this.InternalBankId == null) {
                                                this.InternalBankId = this.BanksList[0].Id;
                                            }
                                            var bank: CustomBankList = this.BanksList.filter(d => d.Id == this.InternalBankId)[0];
                                            this.SelectedBank = bank;
                                        }
                                        else {
                                            if (this.InternalBankId != null) {
                                                var bank: CustomBankList = this.BanksList.filter(d => d.Id == this.InternalBankId)[0];
                                                this.SelectedBank = bank;
                                            }
                                        }
                                    }
                                }
                                else {
                                    if (this.BanksList.length > 0) {
                                        if (this.BanksList.length == 1) {
                                            if (this.InternalBankId == null) {
                                                this.InternalBankId = this.BanksList[0].Id;
                                            }
                                            this.customBankListService.getAllFromCache().subscribe((response: ServiceResponse) => {
                                                if (response) {
                                                    if (!response.HasError) {
                                                        this.BanksList = response.Result.filter(d => !d.InActive); 
                                                        if (this.InternalBankId != null) {
                                                            var bank: CustomBankList = this.BanksList.filter(d => d.Id == this.InternalBankId)[0];
                                                            this.SelectedBank = bank;
                                                        }
                                                    }
                                                }
                                            });
                                        }
                                        else {
                                            this.customBankListService.getAllFromCache().subscribe((response: ServiceResponse) => {
                                                if (response) {
                                                    if (!response.HasError) {
                                                        this.BanksList = response.Result.filter(d => !d.InActive);
                                                        if (this.InternalBankId != null) {
                                                            var bank: CustomBankList = this.BanksList.filter(d => d.Id == this.InternalBankId)[0];
                                                            this.SelectedBank = bank;
                                                        }
                                                    }
                                                }
                                            });
                                        }
                                    }

                                    else if (this.BanksList.length == 0) {
                                        this.customBankListService.getAllFromCache().subscribe((response: ServiceResponse) => {
                                            if (response) {
                                                if (!response.HasError) {
                                                    this.AgentBanks = response.Result.filter(d => d.PayerTypeCode == "3" && !d.InActive);
                                                    if (this.AgentBanks.length > 0) {
                                                        this.BanksList = this.AgentBanks;
                                                        if (this.AgentBanks.length == 1) {
                                                            if (this.InternalBankId == null) {
                                                                this.InternalBankId = this.BanksList[0].Id;
                                                            }
                                                        }
                                                        else {
                                                            if (this.InternalBankId != null) {
                                                                var bank: CustomBankList = this.BanksList.filter(d => d.Id == this.InternalBankId)[0];
                                                                this.SelectedBank = bank;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        });
                                    }
                                }
                            }
                        //}

                    }
                });
            }
        });
    }

    //#region Properties
    selectedBank: CustomBankList;
    get SelectedBank() { return this.selectedBank; }
    set SelectedBank(value: CustomBankList) {
        if (this.selectedBank != value) {
            this.selectedBank = value;

            if (!AppTool.IsNullOrEmpty(value)) {
                this.InternalBankId = value.Id;
                this.InternalBankName = value.LocalName;
                if (AppTool.IsNullOrEmpty(value.LocalName)) {
                    this.InternalBankName = value.EnglishName;
                }
            } else {
                this.InternalBankId = null;
                this.InternalBankName = null;
            }
        }
    }

    get TypeCode() {
        return this.methodPM.TypeCode;
    }
    set TypeCode(value: string) {
        if (this.methodPM.TypeCode != value) {
            this.methodPM.TypeCode = value;
            if (value == "1") {
                this.BanksList = [];
                this.AgentBanks = [];
                this.LoadBanks();
            }
            else {
                this.InternalBankId = null;
                this.CustomerActivityTypeCode = null;
                this.CustomerActivityTypeName = null;
                this.BanksList = [];
                this.AgentBanks = [];
                this.SelectedBank = null;
            }
        }
    }

    get TypeName() { return this.methodPM.TypeName; }
    set TypeName(value: string) {
        if (this.methodPM.TypeName != value) {
            this.methodPM.TypeName = value;
        }
    }

    get Amount() { return this.methodPM.Amount; }
    set Amount(value: number) {
        if (this.methodPM.Amount != value) {
            this.methodPM.Amount = value;
            this.parent.CalcMethodItemsAmountTotal();
        }
    }

    get BankCode() { return this.methodPM.BankCode; }
    set BankCode(value: string) {
        if (this.methodPM.BankCode != value) {
            this.methodPM.BankCode = value;
        }
    }

    get InternalBankId() { return this.methodPM.InternalBankId; }
    set InternalBankId(value: string) {
        if (this.methodPM.InternalBankId != value) {
            this.methodPM.InternalBankId = value;

            this.customBankListService.getAllFromCache().subscribe((response: ServiceResponse) => {
                if (response) {
                    if (!response.HasError) {
                        var customBank: CustomBankList = response.Result.filter(d => d.Id == value)[0];

                        if (customBank == null && this.BanksList != null) {
                            customBank = this.BanksList.filter(d => d.Id == value)[0];
                        }
                        if (customBank != null) {
                            this.InternalBankName = customBank.LocalName != null ? customBank.LocalName : customBank.BankName;
                            if (customBank.PayerTypeCode == "0") {

                                this.customBankCardExtendedPMService.GetSingleCustomBanksCard(value, this.parent.EntityPM.CustomerId).subscribe((response: ServiceResponse) => {

                                    if (response) {
                                        if (!response.HasError) {
                                            var bankCard: CustomBanksCardPM = response.Result;
                                            if (bankCard != null) {
                                                this.methodPM.BankCode = customBank.BankCode;
                                                this.methodPM.BranchCode = customBank.BranchCode;
                                                this.methodPM.CustomsBranchId = customBank.CustomsBranchId;
                                                this.methodPM.AccountNumber = customBank.AccountNumber;
                                                this.CustomerActivityTypeCode = customBank.PayerTypeCode;
                                                this.CustomerActivityTypeName = customBank.PayerTypeName;
                                            }
                                            else {
                                                var messageWindow = new MessageWindow();
                                                messageWindow.Show(TextCodeTranslator.Translate("Customs.CustomBank.O.BankNotConnectedToCustomer"));
                                                this.InternalBankId = null;
                                                this.methodPM.InternalBankId = null;
                                                this.InternalBankName = null;
                                                this.SelectedBank = null;
                                            }
                                        }
                                    }

                                });

                            }
                            else {
                                this.methodPM.BankCode = customBank.BankCode;
                                this.methodPM.BranchCode = customBank.BranchCode;
                                this.methodPM.CustomsBranchId = customBank.CustomsBranchId;
                                this.methodPM.AccountNumber = customBank.AccountNumber;
                                this.CustomerActivityTypeCode = customBank.PayerTypeCode;
                                this.CustomerActivityTypeName = customBank.PayerTypeName;
                            }
                        }

                        if (customBank == null) {
                            this.methodPM.BankCode = null;
                            this.methodPM.BranchCode = null;
                            this.methodPM.CustomsBranchId = null;
                            this.methodPM.AccountNumber = null;
                            this.CustomerActivityTypeCode = null;
                        }

                    }
                }
            });

        }
    }

    private internalBankName: string;
    get InternalBankName() { return this.internalBankName; }
    set InternalBankName(value: string) {
        if (this.internalBankName != value) {
            this.internalBankName = value;
        }
    }

    public get CustomerActivityTypeCode() { return this.methodPM.CustomerActivityTypeCode; }
    public set CustomerActivityTypeCode(newValue: string) { this.methodPM.CustomerActivityTypeCode = newValue; }

    get CustomerActivityTypeName() { return this.methodPM.CustomerActivityTypeName; }
    set CustomerActivityTypeName(value: string) {
        if (this.methodPM.CustomerActivityTypeName != value) {
            this.methodPM.CustomerActivityTypeName = value;
        }
    }

    public get PaymentMethodStatusCode() { return this.methodPM.PaymentMethodStatusCode; }
    public set PaymentMethodStatusCode(newValue: string) { this.methodPM.PaymentMethodStatusCode = newValue; }

    get PaymentMethodStatusName() { return this.methodPM.PaymentMethodStatusName; }
    set PaymentMethodStatusName(value: string) {
        if (this.methodPM.PaymentMethodStatusName != value) {
            this.methodPM.PaymentMethodStatusName = value;
        }
    }
    //#endregion

    public SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }
    }

}
