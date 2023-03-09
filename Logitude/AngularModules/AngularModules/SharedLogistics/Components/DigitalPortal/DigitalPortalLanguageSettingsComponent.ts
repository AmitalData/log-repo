declare var window: any;
import { Component, OnInit, EventEmitter, QueryList, ViewChildren } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { DocumentTypePM } from '../../../Common/EntityPMs/DocumentTypePM';
import { DocumentPermissiosViewModel } from '../ViewModel/DocumentPermissiosViewModel';
import { LocationDirective } from 'Infrastructure/Utilities/LocationDirective';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import { Guid } from 'Infrastructure/Utilities/Guid';
import { DigitalLanguageSettingsService } from '../../../Infrastructure/Services/WebServices/DigitalLanguageSettingsService'

@Component({
    templateUrl: './DigitalPortalLanguageSettingsComponent.html',
    inputs: ['OnCloseWindowEvent'],
    providers: [DigitalLanguageSettingsService]
})

export class DigitalPortalLanguageSettingsComponent implements OnInit {
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;

    public DocumentPermissiosSelectedViewModel: any;
    myTenantZeroList: DocumentTypePM[];
    myTenantList: DocumentTypePM[];
    DocumentPermissiosLists: DocumentPermissiosViewModel[];
    DocumentUploadPermissiosLists: DocumentPermissiosViewModel[];
    OnCloseWindowEvent = new EventEmitter();
    ObjectTableId: string;
    FullComponentsVisibility: boolean = false;
    SelectedTabCode: string;
    mySearchText: string;
    MainMessage: string;
    HasAgentDocumentsPermission: boolean = false;
    IsCloud: boolean = false;
    IsDigitalPortal: boolean = false;
    DigitalDisplayLanguageslList: { id: number, name: string, code: string, displayText: string }[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    UploadFileId: string;
    FileData: number;
    ExcelFileTypes: string = ".csv, .xls, .xlsx, text/csv, application/csv,text/comma-separated-values, application/csv, application/excel,application/vnd.msexcel, text/anytext, application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    IsUploadButtonEnabled: boolean;
    IsFileImportedSuccessfully: boolean;
    UploadedTranslationFileMSG: string = '';
    constructor(public _digitalLanguageSettingsService: DigitalLanguageSettingsService) {
        this.UploadFileId = Guid.NewRandomString();
    }

    ngOnInit() {
        this.IsCloud = ObjectsLocator.GlobalSetting.WorkEnvironment == "cloud";
        this.OnCloseWindowEvent.subscribe(($event: any) => {
            this.SaveButtonClicked();
        });
        this.IsUploadButtonEnabled = true;
        this.IsFileImportedSuccessfully = true;
        this.FillDisplayLanguages();
        this.Run();
        
    }

    Run() {
        this.SelectedTabCode = "ETV";
    }

    private selectedDisplayLanguage: any;
    get SelectedDisplayLanguage() { return this.selectedDisplayLanguage; }
    set SelectedDisplayLanguage(value) {
        if (this.selectedDisplayLanguage != value) {
            this.selectedDisplayLanguage = value;
        }
    }

    private FillDisplayLanguages() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.DigitalDisplayLanguageslList = [
            {
                id: 1, name: "English", code: 'EN', displayText: "English",
            },
            {
                id: 2, name: "Espanol", code: 'ES', displayText: "Espanol",
            }
        ];

        this.selectedDisplayLanguage = this.DigitalDisplayLanguageslList[0];
        this.CurrentSession.StopBusyIndicator();

        this._digitalLanguageSettingsService.GetDigitalLanguages().subscribe((myResult) => {
            if (!myResult.HasError) {
                this.DigitalDisplayLanguageslList = [];
                var languagesResult = myResult && myResult.Result ? myResult.Result : [];

                languagesResult.forEach(item => {
                    this.DigitalDisplayLanguageslList.push(
                        {
                            "id": item.Id,
                            "name": item.Name,
                            "code": item.Code,
                            "displayText": item.DisplayText
                        }
                    );
                });


                this.selectedDisplayLanguage = this.DigitalDisplayLanguageslList[0];
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }

    SelectedTabChange(selectedTabCode) {
        this.SelectedTabCode = selectedTabCode;
        this.selectedDisplayLanguage = this.DigitalDisplayLanguageslList[0];
        // if (!this.IsCloud) this.LoadEventCreationResultComponent();
    }

    SetWindowArgs(args: any) {
        this.FullComponentsVisibility = true;
        this.IsDigitalPortal = args.IsDigitalPortal;
    }

    ExportLanguage() {

    }

    OpenUpLoadFileToImportLanguage(){
        this.IsUploadButtonEnabled = false;
        this.IsFileImportedSuccessfully = false;
        this.UploadedTranslationFileMSG = ''
        document.getElementById(this.UploadFileId).click();
    }

    convertFileToBase64(file: any) {
        return new Promise((resolve, reject) => {
            const fileReader = new FileReader();
            fileReader.readAsDataURL(file);

            fileReader.onload = () => {
                resolve(fileReader.result);
            };

            fileReader.onerror = (error) => {
                reject(error);
            };
        });
    };

   async UploadFile(event: any) {
        
        var file: any = event && event.currentTarget && event.currentTarget.files ? event.currentTarget.files[0] : null;

        var allowedFileTypes = ['.csv', ' .xls', ' .xlsx', ' text/csv', ' application/csv', 'text/comma-separated-values', ' application/csv', ' application/excel', 'application/vnd.msexcel', ' text/anytext', ' application/vnd.ms-excel', 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet']
        
        if(!file || !allowedFileTypes.includes(file.type.toLowerCase())){
            this.IsUploadButtonEnabled = true;
            this.IsFileImportedSuccessfully = false;
            this.UploadedTranslationFileMSG = 'Excel files allowed only!'
            return;
        }

        this.CurrentSession.StartBusyIndicator('Uploading...');
        
        var fileInfo : any = {}
        if (file && file.size > 0) {
            fileInfo.Name = file.name;
            fileInfo.Base64 = await this.convertFileToBase64(file);
        }

        
        setTimeout(() => {
            this.CurrentSession.StopBusyIndicator();
            this.IsUploadButtonEnabled = true;
            this.IsFileImportedSuccessfully = true;
            this.UploadedTranslationFileMSG = 'File Imported Successfully.'
            event.target.value = '';
        }, 5000);

    }
    CloseButtonClicked() {
        this.CurrentSession.StopBusyIndicator();
        this.CurrentSession.CloseCurrentWindow();
    }

    SaveButtonClicked() {
        // this.CurrentSession.StartBusyIndicatorSaving();
    }

    // SharedDocumentsPermissionsComponentLoaded: boolean;
    // LoadEventCreationResultComponent() {
    //     if (!this.AllLocations) return;

    //     let myGeneratedComponentLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == 'AGV')[0];
    //     if (myGeneratedComponentLocation == null) {
    //         return;
    //     }
    //     if (this.SharedDocumentsPermissionsComponentLoaded) return;
    //     this.SharedDocumentsPermissionsComponentLoaded = true;
    //     SessionLocator.DynamicLoader.Load('./InfrastructureModules/InfrastructureDocuments/Components/SharedDocument/SharedDocumentsPermissionsComponent', myGeneratedComponentLocation.viewContainerRef)
    //         .then(cmpRef => {
    //             this.SharedDocumentPage = cmpRef.instance;
    //             this.SharedDocumentPage.FullComponentsVisibility = false;
    //             this.SharedDocumentPage.FromAgentView = true;
    //         });

    // }

}

