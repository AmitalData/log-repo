

import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {QuoteTemplatePM} from '../../../Quote/EntityPMs/QuoteTemplatePM';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {QuoteTemplateExtendedPMService} from '../../../Quote/Services/ExtendedPMs/QuoteTemplateExtendedPMService';
import {QuotationComponent} from '../../QuoteOthers/Components/Quotation/QuotationComponent';
import {QuoteTemplateSectionPMService} from '../../../Quote/Services/StandardPMs/QuoteTemplateSectionPMService';
import {QuoteTemplatePMService} from '../../../Quote/Services/StandardPMs/QuoteTemplatePMService';
import {QuoteTemplateSettingPM} from '../../../Quote/EntityPMs/QuoteTemplateSettingPM';
import {QuoteTemplateSettingPMService} from '../../../Quote/Services/StandardPMs/QuoteTemplateSettingPMService';
import {QuoteTemplateTextCodeExtendedPMService} from '../../../Quote/Services/ExtendedPMs/QuoteTemplateTextCodeExtendedPMService';
import {QuoteTemplateSectionExtendedPMService} from '../../../Quote/Services/ExtendedPMs/QuoteTemplateSectionExtendedPMService';
import {FroalaEditorSetting} from '../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/FroalaEditorSetting';
import {QuoteTemplateTextCodePM} from '../../../Quote/EntityPMs/QuoteTemplateTextCodePM';
import {QuoteTemplateSectionPM} from '../../../Quote/EntityPMs/QuoteTemplateSectionPM';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
@Component({
    selector: 'EditQuoteTemplateComponent',
    moduleId: module.id,
    templateUrl: './EditQuoteTemplateComponent.html',
})

export class EditQuoteTemplateComponent extends BaseComponent implements OnInit {
    public DataContext: EditQuoteTemplateComponent = this;
    EntityPM: QuoteTemplatePM;
    IsNewEntityCall: boolean = true;
    public ValidationErrorsList: string[];
    CurrentEntity: any;
    QuoteTemplateSectionLists: QuoteTemplateSectionViewModel[] = [];
    AllQuoteTemplateSectionLists: QuoteTemplateSectionViewModel[] = [];
    QuoteTemplateTextCodeLists: QuoteTemplateTextCodePM[] = [];
    IsLoadQuoteTemplateSectionRuning: boolean = false;
    IsLoadQuoteTemplateTextCodeRuning: boolean = false;
    IsLoadQuoteTemplateSettingsRuning: boolean = false;

    IsSaveQuoteTemplateSectionRuning: boolean = false;
    IsSaveQuoteTemplateRuning: boolean = false;
    IsSaveQuoteTemplateSettingsRuning: boolean = false;
    ShowLocalLanguageCheckBoxKey: string;
    ShowRightToLeftCheckBoxKey: string;
    IsAddSectionRunning: boolean = false;   

    QuoteTemplateSettingPM: QuoteTemplateSettingPM;
    
    quoteTemplateSectionPMService: QuoteTemplateSectionPMService;
    VisibilityGeneralSetting: boolean = true;
    IsLoadPreviewSectionRuning: boolean;
    QuoteTemplateId: string;
    QuotePM: any;
    quoteTemplateExtendedPMService: QuoteTemplateExtendedPMService;
    quoteTemplateTextCodeExtendedPMService: QuoteTemplateTextCodeExtendedPMService;
    quoteTemplateSectionExtendedPMService: QuoteTemplateSectionExtendedPMService;
    quoteTemplateSettingPMService: QuoteTemplateSettingPMService;
    IsShowFroalaEditor: boolean = false;
    froalaEditorSetting: FroalaEditorSetting;
    quoteTemplatePMService: QuoteTemplatePMService;
    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;

    IsDisableEditButton: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
      
        if (!FeatureLocator.HasFeaturePermession("QuoteTemplate", "UPDATE")) this.IsDisableEditButton = true;

        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteTemplate.M.Loading"));
        this.quoteTemplateExtendedPMService = new QuoteTemplateExtendedPMService();
        this.quoteTemplateTextCodeExtendedPMService = new QuoteTemplateTextCodeExtendedPMService();
        this.quoteTemplateSectionExtendedPMService = new QuoteTemplateSectionExtendedPMService();
        this.quoteTemplateSettingPMService = new QuoteTemplateSettingPMService();
        this.quoteTemplateSectionPMService = new QuoteTemplateSectionPMService();
        
        this.quoteTemplatePMService = new QuoteTemplatePMService();

    }

    ngOnInit() {
        this.ShowLocalLanguageCheckBoxKey = Guid.newGuid();
        this.ShowRightToLeftCheckBoxKey = Guid.newGuid();

    }

    IsReady: boolean = false;
    SetWindowArgs(args: any) {

        var _entityResourceService: EntityResourceService = new EntityResourceService();
        _entityResourceService.getEntityResourceByTableName("QuoteTemplate").subscribe(response => {
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
        this.QuotePM = args.QuotePM;
        this.CurrentEntity = args.CurrentEntity;
        this.QuoteTemplateId = args.QuoteTemplateId;

        if (this.EntityPM) {
            this.QuoteTemplateId = this.EntityPM.Id;
            this.LoadData();
        }
        else {
            if (this.CurrentEntity) this.QuoteTemplateId = this.CurrentEntity.Id;
            if (!AppTool.IsNullOrEmpty(this.QuoteTemplateId)) this.LoadQuoteTemplatePM();
            else this.LoadCompleted();
        }

        if (this.QuotePM != null) {
            this.VisibilityGeneralSetting = false;
        }

    }


    private selectQuoteTemplateSection: QuoteTemplateSectionViewModel;
    get SelectQuoteTemplateSection() { return this.selectQuoteTemplateSection; }
    set SelectQuoteTemplateSection(newValue: QuoteTemplateSectionViewModel) {

        if (this.selectQuoteTemplateSection != newValue || !newValue.IsLoaded) {
            this.selectQuoteTemplateSection = newValue;
            this.QuoteTemplateSectionLists.forEach((item) => {
                if (item.Id != this.selectQuoteTemplateSection.Id) {
                    item.Background = "white";
                    item.IsShowArrowUpDown = false;
                }
            });
            this.selectQuoteTemplateSection.Background = "#96D3F0";
            if (this.selectQuoteTemplateSection.QuoteTemplateSectionTypeCode != "PF" && this.selectQuoteTemplateSection.QuoteTemplateSectionTypeCode != "PH" && this.selectQuoteTemplateSection.QuoteTemplateSectionTypeCode != "QH" && this.selectQuoteTemplateSection.QuoteTemplateSectionTypeCode != "QD") {
                this.selectQuoteTemplateSection.IsShowArrowUpDown = true;
            }
           

            this.LoadSectionPreviewData();
        }



    }



    get RightToLeft() {
        var rightToLeft: boolean = false;
        if (this.QuoteTemplateSettingPM) rightToLeft = this.QuoteTemplateSettingPM.RightToLeft;
        return rightToLeft;
    }
    set RightToLeft(value: boolean) {
        if (this.QuoteTemplateSettingPM != null) {
            if (this.QuoteTemplateSettingPM.RightToLeft != value) {
                this.QuoteTemplateSettingPM.RightToLeft = value;
                this.IsSaveQuoteTemplateSettingsRuning = true;
                this.SaveQuoteTemplateSetting();
            }
        }
    }

    get ShowLocalLanguage() {
        var showLocalLanguage: boolean = false;
        if (this.QuoteTemplateSettingPM) showLocalLanguage = this.QuoteTemplateSettingPM.ShowLocalLanguage;
        return showLocalLanguage;
    }
    set ShowLocalLanguage(value: boolean) {
        if (this.QuoteTemplateSettingPM != null) {
            if (this.QuoteTemplateSettingPM.ShowLocalLanguage != value) {
                this.QuoteTemplateSettingPM.ShowLocalLanguage = value;
                this.IsSaveQuoteTemplateSettingsRuning = true;
                this.SaveQuoteTemplateSetting();
            }
        }
    }



 
    GeneralSettingsButtonClicked() {

        var windowArgs: any = {};
        var logWindow = new LogitudeWindow();
        windowArgs.QuoteTemplatePM = this.EntityPM;
        windowArgs.QuoteTemplateSettingPM = this.QuoteTemplateSettingPM;


        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 620;
        logWindow.Height = 450;
        logWindow.Title = TextCodeTranslator.Translate("QuoteTemplate.S.General"); 
        logWindow.Show("./QuoteModules/QuoteTemplates/Components/QuoteTemplateGeneralSetting");

   
    }


    PreviewPdf() {
        var windowArgs: any = {};
        var logWindow = new LogitudeWindow();
        windowArgs.QuoteTemplateId = this.EntityPM.Id;
        windowArgs.QuoteId = this.QuotePM != null ? this.QuotePM.Id : "";
        logWindow.Title = TextCodeTranslator.Translate("QuoteTemplate.S.PreviewTemplate");
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 1000;
        logWindow.Height = (window.innerHeight - 130);
        logWindow.Show("./QuoteModules/QuoteTemplates/Components/PreviewQuoteTemplateReportComponent");
    }

    PreviewButtonClicked() {

        this.SaveDirtySections(true);

    }

    //Event Area
    EditSectionButtonClicked(item: QuoteTemplateSectionViewModel) {

        if (item.IsSection) {
            this.AddEditSection(item);
        }
        else {
            var componentPath = "";
            var logWindow = new LogitudeWindow();
            var windowArgs: any = {};
            windowArgs.QuoteTemplatePM = this.EntityPM;
            windowArgs.QuoteTemplateSettingPM = this.QuoteTemplateSettingPM;
            windowArgs.QuoteTemplateSectionViewModel = item;
            windowArgs.QuoteId = this.QuotePM != null ? this.QuotePM.Id : "";
            windowArgs.QuotePM = this.QuotePM;
            //Pricing Setting
            if (item.QuoteTemplateSectionTypeCode == "PP" || item.QuoteTemplateSectionTypeCode == "PC") {
                windowArgs.QuoteTemplateSectionTypeName = item.QuoteTemplateSectionTypeCode == "PP" ? "Packages" : "Containers";
                logWindow.Title = TextCodeTranslator.Translate("QuoteTemplate.S.Pricing" + windowArgs.QuoteTemplateSectionTypeName.replace(" ", "") + "Settings");
                windowArgs.QuoteTemplateTextCodePMList = this.QuoteTemplateTextCodeLists;
                componentPath = "./QuoteModules/QuoteTemplates/Components/QuoteTemplatePricingSettingComponent";
                logWindow.Width = 950;
                logWindow.Height = 660;
              
            }
      
            //Page Header && Footer  Setting
            else if (item.QuoteTemplateSectionTypeCode == "PH" || item.QuoteTemplateSectionTypeCode == "PF") {
                windowArgs.QuoteTemplateSectionTypeName = item.QuoteTemplateSectionTypeCode == "PH" ? "Header" : "Footer";
                windowArgs.EditQuoteTemplateComponent = this;
                
                componentPath = "./QuoteModules/QuoteTemplates/Components/QuoteTemplateHeaderFooterSettingComponent";
                logWindow.Width = (window.innerWidth / 1.476); // 1920/1300
                logWindow.Height = 600;
  
                logWindow.Title = TextCodeTranslator.Translate("QuoteTemplate.S.Page" + windowArgs.QuoteTemplateSectionTypeName.replace(" ", "") + "Settings");
            }

            else if (item.QuoteTemplateSectionTypeCode == "QH" || item.QuoteTemplateSectionTypeCode == "QD") {
                windowArgs.QuoteTemplateSectionTypeName = item.QuoteTemplateSectionTypeCode == "QH" ? "QuoteHeader" : "QuoteDetails";
                logWindow.Title = TextCodeTranslator.Translate("QuoteTemplate.S." + windowArgs.QuoteTemplateSectionTypeName.replace(" ", "") + "Settings");
                windowArgs.QuoteTemplateSectionTypeCode = item.QuoteTemplateSectionTypeCode;
                windowArgs.QuoteTemplateTextCodePMList = this.QuoteTemplateTextCodeLists;
                windowArgs.QuotePM = this.QuotePM;
                
                componentPath = "./QuoteModules/QuoteTemplates/Components/QuoteTemplateHeaderDetailsSettingComponent";
                logWindow.Width = 940;
                logWindow.Height = 660;
        
            }


            if (!AppTool.IsNullOrEmpty(componentPath)) {
                logWindow.WindowArgs = windowArgs;
                logWindow.Show(componentPath);
                logWindow.WindowClosed.subscribe(($event: any) => {
                    if ($event == "Refresh") {

                        if (windowArgs.QuoteTemplateSectionTypeName == "Header" || windowArgs.QuoteTemplateSectionTypeName == "Footer") {
                            if (this.SelectQuoteTemplateSection != item) {
                                this.SelectQuoteTemplateSection = item;
                            }
                            else {
                                this.LoadSectionPreviewData();
                            }

                        }
                        else {

                            if (item.QuoteTemplateSectionTypeCode == "QH") {
                                var quoteTemplateSectionHeaderViewModel = this.QuoteTemplateSectionLists.filter(d => d.QuoteTemplateSectionTypeCode == "PH")[0];
                                if (quoteTemplateSectionHeaderViewModel) {
                                    quoteTemplateSectionHeaderViewModel.IsLoaded = false;
                                }
                            }
                      

                            item.IsLoaded = false;
                            this.SelectQuoteTemplateSection = item;
                        }

                    }
                });
                //  Refresh
            }
        }

    }

    AddEditSection(item: QuoteTemplateSectionViewModel) {

        this.IsAddSectionRunning = true;
        var widthwindow = window.innerWidth;
        var heighthwindow = window.innerHeight;
        var percentagewidthwindow = widthwindow * 0.252;
        var percentageHeightwindow = heighthwindow * 0.1764705;
        var sendWindowHeight = heighthwindow - percentageHeightwindow;
        var sendWindowWidth = widthwindow - percentagewidthwindow;
        if (sendWindowWidth < 900) sendWindowWidth = 900;
        if (sendWindowHeight < 500) sendWindowHeight = 500;

        var windowArgs: any = {};
        windowArgs.FatherComponent = this;
        windowArgs.HeightWindow = sendWindowHeight;
        windowArgs.QuoteTemplateSectionViewModel = item;
        windowArgs.QuoteTemplateId = this.EntityPM.Id;

        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = windowArgs;
        
        
        logWindow.Width = sendWindowWidth;
        logWindow.Height = sendWindowHeight;
        logWindow.Title = !item ? TextCodeTranslator.Translate("QuoteTemplate.S.AddSection") : TextCodeTranslator.Translate("QuoteTemplate.S.EditSection");
        logWindow.Show("./QuoteModules/QuoteTemplates/Components/AddEditQuoteTemplateSectionComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            if ($event == "Refresh") {
                this.QuoteTemplateSectionLists = this.AllQuoteTemplateSectionLists.filter(d => !d.IsCancel);
                this.LoadSectionPreviewData();
            }
            this.IsAddSectionRunning = false;
        });
    }


    ShowRightToLeftClick() {

    }


    ShowLocalLanguageClick() {

    }

    ArrowUpButtonClicked(item: QuoteTemplateSectionViewModel) {
        if (item.QuoteTemplateSectionTypeCode != "QH" && item.QuoteTemplateSectionTypeCode != "QD" && item.QuoteTemplateSectionTypeCode != "PH" && item.QuoteTemplateSectionTypeCode != "PF") {
            var i = this.AllQuoteTemplateSectionLists.indexOf(item);
            var upColumn = this.AllQuoteTemplateSectionLists[i - 1];
            if (upColumn.QuoteTemplateSectionTypeCode != "PH") {

                if (i > 0) {
                    this.AllQuoteTemplateSectionLists = this.AllQuoteTemplateSectionLists.filter(d => d.Id != upColumn.Id);
                    var tempOrder = item.Order;

                    item.Order = upColumn.Order;
                    upColumn.Order = tempOrder;

                    this.AllQuoteTemplateSectionLists.splice(i, 0, upColumn);
                }
            }


            this.QuoteTemplateSectionLists = this.AllQuoteTemplateSectionLists.filter(d => !d.IsCancel);
        }
    }

    ArrowDownButtonClicked(item: QuoteTemplateSectionViewModel) {
        if (item.QuoteTemplateSectionTypeCode != "QH" && item.QuoteTemplateSectionTypeCode != "QD" && item.QuoteTemplateSectionTypeCode != "PH" && item.QuoteTemplateSectionTypeCode != "PF") {
            var i = this.AllQuoteTemplateSectionLists.indexOf(item);
            var downColumn = this.AllQuoteTemplateSectionLists[i + 1];
            if (downColumn.QuoteTemplateSectionTypeCode != "PF") {
                if (i < this.AllQuoteTemplateSectionLists.length - 1) {
                    this.AllQuoteTemplateSectionLists = this.AllQuoteTemplateSectionLists.filter(d => d.Id != downColumn.Id);
                    var tempOrder = item.Order;
                    item.Order = downColumn.EntityPM.Order;
                    downColumn.Order = tempOrder;

                    this.AllQuoteTemplateSectionLists.splice(i, 0, downColumn);
                }
            }

            this.QuoteTemplateSectionLists = this.AllQuoteTemplateSectionLists.filter(d => !d.IsCancel);
        }
    }

    //End Event Area



    //Loading Area
    LoadQuoteTemplatePM() {

        this.quoteTemplatePMService.get(this.QuoteTemplateId).subscribe(res => {
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
        this.IsLoadQuoteTemplateSectionRuning = true;
        this.IsLoadQuoteTemplateTextCodeRuning = true;
        this.IsLoadQuoteTemplateSettingsRuning = true;
        this.IsLoadPreviewSectionRuning = true;

        this.LoadQuoteTemplateSectionLists();
        this.LoadQuoteTemplateTextCodeLists();
        this.LoadSetting();
    }

    LoadSetting() {
    

        this.quoteTemplateSettingPMService.get(this.EntityPM.QuoteTemplateSettingId).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            this.IsLoadQuoteTemplateSettingsRuning = false;
            this.LoadCompleted();
            if (!pmResponse.HasError && pmResponse.Result) {
                this.QuoteTemplateSettingPM = pmResponse.Result;
           
            }
        
        });
    
    }

    LoadQuoteTemplateSectionLists() {

        this.QuoteTemplateSectionLists = [];
        this.quoteTemplateSectionExtendedPMService.GetQuoteTemplateSectionByQuoteTemplateId(this.EntityPM.Id, SessionLocator.Tenant).subscribe(res => {
            var pmResponse: ServiceResponse = res;

            this.IsLoadQuoteTemplateSectionRuning = false;
            this.LoadCompleted();

            if (!pmResponse.HasError) {
                if (pmResponse.Result) {
                    this.BuildingQuoteTemplateSection(pmResponse.Result);
                }

            }


        });

    }

    LoadQuoteTemplateTextCodeLists() {
        this.QuoteTemplateTextCodeLists = [];
        this.quoteTemplateTextCodeExtendedPMService.GetQuoteTemplateTextCodeByQuoteTemplateId(this.EntityPM.Id, SessionLocator.Tenant).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            this.IsLoadQuoteTemplateTextCodeRuning = false;
            this.LoadCompleted();
            if (!pmResponse.HasError) {
                this.QuoteTemplateTextCodeLists = pmResponse.Result;
            }
            
        });

    }
  
    LoadSectionPreviewData() {
        if (this.selectQuoteTemplateSection != null) {
            if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
                this.froalaEditorSetting.froalaEditorComponent.SetHtml("");
                this.ReloadFroalaEditor();
            }



            if (this.selectQuoteTemplateSection.QuoteTemplateSectionTypeCode != "PB") {
                if (!this.selectQuoteTemplateSection.IsLoaded) {
                    this.selectQuoteTemplateSection.IsLoaded = true;
                    if (!this.IsLoadPreviewSectionRuning) {
                        this.IsLoadPreviewSectionRuning = true;
                        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteTemplate.M.Loading"));
                    }
                    var quoteId: string = this.QuotePM != null ? this.QuotePM.Id : "";
                    var sectionDocId: string = !AppTool.IsNullOrEmpty(this.selectQuoteTemplateSection.SectionDocId) ? this.selectQuoteTemplateSection.SectionDocId : "";

                    this.quoteTemplateSectionExtendedPMService.DownloadQuoteTemplateSectionPdfFile(this.selectQuoteTemplateSection.QuoteTemplateSectionTypeCode, sectionDocId, this.EntityPM.Id, this.EntityPM.QuoteTemplateSettingId, quoteId, this.EntityPM.CreatedByUserId, SessionLocator.Tenant).subscribe(res => {
                        var pmResponse: ServiceResponse = res;
                        this.IsLoadPreviewSectionRuning = false;
                        this.LoadCompleted();

                        if (!pmResponse.HasError && pmResponse.Result) {
                            this.selectQuoteTemplateSection.HtmlBody = pmResponse.Result;
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
                        this.froalaEditorSetting.froalaEditorComponent.SetHtml(this.selectQuoteTemplateSection.HtmlBody);
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
        if (!this.IsLoadQuoteTemplateSectionRuning && !this.IsLoadQuoteTemplateTextCodeRuning && !this.IsLoadQuoteTemplateSettingsRuning && !this.IsLoadPreviewSectionRuning) {
          
            this.CurrentSession.StopBusyIndicator();
        }
    }

    //End Loading Area

   
    AddPageBreakSection() {

        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));

        var order: number = 0;

        if (this.SelectQuoteTemplateSection != null && !this.SelectQuoteTemplateSection.IsCancel) order = (this.SelectQuoteTemplateSection.Order + 1);
        else {
            var footerItem = this.AllQuoteTemplateSectionLists.filter(d => d.QuoteTemplateSectionTypeCode == "PF")[0];
            order = footerItem.Order;
        }
        

        var newQuoteTemplateSectionPM: QuoteTemplateSectionPM = new QuoteTemplateSectionPM();
        newQuoteTemplateSectionPM.Tenant = SessionLocator.TenantPM.Id;
        newQuoteTemplateSectionPM.QuoteTemplateId = this.QuoteTemplateId;
        newQuoteTemplateSectionPM.QuoteTemplateSectionTypeCode = "PB";
        newQuoteTemplateSectionPM.Name = "Page Break";
        newQuoteTemplateSectionPM.Order = order;
    


        this.quoteTemplateSectionPMService.insert(newQuoteTemplateSectionPM).subscribe(res => {
            var pmResponse: ServiceResponse = res;

            var pageBreakSection: QuoteTemplateSectionViewModel = new QuoteTemplateSectionViewModel(pmResponse.Result);

            if (!pmResponse.HasError) {

                var items: any[] = [];
                this.AllQuoteTemplateSectionLists.forEach((item) => {
                    if (item.Order == (newQuoteTemplateSectionPM.Order-1)) {

                        if (item.QuoteTemplateSectionTypeCode != "PF") {
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

    RemoveQuoteTemplateSectionClicked(item: QuoteTemplateSectionViewModel) {


        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show(TextCodeTranslator.Translate("QuoteTemplate.M.DeleteSectionConfirmMessage"));
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                item.IsCancel = true;
                this.QuoteTemplateSectionLists = this.AllQuoteTemplateSectionLists.filter(d => d.IsCancel == false);
                //if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
                //    this.froalaEditorSetting.froalaEditorComponent.SetHtml("");
                //    this.ReloadFroalaEditor();
                //}
                this.SelectQuoteTemplateSection = this.QuoteTemplateSectionLists.filter(d => d.Order == (item.Order + 1))[0];

            }
        });






     
    }

    SaveDirtySections(previewPdfAfterSave: boolean) {

        var quoteTemplateSectionChangedLists: QuoteTemplateSectionPM[] = [];

        this.AllQuoteTemplateSectionLists.filter(d => d.EntityPM.IsDirty).forEach((item) => {
            quoteTemplateSectionChangedLists.push(item.EntityPM);
        });

        if (quoteTemplateSectionChangedLists.length > 0) {

            this.AllQuoteTemplateSectionLists.filter(d => d.EntityPM.IsDirty).forEach((item) => {
                item.EntityPM.IsDirty = false;
            });

            this.quoteTemplateSectionExtendedPMService.updateSections(quoteTemplateSectionChangedLists).subscribe(res => {
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
        this.AllQuoteTemplateSectionLists = items;

        this.QuoteTemplateSectionLists = this.AllQuoteTemplateSectionLists.filter(d => d.IsCancel == false);


        this.SaveDirtySections(false);


    }

    //Saving Area
    SaveButtonClicked() {
       
     this.ValidationErrorsList = [];
   
        var quoteTemplateSectionChangedLists: QuoteTemplateSectionPM[] = [];
        this.AllQuoteTemplateSectionLists.filter(d => d.EntityPM.IsDirty).forEach((item) => {
            quoteTemplateSectionChangedLists.push(item.EntityPM);
        });


        if (quoteTemplateSectionChangedLists.length > 0) this.IsSaveQuoteTemplateSectionRuning = true;
        if (this.EntityPM.IsDirty) this.IsSaveQuoteTemplateRuning = true;



        if (this.IsSaveQuoteTemplateSectionRuning || this.IsSaveQuoteTemplateRuning) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));
            this.SaveQuoteTemplateSection(quoteTemplateSectionChangedLists);
            this.SaveQuoteTemplate();
        }
        else this.SaveCompleted();

    }

    SaveQuoteTemplateSection(sections: any) {

        if (this.IsSaveQuoteTemplateSectionRuning) {
            this.quoteTemplateSectionExtendedPMService.updateSections(sections).subscribe(res => {
                this.IsSaveQuoteTemplateSectionRuning = false;

                this.SaveCompleted();
                
            });
            
        }
    }


    RefreshQuoteTemplate: boolean = false;
    SaveQuoteTemplate() {

        if (this.IsSaveQuoteTemplateRuning) {
            this.RefreshQuoteTemplate = true;
            this.quoteTemplatePMService.update(this.EntityPM).subscribe(res => {
                this.IsSaveQuoteTemplateRuning = false;
                this.SaveCompleted();
            });
        }
    }


    SaveQuoteTemplateSetting() {
        if (this.IsSaveQuoteTemplateSettingsRuning) {
            this.quoteTemplateSettingPMService.update(this.QuoteTemplateSettingPM).subscribe(res => {
                this.IsSaveQuoteTemplateSettingsRuning = false;
                this.SaveCompleted("RefreshPreviewData");
            });

        }
    }


    SaveCompleted(proess:string= null) {
        if (!this.IsSaveQuoteTemplateSectionRuning && !this.IsSaveQuoteTemplateRuning && !this.IsSaveQuoteTemplateSettingsRuning) {
            this.CurrentSession.StopBusyIndicator();
            if (this.QuotePM == null && this.RefreshQuoteTemplate) {
                this.CurrentSession.FireEvent("ReloadAllList");
            }

            if (proess == "RefreshPreviewData") {
                this.AllQuoteTemplateSectionLists.forEach((item) => {
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



    BuildingQuoteTemplateSection(list: QuoteTemplateSectionPM[]) {
        if (list) {
            list = list.sort((a, b) => { return a.Order - b.Order });
        
            list.forEach((item) => {
                var viewModelSection: QuoteTemplateSectionViewModel = new QuoteTemplateSectionViewModel(item);
                this.QuoteTemplateSectionLists.push(viewModelSection);
                this.AllQuoteTemplateSectionLists.push(viewModelSection);

            });

            if (this.QuoteTemplateSectionLists && this.QuoteTemplateSectionLists.length > 0) {
                this.SelectQuoteTemplateSection = this.QuoteTemplateSectionLists[0];
            }

            this.QuoteTemplateSectionLists = this.QuoteTemplateSectionLists.filter(d => d.IsCancel == false);
        }
    }

    public ReloadFroalaEditor() {

        if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
            this.froalaEditorSetting.froalaEditorComponent.SetHtml(this.froalaEditorSetting.froalaEditorComponent.getHtml());
            this.froalaEditorSetting.froalaEditorComponent.ShowEditor();

        }

    }


}

export class QuoteTemplateSectionViewModel {

    Background: string = "white";
    IsShowArrowUpDown: boolean= false;
    Id: string;
    EntityPM: QuoteTemplateSectionPM;
    IsSection: boolean = false;
    IsLoaded: boolean = false;
    HtmlBody: string;
    Description: string;
    SectionDocId: string = "";
    QuoteTemplateSectionTypeCode: string;
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
    constructor(quoteTemplateSection: QuoteTemplateSectionPM, quotationComponent: QuotationComponent = null) {
        this.EntityPM = quoteTemplateSection;
        this.quotationComponent = quotationComponent;
        this.Id = quoteTemplateSection.Id;
        this.SectionDocId = quoteTemplateSection.SectionDocId;
        this.QuoteTemplateSectionTypeCode = quoteTemplateSection.QuoteTemplateSectionTypeCode;
        this.Description = quoteTemplateSection.Description;
        this.Templatedata = quoteTemplateSection.Templatedata;
        this.Order = quoteTemplateSection.Order;
        this.Name = quoteTemplateSection.Name;
        this.IsCancel = quoteTemplateSection.IsCancel;

        var code = this.EntityPM.QuoteTemplateSectionTypeCode;
        this.DisplayName = this.EntityPM.Name;

        if (code == "PP" || code == "PC" || code == "PF" || code == "PH" || code == "QH" || code == "QD" || code == "PB") {
             this.TranslationTextCode(this.EntityPM.QuoteTemplateSectionTypeCode);
         }

         if (quoteTemplateSection.QuoteTemplateSectionTypeCode != "PP" && quoteTemplateSection.QuoteTemplateSectionTypeCode != "PC" && quoteTemplateSection.QuoteTemplateSectionTypeCode != "QH" && quoteTemplateSection.QuoteTemplateSectionTypeCode != "QD" && quoteTemplateSection.QuoteTemplateSectionTypeCode != "PH" && quoteTemplateSection.QuoteTemplateSectionTypeCode != "PF" && quoteTemplateSection.QuoteTemplateSectionTypeCode != "PB") {

             this.IsSection = true;
         } else if (quoteTemplateSection.QuoteTemplateSectionTypeCode == "PB") {
             this.IsPagebrackSession = true;
         }
         if (this.IsPagebrackSession || this.IsSection) this.IsShowDeleteButton = true;


       
    }

    TranslationTextCode(sectionTypeCode: string) {

        var result = "";

        var textCode: string = "QuoteTemplate.S.";
        var textCodeToolTip: string = "QuoteTemplate.M.";
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
            textCode = "QuoteTemplate.B.PageBreak";
            textCodeToolTip = null;//To avoid alert missing textcode
        }

        this.DisplayName = TextCodeTranslator.Translate(textCode);
        this.ToolTipDisplay = TextCodeTranslator.Translate(textCodeToolTip);

        return result; 
    }



}
