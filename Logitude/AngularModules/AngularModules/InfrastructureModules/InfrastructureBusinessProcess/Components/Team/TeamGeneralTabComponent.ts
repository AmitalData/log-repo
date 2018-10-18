import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {DateTool, AppTool} from '../../../../Infrastructure/Tools'
import {TeamPMService} from '../../../../Infrastructure/Services/StandardPMs/TeamPMService';
import {TeamPM} from '../../../../Infrastructure/EntityPMs/TeamPM';
import {LBPTeamMemberPM} from '../../../../Infrastructure/EntityPMs/LBPTeamMemberPM'; 
import {TeamMemberBusinessRolePM} from '../../../../Infrastructure/EntityPMs/TeamMemberBusinessRolePM'; 
import {TeamPMInitService} from '../../../../Infrastructure/EntityPMInitServices/TeamPMInitService';
import {BusinessRoleList} from '../../../../Infrastructure/EntityLists/BusinessRoleList';
import {BusinessRoleListService} from '../../../../Infrastructure/Services/StandardLists/BusinessRoleListService';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {BusinessRoleExtendedListService} from '../../../../Infrastructure/Services/ExtendedLists/BusinessRoleExtendedListService';

@Component({
    selector: 'TeamGeneralTabComponent',
    moduleId: module.id,
    templateUrl: './TeamGeneralTabComponent.html',
})

export class TeamGeneralTabComponent extends BaseComponent implements OnInit {
    public Session: number = SessionLocator.Tenant;
    public EntityPM: TeamPM;
    public DataContext: TeamGeneralTabComponent = this;
    public ObjectTableName: string = "Team";
    public MembersLines: MembersLineData[];
    public BusinessRolesList: Array<BusinessRoleList> = [];
    public TeamsFilterItems: ApiQueryFilters;

    constructor(private entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.TeamsFilterItems = new ApiQueryFilters();
        this.TeamsFilterItems.addAdditionalFilter("Id", this.EntityPM.Id, null, null, "NotEqual", false, false, false, "String");

    }

    ngOnInit() {
        this.GetBusinessRoles();
    }

    private GetBusinessRoles() {
        var service: BusinessRoleListService = new BusinessRoleListService();
        service.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.BusinessRolesList = myResponse.Result;
                this.FillMemberLines();
            }
        });
    }

    public FillMemberLines() {
        this.MembersLines = [];
        if (this.EntityPM.MemberLines.length > 0) {
            this.EntityPM.MemberLines.forEach(item => {
                this.MembersLines.push(new MembersLineData(item, this));
            });
        }
    }

    public get Name() { return this.EntityPM.Name; }
    public set Name(value: string) {
        if (this.EntityPM.Name != value)
            this.EntityPM.Name = value;
    }

    public get LocalName() { return this.EntityPM.LocalName; }
    public set LocalName(value: string) {
        if (this.EntityPM.LocalName != value)
            this.EntityPM.LocalName = value;
    }

    public get ManagerUserId() { return this.EntityPM.ManagerUserId; }
    public set ManagerUserId(value: string) {
        if (this.EntityPM.ManagerUserId != value)
            this.EntityPM.ManagerUserId = value;
    }

    public get Notify() { return this.EntityPM.Notify; }
    public set Notify(value: string) {
        if (this.EntityPM.Notify != value)
            this.EntityPM.Notify = value;
    }

    public get InActive() { return this.EntityPM.InActive; }
    public set InActive(value: boolean) {
        if (this.EntityPM.InActive != value)
            this.EntityPM.InActive = value;
    }

    public get Notes() { return this.EntityPM.Notes; }
    public set Notes(value: string) {
        if (this.EntityPM.Notes != value)
            this.EntityPM.Notes = value;
    }

    NewMemberLine() {
        var newLine: LBPTeamMemberPM = new LBPTeamMemberPM(null);
        newLine.Tenant = SessionLocator.Tenant
        newLine.TeamId = this.EntityPM.Id;
        newLine.AddedByUserId = this.EntityPM.CreatedByUserId;
        newLine.ChangeSetOp = "Insert";
        this.MembersLines.push(new MembersLineData(newLine, this));
    }

    DeleteTeamMemberLine(deletedItem: MembersLineData) {
        var selectedLinePM: LBPTeamMemberPM = deletedItem.MemberPM;

        if (this.EntityPM.MemberLines.indexOf(selectedLinePM) != -1) {
            selectedLinePM.ChangeSetOp = "Delete";
            this.EntityPM.RemoveLBPTeamMember(selectedLinePM);
        }
        var index = this.MembersLines.indexOf(deletedItem);
        if (index > -1) {
            this.MembersLines.splice(index, 1);
        }
    }
}
export class MembersLineData extends BaseComponent {

    public MemberPM: LBPTeamMemberPM;
    private TeamPM: TeamPM;
    public ObjectTableName: string = "LBPTeamMember";
    public DataContext: MembersLineData = this;
    public MembersTypes: TypeClass[] = [];

    public BusinessRolesToggleList: Array<ToggleBusinessRoleData> = [];
    public BusinessRolesList: Array<BusinessRoleData> = [];

    constructor(entity: LBPTeamMemberPM, public father: TeamGeneralTabComponent) {
        super();
        this.MemberPM = entity;
        this.TeamPM = father.EntityPM;
        this.FillBusinessRolesList();
        this.FillToggleBusinessRoles();
        this.CreateTypes(); 
    }

    FillToggleBusinessRoles() {
        this.BusinessRolesToggleList = [];
        if (this.father.BusinessRolesList != null) {
            this.father.BusinessRolesList.forEach(item => {
                this.BusinessRolesToggleList.push(new ToggleBusinessRoleData(item, this));
            });
        }
    }

    public FillBusinessRolesList() {
        this.BusinessRolesList = [];
        if (this.MemberPM.BusinessRolesList != null) {
            this.MemberPM.BusinessRolesList.forEach(item => {
                this.BusinessRolesList.push(new BusinessRoleData(item, this));
            });
        }
    }

    public get MemberTeamId() { return this.MemberPM.MemberTeamId; }
    public set MemberTeamId(value: string) {
        if (this.MemberPM.MemberTeamId != value) {
            this.MemberPM.MemberTeamId = value;
            this.OnMemberChanged(value);
        }

    }

    public get MemberUserId() { return this.MemberPM.MemberUserId; }
    public set MemberUserId(value: string) {
        if (this.MemberPM.MemberUserId != value)
            this.MemberPM.MemberUserId = value; {
            this.OnMemberChanged(value);
            //this.Load();
        }
    }

    private Load() {
        var service: BusinessRoleExtendedListService = new BusinessRoleExtendedListService();
        service.getToggleBusinessRoles(this.MemberPM.Id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.FillToggleBusinessRoles();
            }
        });
    }

    private selectedFilter: TypeClass = null;
    get SelectedFilter() {
        return this.selectedFilter;
    }
    set SelectedFilter(value: TypeClass) {
        if (this.selectedFilter != value) {
            this.selectedFilter = value;
        }
    }

    CreateTypes() {
        this.MembersTypes = [];
        var s_entity = new TypeClass();
        s_entity.Code = "1";
        s_entity.Name = "User";
        this.MembersTypes.push(s_entity);

        var q_entity = new TypeClass();
        q_entity.Code = "2";
        q_entity.Name = "Team";
        this.MembersTypes.push(q_entity);

        if (!AppTool.IsNullOrEmpty(this.MemberTeamId)) {
            this.selectedFilter = q_entity;
        }

        if (!AppTool.IsNullOrEmpty(this.MemberUserId)) {
            this.selectedFilter = s_entity;
        }
    }

    private OnMemberChanged(Id: string) {
        if (AppTool.IsNullOrEmpty(Id)) {
            if (this.TeamPM.MemberLines.indexOf(this.MemberPM) != -1) {
                this.MemberPM.ChangeSetOp = "Delete";
                this.TeamPM.RemoveLBPTeamMember(this.MemberPM);
            }
        }

        else {
            if (this.TeamPM.MemberLines.indexOf(this.MemberPM) == -1) {
                this.MemberPM.ChangeSetOp = "Insert";
                this.TeamPM.AddLBPTeamMember(this.MemberPM);
            }
        }
    }

    DeleteBuseinssRole(role: TeamMemberBusinessRolePM) {
        var item: BusinessRoleData = this.BusinessRolesList.filter(d => d.TeamMemberId == role.TeamMemberId && d.BusinessRoleId == role.BusinessRoleId)[0];
        if (item != null) {
            this.BusinessRolesList = this.BusinessRolesList.filter(obj => obj !== item);
            if (this.MemberPM.BusinessRolesList.indexOf(item.EntityPM) != -1) {
                item.EntityPM.ChangeSetOp = "Delete";
                this.MemberPM.RemoveTeamMemberBusinessRole(item.EntityPM);
            }
        }
    }
}
export class ToggleBusinessRoleData extends BaseComponent {

    public EntityPM: BusinessRoleList;
    public ObjectTableName: string = "BusinessRole";
    public DataContext: ToggleBusinessRoleData = this;

    constructor(entity: BusinessRoleList, public trigger: MembersLineData) {
        super();
        this.EntityPM = entity;
        this.isChecked = trigger.MemberPM.BusinessRolesList.filter(d => d.TeamMemberId == this.trigger.MemberPM.Id && d.BusinessRoleId == this.EntityPM.Id)[0] != null ? true : false ;
    }

    public get Name() { return this.EntityPM.Name; }
    public set Name(value: string) {
        if (this.EntityPM.Name != value)
            this.EntityPM.Name = value;
    }

    public get LocalName() { return this.EntityPM.LocalName; }
    public set LocalName(value: string) {
        if (this.EntityPM.LocalName != value)
            this.EntityPM.LocalName = value;
    }

    private isChecked: boolean;
    public get IsChecked() { return this.isChecked; }
    public set IsChecked(value: boolean) {
        if (this.isChecked != value) {
            this.isChecked = value;
            if (value) {
                var newItemPM: TeamMemberBusinessRolePM = new TeamMemberBusinessRolePM(null);
                newItemPM.Tenant = this.EntityPM.Tenant;
                newItemPM.ChangeSetOp = "Insert";
                newItemPM.TeamMemberId = this.trigger.MemberPM.Id;
                newItemPM.BusinessRoleId = this.EntityPM.Id;
                newItemPM.AddedByUserId = this.EntityPM.CreatedByUserId;
                newItemPM.RoleName = this.Name;
                 
                if (newItemPM != null) {
                    if (this.trigger.MemberPM.BusinessRolesList.indexOf(newItemPM) == -1) {
                        newItemPM.ChangeSetOp = "Insert";
                        this.trigger.MemberPM.AddTeamMemberBusinessRole(newItemPM);
                    }
                }
            }
            else {
                var itemPM: TeamMemberBusinessRolePM = this.trigger.MemberPM.BusinessRolesList.filter(d => d.TeamMemberId == this.trigger.MemberPM.Id && d.BusinessRoleId == this.EntityPM.Id)[0];
                if (itemPM != null) {
                    if (this.trigger.MemberPM.BusinessRolesList.indexOf(itemPM) != -1) {
                        itemPM.ChangeSetOp = "Delete";
                        this.trigger.MemberPM.RemoveTeamMemberBusinessRole(itemPM);
                    }
                }
            }
            this.trigger.FillBusinessRolesList();
        }
    }
}
export class BusinessRoleData extends BaseComponent {

    public EntityPM: TeamMemberBusinessRolePM;
    public ObjectTableName: string = "TeamMemberBusinessRole";
    public DataContext: BusinessRoleData = this;

    constructor(entity: TeamMemberBusinessRolePM, public trigger: MembersLineData) {
        super();
        this.EntityPM = entity;
    }

    public get Id() { return this.EntityPM.Id; }
    public get TeamMemberId() { return this.EntityPM.TeamMemberId; }
    public get BusinessRoleId() { return this.EntityPM.BusinessRoleId; }

    public get RoleName() { return this.EntityPM.RoleName; }
    public set RoleName(value: string) {
        if (this.EntityPM.RoleName != value) {
            this.EntityPM.RoleName = value;           
        }
    }
}
export class TypeClass {
    public Code: string;
    public Name: string;
}