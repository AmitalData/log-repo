
import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { API_MainService } from '../../../core/API_MainService';
import { CB_CustomsItemComputedDataList, RulesDetailsList } from '../main-display/main-display.component';
import { NgFor, NgIf } from '@angular/common';
import { faChevronLeft, faSquareCaretRight } from '@fortawesome/free-solid-svg-icons';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';


@Component({
  selector: 'app-rules',
  standalone: true,
  imports: [FontAwesomeModule, NgIf, NgFor],
  templateUrl: './rules.component.html',
  styleUrl: './rules.component.css'
})
export class RulesComponent implements OnInit, OnChanges {
  isOpenData: boolean;
  @Input() showRules: boolean;
  // @Input() currentItem: CB_CustomsItemComputedDataList;
  @Input() currentItem: BehaviorSubject<CB_CustomsItemComputedDataList>;
  alephBetHelper = new alephBetHelper();
  allRules: CB_RulesDetailsList[] = [];

  constructor(private API_MainService: API_MainService) { }

  ngOnInit(): void {
    this.currentItem.subscribe((data: CB_CustomsItemComputedDataList) => {
      if (data?.CustomsItemID != null)
        this.initData(data?.CustomsItemID);
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['showRules']) {
      this.showRules = changes['showRules'].currentValue;
    }
  }

  initData(customsItemID: number) {
    this.API_MainService.GetCustomsBookRulesData(customsItemID).subscribe((data: any) => {

      if (!data.body) return; // TODO: add error message
      this.allRules = this.buildRulesHierarchy(data.body);

      console.log(this.allRules);
    });
  }

  toggleRule(rule: any, event: Event) {
    event.preventDefault();
    event.stopPropagation();
    rule.expanded = !rule.expanded;
    setTimeout(() => {
      if (rule.expanded) {
        rule.showDropdown = true;
        setTimeout(() => {
          rule.showDropdown = false;
        }, 400);
      } else {
        rule.hideDropdown = true;
        setTimeout(() => {
          rule.hideDropdown = false;
        }, 400);
      }
    }, 0);
  }


  buildRulesHierarchy(rulesList: CB_RulesDetailsList[]): CB_RulesDetailsList[] {

    // for each rule, get its children recursively
    const getChildren = (parentRule: CB_RulesDetailsList) => {
      // Filter for children of the current parent rule
      const children = rulesList.filter(rule => rule.Parent_RuleDetailsHistoryID === parentRule.ID);
      // For each child, get its own children recursively
      children.forEach(child => {
        child.childrens = getChildren(child);
      });
      return children;
    };

    // Find root rules
    const rootRules = rulesList.filter(rule => rule.Parent_RuleDetailsHistoryID == 0 || rule.Parent_RuleDetailsHistoryID == null);

    // Build the hierarchy for root rules
    const rulesListData = rootRules.map(rootRule => {
      const children = getChildren(rootRule);
      return {
        ...rootRule,
        childrens: children
      };
    });

    // group by title and put inside the array of in grouped   the paents by same rule id:
    // key is the title and rules is the array of the parents by same rule id from rootRules
    const grouped = rulesListData.reduce((acc, rule) => {
      const key = rule.Title;
      if (!acc[key]) {
        acc[key] = [];
      }
      acc[key].push(rule);
      return acc;
    }, {});
    console.log(grouped);

    return rulesListData;
  }


  orderText(text: string): string {
    if (!text) return text;
    return text.trimStart();
  }

  // dataList: RulesDetailsList= [];
}

export interface CB_RulesDetailsList {
  ID: number;
  RuleID: number;
  Title: string;
  Rules: string;
  UpdateDate: Date;
  ChangeRequestTypePriority: number;
  OrderinalPostion: number;
  EntityStatusID: string;
  Parent_RuleDetailsHistoryID: number;
  childrens: CB_RulesDetailsList[];
  expanded?: boolean;
  showDropdown?: boolean;
  hideDropdown?: boolean;
}

class alephBetHelper {
  alephBet: string[] = ['א', 'ב', 'ג', 'ד', 'ה', 'ו', 'ז', 'ח', 'ט', 'י', 'כ', 'ל', 'מ', 'נ', 'ס', 'ע', 'פ', 'צ', 'ק', 'ר', 'ש', 'ת'];
  getLetterFromNumber(number) {
    // Adjust for zero-based indexing
    const index = number - 1;
    if (index < 0 || index >= this.alephBet.length) {
      throw new Error("Number out of range. Please enter a number between 1 and 22.");
    }
    return `${this.alephBet[index]}-`;
  }
}
