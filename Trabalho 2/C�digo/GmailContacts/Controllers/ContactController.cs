using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using System.Xml.Linq;
using GmailContacts.Extensions;
using GmailContacts.Models;

namespace GmailContacts.Controllers
{
    public class ContactController : Controller
    {
        //string redirectUri = @"http://localhost:43572/Contact/Authorized";
        const string RedirectUri = @"https://localhost:44300/Contact/Authorized";

        //
        // GET: /AllContacts/

        public ActionResult ObtainAll(string email)
        {
            string clientId = "19820391233.apps.googleusercontent.com";
            string scope = @"https://www.google.com/m8/feeds+https://www.googleapis.com/auth/userinfo.profile";

            string oauthEndpoint =
                String.Format(@"https://accounts.google.com/o/oauth2/auth?response_type=code&client_id={0}&redirect_uri={1}&scope={2}&state={3}", clientId, RedirectUri, scope, email);
        
            return Redirect(oauthEndpoint);
        }


        public ActionResult Authorized(string code, string state)
        {
            string clientId = "19820391233.apps.googleusercontent.com";
            string clientSecret = "WlLnme7lieXTOLOGM0Z-aeZJ";
            string grantType = "authorization_code"; 
            
            string tokenEndpoint = "https://accounts.google.com/o/oauth2/token";
            string tokenBody =     
                String.Format("code={0}&client_id={1}&client_secret={2}&redirect_uri={3}&grant_type={4}",
                                code, clientId, clientSecret, RedirectUri, grantType);

            HttpWebRequest postRequest = (HttpWebRequest) WebRequest.Create(tokenEndpoint);
            postRequest.Method = "POST";
            byte[] requestBody = System.Text.Encoding.UTF8.GetBytes(tokenBody);
            postRequest.ContentLength = requestBody.Length;
            postRequest.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
            postRequest.GetRequestStream().Write(requestBody, 0, requestBody.Length);

            string response = postRequest.GetStringResponse();

            Regex regex = new Regex(@".+""access_token"" : ""(?<token>.+)"".+");

            string accessToken = regex.Match(response).Groups["token"].Captures[0].Value;

            return All(accessToken, state);
        }

        public ActionResult All(string access_token, string email)
        {
            string emailEndpoint = String.Format(@"https://www.google.com/m8/feeds/contacts/{0}/full?access_token={1}", email, access_token);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(emailEndpoint);
            request.Method = "GET";

            string response = request.GetStringResponse();

            XDocument xmlData = XDocument.Parse(response);
            XElement rootElement = xmlData.Element(XName.Get("feed", "http://www.w3.org/2005/Atom"));

            IEnumerable<IEnumerable<XElement>> contactsElements = from item in rootElement.Elements()
                                                          where item.Name.Equals(XName.Get("entry", "http://www.w3.org/2005/Atom"))
                                                          select item.Elements();

            List<Contact> contacts = new List<Contact>();
            foreach (IEnumerable<XElement> contact in contactsElements)
            {
                 contacts.Add(Contact.ParseFromXML(contact));
            }

            return View("All", contacts);
        }

        public ActionResult Show(string xmlResponse)
        {
            return View(xmlResponse);
        }
    }
}
