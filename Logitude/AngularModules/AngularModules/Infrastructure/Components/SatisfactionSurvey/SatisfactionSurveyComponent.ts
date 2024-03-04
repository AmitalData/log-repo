import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Location } from '@angular/common';

@Component({
    selector: 'SatisfactionSurveyComponent',
    templateUrl: './SatisfactionSurveyComponent.html',
})
export class SatisfactionSurveyComponent implements OnInit {
    tabs = [
        { id: 'tab1', content: 'כלל לא מרוצה' },
        { id: 'tab2', content: 'במידה מועטה' },
        { id: 'tab3', content: 'במידה בינונית' },
        { id: 'tab4', content: 'במידה רבה' },
        { id: 'tab5', content: 'במידה רבה מאוד' },
    ];

    activeTab: string = this.tabs[4].content;
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
