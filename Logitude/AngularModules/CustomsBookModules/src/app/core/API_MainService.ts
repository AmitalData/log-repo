import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseService } from './Services/BaseService';
import { AppTool } from './Infrastructure/Tools';

export interface Filters {
	SearchFields?: string;
	CustomsBookType?: string;
	CustomsItemHierarchic?: string;
	Reamarks?: boolean;
	Rules?: boolean;
	SkippedRows?: number;
	PageSize?: number;
	Tenant?: number;
}

@Injectable({
	providedIn: 'root',
})
export class API_MainService extends BaseService {
	//private _apiUrl: string = 'http://localhost:9996/api/';
	private _apiUrl: string = AppTool.GetLogitudeURL() + "api/";

	constructor(httpClient: HttpClient) {
		super(httpClient);
		// this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/ShipmentDomain';
		// this.ApiURL = this.BaseURL + 'api/ShipmentDomain';
	}

	AddNEWRemarksClassification(data) {
		const url = `${this._apiUrl}CB_CustomsItemExtended/AddNEWRemarksClassification`;
		// const url = `${this._apiUrl}RemarksClassifications/Post`;
		return this.Post(url, data);
	}

	EditRemarksClassification(data) {
		const url = `${this._apiUrl}CB_CustomsItemExtended/EditRemarksClassification`;
		return this.Post(url, data);
  }

	DeleteRemarksClassification(data) {
		const url = `${this._apiUrl}CB_CustomsItemExtended/DeleteRemarksClassification`;
		return this.Post(url, data);
	}
	
	GetCustomsBookMainView(filters: Filters) {
		const url = `${this._apiUrl}CB_CustomsItemExtended/GetCustomsBookMainView?customsBookType=${filters.CustomsBookType}&Tenant=${filters.Tenant ? filters.Tenant : 0}`;
		return this.Get(url);
	}


	GetCustomsBookAgreementLevelData(customsItemId: number, measurementUnitMalamId: number) {
		const url = `${this._apiUrl}CB_TariffExtended/GetCustomsBookAgreementLevelData?customsItemId=${customsItemId}&measurementUnitMalamId=${measurementUnitMalamId}`;
		return this.Get(url);
	}

	GetCustomsBookRegularityRequirementData(customsItemId: number) {
		const url = `${this._apiUrl}CB_TariffExtended/GetCustomsBookRegularityRequirementData?customsItemId=${customsItemId}`;
		return this.Get(url);
	}


	GetCustomsBookMainViewSearchByClassification(filters: Filters) {
		const url = `${this._apiUrl}CB_CustomsItemExtended/GetCustomsBookMainViewSearchByClassification`;
		return this.Post(url, filters);
	}

	GetCustomsBookMainViewSearchByText(filters: Filters) {
		const url = `${this._apiUrl}CB_CustomsItemExtended/GetCustomsBookMainViewSearchByText`;
		return this.Post(url, filters);
	}

	GetCustomsBookTaxRates(customsItemId: number) {
		const url = `${this._apiUrl}CB_TariffExtended/GetCustomsBookTaxRates?customsItemId=${customsItemId}`;
		return this.Get(url);
	}
	GetAllCommentsByCustomsItemId(customsItemId: number, tenant: number) {
		const url = `${this._apiUrl}CB_CustomsItemExtended/GetAllCommentsByCustomsItemId?customsItemId=${customsItemId}&tenant=${tenant}`;
		return this.Get(url);
	}

	GetFromTypesense(searchValue: string, customsBookType: string, tenant: number) {
		const url = `${this._apiUrl}CB_CustomsItemExtended/GetFromTypesense?searchValue=${searchValue}&customsBookType=${customsBookType}&tenant=${tenant}`;
		return this.Get(url);
	}
}
