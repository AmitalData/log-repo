import { Component } from '@angular/core';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool, ArrayTool } from '../../../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ClaimPM } from '../../../../../Customs/EntityPMs/ClaimPM';
import { ClientAddressPM } from '../../../../../Customs/EntityPMs/ClientAddressPM';
import { ClaimsRelatedEntityPM } from '../../../../../Customs/EntityPMs/ClaimsRelatedEntityPM';
import { ClientPM } from '../../../../../Customs/EntityPMs/ClientPM';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { CustomsRequestMenuService } from '../../../../../Customs/Services/Others/CustomsRequestMenuService';import { ClientsAddressCommTypePM } from '../../../../../Customs/EntityPMs/ClientsAddressCommTypePM';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { ConfirmWindow } from '../../../../../Controls/Windows/ConfirmWindow';
import { ClientMessagesService } from '../../../../../Customs/Services/WebServices/ClientMessagesService';
import { ClaimPMService } from '../../../../../Customs/Services/StandardPMs/ClaimPMService';
import { ClientPMService } from '../../../../../Customs/Services/StandardPMs/ClientPMService';
import { Validator } from '../../../../../Infrastructure/Validators/Validator';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: './ClaimGeneralTabComponent.html',
})

export class ClaimGeneralTabComponent extends BaseComponent {
    public DataContext: ClaimGeneralTabComponent = this;
    public EntityPM: ClaimPM = new ClaimPM();
    public ObjectTableName: string = "Customs.Claim";

    public ClaimsRelatedEntitiesObslist: ObservableCollection;

    public AgentAddressesList: AddressItemComponent[] = [];
    public ClienAddressesList: AddressItemComponent[] = [];
    public ContactAddressesList: AddressItemComponent[] = [];

    public CurrentEditComponentId: string;
    private isControlEnabled: boolean = true;
    private IsClientPassportEnabled: boolean = false;

    public ClaimPMService: ClaimPMService = new ClaimPMService;
    public ClientMessagesService: ClientMessagesService = new ClientMessagesService;
    public ClientPMService: ClientPMService = new ClientPMService;

    IsLoaded: boolean = false;
    constructor(public entityArgs: EntityArgs, private EntityResourceService: EntityResourceService) {
        super();
        this.ClaimsRelatedEntitiesObslist = new ObservableCollection([]);
        SessionLocator.SelectedSession.CurrentEditComponent.ValidationErrorsList = [];

        SessionLocator.SelectedSession.StartBusyIndicator("");
        this.EntityResourceService.getEntityResourceByTableName("Customs.Claim").subscribe(response => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.ClaimsRelatedEntity").subscribe(response => {
                this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrderMethod").subscribe(response => {
                    SessionLocator.SelectedSession.StopBusyIndicator();
                    if (this.entityArgs.EntityPM != null) {
                        this.EntityPM = this.entityArgs.EntityPM;
                        this.BuildRelatedEntitiesList();
                        this.BuildAddressesList();
                        if (AppTool.IsNullOrEmpty(this.ClientId) &&
                            !AppTool.IsNullOrEmpty(this.EntityPM.PassportTypeCode) || !AppTool.IsNullOrEmpty(this.EntityPM.PassportNumber) || !AppTool.IsNullOrEmpty(this.EntityPM.PassportCountryTypeCode)) {
                            this.IsClientPassportEnabled = true;
                            this.UIProperties.SetEnabled("ClientId", "Customs.Claim", false);
                        }
                    }
                    this.Listen();
                    this.IsLoaded = true;
                });
            });
        });

    }

    private Listen() {
        if (SessionLocator.SelectedSession.CurrentEditComponent != null) {

            this.CurrentEditComponentId = SessionLocator.SelectedSession.CurrentEditComponent.ComponentId;
            SessionLocator.SelectedSession.CurrentEditComponent.SubscriptionAdd(
                SessionLocator.SelectedSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = SessionLocator.SelectedSession.CurrentEditComponent.EntityPM;
                    }
                })
            );
            SessionLocator.SelectedSession.CurrentEditComponent.SubscriptionAdd(
                SessionLocator.SelectedSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = SessionLocator.SelectedSession.CurrentEditComponent.EntityPM;
                        this.BuildRelatedEntitiesList();
                        //this.BuildAddressesList();
                    }
                })
            );
            SessionLocator.SelectedSession.CurrentEditComponent.SubscriptionAdd(
                SessionLocator.SelectedSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == SessionLocator.SelectedSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "CLMG") {
                            //this.RefreshEntity();
                        }
                    }
                })
            );
        }
    }

    RefreshEntity() {
        SessionLocator.SelectedSession.CurrentEditComponent.ReloadEntityPM();
    }

    BuildRelatedEntitiesList() {
        this.ClaimsRelatedEntitiesObslist = new ObservableCollection([]);

        if (this.EntityPM.ClaimsRelatedEntities != null && this.EntityPM.ClaimsRelatedEntities.length > 0) {
            this.EntityPM.ClaimsRelatedEntities.sort(
                (a, b) => { return (a.EntityCounterKey === b.EntityCounterKey) ? 0 : (a.EntityCounterKey < b.EntityCounterKey) ? -1 : 1 });
            for (let item of this.EntityPM.ClaimsRelatedEntities) {
                this.ClaimsRelatedEntitiesObslist.Insert(new ClaimsRelatedEntityLineComponent(item, this.EntityPM,false));
            }
        }
    }

    public BuildAddressesList(){
        //AddressCode
        this.BuildAgentAddressesList();

        //CustomsAddressCode & ContactPhoneAddressCode
        this.BuildClienAddressesList();
    }

    public BuildAgentAddressesList() {
        this.AgentAddressesList = [];
        
        if (!AppTool.IsNullOrEmpty(this.EntityPM.ClaimSubmiterNumber)) {
            this.ClientMessagesService.GetSingleClientPMByCode(this.EntityPM.ClaimSubmiterNumber, true)
                .subscribe((myResponse: ServiceResponse) => {
                    this.GetSingleCustomerOp_Completed(myResponse, false);
                });
        }
        else {
            var clientPM: ClientPM = new ClientPM();
            var clientAddressPM: ClientAddressPM = new ClientAddressPM(clientPM);
            var addressItemViewModel: AddressItemComponent = new AddressItemComponent(clientAddressPM, clientPM, false, "AddressCode");
            this.AgentAddressesList.push(addressItemViewModel);
        }
    }

    public BuildClienAddressesList() {
        this.ClienAddressesList = [];
        this.ContactAddressesList = [];

        if (!AppTool.IsNullOrEmpty(this.EntityPM.ClientId)) {
            this.ClientPMService.get(this.EntityPM.ClientId)
                .subscribe((myResponse: ServiceResponse) => {
                    this.GetSingleClientOp_Completed(myResponse, false);
                });
        }
        else if (!AppTool.IsNullOrEmpty(this.EntityPM.PassportNumber)) {
            this.ClientMessagesService.GetSingleClientPMByPassportNumberOrCountry(this.EntityPM.PassportNumber, this.EntityPM.PassportCountryTypeCode)
                .subscribe((myResponse: ServiceResponse) => {
                    this.GetSingleClientOp_Completed(myResponse, false);
                });
        }
        else {
            var clientPM: ClientPM = new ClientPM();
            var clientAddressPM: ClientAddressPM = new ClientAddressPM(clientPM);
            var addressItemViewModelClient: AddressItemComponent = new AddressItemComponent(clientAddressPM, clientPM, false, "CustomsAddressCode");
            this.ClienAddressesList.push(addressItemViewModelClient);

            var addressItemViewModelContact: AddressItemComponent = new AddressItemComponent(clientAddressPM, clientPM, false, "ContactPhoneAddressCode");
            this.ContactAddressesList.push(addressItemViewModelContact);
        }
    }

    private GetSingleCustomerOp_Completed(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        
        if (myResponse.Result != null) {
            var clientPM: ClientPM = myResponse.Result;
            var clientAddressPM: ClientAddressPM = new ClientAddressPM(clientPM);

            if (!AppTool.IsNullOrEmpty(this.AddressCode) && (clientPM.ClientAddresses != null && clientPM.ClientAddresses.length > 0)) {
                clientAddressPM = clientPM.ClientAddresses.filter(d => d.CustomAddressCode == this.AddressCode)[0];
                if (clientAddressPM == null) {
                    clientAddressPM = new ClientAddressPM(clientPM); 
                }
            }
            var addressItemViewModel: AddressItemComponent = new AddressItemComponent(clientAddressPM, clientPM, false, "AddressCode");
            this.AgentAddressesList.push(addressItemViewModel);
        }
        else {
            var clientPM: ClientPM = new ClientPM();
            var clientAddressPM: ClientAddressPM = new ClientAddressPM(clientPM);
            var addressItemViewModel: AddressItemComponent = new AddressItemComponent(clientAddressPM, clientPM, false, "AddressCode");
            this.AgentAddressesList.push(addressItemViewModel);
        }
    }

    private GetSingleClientOp_Completed(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {

        if (myResponse.Result != null) {
            var clientPM: ClientPM = myResponse.Result;

            //Get CustomsAddressCode
            var clientCustomsAddressPM: ClientAddressPM = new ClientAddressPM(clientPM);
            if (!AppTool.IsNullOrEmpty(this.CustomsAddressCode) && (clientPM.ClientAddresses != null && clientPM.ClientAddresses.length > 0)) {
                clientCustomsAddressPM = clientPM.ClientAddresses.filter(d => d.CustomAddressCode == this.CustomsAddressCode)[0];
                if (clientCustomsAddressPM == null) {
                    clientCustomsAddressPM = new ClientAddressPM(clientPM);
                }
            }
            var customsAddressItemViewModel: AddressItemComponent = new AddressItemComponent(clientCustomsAddressPM, clientPM, false,"CustomsAddressCode");
            this.ClienAddressesList.push(customsAddressItemViewModel);

            //Get ContactPhoneAddressCode
            var clientAddressPM: ClientAddressPM = new ClientAddressPM(clientPM);
            if (!AppTool.IsNullOrEmpty(this.ContactPhoneAddressCode) && (clientPM.ClientAddresses != null && clientPM.ClientAddresses.length > 0)) {
                clientAddressPM = clientPM.ClientAddresses.filter(d => d.CustomAddressCode == this.ContactPhoneAddressCode)[0];
                if (clientAddressPM == null) {
                    clientAddressPM = new ClientAddressPM(clientPM);
                }
            }
            var addressItemViewModel: AddressItemComponent = new AddressItemComponent(clientAddressPM, clientPM, false,"ContactPhoneAddressCode");
            this.ContactAddressesList.push(addressItemViewModel);
        }
    }

    selectedTab: LogTab;
    public get SelectedTab() { return this.selectedTab; }
    public set SelectedTab(tab: LogTab) {
        this.selectedTab = tab;
    }
    
    public SetTabArgs(args: any, valdationErrorList: any[] = null) {
        this.EntityPM = args.EntityPM;
        console.log("EntityPM", this.EntityPM);
    }

    public get IsControlEnabled() { return this.isControlEnabled; }
    public set IsControlEnabled(newValue: boolean) { this.isControlEnabled = newValue; }

    public get SubmitDate() { return this.EntityPM.SubmitDate; }
    public set SubmitDate(newValue: Date) { this.EntityPM.SubmitDate = newValue; }

    public get ReferantId() { return this.EntityPM.ReferantId; }
    public set ReferantId(newValue: string) { this.EntityPM.ReferantId = newValue; }

    public get ClientId() { return this.EntityPM.ClientId; }
    public set ClientId(newValue: string) {
        if (this.EntityPM.ClientId != newValue) {
            this.EntityPM.ClientId = newValue;
            this.ClientIdSelectionChanged();
        }
    }

    public get SoldierPersonalNumber() { return this.EntityPM.SoldierPersonalNumber; }
    public set SoldierPersonalNumber(newValue: string) { this.EntityPM.SoldierPersonalNumber = newValue; }

    public CustomerDependencyProperty1: string = "CS";
    public get CustomerId() { return this.EntityPM.CustomerId; }
    public set CustomerId(newValue: string) { this.EntityPM.CustomerId = newValue; }

    public get AddressCode() { return this.EntityPM.AddressCode; }
    public set AddressCode(newValue: string) { this.EntityPM.AddressCode = newValue; }

    public get CustomsAddressCode() { return this.EntityPM.CustomsAddressCode; }
    public set CustomsAddressCode(newValue: string) { this.EntityPM.CustomsAddressCode = newValue; }

    public get ContactPhoneAddressCode() { return this.EntityPM.ContactPhoneAddressCode; }
    public set ContactPhoneAddressCode(newValue: string) { this.EntityPM.ContactPhoneAddressCode = newValue; }

    //#region Address
    ClientIdSelectionChanged() {

        if (AppTool.IsNullOrEmpty(this.ContactPhoneAddressCode) && AppTool.IsNullOrEmpty(this.CustomsAddressCode)) {
            this.BuildClienAddressesList();
            return;
        }

        var clientSelectionChangedWindow = new ConfirmWindow();
        clientSelectionChangedWindow.Width = 250;
        clientSelectionChangedWindow.Height = 150;
        clientSelectionChangedWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
        clientSelectionChangedWindow.NoButtonText = TextCodeTranslator.Translate("Customs.General.B.Cancel");
        clientSelectionChangedWindow.ShowCancelButton = false;
        clientSelectionChangedWindow.Show("בפעולה זו ימחקו כל הכתובות של הלקוח, האם להמשיך?");

        clientSelectionChangedWindow.WindowClosed.subscribe((event: any) => {
            if (clientSelectionChangedWindow.Yes) {
                this.DeleteClientAddressLines();
            }
            else if (clientSelectionChangedWindow.No) {
                var oldClientId: string = "";
                if (this.ClienAddressesList.length == 1) {
                    oldClientId = this.ClienAddressesList[0].ClientPM.Id;
                }
                else if (this.ContactAddressesList.length == 1) {
                    oldClientId = this.ContactAddressesList[0].ClientPM.Id;
                }
                this.ClientId = oldClientId;
            }

        });

    }

    DeleteClientAddressLines() {
        this.CustomsAddressCode = null;
        this.ContactPhoneAddressCode = null;

        this.BuildClienAddressesList();
    }

    DeleteAddress(item: AddressItemComponent) {
        if ((item.AddressMode == "AddressCode" && AppTool.IsNullOrEmpty(this.AddressCode))
            || (item.AddressMode == "CustomsAddressCode" && AppTool.IsNullOrEmpty(this.CustomsAddressCode))
            || (item.AddressMode == "ContactPhoneAddressCode" && AppTool.IsNullOrEmpty(this.ContactPhoneAddressCode))) {

            return;
        }

        var clientAddressPM: ClientAddressPM = new ClientAddressPM(item.ClientPM);

        switch (item.AddressMode) {
            case "AddressCode":
                this.EntityPM.AddressCode = null;
                this.AgentAddressesList = [];
                var addressItemViewModel: AddressItemComponent = new AddressItemComponent(clientAddressPM, item.ClientPM, false, "AddressCode");
                this.AgentAddressesList.push(addressItemViewModel);
                break;
            case "CustomsAddressCode":
                this.EntityPM.CustomsAddressCode = null;
                this.ClienAddressesList = [];
                var addressItemViewModelClient: AddressItemComponent = new AddressItemComponent(clientAddressPM, item.ClientPM, false, "CustomsAddressCode");
                this.ClienAddressesList.push(addressItemViewModelClient);
                break;
            case "ContactPhoneAddressCode":
                this.EntityPM.ContactPhoneAddressCode = null;
                this.ContactAddressesList = [];
                var addressItemViewModelContact: AddressItemComponent = new AddressItemComponent(clientAddressPM, item.ClientPM, false, "ContactPhoneAddressCode");
                this.ContactAddressesList.push(addressItemViewModelContact);
                break;
            default:
                break;
        }
    }

    private CurrentSearchAddressMode: string;
    SearchAddress(item: AddressItemComponent) {
        this.CurrentSearchAddressMode = "";

        if ((item.AddressMode == "AddressCode" && AppTool.IsNullOrEmpty(item.ClientPM.Id)
            || (item.AddressMode == "CustomsAddressCode" || item.AddressMode == "ContactPhoneAddressCode") && AppTool.IsNullOrEmpty(item.ClientPM.Id) && AppTool.IsNullOrEmpty(this.EntityPM.PassportNumber))) {

            var text: string = "ראשית חובה לבחור לקוח";
            if (item.AddressMode == "AddressCode") {
                text = "ראשית חובה לבחור מגיש לתביעה";
            }
            
            var messageWindow = new MessageWindow();
            messageWindow.Title = TextCodeTranslator.Translate("Customs.General.O.Warning");
            messageWindow.Width = 250;
            messageWindow.Height = 150;
            messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            messageWindow.Show(text);
            return;
        }

        this.CurrentSearchAddressMode = item.AddressMode;

        this.EntityResourceService.getEntityResourceByTableName("Customs.ClientAddress").subscribe(response => {
            var windowArgs: any = {};
            windowArgs.EntityPM = item.ClientPM;
            windowArgs.Parent = this;

            var logWindow = new LogitudeWindow();
            logWindow.Width = 850;
            logWindow.Height = 500;
            logWindow.ShowCloseButton = true;
            logWindow.WindowArgs = windowArgs;
            logWindow.Title = "כתובות לקוח " + item.ClientPM.Code;
            logWindow.Show('./CustomsModules/CustomsClient/Components/EditTabs/Addresses/ClientAddressesTabComponent');
        });
    }

    SelectAddresseCompleted(selectedAddresse: ClientAddressPM) {

        if (selectedAddresse == null) {
            return;
        }

        if (AppTool.IsNullOrEmpty(selectedAddresse.CustomAddressCode)) {
            var messageWindow = new MessageWindow();
            messageWindow.Title = TextCodeTranslator.Translate("Customs.General.O.Warning");
            messageWindow.Width = 250;
            messageWindow.Height = 150;
            messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            messageWindow.Show("לא ניתן לבחור כתובת זו, אנא שלוף לקוח מחדש");
            return;
        }

        switch (this.CurrentSearchAddressMode) {
            case "AddressCode":
                this.AddressCode = selectedAddresse.CustomAddressCode;
                this.BuildAgentAddressesList();
                break;
            case "CustomsAddressCode":
                this.CustomsAddressCode = selectedAddresse.CustomAddressCode;
                this.BuildClienAddressesList();
                break;
            case "ContactPhoneAddressCode":
                this.ContactPhoneAddressCode = selectedAddresse.CustomAddressCode;
                this.BuildClienAddressesList();
                break;
            default:
                break;
        }
        this.CurrentSearchAddressMode = "";
    }

    //#endregion


    //#region Related Entities
    EditButtonClicked(item: ClaimsRelatedEntityLineComponent) {
        SessionLocator.SelectedSession.CurrentEditComponent.ValidationErrorsList = [];
        SessionLocator.SelectedSession.StartBusyIndicator("");

        this.ClaimPMService.update(this.EntityPM).subscribe((response: ServiceResponse) => {
            var claim = response.Result;
            SessionLocator.SelectedSession.StopBusyIndicator();
            if (!AppTool.IsNullOrEmpty(claim)) {
                if (!AppTool.IsNullOrEmpty(item)) {
                    this.EditClaimsRelatedEntityLine(item, false);
                }
            }
        });
    }

    EditClaimsRelatedEntityLine(item: ClaimsRelatedEntityLineComponent, isNewEntity: boolean) {
        SessionLocator.SelectedSession.StartBusyIndicator("");

        var windowArgs: any = {};
        windowArgs.EntityCounterKey = item.entityPM.EntityCounterKey;
        windowArgs.ClaimPM = this.EntityPM;
        windowArgs.IsNewEntity = isNewEntity;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 700;
        logWindow.Height = 550;

        if (isNewEntity) {
            windowArgs.WindowTitle = TextCodeTranslator.Translate("Customs.Claim.O.NewClaimsRelatedEntity");
        }
        else {
            var tapagNumberAndNumeral: string = "";
            if (!AppTool.IsNullOrEmpty(item.TapagNumberAndNumeral)) {
                tapagNumberAndNumeral = item.TapagNumberAndNumeral;
            }
            windowArgs.WindowTitle = TextCodeTranslator.Translate("Customs.Claim.O.EditClaimsRelatedEntity") + " " + tapagNumberAndNumeral;
        }
            
        //windowArgs.IsDisplayOnly = this.IsDisplayOnly;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe((event: any) => {
            if (event == 'ok') {
                this.RefreshEntity();
                //this.EntityPM = SessionLocator.SelectedSession.CurrentEditComponent.EntityPM;
            }
        });

        logWindow.IsHideHeader = true;
        logWindow.Show('./CustomsModules/CustomsClaim/Components/EditTabs/RelatedEntity/ClaimRelatedEntityTabComponent');
        SessionLocator.SelectedSession.StopBusyIndicator();
        
    }

    DeleteButtonClicked(item: ClaimsRelatedEntityLineComponent) {
        if (!this.IsControlEnabled) return;

        if (!AppTool.IsNullOrEmpty(item.entityPM.TapagNumber)) {
            var messageWindow = new MessageWindow();
            messageWindow.Width = 250;
            messageWindow.Height = 150;
            messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            messageWindow.Show("לא ניתן למחוק ישות תביעה המקושרת לתיק תפג");
            return;
        }

        var rfundDemandUncheckedWindow: ConfirmWindow = new ConfirmWindow();
        rfundDemandUncheckedWindow.Title = "Delete";
        rfundDemandUncheckedWindow.Width = 250;
        rfundDemandUncheckedWindow.Height = 150;
        rfundDemandUncheckedWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
        rfundDemandUncheckedWindow.NoButtonText = TextCodeTranslator.Translate("Customs.General.B.Cancel");
        rfundDemandUncheckedWindow.ShowCancelButton = false;
        rfundDemandUncheckedWindow.Show(TextCodeTranslator.Translate("Customs.Claim.G.DeleteClaimRelatedEntity"));
        rfundDemandUncheckedWindow.WindowClosed.subscribe((event: any) => {
            if (rfundDemandUncheckedWindow.Yes) {
                this.DeleteSelected(item);
            }
        });

    }

    DeleteSelected(item: ClaimsRelatedEntityLineComponent) {
        this.ClaimsRelatedEntitiesObslist.Remove(item);
        this.EntityPM.RemoveClaimsRelatedEntity(item.entityPM);
    }


    CancelOrObjectionButtonClicked(item: ClaimsRelatedEntityLineComponent, isNewEntity: boolean) {
        if (!this.IsControlEnabled) return;

        if (AppTool.IsNullOrEmpty(item.entityPM.TapagNumber)) {
            var messageWindow = new MessageWindow();
            messageWindow.Width = 250;
            messageWindow.Height = 150;
            messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            messageWindow.Show("לא ניתן לבטל/ערר ישות תביעה שאינה המקושרת לתיק תפג");
            return;
        }

        SessionLocator.SelectedSession.StartBusyIndicator("");
        var windowArgs: any = {};
        windowArgs.ClaimsRelatedEntityPM = item.entityPM;
        windowArgs.ClaimPM = this.EntityPM;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 500;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.Title = TextCodeTranslator.Translate("Customs.Claim.TH.CancelOrObjection");

        logWindow.Show('./CustomsModules/CustomsClaim/Components/EditTabs/RelatedEntity/ClaimRelatedEntityCancelOrObjectionTabComponent');
        SessionLocator.SelectedSession.StopBusyIndicator();
    }

    AddEntityCommand() {

        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (errors.length > 0) {
            SessionLocator.SelectedSession.CurrentEditComponent.ValidationErrorsList = errors;
            return;
        }
        var newClaimsRelatedEntityPM = new ClaimsRelatedEntityPM(this.EntityPM);
        newClaimsRelatedEntityPM.ClaimId = this.EntityPM.Id;
        newClaimsRelatedEntityPM.Tenant = this.EntityPM.Tenant;
        //newClaimsRelatedEntityPM.EntityCounterKey = (ArrayTool.Max(this.EntityPM.ClaimsRelatedEntities, "EntityCounterKey") + 1);
        newClaimsRelatedEntityPM.IsSendClaimsRelatedEntity = true;

        let newClaimsRelatedEntityLineComponent = new ClaimsRelatedEntityLineComponent(newClaimsRelatedEntityPM, this.EntityPM, false);
        this.ClaimsRelatedEntitiesObslist.Insert(newClaimsRelatedEntityLineComponent);
        this.EntityPM.AddClaimsRelatedEntity(newClaimsRelatedEntityPM);

        SessionLocator.SelectedSession.CurrentEditComponent.ValidationErrorsList = [];
        SessionLocator.SelectedSession.StartBusyIndicator("");

        this.EditClaimsRelatedEntityLine(newClaimsRelatedEntityLineComponent,true);
    }

    EditClient() {

        if (!AppTool.IsNullOrEmpty(this.ClientId)) return;

        SessionLocator.SelectedSession.StartBusyIndicatorLoading();
        SessionLocator.SelectedSession.CurrentEditComponent.SaveChanges();
        SessionLocator.SelectedSession.StopBusyIndicator();
        var windowArgs: any = {};
        windowArgs.EntityPM = this.EntityPM;
        //windowArgs.IsDisplayOnly = this.IsDisplayOnly;
        var windowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.ImporterDetails");

        var logWindow = new LogitudeWindow();
        logWindow.Width = 550;
        logWindow.Height = 350;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.SetFieldsDisabled($event));
        logWindow.Show('./CustomsModules/CustomsClaim/Components/EditTabs/General/PassportDetails/PassportDetailsComponent');
    }

    SetFieldsDisabled(message: string) {
        if (message == "ok") {
            if (AppTool.IsNullOrEmpty(this.ClientId) &&
                !AppTool.IsNullOrEmpty(this.EntityPM.PassportTypeCode) || !AppTool.IsNullOrEmpty(this.EntityPM.PassportNumber) || !AppTool.IsNullOrEmpty(this.EntityPM.PassportCountryTypeCode)) {
                this.IsClientPassportEnabled = true;
                this.UIProperties.SetEnabled("ClientId", "Customs.Claim", false);
            }
            else {
                this.IsClientPassportEnabled = false;
                this.UIProperties.SetEnabled("ClientId", "Customs.Claim", true);
            }
        }
           
    }

    //#endregion
}

export class ClaimsRelatedEntityLineComponent extends BaseComponent {
    public ObjectTableName = "Customs.ClaimsRelatedEntity";
    public DataContext = this;
    private _IsSendEnabled: boolean = true;

    constructor(public entityPM: ClaimsRelatedEntityPM, public claimPM: ClaimPM, isEnable: Boolean) {
        super();
    }

    public get ClaimEntityTypeName() { return this.entityPM.ClaimEntityTypeName; }
    public set ClaimEntityTypeName(newValue: string) { this.entityPM.ClaimEntityTypeName = newValue; }

    public get ClaimEntityNumber() { return this.entityPM.ClaimEntityNumber; }
    public set ClaimEntityNumber(newValue: string) { this.entityPM.ClaimEntityNumber = newValue; }

    public get TapagNumberAndNumeral() {
        if (this.entityPM != null && !AppTool.IsNullOrEmpty(this.entityPM.TapagNumber)) {
            this.IsSendEnabled = false;
            this.IsSendClaimsRelatedEntity = false;
            this.UIProperties.SetEnabled("IsSendClaimsRelatedEntity", "Customs.Claim", false);
            if (this.entityPM.Numeral != null && this.entityPM.Numeral > 0) {
                return this.entityPM.TapagNumber + "-" + this.entityPM.Numeral;
            }
            return this.entityPM.TapagNumber;
        }
    }

    public get IsFinancialRefundDemand() { return this.entityPM.IsFinancialRefundDemand; }
    public set IsFinancialRefundDemand(newValue: boolean) { this.entityPM.IsFinancialRefundDemand = newValue; }

    public get ClaimAmount() { return this.entityPM.ClaimAmount; }
    public set ClaimAmount(newValue: number) { this.entityPM.ClaimAmount = newValue; }

    public get ContinuousMessagesTypeName() { return this.entityPM.ContinuousMessagesTypeName; }
    public set ContinuousMessagesTypeName(newValue: string) { this.entityPM.ContinuousMessagesTypeName = newValue; }

    public get DecisionName() { return this.entityPM.DecisionName; }
    public set DecisionName(newValue: string) { this.entityPM.DecisionName = newValue; }

    public get IsSendClaimsRelatedEntity() { return this.entityPM.IsSendClaimsRelatedEntity; }
    public set IsSendClaimsRelatedEntity(newValue: boolean) { this.entityPM.IsSendClaimsRelatedEntity = newValue; }

    public get IsSendEnabled() { return this._IsSendEnabled; }
    public set IsSendEnabled(newValue: boolean) { this._IsSendEnabled = newValue; }
}

export class AddressItemComponent extends BaseComponent {
    public AddressPM: ClientAddressPM;
    private IsNew: boolean = false;
    public ClientPM: ClientPM;
    public CommunicationList: ClientsAddressCommTypePM[] = [];
    public AddressMode: string;

    constructor(addressPm: ClientAddressPM, clientPM: ClientPM, isNew: boolean, addressMode: string) {
        super();

        this.AddressPM = addressPm;
        this.ClientPM = clientPM;
        this.IsNew = isNew;
        this.AddressMode = addressMode;

        this.BuildCommunicationList();
    }

    BuildCommunicationList() {
        this.CommunicationList = [];

        if (this.AddressPM.ClientsAddressCommTypes != null && this.AddressPM.ClientsAddressCommTypes.length > 0) {
            this.CommunicationList.push(this.AddressPM.ClientsAddressCommTypes[0]);
        }
    }

    public get IsHebrewAddress() { return this.AddressPM.IsHebrewAddress; }
    public set IsHebrewAddress(newValue: boolean) { this.AddressPM.IsHebrewAddress = newValue; }

    public get AddressTypeName() { return this.AddressPM.AddressTypeName; }
    public set AddressTypeName(newValue: string) { this.AddressPM.AddressTypeName = newValue; }

    public get AddressPurposeName() { return this.AddressPM.AddressPurposeName; }
    public set AddressPurposeName(newValue: string) { this.AddressPM.AddressPurposeName = newValue; }

    public get LocalStreetName() { return this.AddressPM.LocalStreetName; }
    public set LocalStreetName(newValue: string) { this.AddressPM.LocalStreetName = newValue; }

    public get LocalHouseNumber() { return this.AddressPM.LocalHouseNumber; }
    public set LocalHouseNumber(newValue: string) { this.AddressPM.LocalHouseNumber = newValue; }

    public get LocalCityName() { return this.AddressPM.LocalCityName; }
    public set LocalCityName(newValue: string) { this.AddressPM.LocalCityName = newValue; }

    public get EnglishCountryName() { return this.AddressPM.EnglishCountryName; }
    public set EnglishCountryName(newValue: string) { this.AddressPM.EnglishCountryName = newValue; }

    public get EnglishSubCountryName() { return this.AddressPM.EnglishSubCountryName; }
    public set EnglishSubCountryName(newValue: string) { this.AddressPM.EnglishSubCountryName = newValue; }

    public get EnglishCityName() { return this.AddressPM.EnglishCityName; }
    public set EnglishCityName(newValue: string) { this.AddressPM.EnglishCityName = newValue; }

    public get ContactFirstName() { return this.AddressPM.ContactFirstName; }
    public set ContactFirstName(newValue: string) { this.AddressPM.ContactFirstName = newValue; }

    public get ContactRoleTypeName() { return this.AddressPM.ContactRoleTypeName; }
    public set ContactRoleTypeName(newValue: string) { this.AddressPM.ContactRoleTypeName = newValue; }
}
