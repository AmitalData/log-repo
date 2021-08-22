import { Component, OnInit, AfterViewInit, ChangeDetectorRef, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeclarationRestoreArgs } from '../../../../Customs/Args';
import { DeclarationPM } from '../../../../Customs/EntityPMs/DeclarationPM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DeclarationExtendedListService } from '../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { DeclarationList } from '../../../../Customs/EntityLists/DeclarationList';
import { DeclarationMessagesService } from '../../../../Customs/Services/WebServices/DeclarationMessagesService';
import { PrintRequestRequestParams } from '../../../../Customs/DataContract/RequestParams/PrintRequestRequestParams';
import { PrintRequestResponseData, PrintRequestResultList } from '../../../../Customs/DataContract/ResponseData/PrintRequestResponseData';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomSendOptionsArgs } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';


@Component({
    selector: 'PrintRequestComponent',
    
    templateUrl: './PrintRequestComponent.html',
})

export class PrintRequestComponent    extends BaseRequestsSheetMassaging    implements AfterViewInit,IRequestsSheetMassagingComponent {
  public IsDisplayOnly: boolean = false;

    public DataContext: PrintRequestComponent = this;
    public ObjectTableName: string = "Customs.Declaration";
    private isSuccessMessageVisible: boolean = false;

    public DeclarationPrintList: DeclarationPrintVM[];
    _DeclarationMessagesService: DeclarationMessagesService = new DeclarationMessagesService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _CD: ChangeDetectorRef) {
        super();
    }

    @ViewChild(CustomMessageWrapperComponent)
    SuperCustomMessageWrapperComponent: CustomMessageWrapperComponent = new CustomMessageWrapperComponent();
    ngAfterViewInit() {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        } else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent()
    }

    RemoveDeclarationPrint(declarationPrintItem) {
        var index = this.DeclarationPrintList.indexOf(declarationPrintItem, 0);

        if (index > -1) {
            this.DeclarationPrintList.splice(index, 1);
        }

        if (this.DeclarationPrintList.length == 0) {
            this.AddDeclarationPrint(null);
        }
    }

    AddDeclarationPrint(declarationPrintItem) {
        if (declarationPrintItem != null) {
            if (AppTool.IsNullOrEmpty(declarationPrintItem.CustomFileNo) && AppTool.IsNullOrEmpty(declarationPrintItem.DeclarationNumber)) {
                return;
            }
        }
        var my = new DeclarationPrintVM(this._CD);
        this.DeclarationPrintList.push(my);
    }

    OnMassageDisplayMethod() {
        if (this.RequestParams == null) {
            this.RequestParams = new PrintRequestRequestParams();
            this.SetIsByDeclarationNumber(true);
        }      

        if (this.ResponseData) {
            if (this.ResponseData.DeclarationPrintAnswer) {
                if (this.DeclarationPrintList == null || this.DeclarationPrintList.length == 0) {
                    this.DeclarationPrintList = [];
                    this.ResponseData.DeclarationPrintAnswer.forEach(item => {
                        this.AddDeclarationPrint(null);
                        this.DeclarationPrintList[item.SequenceNumber - 1].CustomFileNo = item.CustomFileNo;
                        this.DeclarationPrintList[item.SequenceNumber - 1].DeclarationNumber = item.DeclarationNumber;
                    });
                }
                this.LoadResponsData();
            }
        }
        else {
            this.ResponseData = new PrintRequestResponseData();
        }
    }

    //#region Properties
    SetIsByDeclarationNumber(newValue: boolean) {
        this.ClearOldValues();
        this.IsSearchByDeclarationRadio = newValue;
    }

    ClearOldValues() {
        this.DeclarationPrintList = [];
        this.AddDeclarationPrint(null);

        this.CargoTypeCode = null;
        this.ManifestNumber = null;
        this.SecondCargoID = null;
        this.ThirdCargoID = null;

        return;
    }
        
    get IsSearchByDeclarationRadio() { return this.RequestParams ? this.RequestParams.IsSearchByDeclarationRadio : null; }
    set IsSearchByDeclarationRadio(newValue: boolean) {
        if (this.RequestParams.IsSearchByDeclarationRadio != newValue) {
            this.RequestParams.IsSearchByDeclarationRadio = newValue;
            if (newValue == true) {
                this.IsSearchByCargoRadio = false;
            }
        }
    }

    get IsSuccessMessageVisible() { return this.isSuccessMessageVisible; }
    set IsSuccessMessageVisible(newValue: boolean) {
        if (this.isSuccessMessageVisible != newValue) {
            this.isSuccessMessageVisible = newValue;
        }
    }

    SetIsByCargo(newValue: boolean) {
        this.IsSearchByCargoRadio = newValue;
    }

    get IsSearchByCargoRadio() { return this.RequestParams ? this.RequestParams.IsSearchByCargoRadio : null; }
    set IsSearchByCargoRadio(newValue: boolean) {
        if (this.RequestParams.IsSearchByCargoRadio != newValue) {
            this.RequestParams.IsSearchByCargoRadio = newValue;

            if (newValue == true) {
                this.IsSearchByDeclarationRadio = false;
            }
        }
    }

    get CargoTypeCode() { return this.RequestParams ? this.RequestParams.CargoTypeCode : null; }
    set CargoTypeCode(value: string) {
        if (this.RequestParams.CargoTypeCode != value) {
            this.RequestParams.CargoTypeCode = value;
        }
    }

    get ManifestNumber() { return this.RequestParams ? this.RequestParams.ManifestNumber : null; }
    set ManifestNumber(value: string) {
        if (this.RequestParams.ManifestNumber != value) {
            this.RequestParams.ManifestNumber = value;
        }
    }

    get SecondCargoID() { return this.RequestParams ? this.RequestParams.SecondCargoID : null; }
    set SecondCargoID(value: string) {
        if (this.RequestParams.SecondCargoID != value) {
            this.RequestParams.SecondCargoID = value;
        }
    }

    get ThirdCargoID() { return this.RequestParams ? this.RequestParams.ThirdCargoID : null; }
    set ThirdCargoID(value: string) {
        if (this.RequestParams.ThirdCargoID != value) {
            this.RequestParams.ThirdCargoID = value;
        }
    }

    get ResponseMessage() { return this.ResponseData ? this.ResponseData.ResponseMessage : null; }
    set ResponseMessage(value: string) {
        if (this.ResponseData.ResponseMessage != value) {
            this.ResponseData.ResponseMessage = value;
        }
    }
    //#endregion Properties


    //#region Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    FillErrors() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.IsSearchByDeclarationRadio) {
            if (this.DeclarationPrintList.length == 0) {
                var msg = TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationNumberIsMandatory");
                this.ValidationErrorsList.push(msg);
            }
            else {
                this.DeclarationPrintList.forEach(item => {
                    if (AppTool.IsNullOrEmpty(item.DeclarationNumber)) {
                        var msg = TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationNumberIsMandatory");
                        this.ValidationErrorsList.push(msg);
                    }
                });                
            }
        }
        else {
            if (AppTool.IsNullOrEmpty(this.CargoTypeCode)) {
                var msg = TextCodeTranslator.Translate("Customs.Declaration.O.CargoTypeCodeIsMandatory");
                this.ValidationErrorsList.push(msg);
            }
            if (AppTool.IsNullOrEmpty(this.ManifestNumber)) {
                var msg = TextCodeTranslator.Translate("Customs.Declaration.O.FirstCargoIdIsMandatory");
                this.ValidationErrorsList.push(msg);
            }
        }
    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
               
        var currRequestParams = new PrintRequestRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.IsSearchByDeclarationRadio = this.IsSearchByDeclarationRadio;
        currRequestParams.IsSearchByCargoRadio = this.IsSearchByCargoRadio;
        currRequestParams.DeclarationNumber = [];
        if (this.IsSearchByDeclarationRadio) {
            this.DeclarationPrintList.forEach(
                (item) => {
                    currRequestParams.DeclarationNumber.push(item.DeclarationNumber);
                }
            );
        }
        else {
            currRequestParams.CargoTypeCode = this.CargoTypeCode;
            currRequestParams.ManifestNumber = this.ManifestNumber;
            currRequestParams.SecondCargoID = this.SecondCargoID;
            currRequestParams.ThirdCargoID = this.ThirdCargoID;
        }

        CustomMessageProgressComponent
            .ShowProgressBar(this.CurrentSession,currRequestParams.PBId,
            "שליחת שאילתא להדפסת הצהרה", true)
            .then((res) => {
                this.ResponseData = res;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });


        this._DeclarationMessagesService.PostPrintRequestRequest(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
            });

    }

    LoadResponsData() {

        if (this.ResponseData) {
            if (this.ResponseData.DeclarationPrintAnswer && this.IsSearchByDeclarationRadio) {

                this.ResponseData.DeclarationPrintAnswer.forEach(item => {
                    if (item.IsFiled == true) {
                        this.DeclarationPrintList[item.SequenceNumber - 1].VGreenVisibility = true;
                        this.DeclarationPrintList[item.SequenceNumber - 1].XRedVisibility = false;
                    }
                    else {
                        this.DeclarationPrintList[item.SequenceNumber - 1].PrintErrorText = item.ErrorText;
                        this.DeclarationPrintList[item.SequenceNumber - 1].XRedVisibility = true;
                        this.DeclarationPrintList[item.SequenceNumber - 1].VGreenVisibility = false;
                    }
                });
            }
            if (this.ResponseData.Succeeded == true && this.ResponseData.HasException == false) {
                this.IsSuccessMessageVisible = true;
            }
        }
    }
    //#endregion Commands
}


export class DeclarationPrintVM extends BaseComponent {
    ObjectTableName: string = "Customs.Declaration";
    public CustomFileNo: string;
    public DeclarationNumber: string;
    public PrintErrorText: string = null;
    public VGreenVisibility: boolean = false;
    public XRedVisibility: boolean = false;

    public OldCustomFile: String;
    public OldDeclarationNo: String;
    public _DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _CD: ChangeDetectorRef) {
        super();       
    }

    DeclarationNumberTextChanged(searchtext) {
        if (AppTool.IsNullOrEmpty(this.DeclarationNumber)) {
            return;
        }
        if (this.OldDeclarationNo == this.DeclarationNumber) {
            return;
        }

        this.DueChangeClearChildField(false);

        this.CurrentSession.StartBusyIndicator("")
        this._DeclarationExtendedListService.GetSingleDeclarationByNumber(this.DeclarationNumber, SessionLocator.Tenant)
            .subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                this.FetchDeclaration(myResponse, false);
            });
    }

    FetchDeclaration(myResponse: ServiceResponse, sourceIsCustomFile: boolean) {
        var lastFetchDeclarationList = myResponse.Result;
        if (lastFetchDeclarationList != null) {
            this.DeclarationNumber = lastFetchDeclarationList.DeclarationNumber;
            this.CustomFileNo = lastFetchDeclarationList.CustomFileNo;
            this.OldCustomFile = this.CustomFileNo;
            this.OldDeclarationNo = this.DeclarationNumber;

            this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
            this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, true, "");
            //this._CD.detectChanges();

        } else {
            if (sourceIsCustomFile) {
                this.SetValidityCustomFileNo();
            } else {
                this.SetValidityDeclarationNumber();
            }
        }
    }

    SetValidityDeclarationNumber() {
        var msg = TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationNumberIsMandatory");
        this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, false, msg);
    }

    SetValidityCustomFileNo() {
        var msg = TextCodeTranslator.Translate("Customs.Declaration.O.Didntfindcustomfile");
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, false, msg);
    }

    DueChangeClearChildField(sourceIsCostomFile: boolean): void {
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
        this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, true, "");

        if (sourceIsCostomFile) {
            this.DeclarationNumber = "";
        } else {
            this.CustomFileNo = "";
        }
    }

    CustomFileNoTextChanged(searchtext) {

        if (AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            return;
        }
        if (this.OldCustomFile == this.CustomFileNo) {
            return;
        }

        this.DueChangeClearChildField(true);
        this.CurrentSession.StartBusyIndicator("");
        this._DeclarationExtendedListService.GetSingleDeclarationByCustomFileNo(this.CustomFileNo)
            .subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                this.FetchDeclaration(myResponse, true);
            });
    }

}
