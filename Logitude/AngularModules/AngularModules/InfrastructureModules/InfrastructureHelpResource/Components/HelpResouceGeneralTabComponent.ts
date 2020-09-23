import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { Component, OnInit } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { AppTool } from '../../../Infrastructure/Tools';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { HelpResourcePM } from '../../../Infrastructure/EntityPMs/HelpResourcePM';
import { CodeNameClass } from '../../../Infrastructure/DataContracts/CodeNameClass';
import { UIProperty } from '../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import { Guid } from '../../../Infrastructure/Utilities/Guid';
declare var querySelection, resultToUnitArray: any;

@Component({
    templateUrl: './HelpResouceGeneralTabComponent.html',
})

export class HelpResouceGeneralTabComponent extends BaseComponent implements OnInit {
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext: HelpResouceGeneralTabComponent = this;
    public ObjectTableName: string = "HelpResource";
    public EntityPM: HelpResourcePM;
    public IsResourcesReady: boolean = false;
    public uiProperty: UIProperty;
    constructor(public entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        super();

        this.EntityPM = this.entityArgs.EntityPM;

        entityResourceService.getEntityResourceByTableName("HelpResource", 0).subscribe((response: any) => {            
            this.IsResourcesReady = true;
        });
    }

    ngOnInit() {
        if (this.EntityPM != null) {
            this.BuildLanguages();
            this.BuildCategories();
            this.BuildTypes();
            this.SetUIProperties();
        }
    }

    public VideoPropertiesVisible: boolean = false;
    private SetUIProperties() {
        var videoPropertiesVisible: boolean = false;

        this.UIProperties.SetEnabled("FileName", this.ObjectTableName, false);

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

        this.selectedLanguage = this.LanguageList.filter(d => d.Code == this.EntityPM.Language)[0];
    }

    private BuildCategories() {
        this.CategoryList = [];
        this.CategoryList.push(new CodeNameClass("OPE", "Operational"));
        this.CategoryList.push(new CodeNameClass("ACC", "Accounting"));
        this.CategoryList.push(new CodeNameClass("AWB", "E-AWB"));
        this.CategoryList.push(new CodeNameClass("CRM", "CRM"));

        this.selectedCategory = this.CategoryList.filter(d => d.Code == this.EntityPM.Category)[0];
    }

    private BuildTypes() {
        this.TypeList = [];
        this.TypeList.push(new CodeNameClass("TUT", "Tutorials"));
        this.TypeList.push(new CodeNameClass("HOW", "How To"));
        this.TypeList.push(new CodeNameClass("REL", "Release Notes"));
        this.TypeList.push(new CodeNameClass("VID", "Videos"));

        this.selectedType = this.TypeList.filter(d => d.Code == this.EntityPM.Type)[0];
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

    get Inactive() { return this.EntityPM.Inactive; }
    set Inactive(value: boolean) {
        if (this.EntityPM.Inactive != value) {
            this.EntityPM.Inactive = value;
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

