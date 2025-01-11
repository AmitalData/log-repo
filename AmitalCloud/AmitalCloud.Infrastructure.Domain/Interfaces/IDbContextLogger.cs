using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.Interfaces
{
    public interface IDbContextLogger : IDisposable
    {
        string ToString();
        void AddExplainLog(string ExplainLog);
        string ToString(int LastCharacter);
    }
}
