import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';

import { DateTool } from '../../Infrastructure/Tools';
import { TeamPM } from '../EntityPMs/TeamPM';

export class TeamPMInitService {
	public static InitValues(entityPM: TeamPM, isNew: boolean) {
		if (isNew) {
			var todayDate: Date = DateTool.GetCurrentDateTimeAsUtc();
			entityPM.Tenant = SessionLocator.Tenant;
			entityPM.CreateDate = DateTool.GetCurrentDateAsUtc();
			entityPM.CreatedByUserId = SessionLocator.LoggedUserId;
			entityPM.UpdateDate = DateTool.GetCurrentDateAsUtc();
			entityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
		}
	}

	public static ApplyUIPoperties(entityPM: TeamPM, isNew: boolean) {}
}
