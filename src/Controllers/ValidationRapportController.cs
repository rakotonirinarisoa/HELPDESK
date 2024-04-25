using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web.Mvc;

namespace Helpdesk.Controllers {
    public class ValidationRapportController : Controller {
        //
        // GET: /ValidationRapport/

        ModelHELPD db = new ModelHELPD();

        public ActionResult Index(int id, DateTime? dateDeb, DateTime? dateFin) {
            var list2d = new List<List<string>>();

            if (id != 0) {
                Session["UserId"] = id;

                if (db.users.Where(a => a.User_UserId == id && a.User_Deleted != 1).Count() != 0) {
                    var userAssig = db.users.Where(a => a.User_UserId == id && a.User_Deleted != 1).FirstOrDefault();
                    var agent = string.Format("{0} {1}", userAssig.User_LastName, userAssig.User_FirstName);

                    Session["NameSess"] = agent;
                }
            }

            try {
                if (db.Crmcli_SUPSUBo.Where(a => a.SUPe == id).Count() != 0) {
                    var dtVE = new TimeSpan();
                    var dtVF = new TimeSpan();
                    var EF = new TimeSpan();
                    var isSUP = db.Crmcli_SUPSUBo.Where(a => a.SUPe == id).FirstOrDefault();
                    string[] separators = { "," };
                    if (!string.IsNullOrEmpty(isSUP.SUBo)) {
                        string[] agentL = isSUP.SUBo.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var ag in agentL) {
                            DateTime du = DateTime.Now.Date;
                            //DateTime fin = new DateTime(2022, 12, 08, 0, 0, 0).Date;
                            DateTime fin = DateTime.Now.Date;
                            if (dateDeb != null && dateFin != null) {
                                du = dateDeb.Value;
                                //DateTime fin = new DateTime(2022, 12, 08, 0, 0, 0).Date;
                                fin = dateFin.Value;
                                while (du <= fin) {
                                    EF = new TimeSpan();
                                    dtVE = new TimeSpan();
                                    dtVF = new TimeSpan();
                                    var list = new List<string>();
                                    var listOfINOUT = new List<INOUT>();
                                    var pause = 0;
                                    var total = TimeSpan.Zero;
                                    DateTime date1 = new DateTime(du.Year, du.Month, du.Day, 0, 0, 0).Date;
                                    if (db.Communication.Where(a => a.Comm_Deleted == null && a.Comm_CaseId != null && (a.comm_typeass == "01" || a.comm_typeass == "02" || a.comm_typeass == "03" || a.comm_typeass == "04") && a.comm_presence != "1"
                                    //&& a.Comm_ToDateTime.Value == du
                                    && (a.comm_userintervenant.Contains(ag) || a.comm_assistform.Contains(ag))).Count() != 0) {
                                        foreach (var y in db.Communication.Where(a => a.Comm_Deleted == null && a.Comm_CaseId != null && (a.comm_typeass == "01" || a.comm_typeass == "02" || a.comm_typeass == "03" || a.comm_typeass == "04") && a.comm_presence != "1"
                                            && (a.comm_userintervenant.Contains(ag) || a.comm_assistform.Contains(ag))).OrderBy(a => a.Comm_ToDateTime).ToList()) {
                                            if (y.comm_typepresta != "001" && y.Comm_DateTime == du) {
                                                var aze = db.Crmcli_HistoIntervs.Where(x => x.Activite != "001" || x.Activite != "004" && x.ID_Agent.Contains(ag) && x.Date_Comm == du).ToList();
                                                //var aze = db.Crmcli_HistoIntervs.Where(x => x.Activite == "002" && x.Nature=="002008" && x.ID_Agent.Contains(ag) && x.Date_Comm.Value.CompareTo(du) == 0).ToList();
                                                if (aze.Count > 0) {
                                                    foreach (var t in aze) {
                                                        if (t.Debut == null || t.Fin == null) {
                                                            dtVE = new TimeSpan();
                                                            dtVF = new TimeSpan();
                                                            EF = new TimeSpan();
                                                            EF = dtVF - dtVE;
                                                            var dtf = new DateTime();
                                                            var dts = new DateTime();
                                                            listOfINOUT.Add(new INOUT(dtf, dts));
                                                        }
                                                        else {

                                                            dtVE = t.Debut.Value;
                                                            dtVF = t.Fin.Value;
                                                            EF = dtVF - dtVE;
                                                            var dtf = new DateTime();
                                                            var dts = new DateTime();
                                                            //dtf = y.Date_Comm.Value;
                                                            //dts = y.Date_Comm.Value;
                                                            //dtf = db.Crmcli_HistoIntervs.Select(a => a.Date_Validation).FirstOrDefault().Value;
                                                            //dts = db.Crmcli_HistoIntervs.Select(a => a.Date_Validation).FirstOrDefault().Value;
                                                            dtf = du + dtVE;
                                                            dts = du + dtVF;
                                                            //total += y.comm_datefin2.Value - y.Comm_ToDateTime.Value;
                                                            listOfINOUT.Add(new INOUT(dtf, dts));
                                                            //var dtVE = db.Crmcli_HistoIntervs.Where(a=>a.ID_Agent.Contains(ag)).ToList();                
                                                            //listOfINOUT.Add(new INOUT(dtVE.));
                                                            //pause += y.comm_pause.Value;
                                                        }

                                                    }
                                                }

                                            }
                                            else if (y.Comm_ToDateTime.Value.Date == du.Date && y.comm_typepresta != "002") {
                                                //total += y.comm_datefin2.Value - y.Comm_ToDateTime.Value;
                                                listOfINOUT.Add(new INOUT(y.Comm_ToDateTime.Value, y.comm_datefin2.Value));
                                                //var dtVE = db.Crmcli_HistoIntervs.Where(a=>a.ID_Agent.Contains(ag)).ToList();                
                                                //listOfINOUT.Add(new INOUT(dtVE.));
                                                pause += y.comm_pause.Value;
                                            }

                                        }
                                        //eto tsika zao

                                    }

                                    var isag = int.Parse(ag);
                                    if (db.pro_rapport.Where(a => a.Absent != 1 //&& a.TypeLieu != 0 //&& a.StartDate.Value == du
                                        && a.ResourceID == isag).Count() != 0) {
                                        foreach (var y in db.pro_rapport.Where(a => a.Absent != 1 //&& a.TypeLieu != 0 //&& a.StartDate.Value == du 
                                            && a.ResourceID == isag).OrderBy(a => a.StartDate).ToList()) {
                                            if (y.StartDate.Value.Date == du.Date) {
                                                //total += y.EndDate.Value - y.StartDate.Value;
                                                listOfINOUT.Add(new INOUT(y.StartDate.Value, y.EndDate.Value));
                                            }
                                        }
                                    }

                                    listOfINOUT.Sort();
                                    DateTime? tempIn = null;
                                    DateTime? tempOut = null;
                                    foreach (var inE in listOfINOUT) {
                                        var minHD = inE.GetinE();
                                        var sO = inE.GetsO();

                                        if (tempIn == null || tempOut == null) {
                                            tempIn = inE.GetinE();
                                            tempOut = inE.GetsO();

                                            total += tempOut.Value - tempIn.Value;//1
                                        }

                                        if (tempOut.Value >= minHD && tempOut.Value < sO)//2
                                        {
                                            //tempOut = sO;
                                            total += sO - tempOut.Value;
                                            tempOut = sO;
                                        }
                                        else if (tempOut.Value < minHD)//3
                                        {
                                            total += sO - minHD;
                                            tempOut = sO;
                                        }
                                    }

                                    if (total != TimeSpan.Zero && db.Crmcli_SUPSUBv.Where(a => a.DateRapport.Value == du && a.IDUser == isag).Count() == 0
                                        && db.ThistoSendMail.Where(a => DbFunctions.TruncateTime(a.DateRapport) == du.Date && a.User_id == isag).Count() != 0) {
                                        total -= TimeSpan.FromMinutes(pause);

                                        string totalHM = total.ToString(@"hh\:mm");//total.Hours + ":" + total.Minutes;

                                        //IDUSER//0
                                        list.Add(isag.ToString());
                                        //User name//1
                                        var userName = "";
                                        if (db.users.Where(a => a.User_UserId == isag && a.User_Deleted != 1).Count() != 0) {
                                            var userAssig = db.users.Where(a => a.User_UserId == isag && a.User_Deleted != 1).FirstOrDefault();
                                            var agent = string.Format("{0} {1}", userAssig.User_LastName, userAssig.User_FirstName);

                                            userName = agent;
                                        }
                                        list.Add(userName);
                                        //Date rapport//2
                                        var dateRAP = du.Date.ToShortDateString();
                                        list.Add(dateRAP);
                                        //Total heure//3
                                        list.Add(totalHM);
                                        var heureValider = dtVE.ToString() + " - " + dtVF.ToString();
                                        list.Add(heureValider);
                                        list.Add(EF.ToString());

                                        list2d.Add(list);
                                    }

                                    du = du.AddDays(1);
                                }
                            }
                            else {
                                return View();
                            }


                        }
                    }
                }

                var lis = list2d;
                ViewBag.ldv = lis;

                return View();
            }
            catch (Exception ex) {
                var eee = ex.Message;
                return View();
                throw;
            }
        }

        [HttpPost]
        public JsonResult Valider(int IDvalidateur, int IdU, DateTime Date, TimeSpan TheureA, TimeSpan ECARTPv, TimeSpan ECARTNv, string COMMSv) {
            try {
                var db = new ModelHELPD();
                var total = TheureA;

                if (Session["UserId"] == null)
                    return Json("Erreur! Reconnectez-vous svp!", JsonRequestBehavior.AllowGet);
                else if (!string.IsNullOrEmpty(IdU.ToString())) {
                    total += ECARTPv;
                    total -= ECARTNv;

                    var ins = new Crmcli_SUPSUBv {
                        DateValidation = DateTime.Now.Date,
                        IDUser = IdU,
                        IDValideur = IDvalidateur,
                        HUser = TheureA,
                        HValidateur = total,
                        HPLUS = ECARTPv,
                        HMOINS = ECARTNv,
                        DateRapport = Date,
                        COMMS = COMMSv
                    };

                    db.Crmcli_SUPSUBv.Add(ins);
                    db.SaveChanges();

                    var nameUser = db.users.Where(a => a.User_UserId == IdU && a.User_Deleted != 1).FirstOrDefault();

                    //SEND MAIL//
                    var isSender = db.users.Where(a => a.User_UserId == IDvalidateur && a.User_Deleted != 1).FirstOrDefault();
                    string mdpMail = isSender.user_pwdmail;
                    string MailAdresse = isSender.User_EmailAddress;

                    using (var mail = new MailMessage()) {
                        var smtp = new SmtpClient("smtpauth.moov.mg");

                        smtp.UseDefaultCredentials = true;

                        mail.From = new MailAddress(MailAdresse);

                        var mailD = "";
                        string[] separators = { "," };
                        if (nameUser.user_destinatairesmail2 != null)//Agents
                        {
                            string listUser = nameUser.user_destinatairesmail2;

                            string[] agentL = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                            foreach (var ag in agentL) {
                                mailD += ag + ",";
                            }
                        }

                        mailD += nameUser.User_EmailAddress + "," + MailAdresse;
                        mail.To.Add(mailD);
                        //mail.To.Add(mailD.TrimEnd(','));
                        //mail.To.Add("rinah.raharinosy@softwell.mg");


                        mail.Subject = "Validation du rapport de " + nameUser.User_FirstName + " " + nameUser.User_LastName + " du : " + Date.ToShortDateString() + " par " + isSender.User_FirstName + " " + isSender.User_LastName;

                        var pathFile = Server.MapPath(string.Format("~/SIGNATURES/{0}.jpg", IDvalidateur));

                        if (System.IO.File.Exists(pathFile)) {
                            mail.IsBodyHtml = true;
                            string path = Server.MapPath(string.Format("~/SIGNATURES/{0}.jpg", IDvalidateur));
                            var Img = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
                            Img.ContentId = "MyImage";

                            var str = @"<head><style> thead th { font-size: 13px; padding: 15px !important; text-align: center; background-color: #296d8a; color:white; }" +
                                "tr { font-size: 12px; padding: 15px !important; background-color: #242426; color:white; }" +
                                "td { font-size: 12px; padding: 15px !important; background-color: #242426; color:white; } </style> </head> " +

                                "<table>" +
                                        "<thead>" +
                                            "<tr>" +
                                                "<th>TOTAL HEURE PAR LE CONSULTANT : " + nameUser.User_FirstName + " " + nameUser.User_LastName + "</th>" +
                                                "<th>TOTAL HEURE VALIDEE PAR LE SUPERVISEUR : " + isSender.User_FirstName + " " + isSender.User_LastName + "</th>" +
                                                "<th>EN PLUS</th>" +
                                                "<th>EN MOINS</th>" +
                                                "<th>COMMENTAIRES</th>" +
                                            "</tr>" +
                                        "</thead>" +
                                        "<tbody>" +
                                            "<tr>" +
                                                "<td>" + TheureA + "</td>" +
                                                "<td>" + total + "</td>" +
                                                "<td>" + "+" + ECARTPv + "</td>" +
                                                "<td>" + "-" + ECARTNv + "</td>" +
                                                "<td>" + COMMSv + "</td>" +
                                            "</tr>" +
                                        "</tbody>" +
                                "</table>" + " <br/>" + "Cordialement,<br/><br>" + "<img src=cid:MyImage  id='img' alt='' width='450px' height='195px'/>";

                            var AV = AlternateView.CreateAlternateViewFromString(str, null, MediaTypeNames.Text.Html);
                            AV.LinkedResources.Add(Img);

                            mail.AlternateViews.Add(AV);
                        }
                        else {
                            mail.IsBodyHtml = true;
                            mail.Body = "<head><style> thead th { font-size: 13px; padding: 15px !important; text-align: center; background-color: #296d8a; color:white; }" +
                                "tr { font-size: 12px; padding: 15px !important; background-color: #242426; color:white; }" +
                                "td { font-size: 12px; padding: 15px !important; background-color: #242426; color:white; } </style> </head> " +

                                "<table>" +
                                        "<thead>" +
                                            "<tr>" +
                                                "<th>TOTAL HEURE PAR LE CONSULTANT : " + nameUser.User_FirstName + " " + nameUser.User_LastName + "</th>" +
                                                "<th>TOTAL HEURE VALIDEE PAR LE SUPERVISEUR : " + isSender.User_FirstName + " " + isSender.User_LastName + "</th>" +
                                                "<th>EN PLUS</th>" +
                                                "<th>EN MOINS</th>" +
                                                "<th>COMMENTAIRES</th>" +
                                            "</tr>" +
                                        "</thead>" +
                                        "<tbody>" +
                                            "<tr>" +
                                                "<td>" + TheureA + "</td>" +
                                                "<td>" + total + "</td>" +
                                                "<td>" + "+" + ECARTPv + "</td>" +
                                                "<td>" + "-" + ECARTNv + "</td>" +
                                                "<td>" + COMMSv + "</td>" +
                                            "</tr>" +
                                        "</tbody>" +
                                "</table>" + " <br/><br>" + "Cordialement,<br/><br>" + "<img src=cid:MyImage  id='img' alt='' width='450px' height='195px'/>";
                        }

                        smtp.Port = 587;

                        smtp.Credentials = new System.Net.NetworkCredential(MailAdresse, mdpMail);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);

                        return Json("Validation avec succès!", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                    return Json("Erreur! Certains destinataires n'ont pas reçus, Mail non actif!", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)//DbEntityValidationException e)
            {
                return Json("Erreur! Certains destinataires n'ont pas reçus, Mail non actif!", JsonRequestBehavior.AllowGet);
            }
        }

        public class INOUT : IComparable<INOUT> {
            private DateTime inE;
            private DateTime sO;

            public INOUT(DateTime inE, DateTime sO) {
                this.inE = inE;
                this.sO = sO;
            }

            public DateTime GetinE() {
                return inE;
            }

            public void SetinE(DateTime inE) {
                this.inE = inE;
            }

            public DateTime GetsO() {
                return sO;
            }
            public void SetsO(DateTime sO) {
                this.sO = sO;
            }

            public int CompareTo(INOUT obj) {
                return inE.CompareTo(obj.inE);
            }
        }
    }
}
