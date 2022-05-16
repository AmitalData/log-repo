import {Component, ChangeDetectorRef} from '@angular/core';
import {UserExtendedList} from '../../../Common/Services/ExtendedLists/UserExtendedListService';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';

@Component({
    
    templateUrl: './ColumnCheckBoxComponent.html',
})

export class ColumnCheckBoxComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef) {

    }
    
    public rowData: UserExtendedList;
    public fieldName: any;
    public packageCode: string;
    public packageName: string;
    public columnIndex: string;
    public IsEnabled: boolean = true;
    public LicenseManagementTitle = "";
    public mainAdditionalPackageApplied = "false";

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.fieldName = fieldName.split(",");
        this.packageCode = this.fieldName[0];
        this.columnIndex = this.fieldName[1];
        this.packageName = this.fieldName[2];
        this.mainAdditionalPackageApplied = this.fieldName[3];
        this.SetIsChecked();
        this.SetLicenseManagementTitle();
    
        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }

    private SetLicenseManagementTitle() {
        this.LicenseManagementTitle = "";
        if (this.mainAdditionalPackageApplied == "true")
            this.LicenseManagementTitle = "To remove the main package (" + this.packageName + ") from a user, please make sure the 'Additional Packages Only' field is checked for that user.";
    }

    private SetIsChecked() {
        switch (this.columnIndex) {
            case "0":
                {
                    this.isChecked = this.rowData.AdditionalPackagesOnly ? false : true;
                    this.IsEnabled = false;
                    break;
                }

            case "1":
                {
                    this.isChecked = this.rowData.IsChecked1;
                    this.SetIsEnabled(this.rowData.IsChecked1);
                    break;
                }

            case "2":
                {
                    this.isChecked = this.rowData.IsChecked2;
                    this.SetIsEnabled(this.rowData.IsChecked2);
                    break;
                }

            case "3":
                {
                    this.isChecked = this.rowData.IsChecked3;
                    this.SetIsEnabled(this.rowData.IsChecked3);
                    break;
                }

            case "4":
                {
                    this.isChecked = this.rowData.IsChecked4;
                    this.SetIsEnabled(this.rowData.IsChecked4);
                    break;
                }

            case "5":
                {
                    this.isChecked = this.rowData.IsChecked5;
                    this.SetIsEnabled(this.rowData.IsChecked5);
                    break;
                }

            case "6":
                {
                    this.isChecked = this.rowData.IsChecked6;
                    this.SetIsEnabled(this.rowData.IsChecked6);
                    break;
                }

            case "7":
                {
                    this.isChecked = this.rowData.IsChecked7;
                    this.SetIsEnabled(this.rowData.IsChecked7);
                    break;
                }

            case "8":
                {
                    this.isChecked = this.rowData.IsChecked8;
                    this.SetIsEnabled(this.rowData.IsChecked8);
                    break;
                }

            case "9":
                {
                    this.isChecked = this.rowData.IsChecked9;
                    this.SetIsEnabled(this.rowData.IsChecked9);
                    break;
                }

            case "10":
                {
                    this.isChecked = this.rowData.IsChecked10;
                    this.SetIsEnabled(this.rowData.IsChecked10);
                    break;
                }
        }
    }

    private SetIsEnabled(isChecked: boolean) {
        if (this.rowData.InActive) {
            if (isChecked) {
                this.IsEnabled = true;
            }

            else {
                this.IsEnabled = false;
            }
        }

        else {
            this.IsEnabled = true;
        }
    }

    private isChecked: boolean;
    get IsChecked() { return this.isChecked; }
    set IsChecked(newValue: boolean) {
        if (this.isChecked != newValue) {
            this.isChecked = newValue;

            var refreshAfterRemove: boolean = false;
            if (this.rowData.InActive) {
                if (!newValue) {
                    this.IsEnabled = false;
                    refreshAfterRemove = true;
                }
            }

            if (newValue) {
                this.CurrentSession.PseventRowSelectEvent.emit({ Name: "Add", User: this.rowData, PackageCode: this.packageCode });
            }

            else {
                this.CurrentSession.PseventRowSelectEvent.emit({ Name: "Remove", User: this.rowData, PackageCode: this.packageCode, Refresh: refreshAfterRemove });
            }
        }
    }
}
