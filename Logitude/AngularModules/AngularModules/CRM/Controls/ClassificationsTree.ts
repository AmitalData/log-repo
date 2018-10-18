import { Component, EventEmitter, Output, OnInit} from '@angular/core';
import {TicketClassificationList} from '../EntityLists/TicketClassificationList';
import {TicketClassificationListService} from '../Services/StandardLists/TicketClassificationListService';
import {CRMDomainService} from '../Services/CRMDomainService';
import {TicketClassificationPM} from '../EntityPMs/TicketClassificationPM';
import {TicketClassificationPMService} from '../Services/StandardPMs/TicketClassificationPMService';
import {CRMTool} from '../../CRM/Tools';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {BaseComponent} from '../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {LogitudeWindow} from '../../Controls/Windows/LogitudeWindow';
import {AppTool} from '../../Infrastructure/Tools';

@Component({
    selector: 'ClassificationsTree',
    moduleId: module.id,
    templateUrl: './ClassificationsTree.html',
    inputs: ['Item', 'CollapseAll'],
})

export class ClassificationsTree implements OnInit {
    public Items: ClassificationsTreeItem[] = [];
    public AllClassification: TicketClassificationList[] = [];
    public Item: ClassificationsTreeItem;

    private collapseAll = false;
    get CollapseAll() {
        return this.collapseAll;
    }
    set CollapseAll(value: boolean) {
        if (this.collapseAll != value) {
            this.collapseAll = value;
            if (this.collapseAll) {
                this.Items.forEach(item => {
                    item.IsExpanded = false;
                });
            }
        }
    }

    constructor() {

    }

    ngOnInit() {
        this.Items = [];
        if (this.Item == null) {
            var service: TicketClassificationListService = new TicketClassificationListService();
            service.getAll().subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.AllClassification = myResponse.Result;
                    var level = 0;
                    if (this.AllClassification.length > 1) {
                        level = 1;
                    }
                    this.AllClassification.filter(f => f.ParentId == null).sort((a, b) => { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1 }).forEach(item => {
                        this.Items.push(new ClassificationsTreeItem(item, this.AllClassification, this, level, true));
                    });
                }
            });
        }
        else {
            this.Item.AllClassification.filter(f => f.ParentId == this.Item.Id).sort((a, b) => { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1 }).forEach(item => {
                this.Items.push(new ClassificationsTreeItem(item, this.Item.AllClassification, this, this.Item.Level + 1, true));
            });
        }
    }
    public BuildTreeView() {
        this.Items = [];
        var service: TicketClassificationListService = new TicketClassificationListService();
        service.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllClassification = myResponse.Result;
                if (this.Item == null) {
                    var level = 0;
                    if (this.AllClassification.length > 1) {
                        level = 1;
                    }
                    this.AllClassification.filter(f => f.ParentId == null).sort((a, b) => { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1 }).forEach(item => {
                        this.Items.push(new ClassificationsTreeItem(item, this.AllClassification, this, level, true));
                    });
                }
                else {
                    this.Item.AllClassification = this.AllClassification;
                    this.Item.AllClassification.filter(f => f.ParentId == this.Item.Id).sort((a, b) => { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1 }).forEach(item => {
                        this.Items.push(new ClassificationsTreeItem(item, this.Item.AllClassification, this, this.Item.Level + 1, true));
                    });
                }
            }
        });
    }

    private selectedItem: ClassificationsTreeItem;
    get SelectedItem() { return this.selectedItem; }
    set SelectedItem(value) {
        if (this.selectedItem != value) {
            this.selectedItem = value;
        }
    }
}
class ClassificationsTreeItem {
    public Level: number = 0;
    public HasItems: boolean = false;
    private entity: TicketClassificationList;
    public NameColor: string = "black";

    public AllClassification: TicketClassificationList[] = [];
    constructor(item: TicketClassificationList, AllClassification: TicketClassificationList[], public Father: ClassificationsTree, level: number, isExpanded: boolean) {
        this.entity = item;
        this.AllClassification = AllClassification;
        this.Level = level;
        this.HasItems = this.AllClassification.filter(f => f.ParentId == this.Id).length > 0 ? true : false;
        this.IsExpanded = isExpanded;
        if (this.Inactive) {
            this.NameColor = "gray";
        }
    }

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
        logitudeWindow.WindowClosed.subscribe(($event: any) => this.OnWindowClosed($event));
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
                logitudeWindow.WindowClosed.subscribe(($event: any) => this.OnWindowClosed($event));
                logitudeWindow.Show('./CRM/Components/Workspaces/AddEditClassificationComponent');
            }
        });
    }
    private OnWindowClosed(arg) {
        if (arg == "OK") {
            this.Father.BuildTreeView();
        }
    }
}
export class ClassificationChildArgs extends BaseComponent {
    public entityPM: TicketClassificationPM;
    public isNew: boolean;
    public ObjectTableName = "TicketClassification";
    public DataContext: ClassificationChildArgs = this;

    constructor(Id: string, entityPM: TicketClassificationPM, isNew: boolean, public trigger: ClassificationsTreeItem) {
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
    set Name(value: string) {
        this.entityPM.Name = value;
    }

    get Inactive() { return this.entityPM.Inactive; }
    set Inactive(value: boolean) {
        this.entityPM.Inactive = value;
    }

    get EmployeeGroupId() { return this.entityPM.EmployeeGroupId; }
    set EmployeeGroupId(value: string) {
        if (this.entityPM.EmployeeGroupId != value) {
            this.entityPM.EmployeeGroupId = value;
            this.SetUIRequiredProperties();
        }
    }

    get DefaultSeverityId() { return this.entityPM.DefaultSeverityId; }
    set DefaultSeverityId(value: string) {
        if (this.entityPM.DefaultSeverityId != value) {
            this.entityPM.DefaultSeverityId = value;
        }
    }

    get ManagerUserId() { return this.entityPM.ManagerUserId; }
    set ManagerUserId(value: string) {
        if (this.entityPM.ManagerUserId != value) {
            this.entityPM.ManagerUserId = value;
            this.SetUIRequiredProperties();
        }
    }

    get EscalationNotify() { return this.entityPM.EscalationNotify; }
    set EscalationNotify(value: string) {
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