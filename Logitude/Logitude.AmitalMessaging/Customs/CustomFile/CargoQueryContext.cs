using System;
using Logitude.AmitalMessaging.Customs.CustomFile;
public partial class CargoQueryContext
{
    public string CargoData { get; set; }
    public string RaiseStatus { get; set; }
    public bool RequestAutoSend { get; set; }
    public CFIPACKS ResponseCFIPACKS { get; set; }
    public DateTime? StatusDate { get; set; }
    public FileAdditionalData FileAdditionalData { get; set; }
}
