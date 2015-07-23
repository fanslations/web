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
    }
}