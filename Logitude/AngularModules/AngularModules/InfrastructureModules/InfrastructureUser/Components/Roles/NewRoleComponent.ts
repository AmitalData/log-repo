import {Component} from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {RolePM} from '../../../../Common/EntityPMs/RolePM';
import {RoleList} from '../../../../Common/EntityLists/RoleList';
import {RolePMService} from '../../../../Common/Services/StandardPMs/RolePMService';
import {RoleListService} from '../../../../Common/Services/StandardLists/RoleListService';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';

@Component({
    moduleId: module.id,
    templateUrl: './NewRoleComponent.html',
})

export class NewRoleComponent extends BaseComponent {
    public EntityPM: RolePM;
    public DataContext = this;
    public ObjectTableName: string = "Role";
    public ValidationErrorsList: string[] = [];
    public IsResourcesReady: boolean = false;
    public IsNewEntity: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        super();
        this.InitializeServices();
    }

    private myRolePMService: RolePMService;
    private myRoleListService: RoleListService;
    InitializeServices() {
        this.myRolePMService = new RolePMService();
        this.myRoleListService = new RoleListService();
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args['RolePM'];
        this.IsNewEntity = args['IsNew'];

        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {
            this.IsResourcesReady = true;
            this.SetUIProperties();
        });
    }

    SetUIProperties() {
        if (this.IsNewEntity) {
            var isFieldEnabled = false;
            var isFieldRequired = true;

            if (!AppTool.IsNullOrEmpty(this.ParentRoleId)) {
                isFieldEnabled = true;
                isFieldRequired = false;
            }

            this.UIProperties.SetRequired("ParentRoleId", this.ObjectTableName, isFieldRequired);
            this.UIProperties.SetEnabled("Name", this.ObjectTableName, isFieldEnabled);
            this.UIProperties.SetEnabled("RoleTypeCode", this.ObjectTableName, isFieldEnabled);
            this.UIProperties.SetEnabled("Description", this.ObjectTableName, isFieldEnabled);
        }

        else {
            this.UIProperties.SetEnabled("ParentRoleId", this.ObjectTableName, false);
        }
    }

    public get ParentRoleId() { return this.EntityPM.ParentRoleId; }
    public set ParentRoleId(value: string) {
        if (this.EntityPM.ParentRoleId != value) {
            this.EntityPM.ParentRoleId = value;
            this.SetUIProperties();

            if (AppTool.IsNullOrEmpty(value)) {
                this.Name = null;
                this.RoleTypeCode = null;
            }

            else {
                this.myRoleListService.getSingle(value).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var list: RoleList = myResponse.Result;
                        if (list != null) {
                            this.Name = list.Name;
                            this.RoleTypeCode = list.RoleTypeCode;
                        }
                    }
                });
            }
        }
    }

    public get RoleTypeCode() { return this.EntityPM.RoleTypeCode; }
    public set RoleTypeCode(value: string) {
        if (this.EntityPM.RoleTypeCode != value) {
            this.EntityPM.RoleTypeCode = value;
        }
    }

    public get Name() { return this.EntityPM.Name; }
    public set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
        }
    }

    public get Description() { return this.EntityPM.Description; }
    public set Description(value: string) {
        if (this.EntityPM.Description != value) {
            this.EntityPM.Description = value;
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (AppTool.IsNullOrEmpty(this.ParentRoleId)) {
            var msg: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");
            errors.push(msg.replace("%FieldName", "Parent Role"));
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            if (this.IsNewEntity) {
                var confirmWindow = new ConfirmWindow();
                confirmWindow.Show("The user who will be assigned this role will need to logout and login so the changes will take place");
                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {

                        this.CurrentSession.StartBusyIndicatorSaving();

                        this.myRolePMService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {

                            this.CurrentSession.StopBusyIndicator();

                            if (!myResponse.HasError) {
                                this.CurrentSession.CloseCurrentWindowEmit("OK");
                            }
                        });
                    }
                });
            }

            else {

                this.CurrentSession.StartBusyIndicatorSaving();

                this.myRolePMService.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {

                    this.CurrentSession.StopBusyIndicator();

                    if (!myResponse.HasError) {
                        this.CurrentSession.CloseCurrentWindowEmit("OK");
                    }
                });
            }
        }
    }
}
