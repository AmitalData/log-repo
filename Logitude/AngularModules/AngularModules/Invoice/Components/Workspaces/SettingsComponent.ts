import {Component, ViewChildren, QueryList} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';

@Component({
    selector: 'SettingsComponent',
    moduleId: module.id,
    templateUrl: './SettingsComponent.html',
})

export class SettingsComponent {
    private _entityResourceService: EntityResourceService;
    public IsResourcesReady: boolean = true;
    public IsVisible_SATInterface: boolean = false;
    public HasTransferFeature: boolean = false;
    public AccountingSystemName: string = null;
    public SATSettingsName: string = null;
    public IsVisible_AccountingSetting: boolean = false;
    constructor(private entityResourceService: EntityResourceService) {

        this._entityResourceService = new EntityResourceService();
        this.IsVisible_SATInterface = FeatureLocator.HasFeaturePermession("General", "SATINTERFACE") ? true : false;
        this.IsVisible_AccountingSetting = FeatureLocator.HasFeaturePermession("General", "HOWTOACCOUNTINGSETTINGS") ? true : false;

        if (FeatureLocator.HasFeaturePermession("General", "ACCOUNTINGTRANSFER")) {
            this.HasTransferFeature = true;
        }
        if (SessionLocator.SATInterfaceSettings) {
            this.SATSettingsName = SessionLocator.SATInterfaceSettings.SATInterfaceName;
        }
        if (SessionLocator.AccountingSystemPM) {
            this.AccountingSystemName = SessionLocator.AccountingSystemPM.Name;
        }
    }

    onSATSettingsClicked() {
        this._entityResourceService.getEntityResourceByTableName("SATInterfaceSetting", 0).subscribe(response => {
            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Width = 800;
            logitudeWindow.Height = 550;
            logitudeWindow.Title = "SAT Interface Settings";
            logitudeWindow.Show('./Invoice/Components/Workspaces/SATInterfaceSettingsComponent');
            logitudeWindow.WindowClosed.subscribe(e => {
                if (SessionLocator.SATInterfaceSettings) {
                    this.SATSettingsName = SessionLocator.SATInterfaceSettings.SATInterfaceName;
                }
            });
        });
    }

    OpenAccountingSystem() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Accounting Transfer Settings";
        logWindow.Show('./Invoice/Components/Workspaces/Windows/TransferSettingsComponent');
        logWindow.WindowClosed.subscribe(e => {           
                this.AccountingSystemName = SessionLocator.AccountingSystemPM.Name;            
        });
    }

    OpenAccountingSettings() {

        this._entityResourceService.getEntityResourceByTableName("AccountingSetting", 0).subscribe(response => {
            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Width = 800;
            logitudeWindow.Height = 550;
            logitudeWindow.Title = "Accounting Settings";
            logitudeWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/AccountingSettings/AccountingSettingsComponent');
            
        });
       
    }

    
}
