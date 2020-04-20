import {Component} from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import { TariffPM } from '../../EntityPMs/TariffPM';
import {AppTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';

@Component({
    moduleId: module.id,
    templateUrl: "./TariffShortTitleComponent.html",
})

export class TariffShortTitleComponent {
    public EntityPM: TariffPM;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;
    }
    
    ViewSeller() {
        if (this.EntityPM != null) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.SellerId)) {
                var objectTableName = null;

                switch (this.EntityPM.SellerPartnerTypeId) {
                    case "SL": { objectTableName = "ShippingLine"; break; }
                    case "AL": { objectTableName = "Airline"; break; }
                }

                if (!AppTool.IsNullOrEmpty(objectTableName)) {
                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run({ EntityId: this.EntityPM.SellerId, ObjectTableName: objectTableName, BackButtonLabel: 'Tariff' });

                            let isEditComponentSaved = false;
                            cmpRef.instance.BackCompleted.subscribe(bk => {
                                if (isEditComponentSaved) {
                                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                                }
                            });

                            cmpRef.instance.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                                if (isSaveSuccess) {
                                    isEditComponentSaved = true;
                                }
                            });
                        });
                }
            }
        }
    }
}
