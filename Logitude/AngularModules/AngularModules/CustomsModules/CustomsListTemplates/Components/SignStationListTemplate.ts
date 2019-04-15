import {Component, ChangeDetectorRef} from '@angular/core';
import {WebFreightDomainService} from '../../../Infrastructure/Services/WebFreightDomainService';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {OnInit, Output, EventEmitter, ComponentRef, QueryList} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';

import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool} from '../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { CustomsRequestMenuService } from '../../../Customs/Services/Others/CustomsRequestMenuService';

import { SignStationExtendedListService, SignStationList} from   '../../../Customs/Services/ExtendedLists/SignStationExtendedListService';

import { ResponseDataBase } from '../../../Customs/DataContract/ResponseData/ResponseDataBase';


@Component({
    moduleId: module.id,
    templateUrl: './SignStationListTemplate.html',
})

export class SignStationListTemplate {

    _SignStationList: SignStationList;
    public fieldName: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef) {
        //        this.TenantCurrencySign = SessionLocator.TenantPM.CurrencySign;
    }

    _Color: String;
    _ShowDate: boolean = true;

    //_StatusList = ["Start", "תקין", "כישלון", "ממתין להזנת סיסמא", "כרטיס שגוי", "ללא הגדרה"];
    //_StatusList = ["Start", "OK", "Failure", "Waitingtoenterapassword", "IncorrectCard", "NoDefinition"];

    StatusHebrew: string;
    setVariables(signStationList: SignStationList, fieldName: string) {
        ///console.log(rowData);
        this._SignStationList = signStationList;
        this.fieldName = fieldName;

        //#region Set Icons
        switch (this._SignStationList.Status) {
            case "Failure": { this.StatusHebrew = "כישלון" } break;
            case "Waitingtoenterapassword": { this.StatusHebrew = "ממתין להזנת סיסמא" } break;
            case "NoDefinition": { this.StatusHebrew = "ללא הגדרה" } break;
            case "OK": { this.StatusHebrew = "תקין" } break;
            case "Incorrectcard": { this.StatusHebrew = "נבחר כרטיס שגוי" } break;
            case "IncorrectCard": { this.StatusHebrew = "נבחר כרטיס שגוי" } break;
            default: { this.StatusHebrew = this._SignStationList.Status } break;
        }

        this._ShowDate = (this._SignStationList.LastSignAt.toString() != "0001-01-01T00:00:00");
        //#endregion 
        if (this._SignStationList.IsOk === true) {
            this._Color = "green";
        }
        else if (this._SignStationList.IsOk === false) {
            this._Color = "red";
        } else {
            this._Color = "orange";
        }

        this.CD.detectChanges();
    }





    //TestSignCommandAction() {
        


    //    this.CD.detectChanges();
    //    this.CurrentSession.StartBusyIndicator("");
    //    let signStationExtendedListService = new SignStationExtendedListService();
    //    signStationExtendedListService.PostTestSign(this._SignStationList)
    //        .Subscribe(rsp ==>{
    //            rsp.Result
    //        });


    //}
}
