import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';

import { DateTool } from '../../Infrastructure/Tools';
import { BusinessProcessQueuePM } from '../EntityPMs/BusinessProcessQueuePM';

export class BusinessProcessQueuePMInitService {
	public static InitValues(entityPM: BusinessProcessQueuePM, isNew: boolean) {
		if (isNew) {
			var todayDate: Date = DateTool.GetCurrentDateTimeAsUtc();
			entityPM.Tenant = SessionLocator.Tenant;
			entityPM.CreateDate = DateTool.GetCurrentDateAsUtc();
			entityPM.CreatedByUserId = SessionLocator.LoggedUserId;
			entityPM.UpdateDate = DateTool.GetCurrentDateAsUtc();
			entityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
		}
	}

	public static ApplyUIPoperties(entityPM: BusinessProcessQueuePM, isNew: boolean) {}
}
