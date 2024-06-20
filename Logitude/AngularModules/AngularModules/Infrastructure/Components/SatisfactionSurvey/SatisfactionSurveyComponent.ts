import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Location } from '@angular/common';
import * as Translate from './he.json';
import { SatisfactionSurveyService } from 'Infrastructure/Services/WebServices/SatisfactionSurveyService';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';

@Component({
    selector: 'SatisfactionSurveyComponent',
    templateUrl: './SatisfactionSurveyComponent.html',
})
export class SatisfactionSurveyComponent implements OnInit {
    tabs = [
        { id: 'tab1', content: Translate.NotAtAllSatisfied, score: 1, mainColor: '#E7340F', secondaryColor:'#FCE3E0'},
        { id: 'tab2', content: Translate.Slightly, score: 2, mainColor: '#FF8C00', secondaryColor:'#FFEFDF' },
        { id: 'tab3', content: Translate.Moderately, score: 3, mainColor: '#F2DB15', secondaryColor:'#FDFAE2' },
        { id: 'tab4', content: Translate.VeryMuch, score: 4, mainColor: '#6BB437', secondaryColor:'#EBF4E3' },
        { id: 'tab5', content: Translate.Excellent, score: 5, mainColor:'#4E8D00', secondaryColor:'#E6EFDD' },
    ];

    activeTab: string;
    moreDetailsTitle: string = Translate.MoreDetails;
    howSatisfiedAreYou: string = Translate.HowSatisfiedAreYou;
    thanks: string = Translate.Thanks;
    sentSuccessfully: string = Translate.SentSuccessfully;
    alreadySubmitted: string = Translate.AlreadySubmitted;
    surveyForm: FormGroup;
    formSubmitted: boolean = false;
    formAlreadySubmitted: boolean = false;
    formError: boolean = false;
    guid: string;
    hash: string;

    formSubmitting: boolean = false;

    constructor(private location: Location) {
        const ratingIndex = this.getRatingFromUrl() - 1;
        const tabContentIndex = ratingIndex >= 0 && ratingIndex < this.tabs.length ? ratingIndex : 0;
        this.activeTab = this.tabs[tabContentIndex].content;

        const urlParams = new URLSearchParams(window.location.search);
        this.guid = (urlParams.get('Guid') || urlParams.get('GUID') || '').toLowerCase();
        this.hash = (urlParams.get('Hash') || urlParams.get('HASH') || '').toLowerCase();


        this.surveyForm = new FormGroup({
            Comments: new FormControl(''),
            Rating: new FormControl(this.tabs[tabContentIndex].score),
            Id: new FormControl(this.guid),
            Guid: new FormControl(this.guid),
            Hash: new FormControl(this.hash),
        });
    }

    private getCurrentUrl(): string {
        return this.location.path();
    }

    private getRatingFromUrl(): number {
        const url = this.getCurrentUrl();
        const ratingIndex = url.indexOf('Rating=');
        if (ratingIndex !== -1) {
            const ratingString = url.substr(ratingIndex + 7);
            const rating = parseInt(ratingString, 10);
            if (!isNaN(rating)) {
                return rating;
            }
        }
        return 0;
    }

    ngOnInit() { }

    setActiveTab(tabContent: string) {
        this.activeTab = tabContent;
        const selectedTab = this.tabs.find(tab => tab.content === tabContent);
        if (selectedTab) {
            this.surveyForm?.get('Rating')?.setValue(selectedTab.score);
        }
    }

    satisfactionSurveyService: SatisfactionSurveyService = new SatisfactionSurveyService;

    ErrorMessage: string = "";
    onSubmit() {
        this.formSubmitting = true;
        this.satisfactionSurveyService.insert(this.surveyForm.value).pipe(
            catchError(error => {
                this.ErrorMessage = error.error.ErrorType;
                return of({ HasError: true, ErrorMessage: error.error.ErrorType });
            })
        ).subscribe(res => {
            if (!res.HasError) {
                this.formSubmitted = true;
            } else {
                this.formError = true;
                if (this.ErrorMessage === 'DbUpdateException') {
                    this.formAlreadySubmitted = true;
                }
            }
            this.formSubmitting = false;
        });
    }
}
