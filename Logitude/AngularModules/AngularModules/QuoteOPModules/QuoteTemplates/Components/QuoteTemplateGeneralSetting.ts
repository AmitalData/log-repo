import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {QuoteOPTemplatePM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplatePM';
import {QuoteOPTemplateSettingPM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplateSettingPM';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {QuoteOPTemplateSettingPMService} from '../../../QuoteOPM/Services/StandardPMs/QuoteOPTemplateSettingPMService';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {CitySelectionArgs} from '../../../Common/Args';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {NewEntityArgs} from '../../../Infrastructure/Args';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';

@Component({
    selector: 'QuoteTemplateGeneralSetting',
    
    templateUrl: './QuoteTemplateGeneralSetting.html',
})

export class QuoteTemplateGeneralSetting extends BaseComponent implements OnInit {
  public IsNewEntityCall: boolean = false;

    QuoteOPTemplateSettingPMService: QuoteOPTemplateSettingPMService;
    public DataContext: QuoteTemplateGeneralSetting = this;
    QuoteOPTemplatePM: QuoteOPTemplatePM;
    QuoteOPTemplateSettingPM: QuoteOPTemplateSettingPM;
    Name: string;
    TemplateTypeCode: string;
    IsDefault: boolean;
    InActive: boolean;
    IsLoadPage: boolean;
    QuoteOPTemplatePDFMarginLeft: number;
    QuoteOPTemplatePDFMarginRight: number;
    IsShowIsCopiedAtSignup: boolean = false;
    IsShowEnableForCustomer: boolean = false;
    QuoteOPTemplatePDFMarginTop: number;
    QuoteOPTemplatePDFMarginBottom: number;
    IsCopiedAtSignup: boolean;
    IsEnabledForCustomers: boolean;


    public ValidationErrorsList: string[];

    @ViewChild('Child', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.QuoteOPTemplateSettingPMService = new QuoteOPTemplateSettingPMService();
    }

    ngOnInit() {

    }


    SetWindowArgs(args: any) {
        this.QuoteOPTemplatePM = args.QuoteOPTemplatePM;
        this.QuoteOPTemplateSettingPM = args.QuoteOPTemplateSettingPM;
        if (this.QuoteOPTemplatePM) {
            this.Name = this.QuoteOPTemplatePM.Name;
            this.TemplateTypeCode = this.QuoteOPTemplatePM.TemplateTypeCode;
            this.IsDefault = this.QuoteOPTemplatePM.IsDefault;
            this.InActive = this.QuoteOPTemplatePM.InActive;
            this.IsCopiedAtSignup = this.QuoteOPTemplatePM.IsCopiedAtSignup;
            this.IsEnabledForCustomers = this.QuoteOPTemplatePM.IsEnabledForCustomers;
        }

        if (this.QuoteOPTemplateSettingPM) {
            this.QuoteOPTemplatePDFMarginRight = this.QuoteOPTemplateSettingPM.QuoteTemplatePDFMarginRight;
            this.QuoteOPTemplatePDFMarginLeft = this.QuoteOPTemplateSettingPM.QuoteTemplatePDFMarginLeft;
            this.QuoteOPTemplatePDFMarginBottom = this.QuoteOPTemplateSettingPM.QuoteTemplatePDFMarginBottom;
            this.QuoteOPTemplatePDFMarginTop = this.QuoteOPTemplateSettingPM.QuoteTemplatePDFMarginTop;
            

        }


        if (FeatureLocator.HasFeaturePermession("QuoteOPTemplate", "COPYATSIGNUP")) {
            this.IsShowIsCopiedAtSignup = true;
        }


        if (FeatureLocator.HasFeaturePermession("QuoteOPTemplate", "ENABLEDFORCUSTOMERS")) {
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
            this.ValidationErrorsList.push("Please Select QuoteOPTemplate");
        }

        if (this.QuoteOPTemplatePDFMarginRight > 200) {
            this.ValidationErrorsList.push("Right margin must be less than 200");
        }


        if (this.QuoteOPTemplatePDFMarginLeft > 200) {
            this.ValidationErrorsList.push("Left margin must be less than 200");
        }


        if (this.QuoteOPTemplatePDFMarginBottom > 200) {
            this.ValidationErrorsList.push("Bottom margin must be less than 200");
        }


        if (this.QuoteOPTemplatePDFMarginTop > 200) {
            this.ValidationErrorsList.push("Top margin must be less than 200");
        }

       
        if (this.ValidationErrorsList.length == 0) {

            this.QuoteOPTemplatePM.Name = this.Name;
            this.QuoteOPTemplatePM.TemplateTypeCode = this.TemplateTypeCode;
            this.QuoteOPTemplatePM.IsDefault = this.IsDefault;
            this.QuoteOPTemplatePM.InActive = this.InActive;
            this.QuoteOPTemplatePM.IsCopiedAtSignup = this.IsCopiedAtSignup;
            this.QuoteOPTemplatePM.IsEnabledForCustomers = this.IsEnabledForCustomers;



            this.QuoteOPTemplateSettingPM.QuoteTemplatePDFMarginRight = this.QuoteOPTemplatePDFMarginRight;
            this.QuoteOPTemplateSettingPM.QuoteTemplatePDFMarginLeft = this.QuoteOPTemplatePDFMarginLeft;

            this.QuoteOPTemplateSettingPM.QuoteTemplatePDFMarginTop = this.QuoteOPTemplatePDFMarginTop;
            this.QuoteOPTemplateSettingPM.QuoteTemplatePDFMarginBottom = this.QuoteOPTemplatePDFMarginBottom;


            if (this.QuoteOPTemplateSettingPM.IsDirty) {
                this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteOPTemplate.M.Saving"));
                this.QuoteOPTemplateSettingPMService.update(this.QuoteOPTemplateSettingPM).subscribe((res:any) => {
                    this.QuoteOPTemplateSettingPM.IsDirty = false;
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
