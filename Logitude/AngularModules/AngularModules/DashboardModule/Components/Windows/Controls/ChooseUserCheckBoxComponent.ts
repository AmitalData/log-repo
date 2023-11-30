import { Component, ChangeDetectorRef } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
declare var window: any;

@Component({
    templateUrl: './ChooseUserCheckBoxComponent.html',
})
export class ChooseUserCheckBoxComponent extends BaseComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    public rowData;
    constructor(private cd: ChangeDetectorRef) {
        super();
    }

    setVariables(rowData: any) {
        this.rowData = rowData;

        if (this.rowData.Email) {
            if (window.DashboardUsers) {
                var item = window.DashboardUsers.filter(d => d.UserId == this.rowData.Id)[0];
                if (item) {
                    this.IsChecked = true;
                } else {
                    this.IsChecked = false;
                }
            }

            this.Destroyed();
            this.isCheckedFlag = false;
        }
    }

    private isChecked: boolean = false;
    public get IsChecked() { return this.isChecked; }
    public set IsChecked(value: boolean) {
        if (this.isChecked != value) {
            this.isChecked = value;

            this.isCheckedFlag = true;
            this.FireCheckedEvent();
        }
    }

    Checkclick(item: any) {
        if (item.Email) {
            this.cd.detectChanges();
            this.isClickedFlag = true;
            this.selectedItem = item;
            this.FireCheckedEvent();
        }
    }

    private isCheckedFlag: boolean = false;
    private isClickedFlag: boolean = false;
    private selectedItem: any = null;
    FireCheckedEvent() {
        if (this.selectedItem != null && this.isCheckedFlag && this.isClickedFlag) {
            var select = new UserParameterInput(this.selectedItem.Email, this.selectedItem.Id, this.selectedItem.EnglishName, this.IsChecked);
            this.Destroyed();
            this.CurrentSession.SessionEvent.emit({ Name: "ChooseUserCheckBoxComponent", select: select });
            this.selectedItem = null;
            this.isCheckedFlag = false;
            this.isClickedFlag = false;
        }
    }

    Destroyed() {
        var isDestroyed: boolean = this.cd['destroyed'];
        if (!isDestroyed) {
            this.cd.detectChanges();
        }
    }
}

class UserParameterInput {
    Email: string;
    UserId: string;
    UserName: string;
    IsCheck: boolean;
    constructor(email: any, userId: string, userName: string, isCheck: boolean) {
        this.Email = email;
        this.UserId = userId;
        this.UserName = userName;
        this.IsCheck = isCheck;
    }
}


