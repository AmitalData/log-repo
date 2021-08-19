 
import {Component, ViewChildren, QueryList} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {SATInterfaceSettingPMService} from '../../Services/StandardPMs/SATInterfaceSettingPMService';
import {SATInterfaceSettingPM} from '../../EntityPMs/SATInterfaceSettingPM';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {CommonDomainService} from '../../../Common/Services/CommonDomainService';
import {AppTool} from '../../../Infrastructure/Tools';

@Component({
    selector: 'SATInterfaceSettingsComponent',
    
    templateUrl: './SATInterfaceSettingsComponent.html',
})

export class SATInterfaceSettingsComponent {
  public ActivationDate: any;


    private _entityResourceService: EntityResourceService;
    private sATInterfaceSettingPMService: SATInterfaceSettingPMService;
    public IsResourcesReady: boolean = true;
    public EntityPM: SATInterfaceSettingPM;
    public ObjectTableName: string = "SATInterfaceSetting";
    public ValidationErrorsList: string[];
    //public SATFolderName = "FromLogitude\SAT";
    IsDropboxConnected: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsCartaPorteSettingsEnabled: boolean = false;
    constructor(private entityResourceService: EntityResourceService) {
        this._entityResourceService = new EntityResourceService();
        this.sATInterfaceSettingPMService = new SATInterfaceSettingPMService();

        //entityResourceService.getEntityResourceByTableName("SATInterfaceSetting").subscribe(res1 => {
            this.LoadData();
        //});


        this.IsCartaPorteSettingsEnabled = SessionLocator.FeatureToggles.some(d => d.ToggleCode == "CPT");
    }

    private LoadData() {
        this.sATInterfaceSettingPMService.get(SessionLocator.Tenant).subscribe((response:any) => {
            if (!response.HasError) {
                this.EntityPM = response.Result;
               
            }

            this.IsResourcesReady = true;
        });
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        this.ValidationErrorsList = [];
        if (AppTool.IsNullOrEmpty(this.EntityPM.SATInterfaceCode)) {
            this.ValidationErrorsList.push("SAT Interface Code field is required!");
        }

        if ((this.EntityPM.SATInterfaceCode === "PROF" || this.EntityPM.SATInterfaceCode == "PROF33") && AppTool.IsNullOrEmpty(this.EntityPM.Token)) {
            this.ValidationErrorsList.push("Token field is required!");
        }
        if (this.ValidationErrorsList.length > 0)
            return;

       
        if (this.EntityPM.SATInterfaceCode === "CONT") {
            this.CheckDropBoxAndSave();
        }
        else {
            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
            this.sATInterfaceSettingPMService.update(this.EntityPM).subscribe((response:any) => {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                if (response.HasError) {
                    this.ValidationErrorsList = response.ErrorsArray;
                }
                else {

                    this.sATInterfaceSettingPMService.get(SessionLocator.Tenant).subscribe((response:any) => {
                        if (!response.HasError) {
                            SessionLocator.SATInterfaceSettings  = response.Result;

                        }

                        this.CurrentSession.CloseCurrentWindow();
                    });

                   
                }
            });
        }
    }

    ViewDropboxConnection() {
        var windowTitle = "Dropbox Connection";
        var logWindow = new LogitudeWindow();
        logWindow.Width = 350;
        logWindow.Height = 225;
        logWindow.Title = windowTitle;
        logWindow.IsShowCloseButton = false;
        logWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/DropBox/DropBoxConnectionComponent');
    }

    CheckDropBoxAndSave() {
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
        var myService: CommonDomainService = new CommonDomainService();
        myService.GetDropBoxAccessTocken(SessionLocator.Tenant).subscribe((myResult:any) => {
            if (myResult.HasError == false && !AppTool.IsNullOrEmpty(myResult.Result.DropBoxAccessToken)) {
                this.IsDropboxConnected = true;
            }
            if (this.IsDropboxConnected) {
                this.sATInterfaceSettingPMService.update(this.EntityPM).subscribe((response:any) => {
                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    if (response.HasError) {
                        this.ValidationErrorsList = response.ErrorsArray;
                    }
                    else {
                        this.CurrentSession.CloseCurrentWindow();
                    }
                });
            }
            else {

                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                this.ValidationErrorsList.push("Dropbox is not connected!");
            }

        });


    }
}
