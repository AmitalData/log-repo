import { AmendmentView } from './AmendmentView';
export class GeneralDataView {
    CorrectionDate: Date;
    Version: string;
    AdditionalInformation: AdditionalInformationView[];
    AmendmentViews: AmendmentView[];
}
export class AdditionalInformationView {
    StatmentName: string;
    Content: string;
}