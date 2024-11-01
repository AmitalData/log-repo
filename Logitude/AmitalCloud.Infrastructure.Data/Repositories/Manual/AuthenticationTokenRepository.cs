using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Domain.Helpers;
using AmitalCloud.Infrastructure.Domain.Interfaces;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class AuthenticationTokenRepository : Repository< AuthenticationToken> 
    {
        IAmitalCloudContext currentContext;
        public AuthenticationTokenRepository(IAmitalCloudContext context) : base(context)
        {
            currentContext = context;
        }
        public AuthenticationTokenRepository() : this(new AmitalCloudContext())
        {
        }
        public AuthenticationTokenRepository(int tenant) : this( AmitalCloudContext.GetContext(tenant))
        {
        }
        public IAmitalCloudContext context
        {
            get { return currentContext; }
        }
        public AuthenticationToken GetSingleToken(string token)
        {
            if (!string.IsNullOrEmpty(token) && !token.Contains("+"))
            {
                token = token.Replace(" ", "+");
            }
            return GetMulti(a => a.Token == token).FirstOrDefault();
        }
        public static AuthenticationToken GetSingleTokenFromCache(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;
            if (!string.IsNullOrEmpty(token) && !token.Contains("+"))
            {
                token = token.Replace(" ", "+");
            }
            string cacheKey = $"Token_({token})";
            AuthenticationToken authenticationToken = (AuthenticationToken)CacheManager.CacheWrapper.Get(cacheKey);
            if (authenticationToken != null)
            {
                if (authenticationToken.ExpirationDate != null && authenticationToken.ExpirationDate < DateTime.Now)
                {
                    throw new AutenticationException("Session expired. Please log in again");
                }
                return authenticationToken;
            }
            else
            {
                IAmitalCloudContext context = AmitalCloudContext.GetContext(0);
                authenticationToken = (from a in context.AuthenticationTokens
                              where a.Token == token
                              select a).FirstOrDefault();
                if (authenticationToken != null)
                {
                    if (authenticationToken.ExpirationDate != null && authenticationToken.ExpirationDate < DateTime.Now)
                    {
                        throw new AutenticationException("Session expired. Please log in again");
                    }
                    CacheManager.CacheWrapper.Insert(cacheKey, authenticationToken, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                return authenticationToken;
            }
        }
        public class AutenticationException : Exception
        {
            public AutenticationException(string message) : base(message) { }
        }
    }
}
