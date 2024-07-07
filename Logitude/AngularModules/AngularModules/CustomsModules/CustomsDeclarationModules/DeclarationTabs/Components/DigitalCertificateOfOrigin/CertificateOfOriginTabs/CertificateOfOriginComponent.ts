import { Component, ViewChildren, QueryList, ChangeDetectorRef } from '@angular/core';
import { EntityArgs } from '../../../../../../Infrastructure/DataContracts/EntityArgs';
import { CreateClientRequestParams, ClientAdressParams, ClientAddressCommunicationType, ClientDrivingLicenseParams, ClientDrivingLicenseTypeParams } from 'Customs/DataContract/RequestParams/CreateClientRequestParams';

declare var window: any;
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { FeatureLocator } from 'Infrastructure/Utilities/FeatureLocator';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { ClientMessagesService } from 'Customs/Services/WebServices/ClientMessagesService';
import { INF_MSG_GenericResponseData } from 'Customs/DataContract/ResponseData/INF_MSG_GenericResponseData';
import { CustomMessageProgressComponent } from 'CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { Validator } from 'Infrastructure/Validators/Validator';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { AppTool } from 'Infrastructure/Tools';
import { LocationDirective } from 'Infrastructure/Utilities/LocationDirective';
import { CertificateOfOriginPM } from 'Customs/EntityPMs/CertificateOfOriginPM';
import { CertificateOfOriginPMService } from 'Customs/Services/StandardPMs/CertificateOfOriginPMService';
import { DeclarationPM } from 'Customs/EntityPMs/DeclarationPM';
import { StatusCertificateOfOrigin } from '../DigitalCertificateOfOriginTabComponent';
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { SendRequestVIA } from 'Customs/DataContract/RequestParams/RequestParamsBase';
import { CertificateOfOriginWebService } from 'Customs/Services/WebServices/CertificateOfOriginWebService';
import { CertificateOfOriginRequestRequestParams } from 'Customs/DataContract/RequestParams/CertificateOfOriginRequestRequestParams';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from 'CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CertificateOfOriginListService } from 'Customs/Services/StandardLists/CertificateOfOriginListService';
import { BehaviorSubject } from 'rxjs';


@Component({

    templateUrl: './CertificateOfOriginComponent.html',
    providers: [EntityArgs],
})

export class CertificateOfOriginComponent extends BaseRequestsSheetMassaging {
    public right: any;

    public TabsItemsSource: TabItem[] = [];
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    public EntityPM: CertificateOfOriginPM;
    certificateOfOriginPMService: CertificateOfOriginPMService = new CertificateOfOriginPMService();
    certificateOfOriginWebService: CertificateOfOriginWebService = new CertificateOfOriginWebService();
    certificateOfOriginListService: CertificateOfOriginListService = new CertificateOfOriginListService();


    public entityResourceService: EntityResourceService = new EntityResourceService();
    public DataContext: any = this;

    responseData: INF_MSG_GenericResponseData;
    private CurrentSession = SessionLocator.SelectedSession;
    public DecalarationData: DeclarationPM;
    public IsNewOrEdit: StatusCertificateOfOrigin;
    isDispalyOnlyStatusList: number[] = [4, 8];
    public isAllowChange: boolean = false;


    constructor(
        public entityArgs: EntityArgs,
    ) {
        super();

    }

    private CertificateChanges = new BehaviorSubject<boolean>(false);
    isListenToChangeInCertificate(logWindow: LogitudeWindow = null) {
        // Certificate Changes subscriber:
        this.CertificateChanges.subscribe((value) => {
            // Main Title
            let title = TextCodeTranslator.Translate("Customs.Declaration.TH.CertificateOfOrigin");
            logWindow.Title = this.IsNewOrEdit == StatusCertificateOfOrigin.IsEdit && !AppTool.IsNullOrEmpty(this.EntityPM.COONumber) ? title += `: ${this.EntityPM.COONumber}` : title;

            // Side Title
            let CertificateOfOriginStatus = TextCodeTranslator.Translate("Customs.CertificateOfOrigin.O.CooStatusCode");
            logWindow.SubTitle = this.IsNewOrEdit == StatusCertificateOfOrigin.IsEdit && !AppTool.IsNullOrEmpty(this.EntityPM.CooStatusCodeName) ? CertificateOfOriginStatus += `: ${this.EntityPM.CooStatusCodeName}` : null;

            this.CurrentSession.CurrentEditComponent.SaveChanges();
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        });
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args.CertificateOfOrigin;
        this.DecalarationData = args.Decalaration;
        this.IsNewOrEdit = args.IsNewOrEdit;
        this.isAllowChange = args.isAllowChange;
        
        this.isListenToChangeInCertificate(args.logWindow);

        this.InitMoreDataScreenValues();
        this.BuildTabs();
        this.RunComponent();
        this.entityArgs.EntityPM = this.EntityPM;
        this.entityArgs.ObjectTableName = "Customs.CertificateOfOrigin";
    }

    private selectedTabCode: string;
    get SelectedTabCode() { return this.selectedTabCode; }
    set SelectedTabCode(newValue: string) {
        if (this.selectedTabCode != newValue) {
            this.selectedTabCode = newValue;
            this.SelectionChanged();
        }
    }



    addTapagEnabled: boolean;
    public get AddTapagEnabled() { return this.EntityPM ? this.addTapagEnabled : null; }
    public set AddTapagEnabled(newValue: boolean) {
        this.addTapagEnabled = newValue;
    }
    public get IsDisplayOnly() {
        return this.isDispalyOnlyStatusList.includes(Number(this.EntityPM?.CooStatusCode)) || !this.isAllowChange
    }

    BuildTabs() {
        this.TabsItemsSource = [];
        this.TabsItemsSource.push(new TabItem("GENERAL", "Customs.Declaration.TH.General"));
        this.TabsItemsSource.push(new TabItem("MOREDATA", "Customs.Declaration.O.MoreData"));
        this.TabsItemsSource.push(new TabItem("REQUESTSHEET", "Customs.Declaration.TH.RequestSheet"));
        this.TabsItemsSource.push(new TabItem("ANSWERTOCERTIFICATE", "Customs.CertificateOfOrigin.O.AnswerToCertificate"));

        // this.BuildClientsTapagList();
        this.selectedTabCode = "GENERAL"; 
    }

    RunComponent() {
        if (this.AllLocations) {

            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }

            else {
                this.isViewInited = true;
                this.InitializeComponent();
            }
        }

        else {
            this.RunComponentTimer();
        }
    }
    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }
    private isViewInited = false;
    InitializeComponent() {
        if (this.isViewInited) {
            this.SelectionChanged();
        }
    }

    private GENERAL: any = null;
    private MOREDATA: any = null;
    private REQUESTSHEET: any = null;
    private ANSWERTOCERTIFICATE: any = null;

    public ClientItemsList = null;
    public SelectedTab: TabItem;
    SelectionChanged() {
        if (!AppTool.IsNullOrEmpty(this.SelectedTabCode)) {
            let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedTabCode)[0];
            if (myLocation != null) {
                switch (this.SelectedTabCode) {
                    case "GENERAL": {
                        if (this.GENERAL == null) {
                            SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/DigitalCertificateOfOrigin/CertificateOfOriginTabs/General/CertificateOfOriginGeneralTabComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.GENERAL = cmpRef.instance;
                                    this.GENERAL.InitTab(this.EntityPM, this.DecalarationData, this.IsNewOrEdit, this.IsDisplayOnly);
                                });
                        }
                        break;
                    }

                    case "MOREDATA": {
                        if (this.MOREDATA == null) {
                            SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/DigitalCertificateOfOrigin/CertificateOfOriginTabs/MoreData/CertificateOfOriginMoreDetailsTabComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.MOREDATA = cmpRef.instance;
                                    this.MOREDATA.InitTab(this.EntityPM, this.DecalarationData, this.IsNewOrEdit, this.IsDisplayOnly);
                                });
                        }
                        else {
                            this.MOREDATA.SetWarningByCooTypeCode(this.EntityPM.CooTypeCode);
                            this.MOREDATA.updateEntity(this.EntityPM);
                        }
                        break;
                    }

                    case "REQUESTSHEET": {
                        if (this.REQUESTSHEET == null) {
                            this.entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response: any) => {
                                SessionLocator.DynamicLoader.Load("./CustomsModules/CustomsControls/Components/CustomsRequestsSheetsComponent", myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.REQUESTSHEET = cmpRef.instance;
                                        this.REQUESTSHEET.IsTitleHidden = false;
                                        this.REQUESTSHEET.Title = TextCodeTranslator.Translate("Customs.Declaration.TH.RequestSheet");
                                        this.REQUESTSHEET.MyRequestOnly = false;
                                        this.entityArgs.IsFromStandAloneScreen = true;
                                        this.REQUESTSHEET.SetEntityArgs(this.entityArgs);
                                    });

                            });
                        }
                        break;
                    }

                    case "ANSWERTOCERTIFICATE": {
                        if (this.ANSWERTOCERTIFICATE == null) {
                            SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/DigitalCertificateOfOrigin/CertificateOfOriginTabs/CertificateAnswers/CertificateAnswersComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.ANSWERTOCERTIFICATE = cmpRef.instance;
                                    this.ANSWERTOCERTIFICATE.InitTab(this.EntityPM);
                                });
                        }
                        else
                            this.ANSWERTOCERTIFICATE.InitTab(this.EntityPM);
                        break;
                    }

                    // Add more cases as needed for other options

                }
            }


        }
    }

    
    checkRequestReasonCode() {
        let counterLine = 1;
        if (this.EntityPM.RequestReasonCode != "10" && this.EntityPM.RequestReasonCode != "13"  && this.EntityPM.RequestReasonCode != "14") {
            this.EntityPM.CertificateOriginItemItems.forEach(item => {
                if(AppTool.IsNullOrEmpty(item.MarksAndNumbers)){
                    // chenge to textcode
                    // this.ValidationErrors.push( "שורה " + counterLine +"- "+ TextCodeTranslator.Translate("Customs.CertificateOfOrigin.O.MarkIsReq"));
                    this.ValidationErrors.push( "שורה " + counterLine+"- "+ "שדה סימונים ומספרים חובה למילוי");
                }
                if(item.PackingTypeName == "CONTAINER" || item.PackageType == "D5" ){
                    // chenge to textcode
                    // this.ValidationErrors.push( "שורה " + counterLine +"- "+ TextCodeTranslator.Translate("Customs.CertificateOfOrigin.O.ContainerTypeReq"));
                    this.ValidationErrors.push( "שורה " + counterLine +"- "+ "חובה סוג מכולה");
                }
                counterLine++;
            });
        }
    }

    // Init data from MOREDATA page:
    InitMoreDataScreenValues() {
        if (AppTool.IsNullOrEmpty(this.EntityPM.IsConsigneeForPrint)) {
            this.EntityPM.IsConsigneeForPrint = true;
        }
        if (AppTool.IsNullOrEmpty(this.EntityPM.IsDeclaredByManufacture)) {
            this.EntityPM.IsDeclaredByManufacture = true;
        }
        
        if (AppTool.IsNullOrEmpty(this.EntityPM.CityOfDeclaration)) {
            this.certificateOfOriginWebService.GetCityOfDeclarationByImporterID(this.DecalarationData.ImporterId, this.EntityPM.Tenant).subscribe(myResult => {
                var myResponse: ServiceResponse = myResult;
                if (!myResult.HasError && myResult.Result) {
                    this.EntityPM.CityOfDeclaration = myResponse.Result;
                }
            });
        }
    }

    SaveAndSendClick(customSendOptionsArgs: any = null) {
        if (!this.EntityPM.CooTypeCode || !this.EntityPM.RequestReasonCode) {// manddatory fields
            this.GENERAL.CheckMandatoryFields();
            return;
        }
        this.CheckItenDecriptionData();
        this.EntityPM.IsUnitedInvoices ? this.EntityPM.IsUnitedInvoices : this.EntityPM.IsUnitedInvoices = false;

        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
               
        if (this.IsNewOrEdit == StatusCertificateOfOrigin.IsNew) {
            // init open date:
            this.EntityPM.OpenDate = new Date();
            
            this.certificateOfOriginPMService.insert(this.EntityPM).subscribe((response: any) => {
                if (!response.HasError) {
                    var result = response.Result;
                    this.EntityPM = result;
                    this.GENERAL.InitilizeListsFromCertificateOfOrigin(this.EntityPM);
                    this.CurrentSession.CurrentEditComponent.SaveChanges();
                    this.CurrentSession.StopBusyIndicator();
                    this.IsNewOrEdit = StatusCertificateOfOrigin.IsEdit;
                    
                    this.Send(customSendOptionsArgs);
                }
                else {
                    this.CurrentSession.StopBusyIndicator();
                }
            });
        }

        else if (this.IsNewOrEdit == StatusCertificateOfOrigin.IsEdit) {

            if(!this.EntityPM.IsDirty){
                this.CurrentSession.StopBusyIndicator();
                this.Send(customSendOptionsArgs);
                return;
            }
            // this.certificateOfOriginPMService.update(this.EntityPM).subscribe((response: any) => {
            this.certificateOfOriginWebService.update(this.EntityPM).subscribe((response: any) => {
                if (!response.HasError) {
                    var result = response.Result;
                    this.EntityPM = result;
                    this.GENERAL.InitilizeListsFromCertificateOfOrigin(this.EntityPM);
                    this.CurrentSession.CurrentEditComponent.SaveChanges();
                    this.CurrentSession.StopBusyIndicator();
                    this.UpdateIsChange(true);//#103474
                    this.Send(customSendOptionsArgs);
                }
                else {
                    this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    }
    
    Send(customSendOptionsArgs = null){
        if (customSendOptionsArgs) {
            this.SendButtonClicked(customSendOptionsArgs);
        }
    }

    CheckItenDecriptionData(){
        this.EntityPM.CertificateOriginInvoiceItems.forEach(invoice =>{
            if(AppTool.IsNullOrEmpty(invoice.DescriptionOfInvoice)){
                invoice.DescriptionOfInvoice = !AppTool.IsNullOrEmpty(this.DecalarationData.Consignments[0].CargoDescription) ? this.DecalarationData.Consignments[0].CargoDescription : ""; 
            }
        })
    }

    ValidationErrors = [];
    GeneralValidationErrors = [];
    MoreDataValidationErrors = [];

    async SendButtonClicked(customSendOptionsArgs: any) {
        
        // init lists:
        this.ValidationErrors = [];
        this.GeneralValidationErrors = [];
        this.MoreDataValidationErrors = [];

        this.checkRequestReasonCode();
        
        if (this.SelectedTabCode == "GENERAL"){
            this.GENERAL.CheckMandatoryCustomsFields(this.GeneralValidationErrors);
        }
        else if(this.SelectedTabCode == "MOREDATA"){
            this.MOREDATA.CheckMandatoryCustomsFields(this.MoreDataValidationErrors);
        }
        // var generalScreen = "כללי";
        // var moreDataScreen = "נוספים";
        // var bothDataScreen = "כללי ונוספים";
        this.GeneralValidationErrors.forEach(i => {
            const isUniqueElement = !this.MoreDataValidationErrors.includes(i);
            if (isUniqueElement && i != "" ) {
                this.ValidationErrors.push(i);
            }
        });

        this.MoreDataValidationErrors.forEach(j => {
            const isUniqueElement = !this.GeneralValidationErrors.includes(j);
            if (isUniqueElement && j != "") {
                this.ValidationErrors.push(j);
            }
        });

        // check duplicates items: 
        if(this.ValidationErrors.length > 0){
            this.ValidationErrors = Array.from(new Set(this.ValidationErrors));
        }
        // check mandatory fields
        if (this.ValidationErrors.length > 0 ) {  
            this.CheckMandatoryCustomsFields(customSendOptionsArgs, this.ValidationErrors, "");
        }
        else {
            this.SendCertificateOfOrigin(customSendOptionsArgs);
        }
    }

    async SendCertificateOfOrigin(customSendOptionsArgs: any) {
        if (!this.EntityPM.CooTypeCode || !this.EntityPM.RequestReasonCode) {// manddatory fields
            this.GENERAL.CheckMandatoryFields();
            return;
        }

        if (this.EntityPM.RequestReasonCode == '10' && !AppTool.IsNullOrEmpty(this.EntityPM.COONumber)) {
            var message = new MessageWindow();
            message.RTL = true;

            message.Show(TextCodeTranslator.Translate("Customs.CertificateOfOrigin.O.NotSendwithNum"));
            return;
        }

        var requestParams = new CertificateOfOriginRequestRequestParams();
        requestParams.LoggingEnabled = true;
        requestParams.LoggingUserId = SessionLocator.LoggedUserId;
        requestParams.LoggingObjectTableId = window.ObjectTables.filter(d => d.Name === 'Customs.CertificateOfOrigin')[0].Id;
        requestParams.LoggingEntityId = this.EntityPM.Id;
        requestParams.RequestVIA = customSendOptionsArgs.SendRequestVIA;
        requestParams.Tenant = SessionLocator.Tenant;
        requestParams.CertificateOfOriginId = this.EntityPM.Id;
        requestParams.DeclarationId = this.DecalarationData.Id;
        requestParams.CustomFileNo = this.DecalarationData.CustomFileNo;
        requestParams.RequestReasonCode = Number(this.EntityPM.RequestReasonCode);

        CustomMessageProgressComponent
            .ShowProgressBar(this.CurrentSession, requestParams.PBId,
                "שליחת בקשה לתעודת מקור", true)
            .then((res) => {

                this.ResponseData = res;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {

                this.ValidationErrorsList.push(err);
            });


        this.certificateOfOriginWebService.PostCertificateOfOriginRequest(requestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
                if (myServiceResponse?.Result && !myServiceResponse?.Result.HasException) {
                    var message = new MessageWindow();
                    message.ShowSuccessIcon = true;
                    message.RTL = true;
                    message.Show(myServiceResponse?.Result.UserMessage);
                }

            });
    }

    CheckMandatoryCustomsFields(customSendOptionsArgs: any, ValidationErrors: any[], screenName: string) {
        var windowArgs: any = {};
        // windowArgs.Errors = ValidationErrors;
        windowArgs.Warning = ValidationErrors;
        windowArgs.NoButtonVisibility = false;
        windowArgs.CancelButtonVisibility = true;
        windowArgs.SaveButtonText = "אשר";
        windowArgs.CancelButtonText = "בטל";
        windowArgs.ComponentHeight = '328px';
        // TODO: replace to TTextCodeTranslator.Translate()
        var windowTitle = "שגיאה במילוי שדות חובה במסך " + screenName;
        var logWindow = new LogitudeWindow(this.CurrentSession);
        logWindow.Width = 600;
        logWindow.Height = 400;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => {
            if (this.TaxationWindowClosed($event))
                return this.SendCertificateOfOrigin(customSendOptionsArgs);
            else
                return false;
        });

        logWindow.Show('./CustomsModules/CustomsControls/Components/CustomsErrorsComponent');
        this.CurrentSession.StopBusyIndicator();
    }

    TaxationWindowClosed(event) {
        this.ValidationErrors = [];
        this.GeneralValidationErrors = [];
        this.MoreDataValidationErrors = [];

        switch (event) {
            case "ok": {
                return true;
            }
            case "cancel": {
                return false;
            }
        }
    }

    OnMassageDisplayMethod() {
        if (this.RequestParams == null) {
            this.RequestParams = new CertificateOfOriginRequestRequestParams();
        }
        this.RefreshScreen();
    }

    RefreshScreen() {
        if (this.ResponseData == null) {
            return;
        }
        this.certificateOfOriginWebService.GetCertificateOfOriginByIDIncludeChildrens(this.ResponseData.ApplicationID, this.DecalarationData.Id, this.EntityPM.Tenant).subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;
            if (!myResult.HasError && myResult.Result) {
                this.EntityPM = myResult.Result;
                this.GENERAL.updateEntity(myResult.Result);
                this.CertificateChanges.next(true);               

                if (this.EntityPM?.ErrXml && !AppTool.IsNullOrEmpty(this.EntityPM.ErrXml)) {
                    this.selectedTabCode = "ANSWERTOCERTIFICATE"
                    this.SelectionChanged();
                    if (!this.EntityPM.IsChange)
                        this.UpdateIsChange(true);//#103474
                }
                else
                    this.UpdateIsChange(false);//#103474
            }
        });

    }

    UpdateIsChange(isChange: boolean) {//#103474
        this.EntityPM.IsChange = isChange;

        this.certificateOfOriginWebService.update(this.EntityPM).subscribe((response: any) => {
            if (!response.HasError) {
                var result = response.Result;
                this.EntityPM = result;
                this.GENERAL.InitilizeListsFromCertificateOfOrigin(this.EntityPM);
                this.CurrentSession.CurrentEditComponent.SaveChanges();
                this.CurrentSession.StopBusyIndicator()
            }
            else
                this.CurrentSession.StopBusyIndicator();
        });
    }
    CancelButtonClicked() {
        // this.EntityPM.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }
}

class TabItem {
    public code: string;
    public textCode: string;
    constructor(Code: string, TextCode: string) {
        this.code = Code;

        this.textCode = TextCode;
    }
}

