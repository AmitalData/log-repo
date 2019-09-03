import {Component, Output, EventEmitter , OnInit , AfterViewInit} from '@angular/core';
import {LogitudeWindow} from '../../Controls/Windows/LogitudeWindow';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {InfraSettings} from '../../Infrastructure/Utilities/InfraSettings';
import {ListComponentArgs} from '../../Infrastructure/Args';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../Infrastructure/Services/EntityResourceService';
import { CustomsSettingExtendedListService } from '../../Customs/Services/ExtendedLists/CustomsSettingExtendedListService';
import { AppTool, DateTool } from '../../Infrastructure/Tools';
import { BaseComponent } from '../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../Infrastructure/DataContracts/EntityArgs';
import { CourierMasterPM } from '../../Customs/EntityPMs/CourierMasterPM';
import { CourierMasterService } from '../../Customs/Services/Others/CourierMasterService';
import { CourierMasterValidator } from '../../Customs/Validators/CourierMasterValidator';
import { TextCodeTranslator } from '../../Infrastructure/Utilities/TextCodeTranslator';
import { ApiQueryFilters } from '../../Infrastructure/DataContracts/ApiQueryFilters';
import { DeclarationCourierStatusListService } from '../../Customs/Services/StandardLists/DeclarationCourierStatusListService';
import { EntityListService } from '../../Infrastructure/Services/EntityListService';
import { MessageWindow } from '../../Controls/Windows/MessageWindow';
import { DeclarationEditComponentController } from '../../Customs/Controller/DeclarationEditComponentController';
import { ObservableCollection } from '../../Infrastructure/Utilities/ObservableCollection';
import { CourierWorksheetSharedDataService } from '../../Customs/Services/DataChange/CourierWorksheetSharedDataService';
import { DeclarationCourierStatusList } from '../../Customs/EntityLists/DeclarationCourierStatusList';
import { CustomsRequestsSheetPM } from '../../Customs/EntityPMs/CustomsRequestsSheetPM';
import { ObjectsLocator } from '../../Infrastructure/Locators/ObjectsLocator';
import { DeclarationExtendedListService } from '../../Customs/Services/ExtendedLists/DeclarationExtendedListService';

@Component({
    moduleId: module.id,
    templateUrl: './CourierDeclarationWorkspaceComponent.html',

})

export class CourierDeclarationWorkspaceComponent implements AfterViewInit {
    @Output() ReloadUserQueries = new EventEmitter();
    public RecentGLAccountsCount: number = 0;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private _DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    _CourierMasterService: CourierMasterService = new CourierMasterService();

    // Queries Features
    public OpenCourierMasterVisibility: boolean = false;
    public InactiveGLAccountsVisibility: boolean = false;
    public AllGLAccountsVisibility: boolean = false;
    public OpenFilesVisibility: boolean = false;
    public ClosedFilesVisibility: boolean = false;
    public AllFilesVisibility: boolean = false;
    public AllJobsVisibility: boolean = false;
    public isRTL: boolean = false;
    public isScreenLoaded: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.LoadAllScreenData();
        this.CurrentSession.StartBusyIndicatorLoading();
        this._entityResourceService.getEntityResourceByTableName("Declaration").subscribe((response: any) => {
            {
                this.isScreenLoaded = true;
                this.CurrentSession.StopBusyIndicator();
            }
        });

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    }
    ngAfterViewInit() {
        this.LoadAllScreenData();
    }
    public IsQueryVisible_MyViewsGroup: boolean = false;

    InitComponent() {
        this.LoadAllScreenData();
        this.SetQueriesVisibility();
    }

    RefreshButtonClicked() {
        this.LoadAllScreenData();
    }

    public LoadAllScreenData() {
        this.LoadQueriesCounts();
        //this.LoadRecentGLAccounts();
        this.ReloadUsersQuery();
    }

    SetQueriesVisibility() {
        this.OpenCourierMasterVisibility = FeatureLocator.HasFeaturePermession("GLAccount", "ACTIVEGLACCOUNTS") ? true : false;
        this.InactiveGLAccountsVisibility = FeatureLocator.HasFeaturePermession("GLAccount", "INACTIVEGLACCOUNTS") ? true : false;
        this.AllGLAccountsVisibility = FeatureLocator.HasFeaturePermession("GLAccount", "ALLGLACCOUNTS") ? true : false;
        this.OpenFilesVisibility = FeatureLocator.HasFeaturePermession("GLAccount", "OPENFILESGLACCOUNTS") ? true : false;
        this.ClosedFilesVisibility = FeatureLocator.HasFeaturePermession("GLAccount", "CLOSEDFILESGLACCOUNTS") ? true : false;
        this.AllFilesVisibility = FeatureLocator.HasFeaturePermession("GLAccount", "ALLFILESGLACCOUNTS") ? true : false;
        this.AllJobsVisibility = FeatureLocator.HasFeaturePermession("GLAccount", "ALLJOBSGLACCOUNTS") ? true : false;

        
    }

    ReloadUsersQuery() {
        this.ReloadUserQueries.emit();
    }

    onUserQueriesBackComplete(event) {
        this.LoadAllScreenData();
    }

    LoadQueriesCounts() {
        /*
        this._CourierMasterService.GetSummary().subscribe(myResult => {
            if (myResult != null) {
                this.glAccountSummary.ActiveGLAccountCount = myResult.ActiveGLAccountCount > 1000 ? "1000+" : myResult.ActiveGLAccountCount.toString();
                this.glAccountSummary.InactiveGLAccountCount = myResult.InactiveGLAccountCount > 1000 ? "1000+" : myResult.InactiveGLAccountCount.toString();
                this.glAccountSummary.AllGLAccountCount = myResult.AllGLAccountCount > 1000 ? "1000+" : myResult.AllGLAccountCount.toString();
                this.glAccountSummary.OpenFilesCount = myResult.OpenFilesCount > 1000 ? "1000+" : myResult.OpenFilesCount.toString();
                this.glAccountSummary.ClosedFilesGLAccountCount = myResult.ClosedFilesGLAccountCount > 1000 ? "1000+" : myResult.ClosedFilesGLAccountCount.toString();
                this.glAccountSummary.AllFilesCount = myResult.AllFilesCount > 1000 ? "1000+" : myResult.AllFilesCount.toString();
                this.glAccountSummary.AllJobsCount = myResult.AllJobsCount > 1000 ? "1000+" : myResult.AllJobsCount.toString();
            }
        });
        */
    }

    EditCourierMaster(entity: any) {
        if (entity != null) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'GLAccount', BackButtonLabel: TextCodeTranslator.Translate("Accounting.General.O.Main") });
                    cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                        this.RefreshButtonClicked();
                    });
                });
        }
    }

    RunNewGLAccountWizard() {
        var windowTitle = TextCodeTranslator.Translate("Accounting.General.O.NewAccount"); // "New Account";
        //var windowArgs: BookingWizardArgs = new BookingWizardArgs();
        //windowArgs.IsNewEntity = true;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = windowTitle;
        //logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.LoadAllScreenData());
        logWindow.Show('./Accounting/Components/NewEntity/NewGLAccountComponent');
    }

    ViewAccountingQuery(myQueryCode: string) {
        if (myQueryCode != null) {

            var displayTitle = "";
            var queryCode = myQueryCode;
            queryCode = "All GLAccounts";
            var filters = new ApiQueryFilters();
            switch (myQueryCode) {
                case "OpenCourierMaster":
                    {
                        displayTitle = "Open Courier Master";
                        displayTitle = TextCodeTranslator.Translate("Customs.CourierMaster.O.CourierMasterOpen");

                        //filters.addAdditionalFilter("AccountTypeCode", "1", null, null, "Equals", false, false, false, "string");
                        //filters.addAdditionalFilter("Inactive", false, null, null, "Equals", false, false, false, "boolean");

                        break;
                    }
                case "InactiveGLAccounts":
                    {
                        displayTitle = "Inactive GLAccounts";
                        displayTitle = TextCodeTranslator.Translate("GLAccounts.Q.InActiveGLAccounts");
                        //filters.addAdditionalFilter("AccountTypeCode", "1", null, null, "Equals", false, false, false, "string");
                        //filters.addAdditionalFilter("Inactive", true, null, null, "Equals", false, false, false, "string");

                        break;
                    }
                case "All GLAccounts":
                    {
                        displayTitle = "All GLAccounts";
                        displayTitle = TextCodeTranslator.Translate("GLAccounts.Q.AllGLAccounts");
                        //filters.addAdditionalFilter("AccountTypeCode", "1", null, null, "Equals", false, false, false, "string");

                        break;
                    }
                case "OpenFiles":
                    {
                        displayTitle = "Open Files";
                        displayTitle = TextCodeTranslator.Translate("GLAccounts.Q.OpenFiles");
                        //filters.addAdditionalFilter("AccountTypeCode", "5", null, null, "Equals", false, false, false, "string");
                        //filters.addAdditionalFilter("BalanceInLocalCurrency", "0", null, null, "NotEqual", true, false, false, "decimal");

                        break;
                    }
                case "ClosedFiles":
                    {
                        displayTitle = "Closed Files";
                        displayTitle = TextCodeTranslator.Translate("GLAccounts.Q.ClosedFiles");
                        //filters.addAdditionalFilter("AccountTypeCode", "5", null, null, "Equals", false, false, false, "string");
                        //filters.addAdditionalFilter("BalanceInLocalCurrency", "0", null, null, "Equals", true, false, false, "decimal");

                        break;
                    }
                case "AllFiles":
                    {
                        displayTitle = "All Files";
                        displayTitle = TextCodeTranslator.Translate("GLAccounts.Q.AllFiles");
                        //filters.addAdditionalFilter("AccountTypeCode", "5", null, null, "Equals", false, false, false, "string");

                        break;
                    }
                case "AllJobs":
                    {
                        displayTitle = "All Jobs";
                        displayTitle = TextCodeTranslator.Translate("GLAccounts.Q.AllJobs");
                        //filters.addAdditionalFilter("AccountTypeCode", "4", null, null, "Equals", false, false, false, "string");

                        break;
                    }


                default: { break; }
            }

            var listArgs = new ListComponentArgs();
            listArgs.QueryCode = myQueryCode;
            listArgs.Filters = filters;
            listArgs.ObjectTableName = "GLAccount";
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = TextCodeTranslator.Translate("Accounting.General.O.Main");
            listArgs.Perspective = "GLAccountMain";
            listArgs.IgnoreSelectedPerspective = true;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        cmpRef.instance.BackCompleted.subscribe(($event: any) => this.LoadAllScreenData());
                        this.CurrentSession.AddMenuReference(cmpRef);
                    });
            });
        }
    }

    /*

    public RecentGLAccountsList: GLAccountList[];
    LoadRecentGLAccounts() {
        this.RecentGLAccountsList = [];
        this.RecentGLAccountsCount = 0;

        this._DeclarationExtendedListService.GetRecentGLAccounts("1").subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var myResult = myResponse.Result;

                    this.RecentGLAccountsList = myResult;
                    this.RecentGLAccountsCount = myResult.length;
                }
            }
        });
    }
    */
}
