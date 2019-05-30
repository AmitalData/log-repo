using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;
namespace Logitude.Customs.Data
{
    public partial class CustomContext : DbContextBase, ICustomContext
    {
        

        public IDbSet<Card> Cards { get; set; }
        
        
    }
    public partial interface ICustomContext : IContext
    {
        
        IDbSet<Card> Cards { get; set; }
        
    }
}
