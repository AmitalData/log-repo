import {Component, ChangeDetectorRef} from '@angular/core';
import {UserExtendedList} from '../../../Common/Services/ExtendedLists/UserExtendedListService';
import {UserLicensePM} from '../../../Common/EntityPMs/UserLicensePM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';

@Component({
    moduleId: module.id,
    templateUrl: './ColumnCheckBoxComponent.html',
})

export class ColumnCheckBoxComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef) {

    }
    
    public rowData: UserExtendedList;
    public fieldName: any;
    public packageCode: string;
    public columnIndex: string;

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.fieldName = fieldName.split(",");
        this.packageCode = this.fieldName[0];
        this.columnIndex = this.fieldName[1];
        
        this.SetIsChecked();   

        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }

    private SetIsChecked() {
        switch (this.columnIndex) {
            case "1":
                {
                    this.isChecked = this.rowData.IsChecked1;
                    break;
                }

            case "2":
                {
                    this.isChecked = this.rowData.IsChecked2;
                    break;
                }

            case "3":
                {
                    this.isChecked = this.rowData.IsChecked3;
                    break;
                }

            case "4":
                {
                    this.isChecked = this.rowData.IsChecked4;
                    break;
                }

            case "5":
                {
                    this.isChecked = this.rowData.IsChecked5;
                    break;
                }

            case "6":
                {
                    this.isChecked = this.rowData.IsChecked6;
                    break;
                }

            case "7":
                {
                    this.isChecked = this.rowData.IsChecked7;
                    break;
                }

            case "8":
                {
                    this.isChecked = this.rowData.IsChecked8;
                    break;
                }

            case "9":
                {
                    this.isChecked = this.rowData.IsChecked9;
                    break;
                }

            case "10":
                {
                    this.isChecked = this.rowData.IsChecked10;
                    break;
                }
        }
    }

    private isChecked: boolean;
    get IsChecked() { return this.isChecked; }
    set IsChecked(newValue: boolean) {
        if (this.isChecked != newValue) {
            this.isChecked = newValue;

            if (newValue) {
                this.CurrentSession.PseventRowSelectEvent.emit({ Name: "Add", User: this.rowData, PackageCode: this.packageCode });
            }

            else {
                this.CurrentSession.PseventRowSelectEvent.emit({ Name: "Remove", User: this.rowData, PackageCode: this.packageCode });
            }
        }
    }
}
