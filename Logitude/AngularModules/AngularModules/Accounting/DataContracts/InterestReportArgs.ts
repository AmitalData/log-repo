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
  public securityId:string;
  public Entities:any[];

}

export class  SelectItem {
  Id:string;
  SecurityId:string;
  TempId:string;
}
