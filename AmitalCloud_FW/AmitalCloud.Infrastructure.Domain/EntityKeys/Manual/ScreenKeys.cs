using AmitalCloud.Infrastructure.Domain.BaseClasses;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityKeys
{
    public class ScreenKeys<T> : BaseEntityKeyFields<Screen, T>
    {
        public ScreenKeys() : base() { }
        public ScreenKeys(IEnumerable<KeyValuePair<string, string>> paramList) : base(paramList) { }
        public override void Initialize(IEnumerable<KeyValuePair<string, string>> paramList)
        {
            Id = (string)Convert.ChangeType((paramList.Single(t => t.Key == "Id").Value), typeof(string));
        }
        public string Id { get; set; }

        public override T GetFullKey() => (T)Convert.ChangeType(Id.ToString(), typeof(T));
        public override string GetEntityPMName() => "ScreensPM";
        public override Expression<Func<Screen, bool>> Predicate => a => a.Id == Id;
    }

}
