using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using System.Net;
using System.IO;

namespace Authentication
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
            //ViewState[AntiXsrfUserNameKey] = Session["email"] ?? String.Empty;
            var ui = Session["email"];
            if (ui != null)
            {
                DivAnonymous.Visible = false;
                DivAuthorised.Visible = true;
            }
            else
            {
                DivAnonymous.Visible = true;
                DivAuthorised.Visible = false;
            }
        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }

        protected void logOut_Click(object sender, EventArgs e)
        {
            Session["email"] = null;
            //WebClient webClient = new WebClient();
            //Stream stream = webClient.OpenRead("https://www.google.com/accounts/Logout");
            //string b;

            ///*I have not used any JSON parser because I do not want to use any extra dll/3rd party dll*/
            //using (StreamReader br = new StreamReader(stream))
            //{
            //    b = br.ReadToEnd();
            //}
            //WebRequest request = WebRequest.Create("https://accounts.google.com/Logout");
            //request.Method = "GET";
            //request.GetResponse();
            WebClient g = new WebClient();
            g.DownloadString("https://www.google.com/accounts/Logout");
            Response.Redirect("~/Account/Login");
        }
    }

}