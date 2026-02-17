

import {Component, ChangeDetectorRef} from '@angular/core';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {AppTool,DateTool} from '../../../Infrastructure/Tools';
import {DateAgeHelper} from '../../../Infrastructure/Utilities/DateAgeHelper';
import { NotificationExtendedListService } from '../../../Customs/Services/ExtendedLists/NotificationExtendedListService';

@Component({
    moduleId: module.id,
    templateUrl: './NotificationListTemplate.html',
})


export class NotificationListTemplate {


    public rowData: any;
    public fieldName: any;
    public AdditionalData: any;
    IconeVisibility: boolean = false;
    BlueIconVisibility: boolean = false;
    IsSeenByAssignee: boolean;
    ShowGreenTick: boolean = true;
    notificationExtendedListService: NotificationExtendedListService = new NotificationExtendedListService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef) {
    }

    setVariables(rowData: any, fieldName: string,additionalData:any) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.AdditionalData = additionalData;
        this.AdditionalData.RowOverEvent.subscribe(index => {
            if (this.IsClosed && fieldName == 'IsClosedByAssignee') {
                var template = document.getElementById('Text' + index);
                var template1 = document.getElementById('Tick' + index);
                if (template) {
                    template.style.display = 'block'; 
                }
                if (template1) {
                    template1.style.display = 'none';
                }
            }
            //alert("RowOverEvent");
        });
        this.AdditionalData.RowOutEvent.subscribe(index => { 
            if (this.IsClosed && fieldName == 'IsClosedByAssignee') {
                var template = document.getElementById('Text' + index);
                var template1 = document.getElementById('Tick' + index);
                if (template) {
                    template.style.display = 'none';
                }
                if (template1) {
                    template1.style.display = 'block';
                }
            }
        });
        this.IsSeenByAssignee = rowData.IsSeenByAssignee;
        if (rowData.IsClosedByAssignee) {
            this.IsClosed = true;
        }
        if (this.IsSeenByAssignee) {
            console.log("seen");
        }
        if (this.rowData.AssigneToNotificationTypeCode == "A") {
            this.IconeVisibility = true;
            this.BlueIconVisibility = false;
        }
        else {
            this.IconeVisibility = false;
        }

        if (this.rowData.NotificationDefinitionCode == "5101N") {
            this.BlueIconVisibility = true;
            this.IconeVisibility = false;

        }

        var valueDate = new Date(rowData.DueDate.valueOf()).valueOf();
        var today = DateTool.GetCurrentDateAsUtc().valueOf();
        if (valueDate != null && valueDate < today) {
            this.datecolor = "#ff6a00";
            this.fontcolor = "#ffffff";
            }

            else {
            this.datecolor = "#F0F0F0";
            this.fontcolor = "#6E7172";
            }

        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }

    }
    fontcolor: string;
    datecolor: string;
    ClosedByAssignee: string = null;
    ClosedByAssigneeClicked() {
        this.CurrentSession.FireEvent({ Name: 'ClosedByAssigneeClicked', rowIndex: this.AdditionalData.rowIndex, gridId: this.AdditionalData.gridId });
        this.notificationExtendedListService.PutNotificationsStatus(this.rowData).subscribe(response => {
            if (response) {
                if (!response.HasError) {
                    this.IsClosed = true;
                    this.ClosedByAssignee = SessionLocator.LoggedUserPM.LocalName;
                }
            }
        });



    }
    IsClosed: boolean = false;

    FirePreventSelect() {
        this.CurrentSession.PseventRowSelectEvent.emit("select");
    }


}
