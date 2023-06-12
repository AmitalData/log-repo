import { CustomSendOptionsArgs, SendRequestVIA } from "Customs/DataContract/RequestParams/RequestParamsBase";
import { CustomsRequestsSheetPM } from "Customs/EntityPMs/CustomsRequestsSheetPM";
import { CustomsRequestSheetExtendedPMService } from "Customs/Services/ExtendedPMs/CustomsRequestSheetExtendedPMService";


export class RequestConflictService {
    static async runInBackground(customSendOptionsArgs: CustomSendOptionsArgs, tenant: number, customFileNo: string, ...interfaceTypeCodeArr: string[]): Promise<CustomSendOptionsArgs> {
        if ((customSendOptionsArgs.RequestVIA === SendRequestVIA.WebServiceInteractive ||
            customSendOptionsArgs.RequestVIA === SendRequestVIA.Default) &&
            await this.checkRequestsInProgress(tenant, customFileNo, interfaceTypeCodeArr)) {

            customSendOptionsArgs.Option = 'WB';
            customSendOptionsArgs.RequestVIA = SendRequestVIA.WebServiceBatch;
        }

        return customSendOptionsArgs;
    }

    private static async checkRequestsInProgress(tenant: number, customFileNo: string, interfaceTypeCodes: string[]): Promise<boolean> {
        const processRuns: boolean[] = await Promise.all(interfaceTypeCodes.map(async interfaceTypeCode => await this.checkRequestInProgress(interfaceTypeCode, tenant, customFileNo)))
        return processRuns.some(x=> x);
    }

    public static async checkRequestInProgress(interfaceTypeCode: string, tenant: number, customFileNo: string): Promise<boolean> {
        const response: CustomsRequestsSheetPM[] = await new CustomsRequestSheetExtendedPMService().getRequestsInProgress(interfaceTypeCode, tenant, false, '', '','','',customFileNo);
        return !!response?.length;
    }
}

export const interfaceTypeCodes = {
    exportStorage: '2791',
    exportSubmit: '2755E',
    declarationStatus: '8250',
}