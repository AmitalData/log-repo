import { Component, Input, OnInit } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faChevronLeft, faChevronDown } from '@fortawesome/free-solid-svg-icons';
import { GenericTableComponent, TableData } from '../generic-table/generic-table.component';
import { CB_CustomsItemComputedDataList, CB_RequirementComputedDataList, CB_TariffList, CustomItemClassifGuidanceResult, ItemData, MainEntity } from '../main-display/main-display.component';
import { API_MainService, Filters } from '../../../core/API_MainService';
import { CommonModule } from '@angular/common';
import { NgFor, NgForOf } from '@angular/common';
import { BehaviorSubject } from 'rxjs';
import { SessionInfo } from '../../../core/Infrastructure/Utilities/SessionInfo';
import { ClasisificationGuidanceComponent } from "../clasisification-guidance/clasisification-guidance.component";

@Component({
  selector: 'app-accordion',
  standalone: true,
  imports: [FontAwesomeModule, GenericTableComponent, ClasisificationGuidanceComponent, CommonModule, NgFor, NgForOf],
  templateUrl: './accordion.component.html',
  styleUrl: './accordion.component.css',
})
export class AccordionComponent implements OnInit {
  // add input type customs:
  // @Input() itemData: BehaviorSubject<ItemData> = new BehaviorSubject<ItemData>(null);
  @Input() currentItem: BehaviorSubject<CB_CustomsItemComputedDataList> = new BehaviorSubject<CB_CustomsItemComputedDataList>(null);
  itemData: CB_CustomsItemComputedDataList;

  customsItemId: number;
  tableData1: TableData;
  tableData2: TableData;
  tableData3: TableData;
  tableData4: TableData;
  MainEntity: MainEntity = new MainEntity([], [], [], []);

  expandedArea1: boolean = false;
  expandedArea2: boolean = false;
  expandedArea3: boolean = true;
  faChevronLeft = faChevronLeft;
  faChevronDown = faChevronDown;
  noExistMessageClasisificationGuidance = "לא התקבלו הנחיות סיווג";
  isLoadingClasisificationGuidance = false;

  constructor(private API_MainService: API_MainService) {
    this.MainEntity = new MainEntity([], [], [], []);
  }

  ngOnInit() {
    this.InitData();

    this.listenToChanges();
  }

  listenToChanges() {
    this.currentItem.subscribe((data: CB_CustomsItemComputedDataList) => {
      // init expandedAreas:
      this.expandedArea1 = false;
      this.expandedArea2 = false;
      this.expandedArea3 = true;

      this.customsItemId = data?.CustomsItemID;
      if (this.customsItemId) {
        this.resetData();
        this.buildAgreementsList(data?.agreementsList);
        this.buildRegularityRequirementList();
        this.buildClasisificationGuidance();
      }
    });
  }

  InitData() {
    this.tableData1 = {
      columns: [
        { key: 'Logo', displayName: '', dataType: 'img', visible: false },
        { key: 'TradeAgreementName', displayName: 'שם ההסכם', dataType: 'string', visible: true },
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
        { key: '', displayName: 'איזור אוטונמיה', dataType: 'string', visible: false },
        { key: 'IsVoluntaryOrImporterOfTrust', displayName: 'האם ולנטרי יבואן מפר אמון', dataType: 'boolean', visible: true }

      ],
      data: []
    };
    this.tableData4 = {
      columns: [
        { key: 'classificationGuidanceNumber', displayName: 'מספר הנחיה', dataType: 'button', visible: true },
        { key: 'title', displayName: 'כותרת', dataType: 'string', visible: true },
        { key: 'classificationGuidanceTypeName', displayName: 'סוג הנחיה', dataType: 'string', visible: true },
        { key: 'fullClassification', displayName: 'חלק/פרק/פרט מכס', dataType: 'string', visible: true },
        { key: 'publicationDate', displayName: 'תאריך פרסום', dataType: 'date', visible: true },
        { key: 'customsItemId', displayName: 'מספר פריט מכס', dataType: 'number', visible: false },
      ],
      data: []
    };
  }

  resetData() {
    this.tableData1.data = [];
    this.tableData2.data = [];
    this.tableData3.data = [];
    this.tableData4.data = [];
  }

  handleButtonClick(data: { event: Event, row: any, key: string }): void {
    const { event, row, key } = data;
    // console.log('Button clicked:', { event, row, key });
    // console.log('Row data:', row[key]);
    console.log(row);
    
    if(row.classificationGuidanceNumber != this.ClassificationGuidanceId.getValue()){
      this.isShowTableClassificationGuidance = true;
    } 
    else{
      this.isShowTableClassificationGuidance = true;
    }
    this.ClassificationGuidanceId.next(row.classificationGuidanceNumber);
  }
  isShowTableClassificationGuidance: boolean = false;
  // ClassificationGuidanceData: any;
  ClassificationGuidanceId: BehaviorSubject<number> = new BehaviorSubject<number>(0);

  // שיעורי מס
  buildAgreementsList(agreementsList: CB_TariffList[]) {
    if (agreementsList?.length == 0) return;
    this.MainEntity.CB_TariffList = agreementsList;
    this.tableData1.data = this.MainEntity.CB_TariffList.filter(x => x.TradeAgreementName != 'מס קניה');
    this.tableData2.data = this.MainEntity.CB_TariffList.filter(x => x.TradeAgreementName == 'מס קניה');
  }

  // דרישות חוקיות
  buildRegularityRequirementList() {
    this.API_MainService.GetCustomsBookRegularityRequirementData(this.customsItemId).subscribe(
      (data: any) => {
        const result: CB_RequirementComputedDataList[] = data.body;
        if (!result) return;
        this.MainEntity.CB_RequirementComputedDataList = result;
        this.tableData3.data = this.MainEntity.CB_RequirementComputedDataList;
      },
      (error) => {
        console.log(error.message);
      }
    );
  }

  // הנחיות סיווג
  GetDataCustomItemClassifGuidance(customsItemId: number, tenant: number) {
    this.API_MainService.GetCustomItemClassifGuidance(customsItemId, tenant).subscribe(
      (data: any) => {
        const result: CustomItemClassifGuidanceResult[] = data?.body?.CustomItemClassifGuidanceList;
        if (!result) return;;
        this.MainEntity.CustomItemClassifGuidanceResult = result;
        this.tableData4.data = this.MainEntity.CustomItemClassifGuidanceResult;
        this.isLoadingClasisificationGuidance = false;
        // this.expandedArea3 = false;
      },
      (error) => {
        console.log(error.message);
      }
    );
  }

  buildClasisificationGuidance() {
    this.isLoadingClasisificationGuidance = true;
    if (SessionInfo.LoggedUserTenant != 0)
      this.GetDataCustomItemClassifGuidance(this.customsItemId, SessionInfo.LoggedUserTenant);
    else {
      this.API_MainService.GetTenantFromCustomsSettings().subscribe(
        (data: any) => {
          const tenant: number = data?.body;
          this.GetDataCustomItemClassifGuidance(this.customsItemId, tenant);
        },
        (error) => {
          console.log(error.message);
        }
      );
    }
  }
}
