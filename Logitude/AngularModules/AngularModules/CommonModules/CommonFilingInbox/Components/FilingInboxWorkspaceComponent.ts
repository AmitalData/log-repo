import { Component, OnInit, OnDestroy, EventEmitter, Output, AfterViewInit } from '@angular/core';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { FilingInboxPM } from '../../../Common/EntityPMs/FilingInboxPM';
import { FilingInboxPMService } from '../../../Common/Services/StandardPMs/FilingInboxPMService';
import { FilingInboxAttachmentPM } from '../../../Common/EntityPMs/FilingInboxAttachmentPM';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { AppTool } from '../../../Infrastructure/Tools';
import { CommonDomainService, FilingInboxSummary, FilingInboxAttachItem } from '../../../Common/Services/CommonDomainService';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { ShipmentList } from '../../../Shipment/EntityLists/ShipmentList';
import { ShipmentPMService } from '../../../Shipment/Services/StandardPMs/ShipmentPMService';
import { QuoteList } from '../../../Quote/EntityLists/QuoteList';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { UserPMService } from '../../../Common/Services/StandardPMs/UserPMService';
import { DocumentTypeListExtendedService } from '../../../Common/Services/ExtendedLists/DocumentTypeListExtendedService';
import { DocumentsFilingPM } from '../../../Common/EntityPMs/DocumentsFilingPM';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { DownloadManager } from '../../../Infrastructure/Utilities/DownloadManager';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { ServiceLocator } from '../../../Infrastructure/Locators/ServiceLocator';
import { DocumentsFilingExtendedPMService } from '../../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';


declare var window, SetHtmlToFrame: any;

@Component({
    moduleId: module.id,
    templateUrl: './FilingInboxWorkspaceComponent.html',
})

export class FilingInboxWorkspaceComponent extends BaseComponent implements OnInit, AfterViewInit, OnDestroy {
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public ObjectTableName = "FilingInbox";
    public DataContext = this;
    public ItemsSource: FilingInboxData[] = [];
    public FilingInboxAttachments = [];
    private LoggedUserId = SessionLocator.LoggedUserId;
    public SettingsDomain = "domain.com";
    public IFrameURI: string = "";
    public QuickSearchItems: any[] = [];
    public Filters: ApiQueryFilters = null;
    public ObjectTableId = null;
    public ShipmentObjectTableId = null;
    public QuoteObjectTableId = null;
    public IsVisible = false;
    MyInActiveFilter: ApiQueryFilters;
    public IsHouseDisabled = false;
    public IsNewShipmentVisible = false;
    public IsHouseVisible = false;
    public IsConnectToFilterVisible = false;
    public IsDescriptionVisible = false;
    public IsShareAgentVisible = false;
    public IsDigitallySignVisible = false;
    public DontShowInboxToolTip: boolean = false;
    public IsLogBox = false;
    public IconBackground = "./Images/single-tick.png";
    public ShareAsDefault: boolean = false;
    public IsDSVConnectVisible: boolean = false;
    public IsDSVConnectEnable: boolean = false;
    public IsHebrewSettings = false;
    public _documentsFilingExtendedPMService: DocumentsFilingExtendedPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.entityResourceService = new EntityResourceService();
        this.Listen();
        this.SetDefaultValues();
        this.Initialize();
        this.InitializeFilters();
        //this.LoadAllData();
        this.FillDocumentFiling();
        this.SetUIPropertires();
    }
    ngOnInit() {

        if (this.IsLogBox || SessionLocator.PrivateLableSettings) {
            this.IsHebrewSettings = true;
        }
        if (FeatureLocator.HasFeaturePermession("FilingInbox", "DigitallySign")) {
            this.CheckDigitalSign();
        }
        this.Id = Guid.newGuid();
        var divId = this.CurrentSession.GetNewId("PreviewDiv");
        this.PreviewDivId = "PreviewDiv_" + divId;
        this.DontShowInboxToolTip = SessionLocator.LoggedUserPM.ShowInboxToolTip;
        this.DontShowAgain = SessionLocator.LoggedUserPM.ShowInboxToolTip;

        if (FeatureLocator.HasFeaturePermession("FilingInbox", "NewShipment")) {
            this.IsNewShipmentVisible = true;
        }
        if (FeatureLocator.HasFeaturePermession("FilingInbox", "MastersFilter")) {
            this.IsHouseVisible = true;
        }
        if (FeatureLocator.HasFeaturePermession("FilingInbox", "FilingInboxDescription")) {
            this.IsDescriptionVisible = true;
        }
        if (FeatureLocator.HasFeaturePermession("FilingInbox", "ShareWithAgent")) {
            this.IsShareAgentVisible = true;
        }
        if (FeatureLocator.HasFeaturePermession("FilingInbox", "DigitallySign")) {
            this.IsDigitallySignVisible = true;
        }
        if (FeatureLocator.HasFeaturePermession("FilingInbox", "ShipmentsFilter") || FeatureLocator.HasFeaturePermession("FilingInbox", "MastersFilter") || FeatureLocator.HasFeaturePermession("FilingInbox", "QuotesFilter")) {
            this.IsConnectToFilterVisible = true;

            if (FeatureLocator.HasFeaturePermession("FilingInbox", "MastersFilter")) {
                this.SelectedConnectToFilter = "M";
            }
            else if (FeatureLocator.HasFeaturePermession("FilingInbox", "ShipmentsFilter")) {
                this.SelectedConnectToFilter = "S";
            }
            else if (FeatureLocator.HasFeaturePermession("FilingInbox", "QuotesFilter")) {
                this.SelectedConnectToFilter = "Q";
            }
            else {
                this.SelectedConnectToFilter = "";
            }
        }
        this.GetUser();
    }
    ngAfterViewInit() {
        this.SetHtml();
    }

    SetUIPropertires() {
        if (AppTool.IsNullOrEmpty(this.EntityId)) {
            this.IsHouseDisabled = true;
        }
        else {
            this.IsHouseDisabled = false;
        }
    }
    SetDefaultValues() {
        this.ShareAsDefault = SessionLocator.TenantPM.DocumentShareAsDefault;
        this.Filters = new ApiQueryFilters();
        this.Filters.PageIndex = 0;
        this.Filters.PageSize = 10;
        this.MyInActiveFilter = new ApiQueryFilters();
        this.MyInActiveFilter.Filter1Name = "InActive";
        this.MyInActiveFilter.Filter1Operator = "Equals";
        this.MyInActiveFilter.Filter1Value = false;
        var objecttabel = window.ObjectTables.filter(x => x.Name === "Shipment")[0];
        this.ShipmentObjectTableId = objecttabel.Id;
        this.ObjectTableId = this.ShipmentObjectTableId;
        this.EntityObjectTableName = "Master";
        objecttabel = window.ObjectTables.filter(x => x.Name === "Quote")[0];
        this.QuoteObjectTableId = objecttabel.Id;

        if (ObjectsLocator.GlobalSetting.DeploymentStage == "logboxwe1" || ObjectsLocator.GlobalSetting.DeploymentStage == "Test2" && this.EntityObjectTableName == "Shipment") {
            this.IsLogBox = true;
        }

        if (SessionLocator.PrivateLableSettings) {
            this.IsDSVConnectVisible = true;
        }
        else {
            this.IsDSVConnectVisible = false;
        }

        this.PageIndex = 1;
        this.QueryPageIndex = 0;
    }

    public IsDigitallySignDisabled = false;
    CheckDigitalSign() {
        if (this.myCommonDomainService == null) {
            this.myCommonDomainService = new CommonDomainService();
        }
        this.myCommonDomainService.GetSignRequestReceived().subscribe((response: ServiceResponse) => {
            if (response != null && !response.HasError) {
                var result = response.Result;
                if (!AppTool.IsNullOrEmpty(result)) {
                    this.IsDigitallySignDisabled = true;
                }
            }
        });
    }

    //Search 
    public SearchFilter: string = "";
    public searchFields: string;
    onSearchTextChangeEvent(event) {
        var temp = null;
        if (event) {
            temp = event.replace(/\s+$/, '');
        }
        this.searchFields = temp;
        this.SearchFilter = temp;
        this.IsVisible = false;
        this.LoadAllData();
    }

    //Tip 
    public DontShowagainText = "Don't Show again";
    private dontShowAgain: boolean = false;
    get DontShowAgain() {
        return this.dontShowAgain;
    }
    set DontShowAgain(value: boolean) {
        if (this.dontShowAgain != value) {
            this.dontShowAgain = value;
            SessionLocator.LoggedUserPM.ShowInboxToolTip = value;
            var myPM = SessionLocator.LoggedUserPM;
            this.UserPMService.update(myPM).subscribe(myResult => {
            });
        }
    }
    CloseToolTipArea(arg: boolean) {
        this.DontShowInboxToolTip = true;
    }
    OpenToolTipArea() {
        this.DontShowInboxToolTip = false;
    }

    private MenuEvent: any = null;
    Listen() {
        this.MenuEvent = this.CurrentSession.MainMenuComponent.SelectionChanging.subscribe((isSelectionChanging: boolean) => {
            if (isSelectionChanging) {
                this.ShowUnsaveChanges();
            }
        });
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.MenuEvent);
    }

    ShowUnsaveChanges() {
        var hasChanges = false;
        if (this.SelectedFilingInbox != null) {
            this.SelectedFilingInbox.FilingInboxAttachments.forEach(item => {
                if (!AppTool.IsNullOrEmpty(item.DocumentTypeId)) {
                    hasChanges = true;
                }
            });
            if (hasChanges) {
                var confirmWindow = new ConfirmWindow();
                confirmWindow.Width = 450;
                confirmWindow.Height = 190;
                confirmWindow.ShowCancelButton = true;
                confirmWindow.NoButtonText = "Don't Save";
                confirmWindow.YesButtonText = "Save ";
                confirmWindow.CancelButtonText = "Cancel";
                confirmWindow.Title = TextCodeTranslator.Translate("General.O.UnSavedChanges");
                confirmWindow.Show("This Email has unsaved changes. Do you want to save it?");
                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        // save filing 
                        this.FileButtonClicked();
                    }
                    else if (confirmWindow.No) {
                        this.ChangeMenu();
                    }
                    else if (confirmWindow.Cancel) {
                        // nth
                    }
                });
            }
            else {
                this.ChangeMenu();
            }
        }
        else {
            this.ChangeMenu();
        }
    }
    ChangeMenu() {
        this.CurrentSession.MainMenuComponent.ChangeMenu();
    }

    public Id: string;
    public PreviewDivId: string;
    SetHtml() {
        var element = document.getElementById(this.PreviewDivId);
        if (element) {
            element.innerHTML = this.EmailBody;
        }
    }
    GetUser() {
        this.UserPMService.get(SessionLocator.LoggedUserId).subscribe((myResult: ServiceResponse) => {
            if (myResult != null && !myResult.HasError) {
                var domain = ObjectsLocator.GlobalSetting.DocumentFilingEmailDomain;
                if (SessionLocator.PrivateLableSettings) {
                    domain = "inbox.dsv.co.il";
                }
                this.SettingsDomain = myResult.Result != null ? myResult.Result.DocumentFilingInbox + "@" + domain : "";
            }
        });
    }

    //Fill DocumentTypeList 
    TopTypes: any[];
    public DocumentTypeList = [];
    FillDocumentFiling() {
        this._DocumentTypeListService.getTop5DocumentTypesPMsByObjectTableAndTenant(SessionLocator.Tenant, this.ObjectTableId).subscribe(res => {
            var MyType = "";
            res.Result.forEach((item) => {
                MyType = item.Name.trim();
                var tempList = MyType.split(' ');
                var tempName = "";
                if (tempList.length == 1) {
                    tempName = tempList[0].substring(0, 3).toUpperCase();
                }
                else if (tempList.length == 2) {
                    tempName = (tempList[0].substring(0, 1) + tempList[1].substring(0, 2)).toUpperCase();
                }
                else {
                    tempName = (tempList[0].substring(0, 1) + tempList[1].substring(0, 1) + tempList[2].substring(0, 1)).toUpperCase();
                }
                item.OrderedDisplayName = tempName;
            });
            this.DocumentTypeList = res.Result;
            this.TopTypes = res.Result;
        });
    }

    InitializeFilters() {
        this.mySelectedUserFilter = "M";
        this.UIProperties.SetEnabled("UserId", "UserFilter", false);
        this.myUserId = this.LoggedUserId;
    }

    public count: number = 0;
    private filters: ApiQueryFilters;
    SearchedList = [];
    LoadAllData() {
        this.filters = new ApiQueryFilters();
        this.filters.PageIndex = this.QueryPageIndex;
        this.filters.PageSize = this.PageSize;

        if (this.IsRefreshClicked) {
            this.IsRefreshEnabled = false;
        }
        this.ItemsSource = [];
        this.myCommonDomainService.GetFilingInboxes(this.filters, this.myUserId, this.IsShowDeletedEnabled).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var result: FilingInboxPM[] = response.Result;

                if (result != null && !AppTool.IsNullOrEmpty(this.searchFields)) {
                    result = result.filter(d => d.SearchFields != null && d.SearchFields && d.SearchFields.toUpperCase().indexOf(this.searchFields.toUpperCase()) > -1);
                }

                result.forEach(item => {
                    this.ItemsSource.push(new FilingInboxData(item, this));
                });
                if (this.ItemsSource.length > 0) {
                    this.SelectedFilingInbox = this.ItemsSource[0];
                }
                else {
                    this.SelectedFilingInbox = null;
                }

                if (!AppTool.IsNullOrEmpty(this.searchFields)) {
                    this.count = result.length;
                }
                else {
                    this.count = response.Count;
                }
                var size = this.pageSize;
                this.TotalPagesCount = Math.ceil(this.count / size);

                if (this.TotalPagesCount == 0) {
                    this.TotalPagesCount = 1;
                }

                this.SetPagerButtonsStates();
                this.IsVisible = true;
                this.IsRefreshEnabled = true;
            }
            else {

                if (response.ErrorsArray && response.ErrorsArray.length > 0) {
                    this.ShowMessage(response.ErrorsArray[0]);
                }
            }

        });
    }
    LoadDateCount() {
        var size = this.pageSize;
        this.TotalPagesCount = Math.ceil(this.count / size);

        if (this.TotalPagesCount == 0) {
            this.TotalPagesCount = 1;
        }

        this.SetPagerButtonsStates();
    }

    /*Pager & Provider*/
    private queryPageIndex = 0;
    get QueryPageIndex() {
        return this.queryPageIndex;
    }
    set QueryPageIndex(value: number) {
        this.queryPageIndex = value;
    }

    private pageIndex = 1;
    get PageIndex() {
        return this.pageIndex;
    }
    set PageIndex(value: number) {
        this.pageIndex = value;
    }

    private totalPagesCount = 1;
    get TotalPagesCount() {
        return this.totalPagesCount;
    }
    set TotalPagesCount(value: number) {
        this.totalPagesCount = value;
    }

    private pageSize = 100;
    get PageSize() {
        return this.pageSize;
    }
    set PageSize(value: number) {
        this.pageSize = value;
    }

    /* Pager Buttons States */
    private isHitStateFirstButton: boolean = false;
    get IsHitState_FirstButton() {
        return this.isHitStateFirstButton;
    }
    set IsHitState_FirstButton(value: boolean) {
        this.isHitStateFirstButton = value;
    }

    private opacityFirstButton: number = 0.5;
    get Opacity_FirstButton() {
        return this.opacityFirstButton;
    }
    set Opacity_FirstButton(value: number) {
        this.opacityFirstButton = value;
    }

    private isHitStatePrevButton: boolean = false;
    get IsHitState_PrevButton() {
        return this.isHitStatePrevButton;
    }
    set IsHitState_PrevButton(value: boolean) {
        this.isHitStatePrevButton = value;
    }

    private opacityPrevButton: number = 0.5;
    get Opacity_PrevButton() {
        return this.opacityPrevButton;
    }
    set Opacity_PrevButton(value: number) {
        this.opacityPrevButton = value;
    }

    private isHitStateNextButton: boolean = false;
    get IsHitState_NextButton() {
        return this.isHitStateNextButton;
    }
    set IsHitState_NextButton(value: boolean) {
        this.isHitStateNextButton = value;
    }

    private opacityNextButton: number = 0.5;
    get Opacity_NextButton() {
        return this.opacityNextButton;
    }
    set Opacity_NextButton(value: number) {
        this.opacityNextButton = value;
    }

    private isHitStateLastButton: boolean = false;
    get IsHitState_LastButton() {
        return this.isHitStateLastButton;
    }
    set IsHitState_LastButton(value: boolean) {
        this.isHitStateLastButton = value;
    }

    private opacityLastButton: number = 0.5;
    get Opacity_LastButton() {
        return this.opacityLastButton;
    }
    set Opacity_LastButton(value: number) {
        this.opacityLastButton = value;
    }


    /* First Page */
    private SetPagerButtonsStates() {
        if (this.PageIndex == 1 && this.PageIndex == this.TotalPagesCount) {
            this.IsHitState_FirstButton = false;
            this.IsHitState_PrevButton = false;
            this.IsHitState_NextButton = false;
            this.IsHitState_LastButton = false;

            this.Opacity_FirstButton = 0.5;
            this.Opacity_PrevButton = 0.5;
            this.Opacity_NextButton = 0.5;
            this.Opacity_LastButton = 0.5;
        }

        else if (this.PageIndex == 1 && this.PageIndex < this.TotalPagesCount) {
            this.IsHitState_FirstButton = false;
            this.IsHitState_PrevButton = false;
            this.Opacity_FirstButton = 0.5;
            this.Opacity_PrevButton = 0.5;

            this.IsHitState_NextButton = true;
            this.IsHitState_LastButton = true;
            this.Opacity_NextButton = 1;
            this.Opacity_LastButton = 1;
        }

        else if (this.PageIndex > 1 && this.PageIndex == this.TotalPagesCount) {
            this.IsHitState_FirstButton = true;
            this.IsHitState_PrevButton = true;
            this.Opacity_FirstButton = 1;
            this.Opacity_PrevButton = 1;

            this.IsHitState_NextButton = false;
            this.IsHitState_LastButton = false;
            this.Opacity_NextButton = 0.5;
            this.Opacity_LastButton = 0.5;
        }

        else if (this.PageIndex > 1 && this.PageIndex < this.TotalPagesCount) {
            this.IsHitState_FirstButton = true;
            this.IsHitState_PrevButton = true;
            this.IsHitState_NextButton = true;
            this.IsHitState_LastButton = true;

            this.Opacity_FirstButton = 1;
            this.Opacity_PrevButton = 1;
            this.Opacity_NextButton = 1;
            this.Opacity_LastButton = 1;
        }
    }
    FirstPageClick() {
        var hasChanges = false;
        if (this.selectedFilingInbox != null) {
            this.selectedFilingInbox.FilingInboxAttachments.forEach(item => {
                if (!AppTool.IsNullOrEmpty(item.DocumentTypeId)) {
                    hasChanges = true;
                }
            });
            if (hasChanges) {
                var confirmWindow = new ConfirmWindow();
                confirmWindow.Width = 450;
                confirmWindow.Height = 190;
                confirmWindow.ShowCancelButton = true;
                confirmWindow.NoButtonText = "Don't Save";
                confirmWindow.YesButtonText = "Save ";
                confirmWindow.CancelButtonText = "Cancel";
                confirmWindow.Title = TextCodeTranslator.Translate("General.O.UnSavedChanges");
                confirmWindow.Show("This Email has unsaved changes. Do you want to save it?");
                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        // save filing 
                        this.FileButtonClicked();
                    }
                    else if (confirmWindow.No) {
                        this.FirstPageWork();
                    }
                    else if (confirmWindow.Cancel) {
                        // nth
                    }
                });
            }
            else {
                this.FirstPageWork();
            }
        }
        else {
            this.FirstPageWork();
        }

    }
    FirstPageWork() {
        this.ClearConnectToFilter();
        this.PageIndex = 1;
        this.QueryPageIndex = 0;
        this.SetPagerButtonsStates();
        this.IsVisible = false;
        this.LoadAllData();
    }
    PreviousPageClick() {
        var hasChanges = false;
        if (this.selectedFilingInbox != null) {
            this.selectedFilingInbox.FilingInboxAttachments.forEach(item => {
                if (!AppTool.IsNullOrEmpty(item.DocumentTypeId)) {
                    hasChanges = true;
                }
            });
            if (hasChanges) {
                var confirmWindow = new ConfirmWindow();
                confirmWindow.Width = 450;
                confirmWindow.Height = 190;
                confirmWindow.ShowCancelButton = true;
                confirmWindow.NoButtonText = "Don't Save";
                confirmWindow.YesButtonText = "Save ";
                confirmWindow.CancelButtonText = "Cancel";
                confirmWindow.Title = TextCodeTranslator.Translate("General.O.UnSavedChanges");
                confirmWindow.Show("This Email has unsaved changes. Do you want to save it?");
                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        // save filing 
                        this.FileButtonClicked();
                    }
                    else if (confirmWindow.No) {
                        this.PreviosButtonWork();
                    }
                    else if (confirmWindow.Cancel) {
                        // nth
                    }
                });
            }
            else {
                this.PreviosButtonWork();
            }
        }
        else {
            this.PreviosButtonWork();
        }
    }
    PreviosButtonWork() {
        this.ClearConnectToFilter();
        this.PageIndex = this.PageIndex - 1;
        this.QueryPageIndex = this.QueryPageIndex - 100;
        this.SetPagerButtonsStates();
        this.IsVisible = false;
        this.LoadAllData();
    }
    NextPageClick() {
        var hasChanges = false;
        if (this.selectedFilingInbox != null) {
            this.selectedFilingInbox.FilingInboxAttachments.forEach(item => {
                if (!AppTool.IsNullOrEmpty(item.DocumentTypeId)) {
                    hasChanges = true;
                }
            });
            if (hasChanges) {
                var confirmWindow = new ConfirmWindow();
                confirmWindow.Width = 450;
                confirmWindow.Height = 190;
                confirmWindow.ShowCancelButton = true;
                confirmWindow.NoButtonText = "Don't Save";
                confirmWindow.YesButtonText = "Save ";
                confirmWindow.CancelButtonText = "Cancel";
                confirmWindow.Title = TextCodeTranslator.Translate("General.O.UnSavedChanges");
                confirmWindow.Show("This Email has unsaved changes. Do you want to save it?");
                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        // save filing 
                        this.FileButtonClicked();
                    }
                    else if (confirmWindow.No) {
                        this.NextPageWork();
                    }
                    else if (confirmWindow.Cancel) {
                        // nth
                    }
                });
            }
            else {
                this.NextPageWork();
            }
        }
        else {
            this.NextPageWork();
        }
    }
    NextPageWork() {
        this.ClearConnectToFilter();
        this.PageIndex = this.PageIndex + 1;
        this.QueryPageIndex = this.QueryPageIndex + 100;
        this.SetPagerButtonsStates();
        this.IsVisible = false;
        this.LoadAllData();
    }
    LastPageClick() {
        var hasChanges = false;
        if (this.selectedFilingInbox != null) {
            this.selectedFilingInbox.FilingInboxAttachments.forEach(item => {
                if (!AppTool.IsNullOrEmpty(item.DocumentTypeId)) {
                    hasChanges = true;
                }
            });
            if (hasChanges) {
                var confirmWindow = new ConfirmWindow();
                confirmWindow.Width = 450;
                confirmWindow.Height = 190;
                confirmWindow.ShowCancelButton = true;
                confirmWindow.NoButtonText = "Don't Save";
                confirmWindow.YesButtonText = "Save ";
                confirmWindow.CancelButtonText = "Cancel";
                confirmWindow.Title = TextCodeTranslator.Translate("General.O.UnSavedChanges");
                confirmWindow.Show("This Email has unsaved changes. Do you want to save it?");
                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        // save filing 
                        this.FileButtonClicked();
                    }
                    else if (confirmWindow.No) {
                        this.LastPageWork();
                    }
                    else if (confirmWindow.Cancel) {
                        // nth
                    }
                });
            }
            else {
                this.LastPageWork();
            }
        }
        else {
            this.LastPageWork();
        }
    }
    LastPageWork() {
        this.ClearConnectToFilter();
        this.PageIndex = this.TotalPagesCount;
        this.QueryPageIndex = (this.TotalPagesCount - 1) * this.pageSize;
        this.SetPagerButtonsStates();
        this.IsVisible = false;
        this.LoadAllData();
    }

    private myCommonDomainService: CommonDomainService;
    private myFilingInboxPMService: FilingInboxPMService;
    private UserPMService: UserPMService;
    private _ShipmentPMService: ShipmentPMService;
    private _DocumentTypeListService: DocumentTypeListExtendedService;
    Initialize() {
        this.myCommonDomainService = new CommonDomainService();
        this.myFilingInboxPMService = new FilingInboxPMService();
        this.UserPMService = new UserPMService();
        this._DocumentTypeListService = new DocumentTypeListExtendedService();
    }

    // Props
    private EmailBody: string;
    private description: string = null;
    get Description() { return this.description; }
    set Description(value: string) {
        if (this.description != value) {
            this.description = value;
        }
    }

    // Filters 
    private myUserId: string = this.LoggedUserId;
    get UserId() { return this.myUserId; }
    set UserId(value: string) {
        if (this.myUserId != value) {
            this.myUserId = value;
            this.IsVisible = false;
            this.LoadAllData();
        }
    }

    private mySelectedUserFilter: string = "M";
    get SelectedUserFilter() { return this.mySelectedUserFilter; }
    set SelectedUserFilter(value: string) {
        if (this.mySelectedUserFilter != value) {
            this.mySelectedUserFilter = value;
            if (value == "M") {
                this.myUserId = this.LoggedUserId;
                this.UIProperties.SetEnabled("UserId", "UserFilter", false);
            }
            else {
                this.myUserId = null;
                this.UIProperties.SetEnabled("UserId", "UserFilter", true);
                this.IsStopPreviewHtml = false;
            }
            this.IsVisible = false;
            this.LoadAllData();
        }
    }

    public ConnectToFilterLabel = "Master";
    private mySelectedConnectToFilter: string;
    get SelectedConnectToFilter() { return this.mySelectedConnectToFilter; }
    set SelectedConnectToFilter(value: string) {
        if (this.mySelectedConnectToFilter != value) {
            this.ClearConnectToFilter();
            this.mySelectedConnectToFilter = value;
            this.GetConnectToFilterLabel();
            if (this.SelectedAttachment != null) {
                this.SelectedAttachment.IsSharedWithAgent = this.ShareAsDefault;
                this.SelectedAttachment.FillDocumentFiling();
                if (AppTool.IsNullOrEmpty(this.SelectedAttachment.Description)) {
                    this.SelectedAttachment.Description = this.SelectedAttachment.FileName != null ? this.SelectedAttachment.FileName.split('.')[0] : this.SelectedAttachment.FileName;
                }
            }
            if (value == "M") {
                if (this.SelectedAttachment != null) {
                    this.SelectedAttachment.SetHousesFilters();
                }
            }
        }
    }

    GetConnectToFilterLabel() {
        switch (this.SelectedConnectToFilter) {
            case "M": {
                this.ConnectToFilterLabel = "Master";
                this.EntityObjectTableName = "Master";
                this.IsLogBox = false;
                this.ObjectTableId = this.ShipmentObjectTableId; break;
            }
            case "S": {
                this.ConnectToFilterLabel = "Shipment";
                this.EntityObjectTableName = "Shipment";
                if (ObjectsLocator.GlobalSetting.DeploymentStage == "logboxwe1" || ObjectsLocator.GlobalSetting.DeploymentStage == "Test2") {
                    this.IsLogBox = true;
                }
                this.ObjectTableId = this.ShipmentObjectTableId; break;
            }
            case "Q": {
                this.ConnectToFilterLabel = "Quote";
                this.EntityObjectTableName = "Quote";
                this.IsLogBox = false;
                this.ObjectTableId = this.QuoteObjectTableId; break;
            }
        }
    }

    private isShowDeletedEnabled = false;
    get IsShowDeletedEnabled() {
        return this.isShowDeletedEnabled;
    }
    set IsShowDeletedEnabled(value: boolean) {
        if (this.isShowDeletedEnabled != value) {
            this.isShowDeletedEnabled = value;
            this.IsVisible = false;
            this.LoadAllData();
        }
    }

    //Commands 
    public Customer: string;
    public Route: string;
    public EntityNumber: string;
    public entityId: string;
    get EntityId() {
        return this.entityId;
    }
    set EntityId(value: string) {
        if (this.entityId != value) {
            this.entityId = value;
            if (this.SelectedAttachment != null) {
                this.SelectedAttachment.SetHousesFilters();
            }
            this.SetUIPropertires();
        }
    }

    public EntityObjectTableName: string;
    public IsItemSelected: boolean = false;
    QuickSearchTextChanged(entity: any) {
        this.IsItemSelected = true;
        if (this.IsLogBox || SessionLocator.PrivateLableSettings) {
            if (this.EntityObjectTableName == "Master") {
                this.Customer = entity.AgentName;
            }
            else {
                this.Customer = entity.ShipperName;
            }

            if (AppTool.IsNullOrEmpty(entity.ForwarderShipmentNumber) && !AppTool.IsNullOrEmpty(entity.StatusName) && entity.StatusName.toLocaleLowerCase() != "in progress") {
                this.IsDSVConnectEnable = true;
            }
            else {
                this.IsDSVConnectEnable = false;
            }

            this.Route = entity.Routing;
            this.EntityNumber = AppTool.IsNullOrEmpty(entity.ForwarderShipmentNumber) ? entity.CustomerReference1 : entity.ForwarderShipmentNumber;
            this.EntityId = entity.Id;
        }
        else {
            if (this.EntityObjectTableName == "Master") {
                this.Customer = entity.AgentName;
            }
            else {
                this.Customer = entity.CustomerName;
            }
            this.Route = entity.Routing;
            if (this.EntityObjectTableName == "Shipment" || this.EntityObjectTableName == "Master") {
                this.EntityNumber = entity.ShipmentNumber;
            }
            else {
                this.EntityNumber = entity.QuoteNumber;
            }
            this.EntityId = entity.Id;
        }
    }

    ChooseEntity(arg: string) {
        if (this.SelectedFilingInbox != null) {
            if (this.IsLogBox || SessionLocator.PrivateLableSettings) {
                this.ChooseForwarderShipment();
            }
            else {
                var logWindow = new LogitudeWindow();
                logWindow.Width = 800;
                logWindow.Height = 570;
                var args: any = {};

                if (arg == "house") {
                    logWindow.Title = "Houses Search";
                    args.EntityObjectTableName = "House";
                    args.EntityId = this.EntityId;
                }
                else {
                    if (this.EntityObjectTableName == "Shipment" || this.EntityObjectTableName == "Master") {
                        logWindow.Title = "Shipments Search";
                    }
                    else {
                        logWindow.Title = "Quotes Search";
                    }

                    args.EntityObjectTableName = this.EntityObjectTableName;
                }

                if (arg == "house" && this.IsHouseDisabled) {

                } else {
                    logWindow.WindowArgs = args;
                    logWindow.Show('./CommonModules/CommonFilingInbox/Components/ChooseEntityComponent');
                    logWindow.ComponentLoaded.subscribe(s => {
                        logWindow.WindowClosed.subscribe(d => {
                            var entityList = null;
                            if (s.EntityObjectTableName == "Shipment" || s.EntityObjectTableName == "Master" || s.EntityObjectTableName == "House") {
                                entityList = s.SelectedShipment;
                            }
                            else {
                                entityList = s.SelectedQuote;
                            }
                            if (entityList != null) {
                                if (s.EntityObjectTableName == "Master") {
                                    this.Customer = entityList.AgentName;
                                }
                                else {
                                    this.Customer = entityList.CustomerName;
                                }

                                this.Route = entityList.Routing;
                                if (s.EntityObjectTableName == "House") {
                                    this.SelectedAttachment.HouseNumber = entityList.ShipmentNumber;
                                }
                                else if (s.EntityObjectTableName == "Shipment" || s.EntityObjectTableName == "Master") {
                                    this.EntityNumber = entityList.ShipmentNumber;
                                }
                                else {
                                    this.EntityNumber = entityList.QuoteNumber;
                                }
                                this.EntityId = entityList.Id;
                            }
                        });
                    });
                }
            }
        }
    }
    ChooseForwarderShipment() {
        var newWindow = new LogitudeWindow();
        newWindow.Width = 1050;
        newWindow.Height = 700;
        newWindow.Title = "Connect To Agent Shipment";
        var windowArgs: any = {};
        newWindow.WindowArgs = windowArgs;
        newWindow.Show('./CommonModules/CommonFilingInbox/Components/ForwarderChooseShipmentsComponent');
        newWindow.ComponentLoaded.subscribe(s => {
            newWindow.WindowClosed.subscribe(d => {
                var entityList = null;
                entityList = s.SelectedRow;
                if (entityList != null) {
                    if (this.EntityObjectTableName == "Master") {
                        this.Customer = entityList.AgentName;
                    }
                    else {
                        this.Customer = entityList.ShipperName;
                    }

                    if (AppTool.IsNullOrEmpty(entityList.ForwarderShipmentNumber) && !AppTool.IsNullOrEmpty(entityList.StatusName) && entityList.StatusName.toLocaleLowerCase() != "in progress") {
                        this.IsDSVConnectEnable = true;
                    }
                    else {
                        this.IsDSVConnectEnable = false;
                    }
                    this.Route = entityList.Routing;
                    this.EntityNumber = AppTool.IsNullOrEmpty(entityList.ForwarderShipmentNumber) ? entityList.CustomerReference1 : entityList.ForwarderShipmentNumber;
                    this.EntityId = entityList.Id;
                }
            });
        });
    }

    ClearConnectToFilter() {
        this.QuickSearchItems = [];
        this.EntityNumber = null
        this.Route = null;
        this.Customer = null;
        this.EntityId = null;
        this.IsDSVConnectEnable = false;
        if (this.SelectedFilingInbox != null) {
            this.SelectedFilingInbox.FilingInboxAttachments.forEach(item => {
                item.DocumentTypeId = null;
                item.House = null;
                item.Description = null;
                item.IsSharedWithAgent = false;
                item.IsDigitallySign = false;
                item.HouseNumber = null;
                item.TypeSelected = false;
                item.SelectedName = null;
                item.ShowTypes = false;
                item.SelectedValue = null;
                item.IsSingleTick = false;
                item.CellTooltipIconPath = "./Images/double-tick.png";
            });
        }
    }

    public IsPDF = false;

    private selectedAttachment: FilingInboxAttachment = new FilingInboxAttachment(null, this);
    get SelectedAttachment() {
        return this.selectedAttachment;
    }
    set SelectedAttachment(value: FilingInboxAttachment) {
        if (this.selectedAttachment != value) {
            this.selectedAttachment = value;
            this.selectedAttachment.IsSharedWithAgent = this.ShareAsDefault;
            if (AppTool.IsNullOrEmpty(this.selectedAttachment.Description)) {
                this.selectedAttachment.Description = this.selectedAttachment.FileName != null ? this.selectedAttachment.FileName.split('.')[0] : this.selectedAttachment.FileName;
            }
            this.isMailBody = false;
            if (value != null) {

                if (value.FileName != null && value.FileName == "Mail Body") {
                    this.isMailBody = true;
                }
                if (value.FileName != null && value.FileName.split('.') != null && value.FileName.split('.')[1] != null && (value.FileName.split('.')[1].toUpperCase().trim() == "PDF")) {
                    this.IsPDF = true;
                    this.myCommonDomainService.GetFilingAttachPdfReport(value.DocumentId).subscribe((response: ServiceResponse) => {
                        if (!response.HasError) {
                            var buffer = EntityResourceService.base64ToBufferConvertor(response.Result);
                            var blob = new Blob([buffer], { type: 'application/pdf' });
                            var objectURL = URL.createObjectURL(blob);
                            this.IFrameURI = objectURL;
                        }
                    });
                }
                else {
                    this.IsPDF = false;
                }
            }
            this.SelectedFilingInbox.FilingInboxAttachments.forEach(item => {
                if (value == item) {
                    item.CellTooltipIconPath = "./Images/double-tick-white.png";
                }
                else {
                    item.CellTooltipIconPath = "./Images/double-tick.png";
                }
            });
        }
    }
    PreviewButtonClicked() {
        this.ViewAttachment();
    }
    ViewAttachment() {
        var documentName = this.SelectedAttachment.DocumentId;
        DownloadManager.DownloadPage(documentName);
    }

    private isMailBody = false;
    get IsMailBody() {
        return this.isMailBody;
    }
    set IsMailBody(value: boolean) {
        if (this.isMailBody != value) {
            this.isMailBody = value;
            this.selectedAttachment = null;
        }
    }

    public IsFilingButtonEnabled = true;
    private selectedFilingInbox: FilingInboxData = null;
    public get SelectedFilingInbox() {
        return this.selectedFilingInbox;
    }
    public set SelectedFilingInbox(value: FilingInboxData) {
        if (this.selectedFilingInbox != value) {

            var hasChanges = false;
            if (this.selectedFilingInbox != null) {
                this.selectedFilingInbox.FilingInboxAttachments.forEach(item => {
                    if (!AppTool.IsNullOrEmpty(item.DocumentTypeId)) {
                        hasChanges = true;
                    }
                });
                if (hasChanges) {
                    var confirmWindow = new ConfirmWindow();
                    confirmWindow.Width = 450;
                    confirmWindow.Height = 190;
                    confirmWindow.ShowCancelButton = true;
                    confirmWindow.NoButtonText = "Don't Save";
                    confirmWindow.YesButtonText = "Save ";
                    confirmWindow.CancelButtonText = "Cancel";
                    confirmWindow.Title = TextCodeTranslator.Translate("General.O.UnSavedChanges");
                    confirmWindow.Show("This Email has unsaved changes. Do you want to save it?");
                    confirmWindow.WindowClosed.subscribe((event: any) => {
                        if (confirmWindow.Yes) {
                            // save filing 
                            this.FileButtonClicked();
                        }
                        else if (confirmWindow.No) {
                            this.SetSelectedFilingInbox(value);
                        }
                        else if (confirmWindow.Cancel) {
                            // nth
                        }
                    });
                }
                else {
                    this.SetSelectedFilingInbox(value);
                }
            }
            else {
                this.SetSelectedFilingInbox(value);
            }
        }
    }
    SetSelectedFilingInbox(value: FilingInboxData) {
        this.ClearConnectToFilter();
        this.selectedFilingInbox = value;
        this.EmailBody = value != null ? value.FilingInboxPM.EmailBody : "";
        this.FilingInboxAttachments = value != null ? value.FilingInboxAttachments : [];
        this.isMailBody = false;
        this.SetHtml();
        if (this.FilingInboxAttachments != null && this.FilingInboxAttachments.length > 0) {
            this.SelectedAttachment = this.FilingInboxAttachments[0];
            this.IsFilingButtonEnabled = true;
        }
        else {
            this.IsMailBody = true;
            this.IsFilingButtonEnabled = false;
        }
    }

    AttachmentChanged(item: FilingInboxAttachment) {
        this.SelectedAttachment = item;
    }

    DeleteFilingInbox(filingInbox: FilingInboxData) {
        filingInbox.FilingInboxPM.IsDeleted = true;
        this.myFilingInboxPMService.update(filingInbox.FilingInboxPM).subscribe((myRespone: ServiceResponse) => {
            if (myRespone != null) {
                if (!myRespone.HasError) {
                    this.IsVisible = false;
                    this.LoadAllData();
                }
            }
        });
    }

    ViewUser() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityId: this.LoggedUserId, ObjectTableName: "User", BackButtonLabel: "Filing Inbox", SelectedTabCode: "USDF"
                });
                cmpRef.instance.BackCompleted.subscribe(bk => {
                    this.GetUser();
                });
            });
    }
    CopyDomain() {
        let selBox = document.createElement('textarea');
        selBox.style.position = 'fixed';
        selBox.style.left = '0';
        selBox.style.top = '0';
        selBox.style.opacity = '0';
        selBox.value = this.SettingsDomain;
        document.body.appendChild(selBox);
        selBox.focus();
        selBox.select();
        document.execCommand('copy');
        document.body.removeChild(selBox);
    }

    private IsRefreshClicked = false;
    public IsRefreshEnabled = true;
    RefreshButtonClicked() {
        this.IsRefreshClicked = true;
        this.IsVisible = false;
        this.LoadAllData();
    }
    FileButtonClicked() {
        if (this.SelectedFilingInbox != null) {
            this.CurrentSession.StartBusyIndicator("Filing ...");
            var summary: FilingInboxSummary = new FilingInboxSummary();
            var objectTableName = this.EntityObjectTableName;
            if (this.EntityObjectTableName == "Master") {
                objectTableName = "Shipment";
            }
            summary.ObjectTableName = objectTableName;
            summary.IsDeleted = false;
            summary.UserId = this.UserId;
            summary.Attaches = [];
            if (!AppTool.IsNullOrEmpty(this.EntityNumber)) {
                var isValid = false;
                var isDescriptionFilled = true;
                var docsErrorMsg = "";
                this.SelectedFilingInbox.FilingInboxAttachments.forEach(item => {

                    if (!AppTool.IsNullOrEmpty(item.DocumentTypeId)) {
                        if (item.AttachLogs != null && item.AttachLogs.length > 0) {
                            docsErrorMsg += item.FileName + ", ";
                        }
                        var attach = new FilingInboxAttachItem();
                        attach.FileName = item.FileName;
                        attach.EntityId = item.EntityId;
                        attach.DocumentType = item.DocumentTypeId;
                        attach.House = item.House;
                        attach.HouseNumber = item.HouseNumber;
                        attach.Description = item.Description;
                        attach.DocumentId = item.DocumentId;
                        attach.IsSharedWithAgent = item.IsSharedWithAgent;
                        attach.IsDigitallySign = item.IsDigitallySign;
                        item.IsSingleTick = false;
                        summary.Attaches.push(attach);
                        isValid = true;
                        if (ObjectsLocator.GlobalSetting.DeploymentStage == "logboxwe1" || ObjectsLocator.GlobalSetting.DeploymentStage == "Test2" && this.EntityObjectTableName == "Shipment") {
                            if (AppTool.IsNullOrEmpty(item.Description)) {
                                isDescriptionFilled = false;
                            }
                        }
                    }
                });
                if (isValid && isDescriptionFilled) {
                    if (!AppTool.IsNullOrEmpty(this.EntityNumber)) {
                        if (!AppTool.IsNullOrEmpty(docsErrorMsg)) {
                            this.CurrentSession.StopBusyIndicator();
                            if (docsErrorMsg.match(/,/g).length == 1) {
                                docsErrorMsg = docsErrorMsg.replace(/,/g, '');
                                docsErrorMsg = "The document " + docsErrorMsg + " is already filled";
                            }
                            else {
                                docsErrorMsg = "The documents: " + docsErrorMsg + " are already filled";
                            }
                            var confirmWindow = new ConfirmWindow();
                            confirmWindow.NoButtonText = "Cancel";
                            confirmWindow.YesButtonText = "Ok";
                            confirmWindow.Title = "Warning";
                            confirmWindow.Show(docsErrorMsg);
                            confirmWindow.WindowClosed.subscribe((event: any) => {
                                if (confirmWindow.Yes) {
                                    this.CompleteFiling(summary, false);
                                }
                            });
                        }
                        else {
                            this.CompleteFiling(summary, false);
                        }
                    }
                    else {
                        this.CurrentSession.StopBusyIndicator();
                        this.ShowMessage("Please Enter Entity number");
                    }
                }
                else {
                    this.CurrentSession.StopBusyIndicator();
                    var msg = "";
                    if (ObjectsLocator.GlobalSetting.DeploymentStage == "logboxwe1" || ObjectsLocator.GlobalSetting.DeploymentStage == "Test2" && this.EntityObjectTableName == "Shipment") {
                        if (!isDescriptionFilled) {
                            msg += "Please fill Description fields for all attachments. ";
                        }
                    }
                    if (!isValid) {
                        msg += "Please enter at least one document type.";
                    }
                    this.ShowMessage(msg);
                }
            }
            else {
                this.CurrentSession.StopBusyIndicator();
                this.ShowMessage("Please Enter Entity number");
            }
        }
    }
    FileAndDeleteButtonClicked(arg: boolean) {
        if (this.SelectedFilingInbox != null) {
            this.CurrentSession.StartBusyIndicator("Filing & Delete...");
            var summary: FilingInboxSummary = new FilingInboxSummary();
            var objectTableName = this.EntityObjectTableName;
            if (this.EntityObjectTableName == "Master") {
                objectTableName = "Shipment";
            }
            summary.ObjectTableName = objectTableName;
            summary.IsDeleted = true;
            this.SelectedFilingInbox.IsDeleted = true;
            summary.UserId = this.UserId;
            summary.EntityId = this.EntityId;
            summary.FilingId = this.SelectedFilingInbox.FilingInboxPM.Id;
            summary.Attaches = [];
            if (!AppTool.IsNullOrEmpty(this.EntityNumber)) {
                var isValid = false;
                var isDescriptionFilled = true;
                var docsErrorMsg = "";
                this.SelectedFilingInbox.FilingInboxAttachments.forEach(item => {
                    if (!AppTool.IsNullOrEmpty(item.DocumentTypeId)) {
                        if (item.AttachLogs != null && item.AttachLogs.length > 0) {
                            docsErrorMsg += item.FileName + ", ";
                        }
                        var attach = new FilingInboxAttachItem();
                        attach.FileName = item.FileName;
                        attach.EntityId = item.EntityId;
                        attach.DocumentType = item.DocumentTypeId;
                        attach.House = item.House;
                        attach.HouseNumber = item.HouseNumber;
                        attach.Description = item.Description;
                        attach.DocumentId = item.DocumentId;
                        attach.IsSharedWithAgent = item.IsSharedWithAgent;
                        attach.IsDigitallySign = item.IsDigitallySign;
                        item.IsSingleTick = false;
                        summary.Attaches.push(attach);
                        isValid = true;
                        if (ObjectsLocator.GlobalSetting.DeploymentStage == "logboxwe1" || ObjectsLocator.GlobalSetting.DeploymentStage == "Test2" && this.EntityObjectTableName == "Shipment") {
                            if (AppTool.IsNullOrEmpty(item.Description)) {
                                isDescriptionFilled = false;
                            }
                        }
                    }
                });

                if (isValid && isDescriptionFilled) {
                    if (!AppTool.IsNullOrEmpty(this.EntityNumber)) {
                        if (!AppTool.IsNullOrEmpty(docsErrorMsg)) {
                            this.CurrentSession.StopBusyIndicator();
                            if (docsErrorMsg.match(/,/g).length == 1) {
                                docsErrorMsg = docsErrorMsg.replace(/,/g, '');
                                docsErrorMsg = "The document " + docsErrorMsg + " is already filled";
                            }
                            else {
                                docsErrorMsg = "The documents: " + docsErrorMsg + " are already filled";
                            }
                            var confirmWindow = new ConfirmWindow();
                            confirmWindow.NoButtonText = "Cancel";
                            confirmWindow.YesButtonText = "Ok";
                            confirmWindow.Title = "Warning";
                            confirmWindow.Show(docsErrorMsg);
                            confirmWindow.WindowClosed.subscribe((event: any) => {
                                if (confirmWindow.Yes) {
                                    this.CompleteFiling(summary, arg);
                                }
                            });
                        }
                        else {
                            this.CompleteFiling(summary, arg);
                        }
                    }
                    else {
                        this.CurrentSession.StopBusyIndicator();
                        this.ShowMessage("Please Enter Entity number");
                    }
                }
                else {
                    this.CurrentSession.StopBusyIndicator();
                    var msg = "";
                    if (ObjectsLocator.GlobalSetting.DeploymentStage == "logboxwe1" || ObjectsLocator.GlobalSetting.DeploymentStage == "Test2" && this.EntityObjectTableName == "Shipment") {
                        if (!isDescriptionFilled) {
                            msg += "Please fill Description fields for all attachments. ";
                        }
                    }
                    if (!isValid) {
                        msg += "Please enter at least one document type.";
                    }
                    this.ShowMessage(msg);

                }
            }
            else {
                this.CurrentSession.StopBusyIndicator();
                this.ShowMessage("Please Enter Entity number");
            }
        }
    }
    CompleteFiling(summary: FilingInboxSummary, arg: boolean) {
        summary.EntityId = this.EntityId;
        summary.EntityNumber = this.EntityNumber;
        summary.FilingId = this.SelectedFilingInbox.FilingInboxPM.Id;
        this.myCommonDomainService.PutFilingInboxLogs(summary).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                this.ClearConnectToFilter();
                this.IsVisible = false;
                this.LoadAllData();
            }
            this.CurrentSession.StopBusyIndicator();
        });

        if (arg == true) {
            // open dsv window
            var hasSharedDocs;
            this._ShipmentPMService = new ShipmentPMService();
            this._ShipmentPMService.get(this.EntityId).subscribe(myResult => {
                if (!myResult.HasError) {
                    if (SessionLocator.PrivateLableSettings) {
                        this._documentsFilingExtendedPMService = new DocumentsFilingExtendedPMService();
                        this._documentsFilingExtendedPMService.IsEntityHasSharedDocs(this.EntityId, SessionLocator.Tenant).subscribe(res => {
                            if (res.Result == false && summary.Attaches.filter(a => a.IsSharedWithAgent == true).length == 0) {
                                hasSharedDocs = false;
                            }
                            else {
                                hasSharedDocs = true;
                            }
                            this.CurrentSession.StopBusyIndicator();
                            var newWindow = new LogitudeWindow();
                            newWindow.Width = 1050;
                            newWindow.Height = 700;
                            if (SessionLocator.PrivateLableSettings) {
                                newWindow.Title = "Connect/Create new shipment in " + SessionLocator.PrivateLableSettings.PrivateLabelShortName;
                            }
                            else {
                                newWindow.Title = "Connect To Agent Shipment";
                            }

                            var windowArgs: any = {};
                            windowArgs.SourceEntity = myResult.Result;//this.rowData;
                            windowArgs.HasSharedDocs = hasSharedDocs;
                            newWindow.WindowArgs = windowArgs;
                            newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/ForwarderShipmentsComponent');
                        });
                    }
                }
            });
        }
    }
    ShowMessage(msg) {
        var myMessageWindow = new MessageWindow();
        myMessageWindow.Show(msg);
    }

    // New Shipment
    private entityResourceService: EntityResourceService;
    NewShipmentClicked() {
        this.entityResourceService.getEntityResourceByTableName("Shipment", 0).subscribe(response => {
            var newWindow = new LogitudeWindow();
            newWindow.Width = 600;
            newWindow.Height = 350;
            newWindow.Title = "Create New Shipment";
            var windowArgs: any = {};
            windowArgs.IsNew = true;
            newWindow.WindowArgs = windowArgs;
            if (SessionLocator.PrivateLableSettings) {
                newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/AddEditPrivateLabelShipmentComponent');
            }
            else if (ObjectsLocator.GlobalSetting.DeploymentStage == "logboxwe1" || ObjectsLocator.GlobalSetting.DeploymentStage == "Test2") {
                newWindow.Show('./ShipmentModules/ShipmentLogBox/Components/Logbox/AddEditImporterShipmentComponent');
            }
            else {

                var str: string = TextCodeTranslator.Translate("General.O.NewEntity");
                str = str.replace("%Entity", TextCodeTranslator.TranslateTable("Shipment"));
                newWindow.Title = str;
                newWindow.Width = 960;
                newWindow.Height = 570;
                newWindow.Show('./Shipment/Components/NewShipment/NewShipmentComponent');
            }

            newWindow.ComponentLoaded.subscribe(s => {
                newWindow.WindowClosed.subscribe(d => {
                    var shipment = s.EntityPM;
                    if (shipment != null) {
                        this.EntityId = shipment.Id;
                        if ((ObjectsLocator.GlobalSetting.DeploymentStage == "logboxwe1" || ObjectsLocator.GlobalSetting.DeploymentStage == "Test2") || SessionLocator.PrivateLableSettings) {
                            this.EntityNumber = AppTool.IsNullOrEmpty(shipment.ForwarderShipmentNumber) ? shipment.CustomerReference1 : shipment.ForwarderShipmentNumber;
                            if (AppTool.IsNullOrEmpty(shipment.ForwarderShipmentNumber) && !AppTool.IsNullOrEmpty(shipment.StatusName) && shipment.StatusName.toLocaleLowerCase() != "in progress") {
                                this.IsDSVConnectEnable = true;
                            }
                            else {
                                this.IsDSVConnectEnable = false;
                            }
                        }
                        else {
                            this.EntityNumber = shipment.ShipmentNumber;
                        }
                    }
                });
            });
        });
    }

    IsStopPreviewHtml: boolean = false;
    get IsSetHtml() {
        var isSetHtml = false;
        var element = document.getElementById(this.PreviewDivId);
        if (element) {
            isSetHtml = true;
            if (!this.IsStopPreviewHtml) {
                this.SetHtml();
                this.IsStopPreviewHtml = true;
            }
        }
        return isSetHtml;
    }


    DSVConnectClicked() {
        this.FileAndDeleteButtonClicked(true);
    }
}
export class FilingInboxData {

    public FilingInboxPM: FilingInboxPM;
    public FilingInboxAttachments: FilingInboxAttachment[] = [];
    public Background = "White";
    public HasAttachmanets = false;
    constructor(filingInboxPM: FilingInboxPM, public father: FilingInboxWorkspaceComponent) {
        this.FilingInboxPM = filingInboxPM;
        this.FillFilingInboxAttachments();
        this.GetHasAttachmanets();
        if (this.IsDeleted) {
            this.Background = "#F7E3E3";
        }
    }

    FillFilingInboxAttachments() {
        this.FilingInboxAttachments = [];
        this.FilingInboxAttachments = [];
        //this.FilingInboxPM.FilingInboxAttachments.filter(a => a.FileName != null && (a.FileName.split('.')[1] != null && a.FileName.split('.')[1].toUpperCase() == "PDF")).forEach(item => {
        //    this.FilingInboxAttachments.push(new FilingInboxAttachment(item, this.father));
        //});
        //this.FilingInboxPM.FilingInboxAttachments.filter(a => a.FileName != null && (a.FileName.split('.')[1] != null && a.FileName.split('.')[1].toUpperCase() != "PDF")).forEach(item => {
        //    this.FilingInboxAttachments.push(new FilingInboxAttachment(item, this.father));
        //});

        this.FilingInboxPM.FilingInboxAttachments.filter(a => a.FileName != null).forEach(item => {

            this.FilingInboxAttachments.push(new FilingInboxAttachment(item, this.father));
        });
    }
    GetHasAttachmanets() {
        if (this.FilingInboxAttachments.length > 0) {
            this.HasAttachmanets = true;
        }
        else {
            this.HasAttachmanets = false;
        }
    }

    get Subject() {
        return this.FilingInboxPM.Subject;
    }
    set Subject(value: string) {
        if (this.FilingInboxPM.Subject != value) {
            this.FilingInboxPM.Subject = value;
        }
    }

    get IsDeleted() {
        return this.FilingInboxPM.IsDeleted;
    }
    set IsDeleted(value: boolean) {
        if (this.FilingInboxPM.IsDeleted != value) {
            this.FilingInboxPM.IsDeleted = value;
        }
    }

    get CreateDate() {
        return this.FilingInboxPM.CreateDate;
    }
    set CreateDate(value: Date) {
        if (this.FilingInboxPM.CreateDate != value) {
            this.FilingInboxPM.CreateDate = value;
        }
    }

    get SenderName() {
        return this.FilingInboxPM.SenderName;
    }
    set SenderName(value: string) {
        if (this.FilingInboxPM.SenderName != value) {
            this.FilingInboxPM.SenderName = value;
        }
    }
}
export class FilingInboxAttachment extends BaseComponent {

    public entity: FilingInboxAttachmentPM;
    public DataContext: FilingInboxAttachment = this;
    public AttachLogs: any[] = [];
    public QuickSearchHouses: any[] = [];
    public HouseFilters: ApiQueryFilters = null;
    public IsSingleTick = false;

    private cellTooltipIconPath = "./Images/double-tick.png";
    get CellTooltipIconPath() {
        return this.cellTooltipIconPath;
    }
    set CellTooltipIconPath(value: string) {
        if (this.cellTooltipIconPath != value) {
            this.cellTooltipIconPath = value;
        }
    }

    constructor(entity: FilingInboxAttachmentPM, public father: FilingInboxWorkspaceComponent) {
        super();
        this.entity = entity;

        this.AttachLogs = entity != null ? entity.AttachLogs : [];
        if (this.AttachLogs != null && this.AttachLogs.length > 0) {
            this.IsSingleTick = false;
        }

        this.IsSharedWithAgent = father.ShareAsDefault;

        this.CalculatingWidth();
        this.HouseFilters = new ApiQueryFilters();
        this.HouseFilters.PageIndex = 0;
        this.HouseFilters.PageSize = 10;
        this.SetDescriptionUIProperties();
        this.FillDocumentFiling();
    }

    SetDescriptionUIProperties() {
        if (ObjectsLocator.GlobalSetting.DeploymentStage == "logboxwe1" || ObjectsLocator.GlobalSetting.DeploymentStage == "Test2" && this.father.EntityObjectTableName == "Shipment") {
            // var isreq = AppTool.IsNullOrEmpty(this.Description);
            //this.UIProperties.SetRequired("Description", null, isreq);
        }
    }

    public EntityNumberColumnWidth: number = 100;
    CalculatingWidth() {
        var entityNumberColumnWidth = 100;
        if (this.AttachLogs != null && this.AttachLogs.length > 0) {
            this.AttachLogs.forEach((item) => {
                if (!AppTool.IsNullOrEmpty(item.EntityNumber)) {
                    var textWidth = AppTool.GetTextWidth(item.EntityNumber, 12) + 10;
                    if (textWidth > entityNumberColumnWidth) {
                        entityNumberColumnWidth = textWidth;
                    }
                }
            })
        }

        this.EntityNumberColumnWidth = entityNumberColumnWidth;
    }

    get EntityId() {
        if (this.entity != null) {
            return this.entity.Id;
        }
        else {
            return null;
        }
    }
    get FileName() {
        if (this.entity != null) {
            return this.entity.FileName;
        }
        else {
            return null;
        }
    }
    get DocumentId() {
        if (this.entity != null) {
            return this.entity.DocumentId;
        }
        else {
            return null;
        }
    }

    private house: string = null;
    get House() {
        return this.house;
    }
    set House(value: string) {
        if (this.house != value) {
            this.house = value;
        }
    }

    private houseNumber: string = null;
    get HouseNumber() {
        return this.houseNumber;
    }
    set HouseNumber(value: string) {
        if (this.houseNumber != value) {
            this.houseNumber = value;
        }
    }

    private description: string = null;
    get Description() {
        return this.description;
    }
    set Description(value: string) {
        if (this.description != value) {
            this.description = value;
            this.SetDescriptionUIProperties();
        }
    }

    SetHousesFilters() {
        this.HouseFilters = new ApiQueryFilters();
        this.HouseFilters.PageIndex = 0;
        this.HouseFilters.PageSize = 100;
        if (!AppTool.IsNullOrEmpty(this.father.EntityId)) {
            this.HouseFilters.addAdditionalFilter("MasterShipmentDataId", this.father.EntityId, null, null, "Equals", false, false, false, "string");
        }
        this.HouseFilters.addAdditionalFilter("ShipmentLevelCode", "H", null, null, "Equals", false, true, false, "string");
        this.HouseFilters.addAdditionalFilter("MasterConnectedHouses", true, null, null, "Equals", true, false, false, "Boolean");
    }
    QuickSearchHouseChanged(entity: any) {
        if (entity != null) {
            this.House = entity.Id;
            this.HouseNumber = entity.ShipmentNumber;
        }
    }

    private documentTypeId: string = null;
    get DocumentTypeId() {
        return this.documentTypeId;
    }
    set DocumentTypeId(value: string) {
        if (this.documentTypeId != value) {
            this.documentTypeId = value;
        }
    }

    // Document Filling 
    ShowTypes: boolean = false;
    SelectedValue: string = "";
    SelectedName: string = "";
    TypeSelected: boolean = false;
    FillDocumentFiling() {
        if (AppTool.IsNullOrEmpty(this.DocumentTypeId)) {
            this.SelectedValue = "";
            this.ShowTypes = false;
        }
        else {
            if (this.father.DocumentTypeList.filter(a => a.Id == this.DocumentTypeId).length == 0) {
                this.SelectedValue = "O";
                this.ShowTypes = false;
                this.TypeSelected = true;
                this.SelectedName = this.DocumentType.Name;
            }
            else {
                var myTempData = this.father.DocumentTypeList.filter(a => a.Id == this.DocumentTypeId)[0];
                this.DocumentTypeId = myTempData.Id;
                this.SelectedValue = "";
                this.ShowTypes = false;
                this.TypeSelected = true;
                this.SelectedName = myTempData.Name;
            }
        }
    }

    itemClicked(itemValue: string, Name: string) {
        if (this.DocumentTypeId != itemValue) {
            if (itemValue == "O") {
                this.ShowTypes = true;
                this.SelectedValue = "O";
                this.DocumentTypeId = "";
                this.TypeSelected = false;
                this.SelectedName = "";
            }
            else {
                this.DocumentTypeId = itemValue;
                this.ShowTypes = false;
                this.SelectedValue = itemValue;
                this.TypeSelected = true;
                this.SelectedName = Name;
            }

            this.IsSingleTick = true;
        }
    }
    RedxClick() {
        this.SelectedName = "";
        this.TypeSelected = false;
        this.DocumentTypeId = "";
        if (this.SelectedValue == "O") {
            this.ShowTypes = true;
        }
        this.IsSingleTick = false;
    }
    private documentType: DocumentsFilingPM;
    public get DocumentType() { return this.documentType }
    public set DocumentType(newValue: DocumentsFilingPM) {
        this.documentType = newValue;
        if (newValue) {
            this.DocumentTypeId = newValue.Id;
        }
    }
    OnDocumentTypeChanged(event) {
        if (event) {
            this.DocumentType = event;
            this.TypeSelected = true;
            this.SelectedName = event.Name;
            this.ShowTypes = false;
        }
    }

    private isSharedWithAgent = false;
    get IsSharedWithAgent() {
        return this.isSharedWithAgent;
    }
    set IsSharedWithAgent(value: boolean) {
        if (this.isSharedWithAgent != value) {
            this.isSharedWithAgent = value;
        }
    }

    private isDigitallySign = false;
    get IsDigitallySign() {
        return this.isDigitallySign;
    }
    set IsDigitallySign(value: boolean) {
        if (this.isDigitallySign != value) {
            this.isDigitallySign = value;
        }
    }
}
