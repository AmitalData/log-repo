
import { Component, ChangeDetectorRef } from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
 import {AmitalGatewayUtil, UnifreightMessageM} from '../../../Infrastructure/Utilities/AmitalGatewayUtil';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
//C:\LW\Customs\AngularModules\AngularModules\Customs\Controller\DeclarationEditComponentController.ts
import {DeclarationEditComponentController} from '../../Controller/DeclarationEditComponentController';
import { ExportStorgePM } from '../../EntityPMs/ExportStorgePM';
@Component({
    
    templateUrl: "ExportStorgeShortTitleComponent.html",
})

//C: \LW\Customs\AngularModules\AngularModules\Infrastructure\Utilities\AmitalGatewayUtil.ts
export class ExportStorgeShortTitleComponent {
    public EntityPM: ExportStorgePM;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private cd: ChangeDetectorRef,public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;
        this.Listen();

        if (this.EntityPM != null) {
         }
    }

    public EntityNumber: string = null;
    public _EntityNumber: string = null;
    _ShowEntityNumberClick: boolean = false;
    _CourierImporterName: string = null;
    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

     
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess && this.CurrentSession.CurrentEditComponent) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                         this.cd.detectChanges();
                    }
                })
            );

   
        }
    }
    
  

 
 }
