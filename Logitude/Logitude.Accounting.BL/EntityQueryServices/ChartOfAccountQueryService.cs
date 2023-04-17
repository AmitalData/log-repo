using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.BL.EntityUpdateServices;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class ChartOfAccountQueryService : EntityQueryService<ChartOfAccount, ChartOfAccountKeys, ChartOfAccountPM, object, ChartOfAccountKeys>
    {
        public bool CheckWhetherCodeExists(string code, string id, int tenant)
        {
            return this.repository.CheckWhetherCodeExists(code, id, tenant);
        }

        public IEnumerable<ChartOfAccount> GetChartOfAccountChildRecursive(string id, int tenant, IEnumerable<ChartOfAccount> all = null)
        {
            all = all ?? this.repository.GetAll(tenant).ToList();
            return all.Where(x => x.ParentId == id || x.Id == id)
                .Union(
                all.Where(x => x.ParentId == id)
                .SelectMany(y => GetChartOfAccountChildRecursive(y.Id, tenant, all)));
        }

        public IQueryable<ChartOfAccount5LevelM> GetQChartOfAccount5LevelM(int tenant, List<string> chartOfAccountsTypes, bool topMostOnly = false)
        {
            var qBase = this.repository.GetAll(tenant);
            if (chartOfAccountsTypes != null)
            {
                qBase = qBase.Where(coa => chartOfAccountsTypes.Contains(coa.TypeCode));
            }
            var qL1 = qBase
            .Where(chartOA => chartOA.ParentId == null)
                .Select(chartL1 => new ChartOfAccount5LevelM()
                {
                    Tenant = chartL1.Tenant,
                    Level1Id = chartL1.Id,
                    Level1Name = chartL1.LocalName,
                    Level1Code = chartL1.Code,

                    Level2Id = "",
                    Level2Name = "",
                    Level2Code = "",

                    Level3Id = "",
                    Level3Name = "",
                    Level3Code = "",

                    Level4Id = "",
                    Level4Name = "",
                    Level4Code = "",

                    Level5Id = "",
                    Level5Name = "",
                    Level5Code = "",

                    GLAccountId = "",
                    GLAccountName = "",
                    GLAccountNumber = "",
                    ChartOfAccountId = "",
                    ChartOfAccountTypeCode = chartL1.TypeCode,

                    LeafId = chartL1.Id,
                });
            if (topMostOnly)
            {
                return qL1;
            }
            var qL2 =
                (from currChartL in qBase
                 join lastChartL in qL1
                 on currChartL.ParentId equals lastChartL.LeafId
                 select
                 new ChartOfAccount5LevelM()
                 {
                     Tenant = lastChartL.Tenant,
                     Level1Id = lastChartL.Level1Id,
                     Level1Name = lastChartL.Level1Name,
                     Level1Code = lastChartL.Level1Code,

                     Level2Id = currChartL.Id,
                     Level2Name = currChartL.LocalName,
                     Level2Code = currChartL.Code,

                     Level3Id = "",
                     Level3Name = "",
                     Level3Code = "",

                     Level4Id = "",
                     Level4Name = "",
                     Level4Code = "",

                     Level5Id = "",
                     Level5Name = "",
                     Level5Code = "",

                     GLAccountId = "",
                     GLAccountName = "",
                     GLAccountNumber = "",
                     ChartOfAccountId = "",
                     ChartOfAccountTypeCode = currChartL.TypeCode,
                     //currChartL.ChartOfAccountsTypeCode,

                     LeafId = currChartL.Id,
                 });

            var qL3 =
                    (from currentChartLevel in qBase
                     join lastChartLevel in qL2
                     on currentChartLevel.ParentId equals lastChartLevel.LeafId
                     select
                     new ChartOfAccount5LevelM()
                     {
                         Tenant = lastChartLevel.Tenant,
                         Level1Id = lastChartLevel.Level1Id,
                         Level1Name = lastChartLevel.Level1Name,
                         Level1Code = lastChartLevel.Level1Code,

                         Level2Id = lastChartLevel.Level2Id,
                         Level2Name = lastChartLevel.Level2Name,
                         Level2Code = lastChartLevel.Level2Code,

                         Level3Id = currentChartLevel.Id,
                         Level3Name = currentChartLevel.LocalName,
                         Level3Code = currentChartLevel.Code,

                         Level4Id = lastChartLevel.Level4Id,
                         Level4Name = lastChartLevel.Level4Name,
                         Level4Code = lastChartLevel.Level4Code,

                         Level5Id = lastChartLevel.Level5Id,
                         Level5Name = lastChartLevel.Level5Name,
                         Level5Code = lastChartLevel.Level5Code,

                         GLAccountId = "",
                         GLAccountName = "",
                         GLAccountNumber = "",
                         ChartOfAccountId = "",
                         ChartOfAccountTypeCode = currentChartLevel.TypeCode,
                         //currentChartLevel.ChartOfAccountsTypeCode,

                         LeafId = currentChartLevel.Id,
                     });


            var qL4 =
                   (from currentChartLevel in qBase
                    join lastChartLevel in qL3
                    on currentChartLevel.ParentId equals lastChartLevel.LeafId
                    select
                    new ChartOfAccount5LevelM()
                    {
                        Tenant = lastChartLevel.Tenant,
                        Level1Id = lastChartLevel.Level1Id,
                        Level1Name = lastChartLevel.Level1Name,
                        Level1Code = lastChartLevel.Level1Code,

                        Level2Id = lastChartLevel.Level2Id,
                        Level2Name = lastChartLevel.Level2Name,
                        Level2Code = lastChartLevel.Level2Code,

                        Level3Id = lastChartLevel.Level3Id,
                        Level3Name = lastChartLevel.Level3Name,
                        Level3Code = lastChartLevel.Level3Code,

                        Level4Id = currentChartLevel.Id,
                        Level4Name = currentChartLevel.LocalName,
                        Level4Code = currentChartLevel.Code,


                        Level5Id = lastChartLevel.Level5Id,
                        Level5Name = lastChartLevel.Level5Name,
                        Level5Code = lastChartLevel.Level5Code,

                        GLAccountId = "",
                        GLAccountName = "",
                        GLAccountNumber = "",
                        ChartOfAccountId = "",
                        ChartOfAccountTypeCode = currentChartLevel.TypeCode,
                        //currentChartLevel.ChartOfAccountsTypeCode,

                        LeafId = currentChartLevel.Id,
                    });


            var qL5 =
                 (from currentChartLevel in qBase
                  join lastChartLevel in qL4
                  on currentChartLevel.ParentId equals lastChartLevel.LeafId
                  select
                  new ChartOfAccount5LevelM()
                  {
                      Tenant = lastChartLevel.Tenant,
                      Level1Id = lastChartLevel.Level1Id,
                      Level1Name = lastChartLevel.Level1Name,
                      Level1Code = lastChartLevel.Level1Code,

                      Level2Id = lastChartLevel.Level2Id,
                      Level2Name = lastChartLevel.Level2Name,
                      Level2Code = lastChartLevel.Level2Code,

                      Level3Id = lastChartLevel.Level3Id,
                      Level3Name = lastChartLevel.Level3Name,
                      Level3Code = lastChartLevel.Level3Code,

                      Level4Id = lastChartLevel.Level4Id,
                      Level4Name = lastChartLevel.Level4Name,
                      Level4Code = lastChartLevel.Level4Code,

                      Level5Id = currentChartLevel.Id,
                      Level5Name = currentChartLevel.LocalName,
                      Level5Code = currentChartLevel.Code,

                      GLAccountId = "",
                      GLAccountName = "",
                      GLAccountNumber = "",
                      ChartOfAccountId = "",
                      ChartOfAccountTypeCode = currentChartLevel.TypeCode,
                      //currentChartLevel.ChartOfAccountsTypeCode

                      LeafId = currentChartLevel.Id,
                  });
            return qL1.Union(qL2).Union(qL3).Union(qL4).Union(qL5);
        }

        public List<ChartOfAccount5LevelM> GetChartOfAccount5LevelM(int tenant
            //, IEnumerable<ChartOfAccount> all = null
            )
        {
            ChartOfAccount5LevelM newChartOfAccount5LevelM = null;
            var listOfChartOfAccount5LevelM = new List<ChartOfAccount5LevelM>();
            var all = //all ?? 
                this.repository.GetAll(tenant)
                //.ToList();
                ;
            var lookUp = all.ToLookup(r => r.ParentId);

            var allPrim = lookUp.Where(g => g.Key == null);
            foreach (var item0Group in allPrim)
            {
                foreach (var item1 in item0Group)
                {
                    listOfChartOfAccount5LevelM.Add(
                        new ChartOfAccount5LevelM()
                        {
                            Tenant = item1.Tenant,
                            ChartOfAccountTypeCode = item1.TypeCode,
                            Level1Id = item1.Id,
                            Level1Name = item1.LocalName,
                            LeafId = item1.Id,
                            Level1Code = item1.Code,
                        });
                    var item1Look = lookUp.Where(g => g.Key == item1.Id);
                    foreach (var item2G in item1Look)
                    {
                        foreach (var item2 in item2G)
                        {
                            listOfChartOfAccount5LevelM.Add(
                            new ChartOfAccount5LevelM()
                            {
                                Tenant = item1.Tenant,
                                ChartOfAccountTypeCode = item1.TypeCode,
                                Level1Id = item1.Id,
                                Level1Name = item1.LocalName,
                                Level1Code = item1.Code,

                                Level2Id = item2.Id,
                                Level2Name = item2.LocalName,
                                LeafId = item2.Id,
                                Level2Code = item2.Code,
                            });


                            var item2Look = lookUp.Where(g => g.Key == item2.Id);
                            foreach (var item3G in item2Look)
                            {
                                foreach (var item3 in item3G)
                                {
                                    listOfChartOfAccount5LevelM.Add(
                                    new ChartOfAccount5LevelM()
                                    {
                                        Tenant = item1.Tenant,
                                        ChartOfAccountTypeCode = item1.TypeCode,
                                        Level1Id = item1.Id,
                                        Level1Name = item1.LocalName,
                                        Level1Code = item1.Code,

                                        Level2Id = item2.Id,
                                        Level2Name = item2.LocalName,
                                        Level2Code = item2.Code,

                                        Level3Id = item3.Id,
                                        Level3Name = item3.LocalName,
                                        LeafId = item3.Id,
                                        Level3Code = item3.Code,
                                    });


                                    var item3Look = lookUp.Where(g => g.Key == item3.Id);
                                    foreach (var item4G in item3Look)
                                    {
                                        foreach (var item4 in item4G)
                                        {
                                            listOfChartOfAccount5LevelM.Add(
                                            new ChartOfAccount5LevelM()
                                            {
                                                Tenant = item1.Tenant,
                                                ChartOfAccountTypeCode = item1.TypeCode,
                                                Level1Id = item1.Id,
                                                Level1Name = item1.LocalName,
                                                Level1Code = item1.Code,

                                                Level2Id = item2.Id,
                                                Level2Name = item2.LocalName,
                                                Level2Code = item2.Code,

                                                Level3Id = item3.Id,
                                                Level3Name = item3.LocalName,
                                                Level3Code = item3.Code,

                                                Level4Id = item4.Id,
                                                Level4Name = item4.LocalName,
                                                LeafId = item4.Id,
                                                Level4Code = item4.Code,
                                            });



                                            var item4Look = lookUp.Where(g => g.Key == item4.Id);
                                            foreach (var item5G in item4Look)
                                            {
                                                foreach (var item5 in item5G)
                                                {
                                                    listOfChartOfAccount5LevelM.Add(
                                                    new ChartOfAccount5LevelM()
                                                    {
                                                        Tenant = item1.Tenant,
                                                        ChartOfAccountTypeCode = item1.TypeCode,
                                                        Level1Id = item1.Id,
                                                        Level1Name = item1.LocalName,
                                                        Level1Code = item1.Code,

                                                        Level2Id = item2.Id,
                                                        Level2Name = item2.LocalName,
                                                        Level2Code = item2.Code,

                                                        Level3Id = item3.Id,
                                                        Level3Name = item3.LocalName,
                                                        Level3Code = item3.Code,


                                                        Level4Id = item4.Id,
                                                        Level4Name = item4.LocalName,
                                                        Level5Id = item5.Id,
                                                        Level5Name = item5.LocalName,
                                                        Level5Code = item5.Code,
                                                        LeafId = item5.Id,
                                                    });
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }


            return listOfChartOfAccount5LevelM;
        }

        public bool CheckWhetherConnected(string code, string id, int tenant)
        {
            return this.repository.CheckWhetherConnected(code, id, tenant);
        }

        public List<ChartOfAccountPM> GetByCode(string code, int tenant)
        {
            List<ChartOfAccount> pocos = this.repository.GetByCode(code, tenant);
            return pocos.Select(rec => this.GetEntityPM(rec)).ToList();
        }

        public ChartOfAccountPM GetSinglePM(string id, int tenant)
        {
            ChartOfAccountPM entityPM = null;

            entityPM =
                (from a in repository.All()
                 where a.Id == id && a.Tenant == tenant
                 select new ChartOfAccountPM()
                 {
                     Id = a.Id,
                     LocalName = a.LocalName,
                     EnglishName = a.EnglishName,
                     Inactive = a.Inactive,
                     Tenant = a.Tenant,
                     Code = a.Code,
                     ParentId = a.ParentId,
                     TypeCode = a.TypeCode,







                     SearchFields = a.SearchFields,
                 }).FirstOrDefault();

            return entityPM;
        }

        public ChartOfAccountPM GetSinglePMByCode(string code, int tenant)
        {
            ChartOfAccountPM entityPM = null;
            entityPM =
                (from a in repository.All()
                 where a.Code == code && a.Tenant == tenant
                 select new ChartOfAccountPM()
                 {
                     Id = a.Id,
                     LocalName = a.LocalName,
                     EnglishName = a.EnglishName,
                     Inactive = a.Inactive,
                     Tenant = a.Tenant,
                     Code = a.Code,
                     ParentId = a.ParentId,
                     TypeCode = a.TypeCode,







                     SearchFields = a.SearchFields,
                 }).FirstOrDefault();

            return entityPM;
        }
        public void CopyFromTenant0(int tenant, int tenatToCopy)
        {
            ChartOfAccountUpdateService service = new ChartOfAccountUpdateService(context, new Dictionary<string, IContext>(), tenatToCopy);
            List<ChartOfAccount> pocos = this.repository.GetAll(tenant).Where(t => t.Inactive == false).ToList();
            foreach (var item in pocos)
            {
                
                ChartOfAccountPM chartOfAccount = new ChartOfAccountPM();
                chartOfAccount.Tenant = tenatToCopy;
                chartOfAccount.Code = item.Code;
                chartOfAccount.LocalName = item.LocalName;
                chartOfAccount.EnglishName = item.EnglishName;
                chartOfAccount.ParentId = item.ParentId;
                chartOfAccount.TypeCode = item.TypeCode;
                chartOfAccount.Inactive = item.Inactive;
                chartOfAccount.SearchFields = item.SearchFields;
                chartOfAccount.ChartOfAccountSecurityLevel = item.ChartOfAccountSecurityLevel;
                chartOfAccount.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                service.Update(chartOfAccount, true);

            }
            this.context.SaveChanges();

        }


    }
    public class ChartOfAccount5LevelM
    {

        public int Tenant { get; set; }


        public string Level1Id { get; set; }
        public string Level1Name { get; set; }
        public string Level1Code { get; set; }

        public string Level2Id { get; set; }
        public string Level2Name { get; set; }
        public string Level2Code { get; set; }

        public string Level3Id { get; set; }
        public string Level3Name { get; set; }
        public string Level3Code { get; set; }

        public string Level4Id { get; set; }
        public string Level4Name { get; set; }
        public string Level4Code { get; set; }

        public string Level5Id { get; set; }
        public string Level5Name { get; set; }
        public string Level5Code { get; set; }

        public string GLAccountId { get; set; }
        public string GLAccountName { get; set; }

        public string ChartOfAccountTypeCode { get; set; }
        public string ChartOfAccountTypeName { get; set; }

        public string LeafId { get; set; }
        public string GLAccountNumber { get; set; }
        public string ChartOfAccountId { get; set; }

    }

}
