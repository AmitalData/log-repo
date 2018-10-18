using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class PasswordResetRequestRepository : IRepository<PasswordResetRequest>
    {
        IGlobalContext globalContext;
        public PasswordResetRequestRepository()
        {
            globalContext = GlobalContext.GetContext();
        }

        public PasswordResetRequestRepository(IGlobalContext context)
        {
            globalContext = context;
        }


        public IQueryable<PasswordResetRequest> GetPasswordResetRequests()
        {
            return context.PasswordResetRequests;
        }

        public PasswordResetRequest GetSinglePasswordResetRequest(string requestNumber)
        {
            PasswordResetRequest request = (from record in context.PasswordResetRequests where record.RequestNumber == requestNumber select record).FirstOrDefault();

            return request; //Your password reset link is: http://127.0.0.1:81/Default.aspx?reset_request_number=16246e3576434691b45819542dde4f688003046
        }




        public void Add(PasswordResetRequest entity)
        {
            context.PasswordResetRequests.Add(entity);
        }

        public void Remove(PasswordResetRequest entity)
        {
            context.PasswordResetRequests.Attach(entity);
            context.PasswordResetRequests.Remove(entity);
        }

        public void Update(PasswordResetRequest entity)
        {
            context.PasswordResetRequests.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PasswordResetRequest> All()
        {
            return context.PasswordResetRequests.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<PasswordResetRequest> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public PasswordResetRequest GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<PasswordResetRequest> GetPasswordResetRequestByEmail(string email ,string verificationCode)
        {
            IQueryable<PasswordResetRequest> requestList = (from record in context.PasswordResetRequests where record.Email == email  && record.IsDone == false  && record.DoneDate == null select record).OrderByDescending(d => d.CreateDate).Take(3);

            return requestList;
        }
    }
}
