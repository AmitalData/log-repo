import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { API_MainService } from '../../../core/API_MainService';
import { CB_CustomsItemComputedDataList, RulesDetailsList } from '../main-display/main-display.component';
import { CommonModule, NgFor, NgIf } from '@angular/common';
import { faChevronLeft, faSquareCaretRight } from '@fortawesome/free-solid-svg-icons';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';

@Component({
  selector: 'app-rules',
  standalone: true,
  imports: [FontAwesomeModule, NgIf, NgFor, CommonModule],
  templateUrl: './rules.component.html',
  styleUrl: './rules.component.css'
})
export class RulesComponent implements OnInit, OnChanges {
  isOpenData: boolean;
  @Input() showRules: boolean;
  @Input() currentItem: BehaviorSubject<CB_CustomsItemComputedDataList>;
  alephBetHelper = new alephBetHelper();
  allRules: CB_RulesDetailsList[] = [];
  groupRulesList: GroupedRules[] = [];
  clickPin: boolean = true;
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

  // Method to fetch rules data from the API and build the rules hierarchy
  initData(customsItemID: number) {
    this.API_MainService.GetCustomsBookRulesData(customsItemID).subscribe((data: any) => {
      if (!data.body) return; // TODO: add error message
      this.allRules = this.buildRulesHierarchy(data.body);
    });
  }

  // Method to toggle the expanded state of a rule and manage dropdown visibility
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

  // Method to build a hierarchical structure of rules and group them by title
  buildRulesHierarchy(rulesList: CB_RulesDetailsList[]): CB_RulesDetailsList[] {
    // Function to get children of a parent rule recursively
    const getChildren = (parentRule: CB_RulesDetailsList): CB_RulesDetailsList[] => {
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

    // Group rules by title
    const grouped: GroupedRules[] = rulesListData.reduce((acc: GroupedRules[], rule) => {
      const key = rule.Title;
      let group = acc.find(g => g.title === key);

      // If the group doesn't exist, create a new one
      if (!group) {
        group = new GroupedRules();
        group.title = key;
        acc.push(group);
      }
      // Add the current rule to the group's rules
      group.rules.push(rule);
      return acc;
    }, []);
    this.groupRulesList = grouped;
    return rulesListData;
  }

  // Method to trim the start of the given text
  orderText(text: string): string {
    if (!text) return text;
    return text.trimStart();
  }
}

// Interface extending RulesDetailsList and adding childrens property
interface CB_RulesDetailsList extends RulesDetailsList {
  childrens: CB_RulesDetailsList[];
}

// Class defining a structure for grouped rules
class GroupedRules {
  title: string;
  rules: CB_RulesDetailsList[] = [];
  expanded?: boolean = false;
  showDropdown?: boolean = false;
  hideDropdown?: boolean = false;
}

// Helper class to get Hebrew letter from a number
class alephBetHelper {
  alephBet: string[] = ['א', 'ב', 'ג', 'ד', 'ה', 'ו', 'ז', 'ח', 'ט', 'י', 'כ', 'ל', 'מ', 'נ', 'ס', 'ע', 'פ', 'צ', 'ק', 'ר', 'ש', 'ת'];
  getLetterFromNumber(number) {
    const index = number - 1;
    if (index < 0 || index >= this.alephBet.length)
      throw new Error("Number out of range. Please enter a number between 1 and 22.");
    return `${this.alephBet[index]}-`;
  }
}

