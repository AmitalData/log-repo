import { FilterMetadata, LazyLoadEvent } from 'primeng/api';
import { AppTool } from '../Tools';
import { ApiQueryFiltersAddParams } from './ApiQueryFiltersAddParams';
export class ApiQueryFilters {
	constructor(getAll: boolean = false, private usePrimNG = false) {
		this.GetAll = getAll;
	}
	public PageIndex: number;
	public PageSize: number;
	public SortBy: string;
	public SortDirection: 'Decending' | 'Ascending' | string;
	public GetCount: boolean;
	public Tenant: number;
	public AdditionalFilters: FilterItem[] = [];
	public ForceCacheRefresh: boolean = false;
	public DontApplyVirtualization: boolean = false;

	public MyAmitalLazyLoadEvent: AmitalLazyLoadEvent;
	AddAmitaFilterMetadata(prop: string, propValue: any, matchMode: string, operator: string) {
		if (AppTool.IsNullOrEmpty(this.MyAmitalLazyLoadEvent)) {
			this.MyAmitalLazyLoadEvent = new AmitalLazyLoadEvent();
		}
		let myAmitaFilterMetadata = new AmitaFilterMetadata(prop);
		myAmitaFilterMetadata.value = [];
		myAmitaFilterMetadata.value.push({
			value: propValue,
			matchMode: matchMode,
			operator: operator,
		});
		if (this.MyAmitalLazyLoadEvent.filters == null) {
			this.MyAmitalLazyLoadEvent.filters = [];
		}
		this.MyAmitalLazyLoadEvent.filters.push(myAmitaFilterMetadata);
	}
	MapLazyEvent($event: LazyLoadEvent, TotalRecords?: number) {
		if (AppTool.IsNullOrEmpty(this.MyAmitalLazyLoadEvent)) {
			this.MyAmitalLazyLoadEvent = new AmitalLazyLoadEvent();
		}
		this.MyAmitalLazyLoadEvent.GetCount = TotalRecords == null;
		this.MyAmitalLazyLoadEvent.first = $event.first;
		this.MyAmitalLazyLoadEvent.rows = $event.rows;
		this.MyAmitalLazyLoadEvent.sortField = $event.sortField;
		this.MyAmitalLazyLoadEvent.sortOrder = $event.sortOrder;
		if (this.MyAmitalLazyLoadEvent.filters == null) {
			this.MyAmitalLazyLoadEvent.filters = [];
		}
		for (let prop in $event.filters) {
			let myAmitaFilterMetadata = new AmitaFilterMetadata(prop);
			//myAmitaFilterMetadata.key = prop;
			let filterMeta = $event.filters[prop];
			myAmitaFilterMetadata.value = [];
			if (Array.isArray(filterMeta)) {
				let haveFilterValue = false;
				for (let meta of filterMeta) {
					if (!AppTool.IsNullOrEmpty((meta as FilterMetadata).value)) {
						haveFilterValue = true;
						myAmitaFilterMetadata.value.push(meta);
					}
				}
				if (haveFilterValue) {
					this.MyAmitalLazyLoadEvent.filters.push(myAmitaFilterMetadata);
				}
			} else {
				myAmitaFilterMetadata.value.push(filterMeta);
				this.MyAmitalLazyLoadEvent.filters.push(myAmitaFilterMetadata);
			}
		}
	}

	addAdditionalFilter(
		FieldName: string,
		FieldValue: any,
		FieldValue2: any,
		FieldValue3: any,
		Operator: string,
		IsCustom: boolean,
		DisplayInList: boolean,
		IsCustomField: boolean,
		FieldDataType: string,
		IgnoreFilter: boolean = false,
		IsCacheOnClient: boolean = false,
		ForceEnableAdd: boolean = false,
	) {
		let params = new ApiQueryFiltersAddParams();
		params.FieldName = FieldName;
		params.FieldValue = FieldValue;
		params.FieldValue2 = FieldValue2;
		params.FieldValue3 = FieldValue3;
		params.Operator = Operator;
		params.IsCustom = IsCustom;
		params.DisplayInList = DisplayInList;
		params.IsCustomField = IsCustomField;
		params.FieldDataType = FieldDataType;
		params.IgnoreFilter = IgnoreFilter;
		params.IsCacheOnClient = IsCacheOnClient;
		params.ForceEnableAdd = ForceEnableAdd;
		this.pushAdditionalFilter(params);
		if (this.usePrimNG) {
			if (Operator == 'Between') {
				this.AddAmitaFilterMetadata(FieldName, FieldValue, 'LessThanOrEqual'.toLowerCase(), 'and');
				this.AddAmitaFilterMetadata(FieldName, FieldValue2, 'GreaterThanOrEqual'.toLowerCase(), 'and');
			} else {
				this.AddAmitaFilterMetadata(FieldName, FieldValue, Operator.toLowerCase(), 'and');
			}
		}

		// if (!IsCacheOnClient) {
		//     if (typeof (FieldValue) === "string") {
		//         if (FieldValue)
		//         FieldValue = this.myReplace(FieldValue);//FieldValue.replace('"', '\\"');
		//         if (FieldName != "ImportersFilter")
		//         FieldValue = encodeURIComponent(FieldValue)
		//         //FieldValue = FieldValue.replace("%22", "\%22");
		//     }
		//     if (typeof (FieldValue2) === "string") {
		//       if (FieldValue)
		//         FieldValue2 = this.myReplace(FieldValue2);//.replace('"', '\\"');
		//         FieldValue2 = encodeURIComponent(FieldValue2)
		//         //FieldValue = FieldValue.replace("%20", " ");
		//     }
		//     if (typeof (FieldValue3) === "string") {
		//         if (FieldValue)
		//           FieldValue3 = this.myReplace(FieldValue3);//FieldValue3.replace('"', '\\"');
		//         FieldValue3 = encodeURIComponent(FieldValue3)
		//         //FieldValue = FieldValue.replace("%20", " ");
		//     }
		// }
		// var existedItem = this.AdditionalFilters.find(d => d.FieldName == FieldName);
		// if (!existedItem || ForceEnableAdd) {
		//     var item = new FilterItem(FieldName, FieldValue, FieldValue2, FieldValue3, Operator, IsCustom, DisplayInList, IsCustomField, FieldDataType, IgnoreFilter, IsCacheOnClient);
		//     this.AdditionalFilters.push(item);
		// }
	}
	myReplace(myString: string) {
		var myNewString = '';
		for (var i = 0; i < myString.length; i++) {
			if (myString[i] == '"') {
				myNewString = myNewString + '\\"';
			} else {
				myNewString = myNewString + myString[i];
			}
		}
		return myNewString;
	}
	removeAdditionalFilter(FieldName: string) {
		var item = this.AdditionalFilters.filter((d) => d.FieldName == FieldName && d.IsLookUpfilter == true)[0];
		if (item) {
			var index = this.AdditionalFilters.indexOf(item);
			this.AdditionalFilters.splice(index, 1);
		}
	}

	pushAdditionalFilter(params: ApiQueryFiltersAddParams) {
		if (!params.IsCacheOnClient) {
			if (typeof params.FieldValue === 'string') {
				if (params.FieldValue) params.FieldValue = this.myReplace(params.FieldValue); //FieldValue.replace('"', '\\"');
				if (params.FieldName != 'ImportersFilter') params.FieldValue = encodeURIComponent(params.FieldValue);
				//FieldValue = FieldValue.replace("%22", "\%22");
			}
			if (typeof params.FieldValue2 === 'string') {
				if (params.FieldValue) params.FieldValue2 = this.myReplace(params.FieldValue2); //.replace('"', '\\"');
				params.FieldValue2 = encodeURIComponent(params.FieldValue2);
				//FieldValue = FieldValue.replace("%20", " ");
			}
			if (typeof params.FieldValue3 === 'string') {
				if (params.FieldValue) params.FieldValue3 = this.myReplace(params.FieldValue3); //FieldValue3.replace('"', '\\"');
				params.FieldValue3 = encodeURIComponent(params.FieldValue3);
				//FieldValue = FieldValue.replace("%20", " ");
			}
		}
		var existedItem = this.AdditionalFilters.find((d) => d.FieldName == params.FieldName);
		if (!existedItem || params.ForceEnableAdd) {
			var item = new FilterItem(
				params.FieldName,
				params.FieldValue,
				params.FieldValue2,
				params.FieldValue3,
				params.Operator,
				params.IsCustom,
				params.DisplayInList,
				params.IsCustomField,
				params.FieldDataType,
				params.IgnoreFilter,
				params.IsCacheOnClient,
				params.IsLookUpFilter,
			);
			this.AdditionalFilters.push(item);
		}
	}

	public queryId: string;
	public queryCode: string;

	//public tenant: number;
	public userid: string;
	public ObjectTableName: string;

	public Filter1Name: string;
	public Filter1Value: Object;
	public Filter1Operator: string;
	public Filter1Value2: Object;

	public Filter2Name: string;
	public Filter2Value: Object;
	public Filter2Operator: string;
	public Filter2Value2: Object;

	public Filter3Name: string;
	public Filter3Value: Object;
	public Filter3Operator: string;
	public Filter3Value2: Object;

	public Filter4Name: string;
	public Filter4Value: Object;
	public Filter4Operator: string;
	public Filter4Value2: Object;

	public Filter5Name: string;
	public Filter5Value: Object;
	public Filter5Operator: string;
	public Filter5Value2: Object;

	public Filter6Name: string;
	public Filter6Value: Object;
	public Filter6Operator: string;
	public Filter6Value2: Object;

	public Filter7Name: string;
	public Filter7Value: Object;
	public Filter7Operator: string;
	public Filter7Value2: Object;

	public Filter8Name: string;
	public Filter8Value: Object;
	public Filter8Operator: string;
	public Filter8Value2: Object;

	public Filter9Name: string;
	public Filter9Value: Object;
	public Filter9Operator: string;
	public Filter9Value2: Object;

	public Filter10Name: string;
	public Filter10Value: Object;
	public Filter10Operator: string;
	public Filter10Value2: Object;

	public GetAll: boolean;
}

export class FilterItem {
	constructor(
		public FieldName: string,
		public FieldValue: any,
		public FieldValue2: any,
		public FieldValue3: any,
		public Operator: string,
		public IsCustom: boolean,
		public DisplayInList: boolean,
		public IsCustomField: boolean,
		public FieldDataType: string,
		public IgnoreFilter: boolean,
		public IsCacheOnClient: boolean = false,
		public IsLookUpfilter: boolean = false,
	) {}
}

export class AmitalLazyLoadEvent {
	//constructor() { }
	GetCount: boolean;
	first?: number;
	rows?: number;
	sortField?: string;
	sortOrder?: number;
	filters?: AmitaFilterMetadata[];
	//MyLazyLoadEvent: lazyLoadEvent
}
export class AmitaFilterMetadata {
	constructor(public key: string) {}
	value: any[];
}

class OperetorConverter {
	/*
    
    C:\C21R01\Logitude\Simplog.Server.Infrastructure\Helpers\GenericFilter.cs(44):                                case "LargerThan":
    C:\C21R01\Logitude\Simplog.Server.Infrastructure\Helpers\GenericFilter.cs(88):                                case "GreaterThanOrEqual":
    C:\C21R01\Logitude\Simplog.Server.Infrastructure\Helpers\GenericFilter.cs(131):                                case "LessThan":
    C:\C21R01\Logitude\Simplog.Server.Infrastructure\Helpers\GenericFilter.cs(173):                                case "LessThanOrEqual":
    C:\C21R01\Logitude\Simplog.Server.Infrastructure\Helpers\GenericFilter.cs(215):                                case "StartsWith":
    C:\C21R01\Logitude\Simplog.Server.Infrastructure\Helpers\GenericFilter.cs(234):                                case "Contains":
    C:\C21R01\Logitude\Simplog.Server.Infrastructure\Helpers\GenericFilter.cs(276):                                case "InList":
    C:\C21R01\Logitude\Simplog.Server.Infrastructure\Helpers\GenericFilter.cs(323):                                case "InListExact":
    C:\C21R01\Logitude\Simplog.Server.Infrastructure\Helpers\GenericFilter.cs(374):                                //case "InListExact":
    C:\C21R01\Logitude\Simplog.Server.Infrastructure\Helpers\GenericFilter.cs(421):                                case "Between":
    C:\C21R01\Logitude\Simplog.Server.Infrastructure\Helpers\GenericFilter.cs(566):                                case "NotEqual":
    C:\C21R01\Logitude\Simplog.Server.Infrastructure\Helpers\GenericFilter.cs(603):                                case "Exclude":
    C:\C21R01\Logitude\Simplog.Server.Infrastructure\Helpers\GenericFilter.cs(647):                                case "IsNotNull":
    C:\C21R01\Logitude\Simplog.Server.Infrastructure\Helpers\GenericFilter.cs(666):                                case "IsNull":
    C:\C21R01\Logitude\Simplog.Server.Infrastructure\Helpers\GenericFilter.cs(686):                                case "InListInt":
    
     */

	public Converter(primengOperetor: string): string {
		const logiOperetorList = [
			'LargerThan',
			'GreaterThanOrEqual',
			'LessThan',
			'LessThanOrEqual',
			'StartsWith',
			'Contains',
			'InList',
			'InListExact',
			'Between',
			'NotEqual',
			'Exclude',
			'IsNotNull',
			'IsNull',
			'InListInt',
		];
		var lower = logiOperetorList.map((opr) => opr.toLowerCase());
		var indx = lower.findIndex((logi) => logi == primengOperetor.toLowerCase());
		if (indx == -1) {
			return '';
		}
		return logiOperetorList[indx];
	}
}
