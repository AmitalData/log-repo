using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class AuthenticationTokenRepository : IRepository<AuthenticationToken>
    {
        ICommonDataContext commonDataContext;
        public AuthenticationTokenRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public AuthenticationTokenRepository()
        {
            commonDataContext=new CommonDataContext();
        }

        public AuthenticationTokenRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }
        
        public void Add(AuthenticationToken entity)
        {
            context.AuthenticationTokens.Add(entity);
        }

        public void Remove(AuthenticationToken entity)
        {
            try
            {
                context.AuthenticationTokens.Attach(entity);
            }
            catch { };
            context.AuthenticationTokens.Remove(entity);
        }

        public void Update(AuthenticationToken entity)
        {
            try
            {
                context.AuthenticationTokens.Attach(entity);
            }
            catch { };
            context.SetAsModified(entity);
        }

        public List<AuthenticationToken> All()
        {
            return context.AuthenticationTokens.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public AuthenticationToken GetSingleToken(string token)
        {
            if (!string.IsNullOrEmpty(token) && !token.Contains("+"))
            {
                token = token.Replace(" ", "+");
            }

            return (from a in context.AuthenticationTokens
                    where a.Token == token
                    select a).FirstOrDefault();
        }

        public static AuthenticationToken GetSingleTokenFromCache(string token)
        {
            if (string.IsNullOrEmpty(token)) return null;
            string cacheKey = $"Token_{token}";
            AuthenticationToken authenticationToken  =(AuthenticationToken)HttpContext.Current.Items["authToken"];
            if (authenticationToken == null)
            {
                if ( !token.Contains("+"))
                {
                    token = token.Replace(" ", "+");
                }
                authenticationToken = (AuthenticationToken)CacheManager.CacheWrapper.Get(cacheKey);
            }
            if (authenticationToken != null)
            {
                if (token != authenticationToken.Token)
                {
                    throw new AutenticationException("Invalid token");
                }   
                if (authenticationToken.ExpirationDate != null && authenticationToken.ExpirationDate < DateTime.Now)
                {
                    throw new AutenticationException("Session expired. Please log in again");
                }
            }
            else
            {
                authenticationToken = new AuthenticationTokenRepository().GetSingleToken(token);
                if (authenticationToken != null)
                {
                    CacheManager.CacheWrapper.Insert(cacheKey, authenticationToken, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
            }
            return authenticationToken;
        }

        public class AutenticationException : Exception
        {
            public AutenticationException(string message) : base(message) { }
        }

        public List<AuthenticationToken> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public AuthenticationToken GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        //public static AuthenticationToken GetSingleTokenFromCache(string token)
        //{
        //    if (HttpContext.Current != null)
        //    {
        //        string entityName = "Token" + token;
        //        if (CacheManager.CacheWrapper.Get(entityName) != null)
        //        {
        //            return (AuthenticationToken)CacheManager.CacheWrapper.Get(entityName);
        //        }
        //        else
        //        {
        //            ICommonDataContext context = CommonDataContext.GetContext(0);
        //            var entity = (from a in context.AuthenticationTokens
        //                          where a.Token == token
        //                          select a).FirstOrDefault();

        //            CacheManager.CacheWrapper.Insert(entityName, entity, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);

        //            return entity;
        //        }
        //    }
        //    else
        //    {
        //        ICommonDataContext context = CommonDataContext.GetContext(0);
        //        return (from a in context.AuthenticationTokens
        //                where a.Token == token
        //                select a).FirstOrDefault();
        //    }
        //}

    }


}
