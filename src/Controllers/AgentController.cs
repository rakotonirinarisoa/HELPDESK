using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Helpdesk.Controllers {
    public class AgentController : Controller {
        ModelHELPD db = new ModelHELPD();

        //
        // GET: /Agent/

        public ActionResult Index() {
            var list2d = new List<List<string>>();

            if (db.users.Where(a => a.User_Deleted != 1 && a.User_Logon != null && a.User_Logon != "Admin").Count() != 0) {
                foreach (var x in db.users.Where(a => a.User_Deleted != 1 && a.User_Logon != null && a.User_Logon != "Admin").OrderBy(a => a.User_UserId).ToList()) {
                    var list = new List<string>();
                    list.Add(x.User_LastName);
                    list.Add(x.User_FirstName);
                    list.Add(x.User_Department);
                    list.Add(x.User_EmailAddress);

                    list2d.Add(list);
                }
            }

            var lis = list2d;
            ViewBag.ldv = lis;

            return View();
        }


        public ActionResult AffectationProd() {
            var list2d = new List<List<string>>();

            if (db.Crmcli_AffectationProds.Count() != 0) {
                foreach (var x in db.Crmcli_AffectationProds.ToList()) {
                    var list = new List<string>();

                    list.Add(x.ID.ToString());

                    var interv = "";
                    string[] separators = { "," };

                    if (x.AFF_Agent != null)//Agents
                    {
                        string listUser = x.AFF_Agent.ToString();

                        string[] agent = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var ag in agent) {
                            int idA = int.Parse(ag);
                            if (db.users.Where(a => a.User_UserId == idA && a.User_Deleted != 1).Count() != 0) {
                                var userAssig = db.users.Where(a => a.User_UserId == idA && a.User_Deleted != 1).FirstOrDefault();

                                interv += userAssig.User_FirstName + ",";
                            }
                        }
                        list.Add(interv);
                    }
                    else
                        list.Add("");


                    if (db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted != 1 && a.Capt_Code == x.AFF_Produit.ToString()).Count() != 0) {
                        var prod = db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted != 1 && a.Capt_Code == x.AFF_Produit.ToString()).FirstOrDefault();
                        list.Add(prod.Capt_FR);
                    }
                    else
                        list.Add("");

                    list2d.Add(list);
                }
            }

            var lis = list2d;
            ViewBag.ldv = lis;

            return View();
        }


        public ActionResult SignatSoc() {
            var list2d = new List<List<string>>();

            string Npers = "";

            if (db.Crmcli_Reg.Count() != 0) {
                foreach (var x in db.Crmcli_Reg.ToList()) {
                    var list = new List<string>();

                    list.Add(x.Comp_CompanyId.ToString());

                    int idAClientCount = x.Comp_CompanyId.Value;

                    if (db.Company.Where(a => a.Comp_CompanyId == idAClientCount && a.Comp_Deleted != 1).Count() != 0) {
                        var comp = db.Company.Where(a => a.Comp_CompanyId == idAClientCount && a.Comp_Deleted != 1).FirstOrDefault();
                        list.Add(comp.Comp_Name);

                        if (db.Crmcli_UsersSession.Where(a => a.ID_Company == comp.Comp_CompanyId).Count() != 0) {
                            foreach (var y in db.Crmcli_UsersSession.Where(a => a.ID_Company == comp.Comp_CompanyId).ToList()) {
                                var companyName = db.Company.Where(a => a.Comp_CompanyId == comp.Comp_CompanyId && a.Comp_Deleted != 1).FirstOrDefault().Comp_Name;

                                if (db.Company.Where(a => a.Comp_Name == companyName && a.Comp_Deleted != 1).Count() != 0) {
                                    foreach (var cltname in db.Company.Where(a => a.Comp_Name == companyName && a.Comp_Deleted != 1).ToList()) {
                                        var co = db.Company.Where(a => a.Comp_Name == companyName && a.Comp_Deleted != 1).Count();
                                        int idAClient = cltname.Comp_CompanyId;

                                        if (db.Person.Where(a => a.Pers_PersonId == y.ID_Person && a.Pers_CompanyId == idAClient && a.Pers_Deleted != 1).Count() != 0) {
                                            var pers = db.Person.Where(a => a.Pers_PersonId == y.ID_Person && a.Pers_CompanyId == idAClient && a.Pers_Deleted != 1).FirstOrDefault();

                                            Npers += pers.Pers_LastName + " " + @pers.Pers_FirstName + "-" + pers.Pers_Title + ", ";
                                        }
                                    }
                                }
                            }
                            list.Add(Npers);
                        }
                        else
                            list.Add("");
                    }

                    list2d.Add(list);
                }
            }

            var lis = list2d;
            ViewBag.ldv = lis;

            return View();
        }

        //
        // GET: /Agent/Details/5

        public ActionResult Details(int id) {
            return View();
        }

        //
        // GET: /Agent/Create

        public ActionResult Create(int id = 0) {
            var collection = new Crmcli_AffectationProds();

            //Produit//
            var produits = new List<string>();
            if (db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted != 1).Count() != 0) {
                foreach (var x in db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted != 1).ToList()) {
                    produits.Add(x.Capt_FR);
                }
            }

            var list2d = new List<List<string>>();
            if (db.users.Where(a => a.User_Deleted != 1 && a.User_Logon != null && a.User_Logon != "Admin").Count() != 0) {
                foreach (var x in db.users.Where(a => a.User_Deleted != 1 && a.User_Logon != null && a.User_Logon != "Admin").OrderBy(a => a.User_UserId).ToList()) {
                    var list = new List<string>();
                    list.Add(x.User_UserId.ToString());
                    list.Add(x.User_LastName);
                    list.Add(x.User_FirstName);

                    list2d.Add(list);
                }
            }

            var lis = list2d;
            ViewBag.ldv = lis;

            collection.Produit = produits;
            return View(collection);
        }

        //
        // POST: /Agent/Create

        [HttpPost]
        public ActionResult Create(Crmcli_AffectationProds collection) {
            try {
                var prod = collection.Produit.FirstOrDefault();
                if (db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted != 1 && a.Capt_FR == prod).Count() != 0) {
                    var produit = db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted != 1 && a.Capt_FR == prod).FirstOrDefault();

                    if (db.Crmcli_AffectationProds.Where(a => a.AFF_Produit == produit.Capt_Code).Count() == 0) {
                        collection.AFF_Produit = produit.Capt_Code;

                        db.Crmcli_AffectationProds.Add(collection);
                        db.SaveChanges();

                        return Content("<script language='javascript' type='text/javascript'>alert('Avec succès!');</script>");
                    }
                    else {
                        var forUpdate = db.Crmcli_AffectationProds.Where(a => a.AFF_Produit == produit.Capt_Code).FirstOrDefault();

                        forUpdate.AFF_Agent = collection.AFF_Agent;

                        db.SaveChanges();

                        return Content("<script language='javascript' type='text/javascript'>alert('Avec succès!');</script>");
                    }
                }
                else
                    return Content("<script language='javascript' type='text/javascript'>alert('Erreur!');</script>");
            }
            catch {
                return Content("<script language='javascript' type='text/javascript'>alert('Erreur!');</script>");
            }
        }

        //
        // GET: /Agent/Edit/5

        public ActionResult Edit(int id) {
            var list2d = new List<List<string>>();

            if (db.users.Where(a => a.User_Deleted != 1 && a.User_Logon != null && a.User_Logon != "Admin").Count() != 0) {
                foreach (var x in db.users.Where(a => a.User_Deleted != 1 && a.User_Logon != null && a.User_Logon != "Admin").OrderBy(a => a.User_UserId).ToList()) {
                    var list = new List<string>();
                    list.Add(x.User_UserId.ToString());
                    list.Add(x.User_LastName);
                    list.Add(x.User_FirstName);

                    list2d.Add(list);
                }
            }

            //Produit//
            if (db.Crmcli_AffectationProds.Where(a => a.ID == id).Count() != 0) {
                var aff = db.Crmcli_AffectationProds.Where(a => a.ID == id).FirstOrDefault();

                if (db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted != 1 && a.Capt_Code == aff.AFF_Produit.ToString()).Count() != 0) {
                    var prod = db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted != 1 && a.Capt_Code == aff.AFF_Produit.ToString()).FirstOrDefault();
                    ViewBag.Prod = prod.Capt_FR;
                }

                ViewBag.Agent = aff.AFF_Agent;
            }

            var lis = list2d;
            ViewBag.ldv = lis;

            return View();
        }

        //
        // POST: /Agent/Edit/5

        [HttpPost]
        public ActionResult Edit(Crmcli_AffectationProds collection) {
            try {
                Crmcli_AffectationProds aff = db.Crmcli_AffectationProds.Where(a => a.ID == collection.ID).FirstOrDefault();

                if (aff.AFF_Agent != null)
                    aff.AFF_Agent = collection.AFF_Agent;
                else
                    aff.AFF_Agent = "";

                db.SaveChanges();

                return Content("<script language='javascript' type='text/javascript'>alert('Avec succès!');</script>");
            }
            catch (Exception) {
                return Content("<script language='javascript' type='text/javascript'>alert('Erreur!');</script>");
            }
        }


        //GET: /Agent/Delete/5

        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}


        // //POST: /Agent/Delete/5

        //[HttpPost]
        public ActionResult Delete(int idClient, int idPers) {
            try {
                Crmcli_UsersSession aff = db.Crmcli_UsersSession.Where(a => a.ID_Company == idClient && a.ID_Person == idPers).FirstOrDefault();

                //EDIT FUNCTION
                db.Crmcli_UsersSession.Remove(aff);

                db.SaveChanges();

                return Content("<script language='javascript' type='text/javascript'>alert('Avec succès!');</script>");
            }
            catch (Exception) {
                return Content("<script language='javascript' type='text/javascript'>alert('Erreur!');</script>");
            }
        }



        //////////////////////////SIGNATAIRE//////////////////////
        public ActionResult CreateSIGN(int idClient, int idPers) {
            var collection = new Crmcli_UsersSession();

            var cltSIGN = new List<string>();

            var clt = new List<string>();

            //Correspondances des clients//
            var companyName = db.Company.Where(a => a.Comp_CompanyId == idClient).FirstOrDefault();

            if (companyName.Comp_Name != null && companyName.Comp_Deleted != 1) {
                if (db.Company.Where(a => a.Comp_Name == companyName.Comp_Name && a.Comp_Deleted != 1).Count() != 0) {
                    foreach (var cltname in db.Company.Where(a => a.Comp_Name == companyName.Comp_Name && a.Comp_Deleted != 1).Select(a => a.Comp_CompanyId).Distinct().ToList()) {
                        if (db.Crmcli_Reg.Where(a => a.Comp_CompanyId == cltname).Count() != 0) {
                            foreach (var y in db.Person.Where(a => a.Pers_CompanyId == cltname && a.Pers_Deleted != 1).ToList()) {
                                var list = new List<string>();

                                if (cltSIGN.Contains(y.Pers_LastName + " " + y.Pers_FirstName + "-" + y.Pers_Title) == false) {
                                    cltSIGN.Add(y.Pers_LastName + " " + y.Pers_FirstName + "-" + y.Pers_Title);
                                }
                            }

                            if (db.Person.Where(a => a.Pers_CompanyId == cltname && a.Pers_Deleted != 1).Count() != 0) {
                                var y = db.Person.Where(a => a.Pers_CompanyId == cltname && a.Pers_Deleted != 1).FirstOrDefault();

                                clt.Add(y.Pers_LastName + " " + y.Pers_FirstName + "-" + y.Pers_Title);
                            }

                            ViewBag.IDCOMP = cltname;
                        }
                    }
                }
            }

            var lis = clt;
            ViewBag.ldv = lis;

            collection.SIGNNAME2 = cltSIGN;

            return View(collection);
        }

        //
        // POST: /Agent/Create

        [HttpPost]
        public ActionResult CreateSIGN(Crmcli_UsersSession collection) {
            try {
                var SignPersFonct = collection.SIGNNAME2.FirstOrDefault();
                if (db.Person.Where(a => a.Pers_LastName + " " + a.Pers_FirstName + "-" + a.Pers_Title == SignPersFonct).Count() != 0) {
                    var idPe = db.Person.Where(a => a.Pers_LastName + " " + a.Pers_FirstName + "-" + a.Pers_Title == SignPersFonct).FirstOrDefault();

                    collection.ID_Person = idPe.Pers_PersonId;
                }

                if (db.Crmcli_UsersSession.Where(a => a.ID_Company == collection.ID_Company && a.ID_Person == collection.ID_Person).Count() == 0) {
                    if (db.Crmcli_UsersSession.Where(a => a.ID_Company == collection.ID_Company).Count() < 4) {
                        db.Crmcli_UsersSession.Add(collection);
                        db.SaveChanges();

                        return Content("<script language='javascript' type='text/javascript'>alert('Avec succès!');</script>");
                    }
                    else
                        return Content("<script language='javascript' type='text/javascript'>alert('Erreur! Le nombre de signataire autorisé est atteint');</script>");
                }
                else
                    return Content("<script language='javascript' type='text/javascript'>alert('Erreur! Signataire existant');</script>");
            }
            catch (Exception) {
                return Content("<script language='javascript' type='text/javascript'>alert('Erreur!');</script>");
            }
        }

        //
        // GET: /Agent/Edit/5

        public ActionResult EditSIGN(int idClient, int idPers) {
            var collection = new Crmcli_UsersSession();

            var cltSIGN = new List<string>();

            var clt = new List<string>();

            ViewBag.CountIs = 0;

            //Correspondances des clients//
            var companyName = db.Company.Where(a => a.Comp_CompanyId == idClient).FirstOrDefault();

            if (companyName.Comp_Name != null && companyName.Comp_Deleted != 1) {
                if (db.Company.Where(a => a.Comp_Name == companyName.Comp_Name && a.Comp_Deleted != 1).Count() != 0) {
                    foreach (var cltname in db.Company.Where(a => a.Comp_Name == companyName.Comp_Name && a.Comp_Deleted != 1).Select(a => a.Comp_CompanyId).Distinct().ToList()) {
                        foreach (var y in db.Person.Where(a => a.Pers_CompanyId == cltname && a.Pers_Deleted != 1).ToList()) {
                            var list = new List<string>();

                            if (cltSIGN.Contains(y.Pers_LastName + " " + y.Pers_FirstName + "-" + y.Pers_Title) == false) {
                                cltSIGN.Add(y.Pers_LastName + " " + y.Pers_FirstName + "-" + y.Pers_Title);
                            }
                        }

                        if (idPers != 0) {
                            //Edit//
                            if (db.Person.Where(a => a.Pers_CompanyId == cltname && a.Pers_PersonId == idPers && a.Pers_Deleted != 1).Count() != 0) {
                                var y = db.Person.Where(a => a.Pers_CompanyId == cltname && a.Pers_PersonId == idPers && a.Pers_Deleted != 1).FirstOrDefault();
                                if (db.Crmcli_UsersSession.Where(a => a.ID_Company == cltname && a.ID_Person == idPers).Count() != 0) {
                                    var p = db.Crmcli_UsersSession.Where(a => a.ID_Company == cltname && a.ID_Person == idPers).FirstOrDefault();

                                    clt.Add(p.Pseudo);
                                    clt.Add(p.Mdp);
                                    clt.Add(y.Pers_LastName + " " + y.Pers_FirstName + "-" + y.Pers_Title);
                                }
                            }
                            ViewBag.CountIs = 1;
                        }
                    }
                }
            }

            var lis = clt;
            ViewBag.ldv = lis;

            ViewBag.SIGNNAME = new SelectList(cltSIGN, clt[2]);

            ViewBag.IDCOMP = idClient;
            ViewBag.IDPER = idPers;

            return View(collection);
        }

        //
        // POST: /Agent/Edit/5

        [HttpPost]
        public ActionResult EditSIGN(Crmcli_UsersSession collection) {
            try {
                Crmcli_UsersSession aff = db.Crmcli_UsersSession.Where(a => a.ID_Company == collection.ID_Company && a.ID_Person == collection.ID_Person).FirstOrDefault();

                //EDIT FUNCTION
                aff.Pseudo = collection.Pseudo;
                aff.Mdp = collection.Mdp;

                db.SaveChanges();

                return Content("<script language='javascript' type='text/javascript'>alert('Avec succès!');</script>");
            }
            catch (Exception) {
                return Content("<script language='javascript' type='text/javascript'>alert('Erreur!');</script>");
            }
        }

        public ActionResult DetailsSIGN(int id) {
            var list2d = new List<List<string>>();
            int i = 0;
            if (db.Crmcli_Reg.Where(a => a.Comp_CompanyId == id).Count() != 0) {
                if (db.Company.Where(a => a.Comp_CompanyId == id && a.Comp_Deleted != 1).Count() != 0) {
                    var compA = db.Company.Where(a => a.Comp_CompanyId == id && a.Comp_Deleted != 1).FirstOrDefault();

                    if (compA.comp_raison_sociale_af_ay_iltx != null) {
                        ViewBag.Titre = compA.comp_raison_sociale_af_ay_iltx;
                    }
                    else if (compA.Comp_Name != null) {
                        ViewBag.Titre = compA.Comp_Name;
                    }
                }
                else
                    ViewBag.Titre = "";


                foreach (var x in db.Crmcli_Reg.Where(a => a.Comp_CompanyId == id).ToList()) {
                    ViewBag.IdComp = x.Comp_CompanyId;

                    if (db.Crmcli_UsersSession.Where(a => a.ID_Company == x.Comp_CompanyId).Count() != 0) {
                        foreach (var y in db.Crmcli_UsersSession.Where(a => a.ID_Company == x.Comp_CompanyId).ToList()) {
                            var list = new List<string>();

                            var pers = db.Person.Where(a => a.Pers_PersonId == y.ID_Person && a.Pers_CompanyId == x.Comp_CompanyId && a.Pers_Deleted != 1).FirstOrDefault();

                            list.Add(pers.Pers_PersonId.ToString());
                            list.Add(pers.Pers_LastName + " " + pers.Pers_FirstName);
                            list.Add(pers.Pers_Title);
                            list.Add(y.Pseudo);
                            list.Add(y.Mdp);

                            list2d.Add(list);

                            i++;
                        }
                    }
                }
            }

            var lis = list2d;
            ViewBag.ldv = lis;

            ViewBag.Incr = i;

            return View();
        }
    }
}
