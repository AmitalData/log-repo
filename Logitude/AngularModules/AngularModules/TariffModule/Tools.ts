import { TariffPM } from './EntityPMs/TariffPM';
import { TariffVersionPM } from './EntityPMs/TariffVersionPM';
import { SessionInfo } from '../Infrastructure/Utilities/SessionInfo';
import { DateTool } from '../Infrastructure/Tools';



export class TariffTool {

    public static IsSurchargeTariff(entityPM: TariffPM): boolean {
        var myResult: boolean = false;

        if (entityPM.TypeCode == "ASC" || entityPM.TypeCode == "OSC" || entityPM.TypeCode == "OFS") {
            myResult = true;
        }

        return myResult;
    }

    public static IsFreightTariff(entityPM: TariffPM): boolean {
        var myResult: boolean = false;

        if (entityPM.TypeCode == "AFC" || entityPM.TypeCode == "OLC" || entityPM.TypeCode == "OFC") {
            myResult = true;
        }

        return myResult;
    }

    public static CopyTariffVersion(tariff: TariffPM, oldVersion: TariffVersionPM): TariffVersionPM {
        var copiedVersion: TariffVersionPM = new TariffVersionPM(tariff);

        copiedVersion.TariffId = oldVersion.TariffId;
        copiedVersion.Version = tariff.LastVersion;
        copiedVersion.CreateDate = DateTool.GetCurrentDateAsUtc();
        copiedVersion.CreatedByUserId = SessionInfo.LoggedUserId;
        copiedVersion.IsDraft = true;
        copiedVersion.Tenant = SessionInfo.LoggedUserTenant;
        copiedVersion.ParentVersionNumber = oldVersion.Version;
        
        return copiedVersion;
    }
}
