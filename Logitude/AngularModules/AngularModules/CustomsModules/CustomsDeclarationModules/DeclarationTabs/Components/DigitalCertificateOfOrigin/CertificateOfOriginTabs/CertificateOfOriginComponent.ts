import { Component, ViewChildren, QueryList, SimpleChanges, Input } from '@angular/core';
import { EntityArgs } from '../../../../../../Infrastructure/DataContracts/EntityArgs';
import { OriginCriterionListService } from 'Customs/Services/StandardLists/OriginCriterionListService';
declare var window: any;
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { INF_MSG_GenericResponseData } from 'Customs/DataContract/ResponseData/INF_MSG_GenericResponseData';
import { CustomMessageProgressComponent } from 'CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { AppTool } from 'Infrastructure/Tools';
import { LocationDirective } from 'Infrastructure/Utilities/LocationDirective';
import { CertificateOfOriginPMService } from 'Customs/Services/StandardPMs/CertificateOfOriginPMService';
import { DeclarationPM } from 'Customs/EntityPMs/DeclarationPM';
import { StatusCertificateOfOrigin } from '../DigitalCertificateOfOriginTabComponent';
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { SendRequestVIA } from 'Customs/DataContract/RequestParams/RequestParamsBase';
import { CertificateOfOriginWebService } from 'Customs/Services/WebServices/CertificateOfOriginWebService';
import { CertificateOfOriginRequestRequestParams } from 'Customs/DataContract/RequestParams/CertificateOfOriginRequestRequestParams';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from 'CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CertificateOfOriginListService } from 'Customs/Services/StandardLists/CertificateOfOriginListService';
import { CertificateOfOriginStatusCodeEnumListService } from 'Customs/Services/StandardLists/CertificateOfOriginStatusCodeEnumListService';
import { CertificateOfOriginConnectionListService } from 'Customs/Services/StandardLists/CertificateOfOriginConnectionListService';
import { CertificateOfOriginConnectionList } from 'Customs/EntityLists/CertificateOfOriginConnectionList';
import { BehaviorSubject, Observable } from 'rxjs';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { tap } from 'rxjs/operators';


@Component({

    templateUrl: './CertificateOfOriginComponent.html',
    providers: [EntityArgs],
})


export class CertificateOfOriginComponent extends BaseRequestsSheetMassaging {
    public right: any;
    public TabsItemsSource: TabItem[] = [];
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    certificateOfOriginPMService: CertificateOfOriginPMService = new CertificateOfOriginPMService();
    certificateOfOriginWebService: CertificateOfOriginWebService = new CertificateOfOriginWebService();
    certificateOfOriginListService: CertificateOfOriginListService = new CertificateOfOriginListService();
    certificateOfOriginStatusCodeEnumListService: CertificateOfOriginStatusCodeEnumListService = new CertificateOfOriginStatusCodeEnumListService();
    certificateOfOriginConnectionListService: CertificateOfOriginConnectionListService = new CertificateOfOriginConnectionListService();


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
        this.getCertificateOfOriginStatusCodeEnum();
        this.getCertificateOfOriginConnection();

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
        return this.isDispalyOnlyStatusList.includes(Number(this.EntityPM?.CooStatusCode)) || !this.isAllowChange;
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
                                    this.GENERAL.InitTab(this.EntityPM, this.DecalarationData, this.IsNewOrEdit, this.IsDisplayOnly, this.IsDisplayOnlyByCooConnection);
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
        if (this.EntityPM.RequestReasonCode != "10" && this.EntityPM.RequestReasonCode != "13" && this.EntityPM.RequestReasonCode != "14") {
            let counterLine = 1;
            this.EntityPM.CertificateOriginItemItems.forEach(item => {
                if (AppTool.IsNullOrEmpty(item.MarksAndNumbers)) {
                    this.ValidationErrors.push(`${TextCodeTranslator.Translate("Customs.CertificateOfOrigin.O.number")} ${counterLine}- ${TextCodeTranslator.Translate("Customs.CertificateOfOrigin.O.MarkIsReq")}`);
                }
                if ((item.PackingTypeName == "CONTAINER" || item.PackageType == "D5") && AppTool.IsNullOrEmpty(item.ContainerIsoCode)) {
                    this.ValidationErrors.push(`${TextCodeTranslator.Translate("Customs.CertificateOfOrigin.O.number")} ${counterLine}- ${TextCodeTranslator.Translate("Customs.CertificateOfOrigin.O.ContainerTypeReq")}`);
                }
                counterLine++;
            });

            counterLine = 1;
            this.EntityPM.CertificateOriginInvoiceItems.forEach(item => {
                if (AppTool.IsNullOrEmpty(item.DescriptionOfInvoice)) {
                    this.ValidationErrors.push(`${TextCodeTranslator.Translate("Customs.CertificateOfOrigin.O.number")} ${counterLine}- ${TextCodeTranslator.Translate("Customs.CertificateOfOrigin.O.DescIsReq")}`);
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
        // if (AppTool.IsNullOrEmpty(this.EntityPM.IsDeclaredByManufacture)) {
        //     this.EntityPM.IsDeclaredByManufacture = true;
        // }

        if (AppTool.IsNullOrEmpty(this.EntityPM.CityOfDeclaration)) {
            this.certificateOfOriginWebService.GetCityOfDeclarationByImporterID(this.DecalarationData.ImporterId, this.EntityPM.Tenant).subscribe(myResult => {
                var myResponse: ServiceResponse = myResult;
                if (!myResult.HasError && myResult.Result != null) {
                    this.EntityPM.CityOfDeclaration = myResponse.Result.LocalCityCode;
                }
            });
        }
    }

    checkDeclarationStatusBeforeSend() {
        const warningStatuses: string[] = ["8", "20", "36", "37"];
        if (this.EntityPM.RequestReasonCode == "1" && warningStatuses.includes(this.DecalarationData?.DeclarationStatusTypeCode)){
            this.ValidationErrors.push(TextCodeTranslator.Translate("Customs.CertificateOfOrigin.O.AttentionStatusCert"));
        }
    }

    SaveAndSendClick(customSendOptionsArgs: any = null) {
        if ((!this.EntityPM.CooTypeCode || !this.EntityPM.RequestReasonCode) && ((this.EntityPM.RequestReasonCode != "10" && this.EntityPM.RequestReasonCode != "13" && this.EntityPM.RequestReasonCode != "14"))) {// manddatory fields
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
            const isDirtyInvoiceItems = this.hasDirtyInvoiceItems(this.EntityPM.CertificateOriginInvoiceItems);

            if (!this.EntityPM.IsDirty && !isDirtyInvoiceItems) {
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

    hasDirtyInvoiceItems(items: { IsDirty: boolean }[]): boolean {
        return items.some(item => item.IsDirty);
    }

    Send(customSendOptionsArgs = null) {
        if (customSendOptionsArgs) {
            this.SendButtonClicked(customSendOptionsArgs);
        }
    }

    CheckItenDecriptionData() {
        this.EntityPM.CertificateOriginInvoiceItems.forEach(invoice => {
            if (AppTool.IsNullOrEmpty(invoice.DescriptionOfInvoice)) {
                invoice.DescriptionOfInvoice = !AppTool.IsNullOrEmpty(this.DecalarationData.Consignments[0].CargoDescription) ? this.DecalarationData.Consignments[0].CargoDescription : "";
                invoice.DescriptionOfInvoice = AppTool.IsNullOrEmpty(invoice.DescriptionOfInvoice) ? invoice.InvoiceNumber : "";  // if not exist CargoDescription- use InvoiceNumber
            }
        })
    }

    ValidationErrors = [];
    GeneralValidationErrors = [];
    MoreDataValidationErrors = [];

    originCriterionListService: OriginCriterionListService = new OriginCriterionListService();
    private _length;

    getOriginCriterionCodesByCooTypeCode(): Observable<any> {

        let filters: ApiQueryFilters = new ApiQueryFilters();
        filters.GetAll = true;
        filters.addAdditionalFilter("CertificateOfOriginTypeCodeID", this.EntityPM.CooTypeCode, null, null, "Equals", false, false, false, "number");
        filters.addAdditionalFilter("Inactive", false, null, null, "Equals", true, false, false, "boolean");
        return this.originCriterionListService.getByFilters(filters).pipe(tap((response: any) => {
            if (!response.HasError) {
                this._length = response.Result?.length
            }
        })
        )
    }


    async SendButtonClicked(customSendOptionsArgs: any) {
        this.ValidationErrors = [];
        this.GeneralValidationErrors = [];
        this.MoreDataValidationErrors = [];
        this._length = 0;
        this.checkDeclarationStatusBeforeSend();

        this.getOriginCriterionCodesByCooTypeCode().toPromise().then(() => {

            if (this._length > 0 && this.EntityPM?.CertificateOriginItemItems.some(x => AppTool.IsNullOrUndefined(x?.OriginCriterionCode) === true)) {
                this.ValidationErrors.push(TextCodeTranslator.Translate("Customs.CertificateOfOrigin.O.OriginCriterionCodeRequired"));
            }
        }).then(() => {
            if (AppTool.IsNullOrEmpty(this.DecalarationData.DeclarationNumber) && this.DecalarationData.IsAmendment !== true) {
                this.ValidationErrors.push(TextCodeTranslator.Translate("Customs.CertificateOfOrigin.O.NotDeclaration"));
            }
        }).then(() => {
            if (this.EntityPM.RequestReasonCode != "10" && this.EntityPM.RequestReasonCode != "13" && this.EntityPM.RequestReasonCode != "14") {
                this.checkRequestReasonCode();
                if (this.SelectedTabCode == "GENERAL") {
                    this.GeneralValidationErrors = this.GENERAL.CheckMandatoryCustomsFields(this.GeneralValidationErrors);

                    this.ValidationErrors = this.ValidationErrors.concat(this.GeneralValidationErrors);
                }
                else if (this.SelectedTabCode == "MOREDATA") {
                    this.GeneralValidationErrors = this.GENERAL.CheckMandatoryCustomsFields(this.GeneralValidationErrors);
                    this.MOREDATA.CheckMandatoryCustomsFields(this.MoreDataValidationErrors);

                    this.ValidationErrors = this.ValidationErrors.concat(this.GeneralValidationErrors)
                        .concat(this.MoreDataValidationErrors);
                }

                // check duplicates items: 
                if (this.ValidationErrors.length > 0) {
                    this.ValidationErrors = Array.from(new Set(this.ValidationErrors));
                }
                // check mandatory fields
                if (this.ValidationErrors.length > 0) {
                    this.CheckMandatoryCustomsFields(customSendOptionsArgs, this.ValidationErrors, "");
                }
                else {
                    this.SendCertificateOfOrigin(customSendOptionsArgs);
                }
            } else {
                this.SendCertificateOfOrigin(customSendOptionsArgs);
            }
        }
        );


        // var generalScreen = "כללי";
        // var moreDataScreen = "נוספים";
        // var bothDataScreen = "כללי ונוספים";

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

        //requestParams.RequestVIA = customSendOptionsArgs.SendRequestVIA;
        requestParams.RequestVIA = customSendOptionsArgs.RequestVIA == 0 ? SendRequestVIA.WebServiceInteractive : customSendOptionsArgs.RequestVIA; // #112254
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
                this.getCertificateOfOriginStatusCodeEnum();
                this.getCertificateOfOriginConnection();
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

    IsDisplayOnlyByRecordEditable: boolean = false;
    getCertificateOfOriginStatusCodeEnum() {
        this.certificateOfOriginStatusCodeEnumListService.getSingle(this.EntityPM?.CooStatusCode).subscribe((response: any) => {
            if (!response.HasError) {
                this.IsDisplayOnlyByRecordEditable = response?.Result?.RecordEditable;
                this.updateDisplayByStatusAndConnection();
            }
        });
    }
    IsDisplayOnlyByCooConnection: boolean = false;
    getCertificateOfOriginConnection() {
        let filters: ApiQueryFilters = new ApiQueryFilters();
        filters.GetAll = true;
        filters.addAdditionalFilter("CooStatus", this.EntityPM.CooStatusCode, null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("Active", true, null, null, "Equals", true, false, false, "boolean");
        this.certificateOfOriginConnectionListService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var result: CertificateOfOriginConnectionList[] = myResponse.Result;
                if (result != null) {
                    this.IsDisplayOnlyByCooConnection = result.length > 0 ? false : true;
                    this.updateDisplayByStatusAndConnection();
                    if (result.filter(d => d.CooReason == this.EntityPM.RequestReasonCode)?.length == 0) this.EntityPM.RequestReasonCode = "";
                }
            }
        });
    }

    updateDisplayByStatusAndConnection() {
        this.GENERAL?.updateIsDisplay(this.IsDisplayOnlyByCooConnection, this.IsDisplayOnlyByRecordEditable);
        this.MOREDATA?.updateIsDisplay(this.IsDisplayOnlyByRecordEditable);
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
