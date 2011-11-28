using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GmailContacts.Models
{
    public class Contact
    {
        //public string FullName { get; set; }

        public string Photo { get; set; }

        //public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public static Contact ParseFromXML(IEnumerable<XElement> elements)
        {
            Contact c = new Contact();
            
            //XElement nameElement = (from item in elements
            //                        where item.Name.Equals(XName.Get("name", "http://www.w3.org/2005/Atom"))
            //                        select item.Element(XName.Get("fullName", "http://www.w3.org/2005/Atom"))).SingleOrDefault();

            //c.FullName = (nameElement != null) ? nameElement.Value : null; 

            c.Photo = (from item in elements
                       where item.Name.Equals(XName.Get("link", "http://www.w3.org/2005/Atom")) && item.Attribute(XName.Get("type")).Value.Equals("image/*")
                        select item.Attribute(XName.Get("href")).Value).FirstOrDefault();

            //c.PhoneNumber = (from item in elements
            //                 where item.Name.Equals(XName.Get("phoneNumber", "http://www.w3.org/2005/Atom"))
            //                 select item.Value).FirstOrDefault();

            c.Email = (from item in elements
                       where item.Name.Equals(XName.Get("email", "http://schemas.google.com/g/2005"))
                        select item.Attribute(XName.Get("address")).Value).FirstOrDefault();

            return c;

        }
    }
}