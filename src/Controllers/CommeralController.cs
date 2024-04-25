using Helpdesk.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Helpdesk.Controllers
{
    public class CommeralController : Controller
    {
        ModelHELPD db = new ModelHELPD();
        //
        // GET: /Commeral/

        /*public ActionResult Ind(int id)
        {
            if (id != null)
            {
                Session["UserId"] = id;

                if (db.users.Where(a => a.User_UserId == id && a.User_Deleted != 1).Count() != 0)
                {
                    var userAssig = db.users.Where(a => a.User_UserId == id && a.User_Deleted != 1).FirstOrDefault();
                    var agent = string.Format("{0} {1}", userAssig.User_LastName, userAssig.User_FirstName);

                    Session["NameSess"] = agent;
                }
            }

            return RedirectToAction("Index", "Commeral");
        }*/

        public ActionResult Index(int id)
        {
            List<List<string>> list2d = new List<List<string>>();

            if (id != 0)
            {
                Session["UserId"] = id;

                if (db.users.Where(a => a.User_UserId == id && a.User_Deleted != 1).Count() != 0)
                {
                    var userAssig = db.users.Where(a => a.User_UserId == id && a.User_Deleted != 1).FirstOrDefault();
                    var agent = string.Format("{0} {1}", userAssig.User_LastName, userAssig.User_FirstName);

                    Session["NameSess"] = agent;
                }
            }

            try
            {
                if (db.Crmcom_CommercialeProsp.Count() != 0)
                {
                    foreach (var x in db.Crmcom_CommercialeProsp.OrderBy(a => a.ID).ToList())
                    {
                        var list = new List<string>();

                        //ID//0
                        list.Add(x.ID.ToString());

                        //CLIENT//1
                        var client = "";
                        if (x.Comp_CompanyId != null)
                        {
                            var companyFirst = db.Company.Where(a => a.Comp_CompanyId == x.Comp_CompanyId && a.Comp_Deleted != 1).FirstOrDefault();
                            if (companyFirst != null)
                            {
                                if (companyFirst.comp_raison_sociale_af_ay_iltx != null)
                                {
                                    client = companyFirst.comp_raison_sociale_af_ay_iltx;
                                }
                                else if (companyFirst.Comp_Name != null)
                                {
                                    client = companyFirst.Comp_Name;
                                }
                            }

                        }
                        list.Add(client);

                        //TypeClient//2
                        var typeC = "";
                        if (x.ID_TypeClient != null)
                        {
                            typeC = db.Crmcom_TypeCProsp.Where(a => a.ID_TypeClient == x.ID_TypeClient).FirstOrDefault().Intitule;
                        }
                        list.Add(typeC);

                        //Reference//3
                        var reference = "";
                        if (!String.IsNullOrEmpty(x.ReferenceOppo))
                        {
                            reference = x.ReferenceOppo;
                        }
                        list.Add(reference);

                        //Saisisseur//4
                        var agent = "";
                        if (db.users.Where(a => a.User_UserId == x.User_UserId && a.User_Deleted != 1).Count() != 0)
                        {
                            var userAssig = db.users.Where(a => a.User_UserId == x.User_UserId && a.User_Deleted != 1).FirstOrDefault();
                            agent = string.Format("{0} {1}", userAssig.User_LastName, userAssig.User_FirstName);
                        }
                        list.Add(agent);

                        //Etat//5
                        var Etat = "";
                        if (x.ID_Etat != null)
                        {
                            Etat = db.Crmcom_EtatProsp.Where(a => a.ID_Etat == x.ID_Etat).FirstOrDefault().Intitule;
                        }
                        list.Add(Etat);

                        //Date Debut//6
                        var date = "";
                        if (x.Date_Debut != null)
                        {
                            date = x.Date_Debut.Value.ToShortDateString();
                        }
                        list.Add(date);

                        //Produit//7
                        var produit = "";
                        if (x.Produit != null)
                        {
                            if (db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted != 1 && a.Capt_Code == x.Produit).Count() != 0)
                            {
                                var pr = db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted != 1 && a.Capt_Code == x.Produit).FirstOrDefault();
                                produit = pr.Capt_FR;
                            }
                        }
                        list.Add(produit);
                        /*var produit = "";
                        if (x.Produit != null)
                        {
                            string[] separators = { "," };
                            string[] Px = x.Produit.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var ppx in Px)
                            {
                                var codeP = ppx;
                                if (db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted != 1 && a.Capt_Code == codeP).Count() != 0)
                                {
                                    produit += db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted != 1 && a.Capt_Code == codeP).FirstOrDefault().Capt_FR;
                                }
                            }
                        }
                        list.Add(produit);*/

                        //Classification//8
                        var classif = "";
                        if (x.ID_Classification != null)
                        {
                            classif = db.Crmcom_ClassifProsp.Where(a => a.ID_Classification == x.ID_Classification).FirstOrDefault().Intitule;
                        }
                        list.Add(classif);

                        //Source//9
                        var source = "";
                        if (x.ID_Source != null)
                        {
                            if (db.Custom_Captions.Where(a => a.Capt_Family == "Oppo_Source" && a.Capt_Deleted == null && a.Capt_Code == x.ID_Source).Count() != 0)
                            {
                                source = db.Custom_Captions.Where(a => a.Capt_Family == "Oppo_Source" && a.Capt_Deleted == null && a.Capt_Code == x.ID_Source).FirstOrDefault().Capt_FR;
                            }
                        }
                        list.Add(source);

                        //Etape softwell//10
                        var etpS = "";
                        if (x.EtapS != null)
                        {
                            if (db.Custom_Captions.Where(a => a.Capt_Family == "oppo_etapesoftwell" && a.Capt_Deleted == null && a.Capt_Code == x.EtapS).Count() != 0)
                            {
                                etpS = db.Custom_Captions.Where(a => a.Capt_Family == "oppo_etapesoftwell" && a.Capt_Deleted == null && a.Capt_Code == x.EtapS).FirstOrDefault().Capt_FR;
                            }
                        }
                        list.Add(etpS);

                        list2d.Add(list);
                    }
                }

                var lis = list2d;
                ViewBag.ldv = lis;

                return View();
            }
            catch (Exception ex)
            {
                var x = ex.Message + ex.StackTrace;
                return View();
            }
        }

        private static List<SelectListItem> PopulateAgOp()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            ModelHELPD db = new ModelHELPD();

            if (db.Custom_Captions.Where(a => a.Capt_Family == "oppo_option" && a.Capt_Deleted == null).Count() != 0)
            {
                foreach (var x in db.Custom_Captions.Where(a => a.Capt_Family == "oppo_option" && a.Capt_Deleted == null).ToList())
                {
                    var list = new List<string>();
                    items.Add(new SelectListItem
                    {
                        Text = x.Capt_FR,
                        Value = x.Capt_FR
                    });
                }
            }
            return items;
        }

        private static List<SelectListItem> PopulateAg()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            ModelHELPD db = new ModelHELPD();

            if (db.users.Where(a => a.User_Deleted != 1 && a.User_Logon != null && a.User_Logon != "Admin").Count() != 0)
            {
                foreach (var x in db.users.Where(a => a.User_Deleted != 1 && a.User_Logon != null && a.User_Logon != "Admin").OrderBy(a => a.User_UserId).ToList())
                {
                    var list = new List<string>();
                    items.Add(new SelectListItem
                    {
                        Text = x.User_LastName + " " + x.User_FirstName,
                        Value = x.User_LastName + " " + x.User_FirstName
                        //Value = x.User_UserId.ToString()
                    });
                }
            }
            return items;
        }

        private static List<SelectListItem> PopulateProdMod()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            ModelHELPD db = new ModelHELPD();

            if (db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted != 1).Count() != 0)
            {
                foreach (var x in db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted != 1).ToList())
                {
                    var list = new List<string>();
                    items.Add(new SelectListItem
                    {
                        Text = x.Capt_FR,
                        Value = x.Capt_FR
                        //Value = x.User_UserId.ToString()
                    });
                }
            }
            return items;
        }

        //
        // GET: /Demande/Create
        public ActionResult Create()
        {
            var collection = new Crmcom_CommercialeProsp();

            //Produit//
            List<string> produits = new List<string>();
            if (db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted == null).Count() != 0)
            {
                foreach (var x in db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted == null).ToList())
                {
                    produits.Add(x.Capt_FR);
                }
            }
            collection.ProduitsCollection = produits;

            //FMFP//
            List<string> fmfp = new List<string>();
            if (db.Custom_Captions.Where(a => a.Capt_Family == "oppo_fmfp" && a.Capt_Deleted == null).Count() != 0)
            {
                foreach (var x in db.Custom_Captions.Where(a => a.Capt_Family == "oppo_fmfp" && a.Capt_Deleted == null).ToList())
                {
                    fmfp.Add(x.Capt_FR);
                }
            }
            collection.FMFPCollection = fmfp;

            //Mode de commercialisation//
            List<string> modecomm = new List<string>();
            if (db.Custom_Captions.Where(a => a.Capt_Family == "oppo_modecom" && a.Capt_Deleted == null).Count() != 0)
            {
                foreach (var x in db.Custom_Captions.Where(a => a.Capt_Family == "oppo_modecom" && a.Capt_Deleted == null).ToList())
                {
                    modecomm.Add(x.Capt_FR);
                }
            }
            collection.ModeCollection = modecomm;

            //Etape SOFTWELL//
            List<string> EtapS = new List<string>();
            if (db.Custom_Captions.Where(a => a.Capt_Family == "oppo_etapesoftwell" && a.Capt_Deleted == null).Count() != 0)
            {
                foreach (var x in db.Custom_Captions.Where(a => a.Capt_Family == "oppo_etapesoftwell" && a.Capt_Deleted == null).OrderBy(a => a.Capt_Order).ToList())
                {
                    EtapS.Add(x.Capt_FR);
                }
            }
            collection.EtapSCollection = EtapS;

            //Formation//
            List<string> Formation = new List<string>();
            if (db.Custom_Captions.Where(a => a.Capt_Family == "oppo_formation" && a.Capt_Deleted == null).Count() != 0)
            {
                foreach (var x in db.Custom_Captions.Where(a => a.Capt_Family == "oppo_formation" && a.Capt_Deleted == null).ToList())
                {
                    Formation.Add(x.Capt_FR);
                }
            }
            collection.FormationCollection = Formation;

            //Monnetaire//
            List<string> budgetMon = new List<string>();
            if (db.Currency.Where(a => a.Curr_Deleted == null).Count() != 0)
            {
                foreach (var x in db.Currency.Where(a => a.Curr_Deleted == null).ToList())
                {
                    budgetMon.Add(x.Curr_Symbol);
                }
            }
            collection.BudgetMonCollection = budgetMon;

            List<string> montantOff = new List<string>();
            if (db.Currency.Where(a => a.Curr_Deleted == null).Count() != 0)
            {
                foreach (var x in db.Currency.Where(a => a.Curr_Deleted == null).ToList())
                {
                    montantOff.Add(x.Curr_Symbol);
                }
            }
            collection.MontantOffCollection = montantOff;

            //Prevision ou Réel//
            List<string> previR = new List<string>();
            previR.Add("P");
            previR.Add("R");
            collection.PrevisReel = previR;

            //P R Deadline//
            List<string> pD = new List<string>();
            pD.Add("P");
            pD.Add("R");
            collection.PDeadCollection = pD;

            //Type//
            List<string> types = new List<string>();
            if (db.Crmcom_TypeCProsp.Count() != 0)
            {
                foreach (var x in db.Crmcom_TypeCProsp.ToList())
                {
                    types.Add(x.Intitule);
                }
            }
            collection.TypesCollection = types;

            //Etat//
            List<string> etats = new List<string>();
            if (db.Crmcom_EtatProsp.Count() != 0)
            {
                foreach (var x in db.Crmcom_EtatProsp.ToList())
                {
                    etats.Add(x.Intitule);
                }
            }
            collection.EtatsCollection = etats;

            //Classifications//
            List<string> classifications = new List<string>();
            if (db.Crmcom_ClassifProsp.Count() != 0)
            {
                foreach (var x in db.Crmcom_ClassifProsp.ToList())
                {
                    classifications.Add(x.Intitule);
                }
            }
            collection.ClassificationsCollection = classifications;

            //Sources//
            List<string> sources = new List<string>();
            if (db.Custom_Captions.Where(a => a.Capt_Family == "Oppo_Source" && a.Capt_Deleted == null).Count() != 0)
            {
                foreach (var x in db.Custom_Captions.Where(a => a.Capt_Family == "Oppo_Source" && a.Capt_Deleted == null).ToList())
                {
                    sources.Add(x.Capt_FR);
                }
            }
            collection.SourcesCollection = sources;

            //Bailleur//
            List<string> bailleurs = new List<string>();
            if (db.Custom_Captions.Where(a => a.Capt_Family == "comp_bdf" && a.Capt_Deleted == null).Count() != 0)
            {
                foreach (var x in db.Custom_Captions.Where(a => a.Capt_Family == "comp_bdf" && a.Capt_Deleted == null).ToList())
                {
                    bailleurs.Add(x.Capt_FR);
                }
            }
            collection.BailleursCollection = bailleurs;

            //Rebut//
            List<string> rebuts = new List<string>();
            if (db.Crmcom_RebutProsp.Count() != 0)
            {
                foreach (var x in db.Crmcom_RebutProsp.ToList())
                {
                    rebuts.Add(x.Intitule);
                }
            }
            collection.RebutsCollection = rebuts;

            //Rebut Reasons//
            List<string> rebutsR = new List<string>();
            if (db.Crmcom_RebutReaProsp.Count() != 0)
            {
                foreach (var x in db.Crmcom_RebutReaProsp.ToList())
                {
                    rebutsR.Add(x.Intitule);
                }
            }
            collection.RebutsRCollection = rebutsR;

            //Client//
            List<List<string>> elem = new List<List<string>>();
            var elem2 = new List<string>();

            var clients = db.Company
            .Select(s => new
            {
                Text = s.Comp_Name + " / " + db.ERPIntegrations.Where(a => a.ERPI_IntegrationID == s.comp_DefaultIntId).FirstOrDefault().ERPI_IntegrationName,
                Value = s.Comp_CompanyId
            })
            .ToList();

            ViewBag.ClientsList = new SelectList(clients, "Value", "Text");

            //Campagne//
            List<string> Camp = new List<string>();
            if (db.Campaigns.Where(a => a.Camp_Deleted == null).Count() != 0)
            {
                foreach (var x in db.Campaigns.Where(a => a.Camp_Deleted == null).ToList())
                {
                    if (db.Waves.Where(a => a.Wave_CampaignId == x.Camp_CampaignId && a.Wave_Deleted == null).Count() != 0)
                    {
                        var iswave = db.Waves.Where(a => a.Wave_CampaignId == x.Camp_CampaignId && a.Wave_Deleted == null).FirstOrDefault().Wave_WaveId;

                        if (db.WaveItems.Where(a => a.WaIt_WaveId == iswave && a.WaIt_Deleted == null).Count() != 0)
                        {
                            foreach (var y in db.WaveItems.Where(a => a.WaIt_WaveId == iswave && a.WaIt_Deleted == null).ToList())
                            {
                                Camp.Add(x.Camp_Name + "/" + y.WaIt_Name);
                            }
                        }
                    }
                }
            }
            collection.CampagneCollection = Camp;

            collection.Ag = PopulateAg();
            collection.Ag2 = PopulateAg();

            collection.Op = PopulateAgOp();

            //collection.ProdMod2 = PopulateProdMod();

            return View(collection);
        }

        //public JsonResult GetRub(string prod)
        //{
        //    List<SelectListItem> items = new List<SelectListItem>();

        //    items.Add(new SelectListItem
        //    {
        //        Text = "",
        //        Value = ""
        //    });

        //    var codeProd = 0;
        //    if (db.Campaigns.Where(a => a.Camp_Name == prod && a.Camp_Deleted == null).Count() != 0)
        //    {
        //        codeProd = db.Campaigns.Where(a => a.Camp_Name == prod && a.Camp_Deleted == null).FirstOrDefault().Camp_CampaignId;
        //    }

        //    if (codeProd != 0)
        //    {
        //        if(db.Waves.Where(a => a.Wave_CampaignId == codeProd && a.Wave_Deleted == null).Count() != 0)
        //        {
        //            var iswave = db.Waves.Where(a => a.Wave_CampaignId == codeProd && a.Wave_Deleted == null).FirstOrDefault().Wave_WaveId;

        //            if(db.WaveItems.Where(a => a.WaIt_WaveId == iswave && a.WaIt_Deleted == null).Count() != 0)
        //            {
        //                foreach(var x in db.WaveItems.Where(a => a.WaIt_WaveId == iswave && a.WaIt_Deleted == null).ToList())
        //                {
        //                    items.Add(new SelectListItem
        //                    {
        //                        Text = x.WaIt_Name,
        //                        Value = x.WaIt_Name
        //                    });
        //                }
        //            }
        //        }
        //    }

        //    var json = JsonConvert.SerializeObject(items);

        //    return Json(json, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public ActionResult CreateD(List<AnalY> analY, List<rdvY> rdvY, List<demolY> demolY, List<dexcelY> dexcelY, List<demoQY> demoQY,
            List<rdvvY> rdvvY, List<negoY> negoY, List<dgescY> dgescY, List<dbpaY> dbpaY, List<dbparY> dbparY, List<rfaY> rfaY, List<OptionsY> OptionsY,
            Crmcom_CommercialeProsp collection, idForeign idForeign/*, List<String> ProduiTs*/)
        {
            try
            {
                if (Session["UserId"] == null)
                    return Content("Erreur! Reconnectez-vous svp!");
                else if (!String.IsNullOrEmpty(collection.ReferenceOppo))
                {
                    if (db.Crmcom_CommercialeProsp.Where(a => a.ReferenceOppo == collection.ReferenceOppo).Count() == 0)
                    {
                        int idAgent = int.Parse(Session["UserId"].ToString());
                        collection.User_UserId = idAgent;

                        //TypeCLient//
                        if (!String.IsNullOrEmpty(idForeign.TypeClient))
                            collection.ID_TypeClient = db.Crmcom_TypeCProsp.Where(a => a.Intitule == idForeign.TypeClient).FirstOrDefault().ID_TypeClient;
                        //Etat//
                        if (!String.IsNullOrEmpty(idForeign.Etat))
                            collection.ID_Etat = db.Crmcom_EtatProsp.Where(a => a.Intitule == idForeign.Etat).FirstOrDefault().ID_Etat;
                        //Classification//
                        if (!String.IsNullOrEmpty(idForeign.Classification))
                            collection.ID_Classification = db.Crmcom_ClassifProsp.Where(a => a.Intitule == idForeign.Classification).FirstOrDefault().ID_Classification;
                        //Source//
                        if (!String.IsNullOrEmpty(idForeign.Source))
                            collection.ID_Source = db.Custom_Captions.Where(a => a.Capt_Family == "Oppo_Source" && a.Capt_Deleted == null && a.Capt_FR == idForeign.Source).FirstOrDefault().Capt_Code;
                        //Rebut//
                        if (!String.IsNullOrEmpty(idForeign.Rebut))
                            collection.ID_Rebut = db.Crmcom_RebutProsp.Where(a => a.Intitule == idForeign.Rebut).FirstOrDefault().ID_Rebut;
                        //Rebut reason
                        if (!String.IsNullOrEmpty(idForeign.Rebut_Reason))
                            collection.ID_Rebut_Reason = db.Crmcom_RebutReaProsp.Where(a => a.Intitule == idForeign.Rebut_Reason).FirstOrDefault().ID_Rebut_Reason;
                        //Produit//
                        var prodaka = "";
                        if (!String.IsNullOrEmpty(collection.Produit))
                        {
                            prodaka = db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted == null && a.Capt_FR == collection.Produit).FirstOrDefault().Capt_Code;
                        }
                        collection.Produit = prodaka;
                        //FMFP//
                        var fmfp = "";
                        if (!String.IsNullOrEmpty(collection.FMFP))
                        {
                            fmfp = db.Custom_Captions.Where(a => a.Capt_Family == "oppo_fmfp" && a.Capt_Deleted == null && a.Capt_FR == collection.FMFP).FirstOrDefault().Capt_Code;
                        }
                        collection.FMFP = fmfp;
                        //Etape SOFTWELL//
                        var etapS = "";
                        if (!String.IsNullOrEmpty(collection.EtapS))
                        {
                            etapS = db.Custom_Captions.Where(a => a.Capt_Family == "oppo_etapesoftwell" && a.Capt_Deleted == null && a.Capt_FR == collection.EtapS).FirstOrDefault().Capt_Code;
                        }
                        collection.EtapS = etapS;
                        //Mode de commercialisation//
                        var ModeComm = "";
                        if (!String.IsNullOrEmpty(collection.ModeComm))
                        {
                            ModeComm = db.Custom_Captions.Where(a => a.Capt_Family == "oppo_modecom" && a.Capt_Deleted == null && a.Capt_FR == collection.ModeComm).FirstOrDefault().Capt_Code;
                        }
                        collection.ModeComm = ModeComm;
                        //Apporteur affraire//
                        var aport = "";
                        if (!String.IsNullOrEmpty(collection.ApporteurAffaire))
                        {
                            aport = collection.ApporteurAffaire;
                        }
                        collection.ApporteurAffaire = aport;
                        //Formation//
                        var Formation = "";
                        if (!String.IsNullOrEmpty(collection.Formation))
                        {
                            Formation = db.Custom_Captions.Where(a => a.Capt_Family == "oppo_formation" && a.Capt_Deleted == null && a.Capt_FR == collection.Formation).FirstOrDefault().Capt_Code;
                        }
                        collection.Formation = Formation;

                        ///Budget Devise//
                        var isBdg = "";
                        if (!String.IsNullOrEmpty(collection.BudgetDevise))
                        {
                            if (db.Currency.Where(a => a.Curr_Symbol == collection.BudgetDevise && a.Curr_Deleted == null).Count() != 0)
                            {
                                isBdg = db.Currency.Where(a => a.Curr_Symbol == collection.BudgetDevise && a.Curr_Deleted == null).FirstOrDefault().Curr_Symbol;
                            }
                        }
                        collection.BudgetDevise = isBdg;

                        ///Montat offre Devise//
                        var isBdgMOF = "";
                        if (!String.IsNullOrEmpty(collection.MontantOffreDevise))
                        {
                            if (db.Currency.Where(a => a.Curr_Symbol == collection.MontantOffreDevise && a.Curr_Deleted == null).Count() != 0)
                            {
                                isBdgMOF = db.Currency.Where(a => a.Curr_Symbol == collection.MontantOffreDevise && a.Curr_Deleted == null).FirstOrDefault().Curr_Symbol;
                            }
                        }
                        collection.MontantOffreDevise = isBdgMOF;

                        //Option//
                        //Analyse des besoins//
                        var Option = "";
                        if (OptionsY != null)
                        {
                            foreach (var x in OptionsY.ToList())
                            {
                                if (db.Custom_Captions.Where(a => a.Capt_Family == "oppo_option" && a.Capt_Deleted == null && a.Capt_FR == x.Options).Count() != 0)
                                {
                                    var isCode = db.Custom_Captions.Where(a => a.Capt_Family == "oppo_option" && a.Capt_Deleted == null && a.Capt_FR == x.Options).FirstOrDefault().Capt_Code;

                                    Option += isCode + ",";
                                }
                            }
                        }
                        collection.Options = Option;

                        //Bailleur de fond//
                        if (!String.IsNullOrEmpty(collection.Bailleur_Fond))
                            collection.Bailleur_Fond = db.Custom_Captions.Where(a => a.Capt_Family == "comp_bdf" && a.Capt_Deleted == null & a.Capt_FR == collection.Bailleur_Fond).FirstOrDefault().Capt_Code;

                        //CAMPAGNE//
                        var CampName = "";
                        var WaitName = "";
                        if (!String.IsNullOrEmpty(collection.CCAMP))
                        {
                            foreach (var campW in collection.CCAMP.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                if (String.IsNullOrEmpty(CampName))
                                    CampName = campW;
                                else
                                    WaitName = campW;
                            }
                        }

                        if (!String.IsNullOrEmpty(CampName))
                        {
                            var isCamp = db.Campaigns.Where(a => a.Camp_Name == CampName).FirstOrDefault();
                            collection.Camp_CampaignId = isCamp.Camp_CampaignId;
                        }

                        if (!String.IsNullOrEmpty(WaitName))
                        {
                            var isWAVE = db.Waves.Where(a => a.Wave_CampaignId == collection.Camp_CampaignId).FirstOrDefault();
                            if (db.WaveItems.Where(a => a.WaIt_WaveId == isWAVE.Wave_WaveId && a.WaIt_Name == WaitName).Count() != 0)
                            {
                                var isWait = db.WaveItems.Where(a => a.WaIt_WaveId == isWAVE.Wave_WaveId && a.WaIt_Name == WaitName).FirstOrDefault();
                                collection.WaIt_WaveItemId = isWait.WaIt_WaveItemId;
                            }
                        }

                        DateTime now = DateTime.Now;

                        collection.Date_Creation = now;
                        collection.Date_Update = now;
                        collection.AssAnnuel = collection.AssAnnuel;
                        collection.AssDemarage = collection.AssDemarage;
                        collection.AssFormation = collection.AssFormation;
                        collection.DateEnvoieOFF = collection.DateEnvoieOFF;
                        collection.MoisPrev = collection.MoisPrev;

                        db.Crmcom_CommercialeProsp.Add(collection);
                        db.SaveChanges();

                        ModelHELPD ft = new ModelHELPD();
                        var comer = ft.Crmcom_CommercialeProsp.Where(a => a.ReferenceOppo == collection.ReferenceOppo).FirstOrDefault().ID;

                        //Analyse des besoins//
                        if (analY != null)
                        {
                            foreach (var x in analY.ToList())
                            {
                                var ins = new Crmcom_AnalBesoinProsp
                                {
                                    ID_Prosp = comer,
                                    Date = DateTime.Parse(x.Date),
                                    Rmq = x.Rmq,
                                    Date_CRR = DateTime.Parse(x.DateCRR),
                                    P1 = x.P1,
                                    P2 = x.P2
                                };

                                ft.Crmcom_AnalBesoinProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }
                        //Autr rdv//
                        if (rdvY != null)
                        {
                            foreach (var x in rdvY.ToList())
                            {
                                var ins = new Crmcom_AutrRDVProsp
                                {
                                    ID_Prosp = comer,
                                    Date = DateTime.Parse(x.Date),
                                    Rmq = x.Rmq,
                                    Date_CRR = DateTime.Parse(x.DateCRR),
                                    P1 = x.P1,
                                    P2 = x.P2
                                };

                                ft.Crmcom_AutrRDVProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }
                        //Remis devis excel//
                        if (dexcelY != null)
                        {
                            foreach (var x in dexcelY.ToList())
                            {
                                var ins = new Crmcom_RemiDevisProsp
                                {
                                    ID_Prosp = comer,
                                    Date = DateTime.Parse(x.Date),
                                    P1 = x.P1
                                };

                                ft.Crmcom_RemiDevisProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }
                        //Autr rdvv//
                        if (rdvvY != null)
                        {
                            foreach (var x in rdvvY.ToList())
                            {
                                var ins = new Crmcom_AutrRDVvProsp
                                {
                                    ID_Prosp = comer,
                                    Date = DateTime.Parse(x.Date),
                                    Rmq = x.Rmq,
                                    Date_CRR = DateTime.Parse(x.DateCRR),
                                    P1 = x.P1,
                                    P2 = x.P2
                                };

                                ft.Crmcom_AutrRDVvProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }
                        //NEGO//
                        if (negoY != null)
                        {
                            foreach (var x in negoY.ToList())
                            {
                                var ins = new Crmcom_NegoProsp
                                {
                                    ID_Prosp = comer,
                                    Date = DateTime.Parse(x.Date),
                                    Rmq = x.Rmq,
                                    Date_CRR = DateTime.Parse(x.DateCRR),
                                    P1 = x.P1,
                                    P2 = x.P2
                                };

                                ft.Crmcom_NegoProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }
                        //Envoi devis GESCOM//
                        if (dgescY != null)
                        {
                            foreach (var x in dgescY.ToList())
                            {
                                var ins = new Crmcom_DevisGescomProsp
                                {
                                    ID_Prosp = comer,
                                    Date = DateTime.Parse(x.Date),
                                    P1 = x.P1
                                };

                                ft.Crmcom_DevisGescomProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }
                        //Envoi BPA SAGE//
                        if (dbpaY != null)
                        {
                            foreach (var x in dbpaY.ToList())
                            {
                                var ins = new Crmcom_BPAProsp
                                {
                                    ID_Prosp = comer,
                                    Date = DateTime.Parse(x.Date),
                                    P1 = x.P1
                                };

                                ft.Crmcom_BPAProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }
                        //Envoi BPA SAGE//
                        if (dbparY != null)
                        {
                            foreach (var x in dbparY.ToList())
                            {
                                var ins = new Crmcom_BPARELProsp
                                {
                                    ID_Prosp = comer,
                                    Date = DateTime.Parse(x.Date),
                                    P1 = x.P1
                                };

                                ft.Crmcom_BPARELProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }
                        //Remise FA//
                        if (rfaY != null)
                        {
                            foreach (var x in rfaY.ToList())
                            {
                                var ins = new Crmcom_FAProsp
                                {
                                    ID_Prosp = comer,
                                    Date = DateTime.Parse(x.Date),
                                    P1 = x.P1
                                };

                                ft.Crmcom_FAProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }
                        //Demo logiciel//
                        if (demolY != null)
                        {
                            foreach (var x in demolY.ToList())
                            {
                                var actt = "";
                                string[] separators = { "," };
                                if (!String.IsNullOrEmpty(x.Acteur))
                                {
                                    string[] agentL = x.Acteur.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                    foreach (var ag in agentL)
                                    {
                                        var nomPrenom = ag;
                                        foreach (var y in db.users.ToList())
                                        {
                                            if ((y.User_LastName + " " + y.User_FirstName == nomPrenom) && y.User_Deleted != 1)
                                                actt += y.User_UserId + ",";
                                        }
                                    }
                                }

                                var ins = new Crmcom_DemoProsp
                                {
                                    ID_Prosp = comer,
                                    Date = DateTime.Parse(x.Date),
                                    Agent = actt,
                                    Rmq = x.Rmq,
                                    Date_CRR = DateTime.Parse(x.DateCRR),
                                    P1 = x.P1,
                                    P2 = x.P2
                                };

                                ft.Crmcom_DemoProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }
                        //Demo maquettées//
                        if (demoQY != null)
                        {
                            foreach (var x in demoQY.ToList())
                            {
                                var actt = "";
                                string[] separators = { "," };
                                if (!String.IsNullOrEmpty(x.Acteur))
                                {
                                    string[] agentL = x.Acteur.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                    foreach (var ag in agentL)
                                    {
                                        var nomPrenom = ag;
                                        foreach (var y in db.users.ToList())
                                        {
                                            if ((y.User_LastName + " " + y.User_FirstName == nomPrenom) && y.User_Deleted != 1)
                                                actt += y.User_UserId + ",";
                                        }
                                    }
                                }

                                var ins = new Crmcom_DemoMaqProsp
                                {
                                    ID_Prosp = comer,
                                    Date = DateTime.Parse(x.Date),
                                    Agent = actt,
                                    Rmq = x.Rmq,
                                    Date_CRR = DateTime.Parse(x.DateCRR),
                                    P1 = x.P1,
                                    P2 = x.P2
                                };

                                ft.Crmcom_DemoMaqProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }

                        return Content("Avec succès");
                    }
                    else
                        return Content("Erreur! La référence doit être unique!");
                }
                else
                    return Content("Erreur! La référence est obligatoire!");
            }
            catch (Exception)
            {
                return Content("Erreur!");
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                List<List<string>> listCommPros = new List<List<string>>();
                var listPro = new List<string>();

                List<List<string>> listABY = new List<List<string>>();

                List<List<string>> OPY = new List<List<string>>();

                List<List<string>> listRdvY = new List<List<string>>();

                List<List<string>> listRdY = new List<List<string>>();

                List<List<string>> listRdvvY = new List<List<string>>();

                List<List<string>> listnegoY = new List<List<string>>();

                List<List<string>> listdgY = new List<List<string>>();

                List<List<string>> listbpaY = new List<List<string>>();

                List<List<string>> listbparY = new List<List<string>>();

                List<List<string>> listfaY = new List<List<string>>();

                List<List<string>> listdemY = new List<List<string>>();

                List<List<string>> listdemqY = new List<List<string>>();

                if (db.Crmcom_CommercialeProsp.Where(a => a.ID == id).Count() != 0)
                {
                    var isCom = db.Crmcom_CommercialeProsp.Where(a => a.ID == id).FirstOrDefault();

                    //ID//0
                    listPro.Add(isCom.ID.ToString());
                    //Reference//1
                    listPro.Add(isCom.ReferenceOppo);
                    //Date début//2
                    var datedeb = "";
                    if (isCom.Date_Debut != null)
                        datedeb = isCom.Date_Debut.Value.ToString("yyyy-MM-dd");
                    listPro.Add(datedeb);
                    //Nbr user//3
                    var nbruser = "";
                    if (isCom.Nombre_User != null)
                        nbruser = isCom.Nombre_User.ToString();
                    listPro.Add(nbruser);
                    //Nbr salarié//4
                    var nbrSal = "";
                    if (isCom.Nombre_Salarie != null)
                        nbrSal = isCom.Nombre_Salarie.ToString();
                    listPro.Add(nbrSal);
                    //Existant//5
                    var existant = "";
                    if (isCom.Existant != null)
                        existant = isCom.Existant;
                    listPro.Add(existant);
                    //Analyse besoin//6
                    var analB = "";
                    if (isCom.Analyse_Besoin != null)
                        analB = isCom.Analyse_Besoin;
                    listPro.Add(analB);
                    //Budget//7
                    var budg = "";
                    if (isCom.Budget != null)
                        budg = isCom.Budget;
                    listPro.Add(budg);
                    //Deadline//8
                    var deadline = "";
                    if (isCom.DeadLine != null)
                        deadline = isCom.DeadLine.Value.ToString("yyyy-MM-dd");
                    listPro.Add(deadline);
                    //Interloculteur//9
                    var interloc = "";
                    if (isCom.Interlocuteur != null)
                        interloc = isCom.Interlocuteur;
                    listPro.Add(interloc);
                    //Decisionnaire//10
                    var decisio = "";
                    if (isCom.Decisionnaire != null)
                        decisio = isCom.Decisionnaire;
                    listPro.Add(decisio);
                    //PA//11
                    var pa = "";
                    if (isCom.PA != null)
                        pa = isCom.PA;
                    listPro.Add(pa);
                    //DatePropoContrat//12
                    var propoC = "";
                    if (isCom.Date_ProposContrat != null)
                        propoC = isCom.Date_ProposContrat.Value.ToString("yyyy-MM-dd");
                    listPro.Add(propoC);
                    //DateProgForm//13
                    var progForm = "";
                    if (isCom.Date_ProgForm != null)
                        progForm = isCom.Date_ProgForm.Value.ToString("yyyy-MM-dd");
                    listPro.Add(progForm);
                    //DatePrecoTech//14
                    var precoTech = "";
                    if (isCom.Date_PrecoTech != null)
                        precoTech = isCom.Date_PrecoTech.Value.ToString("yyyy-MM-dd");
                    listPro.Add(precoTech);
                    //DateProFormat//15
                    var proforma = "";
                    if (isCom.Date_ProForma != null)
                        proforma = isCom.Date_ProForma.Value.ToString("yyyy-MM-dd");
                    listPro.Add(proforma);
                    //Remise//16
                    var remise = "";
                    if (isCom.Remise != null)
                        remise = isCom.Remise;
                    listPro.Add(remise);
                    //ModalPaiem//17
                    var modalP = "";
                    if (isCom.Modalite_Paiement != null)
                        modalP = isCom.Modalite_Paiement;
                    listPro.Add(modalP);
                    //DateRecepBC//18
                    var recepBC = "";
                    if (isCom.Date_ReceptionBC != null)
                        recepBC = isCom.Date_ReceptionBC.Value.ToString("yyyy-MM-dd");
                    listPro.Add(recepBC);
                    //DureeTraitem//19
                    var durT = "";
                    if (isCom.Duree_Traitement != null)
                        durT = isCom.Duree_Traitement;
                    listPro.Add(durT);
                    //dateReceptAcompte//20
                    var dateRecpAc = "";
                    if (isCom.Date_ReceptionAcompte != null)
                        dateRecpAc = isCom.Date_ReceptionAcompte.Value.ToString("yyyy-MM-dd");
                    listPro.Add(dateRecpAc);
                    //DateBL//21
                    var datebl = "";
                    if (isCom.Date_BL != null)
                        datebl = isCom.Date_BL.Value.ToString("yyyy-MM-dd");
                    listPro.Add(datebl);
                    //dateContrat//22
                    var dateContrat = "";
                    if (isCom.Date_Contrat != null)
                        dateContrat = isCom.Date_Contrat.Value.ToString("yyyy-MM-dd");
                    listPro.Add(dateContrat);
                    //DateCRL//23
                    var dateCrl = "";
                    if (isCom.Date_CRL != null)
                        dateCrl = isCom.Date_CRL.Value.ToString("yyyy-MM-dd");
                    listPro.Add(dateCrl);
                    //DateRetourContrat//24
                    var dateRetouContrat = "";
                    if (isCom.Date_RetourContrat != null)
                        dateRetouContrat = isCom.Date_RetourContrat.Value.ToString("yyyy-MM-dd");
                    listPro.Add(dateRetouContrat);

                    //Client//25
                    var client = "";
                    if (isCom.Comp_CompanyId != null)
                    {
                        if (db.Company.Where(a => a.Comp_CompanyId == isCom.Comp_CompanyId && a.Comp_Deleted != 1).Count() != 0)
                        {
                            var companyFirst = db.Company.Where(a => a.Comp_CompanyId == isCom.Comp_CompanyId && a.Comp_Deleted != 1).FirstOrDefault();
                            /*if (companyFirst.comp_raison_sociale_af_ay_iltx != null)
                            {
                                client = companyFirst.comp_raison_sociale_af_ay_iltx;
                            }
                            else */
                            var integ = " / ";
                            if (companyFirst.Comp_Name != null)
                            {
                                if (db.ERPIntegrations.Where(a => a.ERPI_IntegrationID == companyFirst.comp_DefaultIntId).Count() != 0)
                                {
                                    integ = db.ERPIntegrations.Where(a => a.ERPI_IntegrationID == companyFirst.comp_DefaultIntId).FirstOrDefault().ERPI_IntegrationName;
                                }
                                client = companyFirst.Comp_Name + integ;
                            }
                        }
                    }
                    listPro.Add(client);

                    //Bailleur de fond//26
                    var baF = "";
                    if (isCom.Bailleur_Fond != null)
                    {
                        if (db.Custom_Captions.Where(a => a.Capt_Family == "comp_bdf" && a.Capt_Deleted != 1 && a.Capt_Code == isCom.Bailleur_Fond).Count() != 0)
                        {
                            baF = db.Custom_Captions.Where(a => a.Capt_Family == "comp_bdf" && a.Capt_Deleted != 1 && a.Capt_Code == isCom.Bailleur_Fond).FirstOrDefault().Capt_FR;
                        }
                    }
                    listPro.Add(baF);

                    //Type client//27
                    var typC = "";
                    if (isCom.ID_TypeClient != null)
                    {
                        if (db.Crmcom_TypeCProsp.Where(a => a.ID_TypeClient == isCom.ID_TypeClient).Count() != 0)
                        {
                            typC = db.Crmcom_TypeCProsp.Where(a => a.ID_TypeClient == isCom.ID_TypeClient).FirstOrDefault().Intitule;
                        }
                    }
                    listPro.Add(typC);

                    //Etat//28
                    var etat = "";
                    if (isCom.ID_Etat != null)
                    {
                        if (db.Crmcom_EtatProsp.Where(a => a.ID_Etat == isCom.ID_Etat).Count() != 0)
                        {
                            etat = db.Crmcom_EtatProsp.Where(a => a.ID_Etat == isCom.ID_Etat).FirstOrDefault().Intitule;
                        }
                    }
                    listPro.Add(etat);

                    //Rebut//29
                    var rebut = "";
                    if (isCom.ID_Rebut != null)
                    {
                        if (db.Crmcom_RebutProsp.Where(a => a.ID_Rebut == isCom.ID_Rebut).Count() != 0)
                        {
                            rebut = db.Crmcom_RebutProsp.Where(a => a.ID_Rebut == isCom.ID_Rebut).FirstOrDefault().Intitule;
                        }
                    }
                    listPro.Add(rebut);
                    //dateRebut Fiderana
                    var dateRebut = "";
                    if (isCom.Date_REBUT != null)
                    {
                        if (db.Crmcom_RebutProsp.Where(a => a.ID_Rebut == isCom.ID_Rebut).Count() != 0)
                        {
                             dateRebut = db.Crmcom_CommercialeProsp.Where(a => a.ID_Rebut == isCom.ID_Rebut).FirstOrDefault().ToString();
                            //dateRebut = Convert.ToDateTime(aaaa);
                        }

                    }
                    listPro.Add(dateRebut);
                    //Rebut Reason//30
                    var rebutR = "";
                    if (isCom.ID_Rebut_Reason != null)
                    {
                        if (db.Crmcom_RebutReaProsp.Where(a => a.ID_Rebut_Reason == isCom.ID_Rebut_Reason).Count() != 0)
                        {
                            rebutR = db.Crmcom_RebutReaProsp.Where(a => a.ID_Rebut_Reason == isCom.ID_Rebut_Reason).FirstOrDefault().Intitule;
                        }
                    }
                    listPro.Add(rebutR);

                    //Produit//31
                    var prod = "";
                    if (isCom.Produit != null)
                    {
                        if (db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted == null && a.Capt_Code == isCom.Produit).Count() != 0)
                        {
                            prod = db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted == null && a.Capt_Code == isCom.Produit).FirstOrDefault().Capt_FR;
                        }
                    }
                    listPro.Add(prod);

                    //Classification//32
                    var Classif = "";
                    if (isCom.ID_Classification != null)
                    {
                        if (db.Crmcom_ClassifProsp.Where(a => a.ID_Classification == isCom.ID_Classification).Count() != 0)
                        {
                            Classif = db.Crmcom_ClassifProsp.Where(a => a.ID_Classification == isCom.ID_Classification).FirstOrDefault().Intitule;
                        }
                    }
                    listPro.Add(Classif);

                    //Source//33
                    var source = "";
                    if (isCom.ID_Source != null)
                    {
                        if (db.Custom_Captions.Where(a => a.Capt_Family == "Oppo_Source" && a.Capt_Deleted == null && a.Capt_Code == isCom.ID_Source).Count() != 0)
                        {
                            source = db.Custom_Captions.Where(a => a.Capt_Family == "Oppo_Source" && a.Capt_Deleted == null && a.Capt_Code == isCom.ID_Source).FirstOrDefault().Capt_FR;
                        }
                    }
                    listPro.Add(source);

                    //User Name//34
                    var nameuser = "";
                    if (isCom.User_UserId != null)
                    {
                        var userAssig = db.users.Where(a => a.User_UserId == isCom.User_UserId).FirstOrDefault();
                        nameuser = string.Format("{0} {1}", userAssig.User_LastName, userAssig.User_FirstName);
                    }
                    listPro.Add(nameuser);

                    //Etape SOFTWELL//35
                    var etpS = "";
                    if (isCom.EtapS != null)
                    {
                        if (db.Custom_Captions.Where(a => a.Capt_Family == "oppo_etapesoftwell" && a.Capt_Deleted == null && a.Capt_Code == isCom.EtapS).Count() != 0)
                        {
                            etpS = db.Custom_Captions.Where(a => a.Capt_Family == "oppo_etapesoftwell" && a.Capt_Deleted == null && a.Capt_Code == isCom.EtapS).FirstOrDefault().Capt_FR;
                        }
                    }
                    listPro.Add(etpS);

                    //Mode de commercialisation//36
                    var modeComm = "";
                    if (isCom.ModeComm != null)
                    {
                        if (db.Custom_Captions.Where(a => a.Capt_Family == "oppo_modecom" && a.Capt_Deleted == null && a.Capt_Code == isCom.ModeComm).Count() != 0)
                        {
                            modeComm = db.Custom_Captions.Where(a => a.Capt_Family == "oppo_modecom" && a.Capt_Deleted == null && a.Capt_Code == isCom.ModeComm).FirstOrDefault().Capt_FR;
                        }
                    }
                    listPro.Add(modeComm);

                    //Formation//37
                    var forma = "";
                    if (isCom.Formation != null)
                    {
                        if (db.Custom_Captions.Where(a => a.Capt_Family == "oppo_formation" && a.Capt_Deleted == null && a.Capt_Code == isCom.Formation).Count() != 0)
                        {
                            forma = db.Custom_Captions.Where(a => a.Capt_Family == "oppo_formation" && a.Capt_Deleted == null && a.Capt_Code == isCom.Formation).FirstOrDefault().Capt_FR;
                        }
                    }
                    listPro.Add(forma);

                    //Devis//38
                    var devis = "";
                    if (isCom.BudgetDevise != null)
                    {
                        devis = isCom.BudgetDevise;
                    }
                    listPro.Add(devis);

                    //Apporteur affaire//39
                    var apportAff = "";
                    if (isCom.ApporteurAffaire != null)
                        apportAff = isCom.ApporteurAffaire;
                    listPro.Add(apportAff);

                    //P/R Date remise proposition contrat// 843 40
                    //P1//
                    var prpc1 = "P";
                    if (isCom.P1ProposContrat != null)
                        prpc1 = isCom.P1ProposContrat;
                    listPro.Add(prpc1);

                    ///R Date remise PROG FORM // 861 41
                    var prgf1 = "P";
                    if (isCom.P1ProgForm != null)
                        prgf1 = isCom.P1ProgForm;
                    listPro.Add(prgf1);

                    ///R Date remise PRECO TECH // 879 42
                    var ppreco1 = "P";
                    if (isCom.P1PrecoTech != null)
                        ppreco1 = isCom.P1PrecoTech;
                    listPro.Add(ppreco1);

                    ///R Date envoi PRO FORMA // 1250 43
                    var pprof1 = "P";
                    if (isCom.P1ProForma != null)
                        pprof1 = isCom.P1ProForma;
                    listPro.Add(pprof1);

                    ///R Date réception BC // 1290 44
                    var prbc = "P";
                    if (isCom.P1ReceptionBC != null)
                        prbc = isCom.P1ReceptionBC;
                    listPro.Add(prbc);

                    ///R Date réception ACOMPTE // 1317 45
                    var prac = "P";
                    if (isCom.P1ReceptionAcompte != null)
                        prac = isCom.P1ReceptionAcompte;
                    listPro.Add(prac);

                    ///R Date de rdv remise BL = LIVRAISON LOGICIEL // 1449 46
                    var pbl = "P";
                    if (isCom.P1BL != null)
                        pbl = isCom.P1BL;
                    listPro.Add(pbl);

                    ///R CONTRAT // 1528 47
                    var pcon = "P";
                    if (isCom.P1Contrat != null)
                        pcon = isCom.P1Contrat;
                    listPro.Add(pcon);

                    //PR Date envoi CRL // 1546 48
                    var pcrl = "P";
                    if (isCom.P1CRL != null)
                        pcrl = isCom.P1CRL;
                    listPro.Add(pcrl);

                    //PR Date retour contrat // 1564 49
                    var prcon = "P";
                    if (isCom.P1RetourContrat != null)
                        prcon = isCom.P1RetourContrat;
                    listPro.Add(prcon);

                    //MKT// 50
                    var MKT = "";
                    if (isCom.Camp_CampaignId != null && isCom.WaIt_WaveItemId != null)
                    {
                        var isCamp = db.Campaigns.Where(a => a.Camp_CampaignId == isCom.Camp_CampaignId).FirstOrDefault();
                        var isWait = db.WaveItems.Where(a => a.WaIt_WaveItemId == isCom.WaIt_WaveItemId).FirstOrDefault();

                        MKT = isCamp.Camp_Name + "/" + isWait.WaIt_Name;
                    }
                    listPro.Add(MKT);

                    //FMFP// 51
                    var FMFP = "";
                    if (isCom.FMFP != null)
                    {
                        if (db.Custom_Captions.Where(a => a.Capt_Family == "oppo_fmfp" && a.Capt_Deleted == null && a.Capt_Code == isCom.FMFP).Count() != 0)
                        {
                            var isFMFP = db.Custom_Captions.Where(a => a.Capt_Family == "oppo_fmfp" && a.Capt_Deleted == null && a.Capt_Code == isCom.FMFP).FirstOrDefault();

                            FMFP = isFMFP.Capt_FR;
                        }
                    }
                    listPro.Add(FMFP);

                    //Date fin//52//53
                    var datefin = "";
                    if (isCom.Date_Fin != null)
                        datefin = isCom.Date_Fin.Value.ToString("yyyy-MM-dd");
                    listPro.Add(datefin);

                    //P R Deadline//53
                    var pdead = "P";
                    if (isCom.PDead == "R")
                        pdead = isCom.PDead;
                    listPro.Add(pdead);

                    //MOF/54
                    var mof = "";
                    if (isCom.MontantOffre != null)
                        mof = isCom.MontantOffre;
                    listPro.Add(mof);

                    //DevisMOF//55
                    var mofD = "";
                    if (isCom.MontantOffreDevise != null)
                    {
                        mofD = isCom.MontantOffreDevise;
                    }
                    listPro.Add(mofD);

                    //MPF/56
                    var mpf = "";
                    if (isCom.MPF != null)
                        mpf = isCom.MPF;
                    listPro.Add(mpf);

                    //dateEnvoieOFF//58
                    var dateEnvoieOFF = "";
                    if (isCom.DateEnvoieOFF != null)
                    {
                        dateEnvoieOFF = db.Crmcom_CommercialeProsp.FirstOrDefault(a => a.ID == isCom.ID).DateEnvoieOFF.Value.ToString("yyyy-MM-dd");
                    }
                    listPro.Add(dateEnvoieOFF);
                    //MoisPrev//59
                    var MoisPrev = "";
                    if (isCom.MoisPrev != null)
                    {
                        MoisPrev = db.Crmcom_CommercialeProsp.FirstOrDefault(a => a.ID == isCom.ID).MoisPrev.Value.ToString("yyyy-MM-dd");
                    }
                    listPro.Add(MoisPrev);
                    //AssDemarage//60
                    var AssDemarage = "";
                    if (isCom.MoisPrev != null)
                    {
                        AssDemarage = db.Crmcom_CommercialeProsp.FirstOrDefault(a => a.ID == isCom.ID).AssDemarage.ToString();
                    }
                    listPro.Add(AssDemarage);
                    //AssAnnuelle//61
                    var AssAnnuelle = "";
                    if (isCom.MoisPrev != null)
                    {
                        AssAnnuelle = db.Crmcom_CommercialeProsp.FirstOrDefault(a => a.ID == isCom.ID).AssAnnuel.ToString();
                    }
                    listPro.Add(AssAnnuelle);
                    //assFormation//62
                    var assFormation = "";
                    if (isCom.MoisPrev != null)
                    {
                        assFormation = db.Crmcom_CommercialeProsp.FirstOrDefault(a => a.ID == isCom.ID).AssAnnuel.ToString();
                    }
                    listPro.Add(assFormation);

                    listCommPros.Add(listPro);

                    //OPTIONS//
                    if (isCom.Options != null)
                    {
                        string[] separators = { "," };
                        string[] agentL = isCom.Options.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var ag in agentL)
                        {
                            var listOP = new List<string>();

                            if (db.Custom_Captions.Where(a => a.Capt_Family == "oppo_option" && a.Capt_Deleted == null && a.Capt_Code == ag).Count() != 0)
                            {
                                var isOp = db.Custom_Captions.Where(a => a.Capt_Family == "oppo_option" && a.Capt_Deleted == null && a.Capt_Code == ag).FirstOrDefault().Capt_FR;
                                listOP.Add(isOp);
                            }

                            OPY.Add(listOP);
                        }
                    }

                    //Analse des besoins//
                    if (db.Crmcom_AnalBesoinProsp.Where(a => a.ID_Prosp == isCom.ID).Count() != 0)
                    {
                        foreach (var x in db.Crmcom_AnalBesoinProsp.Where(a => a.ID_Prosp == isCom.ID).ToList())
                        {
                            var listAB = new List<string>();
                            var isr = db.Crmcom_AnalBesoinProsp.Where(a => a.ID_AnalBesoin == x.ID_AnalBesoin).FirstOrDefault();
                            //date//0
                            var date = "";
                            if (isr.Date != null)
                                date = isr.Date.Value.ToString("yyyy-MM-dd");
                            listAB.Add(date);
                            //P1//
                            var p1 = "";
                            if (isr.P1 != null)
                                p1 = isr.P1;
                            listAB.Add(p1);
                            //Rmq//1
                            var rmq = "";
                            if (isr.Rmq != null)
                                rmq = isr.Rmq.Replace("\\n", "<br>");
                            listAB.Add(rmq);
                            //dateCRR//2
                            var dateCRR = "";
                            if (isr.Date_CRR != null)
                                dateCRR = isr.Date_CRR.Value.ToString("yyyy-MM-dd");
                            listAB.Add(dateCRR);
                            //P2//
                            var p2 = "";
                            if (isr.P2 != null)
                                p2 = isr.P2;
                            listAB.Add(p2);

                            listABY.Add(listAB);
                        }
                    }

                    //Autre rdv//
                    if (db.Crmcom_AutrRDVProsp.Where(a => a.ID_Prosp == isCom.ID).Count() != 0)
                    {
                        foreach (var x in db.Crmcom_AutrRDVProsp.Where(a => a.ID_Prosp == isCom.ID).ToList())
                        {
                            var listRdv = new List<string>();
                            var isr = db.Crmcom_AutrRDVProsp.Where(a => a.ID_AutrRDV == x.ID_AutrRDV).FirstOrDefault();
                            //date//0
                            var date = "";
                            if (isr.Date != null)
                                date = isr.Date.Value.ToString("yyyy-MM-dd");
                            listRdv.Add(date);
                            //P1//
                            var p1 = "";
                            if (isr.P1 != null)
                                p1 = isr.P1;
                            listRdv.Add(p1);
                            //Rmq//1
                            var rmq = "";
                            if (isr.Rmq != null)
                                rmq = isr.Rmq.Replace("\\n", "<br>");
                            listRdv.Add(rmq);
                            //dateCRR//2
                            var dateCRR = "";
                            if (isr.Date_CRR != null)
                                dateCRR = isr.Date_CRR.Value.ToString("yyyy-MM-dd");
                            listRdv.Add(dateCRR);
                            //P2//
                            var p2 = "";
                            if (isr.P2 != null)
                                p2 = isr.P2;
                            listRdv.Add(p2);

                            listRdvY.Add(listRdv);
                        }
                    }

                    //Remise devis//
                    if (db.Crmcom_RemiDevisProsp.Where(a => a.ID_Prosp == isCom.ID).Count() != 0)
                    {
                        foreach (var x in db.Crmcom_RemiDevisProsp.Where(a => a.ID_Prosp == isCom.ID).ToList())
                        {
                            var listRd = new List<string>();
                            var isr = db.Crmcom_RemiDevisProsp.Where(a => a.ID_DateRemisDevis == x.ID_DateRemisDevis).FirstOrDefault();
                            //date//0
                            var date = "";
                            if (isr.Date != null)
                                date = isr.Date.Value.ToString("yyyy-MM-dd");
                            listRd.Add(date);
                            //P1//
                            var p1 = "";
                            if (isr.P1 != null)
                                p1 = isr.P1;
                            listRd.Add(p1);

                            listRdY.Add(listRd);
                        }
                    }

                    //Autre rdvv//
                    if (db.Crmcom_AutrRDVvProsp.Where(a => a.ID_Prosp == isCom.ID).Count() != 0)
                    {
                        foreach (var x in db.Crmcom_AutrRDVvProsp.Where(a => a.ID_Prosp == isCom.ID).ToList())
                        {
                            var listRdvv = new List<string>();
                            var isr = db.Crmcom_AutrRDVvProsp.Where(a => a.ID_AutrRDVv == x.ID_AutrRDVv).FirstOrDefault();
                            //date//0
                            var date = "";
                            if (isr.Date != null)
                                date = isr.Date.Value.ToString("yyyy-MM-dd");
                            listRdvv.Add(date);
                            //P1//
                            var p1 = "";
                            if (isr.P1 != null)
                                p1 = isr.P1;
                            listRdvv.Add(p1);
                            //Rmq//1
                            var rmq = "";
                            if (isr.Rmq != null)
                                rmq = isr.Rmq.Replace("\\n", "<br>");
                            listRdvv.Add(rmq);
                            //dateCRR//2
                            var dateCRR = "";
                            if (isr.Date_CRR != null)
                                dateCRR = isr.Date_CRR.Value.ToString("yyyy-MM-dd");
                            listRdvv.Add(dateCRR);
                            //P2//
                            var p2 = "";
                            if (isr.P2 != null)
                                p2 = isr.P2;
                            listRdvv.Add(p2);

                            listRdvvY.Add(listRdvv);
                        }
                    }

                    //Nego//
                    if (db.Crmcom_NegoProsp.Where(a => a.ID_Prosp == isCom.ID).Count() != 0)
                    {
                        foreach (var x in db.Crmcom_NegoProsp.Where(a => a.ID_Prosp == isCom.ID).ToList())
                        {
                            var listnego = new List<string>();
                            var isr = db.Crmcom_NegoProsp.Where(a => a.ID_Nego == x.ID_Nego).FirstOrDefault();
                            //date//0
                            var date = "";
                            if (isr.Date != null)
                                date = isr.Date.Value.ToString("yyyy-MM-dd");
                            listnego.Add(date);
                            //P1//
                            var p1 = "";
                            if (isr.P1 != null)
                                p1 = isr.P1;
                            listnego.Add(p1);
                            //Rmq//1
                            var rmq = "";
                            if (isr.Rmq != null)
                                rmq = isr.Rmq.Replace("\\n", "<br>");
                            listnego.Add(rmq);
                            //dateCRR//2
                            var dateCRR = "";
                            if (isr.Date_CRR != null)
                                dateCRR = isr.Date_CRR.Value.ToString("yyyy-MM-dd");
                            listnego.Add(dateCRR);
                            //P2//
                            var p2 = "";
                            if (isr.P2 != null)
                                p2 = isr.P2;
                            listnego.Add(p2);

                            listnegoY.Add(listnego);
                        }
                    }

                    //Devis GESCOM//
                    if (db.Crmcom_DevisGescomProsp.Where(a => a.ID_Prosp == isCom.ID).Count() != 0)
                    {
                        foreach (var x in db.Crmcom_DevisGescomProsp.Where(a => a.ID_Prosp == isCom.ID).ToList())
                        {
                            var listdg = new List<string>();
                            var isr = db.Crmcom_DevisGescomProsp.Where(a => a.ID_DateDevisGescom == x.ID_DateDevisGescom).FirstOrDefault();
                            //date//0
                            var date = "";
                            if (isr.Date != null)
                                date = isr.Date.Value.ToString("yyyy-MM-dd");
                            listdg.Add(date);
                            //P1//
                            var p1 = "";
                            if (isr.P1 != null)
                                p1 = isr.P1;
                            listdg.Add(p1);

                            listdgY.Add(listdg);
                        }
                    }

                    //BPA//
                    if (db.Crmcom_BPAProsp.Where(a => a.ID_Prosp == isCom.ID).Count() != 0)
                    {
                        foreach (var x in db.Crmcom_BPAProsp.Where(a => a.ID_Prosp == isCom.ID).ToList())
                        {
                            var listbpa = new List<string>();
                            var isr = db.Crmcom_BPAProsp.Where(a => a.ID_DateBPA == x.ID_DateBPA).FirstOrDefault();
                            //date//0
                            var date = "";
                            if (isr.Date != null)
                                date = isr.Date.Value.ToString("yyyy-MM-dd");
                            listbpa.Add(date);
                            //P1//
                            var p1 = "";
                            if (isr.P1 != null)
                                p1 = isr.P1;
                            listbpa.Add(p1);

                            listbpaY.Add(listbpa);
                        }
                    }

                    //BPA RElance//
                    if (db.Crmcom_BPARELProsp.Where(a => a.ID_Prosp == isCom.ID).Count() != 0)
                    {
                        foreach (var x in db.Crmcom_BPARELProsp.Where(a => a.ID_Prosp == isCom.ID).ToList())
                        {
                            var listbpar = new List<string>();
                            var isr = db.Crmcom_BPARELProsp.Where(a => a.ID_DateRelBPA == x.ID_DateRelBPA).FirstOrDefault();
                            //date//0
                            var date = "";
                            if (isr.Date != null)
                                date = isr.Date.Value.ToString("yyyy-MM-dd");
                            listbpar.Add(date);
                            //P1//
                            var p1 = "";
                            if (isr.P1 != null)
                                p1 = isr.P1;
                            listbpar.Add(p1);

                            listbparY.Add(listbpar);
                        }
                    }

                    //FA//
                    if (db.Crmcom_FAProsp.Where(a => a.ID_Prosp == isCom.ID).Count() != 0)
                    {
                        foreach (var x in db.Crmcom_FAProsp.Where(a => a.ID_Prosp == isCom.ID).ToList())
                        {
                            var listfa = new List<string>();
                            var isr = db.Crmcom_FAProsp.Where(a => a.ID_DateFA == x.ID_DateFA).FirstOrDefault();
                            //date//0
                            var date = "";
                            if (isr.Date != null)
                                date = isr.Date.Value.ToString("yyyy-MM-dd");
                            listfa.Add(date);
                            //P1//
                            var p1 = "";
                            if (isr.P1 != null)
                                p1 = isr.P1;
                            listfa.Add(p1);

                            listfaY.Add(listfa);
                        }
                    }

                    //Demo logiciel//
                    if (db.Crmcom_DemoProsp.Where(a => a.ID_Prosp == isCom.ID).Count() != 0)
                    {
                        foreach (var x in db.Crmcom_DemoProsp.Where(a => a.ID_Prosp == isCom.ID).ToList())
                        {
                            var listdem = new List<string>();
                            var isr = db.Crmcom_DemoProsp.Where(a => a.ID_DemoLogk == x.ID_DemoLogk).FirstOrDefault();
                            //date//0
                            var date = "";
                            if (isr.Date != null)
                                date = isr.Date.Value.ToString("yyyy-MM-dd");
                            listdem.Add(date);
                            //P1//
                            var p1 = "";
                            if (isr.P1 != null)
                                p1 = isr.P1;
                            listdem.Add(p1);
                            //Acteurs//1
                            var actt = "";
                            string[] separators = { "," };
                            if (!String.IsNullOrEmpty(isr.Agent))
                            {
                                string[] agentL = isr.Agent.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                foreach (var ag in agentL)
                                {
                                    var ida = int.Parse(ag.ToString());
                                    if (db.users.Where(a => a.User_UserId == ida).Count() != 0)
                                    {
                                        var us = db.users.Where(a => a.User_UserId == ida).FirstOrDefault();
                                        actt += us.User_LastName + " " + us.User_FirstName + ",";
                                    }
                                }
                            }
                            listdem.Add(actt);
                            //Rmq//2
                            var rmq = "";
                            if (isr.Rmq != null)
                                rmq = isr.Rmq.Replace("\\n", "<br>");
                            listdem.Add(rmq);
                            //dateCRR//3
                            var dateCRR = "";
                            if (isr.Date_CRR != null)
                                dateCRR = isr.Date_CRR.Value.ToString("yyyy-MM-dd");
                            listdem.Add(dateCRR);
                            //P1//
                            var p2 = "";
                            if (isr.P2 != null)
                                p2 = isr.P2;
                            listdem.Add(p2);

                            listdemY.Add(listdem);
                        }
                    }

                    //Demo Maquettées//
                    if (db.Crmcom_DemoMaqProsp.Where(a => a.ID_Prosp == isCom.ID).Count() != 0)
                    {
                        foreach (var x in db.Crmcom_DemoMaqProsp.Where(a => a.ID_Prosp == isCom.ID).ToList())
                        {
                            var listdemq = new List<string>();
                            var isr = db.Crmcom_DemoMaqProsp.Where(a => a.ID_DemoMaquette == x.ID_DemoMaquette).FirstOrDefault();
                            //date//0
                            var date = "";
                            if (isr.Date != null)
                                date = isr.Date.Value.ToString("yyyy-MM-dd");
                            listdemq.Add(date);
                            //P1//
                            var p1 = "";
                            if (isr.P1 != null)
                                p1 = isr.P1;
                            listdemq.Add(p1);
                            //Acteurs//1
                            var actt = "";
                            string[] separators = { "," };
                            if (!String.IsNullOrEmpty(isr.Agent))
                            {
                                string[] agentL = isr.Agent.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                foreach (var ag in agentL)
                                {
                                    var ida = int.Parse(ag.ToString());
                                    if (db.users.Where(a => a.User_UserId == ida).Count() != 0)
                                    {
                                        var us = db.users.Where(a => a.User_UserId == ida).FirstOrDefault();
                                        actt += us.User_LastName + " " + us.User_FirstName + ",";
                                    }
                                }
                            }
                            listdemq.Add(actt);
                            //Rmq//2
                            var rmq = "";
                            if (isr.Rmq != null)
                                rmq = isr.Rmq.Replace("\\n", "<br>");
                            listdemq.Add(rmq);
                            //dateCRR//3
                            var dateCRR = "";
                            if (isr.Date_CRR != null)
                                dateCRR = isr.Date_CRR.Value.ToString("yyyy-MM-dd");
                            listdemq.Add(dateCRR);
                            //P2//
                            var p2 = "";
                            if (isr.P2 != null)
                                p2 = isr.P2;
                            listdemq.Add(p2);

                            listdemqY.Add(listdemq);
                        }
                    }

                }

                var lis = listCommPros;
                ViewBag.listCommPros = lis;

                var lisA = listABY;
                ViewBag.listABY = lisA;

                var OP = OPY;
                ViewBag.OPY = OP;

                var lisR = listRdvY;
                ViewBag.listRdvY = lisR;

                var lisRd = listRdY;
                ViewBag.listRdY = lisRd;

                var lisRr = listRdvvY;
                ViewBag.listRdvvY = lisRr;

                var lisne = listnegoY;
                ViewBag.listnegoY = lisne;

                var lisdg = listdgY;
                ViewBag.listdgY = lisdg;

                var lisbpa = listbpaY;
                ViewBag.listbpaY = lisbpa;

                var lisbpar = listbparY;
                ViewBag.listbparY = lisbpar;

                var lisfa = listfaY;
                ViewBag.listfaY = lisfa;

                var lisdem = listdemY;
                ViewBag.listdemY = lisdem;

                var lisdemq = listdemqY;
                ViewBag.listdemqY = lisdemq;

                var collection = new Crmcom_CommercialeProsp();

                //Mode de commercialisation//
                List<string> modecomm = new List<string>();
                if (db.Custom_Captions.Where(a => a.Capt_Family == "oppo_modecom" && a.Capt_Deleted == null).Count() != 0)
                {
                    foreach (var x in db.Custom_Captions.Where(a => a.Capt_Family == "oppo_modecom" && a.Capt_Deleted == null).ToList())
                    {
                        modecomm.Add(x.Capt_FR);
                    }
                }
                collection.ModeCollection = modecomm;

                //Formation//
                List<string> Formation = new List<string>();
                if (db.Custom_Captions.Where(a => a.Capt_Family == "oppo_formation" && a.Capt_Deleted == null).Count() != 0)
                {
                    foreach (var x in db.Custom_Captions.Where(a => a.Capt_Family == "oppo_formation" && a.Capt_Deleted == null).ToList())
                    {
                        Formation.Add(x.Capt_FR);
                    }
                }
                collection.FormationCollection = Formation;

                //Monnetaire//
                List<string> budgetMon = new List<string>();
                if (db.Currency.Where(a => a.Curr_Deleted == null).Count() != 0)
                {
                    foreach (var x in db.Currency.Where(a => a.Curr_Deleted == null).ToList())
                    {
                        budgetMon.Add(x.Curr_Symbol);
                    }
                }
                collection.BudgetMonCollection = budgetMon;

                //MOFD//
                List<string> budgetMonMOF = new List<string>();
                if (db.Currency.Where(a => a.Curr_Deleted == null).Count() != 0)
                {
                    foreach (var x in db.Currency.Where(a => a.Curr_Deleted == null).ToList())
                    {
                        budgetMonMOF.Add(x.Curr_Symbol);
                    }
                }
                collection.MontantOffCollection = budgetMonMOF;

                //Prevision ou Réel//
                List<string> previR = new List<string>();
                previR.Add("P");
                previR.Add("R");
                collection.PrevisReel = previR;

                //P R Deadline//
                List<string> pD = new List<string>();
                pD.Add("P");
                pD.Add("R");
                collection.PDeadCollection = pD;

                //Produit//
                List<string> produits = new List<string>();
                if (db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted == null).Count() != 0)
                {
                    foreach (var x in db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted == null).ToList())
                    {
                        produits.Add(x.Capt_FR);
                    }
                }
                collection.ProduitsCollection = produits;

                //FMFP//
                List<string> fmfp = new List<string>();
                if (db.Custom_Captions.Where(a => a.Capt_Family == "oppo_fmfp" && a.Capt_Deleted == null).Count() != 0)
                {
                    foreach (var x in db.Custom_Captions.Where(a => a.Capt_Family == "oppo_fmfp" && a.Capt_Deleted == null).ToList())
                    {
                        fmfp.Add(x.Capt_FR);
                    }
                }
                collection.FMFPCollection = fmfp;

                //UserName//
                List<string> UserName = new List<string>();
                foreach (var x in db.users.Where(a => a.User_Deleted != 1 && a.User_Logon != null && a.User_Logon != "Admin" && a.user_deptsoft == "03").OrderBy(a => a.User_UserId).ToList())
                {
                    UserName.Add(x.User_LastName + " " + x.User_FirstName);
                }
                collection.UserCollection = UserName;

                //Type//
                List<string> types = new List<string>();
                if (db.Crmcom_TypeCProsp.Count() != 0)
                {
                    foreach (var x in db.Crmcom_TypeCProsp.ToList())
                    {
                        types.Add(x.Intitule);
                    }
                }
                collection.TypesCollection = types;

                //Etat//
                List<string> etats = new List<string>();
                if (db.Crmcom_EtatProsp.Count() != 0)
                {
                    foreach (var x in db.Crmcom_EtatProsp.ToList())
                    {
                        etats.Add(x.Intitule);
                    }
                }
                collection.EtatsCollection = etats;

                //Classifications//
                List<string> classifications = new List<string>();
                if (db.Crmcom_ClassifProsp.Count() != 0)
                {
                    foreach (var x in db.Crmcom_ClassifProsp.ToList())
                    {
                        classifications.Add(x.Intitule);
                    }
                }
                collection.ClassificationsCollection = classifications;

                //Sources//
                List<string> sources = new List<string>();
                if (db.Custom_Captions.Where(a => a.Capt_Family == "Oppo_Source" && a.Capt_Deleted == null).Count() != 0)
                {
                    foreach (var x in db.Custom_Captions.Where(a => a.Capt_Family == "Oppo_Source" && a.Capt_Deleted == null).ToList())
                    {
                        sources.Add(x.Capt_FR);
                    }
                }
                collection.SourcesCollection = sources;

                //Bailleur//
                List<string> bailleurs = new List<string>();
                if (db.Custom_Captions.Where(a => a.Capt_Family == "comp_bdf" && a.Capt_Deleted == null).Count() != 0)
                {
                    foreach (var x in db.Custom_Captions.Where(a => a.Capt_Family == "comp_bdf" && a.Capt_Deleted == null).ToList())
                    {
                        bailleurs.Add(x.Capt_FR);
                    }
                }
                collection.BailleursCollection = bailleurs;

                //Rebut//
                List<string> rebuts = new List<string>();
                if (db.Crmcom_RebutProsp.Count() != 0)
                {
                    foreach (var x in db.Crmcom_RebutProsp.ToList())
                    {
                        rebuts.Add(x.Intitule);
                    }
                }
                collection.RebutsCollection = rebuts;

                //Rebut Reasons//
                List<string> rebutsR = new List<string>();
                if (db.Crmcom_RebutReaProsp.Count() != 0)
                {
                    foreach (var x in db.Crmcom_RebutReaProsp.ToList())
                    {
                        rebutsR.Add(x.Intitule);
                    }
                }
                collection.RebutsRCollection = rebutsR;

                //Etape SOFTWELL//
                List<string> EtapS = new List<string>();
                if (db.Custom_Captions.Where(a => a.Capt_Family == "oppo_etapesoftwell" && a.Capt_Deleted == null).Count() != 0)
                {
                    foreach (var x in db.Custom_Captions.Where(a => a.Capt_Family == "oppo_etapesoftwell" && a.Capt_Deleted == null).OrderBy(a => a.Capt_Order).ToList())
                    {
                        EtapS.Add(x.Capt_FR);
                    }
                }
                collection.EtapSCollection = EtapS;

                //Client//
                List<List<string>> elem = new List<List<string>>();
                var elem2 = new List<string>();

                var clients = db.Company
                .Select(s => new
                {
                    Text = s.Comp_Name + " / " + db.ERPIntegrations.Where(a => a.ERPI_IntegrationID == s.comp_DefaultIntId).FirstOrDefault().ERPI_IntegrationName,
                    Value = s.Comp_CompanyId
                })
                .ToList();

                ViewBag.ClientsList = new SelectList(clients, "Value", "Text");

                //Campagne//
                List<string> Camp = new List<string>();
                if (db.Campaigns.Where(a => a.Camp_Deleted == null).Count() != 0)
                {
                    foreach (var x in db.Campaigns.Where(a => a.Camp_Deleted == null).ToList())
                    {
                        if (db.Waves.Where(a => a.Wave_CampaignId == x.Camp_CampaignId && a.Wave_Deleted == null).Count() != 0)
                        {
                            var iswave = db.Waves.Where(a => a.Wave_CampaignId == x.Camp_CampaignId && a.Wave_Deleted == null).FirstOrDefault().Wave_WaveId;

                            if (db.WaveItems.Where(a => a.WaIt_WaveId == iswave && a.WaIt_Deleted == null).Count() != 0)
                            {
                                foreach (var y in db.WaveItems.Where(a => a.WaIt_WaveId == iswave && a.WaIt_Deleted == null).ToList())
                                {
                                    Camp.Add(x.Camp_Name + "/" + y.WaIt_Name);
                                }
                            }
                        }
                    }
                }
                collection.CampagneCollection = Camp;

                collection.Ag = PopulateAg();
                collection.Ag2 = PopulateAg();

                collection.Op = PopulateAgOp();

                //collection.ProdMod2 = PopulateProdMod();

                return View(collection);
            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Edit(List<AnalY> analY, List<rdvY> rdvY, List<demolY> demolY, List<dexcelY> dexcelY, List<demoQY> demoQY,
            List<rdvvY> rdvvY, List<negoY> negoY, List<dgescY> dgescY, List<dbpaY> dbpaY, List<dbparY> dbparY, List<rfaY> rfaY, List<OptionsY> OptionsY,
            Crmcom_CommercialeProsp collection, idForeign idForeign/*, List<String> ProduiTs*/)
        {
            try
            {
                if (Session["UserId"] == null)
                    return Content("Erreur! Reconnectez-vous svp!");
                else if (!String.IsNullOrEmpty(collection.ReferenceOppo))
                {
                    if (db.Crmcom_CommercialeProsp.Where(a => a.ID == collection.ID).Count() != 0)
                    {
                        var isModf = db.Crmcom_CommercialeProsp.Where(a => a.ReferenceOppo == collection.ReferenceOppo).FirstOrDefault();

                        int idAgent = int.Parse(Session["UserId"].ToString());
                        isModf.User_UserId = idAgent;

                        //CAMPAGNE//
                        var CampName = "";
                        var WaitName = "";
                        if (!String.IsNullOrEmpty(collection.CCAMP))
                        {
                            foreach (var campW in collection.CCAMP.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                if (String.IsNullOrEmpty(CampName))
                                    CampName = campW;
                                else
                                    WaitName = campW;
                            }
                        }
                        if (!String.IsNullOrEmpty(CampName))
                        {
                            var isCamp = db.Campaigns.Where(a => a.Camp_Name == CampName).FirstOrDefault();
                            isModf.Camp_CampaignId = isCamp.Camp_CampaignId;
                        }

                        if (!String.IsNullOrEmpty(WaitName))
                        {
                            var isWAVE = db.Waves.Where(a => a.Wave_CampaignId == isModf.Camp_CampaignId).FirstOrDefault();
                            if (db.WaveItems.Where(a => a.WaIt_WaveId == isWAVE.Wave_WaveId && a.WaIt_Name == WaitName).Count() != 0)
                            {
                                var isWait = db.WaveItems.Where(a => a.WaIt_WaveId == isWAVE.Wave_WaveId && a.WaIt_Name == WaitName).FirstOrDefault();
                                isModf.WaIt_WaveItemId = isWait.WaIt_WaveItemId;
                            }
                        }

                        //TypeCLient//
                        if (!String.IsNullOrEmpty(idForeign.TypeClient))
                            isModf.ID_TypeClient = db.Crmcom_TypeCProsp.Where(a => a.Intitule == idForeign.TypeClient).FirstOrDefault().ID_TypeClient;
                        //Etat//
                        if (!String.IsNullOrEmpty(idForeign.Etat))
                            isModf.ID_Etat = db.Crmcom_EtatProsp.Where(a => a.Intitule == idForeign.Etat).FirstOrDefault().ID_Etat;
                        //Classification//
                        if (!String.IsNullOrEmpty(idForeign.Classification))
                            isModf.ID_Classification = db.Crmcom_ClassifProsp.Where(a => a.Intitule == idForeign.Classification).FirstOrDefault().ID_Classification;
                        //Source//
                        if (!String.IsNullOrEmpty(idForeign.Source))
                            isModf.ID_Source = db.Custom_Captions.Where(a => a.Capt_Family == "Oppo_Source" && a.Capt_Deleted == null && a.Capt_FR == idForeign.Source).FirstOrDefault().Capt_Code;
                        //Rebut//
                        if (!String.IsNullOrEmpty(idForeign.Rebut))
                            isModf.ID_Rebut = db.Crmcom_RebutProsp.Where(a => a.Intitule == idForeign.Rebut).FirstOrDefault().ID_Rebut;
                        //Rebut reason
                        if (!String.IsNullOrEmpty(idForeign.Rebut_Reason))
                            isModf.ID_Rebut_Reason = db.Crmcom_RebutReaProsp.Where(a => a.Intitule == idForeign.Rebut_Reason).FirstOrDefault().ID_Rebut_Reason;
                        //Produit//
                        var prodaka = "";
                        if (!String.IsNullOrEmpty(collection.Produit))
                        {
                            prodaka = db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted == null && a.Capt_FR == collection.Produit).FirstOrDefault().Capt_Code;
                            isModf.Produit = prodaka;
                        }
                        //FMFP//
                        if (!String.IsNullOrEmpty(collection.FMFP))
                        {
                            var fmfp = db.Custom_Captions.Where(a => a.Capt_Family == "oppo_fmfp" && a.Capt_Deleted == null && a.Capt_FR == collection.FMFP).FirstOrDefault().Capt_Code;
                            isModf.FMFP = fmfp;
                        }
                        /*var prodaka = "";
                        if (ProduiTs != null)
                        {
                            foreach (var x in ProduiTs.ToList())
                            {
                                prodaka += db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted == null && a.Capt_FR == x).FirstOrDefault().Capt_Code + ",";
                            }
                        }
                        isModf.Produit = prodaka;*/

                        //Etape SOFTWELL//
                        var etapS = "";
                        if (!String.IsNullOrEmpty(collection.EtapS))
                        {
                            etapS = db.Custom_Captions.Where(a => a.Capt_Family == "oppo_etapesoftwell" && a.Capt_Deleted == null && a.Capt_FR == collection.EtapS).FirstOrDefault().Capt_Code;
                            isModf.EtapS = etapS;
                        }

                        //Mode de commercialisation//
                        var ModeComm = "";
                        if (!String.IsNullOrEmpty(collection.ModeComm))
                        {
                            ModeComm = db.Custom_Captions.Where(a => a.Capt_Family == "oppo_modecom" && a.Capt_Deleted == null && a.Capt_FR == collection.ModeComm).FirstOrDefault().Capt_Code;
                            isModf.ModeComm = ModeComm;
                        }
                        //Formation//
                        var Formation = "";
                        if (!String.IsNullOrEmpty(collection.Formation))
                        {
                            Formation = db.Custom_Captions.Where(a => a.Capt_Family == "oppo_formation" && a.Capt_Deleted == null && a.Capt_FR == collection.Formation).FirstOrDefault().Capt_Code;
                            isModf.Formation = Formation;
                        }

                        ///Budget Devise//
                        var isBdg = "";
                        if (!String.IsNullOrEmpty(collection.BudgetDevise))
                        {
                            if (db.Currency.Where(a => a.Curr_Symbol == collection.BudgetDevise && a.Curr_Deleted == null).Count() != 0)
                            {
                                isBdg = db.Currency.Where(a => a.Curr_Symbol == collection.BudgetDevise && a.Curr_Deleted == null).FirstOrDefault().Curr_Symbol;
                                isModf.BudgetDevise = isBdg;
                            }
                        }

                        ///Budget Devise MOF//
                        var isBdgMOF = "";
                        if (!String.IsNullOrEmpty(collection.MontantOffreDevise))
                        {
                            if (db.Currency.Where(a => a.Curr_Symbol == collection.MontantOffreDevise && a.Curr_Deleted == null).Count() != 0)
                            {
                                isBdgMOF = db.Currency.Where(a => a.Curr_Symbol == collection.MontantOffreDevise && a.Curr_Deleted == null).FirstOrDefault().Curr_Symbol;
                                isModf.MontantOffreDevise = isBdgMOF;
                            }
                        }

                        //Apportteur affaire//
                        if (!String.IsNullOrEmpty(collection.ApporteurAffaire))
                        {
                            isModf.ApporteurAffaire = collection.ApporteurAffaire;
                        }

                        //Option//
                        //Analyse des besoins//
                        var Option = "";
                        if (OptionsY != null)
                        {
                            foreach (var x in OptionsY.ToList())
                            {
                                if (db.Custom_Captions.Where(a => a.Capt_Family == "oppo_option" && a.Capt_Deleted == null && a.Capt_FR == x.Options).Count() != 0)
                                {
                                    var isCode = db.Custom_Captions.Where(a => a.Capt_Family == "oppo_option" && a.Capt_Deleted == null && a.Capt_FR == x.Options).FirstOrDefault().Capt_Code;

                                    Option += isCode + ",";
                                }
                            }
                            isModf.Options = Option;
                        }
                        //Option//
                        //Analyse des besoins//

                        //Bailleur de fond//
                        if (!String.IsNullOrEmpty(collection.Bailleur_Fond))
                            isModf.Bailleur_Fond = db.Custom_Captions.Where(a => a.Capt_Family == "comp_bdf" && a.Capt_Deleted == null & a.Capt_FR == collection.Bailleur_Fond).FirstOrDefault().Capt_Code;

                        DateTime now = DateTime.Now;
                        //collection.Date_Creation = now;
                        isModf.Date_Update = now;

                        if (collection.Comp_CompanyId != null)
                            isModf.Comp_CompanyId = collection.Comp_CompanyId;

                        isModf.Date_Debut = collection.Date_Debut;
                        isModf.Nombre_User = collection.Nombre_User;
                        isModf.Nombre_Salarie = collection.Nombre_Salarie;
                        isModf.Existant = collection.Existant;
                        isModf.Analyse_Besoin = collection.Analyse_Besoin;
                        isModf.Budget = collection.Budget;
                        isModf.DeadLine = collection.DeadLine;
                        isModf.Interlocuteur = collection.Interlocuteur;
                        isModf.Decisionnaire = collection.Decisionnaire;
                        isModf.PA = collection.PA;
                        isModf.Date_ProposContrat = collection.Date_ProposContrat;
                        if (collection.P1ProposContrat == "R")
                            isModf.P1ProposContrat = "R";
                        else
                            isModf.P1ProposContrat = "P";
                        isModf.Date_ProgForm = collection.Date_ProgForm;
                        if (collection.P1ProgForm == "R")
                            isModf.P1ProgForm = "R";
                        else
                            isModf.P1ProgForm = "P";
                        isModf.Date_PrecoTech = collection.Date_PrecoTech;
                        if (collection.P1PrecoTech == "R")
                            isModf.P1PrecoTech = "R";
                        else
                            isModf.P1PrecoTech = "P";
                        isModf.Date_ProForma = collection.Date_ProForma;
                        if (collection.P1ProForma == "R")
                            isModf.P1ProForma = "R";
                        else
                            isModf.P1ProForma = "P";
                        isModf.Remise = collection.Remise;
                        isModf.Modalite_Paiement = collection.Modalite_Paiement;
                        isModf.Date_ReceptionBC = collection.Date_ReceptionBC;
                        if (collection.P1ReceptionBC == "R")
                            isModf.P1ReceptionBC = "R";
                        else
                            isModf.P1ReceptionBC = "P";
                        isModf.Duree_Traitement = collection.Duree_Traitement;
                        isModf.Date_ReceptionAcompte = collection.Date_ReceptionAcompte;
                        if (collection.P1ReceptionAcompte == "R")
                            isModf.P1ReceptionAcompte = "R";
                        else
                            isModf.P1ReceptionAcompte = "P";
                        isModf.Date_BL = collection.Date_BL;
                        if (collection.P1BL == "R")
                            isModf.P1BL = "R";
                        else
                            isModf.P1BL = "P";
                        isModf.Date_Contrat = collection.Date_Contrat;
                        if (collection.P1Contrat == "R")
                            isModf.P1Contrat = "R";
                        else
                            isModf.P1Contrat = "P";
                        isModf.Date_CRL = collection.Date_CRL;
                        if (collection.P1CRL == "R")
                            isModf.P1CRL = "R";
                        else
                            isModf.P1CRL = "P";
                        isModf.Date_RetourContrat = collection.Date_RetourContrat;
                        if (collection.P1RetourContrat == "R")
                            isModf.P1RetourContrat = "R";
                        else
                            isModf.P1RetourContrat = "P";
                        isModf.ReferenceOppo = collection.ReferenceOppo;
                        //isModf.FMFP = collection.FMFP;
                        if (collection.PDead == "R")
                            isModf.PDead = "R";
                        else
                            isModf.PDead = "P";

                        isModf.Date_Fin = collection.Date_Fin;

                        db.SaveChanges();

                        ModelHELPD ft = new ModelHELPD();
                        var comer = ft.Crmcom_CommercialeProsp.Where(a => a.ReferenceOppo == collection.ReferenceOppo).FirstOrDefault().ID;

                        //Analyse des besoins//
                        if (ft.Crmcom_AnalBesoinProsp.Where(a => a.ID_Prosp == comer).Count() != 0)
                        {
                            foreach (var y in ft.Crmcom_AnalBesoinProsp.Where(a => a.ID_Prosp == comer).ToList())
                            {
                                var isdelet = ft.Crmcom_AnalBesoinProsp.Where(a => a.ID_AnalBesoin == y.ID_AnalBesoin).FirstOrDefault();
                                ft.Crmcom_AnalBesoinProsp.Remove(isdelet);
                                ft.SaveChanges();
                            }
                        }
                        if (analY != null)
                        {
                            foreach (var x in analY.ToList())
                            {
                                var rmqq = x.Rmq;
                                var ceci = rmqq.Substring(rmqq.Length - 1, 1);
                                if (rmqq != null)
                                {
                                    while (rmqq.Substring(rmqq.Length - 1, 1) == "\n")
                                    {
                                        rmqq = rmqq.Substring(0, rmqq.Length - 1);
                                    }
                                }


                                var ins = new Crmcom_AnalBesoinProsp
                                {
                                    ID_Prosp = comer,
                                    Date = DateTime.Parse(x.Date),
                                    Rmq = rmqq,
                                    Date_CRR = DateTime.Parse(x.DateCRR),
                                    P1 = x.P1,
                                    P2 = x.P2
                                };

                                ft.Crmcom_AnalBesoinProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }
                        //Autr rdv//
                        if (ft.Crmcom_AutrRDVProsp.Where(a => a.ID_Prosp == comer).Count() != 0)
                        {
                            foreach (var y in ft.Crmcom_AutrRDVProsp.Where(a => a.ID_Prosp == comer).ToList())
                            {
                                var isdelet = ft.Crmcom_AutrRDVProsp.Where(a => a.ID_AutrRDV == y.ID_AutrRDV).FirstOrDefault();
                                ft.Crmcom_AutrRDVProsp.Remove(isdelet);
                                ft.SaveChanges();
                            }
                        }
                        if (rdvY != null)
                        {
                            foreach (var x in rdvY.ToList())
                            {
                                var rmqq = x.Rmq;
                                var ceci = rmqq.Substring(rmqq.Length - 1, 1);
                                if (rmqq != null)
                                {
                                    while (rmqq.Substring(rmqq.Length - 1, 1) == "\n")
                                    {
                                        rmqq = rmqq.Substring(0, rmqq.Length - 1);
                                    }
                                }


                                var ins = new Crmcom_AutrRDVProsp
                                {
                                    ID_Prosp = comer,
                                    Date = DateTime.Parse(x.Date),
                                    Rmq = rmqq,
                                    Date_CRR = DateTime.Parse(x.DateCRR),
                                    P1 = x.P1,
                                    P2 = x.P2
                                };

                                ft.Crmcom_AutrRDVProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }
                        //Remis devis excel//
                        if (ft.Crmcom_RemiDevisProsp.Where(a => a.ID_Prosp == comer).Count() != 0)
                        {
                            foreach (var y in ft.Crmcom_RemiDevisProsp.Where(a => a.ID_Prosp == comer).ToList())
                            {
                                var isdelet = ft.Crmcom_RemiDevisProsp.Where(a => a.ID_DateRemisDevis == y.ID_DateRemisDevis).FirstOrDefault();
                                ft.Crmcom_RemiDevisProsp.Remove(isdelet);
                                ft.SaveChanges();
                            }
                        }
                        if (dexcelY != null)
                        {
                            foreach (var x in dexcelY.ToList())
                            {
                                var ins = new Crmcom_RemiDevisProsp
                                {
                                    ID_Prosp = comer,
                                    Date = DateTime.Parse(x.Date),
                                    P1 = x.P1
                                };

                                ft.Crmcom_RemiDevisProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }
                        //Autr rdvv//
                        if (ft.Crmcom_AutrRDVvProsp.Where(a => a.ID_Prosp == comer).Count() != 0)
                        {
                            foreach (var y in ft.Crmcom_AutrRDVvProsp.Where(a => a.ID_Prosp == comer).ToList())
                            {
                                var isdelet = ft.Crmcom_AutrRDVvProsp.Where(a => a.ID_AutrRDVv == y.ID_AutrRDVv).FirstOrDefault();
                                ft.Crmcom_AutrRDVvProsp.Remove(isdelet);
                                ft.SaveChanges();
                            }
                        }
                        if (rdvvY != null)
                        {
                            foreach (var x in rdvvY.ToList())
                            {
                                var rmqq = x.Rmq;
                                var ceci = rmqq.Substring(rmqq.Length - 1, 1);
                                if (rmqq != null)
                                {
                                    while (rmqq.Substring(rmqq.Length - 1, 1) == "\n")
                                    {
                                        rmqq = rmqq.Substring(0, rmqq.Length - 1);
                                    }
                                }

                                var ins = new Crmcom_AutrRDVvProsp
                                {
                                    ID_Prosp = comer,
                                    Date = DateTime.Parse(x.Date),
                                    Rmq = rmqq,
                                    Date_CRR = DateTime.Parse(x.DateCRR),
                                    P1 = x.P1,
                                    P2 = x.P2
                                };

                                ft.Crmcom_AutrRDVvProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }
                        //NEGO//
                        if (ft.Crmcom_NegoProsp.Where(a => a.ID_Prosp == comer).Count() != 0)
                        {
                            foreach (var y in ft.Crmcom_NegoProsp.Where(a => a.ID_Prosp == comer).ToList())
                            {
                                var isdelet = ft.Crmcom_NegoProsp.Where(a => a.ID_Nego == y.ID_Nego).FirstOrDefault();
                                ft.Crmcom_NegoProsp.Remove(isdelet);
                                ft.SaveChanges();
                            }
                        }
                        if (negoY != null)
                        {
                            foreach (var x in negoY.ToList())
                            {
                                var rmqq = x.Rmq;
                                var ceci = rmqq.Substring(rmqq.Length - 1, 1);
                                if (rmqq != null)
                                {
                                    while (rmqq.Substring(rmqq.Length - 1, 1) == "\n")
                                    {
                                        rmqq = rmqq.Substring(0, rmqq.Length - 1);
                                    }
                                }


                                var ins = new Crmcom_NegoProsp
                                {
                                    ID_Prosp = comer,
                                    Date = DateTime.Parse(x.Date),
                                    Rmq = rmqq,
                                    Date_CRR = DateTime.Parse(x.DateCRR),
                                    P1 = x.P1,
                                    P2 = x.P2
                                };

                                ft.Crmcom_NegoProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }
                        //Envoi devis GESCOM//
                        if (ft.Crmcom_DevisGescomProsp.Where(a => a.ID_Prosp == comer).Count() != 0)
                        {
                            foreach (var y in ft.Crmcom_DevisGescomProsp.Where(a => a.ID_Prosp == comer).ToList())
                            {
                                var isdelet = ft.Crmcom_DevisGescomProsp.Where(a => a.ID_DateDevisGescom == y.ID_DateDevisGescom).FirstOrDefault();
                                ft.Crmcom_DevisGescomProsp.Remove(isdelet);
                                ft.SaveChanges();
                            }
                        }
                        if (dgescY != null)
                        {
                            foreach (var x in dgescY.ToList())
                            {
                                var ins = new Crmcom_DevisGescomProsp
                                {
                                    ID_Prosp = comer,
                                    Date = DateTime.Parse(x.Date),
                                    P1 = x.P1
                                };

                                ft.Crmcom_DevisGescomProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }
                        //Envoi BPA SAGE//
                        if (ft.Crmcom_BPAProsp.Where(a => a.ID_Prosp == comer).Count() != 0)
                        {
                            foreach (var y in ft.Crmcom_BPAProsp.Where(a => a.ID_Prosp == comer).ToList())
                            {
                                var isdelet = ft.Crmcom_BPAProsp.Where(a => a.ID_DateBPA == y.ID_DateBPA).FirstOrDefault();
                                ft.Crmcom_BPAProsp.Remove(isdelet);
                                ft.SaveChanges();
                            }
                        }
                        if (dbpaY != null)
                        {
                            foreach (var x in dbpaY.ToList())
                            {
                                var ins = new Crmcom_BPAProsp
                                {
                                    ID_Prosp = comer,
                                    Date = DateTime.Parse(x.Date),
                                    P1 = x.P1
                                };

                                ft.Crmcom_BPAProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }
                        //Envoi BPA SAGE//
                        if (ft.Crmcom_BPARELProsp.Where(a => a.ID_Prosp == comer).Count() != 0)
                        {
                            foreach (var y in ft.Crmcom_BPARELProsp.Where(a => a.ID_Prosp == comer).ToList())
                            {
                                var isdelet = ft.Crmcom_BPARELProsp.Where(a => a.ID_DateRelBPA == y.ID_DateRelBPA).FirstOrDefault();
                                ft.Crmcom_BPARELProsp.Remove(isdelet);
                                ft.SaveChanges();
                            }
                        }
                        if (dbparY != null)
                        {
                            foreach (var x in dbparY.ToList())
                            {
                                var ins = new Crmcom_BPARELProsp
                                {
                                    ID_Prosp = comer,
                                    Date = DateTime.Parse(x.Date),
                                    P1 = x.P1
                                };

                                ft.Crmcom_BPARELProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }
                        //Remise FA//
                        if (ft.Crmcom_FAProsp.Where(a => a.ID_Prosp == comer).Count() != 0)
                        {
                            foreach (var y in ft.Crmcom_FAProsp.Where(a => a.ID_Prosp == comer).ToList())
                            {
                                var isdelet = ft.Crmcom_FAProsp.Where(a => a.ID_DateFA == y.ID_DateFA).FirstOrDefault();
                                ft.Crmcom_FAProsp.Remove(isdelet);
                                ft.SaveChanges();
                            }
                        }
                        if (rfaY != null)
                        {
                            foreach (var x in rfaY.ToList())
                            {
                                var ins = new Crmcom_FAProsp
                                {
                                    ID_Prosp = comer,
                                    Date = DateTime.Parse(x.Date),
                                    P1 = x.P1
                                };

                                ft.Crmcom_FAProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }
                        //Demo logiciel//
                        if (ft.Crmcom_DemoProsp.Where(a => a.ID_Prosp == comer).Count() != 0)
                        {
                            foreach (var y in ft.Crmcom_DemoProsp.Where(a => a.ID_Prosp == comer).ToList())
                            {
                                var isdelet = ft.Crmcom_DemoProsp.Where(a => a.ID_DemoLogk == y.ID_DemoLogk).FirstOrDefault();
                                ft.Crmcom_DemoProsp.Remove(isdelet);
                                ft.SaveChanges();
                            }
                        }
                        if (demolY != null)
                        {
                            foreach (var x in demolY.ToList())
                            {
                                var actt = "";
                                string[] separators = { "," };
                                if (!String.IsNullOrEmpty(x.Acteur))
                                {
                                    string[] agentL = x.Acteur.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                    foreach (var ag in agentL)
                                    {
                                        var nomPrenom = ag;
                                        foreach (var y in ft.users.ToList())
                                        {
                                            if ((y.User_LastName + " " + y.User_FirstName == nomPrenom) && y.User_Deleted != 1)
                                                actt += y.User_UserId + ",";
                                        }
                                    }
                                }

                                var rmqq = x.Rmq;

                                var ceci = "";


                                if (rmqq != null)
                                {
                                    ceci = rmqq.Substring(rmqq.Length - 1, 1);

                                    while (rmqq.Substring(rmqq.Length - 1, 1) == "\n")
                                    {
                                        rmqq = rmqq.Substring(0, rmqq.Length - 1);
                                    }
                                }


                                var ins = new Crmcom_DemoProsp
                                {
                                    ID_Prosp = comer,
                                    Date = DateTime.Parse(x.Date),
                                    Agent = actt,
                                    Rmq = rmqq,
                                    Date_CRR = DateTime.Parse(x.DateCRR),
                                    P1 = x.P1,
                                    P2 = x.P2
                                };

                                ft.Crmcom_DemoProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }
                        //Demo maquettées//
                        if (ft.Crmcom_DemoMaqProsp.Where(a => a.ID_Prosp == comer).Count() != 0)
                        {
                            foreach (var y in ft.Crmcom_DemoMaqProsp.Where(a => a.ID_Prosp == comer).ToList())
                            {
                                var isdelet = ft.Crmcom_DemoMaqProsp.Where(a => a.ID_DemoMaquette == y.ID_DemoMaquette).FirstOrDefault();
                                ft.Crmcom_DemoMaqProsp.Remove(isdelet);
                                ft.SaveChanges();
                            }
                        }
                        if (demoQY != null)
                        {
                            foreach (var x in demoQY.ToList())
                            {
                                var actt = "";
                                string[] separators = { "," };
                                if (!String.IsNullOrEmpty(x.Acteur))
                                {
                                    string[] agentL = x.Acteur.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                    foreach (var ag in agentL)
                                    {
                                        var nomPrenom = ag;
                                        foreach (var y in ft.users.ToList())
                                        {
                                            if ((y.User_LastName + " " + y.User_FirstName == nomPrenom) && y.User_Deleted != 1)
                                                actt += y.User_UserId + ",";
                                        }
                                    }
                                }

                                var rmqq = x.Rmq;
                                var ceci = "";
                                if (rmqq != null)
                                {
                                    ceci = rmqq.Substring(rmqq.Length - 1, 1);
                                    while (rmqq.Substring(rmqq.Length - 1, 1) == "\n")
                                    {
                                        rmqq = rmqq.Substring(0, rmqq.Length - 1);
                                    }
                                }


                                var ins = new Crmcom_DemoMaqProsp
                                {
                                    ID_Prosp = comer,
                                    Date = DateTime.Parse(x.Date),
                                    Agent = actt,
                                    Rmq = rmqq,
                                    Date_CRR = DateTime.Parse(x.DateCRR),
                                    P1 = x.P1,
                                    P2 = x.P2
                                };

                                ft.Crmcom_DemoMaqProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }

                        return Content("Modification avec succès");
                    }
                    else
                        return Content("Erreur! La référence doit être unique!");
                }
                else
                    return Content("Erreur! La référence est obligatoire!");
            }
            catch (Exception)
            {
                return Content("Erreur!");
            }
        }

        public ActionResult Dupli(int id)
        {
            try
            {
                List<List<string>> listCommPros = new List<List<string>>();
                var listPro = new List<string>();

                if (db.Crmcom_CommercialeProsp.Where(a => a.ID == id).Count() != 0)
                {
                    var isCom = db.Crmcom_CommercialeProsp.Where(a => a.ID == id).FirstOrDefault();

                    //ID//0
                    listPro.Add(isCom.ID.ToString());
                    //Reference//1
                    listPro.Add(isCom.ReferenceOppo);

                    //Client//2
                    var client = "";
                    if (isCom.Comp_CompanyId != null)
                    {
                        if (db.Company.Where(a => a.Comp_CompanyId == isCom.Comp_CompanyId && a.Comp_Deleted != 1).Count() != 0)
                        {
                            var companyFirst = db.Company.Where(a => a.Comp_CompanyId == isCom.Comp_CompanyId && a.Comp_Deleted != 1).FirstOrDefault();

                            if (companyFirst.Comp_Name != null)
                            {
                                client = companyFirst.Comp_Name;
                            }
                            else if (companyFirst.comp_raison_sociale_af_ay_iltx != null)
                            {
                                client = companyFirst.comp_raison_sociale_af_ay_iltx;
                            }
                        }
                    }
                    listPro.Add(client);

                    //Produit//3
                    var prod = "";
                    if (isCom.Produit != null)
                    {
                        if (db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted == null && a.Capt_Code == isCom.Produit).Count() != 0)
                        {
                            prod = db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted == null && a.Capt_Code == isCom.Produit).FirstOrDefault().Capt_FR;
                        }
                    }
                    listPro.Add(prod);

                    listCommPros.Add(listPro);
                }

                var lis = listCommPros;
                ViewBag.listCommPros = lis;

                var collection = new Crmcom_CommercialeProsp();

                //Produit//
                List<string> produits = new List<string>();
                if (db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted == null).Count() != 0)
                {
                    foreach (var x in db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted == null).ToList())
                    {
                        produits.Add(x.Capt_FR);
                    }
                }
                collection.ProduitsCollection = produits;

                collection.Ag = PopulateAg();
                collection.Ag2 = PopulateAg();

                collection.Op = PopulateAgOp();

                return View(collection);
            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpPost]
        public JsonResult DupliD(Crmcom_CommercialeProsp collection)
        {
            try
            {
                if (Session["UserId"] == null)
                    return Json("Erreur! Reconnectez-vous svp!", JsonRequestBehavior.AllowGet);
                else if (!String.IsNullOrEmpty(collection.ReferenceOppo))
                {
                    if (db.Crmcom_CommercialeProsp.Where(a => a.ReferenceOppo == collection.ReferenceOppo).Count() == 0)
                    {
                        var isOp = db.Crmcom_CommercialeProsp.Where(a => a.ID == collection.ID).FirstOrDefault();

                        int idAgent = int.Parse(Session["UserId"].ToString());
                        collection.User_UserId = idAgent;

                        //TypeCLient//
                        collection.ID_TypeClient = isOp.ID_TypeClient;
                        //Etat//
                        collection.ID_Etat = isOp.ID_Etat;
                        //Classification//
                        collection.ID_Classification = isOp.ID_Classification;
                        //Source//
                        collection.ID_Source = isOp.ID_Source;
                        //Rebut//
                        collection.ID_Rebut = isOp.ID_Rebut;
                        //Rebut reason
                        collection.ID_Rebut_Reason = isOp.ID_Rebut_Reason;
                        //Produit//
                        var prodaka = "";
                        if (!String.IsNullOrEmpty(collection.Produit))
                        {
                            prodaka = db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted == null && a.Capt_FR == collection.Produit).FirstOrDefault().Capt_Code;
                        }
                        collection.Produit = prodaka;
                        //FMFP//
                        collection.FMFP = isOp.FMFP;
                        //Etape SOFTWELL//
                        collection.EtapS = isOp.EtapS;
                        //Mode de commercialisation//
                        collection.ModeComm = isOp.ModeComm;
                        //Apporteur affraire//
                        collection.ApporteurAffaire = isOp.ApporteurAffaire;
                        //Formation//
                        collection.Formation = isOp.Formation;
                        ///Budget Devise//
                        collection.BudgetDevise = isOp.BudgetDevise;
                        ///Budget Devise OF//
                        collection.MontantOffreDevise = isOp.MontantOffreDevise;
                        //Option//
                        collection.Options = isOp.Options;
                        //Bailleur de fond//
                        collection.Bailleur_Fond = isOp.Bailleur_Fond;
                        //CAMPAGNE//
                        collection.Camp_CampaignId = isOp.Camp_CampaignId;
                        collection.WaIt_WaveItemId = isOp.WaIt_WaveItemId;
                        //Client//
                        collection.Comp_CompanyId = isOp.Comp_CompanyId;
                        //Date debut//
                        collection.Date_Debut = isOp.Date_Debut;
                        //Nbr user//
                        collection.Nombre_User = isOp.Nombre_User;
                        //Nbr salarie//
                        collection.Nombre_Salarie = isOp.Nombre_Salarie;
                        //Existant//
                        collection.Existant = isOp.Existant;
                        //Analyse besoin//
                        collection.Analyse_Besoin = isOp.Analyse_Besoin;
                        //Budget//
                        collection.Budget = isOp.Budget;
                        //MOF//
                        collection.MontantOffre = isOp.MontantOffre;
                        //Budget//
                        collection.MPF = isOp.MPF;
                        //Dead line//
                        collection.DeadLine = isOp.DeadLine;
                        //Interloc//
                        collection.Interlocuteur = isOp.Interlocuteur;
                        //Decisionnaire//
                        collection.Decisionnaire = isOp.Decisionnaire;
                        //PA//
                        collection.PA = isOp.PA;
                        //Date propos contrat//
                        collection.Date_ProposContrat = isOp.Date_ProposContrat;
                        //Date prog form//
                        collection.Date_ProgForm = isOp.Date_ProgForm;
                        //Date preco tech//
                        collection.Date_PrecoTech = isOp.Date_PrecoTech;
                        //Date proforma//
                        collection.Date_ProForma = isOp.Date_ProForma;
                        //Remise//
                        collection.Remise = isOp.Remise;
                        //Modalite Paiement//
                        collection.Modalite_Paiement = isOp.Modalite_Paiement;
                        //Date reception BC//
                        collection.Date_ReceptionBC = isOp.Date_ReceptionBC;
                        //Duree traitement//
                        collection.Duree_Traitement = isOp.Duree_Traitement;
                        //Reception accompte//
                        collection.Date_ReceptionAcompte = isOp.Date_ReceptionAcompte;
                        //Date BL//
                        collection.Date_BL = isOp.Date_BL;
                        //Date COntrat//
                        collection.Date_Contrat = isOp.Date_Contrat;
                        //Date CRL//
                        collection.Date_CRL = isOp.Date_CRL;
                        //Date retour contrat//
                        collection.Date_RetourContrat = isOp.Date_RetourContrat;

                        collection.P1ProposContrat = isOp.P1ProposContrat;
                        collection.P1ProgForm = isOp.P1ProgForm;
                        collection.P1PrecoTech = isOp.P1PrecoTech;
                        collection.P1ReceptionBC = isOp.P1ReceptionBC;
                        collection.P1ReceptionAcompte = isOp.P1ReceptionAcompte;
                        collection.P1BL = isOp.P1BL;
                        collection.P1Contrat = isOp.P1Contrat;
                        collection.P1CRL = isOp.P1CRL;
                        collection.P1RetourContrat = isOp.P1RetourContrat;
                        collection.P1ProForma = isOp.P1ProForma;
                        collection.PDead = isOp.PDead;

                        collection.Date_Fin = isOp.Date_Fin;

                        DateTime now = DateTime.Now;

                        collection.Date_Creation = now;
                        collection.Date_Update = now;

                        db.Crmcom_CommercialeProsp.Add(collection);
                        db.SaveChanges();

                        ModelHELPD ft = new ModelHELPD();
                        var comer = ft.Crmcom_CommercialeProsp.Where(a => a.ReferenceOppo == collection.ReferenceOppo).FirstOrDefault().ID;

                        //Analyse des besoins//
                        if (ft.Crmcom_AnalBesoinProsp.Where(a => a.ID_Prosp == isOp.ID).Count() != 0)
                        {
                            foreach (var x in ft.Crmcom_AnalBesoinProsp.Where(a => a.ID_Prosp == isOp.ID).ToList())
                            {
                                var ins = new Crmcom_AnalBesoinProsp
                                {
                                    ID_Prosp = comer,
                                    Date = x.Date,
                                    Rmq = x.Rmq,
                                    Date_CRR = x.Date_CRR,
                                    P1 = x.P1,
                                    P2 = x.P2
                                };

                                ft.Crmcom_AnalBesoinProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }
                        //Autr rdv//
                        if (ft.Crmcom_AutrRDVProsp.Where(a => a.ID_Prosp == isOp.ID).Count() != 0)
                        {
                            foreach (var x in ft.Crmcom_AutrRDVProsp.Where(a => a.ID_Prosp == isOp.ID).ToList())
                            {
                                var ins = new Crmcom_AutrRDVProsp
                                {
                                    ID_Prosp = comer,
                                    Date = x.Date,
                                    Rmq = x.Rmq,
                                    Date_CRR = x.Date_CRR,
                                    P1 = x.P1,
                                    P2 = x.P2
                                };

                                ft.Crmcom_AutrRDVProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }
                        //Remis devis excel//
                        if (ft.Crmcom_RemiDevisProsp.Where(a => a.ID_Prosp == isOp.ID).Count() != 0)
                        {
                            foreach (var x in ft.Crmcom_RemiDevisProsp.Where(a => a.ID_Prosp == isOp.ID).ToList())
                            {
                                var ins = new Crmcom_RemiDevisProsp
                                {
                                    ID_Prosp = comer,
                                    Date = x.Date,
                                    P1 = x.P1
                                };

                                ft.Crmcom_RemiDevisProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }
                        //Autr rdvv//
                        if (ft.Crmcom_AutrRDVvProsp.Where(a => a.ID_Prosp == isOp.ID).Count() != 0)
                        {
                            foreach (var x in ft.Crmcom_AutrRDVvProsp.Where(a => a.ID_Prosp == isOp.ID).ToList())
                            {
                                var ins = new Crmcom_AutrRDVvProsp
                                {
                                    ID_Prosp = comer,
                                    Date = x.Date,
                                    Rmq = x.Rmq,
                                    Date_CRR = x.Date_CRR,
                                    P1 = x.P1,
                                    P2 = x.P2
                                };

                                ft.Crmcom_AutrRDVvProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }
                        //NEGO//
                        if (ft.Crmcom_NegoProsp.Where(a => a.ID_Prosp == isOp.ID).Count() != 0)
                        {
                            foreach (var x in ft.Crmcom_NegoProsp.Where(a => a.ID_Prosp == isOp.ID).ToList())
                            {
                                var ins = new Crmcom_NegoProsp
                                {
                                    ID_Prosp = comer,
                                    Date = x.Date,
                                    Rmq = x.Rmq,
                                    Date_CRR = x.Date_CRR,
                                    P1 = x.P1,
                                    P2 = x.P2
                                };

                                ft.Crmcom_NegoProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }
                        //Envoi devis GESCOM//
                        if (ft.Crmcom_DevisGescomProsp.Where(a => a.ID_Prosp == isOp.ID).Count() != 0)
                        {
                            foreach (var x in ft.Crmcom_DevisGescomProsp.Where(a => a.ID_Prosp == isOp.ID).ToList())
                            {
                                var ins = new Crmcom_DevisGescomProsp
                                {
                                    ID_Prosp = comer,
                                    Date = x.Date,
                                    P1 = x.P1
                                };

                                ft.Crmcom_DevisGescomProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }
                        //Envoi BPA SAGE//
                        if (ft.Crmcom_BPAProsp.Where(a => a.ID_Prosp == isOp.ID).Count() != 0)
                        {
                            foreach (var x in ft.Crmcom_BPAProsp.Where(a => a.ID_Prosp == isOp.ID).ToList())
                            {
                                var ins = new Crmcom_BPAProsp
                                {
                                    ID_Prosp = comer,
                                    Date = x.Date,
                                    P1 = x.P1
                                };

                                ft.Crmcom_BPAProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }
                        //Envoi BPA SAGE//
                        if (ft.Crmcom_BPARELProsp.Where(a => a.ID_Prosp == isOp.ID).Count() != 0)
                        {
                            foreach (var x in ft.Crmcom_BPARELProsp.Where(a => a.ID_Prosp == isOp.ID).ToList())
                            {
                                var ins = new Crmcom_BPARELProsp
                                {
                                    ID_Prosp = comer,
                                    Date = x.Date,
                                    P1 = x.P1
                                };

                                ft.Crmcom_BPARELProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }
                        //Remise FA//
                        if (ft.Crmcom_FAProsp.Where(a => a.ID_Prosp == isOp.ID).Count() != 0)
                        {
                            foreach (var x in ft.Crmcom_FAProsp.Where(a => a.ID_Prosp == isOp.ID).ToList())
                            {
                                var ins = new Crmcom_FAProsp
                                {
                                    ID_Prosp = comer,
                                    Date = x.Date,
                                    P1 = x.P1
                                };

                                ft.Crmcom_FAProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }
                        //Demo logiciel//
                        if (ft.Crmcom_DemoProsp.Where(a => a.ID_Prosp == isOp.ID).Count() != 0)
                        {
                            foreach (var x in ft.Crmcom_DemoProsp.Where(a => a.ID_Prosp == isOp.ID).ToList())
                            {
                                var ins = new Crmcom_DemoProsp
                                {
                                    ID_Prosp = comer,
                                    Date = x.Date,
                                    Rmq = x.Rmq,
                                    Date_CRR = x.Date_CRR,
                                    P1 = x.P1,
                                    P2 = x.P2
                                };

                                ft.Crmcom_DemoProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }
                        //Demo maquettées//
                        if (ft.Crmcom_DemoMaqProsp.Where(a => a.ID_Prosp == isOp.ID).Count() != 0)
                        {
                            foreach (var x in ft.Crmcom_DemoMaqProsp.Where(a => a.ID_Prosp == isOp.ID).ToList())
                            {
                                var ins = new Crmcom_DemoMaqProsp
                                {
                                    ID_Prosp = comer,
                                    Date = x.Date,
                                    Rmq = x.Rmq,
                                    Date_CRR = x.Date_CRR,
                                    P1 = x.P1,
                                    P2 = x.P2
                                };

                                ft.Crmcom_DemoMaqProsp.Add(ins);
                                ft.SaveChanges();
                            }
                        }

                        return Json("Avec succès", JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json("Erreur! La référence doit être unique!", JsonRequestBehavior.AllowGet);
                }
                else
                    return Json("Erreur! La référence est obligatoire!", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Erreur!", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                if (Session["UserId"] == null)
                    return Json("Erreur! Reconnectez-vous svp!", JsonRequestBehavior.AllowGet);
                else if (!String.IsNullOrEmpty(id.ToString()))
                {
                    if (db.Crmcom_CommercialeProsp.Where(a => a.ID == id).Count() != 0)
                    {
                        var isDelete = db.Crmcom_CommercialeProsp.Where(a => a.ID == id).FirstOrDefault();

                        if (isDelete.Date_Synch != null && db.Opportunity.Where(a => a.Oppo_Deleted != 1 && a.Oppo_Description == isDelete.ReferenceOppo).Count() != 0)
                        {
                            return Json("Erreur! Déjà synchronisée!", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            db.Crmcom_CommercialeProsp.Remove(isDelete);
                            db.SaveChanges();

                            //Analyse des besoins//
                            if (db.Crmcom_AnalBesoinProsp.Where(a => a.ID_Prosp == id).Count() != 0)
                            {
                                foreach (var y in db.Crmcom_AnalBesoinProsp.Where(a => a.ID_Prosp == id).ToList())
                                {
                                    var isdelet = db.Crmcom_AnalBesoinProsp.Where(a => a.ID_AnalBesoin == y.ID_AnalBesoin).FirstOrDefault();
                                    db.Crmcom_AnalBesoinProsp.Remove(isdelet);
                                    db.SaveChanges();
                                }
                            }

                            //Autr rdv//
                            if (db.Crmcom_AutrRDVProsp.Where(a => a.ID_Prosp == id).Count() != 0)
                            {
                                foreach (var y in db.Crmcom_AutrRDVProsp.Where(a => a.ID_Prosp == id).ToList())
                                {
                                    var isdelet = db.Crmcom_AutrRDVProsp.Where(a => a.ID_AutrRDV == y.ID_AutrRDV).FirstOrDefault();
                                    db.Crmcom_AutrRDVProsp.Remove(isdelet);
                                    db.SaveChanges();
                                }
                            }

                            //Remis devis excel//
                            if (db.Crmcom_RemiDevisProsp.Where(a => a.ID_Prosp == id).Count() != 0)
                            {
                                foreach (var y in db.Crmcom_RemiDevisProsp.Where(a => a.ID_Prosp == id).ToList())
                                {
                                    var isdelet = db.Crmcom_RemiDevisProsp.Where(a => a.ID_DateRemisDevis == y.ID_DateRemisDevis).FirstOrDefault();
                                    db.Crmcom_RemiDevisProsp.Remove(isdelet);
                                    db.SaveChanges();
                                }
                            }

                            //Autr rdvv//
                            if (db.Crmcom_AutrRDVvProsp.Where(a => a.ID_Prosp == id).Count() != 0)
                            {
                                foreach (var y in db.Crmcom_AutrRDVvProsp.Where(a => a.ID_Prosp == id).ToList())
                                {
                                    var isdelet = db.Crmcom_AutrRDVvProsp.Where(a => a.ID_AutrRDVv == y.ID_AutrRDVv).FirstOrDefault();
                                    db.Crmcom_AutrRDVvProsp.Remove(isdelet);
                                    db.SaveChanges();
                                }
                            }

                            //NEGO//
                            if (db.Crmcom_NegoProsp.Where(a => a.ID_Prosp == id).Count() != 0)
                            {
                                foreach (var y in db.Crmcom_NegoProsp.Where(a => a.ID_Prosp == id).ToList())
                                {
                                    var isdelet = db.Crmcom_NegoProsp.Where(a => a.ID_Nego == y.ID_Nego).FirstOrDefault();
                                    db.Crmcom_NegoProsp.Remove(isdelet);
                                    db.SaveChanges();
                                }
                            }

                            //Envoi devis GESCOM//
                            if (db.Crmcom_DevisGescomProsp.Where(a => a.ID_Prosp == id).Count() != 0)
                            {
                                foreach (var y in db.Crmcom_DevisGescomProsp.Where(a => a.ID_Prosp == id).ToList())
                                {
                                    var isdelet = db.Crmcom_DevisGescomProsp.Where(a => a.ID_DateDevisGescom == y.ID_DateDevisGescom).FirstOrDefault();
                                    db.Crmcom_DevisGescomProsp.Remove(isdelet);
                                    db.SaveChanges();
                                }
                            }

                            //Envoi BPA SAGE//
                            if (db.Crmcom_BPAProsp.Where(a => a.ID_Prosp == id).Count() != 0)
                            {
                                foreach (var y in db.Crmcom_BPAProsp.Where(a => a.ID_Prosp == id).ToList())
                                {
                                    var isdelet = db.Crmcom_BPAProsp.Where(a => a.ID_DateBPA == y.ID_DateBPA).FirstOrDefault();
                                    db.Crmcom_BPAProsp.Remove(isdelet);
                                    db.SaveChanges();
                                }
                            }

                            //Envoi BPA SAGE//
                            if (db.Crmcom_BPARELProsp.Where(a => a.ID_Prosp == id).Count() != 0)
                            {
                                foreach (var y in db.Crmcom_BPARELProsp.Where(a => a.ID_Prosp == id).ToList())
                                {
                                    var isdelet = db.Crmcom_BPARELProsp.Where(a => a.ID_DateRelBPA == y.ID_DateRelBPA).FirstOrDefault();
                                    db.Crmcom_BPARELProsp.Remove(isdelet);
                                    db.SaveChanges();
                                }
                            }

                            //Remise FA//
                            if (db.Crmcom_FAProsp.Where(a => a.ID_Prosp == id).Count() != 0)
                            {
                                foreach (var y in db.Crmcom_FAProsp.Where(a => a.ID_Prosp == id).ToList())
                                {
                                    var isdelet = db.Crmcom_FAProsp.Where(a => a.ID_DateFA == y.ID_DateFA).FirstOrDefault();
                                    db.Crmcom_FAProsp.Remove(isdelet);
                                    db.SaveChanges();
                                }
                            }

                            //Demo logiciel//
                            if (db.Crmcom_DemoProsp.Where(a => a.ID_Prosp == id).Count() != 0)
                            {
                                foreach (var y in db.Crmcom_DemoProsp.Where(a => a.ID_Prosp == id).ToList())
                                {
                                    var isdelet = db.Crmcom_DemoProsp.Where(a => a.ID_DemoLogk == y.ID_DemoLogk).FirstOrDefault();
                                    db.Crmcom_DemoProsp.Remove(isdelet);
                                    db.SaveChanges();
                                }
                            }

                            //Demo maquettées//
                            if (db.Crmcom_DemoMaqProsp.Where(a => a.ID_Prosp == id).Count() != 0)
                            {
                                foreach (var y in db.Crmcom_DemoMaqProsp.Where(a => a.ID_Prosp == id).ToList())
                                {
                                    var isdelet = db.Crmcom_DemoMaqProsp.Where(a => a.ID_DemoMaquette == y.ID_DemoMaquette).FirstOrDefault();
                                    db.Crmcom_DemoMaqProsp.Remove(isdelet);
                                    db.SaveChanges();
                                }
                            }

                            return Json("Suppression avec succès!", JsonRequestBehavior.AllowGet);

                            //int idAgent = int.Parse(Session["UserId"].ToString());

                            //return RedirectToAction("Index");
                        }
                    }
                    else
                        return Json("Erreur!", JsonRequestBehavior.AllowGet);
                }
                else
                    return Json("Erreur!", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)//DbEntityValidationException e)
            {
                return Json("Erreur!", JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpPost]
        public ActionResult Syncho(int id)
        {
            try
            {
                if (Session["UserId"] == null)
                    return Content("Erreur! Reconnectez-vous svp!");
                else if (!String.IsNullOrEmpty(id.ToString()))
                {
                    var isOP = db.Crmcom_CommercialeProsp.Where(a => a.ID == id).FirstOrDefault();
                    var refOppo = isOP.ReferenceOppo.Replace("  ", " ").TrimEnd(' ');
                    var Fopp = db.Opportunity.Where(a => a.Oppo_Description == refOppo && a.Oppo_Deleted == null).FirstOrDefault();

                    if (Fopp != null)
                    {
                        var isOppoCRM = db.Opportunity.Where(a => a.Oppo_Description == refOppo && a.Oppo_Deleted == null).FirstOrDefault();

                        //Source//
                        if (db.Custom_Captions.Where(a => a.Capt_Family == "Oppo_Source" && a.Capt_Deleted == null && a.Capt_Code == isOP.ID_Source).Count() != 0)
                        {
                            isOppoCRM.Oppo_Source = isOP.ID_Source;
                        }

                        //FMFP//
                        if (db.Custom_Captions.Where(a => a.Capt_Family == "oppo_fmfp" && a.Capt_Deleted == null && a.Capt_Code == isOP.FMFP).Count() != 0)
                        {
                            isOppoCRM.oppo_fmfp = isOP.FMFP;
                        }

                        //Analyse des besoin = Oppo_Note//
                        if (!String.IsNullOrEmpty(isOP.Analyse_Besoin))
                        {
                            isOppoCRM.Oppo_Note = isOP.Analyse_Besoin;
                        }

                        //Nb Salarié//
                        isOppoCRM.oppo_NbreSal = isOP.Nombre_Salarie;

                        //Mode de commercialisation//
                        if (isOP.ModeComm != null)
                        {
                            if (db.Custom_Captions.Where(a => a.Capt_Family == "oppo_modecom" && a.Capt_Deleted == null && a.Capt_Code == isOP.ModeComm).Count() != 0)
                            {
                                isOppoCRM.oppo_modecom = isOP.ModeComm;
                            }
                        }

                        //Produit//
                        isOppoCRM.oppo_produit2 = "," + isOP.Produit + ",";

                        //Deadline//
                        if (isOP.DeadLine != null)
                        {
                            isOppoCRM.Oppo_TargetClose = isOP.DeadLine;
                        }

                        //Nbr user//
                        isOppoCRM.oppo_nbreuser = isOP.Nombre_User;

                        //Date analyse des besoins et CRA//
                        if (db.Crmcom_AnalBesoinProsp.Where(a => a.ID_Prosp == isOP.ID).Count() != 0)
                        {
                            isOppoCRM.oppo_DateAnalyseBesoin = db.Crmcom_AnalBesoinProsp.Where(a => a.ID_Prosp == isOP.ID).Select(a => a.Date).Max();
                            isOppoCRM.oppo_DateCRA = db.Crmcom_AnalBesoinProsp.Where(a => a.ID_Prosp == isOP.ID).Select(a => a.Date_CRR).Max();

                            var isDateMax = db.Crmcom_AnalBesoinProsp.Where(a => a.ID_Prosp == isOP.ID).Max(a => a.Date);
                            var isA = db.Crmcom_AnalBesoinProsp.Where(a => a.ID_Prosp == isOP.ID && a.Date == isDateMax).FirstOrDefault();
                            if (!String.IsNullOrEmpty(isA.P1))
                            {
                                if (isA.P1 == "R")
                                    isOppoCRM.oppo_p1 = "Y";
                            }
                            if (!String.IsNullOrEmpty(isA.P2))
                            {
                                if (isA.P2 == "R")
                                    isOppoCRM.oppo_p2 = "Y";
                            }
                        }

                        //DemoProsp = demoStandard//
                        if (db.Crmcom_DemoProsp.Where(a => a.ID_Prosp == isOP.ID).Count() != 0)
                        {
                            isOppoCRM.oppo_DateDemo = db.Crmcom_DemoProsp.Where(a => a.ID_Prosp == isOP.ID).Select(a => a.Date).Max();
                            isOppoCRM.oppo_DateCRDemo = db.Crmcom_DemoProsp.Where(a => a.ID_Prosp == isOP.ID).Select(a => a.Date_CRR).Max();
                            isOppoCRM.oppo_condemostd = "," + db.Crmcom_DemoProsp.Where(a => a.ID_Prosp == isOP.ID).FirstOrDefault().Agent;

                            var isDateMax = db.Crmcom_DemoProsp.Where(a => a.ID_Prosp == isOP.ID).Max(a => a.Date);
                            var isA = db.Crmcom_DemoProsp.Where(a => a.ID_Prosp == isOP.ID && a.Date == isDateMax).FirstOrDefault();
                            if (!String.IsNullOrEmpty(isA.P1))
                            {
                                if (isA.P1 == "R")
                                    isOppoCRM.oppo_p3 = "Y";
                            }
                            if (!String.IsNullOrEmpty(isA.P2))
                            {
                                if (isA.P2 == "R")
                                    isOppoCRM.oppo_p6 = "Y";
                            }
                        }

                        //Date demo maquetté = date demoADV// sans CR
                        if (db.Crmcom_DemoMaqProsp.Where(a => a.ID_Prosp == isOP.ID).Count() != 0)
                        {
                            isOppoCRM.oppo_Datedemoadv = db.Crmcom_DemoMaqProsp.Where(a => a.ID_Prosp == isOP.ID).Select(a => a.Date).Max();
                            isOppoCRM.oppo_condemoavc = "," + db.Crmcom_DemoMaqProsp.Where(a => a.ID_Prosp == isOP.ID).FirstOrDefault().Agent;

                            var isDateMax = db.Crmcom_DemoMaqProsp.Where(a => a.ID_Prosp == isOP.ID).Max(a => a.Date);
                            var isA = db.Crmcom_DemoMaqProsp.Where(a => a.ID_Prosp == isOP.ID && a.Date == isDateMax).FirstOrDefault();
                            if (!String.IsNullOrEmpty(isA.P1))
                            {
                                if (isA.P1 == "R")
                                    isOppoCRM.oppo_p4 = "Y";
                            }
                        }

                        //Date remise devis = date devis// AVEC CR TY ANATY CRM NEFA TSIS CR ANATY HELPDESK => MILA JERENA
                        if (db.Crmcom_RemiDevisProsp.Where(a => a.ID_Prosp == isOP.ID).Count() != 0)
                        {
                            isOppoCRM.oppo_DateDevis = db.Crmcom_RemiDevisProsp.Where(a => a.ID_Prosp == isOP.ID).Select(a => a.Date).Max();

                            var isDateMax = db.Crmcom_RemiDevisProsp.Where(a => a.ID_Prosp == isOP.ID).Max(a => a.Date);
                            var isA = db.Crmcom_RemiDevisProsp.Where(a => a.ID_Prosp == isOP.ID && a.Date == isDateMax).FirstOrDefault();
                            if (!String.IsNullOrEmpty(isA.P1))
                            {
                                if (isA.P1 == "R")
                                    isOppoCRM.oppo_p7 = "Y";
                            }
                        }

                        //Date proposition CONTRAT//
                        if (isOP.Date_ProposContrat != null)
                        {
                            isOppoCRM.oppo_DatepropoContr = isOP.Date_ProposContrat;

                            if (!String.IsNullOrEmpty(isOP.P1ProposContrat))
                            {
                                if (isOP.P1ProposContrat == "R")
                                    isOppoCRM.oppo_p8 = "Y";
                            }
                        }

                        //Date precoTECH//
                        if (isOP.Date_PrecoTech != null)
                        {
                            isOppoCRM.oppo_DatePRECO = isOP.Date_PrecoTech;

                            if (!String.IsNullOrEmpty(isOP.P1PrecoTech))
                            {
                                if (isOP.P1PrecoTech == "R")
                                    isOppoCRM.oppo_p9 = "Y";
                            }
                        }

                        //Date progForm//
                        if (isOP.Date_ProgForm != null)
                        {
                            isOppoCRM.oppo_DatePROFORM = isOP.Date_ProgForm;

                            if (!String.IsNullOrEmpty(isOP.P1ProgForm))
                            {
                                if (isOP.P1ProgForm == "R")
                                    isOppoCRM.oppo_p10 = "Y";
                            }
                        }

                        //Autre RDV 2 = DateAutreRDV//
                        if (db.Crmcom_AutrRDVvProsp.Where(a => a.ID_Prosp == isOP.ID).Count() != 0)
                        {
                            isOppoCRM.oppo_DateAutreRDV = db.Crmcom_AutrRDVvProsp.Where(a => a.ID_Prosp == isOP.ID).Select(a => a.Date).Max();
                            isOppoCRM.oppo_CRAutreRDV = db.Crmcom_AutrRDVvProsp.Where(a => a.ID_Prosp == isOP.ID).Select(a => a.Date_CRR).Max();

                            var isDateMax = db.Crmcom_AutrRDVvProsp.Where(a => a.ID_Prosp == isOP.ID).Max(a => a.Date);
                            var isA = db.Crmcom_AutrRDVvProsp.Where(a => a.ID_Prosp == isOP.ID && a.Date == isDateMax).FirstOrDefault();
                            if (!String.IsNullOrEmpty(isA.P1))
                            {
                                if (isA.P1 == "R")
                                    isOppoCRM.oppo_p12 = "Y";
                            }
                            if (!String.IsNullOrEmpty(isA.P2))
                            {
                                if (isA.P2 == "R")
                                    isOppoCRM.oppo_p13 = "Y";
                            }
                        }

                        //NEGO et CR//
                        if (db.Crmcom_NegoProsp.Where(a => a.ID_Prosp == isOP.ID).Count() != 0)
                        {
                            isOppoCRM.oppo_DateNego = db.Crmcom_NegoProsp.Where(a => a.ID_Prosp == isOP.ID).Select(a => a.Date).Max();
                            isOppoCRM.oppo_DateCRNego = db.Crmcom_NegoProsp.Where(a => a.ID_Prosp == isOP.ID).Select(a => a.Date_CRR).Max();

                            var isDateMax = db.Crmcom_NegoProsp.Where(a => a.ID_Prosp == isOP.ID).Max(a => a.Date);
                            var isA = db.Crmcom_NegoProsp.Where(a => a.ID_Prosp == isOP.ID && a.Date == isDateMax).FirstOrDefault();
                            if (!String.IsNullOrEmpty(isA.P1))
                            {
                                if (isA.P1 == "R")
                                    isOppoCRM.oppo_p14 = "Y";
                            }
                            if (!String.IsNullOrEmpty(isA.P2))
                            {
                                if (isA.P2 == "R")
                                    isOppoCRM.oppo_p15 = "Y";
                            }
                        }

                        //Date proFORMA//
                        if (isOP.Date_ProForma != null)
                        {
                            isOppoCRM.oppo_DateProfo = isOP.Date_ProForma;

                            if (!String.IsNullOrEmpty(isOP.P1ProForma))
                            {
                                if (isOP.P1ProForma == "R")
                                    isOppoCRM.oppo_p17 = "Y";
                            }
                        }

                        //Date reception BC//
                        if (isOP.Date_ReceptionBC != null)
                        {
                            isOppoCRM.oppo_DateBC = isOP.Date_ReceptionBC;

                            if (!String.IsNullOrEmpty(isOP.P1ReceptionBC))
                            {
                                if (isOP.P1ReceptionBC == "R")
                                    isOppoCRM.oppo_p18 = "Y";
                            }
                        }

                        //Date BL//
                        if (isOP.Date_BL != null)
                        {
                            isOppoCRM.oppo_ssdi_datelivr = isOP.Date_BL;

                            if (!String.IsNullOrEmpty(isOP.P1BL))
                            {
                                if (isOP.P1BL == "R")
                                    isOppoCRM.oppo_p25 = "Y";
                            }
                        }

                        //Date FA//
                        if (db.Crmcom_FAProsp.Where(a => a.ID_Prosp == isOP.ID).Count() != 0)
                        {
                            isOppoCRM.oppo_DateFA = db.Crmcom_FAProsp.Where(a => a.ID_Prosp == isOP.ID).Select(a => a.Date).Max();

                            var isDateMax = db.Crmcom_FAProsp.Where(a => a.ID_Prosp == isOP.ID).Max(a => a.Date);
                            var isA = db.Crmcom_FAProsp.Where(a => a.ID_Prosp == isOP.ID && a.Date == isDateMax).FirstOrDefault();
                            if (!String.IsNullOrEmpty(isA.P1))
                            {
                                if (isA.P1 == "R")
                                    isOppoCRM.oppo_p20 = "Y";
                            }
                        }

                        //Date Contrat/
                        if (isOP.Date_Contrat != null)
                        {
                            isOppoCRM.oppo_DateContrat = isOP.Date_Contrat;

                            if (!String.IsNullOrEmpty(isOP.P1Contrat))
                            {
                                if (isOP.P1Contrat == "R")
                                    isOppoCRM.oppo_p21 = "Y";
                            }
                        }

                        //Date envoi BPA SAGE (editeur)//
                        if (db.Crmcom_BPAProsp.Where(a => a.ID_Prosp == isOP.ID).Count() != 0)
                        {
                            isOppoCRM.oppo_BPSage = db.Crmcom_BPAProsp.Where(a => a.ID_Prosp == isOP.ID).Select(a => a.Date).Max();

                            var isDateMax = db.Crmcom_BPAProsp.Where(a => a.ID_Prosp == isOP.ID).Max(a => a.Date);
                            var isA = db.Crmcom_BPAProsp.Where(a => a.ID_Prosp == isOP.ID && a.Date == isDateMax).FirstOrDefault();
                            if (!String.IsNullOrEmpty(isA.P1))
                            {
                                if (isA.P1 == "R")
                                    isOppoCRM.oppo_p22 = "Y";
                            }
                        }

                        //Date début//
                        if (isOP.Date_Debut != null)
                        {
                            isOppoCRM.Oppo_Opened = isOP.Date_Debut;
                        }

                        //Date fin//
                        if (isOP.Date_Fin != null)
                        {
                            isOppoCRM.Oppo_Closed = isOP.Date_Fin;
                        }

                        //Durée du traitement en jour//
                        if (isOP.Date_Debut != null && isOP.Date_Fin != null)
                        {
                            decimal diff = (isOP.Date_Fin.Value - isOP.Date_Debut.Value).Days;

                            isOppoCRM.oppo_durreTraitement = diff;
                        }

                        //Budget montant//
                        if (isOP.Budget != null)
                        {
                            decimal bdg = 0;
                            decimal.TryParse(isOP.Budget, out bdg);

                            isOppoCRM.Oppo_Forecast = bdg;
                        }

                        ///Budget Devise//
                        if (isOP.BudgetDevise != null)
                        {
                            if (db.Currency.Where(a => a.Curr_Symbol == isOP.BudgetDevise && a.Curr_Deleted == null).Count() != 0)
                            {
                                var isBdg = db.Currency.Where(a => a.Curr_Symbol == isOP.BudgetDevise && a.Curr_Deleted == null).FirstOrDefault();
                                isOppoCRM.Oppo_Forecast_CID = byte.Parse(isBdg.Curr_CurrencyID.ToString());
                            }
                        }

                        //Options//
                        if (!String.IsNullOrEmpty(isOP.Options))
                        {
                            isOppoCRM.oppo_option = ',' + isOP.Options;
                        }

                        //Modalité Détails//
                        if (!String.IsNullOrEmpty(isOP.Modalite_Paiement))
                        {
                            isOppoCRM.oppo_detailmodalite = isOP.Modalite_Paiement;
                        }

                        //P = PDead//
                        if (!String.IsNullOrEmpty(isOP.PDead))
                        {
                            if (isOP.PDead == "R")
                                isOppoCRM.oppo_p = "Y";
                        }

                        //Oppo_WaveItemId//
                        if (isOP.WaIt_WaveItemId != null)
                        {
                            isOppoCRM.Oppo_WaveItemId = isOP.WaIt_WaveItemId;
                        }

                        //Etape SOFTWELL//
                        if (isOP.EtapS != null)
                        {
                            isOppoCRM.oppo_etapesoftwell = isOP.EtapS;
                        }

                        isOP.Date_Synch = DateTime.Now;

                        db.SaveChanges();
                        return Content("Synchronisation avec succès!");
                    }
                    else
                        return Content("Erreur! Veuillez d'abord créer l'opportunité dans CRM avant la synchronisation");
                }
                else
                    return Content("Erreur!");
            }
            catch (Exception)//DbEntityValidationException e)
            {
                return Content("Erreur! Certains destinataires n'ont pas reçus, Mail non actif!");
            }
        }
    }

    public class AnalY
    {
        public string Date { get; set; }
        public string Rmq { get; set; }
        public string DateCRR { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
    }
    public class rdvY
    {
        public string Date { get; set; }
        public string Rmq { get; set; }
        public string DateCRR { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
    }
    public class demolY
    {
        public string Date { get; set; }
        public string Acteur { get; set; }
        public string Rmq { get; set; }
        public string DateCRR { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
    }
    public class OptionsY
    {
        public string Options { get; set; }
    }
    public class dexcelY
    {
        public string Date { get; set; }
        public string P1 { get; set; }
    }
    public class demoQY
    {
        public string Date { get; set; }
        public string Acteur { get; set; }
        public string Rmq { get; set; }
        public string DateCRR { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
    }
    public class rdvvY
    {
        public string Date { get; set; }
        public string Rmq { get; set; }
        public string DateCRR { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
    }
    public class negoY
    {
        public string Date { get; set; }
        public string Rmq { get; set; }
        public string DateCRR { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
    }
    public class dgescY
    {
        public string Date { get; set; }
        public string P1 { get; set; }
    }
    public class dbpaY
    {
        public string Date { get; set; }
        public string P1 { get; set; }
    }
    public class dbparY
    {
        public string Date { get; set; }
        public string P1 { get; set; }
    }
    public class rfaY
    {
        public string Date { get; set; }
        public string P1 { get; set; }
    }
    public class idForeign
    {
        public string TypeClient { get; set; }
        public string Etat { get; set; }
        public string Classification { get; set; }
        public string Source { get; set; }
        public string Rebut { get; set; }
        public string Rebut_Reason { get; set; }
    }
    public class ProduiT
    {
        public string Prod { get; set; }
    }
}
