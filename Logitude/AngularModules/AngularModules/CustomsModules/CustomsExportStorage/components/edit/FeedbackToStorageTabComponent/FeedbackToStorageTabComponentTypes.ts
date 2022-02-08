export interface Exception {
  ExceptionLevel: string;
  ExeptionType: string;
  ExceptionParms: string;
  ExeptionDescription: string;
  EnglishDescription: string;
  BindingXpathField: string;
}

export interface UIMessage {
  $id: string;
  Code: string;
  EnglishName: string;
  Inactive: boolean;
  LocalName: string;
  SearchFields: string;
  Sort?: any;
}
