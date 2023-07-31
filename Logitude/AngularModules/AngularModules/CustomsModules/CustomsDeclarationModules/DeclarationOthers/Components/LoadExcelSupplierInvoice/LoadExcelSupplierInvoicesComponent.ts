import { OnInit, Component, ViewChild, ElementRef } from '@angular/core';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { Guid } from '../../../../../Infrastructure/Utilities/Guid';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { ImageParameter } from '../../../../../Infrastructure/DataContracts/ImageParameter';
import { SupplierInvoiceService } from '../../../../../Customs/Services/Others/SupplierInvoiceService';
import { AppTool } from '../../../../../Infrastructure/Tools';
import { AmitalGatewayUtil, UnifreightMessageM } from 'Infrastructure/Utilities/AmitalGatewayUtil';
import { UnifreightController, UnifreightResponseEventArgs } from 'Customs/Controller/UnifreightController';
import { ConfirmWindow } from '../../../../../Controls/Windows/ConfirmWindow';
declare var attachmentUploader, ResultAsArray: any;
@Component({
    selector: 'LoadExcelSupplierInvoicesComponent',
    templateUrl: './LoadExcelSupplierInvoicesComponent.html',
})

export class LoadExcelSupplierInvoicesComponent extends BaseComponent {

    DataContext: any = this;
    FileName: string;
    FileExtension: string;
    File: any;
    IsShowProgressBar: boolean = false;
    UploadButtonIsEnabled: boolean = true;
    filterImageParameter: ImageParameter;
    UploadSuccessLabel: boolean;
    FileData: number;
    Placeholder: any = "";
    ProgressBarPercentText: string;
    public UploadFileId: string = Guid.NewRandomString();
    private CurrentSession = SessionLocator.SelectedSession;
    public DeclarationId: string;
    private _UnifacePartnerCode: string;
    public get UnifacePartnerCode(): string {
        return this._UnifacePartnerCode;
    }
    public set UnifacePartnerCode(value: string) {
        this._UnifacePartnerCode = value;
    }

    constructor(private EntityResourceService: EntityResourceService) {
        super();
    }

    customerId: string;
    get CustomerId() { return this.customerId }
    set CustomerId(value: string) {
        this.customerId = value;
    }

    partnerId: string;
    get PartnerId() { return this.partnerId }
    set PartnerId(value: string) {
        this.partnerId = value;
    }

    
    SetWindowArgs(args: any)
    {
        if (!AppTool.IsNullOrEmpty(args))
        {
            this.DeclarationId = args.DeclarationId;
        }
    }

    OpenUpLoadFile()
    {
        document.getElementById(this.UploadFileId).click();
    }

    public ShowMessage(message: string)
    {
        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);
    }

    UploadFile(event: any)
    {
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
                this.UploadSuccessLabel = true;
                this.ArrayBufferToBase64(file, this);
            }
        }
    }

    ArrayBufferToBase64(file: any, viewmodel: any)
    {
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

    IncreaseProgressBar(ProgressBarValue: number)
    {
        var elem = document.getElementById("myBar") as HTMLProgressElement;
        if (ProgressBarValue == 100) {
            elem.value = ProgressBarValue;
            this.ProgressBarPercentText = ProgressBarValue.toString() + ' %';
            elem.dataset['label'] = this.ProgressBarPercentText;
        }
        else {
            elem.value = ProgressBarValue;
            this.ProgressBarPercentText = ProgressBarValue.toFixed(2).toString() + ' %';
            elem.dataset['label'] = this.ProgressBarPercentText;
        }

    }

    OkButtonClicked()
    {
        if (AppTool.IsNullOrEmpty(this.UnifacePartnerCode))
        {
            this.ShowMessage("חובה לבחור פרטנר");
        }
        else
        {
            var supplierInvoiceService = new SupplierInvoiceService();
            supplierInvoiceService.PutSupplierInvioceFromFileRequest(this.filterImageParameter, SessionLocator.Tenant, this.CustomerId, this.UnifacePartnerCode, this.DeclarationId).subscribe((myServiceResponse: ServiceResponse) => {

                if (myServiceResponse.HasError) {
                    this.ShowMessage(myServiceResponse.ErrorsArray[0]);
                }
                else {
                    if (myServiceResponse.Result.startsWith("לא נמצא סיווג")) {
                        let messageWindow = new ConfirmWindow();
                        messageWindow.Width = 350;
                        messageWindow.Height = 250;
                        messageWindow.Title = "יצירת חשבון ספק";
                        messageWindow.YesButtonText = "המשך";
                        messageWindow.ShowCancelButton = true;
                        messageWindow.ShowNoButton = false;
                        messageWindow.ShowWarningImage = true;
                        messageWindow.Show(myServiceResponse.Result);
                        messageWindow.WindowClosed.subscribe((event: any) => {
                            if (messageWindow.Yes) {
                                supplierInvoiceService.PutSupplierInvioceFromFileRequest(this.filterImageParameter, SessionLocator.Tenant, this.CustomerId, this.UnifacePartnerCode, this.DeclarationId, true).subscribe((myServiceResponse1: ServiceResponse) => {

                                    if (myServiceResponse1.HasError) {
                                        this.ShowMessage(myServiceResponse1.ErrorsArray[0]);
                                    }
                                    else {
                                        this.CurrentSession.CloseCurrentWindow();
                                        this.ShowMessage(myServiceResponse1.Result);
                                    }
                                });
                            }
                        });
                    }
                    else {
                        this.CurrentSession.CloseCurrentWindow();
                        this.ShowMessage(myServiceResponse.Result);
                    }
                    
                }
            });
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    @ViewChild('myInput')
    myInputVariable: ElementRef;
    DeleteFileButtonClicked() {
        this.myInputVariable.nativeElement.value = "";
        this.filterImageParameter = null;
        this.IsShowProgressBar = false;
        this.UploadButtonIsEnabled = true;
        this.FileName = "";
        this.ProgressBarPercentText = "";
        //this.ErrorsResultList.Clear();
        this.UploadSuccessLabel = false;
    }

    public ButtonPartnerClick(): void {

        console.log("ButtonPartnerClick");
        //if (AmitalGatewayUtil.Instance.IsDeclarationInUse(this.EntityPM?.CustomFileNo, this.EntityPM?.IsConvertedDeclaration, this.EntityPM?.IsConnectedToUnifreight)) {//if (!AppTool.IsNullOrEmpty(this.EntityPM.CustomFileNo) && AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
            
        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse){
            this.CurrentSession.CurrentWindow.StartBusyIndicator("");

            const myUnifreightController = new UnifreightController(
                this.CurrentSession.CurrentEditComponent.EntityPM,
                "Logitude.Customs.LoadExcelSupplierInvoicesComponent");
            myUnifreightController.RaiseLookUpAsync("GTRLPART","PARTNER_ID");
            //myUnifreightController.RaiseLookUpAsync("ATBLPTIL","BRAN_ID");
            myUnifreightController.GetPromise().
                then((e :UnifreightResponseEventArgs) => { 
                    this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    const UnifreightResponseStatus = e.UnifreightResponseStatus;
                    const UnifreightMessage = e.UnifreightMessage;
                    const LOV_RETURN_VALUE = UnifreightMessageM.GetStringValue(UnifreightMessage , "Response.LOV_RETURN_VALUE");
                    console.log(LOV_RETURN_VALUE);
                    this.UnifacePartnerCode=LOV_RETURN_VALUE;
                });
        } else {
            console.error("not connect to uniface");
        }
    }
}




