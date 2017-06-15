using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Google.GData.Client;
using Google.GData.Spreadsheets;

namespace Authentication
{
    public partial class UploadPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            method1();
            if (Request.QueryString["error"] != null)
            {
                Response.Redirect("Home.aspx?error=" + Request.QueryString["error"].ToString());
            }
        }
        public void method1()
        {
            OAuth2Parameters parameters = (OAuth2Parameters)Session["para"];
            if (!string.IsNullOrEmpty(Request.QueryString["Code"]))
            {


                parameters.AccessCode = Request.QueryString["Code"].ToString();// = Console.ReadLine();


                OAuthUtil.GetAccessToken(parameters);
                //
                //parameters.AccessToken= credential.Token.AccessToken;
                //
                string accessToken = parameters.AccessToken;
                Console.WriteLine("OAuth Access Token: " + accessToken);

                GOAuth2RequestFactory requestFactory =
                    new GOAuth2RequestFactory(null, "MySpreadsheetIntegration-v1", parameters);
                SpreadsheetsService service = new SpreadsheetsService("MySpreadsheetIntegration-v1");
                service.RequestFactory = requestFactory;


                SpreadsheetQuery query = new SpreadsheetQuery();

                SpreadsheetFeed feed = service.Query(query);

                //foreach (SpreadsheetEntry entry in feed.Entries)
                //{
                //    Console.WriteLine(entry.Title.Text);
                //}

                //SpreadsheetEntry spreadsheet = (SpreadsheetEntry)feed.Entries[0];
                if (feed.Entries.Where(x => x.Title.Text == "testSS").First() != null)
                {

                    SpreadsheetEntry spreadsheet = (SpreadsheetEntry)feed.Entries.Where(x => x.Title.Text == "testSS").First();
                    //SpreadsheetEntry spreadsheet = (SpreadsheetEntry)feed.CreateFeedEntry();
                    //Console.WriteLine(spreadsheet.Title.Text);
                    //AtomTextConstruct at = new AtomTextConstruct(AtomTextConstructElementType.Title);
                    //at.Text = "test";
                    //spreadsheet.Title = at;
                    //    spreadsheet.Worksheets.AddExtension(requestFactory);
                    var t = (Home.JSONClass)Session["UserDataGet"];
                    List<object> lists = new List<object>() { t.email, t.name, t.given_name, t.family_name, t.picture, t.Gender, t.locale };

                    // Get the first worksheet of the first spreadsheet.
                    // TODO: Choose a worksheet more intelligently based on your
                    // app's needs.
                    WorksheetFeed wsFeed = spreadsheet.Worksheets;
                    WorksheetEntry worksheet = (WorksheetEntry)wsFeed.Entries[0];

                    // Define the URL to request the list feed of the worksheet.
                    AtomLink listFeedLink = worksheet.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);

                    // Fetch the list feed of the worksheet.
                    ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
                    ListFeed listFeed = service.Query(listQuery);


                    CellQuery cellQuery = new CellQuery(worksheet.CellFeedLink);
                    CellFeed cellFeed = service.Query(cellQuery);

                    CellEntry cellEntry = new CellEntry(1, 1, "email");
                    cellFeed.Insert(cellEntry);
                    cellEntry = new CellEntry(1, 2, "name");
                    cellFeed.Insert(cellEntry);
                    cellEntry = new CellEntry(1, 3, "givenname");
                    cellFeed.Insert(cellEntry);
                    cellEntry = new CellEntry(1, 4, "familyname");
                    cellFeed.Insert(cellEntry);
                    cellEntry = new CellEntry(1, 5, "picture");
                    cellFeed.Insert(cellEntry);
                    cellEntry = new CellEntry(1, 6, "gender");
                    cellFeed.Insert(cellEntry);
                    cellEntry = new CellEntry(1, 7, "locale");
                    cellFeed.Insert(cellEntry);



                    // Create a local representation of the new row.
                    ListEntry row = new ListEntry();
                    row.Elements.Add(new ListEntry.Custom() { LocalName = "email", Value = t.email });
                    row.Elements.Add(new ListEntry.Custom() { LocalName = "name", Value = t.name });
                    row.Elements.Add(new ListEntry.Custom() { LocalName = "givenname", Value = t.given_name });
                    row.Elements.Add(new ListEntry.Custom() { LocalName = "familyname", Value = t.family_name });
                    row.Elements.Add(new ListEntry.Custom() { LocalName = "picture", Value = t.picture });
                    row.Elements.Add(new ListEntry.Custom() { LocalName = "gender", Value = t.Gender });
                    row.Elements.Add(new ListEntry.Custom() { LocalName = "locale", Value = t.locale });
                    // Send the new row to the API for insertion.
                    service.Insert(listFeed, row);
                }
                Response.Redirect("Home.aspx?access_token=" + Session["access_token"].ToString());
            }
           
        }
    }
}