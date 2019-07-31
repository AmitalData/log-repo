import 'rxjs/add/operator/map';
declare var System: any;
declare var window: any;
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {Component, OnInit}  from '@angular/core';
import {DocumentTypePM} from '../../../../Common/EntityPMs/DocumentTypePM';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {DocumentTypeTemplatePM} from '../../../../Common/EntityPMs/DocumentTypeTemplatePM';
import {DocumentTypeTemplateListExtendedService} from '../../../../Common/Services/ExtendedLists/DocumentTypeTemplateListExtendedService';
import {DocumentTypeTemplatePMExtendedService} from '../../../../Common/Services/ExtendedPMs/DocumentTypeTemplatePMExtendedService';
import {DocumentTypeTemplatePMService} from '../../../../Common/Services/StandardPMs/DocumentTypeTemplatePMService';
import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {AppTool} from '../../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {DocumentTypeTemplateViewModel} from '../DocumentComponent/DocsOut/ViewModel/DocumentTypeTemplateViewModel';
import {DocumentTypeTemplateComponent} from  './DocumentTypeTemplateComponent';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
declare var querySelection, StringToBase64, resultToUnitArray: any;

@Component({
    moduleId: module.id,
    selector: 'NewReportTemplate',
    templateUrl: './NewReportTemplateComponent.html',
    providers: [DocumentTypeTemplateListExtendedService, DocumentTypeTemplatePMService, DocumentTypeTemplatePMExtendedService]
})

export class NewReportTemplateComponent extends BaseComponent implements OnInit {
    DocumentType: DocumentTypePM;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    DocumentTypeTemplateLists: any[];
    public documentTypeTemplatePMService: DocumentTypeTemplatePMService;
    public DocumentTypeTemplateViewModelSelected: DocumentTypeTemplateViewModel;
    DocumentTypeTemplatePMLists: DocumentTypeTemplatePM[];
    FullDocumentTypeTemplateLists: any[];
    DocumentTemplateFileId: string = Guid.NewRandomString();
    Description: string;
    DocumentsGridVisibility: boolean = false;
    LoadMrtButtonVisibility: boolean = false;
    EditorTypeVisibility: boolean = false;

    ShowSaveButton: boolean = true;
    TypeTab: string;
    DataViewModel: DocumentTypeTemplateComponent;
    CloseButtonLable: string = "Cancel";
    ValueRadioChoice: string = "Blank";
    ValueEditorRadio: string = "StimulSoft";
    PageType: string;
    public ValidationErrorsList: string[];
    UploadTemplateBodyData: any;

    RadioButtonChoice1Id: string = Guid.newGuid();
    RadioButtonChoice2Id: string = Guid.newGuid();
    RadioButtonChoice3Id: string = Guid.newGuid();
    RadioButtonChoice4Id: string = Guid.newGuid();

    RadioEditorChoice1Id: string = Guid.newGuid();
    RadioEditorChoice2Id: string = Guid.newGuid();

    NameRadioButtonChoice: string = Guid.NewRandomString();
    NameRadioEditorChoice: string = Guid.NewRandomString();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _documentTypeTemplateListExtendedService: DocumentTypeTemplateListExtendedService, public _documentTypeTemplatePMExtendedService: DocumentTypeTemplatePMExtendedService) {
        super();

        if (this.documentTypeTemplatePMService == null) {
            this.documentTypeTemplatePMService = new DocumentTypeTemplatePMService();

        }

    }
    ngOnInit(


    ) {

    }

    ObjectTableId: string;
    IsLoadPage: boolean = false;
    SetWindowArgs(args: any) {


        this._entityResourceService.getEntityResourceByTableName("DocumentTypeTemplate").subscribe(response => {

            this.IsLoadPage = true;
            this.DocumentTypeTemplateLists = [];
            this.DocumentTypeTemplatePMLists = [];

            this.ObjectTableId = args.ObjectTableId;
            this.DataViewModel = args.DataViewModel;
            this.PageType = args.PageType;
            this.DocumentType = args.CurrentEntityPM;
            this.TypeTab = args.TypeTab;
            this.FullDocumentTypeTemplateLists = args.DocumentTypeTemplateLists;

            if (this.TypeTab == "Document") {
                this.ValueEditorRadio = "StimulSoft";
                this.EditorTypeVisibility = true;
            }
            else {
                this.EditorTypeVisibility = false;
                this.ValueEditorRadio = "RichText";
            }

        });
    }






    OnSelectedDocumentTypeTemplateLists(selectedItem: DocumentTypeTemplateViewModel) {
        this.DocumentTypeTemplateViewModelSelected = selectedItem;
        this.Description = selectedItem.Description;



        this.DocumentTypeTemplateLists.forEach((item) => {
            item.DivSelectBackgroud = "#ffffff";
        });

        selectedItem.DivSelectBackgroud = "#B6E0F5";
        this.DocumentTypeTemplateViewModelSelected = selectedItem;


    }

    GetNewStanceFromDocumentTypeTemplatePM() {
        var template = new DocumentTypeTemplatePM();
        template.Tenant = SessionInfo.LoggedUserTenant;
        template.DocumentTypeId = this.DocumentType.Id;
        template.LastUpdatedByUserId = SessionInfo.LoggedUserId;

        if (SessionInfo.LoggedUserPM) template.LastUpdateByUserName = SessionInfo.LoggedUserPM.EnglishName;

        template.Description = this.Description;
        template.Subject = this.DocumentType.Subject;
        template.DocumentTypeId = this.DocumentType.Id;
        template.IsEnabledForCustomers = true;
        template.IsCopiedAtSignup = true;
        if (this.TypeTab == "Document") {
            template.TemplateType = "P";
            template.EditorTool = this.ValueEditorRadio == "StimulSoft" ? "S" : "R";
        }

        else {
            template.TemplateType = "M";
            template.EditorTool = "R";
        }

        if (this.ValueRadioChoice == "FromFile") {
             if (template.EditorTool == "S") template.TemplateBody = this.UploadTemplateBodyData;

        }

        return template;
    }



    OpenUpLoadTemplateFile() {

        document.getElementById(this.DocumentTemplateFileId).click();

    }


    UpLoadTemplateFileMethod(event: any) {

        var file = querySelection(this.DocumentTemplateFileId);
        //document.querySelector('#DocumentTemplateFile').files[0];


        if (file) {
            this.ArrayBufferToBase64(file, this);
        }



    }


    ArrayBufferToBase64(file: any, viewmodel: any) {

        var reader: FileReader = new FileReader();
        var reader = new FileReader();
        reader.onload = function (e) {
            var binary = '';
            var bytes = new Uint8Array(resultToUnitArray(e));
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }
            viewmodel.UploadTemplateBodyData = window.btoa(binary);

        };

        reader.onerror = function (e) {

        };
        reader.readAsArrayBuffer(file);

    }

    GetCopyStanceFromDocumentTypeTemplatePM(selectedItem: DocumentTypeTemplatePM) {


        var template = new DocumentTypeTemplatePM();
        template.Tenant = this.DocumentType.Tenant;
        template.DocumentTypeId = selectedItem.DocumentTypeId;
        template.LastUpdatedByUserId = selectedItem.LastUpdatedByUserId;
        template.LastUpdateByUserName = selectedItem.LastUpdateByUserName;
        template.Description = this.Description;
        template.Subject = selectedItem.Subject;
        template.TemplateType = selectedItem.TemplateType;
        template.EditorTool = selectedItem.EditorTool;
        template.TemplateBody = selectedItem.TemplateBody;
        template.TemplateBodyHtml = selectedItem.TemplateBodyHtml;
        template.OriginalTemplateId = selectedItem.Id;
        template.OriginalTemplateName = selectedItem.Description;
        template.ReplyTo = selectedItem.ReplyTo;
        template.From = selectedItem.From;
        template.DocumentTypeId = this.DocumentType.Id;
        template.Language = selectedItem.Language;
        template.CountryCode = selectedItem.CountryCode;
        template.DocumentTypeCode = selectedItem.DocumentTypeCode;
        template.InternalRemarks = selectedItem.InternalRemarks;
        template.IsEnabledForCustomers = true;
        template.IsCopiedAtSignup = true;
        template.TemplateHeaderHtml = selectedItem.TemplateHeaderHtml;
        template.TemplateFooterHtml = selectedItem.TemplateFooterHtml;
        template.TemplateHeaderHeight = selectedItem.TemplateHeaderHeight;
        template.TemplateFooterHeight = selectedItem.TemplateFooterHeight;
        return template;
    }


    InsertDocumentTypeTemplatePm(newTemplatePm: any) {

        this.ValidationErrorsList = [];
        if (this.FullDocumentTypeTemplateLists.length == 0) {
            newTemplatePm.IsDefault = true;
        }



        this.documentTypeTemplatePMService.insert(newTemplatePm).subscribe(myResult=> {

            this.CurrentSession.CurrentWindow.StopBusyIndicator();

            if (myResult) {
                if (myResult.HasError) {
                    myResult.ErrorsArray.forEach((item) => {
                        this.ValidationErrorsList.push(item);
                    });
                }
                else {

                    var templateViewModel = new DocumentTypeTemplateViewModel(myResult.Result);
                    if (this.FullDocumentTypeTemplateLists.length == 0) {
                        if (this.TypeTab == "Document") {
                            this.DocumentType.DocumentTypeDefaultReportTemplateId = myResult.Result.Id;

                        }
                        else {
                            this.DocumentType.DocumentTypeDefaultHTMLTemplateId = myResult.Result.Id;
                        }
                    }

                    this.DataViewModel.DocumentTypeTemplateLists.push(templateViewModel);
                    this.DataViewModel.EditDocumentTemplate(templateViewModel);
                    this.CurrentSession.CurrentWindow.Close(myResult.Result.Id);
                }


            }


        }, error=> {
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            var dd: any = error;
            console.log(dd.text);
        })


    }

    RadioButtonChoice(choose: string) {

        this.ValueRadioChoice = choose;
        this.UploadTemplateBodyData = null;
        this.DocumentsGridVisibility = false;

        switch (choose) {
            case "Blank":
                {


                    this.ShowSaveButton = true;
                    this.CloseButtonLable = "Cancel";
                    break;
                }

            case "Duplicate":
                {
                    var editorTool = this.ValueEditorRadio == "StimulSoft" ? "S" : "R";
                    this.DocumentTypeTemplateLists = this.FullDocumentTypeTemplateLists ? this.FullDocumentTypeTemplateLists.filter(d=> d.InActive == false && d.EditorTool == editorTool) : [];
                    this.DocumentsGridVisibility = true;
                    this.ShowSaveButton = true;
                    this.CloseButtonLable = "Cancel";
                    break;
                }



            case "FromLibrary":
                {
                    this.ShowSaveButton = false;
                    this.DocumentsGridVisibility = true;
                    this.CloseButtonLable = "Close";
                    this.LoadDocumentTypeTemplateFromLibrary();


                    break;
                }

            case "FromFile":
                {
                    this.ShowSaveButton = true;
                    this.CloseButtonLable = "Cancel";
                    break;
                }



        }

    }

    CloseButtonClicked() {

        this.CurrentSession.CloseCurrentWindow();

    }


    EditorRadioButtonChoice(choose: string) {
        this.ValueEditorRadio = choose;

        if (this.ValueRadioChoice == "FromLibrary") {
            this.LoadDocumentTypeTemplateFromLibrary();
        }
        else {
            var editorTool = this.ValueEditorRadio == "StimulSoft" ? "S" : "R";
            this.DocumentTypeTemplateLists = this.FullDocumentTypeTemplateLists.filter(d=> d.InActive == false && d.EditorTool == editorTool);
        }



    }



    SaveButtonClicked() {

        this.ValidationErrorsList = [];

        if ((this.ValueRadioChoice == "FromLibrary" || this.ValueRadioChoice == "Duplicate") && !this.DocumentTypeTemplateViewModelSelected) {
            this.ValidationErrorsList.push("Please select at least template");
        }
        else { 
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
            switch (this.ValueRadioChoice) {
                case "Blank":
                    {

                        var newTemplatePm = this.GetNewStanceFromDocumentTypeTemplatePM()
                        this.InsertDocumentTypeTemplatePm(newTemplatePm);
                        break;
                    }

                case "Duplicate":
                    {
                        if (this.DocumentTypeTemplateViewModelSelected) {
                            var newTemplatePm = this.GetCopyStanceFromDocumentTypeTemplatePM(this.DocumentTypeTemplateViewModelSelected.Entity)
                            this.InsertDocumentTypeTemplatePm(newTemplatePm);
                        }

                        break;
                    }

                case "FromFile":
                    {
                        var newTemplatePm = this.GetNewStanceFromDocumentTypeTemplatePM();
                        if (newTemplatePm.EditorTool == "R") {
                            this._documentTypeTemplatePMExtendedService.ConvertXmalByteTojosnObject(this.UploadTemplateBodyData).subscribe(res => {
                                var pmResponse: ServiceResponse = res;
                                if (!pmResponse.HasError) {
                                    var myResult = pmResponse.Result;
                                    if (myResult) {
                                        var htmltemplate: any = myResult;
                                        if (htmltemplate) {

                                            htmltemplate.HeaderHtml = !AppTool.IsNullOrEmpty(htmltemplate.HeaderHtml) ? htmltemplate.HeaderHtml : "";
                                            htmltemplate.FooterHtml = !AppTool.IsNullOrEmpty(htmltemplate.FooterHtml) ? htmltemplate.FooterHtml : "";

                                            newTemplatePm.TemplateHeaderHtml = StringToBase64(htmltemplate.HeaderHtml);
                                            newTemplatePm.TemplateFooterHtml = StringToBase64(htmltemplate.FooterHtml);
                                            newTemplatePm.TemplateBodyHtml = StringToBase64(htmltemplate.BodyHtml);
                                            newTemplatePm.TemplateHeaderHeight = htmltemplate.HeaderHeight;
                                            newTemplatePm.TemplateFooterHeight = htmltemplate.FooterHeight;

                                        }
                                    }
                                }
                                this.InsertDocumentTypeTemplatePm(newTemplatePm);
                            });


                        } else this.InsertDocumentTypeTemplatePm(newTemplatePm);

                 
                        break;
                    }



            }
        }



    }



    PreviewFromLibraryButtonClicked(item: DocumentTypeTemplateViewModel) {
        this.DocumentTypeTemplateViewModelSelected = item;

        if (item.EditorTool == "R") {

            var windowArgs: any = {};
            windowArgs.DataViewModel = this;
            windowArgs.PageType = "Preview";
            windowArgs.TemplateId = item.Id;
            windowArgs.Tenant = item.Tenant;
            windowArgs.DocumentTypeTemplatePMLists = this.DocumentTypeTemplatePMLists;
            windowArgs.ObjectType = "DocumentTypeTemplateViewModel";
            windowArgs.EntityId = "";
            windowArgs.ObjectTableId = this.DocumentType.ObjectTableId;
            windowArgs.ChildEntityId = "";
            windowArgs.ChildObjectTableId = "";

            var widthwindow = window.innerWidth;
            var heighthwindow = window.innerHeight;

            var logWindow = new LogitudeWindow();
            logWindow.Width = widthwindow - 100;
            logWindow.Height = heighthwindow - 100;
            logWindow.Title = item.Description;


            logWindow.WindowArgs = windowArgs;
            logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/HtmlDocumentPreviewComponent");

        }
        else if (item.TemplateType == "P" && item.EditorTool == "S") {

            var token = ServiceHelper.GetLDocumentDownloadToken();
            window.open(ServiceHelper.GetLogitudeURL() + "WebPages/ReportPdfDownLoad.aspx?documentTypeTemplateId=" + item.Id + "&documentTypeTemplateName" + item.Description + "&entityId=" + "" + "&entityObjectTableId=" + "" + "&childEntityId=" + "" + "&childObjectTableId=" + "" + "&TemplateType=" + item.TemplateType + "&EditorTool=" + item.EditorTool + "&tempId=" + token);



        }


    }

    AddFromLibraryButtonClicked(item: DocumentTypeTemplateViewModel) {
        this.DocumentTypeTemplateViewModelSelected = item;
        // this.Description = this.DocumentTypeTemplateViewModelSelected.Description;
        if (!this.FullDocumentTypeTemplateLists.filter(d=> d.OriginalTemplateId == item.Id)[0]) {
            var template = this.DocumentTypeTemplatePMLists.filter(d=> d.Id == item.Id)[0];
            if (template) {
                var newTemplatePm = this.GetCopyStanceFromDocumentTypeTemplatePM(template)
                this.InsertDocumentTypeTemplatePm(newTemplatePm);
            }
            else {
                this.GetDocumentTypeTemplatePMFromLibrary(item);

            }

        }
        else {

            var confirmWindow = new ConfirmWindow();
            confirmWindow.Show("Please note that this template already exists, Please confirm to add a new template");
            confirmWindow.Title = "Document Type Template";
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    var template = this.DocumentTypeTemplatePMLists.filter(d=> d.Id == item.Id)[0];
                    if (template) {
                        var newTemplatePm = this.GetCopyStanceFromDocumentTypeTemplatePM(template)
                        this.InsertDocumentTypeTemplatePm(newTemplatePm);
                    }
                    else {
                        this.GetDocumentTypeTemplatePMFromLibrary(item);
                    }
                }
            });
        }
    }


    LoadDocumentTypeTemplateFromLibrary() {

        this.DocumentsGridVisibility = true;
        this.DocumentTypeTemplateLists = [];

        var Isfilter = true;
        if (FeatureLocator.HasFeaturePermession("DocumentType", "DOCUMENTTYPE")) {
            Isfilter = false;
        }

        var editorTool = this.ValueEditorRadio == "StimulSoft" ? "S" : "R";
        this._documentTypeTemplateListExtendedService.GetDocumentTypeTemplatesFromLibraryByDocumentTypeId(0, this.DocumentType.Id, Isfilter, this.DocumentType.Tenant).subscribe(res => {


            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    myResult.filter(d=> d.EditorTool == editorTool).forEach((item) => {
                        this.DocumentTypeTemplateLists.push(new DocumentTypeTemplateViewModel(item));

                    });
                }

            }

        });
    }



    GetDocumentTypeTemplatePMFromLibrary(item: DocumentTypeTemplateViewModel) {
        var id = item.Id + "@0";
        this.documentTypeTemplatePMService.get(id).subscribe(res=> {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    var template = this.DocumentTypeTemplatePMLists.filter(d=> d.Id == myResult.Id)[0];
                    if (!template) {
                        this.DocumentTypeTemplatePMLists.push(myResult);
                        var newTemplatePm = this.GetCopyStanceFromDocumentTypeTemplatePM(myResult)
                        this.InsertDocumentTypeTemplatePm(newTemplatePm);
                    }
                }
            }
            else this.CurrentSession.CurrentWindow.StopBusyIndicator();
        }, error=> {
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            var dd: any = error;
            console.log(dd.text);
        });
    }



}
