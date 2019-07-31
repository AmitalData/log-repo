import {Component, OnDestroy}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UserPM} from '../../../../Common/EntityPMs/UserPM';
import {TwoFactorAuthenticationDevicePM} from '../../../../Common/EntityPMs/TwoFactorAuthenticationDevicePM';
import {UserRolesPM} from '../../../../Common/EntityPMs/UserRolesPM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {ExcelExportService} from '../../../../Common/Services/Others/ExcelExportService';
import {TwoFactorAuthenticationDeviceExtendedPMService} from '../../../../Common/Services/ExtendedPMs/TwoFactorAuthenticationDeviceExtendedPMService';
import {TwoFactorAuthenticationDevicePMService} from '../../../../Common/Services/StandardPMs/TwoFactorAuthenticationDevicePMService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {AppTool} from '../../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    templateUrl: './DevicesTabComponent.html',
})

export class DevicesTabComponent extends BaseComponent {
    public EntityPM: UserPM;
    public ObjectTableName: string = "TwoFactorAuthenticationDevice";
    public DataContext = this;
    public ItemsSource: TwoFactorAuthenticationDevicePM[] = [];
    private twoFactorAuthenticationDeviceExtendedPMService: TwoFactorAuthenticationDeviceExtendedPMService;
    private twoFactorAuthenticationDevicePMService: TwoFactorAuthenticationDevicePMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.twoFactorAuthenticationDeviceExtendedPMService = new TwoFactorAuthenticationDeviceExtendedPMService();
        this.twoFactorAuthenticationDevicePMService = new TwoFactorAuthenticationDevicePMService();

        this.twoFactorAuthenticationDeviceExtendedPMService.GetDevicesByUser(this.EntityPM.Id).subscribe(response => {
            if (!response.HasError) {
                this.ItemsSource = response.Result;
            }
        });
        
    }

    DeviceActivation(device: TwoFactorAuthenticationDevicePM, isActive: boolean) {
        device.InActive = !isActive;
        this.CurrentSession.StartBusyIndicatorSaving();
        this.twoFactorAuthenticationDevicePMService.update(device).subscribe(response => {
            this.CurrentSession.StopBusyIndicator();
            if (response.HasError) {
            }

        });

    }

}
