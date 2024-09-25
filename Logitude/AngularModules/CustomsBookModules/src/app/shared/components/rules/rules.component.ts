
import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { API_MainService } from '../../../core/API_MainService';
import { CB_CustomsItemComputedDataList, RulesList } from '../main-display/main-display.component';
import { NgFor, NgIf } from '@angular/common';
import { faChevronLeft, faSquareCaretRight } from '@fortawesome/free-solid-svg-icons';


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
  @Input() currentItem: CB_CustomsItemComputedDataList;
  allRules: CB_RuleList[] = [];

  constructor(private API_MainService: API_MainService) { }


  ngOnInit(): void {

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

  initData() {
    this.API_MainService.GetCustomsBookRulesData(this.currentItem.CustomsItemID).subscribe((data: any) => {
      debugger
      this.allRules = data.body;
      this.allRules.forEach((rule) => {
        rule.expanded = false;
      });
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
}

export interface CB_RuleList extends RulesList {
  content: string;
  expanded?: boolean;
  showDropdown?: boolean;
  hideDropdown?: boolean;
}