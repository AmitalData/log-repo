import {Component} from '@angular/core';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {TicketPM} from '../../../../../CRM/EntityPMs/TicketPM';
import {AppTool, DateTool} from '../../../../../Infrastructure/Tools';
import {TicketClassificationList} from '../../../../../CRM/EntityLists/TicketClassificationList';
import {TicketClassificationListService} from '../../../../../CRM/Services/StandardLists/TicketClassificationListService';
import {TicketStageListService} from '../../../../../CRM/Services/StandardLists/TicketStageListService';
import {ApiQueryFilters} from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {Validator} from '../../../../../Infrastructure/Validators/Validator';
import {TicketClosureArgs} from '../../../../../CRM/Args';
import {Cloner} from '../../../../../Infrastructure/Utilities/Cloner';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ServiceResponse}  from '../../../../../Infrastructure/DataContracts/ServiceResponse';


@Component({
    selector: 'TicketClosureComponent',
    moduleId: module.id,
    templateUrl: './TicketClosureComponent.html',
})

export class TicketClosureComponent extends BaseComponent {   
    public EntityPM: TicketPM;
    public ObjectTableName: string = "Ticket";
    public DataContext: TicketClosureComponent = this;
    public StageButtonCode: string;
    public IsOkClosed = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    SetWindowArgs(args: TicketClosureArgs) {
        if (args) {
            this.EntityPM = args.Ticket;
            this.StageButtonCode = args.StageCode;
        }
        this.SetUIProperties();
        this.getGeneralClassification();
        this.Clone();
    }

    private SetUIProperties() {
        this.UIProperties.SetEnabled("SecondaryClassificationId", this.ObjectTableName, !AppTool.IsNullOrEmpty(this.MainClassificationId));
    }

    // Properties
    get ClosureDescription() { return this.EntityPM.ClosureDescription; }
    set ClosureDescription(newValue: string) {
        if (this.EntityPM.ClosureDescription != newValue) {
            this.EntityPM.ClosureDescription = newValue;
        }
    }

    get MainClassificationId() { return this.EntityPM.MainClassificationId; }
    set MainClassificationId(newValue: string) {
        if (this.EntityPM.MainClassificationId != newValue) {
            this.EntityPM.MainClassificationId = newValue;
            this.getGeneralClassification();
            this.SecondaryClassificationId = null;
            this.SetUIProperties();
        }
    }

    get SecondaryClassificationId() { return this.EntityPM.SecondaryClassificationId; }
    set SecondaryClassificationId(newValue: string) {
        if (this.EntityPM.SecondaryClassificationId != newValue) {
            this.EntityPM.SecondaryClassificationId = newValue;
        }
    }

    public FirstClassificationId: string = "";
    private getGeneralClassification() {
        this.FirstClassificationId = "";
        var myService: TicketClassificationListService = new TicketClassificationListService();
        var filters = new ApiQueryFilters();
        myService.getAllFromCache(filters).subscribe((resp: any) => {
            if (!resp.HasError) {
                var result = resp.Result;
                var myClassification = result.filter(d => d.Name == "General" && d.Tenant == SessionLocator.TenantPM.Id)[0];
                if (myClassification != null) {
                    var filter: string = "!F";
                    this.FirstClassificationId = myClassification.Id.concat(filter);
                }
            }
        });
    }

    get SecondClassificationId() {
        var myGeneralId = "";
        if (this.MainClassificationId != null) {
            var filter: string = "!S";
            myGeneralId = this.MainClassificationId.concat(filter);
        }
        return myGeneralId;
    }

    get TicketTypeId() { return this.EntityPM.TicketTypeId; }
    set TicketTypeId(newValue: string) {
        if (this.EntityPM.TicketTypeId != newValue) {
            this.EntityPM.TicketTypeId = newValue;
        }
    }

    // Commands
    CancelButtonClicked() {
        this.RejectChanges();
        this.IsOkClosed = false;
        this.CurrentSession.CloseCurrentWindow();
    }

    public ValidationErrorsList: string[];
    OkButtonClicked() {
        var errors: string[] = [];
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (this.StageButtonCode == "CS") {
            this.EntityPM.IsClosed = true;
            this.EntityPM.LastCloseDate = DateTool.GetCurrentDateTimeAsUtc();
        }
        else {
            this.EntityPM.IsClosed = false;
        }
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            var myService: TicketStageListService = new TicketStageListService();
            myService.getAll().subscribe((resp: ServiceResponse) => {
                if (!resp.HasError) {
                    var list = resp.Result;
                    var stage = list.filter(d => d.Code == this.StageButtonCode && d.Tenant == SessionLocator.TenantPM.Id)[0];
                    if (stage != null) {
                        this.EntityPM.StageId = stage.Id;
                        this.EntityPM.StageCode = stage.Code;
                        this.EntityPM.StageName = stage.Name;
                    }
                    this.IsOkClosed = true;
                    this.CurrentSession.CloseCurrentWindowEmit("OK");
                }
                else {
                    this.ValidationErrorsList = resp.ErrorsArray;
                }
            });
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('TicketTypeId');
        this.myCloner.AddField('MainClassificationId');
        this.myCloner.AddField('SecondaryClassificationId');
        this.myCloner.AddField('ClosureDescription');
        this.myCloner.AddEntity(this.EntityPM);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
