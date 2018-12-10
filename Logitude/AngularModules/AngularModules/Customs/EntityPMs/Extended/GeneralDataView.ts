import { AmendmentView, error  } from './AmendmentView';
export class GeneralDataView {
    CorrectionDate: Date;
    Version: string;
    AdditionalInformation: AdditionalInformationView[];
    AmendmentViews: AmendmentView[];
    SystemMessageViews: any;//error[];// AOT Compile error Cannot find name 'error'
}
export class AdditionalInformationView {
    StatmentName: string;
    Content: string;
}
