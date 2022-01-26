using System.Text.Encodings.Web;
using AspNet.Security.OAuth.GitLab;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace device_wall_backend.Authentication
{
    public class MDWGitLabAuthHandler : GitLabAuthenticationHandler
    {
        public MDWGitLabAuthHandler(IOptionsMonitor<GitLabAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            return base.BuildChallengeUrl(properties, "http://localhost:4000/");
        }
    }
}