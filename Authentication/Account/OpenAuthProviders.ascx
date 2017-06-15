<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OpenAuthProviders.ascx.cs" Inherits="Authentication.Account.OpenAuthProviders" %>
<link href="../Content/bootstrap-social.css" rel="stylesheet" />
<script type="text/javascript" language="javascript">
    function OpenGoogleLoginPopup() {
        debugger;
            var url = "https://accounts.google.com/o/oauth2/auth?";
            url += "scope=https://www.googleapis.com/auth/userinfo.profile "+
            "https://www.googleapis.com/auth/drive.file https://www.googleapis.com/auth/userinfo.email&";
            url += "state=%2Fprofile&"
            url += "redirect_uri=http://localhost:53278/LoginWithGoogleAcc.aspx&"
            url += "response_type=token&"
            url += "client_id=599831794378-6ouj8vgubtpqieu9ic6ap9f9nnqvmi1p.apps.googleusercontent.com";
            window.location = url;
        }
    </script> 
<div id="socialLoginList">
    <h4>Use another service to log in.</h4>
    <hr />
    <asp:ListView runat="server" ID="providerDetails" ItemType="System.String"
        SelectMethod="GetProviderNames" ViewStateMode="Disabled">
        <ItemTemplate>
            <p>
                <button type="submit" class="btn btn-default" name="provider" value="<%#: Item %>"
                    title="Log in using your <%#: Item %> account.">
                    <%#: Item %>
                </button>
            </p>
        </ItemTemplate>
        <EmptyDataTemplate>
            <div>
                <%--<p>There are no external authentication services configured. See <a href="http://go.microsoft.com/fwlink/?LinkId=252803">this article</a> for details on setting up this ASP.NET application to support logging in via external services.</p>--%>
                <input class="btn btn-block btn-social btn-lg btn-google" type="button" id="Button1"
                            runat="server" value="Sign in with Google" onclick="OpenGoogleLoginPopup();" />
                    <i class="fa fa-google"></i>
                
            </div>
        </EmptyDataTemplate>
    </asp:ListView>
</div>
