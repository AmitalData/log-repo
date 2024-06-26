import { Component, Input, SimpleChanges } from '@angular/core';
import { DataRowComponent } from '../data-row/data-row.component';
import { DetailsFrameComponent } from '../details-frame/details-frame.component';
import { TableTopComponent } from '../table-top/table-top.component';
import { AddCommentComponent } from '../add-comment/add-comment.component';
import { NgFor, NgForOf, NgIf } from '@angular/common';
import { trigger, style, animate, transition } from '@angular/animations';
//@ts-ignore
import { mockData } from '../../../../../mock_data';
import { API_MainService } from '../../../core/API_MainService';

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
	@Input() itemsData;
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

	ngOnInit() {
		// this.data = this.itemsData;

		// CHECK MOKE DATA:
		//this.data = this.orderedData(mockData);
		// this.item = this.data[0];

		
		this.API_MainService.GetCustomsBookMainView(new Filters()).subscribe((data: any) => {
			this.data = this.orderedData(data);
		});
	}

	ngOnChanges(changes: SimpleChanges) {
		if (changes['showDetails']) {
			this.showDetails = changes['showDetails'].currentValue;
		}
		if (changes['itemsData']) {
			this.itemsData = changes['itemsData'].currentValue;
		}		
	}

	showChildern(id: string): boolean {
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
				// @ts-ignore
				child.children = getChildren(child);
			});
			return children;
		};
		const rootItems = data.filter((item) => !item?.CI_Parent_CustomsItemIDNum);

		// CHECK MOKE DATA:
		//const rootItems = data.filter((item) => !item?.Parent_CustomsItemID);

		const orderedData = rootItems.map((rootItem) => {
			// debugger
			const children = getChildren(rootItem);
			return { ...rootItem, children };
		});
		
		return orderedData;
	};
	
}


export class Filters {
	CustomsBookType: string = "1";
	Tenant: number = 0;
	SearchFields: string | null = null;
	CustomsItemHierarchic: string | null = null;
	Reamarks: boolean = false;
	Rules: boolean = false;
}

export class CB_CustomsItemComputedDataList {
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
