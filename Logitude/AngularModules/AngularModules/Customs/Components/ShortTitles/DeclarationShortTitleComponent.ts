
import { Component, ChangeDetectorRef } from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {DeclarationPM} from '../../EntityPMs/DeclarationPM';
import {AmitalGatewayUtil, UnifreightMessageM} from '../../../Infrastructure/Utilities/AmitalGatewayUtil';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
//C:\LW\Customs\AngularModules\AngularModules\Customs\Controller\DeclarationEditComponentController.ts
import {DeclarationEditComponentController} from '../../Controller/DeclarationEditComponentController';
@Component({
    
    templateUrl: "DeclarationShortTitleComponent.html",
})

//C: \LW\Customs\AngularModules\AngularModules\Infrastructure\Utilities\AmitalGatewayUtil.ts
export class DeclarationShortTitleComponent {
    public EntityPM: DeclarationPM;
    private CurrentSession = SessionLocator.SelectedSession;
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
        if (this.CurrentSession.CurrentEditComponent != null) {

     
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess && this.CurrentSession.CurrentEditComponent) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
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
        
        if (!this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty) {
            this.ShowCustomFileOPCFromDeclaration();
        } else {
            this.CurrentSession.StartBusyIndicatorSaving();
            var sub = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                this.CurrentSession.StopBusyIndicator();
                sub.unsubscribe();
                if (isSaveSuccess) {
                    this.ShowCustomFileOPCFromDeclaration();
                } 
            });
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
    }

    ShowCustomFileOPCFromDeclaration() {

        var declarationEditComponentController: DeclarationEditComponentController = (this.CurrentSession.CurrentEditComponent.EditComponentController as DeclarationEditComponentController)
        declarationEditComponentController.ForceCheckIfLockWhileReload();

        let myViewModelName = "DeclarationShortTitleComponent";

        SessionLocator.SelectedSession.StopBusyIndicator();
        let sub = AmitalGatewayUtil.Instance.UnifaceRequestArrived
            .subscribe(
                (mess: UnifreightMessageM) => {
                    var IsMatchUnifreightCallbackCommand = (
                        mess.LogitudeEntity == AmitalGatewayUtil.Instance.DeclarationMessaging.LogitudeEntityDeclaration &&
                        mess.LogitudeEntityNumber == this.EntityPM.Id &&
                        mess.LogitudeViewModel == myViewModelName);
                    if (IsMatchUnifreightCallbackCommand) {
                        sub.unsubscribe();
                        SessionLocator.SelectedSession.StopBusyIndicator();
                        SessionLocator.SelectedSession.CurrentEditComponent.ReloadEntityPM();
                    }
                }
            );

        var unifreightMessageM =
            AmitalGatewayUtil.Instance.
                DeclarationMessaging.GetMessage(this.EntityPM.CustomFileNo, this.EntityPM.Id, myViewModelName
                    , AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightEntity(this.EntityPM.Direction));


        AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
            "ScriptableGatewayUtil.ShowCustomFileOPCFromDeclaration",
            "CFIHMAIN.LogitudeTask",
            "ShowCustomFileOPCFromDeclaration",
            unifreightMessageM,
            " פתיחת תיק עמילות מהצהרה");
    }
}
