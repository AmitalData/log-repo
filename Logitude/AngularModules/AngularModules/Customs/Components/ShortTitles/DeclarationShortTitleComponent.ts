
import { Component, ChangeDetectorRef } from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {DeclarationPM} from '../../EntityPMs/DeclarationPM';
import {AmitalGatewayUtil} from '../../../Infrastructure/Utilities/AmitalGatewayUtil';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
//C:\LW\Customs\AngularModules\AngularModules\Customs\Controller\DeclarationEditComponentController.ts
import {DeclarationEditComponentController} from '../../Controller/DeclarationEditComponentController';
@Component({
    moduleId: module.id,
    templateUrl: "DeclarationShortTitleComponent.html",
})

//C: \LW\Customs\AngularModules\AngularModules\Infrastructure\Utilities\AmitalGatewayUtil.ts
export class DeclarationShortTitleComponent {
    public EntityPM: DeclarationPM;
    constructor(private cd: ChangeDetectorRef,public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;
        this.Listen();

        if (this.EntityPM != null) {
            this.BuildComponent();
        }
    }

    public EntityNumber: string = null;
    public _EntityNumber: string = null;
    _ShowEntityNumberClick: boolean = false;
    _CourierImporterName: string = null;
    private Listen() {
        if (SessionLocator.CurrentSession.CurrentEditComponent != null) {

     
            SessionLocator.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                SessionLocator.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess && SessionLocator.CurrentSession.CurrentEditComponent) {
                        this.EntityPM = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM;
                        this.BuildComponent();
                        this.cd.detectChanges();
                    }
                })
            );

   
        }
    }
    
    

    public get CourierImporterName() {
        if (this.EntityPM.ImporterCode) {
            this._CourierImporterName = this.EntityPM.CalculatedImporterName;
        }
        else {
            this._CourierImporterName = this.EntityPM.ImporterName;
        }
        return this._CourierImporterName;
    }
    public set CourierImporterName(newValue: string) { this._CourierImporterName = newValue; }

    private BuildComponent() {
        this._EntityNumber = null;
        this.EntityNumber = null;
        if (this.EntityPM.IsCourierDeclaration) {

            if (this.EntityPM.CustomFileNo && (this.EntityPM.CalculatedImporterName || this.EntityPM.ImporterName)) {
                this._EntityNumber = this.EntityPM.CustomFileNo;
            }
            else {
                this.EntityNumber = this.EntityPM.CustomFileNo;
            }


            if (this.EntityPM.ImporterCode) {
                this.CourierImporterName = this.EntityPM.CalculatedImporterName;
            }
            else {
                this.CourierImporterName = this.EntityPM.ImporterName;
            }

        }
        else {
            if (this.EntityPM.CustomFileNo && this.EntityPM.CustomerName) {
                this._EntityNumber = this.EntityPM.CustomFileNo;
            }
            else if (this.EntityPM.CustomFileNo == null && this.EntityPM.CustomerName) {
                this.EntityNumber = this.EntityPM.CustomerName;
            }
            else if (this.EntityPM.CustomFileNo && this.EntityPM.CustomerName == null) {
                this.EntityNumber = this.EntityPM.CustomFileNo;
            }

            
        }

        if (AmitalGatewayUtil.Instance.AmitalBrowserInUse && AmitalGatewayUtil.Instance.IsTabCA23) {
            this._ShowEntityNumberClick = true;
        }
    }

    EntityNumberClick() {   
        
        if (!SessionLocator.CurrentSession.CurrentEditComponent.EntityPM.IsDirty) {
            this.ShowCustomFileOPCFromDeclaration();
        } else {
            SessionLocator.CurrentSession.StartBusyIndicatorSaving();
            var sub = SessionLocator.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                SessionLocator.CurrentSession.StopBusyIndicator();
                sub.unsubscribe();
                if (isSaveSuccess) {
                    this.ShowCustomFileOPCFromDeclaration();
                } 
            });
            SessionLocator.CurrentSession.CurrentEditComponent.SaveChanges();
        }
    }

    ShowCustomFileOPCFromDeclaration() {

        var declarationEditComponentController: DeclarationEditComponentController = (SessionLocator.CurrentSession.CurrentEditComponent.EditComponentController as DeclarationEditComponentController)
        declarationEditComponentController.ForceCheckIfLockWhileReload();
         
        var unifreightMessageM =
            AmitalGatewayUtil.Instance.
                DeclarationMessaging.GetMessage(this.EntityPM.CustomFileNo, this.EntityPM.Id, "DeclarationShortTitleComponent");


        AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
            "ScriptableGatewayUtil.ShowCustomFileOPCFromDeclaration",
            "CFIHMAIN.LogitudeTask",
            "ShowCustomFileOPCFromDeclaration",
            unifreightMessageM,
            " פתיחת תיק עמילות מהצהרה");
    }
}
