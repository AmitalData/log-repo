declare var System: any;
declare var window: any;
declare var attachmentUploader, ResultAsArray: any;
import { Input, OnInit, ElementRef, Output, EventEmitter, NgModule, Component } from '@angular/core';
import { ReportPM } from 'Common/EntityPMs/ReportPM';
import { ReportService } from 'Common/Services/ExtendedLists/ReportService';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { List } from 'Infrastructure/DataContracts/Dashboard/List';
import { Variable } from '@angular/compiler/src/render3/r3_ast';
import { ISlvLeaf } from 'Infrastructure/Components/LogitudeComponents/tree';



@Component({

  moduleId: './Report/Components/',
  selector: 'ReportVariablesComponent',
  templateUrl: 'ReportVariablesComponent.html',

})

export class ReportVariablesComponent implements OnInit {
  public treeData1: Array<ISlvLeaf> = [];
  public reportService: ReportService;
  public EntityPM: ReportPM;
  constructor(public entityArgs: EntityArgs, public _elementRef: ElementRef) {
    this.EntityPM = entityArgs.EntityPM;
  }

  ngOnInit() {
    this.getData()
  }

  getData() {
    this.reportService = new ReportService()

    this.reportService.GetDataProviderProperties(this.EntityPM.Code).subscribe(a => {
      if (a.Result?.length > 0) {
        this.treeData1 = a.Result;
      }
      else {
        throw ('No implement!!!!!!')
      }
    })
  }
  

}

















