
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
    public customOfficeNumber: number;
    public declerationStatus: number;
    public externalID?: number;
    public externalIDSpecified: boolean;
    public name: string;
    public statusName: string;
 
}
export class Deficit {
    public agentExternalID?: Number;
    public agentExternalIDSpecified: boolean;
    public agentName: string;
    public closeDate?: Date;
    public closeDateSpecified: boolean;
    public displayFileNumber: string;
    public estimatedBalance: number;
    public fileNumber: string;
    public Numeral: number;
    public productionDate?: Date;
    public productionDateSpecified: boolean;
    public status: number;
    public statusName: string;
    public totalRefundAmount?: number;
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

    public closeDate?: Date;
    public closeDateSpecified: boolean;

    public createDate: Date;

    public displayFileNumber: string;
    public fileNumber: string;

    public Numeral: number;
    public status: number;

    public statusName: string;

    public totalRefundAmount?: number;

    public totalRefundAmountSpecified: boolean;


}