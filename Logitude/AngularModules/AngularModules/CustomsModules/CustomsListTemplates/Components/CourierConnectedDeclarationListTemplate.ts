import {Component, ChangeDetectorRef} from '@angular/core';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {CourierMasterPM} from '../../../Customs/EntityPMs/CourierMasterPM';

@Component({
    moduleId: module.id,
    templateUrl: './CourierConnectedDeclarationListTemplate.html',
})

export class CourierConnectedDeclarationListTemplate {

    public rowData: any;
    public fieldName: any;
    fontcolor: string;
    entityPM: CourierMasterPM;
    IsConnectedDeclarationChecked: boolean = true;
    IsNotConnectedDeclarationChecked: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef) {
    }

    setVariables(rowData: any, fieldName: string, additionalData: any)
    {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.entityPM = this.CurrentSession.CurrentEditComponent.EntityPM as CourierMasterPM;

        this.BuildDeclarationsCheckBox();       
        this.CD.detectChanges();
    }


    BuildDeclarationsCheckBox() {
        let sNotConnectedDeclarations = this.entityPM.NotConnectedDeclarations as string;
        if (!AppTool.IsNullOrEmpty(sNotConnectedDeclarations)) {
            let NotConnectedDeclarations = sNotConnectedDeclarations.split(',')
            let res = NotConnectedDeclarations.filter(r => r == this.rowData.Id)[0];
            this.IsConnectedDeclarationChecked = AppTool.IsNullOrEmpty(res);
        }

        let sConnectedDeclarations = this.entityPM.ConnectedDeclarations as string;
        if (!AppTool.IsNullOrEmpty(sConnectedDeclarations)) {
            let ConnectedDeclarations = sConnectedDeclarations.split(',')
            let res = ConnectedDeclarations.filter(r => r == this.rowData.Id)[0];
            this.IsNotConnectedDeclarationChecked = !AppTool.IsNullOrEmpty(res);
        }
    }
    
    ShowDeclarationScreen() {
      //  this.EditEntity("Customs.Declaration", this.rowData.Id, null, "DEGC");

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityId: this.rowData.Id,
                    ObjectTableName: "Customs.Declaration"
                });
            });


    }

    OnConnectedCheckBoxChecked($event) {

        if (!this.entityPM.NotConnectedDeclarations) {
            this.entityPM.NotConnectedDeclarations = "";
        }
        if (!$event) {
            if (!this.entityPM.NotConnectedDeclarations.includes(this.rowData.Id)) {
                this.entityPM.NotConnectedDeclarations = this.entityPM.NotConnectedDeclarations + this.rowData.Id + ",";
            }
        }
        else {
            if (this.entityPM.NotConnectedDeclarations.includes(this.rowData.Id)) {

                this.entityPM.NotConnectedDeclarations = this.entityPM.NotConnectedDeclarations.replace(this.rowData.Id + ",", "");
            }
        }

    }

    OnNotConnectedCheckBoxChecked($event) {
        if (!this.entityPM.ConnectedDeclarations) {
            this.entityPM.ConnectedDeclarations = "";
        }
        if ($event) {
            if (!this.entityPM.ConnectedDeclarations.includes(this.rowData.Id)) {
                this.entityPM.ConnectedDeclarations = this.entityPM.ConnectedDeclarations + this.rowData.Id + ",";
            }
        }
        else {
            if (this.entityPM.ConnectedDeclarations.includes(this.rowData.Id)) {
                this.entityPM.ConnectedDeclarations = this.entityPM.ConnectedDeclarations.replace(this.rowData.Id + ",", "");
            }
        }
    }


    //public EditEntity(objectTableName: string, entityId: string, windowTitle: string, defaultSelectedTabCode: string) {
       

    //    var editWindow = new LogitudeWindow();

    //    editWindow.ShowHeaderButtons = true;
    //    editWindow.Title = windowTitle;
    //    editWindow.Height = 1000;
    //    editWindow.Width = 1500;
       
    //    this.CD.detach();
    //    editWindow.ShowEditComponent(entityId, objectTableName, defaultSelectedTabCode);
    //    editWindow.WindowClosed.subscribe(res => {
    //        this.CD.reattach();
           
           
    //    });

    //}


}
