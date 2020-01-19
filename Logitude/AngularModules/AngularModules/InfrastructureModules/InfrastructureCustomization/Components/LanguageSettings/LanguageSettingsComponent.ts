import {Component} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {AppTool} from '../../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {CommonDomainService, TranslationHeader} from '../../../../Common/Services/CommonDomainService';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {TenantPMService} from '../../../../Common/Services/StandardPMs/TenantPMService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,
    templateUrl: './LanguageSettingsComponent.html',
})

export class LanguageSettingsComponent {
    public LanguagesCollection: TranslationHeader[] = [];
    public FormatsCollection: string[] = [];
    public ValidationErrorsList: string[] = [];
    public TenantPM: TenantPM;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.LoadTenantPMMethod();
    }

    private LoadTenantPMMethod() {
        var myService: TenantPMService = new TenantPMService();
        myService.get(SessionLocator.TenantPM.Id).subscribe((response: ServiceResponse) => {
            this.TenantPM = response.Result;
            this.LoadLanguages();
            this.LoadFormat();
        });
    }
    LoadFormat() {
        this.FormatsCollection.push("en-US");
        this.FormatsCollection.push("ar-SA");
        this.FormatsCollection.push("he-IL");
        this.FormatsCollection.push("de");
    }
    LoadLanguages() {
        var myService: CommonDomainService = new CommonDomainService();
        myService.GetTranslationHeadersByTenant(SessionLocator.Tenant).subscribe((myResult: any) => {
            this.LanguagesCollection = myResult;
            this.LanguageSelectedItem = this.LanguagesCollection.filter(d => d.Code == SessionLocator.TenantPM.Language)[0];
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
                this.TenantPM.Language = value.Code;
            }
        }
    }
    LanguageSelectedChange(item) {
        this.LanguageSelectedItem = this.LanguagesCollection.filter(d => d.Code == item.Code)[0];
        this.selectedLanguageCode = item.Code;
    }

    private selectedFormat: string;
    get SelectedFormat() { return this.selectedFormat; }
    set SelectedFormat(value: string) {
        if (this.selectedFormat != value) {
            this.selectedFormat = value;
        }
    }
    FormatsSelectedChange(item) {
        this.SelectedFormat = item;
    }

    // Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var errors = [];
        if (this.LanguageSelectedItem == null) {
            errors.push("Language is Required");
        }

        //if (this.SelectedFormat == null) {
        //    errors.push("Format is Required");
        //}
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicator("Saving...");
            var myService: TenantPMService = new TenantPMService();
            myService.update(this.TenantPM).subscribe((myResponse: ServiceResponse) => {
                if (myResponse) {
                    if (!myResponse.HasError) {
                        InfraSettings.TenantPM = this.TenantPM;
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
}
