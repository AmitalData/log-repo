
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

  allRules: CB_RulesDetailsList[] = [];

  constructor(private API_MainService: API_MainService) { }


  ngOnInit(): void {
    this.currentItem.subscribe((data: CB_CustomsItemComputedDataList) => {
      if (data?.CustomsItemID != null)
        this.initData(data?.CustomsItemID);
    });
  }

  // moke data to remove when get data from API
  rules = [
    {
      title: 'כללים לפרק',
      content: [
        'א- דגים, סרטנים לסוגיהם (CRUSTACEANS) , רכיכות (MOLLUSCS) , וחסרי חוליות אחרים החיים במים, שבפרטים 03.01, 03.06, 03.07 או 03.08;' + "\n" +
        'ב- תרביות של מיקרואורגניזמים ומוצרים אחרים שבפרט 30.02; וכן' + "\n" +
        'ג- בעלי חיים שבפרט 95.08.'
      ],
      expanded: false,
      showDropdown: false,
      hideDropdown: false
    },
    {
      title: 'כללים לפרק',
      content: [
        'א- דגים, סרטנים לסוגיהם (CRUSTACEANS) , רכיכות (MOLLUSCS) , וחסרי חוליות אחרים החיים במים, שבפרטים 03.01, 03.06, 03.07 או 03.08;' + "\n" +
        'ב- תרביות של מיקרואורגניזמים ומוצרים אחרים שבפרט 30.02; וכן' + "\n" +
        'ג- בעלי חיים שבפרט 95.08.'
      ],
      expanded: false,
      showDropdown: false,
      hideDropdown: false
    },
    {
      title: 'כללים לפרק',
      content: [
        'א- דגים, סרטנים לסוגיהם (CRUSTACEANS) , רכיכות (MOLLUSCS) , וחסרי חוליות אחרים החיים במים, שבפרטים 03.01, 03.06, 03.07 או 03.08;' + "\n" +
        'ב- תרביות של מיקרואורגניזמים ומוצרים אחרים שבפרט 30.02; וכן' + "\n" +
        'ג- בעלי חיים שבפרט 95.08.'
      ],
      expanded: false,
      showDropdown: false,
      hideDropdown: false
    }
  ];

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['showRules']) {
      this.showRules = changes['showRules'].currentValue;
    }
  }

  initData(customsItemID: number) {
    this.API_MainService.GetCustomsBookRulesData(customsItemID).subscribe((data: any) => {
      // this.allRules = data.body;
      console.log(data.body);
      if(!data.body) return; // TODO: add error message
      this.allRules = this.ConvertToRulesList(data.body);
      console.log(this.allRules);
      
    });
  }

  // convert to rules list to show in the view by grouping the rules by title:
  ConvertToRulesList(rules: any): CB_RulesDetailsList[] {
    let rulesList: CB_RulesDetailsList[] = [];

    rules.forEach((rule: any) => {
      let title = rule.Title;
      let rulesDetailsList: CB_RulesDetailsList = rulesList.find((x: CB_RulesDetailsList) => x.title === title);

      if (rulesDetailsList) {
        rulesDetailsList.rulesList.push(rule);
      }
      else {
        rulesDetailsList = {
          title: title,
          rulesList: [rule]
        }
        rulesList.push(rulesDetailsList);
      }
    });

    return rulesList;
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
}

export interface CB_RulesDetailsList {
  title: string;
  rulesList: RulesDetailsList[];
  expanded?: boolean;
  showDropdown?: boolean;
  hideDropdown?: boolean;
}