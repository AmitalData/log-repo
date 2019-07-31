/// <reference path="../../../infrastructure/utilities/featurelocator.ts" />
import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {QuoteTemplatePM} from '../../../Quote/EntityPMs/QuoteTemplatePM';
import {QuoteTemplateSettingPM} from '../../../Quote/EntityPMs/QuoteTemplateSettingPM';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {QuoteTemplateSettingPMService} from '../../../Quote/Services/StandardPMs/QuoteTemplateSettingPMService';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {CitySelectionArgs} from '../../../Common/Args';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {NewEntityArgs} from '../../../Infrastructure/Args';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';

@Component({
    selector: 'QuoteTemplateGeneralSetting',
    moduleId: module.id,
    templateUrl: './QuoteTemplateGeneralSetting.html',
})

export class QuoteTemplateGeneralSetting extends BaseComponent implements OnInit {
    quoteTemplateSettingPMService: QuoteTemplateSettingPMService;
    public DataContext: QuoteTemplateGeneralSetting = this;
    QuoteTemplatePM: QuoteTemplatePM;
    QuoteTemplateSettingPM: QuoteTemplateSettingPM;
    Name: string;
    TemplateTypeCode: string;
    IsDefault: boolean;
    InActive: boolean;
    IsLoadPage: boolean;
    QuoteTemplatePDFMarginLeft: number;
    QuoteTemplatePDFMarginRight: number;
    IsShowIsCopiedAtSignup: boolean = false;
    IsShowEnableForCustomer: boolean = false;

    IsCopiedAtSignup: boolean;
    IsEnabledForCustomers: boolean;


    public ValidationErrorsList: string[];

    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.quoteTemplateSettingPMService = new QuoteTemplateSettingPMService();
    }

    ngOnInit() {

    }


    SetWindowArgs(args: any) {
        this.QuoteTemplatePM = args.QuoteTemplatePM;
        this.QuoteTemplateSettingPM = args.QuoteTemplateSettingPM;
        if (this.QuoteTemplatePM) {
            this.Name = this.QuoteTemplatePM.Name;
            this.TemplateTypeCode = this.QuoteTemplatePM.TemplateTypeCode;
            this.IsDefault = this.QuoteTemplatePM.IsDefault;
            this.InActive = this.QuoteTemplatePM.InActive;
            this.IsCopiedAtSignup = this.QuoteTemplatePM.IsCopiedAtSignup;
            this.IsEnabledForCustomers = this.QuoteTemplatePM.IsEnabledForCustomers;
        }

        if (this.QuoteTemplateSettingPM) {
            this.QuoteTemplatePDFMarginRight = this.QuoteTemplateSettingPM.QuoteTemplatePDFMarginRight;
            this.QuoteTemplatePDFMarginLeft = this.QuoteTemplateSettingPM.QuoteTemplatePDFMarginLeft;
        }


        if (FeatureLocator.HasFeaturePermession("QuoteTemplate", "COPYATSIGNUP")) {
            this.IsShowIsCopiedAtSignup = true;
        }


        if (FeatureLocator.HasFeaturePermession("QuoteTemplate", "ENABLEDFORCUSTOMERS")) {
            this.IsShowEnableForCustomer = true;
        }
        


        this.IsLoadPage = true;
    }


    SaveButtonClicked() {

        this.ValidationErrorsList = [];

        if (AppTool.IsNullOrEmpty(this.Name)) {
            this.ValidationErrorsList.push("Name field is required");
        }

        if (AppTool.IsNullOrEmpty(this.TemplateTypeCode)) {
            this.ValidationErrorsList.push("Please Select QuoteTemplate");
        }

        if (this.QuoteTemplateSettingPM.QuoteTemplatePDFMarginRight > 200) {
            this.ValidationErrorsList.push("Right margin must be less than 200");
        }


        if (this.QuoteTemplateSettingPM.QuoteTemplatePDFMarginLeft > 200) {
            this.ValidationErrorsList.push("Left margin must be less than 200");
        }
        
        if (this.ValidationErrorsList.length == 0) {

            this.QuoteTemplatePM.Name = this.Name;
            this.QuoteTemplatePM.TemplateTypeCode = this.TemplateTypeCode;
            this.QuoteTemplatePM.IsDefault = this.IsDefault;
            this.QuoteTemplatePM.InActive = this.InActive;
            this.QuoteTemplatePM.IsCopiedAtSignup = this.IsCopiedAtSignup;
            this.QuoteTemplatePM.IsEnabledForCustomers = this.IsEnabledForCustomers;



            this.QuoteTemplateSettingPM.QuoteTemplatePDFMarginRight = this.QuoteTemplatePDFMarginRight;
            this.QuoteTemplateSettingPM.QuoteTemplatePDFMarginLeft = this.QuoteTemplatePDFMarginLeft;

            if (this.QuoteTemplateSettingPM.IsDirty) {
                this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));
                this.quoteTemplateSettingPMService.update(this.QuoteTemplateSettingPM).subscribe(res => {
                    this.QuoteTemplateSettingPM.IsDirty = false;
                    this.CurrentSession.StopBusyIndicator();
                    
                    this.CurrentSession.CloseCurrentWindow();
                });
            } else this.CurrentSession.CloseCurrentWindow();

           
        }


    }
    

    CancelButtonClicked() {

        
        this.CurrentSession.CloseCurrentWindow();
    }
}
