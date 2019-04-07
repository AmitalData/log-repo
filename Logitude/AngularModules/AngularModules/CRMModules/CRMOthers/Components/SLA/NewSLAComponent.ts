import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SLAHeaderPM} from '../../../../CRM/EntityPMs/SLAHeaderPM';
import {SLALinePM} from '../../../../CRM/EntityPMs/SLALinePM';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AddEditEscalationComponent} from './AddEditEscalationComponent';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {CRMDomainService} from '../../../../CRM/Services/CRMDomainService';
import {DateTool, AppTool} from '../../../../Infrastructure/Tools';
import {TicketSeverityList} from  '../../../../CRM/EntityLists/TicketSeverityList';
import {TicketSeverityListService} from '../../../../CRM/Services/StandardLists/TicketSeverityListService';
import {SLAEscalationRecepientPM} from '../../../../CRM/EntityPMs/SLAEscalationRecepientPM';
import {SLAEscalationPM} from '../../../../CRM/EntityPMs/SLAEscalationPM';
import {SLAHeaderPMService} from '../../../../CRM/Services/StandardPMs/SLAHeaderPMService';
import {EscalationPreDefinitionList} from  '../../../../CRM/EntityLists/EscalationPreDefinitionList';
import {EscalationPreDefinitionListService} from '../../../../CRM/Services/StandardLists/EscalationPreDefinitionListService';
import {EscalationActionTimeIndicatorList} from  '../../../../CRM/EntityLists/EscalationActionTimeIndicatorList';
import {EscalationActionTimeIndicatorListService} from '../../../../CRM/Services/StandardLists/EscalationActionTimeIndicatorListService';
import {TimeUnitList} from  '../../../../CRM/EntityLists/TimeUnitList';
import {TimeUnitListService} from '../../../../CRM/Services/StandardLists/TimeUnitListService';
import {UserList} from  '../../../../Common/EntityLists/UserList';
import {UserListService} from '../../../../Common/Services/StandardLists/UserListService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from  '../../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    moduleId: module.id,
    templateUrl: './NewSLAComponent.html',
})

export class NewSLAComponent extends BaseComponent {
    public ObjectTableName: string = "SLAHeader";
    public DataContext: NewSLAComponent = this;
    public entityPM: SLAHeaderPM = new SLAHeaderPM();
    public SLALine: SLALinePM;
    public ObjectTable: any;
    public ValidationErrorsList: string[] = [];
    public SLALinesList: SLALineArgs[] = [];
    public FirstResponseEscalationDataList: EscalationArgs[] = [];
    public ResolveEscalationDataList: EscalationArgs[] = [];
    public UsersCachedList: UserList[];
    public EscalationPreDefinitionCachedList: EscalationPreDefinitionList[];
    public SelectedFirstResponse: EscalationArgs;
    public SelectedResolveEscalation: EscalationArgs;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsNew = false;
    constructor() {
        super();
        this.SLALinesList = [];
        this.FirstResponseEscalationDataList = [];
        this.ResolveEscalationDataList = [];
        this.FillUsers();
        this.FillPredefinitionList();
    }
    SetWindowArgs(args: any) {
        this.entityPM = args.EntityPM;
        this.IsNew = args.IsNewEntity;
        if (this.IsNew) {
            this.entityPM = new SLAHeaderPM();
            var todayDateTime = DateTool.GetCurrentDateTimeAsUtc();
            this.entityPM.Tenant = SessionLocator.Tenant;
            this.entityPM.CreateDate = todayDateTime;
            this.entityPM.CreatedByUserId = SessionLocator.LoggedUserId;
            this.entityPM.UpdateDate = todayDateTime;
            this.entityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
            this.createSLALinesList();
            this.InitalizeData();
        }
        else {
            var service = new SLAHeaderPMService();
            service.get(this.entityPM.Id).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.entityPM = myResponse.Result;
                    this.InitalizeData();
                }
            });
        }
    }

    private InitalizeData() {
        this.FillSLALines();
        this.RefreshData();
        this.SetUIProperties();
    }
    private FillUsers() {
        this.UsersCachedList = [];
        var listService: UserListService = new UserListService();
        listService.getAllFromCache().subscribe(result => {
            this.UsersCachedList = result.Result;
        });
    }
    private FillPredefinitionList() {
        this.EscalationPreDefinitionCachedList = [];
        var service = new EscalationPreDefinitionListService();
        service.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.EscalationPreDefinitionCachedList = myResponse.Result;
            }
        });
    }
    private SetUIProperties() {
        this.AddFirstResponseEscalationEnabled = (this.entityPM.SLAEscalations.filter(a => a.EscalationFor == "FR").length < 5 ? true : false);
        this.AddResolveWithinEscalationEnabled = (this.entityPM.SLAEscalations.filter(a => a.EscalationFor == "RW").length < 5 ? true : false);
    }
    public FillSLALines() {
        if (this.entityPM != null && this.entityPM.SLALines.length > 0) {
            this.SLALinesList = [];
            this.entityPM.SLALines.forEach(item => {
                this.SLALinesList.push(new SLALineArgs(item));
            });
        }
    }
    public FillResponseEscalationList() {
        if (this.entityPM != null && this.entityPM.SLAEscalations.filter(a => a.EscalationFor == "FR").length > 0) {
            this.FirstResponseEscalationDataList = [];

            this.entityPM.SLAEscalations.filter(a => a.EscalationFor == "FR").forEach(item => {
                this.FirstResponseEscalationDataList.push(new EscalationArgs(this, item, item.EscalationFor, false));
            });
            this.SetUIProperties();
        }
    }
    public FillResolveEscalationList() {
        if (this.entityPM != null && this.entityPM.SLAEscalations.filter(a => a.EscalationFor == "RW").length > 0) {
            this.ResolveEscalationDataList = [];
            this.entityPM.SLAEscalations.filter(a => a.EscalationFor == "RW").forEach(item => {
                this.ResolveEscalationDataList.push(new EscalationArgs(this, item, item.EscalationFor, false));
            });
            this.SetUIProperties();
        }
    }

    //private IsNew = false;
    private getSLAHeaderEntityMethod() {
        var service = new CRMDomainService();
        service.GetSingleSLAHeaderPMByTenant().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
               // this.IsNew = false;
                this.entityPM = myResponse.Result;
                this.FillSLALines();
                if (this.entityPM == null) {
                    //this.IsNew = true;
                    this.entityPM = new SLAHeaderPM();
                    var todayDateTime = DateTool.GetCurrentDateTimeAsUtc();
                    this.entityPM.Tenant = SessionLocator.Tenant;
                    this.entityPM.CreateDate = todayDateTime;
                    this.entityPM.CreatedByUserId = SessionLocator.LoggedUserId;
                    this.entityPM.UpdateDate = todayDateTime;
                    this.entityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
                    this.createSLALinesList();
                }
                this.RefreshData();
                this.SetUIProperties();
            }
        });
    }

    private RefreshData() {
        this.FillResponseEscalationList();
        this.FillResolveEscalationList();
    }
    private createSLALinesList() {
        var myService: TicketSeverityListService = new TicketSeverityListService();
        myService.getAllFromCache().subscribe((resp: any) => {
            if (!resp.HasError) {
                var allSeverities: TicketSeverityList[] = resp.Result;
                allSeverities.filter(a => a.Tenant == SessionLocator.Tenant).forEach(item => {
                    var sLALine = new SLALinePM(this.entityPM.Id);
                    sLALine.SeverityName = item.Name;
                    sLALine.SLAHeaderId = this.entityPM.Id;
                    sLALine.SeverityId = item.Id;
                    sLALine.ChangeSetOp = "Insert";
                    sLALine.Tenant = SessionLocator.Tenant;
                    this.SLALinesList.push(new SLALineArgs(sLALine));
                    this.entityPM.AddSLALine(sLALine);
                });
            }
        });
    }
   
    // Properties
    get Inactive() { return this.entityPM.Inactive; }
    set Inactive(value: boolean) {
        if (this.entityPM.Inactive != value) {
            this.entityPM.Inactive = value;
        }
    }

    get Name() { return this.entityPM.Name; }
    set Name(value: string) {
        if (this.entityPM.Name != value) {
            this.entityPM.Name = value;
        }
    }

    get Description(){ return this.entityPM.Description; }
    set Description(value:string)
    {
        if (this.entityPM.Description != value) {
            this.entityPM.Description = value;
        }
    }

    private addResolveWithinEscalationEnabled = true;
    get AddResolveWithinEscalationEnabled() { return this.addResolveWithinEscalationEnabled; }
    set AddResolveWithinEscalationEnabled(value: boolean) {
        if (this.addResolveWithinEscalationEnabled != value) {
            this.addResolveWithinEscalationEnabled = value;
        }
    }

    private addFirstResponseEscalationEnabled = true;
    get AddFirstResponseEscalationEnabled(){ return this.addFirstResponseEscalationEnabled; }
    set AddFirstResponseEscalationEnabled(value: boolean)
    {
        if (this.addFirstResponseEscalationEnabled != value) {
            this.addFirstResponseEscalationEnabled = value;
        }
    }

    // Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var errors = [];
        Validator.TryValidateObject(this.entityPM, this.ObjectTableName, errors);
        this.entityPM.SLALines.forEach(item => {
            Validator.TryValidateObject(item, "SLALine", errors);
            if ((item.FirstResponseTime == null || AppTool.IsNullOrEmpty(item.FirstResponseTimeUnit))) {
                errors.push("First Response fields are required");
            }
            if ((item.ResolveWithinTime == null || AppTool.IsNullOrEmpty(item.ResolveWithinTimeUnit))) {
                errors.push("Resolve Within fields are required");
            }
            if (AppTool.IsNullOrEmpty(item.BusinessHoursId)) {
                errors.push("Operational Hours are required");
            }
        });
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.IsNew) {
                this.InsertSLA();
            }
            else {
                this.UpdateSLA();
            }
        }
    }
    private InsertSLA() {
        this.CurrentSession.StartBusyIndicatorSaving();
        var service = new SLAHeaderPMService();
        service.insert(this.entityPM).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                this.CurrentSession.CloseCurrentWindowEmit('ok');
            }
            else {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });
    }
    private UpdateSLA() {
        this.CurrentSession.StartBusyIndicatorSaving();
        var service = new SLAHeaderPMService();
        service.update(this.entityPM).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                this.CurrentSession.CloseCurrentWindowEmit('ok');
            }
            else {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });
    }

    EditEscalation(item: EscalationArgs) {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Edit Escalation";
        logWindow.DataContext = item;
        logWindow.Show('./CRMModules/CRMOthers/Components/SLA/AddEditEscalationComponent');
    }
    DeleteEscalation(deletedItem: EscalationArgs) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Delete this Escalation?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                var selectedPM: SLAEscalationPM = deletedItem.entityPM;
                if (selectedPM.EscalationFor == "FR") {
                    if (this.entityPM.SLAEscalations.indexOf(selectedPM) != -1) {
                        this.entityPM.RemoveSLAEscalation(selectedPM);
                    }

                    var itemIndex = this.FirstResponseEscalationDataList.indexOf(deletedItem);
                    if (itemIndex > -1) {
                        this.FirstResponseEscalationDataList.splice(itemIndex, 1);
                    }
                }
                else {
                    if (this.entityPM.SLAEscalations.indexOf(selectedPM) != -1) {
                        this.entityPM.RemoveSLAEscalation(selectedPM);
                    }

                    var itemIndex = this.ResolveEscalationDataList.indexOf(deletedItem);
                    if (itemIndex > -1) {
                        this.ResolveEscalationDataList.splice(itemIndex, 1);
                    }
                }

                //Refresh data 
                this.SetUIProperties();
            }
        });
    }

    AddResolveWithinEscalation() {
        var count = this.entityPM.SLAEscalations.filter(a => a.EscalationFor == "RW").length;
        if (count < 5) {
            var escalationFor = "RW"; // Resolve Within

            var myLineNumber = 1;
            if (count > 0) {
                this.entityPM.SLAEscalations.filter(a => a.EscalationFor == "RW").forEach(item => {
                    if (item.LineNumber > myLineNumber)
                        myLineNumber = item.LineNumber;
                });
                myLineNumber = myLineNumber + 1;
            }
            var slaEscalationPM: SLAEscalationPM = new SLAEscalationPM(null);
            slaEscalationPM.Tenant = SessionLocator.Tenant;
            slaEscalationPM.SLAHeaderId = this.entityPM.Id;
            slaEscalationPM.LineNumber = myLineNumber;
            var context: EscalationArgs = new EscalationArgs(this, slaEscalationPM, escalationFor, true);
            var logWindow = new LogitudeWindow();
            logWindow.DataContext = context;
            logWindow.Title = "New Escalation";
            logWindow.Show('./CRMModules/CRMOthers/Components/SLA/AddEditEscalationComponent');
        }
        else {
            this.AddResolveWithinEscalationEnabled = false;
        }
    }
    AddFirstResponseEscalation() {
        var count = this.entityPM.SLAEscalations.filter(a => a.EscalationFor == "FR").length;
        if (count < 5) {
            var escalationFor = "FR"; // First Response
            var myLineNumber = 1;

            if (count > 0) {
                this.entityPM.SLAEscalations.filter(a => a.EscalationFor == "FR").forEach(item => {
                    if (item.LineNumber > myLineNumber)
                        myLineNumber = item.LineNumber;
                });
                myLineNumber = myLineNumber + 1;
            }
            var slaEscalationPM: SLAEscalationPM = new SLAEscalationPM(null);
            slaEscalationPM.Tenant = SessionLocator.Tenant;
            slaEscalationPM.SLAHeaderId = this.entityPM.Id;
            slaEscalationPM.LineNumber = myLineNumber;
            var context: EscalationArgs = new EscalationArgs(this, slaEscalationPM, escalationFor, true);
            var logWindow = new LogitudeWindow();
            logWindow.Title = "New Escalation";
            logWindow.DataContext = context;
            logWindow.Show('./CRMModules/CRMOthers/Components/SLA/AddEditEscalationComponent');
        }
        else {
            this.AddFirstResponseEscalationEnabled = false;
        }
    }
}
export class SLALineArgs extends BaseComponent{
    public SLALine: SLALinePM;
    public ObjectTableName: string = "SLALine";
    public DataContext: SLALineArgs = this; 
    constructor(entityList: SLALinePM) {
        super();
        this.SLALine = entityList;
    }

    // Properties
    get SeverityName() { return this.SLALine.SeverityName; }
    set SeverityName(value: string) {
        if (this.SLALine.SeverityName != value) {
            this.SLALine.SeverityName = value;
        }
    }

    get FirstResponseTime() { return this.SLALine.FirstResponseTime; }
    set FirstResponseTime(value: number) {
        if (this.SLALine.FirstResponseTime != value) {
            this.SLALine.FirstResponseTime = value;
            this.calculateFirstResponceTimeInMinutes();
        }
    }

    private calculateFirstResponceTimeInMinutes() {

        if (this.FirstResponseTimeUnit == "II") //Minutes
        {
            this.FirstResponseTimeInMinute = this.FirstResponseTime;
        }

        if (this.FirstResponseTimeUnit == "YY")//Days 
        {
            var numOfMinutes = 24 * 60;
            this.FirstResponseTimeInMinute = this.FirstResponseTime * numOfMinutes;
        }

        if (this.FirstResponseTimeUnit == "OO")//Hours
        {
            var numOfMinutes = 1 * 60;
            this.FirstResponseTimeInMinute = this.FirstResponseTime * numOfMinutes;
        }
    }

    get FirstResponseTimeUnit() { return this.SLALine.FirstResponseTimeUnit; }
    set FirstResponseTimeUnit(value: string) {
        if (this.SLALine.FirstResponseTimeUnit != value) {
            this.SLALine.FirstResponseTimeUnit = value;
            this.calculateFirstResponceTimeInMinutes();
        }
    }

    get FirstResponseTimeInMinute() { return this.SLALine.FirstResponseTimeInMinute; }
    set FirstResponseTimeInMinute(value: number) {
        if (this.SLALine.FirstResponseTimeInMinute != value) {
            this.SLALine.FirstResponseTimeInMinute = value;
        }
    }

    get ResolveWithinTime() { return this.SLALine.ResolveWithinTime; }
    set ResolveWithinTime(value: number) {
        if (this.SLALine.ResolveWithinTime != value) {
            this.SLALine.ResolveWithinTime = value;
            this.calculateResolveWithinTimeInMinutes();
        }
    }

    private calculateResolveWithinTimeInMinutes() {
        if (this.ResolveWithinTimeUnit == "II") {
            this.ResolveWithinTimeInMinute = this.ResolveWithinTime;
        }

        if (this.ResolveWithinTimeUnit == "YY") {
            var numOfMinutes = 24 * 60;
            this.ResolveWithinTimeInMinute = this.ResolveWithinTime * numOfMinutes;
        }

        if (this.ResolveWithinTimeUnit == "OO") {
            var numOfMinutes = 1 * 60;
            this.ResolveWithinTimeInMinute = this.ResolveWithinTime * numOfMinutes;
        }
    }

    get ResolveWithinTimeUnit() { return this.SLALine.ResolveWithinTimeUnit; }
    set ResolveWithinTimeUnit(value: string) {
        if (this.SLALine.ResolveWithinTimeUnit != value) {
            this.SLALine.ResolveWithinTimeUnit = value;
            this.calculateResolveWithinTimeInMinutes();
        }
    }

    get ResolveWithinTimeInMinute() { return this.SLALine.ResolveWithinTimeInMinute; }
    set ResolveWithinTimeInMinute(value: number) {
        if (this.SLALine.ResolveWithinTimeInMinute != value) {
            this.SLALine.ResolveWithinTimeInMinute = value;
        }
    }

    get FirstResponseEscalate() { return this.SLALine.FirstResponseEscalate; }
    set FirstResponseEscalate(value: boolean) {
        if (this.SLALine.FirstResponseEscalate != value) {
            this.SLALine.FirstResponseEscalate = value;
        }
    }

    get ResolveWithinEscalate() { return this.SLALine.ResolveWithinEscalate; }
    set ResolveWithinEscalate(value: boolean) {
        if (this.SLALine.ResolveWithinEscalate != value) {
            this.SLALine.ResolveWithinEscalate = value;
        }
    }

    get BusinessHoursId() { return this.SLALine.BusinessHoursId; }
    set BusinessHoursId(value: string) {
        if (this.SLALine.BusinessHoursId != value) {
            this.SLALine.BusinessHoursId = value;
        }
    }
}
export class EscalationArgs extends BaseComponent {
    public entityPM: SLAEscalationPM;
    public ObjectTableName: string = "SLAEscalation";
    public DataContext: EscalationArgs = this;
    public UserSelectedList: UserList[];
    public UsersCachedList: UserList[];
    public PreDefinitionList: EscalationPreDefinitionData[];
    public EscalationPreDefinitionCachedList: EscalationPreDefinitionList[];
    public EscalationFor: string;
    public isNew: boolean;
    public trigger: NewSLAComponent;
    private isUserFinised = false;
    private isPreDefinitionFinished = false;

    constructor(trigger: NewSLAComponent, entityPM: SLAEscalationPM, escalationFor: string, isNew: boolean) {
        super();
        this.trigger = trigger;
        this.isNew = isNew;
        this.entityPM = entityPM;
        this.EscalationFor = escalationFor;
        this.entityPM.EscalationFor = escalationFor;

        //Fill Users List
        this.UsersCachedList = trigger.UsersCachedList;
        this.EscalationPreDefinitionCachedList = trigger.EscalationPreDefinitionCachedList;
        this.UserSelectedList = [];
        this.PreDefinitionList =[];
        this.FillPredefinitionList();
    }
    private SetUIProperties() {
        var isEnabled = this.EscalationActionTimeIndicator == "IM" ? false : true;
        var isRequired = this.EscalationActionTimeIndicator == "IM" ? false : true;
        this.UIProperties.SetEnabled("EscalationTime", this.ObjectTableName, isEnabled);
        this.UIProperties.SetEnabled("EscalationTimeUnit", this.ObjectTableName, isEnabled);
        this.UIProperties.SetRequired("EscalationTime", this.ObjectTableName, isRequired && AppTool.IsNullOrEmpty(this.EscalationTime));
        this.UIProperties.SetRequired("EscalationTimeUnit", this.ObjectTableName, isRequired && AppTool.IsNullOrEmpty(this.EscalationTimeUnit));
    }
    public FillPredefinitionList() {
        this.PreDefinitionList = [];
        var service = new EscalationPreDefinitionListService();
        service.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var list: EscalationPreDefinitionList[] = myResponse.Result;
                if (list != null) {
                    list.forEach(item => {
                        this.PreDefinitionList.push(new EscalationPreDefinitionData(item));
                    });
                }
                this.isPreDefinitionFinished = true;
                this.Initialize();
            }
        });
    }
    private Initialize() {
        if (this.isPreDefinitionFinished) {
            if (!this.isNew) {
                var preDefinitionIds = [];
                if (this.entityPM != null) {
                    var usersIds = [];
                    this.entityPM.SLAEscalationRecepients.filter(a => a.UserId != null).forEach(item => {
                        usersIds.push(item.UserId);
                    });

                    this.entityPM.SLAEscalationRecepients.filter(a => a.PreDefinitionId != null).forEach(item => {
                        preDefinitionIds.push(item.PreDefinitionId);
                    });

                    var emailslist = [];
                    this.UsersCachedList.filter(a => usersIds.indexOf(a.Id) > -1).forEach(item => {
                        emailslist.push(item.Email);
                    });
                    this.Users = emailslist.join(";");
                    //this.UserSelectedList = this.UsersCachedList.filter(a => usersIds.indexOf(a.Id) > -1);

                    var preDefinitionsemails = [];
                    this.EscalationPreDefinitionCachedList.filter(a => preDefinitionIds.indexOf(a.Code) > -1).forEach(item => {
                        preDefinitionsemails.push(item.Name);
                    });
                    this.PreDefinitions = preDefinitionsemails.join(",");
                }

                this.PreDefinitionList.forEach(item => {
                    if (preDefinitionIds.indexOf(item.Code) != -1) {
                        item.IsChecked = true;
                    }
                });
            }
            this.SetUIProperties();
            this.getEscalationActionTime();
            this.getTimeIndicator();
        }
    }

    //Properties 
    get EscalationActionTimeIndicator() { return this.entityPM.EscalationActionTimeIndicator; }
    set EscalationActionTimeIndicator(value: string) {
        if (this.entityPM.EscalationActionTimeIndicator != value) {
            this.entityPM.EscalationActionTimeIndicator = value;
            this.EscalationTime = null;
            this.EscalationTimeUnit = null;
            this.SetUIProperties();
        }
    }

    get EscalationTime() { return this.entityPM.EscalationTime; }
    set EscalationTime(value: number) {
        if (this.entityPM.EscalationTime != value) {
            this.entityPM.EscalationTime = value;
            this.calculateEscalationInMinutes();
            this.SetUIProperties();
        }
    }

    get EscalaitonTimeInMinutes() { return this.entityPM.EscalaitonTimeInMinutes; }
    set EscalaitonTimeInMinutes(value: number) {
        if (this.entityPM.EscalaitonTimeInMinutes != value) {
            this.entityPM.EscalaitonTimeInMinutes = value;
        }
    }

    get EscalationTimeUnit() { return this.entityPM.EscalationTimeUnit; }
    set EscalationTimeUnit(value: string) {
        if (this.entityPM.EscalationTimeUnit != value) {
            this.entityPM.EscalationTimeUnit = value;
            this.calculateEscalationInMinutes();
            this.SetUIProperties();
        }
    }

    private calculateEscalationInMinutes() {
        if (this.EscalationTimeUnit == "II") {
            this.EscalaitonTimeInMinutes = this.EscalationTime;
        }

        if (this.EscalationTimeUnit == "YY") {
            var numOfMinutes = 24 * 60;
            this.EscalaitonTimeInMinutes = this.EscalationTime * numOfMinutes;
        }

        if (this.EscalationTimeUnit == "OO") {
            var numOfMinutes = 1 * 60;
            this.EscalaitonTimeInMinutes = this.EscalationTime * numOfMinutes;
        }
    }

    public TimeIndicator: string = "";
    private getEscalationActionTime() {
        var service = new EscalationActionTimeIndicatorListService();
        service.getSingleFromCache(this.entityPM.EscalationActionTimeIndicator).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var timeIndicator = myResponse.Result;
                if (timeIndicator != null) {
                    this.TimeIndicator = timeIndicator.Name;
                }
                else {
                    this.TimeIndicator = "";
                }
            }
        });
    }

    public TimeUnitName: string = "";
    private getTimeIndicator() {
        var service = new TimeUnitListService();
        service.getSingleFromCache(this.entityPM.EscalationTimeUnit).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var timeIndicator = myResponse.Result;
                if (timeIndicator != null) {
                    this.TimeUnitName = timeIndicator.Name;
                }
                else {
                    this.TimeUnitName = "";
                }
            }
        });
    }

    public Users: string;
    public PreDefinitions: string;

    public BuildEscalationRecepients() {
        this.entityPM.SLAEscalationRecepients.filter(a => a.SLAEscalationId == this.entityPM.Id).forEach(item => {
            this.entityPM.RemoveSLAEscalationRecepient(item);
        });

        if (this.UserSelectedList != null && this.UserSelectedList.length > 0) {
            this.UserSelectedList.forEach(item => {
                var escalationLine = new SLAEscalationRecepientPM(null);
                escalationLine.Tenant = SessionLocator.Tenant;
                escalationLine.SLAEscalationId = this.entityPM.Id;
                escalationLine.UserId = item.Id;

                if (this.entityPM.SLAEscalationRecepients.indexOf(escalationLine) == -1) {
                    this.entityPM.AddSLAEscalationRecepient(escalationLine);
                }
            });
        }

        if (this.PreDefinitionList != null && this.PreDefinitionList.length > 0) {
            this.PreDefinitionList.forEach(item => {
                if (item.IsChecked) {
                    var escalationLine = new SLAEscalationRecepientPM(null);
                    escalationLine.Tenant = SessionLocator.Tenant;
                    escalationLine.SLAEscalationId = this.entityPM.Id;
                    escalationLine.PreDefinitionId = item.Code;

                    if (this.entityPM.SLAEscalationRecepients.indexOf(escalationLine) == -1) {
                        this.entityPM.AddSLAEscalationRecepient(escalationLine);
                    }
                }
            });
        }

        //var usersIds = [];
        //this.entityPM.SLAEscalationRecepients.filter(a => a.UserId != null).forEach(item => {
        //    usersIds.push(item.UserId)
        //});

        var preDefinitionIds = [];
        this.entityPM.SLAEscalationRecepients.filter(a => a.PreDefinitionId != null).forEach(item => {
            preDefinitionIds.push(item.PreDefinitionId);
        });

        //var emailslist = [];
        //this.UsersCachedList.filter(a => usersIds.indexOf(a.Id) > -1).forEach(item => {
        //    emailslist.push(item.Email);
        //});
        //this.Users = emailslist.join(";");
        //this.UserSelectedList = this.UsersCachedList.filter(a => usersIds.indexOf(a.Id) > -1);

        var preDefinitionsemails = [];
        this.EscalationPreDefinitionCachedList.filter(a => preDefinitionIds.indexOf(a.Code) > -1).forEach(item => {
            preDefinitionsemails.push(item.Name);
        });
        this.PreDefinitions = preDefinitionsemails.join(",");
    }
}
export class EscalationPreDefinitionData extends BaseComponent{
    public entityPM: EscalationPreDefinitionList;
    public DataContext: EscalationPreDefinitionData = this;
    public ObjectTableName= "EscalationPreDefinition";
    constructor(entityPM: EscalationPreDefinitionList) {
        super();
        this.entityPM = entityPM;
    }

    // Properties
    get Name(){ return this.entityPM.Name; }
    set Name(value: string) { this.entityPM.Name = value; }

    get Code() { return this.entityPM.Code; }
    set Code(value: string) { this.entityPM.Code = value;}

    private isChecked: boolean = false;
    get IsChecked() {
        return this.isChecked;
    }
    set IsChecked(value: boolean)
    {
        if (this.isChecked != value) {
            this.isChecked = value;
        }
    }
}
