 import { AmendmentView, error  } from './AmendmentView';
export class GeneralDataView {
    CorrectionDate: Date;
    Version: string;
    AdditionalInformation: AdditionalInformationView[];
    AmendmentViews: AmendmentView[];
    SystemMessageViews: any;//error[];// AOT Compile error Cannot find name 'error'
    References: ReferenceView[];
}
export class AdditionalInformationView {
    StatmentName: string;
    Content: string;
}


export class ReferenceView {
    ReferenceTypeName: string;
    RefernceStatusName: string;
    RefernceID: string;
    Remarks: string;
    RefernceInputTypeName: string;
}
