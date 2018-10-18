import { RequestParamsBase } from './RequestParamsBase';

export class MorningMessageRequestParams extends RequestParamsBase {
   
    public FromDate? : Date;
    public ToDate?  : Date;
    public SubjectText: string;
    public ContentText: string;
    public RecepientType: string;
    public Category: string;

}
