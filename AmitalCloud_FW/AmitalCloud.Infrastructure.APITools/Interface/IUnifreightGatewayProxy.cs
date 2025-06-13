namespace AmitalCloud.Infrastructure.APITools.Interfaces
{
    public interface IUnifreightGatewayProxy
    {
        string GetLog();
        string GetAssemblyQualifiedName();
        string GetExampleDataIn1();
        string GetExampleDataIn2();
        string GetExampleDataout1();
        string GetExampleDataout2();
        void ProccessRequest(
            string DataIn1,
            string DataIn2,
            out string DataOut1,
            out string DataOut2,
            out string SUCCESS,
            ref string MoreParams,
            out string MessageOut);




        void ProccessBASE64Request(
                  string BASE64DataIn1,
                  string BASE64DataIn2,
            string BASE64DataIn3,
                  out string BASE64DataOut1,
                  out string BASE64DataOut2,
            out string BASE64DataOut3,
                  out string SUCCESS,
                  ref string MoreParams,
                  out string MessageOut
        );
        object MyUnity { get; set; }


    }

}
