import { DataProviderField } from './DataProviderField';

export class ExcelReportArguments {
    public DataProviderFields: DataProviderField[];
    public ReportId: string;
    public ReportsTemplateId: string;
    public IsNew: boolean;
}
