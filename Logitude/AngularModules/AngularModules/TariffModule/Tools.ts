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

    public static IsCustomsTariff(entityPM: TariffPM): boolean {
        var myResult: boolean = false;

        if (entityPM.TypeCode == "ICC" || entityPM.TypeCode == "ECC") {
            myResult = true;
        }

        return myResult;
    }
    public static IsLocalCustomsTariff(entityPM: TariffPM): boolean {
        var myResult: boolean = false;

        if (entityPM.TypeCode == "ICS" || entityPM.TypeCode == "ECS") {
            myResult = true;
        }

        return myResult;
    }
    public static IsInlandFTLTariff(entityPM: TariffPM): boolean {
        var myResult: boolean = false;

        if (entityPM.TypeCode == "IFT") {
            myResult = true;
        }

        return myResult;
    }

    public static IsTariffHasContainers(entityPM: TariffPM): boolean {
        var myResult: boolean = false;

        if (entityPM.TypeCode == "OFC" || entityPM.TypeCode == "OFS" || entityPM.TypeCode == "IFT") {
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
