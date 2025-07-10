using System;
using System.Collections.Generic;
using System.Linq;
using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Interfaces;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class AuthenticationTokenRepository : IRepository<AuthenticationToken,string>
    {
        IAmitalCloudContext currentContext;
        public AuthenticationTokenRepository(IAmitalCloudContext context)
        {
            currentContext = context;
        }
        public AuthenticationTokenRepository()
        {
            currentContext=new AmitalCloudContext();
        }
        public AuthenticationTokenRepository(int tenant)
        {
            currentContext = AmitalCloudContext.GetContext(tenant);
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
        public IAmitalCloudContext context
        {
            get { return currentContext; }
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
            if (string.IsNullOrEmpty(token))
                return null;
            if (!string.IsNullOrEmpty(token) && !token.Contains("+"))
            {
                token = token.Replace(" ", "+");
            }
                string entityName = "Token" + token;
                if (CacheManager.CacheWrapper.Get(entityName) != null)
                {
                    AuthenticationToken authenticationToken = (AuthenticationToken)CacheManager.CacheWrapper.Get(entityName);
                    if(authenticationToken.ExpirationDate != null && authenticationToken.ExpirationDate < DateTime.Now)
                    {
                        throw new AutenticationException("Session expired. Please log in again");
                    }
                    return authenticationToken;
                }
                else
                {
                    IAmitalCloudContext context = AmitalCloudContext.GetContext(0);
                    var entity = (from a in context.AuthenticationTokens
                                  where a.Token == token
                                  select a).FirstOrDefault();
                    if (entity != null)
                    {
                        CacheManager.CacheWrapper.Insert(entityName, entity, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                    }
                    return entity;
                }
        }
        public class AutenticationException : Exception
        {
            public AutenticationException(string message) : base(message) { }
        }
        public List<AuthenticationToken> GetMulti(IEntityKeyFields<AuthenticationToken,string> entityKeys)
        {
            throw new NotImplementedException();
        }
        public AuthenticationToken GetSingle(IEntityKeyFields<AuthenticationToken,string> entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
