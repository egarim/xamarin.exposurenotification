using BIT.Data.Helpers;
using BIT.Xpo.AspNetCore;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using BIT.Xpo.XPOWebApi.Server;
using BIT.Data.Models;

namespace MainServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : LoginControllerBase
    {       
        public LoginController(IDataStoreResolver DataStoreResolver) : base(DataStoreResolver)
        {
            PasswordCryptographer.EnableRfc2898 = true;
            PasswordCryptographer.SupportLegacySha512 = false;
        }

        protected override AuthenticationResult Authenticate(UnitOfWork UoW, LoginParameters LoginParameters)
        {
            AuthenticationResult authenticationResult = new AuthenticationResult();
            try
            {
                PermissionPolicyUser User = UoW.FindObject<PermissionPolicyUser>(new BinaryOperator("UserName", LoginParameters.Username));

                if (User == null)
                {
                    authenticationResult.LastError = "User not found";
                    return authenticationResult;
                }
                if (!User.ComparePassword(LoginParameters.Password))
                {
                    authenticationResult.LastError = "Password do not match";
                    return authenticationResult;
                }

                authenticationResult.Authenticated = true;
                authenticationResult.UserId = User.Oid.ToString();
                authenticationResult.Username = User.UserName;               
                return authenticationResult;

            }
            catch (Exception exception)
            {
                authenticationResult.LastError = exception.Message;
                return authenticationResult;
            }
        }

    }
}
