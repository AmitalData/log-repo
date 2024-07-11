import { Component, OnInit } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faChevronLeft, faChevronDown } from '@fortawesome/free-solid-svg-icons';
import { GenericTableComponent, TableData } from '../generic-table/generic-table.component';
import { CB_RequirementComputedDataList, CB_TariffList, MainEntity } from '../main-display/main-display.component';
import { API_MainService, Filters } from '../../../core/API_MainService';
import { CommonModule } from '@angular/common';
import { NgFor, NgForOf } from '@angular/common';

@Component({
  selector: 'app-accordion',
  standalone: true,
  imports: [FontAwesomeModule, GenericTableComponent, CommonModule, NgFor, NgForOf],
  templateUrl: './accordion.component.html',
  styleUrl: './accordion.component.css',
})
export class AccordionComponent implements OnInit {

  tableData1: TableData;
  tableData2: TableData;
  tableData3: TableData;
  MainEntity: MainEntity = new MainEntity([], [], []);

  expandedArea1: boolean = false;
  expandedArea2: boolean = false;
  faChevronLeft = faChevronLeft;
  faChevronDown = faChevronDown;

  constructor(private API_MainService: API_MainService) {
    this.MainEntity = new MainEntity([], [], []);
  }

  ngOnInit() {
    this.InitData();
  }

  InitData() {

    this.tableData1 = {
      columns: [
        { key: 'Logo', displayName: '', dataType: 'img', visible: true },
        { key: 'TradeAgreementName', displayName: 'שם ההסכם', dataType: 'string', visible: true, notEqual: `'מס קנייה'`},
        { key: 'CustomsRate', displayName: 'שיעור מכס', dataType: 'number', visible: true },
        { key: 'CustomsRateWithinQuota', displayName: 'שיעור מכס במסגרת מכסה', dataType: 'number', visible: true },
        { key: 'QuotaID', displayName: 'מס\' מכסה', dataType: 'number', visible: true },
        { key: 'MeasurementUnitName', displayName: 'יח\' מידה סטטיסטית', dataType: 'string', visible: true },
        { key: 'StartDate', displayName: 'בתוקף מ', dataType: 'date', visible: true },
        { key: 'EndDate', displayName: 'בתוקף עד', dataType: 'date', visible: true },
        // { key: 'ID', displayName: 'ID', dataType: 'number', visible: false },
        // { key: 'CreateDate', displayName: 'Create Date', dataType: 'date', visible: false },
        // { key: 'UpdateDate', displayName: 'Update Date', dataType: 'date', visible: false },
        // { key: 'TradeAgreementID', displayName: 'Trade Agreement ID', dataType: 'number', visible: false },
        // { key: 'CustomsItemID', displayName: 'Customs Item ID', dataType: 'number', visible: false },
        // { key: 'Title', displayName: 'Title', dataType: 'string', visible: false },
        // { key: 'CB_ID', displayName: 'CB ID', dataType: 'number', visible: false },
        // { key: 'Country', displayName: 'Country', dataType: 'string', visible: false },
        // { key: 'OptionalTaxAddition', displayName: 'Optional Tax Addition', dataType: 'number', visible: false },
      ],
      data: []
    };
    this.tableData2 = {
      columns: [
        { key: 'CustomsRate', displayName: 'שיעור מס', dataType: 'number', visible: true },
        { key: 'MeasurementUnitName', displayName: 'יחידת מידה', dataType: 'string', visible: true },
        { key: 'OptionalTaxAddition', displayName: 'תמ"א', dataType: 'string', visible: true },
        { key: 'StartDate', displayName: 'בתוקף מ', dataType: 'date', visible: true },
        { key: 'EndDate', displayName: 'בתוקף עד', dataType: 'date', visible: true },
      ],
      data: []
    };
    this.tableData3 = {
      columns: [
        // { key: 'CustomsItemID', displayName: 'מזהה פריט מכס', dataType: 'string', visible: false },
        // { key: 'ID', displayName: 'מזהה', dataType: 'number', visible: false },
        { key: 'RequirementValidOrigin', displayName: 'המקור החוקי לדרישה', dataType: 'string', visible: true, width: '120px'},
        { key: '', displayName: 'נובע מפרק/ פרט', dataType: 'string', visible: false },
        { key: 'RequirementGoodsDescription', displayName: 'תיאור טובין בדרישה/תיאור הזהרות', dataType: 'string', visible: true },
        { key: 'Authority', displayName: 'גורם מאשר (הפניה לאיש קשר)', dataType: 'string', visible: true },
        { key: 'ConfirmationType', displayName: 'סוג אישור', dataType: 'string', visible: true },
        { key: 'TextualCondition', displayName: 'תיאור תנאים', dataType: 'string', visible: true },
        { key: 'InterConditionsRelationship', displayName: 'יחס תנאים', dataType: 'string', visible: true },
        { key: 'IsPersonalImportIncluded', displayName: 'חל ביבוא אישי', dataType: 'boolean', visible: true },
        { key: 'IsCarnetIncluded', displayName: 'חל בקרנה', dataType: 'boolean', visible: true },
        { key: '', displayName: 'איזור אוטונמיה', dataType: 'string', visible: false }
      ],
      data: []
    };


    // TODO: change to sen real data customItemID and measurementUnitID are exist in CB_CustomsItemComputedDataList:
    this.API_MainService.GetCustomsBookAgreementLevelData(23066, 6).subscribe((data: CB_TariffList[]) => {
      this.MainEntity.CB_TariffList = data;
      
      console.log(this.MainEntity.CB_TariffList);
      this.tableData1.data = this.MainEntity.CB_TariffList.filter(x => x.TradeAgreementName != 'מס קניה');
      
      this.tableData2.data = this.MainEntity.CB_TariffList.filter(x => x.TradeAgreementName == 'מס קניה');
    });
    this.API_MainService.GetCustomsBookRegularityRequirementData(17514).subscribe((data: CB_RequirementComputedDataList[]) => {
      this.MainEntity.CB_RequirementComputedDataList = data;
      console.log(this.MainEntity.CB_RequirementComputedDataList);
      this.tableData3.data = this.MainEntity.CB_RequirementComputedDataList
    });
  }

}
