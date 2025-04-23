using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace AmitalCloud.Infrastructure.Data.Queries
{
    public class ScreenQuery
    {
        private readonly Repository<Screen> repository;
        private readonly IAmitalCloudContext context;
        public ScreenQuery(int tenant)
        {
            context = AmitalCloudContext.GetContext(tenant);
            repository = new Repository<Screen>(context);
        }
        public List<ScreenPM> GetScreenPMsByTenant(int tenant)
        {
            List<ScreenPM> screens;
            List<ScreenPM> zeroscreens;
            List<ScreenPM> currentscreens;

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                zeroscreens = repository.GetMulti(a => a.Tenant == 0, a => new ScreenPM(a)
                {
                    ObjectTableName = a.ObjectTable.Name,
                    UserTenant = tenant,
                }, "ObjectTable");

                Dictionary<string, ScreenModification> screensDictionary = new Dictionary<string, ScreenModification>();
                screensDictionary = new Repository<ScreenModification>(context).GetMulti(te => te.Tenant == tenant).ToDictionary(dic => dic.ScreenId, dic => dic);

                foreach (ScreenPM screen in zeroscreens)
                {
                    if (screensDictionary.Keys.Contains(screen.Id))
                    {
                        ScreenModification mod = screensDictionary[screen.Id];
                        if (mod != null)
                        {
                            screen.NumberOfRows = mod.NumberOfRows;
                            screen.NumberOfColumns = mod.NumberOfColumns;
                        }
                    }
                }
            }

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                currentscreens = repository.GetMulti(a => a.Tenant == tenant, a => new ScreenPM(a)
                {
                    ObjectTableName = a.ObjectTable.Name,
                    UserTenant = tenant,
                }, "ObjectTable");
            }
            screens = zeroscreens.Concat(currentscreens).ToList();

            return screens;
        }
    }
}
