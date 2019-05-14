import {Component, ViewChild, ViewContainerRef, OnInit} from '@angular/core';
import {OpportunityPM} from '../../EntityPMs/OpportunityPM';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {OpportunityClosingReasonListService} from '../../Services/StandardLists/OpportunityClosingReasonListService';
import {OpportunityClosingReasonList} from '../../EntityLists/OpportunityClosingReasonList';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {OpportunityTypeList} from '../../EntityLists/OpportunityTypeList';
import {OpportunityTypeListService} from '../../Services/StandardLists/OpportunityTypeListService';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {OpportunityPMService} from '../../Services/StandardPMs/OpportunityPMService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';


@Component({
    moduleId: module.id,
    templateUrl: './EditClosedOpportunityComponent.html',
})

export class EditClosedOpportunityComponent extends BaseComponent implements OnInit{
    private CurrentSession = SessionLocator.SelectedSession;
    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;

    public EntityPM: OpportunityPM;
    public ObjectTableName: string = "Opportunity";
    public DataContext: EditClosedOpportunityComponent = this;
    private myService: OpportunityPMService;
    public get Subject() { return this.EntityPM.Subject; }
    public set Subject(value: string) { if (this.EntityPM.Subject != value) this.EntityPM.Subject = value; }
    public get OpportunityTypeId() { return this.EntityPM.OpportunityTypeId; }
    public set OpportunityTypeId(value: string) {
        if (this.EntityPM.OpportunityTypeId != value) {
            this.OnOpportunityTypeChanging(value);
        }
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

    SetUIProperties() {
        var oppTypeListService: OpportunityTypeListService = new OpportunityTypeListService();
        oppTypeListService.getAllFromCache().subscribe(result => {

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

            this.UIProperties.SetEnabled("ContactId", "Opportunity", !AppTool.IsNullOrEmpty(this.EntityPM.CustomerId));
            this.ContactIdVisibility = !AppTool.IsNullOrEmpty(this.EntityPM.CustomerId);

        });


    }
    public AgentIdVisibility: boolean = false;
    public ForeignClientIdVisibility: boolean = false;
    public LeadUserIdVisibility: boolean = false;
    public LeadSourceIdVisibility: boolean = false;
    public LeadPartnerIdVisibility: boolean = false;
    public LeadDescriptionVisibility: boolean = false;
    public ContactIdVisibility: boolean = false;

    public newOpportunityTypeId: string;
    public newOpportunityTypeCode: string;
    public confirm: ConfirmWindow;
    public ValidationErrorsList: Array<String> = [];

    CancelButtonClicked() {
        this.SetUIProperties_GeneratedComponent(false);
        this.CurrentSession.CloseCurrentWindowEmit("cancle");
    }
    OkButtonClicked() {
        this.CurrentSession.StartBusyIndicatorCreating();
        this.myService = new OpportunityPMService();
        this.myService.update(this.EntityPM).subscribe(myResult => {

            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                this.SetUIProperties_GeneratedComponent(false);
                this.CurrentSession.CloseCurrentWindowEmit(mm.Result.Id);
                this.CurrentSession.StopBusyIndicator();

            }

            else {
                this.ValidationErrorsList = mm.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }



    OnOpportunityTypeChanging(newValue: string) {
        var isConfirmNeeded: boolean = false;
        this.newOpportunityTypeId = newValue;
        var oppTypeListService: OpportunityTypeListService = new OpportunityTypeListService();
        oppTypeListService.getAllFromCache().subscribe(result => {
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
            }
        });





    }


    confirm_Unloaded(event) {
        if (this.confirm.Yes) {
            this.EntityPM.OpportunityTypeId = this.newOpportunityTypeId;

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

        this.SetUIProperties();



    }


    constructor() {
        super();
   
    }
    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 3) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    RunComponent() {
        if (this.viewContainerRef) {
            this.SetUIProperties();
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    }

    LoadChildComponent() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(cmpRef => {
                this.GeneratedComponent = cmpRef.instance;
                cmpRef.instance.Run(this.EntityPM, this.ObjectTableName, "Opportunity.AdditionalFields");
                    this.SetUIProperties_GeneratedComponent(true);

            });
    }
    private GeneratedComponent: any;
    SetUIProperties_GeneratedComponent(value:boolean) {
        if (this.GeneratedComponent) {
            this.GeneratedComponent.SetEnabled(value);
        }
    }

    SetWindowArgs(args: OpportunityPM) {
        this.EntityPM = args;
        //this.SetUIProperties();
    }

    ngOnInit() {
        this.RunComponent();  
    }
}
