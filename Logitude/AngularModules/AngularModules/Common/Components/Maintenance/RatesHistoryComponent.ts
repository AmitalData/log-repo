import {Component} from '@angular/core';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UIProperty, UIProperties}  from '../../../Infrastructure/Components/LogitudeComponents/UIProperties'
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {RatesTableList} from '../../../Infrastructure/EntityLists/RatesTableList';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {CurrencyRatesService, LastRate} from '../../../Common/Services/CurrencyRatesService';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {RatesTableListService} from '../../../Infrastructure/Services/StandardLists/RatesTableListService';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';

@Component({
    
    templateUrl: './RatesHistoryComponent.html',
})

export class RatesHistoryComponent extends BaseComponent {

    public ItemsSource: RatesTableList[] = [];
    public LastRate: LastRate = new LastRate();
    private RatesTableListService: RatesTableListService;
    public count: number = 0;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    SetWindowArgs(args: LastRate) {
        this.LastRate = args;
        this.BuildData();
    }

    private filters: ApiQueryFilters;
    BuildData() {
        if (this.RatesTableListService == null) {
            this.RatesTableListService = new RatesTableListService();
        }

        this.filters = new ApiQueryFilters();
        this.filters.SortBy = "LogDateTime";
        this.filters.SortDirection = "Descending";
        this.filters.GetCount = true;
        var value = this.LastRate.ForeignCurrencyId;
        this.filters.addAdditionalFilter("ForeignCurrencyId", value, null, null, "Equals", false, false, false, "Text");

        this.PageIndex = 1;
        this.QueryPageIndex = 0;

        this.LoadData();
       // this.LoadDateCount();
    }

    LoadData() {
        //this.ItemsSource = [];
        this.filters.PageIndex = this.QueryPageIndex;
        this.filters.PageSize = this.PageSize;
        this.RatesTableListService.getByFilters(this.filters).subscribe((myResult:any) => {
            if (myResult == null) {
                this.ItemsSource = [];
            }

            else {
                this.ItemsSource = myResult.Result;
                this.count = myResult.Count;
                var size = this.pageSize;
                this.TotalPagesCount = Math.ceil(this.count / size);

                if (this.TotalPagesCount == 0) {
                    this.TotalPagesCount = 1;
                }

                this.SetPagerButtonsStates();
            }
        });
    }

    LoadDateCount() {
        var size = this.pageSize;
        this.TotalPagesCount = Math.ceil(this.count / size);

        if (this.TotalPagesCount == 0) {
            this.TotalPagesCount = 1;
        }

        this.SetPagerButtonsStates();
    }

    /*Pager & Provider*/
    private queryPageIndex = 0;
    get QueryPageIndex() {
        return this.queryPageIndex;
    }
    set QueryPageIndex(value: number) {
        this.queryPageIndex = value;
    }

    private pageIndex = 1;
    get PageIndex() {
        return this.pageIndex;
    }
    set PageIndex(value: number) {
        this.pageIndex = value;
    }

    private totalPagesCount = 1;
    get TotalPagesCount() {
        return this.totalPagesCount;
    }
    set TotalPagesCount(value: number) {
        this.totalPagesCount = value;
    }

    private pageSize = 10;
    get PageSize() {
        return this.pageSize;
    }
    set PageSize(value: number) {
        this.pageSize = value;
    }

    /* Pager Buttons States */
    private isHitStateFirstButton: boolean = false;
    get IsHitState_FirstButton() {
        return this.isHitStateFirstButton;
    }
    set IsHitState_FirstButton(value: boolean) {
        this.isHitStateFirstButton = value;
    }

    private opacityFirstButton: number = 0.5;
    get Opacity_FirstButton() {
        return this.opacityFirstButton;
    }
    set Opacity_FirstButton(value: number) {
        this.opacityFirstButton = value;
    }

    private isHitStatePrevButton: boolean = false;
    get IsHitState_PrevButton() {
        return this.isHitStatePrevButton;
    }
    set IsHitState_PrevButton(value: boolean) {
        this.isHitStatePrevButton = value;
    }

    private opacityPrevButton: number = 0.5;
    get Opacity_PrevButton() {
        return this.opacityPrevButton;
    }
    set Opacity_PrevButton(value: number) {
        this.opacityPrevButton = value;
    }

    private isHitStateNextButton: boolean = false;
    get IsHitState_NextButton() {
        return this.isHitStateNextButton;
    }
    set IsHitState_NextButton(value: boolean) {
        this.isHitStateNextButton = value;
    }

    private opacityNextButton: number = 0.5;
    get Opacity_NextButton() {
        return this.opacityNextButton;
    }
    set Opacity_NextButton(value: number) {
        this.opacityNextButton = value;
    }

    private isHitStateLastButton: boolean = false;
    get IsHitState_LastButton() {
        return this.isHitStateLastButton;
    }
    set IsHitState_LastButton(value: boolean) {
        this.isHitStateLastButton = value;
    }

    private opacityLastButton: number = 0.5;
    get Opacity_LastButton() {
        return this.opacityLastButton;
    }
    set Opacity_LastButton(value: number) {
        this.opacityLastButton = value;
    }

    /* First Page */
    private SetPagerButtonsStates() {
        if (this.PageIndex == 1 && this.PageIndex == this.TotalPagesCount) {
            this.IsHitState_FirstButton = false;
            this.IsHitState_PrevButton = false;
            this.IsHitState_NextButton = false;
            this.IsHitState_LastButton = false;

            this.Opacity_FirstButton = 0.5;
            this.Opacity_PrevButton = 0.5;
            this.Opacity_NextButton = 0.5;
            this.Opacity_LastButton = 0.5;
        }

        else if (this.PageIndex == 1 && this.PageIndex < this.TotalPagesCount) {
            this.IsHitState_FirstButton = false;
            this.IsHitState_PrevButton = false;
            this.Opacity_FirstButton = 0.5;
            this.Opacity_PrevButton = 0.5;

            this.IsHitState_NextButton = true;
            this.IsHitState_LastButton = true;
            this.Opacity_NextButton = 1;
            this.Opacity_LastButton = 1;
        }

        else if (this.PageIndex > 1 && this.PageIndex == this.TotalPagesCount) {
            this.IsHitState_FirstButton = true;
            this.IsHitState_PrevButton = true;
            this.Opacity_FirstButton = 1;
            this.Opacity_PrevButton = 1;

            this.IsHitState_NextButton = false;
            this.IsHitState_LastButton = false;
            this.Opacity_NextButton = 0.5;
            this.Opacity_LastButton = 0.5;
        }

        else if (this.PageIndex > 1 && this.PageIndex < this.TotalPagesCount) {
            this.IsHitState_FirstButton = true;
            this.IsHitState_PrevButton = true;
            this.IsHitState_NextButton = true;
            this.IsHitState_LastButton = true;

            this.Opacity_FirstButton = 1;
            this.Opacity_PrevButton = 1;
            this.Opacity_NextButton = 1;
            this.Opacity_LastButton = 1;
        }
    }

    FirstPageClick() {
        this.PageIndex = 1;
        this.QueryPageIndex = 0;
        this.SetPagerButtonsStates();
        this.LoadData();
    }

    PreviousPageClick() {
        this.PageIndex = this.PageIndex - 1;
        this.QueryPageIndex = this.QueryPageIndex - 10;
        this.SetPagerButtonsStates();
        this.LoadData();
    }

    NextPageClick() {
        this.PageIndex = this.PageIndex + 1;
        this.QueryPageIndex = this.QueryPageIndex + 10;
        this.SetPagerButtonsStates();
        this.LoadData();
    }

    LastPageClick() {
        this.PageIndex = this.TotalPagesCount;
        this.QueryPageIndex = (this.TotalPagesCount - 1) * this.pageSize;
        this.SetPagerButtonsStates();
        this.LoadData();
    }

    // Props
    get ForeignCurrencyCode() {
        return this.LastRate.ForeignCurrencyCode;
    }

    get ValueDate() {
        return this.LastRate.ValueDate;
    }

    get Rate() {
        return this.LastRate.Rate;
    }

    // Commands
    public CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

}
