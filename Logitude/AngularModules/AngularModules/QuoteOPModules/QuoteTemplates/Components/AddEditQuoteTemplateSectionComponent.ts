declare var window: any;
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {QuoteOPTemplateSectionPM} from '../../../QuoteOPM/EntityPMs/QuoteOPTemplateSectionPM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {QuoteOPTemplateSectionPMService} from '../../../QuoteOPM/Services/StandardPMs/QuoteOPTemplateSectionPMService';
import {EditQuoteTemplateComponent, QuoteOPTemplateSectionViewModel} from './EditQuoteTemplateComponent';
import {FroalaEditorSetting} from '../../../InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/DocsOut/FroalaEditorSetting';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {QuoteOPTemplateSectionExtendedPMService} from '../../../QuoteOPM/Services/ExtendedPMs/QuoteOPTemplateSectionExtendedPMService';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
declare var StringToBase64, Base64ToString: any;
import {AppTool} from '../../../Infrastructure/Tools';

@Component({
    selector: 'AddEditQuoteTemplateSectionComponent',
    
    templateUrl: './AddEditQuoteTemplateSectionComponent.html',
})

export class AddEditQuoteTemplateSectionComponent extends BaseComponent implements OnInit {
    QuoteOPTemplateSectionPMService: QuoteOPTemplateSectionPMService;
    FatherComponent: EditQuoteTemplateComponent;
    QuoteOPTemplateSectionExtendedPMService: QuoteOPTemplateSectionExtendedPMService;
    QuoteOPTemplateSectionViewModel: QuoteOPTemplateSectionViewModel;
    froalaEditorSetting: FroalaEditorSetting;
    public ValidationErrorsList: string[];
    IsNewQuoteOPTemplateSession: boolean;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    QuoteOPTemplateId: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.QuoteOPTemplateSectionPMService = new QuoteOPTemplateSectionPMService();
        this.QuoteOPTemplateSectionExtendedPMService = new QuoteOPTemplateSectionExtendedPMService();
    }

    ngOnInit() {

    }

    SetWindowArgs(args: any) {
        this.froalaEditorSetting = new FroalaEditorSetting();
        this.froalaEditorSetting.Id = Guid.newGuid();
        this.froalaEditorSetting.Height = (args.HeightWindow - 175);
        this.QuoteTemplateId = args.QuoteTemplateId;
        this.FatherComponent = args.FatherComponent;
        this.QuoteOPTemplateSectionViewModel = args.QuoteOPTemplateSectionViewModel;
        this.IsNewQuoteOPTemplateSession = this.QuoteOPTemplateSectionViewModel ? false : true;

        if (this.IsNewQuoteOPTemplateSession) this.QuoteOPTemplateSectionViewModel = this.GetNewInstance();
        this.Name = this.QuoteOPTemplateSectionViewModel.Name;

        this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteOPTemplate.M.Loading"));





        var sectionDocId: string = !AppTool.IsNullOrEmpty(this.QuoteOPTemplateSectionViewModel.EntityPM.SectionDocId) ? this.QuoteOPTemplateSectionViewModel.EntityPM.SectionDocId : "";

        this.QuoteOPTemplateSectionExtendedPMService.DownloadQuoteOPTemplateSectionPdfFile(this.QuoteOPTemplateSectionViewModel.QuoteOPTemplateSectionTypeCode, sectionDocId, "", "", "", "", SessionLocator.Tenant).subscribe((res:any) => {
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
        this.IsNewQuoteOPTemplateSession = true;

        var newQuoteOPTemplateSectionPM: QuoteOPTemplateSectionPM = new QuoteOPTemplateSectionPM();
        newQuoteOPTemplateSectionPM.Tenant = SessionLocator.TenantPM.Id;
        newQuoteOPTemplateSectionPM.QuoteTemplateId = this.QuoteTemplateId;
        newQuoteOPTemplateSectionPM.QuoteOPTemplateSectionTypeCode = "S";
        newQuoteOPTemplateSectionPM.Order = this.FatherComponent.AllQuoteOPTemplateSectionLists.length - 1;

        var footerItem = this.FatherComponent.AllQuoteOPTemplateSectionLists.filter(d => d.QuoteOPTemplateSectionTypeCode == "PF")[0];
        if (footerItem) {
            newQuoteOPTemplateSectionPM.Order = footerItem.Order;
        }

        return new QuoteOPTemplateSectionViewModel(newQuoteOPTemplateSectionPM);
    }

    private name = "";
    get Name() {
        return this.name;
    }
    set Name(newValue: string) {
        this.name = newValue;
    }

    UpdateQuoteOPTemplateSession(item: QuoteOPTemplateSectionPM, type: string) {
        this.QuoteOPTemplateSectionPMService.update(item).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                if (pmResponse.Result) {
                    if (type == "Section") {
                        this.QuoteOPTemplateSectionViewModel.EntityPM = pmResponse.Result;
                        this.QuoteOPTemplateSectionViewModel.EntityPM.IschangeBodySection = false;
                        this.QuoteOPTemplateSectionViewModel.SectionDocId = pmResponse.Result.SectionDocId;

                    }
                    else if (type == "Footer") {
                        if (this.FatherComponent.AllQuoteOPTemplateSectionLists.filter(d => d.QuoteOPTemplateSectionTypeCode == "PF")[0]) {
                            this.FatherComponent.AllQuoteOPTemplateSectionLists.filter(d => d.QuoteOPTemplateSectionTypeCode == "PF")[0].EntityPM = pmResponse.Result;
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
        var table = window.ObjectTables.filter(d => d.Name == "QuoteOP")[0];
        if (table) tableId = table.Id;

        this._entityResourceService.getEntityResourceByTableName("SystemData").subscribe((response:any) => {
            this._entityResourceService.getEntityResourceByTableName("QuoteOP").subscribe((response:any) => {
                var windowArgs: any = {};
                windowArgs.InSertDataFieldType = "FroalaEditor";
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
            var QuoteOPTemplateSectionBody = StringToBase64(htmlbody);
            this.QuoteOPTemplateSectionViewModel.Name = this.Name;
            this.QuoteOPTemplateSectionViewModel.DisplayName = this.Name;

            if (this.IsNewQuoteOPTemplateSession) {


                this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteOPTemplate.M.Saving"));



                this.QuoteOPTemplateSectionViewModel.Templatedata = QuoteOPTemplateSectionBody;
                this.RefreshSectionToList();
                this.QuoteOPTemplateSectionPMService.insert(this.QuoteOPTemplateSectionViewModel.EntityPM).subscribe((res:any) => {
                    var pmResponse: ServiceResponse = res;

                    if (!pmResponse.HasError) {

                        this.QuoteOPTemplateSectionViewModel.EntityPM = pmResponse.Result;
                        this.QuoteOPTemplateSectionViewModel.SectionDocId = pmResponse.Result.SectionDocId;
                        this.QuoteOPTemplateSectionViewModel.Id = pmResponse.Result.Id;
                        this.FatherComponent.SelectQuoteOPTemplateSection = this.QuoteOPTemplateSectionViewModel;

                        var footerItem = this.FatherComponent.AllQuoteOPTemplateSectionLists.filter(d => d.QuoteOPTemplateSectionTypeCode == "PF")[0];
                        this.UpdateQuoteOPTemplateSession(footerItem.EntityPM, "Footer");
                    } else this.CloseButtonClicked();

                });
            }
            else {

                this.QuoteOPTemplateSectionViewModel.EntityPM.IschangeBodySection = this.QuoteOPTemplateSectionViewModel.HtmlBody != htmlbody ? true : false;

                if (this.QuoteOPTemplateSectionViewModel.EntityPM.IsDirty || this.QuoteOPTemplateSectionViewModel.EntityPM.IschangeBodySection) {
                    this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator.Translate("QuoteOPTemplate.M.Saving"));

                    this.QuoteOPTemplateSectionViewModel.Templatedata = QuoteOPTemplateSectionBody;

                    this.QuoteOPTemplateSectionViewModel.EntityPM.Templatedata = QuoteOPTemplateSectionBody;

                    this.QuoteOPTemplateSectionViewModel.HtmlBody = "";
                    this.QuoteOPTemplateSectionViewModel.IsLoaded = false;
                    this.UpdateQuoteOPTemplateSession(this.QuoteOPTemplateSectionViewModel.EntityPM, "Section");
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

        var footerItem = this.FatherComponent.AllQuoteOPTemplateSectionLists.filter(d => d.QuoteOPTemplateSectionTypeCode == "PF")[0];

        if (footerItem) {
            this.QuoteOPTemplateSectionViewModel.Order = footerItem.Order;
            this.QuoteOPTemplateSectionViewModel.HtmlBody = "";
            this.QuoteOPTemplateSectionViewModel.IsLoaded = false;


            var index = this.FatherComponent.AllQuoteOPTemplateSectionLists.indexOf(footerItem);
            if (index != -1) this.FatherComponent.AllQuoteOPTemplateSectionLists.splice(index, 1);
            footerItem.Order += 1;
            this.FatherComponent.AllQuoteOPTemplateSectionLists.push(this.QuoteOPTemplateSectionViewModel);
            this.FatherComponent.AllQuoteOPTemplateSectionLists.push(footerItem);


        }


    }

    CloseButtonClicked() {
        this.CurrentSession.StopBusyIndicator();
        this.CurrentSession.CurrentWindow.Close("");
    }
}


