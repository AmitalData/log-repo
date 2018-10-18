using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.QuoteModel.EntityPOCOs;

namespace Simplog.Data.QuoteModel
{
    public interface IQuoteFollowUpDataViewContext
    {
        IDbSet<QuoteFollowUpDataView> QuoteFollowUpDataViews { get; }
        void SetAsModified(object entity);
        void DetectChanges();
        int SaveChanges();
    }
}
