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
        { id: 'tab1', content: Translate.NotAtAllSatisfied },
        { id: 'tab2', content: Translate.Slightly },
        { id: 'tab3', content: Translate.Moderately },
        { id: 'tab4', content: Translate.VeryMuch },
        { id: 'tab5', content: Translate.Extent },
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
        const ratingIndex = this.getRatingFromUrl();
        const tabContentIndex = ratingIndex >= 0 && ratingIndex < this.tabs.length ? ratingIndex : 0;
        this.activeTab = this.tabs[tabContentIndex].content;

        const urlParams = new URLSearchParams(window.location.search);
        this.guid = (urlParams.get('Guid') || urlParams.get('GUID') || '').toLowerCase();
        this.hash = (urlParams.get('Hash') || urlParams.get('HASH') || '').toLowerCase();


        this.surveyForm = new FormGroup({
            Comments: new FormControl(''),
            Rating: new FormControl(this.activeTab),
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
        this.surveyForm?.get('activeTab')?.setValue(this.activeTab);
    }

    satisfactionSurveyService: SatisfactionSurveyService = new SatisfactionSurveyService;

    onSubmit() {
        this.formSubmitting = true;
        this.satisfactionSurveyService.insert(this.surveyForm.value).pipe(
            catchError(error => {
                return of({ HasError: true, ErrorMessage: error.error.ErrorType });
            })
        ).subscribe(res => {
            if (!res.HasError) {
                this.formSubmitted = true;
            } else {
                this.formError = true;
                if (res?.ErrorMessage === 'DbUpdateException') {
                    this.formAlreadySubmitted = true;
                }
            }
            this.formSubmitting = false;
        });
    }
}
