// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;

using ThreeSC.NetStandardLib.StandardTools.Interfaces;

using ThreeSC.Net6Lib.BlazorTools.Interfaces;
using ThreeSC.Net6Lib.BlazorTools.Areas.Identity.Pages.Account;
using UniANPR.Models.ThreeSCBaseImplementations;
using ThreeSC.Net6Lib.BlazorTools.Models;

namespace UniANPR.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : ThreeSCLoginBase<TestAppUser>
    {
        public LoginModel(SignInManager<ThreeSCBaseApplicationUser> signInManager,
                          UserManager<ThreeSCBaseApplicationUser> userManager,
                          IEmailSender emailSender,
                          NavigationManager thisNavigationManager, AuthenticationStateProvider thisAuthenticationStateProvider,
                          IThreeSCApplicationLogger thisThreeSCApplicationLogger,
                          IThreeSCUserSessionReader thisThreeSCUserSessionReader,
                          IThreeSCUserSessionManager<TestAppUser> thisThreeSCUserSessionManager) :
            base(signInManager, userManager, emailSender, thisNavigationManager, thisAuthenticationStateProvider,
                 thisThreeSCApplicationLogger, thisThreeSCUserSessionReader, thisThreeSCUserSessionManager)
        { }

        /// <summary>
        /// Test initial customer account
        /// </summary>
        protected override string InitialCustomerUserEmail
        {
            get { return "fred.fred@fred.fred"; }
        }

        /// <summary>
        /// Test initial customer account password
        /// </summary>
        protected override string InitialCustomerPassword
        {
            get { return "Purpl3!orange"; }
        }

        /// <summary>
        /// On login success return the desired redirection URL
        /// </summary>
        /// <param name="loggedInUser">User profile data of logged in user</param>
        /// <param name="returnUrl">Return URL specified in the login request url, null if none specified</param>
        /// <returns>string - url to send user to, for example Url.Content("/Dashboard");</returns>
        protected override string SuccessLoginRedirect(ThreeSCBaseApplicationUser loggedInUser, string returnUrl)
        {
            // return the passed in return url, if one was specified in the login request url, else go to a start page appropriate for user's roles
            // ------------------------------------------------------------------------------------------------------------------------------------
            if (returnUrl == null)
            {
                // if no return url, send to home page
                // -----------------------------------
                returnUrl = Url.Content("/");
            }

            return returnUrl.StartsWith("/") ? returnUrl : $"/{returnUrl}";
        }

        protected override string SuccessButPasswordExpiredRedirect(ThreeSCBaseApplicationUser loggedInUser)
        {
            return Url.Content("/resetpassword");
        }

        protected override string FailLoginRedirect()
        {
            return Url.Content("/login");
        }

        protected override string UnknownUserLoginRedirect()
        {
            return Url.Content("/login");
        }

        protected override string LockedOutLoginRedirect()
        {
            return Url.Content("/UserLockedOut");
        }

        protected override string ErrorLoginRedirect()
        {
            return Url.Content("/Shared/Error");
        }

        protected override string DisabledLoginRedirect()
        {
            return Url.Content("/UserDisabled");
        }
    }
}
