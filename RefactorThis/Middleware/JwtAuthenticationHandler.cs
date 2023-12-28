
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;


namespace refactor_me.Middleware
{
    public class JwtAuthenticationHandler : DelegatingHandler
    {
        private readonly string issuer;
        private readonly string audience;
        private readonly string secret;

        public JwtAuthenticationHandler(string issuer, string audience, string secret)
        {
            this.issuer = issuer;
            this.audience = audience;
            this.secret = secret;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Check if the request is coming from a local machine
            if (IsLocalRequest(request))
            {
                return base.SendAsync(request, cancellationToken);
            }

            var token = GetJwtTokenFromRequest(request);

            if (token == null)
            {
                return Task.FromResult(request.CreateResponse(HttpStatusCode.Unauthorized));
            }

            var principal = ValidateToken(token);

            if (principal == null)
            {
                return Task.FromResult(request.CreateResponse(HttpStatusCode.Unauthorized));
            }

            HttpContext.Current.User = principal;

            return base.SendAsync(request, cancellationToken);
        }

        private bool IsLocalRequest(HttpRequestMessage request)
        {
            var context = HttpContext.Current;

            // Check if the context is available and the request is local
            return context != null && context.Request.IsLocal;
        }

        private string GetJwtTokenFromRequest(HttpRequestMessage request)
        {
            var authHeader = request.Headers.Authorization;

            if (authHeader != null && authHeader.Scheme.Equals("Bearer", StringComparison.OrdinalIgnoreCase))
            {
                return authHeader.Parameter;
            }

            return null;
        }

        private ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(secret))
            };

            try
            {
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}