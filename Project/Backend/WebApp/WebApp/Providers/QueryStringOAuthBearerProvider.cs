using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebApp.Providers
{
    public class QueryStringOAuthBearerProvider : OAuthBearerAuthenticationProvider
    {
        readonly string _name;

        public QueryStringOAuthBearerProvider(string name)
        {
            _name = name;
        }

        public override Task RequestToken(OAuthRequestTokenContext context)
        {
            var value = context.Request.Query.Get(_name);

            if (!string.IsNullOrEmpty(value))
            {
                value = value.Split(' ')[1];
                context.Token = value;
            }

            return Task.FromResult<object>(null);
        }
    }
}