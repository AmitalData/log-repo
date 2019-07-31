import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {INTTRADomainService, INTTRASettingsHelper, INTTRASettingsHelperItem} from '../../Services/INTTRADomainService';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {BranchPM} from '../../../../Common/EntityPMs/BranchPM';
import {FTPDetailPM} from '../../../../Common/EntityPMs/FTPDetailPM';
import {INTTRASettingPM} from '../../../../Common/EntityPMs/INTTRASettingPM';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    templateUrl: './INTTRASettingsComponent.html',
})

export class INTTRASettingsComponent extends BaseComponent {
    public Helper: INTTRASettingsHelper;
    public EntityPM: INTTRASettingPM = null;
    public ObjectTableName = "INTTRASetting";
    public DataContext = this;
    public Branches: BranchPM[] = [];
    public RegistrationList: RegistrationItem[] = [];
    public IsResourcesReady: boolean = false;
    public ValidationErrorsList: string[] = [];
    private myService: INTTRADomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        super();

        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res1: any) => {
            this.entityResourceService.getEntityResourceByTableName("Branch").subscribe((res2: any) => {
                this.LoadData();
            });
        });
    }

    LoadData() {
        this.myService = new INTTRADomainService();
        this.myService.GetINTTRASettings().subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }

            else {
                this.Helper = myResponse.Result;
                this.EntityPM = this.Helper.INTTRASetting;

                this.Branches = [];
                this.Helper.Branches.sort((a, b) => { return (a.EnglishName === b.EnglishName) ? 0 : (a.EnglishName < b.EnglishName) ? -1 : 1 }).forEach(item => {
                    this.Branches.push(item);
                });

                this.BuildRegistrationList()
                this.SetUIProperties();
                this.IsResourcesReady = true;
            }
        });
    }

    BuildRegistrationList() {
        this.RegistrationList = [];

        this.Helper.Items.filter(f => f.IsLineItem == true).forEach(line => {
            this.RegistrationList.push(new RegistrationItem(line, this));
        });
    }
    SetUIProperties() {
        this.UIProperties.SetEnabled("OutSettingsHost", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("InSettingsHost", this.ObjectTableName, false);
    }

    get INTTRASettingModeCode() { return this.EntityPM.INTTRASettingModeCode; }
    set INTTRASettingModeCode(value: string) {
        if (this.EntityPM.INTTRASettingModeCode != value) {
            this.EntityPM.INTTRASettingModeCode = value;
        }
    }

    get OutSettingsId() { return this.EntityPM.OutSettingsId; }
    set OutSettingsId(value: string) {
        if (this.EntityPM.OutSettingsId != value) {
            this.EntityPM.OutSettingsId = value;
        }
    }
    get OutSettingsHost() { return this.EntityPM.OutSettingsHost; }
    set OutSettingsHost(value: string) {
        if (this.EntityPM.OutSettingsHost != value) {
            this.EntityPM.OutSettingsHost = value;
        }
    }

    get InSettingsId() { return this.EntityPM.InSettingsId; }
    set InSettingsId(value: string) {
        if (this.EntityPM.InSettingsId != value) {
            this.EntityPM.InSettingsId = value;
        }
    }
    get InSettingsHost() { return this.EntityPM.InSettingsHost; }
    set InSettingsHost(value: string) {
        if (this.EntityPM.InSettingsHost != value) {
            this.EntityPM.InSettingsHost = value;
        }
    }

    get INTTRAId() { return this.EntityPM.INTTRAId; }
    set INTTRAId(value: string) {
        if (this.EntityPM.INTTRAId != value) {
            this.EntityPM.INTTRAId = value;
        }
    }

    get INTTRAAlias() { return this.EntityPM.INTTRAAlias; }
    set INTTRAAlias(value: string) {
        if (this.EntityPM.INTTRAAlias != value) {
            this.EntityPM.INTTRAAlias = value;
        }
    }

    AddFTPClicked(Code: string) {

        var isTest = this.INTTRASettingModeCode == "TEST" ? true : false;

        var logWindow = new LogitudeWindow();
        logWindow.Title = "Add FTP Detail";
        logWindow.WindowArgs = { Code: Code, IsNew: true, IsINTTRA: true, IsTestMode: isTest };
        logWindow.Show('./Common/Components/Maintenance/CustomsInterface/FTPDetailComponent');

        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    if (Code == "Out") {
                        this.OutSettingsId = comp.EntityPM.Id;
                        this.OutSettingsHost = comp.EntityPM.Host;

                    }

                    else {
                        this.InSettingsId = comp.EntityPM.Id;
                        this.InSettingsHost = comp.EntityPM.Host;
                    }
                }
            });
        });
    }
    EditFTPClicked(Code: string) {
        var settingId = null;

        if (Code == "Out") {
            settingId = this.OutSettingsId;
        }

        else {
            settingId = this.InSettingsId;
        }

        if (!AppTool.IsNullOrEmpty(settingId)) {

            var isTest = this.INTTRASettingModeCode == "TEST" ? true : false;

            var logWindow = new LogitudeWindow();
            logWindow.Title = "Edit FTP Detail";
            logWindow.WindowArgs = { Code: Code, IsNew: false, IsINTTRA: true, IsTestMode: isTest, EntityId: settingId };
            logWindow.Show('./Common/Components/Maintenance/CustomsInterface/FTPDetailComponent');

            logWindow.ComponentLoaded.subscribe(comp => {
                logWindow.WindowClosed.subscribe(s => {
                    if (s) {
                        if (Code == "Out") {
                            this.OutSettingsHost = comp.EntityPM.Host;
                        }

                        else {
                            this.InSettingsHost = comp.EntityPM.Host;
                        }
                    }
                });
            });
        } 
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        this.Branches.forEach(branch => {
            Validator.TryValidateObject(branch, "Branch", errors);
        });

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {

            this.CurrentSession.StartBusyIndicatorSaving();

            this.myService.UpdateINTTRASettings(this.Helper).subscribe((myResponse: ServiceResponse) => {

                this.CurrentSession.StopBusyIndicator();

                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }

                else {
                    this.CurrentSession.CloseCurrentWindow();
                }                
            });
        }
    }
}
class RegistrationItem {
    public CompinedId: string;
    public Code: string;
    public Name: string;
    public Notes: string;
    public ShippingLineId: string;
    public Carriers: RegistrationItemCarrier[] = [];
    constructor(line: INTTRASettingsHelperItem, private father: INTTRASettingsComponent) {
        this.CompinedId = line.CompinedId;
        this.Code = line.Code;
        this.Name = line.Name;
        this.Notes = line.Notes;
        this.ShippingLineId = line.ShippingLineId;

        father.Branches.forEach(branch => {
            var itemCarrier: INTTRASettingsHelperItem = this.father.Helper.Items.filter(f => f.ShippingLineId == this.ShippingLineId && f.BranchId == branch.Id && f.IsLineItem == false)[0];
            if (!itemCarrier) {

            }

            this.Carriers.push(new RegistrationItemCarrier(itemCarrier));
        });
    }
}
class RegistrationItemCarrier {
    constructor(private itemCarrier: INTTRASettingsHelperItem) {
    }

    get IsRegistered() { return this.itemCarrier.IsRegistered; }
    set IsRegistered(value: boolean) {
        if (this.itemCarrier.IsRegistered != value) {
            this.itemCarrier.IsRegistered = value;
        }
    }
}
