declare const window: any;

import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Location } from '@angular/common';
import * as Translate from './he.json';
import { SatisfactionSurveyService } from 'Infrastructure/Services/WebServices/SatisfactionSurveyService';

@Component({
    selector: 'SatisfactionSurveyComponent',
    templateUrl: './SatisfactionSurveyComponent.html',
})
export class SatisfactionSurveyComponent implements OnInit {
    tabs = [
        { id: 'tab1', content: Translate.NotAtAllSatisfied },
        { id: 'tab2', content: Translate.Slightly },
        { id: 'tab3', content: Translate.Moderately },
        { id: 'tab4', content: Translate.VeryMuch },
        { id: 'tab5', content: Translate.Extent },
    ];

    activeTab: string = this.tabs[4].content;
    moreDetailsTitle: string = Translate.MoreDetails;
    howSatisfiedAreYou: string = Translate.HowSatisfiedAreYou;
    thanks: string = Translate.Thanks;
    sentSuccessfully: string = Translate.SentSuccessfully;
    surveyForm: FormGroup;
    formSubmitted: boolean = false;

    constructor(private location: Location) {
        this.surveyForm = new FormGroup({
            Comments: new FormControl(''),
            Rating: new FormControl(this.activeTab),
            Id: new FormControl(this.getCurrentUrl().split('=')[1]),
        });
    }

    private getCurrentUrl(): string {
        return this.location.path();
    }

    ngOnInit() { }

    setActiveTab(tabContent: string) {
        this.activeTab = tabContent;
        this.surveyForm?.get('activeTab')?.setValue(this.activeTab);
    }

    satisfactionSurveyService: SatisfactionSurveyService = new SatisfactionSurveyService;
    onSubmit() {
        console.log('Form submitted!');
        console.log(this.surveyForm.value);
        this.satisfactionSurveyService.insert(this.surveyForm.value).subscribe(res => {
            if (!res.HasError) {

                this.formSubmitted = true;
            }
        });
    }
}
