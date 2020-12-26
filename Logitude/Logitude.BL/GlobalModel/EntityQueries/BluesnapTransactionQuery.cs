using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.GlobalModel.EntityLists;
using Logitude.BL.GlobalModel.EntityPMs;

namespace Logitude.BL.GlobalModel.EntityQueries
{
    public class BluesnapTransactionQuery
    {
        BluesnapTransactionRepository repository;

        public BluesnapTransactionQuery()
        {
               repository = new BluesnapTransactionRepository(); 
        }
        
        public BluesnapTransactionQuery(BluesnapTransactionRepository BluesnapTransactionRepository)
        {
            repository = BluesnapTransactionRepository;
        }

        public BluesnapTransactionPM GetSinglePM(string Id, int tenant = 0)
        {
            BluesnapTransactionPM entity;
            entity = (from a in repository.context.BluesnapTransactions
                      where a.Id == Id
                      select new BluesnapTransactionPM()
                      {
                          Id = a.Id,
                          Tenant = a.Tenant,
                          CreateDate = a.CreateDate,
                          DocumentId = a.DocumentId,
                          LogitudeAmital = a.LogitudeAmital,
                          TransactionDate = a.TransactionDate,
                          InvoiceAmountInUSD = a.InvoiceAmountInUSD,
                          TaxAmountInUSD = a.TaxAmountInUSD,
                          ContractNumber = a.ContractNumber,
                      }).FirstOrDefault();

            return entity;
        }

        public BluesnapTransactionPM GetSingleBluesnapTransactionPM(string Id, int tenant)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                string entityName = "BluesnapTransactionPM" + Id + tenant;
                BluesnapTransactionPM entity;
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        var entitystatuses = (from a in repository.context.BluesnapTransactions

                                              select new BluesnapTransactionPM()
                                              {
                                                  Id = a.Id,
                                                  Tenant = a.Tenant,
                                                  CreateDate = a.CreateDate,
                                                  DocumentId = a.DocumentId,
                                                  LogitudeAmital = a.LogitudeAmital,
                                                  TransactionDate = a.TransactionDate,
                                                  InvoiceAmountInUSD = a.InvoiceAmountInUSD,
                                                  TaxAmountInUSD = a.TaxAmountInUSD,
                                                  ContractNumber = a.ContractNumber,
                                              });
                        foreach (var s in entitystatuses)
                        {
                            string name = "BluesnapTransactionPM" + s.Id + tenant;
                            if (CacheManager.CacheWrapper.Get(name) == null)
                            {
                                CacheManager.CacheWrapper.Insert(name, s, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                        entity = (BluesnapTransactionPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                    else
                    {
                        entity = (BluesnapTransactionPM)CacheManager.CacheWrapper.Get(entityName);
                    }
                }
                else
                {
                    entity = (from a in repository.context.BluesnapTransactions
                              where a.Id == Id
                              select new BluesnapTransactionPM()
                              {
                                  Id = a.Id,
                                  Tenant = a.Tenant,
                                  CreateDate = a.CreateDate,
                                  DocumentId = a.DocumentId,
                                  LogitudeAmital = a.LogitudeAmital,
                                  TransactionDate = a.TransactionDate,
                                  InvoiceAmountInUSD = a.InvoiceAmountInUSD,
                                  TaxAmountInUSD = a.TaxAmountInUSD,
                                  ContractNumber = a.ContractNumber,

                              }).FirstOrDefault();
                }

                return entity;
            }
            return null;
        }

        public IQueryable<BluesnapTransactionList> GetIQueryableEntityList(IQueryable<BluesnapTransaction> iQueryable)
        {
            IQueryable<BluesnapTransactionList> result = from a in iQueryable
                                                      select new BluesnapTransactionList()
                                                          {
                                                          Id = a.Id,
                                                          Tenant = a.Tenant,
                                                          CreateDate = a.CreateDate,
                                                          DocumentId = a.DocumentId,
                                                          LogitudeAmital = a.LogitudeAmital,
                                                          TransactionDate = a.TransactionDate,
                                                          InvoiceAmountInUSD = a.InvoiceAmountInUSD,
                                                          TaxAmountInUSD = a.TaxAmountInUSD,
                                                          ContractNumber = a.ContractNumber,
                                                      };
            return result;
        }
    }
}
