import { Component, OnInit } from '@angular/core';
import { ExportStoragePM } from 'Customs/EntityPMs/ExportStoragePM';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { Xml2jsonService } from 'Infrastructure/Services/xml2json/xml2json.service';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { LogtuideTableDataService } from 'QuoteOPM/Components/NewEntity/components/autocomplate-table/logtuide-table-data.service';
import { ExportStorageMsgXML, Exception, UIMessage } from './FeedbackToStorageTabComponentTypes';

@Component({
  selector: 'app-feedback-to-storage-tab-component',
  templateUrl: './feedback-to-storage-tab-component.component.html',
  styleUrls: ['./feedback-to-storage-tab-component.component.scss']
})
export class FeedbackToStorageTabComponent extends BaseComponent implements OnInit {
  entityPM: ExportStoragePM = null as any;
  exceptionslist: ObservableCollection = new ObservableCollection([]);
  UIMessageTable: UIMessage[] = [];
  excptionTypes: { id: string, text: string }[] = [
    { id: '1', text: 'שגיאה' },
    { id: '2', text: 'התראה' },
    { id: '3', text: 'לידיעה' },
  ]


  constructor(
    private entityArgs: EntityArgs,
    private logtuideTableDataService: LogtuideTableDataService,
    private xml2jsonService: Xml2jsonService,
  ) {
    super();
  }


  async ngOnInit(): Promise<void> {
    await this.initUIMessageTable();
    this.initEntityArgs()
    this.initTable()
  }


  initEntityArgs() {
    this.entityPM = this.entityArgs.EntityPM;
  }


  async initUIMessageTable() {
    this.UIMessageTable = await this.logtuideTableDataService.getTable('Customs.UIMessage');    
  }


  initTable() {
    let xmlString: string = this.entityPM.StorErrorXML;
    if (!xmlString) return;

    const xml: ExportStorageMsgXML = this.parseXmlString(xmlString);
    if (!xml.ns0ESBResponse.Body.MN_MSG2791_ExportDeliveryAnswerMessage.Exception) return;

    const exceptions: Exception[] = this.GetExeptionFromXml(xml);
    this.updateExcptionLevel(exceptions);
    this.updateExcptionType(exceptions);
    this.insertData(exceptions);
  }


  parseXmlString(xmlString: string): ExportStorageMsgXML {
    xmlString = xmlString.replace(/:/g, '');
    const xml: ExportStorageMsgXML = this.xml2jsonService.xml2json(xmlString);

    return xml;
  }


  GetExeptionFromXml(xml: ExportStorageMsgXML): Exception[] {
    const data: Exception[] | Exception = xml.ns0ESBResponse.Body.MN_MSG2791_ExportDeliveryAnswerMessage.Exception;
    const exceptions: Exception[] = Array.isArray(data) ? data : [data];

    return exceptions;
  }


  updateExcptionLevel(exceptions: Exception[]) {
    exceptions.forEach(e => e.ExceptionLevel = this.excptionTypes.find(ex => ex.id == e.ExceptionLevel)?.text);
  }

  
  updateExcptionType(exceptions: Exception[]) {
    exceptions.forEach(e => e.ExeptionType = this.UIMessageTable.find(ex => ex.Code == e.ExeptionType)?.LocalName);
  }


  insertData(exceptions: Exception[]) {
    this.exceptionslist.InsertCollection(exceptions);
  }
}
