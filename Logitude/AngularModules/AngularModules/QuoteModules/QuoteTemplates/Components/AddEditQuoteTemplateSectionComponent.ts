declare var window: any;
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {QuoteTemplateSectionPM} from '../../../Quote/EntityPMs/QuoteTemplateSectionPM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {QuoteTemplateSectionPMService} from '../../../Quote/Services/StandardPMs/QuoteTemplateSectionPMService';
import {EditQuoteTemplateComponent, QuoteTemplateSectionViewModel} from './EditQuoteTemplateComponent';
import {FroalaEditorSetting} from '../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/FroalaEditorSetting';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {QuoteTemplateSectionExtendedPMService} from '../../../Quote/Services/ExtendedPMs/QuoteTemplateSectionExtendedPMService';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
declare var StringToBase64, Base64ToString: any;
import {AppTool} from '../../../Infrastructure/Tools';

@Component({
    selector: 'AddEditQuoteTemplateSectionComponent',
    
    templateUrl: './AddEditQuoteTemplateSectionComponent.html',
})

export class AddEditQuoteTemplateSectionComponent extends BaseComponent implements OnInit {
    quoteTemplateSectionPMService: QuoteTemplateSectionPMService;
    FatherComponent: EditQuoteTemplateComponent;
    quoteTemplateSectionExtendedPMService: QuoteTemplateSectionExtendedPMService;
    QuoteTemplateSectionViewModel: QuoteTemplateSectionViewModel;
    froalaEditorSetting: FroalaEditorSetting;
    public ValidationErrorsList: string[];
    IsNewQuoteTemplateSession: boolean;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    QuoteTemplateId: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.quoteTemplateSectionPMService = new QuoteTemplateSectionPMService();
        this.quoteTemplateSectionExtendedPMService = new QuoteTemplateSectionExtendedPMService();
    }

    ngOnInit() {

    }

    SetWindowArgs(args: any) {
        this.froalaEditorSetting = new FroalaEditorSetting();
        this.froalaEditorSetting.Id = Guid.newGuid();
        this.froalaEditorSetting.Height = (args.HeightWindow - 175);
        this.QuoteTemplateId = args.QuoteTemplateId;
        this.FatherComponent = args.FatherComponent;
        this.QuoteTemplateSectionViewModel = args.QuoteTemplateSectionViewModel;
        this.IsNewQuoteTemplateSession = this.QuoteTemplateSectionViewModel ? false : true;

        if (this.IsNewQuoteTemplateSession) this.QuoteTemplateSectionViewModel = this.GetNewInstance();
        this.Name = this.QuoteTemplateSectionViewModel.Name;

        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteTemplate.M.Loading"));





        var sectionDocId: string = !AppTool.IsNullOrEmpty(this.QuoteTemplateSectionViewModel.EntityPM.SectionDocId) ? this.QuoteTemplateSectionViewModel.EntityPM.SectionDocId : "";

        this.quoteTemplateSectionExtendedPMService.DownloadQuoteTemplateSectionPdfFile(this.QuoteTemplateSectionViewModel.QuoteTemplateSectionTypeCode, sectionDocId, "", "", "", "", SessionLocator.Tenant).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError && pmResponse.Result) {
                this.ReloadFroalaEditor(pmResponse.Result);
            }
        });
    }

    ReloadFroalaEditor(html: string = null) {
        if (!html) {
            if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
                html = this.froalaEditorSetting.froalaEditorComponent.getHtml();
            }
        }

        else {
            this.froalaEditorSetting.HtmlString = html;
        }

        if (this.froalaEditorSetting && this.froalaEditorSetting.froalaEditorComponent) {
            this.froalaEditorSetting.froalaEditorComponent.SetHtml(html);
            this.froalaEditorSetting.froalaEditorComponent.ShowEditor();
        }
    }

    GetNewInstance() {
        this.IsNewQuoteTemplateSession = true;

        var newQuoteTemplateSectionPM: QuoteTemplateSectionPM = new QuoteTemplateSectionPM();
        newQuoteTemplateSectionPM.Tenant = SessionLocator.TenantPM.Id;
        newQuoteTemplateSectionPM.QuoteTemplateId = this.QuoteTemplateId;
        newQuoteTemplateSectionPM.QuoteTemplateSectionTypeCode = "S";
        newQuoteTemplateSectionPM.Order = this.FatherComponent.AllQuoteTemplateSectionLists.length - 1;

        var footerItem = this.FatherComponent.AllQuoteTemplateSectionLists.filter(d => d.QuoteTemplateSectionTypeCode == "PF")[0];
        if (footerItem) {
            newQuoteTemplateSectionPM.Order = footerItem.Order;
        }

        return new QuoteTemplateSectionViewModel(newQuoteTemplateSectionPM);
    }

    private name = "";
    get Name() {
        return this.name;
    }
    set Name(newValue: string) {
        this.name = newValue;
    }

    UpdateQuoteTemplateSession(item: QuoteTemplateSectionPM, type: string) {
        this.quoteTemplateSectionPMService.update(item).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                if (pmResponse.Result) {
                    if (type == "Section") {
                        this.QuoteTemplateSectionViewModel.EntityPM = pmResponse.Result;
                        this.QuoteTemplateSectionViewModel.EntityPM.IschangeBodySection = false;
                        this.QuoteTemplateSectionViewModel.SectionDocId = pmResponse.Result.SectionDocId;

                    }
                    else if (type == "Footer") {
                        if (this.FatherComponent.AllQuoteTemplateSectionLists.filter(d => d.QuoteTemplateSectionTypeCode == "PF")[0]) {
                            this.FatherComponent.AllQuoteTemplateSectionLists.filter(d => d.QuoteTemplateSectionTypeCode == "PF")[0].EntityPM = pmResponse.Result;
                        }
                    }
                }

                this.CurrentSession.StopBusyIndicator();
                this.CurrentSession.CurrentWindow.Close("Refresh");
            }

            else {
                this.CloseButtonClicked();
            }
        });
    }

    AddDataField() {
        var tableId: string = "";
        var table = window.ObjectTables.filter(d => d.Name == "Quote")[0];
        if (table) tableId = table.Id;

        this._entityResourceService.getEntityResourceByTableName("SystemData").subscribe((response:any) => {
            this._entityResourceService.getEntityResourceByTableName("Quote").subscribe((response:any) => {
                var windowArgs: any = {};
                windowArgs.InSertDataFieldType = "FroalaEditor";
                windowArgs.ObjectTableId = tableId;
                windowArgs.FromComponent = "QuotationComponent";

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
            });
        });
    }

    SaveButtonClicked() {
        this.ValidationErrorsList = [];
        if (AppTool.IsNullOrEmpty(this.Name)) {
            this.ValidationErrorsList.push("Name field is required");
        }
        else if (this.Name.length > 60) this.ValidationErrorsList.push("Name field must be less than 60 and more than 1");


        if (this.ValidationErrorsList.length == 0) {

            var htmlbody: string = this.froalaEditorSetting.froalaEditorComponent.getHtml();
            var quotetemplateSectionBody = StringToBase64(htmlbody);
            this.QuoteTemplateSectionViewModel.Name = this.Name;
            this.QuoteTemplateSectionViewModel.DisplayName = this.Name;

            if (this.IsNewQuoteTemplateSession) {


                this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));



                this.QuoteTemplateSectionViewModel.Templatedata = quotetemplateSectionBody;
                this.RefreshSectionToList();
                this.quoteTemplateSectionPMService.insert(this.QuoteTemplateSectionViewModel.EntityPM).subscribe((res:any) => {
                    var pmResponse: ServiceResponse = res;

                    if (!pmResponse.HasError) {

                        this.QuoteTemplateSectionViewModel.EntityPM = pmResponse.Result;
                        this.QuoteTemplateSectionViewModel.SectionDocId = pmResponse.Result.SectionDocId;
                        this.QuoteTemplateSectionViewModel.Id = pmResponse.Result.Id;
                        this.FatherComponent.SelectQuoteTemplateSection = this.QuoteTemplateSectionViewModel;

                        var footerItem = this.FatherComponent.AllQuoteTemplateSectionLists.filter(d => d.QuoteTemplateSectionTypeCode == "PF")[0];
                        this.UpdateQuoteTemplateSession(footerItem.EntityPM, "Footer");
                    } else this.CloseButtonClicked();

                });
            }
            else {

                this.QuoteTemplateSectionViewModel.EntityPM.IschangeBodySection = this.QuoteTemplateSectionViewModel.HtmlBody != htmlbody ? true : false;

                if (this.QuoteTemplateSectionViewModel.EntityPM.IsDirty || this.QuoteTemplateSectionViewModel.EntityPM.IschangeBodySection) {
                    this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteTemplate.M.Saving"));

                    this.QuoteTemplateSectionViewModel.Templatedata = quotetemplateSectionBody;

                    this.QuoteTemplateSectionViewModel.EntityPM.Templatedata = quotetemplateSectionBody;

                    this.QuoteTemplateSectionViewModel.HtmlBody = "";
                    this.QuoteTemplateSectionViewModel.IsLoaded = false;
                    this.UpdateQuoteTemplateSession(this.QuoteTemplateSectionViewModel.EntityPM, "Section");
                }
                else this.CloseButtonClicked();

            }
        }
        else {
            this.froalaEditorSetting.Height -= 20;
            this.ReloadFroalaEditor();
        }
    }

    RefreshSectionToList() {

        var footerItem = this.FatherComponent.AllQuoteTemplateSectionLists.filter(d => d.QuoteTemplateSectionTypeCode == "PF")[0];

        if (footerItem) {
            this.QuoteTemplateSectionViewModel.Order = footerItem.Order;
            this.QuoteTemplateSectionViewModel.HtmlBody = "";
            this.QuoteTemplateSectionViewModel.IsLoaded = false;


            var index = this.FatherComponent.AllQuoteTemplateSectionLists.indexOf(footerItem);
            if (index != -1) this.FatherComponent.AllQuoteTemplateSectionLists.splice(index, 1);
            footerItem.Order += 1;
            this.FatherComponent.AllQuoteTemplateSectionLists.push(this.QuoteTemplateSectionViewModel);
            this.FatherComponent.AllQuoteTemplateSectionLists.push(footerItem);


        }


    }

    CloseButtonClicked() {
        this.CurrentSession.StopBusyIndicator();
        this.CurrentSession.CurrentWindow.Close("");
    }
}


