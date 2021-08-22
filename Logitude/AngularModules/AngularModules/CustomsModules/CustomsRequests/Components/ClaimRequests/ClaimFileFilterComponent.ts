import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeclarationRestoreArgs } from '../../../../Customs/Args';
import { DeclarationPM } from '../../../../Customs/EntityPMs/DeclarationPM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DeclarationExtendedListService } from '../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { DeclarationList } from '../../../../Customs/EntityLists/DeclarationList';
import { IIGGeneralMessagesService } from '../../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { TPG_NG_8244_ClaimFileFilterRequestParams } from '../../../../Customs/DataContract/RequestParams/TPG_NG_8244_ClaimFileFilterRequestParams';
import { TPG_NG_8245_ClaimFilesDetailResponseData } from '../../../../Customs/DataContract/ResponseData/TPG_NG_8245_ClaimFilesDetailResponseData';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomSendOptionsArgs } from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from '../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';

@Component({
    selector: 'ClaimFileFilterComponent',
    
    templateUrl: './ClaimFileFilterComponent.html',
})

export class ClaimFileFilterComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit,IRequestsSheetMassagingComponent {

    public DataContext: ClaimFileFilterComponent = this;
    public ObjectTableName: string = "Customs.Declaration";

    _DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    _IIGGeneralMessagesService: IIGGeneralMessagesService = new IIGGeneralMessagesService();

    OpenFilesCollapsList: ObservableCollection;
    CloseFilesCollapsList: ObservableCollection;
    RefundOrderList: ObservableCollection;
    RequireDocumentsList: ObservableCollection;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    
        this.OpenFilesCollapsList = new ObservableCollection([]);
        this.CloseFilesCollapsList = new ObservableCollection([]);
        this.RefundOrderList = new ObservableCollection([]);
        this.RequireDocumentsList = new ObservableCollection([]);
        this.UIProperties.SetRequired("FileNumber", this.ObjectTableName, true);
        this.UIProperties.SetRequired("Numeral", this.ObjectTableName, true);
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

    OnMassageDisplayMethod() {
        if (this.RequestParams == null) {
            this.RequestParams = new TPG_NG_8244_ClaimFileFilterRequestParams();
        }

        if (this.ResponseData) {

            if (this.ResponseData.OpenFilesCollapsList) {
                this.OpenFilesCollapsList.InsertCollection(this.ResponseData.OpenFilesCollapsList);
            }

            if (this.ResponseData.CloseFilesCollapsList) {
                this.CloseFilesCollapsList.InsertCollection(this.ResponseData.CloseFilesCollapsList);
            }

            if (this.ResponseData.RefundOrderList) {
                this.RefundOrderList.InsertCollection(this.ResponseData.RefundOrderList);
            }

            if (this.ResponseData.RequireDocumentsList) {
                this.RequireDocumentsList.InsertCollection(this.ResponseData.RequireDocumentsList);
            }

        }
        else {
            this.ResponseData = new TPG_NG_8245_ClaimFilesDetailResponseData();

        }
        this.ResponseData.GeneralDataDetails = this.ResponseData.GeneralDataDetails || {};
        this.ResponseData.GeneralDetailsData = this.ResponseData.GeneralDetailsData || {};
    }

    

    //#region Properties
    
    get FileNumber() { return this.RequestParams.FileNumber; }
    set FileNumber(value: string) {
        if (this.RequestParams.FileNumber != value) {
            this.RequestParams.FileNumber= value;
            if (AppTool.IsNullOrEmpty(this.RequestParams.FileNumber)) {
                this.UIProperties.SetRequired("FileNumber", this.ObjectTableName, true);
            } else {
                this.UIProperties.SetRequired("FileNumber", this.ObjectTableName, false);
            }

        }
    }

    get Numeral() { return this.RequestParams.Numeral; }
    set Numeral(value) {
        if (this.RequestParams.Numeral != value) {
            this.RequestParams.Numeral = value;
            if (AppTool.IsNullOrEmpty(this.RequestParams.Numeral)) {
                this.UIProperties.SetRequired("Numeral", this.ObjectTableName, true);
            } else {
                this.UIProperties.SetRequired("Numeral", this.ObjectTableName, false);
            }
        }
    }


    get ExternalId() { return this.ResponseData.GeneralDataDetails.ExternalID; }
    get CustomOfficeName() { return this.ResponseData.GeneralDataDetails.CustomOfficeName; }
    get OpenFileCounter() { return this.ResponseData.GeneralDataDetails.OpenFileCounter; }


    get ExternalName() { return this.ResponseData.GeneralDataDetails.ExternalName; }
    get AgentName() { return this.ResponseData.GeneralDataDetails.AgentName; }
    get CloseFileCounter() { return this.ResponseData.GeneralDataDetails.CloseFileCounter; }
    
    
    //#endregion Properties


    //#endregion Response Properties



    //#endregion Response Properties

    //#region Declaration Commands



    //#endregion


    //#region General Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    FillErrors() {

        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.FileNumber == null) {
            this.ValidationErrorsList.push("");
            return;
        }
        if (this.Numeral == null) {
            this.ValidationErrorsList.push("");
            return;
        }
        


    }

    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
        this.FillErrors();

        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        var currRequestParams = new TPG_NG_8244_ClaimFileFilterRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator.Tenant;

        currRequestParams.FileNumber = this.FileNumber;
        currRequestParams.Numeral = this.Numeral;
        


        CustomMessageProgressComponent
            .ShowProgressBar(this.CurrentSession,currRequestParams.PBId,
            "שליחת שאילתא לתביעות", true)
            .then((res) => {
                this.ResponseData = res;
                this.OnMassageDisplayMethod();
            }
            ).catch((err) => {
                this.ValidationErrorsList.push(err);
            });




        this._IIGGeneralMessagesService.PostTPG_NG_8244_ClaimFileFilterRequestParams(currRequestParams)
            .subscribe((myServiceResponse: ServiceResponse) => {
            });

    }

    //#endregion Commands
}
