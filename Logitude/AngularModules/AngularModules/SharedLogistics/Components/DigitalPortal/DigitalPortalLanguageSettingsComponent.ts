declare var window: any;
import { Component, OnInit, EventEmitter, QueryList, ViewChildren } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { DocumentTypePM } from '../../../Common/EntityPMs/DocumentTypePM';
import { DocumentPermissiosViewModel } from '../ViewModel/DocumentPermissiosViewModel';
import { LocationDirective } from 'Infrastructure/Utilities/LocationDirective';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import { Guid } from 'Infrastructure/Utilities/Guid';
import { DigitalLanguageSettingsService } from '../../../Infrastructure/Services/WebServices/DigitalLanguageSettingsService'
import { ServiceHelper } from 'Infrastructure/Utilities/ServiceHelper';
import { SessionInfo } from 'Infrastructure/Utilities/SessionInfo';

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
    DigitalDisplayLanguageslList: { name: string, code: string, displayText: string }[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    UploadFileId: string;
    FileData: number;
    ExcelFileTypes: string = ".csv, .xls, .xlsx, text/csv, application/csv,text/comma-separated-values, application/csv, application/excel,application/vnd.msexcel, text/anytext, application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    IsUploadButtonEnabled: boolean;
    IsFileImportedSuccessfully: boolean;
    UploadedTranslationFileMSG: string = '';
    ExportedExcelFileName: string = '';
    ExportErrorMsg: string | null = null;
    ExportedExcelLogId: string = '';
    IsFileReadyToExport: boolean = false;
    constructor(public _digitalLanguageSettingsService: DigitalLanguageSettingsService) {
        this.UploadFileId = Guid.NewRandomString();
    }

    ngOnInit() {
        this.IsCloud = ObjectsLocator.GlobalSetting.WorkEnvironment == "cloud";
        this.IsUploadButtonEnabled = true;
        this.IsFileImportedSuccessfully = true;
        this.Run();
    }

    Run() {
        this.SelectedTabCode = "ETV";
        this.FillDisplayLanguages();
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
        this._digitalLanguageSettingsService.GetDigitalLanguages().subscribe((myResult) => {
            if (!myResult.HasError) {
                this.DigitalDisplayLanguageslList = [];
                var languagesResult = myResult && myResult.Result ? myResult.Result : [];

                languagesResult.forEach(item => {
                    this.DigitalDisplayLanguageslList.push(
                        {
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
    }

    SetWindowArgs(args: any) {
        this.FullComponentsVisibility = true;
        this.IsDigitalPortal = args.IsDigitalPortal;
    }

    GetDigitalExportExecutionLogStatus(){   
        if(this.IsFileReadyToExport){
            return;
        }

        this._digitalLanguageSettingsService.getDigitalExportExecutionLogStatus(this.ExportedExcelLogId).subscribe((myResult: any) => {
            if (myResult) {
                if (myResult.Result && myResult.Result.StatusCode === "D") {
                    this.CurrentSession.StopBusyIndicator();
                    this.IsFileReadyToExport = true;
                    var _documentDownloadToken : any = SessionInfo.DocumentDownloadToken
                    var _link = ServiceHelper.GetLogitudeURL() + "/WebPages/DawnLoadExcelPage.aspx?Type=SaveToMicrosoftExcel2007&fileName=" + this.ExportedExcelFileName + "&tempId=" + _documentDownloadToken + "&requestArea=SharedLogistic" 
                    window.open(_link);
                  }else if (myResult.Result && myResult.Result.StatusCode === "F") {
                    this.CurrentSession.StopBusyIndicator();
                    this.ExportErrorMsg = 'Something went wrong, Exporting file failed!';
                  } else {
                    setTimeout(() => {
                      this.GetDigitalExportExecutionLogStatus();
                    }, 2000);
                  }
            }
        });
      }

    ExportLanguage() {
        this.IsFileReadyToExport= false;
        this.CurrentSession.StartBusyIndicator("Exporting...")
        var payload : ExportExcelParams = {
            ObjectTableName: "DigitalLabelTranslations",
            LanguageCode: this.selectedDisplayLanguage && this.selectedDisplayLanguage.code ? this.selectedDisplayLanguage.code : 'EN',
            CardId: null,
            Tenant: 0,
            PageIndex:0,
            PageSize:0,
            SortBy:"",
            SortDirection:"",
            GetCount:true,
            GetAll:false,
            DontApplyVirtualization:false,
            CardType:"",
            ObjectTableId:"",
            ProfileCode:"",
            AdditionalFilters:[],
        };


        this._digitalLanguageSettingsService.GetDigitalToExcelData(payload).subscribe((myResult: any) => {
            if (myResult) {
                this.ExportedExcelFileName = myResult.FileName;
                this.ExportedExcelLogId = myResult.ExecutionLogId;
                setTimeout(() => {
                    this.GetDigitalExportExecutionLogStatus()
                }, 2000);
            }else{
                this.CurrentSession.StopBusyIndicator();
                this.ExportErrorMsg = 'Something went wrong, Exporting file failed!';
            }
        });
    }

    OpenUpLoadFileToImportLanguage(){
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
            this.UploadedTranslationFileMSG = 'Only excel files allowed!'
            return;
        }

        this.CurrentSession.StartBusyIndicator('Uploading...');
        
        var fileInfo : ImportExcelParams = {
            Tenant: 0,
            FileData: null,
            LanguageCode: this.selectedDisplayLanguage && this.selectedDisplayLanguage.code ? this.selectedDisplayLanguage.code : 'EN',
        }

        if (file && file.size > 0) {
            var fileAs64Base = await this.convertFileToBase64(file);
            fileInfo.FileData = fileAs64Base && typeof fileAs64Base === 'string' ? fileAs64Base.split('base64,')[1] : ''
            this.ImportTextCodesExcelFile(fileInfo, event)
        }
    }

    ImportTextCodesExcelFile(fileInfo: ImportExcelParams, event: any) {
        this._digitalLanguageSettingsService.UploadDigitalTextCode(fileInfo).subscribe((myResult: any) => {
            this.CurrentSession.StopBusyIndicator();
            this.IsUploadButtonEnabled = true;
            event.target.value = '';
            if (myResult && myResult.HasError) {
                this.IsFileImportedSuccessfully = false;
                var errorMSG = myResult && myResult.ErrorsArray ? myResult.ErrorsArray[0] : '';
                this.UploadedTranslationFileMSG = 'Importing file failed!. ' + errorMSG + '.';
            }
            else {
                this.IsFileImportedSuccessfully = true;
                this.UploadedTranslationFileMSG = 'File Imported Successfully.'
            }
        });
    }

    CloseButtonClicked() {
        this.CurrentSession.StopBusyIndicator();
        this.CurrentSession.CloseCurrentWindow();
    }
}


export class ExportExcelParams {
    ObjectTableName: string;
    LanguageCode: string;
    CardId: string | null;
    Tenant: number;
    PageIndex: number;
    PageSize: number;
    SortBy: string;
    SortDirection: string;
    GetCount: boolean;
    GetAll: boolean;
    DontApplyVirtualization: boolean;
    CardType: string;
    ObjectTableId: string;
    ProfileCode: string;
    AdditionalFilters: [];

    constructor() {

    }
}

export class ImportExcelParams {
    Tenant: number;
    FileData: any;
    LanguageCode: string

    constructor() {

    }
}

