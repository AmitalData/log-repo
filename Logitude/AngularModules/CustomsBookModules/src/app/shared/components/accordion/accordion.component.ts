import { Component, Input, OnInit } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faChevronLeft, faChevronDown } from '@fortawesome/free-solid-svg-icons';
import { GenericTableComponent, TableData } from '../generic-table/generic-table.component';
import { CB_CustomsItemComputedDataList, CB_RequirementComputedDataList, CB_TariffList, ItemData, MainEntity } from '../main-display/main-display.component';
import { API_MainService, Filters } from '../../../core/API_MainService';
import { CommonModule } from '@angular/common';
import { NgFor, NgForOf } from '@angular/common';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-accordion',
  standalone: true,
  imports: [FontAwesomeModule, GenericTableComponent, CommonModule, NgFor, NgForOf],
  templateUrl: './accordion.component.html',
  styleUrl: './accordion.component.css',
})
export class AccordionComponent implements OnInit {
  // add input type customs:
  // @Input() itemData: BehaviorSubject<ItemData> = new BehaviorSubject<ItemData>(null);
  @Input() currentItem: BehaviorSubject<CB_CustomsItemComputedDataList> = new BehaviorSubject<CB_CustomsItemComputedDataList>(null);

  customsItemId: number;
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
    this.listenToChanges();
  }

  listenToChanges() {
    this.currentItem.subscribe((data: CB_CustomsItemComputedDataList) => {
      this.customsItemId = data?.CustomsItemID;
      if(this.customsItemId)
        this.InitData();
    });
  }

  InitData() {

    this.tableData1 = {
      columns: [
        { key: 'Logo', displayName: '', dataType: 'img', visible: true },
        { key: 'TradeAgreementName', displayName: 'שם ההסכם', dataType: 'string', visible: true, notEqual: `'מס קנייה'` },
        { key: 'CustomsRate', displayName: 'שיעור מכס', dataType: 'number', visible: true },
        { key: 'CustomsRateWithinQuota', displayName: 'שיעור מכס במסגרת מכסה', dataType: 'number', visible: true },
        { key: 'QuotaID', displayName: 'מס\' מכסה', dataType: 'number', visible: true },
        { key: 'MeasurementUnitName', displayName: 'יח\' מידה סטטיסטית', dataType: 'string', visible: true },
        { key: 'StartDate', displayName: 'בתוקף מ', dataType: 'date', visible: true },
        { key: 'EndDate', displayName: 'בתוקף עד', dataType: 'date', visible: true },
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
        { key: 'RequirementValidOrigin', displayName: 'המקור החוקי לדרישה', dataType: 'string', visible: true, width: '120px' },
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

    
    if(!this.currentItem.getValue().PH_MeasurementUnitID) return;
    // שיעורי מס
    this.API_MainService.GetCustomsBookAgreementLevelData(this.customsItemId, this.currentItem.getValue().PH_MeasurementUnitID).subscribe((data: any) => {
      const result: CB_TariffList[] = data.body;
      if (!result) return; // TODO: add error message

      this.MainEntity.CB_TariffList = result;
      this.tableData1.data = this.MainEntity.CB_TariffList.filter(x => x.TradeAgreementName != 'מס קניה');
      this.tableData2.data = this.MainEntity.CB_TariffList.filter(x => x.TradeAgreementName == 'מס קניה');
    });

    // דרישות חוקיות
    this.API_MainService.GetCustomsBookRegularityRequirementData(this.customsItemId).subscribe((data: any) => {
      const result: CB_RequirementComputedDataList[] = data.body;
      if (!result) return; // TODO: add error message

      this.MainEntity.CB_RequirementComputedDataList = result;
      this.tableData3.data = this.MainEntity.CB_RequirementComputedDataList
    });
  }

}
