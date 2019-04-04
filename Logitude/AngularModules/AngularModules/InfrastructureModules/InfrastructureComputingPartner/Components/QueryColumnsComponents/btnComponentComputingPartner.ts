import {Component, ViewContainerRef, OnInit, ViewChildren, QueryList, Output, EventEmitter, ChangeDetectorRef} from '@angular/core';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ContactListService} from '../../../../Common/Services/StandardLists/ContactListService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
@Component({
    moduleId: module.id,
    selector: 'btnComponentComputingPartner',
    templateUrl: './btnComponentComputingPartner.html',
})

export class btnComponentComputingPartner   {
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef) { }
    MoreDetails() {
        var ServiceContact: ContactListService = new ContactListService();
        this.CurrentSession.StartBusyIndicatorLoading();
        ServiceContact.getSingle(this.rowData.CreatedByUserId).subscribe(res => {
            if (!res.HasError) {
                if (res.Result != null)
                    this.rowData.CreatedByUserName = res.Result.EnglishName;
            }
            ServiceContact.getSingle(this.rowData.UpdatedByUserId).subscribe(res => {
                if (!res.HasError) {
                    if (res.Result != null)
                        this.rowData.UpdatedByUserName = res.Result.EnglishName;
                }
                this.CurrentSession.StopBusyIndicator();

                var logWindow = new LogitudeWindow();
                logWindow.Width = 500;
                logWindow.Height = 300;
                logWindow.Title = "More Details";
                logWindow.WindowArgs = { entityPM: this.rowData };
                logWindow.Show('./InfrastructureModules/InfrastructureComputingPartner/Components/TranslationDetailsComponent');
            });
        });
    }
    public rowData: any;
    public fieldName: any;
    public InUseVisibile: boolean = true;
    public entityId: string;


    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        //this.Check();
        this.InUseVisibile = this.rowData.InUse;
        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }


}
