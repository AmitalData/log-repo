import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {QuoteTemplateTextDesignPMService} from '../../../Quote/Services/StandardPMs/QuoteTemplateTextDesignPMService';
import {QuoteTemplateSettingPM} from '../../../Quote/EntityPMs/QuoteTemplateSettingPM';
import {QuoteTemplateTableDesignPMService} from '../../../Quote/Services/StandardPMs/QuoteTemplateTableDesignPMService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {QuoteTemplateSettingPMService} from '../../../Quote/Services/StandardPMs/QuoteTemplateSettingPMService';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {QuoteTemplateTextDesignPM} from '../../../Quote/EntityPMs/QuoteTemplateTextDesignPM';
import {AppTool} from '../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
@Component({
    selector: 'PageAreaHeaderFooterComponent',
    
    templateUrl: './PageAreaHeaderFooterComponent.html',
})

export class PageAreaHeaderFooterComponent extends BaseComponent implements OnInit {
    quoteTemplateSettingPMService: QuoteTemplateSettingPMService;
    DataContext: this;
    QuoteTemplateSettingPM: QuoteTemplateSettingPM;
    quoteTemplateTextDesignPMService: QuoteTemplateTextDesignPMService;
    DesignAreaFreeTextPM: QuoteTemplateTextDesignPM;
  

    AreaType: string;
    AreaMode: string;
    QuoteTemplateSectionTypeName: string;

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
        this.quoteTemplateSettingPMService = new QuoteTemplateSettingPMService();
        this.quoteTemplateTextDesignPMService = new QuoteTemplateTextDesignPMService();
    }

    ngOnInit() {

    }

     SetWindowArgs(args: any) {
        this.QuoteTemplateSettingPM = args.QuoteTemplateSettingPM;
        this.QuoteTemplateSectionTypeName = args.QuoteTemplateSectionTypeName;
        this.AreaType = args.AreaType;
        this.AreaMode = args.AreaMode;

        //Logo
        if (this.AreaMode == "Logo") {

            if (this.AreaType == "Area1") {
                this.ImageId = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea1ImageDetailId : this.QuoteTemplateSettingPM.PageFooterArea1ImageDetailId;
                this.ImageWidth = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderImage1Width : this.QuoteTemplateSettingPM.PageFooterImage1Width;
                this.ImageHeight = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea1Height : this.QuoteTemplateSettingPM.PageFooterArea1Height;
                this.Alignment = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea1ImageAlignment : this.QuoteTemplateSettingPM.PageFooterArea1ImageAlignment;
            }

            else if (this.AreaType == "Area2") {
                this.ImageId = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea2ImageDetailId : this.QuoteTemplateSettingPM.PageFooterArea2ImageDetailId;
                this.ImageWidth = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderImage2Width : this.QuoteTemplateSettingPM.PageFooterImage2Width;
                this.ImageHeight = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea2Height : this.QuoteTemplateSettingPM.PageFooterArea2Height;
                this.Alignment = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea2ImageAlignment : this.QuoteTemplateSettingPM.PageFooterArea2ImageAlignment;
            }

            else if (this.AreaType == "Area3") {
                this.ImageId = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea3ImageDetailId : this.QuoteTemplateSettingPM.PageFooterArea3ImageDetailId;
                this.ImageWidth = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderImage3Width : this.QuoteTemplateSettingPM.PageFooterImage3Width;
                this.ImageHeight = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea3Height : this.QuoteTemplateSettingPM.PageFooterArea3Height;
                this.Alignment = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea3ImageAlignment : this.QuoteTemplateSettingPM.PageFooterArea3ImageAlignment;
            }

        }

        else if (this.AreaMode == "Text") {

         if (this.AreaType == "Area1") {
                this.PageAreaFreeText = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea1FreeText : this.QuoteTemplateSettingPM.PageFooterArea1FreeText;
                this.LoadDesignAreaFreeText(this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea1FreeTextDesignId : this.QuoteTemplateSettingPM.PageFooterArea1FreeTextDesignId);
            }

            else if (this.AreaType == "Area2") {
                this.PageAreaFreeText = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea2FreeText : this.QuoteTemplateSettingPM.PageFooterArea2FreeText;
                this.LoadDesignAreaFreeText(this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea2FreeTextDesignId : this.QuoteTemplateSettingPM.PageFooterArea2FreeTextDesignId);
            }

            else if (this.AreaType == "Area3") {
                this.PageAreaFreeText = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea3FreeText : this.QuoteTemplateSettingPM.PageFooterArea3FreeText;
                this.LoadDesignAreaFreeText(this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea3FreeTextDesignId : this.QuoteTemplateSettingPM.PageFooterArea3FreeTextDesignId);
            }
        }

    }


     LoadDesignAreaFreeText(headerDesignId: string) {
         this.quoteTemplateTextDesignPMService.get(headerDesignId).subscribe((res:any) => {
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
                if (this.QuoteTemplateSectionTypeName == "Header") {
                    this.QuoteTemplateSettingPM.PageHeaderArea1Height = this.ImageHeight;
                    this.QuoteTemplateSettingPM.PageHeaderImage1Width = this.ImageWidth;
                    this.QuoteTemplateSettingPM.PageHeaderArea1ImageDetailId = this.ImageId;
                    this.QuoteTemplateSettingPM.PageHeaderArea1ImageAlignment = this.Alignment;
                }
                else {
                    this.QuoteTemplateSettingPM.PageFooterArea1Height = this.ImageHeight;
                    this.QuoteTemplateSettingPM.PageFooterImage1Width = this.ImageWidth;
                    this.QuoteTemplateSettingPM.PageFooterArea1ImageDetailId = this.ImageId;
                    this.QuoteTemplateSettingPM.PageFooterArea1ImageAlignment = this.Alignment;
                }
            }
            else if (this.AreaType == "Area2") {
                if (this.QuoteTemplateSectionTypeName == "Header") {
                    this.QuoteTemplateSettingPM.PageHeaderArea2Height = this.ImageHeight;
                    this.QuoteTemplateSettingPM.PageHeaderImage2Width = this.ImageWidth;
                    this.QuoteTemplateSettingPM.PageHeaderArea2ImageDetailId = this.ImageId;
                    this.QuoteTemplateSettingPM.PageHeaderArea2ImageAlignment = this.Alignment;
                }
                else {
                    this.QuoteTemplateSettingPM.PageFooterArea2Height = this.ImageHeight;
                    this.QuoteTemplateSettingPM.PageFooterImage2Width = this.ImageWidth;
                    this.QuoteTemplateSettingPM.PageFooterArea2ImageDetailId = this.ImageId;
                    this.QuoteTemplateSettingPM.PageFooterArea2ImageAlignment = this.Alignment;
                }
            }
            else if (this.AreaType == "Area3") {
                if (this.QuoteTemplateSectionTypeName == "Header") {
                    this.QuoteTemplateSettingPM.PageHeaderArea3Height = this.ImageHeight;
                    this.QuoteTemplateSettingPM.PageHeaderImage3Width = this.ImageWidth;
                    this.QuoteTemplateSettingPM.PageHeaderArea3ImageDetailId = this.ImageId;
                    this.QuoteTemplateSettingPM.PageHeaderArea3ImageAlignment = this.Alignment;
                }
                else {
                    this.QuoteTemplateSettingPM.PageFooterArea3Height = this.ImageHeight;
                    this.QuoteTemplateSettingPM.PageFooterImage3Width = this.ImageWidth;
                    this.QuoteTemplateSettingPM.PageFooterArea3ImageDetailId = this.ImageId;
                    this.QuoteTemplateSettingPM.PageFooterArea3ImageAlignment = this.Alignment;
                }
            }
        }

        else if (this.AreaMode == "Text") {

            this.PageAreaFreeText = this.DesignAreaFreeTextPM.TextValue;
  
            if (this.AreaType == "Area1") {
                if (this.QuoteTemplateSectionTypeName == "Header") this.QuoteTemplateSettingPM.PageHeaderArea1FreeText = this.PageAreaFreeText;
                else this.QuoteTemplateSettingPM.PageFooterArea1FreeText = this.QuoteTemplateSettingPM.PageFooterArea1FreeText = this.PageAreaFreeText;
            }

            else if (this.AreaType == "Area2") {
                if (this.QuoteTemplateSectionTypeName == "Header") this.QuoteTemplateSettingPM.PageHeaderArea2FreeText = this.PageAreaFreeText;
                else this.QuoteTemplateSettingPM.PageFooterArea2FreeText = this.QuoteTemplateSettingPM.PageFooterArea2FreeText = this.PageAreaFreeText;
            }

            else if (this.AreaType == "Area3") {
                if (this.QuoteTemplateSectionTypeName == "Header") this.QuoteTemplateSettingPM.PageHeaderArea3FreeText = this.PageAreaFreeText;
                else this.QuoteTemplateSettingPM.PageFooterArea3FreeText = this.QuoteTemplateSettingPM.PageFooterArea3FreeText = this.PageAreaFreeText;
            }

        }



        if (this.QuoteTemplateSettingPM.IsDirty || (this.DesignAreaFreeTextPM && this.DesignAreaFreeTextPM.IsDirty)) {
            this.IsSaveRuning = true;
            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));
        }


        if (this.DesignAreaFreeTextPM && this.DesignAreaFreeTextPM.IsDirty) {

            this.quoteTemplateTextDesignPMService.update(this.DesignAreaFreeTextPM).subscribe((res:any) => {
                this.SaveQuoteTemplateSetting();
            });
        }
        else {
            this.SaveQuoteTemplateSetting();
        }
     
      
    }

    SaveQuoteTemplateSetting() {
    if (this.QuoteTemplateSettingPM.IsDirty) {
            this.quoteTemplateSettingPMService.update(this.QuoteTemplateSettingPM).subscribe((res:any) => {
                this.QuoteTemplateSettingPM.IsDirty = false;
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


