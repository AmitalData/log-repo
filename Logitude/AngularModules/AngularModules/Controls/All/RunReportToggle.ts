import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';

@Component({
  selector: 'app-run-report-toggle',
  templateUrl: './RunReportToggle.html'
})
export class RunReportToggleComponent implements OnInit {

  @Input() isRTL: boolean = false;
  @Input() IsSchedulerReport: boolean = false;
  @Output() runReport: EventEmitter<boolean> = new EventEmitter<boolean>();

  Preview: boolean = false;
  RunReportTitle: string = '';
  RunReportImmediatelyTitle: string = '';

  ngOnInit(): void {
    this.SetRunReportTitle();
  }

  SetRunReportTitle(): void {
    if (this.IsSchedulerReport) {
      this.Preview = true;
      this.RunReportTitle = TextCodeTranslator.Translate('AgingReport.O.PreviewReport') || "Preview";
    } else {
      this.Preview = false;
      this.RunReportTitle = TextCodeTranslator.Translate('AgingReport.O.RunReport') || "Run Report";
      this.RunReportImmediatelyTitle = TextCodeTranslator.Translate('AgingReport.O.RunReportImmediately') || "Run Immediately";
    }
  }

  RunReport(immediate: boolean): void {
    this.runReport.emit(immediate);
  }
}
