import { Component, Input, OnInit } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faChevronLeft, faChevronDown } from '@fortawesome/free-solid-svg-icons';
import { FileTypes, GenericTableComponent, TableData } from '../generic-table/generic-table.component';
import { AttachmentResponseData, CB_CustomsItemComputedDataList, CB_RequirementComputedDataList, CB_TariffList, CustomItemClassifGuidanceResult, MainEntity, Mekach } from '../main-display/main-display.component';
import { API_MainService } from '../../../core/API_MainService';
import { CommonModule, NgStyle } from '@angular/common';
import { NgFor, NgForOf } from '@angular/common';
import { BehaviorSubject } from 'rxjs';
import { SessionInfo } from '../../../core/Infrastructure/Utilities/SessionInfo';
import { ClasisificationGuidanceComponent } from "../clasisification-guidance/clasisification-guidance.component";
import { Pipes } from '../../../core/Infrastructure/ModuleDeclarations';

@Component({
  selector: 'app-accordion',
  standalone: true,
  imports: [FontAwesomeModule, GenericTableComponent, ClasisificationGuidanceComponent, CommonModule, NgFor, NgForOf, NgStyle, Pipes],
  templateUrl: './accordion.component.html',
  styleUrl: './accordion.component.css',
})
export class AccordionComponent implements OnInit {
  @Input() currentItem: BehaviorSubject<CB_CustomsItemComputedDataList> = new BehaviorSubject<CB_CustomsItemComputedDataList>(null);
  itemData: CB_CustomsItemComputedDataList;
  isShowTableClassificationGuidance: boolean = false;
  ClassificationGuidanceId: BehaviorSubject<string> = new BehaviorSubject<string>("");
  customsItemId: number;
  tableData1: TableData;
  tableData2: TableData;
  tableData3: TableData;
  tableData3A: TableData;
  tableData3B: TableData;
  tableData4: TableData;
  tableData5: TableData;
  MainEntity: MainEntity = new MainEntity([], [], [], [], []);
  reloadMsg: string = "טוען נתונים...";
  freeImportHeader: string = "יבוא חופשי";
  pesonalImportHeader: string = "יבוא אישי";
  classificationHeader: string = "הנחיות סיווג";
  mekachHeader: string = "תדפיסי חקיקה (מקח''ים)";
  expandedArea1: boolean = false;
  expandedArea2: boolean = false;
  expandedArea3: boolean = true;
  expandedArea5: boolean = false;
  faChevronLeft = faChevronLeft;
  faChevronDown = faChevronDown;
  noExistMessageClasisificationGuidance = "לא התקבלו הנחיות סיווג";
  noExistMessageMakach = "לא התקבלו פרטי תדפיסי חקיקה";
  isLoadingClasisificationGuidance = false;
  isLoadingMekach = false;
  resetClassificationGuidanceData: boolean = true;

  constructor(private API_MainService: API_MainService) {
    this.MainEntity = new MainEntity([], [], [], [], []);
  }

  ngOnInit() {
    this.InitData();
    this.listenToChanges();
  }

  listenToChanges() {
    this.currentItem.subscribe((data: CB_CustomsItemComputedDataList) => {
      this.itemData = data;

      // init expandedAreas:
      this.expandedArea1 = false;
      this.expandedArea2 = false;
      this.expandedArea3 = true;
      this.expandedArea5 = true;

      this.customsItemId = data?.CustomsItemID;
      if (this.customsItemId) {
        this.resetData();
        this.buildAgreementsList(data?.CustomsItemID, data?.PH_MeasurementUnitID);
        this.buildRegularityRequirementList();
        this.getClassificationAndMekachData();
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
        { key: 'FromEpisodeDetail', displayName: 'נובע מפרק/ פרט', dataType: 'string', visible: true },
        { key: 'RequirementGoodsDescription', displayName: 'תיאור טובין בדרישה/תיאור הזהרות', dataType: 'string', visible: true },
        { key: 'Authority', displayName: 'גורם מאשר (הפניה לאיש קשר)', dataType: 'string', visible: true },
        { key: 'ConfirmationType', displayName: 'סוג אישור', dataType: 'string', visible: true },
        { key: 'TextualCondition', displayName: 'תיאור תנאים', dataType: 'link', visible: true, link: { url: `https://www.gov.il/he/Departments/DynamicCollectors/mandatory-standards-search?skip=0&standard_number_and_name=`, key: `TrNumber`, condition: { key: "TextualCondition", value: this.TrTextualCondition } } },
        { key: 'InterConditionsRelationship', displayName: 'יחס תנאים', dataType: 'string', visible: true },
        { key: 'IsPersonalImportIncluded', displayName: 'חל ביבוא אישי', dataType: 'boolean', visible: true },
        { key: 'IsCarnetIncluded', displayName: 'חל בקרנה', dataType: 'boolean', visible: true },
        { key: '', displayName: 'איזור אוטונמיה', dataType: 'string', visible: false },
        { key: 'IsVoluntaryOrImporterOfTrust', displayName: 'האם ולנטרי יבואן מפר אמון', dataType: 'boolean', visible: true }

      ],
      data: []
    };
    // tableData3 is contain full data of tableData3A and tableData3B:
    this.tableData3A = {
      columns: this.tableData3.columns,
      data: []
    };
    this.tableData3B = {
      columns: this.tableData3.columns,
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

    this.tableData5 = {
      columns: [
        { key: 'mekachNumber', displayName: 'מס מק"ת/מק"ח', dataType: 'string', visible: true, width: '120px' },
        { key: 'attachedMekahFile', displayName: 'קובץ מצורף', dataType: FileTypes.pdf, visible: true, width: '120px' },
        { key: 'validityDate', displayName: 'בתוקף מיום', dataType: 'date', visible: true, width: '120px' },
        { key: 'changeDescription', displayName: 'דברי הסבר', dataType: 'string', visible: true, width: '120px' }
      ],
      data: []
    };
  }

  resetData() {
    this.tableData1.data = [];
    this.tableData2.data = [];
    this.tableData3.data = [];
    this.tableData3A.data = [];
    this.tableData3B.data = [];
    this.tableData4.data = [];
    this.tableData5.data = [];
    this.ClassificationGuidanceId.next("");
  }

  handleButtonClick(data: { event: Event, row: any, key: string }): void {
    const { event, row, key } = data;
    if (row.classificationGuidanceNumber != this.ClassificationGuidanceId.getValue())
      this.isShowTableClassificationGuidance = true;
    else
      this.isShowTableClassificationGuidance = !this.isShowTableClassificationGuidance;
    this.ClassificationGuidanceId.next(row.classificationGuidanceNumber);
  }

  handleButtonPdfClick(data: { event: Event, row: any, key: string }): void {
    const { event, row, key } = data;
    this.API_MainService.GetMekachDocument(row?.attachedMekahFile, SessionInfo.LoggedUserTenant).subscribe(
      (docData: any) => {
        if (docData?.body) {
          let data: AttachmentResponseData = docData?.body;
          if (data?.AttachedMekahFileData) this.openBase64File(data?.AttachedMekahFileData?.content, FileTypes.pdf);
        }
      },
      (error) => {
        console.log(error.message);
        this.isLoadingMekach = false;
      }
    );
  }

  openBase64File(base64String: string, fileType: string): void {
    if (!base64String) return;
    const byteCharacters = atob(base64String);
    const byteNumbers = new Array(byteCharacters.length);
    for (let i = 0; i < byteCharacters.length; i++) {
      byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    const byteArray = new Uint8Array(byteNumbers);
    const fileBlob = new Blob([byteArray], { type: fileType });
    const fileURL = URL.createObjectURL(fileBlob);
    window.open(fileURL, '_blank');
  }

  // שיעורי מס
  buildAgreementsList(customsItemID: number, measurementUnitID: number) {
    if (!customsItemID || !measurementUnitID) return;

    this.API_MainService.GetCustomsBookAgreementLevelData(customsItemID, measurementUnitID).subscribe(
      (data: any) => {
        const agreementsList: CB_TariffList[] = data.body;
        if (agreementsList?.length == 0) return;
        this.MainEntity.CB_TariffList = agreementsList;
        this.tableData1.data = this.MainEntity.CB_TariffList.filter(x => x.TradeAgreementName != 'מס קניה');
        this.tableData2.data = this.MainEntity.CB_TariffList.filter(x => x.TradeAgreementName == 'מס קניה');
      },
      (error) => {
        console.log(error.message);
      }
    );
  }
  TrTextualCondition: string = 'ת"ר';

  // דרישות חוקיות
  buildRegularityRequirementList() {
    this.API_MainService.GetCustomsBookRegularityRequirementData(this.customsItemId).subscribe(
      (data: any) => {
        const result: CB_RequirementComputedDataList[] = data.body;
        if (!result) return;
        this.MainEntity.CB_RequirementComputedDataList = result;
        this.tableData3.data = this.MainEntity.CB_RequirementComputedDataList;
        this.tableData3A.data = this.tableData3.data?.filter(item => !item.RequirementValidOrigin.includes("אישי"));
        this.tableData3B.data = this.tableData3.data?.filter(item => item.RequirementValidOrigin.includes("אישי"));
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
        this.isLoadingClasisificationGuidance = false;
      }
    );
  }

  buildClasisificationGuidance(tenant: number = 0) {
    this.isLoadingClasisificationGuidance = true;
    this.GetDataCustomItemClassifGuidance(this.customsItemId, tenant);
  }

  buildDataMekach(tenant: number = 0) {
    this.isLoadingMekach = true;
    this.API_MainService.GetMekachDetails(this.customsItemId, tenant).subscribe(
      (data: any) => {
        const result: Mekach[] = data?.body?.CustomItemMekachDataList ?? [];
        this.isLoadingMekach = false;
        if (result?.length === 0) {
          this.noExistMessageMakach = data?.body?.UserMessage ?? this.noExistMessageMakach;
          return;
        };
        this.MainEntity.Mekach = result;
        this.tableData5.data = this.MainEntity.Mekach;
      },
      (error) => {
        console.log(error.message);
        this.isLoadingMekach = false;
      }
    );
  }

  getClassificationAndMekachData() {
    let tenant: number = SessionInfo.LoggedUserTenant;
    if (tenant != 0) {
      this.buildClasisificationGuidance(tenant);
      this.buildDataMekach(tenant);
    }
    else {
      this.API_MainService.GetTenantFromCustomsSettings().subscribe(
        (data: any) => {
          tenant = data?.body;
          this.buildClasisificationGuidance(tenant);
          this.buildDataMekach(tenant);
        },
        (error) => {
          console.log(error.message);
        }
      );
    }
  }
}
