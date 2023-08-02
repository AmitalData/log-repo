declare var window: any;
import { Observable } from 'rxjs';
import { Guid } from '../../../../Infrastructure/Utilities/Guid';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { Component, Output, EventEmitter, OnInit, ComponentRef, ViewChild, OnDestroy, Injectable } from '@angular/core';
import { AppTool } from '../../../../Infrastructure/Tools';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { CourierMasterPM } from '../../../../Customs/EntityPMs/CourierMasterPM';
import { CourierMasterValidator } from '../../../../Customs/Validators/CourierMasterValidator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { DeclarationCourierStatusListService } from '../../../../Customs/Services/StandardLists/DeclarationCourierStatusListService';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { DeclarationEditComponentController } from '../../../../Customs/Controller/DeclarationEditComponentController';
import { DropdownMenuFilterComponent } from './DropdownMenuFilterComponent'
import { SendPayReadyLowRequestParams } from '../../../../Customs/DataContract/RequestParams/SendPayReadyLowRequestParams';
import { SendALLCorrectRequestParams } from '../../../../Customs/DataContract/RequestParams/SendALLCorrectRequestParams';
import { CourierWorksheetSharedDataService } from '../../../../Customs/Services/DataChange/CourierWorksheetSharedDataService';
import { CustomsSettingExtendedListService } from '../../../../Customs/Services/ExtendedLists/CustomsSettingExtendedListService';
import { InterfaceManagementPMService } from '../../../../Customs/Services/StandardPMs/InterfaceManagementPMService';
import { CustomsRequestsSheetPM } from '../../../../Customs/EntityPMs/CustomsRequestsSheetPM';
import { SendUnCorrectDocumentsRequestParams } from '../../../../Customs/DataContract/RequestParams/SendUnCorrectDocumentsRequestParams';
import { CourierPendingReasonListService } from '../../../../Customs/Services/StandardLists/CourierPendingReasonListService';
import { CourierPendingReasonList } from '../../../../Customs/EntityLists/CourierPendingReasonList';
import { CourierMasterPMService } from '../../../../Customs/Services/StandardPMs/CourierMasterPMService';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { PendingRequestParams } from 'Customs/DataContract/RequestParams/PendingRequestParams';
import { CourierMasterService } from 'Customs/Services/Others/CourierMasterService';
import { SendALLDelayFormParams } from '../../../../Customs/DataContract/RequestParams/SendALLDelayFormParams';
import { InterfaceTenantDefinitionsWebService } from 'Customs/Services/WebServices/InterfaceTenantDefinitionsWebService';
import { DeclarationWebService } from 'Customs/Services/WebServices/DeclarationWebService';

@Component({
    templateUrl: './CourierWorksheetFromExcelComponent.html',
    selector: 'CourierWorksheetFromExcelComponent',
    providers: [CourierWorksheetSharedDataService],

})


export class CourierWorksheetFromExcelComponent extends BaseComponent implements OnDestroy {
    public _SelectedItems: any;

    ObjectTableName: string = "Customs.CourierMaster";
    DataContext: any = this;
    entityPM: CourierMasterPM;
    public ComponentBackground: string = "white";
    private _RowsItems: any;
    public get RowsItems(): any {
        return this._RowsItems;
    }
    public set RowsItems(value: any) {
        this._RowsItems = value;
    }

    private _SelectedRow: any;
    public get SelectedRow(): any {
        return this._SelectedRow;
    }
    public set SelectedRow(value: any) {
        this._SelectedRow = value;
    }
    _CourierMasterValidator: CourierMasterValidator = new CourierMasterValidator();
    _CourierMasterService: CourierMasterService = new CourierMasterService();
    _DeclarationCourierStatusListService: DeclarationCourierStatusListService = new DeclarationCourierStatusListService();
    _EntityListService: EntityListService = new EntityListService();

    @ViewChild(DropdownMenuFilterComponent)
    public MyDropdownMenuFilterComponent: DropdownMenuFilterComponent = new DropdownMenuFilterComponent(null, null);

    public ComponentRef: ComponentRef<CourierWorksheetFromExcelComponent>;
    _ValidationErrors: string[] = [];
    _ValidationErrors2: string[] = [];
    _TabFilterList: TabFilter[] = [];
    _SelectedTabFilter: TabFilter;
    set SelectedTabFilter(val: TabFilter) { this._SelectedTabFilter = val; }

    _SelectedBOLValue: string = 'A';//ALL//High//Low
    _SelectedStatusValue: string = 'A';//ALL//Open//Close
    _SelectedAvailableValue: string = 'A';//ALL//Available//NotAvailable//Additional
    _SelectedTotalInvoiceValue: string = 'A';
    _SelectedFastIndividualProcessValue: string = 'A';
    _SelectedCustomStatusValue: string = 'A';
    _SelectedFinalReleaseValue: string = 'A';

    _SelectedMNFValue: string = 'A'; // ALL/Complete/Wrong
    _SelectedDECValue: string = 'A'; // ALL/Complete/Wrong_SelectedItems
    _SelectedDOCValue: string = 'A'; // All/Correction/CorrectionUploaded
    _SelectedACCValue: string = 'A'; // Wrong/WrongSpecial
    //_SelectedPAYValue: string = 'C'; // Correct/InProgress/ReadyToSend
    //_SelectedHOLDValue: string = 'A'; //All/Pending Codes List
    _SelectedDelivered: string = '';

    //Selected tabs
    _SelectedPAYValue: string = 'R'; // Correct/InProgress/ReadyToSend
    _SelectedDECTabValue: string = 'MX'; // Correct/InCorrect/InProgress/ReadyToSend
    _SelectedMNFTABValue: string = 'MX'; // Correct/InCorrect/InProgress/ReadyToSend
    _SelectedDOCTabValue: string = 'MX'; // Correct/InCorrect/InProgress/ReadyToSend

    public columns: any[] = null;
    public columnsPending: any[] = null;

    IsActionButtonsEnabled: boolean = false;
    IsFiltered: boolean = false;
    IsMamanEnabled: boolean = false;
    isAllowAccounting: boolean = false;
    isAllowBulkPendind: boolean = false;
    IsILOVLEnabled: boolean = false;
    IsILSWSEnabled: boolean = false;
    CourierHawbsFromExcelUploaded: boolean = false
    private _DeclarationWebService: DeclarationWebService = new DeclarationWebService;

    @Output() MenuHeaderchangeevent = new EventEmitter();
    @Output() onQueryChangeEvent = new EventEmitter();
    @Output() CustomBackFromEditevent = new EventEmitter();
    public DelayFormVisibility: boolean = false;
    public IsDisplayOnly: boolean = false;
    public IsSendDocumentsFromQueueButton: boolean = false;
    public DisplayOnlyMessage: string = "";
    private currentSession = SessionLocator.SelectedSession;
    private ChangedUnloadPortSite: boolean;
    HasRequiresApprovalFeature: boolean = false;
    EntityResourceImported: boolean = false;
    //constructor(public entityArgs: EntityArgs) {
    constructor(public _CourierWorksheetSharedDataService: CourierWorksheetSharedDataService, public entityArgs: EntityArgs, private EntityResourceService: EntityResourceService) {
        super();
        if (FeatureLocator.HasFeaturePermession("Customs.CourierPendingReason", "PendingRequiresApproval")) {
            this.HasRequiresApprovalFeature = true;
        }
        this.EntityResourceService.getEntityResourceByTableName("Customs.CourierPendingReason").subscribe(response => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.DeclarationCourierStatus").subscribe(response => {
                this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
                    this.EntityResourceService.getEntityResourceByTableName("Customs.CourierMaster").subscribe(response => {
                        this._TabFilterList.push(new TabFilter("ALL", "כל הש.מ.ב ", null, null));
                        this._TabFilterList.push(new TabFilter("DOC", "בעיות במסמכים ", null, null));
                        this._TabFilterList.push(new TabFilter("SVG", "בעיות בסיווג", null, null));
                        this._TabFilterList.push(new TabFilter("MNF", "בעיות במצהר ", null, null));
                        this._TabFilterList.push(new TabFilter("DEC", "בעיות בהצהרה", null, null));
                        this._TabFilterList.push(new TabFilter("PAY", "תשלום", null, null));
                        this._TabFilterList.push(new TabFilter("HOLD", "Pending", null, null));
                        this._TabFilterList.push(new TabFilter("ACC", "מסוף", null, null));
                        this.GetMamanPUR();
                        this.GetIsSendDocumentsFromQueueButton();
                        this.isAllowAccounting = FeatureLocator.HasFeaturePermession("Customs.CourierMaster", "AllowAccounting")
                        this.isAllowBulkPendind = FeatureLocator.HasFeaturePermession("Customs.CourierMaster", "AllowBulkPendind")
                        this.DelayFormVisibility = FeatureLocator.HasFeaturePermession("Customs.CourierMaster", "AllowDelayForm");
                        this.EntityResourceImported = true;
                        this.BuildColumns();
                        this.BuildColumnsPending();
                        this._SelectedTabFilter = this._TabFilterList[0];
                        this.TabFilterClick(this._SelectedTabFilter);
                    });
                });
            });
        });


    }

    ngOnInit() {
        this._CourierWorksheetSharedDataService.CurrentMessage
            .subscribe(message => {
                if (message == "DoRefresh") {
                    this.RefreshButtonClicked();
                }
            });
    }
    ngOnDestroy() {
        this._DeclarationWebService.DeleteCourierMawbsFromExcel(SessionLocator.LoggedUserId).subscribe((response: ServiceResponse) => {
            // this.ErrorsResultList.InsertCollection(res);
        });



    }

    TabFilterClick(item) {
        if (!item)
            return;
        this._CourierWorksheetSharedDataService._SelectedItems.Collection = [];
        this._SelectedTabFilter = item;
        this._SelectedMNFValue = 'A';
        this._SelectedMNFTABValue = 'MX';
        this._SelectedDECValue = 'A';
        this._SelectedDECTabValue = 'MX';
        this._SelectedDOCTabValue = 'MX';
        this._SelectedDOCValue = 'A';
        this._SelectedACCValue = 'A';
        this._SelectedPAYValue = 'R';
        if (this.SelectedPendingCodeFilter == null && this._PendingCodes != null && this._PendingCodes.length > 0) this.SelectedPendingCodeFilter = this._PendingCodes[0];

        switch (item.Code) {
            case "DECR":
                this._ReadyDECToBatchSend = item.Value;
                if (this._ValidationErrors != null && this._ValidationErrors.length > 0) {
                    this._ReadyDECToBatchSend = 0;
                }
                this._ReadyDECToBatchSendButtonText = TextCodeTranslator.Translate("Customs.General.O.FieldForTableIsRequired") + ' (' + this._ReadyDECToBatchSend + ')';
                break;
            case "MNF":
                this._SelectedMNFValue = 'C';
                break;
            case "DEC":
                this._SelectedDECValue = 'C';
                break;
            case "DOC":
                this._SelectedDOCValue = 'A';
                break;
            case "ACC":
                this._SelectedACCValue = 'W';
                break;
            case "HOLD":
                if (this.SelectedPendingCodeFilter == null && this._PendingCodes != null && this._PendingCodes.length > 0) this.SelectedPendingCodeFilter = this._PendingCodes[0];
                break;
        }
        if (item.Code != "HOLD") {
            this.SelectedPendingCodeFilter = this._PendingCodes[0];
            this.RefreshButtonClicked();
        } else {
            this.GetPending();
        }

    }

    SetWindowArgs(windowArgs) {
        this.entityPM = windowArgs.CurrentEntity;
        this.CheckRequiredFields();

        if (!AppTool.IsNullOrEmpty(windowArgs.TabMode)) {
            switch (windowArgs.TabMode) {
                case "Pending902":
                    {
                        this.PendingFilter = "902";
                        this._SelectedTabFilter = this._TabFilterList[6];
                        ///this.TabFilterClick(this._TabFilterList[6]);
                        //if (this._PendingCodes != null && this._PendingCodes.length > 0) this._SelectedPendingCodeFilter = this._PendingCodes.find(r => r.Key == this.PendingFilter);
                        //this.RefreshList();
                        break;
                    }
                case "Pending900":
                    {
                        this.PendingFilter = "900";
                        this._SelectedTabFilter = this._TabFilterList[6];
                        //this.TabFilterClick(this._TabFilterList[6]);
                        //if (this._PendingCodes != null && this._PendingCodes.length > 0) this._SelectedPendingCodeFilter = this._PendingCodes.find(r => r.Key == this.PendingFilter);
                        //this.RefreshList();
                        break;
                    }
                case "OpenDeclarations":
                    {
                        this._SelectedTabFilter = this._TabFilterList[0];
                        this._SelectedStatusValue = 'O';
                        this.SelectedStatusValueClick('O');
                        break;
                    }
                case "SuspendedDeclarations":
                    {
                        this._SelectedTabFilter = this._TabFilterList[0];
                        this._SelectedCustomStatusValue = 'S';
                        this._SelectedStatusValue = 'O';
                        this.SelectedStatusValueClick('O');
                        this.SelectedCustomStatusValueClick('S');
                        break;
                    }
                case "MissingClassification":
                    {
                        this._SelectedTabFilter = this._TabFilterList[2];
                        //this.TabFilterClick(this._TabFilterList[2]);
                        break;
                    }
                case "NoOfCourierHawbWithoutDelivery":
                    {
                        this._SelectedFinalReleaseValue = 'Y';
                        this._SelectedTabFilter = this._TabFilterList[0];
                        this._SelectedDelivered = 'A';
                        break;
                    }
                case "NoOfCourierHawbwWithoutHatara":
                    {
                        this._SelectedFinalReleaseValue = 'N';
                        this._SelectedTabFilter = this._TabFilterList[0];
                        break;
                    }
            }
        }


        this.TabFilterClick(this._SelectedTabFilter);
    }

    CheckRequiredFields() {
        this._CourierMasterService.GetRequiredFieldsForCourierMasterIncludeManifest(this.entityPM.Id).subscribe((response: ServiceResponse) => {
            if (response.Result) {
                this._ValidationErrors = this.GetRequiredErrorsList(response.Result.RequiredFields);
                //this.RefreshButtonClicked();// do not interupt the server 
            }
        });
    }

    GetRequiredErrorsList(errorsList: any[]) {
        var errorsMessages: string[] = [];
        errorsList.forEach((error) => {
            if (!AppTool.IsNullOrEmpty(error.CustomMessageError)) {
                if (error.CustomMessageError.indexOf("specialerror") > -1) {
                    var ErrorMessage = "";
                    var errorArr = error.CustomMessageError.split(',');
                    ErrorMessage = errorArr[1] + TextCodeTranslator.Translate(errorArr[2]);
                    errorsMessages.push(ErrorMessage);
                }
                else {
                    errorsMessages.push(TextCodeTranslator.Translate(error.CustomMessageError));
                }
            }
            else {
                var table = window.ObjectTables.filter(d => d.Name === error.TableName)[0];
                var field = window.ObjectFields.filter(d => d.FieldName == error.FieldName && d.ObjectTableId == table.Id)[0];

                var message = TextCodeTranslator.Translate("Customs.General.O.FieldForTableIsRequired");
                var fieldName = TextCodeTranslator.Translate(field.FullNameTextCodeCode);
                var tableName = TextCodeTranslator.Translate(error.TableName);
                message = message.replace('%FieldName', fieldName);
                message = message.replace('%TableName', tableName);
                message = message.replace('%EntityReference', error.EntityReference);
                errorsMessages.push(message);
            }
        });
        return errorsMessages;
    }

    Close() {
        SessionLocator.SelectedSession.CloseCurrentWindow();
    }

    SearchFilter: string;
    onSearchTextChangeEvent(text: string) {
        this.SearchFilter = text;
        this.RefreshList();
    }

    SendALLCorrectManifest(courierDeclarationStatusCode: string) {
        if (this._ValidationErrors != null && this._ValidationErrors.length > 0) {
            var myMessageWindow = new MessageWindow();
            myMessageWindow.Width = 250;
            myMessageWindow.Height = 150;
            myMessageWindow.Show("חסרים שדות חובה ברמת הטיסה");
            return;
        }

        if (this._ReadyMNFToBatchSend == 0 && courierDeclarationStatusCode == "R") {
            var myMessageWindow = new MessageWindow();
            myMessageWindow.Width = 250;
            myMessageWindow.Height = 150;
            myMessageWindow.Show(TextCodeTranslator.Translate("Customs.CourierMaster.O.NoResults"));
            return;
        }
        if (this._CorrectMNFToBatchSend == 0 && courierDeclarationStatusCode == "RV") {
            var myMessageWindow = new MessageWindow();
            myMessageWindow.Width = 250;
            myMessageWindow.Height = 150;
            myMessageWindow.Show(TextCodeTranslator.Translate("Customs.CourierMaster.O.NoResults"));
            return;
        }

        var currRequestParams = new SendALLCorrectRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.CourierMasterId = this.entityPM?.Id;
        currRequestParams.HAWB = this.entityPM?.HAWB;
        currRequestParams.IsWorkSheetFromExcel = this.CourierHawbsFromExcelUploaded;
        if (this._CourierWorksheetSharedDataService._SelectedItems != null && this._CourierWorksheetSharedDataService._SelectedItems.Collection.length > 0) {
            currRequestParams.Declarations = this._CourierWorksheetSharedDataService._SelectedItems.Collection;
        }

        currRequestParams.CourierDeclarationStatusCode = courierDeclarationStatusCode;
        currRequestParams.SelectedAvailableValue = this._SelectedAvailableValue;
        currRequestParams.SelectedBOLValue = this._SelectedBOLValue;
        currRequestParams.SelectedStatusValue = this._SelectedStatusValue;
        currRequestParams.SelectedTotalInvoiceValue = this._SelectedTotalInvoiceValue;
        currRequestParams.SelectedFastIndividualProcessValue = this._SelectedFastIndividualProcessValue;
        currRequestParams.SelectedCustomStatusValue = this._SelectedCustomStatusValue;
        currRequestParams.SelectedFinalReleaseValue = this._SelectedFinalReleaseValue;


        this._CourierMasterService.PostSendALLCorrectManifest(currRequestParams)
            .subscribe((res: any) => {

                SessionLocator.SelectedSession.StopBusyIndicator();
                var myMessageWindow = new MessageWindow();
                myMessageWindow.Show(res.Result);
                myMessageWindow.WindowClosed.subscribe(s => {
                    this.RefreshButtonClicked();
                });
            });

    }

    SendReadyLOWPAYToBatch() {

        if (this._PAYReadyNotFastindividual == 0) {
            var myMessageWindow = new MessageWindow();
            myMessageWindow.Width = 250;
            myMessageWindow.Height = 150;
            myMessageWindow.Show(TextCodeTranslator.Translate("Customs.CourierMaster.O.NoResults"));
            return;
        }

        this.currentSession.StartBusyIndicatorLoading();
        this.currentSession.entityResourceService.getEntityResourceByTableName("Customs.DeclarationPaymentMethod", 0).subscribe((response: any) => {
            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Width = 500;
            logitudeWindow.Height = 300;
            logitudeWindow.Title = "בנק לתשלום";
            SessionLocator.SelectedSession.StopBusyIndicator();
            logitudeWindow.ComponentLoaded.subscribe(cmpRef => {
                //    cmpRef.IsClosedLost = true;
            });
            logitudeWindow.WindowClosed.subscribe(resultWindowClosed => {
                let InternalBankId: string = resultWindowClosed;

                if (!AppTool.IsNullOrEmpty(InternalBankId)) {
                    SessionLocator.SelectedSession.StartBusyIndicatorCreating();
                    if (this._CourierWorksheetSharedDataService._SelectedItems != null && this._CourierWorksheetSharedDataService._SelectedItems.Collection.length > 0) {
                        var currRequestParams = new SendPayReadyLowRequestParams();
                        currRequestParams.LoggingEnabled = true;
                        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
                        currRequestParams.Tenant = SessionLocator.Tenant;
                        currRequestParams.CourierMasterId = this.entityPM?.Id;
                        currRequestParams.HAWB = this.entityPM?.HAWB;
                        currRequestParams.InternalBankId = InternalBankId;
                        currRequestParams.IsWorkSheetFromExcel = this.CourierHawbsFromExcelUploaded;
                        currRequestParams.Declarations = this._CourierWorksheetSharedDataService._SelectedItems.Collection;
                        this._CourierMasterService.PostSendPayReadyLow2755(currRequestParams)
                            .subscribe((res: any) => {
                                this.currentSession.StopBusyIndicator();
                                var myMessageWindow = new MessageWindow();
                                myMessageWindow.Show(res.Result);
                                myMessageWindow.WindowClosed.subscribe(s => {
                                    this.RefreshButtonClicked();
                                });
                            });
                    }
                    else {
                        this._CourierMasterService.GetSendPayReadyLow2755(this.entityPM?.Id, this.entityPM?.HAWB, InternalBankId, true)
                            .subscribe((res: any) => {
                                this.currentSession.StopBusyIndicator();
                                var myMessageWindow = new MessageWindow();
                                myMessageWindow.Show(res.Result);
                                myMessageWindow.WindowClosed.subscribe(s => {
                                    this.RefreshButtonClicked();
                                });
                            });
                    }
                }

            });
            logitudeWindow.Show('./CustomsModules/CustomsCourier/Components/CourierWorkSheet/GetInternalBankComponent');

            //''
            ;
        });

    }

    SendALLCorrectDec(courierDeclarationStatusCode: string) {

        if (this._ReadyDECToBatchSend == 0 && courierDeclarationStatusCode == "R") {
            var myMessageWindow = new MessageWindow();
            myMessageWindow.Width = 250;
            myMessageWindow.Height = 150;
            myMessageWindow.Show(TextCodeTranslator.Translate("Customs.CourierMaster.O.NoResults"));
            return;
        }
        if (this._CorrectDECToBatchSend == 0 && courierDeclarationStatusCode == "RV") {
            var myMessageWindow = new MessageWindow();
            myMessageWindow.Width = 250;
            myMessageWindow.Height = 150;
            myMessageWindow.Show(TextCodeTranslator.Translate("Customs.CourierMaster.O.NoResults"));
            return;
        }
        if (this._InCorrectDECToBatchSend == 0 && courierDeclarationStatusCode == "X") {
            var myMessageWindow = new MessageWindow();
            myMessageWindow.Width = 250;
            myMessageWindow.Height = 150;
            myMessageWindow.Show(TextCodeTranslator.Translate("Customs.CourierMaster.O.NoResults"));
            return;
        }

        var currRequestParams = new SendALLCorrectRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.CourierMasterId = this.entityPM?.Id;
        currRequestParams.HAWB = this.entityPM?.HAWB;
        if (this._CourierWorksheetSharedDataService._SelectedItems != null && this._CourierWorksheetSharedDataService._SelectedItems.Collection.length > 0) {
            currRequestParams.Declarations = this._CourierWorksheetSharedDataService._SelectedItems.Collection;
        }

        currRequestParams.CourierDeclarationStatusCode = courierDeclarationStatusCode;
        currRequestParams.SelectedAvailableValue = this._SelectedAvailableValue;
        currRequestParams.SelectedBOLValue = this._SelectedBOLValue;
        currRequestParams.SelectedStatusValue = this._SelectedStatusValue;
        currRequestParams.SelectedTotalInvoiceValue = this._SelectedTotalInvoiceValue;
        currRequestParams.SelectedFastIndividualProcessValue = this._SelectedFastIndividualProcessValue;
        currRequestParams.SelectedCustomStatusValue = this._SelectedCustomStatusValue;
        currRequestParams.SelectedFinalReleaseValue = this._SelectedFinalReleaseValue;
        currRequestParams.IsWorkSheetFromExcel= this.CourierHawbsFromExcelUploaded;

        this._CourierMasterService.PostSendALLCorrectDec(currRequestParams)
            .subscribe((res: any) => {
                this.currentSession.StopBusyIndicator();
                var myMessageWindow = new MessageWindow();
                myMessageWindow.Show(res.Result);
                myMessageWindow.WindowClosed.subscribe(s => {
                    this.RefreshButtonClicked();
                });
            });
        //this.SendALLCorrectDec_OLD(courierDeclarationStatusCode);
    }

    SendALLSVG(isAll: boolean) {
        if ((this._SVGTotal == 0 && !isAll) || !this.CourierHawbsFromExcelUploaded) {
            var myMessageWindow = new MessageWindow();
            myMessageWindow.Width = 250;
            myMessageWindow.Height = 150;
            myMessageWindow.Show(TextCodeTranslator.Translate("Customs.CourierMaster.O.NoResults"));
            return;
        }
        else {//task 42046
            this.Navigate();
        }

    }
    //////////////////////////////
    private currentFilters: ApiQueryFilters;
    //    private currentSearchFields: string;
    private currentSortingCol: string;
    private currentSortingDir: string;
    Navigate() {
        //        this.CurrentQueryFilters = new ApiQueryFilters();
        var MyFilters = new ApiQueryFilters();

        MyFilters.SortBy = this.currentSortingCol;
        MyFilters.SortDirection = this.currentSortingDir;

        this.BuildFiltersForQuery(MyFilters);

        MyFilters.GetCount = false;
        MyFilters.PageIndex = 0;

        MyFilters.GetAll = true;
        // MyFilters.PageSize = 100;

        //this.CurrentQueryFilters = MyFilters;
        var ids: string[] = [];
        this._EntityListService.getByFilters("Customs.DeclarationCourierStatus", MyFilters, null).then((observable: Observable<any>) => {
            observable.subscribe((response: ServiceResponse) => {
                console.log(response);
                response.Result.forEach((item) => {
                    ids.push(item.DeclarationId);
                });
                var selectedEntityId = ids[0];
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.SelectedSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        var label = "מסך עבודה";//TextCodeTranslator.Translate(this.SelectedQuery.NameTextCodeCode);
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({
                            EntityId: selectedEntityId,///$event.rowData.Id
                            BackButtonLabel: label,
                            NavigationIds: ids,
                            SelectedTabCode: "DCCF",
                            ObjectTableName: "Customs.Declaration",
                        });
                        cmpRef.instance.BackCompleted.subscribe(bk => {
                            this.OnBackFromEdit(selectedEntityId, event);
                        });                        //  if (SessionLocator.LoggedUserPM.Email == "mohammad@fnarsoft.com") {
                        //this.DestroyMe = true;
                        //}

                    });
            });

        });


    }

    public SortServerProp: any;
    public sortColDef: any;
    public sortColid: any;
    OnSortInvoked($event) {
        this.currentSortingCol = $event.colDef;
        this.currentSortingDir = $event.id;
    }
    //////////////////////////////

    RefreshButtonClicked() {
        //this.onQueryChangeEvent.emit({ Filters: this.filterAgrs, Reload: true });
        this.RefreshStatistic();
        this.RefreshList();
        this.DisplayOnlyCheck();
        this.DisplayOnlyCheckDeletePending();
        this.DisplayOnlyCheckApprovePending();


        if (this._ValidationErrors2.length > 0) {
            this._ValidationErrors2 = []
            this._CourierMasterService.GetIfAllowToCancelCourierMaster(this.entityPM?.Id).subscribe(
                (data: any) => {
                    //if (data.Result != "")
                    //    this._ValidationErrors2.push();
                    switch (data.Result) {
                        case "INVALID_INPROGRESS":
                            this._ValidationErrors2.push(TextCodeTranslator.Translate("Customs.CourierMaster.O.CantCancelFlight") + ' ' + TextCodeTranslator.Translate("Customs.CourierMaster.O.NotValidDecInProccess"))
                            break;
                        case "INVALID_PAYED":
                            this._ValidationErrors2.push(TextCodeTranslator.Translate("Customs.CourierMaster.O.CantCancelFlight") + ' ' + TextCodeTranslator.Translate("Customs.CourierMaster.O.NotValidDecWithPayment"))
                            break;
                    }
                });
        }

    }

    RefreshList() {
        setTimeout(() => {
            this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
        }, 10);
    }

    _ReadyDECToBatchSend = 0;
    _ReadyDECToBatchSendButtonText: string = "";
    _ReadyMNFToBatchSend = 0;
    _ReadyMNFToBatchSendButtonText: string = "";
    _HOLD_TotalButtonText: string = "";
    _PAYReadyNotFastindividual = 0;
    _SVGTotal = 0;
    _DOCTotal = 0;
    _DOC_U_Total = 0;
    _DOC_C_Total = 0;
    _DOC_MX_Total = 0;
    _DOC_V_Total = 0;
    _DOC_I_Total = 0;
    _DEC_W_Total = 0;
    _DEC_C_Total = 0;
    _DEC_MX_Total = 0;
    _DEC_R_Total = 0;
    _DEC_I_Total = 0;
    _DEC_V_Total = 0;
    _MNF_W_Total = 0;
    _MNF_C_Total = 0;
    _MNF_MX_Total = 0;
    _MNF_R_Total = 0;
    _MNF_I_Total = 0;
    _MNF_V_Total = 0;
    _ACC_W_Total = 0;
    _ACC_WS_Total = 0;
    _HOLD_Total = 0;
    _NotApproved = 0;
    _CorrectMNFToBatchSend = 0;
    _CorrectDECToBatchSend = 0;
    _InCorrectDECToBatchSend = 0;
    _PAY_C_Total = 0;
    _PAY_R_Total = 0;
    _PAY_I_Total = 0;

    _TotalNotAccepted = 0;
    // _DecWithoutHaTra = 0;

    _PendingCodes: KeyValuePair[] = [];

    private _SelectedDECToBatchSendButtonText: string = "";
    public get SelectedDECToBatchSendButtonText(): string {
        this._SelectedDECToBatchSendButtonText = TextCodeTranslator.Translate("Customs.CourierMaster.O.ReadyDECToSend");
        if (this._CourierWorksheetSharedDataService._SelectedItems != null && this._CourierWorksheetSharedDataService._SelectedItems.Collection.length > 0) {
            this._SelectedDECToBatchSendButtonText = TextCodeTranslator.Translate("Customs.CourierMaster.O.ReadyDECToSend") + ' (' + this._CourierWorksheetSharedDataService._SelectedItems.Collection.length + ')';
        }
        return this._SelectedDECToBatchSendButtonText;
    }

    private _SelectedDECToDelayFormButtonText: string = "";
    public get SelectedDECToDelayFormButtonText(): string {
        this._SelectedDECToDelayFormButtonText = "הפקת תעודות עיכוב";
        if (this._CourierWorksheetSharedDataService._SelectedItems != null && this._CourierWorksheetSharedDataService._SelectedItems.Collection.length > 0) {
            this._SelectedDECToDelayFormButtonText = "הפקת תעודות עיכוב" + ' (' + this._CourierWorksheetSharedDataService._SelectedItems.Collection.length + ')';
        }
        return this._SelectedDECToDelayFormButtonText;
    }

    private _SelectedMNFToBatchSendButtonText: string = "";
    public get SelectedMNFToBatchSendButtonText(): string {
        this._SelectedMNFToBatchSendButtonText = TextCodeTranslator.Translate("Customs.CourierMaster.O.ReadyMNFToSend");
        if (this._CourierWorksheetSharedDataService._SelectedItems != null && this._CourierWorksheetSharedDataService._SelectedItems.Collection.length > 0) {
            this._SelectedMNFToBatchSendButtonText = TextCodeTranslator.Translate("Customs.CourierMaster.O.ReadyMNFToSend") + ' (' + this._CourierWorksheetSharedDataService._SelectedItems.Collection.length + ')';
        }
        return this._SelectedMNFToBatchSendButtonText;
    }

    private _PendingFilter: string = "";

    get PendingFilter() { return this._PendingFilter; }
    set PendingFilter(value: string) {
        if (this._PendingFilter != value) {
            this._PendingFilter = value;

        }
    }

    RefreshStatistic() {
        // SessionLocator.SelectedSession.StartBusyIndicatorCreating();
        this._CourierMasterService.GetStatistic(this.entityPM?.Id, true, SessionLocator.LoggedUserId)
            .subscribe((res: any) => {
                //    this.currentSession.StopBusyIndicator();
                var list: KeyValuePair[];

                list = res.Result;
                list.forEach(item => {
                    switch (item.Key) {
                        case "DECR": {
                            //statements; 
                            this._ReadyDECToBatchSend = item.Value;
                            if (this._ValidationErrors != null && this._ValidationErrors.length > 0) {
                                this._ReadyDECToBatchSend = 0;
                            }
                            this._ReadyDECToBatchSendButtonText = TextCodeTranslator.Translate("Customs.CourierMaster.O.ReadyDECToSendR") + ' (' + this._ReadyDECToBatchSend + ')';
                            this._DEC_R_Total = item.Value;
                            break;
                        }
                        case "DECR_RV": {
                            //statements; 
                            this._CorrectDECToBatchSend = item.Value;
                            if (this._ValidationErrors != null && this._ValidationErrors.length > 0) {
                                this._CorrectDECToBatchSend = 0;
                            }
                            break;
                        }
                        case "MNFR": {
                            //statements; 
                            this._ReadyMNFToBatchSend = item.Value;
                            if (this._ValidationErrors != null && this._ValidationErrors.length > 0) {
                                this._ReadyMNFToBatchSend = 0;
                            }
                            this._ReadyMNFToBatchSendButtonText = TextCodeTranslator.Translate("Customs.CourierMaster.O.ReadyMNFToSendR") + ' (' + this._ReadyMNFToBatchSend + ')';
                            this._MNF_R_Total = item.Value;
                            break;
                        }
                        case "MNFR_RV": {
                            //statements; 
                            this._CorrectMNFToBatchSend = item.Value;
                            if (this._ValidationErrors != null && this._ValidationErrors.length > 0) {
                                this._CorrectMNFToBatchSend = 0;
                            }
                            break;
                        }
                        case "SVG": {
                            //statements; 
                            this._SVGTotal = item.Value;
                            var TabFilter = this._TabFilterList.filter(d => d.Code == item.Key)[0];
                            TabFilter.Total = item.Value;
                            break;
                        }
                        case "MNF": {
                            this._MNF_MX_Total = item.Value;
                            var TabFilter = this._TabFilterList.filter(d => d.Code == item.Key)[0];
                            TabFilter.Total = item.Value;
                            break;
                        }
                        case "MNF_W": {
                            //statements; 
                            this._MNF_W_Total = item.Value;
                            break;
                        }
                        case "MNF_C": {
                            this._MNF_C_Total = item.Value;
                            break;
                        }
                        case "MNF_V": {
                            this._MNF_V_Total = item.Value;
                            break;
                        }
                        case "MNF_I": {
                            this._MNF_I_Total = item.Value;
                            break;
                        }
                        case "DOC": {
                            //statements; 
                            this._DOCTotal = item.Value;
                            var TabFilter = this._TabFilterList.filter(d => d.Code == item.Key)[0];
                            TabFilter.Total = item.Value;
                            this._DOC_MX_Total = item.Value;
                            break;
                        }
                        case "DOC_U": {
                            //statements; 
                            this._DOC_U_Total = item.Value;
                            break;
                        }
                        case "DOC_C": {
                            this._DOC_C_Total = item.Value;
                            break;
                        }
                        case "DOC_I": {
                            this._DOC_I_Total = item.Value;
                            break;
                        }
                        case "DOC_V": {
                            this._DOC_V_Total = item.Value;
                            break;
                        }
                        case "DEC": {
                            this._DEC_MX_Total = item.Value;
                            var TabFilter = this._TabFilterList.filter(d => d.Code == item.Key)[0];
                            TabFilter.Total = item.Value;
                            break;
                        }
                        case "DEC_C": {
                            this._DEC_C_Total = item.Value;
                            break;
                        }
                        case "DEC_W": {
                            this._DEC_W_Total = item.Value;
                            this._InCorrectDECToBatchSend = item.Value;
                            if (this._ValidationErrors != null && this._ValidationErrors.length > 0) {
                                this._CorrectDECToBatchSend = 0;
                            }
                            break;
                        }
                        case "DEC_V": {
                            this._DEC_V_Total = item.Value;
                            break;
                        }
                        case "DEC_I": {
                            this._DEC_I_Total = item.Value;
                            break;
                        }
                        case "PAYReadyNotFastindividual": {
                            //statements; 
                            this._PAYReadyNotFastindividual = item.Value;
                            break;
                        }
                        case "ACC_W": {
                            this._ACC_W_Total = item.Value;
                            break;
                        }
                        case "ACC_WS": {
                            this._ACC_WS_Total = item.Value;
                            break;
                        }
                        //case "DecWithoutHaTra": {
                        //    this._DecWithoutHaTra = item.Value;
                        //    break;
                        //}
                        /*
                    case "PAY": {
                        this._PAY_Total = item.Value;
                        var TabFilter = this._TabFilterList.filter(d => d.Code == item.Key)[0];
                        TabFilter.Total = item.Value;
                        break;
                    }
                    */
                        case "PAY_C": {
                            //statements; 
                            this._PAY_C_Total = item.Value;
                            break;
                        }
                        //case "PAY_R": {
                        case "PAY": {
                            //statements;
                            this._PAY_R_Total = item.Value;
                            var TabFilter = this._TabFilterList.filter(d => d.Code == item.Key)[0];
                            TabFilter.Total = item.Value;
                            break;
                        }
                        case "PAY_I": {
                            //statements; 
                            this._PAY_I_Total = item.Value;
                            break;
                        }
                        case "HOLD": {
                            //statements; 
                            this._HOLD_Total = item.Value;
                            var TabFilter = this._TabFilterList.filter(d => d.Code == item.Key)[0];
                            TabFilter.Total = item.Value;

                            break;
                        }
                        case "NotApproved": {
                            //statements; 
                            this._NotApproved = item.Value;
                            break;
                        }
                        case "NotApproved": {
                            //statements; 
                            this._NotApproved = item.Value;
                            break;
                        }
                        default: {
                            //statements; 
                            var TabFilter = this._TabFilterList.filter(d => d.Code == item.Key)[0];
                            TabFilter.Total = item.Value;
                            break;
                        }
                    }

                });

            });

        //if (this._SelectedTabFilter.Code == "HOLD") {
        //    this.GetPending();
        //}

    }

    BuildColumnsPending() {
        this.columnsPending = [];

        this.columnsPending.push({
            FieldName: "MyDeclarationCheckBox",
            DataTypeCode: 'String',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '27px' },
            //IsCheckBox: true
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
        });

        this.columnsPending.push({
            FieldName: 'CourierHawb',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.CourierHawb"),
            Styles: { width: '108px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: true,
            SortByName: 'CourierHawb'
        });
        //SortByName: 'CourierHawb'

        this.columnsPending.push({
            FieldName: 'ProcedureCurrentName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.ProcedureCurrentName"),
            Styles: { width: '117px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'ProcedureCurrentName'
        });

        this.columnsPending.push({
            FieldName: 'HighLowValue',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.FastIndividualProcessCode"),
            Styles: { width: '67px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: true,
            SortByName: 'FastIndividualProcessCode'
        });

        this.columnsPending.push({
            FieldName: 'ImporterName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.CustomerName"),
            Styles: { width: '173px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'ImporterName'
        });

        this.columnsPending.push({
            FieldName: 'ImporterCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.ImporterCode"),
            Styles: { width: '90px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'SortedImporterCode'
        });

        this.columnsPending.push({
            FieldName: 'DocumentStatusCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.DocumentStatusCode"),
            Styles: { width: '55px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: true,
            SortByName: 'SortedDocumentStatusCode'
        });

        this.columnsPending.push({
            FieldName: 'IsCourierMissingClassification',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.IsCourierMissingClassification"),
            Styles: { width: '47px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: true,
            SortByName: 'IsCourierMissingClassification'
        });

        this.columnsPending.push({
            FieldName: 'CourierManifestStatusCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.CourierManifestStatusCode"),
            Styles: { width: '50px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: true,
            SortByName: 'SortedCourierManifestStatus'
        });

        this.columnsPending.push({
            FieldName: 'CourierDeclarationStatusCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.CourierDeclarationStatusCode"),
            Styles: { width: '50px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: true,
            SortByName: 'SortedCourierDeclarationStatus'
        });

        this.columnsPending.push({
            FieldName: 'CourierPaymentStatusCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.CourierPaymentStatusCode"),
            Styles: { width: '53px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: true,
            SortByName: 'CourierPaymentStatusCode'
        });

        this.columnsPending.push({
            FieldName: 'CourierCustomStatusName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.CourierCustomStatusName"),
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: true,
            SortByName: 'CourierCustomStatusName'
        });

        this.columnsPending.push({
            FieldName: 'StorageSiteStatusCodeText',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.StorageSiteStatusCode"),
            Styles: { width: '98px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: true,
            SortByName: 'StorageSiteStatusCodeText'
        });

        this.columnsPending.push({
            FieldName: 'SpecialActionStatus',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.SpecialActionStatus"),
            Styles: { width: '54px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: true,
            SortByName: 'SpecialActionStatus'
        });

        this.columnsPending.push({
            FieldName: 'CargoDescription',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.CargoDescription"),
            Styles: { width: '188px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'CargoDescription'
        });

        this.columnsPending.push({
            FieldName: 'CourierPendingReasonName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.CourierPendingReasonList"),
            Styles: { width: '105px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: true,
            SortByName: 'CourierPendingReasonName'
        });
        this.columnsPending.push({
            FieldName: 'IsClosedForFollowUp',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.IsClosedForFollowUp"),
            Styles: { width: '50px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: true,
            SortByName: 'IsClosedForFollowUp'
        });

        this.columnsPending.push({
            FieldName: 'CourierPendingReasonList',
            DataTypeCode: 'String',
            Styles: { width: '37px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: false
        });
        this.columnsPending.push({
            FieldName: 'SendSplitButton',
            DataTypeCode: 'String',
            Styles: { width: '99px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: false
        });
    }

    BuildColumns() {
        this.columns = [];

        this.columns.push({
            FieldName: "MyDeclarationCheckBox",
            DataTypeCode: 'String',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '27px' },
            //IsCheckBox: true
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
        });

        this.columns.push({
            FieldName: 'CourierHawb',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.CourierHawb"),
            Styles: { width: '108px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: true,
            SortByName: 'CourierHawb'
        });
        this.columns.push({
            FieldName: 'MAWB',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.MAWB"),
            Styles: { width: '90px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: true,
            SortByName: 'MAWB'
        });
        this.columns.push({
            FieldName: 'IntegratorName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.IntegratorName"),
            Styles: { width: '94px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: true,
            SortByName: 'IntegratorName'
        });
        //SortByName: 'CourierHawb'

        this.columns.push({
            FieldName: 'ProcedureCurrentName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.ProcedureCurrentName"),
            Styles: { width: '117px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'ProcedureCurrentName'
        });

        this.columns.push({
            FieldName: 'HighLowValue',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.FastIndividualProcessCode"),
            Styles: { width: '67px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: true,
            SortByName: 'FastIndividualProcessCode'
        });

        this.columns.push({
            FieldName: 'ImporterName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.CustomerName"),
            Styles: { width: '160px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'ImporterName'
        });

        this.columns.push({
            FieldName: 'ImporterCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.ImporterCode"),
            Styles: { width: '80px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'SortedImporterCode'
        });

        this.columns.push({
            FieldName: 'DocumentStatusCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.DocumentStatusCode"),
            Styles: { width: '47px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: true,
            SortByName: 'SortedDocumentStatusCode'
        });

        this.columns.push({
            FieldName: 'IsCourierMissingClassification',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.IsCourierMissingClassification"),
            Styles: { width: '47px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: true,
            SortByName: 'IsCourierMissingClassification'
        });

        this.columns.push({
            FieldName: 'CourierManifestStatusCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.CourierManifestStatusCode"),
            Styles: { width: '47px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: true,
            SortByName: 'SortedCourierManifestStatus'
        });

        this.columns.push({
            FieldName: 'CourierDeclarationStatusCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.CourierDeclarationStatusCode"),
            Styles: { width: '47px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: true,
            SortByName: 'SortedCourierDeclarationStatus'
        });

        this.columns.push({
            FieldName: 'CourierPaymentStatusCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.CourierPaymentStatusCode"),
            Styles: { width: '47px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: true,
            SortByName: 'CourierPaymentStatusCode'
        });

        this.columns.push({
            FieldName: 'CourierCustomStatusName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.CourierCustomStatusName"),
            Styles: { width: '90px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: true,
            SortByName: 'CourierCustomStatusName'
        });

        this.columns.push({
            FieldName: 'StorageSiteStatusCodeText',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.StorageSiteStatusCode"),
            Styles: { width: '98px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: true,
            SortByName: 'StorageSiteStatusCodeText'
        });

        this.columns.push({
            FieldName: 'SpecialActionStatus',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.SpecialActionStatus"),
            Styles: { width: '47px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: true,
            SortByName: 'SpecialActionStatus'
        });

        this.columns.push({
            FieldName: 'DeclarationStatusTypeName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.DeclarationStatusTypeName"),
            Styles: { width: '188px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'DeclarationStatusTypeName'
        });

        this.columns.push({
            FieldName: 'CourierPendingReasonName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.CourierPendingReasonList"),
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: true,
            SortByName: 'CourierPendingReasonName'
        });
        this.columns.push({
            FieldName: 'IsClosedForFollowUp',
            DataTypeCode: 'String',
            Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.IsClosedForFollowUp"),
            Styles: { width: '50px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: true,
            SortByName: 'IsClosedForFollowUp'
        });

        this.columns.push({
            FieldName: 'CourierPendingReasonList',
            DataTypeCode: 'String',
            Styles: { width: '37px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: false
        });
        this.columns.push({
            FieldName: 'SendSplitButton',
            DataTypeCode: 'String',
            Styles: { width: '99px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: false
        });
    }

    DataSource = {

        pageSize: 30,
        rowCount: null,
        //sortingCol: "CourierHawb",
        //sortingDir: "Descending",
        getRows: (skip: number, take: number, sortingCol: string, sortingDir: string, getCount: boolean, searchFields?: string, filters: ApiQueryFilters = null) => {
            var tempo = this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
            return tempo;

        },
    };

    filterAgrs: ApiQueryFilters;
    getRows(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
        if (filters == null) {
            filters = new ApiQueryFilters();
        }
        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;

        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        /*        if (AppTool.IsNullOrEmpty(filters.SortBy)) {
                    filters.SortBy = "CourierHawb";
                }
                if (AppTool.IsNullOrEmpty(filters.SortDirection)) {
                    filters.SortDirection = "Descending";
                }*/
        this.BuildFiltersForQuery(filters);

        var myout = this._EntityListService.getExtendedByFilters("Customs.DeclarationCourierStatus", filters);

        return myout;
    }

    BuildFiltersForQuery(filters: ApiQueryFilters = null) {

        if (filters == null) {
            filters = new ApiQueryFilters();
        }
        filters.addAdditionalFilter("CourierHawbsFromExcel", SessionLocator.LoggedUserId, null, null, "Equal", true, false, false, "string");

        filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "number");

        switch (this._SelectedTabFilter.Code) {
            //case "ACC":
            case "ALL": {
                break;
            }
            default: {
                filters.addAdditionalFilter("Is" + this._SelectedTabFilter.Code + "Tab", true, null, null, "Equals", false, false, false, "Boolean");
                break;
            }
        }

        switch (this._SelectedBOLValue) {
            case "L": {
                filters.addAdditionalFilter("HighLowValue", "L", null, null, "Equals", false, false, false, "string");
                break;
            }
            case "H": {
                filters.addAdditionalFilter("HighLowValue", "H", null, null, "Equals", false, false, false, "string");
                break;
            }
        }

        switch (this._SelectedStatusValue) {
            case "O": {
                filters.addAdditionalFilter("IsClosedForFollowUp", false, null, null, "Equals", false, false, false, "Boolean");
                break;
            }
            case "C": {
                filters.addAdditionalFilter("IsClosedForFollowUp", true, null, null, "Equals", false, false, false, "Boolean");
                break;
            }
        }

        if (this._SelectedTabFilter.Code == "MNF") {
            switch (this._SelectedMNFTABValue) {
                case "I": {
                    filters.addAdditionalFilter("CourierManifestStatusCode", "I", null, null, "Equals", false, false, false, "string");
                    break;
                }
                case "R": {
                    filters.addAdditionalFilter("CourierManifestStatusCode", "R", null, null, "Equals", false, false, false, "string");
                    break;
                }
                case "V": {
                    filters.addAdditionalFilter("CourierManifestStatusCode", "V", null, null, "Equals", false, false, false, "string");
                    break;
                }
                case "MX": {
                    filters.addAdditionalFilter("Is" + this._SelectedTabFilter.Code + "Tab", true, null, null, "Equals", false, false, false, "Boolean");
                    switch (this._SelectedMNFValue) {
                        case "C": {
                            filters.addAdditionalFilter("CourierManifestStatusCode", "M", null, null, "Equals", false, false, false, "string");
                            break;
                        }
                        case "W": {
                            filters.addAdditionalFilter("CourierManifestStatusCode", "X", null, null, "Equals", false, false, false, "string");
                            break;
                        }
                    }
                    break;
                }
                default: {
                    filters.addAdditionalFilter("Is" + this._SelectedTabFilter.Code + "Tab", true, null, null, "Equals", false, false, false, "Boolean");
                    break;
                }
            }
        }

        if (this._SelectedTabFilter.Code == "DEC") {
            switch (this._SelectedDECTabValue) {
                case "I": {
                    filters.addAdditionalFilter("CourierDeclarationStatusCode", "I", null, null, "Equals", false, false, false, "string");
                    break;
                }
                case "R": {
                    filters.addAdditionalFilter("CourierDeclarationStatusCode", "R", null, null, "Equals", false, false, false, "string");
                    break;
                }
                case "V": {
                    filters.addAdditionalFilter("CourierDeclarationStatusCode", "V", null, null, "Equals", false, false, false, "string");
                    break;
                }
                case "MX": {
                    filters.addAdditionalFilter("Is" + this._SelectedTabFilter.Code + "Tab", true, null, null, "Equals", false, false, false, "Boolean");
                    switch (this._SelectedDECValue) {
                        case "C": {
                            filters.addAdditionalFilter("CourierDeclarationStatusCode", "M", null, null, "Equals", false, false, false, "string");
                            break;
                        }
                        case "W": {
                            filters.addAdditionalFilter("CourierDeclarationStatusCode", "X", null, null, "Equals", false, false, false, "string");
                            break;
                        }
                    }
                    break;
                }
                default: {
                    filters.addAdditionalFilter("Is" + this._SelectedTabFilter.Code + "Tab", true, null, null, "Equals", false, false, false, "Boolean");
                    break;
                }
            }
        }

        if (this._SelectedTabFilter.Code == "DOC") {
            switch (this._SelectedDOCTabValue) {
                case "I": {
                    filters.addAdditionalFilter("DocumentStatusCode", "I", null, null, "Equals", false, false, false, "string");
                    break;
                }
                case "V": {
                    filters.addAdditionalFilter("DocumentStatusCode", "V", null, null, "Equals", false, false, false, "string");
                    break;
                }
                case "MX": {
                    filters.addAdditionalFilter("Is" + this._SelectedTabFilter.Code + "Tab", true, null, null, "Equals", false, false, false, "Boolean");
                    switch (this._SelectedDOCValue) {
                        case "C": {
                            filters.addAdditionalFilter("DocumentStatusCode", "M", null, null, "Equals", false, false, false, "string");
                            break;
                        }
                        case "U": {
                            filters.addAdditionalFilter("DocumentStatusCode", "X", null, null, "Equals", false, false, false, "string");
                            break;
                        }
                    }
                    break;
                }
                default: {
                    filters.addAdditionalFilter("Is" + this._SelectedTabFilter.Code + "Tab", true, null, null, "Equals", false, false, false, "Boolean");
                    break;
                }
            }
        }

        switch (this._SelectedTotalInvoiceValue) {
            case "75": {
                filters.addAdditionalFilter("TotalInvoiceAmountInUSD", 75, null, null, "LessThanOrEqual", false, false, false, "number");
                break;
            }
            case "500": {
                filters.addAdditionalFilter("TotalInvoiceAmountInUSD", 75.01, 500, null, "Between", false, false, false, "number", false);
                break;
            }
            case "1000": {
                filters.addAdditionalFilter("TotalInvoiceAmountInUSD", 500.01, 1000, null, "Between", false, false, false, "number", false);
                break;
            }
        }

        switch (this._SelectedAvailableValue) {
            case "AD": {
                filters.addAdditionalFilter("AcceptanceStatusCode", "2", null, null, "Equals", false, false, false, "string");
                break;
            }
            case "AV": {
                filters.addAdditionalFilter("AcceptanceStatusCode", "1", null, null, "Equals", false, false, false, "string");
                break;
            }
            case "NAV": {
                filters.addAdditionalFilter("AcceptanceStatusCode", true, null, null, "IsNull", false, false, false, "string");
                break;
            }
        }

        switch (this._SelectedACCValue) {
            case "W": {
                filters.addAdditionalFilter("StorageSiteStatusCode", "2", null, null, "Equals", false, false, false, "string");
                break;
            }
            case "WS": {
                filters.addAdditionalFilter("SpecialActionStatus", "X", null, null, "Equals", false, false, false, "string");
                break;
            }
        }

        if (this._SelectedTabFilter.Code == "PAY") {
            switch (this._SelectedPAYValue) {
                case "C": {
                    filters.addAdditionalFilter("CourierPaymentStatusCode", "R,P", null, null, "InList", false, false, false, "string");
                    break;
                }
                case "R": {
                    filters.addAdditionalFilter("Is" + this._SelectedTabFilter.Code + "Tab", true, null, null, "Equals", false, false, false, "Boolean");
                    break;
                }
                case "I": {
                    filters.addAdditionalFilter("CourierPaymentStatusCode", "I", null, null, "Equals", false, false, false, "string");
                    break;
                }
                default: {
                    filters.addAdditionalFilter("Is" + this._SelectedTabFilter.Code + "Tab", true, null, null, "Equals", false, false, false, "Boolean");
                    break;
                }
            }
        }

        if (this.SelectedPendingCodeFilter != null) {
            switch (this.SelectedPendingCodeFilter.Key) {
                case "A": {
                    break;
                }
                case "NotApproved": {
                    filters.addAdditionalFilter("NotApprovedPendingList", "", null, null, "NotEqual", true, false, false, "string");
                    break;
                }
                default: {
                    filters.addAdditionalFilter("CourierPendingReasonList", this.SelectedPendingCodeFilter.Key, null, null, "Contains", false, false, false, "string");
                    break;
                }
            }
        }
        if (!AppTool.IsNullOrEmpty(this.SearchFilter)) {
            filters.addAdditionalFilter("CourierSearchFields", this.SearchFilter.toLowerCase(), null, null, "Contains", false, false, false, "string", false, true);
        }
        if (AppTool.IsNullOrEmpty(filters.SortBy)) {
            filters.SortBy = "CourierHawb";
        }
        if (AppTool.IsNullOrEmpty(filters.SortDirection)) {
            filters.SortDirection = "Descending";
        }

        switch (this._SelectedFastIndividualProcessValue) {
            case "F": {
                filters.addAdditionalFilter("FastIndividualProcessCode", "F", null, null, "Equals", false, false, false, "string");
                break;
            }
            case "I": {
                filters.addAdditionalFilter("FastIndividualProcessCode", "I", null, null, "Equals", false, false, false, "string");
                break;
            }
        }

        switch (this._SelectedCustomStatusValue) {
            case "H": {
                filters.addAdditionalFilter("CourierCustomStatusCode", "1", null, null, "Equals", false, false, false, "string");
                break;
            }
            case "S": {
                filters.addAdditionalFilter("CourierCustomStatusCode", "2", null, null, "Equals", false, false, false, "string");
                break;
            }
            case "N": {
                filters.addAdditionalFilter("CourierCustomStatusCode", "2", "1", null, "NotEqual", false, false, false, "string");



                break;
            }
        }

        if (this._SelectedFinalReleaseValue !== 'A')
            filters.addAdditionalFilter("FinalRelease", 'is not change what write', null, null, this._SelectedFinalReleaseValue === 'Y' ? 'Equal' : 'NotEqual', false, false, false, "string");
        if (this._SelectedDelivered == 'A') {
            filters.addAdditionalFilter("Delivered", false, null, null, "Equals", false, false, false, "Boolean");
        }

    }


    MNFFilterClicked(value: string) {

        if (this._SelectedMNFValue != value) {
            this._SelectedMNFValue = value;
            this.RefreshList();
        }
    }

    MNFTABFilterClicked(value: string) {
        if (this._SelectedMNFTABValue != value) {
            this._SelectedMNFTABValue = value;
            this.RefreshList();
        }
    }

    DECFilterClicked(value: string) {

        if (this._SelectedDECValue != value) {
            this._SelectedDECValue = value;
            this.RefreshList();
        }
    }

    DECTabFilterClicked(value: string) {
        if (this._SelectedDECTabValue != value) {
            this._SelectedDECTabValue = value;
            this.RefreshList();
        }
    }

    DOCFilterClicked(value: string) {

        if (this._SelectedDOCValue != value) {
            this._SelectedDOCValue = value;
            this.RefreshList();
        }
    }

    DOCTabFilterClicked(value: string) {
        if (this._SelectedDOCTabValue != value) {
            this._SelectedDOCTabValue = value;
        }
    }

    ACCFilterClicked(value: string) {

        if (this._SelectedACCValue != value) {
            this._SelectedACCValue = value;
            this.RefreshList();
        }
    }

    PAYFilterClicked(value: string) {

        if (this._SelectedPAYValue != value) {
            this._SelectedPAYValue = value;
            this.RefreshList();
        }
    }

    GetPending() {
        this._PendingCodes.length = 0;
        this._PendingCodes.push({ 'Key': "A", 'Value': TextCodeTranslator.Translate("Customs.General.O.All") });
        if (this.HasRequiresApprovalFeature) {
            this._PendingCodes.push({ 'Key': "NotApproved", 'Value': TextCodeTranslator.Translate("Customs.CourierPendingReason.O.NotApprovedPending") });
        }
        SessionLocator.SelectedSession.StartBusyIndicatorCreating();
        this._CourierMasterService.GetPending(this.entityPM?.Id)
            .subscribe((resu: any) => {
                SessionLocator.SelectedSession.StopBusyIndicator();
                var list: string[];
                var itemPname = null;
                list = resu.Result;
                list.forEach(itemP => {
                    itemPname = this.getCourierPendingReasonName(itemP);
                    this._PendingCodes.push({ 'Key': itemP, 'Value': itemPname });
                });
                if (AppTool.IsNullOrEmpty(this.PendingFilter)) this.PendingFilter = "A";
                if (!AppTool.IsNullOrEmpty(this.PendingFilter) && this._PendingCodes != null && this._PendingCodes.length > 0) {
                    var tempPendingCodeFilter = this._PendingCodes.find(r => r.Key == this.PendingFilter);
                    if (!AppTool.IsNullOrEmpty(tempPendingCodeFilter)) {
                        this.SelectedPendingCodeFilter = tempPendingCodeFilter;
                    }
                    else {
                        this.SelectedPendingCodeFilter = this._PendingCodes[0];
                    }
                    //this.RefreshList();
                    this.RefreshButtonClicked();
                }
            });
    }


    getCourierPendingReasonName(courierPendingReason: string) {
        var toolTip = courierPendingReason;
        if (!AppTool.IsNullOrEmpty(toolTip) && toolTip.indexOf(',') < 0) {

            var myCourierPendingReasonListService = new CourierPendingReasonListService();
            myCourierPendingReasonListService.getSingleFromCache(toolTip)
                .subscribe(serviceResponse => {
                    var CourierPendingReason = serviceResponse.Result as CourierPendingReasonList;
                    toolTip = CourierPendingReason.LocalName;
                });
        }
        return toolTip;
    }

    HOLDFilterClicked(value) {

        if (this.SelectedPendingCodeFilter != value) {
            this.SelectedPendingCodeFilter = value;
            this.RefreshList();
        }
    }
    /*
    _PendingCodesFilter: KeyValuePair;
    get _SelectedHOLDValue() {
        if (this._PendingFilter == "A" && this._PendingCodes != null) {
            this._PendingCodesFilter = this._PendingCodes[0];
        } else {
            if (this._PendingCodes!= null)this._PendingCodesFilter = this._PendingCodes.find(r => r.Key == this._PendingFilter);
        }
        return this._PendingCodesFilter;
    }
    set _SelectedHOLDValue(value) {
        if (this._PendingCodesFilter != value) {
            this._PendingCodesFilter = value;
            if (this._PendingCodesFilter != null)this._PendingFilter = this._PendingCodesFilter.Key;
        }
    }
    */
    _SelectedPendingCodeFilter: KeyValuePair;
    get SelectedPendingCodeFilter() {
        if (this.PendingFilter == "A" && this._PendingCodes != null) {
            this._SelectedPendingCodeFilter = this._PendingCodes[0];
        } else {
            if (!AppTool.IsNullOrEmpty(this.PendingFilter) && this._PendingCodes != null && this._PendingCodes.length > 0) this._SelectedPendingCodeFilter = this._PendingCodes.find(r => r.Key == this.PendingFilter);
        }
        return this._SelectedPendingCodeFilter;
    }
    set SelectedPendingCodeFilter(value) {
        if (this._SelectedPendingCodeFilter != value) {
            this._SelectedPendingCodeFilter = value;
            if (this._SelectedPendingCodeFilter != null) this.PendingFilter = this._SelectedPendingCodeFilter.Key;
        }
    }

    SelectedBOLValueClick(value: string) {
        this._SelectedBOLValue = value;
        this.selectedFilterClick();
    }

    SelectedTotalInvoiceValue(value: string) {
        this._SelectedTotalInvoiceValue = value;
        this.selectedFilterClick();
    }

    SelectedStatusValueClick(value: string) {
        this._SelectedStatusValue = value;
        this.selectedFilterClick();
    }

    SelectedAvailableValueClick(value: string) {
        this._SelectedAvailableValue = value;
        this.selectedFilterClick();
    }

    SelectedFastIndividualProcessValueClick(value: string) {
        this._SelectedFastIndividualProcessValue = value;
        this.selectedFilterClick();
    }

    SelectedCustomStatusValueClick(value: string) {
        this._SelectedCustomStatusValue = value;
        this.selectedFilterClick();
    }

    SelectedFinalReleaseValueClick(value: string) {
        this._SelectedFinalReleaseValue = value;
        this.selectedFilterClick();
    }

    selectedFilterClick() {
        this.IsFiltered = [
            this._SelectedBOLValue,
            this._SelectedTotalInvoiceValue,
            this._SelectedStatusValue,
            this._SelectedAvailableValue,
            this._SelectedFastIndividualProcessValue,
            this._SelectedCustomStatusValue,
            this._SelectedFinalReleaseValue
        ].some(filter => filter !== 'A');

        this.RefreshList();
    }

    FilterCancelButtonClicked() {
        this.MyDropdownMenuFilterComponent.DropdowndisplayToggle(null);
        this.RefreshList();
    }

    FilterCleanButtonClicked() {
        this._SelectedBOLValue = 'A';
        this._SelectedStatusValue = 'A';
        this._SelectedAvailableValue = 'A';
        this._SelectedTotalInvoiceValue = 'A';
        this._SelectedFastIndividualProcessValue = 'A';
        this._SelectedCustomStatusValue = 'A';
        this._SelectedFinalReleaseValue = 'A';
        this.IsFiltered = false;
        this.RefreshList();
    }

    public MyScrollTop: number = 0;
    OnRowSelected(event) {
        this.MyScrollTop = event.scrollTop;
        if (this._CourierWorksheetSharedDataService.SupperssOnRowSelectedAction) {
            this._CourierWorksheetSharedDataService.SupperssOnRowSelectedAction = false;
            return;
        }
        this.OnRowSelectedBL(event);

        //let timerToken = setTimeout(() => {
        //    this.preventSelect = false;
        //    clearTimeout(timerToken);
        //    this.OnRowSelectedBL(event);
        //}, 1000);
    }

    preventSelect: boolean = false;
    OnRowSelectedBL(event) {
        if (this.preventSelect) {
            return;
        }
        if (true)//(!this.preventSelect) {
            var selected = event.rowData;

        if (selected) {
            var customEditIdentityKey = Guid.newGuid();
            var control = null;
            var logitudeWindow = new LogitudeWindow();
            var currentScreenCode = "";
            var objectTableName = "";
            var filters: any[] = [];

            switch (this._SelectedTabFilter.Code) {
                case "MNF":
                    {
                        if (this._SelectedMNFValue == 'C') {
                            currentScreenCode = "DEGC";
                            objectTableName = "Customs.Declaration";
                        }
                        else if (this._SelectedMNFValue == 'W') {
                            currentScreenCode = "DCCA";
                            objectTableName = "Customs.Declaration";
                            let myfilters = {
                                "IsManifest": true,
                                "IsConstraintsVisible": false,
                            };
                            filters.push(myfilters);
                        }
                        break;
                    }
                case "DEC":
                    {
                        if (this._SelectedDECValue == 'C') {
                            currentScreenCode = "DEGC";
                            objectTableName = "Customs.Declaration";
                        }
                        else if (this._SelectedDECValue == 'W') {
                            currentScreenCode = "DCCA";
                            objectTableName = "Customs.Declaration";
                        }
                        break;
                    }
                case "DOC":
                    {
                        currentScreenCode = "DCCD";
                        objectTableName = "Customs.Declaration";
                        break;
                    }
                case "SVG":
                    {
                        currentScreenCode = "DCCF";
                        objectTableName = "Customs.Declaration";
                    }
                    break;
                default:
                    {
                        currentScreenCode = "DEGC";
                        objectTableName = "Customs.Declaration";
                        break;
                    }
            }

            if (!AppTool.IsNullOrEmpty(currentScreenCode)) {

                if (objectTableName == "Customs.Declaration") {

                    if (currentScreenCode == "DEGC" && selected.IsAmendment == true) currentScreenCode = "DCCR";

                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.SelectedSession.SessionLocation.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run({
                                SelectedTabCode: currentScreenCode,
                                EntityId: selected.DeclarationId,
                                ObjectTableName: objectTableName,
                            });
                            cmpRef.instance.BackCompleted.subscribe(bk => {
                                this.OnBackFromEdit(selected.DeclarationId, event);
                            });
                            if (this._SelectedTabFilter.Code == "MNF" && this._SelectedMNFValue == 'W') {
                                cmpRef.instance.OnFirstTimeAfterSingleDataLoaded
                                    .subscribe(myResult => {
                                        var myDeclarationEditComponentController = cmpRef.instance.EditComponentController as DeclarationEditComponentController;
                                        myDeclarationEditComponentController.CustomsAnswersShowManifest = true;
                                        console.log("myDeclarationEditComponentController.CustomsAnswersShowManifest = true;");
                                    });
                            }
                            if (currentScreenCode = "DCCF") {
                                cmpRef.instance.OnFirstTimeAfterSingleDataLoaded
                                    .subscribe((myResult: any) => {
                                        var myDeclarationEditComponentController = cmpRef.instance.EditComponentController as DeclarationEditComponentController;
                                        myDeclarationEditComponentController.ShowDeclarationClassificationComponentTAB = true;
                                        console.log("myDeclarationEditComponentController.DeclarationClassificationComponent = true;");
                                    });
                            }


                        });

                    //this.preventSelect = false;
                    return;
                }
            }
        }
    }

    OnBackFromEdit(selectedEntityId, $event) {
        this.MyScrollTop = $event.scrollTop;
        this.RefreshButtonClicked();
    }


    DeclarationsStatusRequestMethod() {

        SessionLocator.SelectedSession.StartBusyIndicatorCreating();
        this._CourierMasterService.GetSendALLDeclarationsStatusRequest(this.entityPM?.Id, null, true,SessionLocator.LoggedUserId)
            .subscribe((res: any) => {
                this.currentSession.StopBusyIndicator();
                var myMessageWindow = new MessageWindow();
                myMessageWindow.Show(res.Result);
                //this.RefreshButtonClicked();
            });
    }

    ImportCourierMawbsFromExcel() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = "הטענת םקסל למסך עבודה";
        logitudeWindow.ShowCloseButton = true;
        logitudeWindow.Height = 600;
        logitudeWindow.Width = 700;
        logitudeWindow.Show('./CustomsModules/CustomsCourier/Components/CourierWorkSheet/ImportCourierMawbsFromExcel/ImportCourierMawbsFromExcelComponent')
        logitudeWindow.WindowClosed.subscribe((event: any) => {
            this.CourierHawbsFromExcelUploaded = true;
            this.RefreshButtonClicked();
        });

    }


    SendALLTerminal() {
        var currRequestParams = new SendALLCorrectRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.IsWorkSheetFromExcel = this.CourierHawbsFromExcelUploaded;


        this._CourierMasterService.PostSendALLTerminal(currRequestParams)
            .subscribe((res: any) => {
                SessionLocator.SelectedSession.StopBusyIndicator();
                var myMessageWindow = new MessageWindow();
                myMessageWindow.Show(res.Result);
                myMessageWindow.WindowClosed.subscribe(s => {
                    this.RefreshButtonClicked();
                });
            });

    }

    SendDelayForm() {

        var titleText = "הפקת תעודת עיכוב";
        var questionText = "םשר שליחת מסר פעולה מיוחדת של תעודת עיכוב למסוף";
        var confirm = new ConfirmWindow();
        confirm.Width = 350;
        confirm.Height = 200;
        confirm.Title = titleText;
        confirm.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");
        confirm.ShowNoButton = true;
        confirm.Show(questionText);
        confirm.WindowClosed.subscribe((event: any) => {
            if (confirm.Yes) {
                SessionLocator.SelectedSession.StartBusyIndicator("");
                var currRequestParams = new SendALLDelayFormParams();
                currRequestParams.LoggingEnabled = true;
                currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
                currRequestParams.Tenant = SessionLocator.Tenant;
                currRequestParams.CourierMasterId = this.entityPM?.Id;
                currRequestParams.HAWB = this.entityPM?.HAWB;
                if (this._CourierWorksheetSharedDataService._SelectedItems != null && this._CourierWorksheetSharedDataService._SelectedItems.Collection.length > 0) {
                    currRequestParams.Declarations = this._CourierWorksheetSharedDataService._SelectedItems.Collection;
                }

                this._CourierMasterService.PostSendDelayFormForDeclarations(currRequestParams)
                    .subscribe((res: any) => {
                        this.currentSession.StopBusyIndicator();
                        var myMessageWindow = new MessageWindow();
                        myMessageWindow.Show(res.Result);
                        myMessageWindow.WindowClosed.subscribe(s => {
                            this.RefreshButtonClicked();
                        });
                    });
            }
            confirm.Close();
        });

    }

    private GetIsSendDocumentsFromQueueButton() {

        var myInterfaceTenantDefinitionPMService = new InterfaceTenantDefinitionsWebService();
        myInterfaceTenantDefinitionPMService.get(SessionLocator.Tenant, "2715")
            .subscribe((response: any) => {
                this.IsSendDocumentsFromQueueButton = false;
                if (!response.HasError) {
                    if (response.Result != null && !AppTool.IsNullOrEmpty(response.Result.body.SendTime)) {
                        this.IsSendDocumentsFromQueueButton = true;
                    }
                }
            });
    }
    private GetMamanPUR() {//ILMMN;ILOVL
        var myCustomsSettingExtendedListService = new CustomsSettingExtendedListService();
        myCustomsSettingExtendedListService.GetDefault("ISRAEL", "CGO_CUST_MAMAN", "NON", "NON", SessionLocator.Tenant)
            .subscribe((response: any) => {
                this.IsMamanEnabled = false;
                if (!response.HasError) {// reEdit this default !!!
                    if (response.Result != null && response.Result.DefaultValue != null) {
                        if (response.Result.DefaultValue.includes("ILMMN")) {
                            this.IsMamanEnabled = true;
                        }
                        if (response.Result.DefaultValue.includes("ILOVL")) {
                            this.IsILOVLEnabled = true;
                        }
                        if (response.Result.DefaultValue.includes("ILSWS")) {
                            this.IsILSWSEnabled = true;
                        }
                        this._CourierWorksheetSharedDataService.WebAPICourierGWMessageECTHRDataMaman = response.Result.DefaultValue;
                    } else {
                        console.error("CGO_CUST_MAMAN DefaultValue  is missing !!!")
                    }
                }
            });
    }


    ClosePendingMethod() {
        if (this.IsDisplayOnly) {
            var myMessageWindow = new MessageWindow();
            myMessageWindow.Width = 250;
            myMessageWindow.Height = 150;
            myMessageWindow.Show("קיים מסר זהה בתהליך");
            SessionLocator.SelectedSession.StopBusyIndicator();
            return;
        }
        var currRequestParams = new PendingRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.CourierMasterId = this.entityPM?.Id;
        currRequestParams.MAWB = this.entityPM?.MAWB;
        let text = "נם םשר מחיקת קוד עיכוב";
        if (this._CourierWorksheetSharedDataService._SelectedItems != null && this._CourierWorksheetSharedDataService._SelectedItems.Collection.length > 0) {
            currRequestParams.DeclarationsList = this._CourierWorksheetSharedDataService._SelectedItems.Collection;
        }
        currRequestParams.PendingCode = new Array();
        if (this.SelectedPendingCodeFilter.Key == 'A' && this._PendingCodes.length > 1) {
            this._PendingCodes.forEach(x => currRequestParams.PendingCode.push(x.Key));
        } else {
            currRequestParams.PendingCode.push(this.SelectedPendingCodeFilter.Key);
        }
        //currRequestParams.PendingCode=this.SelectedPendingCodeFilter.Key;
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show(text);
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                SessionLocator.SelectedSession.StartBusyIndicatorLoading();
                this._CourierMasterService.PostSendClosePending(currRequestParams)

                    .subscribe((res: any) => {

                        SessionLocator.SelectedSession.StopBusyIndicator();
                        var myMessageWindow = new MessageWindow();
                        var myMessageWindow = new MessageWindow();
                        myMessageWindow.Show(res.Result);


                        myMessageWindow.WindowClosed.subscribe(s => {
                            this.RefreshButtonClicked();

                        });
                    });


            }
        });

    }
    ApproveAllPendingMethod() {
        if (this.IsDisplayOnly) {
            var myMessageWindow = new MessageWindow();
            myMessageWindow.Width = 250;
            myMessageWindow.Height = 150;
            myMessageWindow.Show("קיים מסר זהה בתהליך");
            SessionLocator.SelectedSession.StopBusyIndicator();
            return;
        }
        var currRequestParams = new PendingRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.CourierMasterId = this.entityPM?.Id;
        currRequestParams.MAWB = this.entityPM?.MAWB;
        let text = "הםם לםשר םת כל Pending שלם םושרו בטיסה";

        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show(text);
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                SessionLocator.SelectedSession.StartBusyIndicatorLoading();
                this._CourierMasterService.PostApproveAllPending(currRequestParams)

                    .subscribe((res: any) => {

                        SessionLocator.SelectedSession.StopBusyIndicator();
                        var myMessageWindow = new MessageWindow();
                        var myMessageWindow = new MessageWindow();
                        myMessageWindow.Show(res.Result);


                        myMessageWindow.WindowClosed.subscribe(s => {
                            this.RefreshButtonClicked();

                        });
                    });


            }
        });

    }

    _CourierMasterPMService: CourierMasterPMService = new CourierMasterPMService();

    Export2Excel() {
        //this._IsDisableToggle = !this._IsDisableToggle;




        //communicationLogStepListService.GetExportExcelByRequestId("8305", this.MyLastCustomsRequestSheetId, SessionLocator.Tenant);
        //http://localhost:9996/api/CourierMaster/GetExportCourierMaster2Excel?CourierMasterId=1-3333&tenant=1
        var url = ServiceHelper.GetLogitudeURL() + 'api/CourierMaster/GetExportCourierMaster2Excel?' + 'CourierMasterId=' + this.entityPM?.Id + '&tenant=' + SessionLocator.Tenant.toString() + '&userId=' + SessionLocator.LoggedUserId + '&IsWorkSheetFromExcel=' + this.CourierHawbsFromExcelUploaded;


        window.open(url);

    }

    OpenCourierMaster(text: string) {
    }

    UpdateIsReadyForInvoice() {
        SessionLocator.SelectedSession.StartBusyIndicatorSaving();
        this.entityPM.IsReadyForInvoice = !this.entityPM.IsReadyForInvoice;
        this._CourierMasterPMService
            .update(this.entityPM)
            .subscribe((response: ServiceResponse) => {
                SessionLocator.SelectedSession.StopBusyIndicator();
                if (response.HasError) {
                    var mess
                } else {
                    this.entityPM = response.Result;
                }
            });
    }


    DisplayOnlyCheckDeletePending() {
        /*this.IsDisplayOnly = false;
        this._CourierWorksheetSharedDataService.IsDisplayOnly = false;

        //Check if deleting pending
        this._CourierMasterValidator.SetEntityPM(this.entityPM);
        this._CourierMasterValidator.CheckRequestInProgressForCourierMaster(this.entityPM.Tenant, "ClosePending", this.entityPM.Id).subscribe((response: any) => {
            var displayOnlyCheckResult = response.Result;
            if (displayOnlyCheckResult != null && displayOnlyCheckResult.length > 0) {
                let customsRequestsSheetPM: CustomsRequestsSheetPM = displayOnlyCheckResult.filter(r => r.InterfaceTypeCode == "ClosePending")[0];
                if (customsRequestsSheetPM != null) {
                    this.IsDisplayOnly = true;
                    this.DisplayOnlyMessage = "לתצוגה בלבד - קיימת בקשה לסגירת PENDING ברקע ";
                    this._CourierWorksheetSharedDataService.IsDisplayOnly = true;
                }
            }
        });*/
    }

    DisplayOnlyCheckApprovePending() {
        /*
        this.IsDisplayOnly = false;
        this._CourierWorksheetSharedDataService.IsDisplayOnly = false;

        //Check if deleting pending
        this._CourierMasterValidator.SetEntityPM(this.entityPM);
        this._CourierMasterValidator.CheckRequestInProgressForCourierMaster(this.entityPM.Tenant, "DCAInUCBApproveAllPending", this.entityPM.Id).subscribe((response: any) => {
            var displayOnlyCheckResult = response.Result;
            if (displayOnlyCheckResult != null && displayOnlyCheckResult.length > 0) {
                let customsRequestsSheetPM: CustomsRequestsSheetPM = displayOnlyCheckResult.filter(r => r.InterfaceTypeCode == "DCAInUCBApproveAllPending")[0];
                if (customsRequestsSheetPM != null) {
                    this.IsDisplayOnly = true;
                    this.DisplayOnlyMessage = "לתצוגה בלבד - קיימת בקשה לםישור PENDING ברקע ";
                    this._CourierWorksheetSharedDataService.IsDisplayOnly = true;
                }
            }
        });*/
    }

    DisplayOnlyCheck() {
        this.IsDisplayOnly = false;
        this._CourierWorksheetSharedDataService.IsDisplayOnly = false;

        //Check if changing StorageSiteCode
        // this._CourierMasterValidator.SetEntityPM(this.entityPM);
        /*this._CourierMasterValidator.CheckRequestInProgressForCourierMaster(SessionLocator.Tenant, "UCBCMSS", this.entityPM.Id).subscribe((response: any) => {
            var displayOnlyCheckResult = response.Result;
            if (displayOnlyCheckResult != null && displayOnlyCheckResult.length > 0) {
                let customsRequestsSheetPM: CustomsRequestsSheetPM = displayOnlyCheckResult.filter(r => r.InterfaceTypeCode == "UCBCMSS")[0];
                if (customsRequestsSheetPM != null) {
                    this.IsDisplayOnly = true;
                    this.DisplayOnlyMessage = "לתצוגה בלבד - קיימת בקשה לשינוי םתר םחסון/פריקה ברקע ";
                    this._CourierWorksheetSharedDataService.IsDisplayOnly = true;
                }
            }
        });*/
    }

    SendUncorrectDocuments(sendMode: string) {

        if (this._DOC_U_Total == 0) {
            var myMessageWindow = new MessageWindow();
            myMessageWindow.Width = 250;
            myMessageWindow.Height = 150;
            myMessageWindow.Show(TextCodeTranslator.Translate("Customs.CourierMaster.O.NoResults"));
            return;
        }

        var currRequestParams = new SendUnCorrectDocumentsRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.CourierMasterId = this.entityPM?.Id;
        currRequestParams.HAWB = this.entityPM?.HAWB;
        /*if (this._CourierWorksheetSharedDataService._SelectedItems != null && this._CourierWorksheetSharedDataService._SelectedItems.Collection.length > 0) {
            currRequestParams.Declarations = this._CourierWorksheetSharedDataService._SelectedItems.Collection;
        }*/

        currRequestParams.SelectedAvailableValue = this._SelectedAvailableValue;
        currRequestParams.SelectedBOLValue = this._SelectedBOLValue;
        currRequestParams.SelectedStatusValue = this._SelectedStatusValue;
        currRequestParams.SelectedTotalInvoiceValue = this._SelectedTotalInvoiceValue;
        currRequestParams.SelectedFastIndividualProcessValue = this._SelectedFastIndividualProcessValue;
        currRequestParams.SelectedCustomStatusValue = this._SelectedCustomStatusValue;
        currRequestParams.SelectedFinalReleaseValue = this._SelectedFinalReleaseValue;
        if (sendMode == 'VX') currRequestParams.IsCreateNewDocumentVersion = true;

        this._CourierMasterService.PostSendUnCorrectDocuments(currRequestParams)
            .subscribe((res: any) => {
                SessionLocator.SelectedSession.StopBusyIndicator();
                var myMessageWindow = new MessageWindow();
                myMessageWindow.Show(res.Result);
                myMessageWindow.WindowClosed.subscribe(s => {
                    this.RefreshButtonClicked();
                });
            });
    }

    SendDocumentsFromQueue() {


        this._CourierMasterService.GetSendDocumentsFromQueue(this.entityPM?.Id, this.entityPM?.MAWB)
            .subscribe((res: any) => {
                SessionLocator.SelectedSession.StopBusyIndicator();
                var myMessageWindow = new MessageWindow();
                myMessageWindow.Show(res.Result);
                myMessageWindow.WindowClosed.subscribe(s => {
                    this.RefreshButtonClicked();
                });
            });
    }


    OpenFlight() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show(TextCodeTranslator.Translate("Customs.CourierMaster.O.SureToOpenCancel"));
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.entityPM.IsCancelled = false;

                SessionLocator.SelectedSession.StartBusyIndicatorSaving();
                this._CourierMasterPMService
                    .update(this.entityPM)
                    .subscribe((response: ServiceResponse) => {
                        SessionLocator.SelectedSession.StopBusyIndicator();
                        if (response.HasError) {
                            var mess
                        } else {
                            this.entityPM = response.Result;
                        }
                    });

            }
        });
    }


    openBulkFeedPending() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 1600;
        logitudeWindow.Height = 800;
        logitudeWindow.IsShowCloseButton = true;
        logitudeWindow.Title = "עדכון גורף";
        logitudeWindow.WindowArgs = { CourierMasterPM: this.entityPM, IsWorkSheetFromExcel: this.CourierHawbsFromExcelUploaded };
        logitudeWindow.Show('./CustomsModules/CustomsCourier/Components/CourierWorkSheet/bulk-feed-pending/BulkFeedPendingComponent');
        this.ChangedUnloadPortSite = true;
        logitudeWindow.WindowClosed.subscribe(($event: any) => this.RefreshButtonClicked());
    }
}


export class KeyValuePair {
    constructor(public Key: string, public Value) { }
}

export class TabFilter {

    constructor(public Code: string, public Header: string, public Total?: number, public Filter?: string) {
    }
}
