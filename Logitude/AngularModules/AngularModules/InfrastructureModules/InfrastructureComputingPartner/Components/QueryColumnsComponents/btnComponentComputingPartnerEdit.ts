import {Component, ViewContainerRef, OnInit, ViewChildren, QueryList, Output, EventEmitter, ChangeDetectorRef} from '@angular/core';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {CommonDomainService, CustomApiQueryFilters} from '../../../../Common/Services/CommonDomainService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
declare var window: any;

@Component({
    moduleId: module.id,
    selector: 'btnComponentComputingPartnerEdit',
    templateUrl: './btnComponentComputingPartnerEdit.html',
})

export class btnComponentComputingPartnerEdit {
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef) { }
    public $event: any;
    Edit() {
        var serviceEntity: EntityResourceService = new EntityResourceService();
                      serviceEntity.getEntityResourceByTableName("ComputingPartnerTranslation", 0).subscribe(p => {
                            var logWindow = new LogitudeWindow();
                            logWindow.Width = 500;
                            logWindow.Height = 300;
                            logWindow.Title = "Edit " + this.rowData.ObjectTableName + " Translation";
                            logWindow.WindowArgs = { entityPM: this.rowData };
                            logWindow.Show('./InfrastructureModules/InfrastructureComputingPartner/Components/EditTranslationComputingPartners');
                            logWindow.ComponentLoaded.subscribe(cmpRef => {
                                cmpRef.BackCompleted.subscribe(($event1: any) => {
                                    this.OnBackFromEdit(this.entityId, this.rowData)
                                });
                            });
                        });           
    }


    OnBackFromEdit(selectedEntityId, $event) {

        this.CurrentSession.PseventRowSelectEvent.emit({ Name: 'btnComponentComputingPartnerEdit', Value: this.rowData, RowIndex: this.AdditionalData.rowIndex}); 

    }
    public rowData: any;
    public fieldName: any;
    public AdditionalData: any;
    public InUseVisibile: boolean = true;
    public entityId: string;


    setVariables(rowData: any, fieldName: string, additionalData: any) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.AdditionalData = additionalData;
        //this.Check();
        this.InUseVisibile = this.rowData.InUse;
        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }


}
