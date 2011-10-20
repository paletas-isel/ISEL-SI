using System;
using System.Collections;
using System.Text;
using System.Security.Cryptography.X509Certificates;

namespace cgi
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding encoding = Encoding.Default;
            Console.WriteLine("Content-Type: text/html; charset={0}\r\n\r\n", encoding.WebName);
            Console.WriteLine("<html><head></head><body>");

            Console.WriteLine("<h1>SSL page info</h1>");

            // certString is the PEM-encoded client certificate (see http://httpd.apache.org/docs/2.2/mod/mod_ssl.html)            
            String certString = Environment.GetEnvironmentVariable("SSL_CLIENT_CERT");
            if (certString != null)
            {
                byte[] certBytes = new ASCIIEncoding().GetBytes(certString);
                X509Certificate2 certificate = new X509Certificate2(certBytes);

                Console.WriteLine("<h2>Client certificate info</h2><br/>");

                Console.WriteLine("Client Issuer = {0}<br/>",certificate.Issuer);
                Console.WriteLine("Client Subject = {0}<br/>", certificate.Subject);                
            }

            // serverCertString is the PEM-encoded server certificate (see http://httpd.apache.org/docs/2.2/mod/mod_ssl.html)            
            String serverCertString = Environment.GetEnvironmentVariable("SSL_SERVER_CERT");
            if (certString != null)
            {
                byte[] certBytes = new ASCIIEncoding().GetBytes(serverCertString);
                X509Certificate2 certificate = new X509Certificate2(certBytes);

                Console.WriteLine("<h2>Server certificate info</h2><br/>");

                Console.WriteLine("Server Issuer = {0}<br/>", certificate.Issuer);
                Console.WriteLine("Server Subject = {0}<br/> ", certificate.Subject);
            }

            if (certString == null && certString == null)
            {
                Console.WriteLine("No certificate available.");
            }
      
            Console.WriteLine("</body></html>");           
             
        }
    }
}
