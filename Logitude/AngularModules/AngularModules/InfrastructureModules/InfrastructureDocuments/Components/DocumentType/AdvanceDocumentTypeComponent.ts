declare var System: any;
declare var window: any;
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import 'rxjs/add/operator/map';
import {Component, OnInit }  from '@angular/core';
import {DocumentTypePM} from '../../../../Common/EntityPMs/DocumentTypePM';

@Component({
    moduleId: module.id,
    selector: 'advancedocumentType',
    templateUrl: './AdvanceDocumentTypeComponent.html',
})

export class AdvanceDocumentTypeComponent implements OnInit {


    IsHouse: boolean = false;
    IsDirect: boolean = false;
    IsMaster: boolean = false;
    IsInland: boolean = false;
    IsOcean: boolean = false;
    IsAir: boolean = false;

 

    EntityPM: DocumentTypePM;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {


    }

    ngOnInit(


    ) {





    }

    ShowDeflut: boolean;
    SetDataContext(entityPM: DocumentTypePM) {

        this.EntityPM = entityPM;
        this.IsAir = this.EntityPM.IsAir;
        this.IsDirect = this.EntityPM.IsDirect;
        this.IsHouse = this.EntityPM.IsHouse;
        this.IsMaster = this.EntityPM.IsMaster;
        this.IsOcean = this.EntityPM.IsOcean;
        this.IsInland = this.EntityPM.IsInland;
        var objecttable = window.ObjectTables.filter((d:any) => d.Name == entityPM.ObjectTableName)[0];
    if (objecttable.Name == "Shipment") {
        this.ShowDeflut = true;
    }
    else this.ShowDeflut = false;
        //ObjectTablePM objectTablePm = TenantContext.Current.ObjectTable.Where(d => d.Id == EntityPm.ObjectTableId).FirstOrDefault();

        //if (objectTablePm != null) {
        //    if (objectTablePm.Name == "Shipment") {
        //        showDefaultsShipment = Visibility.Visible;
        //    }
        //    else {
        //        showDefaultsShipment = Visibility.Collapsed;
        //    }
        //}






    }


    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }    

    SaveButtonClicked() {

        this.EntityPM.IsAir = this.IsAir;
        this.EntityPM.IsDirect = this.IsDirect;
        this.EntityPM.IsHouse = this.IsHouse;
        this.EntityPM.IsMaster = this.IsMaster;
        this.EntityPM.IsOcean = this.IsOcean;
        this.EntityPM.IsInland = this.IsInland;
        this.CurrentSession.CloseCurrentWindow();
    }





}
