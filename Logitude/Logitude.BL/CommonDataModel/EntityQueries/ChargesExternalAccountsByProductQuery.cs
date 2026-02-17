using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class ChargesExternalAccountsByProductQuery
    {
        ChargesExternalAccountsByProductRepository repository;
        public ChargesExternalAccountsByProductQuery(int tenant)
        {
            repository = new ChargesExternalAccountsByProductRepository(tenant);
        }
        public ChargesExternalAccountsByProductQuery(ChargesExternalAccountsByProductRepository myRepository)
        {
            repository = myRepository;
        }

        public ChargesExternalAccountsByProductPM GetSinglePM(string id, int tenant)
        {
            ChargesExternalAccountsByProductPM entityPM = (from a in repository.context.ChargesExternalAccountsByProducts.Include("ProductType").Include("UpdatedByUser").Include("UpdatedByUser.Contact")
                                                           where a.Tenant == tenant && a.Id == id
                                                           select new ChargesExternalAccountsByProductPM()
                                                           {
                                                               Id = a.Id,
                                                               Tenant = a.Tenant,
                                                               PayablesGLAccount = a.PayablesGLAccount,
                                                               PayablesCostCenter = a.PayablesCostCenter,
                                                               ReceivablesGLAccount = a.ReceivablesGLAccount,
                                                               ReceivablesCostCenter = a.ReceivablesCostCenter,
                                                               UpdateDate = a.UpdateDate,
                                                               ChargesTypeId = a.ChargesTypeId,
                                                               ProductTypeCode = a.ProductTypeCode,
                                                               UpdatedByUserId = a.UpdatedByUserId,
                                                               ProductTypeName = a.ProductType == null ? null : a.ProductType.Name,
                                                               UpdatedByUserName = a.UpdatedByUser == null ? null : a.UpdatedByUser.Contact.EnglishName,
                                                           }).FirstOrDefault();

            return entityPM;
        }
        public IQueryable<ChargesExternalAccountsByProductPM> GetChargesExternalAccountsByProductsByChargesTypeId(string chargesTypeId, int tenant)
        {
            IQueryable<ChargesExternalAccountsByProductPM> myResult = from a in repository.context.ChargesExternalAccountsByProducts.Include("ProductType").Include("UpdatedByUser").Include("UpdatedByUser.Contact")
                                                                      where a.Tenant == tenant && a.ChargesTypeId == chargesTypeId
                                                                      select new ChargesExternalAccountsByProductPM()
                                                                      {
                                                                          Id = a.Id,
                                                                          Tenant = a.Tenant,
                                                                          PayablesGLAccount = a.PayablesGLAccount,
                                                                          PayablesCostCenter = a.PayablesCostCenter,
                                                                          ReceivablesGLAccount = a.ReceivablesGLAccount,
                                                                          ReceivablesCostCenter = a.ReceivablesCostCenter,
                                                                          UpdateDate = a.UpdateDate,
                                                                          ChargesTypeId = a.ChargesTypeId,
                                                                          ProductTypeCode = a.ProductTypeCode,
                                                                          UpdatedByUserId = a.UpdatedByUserId,
                                                                          ProductTypeName = a.ProductType == null ? null : a.ProductType.Name,
                                                                          UpdatedByUserName = a.UpdatedByUser == null ? null : a.UpdatedByUser.Contact.EnglishName,
                                                                      };
            return myResult;
        }
        public IQueryable<ChargesExternalAccountsByProductList> GetIQueryableEntityList(IQueryable<ChargesExternalAccountsByProduct> iQueryable)
        {
            IQueryable<ChargesExternalAccountsByProductList> myResult = from a in iQueryable
                                                                        select new ChargesExternalAccountsByProductList()
                                                                        {
                                                                            Id = a.Id,
                                                                            Tenant = a.Tenant,
                                                                            PayablesGLAccount = a.PayablesGLAccount,
                                                                            PayablesCostCenter = a.PayablesCostCenter,
                                                                            ReceivablesGLAccount = a.ReceivablesGLAccount,
                                                                            ReceivablesCostCenter = a.ReceivablesCostCenter,
                                                                            UpdateDate = a.UpdateDate,
                                                                            ChargesTypeId = a.ChargesTypeId,
                                                                            ProductTypeCode = a.ProductTypeCode,
                                                                            UpdatedByUserId = a.UpdatedByUserId,
                                                                        };
            return myResult;
        }
    }
}
