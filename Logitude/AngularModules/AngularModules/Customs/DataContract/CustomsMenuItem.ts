export class CustomsMenuItem{

    constructor(
        public TranslatedName: string,public  ScreenName: string,
        public URLContent: string,
        public WindowWidth: number, public WindowHeight: number,
        public MainInterfaceCode: string,
        public DemoLogId?: string,
        public requestSheetState?: RequestSheetState,
        public objectTableName?: string,
        public SuppressMenuShow?: boolean,
        public CanExportExcel?: boolean

        
    ) {
        // this.TranslatedName = translatedName;
        // this.ScreenName = screenName;
    }
    // public ScreenName: string;
    // public TranslatedName: string;

}
export class RequestSheetState   {
    constructor(
        

        public CustomSendOptionsButtonIsDisable: boolean,
        public CustomRequestContentIsDisable: boolean,
        public CustomResponseContentIsDisable: boolean
        //public DemoRequest: any, public DemoResponse: any,
    )
    {}
}