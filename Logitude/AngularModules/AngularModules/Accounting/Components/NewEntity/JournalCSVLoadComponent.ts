import { Component, ChangeDetectorRef } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { TenantPM } from '../../../Common/EntityPMs/TenantPM';
import { JournalPM } from '../../EntityPMs/JournalPM';
import { JournalLinePM } from '../../EntityPMs/JournalLinePM';
import { BankAccountPM } from '../../EntityPMs/BankAccountPM';
import { GLAccountPM } from '../../EntityPMs/GLAccountPM';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { JournalPMService } from '../../Services/StandardPMs/JournalPMService';
import { CurrencyPMService } from '../../../Common/Services/StandardPMs/CurrencyPMService';
import { CurrencyListService } from '../../../Common/Services/StandardLists/CurrencyListService';
import { JournalExtendedPMService } from '../../Services/ExtendedPMs/JournalExtendedPMService';
import { EntityListService } from '../../../Infrastructure/Services/EntityListService';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';


import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { ImageParameter } from '../../../Infrastructure/DataContracts/ImageParameter';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { AccountingOpService } from '../../Services/Others/AccountingOpService';
declare var attachmentUploader, ResultAsArray: any;


@Component({
    selector: 'JournalCSVLoadComponent',

    providers: [EntityListService],
    templateUrl: './JournalCSVLoadComponent.html',
})

export class JournalCSVLoadComponent extends BaseComponent {
    public JournalPM: JournalPM;
    public BankAccountPM: BankAccountPM;
    public PrevBankPagePM: JournalPM;
    public DataContext: JournalCSVLoadComponent = this;
    public ObjectTableName: string = "Journal";
    public ValidationErrorsList: string[] = [];
    isNewEntity: boolean = false;
    IsCancelApprovedEnabled: boolean = false;
    public IsDisplayOnly: boolean = false;
    public IsMultiCurrency: boolean = false;
    currency: any;

    AmountColHeader: string;
    PageLinesList: ObservableCollection;
    public isRTL: boolean = false;
    public TotalSum: number = 0.0;
    public Difference: number = 0.0;

    _entityResourceService: EntityResourceService = new EntityResourceService();
    _JournalPMService: JournalPMService = new JournalPMService();

    _JournalExtendedPMService: JournalExtendedPMService = new JournalExtendedPMService();
    _CurrencyPMService: CurrencyPMService = new CurrencyPMService();
    currencyListService: CurrencyListService = new CurrencyListService();

    //_AccountingOpService: AccountingOpService;


    public UploadFileId: string = Guid.NewRandomString();
    FileName: string;
    FileSize: string;
    FileExtension: string;
    File: any;
    fileUploadParamerter: ImageParameter;
    FileData: number;
    ProgressBarPercentText: string;
    IsShowProgressBar: boolean = false;
    IsUploadCanceled: boolean;
    IsUploadInProgress: boolean;
    Placeholder: any = '';

    ResponseMessage: any;
    UploadButtonIsEnabled: boolean = true;
    _DecodedLoadedString: string;
    private CurrentSession = SessionLocator.SelectedSession;
    HasError: boolean;
    _LabelLog: string;
    public _NewJournalPM: JournalPM;
    constructor(private CD: ChangeDetectorRef, public entityListService: EntityListService) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

        this.PageLinesList = new ObservableCollection([]);


    }

    SetWindowArgs(args) {

    }

    //#region Properties


    tenantCurrency: any;
    get TenantCurrency() { return this.tenantCurrency }
    set TenantCurrency(value: any) {
        if (this.tenantCurrency != value) {
            this.tenantCurrency = value;

        }
    }
    //#endregion

    //#region Buttons Handlers



    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    //* grid handlers in seperate region

    //#endregion





    //#region Prev Bank Page

    //#endregion

    SendJournal(): any {
        //throw new Error("Method not implemented.");
        this.CurrentSession.StartBusyIndicatorCreating();
        if (this.fileUploadParamerter != null && this.fileUploadParamerter.Base64String != null) {
            this._JournalExtendedPMService.PostJournalAsCSV(this.fileUploadParamerter)
                .subscribe((myServiceResponse: ServiceResponse) => {
                    console.log("[Send] Response/LoadBankPages: ", myServiceResponse.Result);
                    var response = myServiceResponse.Result;


                    this.CurrentSession.StopBusyIndicator();
                    if (myServiceResponse.HasError) {
                        this.HasError = true
                        this._LabelLog = myServiceResponse.ErrorsArray.join(',');

                    } else {

                        if (!AppTool.IsNullOrEmpty(response)) {
                            //this.ShowMessage(JSON.stringify(response.Result));
                            var journalPM: JournalPM;
                            journalPM = myServiceResponse.Result;
                            console.log(journalPM);
                            this._NewJournalPM = journalPM;

                            //this.CancelButtonClicked();
                        }

                    }
                });
        }

    }
    OpenJournal() {
        if (!AppTool.IsNullOrEmpty(this._NewJournalPM.Id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: this._NewJournalPM.Id, ObjectTableName: 'Journal' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                    });
                });
        }
    }

    //#region upload
    public ShowMessage(message: string) {
        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);
    }

    OpenUpLoadFile() {
        document.getElementById(this.UploadFileId).click();
    }

    UploadFile(event: any) {
        var file: any = attachmentUploader(this.UploadFileId);
        if (file) {
            var temp = file.name.split('.');
            this.FileExtension = temp[temp.length - 1];
            this.FileName = file.name.replace("." + this.FileExtension, "");

            if (this.FileExtension.toLowerCase() != "csv") {
                this.ShowMessage("חובה קובץ CSV");
                return;
            }

            this.File = file;

            if (this.FileExtension && this.FileExtension.length > 10) {
                this.ShowMessage("File extension should be less than or equal 10 characters");
            }
            else {
                this.IsShowProgressBar = true;
                this.UploadButtonIsEnabled = false;

                this.fileUploadParamerter = new ImageParameter();
                this.fileUploadParamerter.Key = Guid.newGuid();
                this.fileUploadParamerter.IsFirstTry = true;
                this.fileUploadParamerter.Extension = this.FileExtension;
                this.fileUploadParamerter.UploadMode = "Block";
                this.fileUploadParamerter.FileSize = file.size;
                this.fileUploadParamerter.Tenant = SessionLocator.Tenant;

                this.ArrayBufferToBase64(file, this);
            }
        }
    }


    ArrayBufferToBase64(file: any, viewmodel: JournalCSVLoadComponent) {
        var reader: FileReader = new FileReader();
        var reader = new FileReader();
        reader.onload = function (e) {
            var decodedString = '';
            var bytes = new Uint8Array(ResultAsArray(e));
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                decodedString += String.fromCharCode(bytes[i]);
            }
            viewmodel._DecodedLoadedString = decodedString;
            viewmodel.fileUploadParamerter.Base64String = window.btoa(decodedString);

            viewmodel.IncreaseProgressBar(100);
            viewmodel.TryParseLocally();
        };

        reader.onerror = function (e) {
            console.log(e);
        };
        reader.readAsArrayBuffer(file);
    }
    TryParseLocally(): any {
        //throw new Error("Method not implemented.");
        if (AppTool.IsNullOrEmpty(this._DecodedLoadedString)) {
            this.ShowMessage("File Is Empty");
            return;
        }
        let headers = [];
        let aryLine = this._DecodedLoadedString.split("\n");
        aryLine.forEach(currLine => {
            if (currLine.startsWith("031")) {
                headers.push({ 'L31': currLine, 'L32': "" });
            } else if (currLine.startsWith("032")) {
                var rec = headers[headers.length - 1];
                rec.L32 = currLine;
            }
        });
        if (headers.length < 0) {
            this.ShowMessage("Incorrect file format");
            return;
        }
        this.SendJournal()
    }
    IncreaseProgressBar(ProgressBarValue: number) {
        var elem = document.getElementById("myBar");
        if (ProgressBarValue == 100) {
            elem.style.width = (ProgressBarValue - 0.1) + '%';
            this.ProgressBarPercentText = ProgressBarValue.toString() + ' %';

        }
        else {
            elem.style.width = ProgressBarValue + '%';
            this.ProgressBarPercentText = ProgressBarValue.toFixed(2).toString() + ' %';
        }

    }


    //#endregion upload

}
class ResultLoadBankPage {
    public DBSuccessPageList: MyDTO[];
    public DBExceptionPageList: MyDTO[];
    public ValidateBankPageAgaintDBErrors: MyDTO[];

}
class MyDTO {
    public Message: string
    public Verbose: string
    public RawLine: string

}
