import { Component, Input, OnInit } from '@angular/core';
import { GenericTableComponent, TableData } from '../generic-table/generic-table.component';

@Component({
  selector: 'app-clasisification-guidance',
  standalone: true,
  imports: [GenericTableComponent],
  templateUrl: './clasisification-guidance.component.html',
  styleUrl: './clasisification-guidance.component.css'
})
export class ClasisificationGuidanceComponent implements OnInit {
  @Input() ClassificationGuidanceData: any;
  tableData: TableData;

  ngOnInit(): void {
    this.buildTable();
  }
  constructor() { }

  buildTable() {
    this.tableData = {
      columns: [
        { key: 'classificationGuidanceNumber', displayName: 'מספר הנחיה', dataType: 'string', visible: true },
        { key: 'title', displayName: 'שם הנחיה', dataType: 'string', visible: true },
        { key: 'classificationGuidanceTypeName', displayName: 'סוג הנחיה', dataType: 'string', visible: true },
        { key: 'fullClassification', displayName: 'חלק/פרק/פרט מכס', dataType: 'string', visible: true },
        { key: 'openDate', displayName: 'תאריך פתיחה', dataType: 'date', visible: true },
        { key: 'endDate', displayName: 'תאריך סיום תוקף', dataType: 'date', visible: true },
        { key: 'publicationDate', displayName: 'תאריך פרסום', dataType: 'date', visible: true },
        { key: 'relatedGuidance', displayName: 'הנחיה זו מתייחסת', dataType: 'string', visible: true },
        { key: 'description', displayName: 'תאור הנחיה', dataType: 'string', visible: true },
      ],
      // moke data
      // TODO: remove this data and use the real data from the API
      data: [
        {
          classificationGuidanceNumber: '101',
          title: 'הנחיה לדוגמה 1',
          classificationGuidanceTypeName: 'סוג 1',
          fullClassification: 'חלק 01 / פרק 02 / פרט 03',
          openDate: '2024-01-01',
          endDate: '2024-12-31',
          publicationDate: '2024-01-10',
          relatedGuidance: 'נחיה קודמת',
          description: 'תיאור ההנחיה לדוגמה הראשונה'
        },
        {
          classificationGuidanceNumber: '102',
          title: 'הנחיה לדוגמה 2',
          classificationGuidanceTypeName: 'סוג 2',
          fullClassification: 'חלק 04 / פרק 05 / פרט 06',
          openDate: '2023-07-01',
          endDate: '2025-07-01',
          publicationDate: '2023-07-15',
          relatedGuidance: 'נחיה אחרת',
          description: 'תיאור ההנחיה לדוגמה השנייה'
        },
        {
          classificationGuidanceNumber: '103',
          title: 'הנחיה לדוגמה 3',
          classificationGuidanceTypeName: 'סוג 3',
          fullClassification: 'חלק 07 / פרק 08 / פרט 09',
          openDate: '2022-03-15',
          endDate: '2024-03-15',
          publicationDate: '2022-03-20',
          relatedGuidance: 'נחיה כללית',
          description: 'תיאור ההנחיה לדוגמה השלישית'
        }
      ]
    };
  }

}
