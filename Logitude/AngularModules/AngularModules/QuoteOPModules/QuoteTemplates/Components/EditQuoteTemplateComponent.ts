

import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {QuoteOPTemplatePM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplatePM';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {QuoteOPTemplateExtendedPMService} from '../../../QuoteOPM/Services/ExtendedPMs/QuoteOPTemplateExtendedPMService';
import {QuotationComponent} from '../../QuoteOthers/Components/Quotation/QuotationComponent';
import {QuoteOPTemplateSectionPMService} from '../../../QuoteOPM/Services/StandardPMs/QuoteOPTemplateSectionPMService';
import {QuoteOPTemplatePMService} from '../../../QuoteOPM/Services/StandardPMs/QuoteOPTemplatePMService';
import {QuoteOPTemplateSettingPM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplateSettingPM';
import {QuoteOPTemplateSettingPMService} from '../../../QuoteOPM/Services/StandardPMs/QuoteOPTemplateSettingPMService';
import {QuoteOPTemplateTextCodeExtendedPMService} from '../../../QuoteOPM/Services/ExtendedPMs/QuoteOPTemplateTextCodeExtendedPMService';
import {QuoteOPTemplateSectionExtendedPMService} from '../../../QuoteOPM/Services/ExtendedPMs/QuoteOPTemplateSectionExtendedPMService';
import {FroalaEditorSetting} from '../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/FroalaEditorSetting';
import {QuoteOPTemplateTextCodePM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplateTextCodePM';
import {QuoteOPTemplateSectionPM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplateSectionPM';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
@Component({
    selector: 'EditQuoteTemplateComponent',
    
    templateUrl: './EditQuoteTemplateComponent.html',
})

export class EditQuoteTemplateComponent extends BaseComponent implements OnInit {
    public DataContext: EditQuoteTemplateComponent = this;
    EntityPM: QuoteOPTemplatePM;
    IsNewEntityCall: boolean = true;
    public ValidationErrorsList: string[];
    CurrentEntity: any;
    QuoteOPTemplateSectionLists: QuoteOPTemplateSectionViewModel[] = [];
    AllQuoteOPTemplateSectionLists: QuoteOPTemplateSectionViewModel[] = [];
    QuoteOPTemplateTextCodeLists: QuoteOPTemplateTextCodePM[] = [];
    IsLoadQuoteOPTemplateSectionRuning: boolean = false;
    IsLoadQuoteOPTemplateTextCodeRuning: boolean = false;
    IsLoadQuoteOPTemplateSettingsRuning: boolean = false;

    IsSaveQuoteOPTemplateSectionRuning: boolean = false;
    IsSaveQuoteOPTemplateRuning: boolean = false;
    IsSaveQuoteOPTemplateSettingsRuning: boolean = false;
    ShowLocalLanguageCheckBoxKey: string;
    ShowRightToLeftCheckBoxKey: string;
    IsAddSectionRunning: boolean = false;   

    QuoteOPTemplateSettingPM: QuoteOPTemplateSettingPM;
    
    QuoteOPTemplateSectionPMService: QuoteOPTemplateSectionPMService;
    VisibilityGeneralSetting: boolean = true;
    IsLoadPreviewSectionRuning: boolean;
    QuoteOPTemplateId: string;
    QuoteOPPM: any;
    QuoteOPTemplateExtendedPMService: QuoteOPTemplateExtendedPMService;
    QuoteOPTemplateTextCodeExtendedPMService: QuoteOPTemplateTextCodeExtendedPMService;
    QuoteOPTemplateSectionExtendedPMService: QuoteOPTemplateSectionExtendedPMService;
    QuoteOPTemplateSettingPMService: QuoteOPTemplateSettingPMService;
    IsShowFroalaEditor: boolean = false;
    froalaEditorSetting: FroalaEditorSetting;
    QuoteOPTemplatePMService: QuoteOPTemplatePMService;
    @ViewChild('Child', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;

    IsDisableEditButton: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
      
        if (!FeatureLocator.HasFeaturePermession("QuoteOPTemplate", "UPDATE")) this.IsDisableEditButton = true;

        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteOPTemplate.M.Loading"));
        this.QuoteOPTemplateExtendedPMService = new QuoteOPTemplateExtendedPMService();
        this.QuoteOPTemplateTextCodeExtendedPMService = new QuoteOPTemplateTextCodeExtendedPMService();
        this.QuoteOPTemplateSectionExtendedPMService = new QuoteOPTemplateSectionExtendedPMService();
        this.QuoteOPTemplateSettingPMService = new QuoteOPTemplateSettingPMService();
        this.QuoteOPTemplateSectionPMService = new QuoteOPTemplateSectionPMService();
        
        this.QuoteOPTemplatePMService = new QuoteOPTemplatePMService();

    }

    ngOnInit() {
        this.ShowLocalLanguageCheckBoxKey = Guid.newGuid();
        this.ShowRightToLeftCheckBoxKey = Guid.newGuid();

    }

    IsReady: boolean = false;
    SetWindowArgs(args: any) {

        var _entityResourceService: EntityResourceService = new EntityResourceService();
        _entityResourceService.getEntityResourceByTableName("QuoteOPTemplate").subscribe((response:any) => {
            this.IsReady = true;
            this.Load(args);
        });


       
    }

    Load(args: any) {
        this.froalaEditorSetting = new FroalaEditorSetting();
        this.froalaEditorSetting.Id = Guid.newGuid();
        this.froalaEditorSetting.UseNormalPreview = true;
        this.froalaEditorSetting.IsDisableEdit = true;
        this.froalaEditorSetting.HtmlString = "";
        this.froalaEditorSetting.Height = (this.CurrentSession.CurrentWindow.Height - 100);
        this.IsShowFroalaEditor = true;

        this.IsNewEntityCall = args.IsNewEntityCall;
        this.EntityPM = args.EntityPM;
        this.QuoteOPPM = args.QuoteOPPM;
        this.CurrentEntity = args.CurrentEntity;
        this.QuoteTemplateId = args.QuoteTemplateId;

        if (this.EntityPM) {
            this.QuoteTemplateId = this.EntityPM.Id;
            this.LoadData();
        }
        else {
            if (this.CurrentEntity) this.QuoteTemplateId = this.CurrentEntity.Id;
            if (!AppTool.IsNullOrEmpty(this.QuoteTemplateId)) this.LoadQuoteOPTemplatePM();
            else this.LoadCompleted();
        }

        if (this.QuoteOPPM != null) {
            this.VisibilityGeneralSetting = false;
        }

    }


    private selectQuoteOPTemplateSection: QuoteOPTemplateSectionViewModel;
    get SelectQuoteOPTemplateSection() { return this.selectQuoteOPTemplateSection; }
    set SelectQuoteOPTemplateSection(newValue: QuoteOPTemplateSectionViewModel) {

        if (this.selectQuoteOPTemplateSection != newValue || !newValue.IsLoaded) {
            this.selectQuoteOPTemplateSection = newValue;
            this.QuoteOPTemplateSectionLists.forEach((item) => {
                if (item.Id != this.selectQuoteOPTemplateSection.Id) {
                    item.Background = "white";
                    item.IsShowArrowUpDown = false;
                }
            });
            this.selectQuoteOPTemplateSection.Background = "#96D3F0";
            if (this.selectQuoteOPTemplateSection.QuoteOPTemplateSectionTypeCode != "PF" && this.selectQuoteOPTemplateSection.QuoteOPTemplateSectionTypeCode != "PH" && this.selectQuoteOPTemplateSection.QuoteOPTemplateSectionTypeCode != "QH" && this.selectQuoteOPTemplateSection.QuoteOPTemplateSectionTypeCode != "QD") {
                this.selectQuoteOPTemplateSection.IsShowArrowUpDown = true;
            }
           

            this.LoadSectionPreviewData();
        }



    }



    get RightToLeft() {
        var rightToLeft: boolean = false;
        if (this.QuoteOPTemplateSettingPM) rightToLeft = this.QuoteOPTemplateSettingPM.RightToLeft;
        return rightToLeft;
    }
    set RightToLeft(value: boolean) {
        if (this.QuoteOPTemplateSettingPM != null) {
            if (this.QuoteOPTemplateSettingPM.RightToLeft != value) {
                this.QuoteOPTemplateSettingPM.RightToLeft = value;
                this.IsSaveQuoteOPTemplateSettingsRuning = true;
                this.SaveQuoteOPTemplateSetting();
            }
        }
    }

    get ShowLocalLanguage() {
        var showLocalLanguage: boolean = false;
        if (this.QuoteOPTemplateSettingPM) showLocalLanguage = this.QuoteOPTemplateSettingPM.ShowLocalLanguage;
        return showLocalLanguage;
    }
    set ShowLocalLanguage(value: boolean) {
        if (this.QuoteOPTemplateSettingPM != null) {
            if (this.QuoteOPTemplateSettingPM.ShowLocalLanguage != value) {
                this.QuoteOPTemplateSettingPM.ShowLocalLanguage = value;
                this.IsSaveQuoteOPTemplateSettingsRuning = true;
                this.SaveQuoteOPTemplateSetting();
            }
        }
    }



 
    GeneralSettingsButtonClicked() {

        var windowArgs: any = {};
        var logWindow = new LogitudeWindow();
        windowArgs.QuoteOPTemplatePM = this.EntityPM;
        windowArgs.QuoteOPTemplateSettingPM = this.QuoteOPTemplateSettingPM;


        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 620;
        logWindow.Height = 450;
        logWindow.Title = TextCodeTranslator.Translate("QuoteOPTemplate.S.General"); 
        logWindow.Show("./QuoteModules/QuoteTemplates/Components/QuoteOPTemplateGeneralSetting");

   
    }


    PreviewPdf() {
        var windowArgs: any = {};
        var logWindow = new LogitudeWindow();
        windowArgs.QuoteTemplateId = this.EntityPM.Id;
        windowArgs.QuoteOPId = this.QuoteOPPM != null ? this.QuoteOPPM.Id : "";
        logWindow.Title = TextCodeTranslator.Translate("QuoteOPTemplate.S.PreviewTemplate");
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 1000;
        logWindow.Height = (window.innerHeight - 130);
        logWindow.Show("./QuoteModules/QuoteTemplates/Components/PreviewQuoteTemplateReportComponent");
    }

    PreviewButtonClicked() {

        this.SaveDirtySections(true);

    }

    //Event Area
    EditSectionButtonClicked(item: QuoteOPTemplateSectionViewModel) {

        if (item.IsSection) {
            this.AddEditSection(item);
        }
        else {
            var componentPath = "";
            var logWindow = new LogitudeWindow();
            var windowArgs: any = {};
            windowArgs.QuoteOPTemplatePM = this.EntityPM;
            windowArgs.QuoteOPTemplateSettingPM = this.QuoteOPTemplateSettingPM;
            windowArgs.QuoteOPTemplateSectionViewModel = item;
            windowArgs.QuoteOPId = this.QuoteOPPM != null ? this.QuoteOPPM.Id : "";
            windowArgs.QuoteOPPM = this.QuoteOPPM;
            //Pricing Setting
            if (item.QuoteOPTemplateSectionTypeCode == "PP" || item.QuoteOPTemplateSectionTypeCode == "PC") {
                windowArgs.QuoteOPTemplateSectionTypeName = item.QuoteOPTemplateSectionTypeCode == "PP" ? "Packages" : "Containers";
                logWindow.Title = TextCodeTranslator.Translate("QuoteOPTemplate.S.Pricing" + windowArgs.QuoteOPTemplateSectionTypeName.replace(" ", "") + "Settings");
                windowArgs.QuoteOPTemplateTextCodePMList = this.QuoteOPTemplateTextCodeLists;
                componentPath = "./QuoteModules/QuoteTemplates/Components/QuoteOPTemplatePricingSettingComponent";
                logWindow.Width = 950;
                logWindow.Height = 660;
              
            }
      
            //Page Header && Footer  Setting
            else if (item.QuoteOPTemplateSectionTypeCode == "PH" || item.QuoteOPTemplateSectionTypeCode == "PF") {
                windowArgs.QuoteOPTemplateSectionTypeName = item.QuoteOPTemplateSectionTypeCode == "PH" ? "Header" : "Footer";
                windowArgs.EditQuoteTemplateComponent = this;
                
                componentPath = "./QuoteModules/QuoteTemplates/Components/QuoteOPTemplateHeaderFooterSettingComponent";
                logWindow.Width = (window.innerWidth / 1.476); // 1920/1300
                logWindow.Height = 600;
  
                logWindow.Title = TextCodeTranslator.Translate("QuoteOPTemplate.S.Page" + windowArgs.QuoteOPTemplateSectionTypeName.replace(" ", "") + "Settings");
            }

            else if (item.QuoteOPTemplateSectionTypeCode == "QH" || item.QuoteOPTemplateSectionTypeCode == "QD") {
                windowArgs.QuoteOPTemplateSectionTypeName = item.QuoteOPTemplateSectionTypeCode == "QH" ? "QuoteHeader" : "QuoteDetails";
                logWindow.Title = TextCodeTranslator.Translate("QuoteOPTemplate.S." + windowArgs.QuoteOPTemplateSectionTypeName.replace(" ", "") + "Settings");
                windowArgs.QuoteOPTemplateSectionTypeCode = item.QuoteOPTemplateSectionTypeCode;
                windowArgs.QuoteOPTemplateTextCodePMList = this.QuoteOPTemplateTextCodeLists;
                windowArgs.QuoteOPPM = this.QuoteOPPM;
                
                componentPath = "./QuoteModules/QuoteTemplates/Components/QuoteOPTemplateHeaderDetailsSettingComponent";
                logWindow.Width = 940;
                logWindow.Height = 660;
        
            }


            if (!AppTool.IsNullOrEmpty(componentPath)) {
                logWindow.WindowArgs = windowArgs;
                logWindow.Show(componentPath);
                logWindow.WindowClosed.subscribe(($event: any) => {
                    if ($event == "Refresh") {

                        if (windowArgs.QuoteOPTemplateSectionTypeName == "Header" || windowArgs.QuoteOPTemplateSectionTypeName == "Footer") {
                            if (this.SelectQuoteOPTemplateSection != item) {
                                this.SelectQuoteOPTemplateSection = item;
                            }
                            else {
                                this.LoadSectionPreviewData();
                            }

                        }
                        else {

                            if (item.QuoteOPTemplateSectionTypeCode == "QH") {
                                var QuoteOPTemplateSectionHeaderViewModel = this.QuoteOPTemplateSectionLists.filter(d => d.QuoteOPTemplateSectionTypeCode == "PH")[0];
                                if (QuoteOPTemplateSectionHeaderViewModel) {
                                    QuoteOPTemplateSectionHeaderViewModel.IsLoaded = false;
                                }
                            }
                      

                            item.IsLoaded = false;
                            this.SelectQuoteOPTemplateSection = item;
                        }

                    }
                });
                //  Refresh
            }
        }

    }

    AddEditSection(item: QuoteOPTemplateSectionViewModel) {

        this.IsAddSectionRunning = true;
        var widthwindow = window.innerWidth;
        var heighthwindow = window.innerHeight;
        var percentagewidthwindow = widthwindow * 0.252;
        var percentageHeightwindow = heighthwindow * 0.1764705;
        var sendWindowHeight = heighthwindow - percentageHeightwindow;
        var sendWindowWidth = widthwindow - percentagewidthwindow;
        if (sendWindowWidth < 1080) sendWindowWidth = 1080;
        if (sendWindowHeight < 500) sendWindowHeight = 500;

        var windowArgs: any = {};
        windowArgs.FatherComponent = this;
        windowArgs.HeightWindow = sendWindowHeight;
        windowArgs.QuoteOPTemplateSectionViewModel = item;
        windowArgs.QuoteTemplateId = this.EntityPM.Id;

        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = windowArgs;
        
        
        logWindow.Width = sendWindowWidth;
        logWindow.Height = sendWindowHeight;
        logWindow.Title = !item ? TextCodeTranslator.Translate("QuoteOPTemplate.S.AddSection") : TextCodeTranslator.Translate("QuoteOPTemplate.S.EditSection");
        logWindow.Show("./QuoteModules/QuoteTemplates/Components/AddEditQuoteTemplateSectionComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            if ($event == "Refresh") {
                this.QuoteOPTemplateSectionLists = this.AllQuoteOPTemplateSectionLists.filter(d => !d.IsCancel);
                this.LoadSectionPreviewData();
            }
            this.IsAddSectionRunning = false;
        });
    }


    ShowRightToLeftClick() {

    }


    ShowLocalLanguageClick() {

    }

    ArrowUpButtonClicked(item: QuoteOPTemplateSectionViewModel) {
        if (item.QuoteOPTemplateSectionTypeCode != "QH" && item.QuoteOPTemplateSectionTypeCode != "QD" && item.QuoteOPTemplateSectionTypeCode != "PH" && item.QuoteOPTemplateSectionTypeCode != "PF") {
            var i = this.AllQuoteOPTemplateSectionLists.indexOf(item);
            var upColumn = this.AllQuoteOPTemplateSectionLists[i - 1];
            if (upColumn.QuoteOPTemplateSectionTypeCode != "PH") {

                if (i > 0) {
                    this.AllQuoteOPTemplateSectionLists = this.AllQuoteOPTemplateSectionLists.filter(d => d.Id != upColumn.Id);
                    var tempOrder = item.Order;

                    item.Order = upColumn.Order;
                    upColumn.Order = tempOrder;

                    this.AllQuoteOPTemplateSectionLists.splice(i, 0, upColumn);
                }
            }


            this.QuoteOPTemplateSectionLists = this.AllQuoteOPTemplateSectionLists.filter(d => !d.IsCancel);
        }
    }

    ArrowDownButtonClicked(item: QuoteOPTemplateSectionViewModel) {
        if (item.QuoteOPTemplateSectionTypeCode != "QH" && item.QuoteOPTemplateSectionTypeCode != "QD" && item.QuoteOPTemplateSectionTypeCode != "PH" && item.QuoteOPTemplateSectionTypeCode != "PF") {
            var i = this.AllQuoteOPTemplateSectionLists.indexOf(item);
            var downColumn = this.AllQuoteOPTemplateSectionLists[i + 1];
            if (downColumn.QuoteOPTemplateSectionTypeCode != "PF") {
                if (i < this.AllQuoteOPTemplateSectionLists.length - 1) {
                    this.AllQuoteOPTemplateSectionLists = this.AllQuoteOPTemplateSectionLists.filter(d => d.Id != downColumn.Id);
                    var tempOrder = item.Order;
                    item.Order = downColumn.EntityPM.Order;
                    downColumn.Order = tempOrder;

                    this.AllQuoteOPTemplateSectionLists.splice(i, 0, downColumn);
                }
            }

            this.QuoteOPTemplateSectionLists = this.AllQuoteOPTemplateSectionLists.filter(d => !d.IsCancel);
        }
    }

    //End Event Area



    //Loading Area
    LoadQuoteOPTemplatePM() {

        this.QuoteOPTemplatePMService.get(this.QuoteTemplateId).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError && pmResponse.Result) {
                this.EntityPM = pmResponse.Result;
                this.QuoteTemplateId = this.EntityPM.Id;
                this.LoadData();
            }
            else this.CurrentSession.StopBusyIndicator();
        });
    }



    LoadData() {
        this.IsLoadQuoteOPTemplateSectionRuning = true;
        this.IsLoadQuoteOPTemplateTextCodeRuning = true;
        this.IsLoadQuoteOPTemplateSettingsRuning = true;
        this.IsLoadPreviewSectionRuning = true;

        this.LoadQuoteOPTemplateSectionLists();
        this.LoadQuoteOPTemplateTextCodeLists();
        this.LoadSetting();
    }

    LoadSetting() {
    

        this.QuoteOPTemplateSettingPMService.get(this.EntityPM.QuoteOPTemplateSettingId).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            this.IsLoadQuoteOPTemplateSettingsRuning = false;
            this.LoadCompleted();
            if (!pmResponse.HasError && pmResponse.Result) {
                this.QuoteOPTemplateSettingPM = pmResponse.Result;
           
            }
        
        });
    
    }

    LoadQuoteOPTemplateSectionLists() {

        this.QuoteOPTemplateSectionLists = [];
        this.QuoteOPTemplateSectionExtendedPMService.GetQuoteOPTemplateSectionByQuoteOPTemplateId(this.EntityPM.Id, SessionLocator.Tenant).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;

            this.IsLoadQuoteOPTemplateSectionRuning = false;
            this.LoadCompleted();

            if (!pmResponse.HasError) {
                if (pmResponse.Result) {
                    this.BuildingQuoteOPTemplateSection(pmResponse.Result);
                }

            }


        });

    }

    LoadQuoteOPTemplateTextCodeLists() {
        this.QuoteOPTemplateTextCodeLists = [];
        this.QuoteOPTemplateTextCodeExtendedPMService.GetQuoteOPTemplateTextCodeByQuoteOPTemplateId(this.EntityPM.Id, SessionLocator.Tenant).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            this.IsLoadQuoteOPTemplateTextCodeRuning = false;
            this.LoadCompleted();
            if (!pmResponse.HasError) {
                this.QuoteOPTemplateTextCodeLists = pmResponse.Result;
            }
            
        });

    }
  
    LoadSectionPreviewData() {
        if (this.selectQuoteOPTemplateSection != null) {
            if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
                this.froalaEditorSetting.froalaEditorComponent.SetHtml("");
                this.ReloadFroalaEditor();
            }



            if (this.selectQuoteOPTemplateSection.QuoteOPTemplateSectionTypeCode != "PB") {
                if (!this.selectQuoteOPTemplateSection.IsLoaded) {
                    this.selectQuoteOPTemplateSection.IsLoaded = true;
                    if (!this.IsLoadPreviewSectionRuning) {
                        this.IsLoadPreviewSectionRuning = true;
                        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteOPTemplate.M.Loading"));
                    }
                    var quoteId: string = this.QuoteOPPM != null ? this.QuoteOPPM.Id : "";
                    var sectionDocId: string = !AppTool.IsNullOrEmpty(this.selectQuoteOPTemplateSection.SectionDocId) ? this.selectQuoteOPTemplateSection.SectionDocId : "";

                    this.QuoteOPTemplateSectionExtendedPMService.DownloadQuoteOPTemplateSectionPdfFile(this.selectQuoteOPTemplateSection.QuoteOPTemplateSectionTypeCode, sectionDocId, this.EntityPM.Id, this.EntityPM.QuoteOPTemplateSettingId, quoteId, this.EntityPM.CreatedByUserId, SessionLocator.Tenant).subscribe((res:any) => {
                        var pmResponse: ServiceResponse = res;
                        this.IsLoadPreviewSectionRuning = false;
                        this.LoadCompleted();

                        if (!pmResponse.HasError && pmResponse.Result) {
                            this.selectQuoteOPTemplateSection.HtmlBody = pmResponse.Result;
                            if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
                                this.froalaEditorSetting.froalaEditorComponent.SetHtml(pmResponse.Result);
                                this.ReloadFroalaEditor();
                            }


                        }
                       else if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                            this.ShowMessage(pmResponse.ErrorsArray[0]);
                        }

                    });
                }

                else {
                    this.IsLoadPreviewSectionRuning = false;
                    this.LoadCompleted();
                    if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
                        this.froalaEditorSetting.froalaEditorComponent.SetHtml(this.selectQuoteOPTemplateSection.HtmlBody);
                        this.ReloadFroalaEditor();
                    }
                }
            }
            else{

                this.IsLoadPreviewSectionRuning = false;
                this.LoadCompleted();
            }
        }

    }



    public ShowMessage(message: string, title: string = "") {

        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);

        if (title) {
            messageWindow.Title = title;
        }
    }





    LoadCompleted() {
        if (!this.IsLoadQuoteOPTemplateSectionRuning && !this.IsLoadQuoteOPTemplateTextCodeRuning && !this.IsLoadQuoteOPTemplateSettingsRuning && !this.IsLoadPreviewSectionRuning) {
          
            this.CurrentSession.StopBusyIndicator();
        }
    }

    //End Loading Area

   
    AddPageBreakSection() {

        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteOPTemplate.M.Saving"));

        var order: number = 0;

        if (this.SelectQuoteOPTemplateSection != null && !this.SelectQuoteOPTemplateSection.IsCancel) order = (this.SelectQuoteOPTemplateSection.Order + 1);
        else {
            var footerItem = this.AllQuoteOPTemplateSectionLists.filter(d => d.QuoteOPTemplateSectionTypeCode == "PF")[0];
            order = footerItem.Order;
        }
        

        var newQuoteOPTemplateSectionPM: QuoteOPTemplateSectionPM = new QuoteOPTemplateSectionPM();
        newQuoteOPTemplateSectionPM.Tenant = SessionLocator.TenantPM.Id;
        newQuoteOPTemplateSectionPM.QuoteTemplateId = this.QuoteTemplateId;
        newQuoteOPTemplateSectionPM.QuoteOPTemplateSectionTypeCode = "PB";
        newQuoteOPTemplateSectionPM.Name = "Page Break";
        newQuoteOPTemplateSectionPM.Order = order;
    


        this.QuoteOPTemplateSectionPMService.insert(newQuoteOPTemplateSectionPM).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;

            var pageBreakSection: QuoteOPTemplateSectionViewModel = new QuoteOPTemplateSectionViewModel(pmResponse.Result);

            if (!pmResponse.HasError) {

                var items: any[] = [];
                this.AllQuoteOPTemplateSectionLists.forEach((item) => {
                    if (item.Order == (newQuoteOPTemplateSectionPM.Order-1)) {

                        if (item.QuoteOPTemplateSectionTypeCode != "PF") {
                            items.push(item);
                            items.push(pageBreakSection);
                        } else {
                            items.push(pageBreakSection);
                            items.push(item);
                        }

                    }
                    else {
                        items.push(item);
                    }

                });


                this.UpdateOrderOfSections(items);
            }

            else {
                this.CurrentSession.StopBusyIndicator();
            }
            });


    }

    RemoveQuoteOPTemplateSectionClicked(item: QuoteOPTemplateSectionViewModel) {


        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show(TextCodeTranslator.Translate("QuoteOPTemplate.M.DeleteSectionConfirmMessage"));
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                item.IsCancel = true;
                this.QuoteOPTemplateSectionLists = this.AllQuoteOPTemplateSectionLists.filter(d => d.IsCancel == false);
                //if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
                //    this.froalaEditorSetting.froalaEditorComponent.SetHtml("");
                //    this.ReloadFroalaEditor();
                //}
                this.SelectQuoteOPTemplateSection = this.QuoteOPTemplateSectionLists.filter(d => d.Order == (item.Order + 1))[0];

            }
        });






     
    }

    SaveDirtySections(previewPdfAfterSave: boolean) {

        var QuoteOPTemplateSectionChangedLists: QuoteOPTemplateSectionPM[] = [];

        this.AllQuoteOPTemplateSectionLists.filter(d => d.EntityPM.IsDirty).forEach((item) => {
            QuoteOPTemplateSectionChangedLists.push(item.EntityPM);
        });

        if (QuoteOPTemplateSectionChangedLists.length > 0) {

            this.AllQuoteOPTemplateSectionLists.filter(d => d.EntityPM.IsDirty).forEach((item) => {
                item.EntityPM.IsDirty = false;
            });

            this.QuoteOPTemplateSectionExtendedPMService.updateSections(QuoteOPTemplateSectionChangedLists).subscribe((res:any) => {
                this.CurrentSession.StopBusyIndicator();
                if (previewPdfAfterSave) this.PreviewPdf();
            });
        }

        else {
            this.CurrentSession.StopBusyIndicator();

            if (previewPdfAfterSave) this.PreviewPdf();
                
      
        }

    }

    UpdateOrderOfSections(items:any[]) {


        var order: number = 0;
        items.forEach((sec) => {
            sec.Order = order;
            order += 1;

        });
        this.AllQuoteOPTemplateSectionLists = items;

        this.QuoteOPTemplateSectionLists = this.AllQuoteOPTemplateSectionLists.filter(d => d.IsCancel == false);


        this.SaveDirtySections(false);


    }

    //Saving Area
    SaveButtonClicked() {
       
     this.ValidationErrorsList = [];
   
        var QuoteOPTemplateSectionChangedLists: QuoteOPTemplateSectionPM[] = [];
        this.AllQuoteOPTemplateSectionLists.filter(d => d.EntityPM.IsDirty).forEach((item) => {
            QuoteOPTemplateSectionChangedLists.push(item.EntityPM);
        });


        if (QuoteOPTemplateSectionChangedLists.length > 0) this.IsSaveQuoteOPTemplateSectionRuning = true;
        if (this.EntityPM.IsDirty) this.IsSaveQuoteOPTemplateRuning = true;



        if (this.IsSaveQuoteOPTemplateSectionRuning || this.IsSaveQuoteOPTemplateRuning) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteOPTemplate.M.Saving"));
            this.SaveQuoteOPTemplateSection(QuoteOPTemplateSectionChangedLists);
            this.SaveQuoteOPTemplate();
        }
        else this.SaveCompleted();

    }

    SaveQuoteOPTemplateSection(sections: any) {

        if (this.IsSaveQuoteOPTemplateSectionRuning) {
            this.QuoteOPTemplateSectionExtendedPMService.updateSections(sections).subscribe((res:any) => {
                this.IsSaveQuoteOPTemplateSectionRuning = false;

                this.SaveCompleted();
                
            });
            
        }
    }


    RefreshQuoteOPTemplate: boolean = false;
    SaveQuoteOPTemplate() {

        if (this.IsSaveQuoteOPTemplateRuning) {
            this.RefreshQuoteOPTemplate = true;
            this.QuoteOPTemplatePMService.update(this.EntityPM).subscribe((res:any) => {
                this.IsSaveQuoteOPTemplateRuning = false;
                this.SaveCompleted();
            });
        }
    }


    SaveQuoteOPTemplateSetting() {
        if (this.IsSaveQuoteOPTemplateSettingsRuning) {
            this.QuoteOPTemplateSettingPMService.update(this.QuoteOPTemplateSettingPM).subscribe((res:any) => {
                this.IsSaveQuoteOPTemplateSettingsRuning = false;
                this.SaveCompleted("RefreshPreviewData");
            });

        }
    }


    SaveCompleted(proess:string= null) {
        if (!this.IsSaveQuoteOPTemplateSectionRuning && !this.IsSaveQuoteOPTemplateRuning && !this.IsSaveQuoteOPTemplateSettingsRuning) {
            this.CurrentSession.StopBusyIndicator();
            if (this.QuoteOPPM == null && this.RefreshQuoteOPTemplate) {
                this.CurrentSession.FireEvent("ReloadAllList");
            }

            if (proess == "RefreshPreviewData") {
                this.AllQuoteOPTemplateSectionLists.forEach((item) => {
                    item.IsLoaded = false;
                });
                this.LoadSectionPreviewData();
            } else this.CurrentSession.CurrentWindow.Close("SavedChanges");//this.CloseButtonClicked();
            
        }
    }
    //End Saving Area


    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }



    BuildingQuoteOPTemplateSection(list: QuoteOPTemplateSectionPM[]) {
        if (list) {
            list = list.sort((a, b) => { return a.Order - b.Order });
        
            list.forEach((item) => {
                var viewModelSection: QuoteOPTemplateSectionViewModel = new QuoteOPTemplateSectionViewModel(item);
                this.QuoteOPTemplateSectionLists.push(viewModelSection);
                this.AllQuoteOPTemplateSectionLists.push(viewModelSection);

            });

            if (this.QuoteOPTemplateSectionLists && this.QuoteOPTemplateSectionLists.length > 0) {
                this.SelectQuoteOPTemplateSection = this.QuoteOPTemplateSectionLists[0];
            }

            this.QuoteOPTemplateSectionLists = this.QuoteOPTemplateSectionLists.filter(d => d.IsCancel == false);
        }
    }

    public ReloadFroalaEditor() {

        if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
            this.froalaEditorSetting.froalaEditorComponent.SetHtml(this.froalaEditorSetting.froalaEditorComponent.getHtml());
            this.froalaEditorSetting.froalaEditorComponent.ShowEditor();

        }

    }


}

export class QuoteOPTemplateSectionViewModel {

    Background: string = "white";
    IsShowArrowUpDown: boolean= false;
    Id: string;
    EntityPM: QuoteOPTemplateSectionPM;
    IsSection: boolean = false;
    IsLoaded: boolean = false;
    HtmlBody: string;
    Description: string;
    SectionDocId: string = "";
    QuoteOPTemplateSectionTypeCode: string;
    quotationComponent: QuotationComponent;
    IsShowDeleteButton: boolean;
    IsPagebrackSession: boolean = false;

    private isCancel: boolean;
    public get IsCancel() {

        if (this.EntityPM) {
            this.isCancel = this.EntityPM.IsCancel;
        }
        return this.isCancel;
    }
    public set IsCancel(newValue: boolean) {
        if (this.isCancel != newValue) {
            this.isCancel = newValue;
            if (this.EntityPM) this.EntityPM.IsCancel = newValue;
        }
    }









    private order: number;
    public get Order() {
       
        if (this.EntityPM) {
            this.order = this.EntityPM.Order;
        }
        return this.order;
    }
    public set Order(newValue: number) {
        if (this.order != newValue) {
            this.order = newValue;
           if (this.EntityPM) this.EntityPM.Order = newValue;
        }
    }


    private name: string;
    public get Name() {
        if (this.EntityPM) {
            this.name = this.EntityPM.Name;
        }

        return this.name;
    }
    public set Name(newValue: string) {
        if (this.name != newValue) {
            this.name = newValue; 
            if (this.EntityPM) this.EntityPM.Name = newValue;
        }
    }





    private templatedata: any;
    public get Templatedata() {
        if (this.EntityPM) {
            this.templatedata = this.EntityPM.Templatedata;
        }
        return this.templatedata;
    }
    public set Templatedata(newValue: any) {

        if (this.templatedata != newValue) {
           this.templatedata = newValue;
           if (this.EntityPM) this.EntityPM.Templatedata = newValue;

        }
    }

    private isExcluded: boolean = false;
    public get IsExcluded() {
        if (this.EntityPM) {
            this.isExcluded = this.EntityPM.IsExcluded;
        }

        return this.isExcluded;

    }
    public set IsExcluded(newValue: boolean) {
        if (this.isExcluded != newValue) {
            this.isExcluded = newValue;
            this.EntityPM.IsExcluded = newValue;

            if (newValue === true) {
                this.quotationComponent.ExcludeSection(this);
            }
            else {
                this.quotationComponent.IncludeSection(this);
            }

        }
    }



    
    ToolTipDisplay: string;
    DisplayName: string;
    constructor(QuoteOPTemplateSection: QuoteOPTemplateSectionPM, quotationComponent: QuotationComponent = null) {
        this.EntityPM = QuoteOPTemplateSection;
        this.quotationComponent = quotationComponent;
        this.Id = QuoteOPTemplateSection.Id;
        this.SectionDocId = QuoteOPTemplateSection.SectionDocId;
        this.QuoteOPTemplateSectionTypeCode = QuoteOPTemplateSection.QuoteOPTemplateSectionTypeCode;
        this.Description = QuoteOPTemplateSection.Description;
        this.Templatedata = QuoteOPTemplateSection.Templatedata;
        this.Order = QuoteOPTemplateSection.Order;
        this.Name = QuoteOPTemplateSection.Name;
        this.IsCancel = QuoteOPTemplateSection.IsCancel;

        var code = this.EntityPM.QuoteOPTemplateSectionTypeCode;
        this.DisplayName = this.EntityPM.Name;

        if (code == "PP" || code == "PC" || code == "PF" || code == "PH" || code == "QH" || code == "QD" || code == "PB") {
             this.TranslationTextCode(this.EntityPM.QuoteOPTemplateSectionTypeCode);
         }

         if (QuoteOPTemplateSection.QuoteOPTemplateSectionTypeCode != "PP" && QuoteOPTemplateSection.QuoteOPTemplateSectionTypeCode != "PC" && QuoteOPTemplateSection.QuoteOPTemplateSectionTypeCode != "QH" && QuoteOPTemplateSection.QuoteOPTemplateSectionTypeCode != "QD" && QuoteOPTemplateSection.QuoteOPTemplateSectionTypeCode != "PH" && QuoteOPTemplateSection.QuoteOPTemplateSectionTypeCode != "PF" && QuoteOPTemplateSection.QuoteOPTemplateSectionTypeCode != "PB") {

             this.IsSection = true;
         } else if (QuoteOPTemplateSection.QuoteOPTemplateSectionTypeCode == "PB") {
             this.IsPagebrackSession = true;
         }
         if (this.IsPagebrackSession || this.IsSection) this.IsShowDeleteButton = true;


       
    }

    TranslationTextCode(sectionTypeCode: string) {

        var result = "";

        var textCode: string = "QuoteOPTemplate.S.";
        var textCodeToolTip: string = "QuoteOPTemplate.M.";
        if (sectionTypeCode == "PP") {
            textCode += "PricingPackages";
            textCodeToolTip += "PricingTableDescriptionMessage";
        }
        else if (sectionTypeCode == "PC") {
            textCode += "PricingContainers";
            textCodeToolTip += "PricingTableDescriptionMessage";
        }
        else if (sectionTypeCode == "PF") {
            textCode += "PageFooter";
            textCodeToolTip += "PageFooterDescriptionMessage";
        }
        else if (sectionTypeCode == "PH") {
            textCode += "PageHeader";
            textCodeToolTip += "PageHeaderDescriptionMessage";
        }
        else if (sectionTypeCode == "QH") {
            textCode += "QuoteHeader";
            textCodeToolTip += "QuoteHeaderDescriptionMessage";
        }
        else if (sectionTypeCode == "QD") {
            textCode += "QuoteDetails";
            textCodeToolTip += "QuoteDetailsDescriptionMessage";
        }

        else if (sectionTypeCode == "PB") {
            textCode = "QuoteOPTemplate.B.PageBreak";
            textCodeToolTip = null;//To avoid alert missing textcode
        }

        this.DisplayName = TextCodeTranslator.Translate(textCode);
        this.ToolTipDisplay = TextCodeTranslator.Translate(textCodeToolTip);

        return result; 
    }



}
