
import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { API_MainService } from '../../../core/API_MainService';
import { CB_CustomsItemComputedDataList, RulesList } from '../main-display/main-display.component';
import { NgFor, NgIf } from '@angular/common';


@Component({
  selector: 'app-rules',
  standalone: true,
  imports: [FontAwesomeModule, NgIf, NgFor],
  templateUrl: './rules.component.html',
  styleUrl: './rules.component.css'
})
export class RulesComponent implements OnInit, OnChanges {
  @Input() showRules: boolean;
  @Input() currentItem: CB_CustomsItemComputedDataList;
  allRules: RulesList[] = [];

  constructor(private API_MainService: API_MainService) { }


  ngOnInit(): void {

  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['showRules']) {
      this.showRules = changes['showRules'].currentValue;
    }
  }
}

