import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {QuoteOPTemplatePM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplatePM';
import {QuoteOPTemplateSettingPM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplateSettingPM';
import {QuoteOPTemplateSectionPM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplateSectionPM';
import {QuoteOPTemplateTextDesignPM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplateTextDesignPM';
import {QuoteOPTemplateTableDesignPM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplateTableDesignPM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {QuoteOPTemplateSettingPMService} from '../../../QuoteOPM/Services/StandardPMs/QuoteOPTemplateSettingPMService';
import {QuoteOPTemplateTableDesignPMService} from '../../../QuoteOPM/Services/StandardPMs/QuoteOPTemplateTableDesignPMService';
import {QuoteOPTemplateTextDesignPMService} from '../../../QuoteOPM/Services/StandardPMs/QuoteOPTemplateTextDesignPMService';
import {QuoteOPTemplateTextDesignExtendedPMService} from '../../../QuoteOPM/Services/ExtendedPMs/QuoteOPTemplateTextDesignExtendedPMService';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {FroalaEditorSetting} from '../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/FroalaEditorSetting';
import {QuoteOPTemplateSectionViewModel} from './EditQuoteTemplateComponent';
import {BorderType} from '../../../Infrastructure/Components/LogitudeCustomComponents/TextDesignComponent';
import {QuoteOPTemplateSectionExtendedPMService} from '../../../QuoteOPM/Services/ExtendedPMs/QuoteOPTemplateSectionExtendedPMService';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
@Component({
    selector: 'QuoteTemplateHeaderFooterSettingComponent',
    
    templateUrl: './QuoteTemplateHeaderFooterSettingComponent.html',
})

export class QuoteTemplateHeaderFooterSettingComponent extends BaseComponent implements OnInit {
    QuoteOPTemplateSettingPMService: QuoteOPTemplateSettingPMService;
    QuoteOPTemplateTextDesignPMService: QuoteOPTemplateTextDesignPMService;
    QuoteOPTemplateTableDesignPMService: QuoteOPTemplateTableDesignPMService;
    QuoteOPTemplateTextDesignExtendedPMService: QuoteOPTemplateTextDesignExtendedPMService;
    QuoteOPTemplateSectionExtendedPMService: QuoteOPTemplateSectionExtendedPMService;
    QuoteOPTemplateSettingPM: QuoteOPTemplateSettingPM;
    QuoteOPTemplatePM: QuoteOPTemplatePM;
    IsLoadPage: boolean;
    Alignment: string[] = [];
    QuoteOPTemplateSectionViewModel: QuoteOPTemplateSectionViewModel;
    froalaEditorSetting: FroalaEditorSetting;
    public ValidationErrorsList: string[];
    QuoteOPTemplateSectionTypeName: string = "Packages";
    EditQuoteTemplateComponent: any;
    @ViewChild('Child', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;



    QuoteId: string;
    BorderTypesSelected: BorderType;
    BorderTypes: BorderType[] = [];
    SelectedTabCode: string;
    DesignTextDesignPM: QuoteOPTemplateTextDesignPM;

    AreaType: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.QuoteOPTemplateSettingPMService = new QuoteOPTemplateSettingPMService();
        this.QuoteOPTemplateTableDesignPMService = new QuoteOPTemplateTableDesignPMService();
        this.QuoteOPTemplateTextDesignPMService = new QuoteOPTemplateTextDesignPMService();
        this.QuoteOPTemplateTextDesignExtendedPMService = new QuoteOPTemplateTextDesignExtendedPMService();
        this.QuoteOPTemplateSectionExtendedPMService = new QuoteOPTemplateSectionExtendedPMService();
        
      
    }

    ngOnInit() {

    }

    
    SetWindowArgs(args: any) {
        this.SelectedTabCode = "QCS";
        this.QuoteOPTemplatePM = args.QuoteOPTemplatePM;
        this.QuoteOPTemplateSectionTypeName = args.QuoteOPTemplateSectionTypeName
        this.QuoteOPTemplateSettingPM = args.QuoteOPTemplateSettingPM;
        this.QuoteOPTemplateSectionViewModel = args.QuoteOPTemplateSectionViewModel;  
        this.EditQuoteTemplateComponent = args.EditQuoteTemplateComponent;  
        this.GetQuoteOPTemplateTextDesign();


          
        this.QuoteOPId = args.QuoteOPId;  
        var areaTypeString = "Logo,Text";
        if (this.QuoteOPTemplateSectionTypeName == "Header") areaTypeString += ",Quote Header";
        areaTypeString += ",None";

        this.AreaType = areaTypeString.split(',');

        this.froalaEditorSetting = new FroalaEditorSetting();
        this.froalaEditorSetting.Id = Guid.newGuid();
        this.froalaEditorSetting.IsDisableEdit = true;
        this.froalaEditorSetting.UseNormalPreview = true;
        if (this.QuoteOPTemplateSectionViewModel) {
            this.RefreshQuoteOPTemplateSectionBodyHtml();
            //if (this.QuoteOPTemplateSectionViewModel.IsLoaded) {
            //    this.froalaEditorSetting.HtmlString = this.QuoteOPTemplateSectionViewModel.HtmlBody;
            //}
            //else {

            //    this.RefreshQuoteOPTemplateSectionBodyHtml();
            //}
        }
        
        this.froalaEditorSetting.Height = ((this.CurrentSession.CurrentWindow.Height / 2) -20);


        this.FullProperity();
        this.FullBorderTypesLists();
    }

    GetQuoteOPTemplateTextDesign() {
        var pageNumberingTextDesignId: string = this.QuoteOPTemplateSettingPM.PageNumberingTextDesignId;
        this.QuoteOPTemplateTextDesignPMService.get(pageNumberingTextDesignId).subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError && pmResponse.Result) {
                this.DesignTextDesignPM = pmResponse.Result;
                if (this.DesignTextDesignPM) {
                    this.DesignTextDesignPM.Title = "Design";
                    this.DesignTextDesignPM.SampleText = "Page 1 of 2";
                    this.IsLoadPage = true;
                }
            }
            else if (!pmResponse.HasError) {
                this.CreateNewQuoteOPTemplateTextDesign();
            }
        });     
    }

    CreateNewQuoteOPTemplateTextDesign() {
        var t = new QuoteOPTemplateTextDesignPM();
        t.Tenant = SessionLocator.Tenant,
            t.FontSize = 7,
            t.TextColor = "#FF000000",
            t.FontFamily = "Times New Roman",
            t.BackgroundColor = "#FFFFFFFF",
            t.FontWeight = "normal",
            t.Italic = false,
            t.UnDerLine = false,
            t.Alignment = "right";
        this.QuoteOPTemplateTextDesignPMService.insert(t).subscribe((response: any) => {
            if (!response.HasError && response.Result) {
                this.DesignTextDesignPM = response.Result;
                this.QuoteOPTemplateSettingPM.PageNumberingTextDesignId = response.Result.Id;
                this.QuoteOPTemplateSettingPMService.update(this.QuoteOPTemplateSettingPM).subscribe((res: any) => {
                    if (!res.HasError) {
                        this.DesignTextDesignPM.Title = "Design";
                        this.DesignTextDesignPM.SampleText = "Page 1 of 2";
                        this.IsLoadPage = true;
                    }
                });
            }
        });
    }

    //Full Data

    FullProperity() {

        if (this.QuoteOPTemplateSettingPM) {
            this.HeightArea = this.QuoteOPTemplateSectionTypeName == "Header" ? this.QuoteOPTemplateSettingPM.PageHeaderAreaHeight : this.QuoteOPTemplateSettingPM.PageFooterAreaHeight;
        }
    }
    FullBorderTypesLists() {


        this.BorderTypes = [];
        this.BorderTypes.push(new BorderType("None", "NONE"));
        this.BorderTypes.push(new BorderType("All", "ALL"));
        this.BorderTypes.push(new BorderType("Box", "BOX"));
        this.BorderTypes.push(new BorderType("Horizontal Only", "HORIZONTALLINES"));
        this.BorderTypes.push(new BorderType("Vertical Only", "VERTICALLINES"));

        var selectedBorderCode =  this.QuoteOPTemplateSectionTypeName == "Header" ? this.QuoteOPTemplateSettingPM.PageHeaderBorderTypeCode : this.QuoteOPTemplateSettingPM.PageFooterBorderTypeCode;


        this.BorderTypesSelected = this.BorderTypes.filter(d => d.Code == selectedBorderCode)[0];

    }
    

    // Prop

    private area1Width = 0;
    get Area1Width() {
        if (this.QuoteOPTemplateSectionTypeName == "Header") {
            this.area1Width = this.QuoteOPTemplateSettingPM.PageHeaderArea1Width;
        } else this.area1Width = this.QuoteOPTemplateSettingPM.PageFooterArea1Width;

        return this.area1Width;
    }
    set Area1Width(newValue: number) {
        if (newValue != this.area1Width) {
            this.area1Width = newValue;
            if (this.QuoteOPTemplateSectionTypeName == "Header") {
                this.QuoteOPTemplateSettingPM.PageHeaderArea1Width = newValue;
            } else this.QuoteOPTemplateSettingPM.PageFooterArea1Width = newValue;
            this.SaveAreaWdith();
        }
    }

    private area2Width = 0;
    get Area2Width() {
        if (this.QuoteOPTemplateSectionTypeName == "Header") {
            this.area2Width = this.QuoteOPTemplateSettingPM.PageHeaderArea2Width;
        } else this.area2Width = this.QuoteOPTemplateSettingPM.PageFooterArea2Width;

        return this.area2Width;
    }
    set Area2Width(newValue: number) {
        if (newValue != this.area2Width) {
            this.area2Width = newValue;
            if (this.QuoteOPTemplateSectionTypeName == "Header") {
                this.QuoteOPTemplateSettingPM.PageHeaderArea2Width = newValue;
            } else this.QuoteOPTemplateSettingPM.PageFooterArea2Width = newValue;
            this.SaveAreaWdith();
        }
    }

    private area3Width = 0;
    get Area3Width() {
        if (this.QuoteOPTemplateSectionTypeName == "Header") {
            this.area3Width = this.QuoteOPTemplateSettingPM.PageHeaderArea3Width;
        } else this.area3Width = this.QuoteOPTemplateSettingPM.PageFooterArea3Width;

        return this.area3Width;
    }
    set Area3Width(newValue: number) {
        if (newValue != this.area3Width) {
            if (this.QuoteOPTemplateSectionTypeName == "Header") {
                this.QuoteOPTemplateSettingPM.PageHeaderArea3Width = newValue;
            } else this.QuoteOPTemplateSettingPM.PageFooterArea3Width = newValue;
           this.SaveAreaWdith();
        }

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
        if (this.QuoteOPTemplateSettingPM) pageArea1Type = this.QuoteOPTemplateSectionTypeName == "Header" ? this.QuoteOPTemplateSettingPM.PageHeaderArea1Type : this.QuoteOPTemplateSettingPM.PageFooterArea1Type;
        return pageArea1Type;
    }
    get PageArea2Type() {
        var pageArea2Type: string = "";
        if (this.QuoteOPTemplateSettingPM) pageArea2Type = this.QuoteOPTemplateSectionTypeName == "Header" ? this.QuoteOPTemplateSettingPM.PageHeaderArea2Type : this.QuoteOPTemplateSettingPM.PageFooterArea2Type;
        return pageArea2Type;
    }
    get PageArea3Type() {
        var pageArea3Type: string = "";
        if (this.QuoteOPTemplateSettingPM) pageArea3Type = this.QuoteOPTemplateSectionTypeName == "Header" ? this.QuoteOPTemplateSettingPM.PageHeaderArea3Type : this.QuoteOPTemplateSettingPM.PageFooterArea3Type;
        return pageArea3Type;
    }




    //Events

    SaveAreaWdith() {

        this.ValidateFields();
        if (this.ValidationErrorsList.length == 0) {
            this.SaveChanges();
        }

    }

    HeightAreaLostFocusMethod() {
        var oldValue = this.QuoteOPTemplateSectionTypeName == "Header" ? this.QuoteOPTemplateSettingPM.PageHeaderAreaHeight : this.QuoteOPTemplateSettingPM.PageFooterAreaHeight;
        if (oldValue != this.HeightArea) {
            if (this.QuoteOPTemplateSectionTypeName == "Header") {
                this.QuoteOPTemplateSettingPM.PageHeaderAreaHeight = this.HeightArea;
            } else this.QuoteOPTemplateSettingPM.PageFooterAreaHeight = this.HeightArea;
            this.SaveChanges();
        }


    }

    PageAreaSelectedChanged(area: string, value: string) {



        if (area == "Area1") {
            var oldValue = this.QuoteOPTemplateSectionTypeName == "Header" ? this.QuoteOPTemplateSettingPM.PageHeaderArea1Type : this.QuoteOPTemplateSettingPM.PageFooterArea1Type;
            if (oldValue != value) {
                if (this.QuoteOPTemplateSectionTypeName == "Header") {
                    this.QuoteOPTemplateSettingPM.PageHeaderArea1Type = value;
                } else this.QuoteOPTemplateSettingPM.PageFooterArea1Type = value;

                if (value == "None" && this.Area1Width!=0) {
                    this.Area1Width = 0;
                } else this.SaveChanges();

               
            }
       


        }
        else if (area == "Area2") {
            var oldValue = this.QuoteOPTemplateSectionTypeName == "Header" ? this.QuoteOPTemplateSettingPM.PageHeaderArea2Type : this.QuoteOPTemplateSettingPM.PageFooterArea2Type;
            if (oldValue != value) {
                if (this.QuoteOPTemplateSectionTypeName == "Header") {
                    this.QuoteOPTemplateSettingPM.PageHeaderArea2Type = value;
                } else this.QuoteOPTemplateSettingPM.PageFooterArea2Type = value;

                if (value == "None" && this.Area2Width != 0) {
                    this.Area2Width = 0;
                } else this.SaveChanges();

            }

        } else if (area == "Area3") {
            var oldValue = this.QuoteOPTemplateSectionTypeName == "Header" ? this.QuoteOPTemplateSettingPM.PageHeaderArea3Type : this.QuoteOPTemplateSettingPM.PageFooterArea3Type;
            if (oldValue != value) {
                if (this.QuoteOPTemplateSectionTypeName == "Header") {
                    this.QuoteOPTemplateSettingPM.PageHeaderArea3Type = value;
                } else this.QuoteOPTemplateSettingPM.PageFooterArea3Type = value;

                if (value == "None" && this.Area3Width != 0) {
                    this.Area3Width = 0;
                } else this.SaveChanges();
            }
            
        }



    }

    BorderTypesSelectedChanged(border: BorderType) {
        if (this.QuoteOPTemplateSettingPM) {

            if (this.QuoteOPTemplateSectionTypeName == "Header") {
                if (this.QuoteOPTemplateSettingPM.PageHeaderBorderTypeCode != border.Code) {
                    this.QuoteOPTemplateSettingPM.PageHeaderBorderTypeCode = border.Code;
                    this.SaveChanges();
                }
            } else {

                if (this.QuoteOPTemplateSettingPM.PageFooterBorderTypeCode != border.Code) {
                    this.QuoteOPTemplateSettingPM.PageFooterBorderTypeCode = border.Code;
                    this.SaveChanges();
                }

            }
        }
    }



    //Command


    AdvanceButtonClicked() {
 

            var windowArgs: any = {};
            var logWindow = new LogitudeWindow();
            windowArgs.QuoteOPTemplateSettingPM = this.QuoteOPTemplateSettingPM;
            windowArgs.QuoteOPTemplateSectionTypeName = this.QuoteOPTemplateSectionTypeName;
            logWindow.WindowArgs = windowArgs;
            logWindow.Width = 400;
            logWindow.Height = 130;
            logWindow.Title = TextCodeTranslator.Translate("QuoteOPTemplate.S.DesignTable");
            logWindow.Show("./QuoteModules/QuoteTemplates/Components/AdvanceDesignTableComponent");
            logWindow.WindowClosed.subscribe(($event: any) => {
                if ($event == "Refresh") {
                    this.IsChangeSetting = true;
                    this.RefreshQuoteOPTemplateSectionBodyHtml();
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
            windowArgs.QuoteOPTemplateSettingPM = this.QuoteOPTemplateSettingPM;
            windowArgs.QuoteOPTemplateSectionTypeName = this.QuoteOPTemplateSectionTypeName;
            windowArgs.AreaType = type;

            if (type == "Area1") windowArgs.AreaMode = this.PageArea1Type;
            else if (type == "Area2") windowArgs.AreaMode = this.PageArea2Type;
            else if (type == "Area3") windowArgs.AreaMode = this.PageArea3Type;

            logWindow.WindowArgs = windowArgs;
            logWindow.Width = 800;
            logWindow.Height = 600;
            logWindow.BottomBorderForTitle = "1px solid LightGray";
            logWindow.Title = TextCodeTranslator.Translate("QuoteOPTemplate.S.Edit" + type);
            logWindow.Show("./QuoteModules/QuoteTemplates/Components/PageAreaHeaderFooterComponent");
            logWindow.WindowClosed.subscribe(($event: any) => {
                if ($event == "Refresh") {
                    this.IsChangeSetting = true;
                    this.RefreshQuoteOPTemplateSectionBodyHtml();
                }
            });
        }
    }


    ShowQuoteHeaderEditWindow() {
        if (this.EditQuoteTemplateComponent) {
            var windowArgs: any = {};
            windowArgs.QuoteOPTemplatePM = this.QuoteOPTemplatePM;
            windowArgs.QuoteOPTemplateSettingPM = this.QuoteOPTemplateSettingPM;
            var QuoteOPTemplateSectionHeaderViewModel = this.EditQuoteTemplateComponent.QuoteOPTemplateSectionLists.filter(d => d.QuoteOPTemplateSectionTypeCode == "QH")[0];
            windowArgs.QuoteOPTemplateSectionViewModel = QuoteOPTemplateSectionHeaderViewModel;
            windowArgs.QuoteOPId = this.EditQuoteTemplateComponent.QuoteOPPM != null ? this.EditQuoteTemplateComponent.QuoteOPPM.Id : "";
            windowArgs.QuoteOPTemplateSectionTypeName = "QuoteHeader";
            windowArgs.QuoteOPTemplateSectionTypeCode = "QH";
            windowArgs.QuoteOPTemplateTextCodePMList = this.EditQuoteTemplateComponent.QuoteOPTemplateTextCodeLists;
            windowArgs.QuoteOPPM = this.EditQuoteTemplateComponent.QuoteOPPM;
            var logWindow = new LogitudeWindow();
            logWindow.Title = TextCodeTranslator.Translate("QuoteOPTemplate.S.QuoteHeader" + "Settings");
            logWindow.Width = 940;
            logWindow.Height = 660;
            logWindow.WindowArgs = windowArgs;

            logWindow.Show("./QuoteModules/QuoteTemplates/Components/QuoteOPTemplateHeaderDetailsSettingComponent");
            logWindow.WindowClosed.subscribe(($event: any) => {
                if ($event == "Refresh") {
                    QuoteOPTemplateSectionHeaderViewModel.IsLoaded = false;
                    this.IsChangeSetting = true;
                    this.RefreshQuoteOPTemplateSectionBodyHtml();
                }
            });
        }
    }

    SaveChanges() {

        if (this.QuoteOPTemplateSettingPM.IsDirty) {
            this.IsChangeSetting = true;
            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteOPTemplate.M.Saving"));
            this.QuoteOPTemplateSettingPMService.update(this.QuoteOPTemplateSettingPM).subscribe((res:any) => {
                this.QuoteOPTemplateSettingPM.IsDirty = false;
                this.CurrentSession.StopBusyIndicator();
                this.RefreshQuoteOPTemplateSectionBodyHtml();
            });
        }

    }

    RefreshQuoteOPTemplateSectionBodyHtml() {
        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteOPTemplate.M.Loading"));
        this.QuoteOPTemplateSectionExtendedPMService.DownloadQuoteOPTemplateSectionPdfFile(this.QuoteOPTemplateSectionViewModel.QuoteOPTemplateSectionTypeCode, this.QuoteOPTemplateSectionViewModel.Id, this.QuoteOPTemplateSectionViewModel.EntityPM.QuoteTemplateId, this.QuoteOPTemplateSettingPM.Id, this.QuoteOPId, this.QuoteOPTemplatePM.CreatedByUserId, SessionLocator.Tenant).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.StopBusyIndicator();
 
            if (!pmResponse.HasError && pmResponse.Result) {
                this.QuoteOPTemplateSectionViewModel.HtmlBody = pmResponse.Result;
                this.QuoteOPTemplateSectionViewModel.IsLoaded = true;
                this.QuoteOPTemplateSectionViewModel.HtmlBody = pmResponse.Result;
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


    get SpaceLinesBefore() {
        var spaceLinesBefore: number = 1;
        if (this.QuoteOPTemplateSettingPM) {
            if (this.QuoteOPTemplateSectionTypeName == "Header") spaceLinesBefore = this.QuoteOPTemplateSettingPM.SpaceLinesBeforeHeaders;
            else if (this.QuoteOPTemplateSectionTypeName == "Footer") spaceLinesBefore = this.QuoteOPTemplateSettingPM.SpaceLinesBeforeFooters;
           
        }
        return spaceLinesBefore;
    }
    set SpaceLinesBefore(value: number) {
        if (this.QuoteOPTemplateSettingPM) {

            if (this.QuoteOPTemplateSectionTypeName == "Header") {
                if (this.QuoteOPTemplateSettingPM.SpaceLinesBeforeHeaders != value) {
                    this.QuoteOPTemplateSettingPM.SpaceLinesBeforeHeaders = value;
                    this.SaveChanges();
                }
            }
            else if (this.QuoteOPTemplateSectionTypeName == "Footer") {
                if (this.QuoteOPTemplateSettingPM.SpaceLinesBeforeFooters != value) {
                    this.QuoteOPTemplateSettingPM.SpaceLinesBeforeFooters = value;
                    this.SaveChanges();
                }
            }
           
        }

    }
    //SpaceLinesBefore

    get HidePageNumber() {
        var hidePageNumber: boolean = false;
        if (this.QuoteOPTemplateSettingPM) {
            hidePageNumber = this.QuoteOPTemplateSettingPM.HidePageNumber;
        }
        return hidePageNumber;
    }
    set HidePageNumber(value: boolean) {
        if (this.QuoteOPTemplateSettingPM != null) {
            this.QuoteOPTemplateSettingPM.HidePageNumber = value;
        }
    }

    SaveButtonClicked() {
       this.ValidateFields();
        if (this.ValidationErrorsList.length == 0) {
            this.SaveQuoteOPTemplateTextDesign();
            if (this.QuoteOPTemplateSettingPM.IsDirty) {

                this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteOPTemplate.M.Saving"));
                this.QuoteOPTemplateSettingPMService.update(this.QuoteOPTemplateSettingPM).subscribe((res:any) => {
                    this.QuoteOPTemplateSettingPM.IsDirty = false;
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

    SaveQuoteOPTemplateTextDesign() {
        if (this.DesignTextDesignPM.IsDirty) {
            this.QuoteOPTemplateTextDesignPMService.update(this.DesignTextDesignPM).subscribe((res: any) => { });
        }
    }

    ValidateFields() {
        this.ValidationErrorsList = [];
      
      var  widthArea: number = 0;

      if ((this.QuoteOPTemplateSectionTypeName == "Header" && this.QuoteOPTemplateSettingPM.PageHeaderArea1Type != "None") || (this.QuoteOPTemplateSectionTypeName == "Footer" && this.QuoteOPTemplateSettingPM.PageFooterArea1Type != "None")) {
          widthArea += this.Area1Width;
      }


      if ((this.QuoteOPTemplateSectionTypeName == "Header" && this.QuoteOPTemplateSettingPM.PageHeaderArea2Type != "None") || (this.QuoteOPTemplateSectionTypeName == "Footer" && this.QuoteOPTemplateSettingPM.PageFooterArea2Type != "None")) {
          widthArea += this.Area2Width;
      }


      if ((this.QuoteOPTemplateSectionTypeName == "Header" && this.QuoteOPTemplateSettingPM.PageHeaderArea3Type != "None") || (this.QuoteOPTemplateSectionTypeName == "Footer" && this.QuoteOPTemplateSettingPM.PageFooterArea3Type != "None")) {
          widthArea += this.Area3Width;
      }
   
    

      if (widthArea != 100) {

          if (this.QuoteOPTemplateSectionTypeName == "Header") {
              if (this.QuoteOPTemplateSettingPM.PageHeaderArea1Type != "None" || this.QuoteOPTemplateSettingPM.PageHeaderArea2Type != "None" || this.QuoteOPTemplateSettingPM.PageHeaderArea3Type != "None") {
                  this.ValidationErrorsList.push("width percentages sum have to be 100");
              }
          } else {

              if (this.QuoteOPTemplateSectionTypeName == "Footer") {
                  if (this.QuoteOPTemplateSettingPM.PageFooterArea1Type != "None" || this.QuoteOPTemplateSettingPM.PageFooterArea2Type != "None" || this.QuoteOPTemplateSettingPM.PageFooterArea3Type != "None") {
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


