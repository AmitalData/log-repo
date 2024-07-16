import { Component, Input, SimpleChanges } from '@angular/core';
import { DataRowComponent } from '../data-row/data-row.component';
import { DetailsFrameComponent } from '../details-frame/details-frame.component';
import { TableTopComponent } from '../table-top/table-top.component';
import { AddCommentComponent } from '../add-comment/add-comment.component';
import { NgFor, NgForOf, NgIf } from '@angular/common';
import { trigger, style, animate, transition } from '@angular/animations';
//@ts-ignore
import { mockData } from '../../../../../mock_data';
import { API_MainService, Filters } from '../../../core/API_MainService';
import { BehaviorSubject, filter } from 'rxjs';

@Component({
	selector: 'app-main-display',
	standalone: true,
	imports: [NgFor, NgForOf, NgIf, DataRowComponent, DetailsFrameComponent, TableTopComponent, AddCommentComponent],
	templateUrl: './main-display.component.html',
	styleUrl: './main-display.component.css',
	animations: [
		trigger('inOutAnimation', [
			transition(':enter', [
				style({ height: '0px', opacity: '0' }),
				animate('0.5s ease-in-out', style({ height: '*', opacity: '1' })),
			]),
			transition(':leave', [
				style({ height: '*', opacity: '1' }),
				animate('0.5s ease-in-out', style({ height: '0px', opacity: '0' })),
			]),
		]),
	],
})
export class MainDisplayComponent {
	@Input() showChiledren: boolean = false;
	@Input() itemsData: BehaviorSubject<CB_CustomsItemComputedDataList[]>;
	showDetails: boolean = false;
	showAddComment: boolean = false;
	showCommentSidebar: boolean = false;
	childrenToDesplay: string[] = [];
	private _filters;

	//data: any | never | undefined = {};
	data: CB_CustomsItemComputedDataList[] = [];

	KeyValue = Object.keys;
	Object: ObjectConstructor = Object;

	constructor(private API_MainService: API_MainService) { }

	cbTariffList: CB_TariffList[];
	cbRequirementComputedDataList: CB_RequirementComputedDataList[];

	ngOnInit() {
		this.InitData();
		this.ListenToItemsSearched();
	}

	InitData() {
		let filters: Filters = {
			CustomsBookType: '1',
			Tenant: 0,
			SearchFields: ''
		};

		this.API_MainService.GetCustomsBookMainView(filters).subscribe((data: CB_CustomsItemComputedDataList[]) => {
			this.data = this.orderedData(data);
		});
	}


	// listen to itemsData when change:
	ListenToItemsSearched() {
		this.itemsData.subscribe((data: CB_CustomsItemComputedDataList[] = []) => {
			if (data.length == 0) {
				this.InitData();
				return;
			} 
			console.log(data);
			// remove duplicates customsItemID:
			data = data.filter((v, i, a) => a.findIndex(t => (t.CustomsItemID === v.CustomsItemID)) === i);

			this.data = this.orderedData(data);

			this.extendAll(this.data);
			// this.data = this.orderedData(data);

			// const getChildren = (parentItem) => {
			// 	const children = data.filter((item) => item?.CI_Parent_CustomsItemIDNum === parentItem?.CustomsItemID);
			// 	// CHECK MOKE DATA:
			// 	//const children = data.filter((item) => item?.Parent_CustomsItemID === parentItem?.ID);

			// 	children.forEach((child) => {
			// 		child.children = getChildren(child);
			// 	});
			// 	return children;
			// };
			// // let rootItems = data;
			// // CHECK MOKE DATA:
			// //const rootItems = data.filter((item) => !item?.Parent_CustomsItemID);
			// const rootItems = data.filter((item) => !item?.IsLeaf);
			// if(rootItems.length == 0) return ;

			// const orderedData = rootItems.map((rootItem) => {

			// 	const children = getChildren(rootItem);
			// 	return { ...rootItem, children };
			// });
			// console.log(orderedData);
			// debugger
			// this.data = orderedData;



		});
	}
	extendAll(data: CB_CustomsItemComputedDataList[]) {
		data.forEach((item) => {
			this.showChildern(item.CIH_GoodsDescription, "open");
			if (item.children && item.children.length > 0) this.extendAll(item.children);
		});
	}


	currentItem: CB_CustomsItemComputedDataList;
	itemDataBehaviorSubject: BehaviorSubject<ItemData> = new BehaviorSubject<ItemData>({ customsItemId: 0, measurementUnitMalamId: 0 });
	itemData: ItemData = { customsItemId: 0, measurementUnitMalamId: 0 };
	showDetailsClick(CustomsItemID: number, item: CB_CustomsItemComputedDataList) {
		if (this.itemData.customsItemId == CustomsItemID) return;

		console.log(CustomsItemID);
		// console.log(item);
		// console.log(item.FullClassification);
		this.itemData.customsItemId = CustomsItemID;
		this.itemData.measurementUnitMalamId = 0; // change it
		this.itemDataBehaviorSubject.next(this.itemData);
		this.currentItem = item;
		return this.showDetails;
	}

	ngOnChanges(changes: SimpleChanges) {
		if (changes['showDetails']) {
			this.showDetails = changes['showDetails'].currentValue;
		}
		if (changes['itemsData']) {
			this.itemsData = changes['itemsData'].currentValue;
		}
	}

	showChildern(id: string, isCloseOrOpenAll?: string): boolean {
		
		if (isCloseOrOpenAll == "close") return false;
		if (isCloseOrOpenAll == "open") return true;

		const isShown = this.childrenToDesplay.indexOf(id);
		isShown === -1 ? this.childrenToDesplay.push(id) : this.childrenToDesplay.splice(isShown);
		return Boolean(isShown >= 0);
	}

	public orderedData = (data) => {
		const getChildren = (parentItem) => {
			const children = data.filter((item) => item?.CI_Parent_CustomsItemIDNum === parentItem?.CustomsItemID);
			// CHECK MOKE DATA:
			//const children = data.filter((item) => item?.Parent_CustomsItemID === parentItem?.ID);

			children.forEach((child) => {
				child.children = getChildren(child);
			});
			return children;
		};
		let rootItems = data.filter((item) => !item?.CI_Parent_CustomsItemIDNum);
		// CHECK MOKE DATA:
		//const rootItems = data.filter((item) => !item?.Parent_CustomsItemID);
		if (rootItems.length == 0) return;

		const orderedData = rootItems.map((rootItem) => {
			const children = getChildren(rootItem);
			return { ...rootItem, children };
		});

		return orderedData;
	};

}


export interface ItemData {
	customsItemId: number;
	measurementUnitMalamId: number;
}
export class MainEntity {
	CB_CustomsItemComputedDataList: CB_CustomsItemComputedDataList[];
	CB_TariffList: CB_TariffList[];
	CB_RequirementComputedDataList: CB_RequirementComputedDataList[];

	constructor(CB_CustomsItemComputedDataList: CB_CustomsItemComputedDataList[], CB_TariffList: CB_TariffList[], CB_RequirementComputedDataList: CB_RequirementComputedDataList[]) {
		this.CB_CustomsItemComputedDataList = CB_CustomsItemComputedDataList;
		this.CB_TariffList = CB_TariffList;
		this.CB_RequirementComputedDataList = CB_RequirementComputedDataList;
	}
}


export interface CB_CustomsItemComputedDataList {
	CB_ID: string;
	ID: number;
	CustomsItemID: number;
	FullClassification: string;
	IsLeaf: boolean;
	CustomsItemDetailsHistoryID: number;
	PropertiesDetailsHistoryID: number;
	PH_MeasurementUnitID?: number;
	IsHistoryExists: boolean;
	IsRulesExists: boolean;
	StartDate: Date;
	EndDate: Date;
	CI_Parent_CustomsItemIDNum?: number;
	CI_BaseFullClassification: string;
	CI_ComputedCheckDigit: string;
	CI_CustomsBookTypeIDNum: string;
	CI_CustomsItemCategoryIDNum: string;
	ItemHierarchicLocationID: string;
	CIH_Title: string;
	CIH_GoodsDescription: string;
	CustomsItemEntityStatusIDNum: number;
	PH_IsCarItem?: boolean;
	FullGoodsDescription: string;
	Agreements?: number;
	CustomsRate: string;
	PurchaseTax: string;
	OptionalTaxAddition?: number;
	MeasurementUnitName: string;
	Remarks: string;
	SearchByTextResult: string;
	children: CB_CustomsItemComputedDataList[];
}

export interface CB_RequirementComputedDataList {
	CB_ID: string;
	ID: number;
	RegularityRequirementID: number;
	CountryID?: number;
	CustomsItemID?: number;
	StartDate: Date;
	EndDate: Date;
	RegularitySourceCodeID: string;
	CreateDate?: Date;
	InceptionCodeID: string;
	RegularityPublicationCodeID: string;
	AutonomyCustomsItemID?: number;
	IsAllCustomsItems: boolean;
	RequirementValidOrigin: string;
	RequirementGoodsDescription: string;
	Authority: string;
	ConfirmationType: string;
	InterConditionsRelationship: string;
	TextualCondition: string;
	IsPersonalImportIncluded?: boolean;
	IsCarnetIncluded?: boolean;
	FromEpisodeDetail: string;
	AutonomyRegion: string;
}

export interface CB_TariffList {
	ID: number;
	CreateDate: Date;
	UpdateDate?: Date;
	TradeAgreementID?: number;
	CustomsItemID: number;
	Title: string;
	CB_ID: string;
	Country: string;
	CustomsRate: string;
	CustomsRateWithinQuota: string;
	QuotaID?: number;
	MeasurementUnitName: string;
	OptionalTaxAddition?: number;
	StartDate?: Date;
	EndDate?: Date;
	TradeAgreementName: string;
}
export interface RemarksClassificationPM {
	id?: string;
	tenant: number;
	customsItemsID: number;
	remarkDescription: string;
}
