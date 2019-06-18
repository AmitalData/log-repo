import {Component, Output, EventEmitter, OnInit, AfterViewInit, ChangeDetectorRef}  from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ReconciliationPM} from '../../EntityPMs/ReconciliationPM';
import {AppTool} from '../../../Infrastructure/Tools';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    selector: 'OutOfDepositMessage',
    template:
    `
    <style>
    .ConfirmIcon {
        width: 70px;
        height: 70px;
        line-height: 70px;
        color: white;
        font-size: 45px;
        font-weight: bold;
        font-family: Arial;
        text-align: center;
        vertical-align: middle;
        -webkit-border-radius: 50px;
        -moz-border-radius: 50px;
        border-radius: 50px;
        background: -moz-linear-gradient(50% 0% -90deg,rgba(108, 132, 153, 1) 0%,rgba(112, 136, 156, 1) 18.27%,rgba(125, 147, 166, 1) 37.99%,rgba(147, 166, 182, 1) 58.38%,rgba(177, 192, 205, 1) 79.19%,rgba(215, 225, 234, 1) 100%);
        background: -webkit-linear-gradient(-90deg, rgba(108, 132, 153, 1) 0%, rgba(112, 136, 156, 1) 18.27%, rgba(125, 147, 166, 1) 37.99%, rgba(147, 166, 182, 1) 58.38%, rgba(177, 192, 205, 1) 79.19%, rgba(215, 225, 234, 1) 100%);
        background: -webkit-gradient(linear,50% 0%,50% 100%,color-stop(0,rgba(108, 132, 153, 1) ),color-stop(0.1827,rgba(112, 136, 156, 1) ),color-stop(0.3799,rgba(125, 147, 166, 1) ),color-stop(0.5838,rgba(147, 166, 182, 1) ),color-stop(0.7919,rgba(177, 192, 205, 1) ),color-stop(1,rgba(215, 225, 234, 1) ));
        background: -o-linear-gradient(-90deg, rgba(108, 132, 153, 1) 0%, rgba(112, 136, 156, 1) 18.27%, rgba(125, 147, 166, 1) 37.99%, rgba(147, 166, 182, 1) 58.38%, rgba(177, 192, 205, 1) 79.19%, rgba(215, 225, 234, 1) 100%);
        background: linear-gradient(180deg, rgba(108, 132, 153, 1) 0%, rgba(112, 136, 156, 1) 18.27%, rgba(125, 147, 166, 1) 37.99%, rgba(147, 166, 182, 1) 58.38%, rgba(177, 192, 205, 1) 79.19%, rgba(215, 225, 234, 1) 100%);
    }
    .RedButton{
        position: absolute;
        right: 10px;
        bottom: 10px;
        width: 65px;
    }
    </style>

    <!--<div class="LeftCenter ConfirmIcon" >?</div>-->

    <div style= "padding: 10px 15px;font-size: 12px;white-space: normal;" >
       {{'Accounting.O.OutOfDepositMSG' | TextCodeTranslationPipe }}
    </div>

    <div style= "padding: 15px 10px 10px 15px;font-size: 12px;white-space: normal;" >
            <LogTextBox [Placeholder]="'Accounting.General.O.Notes' | TextCodeTranslationPipe" [IsFreeText]="true" [ObjectTableName]="'PaymentCheque'" [IsMultiline]="true" [ObjectFieldName]="'CancellationRemarks'" [DataContext]="DataContext"></LogTextBox>
    </div>

    <div style="width:100%;height:22px;position: absolute; bottom:0;">
        <!--<button  [style.float]="isRTL ? 'left' : 'right'"  style="width: 80px;position: relative; display: inline-block;top:0;bottom:0;right:0;margin: 0 5px;" class="RedButton" (click)="CustomerButtonClicked()">{{'Accounting.O.Customer' | TextCodeTranslationPipe }}</button>-->
        <button  [style.float]="isRTL ? 'left' : 'right'"  style="width: 60px;position: relative; display: inline-block;top:0;bottom:0;right:0;margin: 0 5px;" class="RedButton" (click)="CashbookButtonClicked()">{{'Accounting.General.B.OK' | TextCodeTranslationPipe }}</button>
        <button  [style.float]="isRTL ? 'left' : 'right'"  style="width: 70px;position: relative; display: inline-block;top:0;bottom:0;right:0;margin: 0 5px;" class="Button" (click)="OkButtonClicked()">{{'Accounting.General.B.Cancel' | TextCodeTranslationPipe }}</button>
    </div>
            `
})

export class OutOfDepositMessage extends BaseComponent {
    public DataContext: OutOfDepositMessage = this;
    public isRTL: boolean = false;

    constructor() {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

    }

    SetWindowArgs(args: any) {
        if (args != null) {
            //this.RecoPM = args.ReconciliationPM;
        }
    }

    _OODMSG: string;
    public get CancellationRemarks() {
        return this._OODMSG;
    }
    public set CancellationRemarks(value: string) {
        this._OODMSG = value;
    }
    CustomerButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindowEmit("Customer;" + this._OODMSG);
    }
    CashbookButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindowEmit("Cashbook;" + this._OODMSG);
    }
    OkButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindowEmit("Cancel");
    }
}
