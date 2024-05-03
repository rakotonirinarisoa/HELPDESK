using DevExpress.UnitConversion;
using Helpdesk.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Helpdesk.Controllers {
    public class DemandeController : Controller {

        ModelHELPD db = new ModelHELPD();

        //
        // GET: /Demande/

        public ActionResult Index(DateTime? dateDeb, DateTime? dateFin) {

            var list2d = new List<List<string>>();
            var dtDem = new DateTime();
            var dtFin = new DateTime();
            if (dateDeb == null)
            {
                dtDem = DateTime.Now;
            }
            else
            {
                dtDem = dateDeb.Value;
                dtFin = dateFin.Value;
            }

            try {
                if (Session["UserId"] != null) {
                    int idAgent = int.Parse(Session["UserId"].ToString());

                    if (db.users.Where(a => a.User_UserId == idAgent && a.User_Deleted != 1).Count() != 0) {
                        if (db.Crmcli_AffectationProds.Where(a => a.AFF_Agent.Contains(idAgent.ToString())).Count() != 0) {
                            foreach (var afprod in db.Crmcli_AffectationProds.Where(a => a.AFF_Agent.Contains(idAgent.ToString())).ToList()) {
                                if (db.Crmcli_Demandes.Where(a => a.ProduitDemande == afprod.AFF_Produit && a.DateDemande >= dtDem && a.DateDemande <= dtFin).Count() != 0) {
                                    foreach (var x in db.Crmcli_Demandes.Where(a => a.ProduitDemande == afprod.AFF_Produit && a.DateDemande >= dtDem).ToList()) {
                                        var list = new List<string>();

                                        //Num demande//0
                                        if (x.NumeroDemande != string.Empty) {
                                            list.Add(x.NumeroDemande);
                                        }
                                        else
                                            list.Add("");

                                        //Sujet demande//1
                                        if (x.SujetDemande != string.Empty) {
                                            list.Add(x.SujetDemande);
                                        }
                                        else
                                            list.Add("");

                                        //Description demande//2
                                        if (x.DescriptionDemande != string.Empty) {
                                            list.Add(x.DescriptionDemande);
                                        }
                                        else
                                            list.Add("");

                                        //TypeDemande//3
                                        //Type de prestation//
                                        var typeP = "";
                                        if (db.Cases.Where(a => a.case_numdemande == x.NumeroDemande && a.Case_Deleted != 1).Count() != 0) {
                                            var RefCases = db.Cases.Where(a => a.case_numdemande == x.NumeroDemande && a.Case_Deleted != 1).FirstOrDefault();

                                            if (db.Communication.Where(a => a.Comm_CaseId == RefCases.Case_CaseId && a.Comm_Deleted != 1).Count() != 0) {
                                                var communication = db.Communication.Where(a => a.Comm_CaseId == RefCases.Case_CaseId && a.Comm_Deleted != 1).FirstOrDefault();

                                                if (communication.comm_typepresta != null)//Type de prestation
                                                {
                                                    if (db.Custom_Captions.Where(a => a.Capt_Family == "comm_typepresta" && a.Capt_Code == communication.comm_typepresta && a.Capt_Deleted != 1).Count() != 0) {
                                                        var typeInterv = db.Custom_Captions.Where(a => a.Capt_Family == "comm_typepresta" && a.Capt_Code == communication.comm_typepresta && a.Capt_Deleted != 1).FirstOrDefault().Capt_FR;
                                                        typeP = typeInterv;
                                                    }
                                                }
                                            }
                                        }
                                        list.Add(typeP);

                                        //Produit//4
                                        if (db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted != 1 && a.Capt_Code == x.ProduitDemande.ToString()).Count() != 0) {
                                            var prod = db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted != 1 && a.Capt_Code == x.ProduitDemande.ToString()).FirstOrDefault();
                                            list.Add(prod.Capt_FR);
                                        }
                                        else
                                            list.Add("");

                                        //Priorité//5
                                        if (x.PrioriteDemande != null) {
                                            switch (x.PrioriteDemande) {
                                                case 1:
                                                    list.Add("Basse");
                                                    break;
                                                case 2:
                                                    list.Add("Moyenne");
                                                    break;
                                                case 3:
                                                    list.Add("Elevée");
                                                    break;
                                                case 4:
                                                    list.Add("Urgente");
                                                    break;
                                                default:
                                                    list.Add("Basse");
                                                    break;
                                            }
                                        }
                                        else
                                            list.Add("Basse");

                                        //Ticket en attache si Existe//6
                                        if (db.Cases.Where(a => a.case_numdemande == x.NumeroDemande && a.Case_Deleted != 1).Count() != 0) {
                                            var RefCases = db.Cases.Where(a => a.case_numdemande == x.NumeroDemande && a.Case_Deleted != 1).FirstOrDefault();

                                            if (RefCases.Case_Status != null) {
                                                if (db.Custom_Captions.Where(a => a.Capt_Family.ToLower() == "case_status" && a.Capt_Code == RefCases.Case_Status && a.Capt_Deleted != 1).Count() != 0) {
                                                    var sta = db.Custom_Captions.Where(a => a.Capt_Family.ToLower() == "case_status" && a.Capt_Code == RefCases.Case_Status && a.Capt_Deleted != 1).FirstOrDefault();
                                                    list.Add(sta.Capt_FR);
                                                }
                                                else
                                                    list.Add("En attente");
                                            }
                                            else
                                                list.Add("En attente");
                                        }
                                        else
                                            list.Add("En attente");

                                        //ID demande//7
                                        if (x.ID != 0) {
                                            list.Add(x.ID.ToString());
                                        }
                                        else
                                            list.Add("");

                                        //Date demande//8
                                        if (x.DateDemande != null) {
                                            list.Add(x.DateDemande.Value.ToString());
                                        }
                                        else
                                            list.Add("");

                                        //9//
                                        if (x.DatePropose != null)
                                            list.Add(x.DatePropose.Value.ToString());
                                        else
                                            list.Add("");

                                        //10//
                                        if (x.DateAccepte != null)
                                            list.Add(x.DateAccepte.Value.ToString());
                                        else
                                            list.Add("");

                                        //Date demande//11
                                        if (x.ComAnnulation != string.Empty) {
                                            list.Add(x.ComAnnulation);
                                        }
                                        else
                                            list.Add("");

                                        //Demandeur//12
                                        var demandeurName = "";
                                        if (x.Theme == null || db.Crmcli_Theme.Where(a => a.ID == x.Theme).FirstOrDefault().Theme == "Intervention demande client") {
                                            if (x.Demandeur != null && db.Person.Where(a => a.Pers_PersonId == x.Demandeur).Count() != 0) {
                                                var demandeur = db.Person.Where(a => a.Pers_PersonId == x.Demandeur).FirstOrDefault();
                                                demandeurName = demandeur.Pers_LastName + " " + demandeur.Pers_FirstName;
                                            }
                                        }
                                        else {
                                            var userAssig = db.users.Where(a => a.User_UserId == x.Demandeur && a.User_Deleted != 1).FirstOrDefault();
                                            if (userAssig != null) {
                                                demandeurName = string.Format("{0} {1}", userAssig.User_LastName, userAssig.User_FirstName);
                                            }
                                        }
                                        list.Add(demandeurName);


                                        //Ticket en attache si Existe//13
                                        if (db.Cases.Where(a => a.case_numdemande == x.NumeroDemande && a.Case_Deleted != 1).Count() != 0) {
                                            var RefCases = db.Cases.Where(a => a.case_numdemande == x.NumeroDemande && a.Case_Deleted != 1).FirstOrDefault();
                                            list.Add(RefCases.Case_ReferenceId);//REFERENCE CASES
                                        }
                                        else
                                            list.Add("");

                                        //date confirme de la demande//14
                                        if (db.Cases.Where(a => a.case_numdemande == x.NumeroDemande && a.Case_Deleted != 1).Count() != 0) {
                                            var RefCases = db.Cases.Where(a => a.case_numdemande == x.NumeroDemande && a.Case_Deleted != 1).FirstOrDefault();

                                            if (!RefCases.case_dateconfirme.Equals(null))
                                                list.Add(RefCases.case_dateconfirme.Value.ToString());
                                            else
                                                list.Add("");
                                        }
                                        else
                                            list.Add("");

                                        //15//
                                        list.Add(x.EtatDemande.ToString());

                                        //Etat accusés//16
                                        if (x.Etat_Accuse == true) {
                                            list.Add("1");
                                        }
                                        else {
                                            list.Add("0");
                                        }

                                        //CLIENT//17
                                        var clttttt = "";
                                        if (db.Company.Where(a => a.Comp_CompanyId == x.Comp_CompanyId && a.Comp_Deleted != 1).Count() != 0) {
                                            var companyFirst = db.Company.Where(a => a.Comp_CompanyId == x.Comp_CompanyId && a.Comp_Deleted != 1).FirstOrDefault();

                                            if (companyFirst.comp_raison_sociale_af_ay_iltx != null) {
                                                clttttt = companyFirst.comp_raison_sociale_af_ay_iltx;
                                            }
                                            else if (companyFirst.Comp_Name != null) {
                                                clttttt = companyFirst.Comp_Name;
                                            }
                                        }
                                        list.Add(clttttt);

                                        //Historique INTERVENTION//
                                        //if (db.Crmcli_HistoIntervs.Where(a => a.ID_Demandes == x.ID).Count() != 0)//17
                                        //{
                                        //    list.Add("1");
                                        //}
                                        //else
                                        //{
                                        //    list.Add("0");
                                        //}

                                        //Niveau//18
                                        if (x.NiveauDemande != null) {
                                            switch (x.NiveauDemande) {
                                                case 1:
                                                    list.Add("Non bloquant");
                                                    break;
                                                case 2:
                                                    list.Add("Bloquant");
                                                    break;
                                                default:
                                                    list.Add("Non bloquant");
                                                    break;
                                            }
                                        }
                                        else
                                            list.Add("Non bloquant");

                                        //Theme//19
                                        var theme = "Intervention demande client";
                                        if (x.Theme != null && x.Theme != 3)
                                            theme = db.Crmcli_Theme.Where(a => a.ID == x.Theme).FirstOrDefault().Theme;
                                        list.Add(theme);

                                        //Rubrique//
                                        var rubrique = "Sans rubrique";
                                        if (x.Rubrique == "1") {
                                            rubrique = "Dérogation";
                                        }
                                        else if (x.Rubrique == "0") {
                                            rubrique = "Sans rubrique";
                                        }
                                        else if (x.Rubrique != null && x.Rubrique != "0" && x.Rubrique != "1") {
                                            int rub = int.Parse(x.Rubrique);
                                            if (db.SSDI_SERVICE.Where(a => a.serv_SSDI_SERVICEid == rub).Count() != 0) {
                                                var rubName = db.SSDI_SERVICE.Where(a => a.serv_SSDI_SERVICEid == rub).FirstOrDefault().serv_name;
                                                rubrique = rubName;
                                            }
                                        }
                                        list.Add(rubrique);

                                        list2d.Add(list);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        return Content("Veuillez vous réconnectez");
                    }
                }
                var lis = list2d;
                ViewBag.ldv = lis;
                return View();
            }
            catch (Exception ex) {
                var x = ex.Message + ex.StackTrace;
                return View();
            }
        }


        [HttpPost]
        public ActionResult LuAcc(LuA collection) {
            try {
                var forLu = db.Crmcli_Demandes.Where(a => a.ID == collection.ID).FirstOrDefault();

                //Mail//
                string mdpMail = "";
                string MailAdresse = "";

                var destLu = "";
                if (db.Crmcli_AffectationProds.Where(a => a.AFF_Produit == forLu.ProduitDemande && a.AFF_Agent != null).Count() != 0) {
                    if (Session["UserId"] != null) {
                        using (var mail = new MailMessage()) {
                            var idA = int.Parse(Session["UserId"].ToString());

                            if (db.users.Where(a => a.User_UserId == idA && a.User_Deleted != 1 && a.user_pwdmail != null && a.User_EmailAddress != null).Count() != 0) {
                                var agent = db.users.Where(a => a.User_UserId == idA && a.User_Deleted != 1 && a.user_pwdmail != null && a.User_EmailAddress != null).FirstOrDefault();

                                mdpMail = agent.user_pwdmail;
                                MailAdresse = agent.User_EmailAddress;

                                mail.From = new MailAddress(MailAdresse);

                                var agentTomail = db.Crmcli_AffectationProds.Where(a => a.AFF_Produit == forLu.ProduitDemande).FirstOrDefault().AFF_Agent;

                                string[] separators = { "," };

                                if (agentTomail != null)//Agents
                                {
                                    string listUser = agentTomail.ToString();

                                    string[] agentL = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                    foreach (var ag in agentL) {
                                        int idAA = int.Parse(ag);
                                        if (db.users.Where(a => a.User_UserId == idAA && a.User_Deleted != 1).Count() != 0) {
                                            var userAssig = db.users.Where(a => a.User_UserId == idAA && a.User_Deleted != 1).FirstOrDefault();

                                            if (userAssig.User_EmailAddress != null) {
                                                destLu += userAssig.User_EmailAddress + ",";
                                            }
                                        }
                                    }
                                }

                                foreach (var address in destLu.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)) {
                                    mail.To.Add(address);
                                }
                            }

                            var cltName = "";
                            var companyFirst = db.Company.Where(a => a.Comp_CompanyId == forLu.Comp_CompanyId && a.Comp_Deleted != 1).FirstOrDefault();
                            if (companyFirst.comp_raison_sociale_af_ay_iltx != null)
                                cltName = companyFirst.comp_raison_sociale_af_ay_iltx;
                            else if (companyFirst.Comp_Name != null)
                                cltName = companyFirst.Comp_Name;

                            mail.Subject = "ACCUSE DE RECEPTION DE MAIL DE DEMANDE D’ASSISTANCE DU CLIENT : " + cltName.ToUpper();

                            var pathFile = Server.MapPath(string.Format("~/SIGNATURES/{0}.jpg", idA));

                            if (System.IO.File.Exists(pathFile)) {
                                mail.IsBodyHtml = true;

                                string pathACC = Server.MapPath("~/SIGNATURES/ACCMAILDEM.JPG");
                                var ImgACC = new LinkedResource(pathACC, MediaTypeNames.Image.Jpeg);
                                ImgACC.ContentId = "MyImageACC";

                                string path = Server.MapPath(string.Format("~/SIGNATURES/{0}.jpg", idA));
                                var Img = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
                                Img.ContentId = "MyImage";
                                string str = @"  
                                <table>
                                    <tr>  
                                        <td>  
                                            <img src=cid:MyImageACC  id='img' alt='' width='450px' height='195px'/><br/><br> 
                                        </td>  
                                    </tr>  
                                    <tr>  
                                        <td>" + "Bonjour,<br/><br>" +
                                                    "Nous confirmons la réception de votre demande, référencée sous le numéro " + forLu.NumeroDemande + ".<br/><br>" +
                                                    "Nous vous assurons que votre requête sera traitée dans les délais prévus par notre contrat d’assistance..<br/><br>" +
                                                    "Cordialement.<br/><br>" + @" 
                                        </td>  
                                    </tr>  
                                    <tr>  
                                        <td>  
                                            <img src=cid:MyImage  id='img' alt='' width='450px' height='195px'/>   
                                        </td>  
                                    </tr></table>  
                                ";
                                var AVACC = AlternateView.CreateAlternateViewFromString(str, null, MediaTypeNames.Text.Html);
                                AVACC.LinkedResources.Add(ImgACC);

                                mail.AlternateViews.Add(AVACC);

                                var AV = AlternateView.CreateAlternateViewFromString(str, null, MediaTypeNames.Text.Html);
                                AV.LinkedResources.Add(Img);

                                mail.AlternateViews.Add(AV);
                            }
                            else {
                                mail.IsBodyHtml = true;
                                string pathACC = Server.MapPath("~/SIGNATURES/ACCMAILDEM.JPG");
                                var ImgACC = new LinkedResource(pathACC, MediaTypeNames.Image.Jpeg);
                                ImgACC.ContentId = "MyImageACC";

                                string str = @"  
                                <table>
                                    <tr>  
                                        <td>  
                                            <img src=cid:MyImageACC  id='img' alt='' width='450px' height='195px'/><br/><br> 
                                        </td>  
                                    </tr>  
                                    <tr>  
                                        <td>" + "Bonjour,<br/><br>" +
                                                    "Nous accusons réception de votre demande portant le numéro " + forLu.NumeroDemande + ".<br/><br>" +
                                                    "Nous reviendrons vers vous dans un délai relatif à votre niveau de demande d'intervention, suivant les termes du contrat d'assistance.\r\n\r\n<br/><br>" +
                                                    "Cordialement.<br/><br>" + @" 
                                        </td>  
                                    </tr></table>  
                                ";

                                //mail.Body = "Bonjour,<br/><br>" +
                                //"Nous accusons réception de votre demande portant le numéro " + forLu.NumeroDemande + ".<br/><br>" +
                                //"Nous reviendrons vers vous dans les meilleurs délais.<br/><br>" + 
                                //"Cordialement.<br/><br>";
                            }

                            if (db.Person.Where(a => a.Pers_PersonId == forLu.Demandeur).Count() != 0) {
                                if (db.vPerson.Where(a => a.Pers_PersonId == forLu.Demandeur).Count() != 0) {
                                    var Perss = db.vPerson.Where(a => a.Pers_PersonId == forLu.Demandeur).FirstOrDefault();
                                    if (Perss.Pers_EmailAddress != null) {
                                        mail.To.Add(Perss.Pers_EmailAddress);

                                        if (forLu.Etat_Accuse != true) {
                                            if (Regex.IsMatch(MailAdresse, @"\bgmail\b")) //if (MailAdresse.Contains("gmail"))
                                            {
                                                var smtpClient = new SmtpClient("smtp.gmail.com") {
                                                    Port = 465,
                                                    Credentials = new System.Net.NetworkCredential(MailAdresse, mdpMail),
                                                    EnableSsl = true,
                                                    UseDefaultCredentials = false,
                                                    DeliveryMethod = SmtpDeliveryMethod.Network
                                                };

                                                smtpClient.Send(mail);
                                            }
                                            else {
                                                var smtp = new SmtpClient("smtpauth.moov.mg") {
                                                    UseDefaultCredentials = true,
                                                    Port = 587,
                                                    Credentials = new System.Net.NetworkCredential(MailAdresse, mdpMail),
                                                    EnableSsl = true
                                                };

                                                smtp.Send(mail);
                                            }

                                            forLu.Etat_Accuse = true;
                                            forLu.Consu_LU = idA;
                                            db.SaveChanges();

                                            return Content("Une accusés de reception a été envoyée!");
                                        }
                                        else
                                            return Content("Accusés de reception déjà envoyée!");
                                    }
                                    return Content("Veuillez vérifier l'adresse mail du destinataire (le contact)!");
                                }
                                return Content("Veuillez vérifier l'adresse mail du destinataire (le contact)!");
                            }
                            else
                                return Content("Veuillez vérifier l'adresse mail du destinataire (le contact)!");
                        }
                    }
                    else
                        return Content("Erreur. Reconnectez-vous!");
                }
                else
                    return Content("Erreur. Veuillez contacter l'Admin pour les destinataires de votre accusés de reception par rapport à ce produit!");

            }
            catch (Exception)//DbEntityValidationException e)
              {
                return Content("Erreur! Certains destinataires n'ont pas reçus, Mail non actif!");
            }
        }
        //
        // GET: /Demande/Details/5

        public ActionResult Details(int id) {
            var list2d = new List<List<string>>();
            var list = new List<string>();

            if (db.Crmcli_Demandes.Where(a => a.ID == id).Count() != 0) {
                var x = db.Crmcli_Demandes.Where(a => a.ID == id).FirstOrDefault();

                //Num demande//0
                if (x.NumeroDemande != string.Empty) {
                    list.Add(x.NumeroDemande);
                }
                else
                    list.Add("");

                //Sujet demande//1
                if (x.SujetDemande != string.Empty) {
                    list.Add(x.SujetDemande);
                }
                else
                    list.Add("");

                //Description demande//2
                if (x.DescriptionDemande != string.Empty) {
                    list.Add(x.DescriptionDemande);
                }
                else
                    list.Add("");

                //TypeDemande//
                //Type de prestation//3
                var typeP = "";
                if (db.Cases.Where(a => a.case_numdemande == x.NumeroDemande && a.Case_Deleted != 1).Count() != 0) {
                    var RefCases = db.Cases.Where(a => a.case_numdemande == x.NumeroDemande && a.Case_Deleted != 1).FirstOrDefault();

                    if (db.Communication.Where(a => a.Comm_CaseId == RefCases.Case_CaseId && a.Comm_Deleted != 1).Count() != 0) {
                        var communication = db.Communication.Where(a => a.Comm_CaseId == RefCases.Case_CaseId && a.Comm_Deleted != 1).FirstOrDefault();

                        if (communication.comm_typepresta != null)//Type de prestation
                        {
                            if (db.Custom_Captions.Where(a => a.Capt_Family == "comm_typepresta" && a.Capt_Code == communication.comm_typepresta && a.Capt_Deleted != 1).Count() != 0) {
                                var typeInterv = db.Custom_Captions.Where(a => a.Capt_Family == "comm_typepresta" && a.Capt_Code == communication.comm_typepresta && a.Capt_Deleted != 1).FirstOrDefault().Capt_FR;
                                typeP = typeInterv;
                            }
                        }
                    }
                }
                list.Add(typeP);

                //Produit//4
                if (db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted != 1 && a.Capt_Code == x.ProduitDemande.ToString()).Count() != 0) {
                    var prod = db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted != 1 && a.Capt_Code == x.ProduitDemande.ToString()).FirstOrDefault();
                    list.Add(prod.Capt_FR);
                }
                else
                    list.Add("");

                //Priorité//5
                if (x.PrioriteDemande != null) {
                    switch (x.PrioriteDemande) {
                        case 1:
                            list.Add("Basse");
                            break;
                        case 2:
                            list.Add("Moyenne");
                            break;
                        case 3:
                            list.Add("Elevée");
                            break;
                        case 4:
                            list.Add("Urgente");
                            break;
                        default:
                            list.Add("Basse");
                            break;
                    }
                }
                else
                    list.Add("Basse");

                //Ticket en attache si Existe//
                //ETAT//6
                var eta = "";
                if (db.Cases.Where(a => a.case_numdemande == x.NumeroDemande && a.Case_Deleted != 1).Count() != 0) {
                    var RefCases = db.Cases.Where(a => a.case_numdemande == x.NumeroDemande && a.Case_Deleted != 1).FirstOrDefault();

                    if (RefCases.Case_Status != null) {
                        if (db.Custom_Captions.Where(a => a.Capt_Family.ToLower() == "case_status" && a.Capt_Code == RefCases.Case_Status && a.Capt_Deleted != 1).Count() != 0) {
                            var sta = db.Custom_Captions.Where(a => a.Capt_Family.ToLower() == "case_status" && a.Capt_Code == RefCases.Case_Status && a.Capt_Deleted != 1).FirstOrDefault();
                            eta = sta.Capt_FR;
                        }
                    }
                }
                list.Add(eta);

                //ID demande//7
                if (x.ID != 0) {
                    list.Add(x.ID.ToString());
                }
                else
                    list.Add("");

                //Date demande//8
                if (x.DateDemande != null) {
                    list.Add(x.DateDemande.Value.ToString());
                }
                else
                    list.Add("");

                //Date propose9
                if (x.DatePropose != null)
                    list.Add(x.DatePropose.Value.ToString());
                else
                    list.Add("");

                //Date accepte/10
                var dateconf = "";
                if (db.Cases.Where(a => a.case_numdemande == x.NumeroDemande && a.Case_Deleted != 1).Count() != 0) {
                    var RefCases = db.Cases.Where(a => a.case_numdemande == x.NumeroDemande && a.Case_Deleted != 1).FirstOrDefault();

                    if (RefCases.case_dateconfirme.HasValue) {
                        dateconf = RefCases.case_dateconfirme.Value.ToString();
                    }
                }
                list.Add(dateconf);

                //Commentaire d'annulation//11
                if (x.ComAnnulation != string.Empty) {
                    list.Add(x.ComAnnulation);
                }
                else
                    list.Add("");

                //Demandeur//12
                var demandeurName = "";
                if (x.Theme == null || db.Crmcli_Theme.Where(a => a.ID == x.Theme).FirstOrDefault().Theme == "Intervention demande client") {
                    if (x.Demandeur != null && db.Person.Where(a => a.Pers_PersonId == x.Demandeur).Count() != 0) {
                        var demandeur = db.Person.Where(a => a.Pers_PersonId == x.Demandeur).FirstOrDefault();
                        demandeurName = demandeur.Pers_LastName + " " + demandeur.Pers_FirstName;
                    }
                }
                else {
                    var userAssig = db.users.Where(a => a.User_UserId == x.Demandeur && a.User_Deleted != 1).FirstOrDefault();
                    demandeurName = string.Format("{0} {1}", userAssig.User_LastName, userAssig.User_FirstName);
                }
                list.Add(demandeurName);

                list.Add(x.EtatDemande.ToString());//13

                if (x.NiveauDemande != null) {
                    switch (x.NiveauDemande) {
                        case 1:
                            list.Add("Non bloquant");
                            break;
                        case 2:
                            list.Add("Bloquant");
                            break;
                        default:
                            list.Add("Non bloquant");
                            break;
                    }
                }
                else
                    list.Add("Non bloquant");

                //commentaire priorité//
                if (x.CommentaireNivDemande != string.Empty) {
                    list.Add(x.CommentaireNivDemande);
                }
                else
                    list.Add("");

                //Historique relance//
                if (db.Crmcli_HistoriqueRelances.Where(a => a.ID_Demande == id).Count() != 0)//14
                {
                    list.Add("1");
                }
                else {
                    list.Add("0");
                }

                //client//
                var clientName = "";
                var companyFirst = db.Company.Where(a => a.Comp_CompanyId == x.Comp_CompanyId && a.Comp_Deleted != 1).FirstOrDefault();
                if (companyFirst.comp_raison_sociale_af_ay_iltx != null)
                    clientName = companyFirst.comp_raison_sociale_af_ay_iltx;
                else if (companyFirst.Comp_Name != null)
                    clientName = companyFirst.Comp_Name;

                list.Add(clientName);

                //Theme//19
                var theme = "Intervention demande client";
                if (x.Theme != null && x.Theme != 3)
                    theme = db.Crmcli_Theme.Where(a => a.ID == x.Theme).FirstOrDefault().Theme;
                list.Add(theme);

                //Rubrique//20
                var rubrique = "Sans rubrique";
                if (x.Rubrique == "1") {
                    rubrique = "Dérogation";
                }
                else if (x.Rubrique == "0") {
                    rubrique = "Sans rubrique";
                }
                else if (x.Rubrique != null && x.Rubrique != "0" && x.Rubrique != "1") {
                    int rub = int.Parse(x.Rubrique);
                    if (db.SSDI_SERVICE.Where(a => a.serv_SSDI_SERVICEid == rub).Count() != 0) {
                        var rubName = db.SSDI_SERVICE.Where(a => a.serv_SSDI_SERVICEid == rub).FirstOrDefault().serv_name;
                        rubrique = rubName;
                    }
                }
                list.Add(rubrique);

                list2d.Add(list);
            }

            //histoIntervention//
            var listHistoInt = new List<List<string>>();
            if (db.Crmcli_HistoIntervs.Where(a => a.ID_Demandes == id).Count() != 0) {
                foreach (var x in db.Crmcli_HistoIntervs.Where(a => a.ID_Demandes == id).ToList()) {
                    var listH = new List<string>();

                    //ID//0
                    listH.Add(x.ID.ToString());

                    //DateIntervention//1
                    var dt = "";
                    if (x.Date_Comm != null)
                        dt = x.Date_Comm.Value.ToShortDateString();

                    listH.Add(dt);

                    //Sujet//2
                    var sujI = "";
                    if (x.Sujets != null) {
                        listH.Add(x.Sujets);
                    }
                    else
                        listH.Add(sujI);

                    //Description//3
                    var descI = "";
                    if (x.Descriptions != null) {
                        listH.Add(x.Descriptions);
                    }
                    else
                        listH.Add(descI);

                    //Observation//4
                    var obsI = "";
                    if (x.Observations != null) {
                        listH.Add(x.Observations);
                    }
                    else
                        listH.Add(obsI);

                    //Debut//5
                    var debI = "";
                    if (x.Debut != null) {
                        listH.Add(x.Debut.Value.ToString());
                    }
                    else
                        listH.Add(debI);

                    //Fin//6
                    var FinI = "";
                    if (x.Fin != null) {
                        listH.Add(x.Fin.Value.ToString());
                    }
                    else
                        listH.Add(FinI);

                    //Validateur//7
                    var signataire = "";
                    if (x.ID_Pers_Validateur != null) {
                        if (db.Person.Where(a => a.Pers_PersonId == x.ID_Pers_Validateur && a.Pers_Deleted != 1).Count() != 0) {
                            var userAssig = db.Person.Where(a => a.Pers_PersonId == x.ID_Pers_Validateur && a.Pers_Deleted != 1).FirstOrDefault();
                            signataire = string.Format("{0} {1}-{2}", userAssig.Pers_LastName, userAssig.Pers_FirstName, userAssig.Pers_Title);

                            listH.Add(signataire);
                        }
                        else
                            listH.Add(signataire);
                    }
                    else
                        listH.Add(signataire);

                    //DateValidation//8
                    var dataI = "";
                    if (x.Date_Validation != null) {
                        listH.Add(x.Date_Validation.Value.ToShortDateString());
                    }
                    else
                        listH.Add(dataI);

                    //IP//9
                    var ipI = "";
                    if (x.IP_CheckPC != null) {
                        listH.Add(x.IP_CheckPC);
                    }
                    else
                        listH.Add(ipI);

                    //Agent//10
                    var agent = "";
                    string[] separators = { "," };
                    if (x.ID_Agent != null)//Agents
                    {
                        string listUser = x.ID_Agent.ToString();

                        string[] agentL = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var ag in agentL) {
                            int idA = int.Parse(ag);
                            if (db.users.Where(a => a.User_UserId == idA && a.User_Deleted != 1).Count() != 0) {
                                var userAssig = db.users.Where(a => a.User_UserId == idA && a.User_Deleted != 1).FirstOrDefault();

                                agent += string.Format("{0} {1},", userAssig.User_LastName, userAssig.User_FirstName);
                            }
                        }
                    }
                    listH.Add(agent);

                    //Etat//11
                    var etat = "";
                    if (db.Custom_Captions.Where(a => a.Capt_Family == "case_status" && a.Capt_Deleted != 1 && a.Capt_Code == x.EtatH).Count() != 0) {
                        etat = db.Custom_Captions.Where(a => a.Capt_Family == "case_status" && a.Capt_Deleted != 1 && a.Capt_Code == x.EtatH).FirstOrDefault().Capt_FR;
                    }
                    listH.Add(etat);

                    //Activité//12
                    var activite = "";
                    if (db.Custom_Captions.Where(a => a.Capt_Family == "comm_typepresta" && a.Capt_Deleted != 1 && a.Capt_Code == x.Activite).Count() != 0) {
                        activite = db.Custom_Captions.Where(a => a.Capt_Family == "comm_typepresta" && a.Capt_Deleted != 1 && a.Capt_Code == x.Activite).FirstOrDefault().Capt_FR;
                    }
                    listH.Add(activite);

                    //Nature//13
                    var nat = "";
                    if (db.Custom_Captions.Where(a => a.Capt_Family == "comm_nature" && a.Capt_Deleted != 1 && a.Capt_Code == x.Nature).Count() != 0) {
                        nat = db.Custom_Captions.Where(a => a.Capt_Family == "comm_nature" && a.Capt_Deleted != 1 && a.Capt_Code == x.Nature).FirstOrDefault().Capt_FR;
                    }
                    listH.Add(nat);

                    //Commentaire//14
                    var commentaire = "";
                    if (x.CommentairesH != null) {
                        commentaire = x.CommentairesH;
                    }
                    listH.Add(commentaire);

                    //Debut P1//15
                    var debP1 = "";
                    if (x.Debut_Pause1 != null) {
                        listH.Add(x.Debut_Pause1.Value.ToString());
                    }
                    else
                        listH.Add(debP1);

                    //Fin P2//16
                    var FinP1 = "";
                    if (x.Fin_Pause1 != null) {
                        listH.Add(x.Fin_Pause1.Value.ToString());
                    }
                    else
                        listH.Add(FinP1);

                    //Debut P1//17
                    var debP2 = "";
                    if (x.Debut_Pause2 != null) {
                        listH.Add(x.Debut_Pause2.Value.ToString());
                    }
                    else
                        listH.Add(debP2);

                    //Fin P2//18
                    var FinP2 = "";
                    if (x.Fin_Pause2 != null) {
                        listH.Add(x.Fin_Pause2.Value.ToString());
                    }
                    else
                        listH.Add(FinP2);

                    //lien//19
                    var lien = "";
                    if (x.Lien_Validation != null) {
                        listH.Add(x.Lien_Validation);
                    }
                    else
                        listH.Add(lien);

                    //Séance//20
                    var seance = "";
                    if (x.Seance != null) {
                        listH.Add(x.Seance);
                    }
                    else
                        listH.Add(seance);

                    //PersP//21
                    var PersP = "";
                    if (x.PERSP != null) {
                        listH.Add(x.PERSP);
                    }
                    else
                        listH.Add(PersP);
                    //22
                    var numhistro = "";
                    if (x.NumeroHisto != null)
                    {
                        numhistro = x.NumeroHisto.ToString();
                        listH.Add(numhistro);
                    }
                    else
                    {
                        listH.Add(numhistro);
                    }
                    listHistoInt.Add(listH);
                }
            }

            var lishisto = listHistoInt;
            ViewBag.lHisto = lishisto;

            var lis = list2d;
            ViewBag.ldv = lis;

            return View();
            //return View(db.Crmcli_Demandes.Where(a => a.ID == id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult HistoRel(Crmcli_Demandes collection) {
            try {
                var list2d = new List<List<string>>();

                if (collection.ID != 0) {
                    if (db.Crmcli_HistoriqueRelances.Where(a => a.ID_Demande == collection.ID).Count() != 0) {
                        foreach (var x in db.Crmcli_HistoriqueRelances.Where(a => a.ID_Demande == collection.ID).ToList()) {
                            var list = new List<string>();

                            list.Add(x.DateRelance.Value.ToString());

                            list2d.Add(list);
                        }
                    }
                }

                ViewBag.name = collection.NumeroDemande;

                var lis = list2d;
                ViewBag.ldv = lis;
                return View();
            }
            catch {
                return View();
            }
        }

        private static List<SelectListItem> PopulateAg() {
            var items = new List<SelectListItem>();
            var db = new ModelHELPD();

            if (db.users.Where(a => a.User_Deleted != 1 && a.User_Logon != null && a.User_Logon != "Admin").Count() != 0) {
                foreach (var x in db.users.Where(a => a.User_Deleted != 1 && a.User_Logon != null && a.User_Logon != "Admin").OrderBy(a => a.User_UserId).ToList()) {
                    var list = new List<string>();
                    items.Add(new SelectListItem {
                        Text = x.User_LastName + " " + x.User_FirstName,
                        Value = x.User_UserId.ToString()
                    });
                }
            }

            return items;
        }

        //FicheNew//
        public ActionResult FicheNew(int id) {
            var collection = new Crmcli_HistoIntervs();

            var eta = new List<string>();
            //Produit//
            if (db.Custom_Captions.Where(a => a.Capt_Family == "case_status" && a.Capt_Deleted != 1).Count() != 0) {
                foreach (var x in db.Custom_Captions.Where(a => a.Capt_Family == "case_status" && a.Capt_Deleted != 1).OrderBy(a => a.Capt_Order).ToList()) {
                    eta.Add(x.Capt_FR);
                }
            }

            //Activité//
            var activ = new List<string>();
            if (db.Custom_Captions.Where(a => a.Capt_Family == "comm_typepresta" && a.Capt_Deleted != 1 && a.Capt_Code.Length == 3).Count() != 0) {
                foreach (var x in db.Custom_Captions.Where(a => a.Capt_Family == "comm_typepresta" && a.Capt_Deleted != 1 && a.Capt_Code.Length == 3).OrderBy(a => a.Capt_Order).ToList()) {
                    activ.Add(x.Capt_FR);
                }
            }

            collection.Ag = PopulateAg();

            collection.EtatsCollection = eta;

            collection.TypePrestaCollection = activ;

            ViewBag.IDDemande = id;

            return View(collection);
        }

        public JsonResult Nat(string typepresta) {
            var items = new List<SelectListItem>();
            var db = new ModelHELPD();

            var presta = db.Custom_Captions.Where(a => a.Capt_Family == "comm_typepresta" && a.Capt_Deleted != 1 && a.Capt_Code.Length == 3 && a.Capt_FR == typepresta).FirstOrDefault().Capt_Code;

            foreach (var x in db.Custom_Captions.Where(a => a.Capt_Family == "comm_nature" && a.Capt_Deleted != 1 && a.Capt_Code.Length == 6).OrderBy(a => a.Capt_Order).ToList()) {
                if (x.Capt_Code.Substring(0, 3) == presta) {
                    items.Add(new SelectListItem {
                        Text = x.Capt_FR,
                        Value = x.Capt_Code
                    });
                }
            }

            var json = JsonConvert.SerializeObject(items);

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult FicheNewD(Crmcli_HistoIntervs collection ) {
            try {
                var IdDemandeur = 0;
                if (db.Crmcli_Demandes.Where(a => a.ID == collection.ID_Demandes).Count() != 0) {
                    var dem = db.Crmcli_Demandes.Where(a => a.ID == collection.ID_Demandes).FirstOrDefault();

                    collection.ID_Company = dem.Comp_CompanyId;

                    IdDemandeur = dem.Demandeur.Value;
                }

                if (IdDemandeur == 0)
                    return Content("Sans demandeur!");

                //date//
                var date = collection.Date_Comm.Value.ToString();
                collection.Date_Comm = DateTime.Parse(date.ToString()).Date;

                collection.DateSaisieHisto = DateTime.Now;

                //Etat//
                var eta = collection.EtatsCollection.FirstOrDefault();
                if (eta != null) {
                    if (db.Custom_Captions.Where(a => a.Capt_Family == "case_status" && a.Capt_Deleted != 1 && a.Capt_FR == eta).Count() != 0) {
                        var etat = db.Custom_Captions.Where(a => a.Capt_Family == "case_status" && a.Capt_Deleted != 1 && a.Capt_FR == eta).FirstOrDefault();
                        collection.EtatH = etat.Capt_Code;
                    }
                }
                else
                    return Content("Veuillez renseigner l'état de l'intervention!");

                //Activités//
                var activite = collection.TypePrestaCollection.FirstOrDefault();
                if (activite != null) {
                    if (db.Custom_Captions.Where(a => a.Capt_Family == "comm_typepresta" && a.Capt_Deleted != 1 && a.Capt_Code.Length == 3 && a.Capt_FR == activite).Count() != 0) {
                        var activitee = db.Custom_Captions.Where(a => a.Capt_Family == "comm_typepresta" && a.Capt_Deleted != 1 && a.Capt_Code.Length == 3 && a.Capt_FR == activite).FirstOrDefault();
                        collection.Activite = activitee.Capt_Code;
                    }
                }
                else
                    return Content("Veuillez renseigner l'activité de l'intervention!");

                //Nature//
                if (collection.Nature == null)
                    return Content("Veuillez renseigner la nature de l'intervention!");

                string idAgent = Session["UserId"].ToString() + ",";

                string idAgent2 = Session["UserId"].ToString();

                collection.ID_Agent = idAgent;

                var olombelona = "";
                if (collection.AgIds != null) {
                    foreach (var x in collection.AgIds) {
                        olombelona += x + ",";
                    }

                    collection.ID_Agent = olombelona;
                }

                if (olombelona == "")
                    return Content("Veuillez renseigner aux moins un(e) (01) intervenant(e)!");

                collection.SenderAgent = int.Parse(idAgent2);

                if (db.Crmcli_HistoIntervs.Where(a => a.ID_Company == collection.ID_Company && a.Date_Comm == collection.Date_Comm
                    && a.EtatH == collection.EtatH && a.ID_Agent == collection.ID_Agent && a.SenderAgent == collection.SenderAgent && a.Sujets == collection.Sujets
                    && a.Descriptions == collection.Descriptions).Count() != 0) {
                    return Content("Déjà saisie et envoyée!");
                }
                else {
                    db.Crmcli_HistoIntervs.Add(collection);
                    db.SaveChanges();
                }

                //CLIENT//
                var client = "";
                var companyFirst = db.Company.Where(a => a.Comp_CompanyId == collection.ID_Company && a.Comp_Deleted != 1).FirstOrDefault();
                if (companyFirst.comp_raison_sociale_af_ay_iltx != null) {
                    client = companyFirst.comp_raison_sociale_af_ay_iltx;
                }
                else if (companyFirst.Comp_Name != null) {
                    client = companyFirst.Comp_Name;
                }

                var lien = "";
                var db2 = new ModelHELPD();
                if (db2.Crmcli_HistoIntervs.Where(a => a.ID_Company == collection.ID_Company && a.EtatH == collection.EtatH && a.ID_Agent == collection.ID_Agent
                    && a.ID_Demandes == collection.ID_Demandes && a.Date_Comm == collection.Date_Comm && a.Sujets == collection.Sujets && a.Descriptions == collection.Descriptions).Count() != 0) {
                    var forupdate = db2.Crmcli_HistoIntervs.Where(a => a.ID_Company == collection.ID_Company && a.EtatH == collection.EtatH && a.ID_Agent == collection.ID_Agent
                    && a.ID_Demandes == collection.ID_Demandes && a.Date_Comm == collection.Date_Comm && a.Sujets == collection.Sujets && a.Descriptions == collection.Descriptions).FirstOrDefault();

                    forupdate.Lien_Validation = "https://softwell.cloud/helpdeskclient/Demande/ValiderInterv/" + forupdate.ID;

                    lien = "https://softwell.cloud/helpdeskclient/Demande/ValiderInterv/" + forupdate.ID;
                    db2.SaveChanges();
                }


                //SEND MAIL NOTIFICATION//
                string mdpMail = "";
                string MailAdresse = "";

                using (var mail = new MailMessage()) {
                    if (Session["UserId"] != null) {
                        var idA = int.Parse(Session["UserId"].ToString());

                        if (db.users.Where(a => a.User_UserId == idA && a.User_Deleted != 1 && a.user_pwdmail != null && a.User_EmailAddress != null).Count() != 0) {
                            var agent = db.users.Where(a => a.User_UserId == idA && a.User_Deleted != 1 && a.user_pwdmail != null && a.User_EmailAddress != null).FirstOrDefault();

                            mdpMail = agent.user_pwdmail;
                            MailAdresse = agent.User_EmailAddress;

                            var smtp = new SmtpClient("smtpauth.moov.mg");

                            smtp.UseDefaultCredentials = true;

                            mail.From = new MailAddress(MailAdresse);


                            collection.ID_Agent = idAgent;

                            var addr = new List<string>();


                            //Mail demandeur//
                            var demT = db.Crmcli_Demandes.Where(a => a.ID == collection.ID_Demandes).FirstOrDefault();
                            if (demT.Theme == null || demT.Theme == 3) {
                                if (IdDemandeur != 0) {
                                    if (db.Person.Where(a => a.Pers_PersonId == IdDemandeur).Count() != 0) {
                                        if (db.vPerson.Where(a => a.Pers_PersonId == IdDemandeur && a.Pers_EmailAddress != null).Count() != 0) {
                                            var demandeur = db.vPerson.Where(a => a.Pers_PersonId == IdDemandeur && a.Pers_EmailAddress != null).FirstOrDefault();

                                            if (addr.Contains(demandeur.Pers_EmailAddress) == false)
                                                addr.Add(demandeur.Pers_EmailAddress);
                                        }
                                    }
                                }
                            }
                            else {
                                var iscompp = demT.Comp_CompanyId;
                                if (db.Crmcli_UsersSession.Where(a => a.ID_Company == iscompp).Count() != 0) {
                                    foreach (var x in db.Crmcli_UsersSession.Where(a => a.ID_Company == iscompp).ToList()) {
                                        if (db.Person.Where(a => a.Pers_PersonId == x.ID_Person).Count() != 0) {
                                            if (db.vPerson.Where(a => a.Pers_PersonId == x.ID_Person && a.Pers_EmailAddress != null).Count() != 0) {
                                                var demandeur = db.vPerson.Where(a => a.Pers_PersonId == x.ID_Person && a.Pers_EmailAddress != null).FirstOrDefault();

                                                if (addr.Contains(demandeur.Pers_EmailAddress) == false)
                                                    addr.Add(demandeur.Pers_EmailAddress);
                                            }
                                        }
                                    }
                                }
                            }

                            //VALIDATE MAIL //
                            if (collection.AgIds != null) {
                                foreach (var x in collection.AgIds) {
                                    if (db.users.Where(a => a.User_UserId == x && a.User_Deleted != 1).Count() != 0) {
                                        var u = db.users.Where(a => a.User_UserId == x && a.User_Deleted != 1).FirstOrDefault();

                                        var agentTomail = u.user_mailhistorique;

                                        string[] separators = { "," };

                                        if (agentTomail != null)//Agents
                                        {
                                            string listUser = agentTomail.ToString();

                                            string[] agentL = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                            foreach (var ag in agentL) {
                                                if (addr.Contains(ag) == false)
                                                    addr.Add(ag);
                                            }
                                        }
                                    }
                                }
                            }

                            //MAIL AGENTS//
                            if (collection.AgIds != null) {
                                foreach (var x in collection.AgIds) {
                                    if (db.users.Where(a => a.User_UserId == x && a.User_Deleted != 1).Count() != 0) {
                                        var u = db.users.Where(a => a.User_UserId == x && a.User_Deleted != 1).FirstOrDefault();

                                        if (addr.Contains(u.User_EmailAddress) == false)
                                            addr.Add(u.User_EmailAddress);
                                    }
                                }
                            }

                            if (addr.Count() != 0) {
                                foreach (var m in addr) {
                                    mail.To.Add(m);
                                }
                            }

                            mail.To.Add("rinah.raharinosy@softwell.mg");

                            mail.Subject = "VALIDATION DE L'INTERVENTION DU " + collection.Date_Comm.Value.ToShortDateString() + " DU CLIENT : " + client.ToUpper();

                            var pathFile = Server.MapPath(string.Format("~/SIGNATURES/{0}.jpg", idA));

                            var numDemande = db.Crmcli_Demandes.Where(a => a.ID == collection.ID_Demandes).FirstOrDefault().NumeroDemande;

                            var bod = "Cher client,<br/><br>" +
                                "Suite à l’intervention effectuée relative à la demande N° " + numDemande +
                                ", nous souhaiterions avoir votre validation et votre appréciation sur l’assistance réalisée en date du " + collection.Date_Comm.Value.ToShortDateString() +
                                " en cliquant <a href='" + lien + "'>ici</a>ou directement à partir de la liste des interventions à valider dans le menu Demande via la plateforme helpdesk ci-après : https://softwell.cloud/helpdeskclient \".<br/><br>" +
                                "En vous remerciant de votre précieuse collaboration.<br/><br>" + "Cordialement,<br/><br>";

                            if (System.IO.File.Exists(pathFile)) {
                                mail.IsBodyHtml = true;
                                string path = Server.MapPath(string.Format("~/SIGNATURES/{0}.jpg", idA));
                                var Img = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
                                Img.ContentId = "MyImage";
                                string str = @"  
                                    <table>  
                                        <tr>  
                                            <td>" + bod + @" 
                                            </td>  
                                        </tr>
                                        <tr>  
                                            <td>  
                                                <img src=cid:MyImage  id='img' alt='' width='450px' height='195px'/>   
                                            </td>  
                                        </tr></table>  
                                    ";

                                var AV = AlternateView.CreateAlternateViewFromString(str, null, MediaTypeNames.Text.Html);
                                AV.LinkedResources.Add(Img);

                                mail.AlternateViews.Add(AV);
                            }
                            else {
                                mail.IsBodyHtml = true;
                                mail.Body = bod;
                            }

                            smtp.Port = 587;

                            smtp.Credentials = new System.Net.NetworkCredential(MailAdresse, mdpMail);
                            smtp.EnableSsl = true;

                            smtp.Send(mail);
                        }
                        else
                            return Content("Erreur mail du consultant!");
                    }
                    else
                        return Content("Erreur! Veuillez-vous connecter svp!");
                }

                return Content("Avec succès");//lien);
            }
            catch (Exception ex) {
                return Content("Erreur!" + ex.Message);
            }
        }


        public ActionResult ModifHeuR(int id) {
            ViewBag.IDDemande = id;

            return View(/*db.Crmcli_Demandes.Where(a => a.ID == id).FirstOrDefault()*/);
        }

        [HttpPost]
        public ActionResult ModifHeuR(HistoInterventions collection) {
            try {
                if (db.Crmcli_HistoIntervs.Where(a => a.ID == collection.ID).Count() != 0) {
                    //Update Historique Interventions//
                    var demHisto = db.Crmcli_HistoIntervs.Where(a => a.ID == collection.ID).FirstOrDefault();

                    demHisto.Date_Validation = DateTime.Now.Date;

                    //Heures Début Fin//
                    demHisto.Debut = TimeSpan.Parse(collection.Hdeb);
                    demHisto.Fin = TimeSpan.Parse(collection.Hfin);

                    //HEURES PAUSES//
                    demHisto.Debut_Pause1 = TimeSpan.Parse(collection.Debut_Pause1);
                    demHisto.Fin_Pause1 = TimeSpan.Parse(collection.Fin_Pause1);

                    demHisto.Debut_Pause2 = TimeSpan.Parse(collection.Debut_Pause2);
                    demHisto.Fin_Pause2 = TimeSpan.Parse(collection.Fin_Pause2);

                    db.SaveChanges();

                    ViewBag.IDDemande = collection.ID;

                    return Content("Avec succès!");
                }
                else
                    return Content("Erreur!");
            }
            catch (Exception) {
                return Content("Erreur!");
            }
        }

        [HttpPost]
        public ActionResult Delete(int id) {
            try {
                if (Session["UserId"] == null)
                    return Content("Erreur! Reconnectez-vous svp!");
                else if (!string.IsNullOrEmpty(id.ToString())) {
                    if (db.Crmcli_Demandes.Where(a => a.ID == id).Count() != 0) {
                        var isDelete = db.Crmcli_Demandes.Where(a => a.ID == id).FirstOrDefault();

                        if (db.Cases.Where(a => a.case_numdemande == isDelete.NumeroDemande && a.Case_Deleted != 1).Count() != 0) {
                            var ireoNumTickt = "";
                            foreach (var y in db.Cases.Where(a => a.case_numdemande == isDelete.NumeroDemande && a.Case_Deleted != 1).ToList()) {
                                if (y.Case_ReferenceId != null)
                                    ireoNumTickt += y.Case_ReferenceId + ", ";
                            }
                            return Content("Erreur! Rattachée à un ticket actif! ( " + ireoNumTickt + ")");
                        }
                        else {
                            db.Crmcli_Demandes.Remove(isDelete);
                            db.SaveChanges();

                            if (db.Crmcli_HistoIntervs.Where(a => a.ID_Demandes == id).Count() != 0) {
                                foreach (var x in db.Crmcli_HistoIntervs.Where(a => a.ID_Demandes == id).ToList()) {
                                    var delH = db.Crmcli_HistoIntervs.Where(a => a.ID == x.ID).FirstOrDefault();

                                    db.Crmcli_HistoIntervs.Remove(delH);
                                    db.SaveChanges();
                                }
                            }

                            if (db.Crmcli_Quests.Where(a => a.ID_Demandes == id).Count() != 0) {
                                foreach (var x in db.Crmcli_Quests.Where(a => a.ID_Demandes == id).ToList()) {
                                    var delH = db.Crmcli_Quests.Where(a => a.ID == x.ID).FirstOrDefault();

                                    db.Crmcli_Quests.Remove(delH);
                                    db.SaveChanges();
                                }
                            }

                            return Content("Suppression avec succès!");
                        }
                    }
                    else
                        return Content("Erreur!");
                }
                else
                    return Content("Erreur!");
            }
            catch (Exception)//DbEntityValidationException e)
              {
                return Content("Erreur!");
            }
        }

        [HttpPost]
        public ActionResult DeleteH(int id) {
            try {
                if (Session["UserId"] == null)
                    return Content("Erreur! Reconnectez-vous svp!");
                else if (!string.IsNullOrEmpty(id.ToString())) {
                    if (db.Crmcli_HistoIntervs.Where(a => a.ID == id).Count() != 0) {
                        var isDelete = db.Crmcli_HistoIntervs.Where(a => a.ID == id).FirstOrDefault();

                        if (isDelete.ID_Pers_Validateur != null) {
                            return Content("Erreur! Déjà validée");
                        }
                        else {
                            db.Crmcli_HistoIntervs.Remove(isDelete);
                            db.SaveChanges();

                            return Content("Suppression avec succès!");
                        }
                    }
                    else
                        return Content("Erreur!");
                }
                else
                    return Content("Erreur!");
            }
            catch (Exception)//DbEntityValidationException e)
            {
                return Content("Erreur!");
            }
        }

        public ActionResult ModifInterv(int id) {
            ViewBag.ID = id;

            var histor = db.Crmcli_HistoIntervs.Where(a => a.ID == id).FirstOrDefault();

            var date = "";
            if (histor.Date_Comm != null)
                date = histor.Date_Comm.Value.ToString("yyyy-MM-dd");
            var etau = "";
            if (histor.EtatH != null) {
                if (db.Custom_Captions.Where(a => a.Capt_Family == "case_status" && a.Capt_Deleted != 1 && a.Capt_Code == histor.EtatH).Count() != 0) {
                    etau = db.Custom_Captions.Where(a => a.Capt_Family == "case_status" && a.Capt_Deleted != 1 && a.Capt_Code == histor.EtatH).FirstOrDefault().Capt_FR;
                }
            }
            var subj = "";
            if (histor.Sujets != null)
                subj = histor.Sujets;
            var desc = "";
            if (histor.Descriptions != null)
                desc = histor.Descriptions;

            var seance = "";
            if (histor.Seance != null)
                seance = histor.Seance;

            ViewBag.Date = date;
            ViewBag.Etat = etau;
            ViewBag.Sujet = subj;
            ViewBag.Desc = desc;

            var collection = new Crmcli_HistoIntervs();

            var eta = new List<string>();
            if (db.Custom_Captions.Where(a => a.Capt_Family == "case_status" && a.Capt_Deleted != 1).Count() != 0) {
                foreach (var x in db.Custom_Captions.Where(a => a.Capt_Family == "case_status" && a.Capt_Deleted != 1).OrderBy(a => a.Capt_Order).ToList()) {
                    eta.Add(x.Capt_FR);
                }
            }

            collection.Ag = PopulateAg();

            collection.EtatsCollection = eta;

            return View(collection);
        }

        [HttpPost]
        public ActionResult ModifInterv(Crmcli_HistoIntervs collection) {
            try {
                if (db.Crmcli_HistoIntervs.Where(a => a.ID == collection.ID).Count() != 0) {
                    var isModif = db.Crmcli_HistoIntervs.Where(a => a.ID == collection.ID).FirstOrDefault();

                    if (isModif.ID_Pers_Validateur != null) {
                        return Content("Erreur! Déjà validée");
                    }
                    else {
                        //date//
                        var date = collection.Date_Comm.Value.ToString();
                        isModif.Date_Comm = DateTime.Parse(date.ToString()).Date;

                        //Etat//
                        var eta = collection.EtatsCollection.FirstOrDefault();
                        if (eta != null) {
                            if (db.Custom_Captions.Where(a => a.Capt_Family == "case_status" && a.Capt_Deleted != 1 && a.Capt_FR == eta).Count() != 0) {
                                var etat = db.Custom_Captions.Where(a => a.Capt_Family == "case_status" && a.Capt_Deleted != 1 && a.Capt_FR == eta).FirstOrDefault();
                                isModif.EtatH = etat.Capt_Code;
                            }
                        }

                        var olombelona = "";
                        if (collection.AgIds != null) {
                            foreach (var x in collection.AgIds) {
                                olombelona += x + ",";
                            }

                            isModif.ID_Agent = olombelona;
                        }

                        //sujet//
                        var subj = "";
                        if (!string.IsNullOrEmpty(collection.Sujets))
                            subj = collection.Sujets;
                        isModif.Sujets = subj;

                        //Descr//
                        var desc = "";
                        if (!string.IsNullOrEmpty(collection.Descriptions))
                            desc = collection.Descriptions;
                        isModif.Descriptions = desc;

                        //Descr//
                        var seance = "";
                        if (!string.IsNullOrEmpty(collection.Seance))
                            seance = collection.Seance;
                        isModif.Seance = seance;


                        if (olombelona == "")
                            return Content("Veuillez renseigner aux moins un(e) (01) intervenant(e)!");
                        else {
                            db.SaveChanges();

                            return Content("Modification avec succès!");
                        }
                    }
                }
                else
                    return Content("Erreur!");
            }
            catch (Exception) {
                return Content("Erreur!");
            }
        }

        //
        // GET: /Demande/Create
        public ActionResult Create(int id = 0) {
            var collection = new Crmcli_Demandes();

            var typeD = new List<string>();

            //TypeDemande//
            if (db.Custom_Captions.Where(a => a.Capt_Family == "comm_typepresta" && a.Capt_Deleted == null).Count() != 0) {
                foreach (var x in db.Custom_Captions.Where(a => a.Capt_Family == "comm_typepresta" && a.Capt_Deleted == null).ToList()) {
                    typeD.Add(x.Capt_FR);
                }
            }

            //Produit//
            /*List<string> produits = new List<string>();
            if (db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted == null).Count() != 0)
            {
                foreach (var x in db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted == null).ToList())
                {
                    produits.Add(x.Capt_FR);
                }
            }*/

            //Theme//
            var theme = new List<string>();
            if (Session["IsAdmin"] != null) {
                foreach (var x in db.Crmcli_Theme.Where(a => a.Theme != "Intervention demande client").OrderBy(a => a.Theme).ToList()) {
                    theme.Add(x.Theme);
                }
            }
            else {
                foreach (var x in db.Crmcli_Theme.Where(a => a.Theme != "Intervention demande client" && a.Theme != "Dérogation").OrderBy(a => a.Theme).ToList()) {
                    theme.Add(x.Theme);
                }
            }
            collection.ThemeCollection = theme;

            collection.TypeDemanCollection = typeD;
            //collection.ProduitsCollection = produits;

            var prio = new List<string>();
            prio.Add("");
            prio.Add("Basse");
            prio.Add("Moyenne");
            prio.Add("Elevée");
            collection.PrioritesCollection = prio;

            var nivo = new List<string>();
            nivo.Add("");
            nivo.Add("Non bloquant");
            nivo.Add("Bloquant");
            collection.NiveauCollection = nivo;

            var etat = new List<string>();
            etat.Add("En attente");
            etat.Add("En cours");
            etat.Add("Terminée");
            etat.Add("Annulée");
            collection.EtatCollection = etat;

            var cc = new List<List<string>>();
            var clients = new List<object>();
            foreach (var x in db.Crmcli_Reg.ToList()) {
                if (db.Company.Where(a => a.Comp_CompanyId == x.Comp_CompanyId).Count() != 0) {
                    var companyFirst = db.Company.Where(a => a.Comp_CompanyId == x.Comp_CompanyId && a.Comp_Deleted != 1).FirstOrDefault();

                    if (companyFirst.comp_raison_sociale_af_ay_iltx != null) {
                        //Fonds d'Intervention pour le Développement
                        clients.Add(new {
                            Text = companyFirst.comp_raison_sociale_af_ay_iltx,
                            Value = companyFirst.comp_raison_sociale_af_ay_iltx
                        });
                    }
                    else if (companyFirst.Comp_Name != null) {
                        clients.Add(new {
                            Text = companyFirst.Comp_Name,
                            Value = companyFirst.Comp_Name
                        });
                    }
                }
            }
            /*collection.ClientCollection = clt;

            var clients = db.Company
            .Select(s => new
            {
                Text = s.Comp_Name,
                Value = s.Comp_CompanyId
            })
            .ToList();*/

            ViewBag.ClientsList = new SelectList(clients, "Value", "Text");

            return View(collection);
        }

        public JsonResult GetRub(string prod, string cltName) {
            var items = new List<SelectListItem>();

            items.Add(new SelectListItem {
                Text = "Sans rubrique",
                Value = "Sans rubrique"
            });

            if (Session["IsAdmin"] != null) {
                items.Add(new SelectListItem {
                    Text = "Dérogation",
                    Value = "Dérogation"
                });
            }

            var codeProd = "0";
            if (db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted == null && a.Capt_FR == prod).Count() != 0) {
                codeProd = db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted == null && a.Capt_FR == prod).FirstOrDefault().Capt_Code;
            }

            if (codeProd != "0") {
                int idACl = db.Company.Where(a => (a.Comp_Name == cltName || a.comp_raison_sociale_af_ay_iltx == cltName) && a.Comp_Deleted != 1).FirstOrDefault().Comp_CompanyId;
                DateTime dDepart = DateTime.Now;
                if (db.Company.Where(a => a.Comp_CompanyId == idACl).FirstOrDefault().comp_corespondance != null) {
                    var coresp = db.Company.Where(a => a.Comp_CompanyId == idACl).FirstOrDefault().comp_corespondance;

                    foreach (var x in db.Company.Where(a => a.comp_corespondance == coresp).ToList()) {
                        if (db.SSDI_BUSINESS.Where(a => a.busi_companyid == x.Comp_CompanyId && a.busi_datefin >= dDepart && a.busi_dateinit <= dDepart && a.busi_Deleted != 1).Count() != 0) {
                            foreach (var y in db.SSDI_BUSINESS.Where(a => a.busi_companyid == x.Comp_CompanyId && a.busi_datefin >= dDepart && a.busi_dateinit <= dDepart && a.busi_Deleted != 1).ToList()) {
                                if (db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted == null && a.Capt_Code == codeProd).Count() != 0) {
                                    if (db.SSDI_SERVICE.Where(a => a.serv_SSDI_BUSINESSid == y.busi_SSDI_BUSINESSid && a.serv_Deleted != 1).Count() != 0) {
                                        foreach (var z in db.SSDI_SERVICE.Where(a => a.serv_SSDI_BUSINESSid == y.busi_SSDI_BUSINESSid && a.serv_Deleted != 1).ToList()) {
                                            items.Add(new SelectListItem {
                                                Text = z.serv_name,
                                                Value = z.serv_name
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            var json = JsonConvert.SerializeObject(items);

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNumHisto(int IDemande)
        {
            string idPerm = "1";
            string numTicket = "";
            int IDD = 0;
            string idDemandeS = IDemande.ToString() + "/";
            ModelHELPD db = new ModelHELPD();
            var Compteurtemp = db.Crmcli_HistoIntervs.Where( a => a.ID_Demandes == IDemande && a.NumeroHisto != null).ToList().OrderBy(a => a.NumeroHisto);
            var Compteur = "";

            foreach (var item in Compteurtemp)
            {
                Compteur = item.NumeroHisto.ToString();
            }
            
            int numOk = int.Parse(idPerm);
            if (Compteur != "")
            {
                var zzz = Compteur.Split('/');
                var item = "";
                if (zzz.Length > 0)
                {
                    item = zzz[zzz.Length - 1];
                }

                numOk = int.Parse(item);
                numOk = numOk + 1;
                if (numOk <= 99999)
                {
                    if (numOk.ToString().Length == 1)
                    {
                        numTicket = string.Format("{0}0000{1}", idDemandeS, numOk);
                    }
                    else if (numOk.ToString().Length == 2)
                    {
                        numTicket = string.Format("{0}000{1}", idDemandeS, numOk);
                    }
                    else if (numOk.ToString().Length == 3)
                    {
                        numTicket = string.Format("{0}00{1}", idDemandeS, numOk);
                    }
                    else if (numOk.ToString().Length == 4)
                    {
                        numTicket = string.Format("{0}0{1}", idDemandeS, numOk);
                    }
                    else if (numOk.ToString().Length == 5)
                    {
                        numTicket = idDemandeS + numOk;
                    }
                }
                string idHistorique = numTicket;
                var json = JsonConvert.SerializeObject(idHistorique);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            else
            {
                
                if (numOk <= 99999)
                {
                    if (numOk.ToString().Length == 1)
                    {
                        numTicket = string.Format("{0}0000{1}", idDemandeS, numOk);
                    }
                    else if (numOk.ToString().Length == 2)
                    {
                        numTicket = string.Format("{0}000{1}", idDemandeS, numOk);
                    }
                    else if (numOk.ToString().Length == 3)
                    {
                        numTicket = string.Format("{0}00{1}", idDemandeS, numOk);
                    }
                    else if (numOk.ToString().Length == 4)
                    {
                        numTicket = string.Format("{0}0{1}", idDemandeS, numOk);
                    }
                    else if (numOk.ToString().Length == 5)
                    {
                        numTicket = idDemandeS + numOk;
                    }
                }
                string idHistorique = numTicket;
                var json = JsonConvert.SerializeObject(idHistorique);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetProd(string cltName) {
            var produits = new List<SelectListItem>();

            produits.Add(new SelectListItem {
                Text = "",
                Value = ""
            });

            int idACl = 0;
            foreach (var i in db.Company.Where(a => a.comp_raison_sociale_af_ay_iltx == cltName || a.Comp_Name == cltName)) {
                var cltizy = db.Company.Where(a => a.Comp_CompanyId == i.Comp_CompanyId).FirstOrDefault();

                if (db.Crmcli_Reg.Where(a => a.Comp_CompanyId == cltizy.Comp_CompanyId).Count() != 0) {
                    idACl = cltizy.Comp_CompanyId;
                }
            }

            DateTime dDepart = DateTime.Now;
            if (db.Company.Where(a => a.Comp_CompanyId == idACl).FirstOrDefault().comp_corespondance != null) {
                var coresp = db.Company.Where(a => a.Comp_CompanyId == idACl).FirstOrDefault().comp_corespondance;

                foreach (var x in db.Company.Where(a => a.comp_corespondance == coresp).ToList()) {
                    if (db.SSDI_BUSINESS.Where(a => a.busi_companyid == x.Comp_CompanyId && a.busi_datefin > dDepart).Count() != 0) {
                        foreach (var y in db.SSDI_BUSINESS.Where(a => a.busi_companyid == x.Comp_CompanyId && a.busi_datefin > dDepart).ToList()) {
                            if (db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted == null && a.Capt_Code == y.busi_name2).Count() != 0) {
                                if (y.busi_name3 == null) {
                                    var prodd = db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted == null && a.Capt_Code == y.busi_name2).FirstOrDefault().Capt_FR;

                                    produits.Add(new SelectListItem {
                                        Text = prodd,
                                        Value = prodd
                                    });
                                }
                                else {
                                    string[] separators = { "," };
                                    string listUser = y.busi_name3.ToString();
                                    string[] agentL = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                    foreach (string ag in agentL) {
                                        if (db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted == null && a.Capt_Code == ag).Count() != 0) {
                                            var prodd = db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted == null && a.Capt_Code == ag).FirstOrDefault().Capt_FR;

                                            produits.Add(new SelectListItem {
                                                Text = prodd,
                                                Value = prodd
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            var json = JsonConvert.SerializeObject(produits);

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult NumdeT(string cltName) {
            var db = new ModelHELPD();

            var idclt = 0;
            foreach (var x in db.Company.Where(a => a.comp_raison_sociale_af_ay_iltx == cltName || a.Comp_Name == cltName).ToList()) {
                if (db.Crmcli_Reg.Where(a => a.Comp_CompanyId == x.Comp_CompanyId).Count() != 0)
                    idclt = x.Comp_CompanyId;
            }

            string numTicket = "";

            if (db.Company.Where(a => a.Comp_CompanyId.Equals(idclt) && idclt != 0).Count() != 0) {
                var getC = db.Crmcli_Reg.Where(a => a.Comp_CompanyId == idclt).FirstOrDefault();
                //Numéro//
                var abreg = "";
                if (getC.Comp_CompanyId != null) {
                    abreg = getC.Comp_CompanyId.ToString() + "/";
                }
                //Num chiffre//
                var numC = 0;
                var numOK = 0;
                if (db.Crmcli_Demandes.Where(a => a.Comp_CompanyId == idclt).Count() != 0) {
                    foreach (var y in db.Crmcli_Demandes.Where(a => a.Comp_CompanyId == idclt).ToList()) {
                        var forTest = y.NumeroDemande.Substring(abreg.Length);
                        if (numC < int.Parse(forTest)) {
                            numC = int.Parse(forTest);
                        }
                    }
                    numOK = numC + 1;
                }
                //Format numéro à 5 chiffres//
                if (numOK <= 99999) {
                    if (numOK.ToString().Length == 1) {
                        numTicket = string.Format("{0}0000{1}", abreg, numOK);
                    }
                    else if (numOK.ToString().Length == 2) {
                        numTicket = string.Format("{0}000{1}", abreg, numOK);
                    }
                    else if (numOK.ToString().Length == 3) {
                        numTicket = string.Format("{0}00{1}", abreg, numOK);
                    }
                    else if (numOK.ToString().Length == 4) {
                        numTicket = string.Format("{0}0{1}", abreg, numOK);
                    }
                    else if (numOK.ToString().Length == 5) {
                        numTicket = abreg + numOK;
                    }
                }
            }

            var json = JsonConvert.SerializeObject(numTicket);

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateD(Crmcli_Demandes collection) {
            //Crmcli_Demandes collection = Formulaire.collection;

            //string typeprod = Formulaire.typeprod;
            //return null;
            try {
                if (collection.ProduitDemande == null)
                    return Content("Veuillez sélectionner le produit de votre demande!");
                if (collection.Rubrique == null)
                    return Content("Veuillez sélectionner le rubrique de votre demande!");
                if (collection.ClientCollection == null)
                    return Content("Veuillez sélectionner le client!");
                if (collection.ThemeCollection == null)
                    return Content("Veuillez sélectionner le thème!");

                var rubriqueName = collection.Rubrique;
                var produitName = collection.ProduitDemande;
                var idContrat = 0;
                var CanCreate = false;

                //Blocage création demande si il existe des interventions debut janvier non validées//
                var dD = new DateTime(2021, 12, 31, 23, 59, 59);

                var idCompany = 0;
                var cltName = collection.ClientCollection;
                foreach (var x in db.Company.Where(a => a.comp_raison_sociale_af_ay_iltx == cltName || a.Comp_Name == cltName).ToList()) {
                    if (db.Crmcli_Reg.Where(a => a.Comp_CompanyId == x.Comp_CompanyId).Count() != 0)
                        idCompany = x.Comp_CompanyId;
                }
                //eto mila regle pour modifier la liste

                var verifications = db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_FR.Contains(collection.ProduitDemande)).Select(x => x.Capt_Order).FirstOrDefault();
                var demandesVerifications = db.Crmcli_Demandes.Where(xc => xc.ProduitDemande == verifications.ToString()).Select(xc => xc.ID).ToList();
                var histointerveverifications = new List<Crmcli_HistoIntervs>();
                var idDemandeHistoIntev = db.Crmcli_HistoIntervs.Where(a => a.ID_Company == idCompany && a.ID_Pers_Validateur == null && a.Date_Comm > dD).Select(x => x.ID_Demandes).ToList();
                bool marina = false;
                foreach (var item in demandesVerifications)
                {
                    foreach (var id in idDemandeHistoIntev)
                    {
                        if(item == id)
                        {
                            marina = true;
                            break;
                        }
                    }
                }
                if (marina) {
                    return Content("Veuillez valider les interventions non encore validées avant de faire une nouvelle demande!");
                }

                //if (db.Crmcli_HistoIntervs.Where(a => a.ID_Company == idCompany && a.ID_Pers_Validateur == null && a.Date_Comm > dD).Count() != 0)
                //                return Content("Veuillez valider les interventions restant à partir du 01/01/2022 avant d'effectuer une nouvelle demande!");
                //else if (db.Crmcli_HistoIntervs.Where(a => a.ID_Company == idCompany && a.ID_Pers_Validateur == null && a.Date_Comm > dD).Count() == 0)
                //{
                if (rubriqueName == "Sans rubrique" || rubriqueName == "Dérogation") {
                    CanCreate = true;
                }
                else {
                    DateTime dDepart = DateTime.Now;
                    if (db.Company.Where(a => a.Comp_CompanyId == idCompany).FirstOrDefault().comp_corespondance != null) {
                        var coresp = db.Company.Where(a => a.Comp_CompanyId == idCompany).FirstOrDefault().comp_corespondance;
                        foreach (var x in db.Company.Where(a => a.comp_corespondance == coresp).ToList()) {
                            if (db.SSDI_BUSINESS.Where(a => a.busi_companyid == x.Comp_CompanyId && a.busi_datefin >= dDepart && a.busi_dateinit <= dDepart && a.busi_Deleted != 1).Count() != 0) {
                                foreach (var y in db.SSDI_BUSINESS.Where(a => a.busi_companyid == x.Comp_CompanyId && a.busi_datefin >= dDepart && a.busi_dateinit <= dDepart && a.busi_Deleted != 1).ToList()) {
                                    if (db.SSDI_SERVICE.Where(a => a.serv_SSDI_BUSINESSid == y.busi_SSDI_BUSINESSid && a.serv_name == rubriqueName && a.serv_Deleted != 1).Count() != 0) {
                                        idContrat = y.busi_SSDI_BUSINESSid;
                                    }
                                }
                            }
                        }
                    }
                }

                if (idContrat == 0) {
                    if (rubriqueName != "Sans rubrique" && rubriqueName != "Dérogation")
                        return Content("Vous ne pouvez pas effectuer une nouvelle demande! Contrat(s) expiré(s)");
                }
                else if (idContrat != 0 && rubriqueName != "Sans rubrique" && rubriqueName != "Dérogation") {
                    var isContrat = db.SSDI_BUSINESS.Where(a => a.busi_SSDI_BUSINESSid == idContrat).FirstOrDefault();

                    if (isContrat.busi_nbjrlim == null || isContrat.busi_nbjrlim == "02")
                        CanCreate = true;
                    else if (isContrat.busi_nbjrlim == "01") {
                        double TotalNbrJourMinute = 0;
                        double TotalIntervMinute = 0;
                        var Service = db.SSDI_SERVICE.Where(a => a.serv_SSDI_BUSINESSid == idContrat && a.serv_name == rubriqueName && a.serv_Deleted != 1).FirstOrDefault();
                        //Total nombre de jour limite par service en minutes//
                        if (Service.serv_nbjr != null && Service.serv_nbjr != 0)
                            TotalNbrJourMinute = Convert.ToDouble(Service.serv_nbjr.Value * 8 * 60);

                        //Get total heure travaillé des intervention de type Assistance entre les dates du contrat pour le client//
                        //All type assistance apart à distance tel ou mail//
                        int idACl = int.Parse(Session["UserId"].ToString());
                        if (db.Company.Where(a => a.Comp_CompanyId == idACl).FirstOrDefault().comp_corespondance != null) {
                            var coresp = db.Company.Where(a => a.Comp_CompanyId == idACl).FirstOrDefault().comp_corespondance;

                            foreach (var x in db.Company.Where(a => a.comp_corespondance == coresp).ToList()) {
                                foreach (var y in (from cmd in db.Comm_Link
                                                   join cmp in db.Company
                                                   on cmd.CmLi_Comm_CompanyId equals cmp.Comp_CompanyId
                                                   join com in db.Communication
                                                   on cmd.CmLi_Comm_CommunicationId equals com.Comm_CommunicationId
                                                   where com.Comm_ToDateTime >= isContrat.busi_dateinit && com.Comm_ToDateTime <= isContrat.busi_datefin
                                                   && com.Comm_Deleted != 1 && cmp.Comp_CompanyId == x.Comp_CompanyId
                                                   && com.Comm_SSDI_BUSINESSId == isContrat.busi_SSDI_BUSINESSid && com.Comm_SSDI_SERVICEId == Service.serv_SSDI_SERVICEid
                                                   && com.comm_typeass != "03" && com.comm_site == "02"
                                                   && (com.comm_acceshotline == "02" || com.comm_acceshotline == "03" || com.comm_acceshotline == null)
                                                   && com.comm_typepresta == "002"
                                                   && (com.comm_nature == "002005" || com.comm_nature == "002007" || com.comm_nature == "002008" || com.comm_nature == "002011")
                                                   select new {
                                                       idCommu = com.Comm_CommunicationId
                                                   }).ToList()) {
                                    var commU = db.Communication.Where(a => a.Comm_CommunicationId == y.idCommu).FirstOrDefault();
                                    var pau = TimeSpan.FromMinutes(Convert.ToDouble(commU.comm_pause)).TotalMinutes;

                                    TotalIntervMinute += TimeSpan.FromMinutes(Convert.ToDouble(((commU.comm_datefin2.Value - commU.Comm_ToDateTime.Value).TotalMinutes - pau))).TotalMinutes;
                                }
                            }
                        }

                        if (TotalIntervMinute < TotalNbrJourMinute) {
                            CanCreate = true;
                        }
                    }
                }
                //}

                if (CanCreate == false) {
                    return Content("Vous ne pouvez pas effectuer une nouvelle demande! Limite dépassée");
                }
                else {
                    //Services//SSDI_SERVICE//
                    DateTime dDepart = DateTime.Now;
                    var rubrique = "0";
                    if (rubriqueName == "Dérogation") {
                        rubrique = "1";
                    }
                    else if (rubriqueName == "Sans rubrique") {
                        rubrique = "0";
                    }

                    if (db.Company.Where(a => a.Comp_CompanyId == idCompany).FirstOrDefault().comp_corespondance != null) {
                        var coresp = db.Company.Where(a => a.Comp_CompanyId == idCompany).FirstOrDefault().comp_corespondance;

                        foreach (var x in db.Company.Where(a => a.comp_corespondance == coresp).ToList()) {
                            if (db.SSDI_BUSINESS.Where(a => a.busi_companyid == x.Comp_CompanyId && a.busi_datefin > dDepart && a.busi_Deleted != 1).Count() != 0) {
                                foreach (var y in db.SSDI_BUSINESS.Where(a => a.busi_companyid == x.Comp_CompanyId && a.busi_datefin > dDepart && a.busi_Deleted != 1).ToList()) {
                                    if (db.SSDI_SERVICE.Where(a => a.serv_SSDI_BUSINESSid == y.busi_SSDI_BUSINESSid && a.serv_Deleted != 1).Count() != 0) {
                                        foreach (var z in db.SSDI_SERVICE.Where(a => a.serv_SSDI_BUSINESSid == y.busi_SSDI_BUSINESSid && a.serv_Deleted != 1).ToList()) {
                                            if (z.serv_name == rubriqueName)
                                                rubrique = z.serv_SSDI_SERVICEid.ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    collection.Rubrique = rubrique;

                    //Demandeur interne//
                    var idAgent = int.Parse(Session["UserId"].ToString());
                    collection.Demandeur = idAgent;

                    var userAssig = db.users.Where(a => a.User_UserId == idAgent && a.User_Deleted != 1).FirstOrDefault();
                    var NameAgent = string.Format("{0} {1}", userAssig.User_LastName, userAssig.User_FirstName);
                    var MailAgent = string.Format("{0}", userAssig.User_EmailAddress);
                    var TelAgent = "";
                    if (userAssig.User_MobilePhone != null)
                        TelAgent = string.Format("{0}", userAssig.User_MobilePhone);

                    //Thème//
                    var thenme = collection.ThemeCollection.FirstOrDefault();
                    if (db.Crmcli_Theme.Where(a => a.Theme == thenme).Count() != 0) {
                        collection.Theme = db.Crmcli_Theme.Where(a => a.Theme == thenme).FirstOrDefault().ID;
                    }

                    //NumeroDemande//
                    string numTicket = "";
                    if (db.Company.Where(a => a.Comp_CompanyId.Equals(idCompany) && idCompany != 0).Count() != 0) {
                        var getC = db.Crmcli_Reg.Where(a => a.Comp_CompanyId == idCompany).FirstOrDefault();
                        //Numéro//
                        var abreg = "";
                        if (getC.Comp_CompanyId != null) {
                            abreg = getC.Comp_CompanyId.ToString() + "/";
                        }
                        //Num chiffre//
                        var numC = 0;
                        var numOK = 0;
                        if (db.Crmcli_Demandes.Where(a => a.Comp_CompanyId == idCompany).Count() != 0) {
                            foreach (var y in db.Crmcli_Demandes.Where(a => a.Comp_CompanyId == idCompany).ToList()) {
                                var forTest = y.NumeroDemande.Substring(abreg.Length);
                                if (numC < int.Parse(forTest)) {
                                    numC = int.Parse(forTest);
                                }
                            }
                            numOK = numC + 1;
                        }
                        //Format numéro à 5 chiffres//
                        if (numOK <= 99999) {
                            if (numOK.ToString().Length == 1) {
                                numTicket = string.Format("{0}0000{1}", abreg, numOK);
                            }
                            else if (numOK.ToString().Length == 2) {
                                numTicket = string.Format("{0}000{1}", abreg, numOK);
                            }
                            else if (numOK.ToString().Length == 3) {
                                numTicket = string.Format("{0}00{1}", abreg, numOK);
                            }
                            else if (numOK.ToString().Length == 4) {
                                numTicket = string.Format("{0}0{1}", abreg, numOK);
                            }
                            else if (numOK.ToString().Length == 5) {
                                numTicket = abreg + numOK;
                            }
                        }
                    }
                    collection.NumeroDemande = numTicket;

                    //Produit//
                    var prod = produitName;
                    if (db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted == null && a.Capt_FR == prod).Count() != 0) {
                        var produit = db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted == null && a.Capt_FR == prod).FirstOrDefault();
                        collection.ProduitDemande = produit.Capt_Code;
                    }

                    //Prioritée//
                    var prio = collection.PrioritesCollection.FirstOrDefault();
                    if (prio != null) {
                        switch (prio) {
                            case "Basse":
                                collection.PrioriteDemande = 1;
                                break;
                            case "Moyenne":
                                collection.PrioriteDemande = 2;
                                break;
                            case "Elevée":
                                collection.PrioriteDemande = 3;
                                break;
                            case "":
                                collection.PrioriteDemande = 1;
                                break;
                        }
                    }
                    else
                        collection.PrioriteDemande = 1;

                    //Niveau//
                    var nivo = collection.NiveauCollection.FirstOrDefault();
                    if (nivo != null) {
                        switch (nivo) {
                            case "Non bloquant":
                                collection.NiveauDemande = 1;
                                break;
                            case "Bloquant":
                                collection.NiveauDemande = 2;
                                break;
                            case "":
                                collection.NiveauDemande = 1;
                                break;
                        }
                    }
                    else
                        collection.NiveauDemande = 1;

                    collection.EtatDemande = 1;

                    //ID COMP//
                    collection.Comp_CompanyId = idCompany;

                    collection.DateDemande = DateTime.Now;

                    if (collection.DatePropose == null) {
                        collection.DatePropose = collection.DateDemande;
                    }

                    if (db.Crmcli_Demandes.Where(a => a.SujetDemande == collection.SujetDemande && a.DescriptionDemande == collection.DescriptionDemande
                        && a.Demandeur == collection.Demandeur && a.Comp_CompanyId == idCompany
                        && (a.DateDemande.Value.Day == collection.DateDemande.Value.Day && a.DateDemande.Value.Month == collection.DateDemande.Value.Month && a.DateDemande.Value.Year == collection.DateDemande.Value.Year)).Count() == 0) {
                        db.Crmcli_Demandes.Add(collection);
                        db.SaveChanges();

                        //SEND MAIL NOTIF//
                        string mdpMail = "";
                        string MailAdresse = "";

                        using (var mail = new MailMessage()) {
                            mdpMail = "09eYpçç0601";
                            MailAdresse = "serviceinfo@softwell.mg";

                            var smtp = new SmtpClient("smtpauth.moov.mg");

                            smtp.UseDefaultCredentials = true;

                            mail.From = new MailAddress(MailAdresse);

                            //CLIENT//
                            var client = "";
                            if (db.Comm_Link.Where(a => a.CmLi_Comm_CompanyId == idCompany && a.CmLi_Deleted == null).Count() != 0)//CLIENT
                            {
                                //var companyT = db.Comm_Link.Where(a => a.CmLi_Comm_CompanyId == idCompany && a.CmLi_Deleted == null).FirstOrDefault();
                                if (db.Company.Where(a => a.Comp_CompanyId == idCompany && a.Comp_Deleted == null).Count() != 0) {
                                    var companyFirst = db.Company.Where(a => a.Comp_CompanyId == idCompany && a.Comp_Deleted == null).FirstOrDefault();
                                    if (companyFirst.comp_raison_sociale_af_ay_iltx != null) {
                                        client = companyFirst.comp_raison_sociale_af_ay_iltx;
                                    }
                                    else {
                                        client = companyFirst.Comp_Name;
                                    }
                                }
                            }

                            //var typeInterv = db.Custom_Captions.Where(a => a.Capt_Family == "comm_typepresta" && a.Capt_FR == typeD && a.Capt_Deleted == null).FirstOrDefault();

                            var produit = db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted == null && a.Capt_FR == prod).FirstOrDefault();

                            var mailD = "";
                            if (db.Crmcli_AffectationProds.Where(a => a.AFF_Produit == produit.Capt_Code).Count() != 0) {
                                var agentTomail = db.Crmcli_AffectationProds.Where(a => a.AFF_Produit == produit.Capt_Code).FirstOrDefault().AFF_Agent;

                                string[] separators = { "," };

                                if (agentTomail != null)//Agents
                                {
                                    string listUser = agentTomail.ToString();

                                    string[] agentL = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                    foreach (var ag in agentL) {
                                        int idA = int.Parse(ag);
                                        if (db.users.Where(a => a.User_UserId == idA && a.User_Deleted == null).Count() != 0) {
                                            var userAssigg = db.users.Where(a => a.User_UserId == idA && a.User_Deleted == null).FirstOrDefault();

                                            if (userAssigg.User_EmailAddress != null) {
                                                mailD += userAssigg.User_EmailAddress + ",";
                                            }
                                        }
                                    }
                                }

                                foreach (var address in mailD.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)) {
                                    mail.To.Add(address);
                                }

                            }
                            else
                                return Content("Erreur! Veuillez contacter SOFTWELL pour les destinataires de votre demande par rapport au produit.");

                            mail.Subject = "DEMANDE DU CLIENT : " + client.ToUpper();

                            if (System.IO.File.Exists(Server.MapPath("~/SIGNATURE/SIGNDEM.JPG"))) {
                                mail.IsBodyHtml = true;
                                string path = Server.MapPath("~/SIGNATURE/SIGNDEM.JPG");
                                var Img = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
                                Img.ContentId = "MyImage";

                                var str =
                                    "Bonjour,<br/><br>" + "Vous avez reçu une nouvelle demande" /*+ typeInterv.Capt_FR*/ + " du client : " + client.ToUpper() + ".<br/><br>" +
                                    "<head><style> thead th { font-size: 13px; padding: 15px !important; text-align: center; background-color: #D14C4C; color:white; }" +
                                    "tr { font-size: 12px; padding: 15px !important; background-color: #242426; color:white; }" +
                                    "td { font-size: 12px; padding: 15px !important; background-color: #242426; color:white; } </style> </head> " +

                                    "<table>" +
                                                "<thead>" +
                                                    "<tr>" +
                                                        "<th>Numéro de la demande</th>" +
                                                        "<th>Client</th>" +
                                                        "<th>Demandeur</th>" +
                                                        "<th>E-mail</th>" +
                                                        "<th>Tel</th>" +
                                                        "<th>Priorité</th>" +
                                                        "<th>Thème</th>" +
                                                        "<th>Rubrique</th>";
                                //If Elevée entête//
                                if (collection.PrioriteDemande == 3) {
                                    str += "<th>Niveau</th>" +
                                           "<th>Commentaire sur la priorité</th>";
                                }

                                str += "<th>Produit</th>" +
                                                    "<th>Sujet</th>" +
                                                    "<th>Description</th>" +

                                                    "<th>Date proposée</th>" +
                                                "</tr>" +
                                            "</thead>" +
                                            "<tbody>" +
                                                "<tr>" +
                                                    "<td>" + collection.NumeroDemande + "</td>" +
                                                    "<td>" + client + "</td>" +
                                                    "<td>" + NameAgent + "</td>" +
                                                    "<td>" + MailAgent + "</td>" +
                                                    "<td>" + TelAgent + "</td>" +
                                                    "<td>" + prio + "</td>" +
                                                    "<td>" + thenme + "</td>" +
                                                    "<td>" + rubriqueName + "</td>";

                                //If elevée//
                                if (collection.PrioriteDemande == 3) {
                                    var nivoo = "Non bloquant";
                                    if (collection.NiveauDemande == 2) {
                                        nivoo = "Bloquant";
                                    }

                                    str += "<td>" + nivoo + "</td>" +
                                           "<td>" + collection.CommentaireNivDemande + "</td>";

                                }

                                str += "<td>" + produit.Capt_FR + "</td>" +
                                                        "<td>" + collection.SujetDemande + "</td>" +
                                                        "<td>" + collection.DescriptionDemande + "</td>" +

                                                        "<td>" + collection.DatePropose + "</td>" +
                                                    "</tr>" +
                                                "</tbody>" +
                                            "</table>" + " <br/><br>" + "Cordialement,<br/><br>" + "<img src=cid:MyImage  id='img' alt='' width='450px' height='195px'/>";

                                var AV = AlternateView.CreateAlternateViewFromString(str, null, MediaTypeNames.Text.Html);
                                AV.LinkedResources.Add(Img);

                                mail.AlternateViews.Add(AV);
                            }
                            else {
                                mail.IsBodyHtml = true;
                                mail.Body = "Bonjour,<br/><br>" + "Vous avez reçu une nouvelle demande" /*+ typeInterv.Capt_FR */+ " du client : " + client.ToUpper() + ".<br/><br>" +
                                    "<head><style> thead th { font-size: 13px; padding: 15px !important; text-align: center; background-color: #D14C4C; color:white; }" +
                                    "tr { font-size: 12px; padding: 15px !important; background-color: #242426; color:white; }" +
                                    "td { font-size: 12px; padding: 15px !important; background-color: #242426; color:white; } </style> </head> " +

                                    "<table>" +
                                        "<thead>" +
                                            "<tr>" +
                                                "<th>Numéro de la demande</th>" +
                                                "<th>Client</th>" +
                                                "<th>Demandeur</th>" +
                                                "<th>E-mail</th>" +
                                                "<th>Tel</th>" +
                                                "<th>Priorité</th>" +
                                                "<th>Thème</th>" +
                                                "<th>Rubrique</th>" +

                                                "<th>Produit</th>" +
                                                "<th>Sujet</th>" +
                                                "<th>Description</th>" +

                                                "<th>Date proposée</th>" +
                                            "</tr>" +
                                        "</thead>" +
                                        "<tbody>" +
                                            "<tr>" +
                                                "<td>" + collection.NumeroDemande + "</td>" +
                                                //"<td>" + typeInterv.Capt_FR + "</td>" +
                                                "<td>" + client + "</td>" +
                                                "<td>" + NameAgent + "</td>" +
                                                "<td>" + MailAgent + "</td>" +
                                                "<td>" + TelAgent + "</td>" +
                                                "<td>" + prio + "</td>" +
                                                "<td>" + thenme + "</td>" +

                                                "<td>" + rubriqueName + "</td>" +

                                                "<td>" + produit.Capt_FR + "</td>" +
                                                "<td>" + collection.SujetDemande + "</td>" +
                                                "<td>" + collection.DescriptionDemande + "</td>" +

                                                "<td>" + collection.DatePropose + "</td>" +
                                            "</tr>" +
                                        "</tbody>" +
                                        "</table>" + " <br/><br>" + "Cordialement,";
                            }

                            smtp.Port = 587;

                            smtp.Credentials = new System.Net.NetworkCredential(MailAdresse, mdpMail);
                            smtp.EnableSsl = true;
                            smtp.Send(mail);

                            return Content("Avec succès!");
                        }
                    }
                    else
                        return Content("Demande déjà envoyée! Merci!");
                }
            }
            catch (Exception) {
                return Content("Erreur!");
            }
        }

        //POST: /Intervention/Generer//
        [HttpPost]
        public ActionResult FicheFormation(int id) {
            try {
                var histor = db.Crmcli_HistoIntervs.Where(a => a.ID == id).FirstOrDefault();

                var isDemande = db.Crmcli_Demandes.Where(a => a.ID == histor.ID_Demandes).FirstOrDefault();

                //client//
                string clientName = "";
                var companyFirst = db.Company.Where(a => a.Comp_CompanyId == isDemande.Comp_CompanyId && a.Comp_Deleted != 1).FirstOrDefault();
                if (companyFirst.comp_raison_sociale_af_ay_iltx != null)
                    clientName = companyFirst.comp_raison_sociale_af_ay_iltx;
                else if (companyFirst.Comp_Name != null)
                    clientName = companyFirst.Comp_Name;

                //Produit//
                string produit = "";
                if (db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted != 1 && a.Capt_Code == isDemande.ProduitDemande.ToString()).Count() != 0) {
                    var prod = db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted != 1 && a.Capt_Code == isDemande.ProduitDemande.ToString()).FirstOrDefault();
                    produit = prod.Capt_FR;
                }

                //DateIntervention//
                string dt = "";
                if (histor.Date_Comm != null)
                    dt = histor.Date_Comm.Value.ToShortDateString();

                //Description//
                string descI = "";
                if (histor.Descriptions != null) {
                    descI = histor.Descriptions;
                }

                //Debut//
                string debI = "";
                if (histor.Debut != null)
                    debI = histor.Debut.Value.ToString();

                //Fin//
                string FinI = "";
                if (histor.Fin != null)
                    FinI = histor.Fin.Value.ToString();

                //Validateur//
                string signataire = "";
                if (histor.ID_Pers_Validateur != null) {
                    if (db.Person.Where(a => a.Pers_PersonId == histor.ID_Pers_Validateur && a.Pers_Deleted != 1).Count() != 0) {
                        var userAssig = db.Person.Where(a => a.Pers_PersonId == histor.ID_Pers_Validateur && a.Pers_Deleted != 1).FirstOrDefault();
                        signataire = string.Format("{0} {1}-{2}", userAssig.Pers_LastName, userAssig.Pers_FirstName, userAssig.Pers_Title);
                    }
                }

                //Agent//
                string agent = "";
                string[] separators = { "," };
                if (histor.ID_Agent != null)//Agents
                {
                    string listUser = histor.ID_Agent.ToString();

                    string[] agentL = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var ag in agentL) {
                        int idA = int.Parse(ag);
                        if (db.users.Where(a => a.User_UserId == idA && a.User_Deleted != 1).Count() != 0) {
                            var userAssig = db.users.Where(a => a.User_UserId == idA && a.User_Deleted != 1).FirstOrDefault();

                            agent += string.Format("{0} {1},", userAssig.User_LastName, userAssig.User_FirstName);
                        }
                    }
                }

                //Séance//
                string seance = "";
                if (histor.Seance != null)
                    seance = histor.Seance;

                //PErsP//
                string PErsP = "";
                if (histor.PERSP != null)
                    PErsP = histor.PERSP;

                string dateForName = "";
                if (dt.Contains("/"))
                    dateForName = dt.Replace("/", "");

                string fullName = "";

                CreatWordDoc(string.Format("FICHE_FORMATION_{0}_{1}.docx", clientName, dateForName),
                        id, clientName, dt, agent, produit, seance, PErsP, signataire, debI, FinI, descI);

                //Generation download//
                fullName = string.Format("{0}FICHE_FORMATION_{1}_{2}.docx", Server.MapPath("~/CRS/"), clientName, dateForName);

                string NameFile = string.Format("FICHE_FORMATION_{0}_{1}.docx", clientName, dateForName);

                return Content(NameFile);
            }
            catch (Exception) {
                return Content("Erreur!");
            }
        }

        [HttpGet]
        public ActionResult GenererG(string filename) {
            string fullName = Server.MapPath("~/CRS/") + filename;
            return File(fullName, "application/vnd.ms-word", filename);
        }

        private void CreatWordDoc(string filename,
           int id, string client, string dt, string agent, string produit, string seance, string PersP, string signataire, string debI, string FinI, string descI) {
            string nameDir = "";
            if (Session["UserId"] != null) {
                nameDir = Session["UserId"].ToString() + "FF";
            }

            Session["FILENAME"] = filename;
            try {
                var newPath = Server.MapPath("~/CRS/WTEMPLATEZ.unzipped" + nameDir);

                if (System.IO.File.Exists(Server.MapPath(string.Format("~/TEMPLATES/TEMPCRS/WTEMPLATEZ{0}.zip", nameDir))) == false) {
                    string sourceFile = Server.MapPath("~/WTEMPLATEZ.zip");
                    string destinationFile = Server.MapPath(string.Format("~/TEMPLATES/TEMPCRS/WTEMPLATEZ{0}.zip", nameDir));
                    System.IO.File.Copy(sourceFile, destinationFile);
                }

                using (ZipArchive archive = ZipFile.OpenRead(Server.MapPath(string.Format("~/TEMPLATES/TEMPCRS/WTEMPLATEZ{0}.zip", nameDir)))) {
                    if (Directory.Exists(newPath)) {
                        Directory.Delete(newPath, true);
                        archive.ExtractToDirectory(newPath);
                    }
                    else
                        archive.ExtractToDirectory(newPath);
                }

                string path = Path.Combine(newPath + @"\word\document.xml");

                string str = System.IO.File.ReadAllText(path);

                //FIND/REPLACE => CORPS
                if (string.IsNullOrEmpty(client))
                    client = "";
                if (string.IsNullOrEmpty(dt))
                    dt = "";
                if (string.IsNullOrEmpty(agent))
                    agent = "";
                if (string.IsNullOrEmpty(produit))
                    produit = "";
                if (string.IsNullOrEmpty(seance))
                    seance = "";
                if (string.IsNullOrEmpty(signataire))
                    signataire = "";
                if (string.IsNullOrEmpty(debI))
                    debI = "00:00";
                if (string.IsNullOrEmpty(FinI))
                    FinI = "00:00";
                if (string.IsNullOrEmpty(descI))
                    descI = " ";

                str = str.Replace("VALCLT", client);
                str = str.Replace("VALDATE", dt);
                str = str.Replace("VALCONSU", agent);
                str = str.Replace("VALMOD", produit);
                str = str.Replace("VALSEAN", seance);
                str = str.Replace("VPERSP", PersP);
                str = str.Replace("VALSIGN", signataire);
                str = str.Replace("VALHDEB", debI);
                str = str.Replace("VALHFIN", FinI);
                str = str.Replace("LSXC", descI);

                System.IO.File.WriteAllText(path, str);

                var pathFile = Server.MapPath("~/CRS/" + filename);

                if (System.IO.File.Exists(pathFile)) {
                    System.IO.File.Delete(pathFile);
                    ZipFile.CreateFromDirectory(newPath, Server.MapPath("~/CRS/" + filename));
                }
                else
                    ZipFile.CreateFromDirectory(newPath, Server.MapPath("~/CRS/" + filename));

            }
            catch (Exception) {
                //var x = false;
            }
        }
    }
}
