import { Component, OnInit, ViewChild, ViewContainerRef, AfterViewInit} from '@angular/core';
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
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {ContactList} from '../../../../Common/EntityLists/ContactList';
import {ContactListService} from '../../../../Common/Services/StandardLists/ContactListService';
@Component({
    selector: 'OpportunityGeneralTabComponent',
    
    templateUrl: './OpportunityGeneralTabComponent.html',
})

export class OpportunityGeneralTabComponent extends BaseComponent implements OnInit, AfterViewInit {
    public LabelWidth: number = 150;
    public ControlWidth: number = 230;

    public ObjectTableName: string = "Opportunity";
    public DataContext: OpportunityGeneralTabComponent = this;
    public EntityPM: OpportunityPM = new OpportunityPM();
    private myService: OpportunityPMService;
    public SalesNotesObsList: Array<CustomerSalesNotePM> = [];
    public ValidationErrorsList: Array<String> = [];
    public ResetOpportunitiy: boolean = true;
    @ViewChild('Child', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
    private CurrentSession = SessionLocator.SelectedSession;

    public ScreenCode: string = "Opportunity.AdditionalFields";
    public get Subject() { return this.EntityPM.Subject; }
    public set Subject(value: string) { if (this.EntityPM.Subject != value) this.EntityPM.Subject = value; }

    public get OwnerId() {
        if (this.EntityPM.OwnerId != null)
            return this.EntityPM.OwnerId;

    }

    constructor(private _entityResourceService: EntityResourceService, private entityArgs: EntityArgs) {
        super();
        this._entityResourceService.getEntityResourceByTableName("Card", 0).subscribe((response: any) => {
        });
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    }


    ngOnInit() {
        this.SetFieldsEnabled();
        this.SetUIProperties();

    }

    ngAfterViewInit() {
        this.LoadChildComponent();
    }

    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    OpportunityPMInitService.InitValues(this.EntityPM, false);
                    this.SetFieldsEnabled();
                }
            });

            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    this.SetFieldsEnabled();

                }
            });
        }
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
                    if (list != null) {
                        this.EntityPM.BusinessUnitId = list.BusinessUnitId;
                        this.EntityPM.OwnerName = list.EnglishName;
                    }
                });

            }
        }

    }
    public OtherFieldsChild: boolean = true;


    SetFieldsEnabled() {
        var fieldIsEnabled: boolean = true;

        if (this.EntityPM.IsClosed || this.EntityPM.IsCancelled) {
            fieldIsEnabled = false;
        }

        this.UIProperties.SetEnabled("Subject", "Opportunity", fieldIsEnabled);
        this.UIProperties.SetEnabled("OwnerId", "Opportunity", fieldIsEnabled);
        this.UIProperties.SetEnabled("CustomerId", "Opportunity", fieldIsEnabled);
        this.UIProperties.SetEnabled("ContactId", "Opportunity", fieldIsEnabled);
        this.UIProperties.SetEnabled("RatingCode", "Opportunity", fieldIsEnabled);
        this.UIProperties.SetEnabled("EstimatedClosingDate", "Opportunity", fieldIsEnabled);
        this.UIProperties.SetEnabled("Description", "Opportunity", fieldIsEnabled);
        this.UIProperties.SetEnabled("OpportunityTypeId", "Opportunity", fieldIsEnabled);
        this.UIProperties.SetEnabled("LeadSourceId", "Opportunity", fieldIsEnabled);
        this.UIProperties.SetEnabled("LeadDescription", "Opportunity", fieldIsEnabled);
        this.UIProperties.SetEnabled("LeadUserId", "Opportunity", fieldIsEnabled);
        this.UIProperties.SetEnabled("LeadPartnerId", "Opportunity", fieldIsEnabled);
        this.UIProperties.SetEnabled("AgentId", "Opportunity", fieldIsEnabled);
        this.UIProperties.SetEnabled("ForeignClientId", "Opportunity", fieldIsEnabled);
        this.UIProperties.SetEnabled("Notes", "Opportunity", fieldIsEnabled);

        if (fieldIsEnabled) {
            this.UIProperties.SetEnabled("ContactId", "Opportunity", !AppTool.IsNullOrEmpty(this.CustomerId));
        }

        this.OtherFieldsChild = fieldIsEnabled;


        this.SetUIProperties_GeneratedComponent();
    }

    public get CustomerId() { return this.EntityPM.CustomerId; }
    public set CustomerId(value: string) {
        if (this.EntityPM.CustomerId != value) {
            this.EntityPM.CustomerId = value;

            this.UpdateCustomerContact();
            if (!this.EntityPM.IsClosed && !this.EntityPM.IsCancelled) {
                this.UIProperties.SetEnabled("ContactId", this.ObjectTableName, !AppTool.IsNullOrEmpty(value));

            }

        }

    }

    private UpdateCustomerContact() {
        var myContactId: string = null;

        if (!AppTool.IsNullOrEmpty(this.CustomerId)) {

            var cardService: CardListService = new CardListService();
            cardService.getAll().subscribe((result:any) => {
                var list: CardList = result.Result.filter(p => p.Id == this.CustomerId)[0];
                if (list != null) {
                    myContactId = list.PrimaryContactId;
                    this.ContactId = myContactId;                   
                }
            });


        }


    }


    public newOpportunityTypeId: string;
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
                this.LeadUserId = null;
                this.LeadSourceId = null;
                this.LeadPartnerId = null;
                this.LeadDescription = null;
            }

            else {
                this.AgentId = null;
                this.ForeignClientId = null;
            }
        }

        else {
            setTimeout(() => this.RejectChanges(), 10);          
        }

        this.SetUIProperties();



    }

    private RejectChanges() {
        this.isConfirmTestRequired = false;
        var isEntityDirty = this.EntityPM.IsDirty;
        var oldId = this.EntityPM.OpportunityTypeId;
        this.OpportunityTypeId = null;
        this.OpportunityTypeId = oldId;
        this.EntityPM.IsDirty = isEntityDirty;
        this.isConfirmTestRequired = true;

    }

    SetSubject() {

        var oppTypeListService: OpportunityTypeListService = new OpportunityTypeListService();
        oppTypeListService.getAllFromCache().subscribe((result:any) => {

            var type: OpportunityTypeList = result.Result.filter(d => d.Name == this.Subject)[0];
            if (type != null) {
                this.Subject = type.Name;
            }

        });


    }

    public get ContactId() { return this.EntityPM.ContactId; }
    public set ContactId(value: string) {
        if (this.EntityPM.ContactId != value) {
            this.EntityPM.ContactId = value;

            var cardService: ContactListService = new ContactListService();
            cardService.getAll().subscribe((response: ServiceResponse) => {
                var contact: ContactList = response.Result.filter(p => p.Id == this.ContactId)[0];
                if (contact != null)
                    this.EntityPM.ContactName = contact.EnglishName;
                else
                    this.EntityPM.ContactName = null;
            });


        }
        }

  
    public get EstimatedClosingDate() { return this.EntityPM.EstimatedClosingDate; }
    public set EstimatedClosingDate(value: Date) { if (this.EntityPM.EstimatedClosingDate != value) this.EntityPM.EstimatedClosingDate = value; }

    private isConfirmTestRequired: boolean = true;
    public get OpportunityTypeId() { return this.EntityPM.OpportunityTypeId; }
    public set OpportunityTypeId(value: string) {
        if (this.EntityPM.OpportunityTypeId != value) {
            if (this.isConfirmTestRequired) {
                this.OnOpportunityTypeChanging(value);
            }

            else {
                this.EntityPM.OpportunityTypeId = value;
            }
        }
    }
    public AgentIdVisibility: boolean = false;
    public ForeignClientIdVisibility: boolean = false;
    public LeadUserIdVisibility: boolean = false;
    public LeadSourceIdVisibility: boolean = false;
    public LeadPartnerIdVisibility: boolean = false;
    public LeadDescriptionVisibility: boolean = false;
    public ContactIdVisibility: boolean = false;

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
                this.LeadUserIdVisibility = false;
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

           // this.UIProperties.SetEnabled("ContactId", "Opportunity", !AppTool.IsNullOrEmpty(this.EntityPM.CustomerId));
            this.ContactIdVisibility = !AppTool.IsNullOrEmpty(this.EntityPM.CustomerId);

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

    SetClosingReasonFields() {

        this.UIProperties.SetEnabled("ClosingReasonId","Opportunity", false);
        this.UIProperties.SetVisibility("ClosingReasonId", "Opportunity", false);

        if (this.EntityPM.IsClosed) {
            if (this.EntityPM.IsClosedLost) {
                this.UIProperties.SetVisibility("ClosingReasonId", "Opportunity", true);
            }
        }            
    }

    LoadChildComponent() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(cmpRef => {
                this.GeneratedComponent = cmpRef.instance;

                cmpRef.instance.LoadCompleted.subscribe(s => {
                    this.SetUIProperties_GeneratedComponent();
                });

                cmpRef.instance.LabelWidth = this.LabelWidth;
                cmpRef.instance.Run(this.entityArgs.EntityPM, this.entityArgs.ObjectTableName, this.ScreenCode);
            });
    }

    private GeneratedComponent: any;
    SetUIProperties_GeneratedComponent() {
        if (this.GeneratedComponent) {
            this.GeneratedComponent.SetEnabled(this.OtherFieldsChild);
        }
    }

    
}
