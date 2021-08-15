import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {QuoteOPTemplateTextDesignPMService} from '../../../QuoteOPM/Services/StandardPMs/QuoteOPTemplateTextDesignPMService';
import {QuoteOPTemplateSettingPM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplateSettingPM';
import {QuoteOPTemplateTableDesignPMService} from '../../../QuoteOPM/Services/StandardPMs/QuoteOPTemplateTableDesignPMService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {QuoteOPTemplateSettingPMService} from '../../../QuoteOPM/Services/StandardPMs/QuoteOPTemplateSettingPMService';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {QuoteOPTemplateTextDesignPM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplateTextDesignPM';
import {AppTool} from '../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
@Component({
    selector: 'PageAreaHeaderFooterComponent',
    
    templateUrl: './PageAreaHeaderFooterComponent.html',
})

export class PageAreaHeaderFooterComponent extends BaseComponent implements OnInit {
    QuoteOPTemplateSettingPMService: QuoteOPTemplateSettingPMService;
    DataContext: this;
    QuoteOPTemplateSettingPM: QuoteOPTemplateSettingPM;
    QuoteOPTemplateTextDesignPMService: QuoteOPTemplateTextDesignPMService;
    DesignAreaFreeTextPM: QuoteOPTemplateTextDesignPM;
  

    AreaType: string;
    AreaMode: string;
    QuoteOPTemplateSectionTypeName: string;

    //Logo
    ImageWidth: number;
    ImageHeight: number;
    Alignment: string;
    ImageId: string;


    //Text
    PageAreaFreeText: string = "";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.QuoteOPTemplateSettingPMService = new QuoteOPTemplateSettingPMService();
        this.QuoteOPTemplateTextDesignPMService = new QuoteOPTemplateTextDesignPMService();
    }

    ngOnInit() {

    }

     SetWindowArgs(args: any) {
        this.QuoteOPTemplateSettingPM = args.QuoteOPTemplateSettingPM;
        this.QuoteOPTemplateSectionTypeName = args.QuoteOPTemplateSectionTypeName;
        this.AreaType = args.AreaType;
        this.AreaMode = args.AreaMode;

        //Logo
        if (this.AreaMode == "Logo") {

            if (this.AreaType == "Area1") {
                this.ImageId = this.QuoteOPTemplateSectionTypeName == "Header" ? this.QuoteOPTemplateSettingPM.PageHeaderArea1ImageDetailId : this.QuoteOPTemplateSettingPM.PageFooterArea1ImageDetailId;
                this.ImageWidth = this.QuoteOPTemplateSectionTypeName == "Header" ? this.QuoteOPTemplateSettingPM.PageHeaderImage1Width : this.QuoteOPTemplateSettingPM.PageFooterImage1Width;
                this.ImageHeight = this.QuoteOPTemplateSectionTypeName == "Header" ? this.QuoteOPTemplateSettingPM.PageHeaderArea1Height : this.QuoteOPTemplateSettingPM.PageFooterArea1Height;
                this.Alignment = this.QuoteOPTemplateSectionTypeName == "Header" ? this.QuoteOPTemplateSettingPM.PageHeaderArea1ImageAlignment : this.QuoteOPTemplateSettingPM.PageFooterArea1ImageAlignment;
            }

            else if (this.AreaType == "Area2") {
                this.ImageId = this.QuoteOPTemplateSectionTypeName == "Header" ? this.QuoteOPTemplateSettingPM.PageHeaderArea2ImageDetailId : this.QuoteOPTemplateSettingPM.PageFooterArea2ImageDetailId;
                this.ImageWidth = this.QuoteOPTemplateSectionTypeName == "Header" ? this.QuoteOPTemplateSettingPM.PageHeaderImage2Width : this.QuoteOPTemplateSettingPM.PageFooterImage2Width;
                this.ImageHeight = this.QuoteOPTemplateSectionTypeName == "Header" ? this.QuoteOPTemplateSettingPM.PageHeaderArea2Height : this.QuoteOPTemplateSettingPM.PageFooterArea2Height;
                this.Alignment = this.QuoteOPTemplateSectionTypeName == "Header" ? this.QuoteOPTemplateSettingPM.PageHeaderArea2ImageAlignment : this.QuoteOPTemplateSettingPM.PageFooterArea2ImageAlignment;
            }

            else if (this.AreaType == "Area3") {
                this.ImageId = this.QuoteOPTemplateSectionTypeName == "Header" ? this.QuoteOPTemplateSettingPM.PageHeaderArea3ImageDetailId : this.QuoteOPTemplateSettingPM.PageFooterArea3ImageDetailId;
                this.ImageWidth = this.QuoteOPTemplateSectionTypeName == "Header" ? this.QuoteOPTemplateSettingPM.PageHeaderImage3Width : this.QuoteOPTemplateSettingPM.PageFooterImage3Width;
                this.ImageHeight = this.QuoteOPTemplateSectionTypeName == "Header" ? this.QuoteOPTemplateSettingPM.PageHeaderArea3Height : this.QuoteOPTemplateSettingPM.PageFooterArea3Height;
                this.Alignment = this.QuoteOPTemplateSectionTypeName == "Header" ? this.QuoteOPTemplateSettingPM.PageHeaderArea3ImageAlignment : this.QuoteOPTemplateSettingPM.PageFooterArea3ImageAlignment;
            }

        }

        else if (this.AreaMode == "Text") {

         if (this.AreaType == "Area1") {
                this.PageAreaFreeText = this.QuoteOPTemplateSectionTypeName == "Header" ? this.QuoteOPTemplateSettingPM.PageHeaderArea1FreeText : this.QuoteOPTemplateSettingPM.PageFooterArea1FreeText;
                this.LoadDesignAreaFreeText(this.QuoteOPTemplateSectionTypeName == "Header" ? this.QuoteOPTemplateSettingPM.PageHeaderArea1FreeTextDesignId : this.QuoteOPTemplateSettingPM.PageFooterArea1FreeTextDesignId);
            }

            else if (this.AreaType == "Area2") {
                this.PageAreaFreeText = this.QuoteOPTemplateSectionTypeName == "Header" ? this.QuoteOPTemplateSettingPM.PageHeaderArea2FreeText : this.QuoteOPTemplateSettingPM.PageFooterArea2FreeText;
                this.LoadDesignAreaFreeText(this.QuoteOPTemplateSectionTypeName == "Header" ? this.QuoteOPTemplateSettingPM.PageHeaderArea2FreeTextDesignId : this.QuoteOPTemplateSettingPM.PageFooterArea2FreeTextDesignId);
            }

            else if (this.AreaType == "Area3") {
                this.PageAreaFreeText = this.QuoteOPTemplateSectionTypeName == "Header" ? this.QuoteOPTemplateSettingPM.PageHeaderArea3FreeText : this.QuoteOPTemplateSettingPM.PageFooterArea3FreeText;
                this.LoadDesignAreaFreeText(this.QuoteOPTemplateSectionTypeName == "Header" ? this.QuoteOPTemplateSettingPM.PageHeaderArea3FreeTextDesignId : this.QuoteOPTemplateSettingPM.PageFooterArea3FreeTextDesignId);
            }
        }

    }


     LoadDesignAreaFreeText(headerDesignId: string) {
         this.QuoteOPTemplateTextDesignPMService.get(headerDesignId).subscribe((res:any) => {
             var pmResponse: ServiceResponse = res;
       
             if (!pmResponse.HasError && pmResponse.Result) {
                 this.DesignAreaFreeTextPM = pmResponse.Result;

                 this.DesignAreaFreeTextPM.TextValue = this.PageAreaFreeText;

                 this.DesignAreaFreeTextPM.Title = "DesignAreaFreeText";
             }

         });

     }




    AlignmentButtonClick(alignment: string) {
        if (!AppTool.IsNullOrEmpty(alignment)) {
            if (alignment == this.Alignment) alignment = "";
            this.Alignment = alignment;
        }
    }

    ImageUploadedCompleted(imageId: string) {
        this.ImageId = imageId;
    }

    ImageNumericButtonClicked(propName:string , isIncreas: boolean) {
        var value: number = propName == "ImageHeight" ? this.ImageHeight : this.ImageWidth;
        value -= 0;
        if (isIncreas) value += 1;
        else value -= 1;
        if (propName == "ImageHeight") this.ImageHeight = value; 
        else this.ImageWidth = value; 

    }
    IsSaveRuning: boolean = false;
    SaveButtonClicked() {
    
        if (this.AreaMode == "Logo") {
            if (this.AreaType == "Area1") {
                if (this.QuoteOPTemplateSectionTypeName == "Header") {
                    this.QuoteOPTemplateSettingPM.PageHeaderArea1Height = this.ImageHeight;
                    this.QuoteOPTemplateSettingPM.PageHeaderImage1Width = this.ImageWidth;
                    this.QuoteOPTemplateSettingPM.PageHeaderArea1ImageDetailId = this.ImageId;
                    this.QuoteOPTemplateSettingPM.PageHeaderArea1ImageAlignment = this.Alignment;
                }
                else {
                    this.QuoteOPTemplateSettingPM.PageFooterArea1Height = this.ImageHeight;
                    this.QuoteOPTemplateSettingPM.PageFooterImage1Width = this.ImageWidth;
                    this.QuoteOPTemplateSettingPM.PageFooterArea1ImageDetailId = this.ImageId;
                    this.QuoteOPTemplateSettingPM.PageFooterArea1ImageAlignment = this.Alignment;
                }
            }
            else if (this.AreaType == "Area2") {
                if (this.QuoteOPTemplateSectionTypeName == "Header") {
                    this.QuoteOPTemplateSettingPM.PageHeaderArea2Height = this.ImageHeight;
                    this.QuoteOPTemplateSettingPM.PageHeaderImage2Width = this.ImageWidth;
                    this.QuoteOPTemplateSettingPM.PageHeaderArea2ImageDetailId = this.ImageId;
                    this.QuoteOPTemplateSettingPM.PageHeaderArea2ImageAlignment = this.Alignment;
                }
                else {
                    this.QuoteOPTemplateSettingPM.PageFooterArea2Height = this.ImageHeight;
                    this.QuoteOPTemplateSettingPM.PageFooterImage2Width = this.ImageWidth;
                    this.QuoteOPTemplateSettingPM.PageFooterArea2ImageDetailId = this.ImageId;
                    this.QuoteOPTemplateSettingPM.PageFooterArea2ImageAlignment = this.Alignment;
                }
            }
            else if (this.AreaType == "Area3") {
                if (this.QuoteOPTemplateSectionTypeName == "Header") {
                    this.QuoteOPTemplateSettingPM.PageHeaderArea3Height = this.ImageHeight;
                    this.QuoteOPTemplateSettingPM.PageHeaderImage3Width = this.ImageWidth;
                    this.QuoteOPTemplateSettingPM.PageHeaderArea3ImageDetailId = this.ImageId;
                    this.QuoteOPTemplateSettingPM.PageHeaderArea3ImageAlignment = this.Alignment;
                }
                else {
                    this.QuoteOPTemplateSettingPM.PageFooterArea3Height = this.ImageHeight;
                    this.QuoteOPTemplateSettingPM.PageFooterImage3Width = this.ImageWidth;
                    this.QuoteOPTemplateSettingPM.PageFooterArea3ImageDetailId = this.ImageId;
                    this.QuoteOPTemplateSettingPM.PageFooterArea3ImageAlignment = this.Alignment;
                }
            }
        }

        else if (this.AreaMode == "Text") {

            this.PageAreaFreeText = this.DesignAreaFreeTextPM.TextValue;
  
            if (this.AreaType == "Area1") {
                if (this.QuoteOPTemplateSectionTypeName == "Header") this.QuoteOPTemplateSettingPM.PageHeaderArea1FreeText = this.PageAreaFreeText;
                else this.QuoteOPTemplateSettingPM.PageFooterArea1FreeText = this.QuoteOPTemplateSettingPM.PageFooterArea1FreeText = this.PageAreaFreeText;
            }

            else if (this.AreaType == "Area2") {
                if (this.QuoteOPTemplateSectionTypeName == "Header") this.QuoteOPTemplateSettingPM.PageHeaderArea2FreeText = this.PageAreaFreeText;
                else this.QuoteOPTemplateSettingPM.PageFooterArea2FreeText = this.QuoteOPTemplateSettingPM.PageFooterArea2FreeText = this.PageAreaFreeText;
            }

            else if (this.AreaType == "Area3") {
                if (this.QuoteOPTemplateSectionTypeName == "Header") this.QuoteOPTemplateSettingPM.PageHeaderArea3FreeText = this.PageAreaFreeText;
                else this.QuoteOPTemplateSettingPM.PageFooterArea3FreeText = this.QuoteOPTemplateSettingPM.PageFooterArea3FreeText = this.PageAreaFreeText;
            }

        }



        if (this.QuoteOPTemplateSettingPM.IsDirty || (this.DesignAreaFreeTextPM && this.DesignAreaFreeTextPM.IsDirty)) {
            this.IsSaveRuning = true;
            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteOPTemplate.M.Saving"));
        }


        if (this.DesignAreaFreeTextPM && this.DesignAreaFreeTextPM.IsDirty) {

            this.QuoteOPTemplateTextDesignPMService.update(this.DesignAreaFreeTextPM).subscribe((res:any) => {
                this.SaveQuoteOPTemplateSetting();
            });
        }
        else {
            this.SaveQuoteOPTemplateSetting();
        }
     
      
    }

    SaveQuoteOPTemplateSetting() {
    if (this.QuoteOPTemplateSettingPM.IsDirty) {
            this.QuoteOPTemplateSettingPMService.update(this.QuoteOPTemplateSettingPM).subscribe((res:any) => {
                this.QuoteOPTemplateSettingPM.IsDirty = false;
                this.SaveCompleted();

            });
        }
        else {
         
            this.SaveCompleted();
        }

    }


    SaveCompleted() {
        this.CurrentSession.StopBusyIndicator();
        if (this.IsSaveRuning) this.CurrentSession.CurrentWindow.Close("Refresh");
        else this.CurrentSession.CloseCurrentWindow();
    }


    CloseButtonClicked() {

     this.CurrentSession.CloseCurrentWindow();
    

        
    }
}


