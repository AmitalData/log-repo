import { Injectable } from '@angular/core';
import { HttpClient, HttpEvent } from '@angular/common/http';
import { BaseService } from './Services/BaseService';
import { AppTool } from './Infrastructure/Tools';
import { Observable } from 'rxjs';

export interface Filters {
	SearchFields?: string;
	CustomsBookType?: string;
	CustomsItemHierarchic?: string;
	Reamarks?: boolean;
	Rules?: boolean;
	SkippedRows?: number;
	PageSize?: number;
	Tenant?: number;
	IsDiscountCodes?: boolean;
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

	GetCustomsBookLastUpdateDateByTenant(tenant: number) {
		const url = `${this._apiUrl}CB_CustomsItemExtended/GetCustomsBookLastUpdateDateByTenant?tenant=${tenant}`;
		return this.Get(url);
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

	GetDefaultCB_CollapseSearchHierarchy(tenant: number) {
		const url = `${this._apiUrl}CB_CustomsItemExtended/GetDefaultCB_CollapseSearchHierarchy?tenant=${tenant}`;
		return this.Get(url);
	}


	GetCustomsBookMainView(filters: Filters) {
		const url = `${this._apiUrl}CB_CustomsItemExtended/GetCustomsBookMainView?customsBookType=${filters.CustomsBookType}&Tenant=${filters.Tenant ? filters.Tenant : 0}&IsDiscountCodes=${filters.IsDiscountCodes}`;
		return this.Get(url);
	}


	GetTenantFromCustomsSettings() {
		const url = `${this._apiUrl}CB_CustomsItemExtended/GetTenantFromCustomsSettings`;
		return this.Get(url);
	}
	GetCustomItemClassifGuidance(customsItemId: number, tenant: number) {
		const url = `${this._apiUrl}CB_CustomsItemExtended/GetCustomItemClassifGuidance?customsItemId=${customsItemId}&tenant=${tenant}`;
		return this.Get(url);
	}
	GetClassifGuidanceDetails(classificationGuidanceNumber: string, tenant: number) {
		const url = `${this._apiUrl}CB_CustomsItemExtended/GetClassifGuidanceDetails?classificationGuidanceNumber=${classificationGuidanceNumber}&tenant=${tenant}`;
		return this.Get(url);
	}

	GetMekachDetails(customsItemId: number, tenant: number) {
		const url = `${this._apiUrl}CB_CustomsItemExtended/GetMekachDetails?customsItemId=${customsItemId}&tenant=${tenant}`;
		return this.Get(url);
	}

	GetMekachDocument(documentId: number, tenant: number) {
		const url = `${this._apiUrl}CB_CustomsItemExtended/GetMekachDocument?documentId=${documentId}&tenant=${tenant}`;
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

	GetCustomsBookRulesData(customsItemId: number) {
		const url = `${this._apiUrl}CB_RuleClassificationExtended/GetCustomsBookRulesData?customsItemId=${customsItemId}`;
		return this.Get(url);
	}

	GetAllCustomsBookRulesData() {
		const url = `${this._apiUrl}CB_RuleClassificationExtended/GetAllCustomsBookRulesData`;
		return this.Get(url);
	}

	GetCustomsBookMainViewSearchByClassification(filters: Filters) {
		// if (!this.checkIsFeaturePermessionCustomsBook()) return;
		const url = `${this._apiUrl}CB_CustomsItemExtended/GetCustomsBookMainViewSearchByClassification`;
		return this.Post(url, filters);
	}

	GetCustomsBookMainViewSearchByText(filters: Filters) {
		// if (!this.checkIsFeaturePermessionCustomsBook()) return;
		const url = `${this._apiUrl}CB_CustomsItemExtended/GetCustomsBookMainViewSearchByText`;
		return this.Post(url, filters);
	}


	GetCustomsBookTaxRates(customsItemId: number) {
		const url = `${this._apiUrl}CB_TariffExtended/GetCustomsBookTaxRates?customsItemId=${customsItemId}`;
		return this.Get(url);
	}

	GetAllComments(tenant: number) {
		const url = `${this._apiUrl}CB_CustomsItemExtended/GetAllComments?tenant=${tenant}`;
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

	GetClassifications(): Observable<HttpEvent<Object>> {
		const url = `${this._apiUrl}CB_CustomsItemExtended/GetClassifications`;
		return this.Get(url);
	}

	GetCB_PreferenceByUserIdAndTenant(userId: string, tenant: number) {
		const url = `${this._apiUrl}CB_Preference/GetCB_PreferenceByUserIdAndTenant?userId=${userId}&tenant=${tenant}`;
		return this.Get(url);
	}

	AddNewCB_Preference(data) {
		const url = `${this._apiUrl}CB_Preference/AddNewCB_Preference`;
		return this.Post(url, data);
	}
	AddNewAllCB_Preferences(data) {
		const url = `${this._apiUrl}CB_Preference/AddNewAllCB_Preferences`;
		return this.Post(url, data);
	}

	EditCB_Preference(data) {
		const url = `${this._apiUrl}CB_Preference/EditCB_Preference`;
		return this.Post(url, data);
	}

	EditAllCB_Preferences(data) {
		const url = `${this._apiUrl}CB_Preference/EditAllCB_Preferences`;
		return this.Post(url, data);
	}

	DeleteCB_Preference(data) {
		const url = `${this._apiUrl}CB_Preference/DeleteCB_Preference`;
		return this.Post(url, data);
	}

	DeleteAllCB_Preferences(data) {
		const url = `${this._apiUrl}CB_Preference/DeleteAllCB_Preferences`;
		return this.Post(url, data);
	}
}
