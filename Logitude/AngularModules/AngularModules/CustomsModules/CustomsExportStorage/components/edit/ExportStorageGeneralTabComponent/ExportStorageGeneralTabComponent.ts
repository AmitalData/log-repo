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
}
