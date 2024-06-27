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
	// GetCustomsBookMainView(filters: Filters) {
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
		const url = `${this._apiUrl}CB_CustomsItemExtended/GetCustomsBookMainViewSearchByClassification?CustomsBookType=${filters.CustomsBookType}&SkippedRows=${filters.SkippedRows}&PageSize=${filters.PageSize}&CustomsItemHierarchic=${filters.CustomsItemHierarchic}`;
		return this.Get(url);
	}
	
	GetCustomsBookMainViewSearchByText(filters: Filters) {
		const url = `${this._apiUrl}CB_CustomsItemExtended/GetCustomsBookMainViewSearchByText?SearchFields=${filters.SearchFields}&CustomsBookType= ${filters.CustomsBookType}&CustomsItemHierarchic= ${filters.CustomsItemHierarchic}&Reamarks= ${filters.Reamarks}&Rules= ${filters.Rules}&SkippedRows= ${filters.SkippedRows}&PageSize= ${filters.PageSize}`;
		return this.Get(url);
	}
	
	GetCustomsBookTaxRates(customsItemId: number) {
		const url = `${this._apiUrl}CB_TariffExtended/GetCustomsBookTaxRates?customsItemId=${customsItemId}`;
		return this.Get(url);
	}
}
