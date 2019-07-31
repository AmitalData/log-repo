import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UIProperty, UIProperties}  from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties'
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {CustomsInterfaceSettingPM} from '../../../EntityPMs/CustomsInterfaceSettingPM';
import {FTPDetailPM} from '../../../EntityPMs/FTPDetailPM';
import {FTPDetailPMService} from '../../../Services/StandardPMs/FTPDetailPMService';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    moduleId: './Common/Components/Maintenance/CustomsInterface/',
    templateUrl: 'ArtemusSettingsComponent.html',
})

export class ArtemusSettingsComponent extends BaseComponent {
    public EntityPM: CustomsInterfaceSettingPM;
    public ObjectTableName: string;
    public DataContext: ArtemusSettingsComponent = this;
    public ValidationErrorsList: string[] = [];
    private myFTPService: FTPDetailPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.myFTPService = new FTPDetailPMService();
    }

    SetWindowArgs(entityPM: CustomsInterfaceSettingPM) {
        this.EntityPM = entityPM;
        this.ObjectTableName = "CustomsInterfaceSetting";

        this.GetData();
        this.Clone();
    }

    private GetData() {
        if (!AppTool.IsNullOrEmpty(this.ArtemusOutSettingsId)) {
            this.Load(this.ArtemusOutSettingsId, "OUT");
        }

        if (!AppTool.IsNullOrEmpty(this.ArtemusInSettingsId)) {
            this.Load(this.ArtemusInSettingsId, "IN");
        }
    }
    private Load(id: string, code: string) {
        this.myFTPService.get(id).subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;

            if (!myResponse.HasError) {
                var myEntity: FTPDetailPM = myResponse.Result;

                if (myEntity != null) {
                    if (code == "OUT") {
                        this.ArtemusOutSettingsHost = myEntity.Host;
                    }

                    else if (code == "IN") {
                        this.ArtemusInSettingsHost = myEntity.Host;
                    }
                }
            }
        });
    }
    
    get ArtemusOutSettingsId() { return this.EntityPM.ArtemusOutSettingsId; }
    set ArtemusOutSettingsId(newValue: string) {
        if (this.EntityPM.ArtemusOutSettingsId != newValue) {
            this.EntityPM.ArtemusOutSettingsId = newValue;
        }
    }

    get ArtemusOutSettingsHost() { return this.EntityPM.ArtemusOutSettingsHost; }
    set ArtemusOutSettingsHost(newValue: string) {
        if (this.EntityPM.ArtemusOutSettingsHost != newValue) {
            this.EntityPM.ArtemusOutSettingsHost = newValue;
        }
    }

    get ArtemusInSettingsId() { return this.EntityPM.ArtemusInSettingsId; }
    set ArtemusInSettingsId(newValue: string) {
        if (this.EntityPM.ArtemusInSettingsId != newValue) {
            this.EntityPM.ArtemusInSettingsId = newValue;
        }
    }  

    get ArtemusInSettingsHost() { return this.EntityPM.ArtemusInSettingsHost; }
    set ArtemusInSettingsHost(newValue: string) {
        if (this.EntityPM.ArtemusInSettingsHost != newValue) {
            this.EntityPM.ArtemusInSettingsHost = newValue;
        }
    }
    
    Add(myCode: string) {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Add FTP Detail";
        logWindow.WindowArgs = { Code: myCode, IsNew: true };
        logWindow.Show('./Common/Components/Maintenance/CustomsInterface/FTPDetailComponent');

        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    if (myCode == "OUT") {
                        this.ArtemusOutSettingsId = comp.EntityPM.Id;
                        this.ArtemusOutSettingsHost = comp.EntityPM.Host;

                    }

                    else if (myCode == "IN") {
                        this.ArtemusInSettingsId = comp.EntityPM.Id;
                        this.ArtemusInSettingsHost = comp.EntityPM.Host;
                    }                    
                }
            });
        });
    }
    Edit(myCode: string) {
        var settingId = null;

        if (myCode == "OUT") {
            settingId = this.EntityPM.ArtemusOutSettingsId;
        }

        else if (myCode == "IN") {
            settingId = this.EntityPM.ArtemusInSettingsId
        }

        if (!AppTool.IsNullOrEmpty(settingId)) {       
            var logWindow = new LogitudeWindow();
            logWindow.Title = "Edit FTP Detail";
            logWindow.WindowArgs = { Code: myCode, IsNew: false, EntityId: settingId };
            logWindow.Show('./Common/Components/Maintenance/CustomsInterface/FTPDetailComponent');

            logWindow.ComponentLoaded.subscribe(comp => {
                logWindow.WindowClosed.subscribe(s => {
                    if (s) {
                        if (myCode == "OUT") {
                            this.ArtemusOutSettingsId = comp.EntityPM.Id;
                            this.ArtemusOutSettingsHost = comp.EntityPM.Host;

                        }

                        else if (myCode == "IN") {
                            this.ArtemusInSettingsId = comp.EntityPM.Id;
                            this.ArtemusInSettingsHost = comp.EntityPM.Host;
                        }
                    }
                });
            });
        } 
    }
    
    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('ArtemusOutSettingsId');
        this.myCloner.AddField('ArtemusInSettingsId');
        this.myCloner.AddEntity(this.EntityPM);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
