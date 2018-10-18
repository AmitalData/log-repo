using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.GlobalModel.CustomFilters
{
    public class PaymentChannelCustomFilter
    {
        int tenant;
        public PaymentChannelCustomFilter(int tenant)
        {
            this.tenant = tenant;
        }

        public IQueryable<PaymentChannel> GetFilteredQuery(QueryOperations operations, IQueryable<PaymentChannel> queryableData)
        {
            return queryableData;
        }
    }
}
