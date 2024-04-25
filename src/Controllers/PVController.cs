//using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Helpdesk.Controllers {
    public class PVController : Controller {
        private readonly ModelHELPD _db;
        private readonly string _connectionString;

        public PVController() {
            _db = new ModelHELPD();

            _connectionString = ConfigurationManager.ConnectionStrings["ModelHELPD"].ConnectionString;
        }

        public async Task<ActionResult> Nouvelle_demande() {
            var collection = new Crmcli_Demandes();

            var typeD = new List<string>();

            var tmp = _db.Custom_Captions.Where(a => a.Capt_Family == "comm_typepresta" && a.Capt_Deleted == null);

            //TypeDemande//
            if (await tmp.CountAsync() != 0) {

                foreach (var x in await tmp.ToListAsync()) {
                    typeD.Add(x.Capt_FR);
                }
            }

            //Produit//
            /*List<string> produits = new List<string>();
            if (_db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted == null).Count() != 0)
            {
                foreach (var x in _db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted == null).ToList())
                {
                    produits.Add(x.Capt_FR);
                }
            }*/

            var theme = new List<string>();
            if (Session["IsAdmin"] != null) {
                foreach (var x in _db.Crmcli_Theme.Where(a => a.Theme != "Intervention demande client").OrderBy(a => a.Theme).ToList()) {
                    theme.Add(x.Theme);
                }
            }
            else {
                foreach (var x in _db.Crmcli_Theme.Where(a => a.Theme != "Intervention demande client" && a.Theme != "Dérogation").OrderBy(a => a.Theme).ToList()) {
                    theme.Add(x.Theme);
                }
            }

            collection.ThemeCollection = theme;

            collection.TypeDemanCollection = typeD;
            //collection.ProduitsCollection = produits;

            var prio = new List<string> {
                "",
                "Basse",
                "Moyenne",
                "Elevée"
            };

            collection.PrioritesCollection = prio;

            var nivo = new List<string> {
                "",
                "Non bloquant",
                "Bloquant"
            };

            collection.NiveauCollection = nivo;

            var etat = new List<string> {
                "En attente",
                "En cours",
                "Terminée",
                "Annulée"
            };
            collection.EtatCollection = etat;

            var cc = new List<List<string>>();
            var clients = new List<object>();

            foreach (var x in _db.Crmcli_Reg.ToList()) {
                if (_db.Company.Where(a => a.Comp_CompanyId == x.Comp_CompanyId).Count() != 0) {
                    var companyFirst = _db.Company.Where(a => a.Comp_CompanyId == x.Comp_CompanyId && a.Comp_Deleted != 1).FirstOrDefault();

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

            /*nouvelle_demande.ClientCollection = clt;

            var clients = _db.Company
            .Select(s => new
            {
                Text = s.Comp_Name,
                Value = s.Comp_CompanyId
            })
            .ToList();*/

            ViewBag.ClientsList = new SelectList(clients, "Value", "Text");

            return View(collection);
        }

        private async Task PostIntoInterventionsHistory(int demandeId) {
            //    using (var conn = new SqlConnection(_connectionString)) {
            //        await conn.OpenAsync();

            //        using (var cmd = new SqlCommand(@"
            //            INSERT INTO Crmcli_HistoIntervs 
            //            (ID_Demandes, Date_Comm, Sujets, Descriptions, IP_CheckPC, ID_Company, ID_Agent, EtatH, Lien_Validation, SenderAgent, Activite, Nature, DateSaisieHisto)
            //            VALUES (@demandeId, @date_Comm, @sujet, @description, @ipCheckPC, @companyId, @agentId, @status, @validationLink, @senderAgent, @activity, @nature, GETDATE();
            //        ", conn)) {
            //            cmd.Parameters.AddWithValue("@demandeId", demandeId);
            //            cmd.Parameters.AddWithValue("@date_Comm", demandeId);
            //            cmd.Parameters.AddWithValue("@sujet", demandeId);
            //            cmd.Parameters.AddWithValue("@description", demandeId);
            //            cmd.Parameters.AddWithValue("@ipCheckPC", demandeId);
            //            cmd.Parameters.AddWithValue("@companyId", demandeId);
            //            cmd.Parameters.AddWithValue("@agentId", demandeId);
            //            cmd.Parameters.AddWithValue("@status", demandeId);
            //            cmd.Parameters.AddWithValue("@validationLink", demandeId);
            //            cmd.Parameters.AddWithValue("@senderAgent", demandeId);
            //            cmd.Parameters.AddWithValue("@activity", demandeId);
            //            cmd.Parameters.AddWithValue("@nature", demandeId);

            //            await cmd.ExecuteNonQueryAsync();
            //        }
            //    };
        }

        private string EscapeXML(string input) {
            var res = input;

            res = res.Replace("\"", "&quot;");
            res = res.Replace("'", "&apos;");
            res = res.Replace("<", "&lt;");
            res = res.Replace(">", "&gt;");
            res = res.Replace("&", "&amp;");

            return res;
        }

        private void CreatWordDoc(string filename, string product, string clientName) {
            string nameDir = $"{DateTime.Now.ToFileTime()}";

            //string nameDir = "";
            if (Session["UserId"] != null) {
                nameDir = Session["UserId"].ToString() + "FF";
            }

            Session["FILENAME"] = filename;

            var newPath = Server.MapPath("~/CRS/PV_INSTALLATION.unzipped" + nameDir);

            if (!System.IO.File.Exists(Server.MapPath($"~/TEMPLATES/TEMPCRS/PV_INSTALLATION{nameDir}.zip"))) {
                string sourceFile = Server.MapPath("~/PV_INSTALLATION.zip");

                string destinationFile = Server.MapPath($"~/TEMPLATES/TEMPCRS/PV_INSTALLATION{nameDir}.zip");

                System.IO.File.Copy(sourceFile, destinationFile);
            }

            using (ZipArchive archive = ZipFile.OpenRead(Server.MapPath($"~/TEMPLATES/TEMPCRS/PV_INSTALLATION{nameDir}.zip"))) {
                if (Directory.Exists(newPath)) {
                    Directory.Delete(newPath, true);

                    archive.ExtractToDirectory(newPath);
                }
                else {
                    archive.ExtractToDirectory(newPath);
                }
            }

            var dir = new DirectoryInfo($@"{newPath}\word");

            FileInfo[] files = dir.GetFiles("*.xml");

            for (int i = 0; i < files.Length; i += 1) {
                string path = Path.Combine(files[i].FullName);

                string fileContent = System.IO.File.ReadAllText(path);

                if (string.IsNullOrEmpty(product))
                    product = "";
                if (string.IsNullOrEmpty(clientName))
                    clientName = "";

                fileContent = fileContent.Replace("[Produit]", EscapeXML(product));
                fileContent = fileContent.Replace("[Société cliente]", EscapeXML(clientName));
                fileContent = fileContent.Replace("[Client]", EscapeXML(clientName));
                fileContent = fileContent.Replace("[Nom du produit – Version du logiciel – Nombre de licence]", EscapeXML(product));
                fileContent = fileContent.Replace("[Signature]", EscapeXML(""));
                fileContent = fileContent.Replace("[Nom et prénoms]", EscapeXML(""));

                System.IO.File.WriteAllText(path, fileContent);
            }

            var pathFile = Server.MapPath("~/CRS/" + filename);

            if (System.IO.File.Exists(pathFile)) {
                System.IO.File.Delete(pathFile);

                ZipFile.CreateFromDirectory(newPath, Server.MapPath("~/CRS/" + filename));
            }
            else {
                ZipFile.CreateFromDirectory(newPath, Server.MapPath("~/CRS/" + filename));
            }
        }

        private async Task<string> GetProductName(string Id) {
            //using (var conn = new SqlConnection(_connectionString)) {
            //    await conn.OpenAsync();

            //    using (var cmd = new SqlCommand(@"
            //        SELECT Capt_FR AS ProductName FROM Custom_Captions 
            //        WHERE Capt_Family = 'case_prod1' 
            //        AND Capt_Code = @Id 
            //        AND Capt_Deleted IS NULL;
            //    ", conn)) {
            //        cmd.Parameters.AddWithValue("Id", Id);

            //        using (var reader = await cmd.ExecuteReaderAsync()) {
            //            if (await reader.ReadAsync()) {
            //                return reader["ProductName"].ToString();
            //            }
            //        }
            //    }
            //};

            return "";
        }

        [HttpPost]
        public async Task<ActionResult> CreateD(Crmcli_Demandes nouvelle_demande) {
            if (nouvelle_demande.ProduitDemande == null)
                return Content("Veuillez sélectionner le produit de votre demande!");
            if (nouvelle_demande.Rubrique == null)
                return Content("Veuillez sélectionner le rubrique de votre demande!");
            if (nouvelle_demande.ClientCollection == null)
                return Content("Veuillez sélectionner le client!");
            if (nouvelle_demande.ThemeCollection == null)
                return Content("Veuillez sélectionner le thème!");

            var rubriqueName = nouvelle_demande.Rubrique;
            var produitName = nouvelle_demande.ProduitDemande;
            var idContrat = 0;
            var CanCreate = false;

            //Blocage création demande si il existe des interventions debut janvier non validées//
            var dD = new DateTime(2021, 12, 31, 23, 59, 59);

            var idCompany = 0;
            var cltName = nouvelle_demande.ClientCollection;
            foreach (var x in _db.Company.Where(a => a.comp_raison_sociale_af_ay_iltx == cltName || a.Comp_Name == cltName).ToList()) {
                if (_db.Crmcli_Reg.Where(a => a.Comp_CompanyId == x.Comp_CompanyId).Count() != 0)
                    idCompany = x.Comp_CompanyId;
            }

            if (_db.Crmcli_HistoIntervs.Where(a => a.ID_Company == idCompany && a.ID_Pers_Validateur == null && a.Date_Comm > dD).Count() != 0)
                return Content("Veuillez valider les interventions restantes à partir du 01/01/2022 avant d'effectuer une nouvelle demande!");
            //else if (_db.Crmcli_HistoIntervs.Where(a => a.ID_Company == idCompany && a.ID_Pers_Validateur == null && a.Date_Comm > dD).Count() == 0) {
            if (rubriqueName == "Sans rubrique" || rubriqueName == "Dérogation") {
                CanCreate = true;
            }
            else {
                DateTime dDepart = DateTime.Now;
                if (_db.Company.Where(a => a.Comp_CompanyId == idCompany).FirstOrDefault().comp_corespondance != null) {
                    var coresp = _db.Company.Where(a => a.Comp_CompanyId == idCompany).FirstOrDefault().comp_corespondance;
                    foreach (var x in _db.Company.Where(a => a.comp_corespondance == coresp).ToList()) {
                        if (_db.SSDI_BUSINESS.Where(a => a.busi_companyid == x.Comp_CompanyId && a.busi_datefin >= dDepart && a.busi_dateinit <= dDepart && a.busi_Deleted != 1).Count() != 0) {
                            foreach (var y in _db.SSDI_BUSINESS.Where(a => a.busi_companyid == x.Comp_CompanyId && a.busi_datefin >= dDepart && a.busi_dateinit <= dDepart && a.busi_Deleted != 1).ToList()) {
                                if (_db.SSDI_SERVICE.Where(a => a.serv_SSDI_BUSINESSid == y.busi_SSDI_BUSINESSid && a.serv_name == rubriqueName && a.serv_Deleted != 1).Count() != 0) {
                                    idContrat = y.busi_SSDI_BUSINESSid;
                                }
                            }
                        }
                    }
                }
            }

            if (idContrat == 0) {
                if (rubriqueName != "Sans rubrique" && rubriqueName != "Dérogation") {
                    return Content("Vous ne pouvez pas effectuer une nouvelle demande! Contrat(s) expiré(s)");
                }
            }
            else if (idContrat != 0 && rubriqueName != "Sans rubrique" && rubriqueName != "Dérogation") {
                var isContrat = _db.SSDI_BUSINESS.Where(a => a.busi_SSDI_BUSINESSid == idContrat).FirstOrDefault();

                if (isContrat.busi_nbjrlim == null || isContrat.busi_nbjrlim == "02")
                    CanCreate = true;
                else if (isContrat.busi_nbjrlim == "01") {
                    double TotalNbrJourMinute = 0;
                    double TotalIntervMinute = 0;
                    var Service = _db.SSDI_SERVICE.Where(a => a.serv_SSDI_BUSINESSid == idContrat && a.serv_name == rubriqueName && a.serv_Deleted != 1).FirstOrDefault();
                    //Total nombre de jour limite par service en minutes//
                    if (Service.serv_nbjr != null && Service.serv_nbjr != 0)
                        TotalNbrJourMinute = Convert.ToDouble(Service.serv_nbjr.Value * 8 * 60);

                    //Get total heure travaillé des intervention de type Assistance entre les dates du contrat pour le client//
                    //All type assistance apart à distance tel ou mail//
                    int idACl = int.Parse(Session["UserId"].ToString());
                    if (_db.Company.Where(a => a.Comp_CompanyId == idACl).FirstOrDefault().comp_corespondance != null) {
                        var coresp = _db.Company.Where(a => a.Comp_CompanyId == idACl).FirstOrDefault().comp_corespondance;

                        foreach (var x in _db.Company.Where(a => a.comp_corespondance == coresp).ToList()) {
                            foreach (var y in (from cmd in _db.Comm_Link
                                               join cmp in _db.Company
                                               on cmd.CmLi_Comm_CompanyId equals cmp.Comp_CompanyId
                                               join com in _db.Communication
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
                                var commU = _db.Communication.Where(a => a.Comm_CommunicationId == y.idCommu).FirstOrDefault();
                                var pau = TimeSpan.FromMinutes(Convert.ToDouble(commU.comm_pause)).TotalMinutes;

                                TotalIntervMinute += TimeSpan.FromMinutes(Convert.ToDouble((commU.comm_datefin2.Value - commU.Comm_ToDateTime.Value).TotalMinutes - pau)).TotalMinutes;
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

                if (_db.Company.Where(a => a.Comp_CompanyId == idCompany).FirstOrDefault().comp_corespondance != null) {
                    var coresp = _db.Company.Where(a => a.Comp_CompanyId == idCompany).FirstOrDefault().comp_corespondance;

                    foreach (var x in _db.Company.Where(a => a.comp_corespondance == coresp).ToList()) {
                        if (_db.SSDI_BUSINESS.Where(a => a.busi_companyid == x.Comp_CompanyId && a.busi_datefin > dDepart && a.busi_Deleted != 1).Count() != 0) {
                            foreach (var y in _db.SSDI_BUSINESS.Where(a => a.busi_companyid == x.Comp_CompanyId && a.busi_datefin > dDepart && a.busi_Deleted != 1).ToList()) {
                                if (_db.SSDI_SERVICE.Where(a => a.serv_SSDI_BUSINESSid == y.busi_SSDI_BUSINESSid && a.serv_Deleted != 1).Count() != 0) {
                                    foreach (var z in _db.SSDI_SERVICE.Where(a => a.serv_SSDI_BUSINESSid == y.busi_SSDI_BUSINESSid && a.serv_Deleted != 1).ToList()) {
                                        if (z.serv_name == rubriqueName)
                                            rubrique = z.serv_SSDI_SERVICEid.ToString();
                                    }
                                }
                            }
                        }
                    }
                }

                nouvelle_demande.Rubrique = rubrique;

                //Demandeur interne//
                var idAgent = int.Parse(Session["UserId"].ToString());
                nouvelle_demande.Demandeur = idAgent;

                var userAssig = _db.users.Where(a => a.User_UserId == idAgent && a.User_Deleted != 1).FirstOrDefault();
                var NameAgent = string.Format("{0} {1}", userAssig.User_LastName, userAssig.User_FirstName);
                var MailAgent = string.Format("{0}", userAssig.User_EmailAddress);
                var TelAgent = "";
                if (userAssig.User_MobilePhone != null) {
                    TelAgent = string.Format("{0}", userAssig.User_MobilePhone);
                }

                //Thème//
                var thenme = nouvelle_demande.ThemeCollection.FirstOrDefault();
                if (_db.Crmcli_Theme.Where(a => a.Theme == thenme).Count() != 0) {
                    nouvelle_demande.Theme = _db.Crmcli_Theme.Where(a => a.Theme == thenme).FirstOrDefault().ID;
                }

                //NumeroDemande//
                string numTicket = "";
                if (_db.Company.Where(a => a.Comp_CompanyId.Equals(idCompany) && idCompany != 0).Count() != 0) {
                    var getC = _db.Crmcli_Reg.Where(a => a.Comp_CompanyId == idCompany).FirstOrDefault();
                    //Numéro//
                    var abreg = "";
                    if (getC.Comp_CompanyId != null) {
                        abreg = getC.Comp_CompanyId.ToString() + "/";
                    }
                    //Num chiffre//
                    var numC = 0;
                    var numOK = 0;
                    if (_db.Crmcli_Demandes.Where(a => a.Comp_CompanyId == idCompany).Count() != 0) {
                        foreach (var y in _db.Crmcli_Demandes.Where(a => a.Comp_CompanyId == idCompany).ToList()) {
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

                nouvelle_demande.NumeroDemande = numTicket;

                var prod = produitName;
                if (_db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted == null && a.Capt_FR == prod).Count() != 0) {
                    var produit = _db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted == null && a.Capt_FR == prod).FirstOrDefault();
                    nouvelle_demande.ProduitDemande = produit.Capt_Code;
                }

                var prio = nouvelle_demande.PrioritesCollection.FirstOrDefault();
                if (prio != null) {
                    switch (prio) {
                        case "Basse":
                            nouvelle_demande.PrioriteDemande = 1;
                            break;
                        case "Moyenne":
                            nouvelle_demande.PrioriteDemande = 2;
                            break;
                        case "Elevée":
                            nouvelle_demande.PrioriteDemande = 3;
                            break;
                        case "":
                            nouvelle_demande.PrioriteDemande = 1;
                            break;
                    }
                }
                else {
                    nouvelle_demande.PrioriteDemande = 1;
                }

                var nivo = nouvelle_demande.NiveauCollection.FirstOrDefault();

                if (nivo != null) {
                    switch (nivo) {
                        case "Non bloquant":
                            nouvelle_demande.NiveauDemande = 1;

                            break;
                        case "Bloquant":
                            nouvelle_demande.NiveauDemande = 2;

                            break;
                        case "":
                            nouvelle_demande.NiveauDemande = 1;

                            break;
                    }
                }
                else {
                    nouvelle_demande.NiveauDemande = 1;
                }

                nouvelle_demande.EtatDemande = 1;

                nouvelle_demande.Comp_CompanyId = idCompany;

                nouvelle_demande.DateDemande = DateTime.Now;

                if (nouvelle_demande.DatePropose == null) {
                    nouvelle_demande.DatePropose = nouvelle_demande.DateDemande;
                }

                if (_db.Crmcli_Demandes.Where(a => a.SujetDemande == nouvelle_demande.SujetDemande && a.DescriptionDemande == nouvelle_demande.DescriptionDemande
                    && a.Demandeur == nouvelle_demande.Demandeur && a.Comp_CompanyId == idCompany
                    && a.DateDemande.Value.Day == nouvelle_demande.DateDemande.Value.Day && a.DateDemande.Value.Month == nouvelle_demande.DateDemande.Value.Month && a.DateDemande.Value.Year == nouvelle_demande.DateDemande.Value.Year).Count() == 0) {
                    _db.Crmcli_Demandes.Add(nouvelle_demande);
                    _db.SaveChanges();

                    var dt = nouvelle_demande.DateDemande.ToString();

                    if (dt.Contains("/")) {
                        dt = dt.Replace("/", "");
                    }

                    if (dt.Contains(" ")) {
                        dt = dt.Replace(" ", "_");
                    }

                    if (dt.Contains(":")) {
                        dt = dt.Replace(":", "-");
                    }

                    var productName = await GetProductName(nouvelle_demande.ProduitDemande);

                    string filename = $"PV_INSTALLATION_{nouvelle_demande.ClientCollection}_{dt}.docx";
                    CreatWordDoc($"{filename}", productName, nouvelle_demande.ClientCollection);

                    return Content(filename);
                }
                else {
                    return Content("Demande déjà envoyée! Merci!");
                }
            }
        }

        [HttpGet]
        public ActionResult GenerateWordFile(string filename) {
            string fullName = Server.MapPath("~/CRS/") + filename;

            return File(fullName, "application/vnd.ms-word", filename);
        }
    }
}
