using refactor_me.DataAccessLibraray;
using System.Web.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using refactor_me.Middleware;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace refactor_this
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            ConfigureJwtAuthentication(config);
            // Web API configuration and services
            var formatters = GlobalConfiguration.Configuration.Formatters;
            formatters.Remove(formatters.XmlFormatter);
            formatters.JsonFormatter.Indent = true;

            // Web API routes
            config.MapHttpAttributeRoutes();


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

 

        }

        private static void ConfigureJwtAuthentication(HttpConfiguration config)
        {
            var issuer = "test";
            var audience = "test";
            var secret = "test"; // Replace with your actual secret key

            //var validationParameters = new TokenValidationParameters
            //{
            //    ValidIssuer = issuer,
            //    ValidAudience = audience,
            //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            //    ValidateIssuerSigningKey = true,
            //    ValidateAudience = true,
            //    ValidateIssuer = true,
            //    ValidateLifetime = true,
            //    ClockSkew = TimeSpan.FromMinutes(30),
            //};

            //config.Filters.Add(new AuthorizeAttribute()); // Require authorization for all controllers/actions

            config.MessageHandlers.Add(new JwtAuthenticationHandler(issuer, audience, secret));
        }
    }
}
