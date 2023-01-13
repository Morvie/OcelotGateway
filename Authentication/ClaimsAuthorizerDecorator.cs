using Ocelot.Authorization;
using Ocelot.DownstreamRouteFinder.UrlMatcher;
using Ocelot.Responses;
using System.Security.Claims;

namespace OcelotGateway.Authentication
{
    public class ClaimsAuthorizerDecorator : IClaimsAuthorizer
    {
        private readonly ClaimsAuthorizer _authorizer;

        public ClaimsAuthorizerDecorator(ClaimsAuthorizer authorizer)
        {
            _authorizer = authorizer;
        }

        public Response<bool> Authorize(ClaimsPrincipal claimsPrincipal, Dictionary<string, string> routeClaimsRequirement,
            List<PlaceholderNameAndValue> urlPathPlaceholderNameAndValues)
        {
            var newRouteClaimsRequirement = new Dictionary<string, string>();
            foreach (var kvp in routeClaimsRequirement)
            {
                if (kvp.Key.StartsWith("http$//"))
                {
                    var key = kvp.Key.Replace("http$//", "http://");
                    newRouteClaimsRequirement.Add(key, kvp.Value);
                }
                else
                {
                    newRouteClaimsRequirement.Add(kvp.Key, kvp.Value);
                }
            }

            return _authorizer.Authorize(claimsPrincipal, newRouteClaimsRequirement,
                urlPathPlaceholderNameAndValues);
        }
    }
}
