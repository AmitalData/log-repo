import { CommonModule, NgFor, NgForOf } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

@Component({
  selector: 'app-generic-table',
  standalone: true,
  imports: [FontAwesomeModule, GenericTableComponent, CommonModule, NgFor, NgForOf],

  templateUrl: './generic-table.component.html',
  styleUrl: './generic-table.component.css'
})
export class GenericTableComponent implements OnInit {
  @Input() tableData: TableData; // Use the TableData interface
  @Output() buttonClicked: EventEmitter<{ event: Event, row: any, key: string }> = new EventEmitter();

  ngOnInit() {

  }

  checkLink(link: string, value: string) {
    return link ? link + value : value;
  }

  onButtonClick(event: Event, row: any, key: string): void {
    this.buttonClicked.emit({ event, row, key });
  }
}


export interface TableData {
  columns: TableColumn[];
  data: any[];
}

export interface TableColumn {
  key: string;
  displayName: string;
  dataType: 'string' | 'number' | 'date' | "img" | "boolean" | "link" | 'button';
  visible: boolean;
  width?: string;
  isLink?: string;
}
