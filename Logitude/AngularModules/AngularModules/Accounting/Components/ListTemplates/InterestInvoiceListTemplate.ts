import { AccountingEntityHelper } from './../../Utilities/AccountingEntityHelper';
import { SessionLocator } from './../../../Infrastructure/Utilities/SessionLocator';
import { Component, ChangeDetectorRef, AfterViewInit, OnInit } from '@angular/core';
import { AppTool } from '../../../Infrastructure/Tools';
import { InterestReportEventManager } from '../../Utilities/InterestReportEventManager';

import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { InterestReportPM } from '../../EntityPMs/InterestReportPM';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
@Component({

  templateUrl: "./InterestInvoiceListTemplate.html"
})


export class InterestInvoiceListTemplate {

  public rowData: any;
  public fieldName: any;
  public AdditionalData: any;


  public isRTL: boolean = false;
  public showLocal: boolean = !SessionLocator.LoggedUserPM.DontShowLocal;
  private CurrentSession = SessionLocator.SelectedSession;
  constructor(private CD: ChangeDetectorRef) {
    if (ObjectsLocator.GlobalSetting)
      this.isRTL = ObjectsLocator.GlobalSetting.LayoutDirection == "rtl";
    this.Listen();
  }

  Listen() {
    InterestReportEventManager.SelectAllEvent.subscribe(($event) => {
      if (!AppTool.IsNullOrEmpty($event)) {
        if ($event.SendSessionIndex != this.CurrentSession.SessionIndex)
          return;
      }
    });
  }

  setVariables(rowData: any, fieldName: string, AdditionalData: any) {
    this.rowData = rowData;
    this.fieldName = fieldName;
    this.AdditionalData = AdditionalData;
    var isDestroyed: boolean = this.CD["destroyed"];
    if (!isDestroyed) {
      this.CD.detectChanges();
    }
  }

  CheckBoxClicked(checked: boolean) {

    this.CurrentSession.InterestReportCheckBoxCheckedEvent.emit({
      line: this.rowData,
      isChecked: checked,
      RowIndex: this.AdditionalData.rowIndex

    });
  }

  OpenInterestReport(id) {
    if (!AppTool.IsNullOrEmpty(id)) {
      SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
        .then(cmpRef => {
          cmpRef.instance.ComponentRef = cmpRef;
          cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'InterestReport' });
          cmpRef.instance.BackCompleted.subscribe(bk => {
          });
        });
    }
  }

  OpenARInvoice(id) {
    if (!AppTool.IsNullOrEmpty(id)) {
      SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
        .then(cmpRef => {
          cmpRef.instance.ComponentRef = cmpRef;
          cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'ARInvoice' });
          cmpRef.instance.BackCompleted.subscribe(bk => {
          });
        });
    }
  }
}
