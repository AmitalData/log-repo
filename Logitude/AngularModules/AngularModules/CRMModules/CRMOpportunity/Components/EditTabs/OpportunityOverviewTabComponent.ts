import {Component, OnInit} from '@angular/core';
import {OpportunityPM} from '../../../../CRM/EntityPMs/OpportunityPM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {OpportunityPMService} from '../../../../CRM/Services/StandardPMs/OpportunityPMService';
import {UserList} from '../../../../Common/EntityLists/UserList';
import {UserListService} from '../../../../Common/Services/StandardLists/UserListService';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {OpportunityTypeList} from '../../../../CRM/EntityLists/OpportunityTypeList';
import {OpportunityTypeListService} from '../../../../CRM/Services/StandardLists/OpportunityTypeListService';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService'
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {OpportunityPMInitService} from '../../../../CRM/EntityPMInitServices/OpportunityPMInitService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {NewQuoteComponentArgs} from '../../../../Quote/Args';
import {QuotePM} from '../../../../Quote/EntityPMs/QuotePM';
import {QuoteList} from '../../../../Quote/EntityLists/QuoteList';
import {QuoteDomainService} from '../../../../Quote/Services/QuoteDomainService';
import {AdditionalServiceListService} from '../../../../Common/Services/StandardLists/AdditionalServiceListService';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {AdditionalServiceList} from '../../../../Common/EntityLists/AdditionalServiceList';
import {OpportunityAdditionalServicePM} from '../../../../CRM/EntityPMs/OpportunityAdditionalServicePM';
import {CompetitorListService} from '../../../../Common/Services/StandardLists/CompetitorListService';
import {CompetitorList} from '../../../../Common/EntityLists/CompetitorList';
import {OpportunityCompetitorPM} from '../../../../CRM/EntityPMs/OpportunityCompetitorPM';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {ActivityList} from '../../../../CRM/EntityLists/ActivityList';
import {DateTimePipe} from '../../../../Controls/Pipes/DateTimePipe';
import {CRMDomainService} from '../../../../CRM/Services/CRMDomainService';
import {GeneralEmailSender} from '../../../../Infrastructure/Helpers/GeneralEmailSender';
import {CRMTool} from '../../../../CRM/Tools';
import {ActivityInputArgs} from '../../../../CRM/Args'; 
import {StageList} from '../../../../CRM/EntityLists/StageList';
import {StageListService} from '../../../../CRM/Services/StandardLists/StageListService';
import {RatingList} from '../../../../CRM/EntityLists/RatingList';
import {RatingListService} from '../../../../CRM/Services/StandardLists/RatingListService';
import {ListComponentArgs} from '../../../../Infrastructure/Args';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceLocator} from '../../../../Infrastructure/Locators/ServiceLocator';

@Component({
    selector: 'OpportunityOverviewTabComponent',
    moduleId: module.id,
    templateUrl: './OpportunityOverviewTabComponent.html',
})

export class OpportunityOverviewTabComponent extends BaseComponent implements OnInit {
    public ObjectTableName: string = "Opportunity";
    public DataContext: OpportunityOverviewTabComponent = this;
    public EntityPM: OpportunityPM;
    public QuotesObslist: Array<QuoteObslistItemClass> = [];
    RegardingEntity: string = "";
    EntityId: string = "";
    EntityDescription: string = "";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(entityArgs: EntityArgs, private _entityResourceService: EntityResourceService) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.EntityId = this.EntityPM.Id;
        this.EntityDescription = this.EntityPM.Subject;
        this.RegardingEntity = "Regarding Opportunity : " + this.EntityPM.Subject;

        this.Listen();
        this.SetUIProperties();
        this.LoadActivities();
    }

    public IsEditingEnabled: boolean = true;
    public IsEditingEnabledWithoutCancel: boolean = true;

    SetUIProperties() {
        var fieldsIsEnabled: boolean = true;
        if (this.EntityPM.IsClosed || this.EntityPM.IsCancelled) {
            fieldsIsEnabled = false;           
        }

        this.IsEditingEnabled = fieldsIsEnabled;
        this.IsEditingEnabledWithoutCancel = true;
        this.UIProperties.SetEnabled("Subject", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("CustomerId", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("ContactId", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("EstimatedClosingDate", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("StageId", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("Probability", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("RatingCode", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("OwnerId", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("CurrencyId", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("Description", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("StageDueDate", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("LeadUserId", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("LeadSourceId", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("AgentId", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("LeadPartnerId", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("LeadDescription", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("ForeignClientId", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("ClosingReasonId", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("Notes", "Opportunity", true);

        this.SetUIProperties_TotalsOfProducts();


        this.SearchTextCompetitorsDropButtonId += "2" + this.CurrentSession.GetNewId("SearchTextCompetitorsDropButtonId_2");
        this.SearchTextCompetitorsId += "2" + this.CurrentSession.GetNewId("SearchTextCompetitorsId_2");
        this.SearchTextAdditionalServiceModeDropButtonId += "2"+ this.CurrentSession.GetNewId("SearchTextAdditionalServiceModeDropButtonId_2");
        this.SearchTextAdditionalServiceId += "2" + this.CurrentSession.GetNewId("SearchTextAdditionalServiceId_2");

    }
    SetUIProperties_TotalsOfProducts() {
        var fieldsIsEnabled: boolean = true;

        if (this.EntityPM.IsClosed || this.EntityPM.IsCancelled) {
            fieldsIsEnabled = false;
        }

        if (fieldsIsEnabled) {
            if (this.EntityPM.OpportunityProducts.length > 0) {
                fieldsIsEnabled = false;
            }
        }
        this.UIProperties.SetEnabled("NumberOfShipments", "Opportunity", fieldsIsEnabled);

    }

    public SearchTextAdditionalServiceModeDropButtonId: string = "SearchTextAdditionalServiceModeDropButtonId";
    public SearchTextAdditionalServiceId: string = "SearchTextAdditionalServiceId";
    public SearchTextCompetitorsDropButtonId: string = "SearchTextCompetitorsDropButtonId";
    public SearchTextCompetitorsId: string = "SearchTextCompetitorsId";
    public CompetitorToggleButtonList: any = [];
    public AllCompetitors: Array<CompetitorList> = [];
    public Competitors: Array<CompetitorViewModelData> = [];
    public searchTextCompetitor: string = null;
    public ProductsToggleButtonList: any = [];
    public get SearchTextCompetitor() { return this.searchTextCompetitor; }
    public set SearchTextCompetitor(newValue: string) {
        this.searchTextCompetitor = newValue;
        this.BuildCompetitorToggleButtonList();
    }

    BuildCompetitorToggleButtonList() {
        this.CompetitorToggleButtonList = [];
        var data: Array<any> = this.AllCompetitors;
        if (!AppTool.IsNullOrEmpty(this.SearchTextCompetitor))
            data = this.AllCompetitors.filter(f => f.Name.toLowerCase().indexOf(this.SearchTextCompetitor.toLowerCase()) > -1);
        data.forEach(item => {
            this.CompetitorToggleButtonList.push(new CompetitorItemClass(item, this.EntityPM, this));

        });
    }
    BuildCompetitorsObsList() {
        this.Competitors = [];
        this.EntityPM.OpportunityCompetitors.forEach(item => {
            this.Competitors.push(new CompetitorViewModelData(item, this));
        });

        if (this.Competitors.length == 0)
            this.NoCompetitorVisibility = true;
        else
            this.NoCompetitorVisibility = false;
    }
    DeleteItemCompetitorList(Item: CompetitorViewModelData) {
        if (this.EntityPM.OpportunityCompetitors.filter(p => p.CompetitorId == Item.CompetitorId)[0] != null)
            this.EntityPM.RemoveOpportunityCompetitor(this.EntityPM.OpportunityCompetitors.filter(d => d.CompetitorId == Item.CompetitorId)[0]);
        this.BuildCompetitorToggleButtonList();
        this.BuildCompetitorsObsList();

    }

    private myCloner: Cloner;
    private Clone(EntityPM: any) {
        this.myCloner = new Cloner(EntityPM);
        this.myCloner.AddField('Notes');
        this.myCloner.AddEntity(EntityPM);
        this.myCloner.AddEntity(this.EntityPM);

    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }

    EditCompetitor(Item: CompetitorViewModelData) {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: Item.CompetitorId, ObjectTableName: 'Competitor', BackButtonLabel: "CRM Details" });
                cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                    this.getCompetitorList();
                });
            });


    }

    EditItemServiceObsList(item: ServiceViewModelData) {
        var editWindow: LogitudeWindow = new LogitudeWindow();
        editWindow.Title = "Edit " + item.AdditionalServiceName + " Additional Service";
        editWindow.Width = 500;
        editWindow.Height = 350;
        editWindow.WindowArgs = item;
        editWindow.ComponentLoaded.subscribe(p => {
            p.ShowRadioButtons = false;

        });
        this.Clone(item);
        editWindow.WindowClosed.subscribe(result => {
            if (result == "Cancel") {
                this.RejectChanges();
            }
            else {

            }

        });
        var entityResource: EntityResourceService = new EntityResourceService();
        entityResource.getEntityResourceByTableName("CustomerAdditionalService", 0).subscribe(p => {
            editWindow.Show('./CommonModules/CommonCustomer/Components/EditTabs/EditCustomerAdditionalServiceComponent');
        });
    }

    ClearPlaceHolderCompetitor() {
        var temp = document.getElementById(this.SearchTextCompetitorsId) as HTMLInputElement;
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
        var ToggleBTN = document.getElementById(this.SearchTextCompetitorsDropButtonId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenuTemp";

        //this.CD.detectChanges();
    }

    OnDeleteValueCompetitor() {
        var temp = document.getElementById(this.SearchTextCompetitorsId) as HTMLInputElement;
        temp.value = null;
        this.SearchTextCompetitor = null;
        temp.focus();
    }

    OnDeleteValueAddtionalService() {
        var temp = document.getElementById(this.SearchTextAdditionalServiceId) as HTMLInputElement;
        temp.value = null;
        this.SearchTextAdditionalService = null;
        temp.focus();
    }


    FillPlaceHoldeCompetitor() {
        if (!this.SearchTextCompetitor) {
            var temp = document.getElementById(this.SearchTextCompetitorsId) as HTMLInputElement;
            temp.placeholder = "Search";
            temp.style.background = "url(Images/Search.png) no-repeat scroll";
            temp.style.backgroundPosition = "right center";
            temp.style.paddingRight = "30px";
        }
        var ToggleBTN = document.getElementById(this.SearchTextCompetitorsDropButtonId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenu";
    }

    setToggleButtonMenuAdditionalServicesTemp() {
        var ToggleBTN = document.getElementById(this.SearchTextAdditionalServiceModeDropButtonId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenuTemp";
    }

    setToggleButtonMenuAdditionalServices() {
        var ToggleBTN = document.getElementById(this.SearchTextAdditionalServiceModeDropButtonId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenu";
    }

    LoadQuotesList() {
        this.QuotesObslist = [];
        var quoteDomainService: QuoteDomainService = new QuoteDomainService();
        quoteDomainService.GetQuotesByOpportunityId(this.EntityPM.Id).subscribe(result => {
            var quoteList: Array<QuoteList> = result.Result;
            quoteList.sort((a, b) => {
                return (DateTool.GetDateParts(a.OpenDate).DateObject === DateTool.GetDateParts(b.OpenDate).DateObject) ? 0 : (DateTool.GetDateParts(a.OpenDate).DateObject < DateTool.GetDateParts(b.OpenDate).DateObject) ? -1 : 1
            });
            quoteList.reverse();
            quoteList.forEach(item => {
                this.QuotesObslist.push(new QuoteObslistItemClass(item));
            });

        });

    }

    public get NoQuotesVisibility() { if (this.QuotesObslist.length > 0) return true; return false; }

    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    // Your work after you get PM
                    if (this.SavingMethodCode == "NewQuote")
                        this.OpenNewQuote();
                    this.SetUIProperties();
                    this.SavingMethodCode = "";
                    this.CurrentSession.FireEvent("SocialPostsRefresh");
                }
            });

            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    this.SetUIProperties();
                    // Your work after you get PM
                }
            });

            this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "LoadActivity") {
                    this.LoadActivities();
                }
            });
        }
    }
    public ToggleButtonListService: any = [];

    getAdditionalSerivceList() {
        var AddtionalService: AdditionalServiceListService = new AdditionalServiceListService();
        AddtionalService.getAllFromCache().subscribe(result => {
            this.ToggleButtonListService = [];
            this.ToggleButtonListService = result.Result.filter(s => !s.InActive);
            this.ToggleButtonListService.sort((a, b) => { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1 });
            this.BuildToggleButtonList();
            this.BuildObsList();
        });
    }
    public ToggleButtonList: Array<ServiceItemClass> = [];
    public searchTextAdditionalService: string = null;
    public get SearchTextAdditionalService() { return this.searchTextAdditionalService; }
    public set SearchTextAdditionalService(newValue: string) {
        this.searchTextAdditionalService = newValue;
        this.BuildToggleButtonList();
    }
    BuildToggleButtonList() {
        this.ToggleButtonList = [];
        var data: Array<any> = this.ToggleButtonListService;
        if (!AppTool.IsNullOrEmpty(this.SearchTextAdditionalService))
            data = this.ToggleButtonListService.filter(f => f.Name.toLowerCase().indexOf(this.SearchTextAdditionalService.toLowerCase()) > -1);
        data.forEach(item => {
            this.ToggleButtonList.push(new ServiceItemClass(item, this.EntityPM, this));

        });
    }

    public get IsAddRemoveEnabled() {
        return true;
    }

    public get RatingCode() { return this.EntityPM.RatingCode; }
    public set RatingCode(value: string) {
        if (this.EntityPM.RatingCode != value) {
            this.EntityPM.RatingCode = value;

            var ratingListService: RatingListService = new RatingListService();
            ratingListService.getAllFromCache().subscribe(result => {
                var ratingList: RatingList = result.Result.filter(p => p.Code == value)[0];
                if (ratingList != null)
                    this.EntityPM.RatingName = ratingList.Name;
                else
                    this.EntityPM.RatingName = null;
            });

        }
    }

    public get StageId() {
        return this.EntityPM.StageId;
    }
    public set StageId(value: string) {
        if (this.EntityPM.StageId != value) {
            this.EntityPM.StageId = value;
            
            this.OnStageChanged();

        }


    }

    public SavingMethodCode: string = "";

    NewQuote() {
        this.SavingMethodCode = "NewQuote";
        this.CurrentSession.CurrentEditComponent.SaveChanges();
    }

    EditQuote(item: QuoteObslistItemClass) {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: item.entityId, ObjectTableName: 'Quote', BackButtonLabel: "Quotes" });
                cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                    this.LoadQuotesList();
                });
            });

    }
    ClearPlaceHolderAdditionalService() {
        var temp = document.getElementById(this.SearchTextAdditionalServiceId) as HTMLInputElement;
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
        var ToggleBTN = document.getElementById(this.SearchTextAdditionalServiceModeDropButtonId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenuTemp";

        //this.CD.detectChanges();
    }

    FillPlaceHolderAdditionalService() {
        if (!this.SearchTextAdditionalService) {
            var temp = document.getElementById(this.SearchTextAdditionalServiceId) as HTMLInputElement;
            temp.placeholder = "Search";
            temp.style.background = "url(Images/Search.png) no-repeat scroll";
            temp.style.backgroundPosition = "right center";
            temp.style.paddingRight = "30px";
        }
        var ToggleBTN = document.getElementById(this.SearchTextAdditionalServiceModeDropButtonId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenu";
    }
    OpenNewQuote() {
        var args = new NewQuoteComponentArgs();
        args.DefaultCustomerId = this.EntityPM.CustomerId;
        args.OpportunityId = this.EntityPM.Id;
        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.WindowArgs = args;
        logWindow.Title = "Create New Quote";
        this._entityResourceService.getEntityResourceByTableName("Quote", 0).subscribe(response => {
            logWindow.Show('./Quote/Components/NewEntity/NewQuoteComponent');

            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.LoadQuotesList();
                }
            });
        });



    }

    public get Notes() { return this.EntityPM.Notes; }
    public set Notes(value: string) { if (this.EntityPM.Notes != value) this.EntityPM.Notes = value; }

    public get StageDueDate() { return this.EntityPM.StageDueDate; }
    public set StageDueDate(value: Date) {
        if (this.EntityPM.StageDueDate != value) {
            this.EntityPM.StageDueDate = value;
        }
    }

    public get Probability() {
        return this.EntityPM.Probability;
    }
    public set Probability(value: number) {
        if (this.EntityPM.Probability != value) {
            this.EntityPM.Probability = value;
            this.ComputeValue();
        }
    }
    public get NumberOfShipments() {
        return this.EntityPM.NumberOfShipments;
    }
    public set NumberOfShipments(value: number) {
        if (this.EntityPM.NumberOfShipments != value) {
            this.EntityPM.NumberOfShipments = value;
            this.ComputeValue();
        }
    }

    public get IsClosed() { return this.EntityPM.IsClosed; }

    ConnectQuotesMethod() {

        var logWindow = new LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.WindowArgs = this.EntityPM;
        logWindow.Title = "Choose Quotes";
        this._entityResourceService.getEntityResourceByTableName("Quote", 0).subscribe(response => {
            logWindow.Show('./CRMModules/CRMOpportunity/Components/EditTabs/QuotesWindowComponent');
            logWindow.WindowClosed.subscribe(s => {
                if (s == "ok") {
                    this.LoadQuotesList();
                }
            });
        });



    }
    public get ClosingBackground() {
        var myResult = "#FFFFFFFF";
        if (this.EntityPM.IsClosed) {
            myResult = "#AA009161";
        }
        if (this.EntityPM.IsClosedLost) {
            myResult = "#AAFFB23C";
        }
        return myResult;
    }

    public get ClosingDescription() { return this.EntityPM.ClosingDescription; }
    public set ClosingDescription(value: string) { if (this.EntityPM.ClosingDescription != value) this.EntityPM.ClosingDescription = value; }

    ComputeValue() {


        var field1: number = this.Probability == null ? 0 : parseFloat(this.Probability + "");
        var field2: number = this.NumberOfShipments == null ? 0 : parseFloat(this.NumberOfShipments+"");
        var myValue: number = field1 * field2 / 100;

        this.ValueField = myValue;

    }

    public get ValueField() { return this.EntityPM == null ? null : this.EntityPM.ValueField; }
    public set ValueField(value: number) {
        if (this.EntityPM.ValueField != value) {
            this.EntityPM.ValueField = value;
        }
    }

    OnStageChanged() {

        var myStageName: string = null;
        var myProbability: number = null;
        var myStageDueDate: Date = DateTool.GetDateParts(this.EntityPM.LastStageDate).DateObject;
        var myStageAge: Date = DateTool.GetDateParts(this.EntityPM.LastStageDate).DateObject;


        var stageService: StageListService = new StageListService();
        stageService.getAllFromCache().subscribe(result => {
            var myStage: StageList = result.Result.filter(d => d.Id == this.StageId && d.Tenant == SessionLocator.Tenant)[0];


            if (myStage != null) {
                var todayDateTime: Date = DateTool.GetCurrentDateTimeAsUtc();

                myStageName = myStage.Name;
                myProbability = myStage.Probability;

                var fieldsIsEnabled: boolean = true;

                if (this.EntityPM.IsClosed || this.EntityPM.IsCancelled) {
                    fieldsIsEnabled = false;
                }

                if (fieldsIsEnabled) {
                    if (myStage.MaxDays != null) {
                       myStageDueDate.setDate(todayDateTime.getDate()+myStage.MaxDays);
                    }
                }

                myStageAge = todayDateTime;
            }

            this.EntityPM.StageName = myStageName;
            this.EntityPM.StageDueDate = myStageDueDate;
            this.EntityPM.Probability = myProbability;
            this.ComputeValue();


        });





    }

    ngOnInit() {
        this.LoadQuotesList();
        this.getAdditionalSerivceList();
        this.getCompetitorList();
    }
    getCompetitorList() {
        var competitorListService: CompetitorListService = new CompetitorListService();
        competitorListService.getAll().subscribe(result => {
            this.AllCompetitors = result.Result;
            this.BuildCompetitorToggleButtonList();
            this.BuildCompetitorsObsList();

        });

    }


    ExistingItemNotes(Item: ServiceViewModelData) {
        return AppTool.IsNullOrEmpty(Item.Notes);
    }

    DeleteItemServiceObsList(Item: ServiceViewModelData) {
        if (this.EntityPM.OpportunityAdditionalServices.filter(p => p.AdditionalServiceId == Item.Id)[0] != null)
            this.EntityPM.RemoveOpportunityAdditionalService(this.EntityPM.OpportunityAdditionalServices.filter(d => d.AdditionalServiceId == Item.Id)[0]);
        this.BuildToggleButtonList();
        this.BuildObsList();

    }

    private noServicesVisibility: boolean = false;
    public get NoServicesVisibility() { return this.noServicesVisibility; }
    public set NoServicesVisibility(value: boolean) { this.noServicesVisibility = value; }
    private servicesVisibility: boolean = false;
    public get ServicesVisibility() { return this.servicesVisibility; }
    public set ServicesVisibility(value: boolean) { this.servicesVisibility = value; }
    private noCompetitorVisibility: boolean = false;
    public get NoCompetitorVisibility() { return this.noCompetitorVisibility; }
    public set NoCompetitorVisibility(value: boolean) { this.noCompetitorVisibility = value; }

    public Services: Array<ServiceViewModelData> = [];

    BuildObsList() {
        this.Services = [];
        this.EntityPM.OpportunityAdditionalServices.forEach(item => {
            this.Services.push(new ServiceViewModelData(item, this));
        });
        this.NoServicesVisibility = this.Services.length == 0 ? true : false;
        this.ServicesVisibility = this.Services.length == 0 ? false : true;
        // this.ActivityWatch = this.Services.Count == 0 ? false : true;         
    }

    //Activities
    private  viewAllActivitiesIsEnabled;
    get ViewAllActivitiesIsEnabled() { return this.viewAllActivitiesIsEnabled; }
    set ViewAllActivitiesIsEnabled(value: boolean)
    {
        this.viewAllActivitiesIsEnabled = value; 
    }
    public ActivitiesContent: string = "";
    public ActivitiesList: ActivityItemClass[] = [];
    GetActivitiesContent() {
        var count = this.ActivitiesList.length;
        this.ViewAllActivitiesIsEnabled = count > 0 ? true : false;
        this.ActivitiesContent = "Activities (" + count + ")";
    }
    public IsAddActivityEnabled: boolean = true;
    AddActivity(code: string) {
        var windowTitle = "";
        var windowTitleIcon = "";
        var path = "";
        var logWindow = new LogitudeWindow();
        switch (code.toUpperCase()) {
            case "TS":
                {
                    windowTitle = "New Task";
                    windowTitleIcon = "./Images/Activities/TS.png";
                    break;
                }

            case "CL": {
                windowTitle = "New Phone Call";
                windowTitleIcon = "./Images/Activities/CL.png";
                break;
            }

            case "AP": {
                windowTitle = "New Appointment";
                windowTitleIcon = "./Images/Activities/AP.png";
                logWindow.Width = 800;
                logWindow.Height = 600;
                break;
            }

            default: { break; }
        }
        var windowArgs: ActivityInputArgs = new ActivityInputArgs();
        windowArgs.TypeCode = code;
        if (code.toUpperCase() == "CL") {
            windowArgs.CallWithId = this.EntityPM.ContactId;
            windowArgs.IsOpen = false;
            windowArgs.IsMarkedCompleted = true;
        }
        windowArgs.CustomerId = this.EntityPM.CustomerId;
        windowArgs.OpportunityId = this.EntityPM.Id;
        logWindow.Title = windowTitle;
        logWindow.TitleIcon = windowTitleIcon;
        logWindow.WindowArgs = windowArgs;

        this._entityResourceService.getEntityResourceByTableName("Activity", 0).subscribe(response => {
            logWindow.Show('./CRMModules/CRMActivity/Components/NewEntity/NewActivity/NewActivityComponent');
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.LoadActivities();
                }
            });
        });
    }

    EmailSender: GeneralEmailSender;
    AddEmail() {
        if (!this.EmailSender || (this.EmailSender && !this.EmailSender.LoadingSendingComponent)) {
            this.EmailSender = new GeneralEmailSender("Opportunity", "OPPO", this.EntityPM.Id, "", "", "", "", "", null, "LoadActivity", this.EntityPM, true, "OEMO");
            this.EmailSender.ShowFullSendControll();
        }
    }


    LoadActivities() {
        this.ActivitiesList = [];
        var myService: CRMDomainService = new CRMDomainService();
        myService.GetActivitiesByOpportunityId(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var dataResult: ActivityList[] = myResponse.Result;
                if (dataResult.length > 0) {
                    dataResult.filter(d => d.IsOpen).sort((a, b) => {
                        return (DateTool.GetDateParts(a.SortingDate).DateObject === DateTool.GetDateParts(b.SortingDate).DateObject) ? 0 : (DateTool.GetDateParts(a.SortingDate).DateObject > DateTool.GetDateParts(b.SortingDate).DateObject) ? -1 : 1
                    }).forEach(item => {
                        this.ActivitiesList.push(new ActivityItemClass(item, this));
                    });

                    dataResult.filter(d => !d.IsOpen).sort((a, b) => { return (DateTool.GetDateParts(a.SortingDate).DateObject === DateTool.GetDateParts(b.SortingDate).DateObject) ? 0 : (DateTool.GetDateParts(a.SortingDate).DateObject > DateTool.GetDateParts(b.SortingDate).DateObject) ? -1 : 1 }).forEach(item => {
                        this.ActivitiesList.push(new ActivityItemClass(item, this));
                    });
                }

                this.GetActivitiesContent();

            }
        });
    }
    ViewEntity(entity) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this._entityResourceService.getEntityResourceByTableName("Activity", 0).subscribe(response => {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({EntityId: entity.Id, ObjectTableName: "Activity", BackButtonLabel: "Opportunities" });
                        cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                            this.LoadActivities();
                        });
                    });
            });
        }
    }
    Updated(arg: boolean) {
        if (arg) {
            this.LoadActivities();
        }
    }

    AddTaskClicked() {
        var windowTitle = "New Task";
        var windowTitleIcon = "./Images/Activities/TS.png";
       
        var windowArgs: ActivityInputArgs = new ActivityInputArgs();
        windowArgs.TypeCode = "TS";
        windowArgs.IsAddCustomerAllowed = true;
        windowArgs.CustomerId = this.EntityPM.CustomerId;
        windowArgs.OpportunityId = this.EntityPM.Id;
        windowArgs.Subject = "due date reminder";
        windowArgs.DueDate = this.StageDueDate;

        var logWindow = new LogitudeWindow();
        logWindow.Title = windowTitle;
        logWindow.TitleIcon = windowTitleIcon;
        logWindow.WindowArgs = windowArgs;

        this._entityResourceService.getEntityResourceByTableName("Activity", 0).subscribe(response => {
            logWindow.Show('./CRMModules/CRMActivity/Components/NewEntity/NewActivity/NewActivityComponent');
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.LoadActivities();
                }
            });
        });
    }
    ViewAllData() {
        var objectTableName = "Activity";
        var queryCode = "All Activities";
        var filterAgrs = new ApiQueryFilters();

        filterAgrs.addAdditionalFilter("OpportunityId", this.EntityPM.Id, null, null, "Equals", false, false, false, "String");
        var listArgs = new ListComponentArgs();
        listArgs.Filters = filterAgrs;
        listArgs.QueryCode = queryCode;
        listArgs.ObjectTableName = objectTableName;
        listArgs.DisplayTitle = listArgs.QueryCode;
        listArgs.BackButtonTitle = "Back";
        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, SessionLocator.Tenant).subscribe(response => {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                });
        });
    }
}
export class ServiceViewModelData {

    private entityPM: OpportunityAdditionalServicePM;
    private trigger: OpportunityOverviewTabComponent;    

    public get AdditionalServiceName() { return this.entityPM.EnglishName; }

    public get Notes() {
        return this.entityPM.Notes;

    }
    public set Notes(value: string) {
        if (this.entityPM.Notes != value) {
            this.entityPM.Notes = value;
            ServiceLocator.SendTotangoUserActivity("Customer", "Notes update");
        }

    }

    private brushedNotesIconVisibility: boolean;
    public get BrushedNotesIconVisibility() {
        if (this.entityPM != null && !(this.entityPM.Notes == null || this.entityPM.Notes == ""))
            return true;
        return false;

    }

    private defaultNotesIconVisibility: boolean;
    public get DefaultNotesIconVisibility() {
        if (this.entityPM != null && (this.entityPM.Notes == null || this.entityPM.Notes == ""))
            return true;
        return false;

    }
    public get Id() { return this.entityPM.AdditionalServiceId }

    constructor(item: OpportunityAdditionalServicePM, trigger: OpportunityOverviewTabComponent) {
        this.entityPM = item;
        this.trigger = trigger;
    }
}
export class QuoteObslistItemClass{
    public entityId: string;
    private entityList: QuoteList;
    constructor(item: QuoteList) {
        this.entityList = item;
        this.entityId = item.Id;        
    }
    public get RatingCode() {return this.entityList.RatingCode; }
    public get RatingName() {return this.entityList.RatingName; }
    public get Subject() {return this.entityList.Subject; }
    public get FromCountryCode() {return this.entityList.FromCountryCode; }
    public get ToCountryCode() {return this.entityList.ToCountryCode; }
    public get OpenDate(){return this.entityList.OpenDate; }
    public get QuoteNumber(){return this.entityList.QuoteNumber; }
    public get DirectionId() {return this.entityList.DirectionId; }
    public get DirectionName() { return this.entityList.DirectionName; }
    public get TransportModeId() {return this.entityList.TransportModeId; }
    public get TransportModeName() {return this.entityList.TransportModeName; }
        

}
export class ServiceItemClass {
    private entityPM: OpportunityPM;
    private entityList: AdditionalServiceList;
    public TenantPM: TenantPM;
    public get Name() { return this.entityList.Name; }

    public get Foreground() { return this.IsChecked ? "#FF6E7172" : "#FF282E30"; }

    public get Id() { return this.entityList.Id; }


    constructor(itemList: AdditionalServiceList, itemPM: OpportunityPM, private Parent: any) {
        this.TenantPM = SessionLocator.TenantPM;
        this.entityPM = itemPM;
        this.entityList = itemList;
        this.isChecked = this.entityPM.OpportunityAdditionalServices.filter(d => d.AdditionalServiceId == this.entityList.Id)[0] != null;

    }
    private isChecked: boolean;
    public get IsChecked() { return this.isChecked; }
    public set IsChecked(value: boolean) {

        if (this.isChecked != value) {
            this.isChecked = value;


            if (value) {

                var notesRightToLeft = false;
                if (SessionLocator.TenantPM.IsNotesRightToLeftEnabled == true) {
                    notesRightToLeft = true;
                }

                var newItem: OpportunityAdditionalServicePM = new OpportunityAdditionalServicePM(null);

                newItem.Tenant = SessionLocator.Tenant;
                newItem.OpportunityId = this.entityPM.Id;
                newItem.AdditionalServiceId = this.Id;
                newItem.NotesRightToLeft = notesRightToLeft;


                var type: string = null;

                var addtionalService: AdditionalServiceListService = new AdditionalServiceListService();
                addtionalService.getSingleFromCache(this.Id).subscribe(result => {
                    var typeList = result.Result;
                    if (typeList != null) {
                        type = typeList.Name;
                    }
                    newItem.EnglishName = type;

                    if (!this.entityPM.OpportunityAdditionalServices.includes(newItem)) {
                        this.entityPM.AddOpportunityAdditionalService(newItem);
                    }

                });
            }

            else {
                var item: OpportunityAdditionalServicePM = this.entityPM.OpportunityAdditionalServices.filter(d => d.AdditionalServiceId == this.Id)[0];
                if (item != null) {
                    if (this.entityPM.OpportunityAdditionalServices.includes(item)) {
                        this.entityPM.RemoveOpportunityAdditionalService(item);
                    }
                }
            }
            this.Parent.BuildObsList();
            this.Parent.BuildToggleButtonList();

        }
    }


}
export class CompetitorViewModelData {

    private entityPM: OpportunityCompetitorPM;
    private trigger: OpportunityOverviewTabComponent;

    public get CompetitorId() { return this.entityPM.CompetitorId; }

    constructor(item: OpportunityCompetitorPM, trigger: OpportunityOverviewTabComponent) {
        this.entityPM = item;
        this.trigger = trigger;
    }

    public get Name() {
        var result: string = "";
        var list: CompetitorList = this.trigger.AllCompetitors.filter(d => d.Id == this.entityPM.CompetitorId)[0];
        if (list != null) {
            result = list.Name;
        }

        return result;


    }
}
class CompetitorItemClass {
    private trigger: OpportunityOverviewTabComponent;
    private entityPM: OpportunityPM;
    private entityList: CompetitorList;
    constructor(item: CompetitorList, entityPM: OpportunityPM, trigger: OpportunityOverviewTabComponent) {
        this.entityList = item;
        this.entityPM = entityPM;
        this.trigger = trigger;
        this.isChecked = entityPM.OpportunityCompetitors.filter(d => d.CompetitorId == this.entityList.Id)[0] != null;

    }
    public get Name() { return this.entityList.Name; }

    private isChecked: boolean;
    public get IsChecked() { return this.isChecked; }
    public set IsChecked(value: boolean) {
        if (this.isChecked != value) {
            this.isChecked = value;
            if (value) {
                var newItem: OpportunityCompetitorPM = new OpportunityCompetitorPM(null)
                newItem.Tenant = SessionLocator.Tenant;
                newItem.OpportunityId = this.entityPM.Id;
                newItem.CompetitorId = this.entityList.Id;
                newItem.EnglishName = this.entityList.Name;
                if (!this.entityPM.OpportunityCompetitors.includes(newItem)) {
                    this.entityPM.AddOpportunityCompetitor(newItem);
                }
            }

            else {

                var item: OpportunityCompetitorPM = this.entityPM.OpportunityCompetitors.filter(d => d.CompetitorId == this.entityList.Id)[0];

                if (item != null) {
                    if (this.entityPM.OpportunityCompetitors.includes(item)) {
                        this.entityPM.RemoveOpportunityCompetitor(item);
                    }
                }
            }

            this.trigger.BuildCompetitorsObsList();
            this.trigger.BuildToggleButtonList();


        }

    }
}
class ProductTypeList {

    public id: string;
    public code: string;
    public name: string;
    public inActive: boolean
    public searchFields: string;

    public get Id() { return this.id; }
    public set Id(value: string) { this.id = value; }

    public get Code() { return this.code; }
    public set Code(value: string) { this.code = value; }


    public get Name() { return this.name; }
    public set Name(value: string) { this.name = value; }


    public get InActive() { return this.inActive; }
    public set InActive(value: boolean) { this.inActive = value; }

    public get SearchFields() { return this.searchFields; }
    public set SearchFields(value: string) { this.searchFields = value; }

}
export class ActivityItemClass extends BaseComponent {
    public entity: ActivityList;
    public EntityId: string;

    constructor(item: ActivityList, public father: OpportunityOverviewTabComponent) {
        super();
        this.entity = item;
        this.EntityId = item.Id;
        this.ImageSrc = CRMTool.GetActivityImageSrc(this.entity.ActivityTypePathCode);
        this.GetDueDateForeground();
        this.GetDateValue();
        this.GetDateLable();
        this.GetAction();
        this.GetActionBy();
        this.getActionDate();
    }

    // Properties
    private background: string = "rgb(255,255,255)";
    get Background() {
        if (!this.entity.IsOpen) {
            this.background = "rgba(0,0,0,0.1)";
        }
        return this.background;
    }
    set Background(value: string) {
        this.background = value;
    }

    ControlIsEnabled() { return this.entity.IsOpen; }
    get ActivityTypePathCode() { return this.entity.ActivityTypePathCode; }
    get Id() { return this.entity.Id; }
    get CallWithId() { return this.entity.CallWithId; }
    get ActivityTypeName() { return this.entity.ActivityTypeName; }
    get ActivityTypeCode() { return this.entity.ActivityTypeCode; }
    get Subject() { return this.entity.Subject; }
    get Owner() { return this.entity.OwnerName; }
    get CustomerId() { return this.entity.CustomerId; }
    get SortingBy() { return this.entity.SortingBy; }
    get DueDate() { return this.entity.DueDate; }
    get StartDate() { return this.entity.StartDateTime; }
    get SortingDate() { return this.entity.SortingDate; }
    get MeetingSummary() { return this.entity.MeetingSummary; }
    set MeetingSummary(value: string) {
        if (this.entity.MeetingSummary != value) {
            this.entity.MeetingSummary = value;
        }
    }
    get PostToFollowers() { return this.entity.PostToFollowers; }
    set PostToFollowers(value: boolean) {
        if (this.entity.PostToFollowers != value) {
            this.entity.PostToFollowers = value;

        }
    }

    public DateLable: string = "";
    GetDateLable() {
        var myResult = "Due Date";
        switch (this.ActivityTypeCode) {
            case "TS":
                {
                    if (this.DueDate != null) {
                        myResult = "Due Date";
                    }

                    else {
                        myResult = "Start Date";
                    }

                    break;
                }

            case "AP":
                {
                    myResult = "Start Date";
                    break;
                }

            case "EO":
                {
                    myResult = "To";
                    break;
                }

            case "EI":
                {
                    myResult = "From";
                    break;
                }
        }

        this.DateLable = myResult;
    }

    public DateValue: string = "";
    GetDateValue() {
        var DatePipe = new DateTimePipe();
        var myResult = "";

        if (this.DueDate != null) {
            myResult = DatePipe.transform(this.DueDate, "SD");
        }

        switch (this.ActivityTypeCode) {
            case "TS":
                {
                    if (this.DueDate != null) {
                        myResult = DatePipe.transform(this.DueDate, "SD");

                    }

                    else if (this.StartDate != null) {
                        myResult = DatePipe.transform(this.StartDate, "SD");
                    }

                    break;
                }

            case "AP":
                {
                    if (this.StartDate != null) {
                        myResult = DatePipe.transform(this.StartDate, "SD");
                    }

                    break;
                }

            case "EO":
                {
                    myResult = this.entity.RecipientsEmails;
                    break;
                }

            case "EI":
                {
                    break;
                }
        }
        this.DateValue = myResult;
    }

    public DueDateForeground: string = "";
    GetDueDateForeground() {
        var result = "rgb(40,46,48)";

        if (this.DueDate != null && DateTool.GetDateParts(this.DueDate) < DateTool.GetDateParts(DateTool.GetCurrentDateTimeAsUtc())) {
            result = "Red";
        }
        this.DueDateForeground = result;
    }

    public ImageSrc: string;

    // Action Fields
    public Action: string = "";
    GetAction() {
        var myResult = "Modified by";

        if (this.entity.ActivityTypeCode == "EI") {
            myResult = "Recorded by";
        }

        else if (this.entity.ActivityTypeCode == "EO") {
            myResult = "Sent by";
        }

        else {
            switch (this.entity.ActivityStatusCode) {
                case "C":
                    {
                        myResult = "Completed by";
                        break;
                    }

                case "X":
                    {
                        myResult = "Closed by";
                        break;
                    }

                default:
                    {
                        myResult = "Modified by";
                        break;
                    }
            }
        }

        this.Action = myResult;
    }

    public ActionBy: string = "";
    GetActionBy() {
        var myResult = this.entity.UpdatedByUserName;

        if (!this.entity.IsOpen) {
            if (this.entity.ActivityTypeCode == "EO") {
                myResult = this.entity.CreatedByUserName;
            }
        }

        this.ActionBy = myResult;
    }

    public ActionDate: Date;
    getActionDate() {
        var myResult = this.entity.UpdateDate;
        if (!this.entity.IsOpen) {
            if (this.entity.ActivityStatusCode == "C") {
                myResult = this.entity.CompleteDate;
            }
        }

        this.ActionDate = myResult;
    }

    private completeVisi = false;
    get CompleteButtonVisibility() {
        if (this.entity.IsOpen && this.entity.ActivityTypeCode != "AP") {
            this.completeVisi = true;
        }
        return this.completeVisi;
    }
    set CompleteButtonVisibility(value: boolean) { this.completeVisi = value; }

    private reopenVisi = false;
    get ReopenButtonVisibility() {
        if (!this.entity.IsOpen) {
            if (this.entity.ActivityTypeCode != "EI" && this.entity.ActivityTypeCode != "EO") {
                this.reopenVisi = true;
            }
        }
        return this.reopenVisi;
    }
    set ReopenButtonVisibility(value: boolean) { this.reopenVisi = value; }

    private completetogVisi = false;
    get CompleteToggleButtonVisibility() {
        if (this.entity.IsOpen && this.entity.ActivityTypeCode == "AP") {
            this.completetogVisi = true;
        }
        return this.completetogVisi;
    }
    set CompleteToggleButtonVisibility(value: boolean) { this.completetogVisi = value; }

    // Commands
    CompleteClicked() {
        var myService: CRMDomainService = new CRMDomainService();
        myService.GetCompleteActivity(this.EntityId, this.PostToFollowers, this.MeetingSummary).subscribe((resp: ServiceResponse) => {
            if (!resp.HasError) {
                this.entity = resp.Result;
                this.father.LoadActivities();
                this.ReopenButtonVisibility = true;
                this.CompleteButtonVisibility = false;
                this.CompleteToggleButtonVisibility = false;
                this.Background = "rgba(0,0,0,0.1)";
            }
        });
    }

    ReopenClicked() {
        var myService: CRMDomainService = new CRMDomainService();
        myService.GetReopenActivity(this.EntityId).subscribe((resp: ServiceResponse) => {
            if (!resp.HasError) {
                this.entity = resp.Result;
                this.father.LoadActivities();
                this.ReopenButtonVisibility = false;
                if (this.entity.ActivityTypeCode == "AP") {
                    this.CompleteToggleButtonVisibility = true;
                }
                else {
                    this.CompleteButtonVisibility = true;
                }
                this.Background = "rgb(255,255,255)";
            }
        });
    }
}
