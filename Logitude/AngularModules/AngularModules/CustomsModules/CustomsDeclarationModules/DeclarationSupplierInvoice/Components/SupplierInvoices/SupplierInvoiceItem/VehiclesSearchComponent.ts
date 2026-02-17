import {Component, ChangeDetectorRef}  from '@angular/core';
import {VehicleExtendedListService} from '../../../../../../Customs/Services/ExtendedLists/VehicleExtendedListService';
import {BaseComponent} from '../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { VehicleList } from '../../../../../../Customs/EntityLists/VehicleList';
import {EntityResourceService} from '../../../../../../Infrastructure/Services/EntityResourceService';
import {ObservableCollection} from '../../../../../../Infrastructure/Utilities/ObservableCollection';
import {SessionLocator} from '../../../../../../Infrastructure/Utilities/SessionLocator';
import {SupplierInvoiceItemVehicleComponent} from './SupplierInvoiceItemVehicleComponent';

@Component({
    moduleId: module.id,
    templateUrl: './VehiclesSearchComponent.html',
})

export class VehiclesSearchComponent extends BaseComponent {
    vehicleListService: VehicleExtendedListService = new VehicleExtendedListService();
    entityResourceService: EntityResourceService = new EntityResourceService();
    Parent: SupplierInvoiceItemVehicleComponent;
    public vehicles = new ObservableCollection([]);
    public selectedVehicles = new ObservableCollection([]);

    DataContext: any = this;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private cd: ChangeDetectorRef) {
        super();
       
        
    }

    tempSelectedRows: any[] = [];
    LaodVehicles() {
       // this.vahicles.Clear();
        this.tempSelectedRows = [];
        this.vehicleListService.GetVehiclesForSelection().subscribe(response => {
            if (response) {

                //response.Result.forEach((vehicle) => {
                //    this.vehicles.Insert(vehicle);
                //});
                this.vehicles.InsertCollection(response.Result);
              
                this.Parent.invoiceItemPM.SupplierInvoiceItemVehicles.forEach((vehicle) => {
                    var item: VehicleList = this.vehicles.Collection.filter(d => d.Id  == vehicle.VehicleId)[0];
                    if (item) {
                        if (!this.selectedVehicles.Collection.includes(item)) {
                            this.selectedVehicles.Collection.push(item);
                        }
                        this.cd.detectChanges();
                    }
                });
                this.tempSelectedRows = this.selectedVehicles.Collection;
            }

        });

    }

    SetWindowArgs(args:any) {
        this.Parent = args.Parent;
        this.entityResourceService.getEntityResourceByTableName("Customs.Vehicle").subscribe(response => {
            this.LaodVehicles();
        });
    }

    SelectedRow: VehicleList;
    SelectedRows: VehicleList[] = [];
  
    OnRowSelected(items: VehicleList[]) {
        this.SelectedRows = items;
        //this.selectedVehicles.Collection.forEach((vehicle) => {
        //    var item = items.filter(d => d.Id == vehicle.Id)[0];
        //    if (!item) {
        //        this.selectedVehicles.Remove(vehicle);
        //    }
        //});
        
    }

    CancelButtonClicked() {

        this.CurrentSession.CloseCurrentWindow();
        


    }

    OkButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
        this.Parent.SelectVehicleCompleted(this.SelectedRows);

   
   
    }

}
