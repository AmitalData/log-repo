import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {QuoteTemplatePM} from '../../../Quote/EntityPMs/QuoteTemplatePM';
import {QuoteTemplateSettingPM} from '../../../Quote/EntityPMs/QuoteTemplateSettingPM';
import {QuoteTemplateSectionPM} from '../../../Quote/EntityPMs/QuoteTemplateSectionPM';
import {QuoteTemplateTextDesignPM} from '../../../Quote/EntityPMs/QuoteTemplateTextDesignPM';
import {QuoteTemplateTableDesignPM} from '../../../Quote/EntityPMs/QuoteTemplateTableDesignPM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {QuoteTemplateSettingPMService} from '../../../Quote/Services/StandardPMs/QuoteTemplateSettingPMService';
import {QuoteTemplateTableDesignPMService} from '../../../Quote/Services/StandardPMs/QuoteTemplateTableDesignPMService';
import {QuoteTemplateTextDesignPMService} from '../../../Quote/Services/StandardPMs/QuoteTemplateTextDesignPMService';
import {QuoteTemplateTextDesignExtendedPMService} from '../../../Quote/Services/ExtendedPMs/QuoteTemplateTextDesignExtendedPMService';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {FroalaEditorSetting} from '../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/FroalaEditorSetting';
import {QuoteTemplateSectionViewModel} from './EditQuoteTemplateComponent';
import {BorderType} from '../../../Infrastructure/Components/LogitudeCustomComponents/TextDesignComponent';
import {QuoteTemplateSectionExtendedPMService} from '../../../Quote/Services/ExtendedPMs/QuoteTemplateSectionExtendedPMService';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
@Component({
    selector: 'QuoteTemplateHeaderFooterSettingComponent',
    moduleId: module.id,
    templateUrl: './QuoteTemplateHeaderFooterSettingComponent.html',
})

export class QuoteTemplateHeaderFooterSettingComponent extends BaseComponent implements OnInit {
    quoteTemplateSettingPMService: QuoteTemplateSettingPMService;
    quoteTemplateTextDesignPMService: QuoteTemplateTextDesignPMService;
    quoteTemplateTableDesignPMService: QuoteTemplateTableDesignPMService;
    quoteTemplateTextDesignExtendedPMService: QuoteTemplateTextDesignExtendedPMService;
    quoteTemplateSectionExtendedPMService: QuoteTemplateSectionExtendedPMService;
    QuoteTemplateSettingPM: QuoteTemplateSettingPM;
    QuoteTemplatePM: QuoteTemplatePM;
    IsLoadPage: boolean;
    Alignment: string[] = [];
    QuoteTemplateSectionViewModel: QuoteTemplateSectionViewModel;
    froalaEditorSetting: FroalaEditorSetting;
    public ValidationErrorsList: string[];
    QuoteTemplateSectionTypeName: string = "Packages";
    EditQuoteTemplateComponent: any;
    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;



    QuoteId: string;
    BorderTypesSelected: BorderType;
    BorderTypes: BorderType[] = [];

    AreaType: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.quoteTemplateSettingPMService = new QuoteTemplateSettingPMService();
        this.quoteTemplateTableDesignPMService = new QuoteTemplateTableDesignPMService();
        this.quoteTemplateTextDesignPMService = new QuoteTemplateTextDesignPMService();
        this.quoteTemplateTextDesignExtendedPMService = new QuoteTemplateTextDesignExtendedPMService();
        this.quoteTemplateSectionExtendedPMService = new QuoteTemplateSectionExtendedPMService();
        
      
    }

    ngOnInit() {

    }

    
    SetWindowArgs(args: any) {
        this.QuoteTemplatePM = args.QuoteTemplatePM;
        this.QuoteTemplateSectionTypeName = args.QuoteTemplateSectionTypeName
        this.QuoteTemplateSettingPM = args.QuoteTemplateSettingPM;
        this.QuoteTemplateSectionViewModel = args.QuoteTemplateSectionViewModel;  
        this.EditQuoteTemplateComponent = args.EditQuoteTemplateComponent;  
     


        this.QuoteId = args.QuoteId;  
        var areaTypeString = "Logo,Text";
        if (this.QuoteTemplateSectionTypeName == "Header") areaTypeString += ",Quote Header";
        areaTypeString += ",None";

        this.AreaType = areaTypeString.split(',');

        this.froalaEditorSetting = new FroalaEditorSetting();
        this.froalaEditorSetting.Id = Guid.newGuid();
        this.froalaEditorSetting.IsDisableEdit = true;

        if (this.QuoteTemplateSectionViewModel) {
            this.RefreshQuoteTemplateSectionBodyHtml();
            //if (this.QuoteTemplateSectionViewModel.IsLoaded) {
            //    this.froalaEditorSetting.HtmlString = this.QuoteTemplateSectionViewModel.HtmlBody;
            //}
            //else {

            //    this.RefreshQuoteTemplateSectionBodyHtml();
            //}
        }
        
        this.froalaEditorSetting.Height = ((this.CurrentSession.CurrentWindow.Height / 2) -20);


        this.FullProperity();
        this.FullBorderTypesLists();
    }


    //Full Data

    FullProperity() {

        if (this.QuoteTemplateSettingPM) {
            this.HeightArea = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderAreaHeight : this.QuoteTemplateSettingPM.PageFooterAreaHeight;
            this.Area1Width = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea1Width : this.QuoteTemplateSettingPM.PageFooterArea1Width;
            this.Area2Width = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea2Width : this.QuoteTemplateSettingPM.PageFooterArea2Width;
            this.Area3Width = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea3Width : this.QuoteTemplateSettingPM.PageFooterArea3Width;
        }
    }
    FullBorderTypesLists() {


        this.BorderTypes = [];
        this.BorderTypes.push(new BorderType("None", "NONE"));
        this.BorderTypes.push(new BorderType("All", "ALL"));
        this.BorderTypes.push(new BorderType("Box", "BOX"));
        this.BorderTypes.push(new BorderType("Horizontal Only", "HORIZONTALLINES"));
        this.BorderTypes.push(new BorderType("Vertical Only", "VERTICALLINES"));

        var selectedBorderCode =  this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderBorderTypeCode : this.QuoteTemplateSettingPM.PageFooterBorderTypeCode;


        this.BorderTypesSelected = this.BorderTypes.filter(d => d.Code == selectedBorderCode)[0];

    }
    

    // Prop

    private area1Width = 0;
    get Area1Width() {
        return this.area1Width;
    }
    set Area1Width(newValue: number) {
        this.area1Width = newValue;
    }

    private area2Width = 0;
    get Area2Width() {
        return this.area2Width;
    }
    set Area2Width(newValue: number) {
        this.area2Width = newValue;
    }

    private area3Width = 0;
    get Area3Width() {
        return this.area3Width;
    }
    set Area3Width(newValue: number) {
        this.area3Width = newValue;
    }

    private heightArea = 0;
    get HeightArea() {
        return this.heightArea;
    }
    set HeightArea(newValue: number) {
        this.heightArea = newValue;
    }

    get PageArea1Type() {
        var pageArea1Type: string = "";
        if (this.QuoteTemplateSettingPM) pageArea1Type = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea1Type : this.QuoteTemplateSettingPM.PageFooterArea1Type;
        return pageArea1Type;
    }
    get PageArea2Type() {
        var pageArea2Type: string = "";
        if (this.QuoteTemplateSettingPM) pageArea2Type = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea2Type : this.QuoteTemplateSettingPM.PageFooterArea2Type;
        return pageArea2Type;
    }
    get PageArea3Type() {
        var pageArea3Type: string = "";
        if (this.QuoteTemplateSettingPM) pageArea3Type = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea3Type : this.QuoteTemplateSettingPM.PageFooterArea3Type;
        return pageArea3Type;
    }




    //Event

    AreaWidthLostFocusMethod(area: string) {

        this.ValidateFields();
        
            if (area == "Area1") {
                var oldValue: number = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea1Width : this.QuoteTemplateSettingPM.PageFooterArea1Width;
                if (oldValue != this.Area1Width) {
                    if (this.QuoteTemplateSectionTypeName == "Header") {
                        this.QuoteTemplateSettingPM.PageHeaderArea1Width = this.Area1Width;
                    } else this.QuoteTemplateSettingPM.PageFooterArea1Width = this.Area1Width;

                    this.SaveChanges();
                }
            }

            if (area == "Area2") {
                var oldValue: number = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea2Width : this.QuoteTemplateSettingPM.PageFooterArea2Width;
                if (oldValue != this.Area2Width) {
                    if (this.QuoteTemplateSectionTypeName == "Header") {
                        this.QuoteTemplateSettingPM.PageHeaderArea2Width = this.Area2Width;
                    } else this.QuoteTemplateSettingPM.PageFooterArea2Width = this.Area2Width;

                    this.SaveChanges();
                }
            }

            if (area == "Area3") {
                var oldValue: number = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea3Width : this.QuoteTemplateSettingPM.PageFooterArea3Width;
                if (oldValue != this.Area3Width) {
                    if (this.QuoteTemplateSectionTypeName == "Header") {
                        this.QuoteTemplateSettingPM.PageHeaderArea3Width = this.Area3Width;
                    } else this.QuoteTemplateSettingPM.PageFooterArea3Width = this.Area3Width;

                    this.SaveChanges();
                }
            }
       
    }

    HeightAreaLostFocusMethod() {
        var oldValue = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderAreaHeight : this.QuoteTemplateSettingPM.PageFooterAreaHeight;
        if (oldValue != this.HeightArea) {
            if (this.QuoteTemplateSectionTypeName == "Header") {
                this.QuoteTemplateSettingPM.PageHeaderAreaHeight = this.HeightArea;
            } else this.QuoteTemplateSettingPM.PageFooterAreaHeight = this.HeightArea;
            this.SaveChanges();
        }


    }

    PageAreaSelectedChanged(area:string , value:string) {
        if (area == "Area1") {
            var oldValue = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea1Type : this.QuoteTemplateSettingPM.PageFooterArea1Type;
            if (oldValue != value) {
                if (this.QuoteTemplateSectionTypeName == "Header") {
                    this.QuoteTemplateSettingPM.PageHeaderArea1Type = value;
                } else this.QuoteTemplateSettingPM.PageFooterArea1Type = value;
                this.SaveChanges();
            }
        }
        else if (area == "Area2") {
            var oldValue = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea2Type : this.QuoteTemplateSettingPM.PageFooterArea2Type;
            if (oldValue != value) {
                if (this.QuoteTemplateSectionTypeName == "Header") {
                    this.QuoteTemplateSettingPM.PageHeaderArea2Type = value;
                } else this.QuoteTemplateSettingPM.PageFooterArea2Type = value;
                this.SaveChanges(); 
            }
        } else if (area == "Area3") {
            var oldValue = this.QuoteTemplateSectionTypeName == "Header" ? this.QuoteTemplateSettingPM.PageHeaderArea3Type : this.QuoteTemplateSettingPM.PageFooterArea3Type;
            if (oldValue != value) {
                if (this.QuoteTemplateSectionTypeName == "Header") {
                    this.QuoteTemplateSettingPM.PageHeaderArea3Type = value;
                } else this.QuoteTemplateSettingPM.PageFooterArea3Type = value;
                this.SaveChanges();
            }
        }
    }

    BorderTypesSelectedChanged(border: BorderType) {
        if (this.QuoteTemplateSettingPM) {

            if (this.QuoteTemplateSectionTypeName == "Header") {
                if (this.QuoteTemplateSettingPM.PageHeaderBorderTypeCode != border.Code) {
                    this.QuoteTemplateSettingPM.PageHeaderBorderTypeCode = border.Code;
                    this.SaveChanges();
                }
            } else {

                if (this.QuoteTemplateSettingPM.PageFooterBorderTypeCode != border.Code) {
                    this.QuoteTemplateSettingPM.PageFooterBorderTypeCode = border.Code;
                    this.SaveChanges();
                }

            }
        }
    }



    //Command


    AdvanceButtonClicked() {
 

            var windowArgs: any = {};
            var logWindow = new LogitudeWindow();
            windowArgs.QuoteTemplateSettingPM = this.QuoteTemplateSettingPM;
            windowArgs.QuoteTemplateSectionTypeName = this.QuoteTemplateSectionTypeName;
            logWindow.WindowArgs = windowArgs;
            logWindow.Width = 400;
            logWindow.Height = 130;
            logWindow.Title = TextCodeTranslator.Translate("QuoteTemplate.S.DesignTable");
            logWindow.Show("./QuoteModules/QuoteTemplates/Components/AdvanceDesignTableComponent");
            logWindow.WindowClosed.subscribe(($event: any) => {
                if ($event == "Refresh") {
                    this.IsChangeSetting = true;
                    this.RefreshQuoteTemplateSectionBodyHtml();
                }
            });


    }
    IsChangeSetting: boolean = false;
    EditPageArea(type: string) {



        if ((type == "Area1" && this.PageArea1Type == "Quote Header") || (type == "Area2" && this.PageArea2Type == "Quote Header") || (type == "Area3" && this.PageArea3Type == "Quote Header")) {
            this.ShowQuoteHeaderEditWindow();
        } else {

            var windowArgs: any = {};
            var logWindow = new LogitudeWindow();
            windowArgs.QuoteTemplateSettingPM = this.QuoteTemplateSettingPM;
            windowArgs.QuoteTemplateSectionTypeName = this.QuoteTemplateSectionTypeName;
            windowArgs.AreaType = type;

            if (type == "Area1") windowArgs.AreaMode = this.PageArea1Type;
            else if (type == "Area2") windowArgs.AreaMode = this.PageArea2Type;
            else if (type == "Area3") windowArgs.AreaMode = this.PageArea3Type;

            logWindow.WindowArgs = windowArgs;
            logWindow.Width = 800;
            logWindow.Height = 600;
            logWindow.BottomBorderForTitle = "1px solid LightGray";
            logWindow.Title = TextCodeTranslator.Translate("QuoteTemplate.S.Edit" + type);
            logWindow.Show("./QuoteModules/QuoteTemplates/Components/PageAreaHeaderFooterComponent");
            logWindow.WindowClosed.subscribe(($event: any) => {
                if ($event == "Refresh") {
                    this.IsChangeSetting = true;
                    this.RefreshQuoteTemplateSectionBodyHtml();
                }
            });
        }
    }


    ShowQuoteHeaderEditWindow() {
        if (this.EditQuoteTemplateComponent) {
            var windowArgs: any = {};
            windowArgs.QuoteTemplatePM = this.QuoteTemplatePM;
            windowArgs.QuoteTemplateSettingPM = this.QuoteTemplateSettingPM;
            var quoteTemplateSectionHeaderViewModel = this.EditQuoteTemplateComponent.QuoteTemplateSectionLists.filter(d => d.QuoteTemplateSectionTypeCode == "QH")[0];
            windowArgs.QuoteTemplateSectionViewModel = quoteTemplateSectionHeaderViewModel;
            windowArgs.QuoteId = this.EditQuoteTemplateComponent.QuotePM != null ? this.EditQuoteTemplateComponent.QuotePM.Id : "";

            windowArgs.QuoteTemplateSectionTypeName = "QuoteHeader";
            windowArgs.QuoteTemplateSectionTypeCode = "QH";
            windowArgs.QuoteTemplateTextCodePMList = this.EditQuoteTemplateComponent.QuoteTemplateTextCodePMList;
            windowArgs.QuotePM = this.EditQuoteTemplateComponent.QuotePM;
            var logWindow = new LogitudeWindow();
            logWindow.Title = TextCodeTranslator.Translate("QuoteTemplate.S.QuoteHeader" + "Settings");
            logWindow.Width = 940;
            logWindow.Height = 660;
            logWindow.WindowArgs = windowArgs;

            logWindow.Show("./QuoteModules/QuoteTemplates/Components/QuoteTemplateHeaderDetailsSettingComponent");
            logWindow.WindowClosed.subscribe(($event: any) => {
                if ($event == "Refresh") {
                    quoteTemplateSectionHeaderViewModel.IsLoaded = false;
                    this.IsChangeSetting = true;
                    this.RefreshQuoteTemplateSectionBodyHtml();
                }
            });
        }
    }

    SaveChanges() {

        if (this.QuoteTemplateSettingPM.IsDirty) {
            this.IsChangeSetting = true;
            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));
            this.quoteTemplateSettingPMService.update(this.QuoteTemplateSettingPM).subscribe(res => {
                this.QuoteTemplateSettingPM.IsDirty = false;
                this.CurrentSession.StopBusyIndicator();
                this.RefreshQuoteTemplateSectionBodyHtml();
            });
        }

    }

    RefreshQuoteTemplateSectionBodyHtml() {
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteTemplate.M.Loading"));
        this.quoteTemplateSectionExtendedPMService.DownloadQuoteTemplateSectionPdfFile(this.QuoteTemplateSectionViewModel.QuoteTemplateSectionTypeCode, this.QuoteTemplateSectionViewModel.Id, this.QuoteTemplateSectionViewModel.EntityPM.QuoteTemplateId, this.QuoteTemplateSettingPM.Id, this.QuoteId, this.QuoteTemplatePM.CreatedByUserId, SessionLocator.Tenant).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.StopBusyIndicator();
 
            if (!pmResponse.HasError && pmResponse.Result) {
                this.QuoteTemplateSectionViewModel.HtmlBody = pmResponse.Result;
                this.QuoteTemplateSectionViewModel.IsLoaded = true;
                this.QuoteTemplateSectionViewModel.HtmlBody = pmResponse.Result;
                if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
                    this.froalaEditorSetting.froalaEditorComponent.SetHtml(pmResponse.Result);
                    this.ReloadFroalaEditor();
                }


            }


        });
    }

    public ReloadFroalaEditor() {

        if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
            this.froalaEditorSetting.froalaEditorComponent.SetHtml(this.froalaEditorSetting.froalaEditorComponent.getHtml());
            this.froalaEditorSetting.froalaEditorComponent.ShowEditor();

        }

    }


    
    SaveButtonClicked() {
       this.ValidateFields();
       if (this.ValidationErrorsList.length == 0) {

           if (this.QuoteTemplateSettingPM.IsDirty) {

               this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));
               this.quoteTemplateSettingPMService.update(this.QuoteTemplateSettingPM).subscribe(res => {
                   this.QuoteTemplateSettingPM.IsDirty = false;
                   this.CurrentSession.StopBusyIndicator();
                   this.CurrentSession.CurrentWindow.Close("Refresh");


               });
           }
           else {

               if (this.IsChangeSetting) this.CurrentSession.CurrentWindow.Close("Refresh");
              else this.CurrentSession.CloseCurrentWindow();
           }
            

        }

    }

    ValidateFields() {
        this.ValidationErrorsList = [];
      
      var  widthArea: number = 0;

      if ((this.QuoteTemplateSectionTypeName == "Header" && this.QuoteTemplateSettingPM.PageHeaderArea1Type != "None") || (this.QuoteTemplateSectionTypeName == "Footer" && this.QuoteTemplateSettingPM.PageFooterArea1Type != "None")) {
          widthArea += this.Area1Width;
      }


      if ((this.QuoteTemplateSectionTypeName == "Header" && this.QuoteTemplateSettingPM.PageHeaderArea2Type != "None") || (this.QuoteTemplateSectionTypeName == "Footer" && this.QuoteTemplateSettingPM.PageFooterArea2Type != "None")) {
          widthArea += this.Area2Width;
      }


      if ((this.QuoteTemplateSectionTypeName == "Header" && this.QuoteTemplateSettingPM.PageHeaderArea3Type != "None") || (this.QuoteTemplateSectionTypeName == "Footer" && this.QuoteTemplateSettingPM.PageFooterArea3Type != "None")) {
          widthArea += this.Area3Width;
      }
   
    

      if (widthArea != 100) {

          if (this.QuoteTemplateSectionTypeName == "Header") {
              if (this.QuoteTemplateSettingPM.PageHeaderArea1Type != "None" || this.QuoteTemplateSettingPM.PageHeaderArea2Type != "None" || this.QuoteTemplateSettingPM.PageHeaderArea3Type != "None") {
                  this.ValidationErrorsList.push("width percentages sum have to be 100");
              }
          } else {

              if (this.QuoteTemplateSectionTypeName == "Footer") {
                  if (this.QuoteTemplateSettingPM.PageFooterArea1Type != "None" || this.QuoteTemplateSettingPM.PageFooterArea2Type != "None" || this.QuoteTemplateSettingPM.PageFooterArea3Type != "None") {
                      this.ValidationErrorsList.push("width percentages sum have to be 100");
                  }
              }
          }

        }


        if (this.HeightArea < 1 || this.HeightArea > 7) {
            this.ValidationErrorsList.push("Height should be have value  between 1 cm and 7 cm");
        
        }




   
    }


    CloseButtonClicked() {


        this.CurrentSession.CloseCurrentWindow();
    }
}


