using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class EmailProviderQuery
    {
        EmailProviderRepository repository;

        public EmailProviderQuery()
        {
            repository = new EmailProviderRepository();
        }

        public EmailProviderQuery(int tenant)
        {
            repository = new EmailProviderRepository(tenant);
        }

        public EmailProviderQuery(EmailProviderRepository EmailProviderRepository)
        {
            repository = EmailProviderRepository;
        }

        public EmailProviderPM GetSinglePM(int tenant, string providerNumber)
        {
            EmailProviderPM EmailProvider = (from a in repository.context.EmailProviders
                                             where a.ProviderNumber == providerNumber
                                             select new EmailProviderPM()
                                             {
                                                 Domain = a.Domain,
                                                 LastTestReceivedDate = a.LastTestReceivedDate,
                                                 LastTestSendDate = a.LastTestSendDate,
                                                 Password = a.Password,
                                                 Port = a.Port,
                                                 ProviderNumber = a.ProviderNumber,
                                                 Status = a.Status,
                                                 UserName = a.UserName,
                                                 SupportsEmailDelivery = a.SupportsEmailDelivery,
                                             }).FirstOrDefault();

            EmailProviderPM securedPm = new EmailProviderPM();
            SecuredMapping.GetMappedPM(EmailProvider, securedPm, "EmailProvider", tenant);

            return securedPm;
        }

        public IQueryable<EmailProviderPM> GetEmailProviderPMs()
        {
            IQueryable<EmailProviderPM> EmailProviders = from a in repository.context.EmailProviders
                                                         select new EmailProviderPM()
                                                         {
                                                             Domain = a.Domain,
                                                             LastTestReceivedDate = a.LastTestReceivedDate,
                                                             LastTestSendDate = a.LastTestSendDate,
                                                             Password = a.Password,
                                                             Port = a.Port,
                                                             ProviderNumber = a.ProviderNumber,
                                                             Status = a.Status,
                                                             UserName = a.UserName,
                                                             SupportsEmailDelivery = a.SupportsEmailDelivery,
                                                         };
            return EmailProviders;
        }

        public IQueryable<EmailProviderList> GetIQueryableEntityList(IQueryable<EmailProvider> iQueryable)
        {
            IQueryable<EmailProviderList> result = from a in iQueryable
                                           select new EmailProviderList()
                                           {
                                               Domain = a.Domain,
                                               LastTestReceivedDate = a.LastTestReceivedDate,
                                               LastTestSendDate = a.LastTestSendDate,
                                               Password = a.Password,
                                               Port = a.Port,
                                               ProviderNumber = a.ProviderNumber,
                                               Status = a.Status,
                                               UserName = a.UserName,
                                               SupportsEmailDelivery = a.SupportsEmailDelivery,
                                           };
            return result;
        }
    }
}
