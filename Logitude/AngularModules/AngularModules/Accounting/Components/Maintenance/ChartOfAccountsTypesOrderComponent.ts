import {Component, OnInit} from '@angular/core';
import { TenantPM } from 'Common/EntityPMs/TenantPM';
import { TranslationHeader, CommonDomainService } from 'Common/Services/CommonDomainService';
import { TenantPMService } from 'Common/Services/StandardPMs/TenantPMService';
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { DateTimeZone, TimeZoneInfoClass, DateTimeFormat } from 'Infrastructure/Utilities/DateTimeZone';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { Validator } from '../../../Infrastructure/Validators/Validator';


@Component({
    selector: 'ChartOfAccountsTypesOrderComponent',
    
    templateUrl: './ChartOfAccountsTypesOrderComponent.html',   
})

export class ChartOfAccountsTypesOrderComponent extends BaseComponent implements OnInit {
    public DataContext: ChartOfAccountsTypesOrderComponent = this;
    public ObjectTableName: string = "ChartOfAccountsType";
    public TenantPm: TenantPM = new TenantPM();
    public demoMessageVisibility: boolean = false;
    public IsVisible = false;
    public ChartOfAccountsTypesOrders = [1,2,3,4,5,6,7];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    ngOnInit() {
        this.LoadTenantPMMethod();
    }

    SetUIPropertiesHitVisible() {
        this.UIProperties.SetEnabled("ShowDayLightSettings", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("DayLightStartDate", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("DayLightEndDate", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("DayLightOffset", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("NumberFormatCode", this.ObjectTableName, false);
    }

    private selectedLanguageCode: string;

    private languageSelectedItem: TranslationHeader;
    get LanguageSelectedItem() { return this.languageSelectedItem; }
    set LanguageSelectedItem(value: TranslationHeader) {
        if (this.languageSelectedItem != value) {
            this.languageSelectedItem = value;

            if (value == null) {
                this.selectedLanguageCode = null;
            }

            else {
                this.selectedLanguageCode = value.Code;
            }
        }
    }
    get NumberFormatCode() {
        if (this.TenantPm.NumberFormatCode == null || this.TenantPm.NumberFormatCode == "")
            return "CD";
        else
        return this.TenantPm.NumberFormatCode;
    }
    set NumberFormatCode(value: string) {
        if (this.TenantPm.NumberFormatCode != value) {
            this.TenantPm.NumberFormatCode = value;
        }
    }


    get DayLightEndDate()
    {
        return this.TenantPm.DayLightEndDate;
    }
    set DayLightEndDate(value: Date) {
        if (this.TenantPm.DayLightEndDate != value) {
            this.TenantPm.DayLightEndDate = value;
        }
    }

    get DayLightOffset()
    {
        return this.TenantPm.DayLightOffset;
    }
    set DayLightOffset(value: number) {
        if (this.TenantPm.DayLightOffset != value) {
            this.TenantPm.DayLightOffset = value;
        }
    }

    private showDayLightSettings: boolean;
    get ShowDayLightSettings() { return this.showDayLightSettings; }
    set ShowDayLightSettings(value: boolean) {
        if (this.showDayLightSettings != value) {
            this.showDayLightSettings = value;
            if (!value) {
                this.DayLightOffset = 0;
                this.DayLightStartDate = null;
                this.DayLightEndDate = null;
            }

            else {
                this.DayLightOffset = 1;
            }
        }
    }

    get IsHitTestVisible() {
        var result = false;
        if (ObjectsLocator.IsDemoTenant(this.TenantPm.Id.toString())) {
            result = true;

            if (SessionLocator.LoggedUserPM.Email.toLowerCase() == "customercare@logitudeworld.com‏") {
                result = false;
            }
        }

        return result;
    }

    // Commands 
    CancelButtonClicked() {
        this.TenantPm = null;
        this.CurrentSession.CloseCurrentWindow();
    }
 
    public ValidationErrorsList: string[];
    public reloadingTranslation: boolean;

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.DataContext.TenantPm, this.DataContext.ObjectTableName, errors);

        if (this.TenantPm.DayLightOffset != 0) {
            if (this.TenantPm.DayLightEndDate == null || this.TenantPm.DayLightStartDate == null) {
                errors.push("DayLightStartDate and DayLightEndDate should have values");
            }
        }

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            this.SubmitChanges();
        }
    }

    SubmitChanges() {
        this.CurrentSession.StartBusyIndicator("Saving...");

        var myService: TenantPMService = new TenantPMService();
        myService.update(this.TenantPm).subscribe((myResponse: ServiceResponse) => {
            if (myResponse) {
                if (!myResponse.HasError) {
                    InfraSettings.TenantPM = this.TenantPm;
                    this.CurrentSession.CloseCurrentWindowEmit("ok");
                }

                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    }
}
