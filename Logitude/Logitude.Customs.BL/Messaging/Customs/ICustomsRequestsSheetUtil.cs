using System;
namespace Logitude.Customs.BL.Messaging.Customs
{
    interface ICustomsRequestsSheetUtil<MyType>
    {
        System.IO.MemoryStream Serialize(MyType MyObject);
    }
}
