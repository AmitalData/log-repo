import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { Component } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { HelpResourcePM } from '../../../Infrastructure/EntityPMs/HelpResourcePM';
import { CodeNameClass } from '../../../Infrastructure/DataContracts/CodeNameClass';
import { HelpResourcePMService } from '../../../Infrastructure/Services/StandardPMs/HelpResourcePMService';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { UIProperty } from '../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
declare var querySelection, resultToUnitArray: any;

@Component({
    templateUrl: './NewHelpResouceComponent.html',
})

export class NewHelpResouceComponent extends BaseComponent {    
    private CurrentSession = SessionLocator.SelectedSession;
    private _entityResourceService: EntityResourceService;
    public DataContext: NewHelpResouceComponent = this;
    public ObjectTableName: string = "HelpResource";
    public EntityPM: HelpResourcePM;
    public IsResourcesReady: boolean = false;
    public uiProperty: UIProperty;
    constructor() {
        super();
        this._entityResourceService = new EntityResourceService();

        this._entityResourceService.getEntityResourceByTableName("HelpResource", 0).subscribe((response: any) => {
            this.IsResourcesReady = true;
            this.InitializeEntityPM();
            this.BuildLanguages();
            this.BuildCategories();
            this.BuildTypes();
            this.SetUIProperties();
        });
    }

    private InitializeEntityPM() {
        this.EntityPM = new HelpResourcePM();
        this.EntityPM.Tenant = 0;
        this.EntityPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
        this.EntityPM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
        this.EntityPM.IsNew = true;
    }

    public VideoPropertiesVisible: boolean = false;
    private SetUIProperties() {
        var videoPropertiesVisible: boolean = false;

        if (this.Type == "VID") {
            videoPropertiesVisible = true;

            this.UIProperties.SetRequired("VideoURL", this.ObjectTableName, AppTool.IsNullOrEmpty(this.VideoURL));
            this.UIProperties.SetRequired("Duration", this.ObjectTableName, AppTool.IsNullOrEmpty(this.Duration));
        }

        this.VideoPropertiesVisible = videoPropertiesVisible;
    }

    public LanguageList: CodeNameClass[] = [];
    public CategoryList: CodeNameClass[] = [];
    public TypeList: CodeNameClass[] = [];

    private BuildLanguages() {
        this.LanguageList = [];
        this.LanguageList.push(new CodeNameClass("EN", "English"));
        this.LanguageList.push(new CodeNameClass("FR", "French"));
        this.LanguageList.push(new CodeNameClass("SP", "Spanish"));
        this.LanguageList.push(new CodeNameClass("HE", "Hebrew"));
    }

    private BuildCategories() {
        this.CategoryList = [];
        this.CategoryList.push(new CodeNameClass("OPE", "Operational"));
        this.CategoryList.push(new CodeNameClass("ACC", "Accounting"));
        this.CategoryList.push(new CodeNameClass("AWB", "E-AWB"));
        this.CategoryList.push(new CodeNameClass("CRM", "CRM"));
    }

    private BuildTypes() {
        this.TypeList = [];
        this.TypeList.push(new CodeNameClass("TUT", "Tutorials"));
        this.TypeList.push(new CodeNameClass("HOW", "How To"));
        this.TypeList.push(new CodeNameClass("REL", "Release Notes"));
        this.TypeList.push(new CodeNameClass("VID", "Videos"));
    }

    private selectedLanguage: CodeNameClass;
    get SelectedLanguage() { return this.selectedLanguage; }
    set SelectedLanguage(value: CodeNameClass) {
        if (this.selectedLanguage != value) {
            this.selectedLanguage = value;

            if (value != null) {
                this.Language = value.Code;
            }

            else {
                this.Language = null;
            }
        }
    }

    private selectedCategory: CodeNameClass;
    get SelectedCategory() { return this.selectedCategory; }
    set SelectedCategory(value: CodeNameClass) {
        if (this.selectedCategory != value) {
            this.selectedCategory = value;

            if (value != null) {
                this.Category = value.Code;
            }

            else {
                this.Category = null;
            }
        }
    }

    private selectedType: CodeNameClass;
    get SelectedType() { return this.selectedType; }
    set SelectedType(value: CodeNameClass) {
        if (this.selectedType != value) {
            this.selectedType = value;

            if (value != null) {
                this.Type = value.Code;
            }

            else {
                this.Type = null;
            }

            this.VideoURL = null;
            this.Duration = null;
        }
    }

    get Name() { return this.EntityPM.Name; }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
        }
    }

    get Language() { return this.EntityPM.Language; }
    set Language(value: string) {
        if (this.EntityPM.Language != value) {
            this.EntityPM.Language = value;

            this.uiProperty = this.DataContext.UIProperties.GetUIProperty("Language", this.ObjectTableName, this.DataContext);
            this.uiProperty.UIPropertyChanged.emit(this.uiProperty);
        }
    }

    get Category() { return this.EntityPM.Category; }
    set Category(value: string) {
        if (this.EntityPM.Category != value) {
            this.EntityPM.Category = value;

            this.uiProperty = this.DataContext.UIProperties.GetUIProperty("Category", this.ObjectTableName, this.DataContext);
            this.uiProperty.UIPropertyChanged.emit(this.uiProperty);
        }
    }

    get Type() { return this.EntityPM.Type; }
    set Type(value: string) {
        if (this.EntityPM.Type != value) {
            this.EntityPM.Type = value;

            this.uiProperty = this.DataContext.UIProperties.GetUIProperty("Type", this.ObjectTableName, this.DataContext);
            this.uiProperty.UIPropertyChanged.emit(this.uiProperty);

            this.SetUIProperties();
        }
    }

    get VideoURL() { return this.EntityPM.VideoURL; }
    set VideoURL(value: string) {
        if (this.EntityPM.VideoURL != value) {
            this.EntityPM.VideoURL = value;

            this.SetUIProperties();
        }
    }

    get Duration() { return this.EntityPM.Duration; }
    set Duration(value: string) {
        if (this.EntityPM.Duration != value) {
            this.EntityPM.Duration = value;

            this.SetUIProperties();
        }
    }

    get FileName() { return this.EntityPM.FileName; }
    set FileName(value: string) {
        if (this.EntityPM.FileName != value) {
            this.EntityPM.FileName = value;
        }
    }

    get IsNew() { return this.EntityPM.IsNew; }
    set IsNew(value: boolean) {
        if (this.EntityPM.IsNew != value) {
            this.EntityPM.IsNew = value;
        }
    }

    //VideoURLKeyUpMethod(url: string) {        
    //    if (!AppTool.IsNullOrEmpty(url)) {
    //        url = url.trim();

    //        var regex = /(http(s)?:\/\/.)?(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)/;
    //        var isOk = true;

    //        if (!regex.test(url)) {
    //            isOk = false;
    //            this.UIProperties.SetValidity("VideoURL", this.ObjectTableName, isOk, url + " has invalid format");                
    //        }
    //    }
    //}

    //DurationKeyUpMethod(duration: string) {
    //    if (!AppTool.IsNullOrEmpty(duration)) {
    //        duration = duration.trim();

    //        var regex = /^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$/;
    //        var isOk = true;

    //        if (!regex.test(duration)) {
    //            isOk = false;
    //            this.UIProperties.SetValidity("Duration", this.ObjectTableName, isOk, duration + " has invalid format");
    //        }
    //    }
    //}

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    public ValidationErrorsList: string[];
    OkButtonClicked() {
        this.ValidationErrorsList = [];

        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, this.ValidationErrorsList);

        if (this.Type == "VID") {
            if (AppTool.IsNullOrEmpty(this.VideoURL)) {
                this.ValidationErrorsList.push("Video URL is required");
            }

            //else {
            //    var regex = /(http(s)?:\/\/.)?(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)/;
                
            //    if (!regex.test(this.VideoURL)) {
            //        this.ValidationErrorsList.push("Video URL has invalid format");
            //    }
            //}

            if (AppTool.IsNullOrEmpty(this.Duration)) {
                this.ValidationErrorsList.push("Duration URL is required");
            }

            //else {
            //    var regex = /^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$/;

            //    if (!regex.test(this.Duration)) {
            //        this.ValidationErrorsList.push("Duration has invalid format");
            //    }
            //}
        }

        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");

            var myService: HelpResourcePMService = new HelpResourcePMService();
            myService.insert(this.EntityPM).subscribe((myResult: any) => {
                if (myResult) {
                    this.CurrentSession.CurrentWindow.StopBusyIndicator();

                    if (myResult.HasError) {
                        this.ValidationErrorsList = myResult.ErrorsArray;
                    }

                    else {
                        this.CurrentSession.CloseCurrentWindow();
                    }
                }
            });
        }
    }

    DocumentFileId: string = Guid.NewRandomString();
    UploadBodyData: any;
    UploadButtonClicked() {
        document.getElementById(this.DocumentFileId).click();
    }
    UpLoadFileMethod(event: any) {
        var file = querySelection(this.DocumentFileId);

        if (file) {
            this.ArrayBufferToBase64(file, this);
        }
    }
    ArrayBufferToBase64(file: any, viewmodel: any) {

        var reader: FileReader = new FileReader();

        var extension: string = "";
        var fileInfo = file.name.split('.');

        if (fileInfo.length > 1) {
            extension = fileInfo[fileInfo.length - 1];
        }
        else extension = fileInfo[1];

        if (extension) {
            extension = extension.toLowerCase();
        }

        reader.onload = function (e) {
            var binary = '';
            var bytes = new Uint8Array(resultToUnitArray(e));
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }

            viewmodel.UploadBodyData = window.btoa(binary);
            viewmodel.EntityPM.File = viewmodel.UploadBodyData;
            viewmodel.EntityPM.FileExtension = extension;
        };

        reader.onerror = function (e) {

        };

        reader.readAsArrayBuffer(file);
    }
}
