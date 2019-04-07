import {Component, OnInit} from '@angular/core';
import {OpportunityPM} from '../../../../CRM/EntityPMs/OpportunityPM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {QuoteList} from '../../../../Quote/EntityLists/QuoteList';
import {QuoteListService} from '../../../../Quote/Services/StandardLists/QuoteListService';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {QuoteDomainService} from '../../../../Quote/Services/QuoteDomainService';
@Component({
    selector: 'QuotesWindowComponent',
    moduleId: module.id,
    templateUrl: './QuotesWindowComponent.html',
})

export class QuotesWindowComponent extends BaseComponent {
    public ObjectTableName: string = "Opportunity";
    public DataContext: QuotesWindowComponent = this;
    public ObsList: Array<QuoteItemClass> = [];
    public EntityPM: OpportunityPM;
    public searchText: string = "";
    private quoteListService: QuoteListService;
    private quoteDomainService: QuoteDomainService;
    public get SearchText() { return this.searchText; }
    public set SearchText(newValue: string) {
        this.searchText = newValue;
        this.OnSearchTextChanged();        
    }

    private CurrentSession = SessionLocator.SelectedSession;
    SetWindowArgs(args: OpportunityPM) {
        this.EntityPM = args;
        this.LoadQuotesList();
    }

    public NoConnectedQuotes: boolean = false;

    LoadQuotesList() {
        this.ObsList = [];
        if (this.EntityPM != null) {

            var filters = new ApiQueryFilters;

            filters.PageSize = 25;
            filters.PageIndex = 0;
            var SearchedValue = null;
            if (this.SearchText != null)
                SearchedValue = this.SearchText.trim();
            filters.addAdditionalFilter("CustomerId", this.EntityPM.CustomerId, null, null, "Equals", false, false, false, "string");
            filters.addAdditionalFilter("NotConnectedOpportunity", true, null, null, "Equals", true, true, false, "string");
            filters.addAdditionalFilter("SearchFields", SearchedValue, null, null, "Contains", false, false, false, "string");

            this.quoteListService.getByFilters(filters).subscribe(result => {
                var QuoteList: Array<QuoteList> = result.Result.reverse();
                if (QuoteList.length == 0)
                    this.NoConnectedQuotes = true;
                else
                    this.NoConnectedQuotes = false;

                QuoteList.forEach(item => {
                    this.ObsList.push(new QuoteItemClass(item, this));
                });

            });
        }

    }

    public selectedItem = null;

    public get SelectedItem() { return this.selectedItem; }

    public set SelectedItem(item: any) { this.selectedItem = item; }

    private timerToken: any;

    CancelButtonClicked() {

   
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    }
    OkButtonClicked() {
        var myConnectedQuotes:string ="";
        this.CurrentSession.StartBusyIndicatorSaving();
        this.ObsList.filter(p => p.IsChecked).forEach(item => { myConnectedQuotes+=item.QuoteId+":"; });
        if (myConnectedQuotes.length > 0) {
            this.quoteDomainService.ConnectQuotesToOpportunity(this.EntityPM.Id, myConnectedQuotes).subscribe(p => {
                this.CurrentSession.StopBusyIndicator();
                this.CurrentSession.CloseCurrentWindowEmit("ok");
            });
        }
        else {

            this.CurrentSession.CloseCurrentWindowEmit("cancel");

        }

    }

    OnSearchTextChanged() {

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        this.timerToken = setTimeout(() => this.LoadQuotesList(), 500);


    }
   
    public get SearchTextLabel() { return TextCodeTranslator.Translate("Quote.F.SearchFields"); }
    constructor() {
        super();
        this.quoteListService = new QuoteListService();
        this.quoteDomainService = new QuoteDomainService();
    }

    public get OKButtonEnabled() {

        var myResult: boolean = false;

        if (this.ObsList.filter(d => d.IsChecked)[0]) {
            myResult = true;
        }

        return myResult;
    


    }

    

}

class QuoteItemClass extends BaseComponent{
    private entityList: QuoteList;
    private trigger: QuotesWindowComponent;
    public quoteId: string;
    public get QuoteId() { return this.quoteId; }
    public set QuoteId(value: string) { this.quoteId = value; }


    constructor(item: QuoteList, trigger: QuotesWindowComponent) {
        super();
        this.entityList = item;
        this.trigger = trigger;
        this.QuoteId = item.Id;

    }

    private isChecked: boolean;
    public get IsChecked() { return this.isChecked; }
    public set IsChecked(value: boolean) {
        if (this.isChecked != value) {
            this.isChecked = value;
        }
    }

    public get QuoteNumber() { return this.entityList.QuoteNumber; }
    public get OpenDate() { return this.entityList.OpenDate; }
    public get ExpirationDate() { return this.entityList.ExpirationDate; }
    public get StageName() { return this.entityList.StageName; } 
    public get QuoteTypeName() { return this.entityList.QuoteTypeName; } 
    public get CarrierName() { return this.entityList.CarrierName; } 
    public get FromCountryCode() { return this.entityList.FromCountryCode; } 
    public get FromPortCountry() { return this.entityList.FromPortCountry; } 
    public get ToCountryCode() { return this.entityList.ToCountryCode; } 
    public get ToPortCountry() { return this.entityList.ToPortCountry; } 
    
}
