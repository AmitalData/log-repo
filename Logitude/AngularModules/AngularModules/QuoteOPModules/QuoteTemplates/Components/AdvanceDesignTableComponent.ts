import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {QuoteOPTemplateSettingPM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplateSettingPM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {QuoteOPTemplateSettingPMService} from '../../../QuoteOPM/Services/StandardPMs/QuoteOPTemplateSettingPMService';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {AppTool} from '../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
@Component({
    selector: 'AdvanceDesignTableComponent',
    
    templateUrl: './AdvanceDesignTableComponent.html',
})

export class AdvanceDesignTableComponent extends BaseComponent implements OnInit {
    QuoteOPTemplateSettingPMService: QuoteOPTemplateSettingPMService; 
    QuoteOPTemplateSettingPM: QuoteOPTemplateSettingPM;  
    public ValidationErrorsList: string[];    
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.QuoteOPTemplateSettingPMService = new QuoteOPTemplateSettingPMService();     
    }

    ngOnInit() {

    }

    QuoteOPTemplateSectionTypeName :string;
    SetWindowArgs(args: any) {
        this.QuoteOPTemplateSettingPM = args.QuoteOPTemplateSettingPM;
        this.QuoteOPTemplateSectionTypeName = args.QuoteOPTemplateSectionTypeName;
        this.BorderColor = this.GetColorFromOrginal(this.QuoteOPTemplateSectionTypeName == "Header" ? this.QuoteOPTemplateSettingPM.PageHeaderBorderColor : this.QuoteOPTemplateSettingPM.PageFooterBorderColor);
        this.BorderThickness = this.QuoteOPTemplateSectionTypeName == "Header" ? this.QuoteOPTemplateSettingPM.PageHeaderBorderThickness : this.QuoteOPTemplateSettingPM.PageFooterBorderThickness;
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
        if (this.QuoteOPTemplateSectionTypeName == "Header") {
            this.QuoteOPTemplateSettingPM.PageHeaderBorderColor = this.GetOrginalFromColor(this.BorderColor);
            this.QuoteOPTemplateSettingPM.PageHeaderBorderThickness = this.BorderThickness;
        }

        else {
            this.QuoteOPTemplateSettingPM.PageFooterBorderColor = this.GetOrginalFromColor(this.BorderColor);
            this.QuoteOPTemplateSettingPM.PageFooterBorderThickness = this.BorderThickness;
        }

        if (this.QuoteOPTemplateSettingPM.IsDirty) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteOPTemplate.M.Saving"));
            this.QuoteOPTemplateSettingPMService.update(this.QuoteOPTemplateSettingPM).subscribe((res:any) => {
                this.CurrentSession.StopBusyIndicator();
                this.QuoteOPTemplateSettingPM.IsDirty = false;
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


