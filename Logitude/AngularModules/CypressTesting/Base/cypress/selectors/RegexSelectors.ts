export class RegexSelectors {
   
    public static readonly CheckBoxLine= "label[id^='CheckBox_'][id$='" + "_LBL" + "']";
    public static readonly CheckBox='[id^="CheckBox"][id$="LBL"]'
    public static readonly SearchField="input[id^='SearchFieldsId_']"
    public static readonly MoreList= "div[id^='MenuButtons_']";
    public static readonly SaveAsOpenButton= "Button[id^='SendButtom_']";
    public static readonly NullSearch='input[id^="null_Search"]'
    public static readonly ComboBoxLast="div[id^=ComboBox_]:last"
}