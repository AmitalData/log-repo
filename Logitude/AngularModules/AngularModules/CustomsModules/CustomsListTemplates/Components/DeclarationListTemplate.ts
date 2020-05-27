

import {Component, ChangeDetectorRef} from '@angular/core';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { DeclarationList } from '../../../Customs/EntityLists/DeclarationList';
import { DeclarationListService } from '../../../Customs/Services/StandardLists/DeclarationListService';
import { AppTool } from '../../../Infrastructure/Tools';
import { CacheCourierPendingReasonService } from "../../../Customs/Services/Others/CacheCourierPendingReasonService";
import { CourierPendingReasonListService } from '../../../Customs/Services/StandardLists/CourierPendingReasonListService';
import { CourierPendingReasonList } from '../../../Customs/EntityLists/CourierPendingReasonList';



@Component({
    templateUrl: './DeclarationListTemplate.html',
})

export class DeclarationListTemplate {
    public rowData: any;
    public fieldName: any;
    _DeclarationList: DeclarationList;

    constructor(private cd: ChangeDetectorRef) {

    }
    setVariables(rowData: any, fieldName: string) {
      
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.cd.detectChanges();
    }

    set CourierPendingReasonListToolTip(value: string) {
        if (this.CourierPendingReasonListToolTip != value) {
            this.CourierPendingReasonListToolTip = value;
        }
    }

    get CourierPendingReasonListToolTip() {

        if (AppTool.IsNullOrEmpty(this._DeclarationList.CourierPendingReasonList)) {
            return "";
        }
        if (this._DeclarationList.CourierPendingReasonList.indexOf(',') < 0) {
            return this._DeclarationList.CourierPendingReasonName;
        }

        let myToolTip = "";
        var mycache: Array<CourierPendingReasonList> = CacheCourierPendingReasonService.Instance.GetCache();
        let listString: string = this._DeclarationList.CourierPendingReasonList;
        let arry = listString.split(',');
        arry.forEach(itemReason => {
            let rec = mycache.filter(r => r.Code == itemReason)[0];
            if (rec != null) {
                if (!AppTool.IsNullOrEmpty(myToolTip)) {
                    myToolTip += '\n'
                }
                if (!AppTool.IsNullOrEmpty(rec.LocalName)) {
                    myToolTip += rec.LocalName;
                } else if (!AppTool.IsNullOrEmpty(rec.EnglishName)) {
                    myToolTip += rec.EnglishName;
                } else {
                    myToolTip += rec.Code;
                }
            }
        });

        return myToolTip;
    }

    get CourierPendingReasonListText() {
        if (AppTool.IsNullOrEmpty(this._DeclarationList.CourierPendingReasonList)) {
            return "";
        }
        if (this._DeclarationList.CourierPendingReasonList.indexOf(',') < 0) {
            return this._DeclarationList.CourierPendingReasonName;
        }
        return "הצג רשימה";
    }

    getCourierPendingReasonName(courierPendingReason: string, isToolTip: boolean) {
        var toolTip = courierPendingReason
        if (!AppTool.IsNullOrEmpty(toolTip) && toolTip.indexOf(',') < 0) {
            if (this._DeclarationList != null && this._DeclarationList.CourierPendingReasonName != null) {
                toolTip = this._DeclarationList.CourierPendingReasonName;
            }
            else {
                var myCourierPendingReasonListService = new CourierPendingReasonListService();
                myCourierPendingReasonListService.getSingleFromCache(toolTip)
                    .subscribe(serviceResponse => {
                        var CourierPendingReason = serviceResponse.Result as CourierPendingReasonList;
                        toolTip = CourierPendingReason.LocalName;
                    });
            }
        }
        return toolTip;
    }
}
