import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeclarationRestoreArgs } from '../../../Customs/Args';
import { DeclarationPM } from '../../../Customs/EntityPMs/DeclarationPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { DeclarationExtendedListService } from '../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { DeclarationList } from '../../../Customs/EntityLists/DeclarationList';
import { IIGGeneralMessagesService } from '../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { MasterBOLQueryRequestParams } from '../../../Customs/DataContract/RequestParams/MasterBOLQueryRequestParams';
import { MasterBOLFeedBackResponseData } from '../../../Customs/DataContract/ResponseData/MasterBOLFeedBackResponseData';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomSendOptionsArgs, SendRequestVIA } from '../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from '../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { UIProperties } from '../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import { GenericRequestParams } from '../../../Customs/DataContract/RequestParams/GenericRequestParams';
import { DeclarationWebService } from '../../../Customs/Services/WebServices/DeclarationWebService';

@Component({
    selector: 'CopyDeclarationComponent',    
    templateUrl: './CopyDeclarationComponent.html',
    providers: [DeclarationExtendedListService, DeclarationWebService]
})

export class CopyDeclarationComponent extends BaseComponent
  {
    //public EntityPM: any;
    //public UIProperties:UIProperties;
    private CurrentSession = SessionLocator.SelectedSession;
    DeclarationNumber: any;
    CustomFileNo: any;
    Id: string;
    public ObjectTableName: string = "Customs.Declaration";
    ValidationErrorsList: any;
    public DataContext: CopyDeclarationComponent = this;


 


    constructor(private _declarationExtendedListService: DeclarationExtendedListService, private _declarationWebService: DeclarationWebService) {
        super();

     }


    DeclarationNumberTextChanged(DeclarationNumberText: string): void {
        if (AppTool.IsNullOrEmpty(this.DeclarationNumber)) {
            return;
        }

 
        this.CurrentSession.StartBusyIndicator("")
        this._declarationExtendedListService.GetSingleDeclarationByNumber(this.DeclarationNumber, SessionLocator.Tenant)
            .subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();

                this.FetchDeclaration(myResponse, false);

            });
    }

    CustomFileNoTextChanged(searchtext) {

        if (AppTool.IsNullOrEmpty(this.CustomFileNo)) {
            return;
        }

         this.CurrentSession.StartBusyIndicator("");
        this._declarationExtendedListService.GetSingleDeclarationByCustomFileNo(this.CustomFileNo)
            .subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                this.FetchDeclaration(myResponse, true);
            });
    }


    FetchDeclaration(myResponse: ServiceResponse, sourceIsCostomFile: boolean) {
        var lastFetchDeclarationList = myResponse.Result;
        if (lastFetchDeclarationList != null) {
            this.DeclarationNumber = lastFetchDeclarationList.DeclarationNumber;
            this.CustomFileNo = lastFetchDeclarationList.CustomFileNo;
            this.Id = lastFetchDeclarationList.Id;
            this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
            this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, true, "");

        } else {

            if (sourceIsCostomFile) {
                this.SetValidityCustomFileNo();
            } else {
                this.SetValidityDeclarationNumber();
            }

        }
    }

    CoptDeclaration() {

        var searchParams: GenericRequestParams = new GenericRequestParams();
        searchParams.Tenant = SessionLocator.Tenant;
        searchParams.AppicationId = this.Id;
        searchParams.LoggingEnabled = true;
        searchParams.LoggingEntityId = this.Id;
        searchParams.LoggingEntityReference = this.DeclarationNumber;
        searchParams.LoggingObjectTableId = this.ObjectTableName;
        searchParams.LoggingUserId = SessionLocator.LoggedUserId;
        searchParams.RequestName = "Declaration Request";
        searchParams.ResponseName = "Declaration Response";
        searchParams.RequestVIA = SendRequestVIA.DCABatch;
        searchParams.ForcePersonalSign = false;

        this._declarationWebService
            .GetNewAmendmentDeclaration(searchParams)
            .subscribe((response: any) => {

                if (response) {
                    if (!response.HasError) {


                        //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        this.CurrentSession.StopBusyIndicator();
                        this.CurrentSession.CloseCurrentWindowEmit(null);
                    }
                }
            });
    }
    SetValidityDeclarationNumber() {
        var msg = TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationNumberIsMandatory");
        this.ValidationErrorsList.push(msg);
        this.UIProperties.SetValidity("DeclarationNumber", this.ObjectTableName, false, msg);
    }

    SetValidityCustomFileNo() {
        var msg = TextCodeTranslator.Translate("Customs.Declaration.O.Didntfindcustomfile");
        this.ValidationErrorsList.push(msg);
        this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, false, msg);
    }


    //#endregion Commands
    
}
