import {ApiQueryFilters, FilterItem} from '../../Infrastructure/DataContracts/ApiQueryFilters';
export class InfraGenericFilter {

    public static GetFilteredArray(dataArray: Array<any>, filters: ApiQueryFilters) {
      
        if (filters.Filter1Name) {
            dataArray = this.FilterArray(dataArray, filters.Filter1Name, filters.Filter1Operator, filters.Filter1Value, filters.Filter1Value2);
        }

        if (filters.Filter2Name) {
            dataArray = this.FilterArray(dataArray, filters.Filter2Name, filters.Filter2Operator, filters.Filter2Value, filters.Filter2Value2);
        }

        if (filters.Filter3Name) {
            dataArray = this.FilterArray(dataArray, filters.Filter3Name, filters.Filter3Operator, filters.Filter3Value, filters.Filter3Value2);
        }

        if (filters.Filter4Name) {
            dataArray = this.FilterArray(dataArray, filters.Filter4Name, filters.Filter4Operator, filters.Filter4Value, filters.Filter4Value2);
        }

        if (filters.Filter5Name) {
            dataArray = this.FilterArray(dataArray, filters.Filter5Name, filters.Filter5Operator, filters.Filter5Value, filters.Filter5Value2);
        }
        var dddd = dataArray.filter(d=> d.Code === 'AR')[0];
        
        for (var k in filters.AdditionalFilters) {

            var filter: FilterItem = filters.AdditionalFilters[k];
            if (!filter.IsCustom) {
            //console.log(dataArray);
                dataArray = this.FilterArray(dataArray, filter.FieldName, filter.Operator, filter.FieldValue, filter.FieldValue2);
            }
        }
        if (filters.PageSize < dataArray.length) {
            return dataArray.slice(0, filters.PageSize);
        }
        else {
            return dataArray;
        }
    }

    private static FilterArray(dataArray: Array<any>, fieldName: string, operator: string, value1: any, value2) {

       
        switch (operator) {
            case "LargerThan":
                {
                    return dataArray.filter(function (item) {
                        return item[fieldName] > value1;
                    });
                    //break;
                }

            case "GreaterThanOrEqual":
                {
                    return dataArray.filter(function (item) {
                        return item[fieldName] >= value1;
                    });

                    //break;
                }

            case "LessThan":
                {
                    return dataArray.filter(function (item) {
                        return item[fieldName] < value1;
                    });

                    //break;
                }
            case "LessThanOrEqual":
                {
                    return dataArray.filter(function (item) {
                        return item[fieldName] <= value1;
                    });

                    //break;
                }
            case "StartsWith":
                {
                    return dataArray.filter(function (item) {
                        var nananana = item[fieldName];
                        
                        if (item[fieldName] != null && item[fieldName] != undefined) {
                            var exist = item[fieldName].toLowerCase().startsWith(value1.toLowerCase());
                            if (exist) {
                                var fffffff = "hello";
                            }
                            return exist;
                        }
                        else {
                            return true;
                        }
                    });
                    //break;
                }

            case "InList":
                {
                    //e.g: var list = value1.split(',');
                    // value1 = 'CS,AG' , fieldName = PartnerTypeId = 'CS'
                    return dataArray.filter(function (item) {
                        if (item[fieldName] != null && item[fieldName] != undefined) {
                            return value1.toLowerCase().indexOf(item[fieldName].toLowerCase()) > -1;
                        }
                        else {
                            return true;
                        }
                    });

                    //break;
                }
            case "InListExact":
                {
                    //e.g: value1 = 'CS,AG' , fieldName = PartnerTypeId = 'CS'
                    var list = value1.toLowerCase().split(',');//{'CS','AG'}
                    var filteredArr = [];
                    for (var key in list) {
                        var subArr = dataArray.filter(function (item) {
                            if (item[fieldName] != null && item[fieldName] != undefined) {
                                return item[fieldName].toLowerCase() === list[key].toLowerCase();
                            }
                            else {
                                return true;
                            }
                        });

                        filteredArr = filteredArr.concat(subArr);
                    }

                    filteredArr = InfraGenericFilter.GetUniqueArray(filteredArr);
                    return filteredArr;
                    
                }
            case "Contains":
                {

                    return dataArray.filter(function (item) {
                        if (item[fieldName] != null && item[fieldName] != undefined) {
                            return item[fieldName].toLowerCase().indexOf(value1.toLowerCase()) > -1;
                        }
                        else {
                            return true;
                        }
                    });
                    //break;
                }
            case "NotContains":
                {

                    return dataArray.filter(function (item) {
                        if (item[fieldName] != null && item[fieldName] != undefined) {
                            return item[fieldName].toLowerCase().indexOf(value1.toLowerCase()) == -1; // not found, returns true
                        }
                        else {
                            return false;
                        }
                    });
                    //break;
                }
            case "Between":
                {
                    return dataArray.filter(function (item) {
                       
                            return (item[fieldName] >= value1 && item[fieldName] <= value2);
                       
                        //return false;

                    });

                    //break;
                }
            case "NotEqual":
                {
                    if (typeof (value1) === "string") {
                        return dataArray.filter(function (item) {
                            if (item[fieldName] != null && item[fieldName] != undefined) {
                                return item[fieldName].toLowerCase() != value1.toLowerCase();
                            }
                            else {
                                return true;
                            }
                        });
                    }
                    else {
                        return dataArray.filter(function (item) {
                            return item[fieldName] != value1;
                        });
                    }

                    //break;
                }
            case "Exclude": {
                return dataArray.filter(function (item) {
                    if (item[fieldName] != null && item[fieldName] != undefined) {
                        return value1.toLowerCase().indexOf(item[fieldName].toLowerCase()) == -1;
                    }
                    else {
                        return true;
                    }
                });
            }
            case "ExcludeList": {
                return dataArray.filter(function (item) {
                    if (item[fieldName] != null && item[fieldName] != undefined) {
                        return value1.toLowerCase().split(',').indexOf(item[fieldName].toLowerCase()) == -1;
                    }
                    else {
                        return true;
                    }
                });
            }
            case "IsNotNull": {
                return dataArray.filter(function (item) {
                    if (item[fieldName] != null && item[fieldName] != undefined) {
                        return value1 != null && value1 != undefined;
                    }
                    else {
                        return true;
                    }
                });
            }
            case "IsNull": {
                return dataArray.filter(function (item) {
                    if (item[fieldName] != null && item[fieldName] != undefined) {
                        return value1 == null || value1 == undefined;
                    }
                    else {
                        return true;
                    }
                });
            }
            default:
                {
                    if (typeof (value1) === "string") {
                        return dataArray.filter(function (item) {
                            if (item[fieldName] != null && item[fieldName] != undefined) {
                                return item[fieldName].toLowerCase() === value1.toLowerCase();
                            }
                            else {
                                return true;
                            }
                        });
                    }
                    else {
                        return dataArray.filter(function (item) {
                            return item[fieldName] === value1;
                        });
                    }
                    

                    //break;
                }

        }
    }


    public static GetUniqueArray(array) {
        var a = array.concat();
        for (var i = 0; i < a.length; ++i) {
            for (var j = i + 1; j < a.length; ++j) {
                if (a[i] === a[j])
                    a.splice(j--, 1);
            }
        }

        return a;
    }
}

