import {Component, ChangeDetectorRef} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';



@Component({
    moduleId: module.id,

    selector: 'CertificateCheckBoxComponent',
    templateUrl: './CertificateCheckBoxComponent.html',
})

export class CertificateCheckBoxComponent{

    public rowData: any;
    public fieldName: any;
    publish: boolean = true;
    isAllSelected: boolean;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private cd: ChangeDetectorRef) {
        this.CurrentSession.SubscriptionAdd(
            this.CurrentSession.ConnectedItemSelectedEvent.subscribe((res) => {
                this.publish = false;
                if (res.Count == "All") {

                    this.IsSelected = true;
                    this.isAllSelected = true;

                }
                else {
                    this.IsSelected = false;
                    this.isAllSelected = false;
                }


                this.publish = true;
            })
        );
    }


    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.fieldName = fieldName;
    


    }

    private isSelected: boolean;
    public get IsSelected() { return this.rowData.IsSelected };
    public set IsSelected(value: boolean) {
        this.isSelected = value;
            if (this.isSelected) {
                //if (!this.isAllSelected) {
                    this.CurrentSession.SelectItemEvent.emit({ data: this.rowData, selected: true });
                //}
                this.rowData.IsSelected = true;
            }
            else {
                this.rowData.IsSelected = false;
                //if (!this.isAllSelected) {
                    this.CurrentSession.SelectItemEvent.emit({ data: this.rowData, selected: false });
                //}

        }

            var isDestroyed: boolean = this.cd['destroyed'];
            if (!isDestroyed) {
                this.cd.detectChanges();
            }

        
        
    }

    FirePreventSelect() {
        this.CurrentSession.PseventRowSelectEvent.emit("certificate");
    }
   

}
