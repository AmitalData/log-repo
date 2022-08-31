import { WidgetFilterComponent } from './WidgetFilterComponent';
import { UIProperties } from 'Infrastructure/Components/LogitudeComponents/UIProperties';
import { AppTool } from 'Infrastructure/Tools';
import { QueryFilterViewItem } from 'Infrastructure/DataContracts/QueryFilterViewItem';
import { FilterItem } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { AnalyticsFactsFieldsMetaDataPM } from 'DashboardModule/EntityPMs/AnalyticsFactsFieldsMetaDataPM';


export class WidgetFilterItem{
    public BaseTreeFilter: WidgetFilterItem;
    public UIProperties: UIProperties;
    public FieldName: string;
    public IndexOrder: number;
    public IsGroup: boolean = false;
    public QueryFilterItems: WidgetFilterItem[] = [];

    public FilterType: string = 'And'
    private andOr: string = "And";
    public AndOrOps = ["And", "Or"];

    constructor(treeFilter: WidgetFilterItem = null) {
        this.BaseTreeFilter = treeFilter;
        this.UIProperties = new UIProperties;
    }

    public setAndOrOperation(Newvalue: string) {
        this.AndOr = Newvalue;
    }

    public set AndOr(newValue: string) {
        this.FilterType = newValue;
        this.andOr = newValue;
    }
   
    public get AndOr() {
        let temp = AppTool.IsNullOrEmpty(this.andOr) ? "And" : this.andOr;
        this.FilterType = temp;
        return temp;
    }
    
    
}