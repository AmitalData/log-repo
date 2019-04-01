import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {DateTimeZone, TimeZoneInfoClass, DateTimeFormat} from '../../../../Infrastructure/Utilities/DateTimeZone';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {CommonDomainService, TranslationHeader} from '../../../../Common/Services/CommonDomainService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {TenantPMService} from '../../../../Common/Services/StandardPMs/TenantPMService';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';

@Component({
    selector: 'LocalSettingsComponent',
    moduleId: module.id,
    templateUrl: './LocalSettingsComponent.html',   
})

export class LocalSettingsComponent extends BaseComponent implements OnInit {
    public DataContext: LocalSettingsComponent = this;
    public ObjectTableName: string = "Tenant";
    public TenantPm: TenantPM = new TenantPM();
    public demoMessageVisibility: boolean = false;
    public IsVisible = false;
    private oldLanguageCode: string = null;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.TimeZonesList = DateTimeZone.GetTimeZonesList();
        this.DateTimeFormatsList = DateTimeZone.GetDateTimeFormats();
    }

    ngOnInit() {
        this.LoadTenantPMMethod();
    }

    //Tenant 65
    public get DemoMessageVisibility() {
        var result = false;
        if (this.TenantPm.Id == 65) {
            result = true;

            if (SessionLocator.LoggedUserPM.Email.toLowerCase() == "customercare@logitudeworld.com‏") {
                result = false;
            }
        }

        return result;
    }

    SetUIPropertiesHitVisible() {
        this.UIProperties.SetEnabled("ShowDayLightSettings", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("DayLightStartDate", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("DayLightEndDate", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("DayLightOffset", this.ObjectTableName, false);
    }

    // Load Tenant 
    private LoadTenantPMMethod() {
        var myService: TenantPMService = new TenantPMService();
        myService.get(SessionLocator.TenantPM.Id).subscribe((response: ServiceResponse) => {
            this.TenantPm = response.Result;
            this.oldLanguageCode = this.TenantPm.Language;

            if (this.TenantPm.DayLightOffset != 0) {
                this.ShowDayLightSettings = true;
            }

            if (this.TenantPm.Id == 65 && SessionLocator.LoggedUserPM.Email.toLowerCase() != "customercare@logitudeworld.com‏") {
                this.SetUIPropertiesHitVisible();
            }

            this.TimeZoneSelectedItem = this.TimeZonesList.filter(a => a.BaseUtcOffset == this.TenantPm.TimeZoneOffset)[0];
            this.LoadLanguagesListMethod();
            this.IsVisible = true;
        });
    }

    //Languages List 
    public LanguagesList: TranslationHeader[] = [];
    private LoadLanguagesListMethod() {
        var myService: CommonDomainService = new CommonDomainService();
        myService.GetTranslationHeadersByTenant(this.TenantPm.Id).subscribe((myResult:any) => {
            this.LanguagesList = myResult;
            this.LanguageSelectedItem = this.LanguagesList.filter(d => d.Code == this.TenantPm.Language)[0];
        });
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

    LanguageSelectedChange(item) {
        this.LanguageSelectedItem = this.LanguagesList.filter(d => d.Code == item.Code)[0];
        this.selectedLanguageCode = item.Code;
    }

    //TimeZonesList 
    public TimeZonesList: TimeZoneInfoClass[] = [];
    private timeZoneSelectedItem: TimeZoneInfoClass;
    get TimeZoneSelectedItem() { return this.timeZoneSelectedItem; }
    set TimeZoneSelectedItem(value: TimeZoneInfoClass) {
        if (this.timeZoneSelectedItem != value) {
            this.timeZoneSelectedItem = value;

            if (value == null) {
                this.TimeZoneOffset = null;
            }

            else {
                this.TimeZoneOffset = value.BaseUtcOffset;
            }
        }
    }

    //DateTimeFormatsList
    public DateTimeFormatsList: DateTimeFormat[] = [];

    get DateTimeFormatSelectedItem()
    {
        return this.DateTimeFormatsList.filter(d => d.Format == this.TenantPm.DateTimeFormat)[0];
    }
    set DateTimeFormatSelectedItem(value: any) {
        this.TenantPm.DateTimeFormat = value.Format;
    }

    DateTimeFormatSelectedChange(item) {
        this.TenantPm.DateTimeFormat = item.Format;
    }

    //Props
    get TimeZoneOffset()
    {
        return this.TenantPm.TimeZoneOffset;
    }
    set TimeZoneOffset(value: number) {
        if (this.TenantPm.TimeZoneOffset != value) {
            this.TenantPm.TimeZoneOffset = value;
        }
    }

    get DayLightStartDate()
    {
        return this.TenantPm.DayLightStartDate;
    }
    set DayLightStartDate(value: Date) {
        if (this.TenantPm.DayLightStartDate != value) {
            this.TenantPm.DayLightStartDate = value;
        }
    }


    get NumberFormatCode() {
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
        if (this.TenantPm.Id == 65) {
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

            if (SessionLocator.TenantPM.Language != this.selectedLanguageCode) {
                this.TenantPm.Language = this.selectedLanguageCode;
                this.reloadingTranslation = true;

                var messageWindow: MessageWindow = new MessageWindow();
                messageWindow.Show("In Order to apply the language change, please logout and login again");
                messageWindow.WindowClosed.subscribe(($event: any) => {
                    this.SubmitChanges();
                });
            }

            else {
                this.SubmitChanges();
            }
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
                    ////Complete work 
                    //var datetimeformat: string = "dd\\/MM\\/yyyy";
                    //if (!LogitudeUtilitie3s.IsNullOrEmpty(this.TenantPm.DateTimeFormat)) {
                    //    datetimeformat = this.TenantPm.DateTimeFormat;
                    //}

                    //if (this.reloadingTranslation) {
                    //    this.ReloadTranslationMethod();
                    //}
                }

                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    }

    ReloadTranslationMethod() {


    }
}
