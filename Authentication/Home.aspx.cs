using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using ASPSnippets.GoogleAPI;
using System.Web.Script.Serialization;
using System.Data;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System.Security.Cryptography.X509Certificates;
using Google.GData.Spreadsheets;
using Google.GData.Client;
using System.Net.Mail;

namespace Authentication
{
    public partial class Home : System.Web.UI.Page
    {
        public string Email_address = "";
        public string Google_ID = "";
        public string firstName = "";
        public string LastName = "";
        public string Client_ID = "";
        public string Return_url = "";
        public string userProfilePic = "";

        static string[] Scopes = { "https://www.googleapis.com/auth/analytics.readonly", "https://www.googleapis.com/auth/drive", "https://www.googleapis.com/auth/spreadsheets" };
        static string ApplicationName = "Google Sheets API .NET Quickstart";
        protected void Page_Load(object sender, EventArgs e)
        {
            GetGoogleDataOfUser();
        }
        protected void drvUpload_Click(object sender, EventArgs e)
        {
            newMethod();
        }
        public void GetGoogleDataOfUser()
        {
            if (Request.QueryString["access_token"] != null)
            {
                Session["access_token"] = Request.QueryString["access_token"];
            }
            if (Session["access_token"] != null)
            {

                String URI = "https://www.googleapis.com/oauth2/v1/userinfo?access_token=" +
                                Session["access_token"].ToString();

                WebClient webClient = new WebClient();
                Stream stream = webClient.OpenRead(URI);
                string b;

                /*I have not used any JSON parser because I do not want to use any extra dll/3rd party dll*/
                using (StreamReader br = new StreamReader(stream))
                {
                    b = br.ReadToEnd();
                }
                var str = Newtonsoft.Json.JsonConvert.DeserializeObject<JSONClass>(b);
                Session["email"] = str.email;
                Session["Profilepicture"] = str.picture;
                Email_address = str.email;
                Google_ID = str.id;
                firstName = str.given_name;
                LastName = str.family_name;
                userProfilePic = str.picture;
                //userProfilePic = userProfilePic.Replace("https", "https:");
                imgProfile.ImageUrl = userProfilePic;
                ClientEmail.Text = Email_address;
                ClientName.Text = firstName + " " + LastName;

                JSONClass oi = new JSONClass();
                oi.email = str.email;
                oi.name = str.name;
                oi.given_name = str.given_name;
                oi.family_name = str.family_name;
                oi.picture = str.picture;
                oi.Gender = str.Gender;
                oi.locale = str.locale;

                Session["UserDataGet"] = oi;

                DataTable tbb = new DataTable();
                tbb.Columns.Add("email");
                tbb.Columns.Add("name");
                tbb.Columns.Add("given_name");
                tbb.Columns.Add("family_name");
                tbb.Columns.Add("picture");
                tbb.Columns.Add("Gender");
                tbb.Columns.Add("locale");
                tbb.Rows.Add(str.email, str.name, str.given_name, str.family_name, str.picture, str.Gender, str.locale);
                if (Request.QueryString["error"] != null)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "msgbox", "alert('Access Denied');", true);
                }
                
            }
        }
        public void newMethod()
        {
            string CLIENT_ID = "599831794378-85d0ai95jak2mobjp1h0p7n0d2bua8n3.apps.googleusercontent.com";

            string CLIENT_SECRET = "AlYRvFcwAILGyTMwYZz2veoJ";

            string SCOPE = "https://spreadsheets.google.com/feeds https://docs.google.com/feeds";

            string REDIRECT_URI = "http://localhost:53278/uploadpage.aspx";// "urn:ietf:wg:oauth:2.0:oob";


            OAuth2Parameters parameters = new OAuth2Parameters();

            parameters.ClientId = CLIENT_ID;

            parameters.ClientSecret = CLIENT_SECRET;

            parameters.RedirectUri = REDIRECT_URI;

            parameters.Scope = SCOPE;
            Session["para"] = parameters;
            string authorizationUrl = OAuthUtil.CreateOAuth2AuthorizationUrl(parameters);
            Response.Redirect(authorizationUrl);

            
        }
        public class JSONClass
        {
            public string id { get; set; }
            public string email { get; set; }
            public string verified_email { get; set; }
            public string name { get; set; }
            public string given_name { get; set; }
            public string family_name { get; set; }
            public string picture { get; set; }
            public string Gender { get; set; }
            public string locale { get; set; }

        }
    }
}