import {Component, ComponentRef} from '@angular/core';
import {AppTool, DateTool, FontTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {CRMDomainService} from '../../Services/CRMDomainService';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {TicketClassificationPM} from '../../EntityPMs/TicketClassificationPM';
import {TicketClassificationPMService} from '../../Services/StandardPMs/TicketClassificationPMService';
import {TicketClassificationList} from '../../EntityLists/TicketClassificationList';
import {TicketClassificationListService} from '../../Services/StandardLists/TicketClassificationListService';
import {CRMTool} from '../../Tools';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow'; 

@Component({
    moduleId: module.id,
    templateUrl: './TicketClassificationMaintenanceComponent.html',
})

export class TicketClassificationMaintenanceComponent extends BaseComponent {
    public ObjectTableName = "TicketClassification";
    public DataContext: TicketClassificationMaintenanceComponent = this;
    public ComponentRef: ComponentRef<TicketClassificationMaintenanceComponent>;
    public Classifications: ClassificationData[] = [];
    public IsVisible = false;
    constructor() {
        super();
        this.Classifications = [];
    }

    Run() {
        //this.BuildTreeView();
    }

    //Classifications Tree
    private BuildTreeView() {
        this.Classifications = [];
        var service: TicketClassificationListService = new TicketClassificationListService();
        service.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var allClassifications: TicketClassificationList[] = myResponse.Result; 
                allClassifications.filter(d => d.ParentId == null).sort((a, b) => { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1 }).forEach(item => {
                    this.Classifications.push(new ClassificationData(item, allClassifications, item.Name, true, this));
                });

                this.IsVisible = true;
            }
        });
    }
    private selectedItem: ClassificationData;
    get SelectedItem() { return this.selectedItem; }
    set SelectedItem(value) {
        if (this.selectedItem != value) {
            this.selectedItem = value;

            if (value == null) {
                this.ClassificationTreePath = null;
            }

            else {
                this.ClassificationTreePath = value.FullName;
            }
        }
    }

    private classificationTreePath;
    get ClassificationTreePath() { return this.classificationTreePath; }
    set ClassificationTreePath(value: string) {
        this.classificationTreePath = value;
    }


    // Commands 
    public BackButtonClicked() {
        if (this.ComponentRef != null) {
            this.ComponentRef.destroy();
            this.ComponentRef = null;
        }
    }

    private collapseAll = false; 
    get CollapseAll() {
        return this.collapseAll;
    }
    set CollapseAll(value: boolean) {
        if (this.collapseAll != value) {
            this.collapseAll = value;
        }
    }
    public CollapseAllClicked() {
        this.CollapseAll = true;
    }
}
export class ClassificationData extends BaseComponent {
    private entity: TicketClassificationList;
    public ChildClassifications: ClassificationData[] = [];
    private trigger: TicketClassificationMaintenanceComponent;
    public ObjectTableName = "TicketClassification";
    public DataContext: ClassificationData = this;

    constructor(entity: TicketClassificationList, allClassifications: TicketClassificationList[], fullName: string, Isexpanded: boolean, trigger: TicketClassificationMaintenanceComponent) {
        super();
        this.entity = entity;
        this.ChildClassifications = [];
        this.FullName = fullName;
        this.trigger = trigger;
        allClassifications.filter(d => d.ParentId == entity.Id).sort((a, b) => { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1 }).forEach(item => {
            var name = fullName + "/" + item.Name;
            this.ChildClassifications.push(new ClassificationData(item, allClassifications, name, false, trigger));
        });

        this.IsExpanded = Isexpanded;
    }

    //Properties
    get Id() { return this.entity.Id; }
    get Name() { return this.entity.Name; } set Name(value: string) { this.entity.Name = value; }
    get ParentId() { return this.entity.ParentId; }
    public FullName: string;
    get Inactive() { return this.entity.Inactive; }
    set Inactive(value: boolean) { this.entity.Inactive = value; }

    get DefaultSeverityId() { return this.entity.DefaultSeverityId; } set DefaultSeverityId(value: string) { this.entity.DefaultSeverityId = value; }

    get EmployeeGroupId() { return this.entity.EmployeeGroupId; } set EmployeeGroupId(value: string) { this.entity.EmployeeGroupId = value; }

    get ManagerUserId() { return this.entity.ManagerUserId; } set ManagerUserId(value: string) { this.entity.ManagerUserId = value; }

    private isExpanded = false;
    get IsExpanded() { return this.isExpanded; }
    set IsExpanded(value: boolean) {
        this.isExpanded = value;
    }

    private expandAllOrNot = true;
    get ExpandAllOrNot() { return this.expandAllOrNot; }
    set ExpandAllOrNot(value: boolean) {
        this.expandAllOrNot = value;
    }

    private isSelected = false;
    get IsSelected() { return this.isSelected; }
    set IsSelected(value: boolean) {
        this.isSelected = value;
    }

    // Commands 
    public AddNewClassificationChild() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = "New Classification";
        var context = new ClassificationChildArgs(this.Id, null, true, this);
        logitudeWindow.DataContext = context;
        logitudeWindow.Show('./CRM/Components/Workspaces/AddEditClassificationComponent');
    }
    public EditClassificationChild() {
        var service: TicketClassificationPMService = new TicketClassificationPMService();
        service.get(this.Id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var editEntityPM: TicketClassificationPM = myResponse.Result;
                var logitudeWindow = new LogitudeWindow();
                logitudeWindow.Title = "Edit Classification";
                var context = new ClassificationChildArgs(editEntityPM.ParentId, editEntityPM, false, this);
                logitudeWindow.DataContext = context;
                logitudeWindow.Show('./CRM/Components/Workspaces/AddEditClassificationComponent');
            }
        });
    }

}
export class ClassificationChildArgs extends BaseComponent {
    public entityPM: TicketClassificationPM;
    public isNew: boolean;
    public ObjectTableName = "TicketClassification";
    public DataContext: ClassificationChildArgs = this;

    constructor(Id: string, entityPM: TicketClassificationPM, isNew: boolean, public trigger: ClassificationData) {
        super();
        this.isNew = isNew;

        if (isNew) {
            this.entityPM = new TicketClassificationPM();
            this.entityPM.Tenant = SessionLocator.Tenant;
        }

        else {
            this.entityPM = entityPM;
            //this.FillUsersList();
        }
        this.ParentId = Id;
    }

     //Properties
    get Name() { return this.entityPM.Name; }
    set Name(value:string)
    {
        this.entityPM.Name = value;
    }

    get Inactive() { return this.entityPM.Inactive; }
    set Inactive(value:boolean)
    {
        this.entityPM.Inactive = value;
    }

    get EmployeeGroupId() { return this.entityPM.EmployeeGroupId; }
    set EmployeeGroupId(value:string)
    {
        if (this.entityPM.EmployeeGroupId != value) {
            this.entityPM.EmployeeGroupId = value;
            this.SetUIRequiredProperties();
        }
    }

    get DefaultSeverityId() { return this.entityPM.DefaultSeverityId; }
    set DefaultSeverityId(value:string)
    {
        if (this.entityPM.DefaultSeverityId != value) {
            this.entityPM.DefaultSeverityId = value;
        }
    }

    get ManagerUserId() { return this.entityPM.ManagerUserId; }
    set ManagerUserId(value:string)
    {
        if (this.entityPM.ManagerUserId != value) {
            this.entityPM.ManagerUserId = value;
            this.SetUIRequiredProperties();
        }
    }

    get EscalationNotify(){ return this.entityPM.EscalationNotify; }
    set EscalationNotify(value:string)
    {
        if (this.entityPM.EscalationNotify != value) {
            this.entityPM.EscalationNotify = value;
        }
    }

    get ParentId() { return this.entityPM.ParentId; }
    set ParentId(value: string) {
        if (this.entityPM.ParentId != value) {
            this.entityPM.ParentId = value;
        }
    }

    private SetUIRequiredProperties() {
        if (!AppTool.IsNullOrEmpty(this.Name) && this.Name == "General") {
            if (AppTool.IsNullOrEmpty(this.EmployeeGroupId))
                this.UIProperties.SetRequired("EmployeeGroupId", this.ObjectTableName, true);
        }
    }
}