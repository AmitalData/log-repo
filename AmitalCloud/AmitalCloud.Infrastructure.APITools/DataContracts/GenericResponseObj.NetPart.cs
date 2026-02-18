namespace AmitalCloud.Infrastructure.APITools.DataContracts
{
    public partial class GenericResponseObj
    {
        public enum StatusEnum
        {
            TecinicalFailure,
            BusinessError,
            Success
        }
        StatusEnum _StatusType;

        public StatusEnum StatusType
        {
            get { return _StatusType; }
            set
            {
                _StatusType = value;
                //this.Status = _StatusType.ToString();
                switch (value)
                {
                    case StatusEnum.TecinicalFailure:
                        this.Status = "-500";
                        break;
                    case StatusEnum.BusinessError:
                        this.Status = "-700";
                        break;
                    case StatusEnum.Success:
                        this.Status = "0";
                        break;
                    default:
                        break;
                }

            }
        }


    }
}
