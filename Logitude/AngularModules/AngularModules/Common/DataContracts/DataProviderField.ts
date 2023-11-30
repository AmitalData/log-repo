export class DataProviderField {
    public Name: string;
    public Text: string;
    public Type: string;
    public Expression: string;
    public IsChecked: boolean;
    public Sort: number;
    public ClassName: string = "ListBoxItem";
    public FieldsOpened: boolean;
    public Fields: DataProviderField[];
    public DivSelectBackgroud: string;
}
