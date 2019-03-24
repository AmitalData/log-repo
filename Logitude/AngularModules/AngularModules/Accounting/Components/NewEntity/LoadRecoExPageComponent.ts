import {Component, ChangeDetectorRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {ReconcileExternalPagePM} from '../../EntityPMs/ReconcileExternalPagePM';
import {ReconcileExternalPageLinePM} from '../../EntityPMs/ReconcileExternalPageLinePM';
import {BankAccountPM} from '../../EntityPMs/BankAccountPM';
import {GLAccountPM} from '../../EntityPMs/GLAccountPM';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ReconcileExternalPagePMService} from '../../Services/StandardPMs/ReconcileExternalPagePMService';
import {CurrencyPMService} from '../../../Common/Services/StandardPMs/CurrencyPMService';
import {CurrencyListService} from '../../../Common/Services/StandardLists/CurrencyListService';
import {ReconcileExternalPageExtendedPMService} from '../../Services/ExtendedPMs/ReconcileExternalPageExtendedPMService';
import {EntityListService} from '../../../Infrastructure/Services/EntityListService';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ObservableCollection} from '../../../Infrastructure/Utilities/ObservableCollection';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';


import { Guid } from '../../../Infrastructure/Utilities/Guid';
import { ImageParameter } from '../../../Infrastructure/DataContracts/ImageParameter';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
declare var attachmentUploader, ResultAsArray: any;


@Component({
    selector: 'LoadRecoExPageComponent',
    moduleId: module.id,
    providers: [EntityListService],
    templateUrl: './LoadRecoExPageComponent.html',
})

export class LoadRecoExPageComponent extends BaseComponent {
    public ReconcileExternalPagePM: ReconcileExternalPagePM;
    public BankAccountPM: BankAccountPM;
    public PrevBankPagePM: ReconcileExternalPagePM;
    public DataContext: LoadRecoExPageComponent = this;
    public ObjectTableName: string = "ReconcileExternalPage";
    public ValidationErrorsList: string[] = [];
    isNewEntity: boolean = false;
    IsCancelApprovedEnabled: boolean = false;
    public IsDisplayOnly: boolean = false;
    public IsMultiCurrency: boolean = false;
    currency: any;
    AMOUNT_TEXT = TextCodeTranslator.Translate("ReconcileExternalPageLine.F.Amount");
    AmountColHeader: string;
    PageLinesList: ObservableCollection;
    public isRTL: boolean = false;
    public TotalSum: number = 0.0;
    public Difference: number = 0.0;

    _entityResourceService: EntityResourceService = new EntityResourceService();
    _ReconcileExternalPagePMService: ReconcileExternalPagePMService = new ReconcileExternalPagePMService();
    _ReconcileExternalPageExtendedPMService: ReconcileExternalPageExtendedPMService = new ReconcileExternalPageExtendedPMService();
    _CurrencyPMService: CurrencyPMService = new CurrencyPMService();
    currencyListService: CurrencyListService = new CurrencyListService();



    public UploadFileId: string = Guid.NewRandomString();
    FileName: string;
    FileSize: string;
    FileExtension: string;
    File: any;
    filterImageParameter: ImageParameter;
    FileData: number;
    ProgressBarPercentText: string;
    IsShowProgressBar: boolean = false;
    IsUploadCanceled: boolean;
    IsUploadInProgress: boolean;
    Placeholder: any;

    ResponseMessage: any;
    UploadButtonIsEnabled: boolean = true;

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
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }

    //* grid handlers in seperate region

    //#endregion

   



    //#region Prev Bank Page

    //#endregion



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

            if (this.FileExtension != "csv") {
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

                this.filterImageParameter = new ImageParameter();
                this.filterImageParameter.Key = Guid.newGuid();
                this.filterImageParameter.IsFirstTry = true;
                this.filterImageParameter.Extension = this.FileExtension;
                this.filterImageParameter.UploadMode = "Block";
                this.filterImageParameter.FileSize = file.size;
                this.filterImageParameter.Tenant = SessionLocator.Tenant;

                this.ArrayBufferToBase64(file, this);
            }
        }
    }


    ArrayBufferToBase64(file: any, viewmodel: any) {
        var reader: FileReader = new FileReader();
        var reader = new FileReader();
        reader.onload = function (e) {
            var binary = '';
            var bytes = new Uint8Array(ResultAsArray(e));
            var len = bytes.byteLength;
            for (var i = 0; i < len; i++) {
                binary += String.fromCharCode(bytes[i]);
            }
            viewmodel.filterImageParameter.Base64String = window.btoa(binary);
            viewmodel.IncreaseProgressBar(100);
        };

        reader.onerror = function (e) {
            console.log(e);
        };
        reader.readAsArrayBuffer(file);
    }

    //#endregion upload

}
