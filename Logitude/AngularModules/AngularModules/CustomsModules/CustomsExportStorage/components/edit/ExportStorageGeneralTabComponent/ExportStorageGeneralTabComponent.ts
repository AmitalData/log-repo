import { Component, OnInit } from '@angular/core';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';

@Component({
  selector: 'app-edit-export-storage',
  templateUrl: './ExportStorageGeneralTabComponent.html',
  styleUrls: ['./ExportStorageGeneralTabComponent.scss']
})
export class ExportStorageGeneralTabComponent implements OnInit {

  constructor(public entityArgs: EntityArgs) { }

  ngOnInit(): void {
    console.log(1)
    console.log(this.entityArgs.EntityPM)
    // this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe((response:any) => {

  }

}
