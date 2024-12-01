import { Component, Input, OnInit } from '@angular/core';
import { GenericTableComponent, TableData } from '../generic-table/generic-table.component';
import { BehaviorSubject } from 'rxjs';
import { API_MainService } from '../../../core/API_MainService';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { CommonModule, DatePipe, NgIf } from '@angular/common';

@Component({
  selector: 'app-clasisification-guidance',
  standalone: true,
  imports: [GenericTableComponent,CommonModule,NgIf],
  templateUrl: './clasisification-guidance.component.html',
  styleUrl: './clasisification-guidance.component.css',
  providers: [DatePipe]
})
export class ClasisificationGuidanceComponent implements OnInit {
  @Input() expandedArea: boolean;
  @Input() ClassificationGuidanceId: BehaviorSubject<number> = new BehaviorSubject<number>(0);

  data: any = null; // TODO: add TYPE
  tableData: TableData;

  ngOnInit(): void {
    this.buildTable();
    this.listenToChanges();
  }
  constructor(private api_MainService: API_MainService, private sanitizer: DomSanitizer,private datePipe: DatePipe) { }
  sanitizeHTML(content: string): SafeHtml {
    return this.sanitizer.bypassSecurityTrustHtml(content) ? this.sanitizer.bypassSecurityTrustHtml(content) : '';
  }
  

  listenToChanges() {
    this.ClassificationGuidanceId.subscribe((data: any) => {
      this.data=null;
      if (data) {
        this.api_MainService.GetClassifGuidanceDetails(this.ClassificationGuidanceId.getValue().toString(), 6).subscribe((data: any) => {
          if (!data.body) return; // TODO: add error message
          this.data = data.body;
          console.log(data.body);
          
          this.tableData.data = data.body;
        });
      }
    });
  }
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
      data: []
    };
  }

}
