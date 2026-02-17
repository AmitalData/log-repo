using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure;
namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class PaymentChannelRepository: IRepository<PaymentChannel>
    {
        IGlobalContext globalContext;

        public PaymentChannelRepository(IGlobalContext context)
        {
            globalContext = context;
        }

        public PaymentChannelRepository()
        {
            globalContext = new GlobalContext();
        }

        public PaymentChannelRepository(int tenant)
        {
            globalContext = GlobalContext.GetContext();
        }

        public PaymentChannel GetSinglePaymentChannel(string code)
        {
            PaymentChannel instance = (from i in context.PaymentChannels
                                 where i.Code == code                                 
                                 select i).FirstOrDefault();
            return instance;
        }

        public IQueryable<PaymentChannel> GetPaymentChannels()
        {
            return context.PaymentChannels;
        }

        public IQueryable<PaymentChannel> GetAll()
        {
            return context.PaymentChannels;
        }

        public void Add(PaymentChannel entity)
        {
            context.PaymentChannels.Add(entity);
        }

        public void Remove(PaymentChannel entity)
        {
            context.PaymentChannels.Attach(entity);
            context.PaymentChannels.Remove(entity);
        }

        public void Update(PaymentChannel entity)
        {
            context.PaymentChannels.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PaymentChannel> All()
        {
            return context.PaymentChannels.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<PaymentChannel> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public PaymentChannel GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}