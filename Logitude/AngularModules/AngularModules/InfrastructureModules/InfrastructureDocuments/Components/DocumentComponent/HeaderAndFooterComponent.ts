import {Component, OnInit, ChangeDetectorRef, EventEmitter }  from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {ServiceArgs} from '../../../../Infrastructure/DataContracts/ServiceArgs';
import {TextCodeTranslationPipe} from '../../../../Controls/Pipes/TextCodeTranslationPipe';
import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';
import {DocumentTypeTemplateViewModel} from './DocsOut/ViewModel/DocumentTypeTemplateViewModel';
import {SendHtmlDocumentFilter} from './DocsOut/Filters/SendHtmlDocumentFilter';
import {FroalaEditorSetting} from './DocsOut/FroalaEditorSetting';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {DocumentTypeTemplatePM} from '../../../../Common/EntityPMs/DocumentTypeTemplatePM';
import {AppTool} from '../../../../Infrastructure/Tools';
import {DocumentTypeTemplatePMExtendedService} from '../../../../Common/Services/ExtendedPMs/DocumentTypeTemplatePMExtendedService';
import {DocumentTypeTemplatePMService} from '../../../../Common/Services/StandardPMs/DocumentTypeTemplatePMService';
import {FroalaEditorComponent} from '../../../../Infrastructure/Components/FroalaEditorComponent/FroalaEditorComponent';
import {DocumentTypeTemplateFilter} from './DocsOut/Filters/DocumentTypeTemplateFilter';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
declare var System: any;
declare var window: any;
import {HtmlEditorService} from '../../../../Common/Services/DocumentServices/HtmlEditorService';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
declare var StringToBase64, Base64ToString: any;
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';

@Component({
    moduleId: module.id,
    selector: 'HeaderAndFooter',
    templateUrl: './HeaderAndFooterComponent.html',
    providers: [DocumentTypeTemplatePMExtendedService, DocumentTypeTemplatePMService, HtmlEditorService]
})

export class HeaderAndFooterComponent implements OnInit {
 
    ChildObjectTableId: string;
    froalaEditorSetting: FroalaEditorSetting;
    OnHeaderAndFooterCompleteEvent = new EventEmitter();
    DocumentTypeCode: string;
    ObjectTableId: string;
    TemplateId: string;
    Tenant: number;
    PageType: string;
    HeightLable: string;
    HeightValue: number;
    HtmlString: string;
    Mode: string;
    IsShowAddDataField: boolean;
    OldDataTemplateByte: any = null;
    public DocumentTypeTemplatePMLists: any[];
    public documentTypeTemplatePM: DocumentTypeTemplatePM;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private documentTypeTemplatePMService: DocumentTypeTemplatePMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _documentTypeTemplatePMExtendedService: DocumentTypeTemplatePMExtendedService, private cd: ChangeDetectorRef, public _htmlEditorService: HtmlEditorService) {
        if (this.documentTypeTemplatePMService == null) {
            this.documentTypeTemplatePMService = new DocumentTypeTemplatePMService();

        }

    }

    ngOnInit(


    ) {

    }
    IsShowHeightBox: boolean = true;
    public DataViewModel: any;
    PageRequse: string;
    SetWindowArgs(args: any) {



        this.froalaEditorSetting = new FroalaEditorSetting();
        this.documentTypeTemplatePM = args.DocumentTypeTemplatePM;
        this.PageType = args.PageType;
        this.HtmlString = args.HtmlString;
        this.HeightValue = args.HeightValue;
        this.ObjectTableId = args.ObjectTableId;
        this.DataViewModel = args.DataViewModel;
        this.ChildObjectTableId = args.ChildObjectTableId ? args.ChildObjectTableId : "";

        this.PageRequse = args.PageRequse;
        this.OnHeaderAndFooterCompleteEvent = args.OnHeaderAndFooterCompleteEvent;



        if (this.PageRequse == "Send") this.IsShowHeightBox = false;

        if (!this.HeightValue) {
            this.HeightValue = 0;
        }
        this.HeightLable = this.PageType == "Header" ? "Header height" : "Footer height";

        if (this.documentTypeTemplatePM) {
            this.HeightValue = this.PageType == "Header" ? this.documentTypeTemplatePM.TemplateHeaderHeight : this.documentTypeTemplatePM.TemplateFooterHeight;
            this.IsShowAddDataField = true;
        }
        else this.IsShowAddDataField = false;


        this.Run();


    }






    Run() {


        this.froalaEditorSetting.PageType = "HeaderAndFooter";

        this.froalaEditorSetting.Id = Guid.newGuid();

        this.froalaEditorSetting.Height = 145;

        if (this.documentTypeTemplatePM) {
            this.FillData();
        }
        else {

            this.froalaEditorSetting.HtmlString = this.HtmlString;
            this.OldDataTemplateByte = StringToBase64(this.froalaEditorSetting.HtmlString);

        }

    }



    FillData() {

        if (this.documentTypeTemplatePM) {
            if (this.froalaEditorSetting) {

                this.DocumentTypeCode = this.documentTypeTemplatePM.DocumentTypeCode;
                if (this.PageType == "Header") {
                    if (this.documentTypeTemplatePM.TemplateHeaderHtml) {
                        this.froalaEditorSetting.HtmlString = Base64ToString(this.documentTypeTemplatePM.TemplateHeaderHtml);
                        
                    }

                    if (this.DataViewModel) {
                        this.froalaEditorSetting.HtmlString = !AppTool.IsNullOrEmpty(this.DataViewModel.TemplateHeaderHtml) ? Base64ToString(this.DataViewModel.TemplateHeaderHtml):"";
                        this.HeightValue = this.DataViewModel.TemplateHeaderHeight;
                    }

                }
                else if (this.PageType == "Footer") {
                    if (this.documentTypeTemplatePM.TemplateFooterHtml) {
                        this.froalaEditorSetting.HtmlString = Base64ToString(this.documentTypeTemplatePM.TemplateFooterHtml);
                    }


                    if (this.DataViewModel) {
                        this.froalaEditorSetting.HtmlString = !AppTool.IsNullOrEmpty(this.DataViewModel.TemplateFooterHtml) ? Base64ToString(this.DataViewModel.TemplateFooterHtml) : "";
                        this.HeightValue = this.DataViewModel.TemplateFooterHeight;
                    }



                }

                this.OldDataTemplateByte = StringToBase64(this.froalaEditorSetting.HtmlString);

            }

            this.CurrentSession.CurrentWindow.StopBusyIndicator();
        }

    }


    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }




    CloseButtonClicked() {
        this.CurrentSession.CurrentWindow.StopBusyIndicator();
        this.HtmlString = this.froalaEditorSetting.froalaEditorComponent.getHtml();

        if (!this.documentTypeTemplatePM && this.OnHeaderAndFooterCompleteEvent) {

            this.OnHeaderAndFooterCompleteEvent.emit(this);
        }

        this.DestroyfroalaEditor();
        this.CurrentSession.CurrentWindow.Close("");


    }


    DestroyfroalaEditor() {

        if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
            this.froalaEditorSetting.froalaEditorComponent.DestroyfroalaEditor();
        }

    }

    public ValidationErrorsList: string[];
    SaveButtonClicked() {

        var froalaString: string = "";

        if (this.documentTypeTemplatePM) {

            if (this.PageType == "Header") {
            
                this.documentTypeTemplatePM.TemplateHeaderHtml = StringToBase64(this.froalaEditorSetting.froalaEditorComponent.getHtml());
                this.documentTypeTemplatePM.TemplateHeaderHeight = this.HeightValue;

                if (this.DataViewModel) {

                    this.DataViewModel.TemplateHeaderHtml = this.documentTypeTemplatePM.TemplateHeaderHtml;
                    this.DataViewModel.TemplateHeaderHeight = this.documentTypeTemplatePM.TemplateHeaderHeight;
 
                }


            }
            else if (this.PageType == "Footer") {
                this.documentTypeTemplatePM.TemplateFooterHtml = StringToBase64(this.froalaEditorSetting.froalaEditorComponent.getHtml());
                this.documentTypeTemplatePM.TemplateFooterHeight = this.HeightValue;
                if (this.DataViewModel) {

                    this.DataViewModel.TemplateFooterHtml = this.documentTypeTemplatePM.TemplateFooterHtml;
                    this.DataViewModel.TemplateFooterHeight = this.documentTypeTemplatePM.TemplateFooterHeight;

                }


            }

            this.documentTypeTemplatePM.TemplateTechnologyCode = "AG";
        }


        this.ValidationErrorsList = [];
        if (this.HeightValue > 8 || this.HeightValue < 1) {

            if (this.HeightValue < 1 && AppTool.IsNullOrEmpty(this.froalaEditorSetting.froalaEditorComponent.getHtml())) 
            {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                this.CloseButtonClicked();
            }
            else this.ValidationErrorsList.push("Height should be have value between 1 cm and 8 cm");

        }
        else {
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            this.CloseButtonClicked();

        }







    }



    public ReloadFroalaEditor() {

        if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
            this.froalaEditorSetting.froalaEditorComponent.SetHtml(this.froalaEditorSetting.froalaEditorComponent.getHtml());
            this.froalaEditorSetting.froalaEditorComponent.ShowEditor();
        }

    }





    AddDataField(type: string) {


        var tableName = "";
        var tableId: string = !AppTool.IsNullOrEmpty(this.ChildObjectTableId) ? this.ChildObjectTableId : this.ObjectTableId;
        var table = window.ObjectTables.filter(d => d.Id == tableId)[0];
        if (table) tableName = table.Name;

        this._entityResourceService.getEntityResourceByTableName("SystemData").subscribe(response => {

            if (table) {
                this._entityResourceService.getEntityResourceByTableName(tableName).subscribe(response => {
                    this.ViewDataField(type, "", tableId);
                });
            }
            else this.ViewDataField(type, "", tableId);

        });


    }

    ViewDataField(type: string, objectTypeField: string, tableId: string) {

        var windowArgs: any = {};

        windowArgs.ObjectTypeField = objectTypeField;
        windowArgs.InSertDataFieldType = "FroalaEditor";
        windowArgs.DocumentTypeCode = this.DocumentTypeCode;
        windowArgs.ObjectTableId = tableId;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 600;

        logWindow.Title = "Insert Data Field";
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocumentObjectFieldsComponent');
        logWindow.WindowClosed.subscribe(($event: any) => {

            if ($event) {

                if (this.froalaEditorSetting.froalaEditorComponent) {
                    this.froalaEditorSetting.froalaEditorComponent.InSertHtml($event);
                    this.ReloadFroalaEditor();
                }

            }

        });
    }






}





