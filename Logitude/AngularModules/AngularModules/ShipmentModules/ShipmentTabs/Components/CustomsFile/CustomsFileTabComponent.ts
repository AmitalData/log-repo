import {Component, OnInit}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    moduleId: module.id,

    templateUrl: './CustomsFileTabComponent.html',
})

export class CustomsFileTabComponent implements OnInit {
    public EntityPM: ShipmentPM;
    public ObjectTableName: string;
    public InfoMessage: string = null;
    public IsButtonEnabled: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
    }

    ngOnInit() {
        if (this.EntityPM != null) {

            if (this.EntityPM.ShipmentLevelCode == "D" || this.EntityPM.ShipmentLevelCode == "H") {
                if (this.EntityPM.CustomFileId != null) {
                    this.IsButtonEnabled = true;
                }
            }

            if (this.EntityPM.CustomFileId == null) {
                this.InfoMessage = "This shipment is not connected to a customs file";
            }            
        }
    }

    ButtonClicked() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: this.EntityPM.CustomFileId, ObjectTableName: 'Shipment' });
            });
    }
}
