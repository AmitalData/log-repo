import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { DeclarationRestoreArgs } from '../../../Customs/Args';
import { DeclarationPM } from '../../../Customs/EntityPMs/DeclarationPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { DeclarationExtendedListService } from '../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { DeclarationList } from '../../../Customs/EntityLists/DeclarationList';
import { DeclarationMessagesService } from '../../../Customs/Services/WebServices/DeclarationMessagesService';
import { DeclarationRestoreRequestParams } from '../../../Customs/DataContract/RequestParams/DeclarationRestoreRequestParams';
import { DeclarationRestoreResponseData } from '../../../Customs/DataContract/ResponseData/DeclarationRestoreResponseData';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomSendOptionsArgs } from '../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CustomMessageProgressComponent } from '../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { LuhnAlgorithm } from '../../../Customs/Utilities/LuhnAlgorithm';
@Component({
    selector: 'DeclarationReshimonConversionComponent',
    
    templateUrl: './DeclarationReshimonConversionComponent.html',
})


export class DeclarationReshimonConversionComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit, OnInit, IRequestsSheetMassagingComponent {
    public DataContext: DeclarationReshimonConversionComponent = this;
    public ObjectTableName: string = "Customs.Declaration";

    _MyResponseObjectToShow: any = null;
    _UserMessagehidden: boolean = true;

    _DeclarationTypeList: any;
    _LastFetchDeclarationList: DeclarationList;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this._DeclarationTypeList =
            [
                { 'DeclarationConvertionDigits': "", 'Name': '' },
                { 'DeclarationConvertionDigits': "99", 'Name': 'יבוא' },
            { 'DeclarationConvertionDigits': "98", 'Name': 'יצוא' },
            { 'DeclarationConvertionDigits': "97", 'Name': 'שטעון' }

            ];
        this.SelectedDeclarationConvertionDigits = this._DeclarationTypeList[0].DeclarationConvertionDigits;
        
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

    DeclarationTypeListSelected($event) {
        if (this.SelectedDeclarationConvertionDigits != $event) {
            this.ValidationErrorsList = [];
            this.DeclarationNumber = null;
            this.DeclarationNumberLast = null;
            this.ReshimonNumber = null;
            this.ReshimonNumberLast = null;
            
        }
        this.SelectedDeclarationConvertionDigits = $event;
    }



    ReshimonNumberTextChanged(ReshimonNumbertext) {

        this.ValidationErrorsList = [];


        if (ReshimonNumbertext == this.ReshimonNumberLast) {
            return;
        }

        this.ReshimonNumber = this.ReshimonNumberLast = ReshimonNumbertext;

        if (AppTool.IsNullOrEmpty(this.ReshimonNumber)) {
            this.DeclarationNumber = null;
            this.DeclarationNumberLast = null;

            return;
        }

        //check reshimon validity
        //9 digits
        if (this.ReshimonNumber.length != 9) {
            this.DeclarationNumber = null;
            this.DeclarationNumberLast = null;

            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.DeclarationReshimonConversion.O.ReshimonNumberLengthError"));

            return;
        }

        //DeclarationNumber = ConvertReshimonToDeclartion(this.ReshimonNumber);
        var declarationNumber = LuhnAlgorithm.ConvertReshimonToDeclartion(this.ReshimonNumber,
            this.SelectedDeclarationConvertionDigits
                //.DeclarationConvertionDigits
        );
        if (AppTool.IsNullOrEmpty(declarationNumber)) {
            this.DeclarationNumber = null;
            this.DeclarationNumberLast = null;

            this.ValidationErrorsList.push("LuhnAlgorithm.ConvertReshimonToDeclartion Failed");

            return;
        }

        this.DeclarationNumber = declarationNumber;
        this.DeclarationNumberLast = this.DeclarationNumber;


    }



    DeclarationNumberTextChanged(DeclarationNumberText) {
        this.ValidationErrorsList = [];

        if (DeclarationNumberText == this.ReshimonNumberLast) {
            return;
        }
        this.ReshimonNumberLast = DeclarationNumberText;

        

        if (AppTool.IsNullOrEmpty(this.DeclarationNumber)) {
            this.ReshimonNumber = null;
            this.ReshimonNumberLast = null;

            return;
        }

        //check Declaration validity
        //14 digits
        if (this.DeclarationNumber.length != 14) {
            this.ReshimonNumber = null;
            this.ReshimonNumberLast = null;
            //ErrorsList.Clear();
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.DeclarationReshimonConversion.O.DeclarationNumberLengthError"));

            return;
        }
        //check digits 3-4 is 98 or 99 according to DclarationType
        if (this.DeclarationNumber.substr(2, 2) != "98" &&
            this.DeclarationNumber.substr(2, 2) != "99") {
            this.ReshimonNumber = null;
            this.ReshimonNumberLast = null;

            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Customs.DeclarationReshimonConversion.O.DeclarationNumberConvertionDigitsError"));


            return;
        }

        //ReshimonNumber = ConvertDeclartionToReshimon(DeclarationNumber);
        var reshimonNumber = LuhnAlgorithm
            .ConvertDeclartionToReshimon(this.DeclarationNumber);
        if (AppTool.IsNullOrEmpty(reshimonNumber)) {
            this.ReshimonNumber = null;
            this.ReshimonNumberLast = null;

            this.ValidationErrorsList.push("LuhnAlgorithm.ConvertDeclartionToReshimon Failed");
            return;
        }
        this.ReshimonNumber = reshimonNumber;
        this.ReshimonNumberLast = this.ReshimonNumber;


    }







    SelectedDeclarationConvertionDigits: string;
    //get SelectedDeclarationType() { return this._SelectedDeclarationType; }
    //set SelectedDeclarationType(value: any) {
    //    if (this._SelectedDeclarationType != value) {
    //        this._SelectedDeclarationType = value;
    //    }
    //}
    ReshimonNumberLast: string;
    DeclarationNumberLast: string;
  

    _ReshimonNumber: string;
    get ReshimonNumber() { return this._ReshimonNumber; }
    set ReshimonNumber(value: string) {
        if (this._ReshimonNumber != value) {
            this._ReshimonNumber = value;
        }
    }
    _DeclarationNumber: string;
    get DeclarationNumber() { return this._DeclarationNumber; }
    set DeclarationNumber(value: string) {
        if (this._DeclarationNumber != value) {
            this._DeclarationNumber = value;
        }
    }




    OnMassageDisplayMethod() {

        if (this.RequestParams == null) {
            this.RequestParams = new DeclarationRestoreRequestParams();
        }

        //this.RefreshScreen();

    }


    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) {
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

}
