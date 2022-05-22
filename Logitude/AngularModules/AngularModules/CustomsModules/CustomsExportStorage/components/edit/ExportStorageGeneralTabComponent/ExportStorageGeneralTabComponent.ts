import { Component, OnInit } from '@angular/core';
import { ExportStoragePM } from 'Customs/EntityPMs/ExportStoragePM';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';

@Component({
  selector: 'app-edit-export-storage',
  templateUrl: './ExportStorageGeneralTabComponent.html',
  styleUrls: ['./ExportStorageGeneralTabComponent.scss']
})
export class ExportStorageGeneralTabComponent extends BaseComponent {
  public DataContext: ExportStorageGeneralTabComponent = this;
  EntityPM: ExportStoragePM = null as any;
  public ObjectTableName: string = '';


  constructor(private entityArgs: EntityArgs) {
    super();
  }


  ngOnInit(): void {
    this.initEntity()

    this.disabledInputs();
    
  }


  initEntity() {
    this.EntityPM = this.entityArgs.EntityPM;
    this.ObjectTableName = this.entityArgs.ObjectTableName;
  }


  disabledInputs() {
    [
      'ShipType',
      'CargoTypeCodeName',
      'SecondCargoID',
      'ExporterCode',
      'FirstCargoID',
      'ThirdCargoID',
      'ExportDealIdentification',
      'PackageQuantity',
        'GrossMassMeasure',
        'MarksNumbers',
        'ExportLoadingPortcode',
        'StorageSiteCode',
        'ExportUnloadingPortCode',
        'FinalDestinationPortCode',
        'IsDangerousGoods'
    ].forEach(fieldName => this.UIProperties.SetEnabled(fieldName, this.ObjectTableName, false))
  }


  public get ShipType() { return this.EntityPM.DeclarationId ? 'יצוא' : ''; }
  public get CargoTypeCodeName() { return this.EntityPM.CargoTypeCodeName; }
  public get SecondCargoID() { return this.EntityPM.SecondCargoID; }
  public get ExporterID() { return this.EntityPM.ExporterID; }
  public get FirstCargoID() { return this.EntityPM.FirstCargoID; }
  public get ThirdCargoID() { return this.EntityPM.ThirdCargoID; }
  public get ExportDealIdentification() { return this.EntityPM.ExportDealIdentification; }
  public get ExporterCode() { return this.EntityPM.ExporterCode; }

  public get PackageQuantity() {return this.EntityPM.PackageQuantity}
  
  public get GrossMassMeasure() {return this.EntityPM.GrossMassMeasure}
  
  public get MarksNumbers() {return this.EntityPM.MarksNumbers}
  
  public get ExportLoadingPortcode() {return this.EntityPM.ExportLoadingPortcode}
  
  public get StorageSiteCode() {return this.EntityPM.StorageSiteCode}
  
  public get ExportUnloadingPortCode() {return this.EntityPM.ExportUnloadingPortCode}
  
  public get FinalDestinationPortCode() {return this.EntityPM.FinalDestinationPortCode}
  
  public get IsDangerousGoods() {return this.EntityPM.IsDangerousGoods===undefined?false:this.EntityPM.IsDangerousGoods;} 

}
