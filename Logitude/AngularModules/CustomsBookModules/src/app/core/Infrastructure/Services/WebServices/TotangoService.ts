// import { AmitalGatewayUtil } from '../../Utilities/AmitalGatewayUtil';
import { SessionLocator } from '../../Utilities/SessionLocator';
import { ServiceHelper } from '../../Utilities/ServiceHelper';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';

@Injectable()
export class TotangoService {
	private _apiUrl: string;
	private _http: HttpClient;
	constructor() {
		this._http = ServiceHelper.HttpClient;
		this._apiUrl = ServiceHelper.GetLogitudeURL() + 'api/TotangoService';
	}

	public SendTotangoUserActivity(module: string, activity: string) {
		try {
			//if (ScriptableGatewayUtil.AmitalBrowserInUse) {
			//    var requset = new Dictionary<string, string>();
			//    requset.Add("module", module);
			//    requset.Add("activity", activity);
			//    ScriptableGatewayUtil.SendRequestToUnifreightAsync("", "", "SendTotangoUserActivity", new UnifreightMessageM() { Requset = requset }, "");
			//}
			// if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
			//     if (true) {
			//         AmitalGatewayUtil.Instance.SendTotangoUserActivity(module, activity);
			//     }
		} catch (e) {
			// if (!SessionLocator.LoggedUserPM.IsCustomerCare) {
			//     if (module == "Agent" || module == "CustomAgent" || module == "ShippingAgent" || module == "Customer" || module == "PotentialCustomer"
			//         || module == "Airline" || module == "Trucker" || module == "Shippingline") {
			//         module = "Card";
			//     }

			//     if (module == null) {

			//     }

			//     //string uri = App.Current.Host.Source.AbsoluteUri;
			//     //uri = uri.Replace("/ClientBin/Simplog.Infrastructure.xap", "/WebServices/TotangoService.asmx");
			//     //TotangoServiceSoapClient totangoService = new TotangoServiceSoapClient();
			//     //totangoService.Endpoint.Address = new System.ServiceModel.EndpointAddress(uri);
			//     //BasicHttpBinding binding = BindingInfo.GetBindingInfo();
			//     //if (totangoService.Endpoint.Address.Uri.Scheme == "https") {
			//     //    binding.Security.Mode = BasicHttpSecurityMode.Transport;
			//     //}
			//     //else {
			//     //    binding.Security.Mode = BasicHttpSecurityMode.None;
			//     //}

			//     //totangoService.Endpoint.Binding = binding;

			//     //(string organizationId, string orgDisplayName, string userName, string module, string activity, string contactId, int tenant, bool isSharedLogisticsContact, string cardId, string partnerTypeId)

			//     var data = new TotangoActivityInfo();
			//     data.UserName = SessionLocator.LoggedUserPM.EnglishName;
			//     data.Module = "(A) " + module;
			//     data.Activity = "(A) " + activity;
			//     data.ContactId = SessionLocator.LoggedUserId;
			//     data.Tenant = SessionLocator.TenantPM.Id;
			//     data.IsSharedLogisticsContact = false;

			//     var orgDisplayName = SessionLocator.TenantPM.Company + (SessionLocator.TenantPM.CountryName != null ? ("-" + SessionLocator.TenantPM.CountryName.trim()) : "");

			//     if (ObjectsLocator.IsDemoTenant(SessionLocator.TenantPM.Id.toString()) || SessionLocator.TenantPM.Id == 153) {
			//         orgDisplayName = SessionLocator.LoggedUserPM.Notes;
			//         data.OrganizationId = SessionLocator.LoggedUserId;

			//         //totangoService.SendUserActivityAsync(SessionInfo.LoggedUserId, orgDisplayName, SessionInfo.LoggedUserPM.EnglishName, module, activity, SessionInfo.LoggedUserId, InfraSettings.TenantPM.Id, false, null, null);

			//     }
			//     else {
			//         data.OrganizationId = SessionLocator.TenantPM.Id.toString();
			//         //totangoService.SendUserActivityAsync(InfraSettings.TenantPM.Id.ToString(), orgDisplayName, SessionInfo.LoggedUserPM.EnglishName, module, activity, SessionInfo.LoggedUserId, InfraSettings.TenantPM.Id, false, null, null);
			//     }

			//     data.OrgDisplayName = orgDisplayName;

			//     this._http.post(this._apiUrl, JSON.stringify(data), ServiceHelper.GetHttpHeaders()).pipe(map((response) => {

			//     }, (error) => {
			//         console.error(error);
			//     })).subscribe((response2) => {

			//     }, (error) => {
			//         console.error(error);
			//     });
			// }
			// }
			console.error(e);
		}
	}
}

export class TotangoActivityInfo {
	public OrganizationId: string;
	public OrgDisplayName: string;
	public UserName: string;
	public Module: string;
	public Activity: string;
	public ContactId: string;
	public Tenant: number;
	public IsSharedLogisticsContact: boolean;
	public CardId: string;
	public PartnerTypeId: string;
}
