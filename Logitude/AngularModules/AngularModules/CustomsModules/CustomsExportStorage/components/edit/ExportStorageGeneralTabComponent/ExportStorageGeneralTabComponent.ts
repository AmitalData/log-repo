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
  // public EntityPM: ExportStoragePM = null as any;
  public ObjectTableName: string = '';

  
  constructor(private entityArgs: EntityArgs) {
    super();
  }

  
  ngOnInit(): void {
    this.initEntity()
    console.log(1)
    console.log(this.entityArgs.EntityPM)
    // this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe((response:any) => {

  }


  initEntity() {
    this.EntityPM = this.entityArgs.EntityPM;
    this.ObjectTableName = this.entityArgs.ObjectTableName;
  }

  
  public get CargoTypeCodeName() { return this.EntityPM.CargoTypeCodeName; }
  public set CargoTypeCodeName(newValue: number) { this.EntityPM.CargoTypeCodeName = newValue; }

  public get SecondCargoID() { return this.EntityPM.SecondCargoID; }
  public set SecondCargoID(newValue: number) { this.EntityPM.SecondCargoID = newValue; }

  public get ExporterID() { return this.EntityPM.ExporterID; }
  public set ExporterID(newValue: number) { this.EntityPM.ExporterID = newValue; }
  
  public get FirstCargoID() { return this.EntityPM.FirstCargoID; }
  public set FirstCargoID(newValue: number) { this.EntityPM.FirstCargoID = newValue; }

  public get ThirdCargoID() { return this.EntityPM.ThirdCargoID; }
  public set ThirdCargoID(newValue: number) { this.EntityPM.ThirdCargoID = newValue; }

  public get ExportDealIdentification() { return this.EntityPM.ExportDealIdentification; }
  public set ExportDealIdentification(newValue: number) { this.EntityPM.ExportDealIdentification = newValue; }
}
