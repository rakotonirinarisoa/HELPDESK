using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Helpdesk.Controllers {
    public class ConventionController : Controller {
        ModelHELPD db = new ModelHELPD();

        public ActionResult Index(int id) {
            try {
                var list2d = new List<List<string>>();

                if (id != 0) {
                    Session["UserId"] = id;

                    if (db.users.Where(a => a.User_UserId == id && a.User_Deleted != 1).Count() != 0) {
                        var userAssig = db.users.Where(a => a.User_UserId == id && a.User_Deleted != 1).FirstOrDefault();
                        var agent = string.Format("{0} {1}", userAssig.User_LastName, userAssig.User_FirstName);

                        Session["NameSess"] = agent;
                    }
                }

                if (db.Crmcli_CONVENTIONS.Count() != 0) {
                    foreach (var x in db.Crmcli_CONVENTIONS.ToList()) {
                        var list = new List<string>();

                        //ID//0
                        list.Add(x.ID.ToString());

                        //REFERENCE//1
                        var reference = "";
                        if (!string.IsNullOrEmpty(x.REFERENCE)) {
                            reference = x.REFERENCE;
                        }
                        list.Add(reference);

                        //INTITULE//2
                        var intitule = "";
                        if (!string.IsNullOrEmpty(x.INTITULE)) {
                            intitule = x.INTITULE;
                        }
                        list.Add(intitule);

                        //ETAT//3
                        var etat = "ENVOIE CLIENT";
                        if (x.ETAT != null)
                            etat = x.ETAT;
                        list.Add(etat);

                        //CLIENT//4
                        var client = "";
                        if (x.CLIENT != null) {
                            var companyFirst = db.Company.Where(a => a.Comp_CompanyId == x.CLIENT && a.Comp_Deleted != 1).FirstOrDefault();
                            if (companyFirst.comp_raison_sociale_af_ay_iltx != null) {
                                client = companyFirst.comp_raison_sociale_af_ay_iltx;
                            }
                            else if (companyFirst.Comp_Name != null) {
                                client = companyFirst.Comp_Name;
                            }
                        }
                        list.Add(client);

                        //CONTACTER//5
                        var cont = "";
                        if (!string.IsNullOrEmpty(x.NOMCONTACT)) {
                            cont = x.NOMCONTACT;
                        }
                        list.Add(cont);

                        //COMMERCIAL//6
                        var comm = "";
                        if (!string.IsNullOrEmpty(x.COMMERCIALE)) {
                            comm = x.COMMERCIALE;
                        }
                        list.Add(comm);

                        //DATEENVOIECONV//7
                        var dateenvoi = "";
                        if (x.DATEENVOI != null) {
                            dateenvoi = x.DATEENVOI.Value.ToShortDateString();
                        }
                        list.Add(dateenvoi);

                        //DATESAISIE//8
                        var datesaisie = "";
                        if (x.DATESAISIE != null) {
                            datesaisie = x.DATESAISIE.Value.ToShortDateString();
                        }
                        list.Add(datesaisie);

                        //SAISISEUR//9
                        var agent = "";
                        if (db.users.Where(a => a.User_UserId == x.SAISISSEUR && a.User_Deleted != 1).Count() != 0) {
                            var userAssig = db.users.Where(a => a.User_UserId == x.SAISISSEUR && a.User_Deleted != 1).FirstOrDefault();
                            agent = string.Format("{0} {1}", userAssig.User_LastName, userAssig.User_FirstName);
                        }
                        list.Add(agent);

                        list2d.Add(list);
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

        public ActionResult Create() {
            var collection = new Crmcli_CONVENTIONS();

            //ETAT//
            var etat = new List<string>();
            etat.Add("ENCOURS");
            etat.Add("ENVOIE CLIENT");
            etat.Add("PHASE 1");
            etat.Add("PHASE 2");
            etat.Add("PHASE 3");
            etat.Add("PHASE 4");
            etat.Add("PHASE 5");
            etat.Add("PHASE 6");
            etat.Add("CLOSED");
            collection.EtatCollection = etat;

            //CLIENT//
            var clients = db.Company
            .Select(s => new {
                Text = s.Comp_Name,
                Value = s.Comp_CompanyId
            })
            .ToList();
            ViewBag.ClientsList = new SelectList(clients, "Value", "Text");

            var clt2 = db.Company
            .Select(s => new {
                Text = s.Comp_Name,
                Value = s.Comp_CompanyId
            })
            .ToList();
            ViewBag.ClientsList = new SelectList(clients, "Value", "Text");

            foreach (var x in clt2) {
                if (db.Crmcli_Reg.Where(a => a.Comp_CompanyId == x.Value).Count() == 0)
                    clients.Remove(x);
            }

            //REFERENCE//
            int numero = 0;
            if (db.Crmcli_CONVENTIONS.Count() != 0) {
                foreach (var x in db.Crmcli_CONVENTIONS.ToList()) {
                    if (x.NUMERO >= numero)
                        numero = x.NUMERO.Value;
                }
            }
            var numeroRef = numero + 1;
            var numeroRefOK = "";
            var abreg = "SOFT-CONVENT-";
            if (numeroRef <= 99999) {
                if (numeroRef.ToString().Length == 1) {
                    numeroRefOK = string.Format("{0}0000{1}", abreg, numeroRef);
                }
                else if (numeroRef.ToString().Length == 2) {
                    numeroRefOK = string.Format("{0}000{1}", abreg, numeroRef);
                }
                else if (numeroRef.ToString().Length == 3) {
                    numeroRefOK = string.Format("{0}00{1}", abreg, numeroRef);
                }
                else if (numeroRef.ToString().Length == 4) {
                    numeroRefOK = string.Format("{0}0{1}", abreg, numeroRef);
                }
                else if (numeroRef.ToString().Length == 5) {
                    numeroRefOK = abreg + numeroRef;
                }
            }
            ViewData["REFCONVENTION"] = numeroRefOK;

            return View(collection);
        }

        [HttpPost]
        public ActionResult CreateD(Crmcli_CONVENTIONS collection) {
            try {
                if (Session["UserId"] == null)
                    return Content("Erreur! Reconnectez-vous svp!");

                int idAgent = int.Parse(Session["UserId"].ToString());
                collection.SAISISSEUR = idAgent;

                int numero = 0;
                if (db.Crmcli_CONVENTIONS.Count() != 0) {
                    foreach (var x in db.Crmcli_CONVENTIONS.ToList()) {
                        if (x.NUMERO >= numero)
                            numero = x.NUMERO.Value;
                    }
                }
                var numeroRef = numero + 1;
                collection.NUMERO = numeroRef;

                collection.DATESAISIE = DateTime.Now.Date;

                collection.ETAT = "ENCOURS";
                if (collection.DATEENVOI != null)
                    collection.ETAT = "ENVOIE CLIENT";
                if (collection.DATEPHASE1 != null)
                    collection.ETAT = "PHASE 1";
                if (collection.DATEPHASE2 != null)
                    collection.ETAT = "PHASE 2";
                if (collection.DATEPHASE3 != null)
                    collection.ETAT = "PHASE 3";
                if (collection.DATEPHASE4 != null)
                    collection.ETAT = "PHASE 4";
                if (collection.DATEPHASE5 != null)
                    collection.ETAT = "PHASE 5";
                if (collection.DATEPHASE6 != null)
                    collection.ETAT = "PHASE 6";

                db.Crmcli_CONVENTIONS.Add(collection);
                db.SaveChanges();

                return Content("Avec succès!");
            }
            catch (Exception) {
                return Content("Erreur de saisie!");
            }
        }

        [HttpPost]
        public ActionResult Delete(int id) {
            try {
                if (Session["UserId"] == null)
                    return Content("Erreur! Reconnectez-vous svp!");
                else if (!string.IsNullOrEmpty(id.ToString())) {
                    if (db.Crmcli_CONVENTIONS.Where(a => a.ID == id).Count() != 0) {
                        var isDelete = db.Crmcli_CONVENTIONS.Where(a => a.ID == id).FirstOrDefault();

                        db.Crmcli_CONVENTIONS.Remove(isDelete);
                        db.SaveChanges();

                        return Content("Suppression avec succès!");
                    }
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

        public ActionResult Edit(int id) {
            try {
                var listCon = new List<List<string>>();
                var listCONV = new List<string>();

                if (db.Crmcli_CONVENTIONS.Where(a => a.ID == id).Count() != 0) {
                    var isCon = db.Crmcli_CONVENTIONS.Where(a => a.ID == id).FirstOrDefault();

                    //ID//0
                    listCONV.Add(isCon.ID.ToString());

                    //REFERENCE//1
                    var reference = "";
                    if (isCon.REFERENCE != null)
                        reference = isCon.REFERENCE;
                    listCONV.Add(reference);

                    //INTITULE//2
                    var intitule = "";
                    if (isCon.INTITULE != null)
                        intitule = isCon.INTITULE;
                    listCONV.Add(intitule);

                    //DATESAISIE//3
                    var datesaisie = "";
                    if (isCon.DATESAISIE != null)
                        datesaisie = isCon.DATESAISIE.Value.ToShortDateString();
                    listCONV.Add(datesaisie);

                    //DATEENVOI//4
                    var dateenvoi = "";
                    if (isCon.DATEENVOI != null)
                        dateenvoi = isCon.DATEENVOI.Value.ToString("yyyy-MM-dd");
                    listCONV.Add(dateenvoi);

                    //SAISISSEUR//5
                    var saisisseur = "";
                    if (isCon.SAISISSEUR != null) {
                        if (db.users.Where(a => a.User_UserId == isCon.SAISISSEUR && a.User_Deleted != 1).Count() != 0) {
                            var userAssig = db.users.Where(a => a.User_UserId == isCon.SAISISSEUR && a.User_Deleted != 1).FirstOrDefault();
                            saisisseur = string.Format("{0} {1}", userAssig.User_LastName, userAssig.User_FirstName);
                        }
                    }
                    listCONV.Add(saisisseur);
                    Session["SAISISS"] = saisisseur;

                    //DESCRIPTION//6
                    var desc = "";
                    if (isCon.DESCRIPTION != null)
                        desc = isCon.DESCRIPTION;
                    listCONV.Add(desc);

                    //CLIENT//7
                    var client = "";
                    if (isCon.CLIENT != null) {
                        var companyFirst = db.Company.Where(a => a.Comp_CompanyId == isCon.CLIENT && a.Comp_Deleted != 1).FirstOrDefault();
                        if (companyFirst.comp_raison_sociale_af_ay_iltx != null) {
                            client = companyFirst.comp_raison_sociale_af_ay_iltx;
                        }
                        else if (companyFirst.Comp_Name != null) {
                            client = companyFirst.Comp_Name;
                        }
                    }
                    listCONV.Add(client);

                    //NOMCLT//8
                    var nom = "";
                    if (isCon.NOMCONTACT != null)
                        nom = isCon.NOMCONTACT;
                    listCONV.Add(nom);

                    //NUMCLT//9
                    var num = "";
                    if (isCon.NUMCONTACT != null)
                        num = isCon.NUMCONTACT;
                    listCONV.Add(num);

                    //MAILCLT//10
                    var mail = "";
                    if (isCon.MAILCONTACT != null)
                        mail = isCon.MAILCONTACT;
                    listCONV.Add(mail);

                    //PH1//11
                    var ph1 = "";
                    if (isCon.DATEPHASE1 != null)
                        ph1 = isCon.DATEPHASE1.Value.ToString("yyyy-MM-dd");
                    listCONV.Add(ph1);

                    //PH2//12
                    var ph2 = "";
                    if (isCon.DATEPHASE2 != null)
                        ph2 = isCon.DATEPHASE2.Value.ToString("yyyy-MM-dd");
                    listCONV.Add(ph2);

                    //PH3//13
                    var ph3 = "";
                    if (isCon.DATEPHASE3 != null)
                        ph3 = isCon.DATEPHASE3.Value.ToString("yyyy-MM-dd");
                    listCONV.Add(ph3);

                    //PH4//14
                    var ph4 = "";
                    if (isCon.DATEPHASE4 != null)
                        ph4 = isCon.DATEPHASE4.Value.ToString("yyyy-MM-dd");
                    listCONV.Add(ph4);

                    //PH5//15
                    var ph5 = "";
                    if (isCon.DATEPHASE5 != null)
                        ph5 = isCon.DATEPHASE5.Value.ToString("yyyy-MM-dd");
                    listCONV.Add(ph5);

                    //PH6//16
                    var ph6 = "";
                    if (isCon.DATEPHASE6 != null)
                        ph6 = isCon.DATEPHASE6.Value.ToString("yyyy-MM-dd");
                    listCONV.Add(ph6);

                    //ETAT//17
                    var etat = "ENVOIE CLIENT";
                    if (isCon.ETAT != null)
                        etat = isCon.ETAT;
                    listCONV.Add(etat);

                    //COMM//18
                    var comm = "";
                    if (isCon.COMMERCIALE != null)
                        comm = isCon.COMMERCIALE;
                    listCONV.Add(comm);

                    listCon.Add(listCONV);
                }

                var collection = new Crmcli_CONVENTIONS();

                //ETAT//
                var etatE = new List<string>();
                etatE.Add("ENCOURS");
                etatE.Add("ENVOIE CLIENT");
                etatE.Add("PHASE 1");
                etatE.Add("PHASE 2");
                etatE.Add("PHASE 3");
                etatE.Add("PHASE 4");
                etatE.Add("PHASE 5");
                etatE.Add("PHASE 6");
                etatE.Add("CLOSED");
                collection.EtatCollection = etatE;

                //CLIENT//
                var clients = db.Company
                .Select(s => new {
                    Text = s.Comp_Name,
                    Value = s.Comp_CompanyId
                })
                .ToList();
                ViewBag.ClientsList = new SelectList(clients, "Value", "Text");

                var clt2 = db.Company
                .Select(s => new {
                    Text = s.Comp_Name,
                    Value = s.Comp_CompanyId
                })
                .ToList();
                ViewBag.ClientsList = new SelectList(clients, "Value", "Text");

                foreach (var x in clt2) {
                    if (db.Crmcli_Reg.Where(a => a.Comp_CompanyId == x.Value).Count() == 0)
                        clients.Remove(x);
                }

                var lis = listCon;
                ViewBag.listCon = lis;

                return View(collection);
            }
            catch (Exception) {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Edit(Crmcli_CONVENTIONS collection) {
            try {
                if (Session["UserId"] == null)
                    return Content("Erreur! Reconnectez-vous svp!");

                if (db.Crmcli_CONVENTIONS.Where(a => a.REFERENCE == collection.REFERENCE).Count() != 0) {
                    var isModf = db.Crmcli_CONVENTIONS.Where(a => a.REFERENCE == collection.REFERENCE).FirstOrDefault();

                    int idAgent = int.Parse(Session["UserId"].ToString());
                    //isModf.SAISISSEUR = idAgent;

                    if (!string.IsNullOrEmpty(collection.INTITULE)) {
                        isModf.INTITULE = collection.INTITULE;
                    }
                    else
                        isModf.INTITULE = "";

                    if (collection.DATEENVOI != null) {
                        isModf.DATEENVOI = collection.DATEENVOI;
                    }
                    else
                        isModf.DATEENVOI = null;

                    if (!string.IsNullOrEmpty(collection.DESCRIPTION)) {
                        isModf.DESCRIPTION = collection.DESCRIPTION;
                    }
                    else
                        isModf.DESCRIPTION = "";

                    /*if (!String.IsNullOrEmpty(collection.ETAT))
                    {
                        isModf.ETAT = collection.ETAT;
                    }
                    else
                        isModf.ETAT = "";*/
                    isModf.ETAT = "ENCOURS";
                    if (collection.DATEENVOI != null)
                        isModf.ETAT = "ENVOIE CLIENT";
                    if (collection.DATEPHASE1 != null)
                        isModf.ETAT = "PHASE 1";
                    if (collection.DATEPHASE2 != null)
                        isModf.ETAT = "PHASE 2";
                    if (collection.DATEPHASE3 != null)
                        isModf.ETAT = "PHASE 3";
                    if (collection.DATEPHASE4 != null)
                        isModf.ETAT = "PHASE 4";
                    if (collection.DATEPHASE5 != null)
                        isModf.ETAT = "PHASE 5";
                    if (collection.DATEPHASE6 != null)
                        isModf.ETAT = "PHASE 6";

                    if (collection.CLIENT != null) {
                        isModf.CLIENT = collection.CLIENT;
                    }
                    else
                        isModf.CLIENT = null;

                    if (!string.IsNullOrEmpty(collection.NOMCONTACT)) {
                        isModf.NOMCONTACT = collection.NOMCONTACT;
                    }
                    else
                        isModf.NOMCONTACT = "";

                    if (!string.IsNullOrEmpty(collection.NUMCONTACT)) {
                        isModf.NUMCONTACT = collection.NUMCONTACT;
                    }
                    else
                        isModf.NUMCONTACT = "";

                    if (!string.IsNullOrEmpty(collection.MAILCONTACT)) {
                        isModf.MAILCONTACT = collection.ETAT;
                    }
                    else
                        isModf.MAILCONTACT = "";

                    if (!string.IsNullOrEmpty(collection.COMMERCIALE)) {
                        isModf.COMMERCIALE = collection.COMMERCIALE;
                    }
                    else
                        isModf.COMMERCIALE = "";

                    if (collection.DATEPHASE1 != null) {
                        isModf.DATEPHASE1 = collection.DATEPHASE1;
                    }
                    else
                        isModf.DATEPHASE1 = null;

                    if (collection.DATEPHASE2 != null) {
                        isModf.DATEPHASE2 = collection.DATEPHASE2;
                    }
                    else
                        isModf.DATEPHASE2 = null;

                    if (collection.DATEPHASE3 != null) {
                        isModf.DATEPHASE3 = collection.DATEPHASE3;
                    }
                    else
                        isModf.DATEPHASE3 = null;

                    if (collection.DATEPHASE4 != null) {
                        isModf.DATEPHASE4 = collection.DATEPHASE4;
                    }
                    else
                        isModf.DATEPHASE4 = null;

                    if (collection.DATEPHASE5 != null) {
                        isModf.DATEPHASE5 = collection.DATEPHASE5;
                    }
                    else
                        isModf.DATEPHASE5 = null;

                    if (collection.DATEPHASE6 != null) {
                        isModf.DATEPHASE6 = collection.DATEPHASE6;
                    }
                    else
                        isModf.DATEPHASE6 = null;

                    db.SaveChanges();

                    return Content("Modification avec succès");
                }
                else
                    return Content("Erreur! La convention n'existe pas!");
            }
            catch (Exception) {
                return Content("Erreur!");
            }
        }
    }
}
