import { Component, Input, OnInit } from '@angular/core';
import { GenericTableComponent, TableData } from '../generic-table/generic-table.component';
import { BehaviorSubject } from 'rxjs';
import { API_MainService } from '../../../core/API_MainService';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { CommonModule, DatePipe, NgIf } from '@angular/common';
import { SessionInfo } from '../../../core/Infrastructure/Utilities/SessionInfo';

@Component({
  selector: 'app-clasisification-guidance',
  standalone: true,
  imports: [GenericTableComponent, CommonModule, NgIf],
  templateUrl: './clasisification-guidance.component.html',
  styleUrl: './clasisification-guidance.component.css',
  providers: [DatePipe]
})
export class ClasisificationGuidanceComponent implements OnInit {
  @Input() expandedArea: boolean;
  @Input() ClassificationGuidanceId: BehaviorSubject<string> = new BehaviorSubject<string>("");

  data: any = null; // TODO: add TYPE
  tableData: TableData;

  ngOnInit(): void {
    this.listenToChanges();
  }
  constructor(private api_MainService: API_MainService, private sanitizer: DomSanitizer, private datePipe: DatePipe) { }
  sanitizeHTML(content: string): SafeHtml {
    return this.sanitizer.bypassSecurityTrustHtml(content) ? this.sanitizer.bypassSecurityTrustHtml(content) : '';
  }

  listenToChanges() {
    let tenant: number = SessionInfo.LoggedUserTenant;
    if (tenant != 0) this.GetClassifGuidanceFullDetails(tenant);
    else {
      this.api_MainService.GetTenantFromCustomsSettings().subscribe(
        (data: any) => {
          tenant = data?.body;
          this.GetClassifGuidanceFullDetails(tenant);
        },
        (error) => {
          console.log(error.message);
        }
      );
    }
  }

  GetClassifGuidanceFullDetails(tenant: number) {
    this.ClassificationGuidanceId.subscribe((ClassificationGuidanceNo: any) => {     
      this.data = null;
      if (ClassificationGuidanceNo) {
        this.api_MainService.GetClassifGuidanceDetails(this.ClassificationGuidanceId.getValue().toString(), tenant).subscribe((data: any) => {
          if (!data.body) return; // TODO: add error message
          this.data = data.body;
          this.tableData.data = data.body;
        });
      }
    });
  }
}
