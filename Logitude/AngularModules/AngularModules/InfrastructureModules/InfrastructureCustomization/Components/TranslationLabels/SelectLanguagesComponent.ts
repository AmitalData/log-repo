import {Component} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {CommonDomainService, TranslationHeader} from '../../../../Common/Services/CommonDomainService';

@Component({
    moduleId: module.id,
    templateUrl: './SelectLanguagesComponent.html',
})

export class SelectLanguagesComponent {

    public LanguagesList: TranslationHeader[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.LoadLanguages();
    }

    LoadLanguages() {
        var myService: CommonDomainService = new CommonDomainService();
        myService.GetTranslationHeadersByTenant(SessionLocator.Tenant).subscribe((myResult: any) => {
            this.LanguagesList = myResult;
            this.LanguageSelectedItem = this.LanguagesList.filter(d => d.Code == SessionLocator.TenantPM.Language)[0];
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

    // Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        this.RunTranslationWindow();
    }
    RunTranslationWindow() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.IsFillScreen = true;
        logitudeWindow.Title = "Translate Labels";
        logitudeWindow.WindowArgs = this.selectedLanguageCode;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureCustomization/Components/TranslationLabels/TranslateLabelsComponent');
        this.CurrentSession.CloseCurrentWindow();
    }
}
