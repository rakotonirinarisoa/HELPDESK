using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Helpdesk
{
    // Remarque : pour obtenir des instructions sur l'activation du mode classique IIS6 ou IIS7, 
    // visitez http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            //System.Threading.Timer timer = new System.Threading.Timer(new TimerCallback(TimerElapsed), null, new TimeSpan(0), new TimeSpan(0, 1, 0));
        }

        //private void TimerElapsed(object o)
        //{
        //    using (System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage())
        //    {
        //        string mdpMail = "2454311391Soft";
        //        string MailAdresse = "dani.randriamamonjisoa@softwell.mg";

        //        SmtpClient smtp = new SmtpClient("smtpauth.moov.mg");

        //        smtp.UseDefaultCredentials = true;

        //        mail.From = new MailAddress(MailAdresse);

        //        mail.To.Add("rinah.raharinosy@softwell.mg");
        //        mail.Subject = "Demande du client";
        //        mail.Body = "Ceci est un test DANIELA-------------";

        //        smtp.Port = 587;

        //        smtp.Credentials = new System.Net.NetworkCredential(MailAdresse, mdpMail);
        //        smtp.EnableSsl = true;
        //        smtp.Send(mail);
        //    }
        //}
    }
}
