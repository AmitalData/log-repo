
import { ResponseDataBase } from './ResponseDataBase';

export class DeclarationFilterResponseData extends ResponseDataBase {
    public ResponseStatusXML: string;
    public DeclarationID: string;

    public GeneralDetailsData: GeneralDetails;
    public ClaimList: Claim[];
    public DeficitList: Deficit[];
    public GuarenteeList: Guarantee[];

}

export class GeneralDetails {
    public customOfficeName: string;
    public customOfficeNumber: string;
    public declerationStatus: string;
    public externalID: string;
    public externalIDSpecified: boolean;
    public name: string;
    public statusName: string;

}

export class Deficit {
    public agentExternalID?: Number;
    public agentExternalIDSpecified: boolean;
    public agentName: string;
    public closeDate: string;
    public closeDateSpecified: boolean;
    public displayFileNumber: string;
    public estimatedBalance: string;
    public fileNumber: string;
    public Numeral: string;
    public productionDate: string;
    public productionDateSpecified: boolean;
    public status: number;
    public statusName: string;
    public totalRefundAmount: string;
    public totalRefundAmountSpecified: boolean;
}

export class Guarantee {
    public agentExternalID?: number;
    public agentExternalIDSpecified: boolean;
    public agentName: string;
    public Amount: number;
    public displayFileNumber: string;
    public fileNumber: string;
    public fileType: number;
    public fileTypeName: string;
    public guaranteeStatus: number;
    public guaranteeStatusName: string;
    public Numeral: number;
    public validity: Date;

}

export class Claim {


    public agentExternalID?: number;
    public agentExternalIDSpecified: boolean;
    public agentName: string;

    public claimAmount?: number;
    public claimAmountSpecified: boolean;

    public closeDate: string;
    public closeDateSpecified: boolean;

    public createDate: string;

    public displayFileNumber: string;
    public fileNumber: string;

    public Numeral: string;
    public status: number;

    public statusName: string;

    public totalRefundAmount: string;

    public totalRefundAmountSpecified: boolean;


}
