
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {Component, ElementRef, OnInit, AfterViewInit, EventEmitter, Output, ChangeDetectorRef} from '@angular/core';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {QuoteTemplateTextDesignPM} from '../../../Quote/EntityPMs/QuoteTemplateTextDesignPM';
import {QuoteTemplateTableDesignPM} from '../../../Quote/EntityPMs/QuoteTemplateTableDesignPM';
import {QuoteTemplateSettingPM} from '../../../Quote/EntityPMs/QuoteTemplateSettingPM';
declare var window: any;
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
declare var insertAtSubject : any;
@Component({
    
    selector: 'TextDesignComponent',
    templateUrl: './TextDesignComponent.html',
    inputs: ['QuoteTemplateTextDesignPM', 'QuoteTemplateTableDesignPM', 'QuoteTemplateSettingPM', 'SectionType']
})

export class TextDesignComponent implements OnInit {
    elementRef: ElementRef;
    public QuoteTemplateTextDesignPM: QuoteTemplateTextDesignPM;
    QuoteTemplateTableDesignPM: QuoteTemplateTableDesignPM;
    QuoteTemplateSettingPM: QuoteTemplateSettingPM;
    FontStyle: string;
    SectionType: string;
    TextDecoration: string;
    FontFamilyLists: string[] = [];
    FontSizeLists: number[] = [];
    SelectFonteSize: string = "";
    SampleText: string = "";
    BorderTypesSelected: BorderType;
    BorderTypes: BorderType[] = [];
   private _entityResourceService: EntityResourceService = new EntityResourceService();
    constructor(elementRef: ElementRef, private cd: ChangeDetectorRef) {
        this.elementRef = elementRef;
   }
    Title: string = "";
    ngOnInit() {
        if (this.QuoteTemplateTextDesignPM) {
            this.Title = TextCodeTranslator.Translate("QuoteTemplate.S." + this.QuoteTemplateTextDesignPM.Title.replace(" ", ""));
            this.SampleText = this.QuoteTemplateTextDesignPM.SampleText ? this.QuoteTemplateTextDesignPM.SampleText : TextCodeTranslator.Translate("QuoteTemplate.S.SampleText");
            var fontFamilyString = "Arial,Arial Black,Calibri,Comic Sans MS,Courier New,Georgia,Lucida Sans Unicode,Times New Roman,Trebuchet MS,Verdana,Impact,Tahoma";
            var fontSizeString = "8,9,10,11,12,14,16,18,20,22,24,26,28,36,48,72";
            fontSizeString.split(',').forEach((fontsize) => { this.FontSizeLists.push(Number(fontsize)); });
            this.FontFamilyLists = fontFamilyString.split(',');

            this.SetFonteSize();
            this.SetFontStyle();
            this.SetTextDecoration();
        }

        this.FullBorderTypesLists();
    }


    FullBorderTypesLists() {

        if (this.QuoteTemplateTableDesignPM) {
            this.BorderTypes = [];

            this.BorderTypes.push(new BorderType("None", "NONE"));
            this.BorderTypes.push(new BorderType("All", "ALL"));
            this.BorderTypes.push(new BorderType("Box", "BOX"));
            this.BorderTypes.push(new BorderType("Horizontal Only", "HORIZONTALLINES"));
            this.BorderTypes.push(new BorderType("Vertical Only", "VERTICALLINES"));

            this.BorderTypesSelected = this.BorderTypes.filter(d => d.Code == this.QuoteTemplateTableDesignPM.BorderTypeCode)[0];
        }
    }
   
    get TextColor() {
        var textColor: string = "";
        if (this.QuoteTemplateTextDesignPM) {
            textColor = this.GetColorFromOrginal(this.QuoteTemplateTextDesignPM.TextColor);
        }
        return textColor;
    }
    set TextColor(value: string) {
        if (this.QuoteTemplateTextDesignPM) {
            this.QuoteTemplateTextDesignPM.TextColor = this.GetOrginalFromColor(value);
        }
        
    }


    get BackgroundColor() {
        var backgroundColor: string = "";
        if (this.QuoteTemplateTextDesignPM) {
            backgroundColor = this.GetColorFromOrginal(this.QuoteTemplateTextDesignPM.BackgroundColor);
        }
        return backgroundColor;
    }
    set BackgroundColor(value: string) {
        if (this.QuoteTemplateTextDesignPM) {
            this.QuoteTemplateTextDesignPM.BackgroundColor = this.GetOrginalFromColor(value);
        }

    }


    get BorderColor() {
        var borderColor: string = "";
        if (this.QuoteTemplateTableDesignPM) {
            borderColor = this.GetColorFromOrginal(this.QuoteTemplateTableDesignPM.BorderColor);
        }
        return borderColor;
    }
    set BorderColor(value: string) {
        if (this.QuoteTemplateTableDesignPM) {
            this.QuoteTemplateTableDesignPM.BorderColor = this.GetOrginalFromColor(value);
        }

    }
    




    get SpaceLinesBefore() {
        var spaceLinesBefore: number = 1;
        if (this.QuoteTemplateSettingPM) {
            if (this.SectionType == "Containers") spaceLinesBefore = this.QuoteTemplateSettingPM.SpaceLinesBeforeContainers;
            else if (this.SectionType == "Packages") spaceLinesBefore = this.QuoteTemplateSettingPM.SpaceLinesBeforePackages;
            else if (this.SectionType == "QuoteDetails") spaceLinesBefore = this.QuoteTemplateSettingPM.SpaceLinesBeforeQuoteDetails;
            else if (this.SectionType == "QuoteHeader") spaceLinesBefore = this.QuoteTemplateSettingPM.SpaceLinesBeforeQuoteHeaders;
            else if (this.SectionType == "Per") spaceLinesBefore = this.QuoteTemplateSettingPM.SpaceLinesBeforePerContainers;
        }
        return spaceLinesBefore;
    }
    set SpaceLinesBefore(value: number) {
        if (this.QuoteTemplateSettingPM) {

            if (this.SectionType == "Containers") this.QuoteTemplateSettingPM.SpaceLinesBeforeContainers = value;
            else if (this.SectionType == "Packages") this.QuoteTemplateSettingPM.SpaceLinesBeforePackages = value;
            else if (this.SectionType == "QuoteDetails") this.QuoteTemplateSettingPM.SpaceLinesBeforeQuoteDetails = value;
            else if (this.SectionType == "QuoteHeader") this.QuoteTemplateSettingPM.SpaceLinesBeforeQuoteHeaders = value;
            else if (this.SectionType == "Per") this.QuoteTemplateSettingPM.SpaceLinesBeforePerContainers = value;
        }

    }

    






 
    //UnDerLineButton
    QuoteTemplateTextDesignButtonClick(type: string) {
        if (type == "BoldButton") {
            this.QuoteTemplateTextDesignPM.FontWeight = this.QuoteTemplateTextDesignPM.FontWeight == "bold" ? this.QuoteTemplateTextDesignPM.FontWeight = "normal" : this.QuoteTemplateTextDesignPM.FontWeight = "bold";
        }
        else if (type == "ItalicButton") {
            this.QuoteTemplateTextDesignPM.Italic = this.QuoteTemplateTextDesignPM.Italic ? false : true;
            this.SetFontStyle();
        }
        else if (type == "UnDerLineButton") {
            this.QuoteTemplateTextDesignPM.UnDerLine = this.QuoteTemplateTextDesignPM.UnDerLine ? false : true;
            this.SetTextDecoration();
        }
    }

    AlignmentButtonClick(alignment:string) {
        if (this.QuoteTemplateTextDesignPM) {
            if (alignment != this.QuoteTemplateTextDesignPM.Alignment) {
                this.QuoteTemplateTextDesignPM.Alignment = alignment;
            } else this.QuoteTemplateTextDesignPM.Alignment = "";
        }
    }

    FontSizeSelectedChange(fontsize: number) {
        this.QuoteTemplateTextDesignPM.FontSize = fontsize;
        this.SetFonteSize();
    }

    SetFontStyle() {
        if (this.QuoteTemplateTextDesignPM) {
            this.FontStyle = this.QuoteTemplateTextDesignPM.Italic ? "italic" : "normal";
        }
    }

    SetTextDecoration() {
        if (this.QuoteTemplateTextDesignPM) {
            this.TextDecoration = this.QuoteTemplateTextDesignPM.UnDerLine ? "underline" : "none";
        }
    }

    SetFonteSize() {
        if (this.QuoteTemplateTextDesignPM && this.QuoteTemplateTextDesignPM.FontSize) {
            this.SelectFonteSize = this.QuoteTemplateTextDesignPM.FontSize.toString() + "px";
        }


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
    GetColorFromOrginal(color:string) {
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

    BorderTypesSelectedChanged(border: BorderType) {
        if (this.QuoteTemplateTableDesignPM) {
            this.QuoteTemplateTableDesignPM.BorderTypeCode = border.Code;
        }
    }
    QuoteTextAreaInputId: string = Guid.newGuid();

    AddDataField() {

        var tableId: string = "";
        var table = window.ObjectTables.filter(d => d.Name == "Quote")[0];
        if (table) tableId = table.Id;

        this._entityResourceService.getEntityResourceByTableName("SystemData").subscribe((response:any) => {

            this._entityResourceService.getEntityResourceByTableName("Quote").subscribe((response:any) => {
                var windowArgs: any = {};
                windowArgs.ObjectTableId = tableId;

                var logWindow = new LogitudeWindow();
                logWindow.Width = 500;
                logWindow.Height = 600;
                windowArgs.InSertDataFieldType = "TextArea";
                logWindow.Title = "Insert Data Field";
                logWindow.WindowArgs = windowArgs;
                logWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocumentObjectFieldsComponent');
                logWindow.WindowClosed.subscribe(($event: any) => {

                    if ($event) {
                        if (this.QuoteTemplateTextDesignPM) {
                            this.QuoteTemplateTextDesignPM.TextValue = insertAtSubject(this.QuoteTextAreaInputId, $event);
                        }
                    }

                });
            });


        });

    }

}




export class BorderType   {

    Code: string;
    Name: string;
    constructor(name: string, code: string) {
        this.Name = TextCodeTranslator.Translate("QuoteTemplate.S." + name.replace(" ",""));

        this.Code = code;
    }
}
