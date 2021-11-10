import {Component, EventEmitter}  from '@angular/core';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {TenantPMService} from '../../Common/Services/StandardPMs/TenantPMService';
import {TenantPM} from '../../Common/EntityPMs/TenantPM';
import {Cloner} from '../../Infrastructure/Utilities/Cloner';
import {BaseComponent} from '../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {CutsomerTenantAccessManagementComponent} from './CutsomerTenantAccessManagementComponent';
import {AppTool} from '../../Infrastructure/Tools';
@Component({
    moduleId: './SharedLogistics/Components/',
    selector: 'TenantAccessSettingsComponent',
    templateUrl: 'TenantAccessSettingsComponent.html',
})
export class TenantAccessSettingsComponent extends BaseComponent  {
    public EntityPM: TenantPM;
    public Parent: CutsomerTenantAccessManagementComponent;
    public ValidationErrorsList: string[] = [];
    public DataContext: TenantAccessSettingsComponent = this;
    public get LogBoxAdminUserId() {
        return this.EntityPM != null ? this.EntityPM.LogBoxAdminUserId : null;
    }
    public set LogBoxAdminUserId(value: string) {
        if (this.EntityPM.LogBoxAdminUserId != value)
            this.EntityPM.LogBoxAdminUserId = value;
    }

    public get IsCustomerTenantShareEnable() {
        return this.EntityPM != null ? this.EntityPM.CustomerTenantShareCustomsFile == true ? false : true : null;
    }
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }
    public get CustomerTenantShareCustomsFile() {
        return this.EntityPM != null ? this.EntityPM.CustomerTenantShareCustomsFile : null;
    }

    public set CustomerTenantShareCustomsFile(value: boolean) {
        if (this.EntityPM.CustomerTenantShareCustomsFile != value) {
            this.EntityPM.CustomerTenantShareCustomsFile = value;
            this.Parent.RefreshTenantScreenData();
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.ValidationErrorsList = [];
        if (this.EntityPM.CustomerTenantShareCustomsFile && !AppTool.IsNullOrEmpty(this.EntityPM.LogBoxAdminUserId)) {
            this.CurrentSession.StartBusyIndicatorSaving();
            var service: TenantPMService = new TenantPMService();
            service.update(this.EntityPM).subscribe((res:any) => {
                this.CurrentSession.StopBusyIndicator();
                SessionLocator.TenantPM = this.EntityPM;
                this.CurrentSession.CloseCurrentWindow();
            });           
        }
        else {
            this.ValidationErrorsList.push("LogBox Administrator User is required");
        }
    }



    SetWindowArgs(args: any) {
        this.EntityPM = args.EntityPM;
        this.Parent = args.Parent;
    }

}
