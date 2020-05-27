"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var CustomsMenuItem = /** @class */ (function () {
    function CustomsMenuItem(TranslatedName, ScreenName, URLContent, WindowWidth, WindowHeight, MainInterfaceCode, DemoLogId, requestSheetState, objectTableName, SuppressMenuShow, CanExportExcel) {
        this.TranslatedName = TranslatedName;
        this.ScreenName = ScreenName;
        this.URLContent = URLContent;
        this.WindowWidth = WindowWidth;
        this.WindowHeight = WindowHeight;
        this.MainInterfaceCode = MainInterfaceCode;
        this.DemoLogId = DemoLogId;
        this.requestSheetState = requestSheetState;
        this.objectTableName = objectTableName;
        this.SuppressMenuShow = SuppressMenuShow;
        this.CanExportExcel = CanExportExcel;
        // this.TranslatedName = translatedName;
        // this.ScreenName = screenName;
    }
    return CustomsMenuItem;
}());
exports.CustomsMenuItem = CustomsMenuItem;
var RequestSheetState = /** @class */ (function () {
    function RequestSheetState(CustomSendOptionsButtonIsDisable, CustomRequestContentIsDisable, CustomResponseContentIsDisable
    //public DemoRequest: any, public DemoResponse: any,
    ) {
        this.CustomSendOptionsButtonIsDisable = CustomSendOptionsButtonIsDisable;
        this.CustomRequestContentIsDisable = CustomRequestContentIsDisable;
        this.CustomResponseContentIsDisable = CustomResponseContentIsDisable;
    }
    return RequestSheetState;
}());
exports.RequestSheetState = RequestSheetState;
//# sourceMappingURL=CustomsMenuItem.js.map