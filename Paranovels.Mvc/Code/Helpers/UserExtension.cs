using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Microsoft.AspNet.Identity;
using Paranovels.Common;
using Paranovels.Facade;
using Paranovels.ViewModels;

namespace Paranovels.Mvc
{
    public static class UserExtension
    {
        public static AuthSession GetSession(this IPrincipal user)
        {
            var criteria = new UserCriteria();
            criteria.Username = user.Identity.IsAuthenticated ? user.Identity.GetUserName() : WebHelper.GetClientIpAddress();
            return new UserFacade().GetAuthSession(criteria);
        }

        public static bool IsAdministrator(this IPrincipal user)
        {
            if (!user.Identity.IsAuthenticated) return false;

            var admins = new[] {"Fanslations","Google", "Twiter", "Facebook", "Microsoft"};
            return admins.Contains(user.Identity.Name);
        }
    }
}