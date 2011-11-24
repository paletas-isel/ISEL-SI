using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace GmailContacts.Controllers
{
    public class ContactController : Controller
    {
        //
        // GET: /Contacts/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /AllContacts/

        public ActionResult All(string email)
        {
            string clientId = "19820391233.apps.googleusercontent.com";
            string redirectUri = @"http://localhost:43572/Contact/Authorized";
            string scope = @"https://www.google.com/m8/feeds+https://www.googleapis.com/auth/userinfo.profile";

            string oauthEndpoint =
                String.Format(@"https://accounts.google.com/o/oauth2/auth?response_type=code&client_id={0}&redirect_uri={1}&scope={2}&state={3}", clientId, redirectUri, scope, email);
        
            return Redirect(oauthEndpoint);
        }


        public ActionResult Authorized(string code, string state)
        {
            string clientId = "19820391233.apps.googleusercontent.com";
            string redirectUri = String.Format(@"http://localhost:43572/Contact/ObtainAll");
            string clientSecret = "WlLnme7lieXTOLOGM0Z-aeZJ";
            string grantType = "authorization_code";
 
            
            string tokenEndpoint = "https://accounts.google.com/o/oauth2/token";
            string tokenBody =     
                String.Format("code={0}&client_id={1}&client_secret={2}&redirect_uri={3}&grant_type={4}",
                                code, clientId, clientSecret, redirectUri, grantType);

            HttpContext.Response.Method

            HttpWebRequest postRequest = new HttpWebRequest(tokenEndpoint);
            postRequest.Method = "POST";
            postRequest.ContentLength = tokenBody.Length;
            postRequest.ContentType = "application/x-www-form-urlencoded";
            HttpWebResponse r = postRequest.GetResponse();
            return r.;
        }

        public ActionResult ObtainAll(string access_token)
        {
            string emailEndpoint = String.Format(@"https://www.google.com/m8/feeds/contacts/userEmail/full?access_token={0}", access_token);

            return Redirect(emailEndpoint);
        }

        public ActionResult Show(string xmlResponse)
        {
            return View(xmlResponse);
        }
    }
}
