import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Location } from '@angular/common';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';

@Component({
    selector: 'SatisfactionSurveyComponent',
    templateUrl: './SatisfactionSurveyComponent.html',
})
export class SatisfactionSurveyComponent implements OnInit {
    tabs = [
        { id: 'tab1', content: TextCodeTranslator.Translate("SatisfactionSurvey.O.NotAtAllSatisfied") },
        { id: 'tab2', content: TextCodeTranslator.Translate("SatisfactionSurvey.O.Slightly") },
        { id: 'tab3', content: TextCodeTranslator.Translate("SatisfactionSurvey.O.Moderately")  },
        { id: 'tab4', content: TextCodeTranslator.Translate("SatisfactionSurvey.O.VeryMuch")  },
        { id: 'tab5', content: TextCodeTranslator.Translate("SatisfactionSurvey.O.Extent")  },
    ];

    activeTab: string = this.tabs[4].content;
    moreDetailsTitle: string = TextCodeTranslator.Translate("SatisfactionSurvey.O.MoreDetails");
    surveyForm: FormGroup;
    formSubmitted: boolean = false;

    constructor(private location: Location) {
        this.surveyForm = new FormGroup({
            feedback: new FormControl(''),
            activeTab: new FormControl(this.activeTab),
            currentUrl: new FormControl(this.getCurrentUrl()),
        });
    }

    private getCurrentUrl(): string {
        return this.location.path(); // Return the current URL path
    }

    ngOnInit() {}

    setActiveTab(tabContent: string) {
        this.activeTab = tabContent;
        this.surveyForm.get('activeTab').setValue(this.activeTab);
    }

    onSubmit() {
        console.log('Form submitted!');
        console.log(this.surveyForm.value);

        this.formSubmitted = true;
    }
}
