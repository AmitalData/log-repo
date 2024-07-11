import { CommonModule, NgFor, NgForOf } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
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
  ngOnInit() {

  }
}


export interface TableData {
  columns: TableColumn[];
  data: any[];
}

export interface TableColumn {
  key: string;
  displayName: string;
  dataType: 'string' | 'number' | 'date' | "img" | "boolean";
  visible: boolean;
  width?: string;
  notEqual?: string;
}
