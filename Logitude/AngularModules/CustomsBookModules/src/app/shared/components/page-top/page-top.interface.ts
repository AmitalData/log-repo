export interface GetFromTypesenseResponse {
	$id: string
	Remarks: RemarkWithCustomsItem[]
	CustomsItems: CustomsItem[]
}

export interface CustomsItem {
	$id: string
	CB_ID: string
	ID: number
	CustomsItemID: string
	FullClassification: string
	IsLeaf: boolean
	CustomsItemDetailsHistoryID: number
	PropertiesDetailsHistoryID: number
	PH_MeasurementUnitID: number
	IsHistoryExists: boolean
	IsRulesExists: boolean
	StartDate: string
	StartDateInt: number
	EndDate: string
	EndDateInt: number
	CI_Parent_CustomsItemIDNum: number
	CI_BaseFullClassification: string
	CI_ComputedCheckDigit: string
	CI_CustomsBookTypeIDNum: string
	CI_CustomsItemCategoryIDNum: string
	ItemHierarchicLocationID: string
	CIH_Title: string
	CIH_GoodsDescription: string
	CustomsItemEntityStatusIDNum: number
	PH_IsCarItem: boolean
	FullGoodsDescription: string
	BaseCustomsItemID: number
}

export interface RemarkWithCustomsItem {
	CustomsItem: CustomsItem
	Remark: Remark
}

export interface Remark {
	$id: string
	Id: string
	Tenant: number
	Drop_CB_ID: string
	CustomsItemsID: number
	RemarkDescription: string
}

export interface CustomsItemsAutocomplate {
	FullClassification: string;
	text: string;
	BaseCustomsItemID?: number;
}

export interface CustomsClassification { Classification: string, Description: string }

export interface CustomBookClassification {
	[classification: string]: CustomsClassification[];
}

export interface AllClassification {
	[customsBookType: string]: CustomBookClassification;
}
export type GroupedCustomsItems = {
	[classification: string]: CustomsItemsAutocomplate[]
};
