import { FeatureLocator } from '../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';

import { DateTool } from '../../Infrastructure/Tools';
import { BusinessRolePM } from '../EntityPMs/BusinessRolePM';

export class BusinessRolePMInitService {
	public static InitValues(entityPM: BusinessRolePM, isNew: boolean) {
		if (isNew) {
			var todayDate: Date = DateTool.GetCurrentDateTimeAsUtc();
			entityPM.Tenant = SessionLocator.Tenant;
			entityPM.CreateDate = DateTool.GetCurrentDateAsUtc();
			entityPM.CreatedByUserId = SessionLocator.LoggedUserId;
			entityPM.UpdateDate = DateTool.GetCurrentDateAsUtc();
			entityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
		}
	}

	public static ApplyUIPoperties(entityPM: BusinessRolePM, isNew: boolean) {}
}
