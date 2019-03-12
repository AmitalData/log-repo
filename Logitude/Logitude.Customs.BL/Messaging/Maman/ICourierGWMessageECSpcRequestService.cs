namespace Logitude.Customs.BL.Messaging
{
    public interface ICourierGWMessageECSpcRequestService
    {
        string BuildQueueSendWebAPI(string declarationId, int tenant, MamanActionCodeUpdateOrCancel mamanActionCode, MamanSpecialCode mamanSpecialCode);
    }

    public enum MamanActionCodeUpdateOrCancel
    {
        Upsert,
        Cancel
    }
    public enum MamanSpecialCode
    {
        /// <summary>
        /// קליטת עיכוב (ללא ששודרה קודם השהיה)
        /// </summary>
        ReceivingDelayCertificate_DelayIt = 2,
        StickerPrinting = 4,
        PrintDocuments = 5,
        Sban = 6
        //2	קליטה תעודת עיכוב	2, קליטה תעודת עיכוב Receiving a delay certificate	0
        //4	הדפסת מדבקה	4, הדפסת מדבקה   Sticker Printing	0
        //5	הדפסת מסמכים	5, הדפסת מסמכים  Printing Documents	0
        //6	סב''ן	6, סב''ן  Sban	0
    }
}