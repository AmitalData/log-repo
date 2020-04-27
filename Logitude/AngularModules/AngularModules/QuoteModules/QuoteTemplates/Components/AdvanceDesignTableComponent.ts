import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {QuoteTemplateSettingPM} from '../../../Quote/EntityPMs/QuoteTemplateSettingPM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {QuoteTemplateSettingPMService} from '../../../Quote/Services/StandardPMs/QuoteTemplateSettingPMService';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {AppTool} from '../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
@Component({
    selector: 'AdvanceDesignTableComponent',
    
    templateUrl: './AdvanceDesignTableComponent.html',
})

export class AdvanceDesignTableComponent extends BaseComponent implements OnInit {
    quoteTemplateSettingPMService: QuoteTemplateSettingPMService; 
    QuoteTemplateSettingPM: QuoteTemplateSettingPM;  
    public ValidationErrorsList: string[];    
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.quoteTemplateSettingPMService = new QuoteTemplateSettingPMService();     
    }

    ngOnInit() {

    }

    QuoteTemplateSectionTypeName :string;
    SetWindowArgs(args: any) {
        this.QuoteTemplateSettingPM = args.QuoteTemplateSettingPM;
        this.QuoteTemplateSectionTypeName = args.QuoteTemplateSectionTypeName;
        this.BorderColor = this.GetColorFromOrginal(this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderBorderColor : this.QuoteTemplateSettingPM.PageFooterBorderColor);
        this.BorderThickness = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderBorderThickness : this.QuoteTemplateSettingPM.PageFooterBorderThickness;
    }
    
    GetColorFromOrginal(color: string) {
        var result = "";

        if (!AppTool.IsNullOrEmpty(color)) {
            result = color;
            if (color && color.length > 7) {
                var colors: string[] = color.split('#');
                if (colors.length > 1) {
                    result = "#" + colors[1].substring(2, 8);
                }
            }
        }

        return result;
    }

    GetOrginalFromColor(color: string) {
        var result = "";

        if (!AppTool.IsNullOrEmpty(color)) {
            result = color;
            var colors: string[] = color.split('#');
            if (colors.length > 1) {
                result = ("#" + "FF" + colors[1]);
            }
        }

        return result;
    }

    private borderColor = "";
    get BorderColor() {
        return this.borderColor;
    }
    set BorderColor(newValue: string) {
        this.borderColor = newValue;
    }

    private borderThickness = 0;
    get BorderThickness() {
        return this.borderThickness;
    }
    set BorderThickness(newValue: number) {
        this.borderThickness = newValue;
    }

    SaveButtonClicked() {
        if (this.QuoteTemplateSectionTypeName == "Header") {
            this.QuoteTemplateSettingPM.PageHeaderBorderColor = this.GetOrginalFromColor(this.BorderColor);
            this.QuoteTemplateSettingPM.PageHeaderBorderThickness = this.BorderThickness;
        }

        else {
            this.QuoteTemplateSettingPM.PageFooterBorderColor = this.GetOrginalFromColor(this.BorderColor);
            this.QuoteTemplateSettingPM.PageFooterBorderThickness = this.BorderThickness;
        }

        if (this.QuoteTemplateSettingPM.IsDirty) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));
            this.quoteTemplateSettingPMService.update(this.QuoteTemplateSettingPM).subscribe((res:any) => {
                this.CurrentSession.StopBusyIndicator();
                this.QuoteTemplateSettingPM.IsDirty = false;
                this.CurrentSession.CurrentWindow.Close("Refresh");
            });
        }

        else {
            this.CurrentSession.CloseCurrentWindow();
        }      
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}


