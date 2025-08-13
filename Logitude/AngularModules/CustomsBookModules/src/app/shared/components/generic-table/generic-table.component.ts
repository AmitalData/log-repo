import { CommonModule, NgFor, NgForOf } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

@Component({
  selector: 'app-generic-table',
  standalone: true,
  imports: [FontAwesomeModule, CommonModule, NgFor, NgForOf],
  templateUrl: './generic-table.component.html',
  styleUrl: './generic-table.component.css'
})

export class GenericTableComponent {
  @Input() tableData: TableData; // Use the TableData interface
  @Output() buttonClicked: EventEmitter<{ event: Event, row: any, key: string }> = new EventEmitter();
  public fileTypes = FileTypes;

  checkLink(link: string, value: string): string {
    return link != "" && value != "" ? link + value : "";
  }

  onButtonClick(event: Event, row: any, key: string): void {
    this.buttonClicked.emit({ event, row, key });
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
    const fileURL = URL.createObjectURL(fileBlob); // Open the file in a new tab:
    window.open(fileURL, '_blank');
  }

  shouldDisplayLink(condition: Condition | undefined, rowData: string): boolean {
    if (!condition || !condition.key || !condition.value) 
      return true;
    return rowData && rowData.includes(condition.value);
  }
}

export interface TableData {
  columns: TableColumn[];
  data: any[];
}

export interface TableColumn {
  key: string;
  displayName: string;
  dataType: 'string' | 'number' | 'img' | 'date' | "boolean" | "link" | 'button' | FileTypes;
  visible: boolean;
  width?: string;
  link?: Link;
}
interface Link {
  url: string;
  key?: string;
  condition?: Condition;
}
interface Condition {
  key: string;
  value: string;
}

export enum FileTypes {
  pdf = 'application/pdf',
  img = 'image/png'
}
