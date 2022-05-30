
import { Component, AfterViewInit, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {OpportunityPM} from '../../../../CRM/EntityPMs/OpportunityPM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {OpportunityPMService} from '../../../../CRM/Services/StandardPMs/OpportunityPMService';
import {UserList} from '../../../../Common/EntityLists/UserList';
import {UserListService} from '../../../../Common/Services/StandardLists/UserListService';
import {AppTool} from '../../../../Infrastructure/Tools';
import {CardListService} from '../../../../Common/Services/StandardLists/CardListService';
import {CardList} from '../../../../Common/EntityLists/CardList';
import {OpportunityTypeList} from '../../../../CRM/EntityLists/OpportunityTypeList';
import {OpportunityTypeListService} from '../../../../CRM/Services/StandardLists/OpportunityTypeListService';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService'
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {OpportunityPMInitService} from '../../../../CRM/EntityPMInitServices/OpportunityPMInitService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {NewEntityArgs} from '../../../../Infrastructure/Args';
import {CustomerSalesNotePM} from '../../../../Common/EntityPMs/CustomerSalesNotePM';
import {PartnersDomainService} from '../../../../Common/Services/PartnersDomainService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {OpportunityArgs} from '../../../../CRM/Args'; 

@Component({
    selector: 'NewOpportunityComponent',
    
    templateUrl: './NewOpportunityComponent.html',
})

export class NewOpportunityComponent extends BaseComponent implements OnInit, AfterViewInit {
    public ObjectTableName: string = "Opportunity";
    public DataContext: NewOpportunityComponent = this;
    public EntityPM: OpportunityPM = new OpportunityPM();
    private myService: OpportunityPMService;
    public SalesNotesObsList: Array<CustomerSalesNotePM> = [];
    public ValidationErrorsList: Array<String> = [];
    public ScreenCode: string = "Opportunity.AdditionalFields";
    private addCustomerVisibility: boolean = true;
    @ViewChild('Child', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
    public get AddCustomerVisibility() { return this.addCustomerVisibility; }
    public set AddCustomerVisibility(value: boolean) { this.addCustomerVisibility = value; }
    public IsNew: boolean = true;
    public get Subject() { return this.EntityPM.Subject; }
    public set Subject(value: string) { if (this.EntityPM.Subject != value) this.EntityPM.Subject = value; }
    private CurrentSession = SessionLocator.SelectedSession;
    public get OwnerId() {
        if (this.EntityPM.OwnerId != null)
            return this.EntityPM.OwnerId;   
        
    }

    constructor(private _entityResourceService: EntityResourceService) {
        super();
        this._entityResourceService.getEntityResourceByTableName("Card", 0).subscribe((response: any) => {
            this.ComputeCustomerDependency();
        });

    }

    ngOnInit() {
        this.ComputeCustomerDependency();
    }

    ngAfterViewInit() {
        this.SetUIProperties();
        this.AddCustomerVisibility = AppTool.IsNullOrEmpty(this.EntityPM.CustomerId) ? true : false;
        OpportunityPMInitService.InitValues(this.EntityPM, this.IsNew);
        this.LoadChildComponent();
    }


    //Customer
    //Customer
    public CustomerDependencyProperty1: string = "PO,CS";
    public CustomerDependencyProperty1IsList: boolean = true;
    public CustomerDependencyProperty2: string = "True";

    private ComputeCustomerDependency() {
        var allowAgentFeatureToggle = this.IsAllowAgentFeatureToggle();
        if (SessionLocator.TenantPM.AllowAgentInCustomersLOV && allowAgentFeatureToggle != null) {
            this.CustomerDependencyProperty1 = "PO,CS,AG";
            this.CustomerDependencyProperty2 = null;
        }
    }

    IsAllowAgentFeatureToggle() {
        var isAllowAgentFeatureToggle = SessionLocator.FeatureToggles.filter(d => d.ToggleCode == "SAC")[0];
        return isAllowAgentFeatureToggle;
    }


    public set OwnerId(value: string) {
        if (this.EntityPM.OwnerId != value) {
            this.EntityPM.OwnerId = value;
            if (value == null)
                this.EntityPM.BusinessUnitId = null;
            else {
                var listService: UserListService = new UserListService();
                listService.getAllFromCache().subscribe((result:any) => {
                    var list: UserList = result.Result.filter(p => p.Id == value)[0];
                    if (list != null)
                        this.EntityPM.BusinessUnitId = list.BusinessUnitId;

                });

                } 
            }

    }


    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        this.CurrentSession.StartBusyIndicatorCreating();
        this.myService = new OpportunityPMService();
        this.myService.insert(this.EntityPM).subscribe((myResult:any) => {

            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                this.CurrentSession.CloseCurrentWindowEmit(mm.Result.Id);
                this.CurrentSession.StopBusyIndicator();

            }

            else {
                this.ValidationErrorsList = mm.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }

    AddCustomerWindow() {

        var windowTitle = "New Potential Customer";

        var logWindow = new LogitudeWindow();
        var windowArgs: NewEntityArgs = new NewEntityArgs();
        logWindow.Width = 960;
        logWindow.Height = 580;
        logWindow.Title = windowTitle;
        this._entityResourceService.getEntityResourceByTableName("Customer", 0).subscribe((response:any) => {
            logWindow.Show('./CommonModules/CommonPartners/Components/NewEntity/NewPotentialCustomerComponent');
            logWindow.ComponentLoaded.subscribe(s => {
                logWindow.WindowClosed.subscribe(d => {
                    if (s.EntityPM != null)
                        if (s.EntityPM.Id != null) {
                            this.EntityPM.CustomerId = s.EntityPM.Id;
                            this.ContactId = s.EntityPM.PrimaryContactId;
                            this.GetCustomerSalesNotes();
                            this.UIProperties.SetEnabled("ContactId", this.ObjectTableName, !AppTool.IsNullOrEmpty(this.EntityPM.CustomerId));
                        }


                });
            });

        });



    }

   
    GetCustomerSalesNotes() {
        this.SalesNotesObsList = [];
        var domainService: PartnersDomainService = new PartnersDomainService();
        if (!AppTool.IsNullOrEmpty(this.CustomerId))
        domainService.GetCustomerSalesNotes(this.CustomerId).subscribe((result:any) => {
            this.SalesNotesObsList = result.sort((a, b) => { return (a.UpdateDate === b.UpdateDate) ? 0 : (a.UpdateDate < b.UpdateDate) ? -1 : 1 });
            console.log(this.SalesNotesObsList);

        });



    }

    public get HasSalesNotes() { return this.SalesNotesObsList.length > 0; }

    public get CustomerId() { return this.EntityPM.CustomerId; }
    public set CustomerId(value: string) {
        if (this.EntityPM.CustomerId != value) {
            this.EntityPM.CustomerId = value;

            this.UpdateCustomerContact();
            this.GetCustomerSalesNotes();
            this.UIProperties.SetEnabled("ContactId", this.ObjectTableName, !AppTool.IsNullOrEmpty(value));

        }

    }

    private UpdateCustomerContact() {
        var myContactId :string = null;

        if (!AppTool.IsNullOrEmpty(this.CustomerId)) {

            var cardService: CardListService = new CardListService();
            cardService.getAll().subscribe((result:any) => {
                var list: CardList = result.Result.filter(p => p.Id == this.CustomerId)[0];
                if (list != null) {
                    myContactId = list.PrimaryContactId;                          
                }
                this.ContactId = myContactId;    
            });

           
        }


    }


    public get ContactId() { return this.EntityPM.ContactId; }
    public set ContactId(value: string) { if (this.EntityPM.ContactId != value) this.EntityPM.ContactId = value; }

    public get RatingCode() { return this.EntityPM.RatingCode; }
    public set RatingCode(value: string) { if (this.EntityPM.RatingCode != value) this.EntityPM.RatingCode = value; }

    public get NumberOfShipments() { return this.EntityPM.NumberOfShipments; }
    public set NumberOfShipments(value: number) { if (this.EntityPM.NumberOfShipments != value) this.EntityPM.NumberOfShipments = value; }

    public get EstimatedClosingDate() { return this.EntityPM.EstimatedClosingDate; }
    public set EstimatedClosingDate(value: Date) { if (this.EntityPM.EstimatedClosingDate != value) this.EntityPM.EstimatedClosingDate = value; }

    public get Notes() { return this.EntityPM.Notes; }
    public set Notes(value: string) { if (this.EntityPM.Notes != value) this.EntityPM.Notes = value; }

    public get OpportunityTypeId() { return this.EntityPM.OpportunityTypeId; }
    public set OpportunityTypeId(value: string) {
        if (this.EntityPM.OpportunityTypeId != value)
        {
            this.OnOpportunityTypeChanging(value);
        }
    }
    public  newOpportunityTypeId:string;
    public newOpportunityTypeCode: string;
    public confirm: ConfirmWindow;
    OnOpportunityTypeChanging(newValue: string) {
        var isConfirmNeeded: boolean = false;
        this.newOpportunityTypeId = newValue;
        var oppTypeListService: OpportunityTypeListService = new OpportunityTypeListService();
        oppTypeListService.getAllFromCache().subscribe((result:any) => {
            var typeList: OpportunityTypeList = result.Result.filter(d => d.Id == newValue)[0];
            if (typeList != null) {
                this.newOpportunityTypeCode = typeList.Code;
            }

            if (this.newOpportunityTypeCode == "T") {
                if (!AppTool.IsNullOrEmpty(this.LeadUserId) || !AppTool.IsNullOrEmpty(this.LeadSourceId) || !AppTool.IsNullOrEmpty(this.LeadPartnerId) || !AppTool.IsNullOrEmpty(this.LeadDescription)) {
                    isConfirmNeeded = true;
                }
            }

            else {
                if (!AppTool.IsNullOrEmpty(this.AgentId) || !AppTool.IsNullOrEmpty(this.ForeignClientId)) {
                    isConfirmNeeded = true;
                }
            }

            if (isConfirmNeeded) {
                if (this.confirm != null) {
                    this.confirm.Close();
                }

                this.confirm = new ConfirmWindow();
                this.confirm.WindowClosed.subscribe(event => { this.confirm_Unloaded(event); });
                this.confirm.ShowCancelButton = false;               

                if (this.newOpportunityTypeCode == "T") {
                    this.confirm.Show("Lead Source details will be lost");
                }

                else {
                    this.confirm.Show("Routing Order details will be lost");
                }
            }

            else {
                this.EntityPM.OpportunityTypeId = newValue;
                this.SetUIProperties();
                this.SetSubject();
            }
        });


   


    }

    confirm_Unloaded(event) {
        if (this.confirm.Yes) {
            this.EntityPM.OpportunityTypeId = this.newOpportunityTypeId;
                this.SetSubject();

                if (this.newOpportunityTypeCode == "T") {
                    this. LeadUserId = null;
                    this. LeadSourceId = null;
                    this.  LeadPartnerId = null;
                    this. LeadDescription = null;
                }

                else {
                    this.AgentId = null;
                    this.ForeignClientId = null;
                }
            }

        this.SetUIProperties();

                 

    }

    public AgentIdVisibility: boolean = false;
    public ForeignClientIdVisibility: boolean = false;
    public LeadUserIdVisibility: boolean = false;
    public LeadSourceIdVisibility: boolean = false;
    public LeadPartnerIdVisibility: boolean = false;
    public LeadDescriptionVisibility: boolean = false;


    SetSubject() {

        var oppTypeListService: OpportunityTypeListService = new OpportunityTypeListService();
        oppTypeListService.getAllFromCache().subscribe((result:any) => {

            var type: OpportunityTypeList = result.Result.filter(d => d.Id == this.OpportunityTypeId)[0];
            if (type != null) {
                this.Subject = type.Name;
            }

        });
      

    }

    SetUIProperties() {
        var oppTypeListService: OpportunityTypeListService = new OpportunityTypeListService();
        oppTypeListService.getAllFromCache().subscribe((result:any) => {

            var typeList: OpportunityTypeList = result.Result.filter(d => d.Id == this.EntityPM.OpportunityTypeId)[0];
            var typeCode: string = typeList == null ? null : typeList.Code;


            if (typeCode == "T") {
                this.UIProperties.SetVisibility("AgentId", "Opportunity", true);
                this.AgentIdVisibility = true;
                this.UIProperties.SetVisibility("ForeignClientId", "Opportunity", true);
                this.ForeignClientIdVisibility = true;
                this.UIProperties.SetVisibility("LeadUserId", "Opportunity", false);
                this. LeadUserIdVisibility = false;
                this.UIProperties.SetVisibility("LeadSourceId", "Opportunity", false);
                this.LeadSourceIdVisibility = false;

                this.UIProperties.SetVisibility("LeadPartnerId", "Opportunity", false);
                this.LeadPartnerIdVisibility = false;

                this.UIProperties.SetVisibility("LeadDescription", "Opportunity", false);
                this.LeadDescriptionVisibility = false;
            }

            else {
             this.UIProperties.SetVisibility("AgentId", "Opportunity", false);
             this.AgentIdVisibility = false;

                this.UIProperties.SetVisibility("ForeignClientId", "Opportunity", false);
                this.ForeignClientIdVisibility = false;

                this.UIProperties.SetVisibility("LeadUserId", "Opportunity", true);
                this.LeadUserIdVisibility = true;

                this.UIProperties.SetVisibility("LeadSourceId", "Opportunity", true);
                this.LeadSourceIdVisibility = true;

                this.UIProperties.SetVisibility("LeadPartnerId", "Opportunity", true);
                this.LeadPartnerIdVisibility = true;

                this.UIProperties.SetVisibility("LeadDescription", "Opportunity", true);
                this.LeadDescriptionVisibility = true;

            }

            this.UIProperties.SetEnabled("ContactId", "Opportunity", !AppTool.IsNullOrEmpty(this.EntityPM.CustomerId));

        });


    }



    public get LeadSourceId() { return this.EntityPM.LeadSourceId; }
    public set LeadSourceId(value: string) { if (this.EntityPM.LeadSourceId != value) this.EntityPM.LeadSourceId = value; }


    public get LeadDescription() { return this.EntityPM.LeadDescription; }
    public set LeadDescription(value: string) { if (this.EntityPM.LeadDescription != value) this.EntityPM.LeadDescription = value; }


    public get LeadUserId() { return this.EntityPM.LeadUserId; }
    public set LeadUserId(value: string) { if (this.EntityPM.LeadUserId != value) this.EntityPM.LeadUserId = value; }

    public get LeadPartnerId() { return this.EntityPM.LeadPartnerId; }
    public set LeadPartnerId(value: string) { if (this.EntityPM.LeadPartnerId != value) this.EntityPM.LeadPartnerId = value; }

    public get AgentId() { return this.EntityPM.AgentId; }
    public set AgentId(value: string) { if (this.EntityPM.AgentId != value) this.EntityPM.AgentId = value; }

    public get ForeignClientId() { return this.EntityPM.ForeignClientId; }
    public set ForeignClientId(value: string) { if (this.EntityPM.ForeignClientId != value) this.EntityPM.ForeignClientId = value; }


    SetWindowArgs(args: OpportunityArgs) {
        if (args) {
            this.EntityPM = args.Entity;
            this.IsNew = args.IsNew;
            this.addCustomerVisibility = args.IsAddCustomerVisible;
        }
    }

    LoadChildComponent() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.Run(this.EntityPM, this.ObjectTableName, this.ScreenCode);
            });
    }

}
