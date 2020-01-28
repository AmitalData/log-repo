declare var window: any;
import {Component, AfterViewInit, ChangeDetectorRef}  from '@angular/core';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {AppTool, ArrayTool} from '../../../../../Infrastructure/Tools';
import {FeatureLocator} from '../../../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ObservableCollection} from '../../../../../Infrastructure/Utilities/ObservableCollection';
import {ConfirmWindow} from '../../../../../Controls/Windows/ConfirmWindow';
import {MessageWindow} from '../../../../../Controls/Windows/MessageWindow';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {DeclarationDisplayOnlyChecks, DisplayOnlyCheckResult} from '../../../../../Customs/Utilities/DeclarationDisplayOnlyChecks';

import {DeclarationPM} from '../../../../../Customs/EntityPMs/DeclarationPM';
import {ConsignmentPM} from '../../../../../Customs/EntityPMs/ConsignmentPM';
import {SupplierInvoicePM} from '../../../../../Customs/EntityPMs/SupplierInvoicePM';
import {DeclarationErrorView} from '../../../../../Customs/EntityPMs/Extended/DeclarationErrorView';
import {DeclarationConstraintPM} from '../../../../../Customs/EntityPMs/DeclarationConstraintPM';
import {DeclarationEventManager} from '../../../../../Customs/Utilities/DeclarationEventManager';

import {DeclarationWebService} from '../../../../../Customs/Services/WebServices/DeclarationWebService';
import {DeclarationPMService} from '../../../../../Customs/Services/StandardPMs/DeclarationPMService';
import {ConstraintApprovalRequestParams} from '../../../../../Customs/DataContract/RequestParams/ConstraintApprovalRequestParams';

// Send Request
import {INF_MSG_GenericResponseData} from '../../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData';
import {VendorCommunicationResult} from '../../../../../Customs/DataContract/ResponseData/VendorCommunicationResult';
import {VendorInsertUpdateDeleteMessageRequestParams, OperationTypes} from '../../../../../Customs/DataContract/RequestParams/VendorInsertUpdateDeleteMessageRequestParams';
import { CustomMessageProgressComponent } from '../../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import {DeclarationMessagesService} from '../../../../../Customs/Services/WebServices/DeclarationMessagesService';
import {SendRequestVIA} from '../../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
import { DeclarationEditComponentController } from '../../../../../Customs/Controller/DeclarationEditComponentController';
import { CustomsSettingExtendedListService } from '../../../../../Customs/Services/ExtendedLists/CustomsSettingExtendedListService';
import { DeclarationExtendedListService } from '../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
@Component({
    moduleId: module.id,
    templateUrl: './CustomsAnswersComponent.html',
    providers: [DeclarationExtendedListService]

})

export class CustomsAnswersComponent extends BaseComponent implements AfterViewInit {
    public EntityPM: DeclarationPM;
    public ObjectTableName: string = "Customs.Declaration";
    public DataContext: any = this;
    public CurrentEditComponentId: string;
    public IsDisplayOnly: boolean = false;
    public ShowStorageStatusMessage: boolean = false;
    public DisplayOnlyMessage: string = "";
    public IsDescriptionVisible: boolean = false;
    ResponseData: INF_MSG_GenericResponseData;

    Errorslist: ObservableCollection = new ObservableCollection([]);
    Warninglist: ObservableCollection = new ObservableCollection([]);
    ConstraintsList: any[] = [];

    ErrorsCount: string = "";
    WarningsCount: string = "";
    ConstraintsCount: string = "";
    AllCount: string = "";
    SystemMessageDescribtion: string;
    IsCourierDeclaration: boolean = false;
    IsDisplayMessage: boolean;
    
    public get DepositionStatusCode(): string {
        if (this.EntityPM == null) return null; 
        return this.EntityPM.DepositionStatusCode;
    }
    
    DepositionStatusCodeIcon: string = "";
    IsDepositionStatusCodeButton: boolean = false;
    IsDepositionStatusCodeSendDigital: boolean = false;
    DepositionStatusCodeText: string = "";

    //Services
    private declarationWebService: DeclarationWebService = new DeclarationWebService;
    private declarationMessagesService: DeclarationMessagesService = new DeclarationMessagesService;
    private declarationPMService: DeclarationPMService = new DeclarationPMService;

    //#region TextCodes translations
    textcode_CollateralRequest: string           ;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
    textcode_RequestedAmount: string             ;
    textcode_CollateralRequestStatusCode: string ;
    textcode_PaymentNumber: string               ;
    textcode_AgentObjection: string              ;
    textcode_DenialReason: string                ;
    textcode_ApprovalReason: string;


    //#endregion
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, public cd: ChangeDetectorRef, private EntityResourceService: EntityResourceService, public declarationExtendedListService: DeclarationExtendedListService) {
        super();

        this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.DeclarationConstraint").subscribe(response => {
                this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsCollateral").subscribe(response => {
                    this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsCollateralsCondition").subscribe(response => {
                        this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe(response => {
                            this.EntityPM = this.entityArgs.EntityPM;
                            this.IsCourierDeclaration = this.EntityPM.IsCourierDeclaration;
                            //this.DepositionStatusCode = this.EntityPM.DepositionStatusCode;
                            this.ObjectTableName = this.entityArgs.ObjectTableName;
                            this.Listen();
                            this.ReloadDeclarationErrors();

                            console.log("Declaration", this.EntityPM);

                            this.DisplayOnlyCheck();

                            //#region Textcodes loading


                            this.textcode_CollateralRequest = TextCodeTranslator.Translate("Customs.CustomsCollateral.O.CollateralRequest");
                            this.textcode_RequestedAmount = TextCodeTranslator.Translate("Customs.CustomsCollateral.O.RequestedAmount");
                            this.textcode_CollateralRequestStatusCode = TextCodeTranslator.Translate("Customs.CustomsCollateral.O.CollateralRequestStatusCode");
                            this.textcode_PaymentNumber = TextCodeTranslator.Translate("Customs.PaymentOrder.F.PaymentNumber");
                            this.textcode_AgentObjection = TextCodeTranslator.Translate("Customs.CustomsCollateral.O.AgentObjection");
                            this.textcode_DenialReason = TextCodeTranslator.Translate("Customs.CustomsCollateral.O.DenialReason");
                            this.textcode_ApprovalReason = TextCodeTranslator.Translate("Customs.CustomsCollateral.O.ApprovalReason");

                            this.GetDepositionDefaults(this.EntityPM.CustomerCode);
                            //this.DepositionStatusCodeIcon = "./Images/Buttons/EditWithGreenTick.png";
                            //#enderegion
                        });
                    });
                });
            });
        });

        //Disable fields
        if (this.IsDisplayOnly) {
            this.SetScreenFieldsEditability();
        }

        

    }

    ngAfterViewInit() {
        this.SetFilter();
    }

    SetFilter() {
        var myDeclarationEditComponentController = this.CurrentSession.CurrentEditComponent.EditComponentController as DeclarationEditComponentController;
        if (myDeclarationEditComponentController.CustomsAnswersShowManifest) {
            this.CourierFilterSelectedValue = 'Manifest';
            this.IsManifest = true;
            this.IsConstraintsVisible = false;
            console.log("CustomsAnswersShowManifest");
            //myDeclarationEditComponentController.CustomsAnswersShowManifest = false;
        }
        else {
            this.CourierFilterSelectedValue = 'Declaration';
            this.IsManifest = false;
            this.IsConstraintsVisible = true;
        }
    }

    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                })
            );
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.SetFilter();
                        this.ReloadDeclarationErrors();
                        this.DisplayOnlyCheck();
                    }
                })
            );
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "DCCA") {
                            this.SetFilter();
                            this.ReloadDeclarationErrors();
                            this.DisplayOnlyCheck();
                        }
                    }
                })
            );
        }
    }

    SetScreenFieldsEditability() {
        //this.UIProperties.SetEnabled("DeclarationOfficeCode", this.ObjectTableName, !this.IsDisplayOnly);

    }

    //#region Properties

    private description: string;
    public get Description() { return this.description }
    public set Description(newValue: string) {
        this.description = newValue;
    }

    private listVersionId: string;
    public get ListVersionId() { return this.listVersionId }
    public set ListVersionId(newValue: string) {
        this.listVersionId = newValue;
        this.RefreshEntity();
    }


    //#endregion

    RefreshEntity() {
        this.CurrentSession.CurrentEditComponent.EditComponentController.ResetMustRefresh();
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    }

    DisplayOnlyCheck() {
        this.IsDisplayOnly = this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayMode;
        if (this.IsDisplayOnly) {
            this.DisplayOnlyMessage = "לתצוגה בלבד - " + this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayModeMessage;
            this.SetScreenFieldsEditability();
            DeclarationEventManager.DisplayModeChanged.emit(this.IsDisplayOnly);
            return;
        }
        else if (this.EntityPM.StorageStatusCode) {
            this.ShowStorageStatusMessage = true;
            this.DisplayOnlyMessage = "בקשת אחסנה הועברה למחסן - סטטוס הבקשה" + " " + this.EntityPM.StorageStatusName;
        }
        else {
            this.InitDisplayOnlyMessage();
        }
        var declarationDisplayOnlyChecks: DeclarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks();
        declarationDisplayOnlyChecks.DeclarationViewDisplayOnlyChecks(this.EntityPM).subscribe((response: ServiceResponse) => {
            var displayOnlyCheckResult: DisplayOnlyCheckResult = response.Result;
            this.IsDisplayOnly = displayOnlyCheckResult.IsDisplayOnly;
            if (this.IsDisplayOnly) {
                this.DisplayOnlyMessage = "לתצוגה בלבד - " + displayOnlyCheckResult.DisplayOnlyMessage;
            }
            else if (this.EntityPM.StorageStatusCode) {
                this.ShowStorageStatusMessage = true;
                this.DisplayOnlyMessage = "בקשת אחסנה הועברה למחסן - סטטוס הבקשה" + " " + this.EntityPM.StorageStatusName;
            }
            else {
                this.InitDisplayOnlyMessage();
            }
            this.SetScreenFieldsEditability();
            DeclarationEventManager.DisplayModeChanged.emit(this.IsDisplayOnly);
        });
    }


    InitDisplayOnlyMessage() {
        if (this.EntityPM.IsAmendment && (this.EntityPM.AmendmentStatus == "2")) {
            this.DisplayOnlyMessage = TextCodeTranslator.Translate("Customs.Declaration.O.IsAmendment") + ' ' + this.EntityPM.AmendmentStatusName;
            this.IsDisplayMessage = true;
        }
        else if (this.EntityPM.IsAmendment == false) {
            this.declarationExtendedListService.GetDeclarationAmendmentsById(this.EntityPM.Id).subscribe
                (data => {
                    if (data.Result == null || data.Result.length <= 0) return;

                    data.Result = data.Result.sort((obj1, obj2) => {
                        if (obj1.amendmentissueDate > obj2.amendmentissueDate) {
                            return 1;
                        }

                        if (obj1.amendmentissueDate < obj2.amendmentissueDate) {
                            return -1;
                        }

                        return 0;
                    });
                    var temp = false;

                     data.Result.forEach((item) => {
                         if ((temp == false) && (item.AmendmentStatus == "3" || item.AmendmentStatus == "1" || item.AmendmentStatus == "6" || item.AmendmentStatus == "4" || item.AmendmentStatus == "2")) {
                            {
                                this.DisplayOnlyMessage = TextCodeTranslator.Translate("Customs.Declaration.O.ExistsAmendments") + ' ' + item.AmendmentStatusName;
                                this.IsDisplayMessage = true;
                                 temp = true;

                            }
                        }

                    });
                    //this.DisplayOnlyMessage = TextCodeTranslator.Translate("Customs.Declaration.O.ExistsAmendments") + ' ' + data.Result[0].AmendmentStatusName;
                    //this.IsDisplayMessage = true;

                }


                );

        }
    }

    //#region Filter Methods
    IsErrorsVisible: boolean = true;
    IsConstraintsVisible: boolean = true;
    IsWarningsVisible: boolean = true;

    public FilterSelectedValue: string = 'All';
    FilterItemClicked(itemValue: string) {
        if (this.FilterSelectedValue != itemValue) {
            this.FilterSelectedValue = itemValue;
            switch (itemValue) {
                case 'All':
                    {
                        this.IsErrorsVisible = true;
                        if (this.CourierFilterSelectedValue == "Declaration") {
                            this.IsConstraintsVisible = true;
                        }
                        else {
                            this.IsConstraintsVisible = false;
                        }
                        this.IsWarningsVisible = true;
                        break;
                    }
                case 'Errors':
                    {

                        this.IsErrorsVisible = true;
                        this.IsConstraintsVisible = false;
                        this.IsWarningsVisible = false;
                        break;
                    }
                case 'Constraints':
                    {

                        this.IsErrorsVisible = false;
                        if (this.CourierFilterSelectedValue == "Declaration") {
                            this.IsConstraintsVisible = true;
                        }
                        else {
                            this.IsConstraintsVisible = false;
                        }
                        this.IsWarningsVisible = false;
                        break;
                    }
                case 'Warnings':
                    {

                        this.IsErrorsVisible = false;
                        this.IsConstraintsVisible = false;
                        this.IsWarningsVisible = true;
                        break;
                    }
                default:
                    {
                        this.IsErrorsVisible = true;
                        if (this.CourierFilterSelectedValue == "Declaration") {
                            this.IsConstraintsVisible = true;
                        }
                        else {
                            this.IsConstraintsVisible = false;
                        }
                        this.IsWarningsVisible = true;
                        break;
                    }
            }
        }
    }
    //#endregion
    CourierFilterSelectedValue: string = 'Declaration';
    CourierFilterClicked(value: string) {

        if (this.CourierFilterSelectedValue != value) {
            this.CourierFilterSelectedValue = value;
            if (value == 'Manifest') {
                this.IsManifest = true;
                this.IsConstraintsVisible = false;
            }
            else {
                this.IsManifest = false;
                this.IsConstraintsVisible = true;

            }
            this.LoadDeclarationErrors();
        }
    }
    IsManifest: boolean = false;
    IsMoreThan50: boolean = false;
    AllConstraintCount: number = 0;
    // get errors code
    ReloadDeclarationErrors() {
        this.CurrentSession.StartBusyIndicatorLoading();

        //[1] GetDeclarationConstraints();
        this.declarationWebService.GetDeclarationConstraintsByDeclrationId(this.EntityPM.Id)
            .subscribe((myServiceResponse: ServiceResponse) => {
                console.log("[Response] GetDeclarationConstraints : ", myServiceResponse.Result);
                var res: any[] = myServiceResponse.Result;

                if (!AppTool.IsNullOrEmpty(res)) {

                    if (res.length == 0)
                        this.CurrentSession.StopBusyIndicator();
                    else if (res.length > 50) {
                        this.AllConstraintCount = res.length;
                        res = res.slice(0, 50);
                        this.IsMoreThan50 = true;
                    } else {
                        this.IsMoreThan50 = false;
                    }

                    var decConsts = this.EntityPM.DeclarationConstraints;
                    for (let constraint of decConsts) {
                        //this.EntityPM.RemoveDeclarationConstraint(constraint)
                    }

                    var newConsts = res;
                    for (let constraint of newConsts) {
                        //this.EntityPM.AddDeclarationConstraint(constraint);
                    }

                    this.LoadDeclarationErrors();
                } else {
                    this.CurrentSession.StopBusyIndicator();

                }

            });



    }
    LoadConstriantsList(decErrors: any[]) {


        this.ConstraintsList = [];
        var constraints = this.EntityPM.DeclarationConstraints;
        if (constraints.length > 50) {

            //var startIndex = (this.currenctPageIndex - 1) * 50; // 1.. 0*50=0   2.. 1*50=50
            //var endIndex = this.currenctPageIndex * 50;    // 1.. 1*50=50  2.. 2*50=100

            constraints = constraints.slice(this.currenctStartIndex, this.currenctEndIndex);

            //constraints = constraints.slice(0, 50);
            this.IsMoreThan50 = true;
        } else {
            this.IsMoreThan50 = false;
        }

        //var declarationConstraints = [];
        for (var constraint of constraints) {
            var error = decErrors.find(d => d.ConstraintId == constraint.ConstraintNumber);
            var item = new ConstraintLineModel(error, constraint,this);
            this.ConstraintsList.push(item);
        }

        //
        // Calculate count
        if (this.ConstraintsList.length > 0) {
            this.ConstraintsCount = "(" + this.ConstraintsList.length + ")";
        }
        else {
            this.ConstraintsCount = "";
        }

        var declarationErrors = decErrors;

        //build errors and warning grids
        this.BuildDeclarationErrorsWithConstraintsList(declarationErrors);

        //calculate all counts
        var allListsCount = this.ConstraintsList.length + this.Errorslist.Length + this.Warninglist.Length;
        if (allListsCount > 0) {
            if (this.IsMoreThan50)
                this.AllCount = "(" + (this.AllConstraintCount + this.Errorslist.Length + this.Warninglist.Length) + ")";
            else
                this.AllCount = "(" + allListsCount + ")";
        }
        else {
            this.AllCount = "";
        }


    }
    errorsForDeclaration;
    LoadDeclarationErrors() {
        //[2] GetDeclarationErrors();
        this.CurrentSession.StartBusyIndicatorLoading();//Avoiding ReSend !!
        this.declarationWebService.GetDeclarationErrors(this.EntityPM.Id, this.ListVersionId, this.CourierFilterSelectedValue)
            .subscribe((myServiceResponse: ServiceResponse) => {
                console.log("[Response] GetDeclarationErrors : ", myServiceResponse.Result);
                var res: any[] = myServiceResponse.Result;

                this.errorsForDeclaration = res;

                if (res && res.length == 0) {
                    this.CurrentSession.StopBusyIndicator();
                    this.LoadConstriantsList([]);
                }
                this.GetResources(res);
                var newErrors: any[] = [];
                var oldErrors: DeclarationErrorView[] = [];

                if (!AppTool.IsNullOrEmpty(res)) {
                    //oldErrors = this.EntityPM.DeclarationErrorViews;
                    for (let error of oldErrors) {
                        //this.EntityPM.RemoveDeclarationErrorView(error);
                    }

                    newErrors = res;
                    for (let error of newErrors) {
                        //this.EntityPM.AddDeclarationErrorView(error);
                    }


                    // this.LoadConstriantsList(newErrors); //temp

                    if (AppTool.IsNullOrEmpty(this.Description)) {
                        this.IsDescriptionVisible = false;
                    }
                    else {
                        this.IsDescriptionVisible = true;
                    }

                    this.DisplayOnlyCheck();
                } else {
                    this.CurrentSession.StopBusyIndicator();

                }

            });
    }

    BuildDeclarationErrorsWithConstraintsList(declarationErrors: DeclarationErrorView[]) {

        var errorsList = [];
        var warningList = [];

        // reset grids
        this.Errorslist.Clear();
        this.Warninglist.Clear();
        this.Description = null;

        var listversion = null;
        for (var error of declarationErrors) {
            if (error.Sort == 0 || error.Sort == null) {
                ///???? error.OrderBy = 99999999;
            }
            if (error.ListVersionId == "1") {
                // var item = new ErrorModel(error);
                //this.Errorslist.Insert(error);
                errorsList.push(error); 
            }
            else if (error.ListVersionId == "4" || error.ListVersionId == "5") {
                //this.Warninglist.Insert(error);
                warningList.push(error);
            }

            else if (error.ListVersionId == "A") {
                listversion = "A";
                if (!this.Description) this.Description = "";
                this.Description += error.Description + ", ";
                //SystemMessagesVisibility = Visibility.Visible;
            }

            //VeiwModel.ConstraintIndication = error.ConstraintIndication;

        }
        if (this.Description) {
            this.Description = this.Description.replace(/,\s*$/, ""); //remove last comma
        }

        errorsList.sort((a, b) => { return (a.Sort === b.Sort) ? 0 : (a.Sort < b.Sort) ? -1 : 1 });
        warningList.sort((a, b) => { return (a.Sort === b.Sort) ? 0 : (a.Sort < b.Sort) ? -1 : 1 });

        this.Errorslist.InsertCollection(errorsList);
        this.Warninglist.InsertCollection(warningList);

        if (listversion == null) {
            this.Description = null;
            //SystemMessagesVisibility = Visibility.Collapsed;
        }
        if (this.Errorslist.Length > 0) {
            this.ErrorsCount = "(" + this.Errorslist.Length + ")";
        }
        else {
            this.ErrorsCount = "";
        }

        if (this.Warninglist.Length > 0) {
            this.WarningsCount = "(" + this.Warninglist.Length + ")";
        }
        else {
            this.WarningsCount = "";
        }

        this.CurrentSession.StopBusyIndicator();

    }

    isResourcesLoaded: boolean = false;
    errorsLength = 0;
    GetResources(errors: DeclarationErrorView[]) {
        var tables = [];
        if (!AppTool.IsNullOrEmpty(errors)) {
            this.errorsLength = errors.length;
            errors.forEach((el) => {
                if (!AppTool.IsNullOrEmpty(el.FieldNameTextCode)) {
                    var splittedWords = el.FieldNameTextCode.split('.');
                    var objectTableName = splittedWords[0] + "." + splittedWords[1];

                    this.GetResourceForTableName(objectTableName, errors);
                    //if (tables.indexOf(el.TableNameTextCode) < 0) {
                    //    tables.push(el.TableNameTextCode);
                    //}
                } else {
                    this.errorsLength--;
                    console.log("No FieldNameTextCode", el);
                }
                if (this.errorsLength == 0)
                    this.LoadConstriantsList(errors);
            });




        }

    }

    GetResourceForTableName(tableName: string, errors: DeclarationErrorView[]) {
        // var tableName = tables.pop();
        if (tableName == 'Customs.SupplierInvioceItemsCertificate') {
            tableName = 'Customs.SupplierInvioceItemCertificat';
        }
        this.EntityResourceService.getEntityResourceByTableName(tableName).subscribe(response => {
            // this.GetResourceForTableName(tables);
            if (this.errorsLength != 1) {
                this.errorsLength--;
            }
            else {
                this.LoadConstriantsList(errors);
            }
        });
    }

    RequestVIA: SendRequestVIA;
    SendButtonClicked(constraint: ConstraintLineModel, event) {
        this.RequestVIA = event.RequestVIA;

        if (this.EntityPM.IsDirty) {
            this.CurrentSession.CurrentEditComponent.SaveChanges();

            var event = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {

                if (event) {
                    event.unsubscribe();
                }

                if (isSaveSuccess) {

                    if (!AppTool.IsNullOrEmpty(constraint.AgentExplanation)) {
                        this.SendConstraintMethod(constraint);
                    }
                    else {
                        var msg = new MessageWindow();
                        msg.Show(TextCodeTranslator.Translate("Customs.Declaration.O.FillAgentExplanation"));
                    }
                }
            });

        } else {
            if (!AppTool.IsNullOrEmpty(constraint.AgentExplanation)) {
                this.SendConstraintMethod(constraint);
            }
            else {
                var msg = new MessageWindow();
                msg.Show(TextCodeTranslator.Translate("Customs.Declaration.O.FillAgentExplanation"));
            }
        }

    }

    SendConstraintMethod(constraint: ConstraintLineModel) {

        var ObjectTable = window.ObjectTables.filter(x => x.Name === "Customs.Declaration")[0];

        var requestParams = new ConstraintApprovalRequestParams();
        requestParams.LoggingEnabled = true;
        requestParams.LoggingUserId = SessionLocator.LoggedUserId;
        requestParams.ConstraintNumber = constraint.ConstraintNumber;
        requestParams.ConstraintTypeName = constraint.ConstraintTypeName;
        requestParams.ConstraintStatusName = constraint.ConstraintStatus;
        requestParams.ApprovalDecision = constraint.ApprovalDecisionName;
        requestParams.AgentExplanation = constraint.AgentExplanation;
        requestParams.Tenant = SessionLocator.Tenant;
        requestParams.RequestName = "Declaration Constriant";
        requestParams.ResponseName = "Declaration Constriant";
        //requestParams.TestCase = SelectedTest;
        requestParams.DeclarationId = this.EntityPM.Id;
        requestParams.LoggingEntityId = this.EntityPM.Id;
        requestParams.LoggingEntityReference = this.EntityPM.DeclarationNumber;
        requestParams.LoggingObjectTableId = ObjectTable.Id;
        requestParams.RequestVIA = this.RequestVIA;


        CustomMessageProgressComponent.ShowProgressBar(requestParams.PBId, "שליחת בקשה לאישור אילוץ", true).then((res) => {

            this.ResponseData = res;
            console.log("Response/ShowProgressBar : ", this.ResponseData);
            this.OnSendCompleted();

        }).catch((err) => {
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = err;
        });



        this.declarationMessagesService.PostSendDeclarationConstraint(requestParams).subscribe((myServiceResponse: ServiceResponse) => {
        });

    }

    OnSendCompleted() {
        var timerTiken = setTimeout(() => {
            this.RefreshEntity();
        }, 500);
    }

    //#region XML Errors

    EditEntity(declarationError: DeclarationErrorView) {


        if (AppTool.IsNullOrEmpty(declarationError)) {
            console.warn("[!] There is no declaraion error for the constraint!");
        } else {
            this.CurrentSession.StartBusyIndicatorLoading();
            switch (declarationError.EntityName.toLowerCase()) {

                case "declaration":
                case "consignment":
                    {
                        var logWindow = new LogitudeWindow();
                        logWindow.Width = 1000;
                        logWindow.Height = 700;
                        logWindow.ShowCloseButton = true;
                        logWindow.Title = this.GetEditedScreenTitle(declarationError.EntityName, declarationError);
                        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/General/DeclarationGeneralComponent');
                        logWindow.WindowArgs = {
                            DeclarationError: declarationError,
                            entityArgs: this.entityArgs,
                            entityPM: this.EntityPM,
                            IsDisplayOnly: this.IsDisplayOnly,
                        };
                        logWindow.WindowClosed.subscribe(($event: any) => {
                            if ($event != 'cancel') {
                                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                            }
                        });
                        this.CurrentSession.StopBusyIndicator();

                        // Remove DetectChanges from answer tab
                        this.cd.detach();
                        logWindow.WindowClosed.subscribe(() => {
                            this.cd.reattach();
                            var timertoken = setTimeout(() => {
                                this.cd.detectChanges();
                            },100);
                        });

                        break;
                    }

                case "supplierinvoice": {

                    this.declarationWebService
                        .GetSupplierInvoiceBySequenceNumber(declarationError.DeclarationId, +declarationError.LineNumber, 0, 500)
                        .subscribe((response: ServiceResponse) => {
                            console.log("[Response] GetSupplierInvoiceBySequenceNumber: ", response);


                            var supplierInvoicePM = response.Result;


                            if (!AppTool.IsNullOrEmpty(supplierInvoicePM)) {

                                this.CurrentSession.StartBusyIndicatorLoading();
                                var windowArgs: any = {};
                                windowArgs.EntityPM = supplierInvoicePM;
                                windowArgs.declarationPM = this.EntityPM;
                                windowArgs.DeclarationError = declarationError;
                                windowArgs.IsFromCustomsAnswer = true;
                                windowArgs.IsInvoiceAnswer = true;
                                windowArgs.NumberOfLoadedItems = 500;
                                var logWindow = new LogitudeWindow();
                                logWindow.Width = 1030;
                                logWindow.Height = 600;

                                if (!AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                                    logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + supplierInvoicePM.InvoiceNumber + "-" + this.EntityPM.DeclarationNumber;

                                }
                                else if (AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                                    logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + this.EntityPM.DeclarationNumber;

                                }
                                else if (!AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                                    logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + supplierInvoicePM.InvoiceNumber;

                                }
                                else if (AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                                    logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");

                                }
                                windowArgs.IsDisplayOnly = this.IsDisplayOnly;
                                logWindow.ShowCloseButton = false;
                                logWindow.WindowArgs = windowArgs;
                                logWindow.Title = this.GetEditedScreenTitle(declarationError.EntityName, declarationError);
                                logWindow.WindowClosed.subscribe(($event: any) => {
                                    if ($event != 'cancel') {
                                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                                    }
                                });
                              logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/AddEditSupplierInvoiceComponent');
                                this.CurrentSession.StopBusyIndicator();

                                // Remove DetectChanges from answer tab
                                this.cd.detach();
                                logWindow.WindowClosed.subscribe(() => {
                                    this.cd.reattach();
                                    var timertoken = setTimeout(() => {
                                        this.cd.detectChanges();
                                    }, 100);                                });

                            }
                            else {
                                this.CurrentSession.StopBusyIndicator();
                                var window = new MessageWindow();
                                window.Show("There is no invoice with such key in this declaration!!");
                            }
                        });

                    break;
                }

                case "supplierinvoiceitem":
                    {
                        if (AppTool.IsNullOrEmpty(declarationError.LineNumber)) {
                            console.log("No line number", declarationError);
                            return;
                        }
                        var lines = declarationError.LineNumber.split(',');

                        var invSequence = +lines[0];
                        var itemSequence = +lines[1];

                        this.declarationWebService
                            .GetSupplierInvoiceWithItemBySequenceNumber(declarationError.DeclarationId, invSequence, itemSequence, 0, 500)
                            .subscribe((response: ServiceResponse) => {
                                console.log("[Response] GetSupplierInvoiceWithItemBySequenceNumber: ", response);


                                var supplierInvoicePM = response.Result;
                             

                                if (!AppTool.IsNullOrEmpty(supplierInvoicePM)) {
                               


                                    this.CurrentSession.StartBusyIndicatorLoading();
                                    var windowArgs: any = {};
                                    windowArgs.EntityPM = supplierInvoicePM;
                                    windowArgs.declarationPM = this.EntityPM;
                                    windowArgs.DeclarationError = declarationError;
                                    windowArgs.IsFromCustomsAnswer = true;
                                    var logWindow = new LogitudeWindow();
                                    logWindow.Width = 1030;
                                    logWindow.Height = 600;

                                    if (!AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                                        logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + supplierInvoicePM.InvoiceNumber + "-" + this.EntityPM.DeclarationNumber;

                                    }
                                    else if (AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                                        logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + this.EntityPM.DeclarationNumber;

                                    }
                                    else if (!AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                                        logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + supplierInvoicePM.InvoiceNumber;

                                    }
                                    else if (AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                                        logWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");

                                    }
                                    this.CurrentSession.StopBusyIndicator();
                                    windowArgs.IsDisplayOnly = this.IsDisplayOnly;
                                    logWindow.ShowCloseButton = false;
                                    logWindow.WindowArgs = windowArgs;
                                    logWindow.Title = this.GetEditedScreenTitle(declarationError.EntityName, declarationError);
                                    logWindow.WindowClosed.subscribe(($event: any) => {
                                        if ($event != 'cancel') {
                                            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                                        }
                                    });
                                  logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/AddEditSupplierInvoiceComponent');
                                    this.CurrentSession.StopBusyIndicator();

                                    // Remove DetectChanges from answer tab
                                    this.cd.detach();
                                    logWindow.WindowClosed.subscribe(() => {
                                        this.cd.reattach();
                                        var timertoken = setTimeout(() => {
                                            this.cd.detectChanges();
                                        }, 100);                                    });
                                }
                                else {
                                    var window = new MessageWindow();
                                    window.Show("There is no invoice with such key in this declaration!!");
                                }
                            });

                        break;
                    }

                case "supplierinvioceitemscertificate":
                    {
                        if (AppTool.IsNullOrEmpty(declarationError.LineNumber)) {
                            console.log("No line number", declarationError);
                            return;
                        }
                        var lines = declarationError.LineNumber.split(',');

                        var invSequence = +lines[0];
                        var itemSequence = +lines[1];

                        this.declarationWebService
                            .GetSupplierInvoiceWithItemBySequenceNumber(declarationError.DeclarationId, invSequence, itemSequence, 0, 500, "certificate")
                            .subscribe((response: ServiceResponse) => {
                                console.log("[Response] GetSupplierInvoiceWithItemBySequenceNumber: ", response);


                                var supplierInvoicePM: SupplierInvoicePM = response.Result;

                                if (!AppTool.IsNullOrEmpty(supplierInvoicePM)) {

                                    this.CurrentSession.StartBusyIndicatorLoading();

                                    var invoiceItem = supplierInvoicePM.SupplierInvoiceItems.find(d => d.SequenceNumeric == declarationError.ParentLine);

                                    if (!AppTool.IsNullOrEmpty(invoiceItem)) {

                                        // open certificate
                                        var windowArgs: any = {};
                                        windowArgs.SupplierInvoiceItemPM = invoiceItem;
                                        windowArgs.DeclarationError = declarationError;
                                        windowArgs.IsDisplayOnly = this.IsDisplayOnly;
                                        windowArgs.InvoiceNumber = supplierInvoicePM.InvoiceNumber;
                                        windowArgs.SupplierInvoicePM = supplierInvoicePM;
                                        windowArgs.LineNumber = declarationError.LineNumber;

                                        var windowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoiceItem");

                                        var logWindow = new LogitudeWindow();
                                        logWindow.Width = 1000;
                                        logWindow.Height = 600;
                                        //if (supplierInvoicePM.ClassificationCode != null) {
                                        //    logWindow.Title = "אישורים לפרט מכס" + " " + supplierInvoicePM.ClassificationCode;
                                        //}
                                        //else {
                                        //    logWindow.Title = "אישורים לפרט מכס";
                                        //}
                                        logWindow.ShowCloseButton = false;
                                        logWindow.WindowArgs = windowArgs;
                                        logWindow.Title = this.GetEditedScreenTitle(declarationError.EntityName, declarationError);
                                        this.CurrentSession.StopBusyIndicator();
                                        logWindow.WindowClosed.subscribe(($event: any) => {
                                            if ($event != 'cancel') {
                                                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                                            }
                                        });
                                      logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/SupplierInvoiceItem/SupplierInvoiceItemCertificatesComponent');
                                        //end open certificate

                                        // Remove DetectChanges from answer tab
                                        this.cd.detach();
                                        logWindow.WindowClosed.subscribe(() => {
                                            this.cd.reattach();
                                            var timertoken = setTimeout(() => {
                                                this.cd.detectChanges();
                                            }, 100);                                        });
                                    }
                                    this.CurrentSession.StopBusyIndicator();

                                }
                                else {
                                    var window = new MessageWindow();
                                    window.Show("There is no invoice with such key in this declaration!!");
                                }
                            });

                        break;
                    }

                default:
                    {
                        var window = new MessageWindow();
                        window.Show(TextCodeTranslator.Translate("Customs.General.O.WrongEntityName"));
                        break;
                    }
            }
        }
    }
    ShowXMLErrors(error) {
        //if (!AppTool.IsNullOrEmpty(error.Field)) {
        //    this.UIProperties.SetValidity(error.Field, "Customs.SupplierInvoice", false, error.Description);
        //}

        var errors = [];
        if (!AppTool.IsNullOrEmpty(error.Description)) {
            var xmlErrors: any[] = error.Description.split(/,|:/);
            for (var xmlError of xmlErrors) {
                errors.push(xmlError);
            }
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
        }
        if (error.EntityName != null) {
            if (error.EntityName.toLowerCase() == "supplierinvoiceitem") {
                //if (OnShowXMLErrors != null) {
                //    OnShowXMLErrors(new OnShowXMLErrorEvenArgs() { SupplierInvoiceItem = InvoiceItemsObslist.Where(d => d.SequenceNumeric == error.Line).FirstOrDefault(), });
                //}
            }
        }
    }
    GetEditedScreenTitle(entityName: string, declarationError: DeclarationErrorView) {

        var title = "";
        switch (entityName.toLowerCase()) {
            case "declaration":
            case "consignment":
                {
                    title = TextCodeTranslator.Translate("Customs.Declaration");
                    break;
                }
            case "supplierinvoice":
                {
                    var declaration = this.EntityPM;
                    var supplierInvoicePM = declaration.SupplierInvoices.find(d => d.DeclarationId == declaration.Id && d.SequenceNumeric == declarationError.Line);

                    if (!AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !AppTool.IsNullOrEmpty(declaration.DeclarationNumber)) {
                        title = supplierInvoicePM.InvoiceNumber + "-" + declaration.DeclarationNumber + " " + TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");

                    }
                    else if ((AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) || supplierInvoicePM.InvoiceNumber == "") && !AppTool.IsNullOrEmpty(declaration.DeclarationNumber)) {
                        title = declaration.DeclarationNumber + " " + TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");

                    }
                    if (!AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && (AppTool.IsNullOrEmpty(declaration.DeclarationNumber) || declaration.DeclarationNumber == "")) {
                        title = supplierInvoicePM.InvoiceNumber + " " + TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");

                    }
                    if ((AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) || supplierInvoicePM.InvoiceNumber == "") && (AppTool.IsNullOrEmpty(declaration.DeclarationNumber) || declaration.DeclarationNumber == "")) {
                        title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");

                    }
                    break;
                }
            case "supplierinvoiceitem":
                {
                    title = TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoiceItem");
                    break;
                }

            case "supplierinvioceitemscertificate":
                {
                    title = TextCodeTranslator.Translate("Customs.Declaration.O.Certificates");
                    break;
                }
        }
        return title;
    }

    //#endregion

    //#region constraint paging - client
    pageSize: number = 50;
    currenctPageIndex: number = 1;
    currenctStartIndex: number = 0;
    currenctEndIndex: number = this.pageSize;
    
    ConstraintNavigationButtonClicked(dir: string) {

        if (dir == "next") {
            var lastPageNumber = Math.ceil(this.AllConstraintCount / this.pageSize);
            if (lastPageNumber == this.currenctPageIndex) {
                return;
            } else {
                this.currenctPageIndex++;
                this.currenctStartIndex = (this.currenctPageIndex - 1) * this.pageSize; // 1.. 0*50=0   2.. 1*50=50
                this.currenctEndIndex = this.currenctPageIndex * this.pageSize;    // 1.. 1*50=50  2.. 2*50=100

                this.LoadConstriantsList(this.errorsForDeclaration);
            }
        }
        else if (dir == "prev") {
            if (this.currenctPageIndex <= 1)
                return;

            this.currenctPageIndex--;
            this.currenctStartIndex = (this.currenctPageIndex - 1) * this.pageSize; // 1.. 0*50=0   2.. 1*50=50
            this.currenctEndIndex = this.currenctPageIndex * this.pageSize;    // 1.. 1*50=50  2.. 2*50=100

            this.LoadConstriantsList(this.errorsForDeclaration);
        }

    }


    DepositionStatusbuttonclicked(event) {

        //if (!AppTool.IsNullOrEmpty(this.EntityPM.Id) && this.EntityPM.DepositionStatusCode != "L") {
        this.EntityPM.DepositionStatusCode = "L";
        this.SaveEntityChanges(null);
        event.stopPropagation();
        return;
        //}
    }
    SaveEntityChanges(args: any): any {
        SessionLocator.SelectedSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
        if (!AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            this.declarationPMService.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                SessionLocator.SelectedSession.StopBusyIndicator();

                if (myResponse.HasError) {
                    //this.ValidationErrorsList = myResponse.ErrorsArray;
                }

                else {
                    this.EntityPM = myResponse.Result;
                    if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {

                        var myErrors: string[] = [];
                        myErrors.push("this.EntityPM.Id is null");
                        //this.ValidationErrorsList = myErrors;
                    }
                    else {
                        if (args == null) {
                            //this.CancelButtonClicked();
                        } else {

                            //this.SendButtonClicked();
                        }

                    }
                }


            });

        }
    }


    private GetDepositionDefaults(CustomerCode: string) {
        var myCustomsSettingExtendedListService = new CustomsSettingExtendedListService();
        myCustomsSettingExtendedListService.GetDefault("ISRAEL", "CGG_SHARE_DESPO", "NON", "NON", SessionLocator.Tenant)
            .subscribe(response => {
                this.IsDepositionStatusCodeButton = false;
                this.IsDepositionStatusCodeSendDigital = false;
                if (!response.HasError && response.Result != null && response.Result.DefaultValue == "Y") {


                    myCustomsSettingExtendedListService.GetDefault("ISRAEL", "GGG_BOX_ACTIVAT", "NON", CustomerCode, SessionLocator.Tenant)
                        .subscribe(response => {
                            this.IsDepositionStatusCodeButton = false;
                            this.IsDepositionStatusCodeSendDigital = false;
                            //this.DepositionStatusCodeIcon = "./Images/LogBox/DSV/U_LOGBOX.png";
                            this.DepositionStatusCodeIcon = "LOGBOX";
                            if (!response.HasError && response.Result != null && response.Result.DefaultValue == "Y") {
                                this.IsDepositionStatusCodeButton = true;
                            }
                            myCustomsSettingExtendedListService.GetDefault("ISRAEL", "GGG_LBL_ACTIVAT", "NON", CustomerCode, SessionLocator.Tenant)
                                .subscribe(res => {
                                    if (!res.HasError && res.Result != null && res.Result.DefaultValue == "Y") {
                                        this.IsDepositionStatusCodeButton = true;
                                        this.IsDepositionStatusCodeSendDigital = true;
                                    }
                                    if (this.IsDepositionStatusCodeSendDigital) {
                                        this.DepositionStatusCodeText = "נשלחה משימה ליבואן בדיגיטל";
                                        this.DepositionStatusCodeIcon = "DEFAULT";
                                    }
                                    else {
                                        this.DepositionStatusCodeText = "נשלחה משימה ליבואן בלוגבוקס";
                                    }

                                    //myCustomsSettingExtendedListService.GetDefault("ISRAEL", "GGG_PRV_LBL_LOG", "NON", "NON", SessionLocator.Tenant)
                                    //  .subscribe(res => {
                                    //    if (!res.HasError && res.Result != null) {
                                    //this.DepositionStatusCodeIcon = "./Images/LogBox/DSV/Tab_Logo_Original.png";
                                    //      this.DepositionStatusCodeIcon = "DEFAULT";
                                    //}
                                    //});
                                });
                        });
                }
            });
    }
    //#endregion
}

export class ConstraintLineModel extends BaseComponent {

    private declarationWebService: DeclarationWebService = new DeclarationWebService;

    lineHeight: number = 62;
    hasNoError: boolean = false;
    parent: CustomsAnswersComponent;
    displayOnly: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private declarationError: DeclarationErrorView, private constraintPM: DeclarationConstraintPM, Parent: CustomsAnswersComponent) {
        super();
        this.parent = Parent;
        if (AppTool.IsNullOrEmpty(this.declarationError)) {
            this.hasNoError = true;
            this.declarationError = new DeclarationErrorView();
        }
        this.textcode_TableNameTextCode = TextCodeTranslator.Translate(this.declarationError.TableNameTextCode);
        this.UIProperties.SetEnabled("AgentExplanation", "Customs.DeclarationConstraint", !this.parent.IsDisplayOnly);
        this.ManageScreensVisibility();

        this.displayOnly = this.parent.IsDisplayOnly;

        if (this.parent.EntityPM.DeclarationStatusTypeCode == '11'
            && this.constraintPM.ConstraintTypeCode == '1'
            && this.constraintPM.ConstraintStatusCode == '1') {
            this.UIProperties.SetEnabled("AgentExplanation", "Customs.DeclarationConstraint", true);
            this.displayOnly = false;
        }
    }


    // TextCode
    textcode_TableNameTextCode: string;

    // Visibility 
    IsApprovalDecisionHyperlinkVisibile: boolean = false;
    IsStatusHyperlinkVisibile: boolean = false;
    IsCollateralInfoVisibile: boolean = false;
    IsAgentObjectionVisibile: boolean = false;
    IsCollateralAnswersVisibile: boolean = false;
    IsRequestedDocumentVisibile: boolean = false;
    IsPaymentNumberVisibile: boolean = false;

    IsStatus1Visibile: boolean = false;
    IsStatus2Visibile: boolean = false;
    IsStatus3Visibile: boolean = false;
    IsStatus4Visibile: boolean = false;
    IsStatus5Visibile: boolean = false;

    IsStatus4AgentObjectionVisibile: boolean = false;

    Collateral: any;

    //#region Properties 

    get ConstraintId() { return this.declarationError.ConstraintId; }
    set ConstraintId(newValue: string) {
        this.declarationError.ConstraintId = newValue;
    }

    get LineNumber() { return this.declarationError.LineNumber; }
    set LineNumber(newValue: string) {
        this.declarationError.LineNumber = newValue;
    }

    get ErrorMessage() {
        return AppTool.IsNullOrEmpty(this.declarationError) ? "" : this.declarationError.Description;
    }
    set ErrorMessage(newValue: string) {
        this.declarationError.Description = newValue;
    }

    get FieldNameTextCode() {
        return this.declarationError.FieldNameTextCode;
    }
    set FieldNameTextCode(newValue: string) {
        this.declarationError.FieldNameTextCode = newValue;
    }

    get FieldName() {
        var field = "";
        if (this.FieldNameTextCode != null) {
            field = TextCodeTranslator.Translate(this.FieldNameTextCode);
        }
        return field;
    }

    get TableNameTextCode() {
        return this.declarationError.TableNameTextCode;
    }
    set TableNameTextCode(newValue: string) {
        this.declarationError.TableNameTextCode = newValue;
    }

    get CustomsCollateralId() { return this.constraintPM.CustomsCollateralId; }
    set CustomsCollateralId(newValue: string) { this.constraintPM.CustomsCollateralId = newValue; }

    get ConstraintNumber() { return this.constraintPM.ConstraintNumber; }
    set ConstraintNumber(newValue: string) { this.constraintPM.ConstraintNumber = newValue; }

    get ConstraintTypeName() { return this.constraintPM.ConstraintTypeName; }
    set ConstraintTypeName(newValue: string) { this.constraintPM.ConstraintTypeName = newValue; }

    get ConstraintStatus() { return this.constraintPM.ConstraintStatusName; }
    set ConstraintStatus(value: string) { this.constraintPM.ConstraintStatusName = value; }

    get ConstraintStatusCode() { return this.constraintPM.ConstraintStatusCode; }
    set ConstraintStatusCode(newValue: string) { this.constraintPM.ConstraintStatusCode = newValue; }

    get AgentExplanation() { return this.constraintPM.AgentExplanation; }
    set AgentExplanation(newValue: string) {
        this.constraintPM.AgentExplanation = newValue;
    }

    get ApprovalDecisionName() {
        if (this.constraintPM != null) {
            return this.constraintPM.ApprovalDecisionName;
        } else
            return null;
    }
    set ApprovalDecisionName(newValue) {
        this.constraintPM.ApprovalDecisionName = newValue;
    }

    get ApprovalUserName() {
        if (this.constraintPM != null) {
            return this.constraintPM.ApprovalUserName;
        } else
            return null;
    }
    set ApprovalUserName(newValue: string) {
        this.constraintPM.ApprovalUserName = newValue;
    }

    get ApprovalNote() {
        if (this.constraintPM != null) {
            return this.constraintPM.ApprovalNote;
        } else
            return null;
    }
    set ApprovalNote(newValue: string) {
        this.constraintPM.ApprovalNote = newValue;
    }

    get AgentObjection() {
        if (this.constraintPM != null) {
            return this.constraintPM.AgentObjection;
        } else
            return null;
    }
    set AgentObjection(newValue: string) {
        this.constraintPM.AgentObjection = newValue;
    }

    collateralRequestNumber: string;
    get CollateralRequestNumber() { return this.collateralRequestNumber; }
    set CollateralRequestNumber(newValue: string) { this.collateralRequestNumber = newValue; }

    paymentNumber: string;
    get PaymentNumber() { return this.paymentNumber; }
    set PaymentNumber(newValue: string) { this.paymentNumber = newValue; }

    paymentOrderId: string;
    get PaymentOrderId() { return this.paymentOrderId; }
    set PaymentOrderId(newValue: string) { this.paymentOrderId = newValue; }

    requestedAmount: string;
    get RequestedAmount() { return this.requestedAmount; }
    set RequestedAmount(newValue: string) { this.requestedAmount = newValue; }

    collateralRequestStatus: string;
    get CollateralRequestStatus() { return this.collateralRequestStatus; }
    set CollateralRequestStatus(newValue: string) { this.collateralRequestStatus = newValue; }
    //#endregion

    private ManageScreensVisibility() {

        this.IsApprovalDecisionHyperlinkVisibile = false;
        this.IsStatusHyperlinkVisibile = true;

        switch (this.ConstraintStatusCode) {
            case "1":
                {
                    this.lineHeight = 80;
                    this.IsStatus1Visibile = true;
                    this.IsStatus2Visibile = false;
                    this.IsStatus3Visibile = false;
                    this.IsStatus4Visibile = false;
                    this.IsStatus5Visibile = false;
                    break;
                }
            case "2":
                {
                    this.IsStatus1Visibile = false;
                    this.IsStatus2Visibile = true;
                    this.IsStatus3Visibile = false;
                    this.IsStatus4Visibile = false;
                    this.IsStatus5Visibile = false;
                    break;
                }
            case "3":
                {
                    this.lineHeight = 115;
                    this.IsStatus1Visibile = false;
                    this.IsStatus2Visibile = false;
                    this.IsStatus3Visibile = true;
                    this.IsStatus4Visibile = false;
                    this.IsStatus5Visibile = false;
                    this.CheckDetailsInfoVisibilities();

                    break;
                }
            case "4":
                {
                    if (AppTool.IsNullOrEmpty(this.constraintPM.AgentObjection)) {
                        this.IsStatus4AgentObjectionVisibile = false;
                    }
                    this.IsStatus1Visibile = false;
                    this.IsStatus2Visibile = false;
                    this.IsStatus3Visibile = false;
                    this.IsStatus4Visibile = true;
                    this.IsStatus5Visibile = false;
                    break;
                }
            case "5":
                {
                    if (AppTool.IsNullOrEmpty(this.constraintPM.CustomsCollateralId)) {
                        this.IsStatus1Visibile = false;
                        this.IsStatus2Visibile = false;
                        this.IsStatus3Visibile = false;
                        this.IsStatus4Visibile = false;
                        this.IsStatus5Visibile = true;
                    }
                    else {
                        this.IsStatus1Visibile = false;
                        this.IsStatus2Visibile = false;
                        this.IsStatus3Visibile = true;
                        this.IsStatus4Visibile = false;
                        this.IsStatus5Visibile = false;
                        this.CheckDetailsInfoVisibilities();
                    }

                    if (!AppTool.IsNullOrEmpty(this.constraintPM.ApprovalDecision)) {
                        this.IsApprovalDecisionHyperlinkVisibile = true;
                        this.IsStatusHyperlinkVisibile = false;
                    }
                    else {
                        this.IsApprovalDecisionHyperlinkVisibile = false;
                        this.IsStatusHyperlinkVisibile = true;
                    }
                    break;
                }

            default:
                {
                    this.IsStatus1Visibile = false;
                    this.IsStatus2Visibile = false;
                    this.IsStatus3Visibile = false;
                    this.IsStatus4Visibile = false;
                    this.IsStatus5Visibile = false;
                    break;
                }

        }
    }

    private CheckDetailsInfoVisibilities() {
        if (!AppTool.IsNullOrEmpty(this.constraintPM.CustomsCollateralId)) {
            this.IsCollateralInfoVisibile = true;
        }
        else {
            this.IsCollateralInfoVisibile = false;
        }

        if (!AppTool.IsNullOrEmpty(this.constraintPM.AgentObjection)) {
            this.IsAgentObjectionVisibile = true;
        }
        else {
            this.IsAgentObjectionVisibile = false;
        }
        if (!AppTool.IsNullOrEmpty(this.constraintPM.CustomsCollateralId)) {
            this.declarationWebService.GetSingleCustomsCollateral(this.constraintPM.CustomsCollateralId).subscribe((response: ServiceResponse) => {
                var result = response.Result;
                console.log("[Response/GetSingleCustomsCollateral]: ", result);


                if (!AppTool.IsNullOrEmpty(result)) {

                    var collateral = result;
                    this.Collateral = collateral;

                    // calculate request amount
                    var sum = 0;
                    collateral.CustomsCollateralsConditions.forEach((el) => {
                        sum += el.RequestedAmount;
                    });
                    this.RequestedAmount = sum + "";
                    console.log("RequestedAmount calculated:", this.RequestedAmount);

                    this.CollateralRequestNumber = collateral.CollateralRequestNumber;
                    this.CollateralRequestStatus = collateral.CollateralRequestStatusName;

                    if (collateral.CustomsCollateralsAnswers.Count != 0) {

                        this.IsCollateralAnswersVisibile = true;
                        if (collateral.PaymentNumber != null) {
                            this.PaymentOrderId = collateral.PaymentOrderId;
                            this.PaymentNumber = collateral.PaymentNumber;

                            this.IsPaymentNumberVisibile = true;
                        }
                        else {
                            this.IsPaymentNumberVisibile = false;
                        }

                    }
                    else {
                        this.IsCollateralAnswersVisibile = false;
                    }
                } else {

                }


                if (this.IsCollateralAnswersVisibile || this.IsAgentObjectionVisibile) {
                    this.lineHeight = 115;
                }

            });
        }




        this.declarationWebService.CheckIfDocumentPointerExistsForConstraint(this.constraintPM.ConstraintNumber).subscribe((response: ServiceResponse) => {
            var result = response.Result;
            console.log("[Response/CheckIfDocumentPointerExistsForConstraint]: ", result);

            if (!AppTool.IsNullOrEmpty(result)) {

                var exist = result;
                if (exist) {
                    this.IsAgentObjectionVisibile = true;
                }
                else {
                    this.IsRequestedDocumentVisibile = false;
                }

            }


        });
    }

    //*//
    IsAgentObjectionButtonVisibile: boolean = true;
    IsApprovalDenaialVisibile: boolean = false;
    ApprovalDenaialTitle = "";

    ViewConstraintDetails() {
        var constraint = this.constraintPM;

        if (constraint.ApprovalDecision == "2") {
            this.IsAgentObjectionButtonVisibile = false;
        }

        if (AppTool.IsNullOrEmpty(constraint.CustomsCollateralId)) {

            if (this.ConstraintStatusCode == "1" || this.ConstraintStatusCode == "2") {
                this.IsApprovalDenaialVisibile = false;
            }

            if (this.ConstraintStatusCode == "4") {
                this.ApprovalDenaialTitle = TextCodeTranslator.Translate("Customs.DeclarationConstraint.O.DenaialReason");
            }
            else if (this.ConstraintStatusCode == "5") {
                this.ApprovalDenaialTitle = TextCodeTranslator.Translate("Customs.DeclarationConstraint.O.ApprovalReason");
            }
            else {
                this.ApprovalDenaialTitle = TextCodeTranslator.Translate("Customs.DeclarationConstraint.O.ApprovalReason");
            }

            var window = new LogitudeWindow();
            window.Width = 600;
            window.Height = 500;
            window.Title = "פרטי אילוץ";
            window.WindowArgs = {
                DeclarationError: this.declarationError,
                ConstraintPM: this.constraintPM,
                ApprovalDenaialTitle: this.ApprovalDenaialTitle,
                IsAgentObjectionButtonVisibile: this.IsAgentObjectionButtonVisibile,
            };
            window.Show("./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/CustomsAnswers/ConstraintsDetailsComponent");
            // Remove DetectChanges from answer tab
            this.parent.cd.detach();
            window.WindowClosed.subscribe(() => {
                this.parent.cd.reattach();
                var timertoken = setTimeout(() => {
                    this.parent.cd.detectChanges();
                }, 100);
            });
        }
        else {

            var window = new LogitudeWindow();
            window.Width = 610;
            window.Height = 660;
            window.ShowCloseButton = true;
            window.WindowArgs = {
                DeclarationError: this.declarationError,
                ConstraintPM: this.constraintPM,
                ApprovalDenaialTitle: this.ApprovalDenaialTitle,
                IsAgentObjectionButtonVisibile: this.IsAgentObjectionButtonVisibile,
            };
            window.Title = "";
            window.Show("./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/CustomsAnswers/ConstraintDetailWithCollateralComponent");
            // Remove DetectChanges from answer tab
            this.parent.cd.detach();
            window.WindowClosed.subscribe(() => {
                this.parent.cd.reattach();
                var timertoken = setTimeout(() => {
                    this.parent.cd.detectChanges();
                }, 100);
            });
        }
    }

    PaymentNumberLinkClicked() {
        if (!AppTool.IsNullOrEmpty(this.PaymentOrderId)) {

            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: this.PaymentOrderId, ObjectTableName: 'Customs.PaymentOrder' });
                    cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                    });
                });
                
        }

    }

    OpenCollateralWindow() {

        if (this.Collateral) {

            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Title = TextCodeTranslator.Translate("Customs.Declaration.O.EditCustomsCollateral");
            logitudeWindow.WindowArgs = { CurrentEntity: this.Collateral };
            logitudeWindow.Height = 730;
            logitudeWindow.Width = 660;
          logitudeWindow.Show('./CustomsModules/CustomsCollateral/Components/CustomsCollateralComponent');
            // Remove DetectChanges from answer tab
            this.parent.cd.detach();
            logitudeWindow.WindowClosed.subscribe(() => {
                this.parent.cd.reattach();
                this.parent.cd.detectChanges();
            });
        } else {
            console.log("[Error] No colleteral to open!!!");
        }

    }
}


