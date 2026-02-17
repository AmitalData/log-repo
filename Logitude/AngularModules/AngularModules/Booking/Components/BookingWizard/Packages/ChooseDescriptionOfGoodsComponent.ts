import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {BookingPM} from '../../../EntityPMs/BookingPM';
import {AWBDescriptionOfGoodsList} from '../../../../Common/EntityLists/AWBDescriptionOfGoodsList';
import {AWBDescriptionOfGoodsListService} from '../../../../Common/Services/StandardLists/AWBDescriptionOfGoodsListService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    selector: 'ChooseDescriptionOfGoodsComponent',
    moduleId: module.id,
    templateUrl: './ChooseDescriptionOfGoodsComponent.html',
})

export class ChooseDescriptionOfGoodsComponent extends BaseComponent {
    public EntityPM: BookingPM;
    public ItemsSource: AWBDescriptionOfGoodsList[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    SetWindowArgs(entityPM: BookingPM) {
        this.EntityPM = entityPM;        
        this.LoadData();
    }

    private searchTextValue: string = null;
    get SearchTextValue() { return this.searchTextValue; }
    set SearchTextValue(newValue: string) {
        if (this.searchTextValue != newValue) {
            this.searchTextValue = newValue;

            this.LoadData();
        }
    }

    public Count: number = 0;
    private LoadData() {
        if (this.ItemsSource == null) {
            this.ItemsSource = new Array<AWBDescriptionOfGoodsList>();
        }

        else {
            this.ItemsSource = [];
        }

        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;

        filters.Filter1Name = "AirlineCode";
        filters.Filter1Value = this.EntityPM.MainCarriageCarrierCode;
        filters.Filter1Operator = "Equals";

        if (!AppTool.IsNullOrEmpty(this.SearchTextValue)) {
            filters.Filter2Name = "SearchFields";
            filters.Filter2Value = this.SearchTextValue;
            filters.Filter2Operator = "Contains";
        }

        var myService: AWBDescriptionOfGoodsListService = new AWBDescriptionOfGoodsListService();
        myService.getByFilters(filters).subscribe(myResult => {            
            if (myResult == null) {
                this.ItemsSource = [];
            }

            else {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {
                    this.ItemsSource = myResponse.Result;
                }
            }

            this.Count = this.ItemsSource.length;
        });
    }

    Selecting(item: AWBDescriptionOfGoodsList) {
        if (item == null) {
            this.EntityPM.DescriptionOfGoodsId = null;
            this.EntityPM.DescriptionOfGoods = null;
            this.EntityPM.DescriptionOfGoodsService = null;
            this.EntityPM.IsTemperatureSensitive = false;

        }

        else {
            this.EntityPM.DescriptionOfGoodsId = item.Id;
            this.EntityPM.DescriptionOfGoods = item.ShortDescriptionOfGoods;
            this.EntityPM.DescriptionOfGoodsService = item.Service;
            this.EntityPM.IsTemperatureSensitive = item.IsTemperatureSensitive;
        }
        
        this.CloseButtonClicked();
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
