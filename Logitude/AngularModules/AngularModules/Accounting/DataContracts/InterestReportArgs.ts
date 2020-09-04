export class InterestReportArguments {
  public AllSelected: boolean;
  public SelectedIds :string[];
  public SelectedItems :SelectItem[];
  public FromDate: Date;
  public ToDate: Date;
  public ExcludedIds: string[];
  public Tenant:number;
  public Email:string;
  public tempId:string;
  public InvoiceDate:Date;
  public securityId:string;
  public Entities:any[];
  public ShowPrintedInvoice:boolean;

}

export class  SelectItem {
  Id:string;
  SecurityId:string;
  TempId:string;
}
