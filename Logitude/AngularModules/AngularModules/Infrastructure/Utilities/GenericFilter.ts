
import {ApiQueryFilters} from '../DataContracts/ApiQueryFilters';

export class GenericFilter {

    static StaticDataArray: any[];

    public static ApplyFilters(apiFilter: ApiQueryFilters, dataArray: any[]) {
        this.StaticDataArray = dataArray;

        var mykeys = Object.keys(apiFilter);
        var addtionalFiltersValues = null;
        if (apiFilter.Filter1Value) {
            this.AddFilter(apiFilter.Filter1Name, apiFilter.Filter1Value, apiFilter.Filter1Operator);
        }
        if (apiFilter.Filter2Value) {
            this.AddFilter(apiFilter.Filter2Name, apiFilter.Filter2Value, apiFilter.Filter2Operator);
        }
        if (apiFilter.Filter3Value) {
            this.AddFilter(apiFilter.Filter3Name, apiFilter.Filter3Value, apiFilter.Filter3Operator);
        }
        if (apiFilter.Filter4Value) {
            this.AddFilter(apiFilter.Filter4Name, apiFilter.Filter4Value, apiFilter.Filter4Operator);
        }
        if (apiFilter.Filter5Value) {
            this.AddFilter(apiFilter.Filter5Name, apiFilter.Filter5Value, apiFilter.Filter5Operator);
        }
        if (apiFilter.Filter6Value) {
            this.AddFilter(apiFilter.Filter6Name, apiFilter.Filter6Value, apiFilter.Filter6Operator);
        }
        if (apiFilter.Filter7Value) {
            this.AddFilter(apiFilter.Filter7Name, apiFilter.Filter7Value, apiFilter.Filter7Operator);
        }
        if (apiFilter.Filter8Value) {
            this.AddFilter(apiFilter.Filter8Name, apiFilter.Filter8Value, apiFilter.Filter8Operator);
        }
        if (apiFilter.Filter9Value) {
            this.AddFilter(apiFilter.Filter9Name, apiFilter.Filter9Value, apiFilter.Filter9Operator);//bug must be operator not perator
        }
        if (apiFilter.Filter10Value) {
            this.AddFilter(apiFilter.Filter10Name, apiFilter.Filter10Value, apiFilter.Filter10Operator);
        }



        return this.StaticDataArray;
    }

    static AddFilter(filterName: string, filterValue: any, filterOperator: string) {
        switch (filterOperator.toLowerCase()) {
            case ('startswith'): {

                this.StaticDataArray = this.StaticDataArray.filter(v=> v[filterName].startsWith(filterValue));

                break;
            }
            case ('contains'): {

                this.StaticDataArray = this.StaticDataArray.filter(v=> v[filterName].search(filterValue));

                break;
            }
            case ('equals'): {
                this.StaticDataArray = this.StaticDataArray.filter(v=> v[filterName] === (filterValue));
                break;
            }
            case ('largerthan'): {
                this.StaticDataArray = this.StaticDataArray.filter(v=> v[filterName] > (filterValue));
                break;
            }
            case ('largerthan'): {
                this.StaticDataArray = this.StaticDataArray.filter(v=> v[filterName] > (filterValue));
                break;
            }
            case ('greaterthanorequal'): {
                this.StaticDataArray = this.StaticDataArray.filter(v=> v[filterName] >= (filterValue));
                break;
            }
            case ('lessthan'): {
                this.StaticDataArray = this.StaticDataArray.filter(v=> v[filterName] < (filterValue));
                break;
            }
            case ('lessthanorequal'): {
                this.StaticDataArray = this.StaticDataArray.filter(v=> v[filterName] <= (filterValue));
                break;
            }
            //case ('inlist'): {
            //    var listOfValus: string[] = filterValue.Split(',');
            //    this.StaticDataArray = this.StaticDataArray.filter(v=> listOfValus.filter(d=> d.search(v)>-1));
            //    break;
            //}

        }
    }

  

}