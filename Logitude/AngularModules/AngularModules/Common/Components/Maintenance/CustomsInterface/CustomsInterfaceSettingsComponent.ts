import {Component, OnInit} from '@angular/core';
import {CustomsInterfaceSettingPM} from '../../../EntityPMs/CustomsInterfaceSettingPM';
import {FTPDetailPM} from '../../../EntityPMs/FTPDetailPM';
import {CustomsInterfaceSettingPMService} from '../../../Services/StandardPMs/CustomsInterfaceSettingPMService';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    selector: 'CustomsInterfaceSettingsComponent',
    
    templateUrl: './CustomsInterfaceSettingsComponent.html',
})

export class CustomsInterfaceSettingsComponent extends BaseComponent implements OnInit {
    public EntityPM: CustomsInterfaceSettingPM;
    public ObjectTableName: string = "CustomsInterfaceSetting";
    public DataContext = this;
    public ValidationErrorsList: string[] = [];
    public IsResourcesReady: boolean = false;
    private myService: CustomsInterfaceSettingPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityResourceService: EntityResourceService) {
        super();

        this.myService = new CustomsInterfaceSettingPMService();
    }
    
    ngOnInit() {
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {
            this.myService.get(SessionLocator.Tenant).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {

                    if (myResponse.Result) {
                        this.EntityPM = myResponse.Result;
                    }

                    else {
                        this.EntityPM = new CustomsInterfaceSettingPM();                        
                        this.EntityPM.IsDirty = false;  
                    }

                    this.SetUIProperties();
                    this.SetTickProperties();
                    this.IsResourcesReady = true;
                }
            });
        });
    }
    
    public IsLocalDetailsEnabled: boolean = false;
    public IsImportDetailsEnabled: boolean = false;
    public IsExportDetailsEnabled: boolean = false;
    SetUIProperties() {
        var localDetailsEnabled = false;
        var importDetailsEnabled = false;
        var exportDetailsEnabled = false;

        if (!AppTool.IsNullOrEmpty(this.EntityPM.LocalCustomsInterfaceCode)) {
            if (this.EntityPM.LocalCustomsInterfaceCode != "NO") {
                localDetailsEnabled = true;
            }
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.ImportToUSAInterfaceCode)) {
            if (this.EntityPM.ImportToUSAInterfaceCode != "NO") {
                importDetailsEnabled = true;
            }
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.ExportFromUSAInterfaceCode)) {
            if (this.EntityPM.ExportFromUSAInterfaceCode != "NO") {
                exportDetailsEnabled = true;
            }
        }

        this.IsLocalDetailsEnabled = localDetailsEnabled;
        this.IsImportDetailsEnabled = importDetailsEnabled;
        this.IsExportDetailsEnabled = exportDetailsEnabled;
    }

    public IsLocalTickVisible: boolean = false;
    public IsImportTickVisible: boolean = false;
    public IsExportTickVisible: boolean = false;
    SetTickProperties() {
        var localTickVisible = false;
        var importTickVisible = false;
        var exportTickVisible = false;

        if (!AppTool.IsNullOrEmpty(this.EntityPM.LocalCustomsInterfaceCode)) {
            if (this.EntityPM.LocalCustomsInterfaceCode != "NO") {
                if (this.EntityPM.LocalCustomsInterfaceCode == "AMC") {
                    if (this.EntityPM.AMCAirStartDate != null && this.EntityPM.AMCOceanStartDate != null) {
                        localTickVisible = true;
                    }
                }

                else {
                    if (!AppTool.IsNullOrEmpty(this.EntityPM.LocalCompanyId) && !AppTool.IsNullOrEmpty(this.EntityPM.LocalUserId) && !AppTool.IsNullOrEmpty(this.EntityPM.LocalPassword)) {
                        localTickVisible = true;
                    }
                }
            }
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.ImportToUSAInterfaceCode)) {
            if (this.EntityPM.ImportToUSAInterfaceCode != "NO") {
                if (!AppTool.IsNullOrEmpty(this.EntityPM.ArtemusOutSettingsId) && !AppTool.IsNullOrEmpty(this.EntityPM.ArtemusInSettingsId)) {
                    importTickVisible = true;
                }
            }
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.ExportFromUSAInterfaceCode)) {
            if (this.EntityPM.ExportFromUSAInterfaceCode != "NO") {
                
            }
        }

        this.IsLocalTickVisible = localTickVisible;
        this.IsImportTickVisible = importTickVisible;
        this.IsExportTickVisible = exportTickVisible;
    }

    get LocalCustomsInterfaceCode() { return this.EntityPM.LocalCustomsInterfaceCode; }
    set LocalCustomsInterfaceCode(value: string) {
        if (this.EntityPM.LocalCustomsInterfaceCode != value) {
            this.EntityPM.LocalCustomsInterfaceCode = value;

            if (value == "NO") {
                this.EntityPM.LocalCompanyId = null;
                this.EntityPM.LocalUserId = null;
                this.EntityPM.LocalPassword = null;
                this.ActivateCustomsManagementInShipments = false;
            }

            else {
                this.ActivateCustomsManagementInShipments = true;
            }

            this.SetUIProperties();
            this.SetTickProperties();
        }
    }

    get ImportToUSAInterfaceCode() { return this.EntityPM.ImportToUSAInterfaceCode; }
    set ImportToUSAInterfaceCode(value: string) {
        if (this.EntityPM.ImportToUSAInterfaceCode != value) {
            this.EntityPM.ImportToUSAInterfaceCode = value;

            this.SetUIProperties();
        }
    }

    get ExportFromUSAInterfaceCode() { return this.EntityPM.ExportFromUSAInterfaceCode; }
    set ExportFromUSAInterfaceCode(value: string) {
        if (this.EntityPM.ExportFromUSAInterfaceCode != value) {
            this.EntityPM.ExportFromUSAInterfaceCode = value;

            this.SetUIProperties();
        }
    }
    
    get ActivateCustomsManagementInShipments() { return this.EntityPM.ActivateCustomsManagementInShipments; }
    set ActivateCustomsManagementInShipments(value: boolean) {
        if (this.EntityPM.ActivateCustomsManagementInShipments != value) {
            this.EntityPM.ActivateCustomsManagementInShipments = value;
        }
    }

    DetailsClicked(type: string) {
        var logitudeWindow = new LogitudeWindow();

        switch (type) {
            case "Local": {
                var windowTitle: string;

                if (this.LocalCustomsInterfaceCode == "AMC") {
                    windowTitle = "AMANAC Start Dates";                    
                }

                else {
                    windowTitle = "Local Interface Credintials";
                }

                logitudeWindow.Title = windowTitle;
                logitudeWindow.WindowArgs = this.EntityPM;
                logitudeWindow.WindowClosed.subscribe(($event: any) => this.OnWindowClosed($event));
                logitudeWindow.Show('./Common/Components/Maintenance/CustomsInterface/LocalCustomsInterfaceDetailsComponent');

                break;
            }

            case "Import": {
                this.entityResourceService.getEntityResourceByTableName("FTPDetail", 0).subscribe((response: any) => {
                    var logitudeWindow = new LogitudeWindow();
                    logitudeWindow.Title = "Artemus Settings";
                    logitudeWindow.WindowArgs = this.EntityPM;
                    logitudeWindow.WindowClosed.subscribe(($event: any) => this.OnWindowClosed($event));
                    logitudeWindow.Show('./Common/Components/Maintenance/CustomsInterface/ArtemusSettingsComponent');                    
                });
                
                break;
            }
        }
    }
    OnWindowClosed(message: string) {
        if (message == "ok") {
            this.SetTickProperties();
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.ValidationErrorsList = [];

        if (this.ValidationErrorsList.length == 0) {
            if (this.EntityPM.IsDirty) {
                this.CurrentSession.StartBusyIndicatorSaving();

                if (this.EntityPM.Tenant == null) {
                    this.EntityPM.Tenant = SessionLocator.Tenant;
                    this.myService.insert(this.EntityPM).subscribe((myRespone: ServiceResponse) => {
                        this.CurrentSession.StopBusyIndicator();

                        if (!myRespone.HasError) {
                            ObjectsLocator.CustomsInterfaceSettingPM = this.EntityPM;
                            this.CurrentSession.CloseCurrentWindowEmit("OK");
                        }

                        else {
                            this.ValidationErrorsList = myRespone.ErrorsArray;
                        }
                    });
                }

                else {
                    this.myService.update(this.EntityPM).subscribe((myRespone: ServiceResponse) => {
                        this.CurrentSession.StopBusyIndicator();

                        if (!myRespone.HasError) {
                            ObjectsLocator.CustomsInterfaceSettingPM = this.EntityPM;
                            this.CurrentSession.CloseCurrentWindowEmit("OK");
                        }

                        else {
                            this.ValidationErrorsList = myRespone.ErrorsArray;
                        }
                    });
                }
            }

            else {
                this.CurrentSession.CloseCurrentWindow();
            }
        }        
    }
}
