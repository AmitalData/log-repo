import { AmendmentView, error  } from './AmendmentView';
export class GeneralDataView {
    CorrectionDate: Date;
    Version: string;
    AdditionalInformation: AdditionalInformationView[];
    AmendmentViews: AmendmentView[];
    SystemMessageViews: error[];
}
export class AdditionalInformationView {
    StatmentName: string;
    Content: string;
}
