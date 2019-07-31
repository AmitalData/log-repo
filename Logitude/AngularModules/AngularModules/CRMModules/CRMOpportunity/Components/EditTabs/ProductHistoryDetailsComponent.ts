import {Component, OnInit} from '@angular/core';
import {OpportunityPM} from '../../../../CRM/EntityPMs/OpportunityPM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {CustomerProductLocationActualDataPM} from '../../../../Common/EntityPMs/CustomerProductLocationActualDataPM';
import {CustomerProductActualDataPM} from '../../../../Common/EntityPMs/CustomerProductActualDataPM';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';

@Component({
    moduleId: module.id,
    templateUrl: './ProductHistoryDetailsComponent.html',
})

export class ProductHistoryDetailsComponent extends BaseComponent {
    private myCurrencyCode: string = "";
    public ObjectTableName = "CustomerProductActualData";
    public DataContext = this;
    private entityPM: CustomerProductActualDataPM;
    public ItemsSource: ObservableCollection;
    public TEUVisibility = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    SetWindowArgs(args: any) {
        this.entityPM = args;
        this.ItemsSource = new ObservableCollection([]);
        if (this.entityPM != null) {
            this.TEUVisibility = this.entityPM.ProductTypeCode.substring(0, 1) == "A" ? false : true; 
            this.ItemsSource.AppendCollection(this.entityPM.ProductLocations.filter(p => p.Year == this.entityPM.Year && p.Month == this.entityPM.Month));
        }
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
