
declare var System: any;
declare var window: any;

import {Component, OnInit, Type, Output, EventEmitter, ComponentRef, ViewChild, Input, AfterViewInit, ChangeDetectorRef} from '@angular/core';
import {TipPM} from '../../../../Infrastructure/EntityPMs/TipPM';
import {TipsVisibilityPM} from '../../../../Infrastructure/EntityPMs/TipsVisibilityPM';
import {TipsVisibilityService} from '../../../../Infrastructure/Services/ExtendedPMs/TipsVisibilityService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
@Component({
    moduleId: module.id,

    templateUrl: './TipsComponent.html',
    inputs: ['ObjectTableName', 'Code','IsInternalTips' , 'IsFirstTipLoad'],

    selector: 'TipsComponent',


    providers: [TipsVisibilityService],
})

export class TipsComponent implements OnInit {

    @Output() TipVisibilityChangedEvent = new EventEmitter();
 
    Code: string;
    ObjectTableName: string;
    DontShowA1gimToolTipAreaCheckBoxId: string;
    Tip: TipPM;
    HasTip: boolean;
    TipVisibility: TipsVisibilityPM;
    TipMessage: string;
    IsFirstTipLoad: boolean;
    ShowTipEvent = new EventEmitter();
    isTipVisible: boolean;
    DontShow: boolean;
    IsInternalTips: boolean;
    TipMessageLineLists: string[];
    HeightTipsArea: string;
    constructor(private CD: ChangeDetectorRef, private _tipsVisibilityService: TipsVisibilityService) {
    }
    ngOnInit() {

        //if (this.ShowTipEvent) {
        //    this.ShowTipEvent.subscribe(($event: any) => {

        //        this.ShowAreaClick();
        //    });

        //}

        this.DontShow = false;
        this.DontShowA1gimToolTipAreaCheckBoxId = Guid.newGuid();
        var table = null;
        if (this.IsInternalTips) {
            this.HeightTipsArea = "80px";
            table = window.ObjectTables.filter(d=> d.Name == this.ObjectTableName && (d.Tenant == SessionInfo.LoggedUserTenant || d.Tenant == 0))[0];
        }
        else {
            this.HeightTipsArea = "120px";
            table = window.ObjectTables.filter(d=> d.Name == this.ObjectTableName)[0];
        }

        if (table) {

            if (this.IsInternalTips) this.Tip = window.Tips.filter(d=> d.Code == this.Code && d.ObjectTableId == table.Id)[0];
            else this.Tip = window.Tips.filter(d=> d.Code == table.MainTipCode)[0];
            if (this.Tip) {
                this.HasTip = true;
                //this.TipMessage = this.Tip.ShortTextCodeCode;

                //if (this.IsInternalTips) {
                    var ft = TextCodeTranslator.Translate(this.Tip.ShortTextCodeCode);
                    if (ft) {
                        var tipMessage:string = ft;
                        if (tipMessage.indexOf("<%L>") > -1) {
                            this.TipMessageLineLists = tipMessage.split("<%L>");
                         //   this.TipMessage = tipMessage.replace("<%L>", "<br>");//tipMessage.Replace("(%L)", Environment.NewLine);
                        }
                        else {
                          //  this.TipMessage = tipMessage.replace("(%L)", "<br>");//tipMessage.Replace("(%L)", Environment.NewLine);
                            this.TipMessageLineLists = tipMessage.split("(%L)");
                        }
                     
                    }
                    else this.TipMessageLineLists = this.Tip.ShortTextCodeCode.split("(%L)");//this.TipMessage = this.Tip.Code;
               // }
                var isVisible: boolean = this.Tip.VisibilityDefaultValue;

                this.TipVisibility = window.TipsVisibilities.filter(d=> d.TipCode == this.Tip.Code && d.UserId == SessionInfo.LoggedUserId)[0];
                if (this.TipVisibility) this.DontShow = !this.TipVisibility.IsVisible;
              
                else {

                    if (!this.IsFirstTipLoad) this.SaveChanges(true);
                
                }

            }

          
        }
   

    }


    //ShowAreaClick() {

    //    //this.TipIsVisible = true;
    //    this.TipMessage = this.Tip.ShortTextCodeCode;
    //    this.TipVisibility = window.TipsVisibilities.filter(d=> d.TipCode == this.Tip.Code && d.UserId == SessionInfo.LoggedUserId)[0];
    //    if (this.TipVisibility) {
    //        this.DontShow = !this.TipVisibility.IsVisible;
           
    //    }
    //    else {
    //        this.SaveChanges(true);
    //    }
    //}


    IsStartSave: boolean = false;
    SaveChanges(checkIsTipVisible: boolean) {

        if (!this.IsStartSave) {
            this.IsStartSave = true;
            this.isTipVisible = checkIsTipVisible;

            var tipVisibility: TipsVisibilityPM = window.TipsVisibilities.filter(d=> d.TipCode == this.Tip.Code && d.UserId == SessionInfo.LoggedUserId)[0];

      
            if (tipVisibility) {
                tipVisibility.IsVisible = this.isTipVisible;
                window.TipsVisibilities = window.TipsVisibilities.filter(d=> d.TipCode != this.Tip.Code && d.UserId != SessionInfo.LoggedUserId);
                window.TipsVisibilities.push(tipVisibility);
                this._tipsVisibilityService.update(tipVisibility).subscribe(res => {
                    this.IsStartSave = false;
                });
                // Update
            }
            else {
                tipVisibility = new TipsVisibilityPM();
                tipVisibility.IsVisible = this.isTipVisible;
                tipVisibility.TipCode = this.Tip.Code;
                tipVisibility.UserId = SessionInfo.LoggedUserId;
                tipVisibility.Tenant = SessionInfo.LoggedUserTenant;
        
                this._tipsVisibilityService.insert(tipVisibility).subscribe(res => {
                    var pmResponse: ServiceResponse = res;
                    this.IsStartSave = false;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {

                            window.TipsVisibilities.push(myResult);
                        }
                    }



                });

                //   Add
            }
        }

      


   
    }


    DontShowAginTipAreaClick() {
   
        this.SaveChanges(this.DontShow);
    }

    CloseToolTipArea() {

        if (this.HasTip) {
            this.TipVisibilityChangedEvent.emit("false");
        }

    }


}

