using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Helpdesk.Models;
using System.IO;
using System.Net.Mail;

using System.IO.Compression;
using System.Net.Mime;
using System.Drawing;

using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraPrinting;
using Syncfusion.DocIO;
using Syncfusion.DocToPDFConverter;

namespace Helpdesk.Controllers
{
    public class InterventionController : Controller
    {
        Microsoft.Office.Interop.Word.Document wordDoc { get; set; }

        RichEditDocumentServer server = new RichEditDocumentServer();

        //private Word.Application WordApp = new Microsoft.Office.Interop.Word.Application();
        //private Word.Document d;

        private object missing = System.Reflection.Missing.Value;
        private object yes = true;
        private object no = false;

        private string pathFileTEST = "";

        private string TACHECON = "";
        private string OBSERVATIONCON = "";

        ModelHELPD db = new ModelHELPD();

        //
        // GET: /Intervention/
        /*public ActionResult AddToCart(int id)
        {
            
        }*/
        public ActionResult Index()
        {
            var notif = "ssdfg";
            ViewBag.toaster = notif;
            //return RedirectToAction("Index");

            List<List<string>> list2d = new List<List<string>>();
            List<List<string>> listTache = new List<List<string>>();

            //Where ID client//
            if (Session["UserId"] != null)
            {
                int idAgent = int.Parse(Session["UserId"].ToString());

                if (db.users.Where(a => a.User_UserId == idAgent && a.User_Deleted != 1).Count() != 0)
                {
                    //Test si participant AGENT//
                    if (db.Communication.Where(a => a.comm_userintervenant.Contains(idAgent.ToString())).Count() != 0)
                    {
                        foreach (var itComm in db.Communication.Where(a => a.comm_userintervenant.Contains(idAgent.ToString())).Select(a => a.Comm_CaseId).Distinct().ToList())
                        {
                            if (db.Cases.Where(a => a.Case_CaseId == itComm && a.Case_Deleted != 1).Count() != 0)
                            {
                                foreach (var x in db.Cases.Where(a => a.Case_CaseId == itComm && a.Case_Deleted != 1).ToList())
                                {
                                    var list = new List<string>();
                                    var tache = new List<string>();

                                    list.Add(x.Case_CaseId.ToString());//ID

                                    var date = "";
                                    if (x.case_ssdi_datemessage != null)
                                    {
                                        date = x.case_ssdi_datemessage.Value.ToShortDateString();
                                    }
                                    list.Add(date);//DATE 
                                    list.Add(x.Case_ReferenceId);//REFERENCE
                                    list.Add(x.Case_Description);//DESCRIPTION
                                    list.Add(x.case_ssdi_demandecomment);//DESCRIPTION PRBLM

                                    //Status//
                                    var stat = "";
                                    if (x.Case_Status != null)
                                    {
                                        if (db.Custom_Captions.Where(a => a.Capt_Family.ToLower() == "case_status" && a.Capt_Code == x.Case_Status && a.Capt_Deleted != 1).Count() != 0)
                                        {
                                            var sta = db.Custom_Captions.Where(a => a.Capt_Family.ToLower() == "case_status" && a.Capt_Code == x.Case_Status && a.Capt_Deleted != 1).FirstOrDefault();
                                            stat = sta.Capt_FR;
                                        }
                                    }
                                    list.Add(stat);

                                    //Etapes//
                                    var etape = "";
                                    if (x.Case_Stage != null)
                                    {
                                        if (db.Custom_Captions.Where(a => a.Capt_Family.ToLower() == "case_stage" && a.Capt_Code == x.Case_Stage && a.Capt_Deleted != 1).Count() != 0)
                                        {
                                            var sta = db.Custom_Captions.Where(a => a.Capt_Family.ToLower() == "case_stage" && a.Capt_Code == x.Case_Stage && a.Capt_Deleted != 1).FirstOrDefault();
                                            etape = sta.Capt_FR;
                                        }
                                    }
                                    list.Add(etape);

                                    if (db.users.Where(a => a.User_UserId == x.Case_AssignedUserId && a.User_Deleted != 1).Count() != 0)//ASSIGNEE AGENT
                                    {
                                        var userAssig = db.users.Where(a => a.User_UserId == x.Case_AssignedUserId && a.User_Deleted != 1).FirstOrDefault();
                                        list.Add(string.Format("{0} {1}", userAssig.User_LastName, userAssig.User_FirstName));
                                    }
                                    else
                                        list.Add("");

                                    if (db.Communication.Where(a => a.Comm_CaseId == x.Case_CaseId && a.Comm_Deleted != 1).Count() != 0)
                                    {
                                        var communication = db.Communication.Where(a => a.Comm_CaseId == x.Case_CaseId && a.Comm_Deleted != 1).FirstOrDefault();

                                        if (db.Comm_Link.Where(a => a.CmLi_Comm_CommunicationId == communication.Comm_CommunicationId && a.CmLi_Deleted != 1).Count() != 0)//CLIENT
                                        {
                                            var companyT = db.Comm_Link.Where(a => a.CmLi_Comm_CommunicationId == communication.Comm_CommunicationId && a.CmLi_Deleted != 1).FirstOrDefault();
                                            if (db.Company.Where(a => a.Comp_CompanyId == companyT.CmLi_Comm_CompanyId && a.Comp_Deleted != 1).Count() != 0)
                                            {
                                                var companyFirst = db.Company.Where(a => a.Comp_CompanyId == companyT.CmLi_Comm_CompanyId && a.Comp_Deleted != 1).FirstOrDefault();
                                                if (companyFirst.comp_raison_sociale_af_ay_iltx != null)
                                                {
                                                    list.Add(companyFirst.comp_raison_sociale_af_ay_iltx);
                                                }
                                                else if (companyFirst.Comp_Name != null)
                                                {
                                                    list.Add(companyFirst.Comp_Name);
                                                }
                                            }
                                            else
                                                list.Add("");
                                        }
                                        else
                                            list.Add("");

                                        if (communication.comm_typepresta != null)//Type de prestation
                                        {
                                            if (db.Custom_Captions.Where(a => a.Capt_Family == "comm_typepresta" && a.Capt_Code == communication.comm_typepresta && a.Capt_Deleted != 1).Count() != 0)
                                            {
                                                var typeInterv = db.Custom_Captions.Where(a => a.Capt_Family == "comm_typepresta" && a.Capt_Code == communication.comm_typepresta && a.Capt_Deleted != 1).FirstOrDefault().Capt_FR;
                                                list.Add(typeInterv);
                                            }
                                            else
                                                list.Add("");
                                        }
                                        else
                                            list.Add("");
                                    }
                                    else
                                    {
                                        list.Add("");
                                        list.Add("");
                                    }

                                    int countTask = 0;
                                    if (db.Communication.Where(a => a.Comm_CaseId == x.Case_CaseId && a.Comm_Deleted != 1).Count() != 0)
                                    {
                                        int i = 0;
                                        foreach (var z in db.Communication.Where(a => a.Comm_CaseId == x.Case_CaseId && a.Comm_Deleted != 1).Select(a => a.Comm_ToDateTime).Distinct().ToList())
                                        {
                                            string dateT = z.Value.ToShortDateString();

                                            if (tache.Count() != 0)
                                            {
                                                var testContains = tache[i].Contains(dateT);

                                                if (testContains == false)
                                                {
                                                    tache.Add(dateT);

                                                    var dateInterne = "";
                                                    var dateclient = "";

                                                    var dateValidate = "";
                                                    var HeurDeb = "";
                                                    var HeurFin = "";
                                                    var IPvalidateur = "";

                                                    var signataire = "";

                                                    if (db.Crmcli_EnvoiMails.Where(a => a.Case_CaseId == x.Case_CaseId && a.CommDATE.Value.Day == z.Value.Day
                                                        && a.CommDATE.Value.Month == z.Value.Month && a.CommDATE.Value.Year == z.Value.Year).Count() != 0)
                                                    {
                                                        var fordate = db.Crmcli_EnvoiMails.Where(a => a.Case_CaseId == x.Case_CaseId && a.CommDATE.Value.Day == z.Value.Day
                                                        && a.CommDATE.Value.Month == z.Value.Month && a.CommDATE.Value.Year == z.Value.Year).FirstOrDefault();
                                                        if (fordate.DateClient != null)
                                                        {
                                                            dateclient = fordate.DateClient.Value.ToShortDateString();
                                                        }
                                                        if (fordate.DateInterne != null)
                                                        {
                                                            dateInterne = fordate.DateInterne.Value.ToShortDateString();
                                                        }
                                                    }

                                                    tache.Add(dateInterne);
                                                    tache.Add(dateclient);

                                                    countTask += 8;

                                                    //Date de validation CR par le client et heure DEB et heure FIN//
                                                    if (db.Crmcli_ValidateFiches.Where(a => a.ID_Cases == x.Case_CaseId && a.Date_Comm.Value.Day == z.Value.Day
                                                        && a.Date_Comm.Value.Month == z.Value.Month && a.Date_Comm.Value.Year == z.Value.Year).Count() != 0)
                                                    {
                                                        var validationFiche = db.Crmcli_ValidateFiches.Where(a => a.ID_Cases == x.Case_CaseId && a.Date_Comm.Value.Day == z.Value.Day
                                                        && a.Date_Comm.Value.Month == z.Value.Month && a.Date_Comm.Value.Year == z.Value.Year).FirstOrDefault();

                                                        dateValidate = validationFiche.Date_Validation.Value.ToShortDateString();
                                                        HeurDeb = validationFiche.Debut.Value.ToString();
                                                        HeurFin = validationFiche.Fin.Value.ToString();
                                                        IPvalidateur = validationFiche.IP_CheckPC;

                                                        if (db.Person.Where(a => a.Pers_PersonId == validationFiche.ID_Pers_Validateur && a.Pers_Deleted != 1).Count() != 0)//ASSIGNEE AGENT
                                                        {
                                                            var userAssig = db.Person.Where(a => a.Pers_PersonId == validationFiche.ID_Pers_Validateur && a.Pers_Deleted != 1).FirstOrDefault();
                                                            signataire = string.Format("{0} {1}-{2}", userAssig.Pers_LastName, userAssig.Pers_FirstName, userAssig.Pers_Title);
                                                        }
                                                    }

                                                    tache.Add(dateValidate);
                                                    tache.Add(signataire);
                                                    tache.Add(HeurDeb);
                                                    tache.Add(HeurFin);
                                                    tache.Add(IPvalidateur);

                                                    i += 8;
                                                }
                                            }
                                            else
                                            {
                                                tache.Add(dateT);

                                                var dateInterne = "";
                                                var dateclient = "";

                                                var dateValidate = "";
                                                var HeurDeb = "";
                                                var HeurFin = "";
                                                var IPvalidateur = "";

                                                var signataire = "";

                                                if (db.Crmcli_EnvoiMails.Where(a => a.Case_CaseId == x.Case_CaseId && a.CommDATE.Value.Day == z.Value.Day
                                                    && a.CommDATE.Value.Month == z.Value.Month && a.CommDATE.Value.Year == z.Value.Year).Count() != 0)
                                                {
                                                    var fordate = db.Crmcli_EnvoiMails.Where(a => a.Case_CaseId == x.Case_CaseId && a.CommDATE.Value.Day == z.Value.Day
                                                    && a.CommDATE.Value.Month == z.Value.Month && a.CommDATE.Value.Year == z.Value.Year).FirstOrDefault();
                                                    if (fordate.DateClient != null)
                                                    {
                                                        dateclient = fordate.DateClient.Value.ToShortDateString();
                                                    }
                                                    if (fordate.DateInterne != null)
                                                    {
                                                        dateInterne = fordate.DateInterne.Value.ToShortDateString();
                                                    }
                                                }

                                                tache.Add(dateInterne);
                                                tache.Add(dateclient);

                                                countTask += 8;

                                                //Date de validation CR par le client et heure DEB et heure FIN//
                                                if (db.Crmcli_ValidateFiches.Where(a => a.ID_Cases == x.Case_CaseId && a.Date_Comm.Value.Day == z.Value.Day
                                                    && a.Date_Comm.Value.Month == z.Value.Month && a.Date_Comm.Value.Year == z.Value.Year).Count() != 0)
                                                {
                                                    var validationFiche = db.Crmcli_ValidateFiches.Where(a => a.ID_Cases == x.Case_CaseId && a.Date_Comm.Value.Day == z.Value.Day
                                                    && a.Date_Comm.Value.Month == z.Value.Month && a.Date_Comm.Value.Year == z.Value.Year).FirstOrDefault();

                                                    dateValidate = validationFiche.Date_Validation.Value.ToShortDateString();
                                                    HeurDeb = validationFiche.Debut.Value.ToString();
                                                    HeurFin = validationFiche.Fin.Value.ToString();
                                                    IPvalidateur = validationFiche.IP_CheckPC;

                                                    if (db.Person.Where(a => a.Pers_PersonId == validationFiche.ID_Pers_Validateur && a.Pers_Deleted != 1).Count() != 0)//ASSIGNEE AGENT
                                                    {
                                                        var userAssig = db.Person.Where(a => a.Pers_PersonId == validationFiche.ID_Pers_Validateur && a.Pers_Deleted != 1).FirstOrDefault();
                                                        signataire = string.Format("{0} {1}-{2}", userAssig.Pers_LastName, userAssig.Pers_FirstName, userAssig.Pers_Title);
                                                    }
                                                }

                                                tache.Add(dateValidate);
                                                tache.Add(signataire);
                                                tache.Add(HeurDeb);
                                                tache.Add(HeurFin);
                                                tache.Add(IPvalidateur);
                                            }
                                        }
                                    }


                                    list.Add(countTask.ToString());
                                    list.AddRange(tache);

                                    list2d.Add(list);
                                }
                            }
                        }
                    }
                }
            }

            var tac = listTache;
            var lis = list2d;

            ViewBag.ldv = lis;
            ViewBag.ltache = tac;
            return View();
        }

        //

        // GET: /Intervention/Details/5

        public ActionResult Details(int id, string dateCom)
        {
            List<List<string>> listCases = new List<List<string>>();
            List<List<string>> listTaches = new List<List<string>>();
            //List<List<string>> listConsultantIntervenants = new List<List<string>>();
            List<List<string>> listPlanings = new List<List<string>>();
            List<string> listC = new List<string>();

            List<string> listCN = new List<string>();

            List<string> listCM = new List<string>();

            List<List<string>> listDATE = new List<List<string>>();

            Session["idCases"] = id;
            DateTime dateTest = DateTime.Parse(dateCom).Date;

            var client = "";

            var idComp = 0;

            var list = new List<string>();
            //Where ID client//
            if (id != 0)
            {
                //CASES//
                var azdazd = db.Cases.Where(a => a.Case_CaseId == id && a.Case_Deleted != 1).ToList();
                if (db.Cases.Where(a => a.Case_CaseId == id && a.Case_Deleted != 1).Count() != 0)
                {
                    var cases = db.Cases.Where(a => a.Case_CaseId == id && a.Case_Deleted != 1).FirstOrDefault();

                    list.Add(cases.Case_CaseId.ToString());//ID

                    list.Add(cases.Case_ReferenceId);//REFERENCE
                    list.Add(cases.Case_Description);//DESCRIPTION
                    list.Add(cases.case_ssdi_demandecomment);//DESCRIPTION PRBLM

                    var agent = "";
                    var salutation = "";
                    if (db.users.Where(a => a.User_UserId == cases.Case_AssignedUserId && a.User_Deleted != 1).Count() != 0)//ASSIGNEE AGENT = REDACTEUR CR
                    {
                        var userAssig = db.users.Where(a => a.User_UserId == cases.Case_AssignedUserId && a.User_Deleted != 1).FirstOrDefault();

                        if (db.Custom_Captions.Where(a => a.Capt_Family == "pers_salutation" && a.Capt_Code == userAssig.user_salutation && a.Capt_Deleted != 1).Count() != 0)
                        {
                            var sal = db.Custom_Captions.Where(a => a.Capt_Family == "pers_salutation" && a.Capt_Code == userAssig.user_salutation && a.Capt_Deleted != 1).FirstOrDefault();
                            salutation = sal.Capt_FR + " ";
                        }
                        else
                        {
                            if (userAssig.user_salutation != null)
                            {
                                if (userAssig.user_salutation.ToLower().Contains("mr"))
                                {
                                    if (userAssig.user_salutation.ToLower() == "mrs.")
                                    {
                                        salutation = "Mme. ";
                                    }
                                    else if (userAssig.user_salutation.ToLower() == "mr.")
                                    {
                                        salutation = "M. ";
                                    }
                                }
                                else if (userAssig.user_salutation.ToLower().Contains("mme"))
                                {
                                    salutation = "Mme. ";
                                }
                                else if (userAssig.user_salutation.ToLower().Contains("miss"))
                                {
                                    salutation = "Mme. ";
                                }
                            }
                        }

                        agent = string.Format("{0}{1} {2}-{3}", salutation, userAssig.User_LastName, userAssig.User_FirstName, userAssig.user_title);

                        //if (userAssig.user_Mailvalidation != null)//MAILVALIDATIONINTERNE//
                        //{
                        //    list.Add(userAssig.user_Mailvalidation);
                        //}
                        //else
                        //    list.Add("");
                    }
                    list.Add(agent);

                    //Modules-Version//
                    string version = "";
                    string version2 = "";
                    string version3 = "";
                    string version4 = "";

                    var caseVers = "";
                    var caseVers2 = "";
                    var caseVers3 = "";
                    var caseVers4 = "";

                    var module = "";
                    var module2 = "";
                    var module3 = "";
                    var module4 = "";
                    if (cases.case_prod1 != null)
                    {
                        if (db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted != 1 && a.Capt_Code == cases.case_prod1).Count() != 0)
                        {
                            var prod1 = db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted != 1 && a.Capt_Code == cases.case_prod1).FirstOrDefault();
                            module = prod1.Capt_FR;
                        }
                        if (cases.case_version != null)
                        {
                            version += cases.case_version;
                        }
                        caseVers = string.Format("{0}-{1}, ", module, version);
                    }
                    if (cases.case_pord2 != null)
                    {
                        if (db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted != 1 && a.Capt_Code == cases.case_pord2).Count() != 0)
                        {
                            var prod2 = db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted != 1 && a.Capt_Code == cases.case_pord2).FirstOrDefault();
                            module2 = prod2.Capt_FR;
                        }
                        if (cases.case_version2 != null)
                        {
                            version2 = cases.case_version2;
                        }
                        caseVers2 = string.Format("{0}-{1}, ", module2, version2);
                    }
                    if (cases.case_prod3 != null)
                    {
                        if (db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted != 1 && a.Capt_Code == cases.case_prod3).Count() != 0)
                        {
                            var prod3 = db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted != 1 && a.Capt_Code == cases.case_prod3).FirstOrDefault();
                            module3 = prod3.Capt_FR;
                        }
                        if (cases.case_version3 != null)
                        {
                            version3 = cases.case_version3;
                        }
                        caseVers3 = string.Format("{0}-{1}, ", module3, version3);
                    }
                    if (cases.case_prod4 != null)
                    {
                        if (db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted != 1 && a.Capt_Code == cases.case_prod4).Count() != 0)
                        {
                            var prod4 = db.Custom_Captions.Where(a => a.Capt_Family == "case_prod1" && a.Capt_Deleted != 1 && a.Capt_Code == cases.case_prod4).FirstOrDefault();
                            module4 = prod4.Capt_FR;
                        }
                        if (cases.case_version4 != null)
                        {
                            version4 = cases.case_version4;
                        }
                        caseVers4 = string.Format("{0}-{1}", module4, version4);
                    }

                    list.Add(caseVers + caseVers2 + caseVers3 + caseVers4);
                    //list.Add(version);

                    var typeInterv = "";
                    var lieux = "";
                    if (db.Communication.Where(a => a.Comm_CaseId == id && a.Comm_Deleted != 1 &&
                        (a.Comm_ToDateTime.Value.Day == dateTest.Day && a.Comm_ToDateTime.Value.Month == dateTest.Month && a.Comm_ToDateTime.Value.Year == dateTest.Year)).Count() != 0)
                    {
                        var com = db.Communication.Where(a => a.Comm_CaseId == id && a.Comm_Deleted != 1 &&
                            (a.Comm_ToDateTime.Value.Day == dateTest.Day && a.Comm_ToDateTime.Value.Month == dateTest.Month && a.Comm_ToDateTime.Value.Year == dateTest.Year)).FirstOrDefault();
                        if (com.Comm_ToDateTime.Value.Date == dateTest)
                        {
                            if (db.Comm_Link.Where(a => a.CmLi_Comm_CommunicationId == com.Comm_CommunicationId && a.CmLi_Deleted != 1).Count() != 0)//CLIENT
                            {
                                var companyT = db.Comm_Link.Where(a => a.CmLi_Comm_CommunicationId == com.Comm_CommunicationId && a.CmLi_Deleted != 1).FirstOrDefault();
                                if (db.Company.Where(a => a.Comp_CompanyId == companyT.CmLi_Comm_CompanyId && a.Comp_Deleted != 1).Count() != 0)
                                {
                                    var companyFirst = db.Company.Where(a => a.Comp_CompanyId == companyT.CmLi_Comm_CompanyId && a.Comp_Deleted != 1).FirstOrDefault();

                                    idComp = companyFirst.Comp_CompanyId;

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

                            if (com.comm_typepresta != null)//Type de prestation
                            {
                                if (db.Custom_Captions.Where(a => a.Capt_Family == "comm_typepresta" && a.Capt_Code == com.comm_typepresta && a.Capt_Deleted != 1).Count() != 0)
                                {
                                    typeInterv = db.Custom_Captions.Where(a => a.Capt_Family == "comm_typepresta" && a.Capt_Code == com.comm_typepresta && a.Capt_Deleted != 1).FirstOrDefault().Capt_FR;
                                }
                            }

                            if (com.comm_lieu != null)//Lieu//
                            {
                                lieux = com.comm_lieu;
                            }
                        }
                    }

                    list.Add(client);
                    list.Add(typeInterv);
                    list.Add(lieux);

                    DateTime dateInterv = DateTime.Parse(dateCom).Date;
                    list.Add(dateInterv.ToShortDateString());//Date intervention
                }

                var heureDeb = "";
                var heureFin = "";

                //COMMUNICATION//
                if (db.Communication.Where(a => a.Comm_CaseId == id && a.Comm_Deleted != 1 &&
                    (a.Comm_ToDateTime.Value.Day == dateTest.Day && a.Comm_ToDateTime.Value.Month == dateTest.Month && a.Comm_ToDateTime.Value.Year == dateTest.Year)).Count() != 0)
                {
                    var commForTest = db.Communication.Where(a => a.Comm_CaseId == id && a.Comm_Deleted != 1 &&
                        (a.Comm_ToDateTime.Value.Day == dateTest.Day && a.Comm_ToDateTime.Value.Month == dateTest.Month && a.Comm_ToDateTime.Value.Year == dateTest.Year)).FirstOrDefault();

                    var heureForTESTD = commForTest.Comm_ToDateTime;
                    var heureForTESTF = commForTest.comm_datefin2;

                    foreach (var x in db.Communication.Where(a => a.Comm_CaseId == id && a.Comm_Deleted != 1 &&
                        (a.Comm_ToDateTime.Value.Day == dateTest.Day && a.Comm_ToDateTime.Value.Month == dateTest.Month && a.Comm_ToDateTime.Value.Year == dateTest.Year)).ToList())
                    {
                        var listTache = new List<string>();
                        var listPlanAction = new List<string>();

                        if (x.Comm_ToDateTime.Value.Date == dateTest)
                        {
                            //ID//
                            listTache.Add(x.Comm_CommunicationId.ToString());

                            //TACHES//
                            if (x.Comm_Subject != null)//Thème
                            {
                                listTache.Add(x.Comm_Subject);
                            }
                            else
                                listTache.Add(""); //listTache.Add("Thème non renseigné");

                            if (x.comm_probleme != null)//Probleme-Besoin
                            {
                                listTache.Add(x.comm_probleme);
                            }
                            else
                                listTache.Add(""); //listTache.Add("Problèmes-Besoins non renseignés");

                            if (x.Comm_Note != null)//Tache
                            {
                                listTache.Add(x.Comm_Note);
                            }
                            else
                                listTache.Add(""); //listTache.Add("Tâches non renseignées");

                            if (x.Comm_ToDateTime != null && x.comm_datefin2 != null)//Durée
                            {
                                var dure = (x.comm_datefin2 - x.Comm_ToDateTime);
                                listTache.Add(dure.Value.ToString());
                            }

                            if (x.comm_obs1 != null)//Observation
                            {
                                listTache.Add(x.comm_obs1);
                            }
                            else
                                listTache.Add("");

                            //Intervenants SOFTWELL et CONSULTANT intervenants
                            var interv = "";
                            string[] separators = { "," };

                            if (x.comm_userintervenant != null)//Agents
                            {
                                string listUser = x.comm_userintervenant.ToString();

                                string[] agent = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                foreach (var ag in agent)
                                {
                                    var listConsultant_Intervenant = new List<string>();
                                    int idA = int.Parse(ag);
                                    var salutation = "";
                                    if (db.users.Where(a => a.User_UserId == idA && a.User_Deleted != 1).Count() != 0)
                                    {
                                        var userAssig = db.users.Where(a => a.User_UserId == idA && a.User_Deleted != 1).FirstOrDefault();

                                        if (db.Custom_Captions.Where(a => a.Capt_Family == "pers_salutation" && a.Capt_Code == userAssig.user_salutation && a.Capt_Deleted != 1).Count() != 0)
                                        {
                                            var sal = db.Custom_Captions.Where(a => a.Capt_Family == "pers_salutation" && a.Capt_Code == userAssig.user_salutation && a.Capt_Deleted != 1).FirstOrDefault();
                                            salutation = sal.Capt_FR + " ";
                                        }
                                        else
                                        {
                                            if (userAssig.user_salutation != null)
                                            {
                                                if (userAssig.user_salutation.ToLower().Contains("mr"))
                                                {
                                                    if (userAssig.user_salutation.ToLower() == "mrs.")
                                                    {
                                                        salutation = "Mme. ";
                                                    }
                                                    else if (userAssig.user_salutation.ToLower() == "mr.")
                                                    {
                                                        salutation = "M. ";
                                                    }
                                                }
                                                else if (userAssig.user_salutation.ToLower().Contains("mme"))
                                                {
                                                    salutation = "Mme. ";
                                                }
                                                else if (userAssig.user_salutation.ToLower().Contains("miss"))
                                                {
                                                    salutation = "Mme. ";
                                                }
                                            }
                                        }

                                        interv += string.Format("{0}{1} {2}-{3}, ", salutation, userAssig.User_LastName, userAssig.User_FirstName, userAssig.user_title);

                                        listConsultant_Intervenant.Add(string.Format("{0}{1} {2}-{3}\r\n", salutation, userAssig.User_LastName, userAssig.User_FirstName, userAssig.user_title));
                                    }

                                    if (listC.Contains(listConsultant_Intervenant.FirstOrDefault()) == false)
                                    {
                                        listC.AddRange(listConsultant_Intervenant);
                                    }
                                }
                            }

                            //Status//
                            var stat = "";
                            if (x.Comm_Status != null)
                            {
                                if (db.Custom_Captions.Where(a => a.Capt_Family.ToLower() == "comm_status" && a.Capt_Code == x.Comm_Status && a.Capt_Deleted != 1).Count() != 0)
                                {
                                    var sta = db.Custom_Captions.Where(a => a.Capt_Family.ToLower() == "comm_status" && a.Capt_Code == x.Comm_Status && a.Capt_Deleted != 1).FirstOrDefault();
                                    stat = sta.Capt_FR;
                                }
                            }
                            listTache.Add(stat);

                            listTache.Add("SOFTWELL: " + interv);

                            //Intervenant Client//
                            //FONCTION GET INTERVEANT CLEINT PAR TACHE//
                            var cltPRESENT = "";
                            if (db.Comm_Link.Where(a => a.CmLi_Comm_CommunicationId == x.Comm_CommunicationId && a.CmLi_IsExternalAttendee == "Y"
                                && a.CmLi_ExternalPersonID != null && a.CmLi_Deleted != 1).Count() != 0)
                            {
                                foreach (var clPr in db.Comm_Link.Where(a => a.CmLi_Comm_CommunicationId == x.Comm_CommunicationId && a.CmLi_IsExternalAttendee == "Y"
                                     && a.CmLi_ExternalPersonID != null && a.CmLi_Deleted != 1).ToList())
                                {
                                    var listClientNoms = new List<string>();
                                    var listClientMails = new List<string>();

                                    if (db.Person.Where(a => a.Pers_PersonId == clPr.CmLi_ExternalPersonID).Count() != 0)
                                    {
                                        var pers = db.Person.Where(a => a.Pers_PersonId == clPr.CmLi_ExternalPersonID).FirstOrDefault();

                                        if (db.vPerson.Where(a => a.Pers_PersonId == pers.Pers_PersonId).Count() != 0)
                                        {
                                            //mailCLIENTDEST += db.vPerson.Where(a => a.Pers_PersonId == pers.Pers_PersonId).FirstOrDefault().Pers_EmailAddress + ",";
                                            listClientMails.Add(db.vPerson.Where(a => a.Pers_PersonId == pers.Pers_PersonId).FirstOrDefault().Pers_EmailAddress + ",");
                                        }

                                        var salutation = "";
                                        if (db.Custom_Captions.Where(a => a.Capt_Family.ToLower() == "pers_salutation" && a.Capt_Code == pers.Pers_Salutation && a.Capt_Deleted != 1).Count() != 0)
                                        {
                                            var sal = db.Custom_Captions.Where(a => a.Capt_Family.ToLower() == "pers_salutation" && a.Capt_Code == pers.Pers_Salutation && a.Capt_Deleted != 1).FirstOrDefault();
                                            salutation = sal.Capt_FR + " ";
                                        }
                                        else
                                        {
                                            if (pers.Pers_Salutation != null)
                                            {
                                                if (pers.Pers_Salutation.ToLower().Contains("mr"))
                                                {
                                                    if (pers.Pers_Salutation.ToLower() == "mrs.")
                                                    {
                                                        salutation = "Mme. ";
                                                    }
                                                    else if (pers.Pers_Salutation.ToLower() == "mr.")
                                                    {
                                                        salutation = "M. ";
                                                    }
                                                }
                                                else if (pers.Pers_Salutation.ToLower().Contains("mme"))
                                                {
                                                    salutation = "Mme. ";
                                                }
                                                else if (pers.Pers_Salutation.ToLower().Contains("miss"))
                                                {
                                                    salutation = "Mme. ";
                                                }
                                            }
                                        }

                                        cltPRESENT += string.Format("{0}{1} {2}-{3}, ", salutation, pers.Pers_LastName, pers.Pers_FirstName, pers.Pers_Title);
                                        listClientNoms.Add(string.Format("{0}{1} {2}-{3}", salutation, pers.Pers_LastName, pers.Pers_FirstName, pers.Pers_Title));
                                    }

                                    if (listCN.Contains(listClientNoms.FirstOrDefault()) == false)
                                    {
                                        listCN.AddRange(listClientNoms);
                                    }

                                    if (listCM.Contains(listClientMails.FirstOrDefault()) == false)
                                    {
                                        listCM.AddRange(listClientMails);
                                    }
                                }
                            }

                            listTache.Add(string.Format("{0}: {1}", client.ToUpper(), cltPRESENT));

                            //Plan d'action//&& a.CmLi_ExternalPersonID != null && a.CmLi_Deleted != 1
                            //if (x.comm_planid != null)
                            //{
                            //    if (db.PLANDACTION.Where(a => a.plan_PLANDACTIONid == x.comm_planid && a.plan_Deleted != 1).Count() != 0)
                            //    {
                            //        var planAct = db.PLANDACTION.Where(a => a.plan_PLANDACTIONid == x.comm_planid && a.plan_Deleted != 1).FirstOrDefault();

                            //        //ID//
                            //        listPlanAction.Add(planAct.plan_PLANDACTIONid.ToString());

                            //        listPlanAction.Add(planAct.plan_plandaction);//OBJET-TACHES

                            //        //INTERVENANT SOFT
                            //        var intervSoft = "";
                            //        if (planAct.plan_intervesoftw != null)
                            //        {
                            //            string listUser = planAct.plan_intervesoftw.ToString();

                            //            string[] agent = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                            //            foreach (var ag in agent)
                            //            {
                            //                int idA = int.Parse(ag);
                            //                var salutation = "";
                            //                if (db.users.Where(a => a.User_UserId == idA && a.User_Deleted != 1).Count() != 0)
                            //                {
                            //                    var userAssig = db.users.Where(a => a.User_UserId == idA && a.User_Deleted != 1).FirstOrDefault();

                            //                    if (db.Custom_Captions.Where(a => a.Capt_Family == "pers_salutation" && a.Capt_Code == userAssig.user_salutation && a.Capt_Deleted != 1).Count() != 0)
                            //                    {
                            //                        var sal = db.Custom_Captions.Where(a => a.Capt_Family == "pers_salutation" && a.Capt_Code == userAssig.user_salutation && a.Capt_Deleted != 1).FirstOrDefault();
                            //                        salutation = sal.Capt_FR + " ";
                            //                    }
                            //                    else
                            //                    {
                            //                        if (userAssig.user_salutation.ToLower().Contains("mr"))
                            //                        {
                            //                            if (userAssig.user_salutation.ToLower() == "mrs.")
                            //                            {
                            //                                salutation = "Mme. ";
                            //                            }
                            //                            else if (userAssig.user_salutation.ToLower() == "mr.")
                            //                            {
                            //                                salutation = "M. ";
                            //                            }
                            //                        }
                            //                        else if (userAssig.user_salutation.ToLower().Contains("mme"))
                            //                        {
                            //                            salutation = "Mme. ";
                            //                        }
                            //                        else if (userAssig.user_salutation.ToLower().Contains("miss"))
                            //                        {
                            //                            salutation = "Mme. ";
                            //                        }
                            //                    }

                            //                    intervSoft = salutation + userAssig.User_LastName + " " + userAssig.User_FirstName + "-" + userAssig.user_title + ",";
                            //                }
                            //            }
                            //        }

                            //        listPlanAction.Add("SOFTWELL: " + intervSoft);//Intervenant SOFT//

                            //        //Intervenant client
                            //        listPlanAction.Add(client.ToUpper() + ": " + planAct.plan_intervclient);

                            //        //Echeance//
                            //        listPlanAction.Add(planAct.plan_datef.Value.ToShortDateString());
                            //    }
                            //}
                            if (db.PLANDACTION.Where(a => a.plan_comm_communicationid == x.Comm_CommunicationId && a.plan_Deleted != 1).Count() != 0)
                            {
                                var planAct = db.PLANDACTION.Where(a => a.plan_comm_communicationid == x.Comm_CommunicationId && a.plan_Deleted != 1).FirstOrDefault();

                                //ID//
                                listPlanAction.Add(planAct.plan_PLANDACTIONid.ToString());

                                listPlanAction.Add(planAct.plan_plandaction);//OBJET-TACHES

                                //INTERVENANT SOFT
                                var intervSoft = "";
                                if (planAct.plan_intervesoftw != null)
                                {
                                    string listUser = planAct.plan_intervesoftw.ToString();

                                    string[] agent = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                    foreach (var ag in agent)
                                    {
                                        int idA = int.Parse(ag);
                                        var salutation = "";
                                        if (db.users.Where(a => a.User_UserId == idA && a.User_Deleted != 1).Count() != 0)
                                        {
                                            var userAssig = db.users.Where(a => a.User_UserId == idA && a.User_Deleted != 1).FirstOrDefault();

                                            if (db.Custom_Captions.Where(a => a.Capt_Family == "pers_salutation" && a.Capt_Code == userAssig.user_salutation && a.Capt_Deleted != 1).Count() != 0)
                                            {
                                                var sal = db.Custom_Captions.Where(a => a.Capt_Family == "pers_salutation" && a.Capt_Code == userAssig.user_salutation && a.Capt_Deleted != 1).FirstOrDefault();
                                                salutation = sal.Capt_FR + " ";
                                            }
                                            else
                                            {
                                                if (userAssig.user_salutation != null)
                                                {
                                                    if (userAssig.user_salutation.ToLower().Contains("mr"))
                                                    {
                                                        if (userAssig.user_salutation.ToLower() == "mrs.")
                                                        {
                                                            salutation = "Mme. ";
                                                        }
                                                        else if (userAssig.user_salutation.ToLower() == "mr.")
                                                        {
                                                            salutation = "M. ";
                                                        }
                                                    }
                                                    else if (userAssig.user_salutation.ToLower().Contains("mme"))
                                                    {
                                                        salutation = "Mme. ";
                                                    }
                                                    else if (userAssig.user_salutation.ToLower().Contains("miss"))
                                                    {
                                                        salutation = "Mme. ";
                                                    }
                                                }
                                            }

                                            intervSoft = string.Format("{0}{1} {2}-{3},", salutation, userAssig.User_LastName, userAssig.User_FirstName, userAssig.user_title);
                                        }
                                    }
                                }

                                listPlanAction.Add("SOFTWELL: " + intervSoft);//Intervenant SOFT//

                                //Intervenant client
                                listPlanAction.Add(string.Format("{0}: {1}", client.ToUpper(), planAct.plan_intervclient));

                                //Echeance//
                                if (planAct.plan_datef != null)
                                {
                                    listPlanAction.Add(planAct.plan_datef.Value.ToShortDateString());
                                }
                                else
                                {
                                    listPlanAction.Add("");
                                }
                            }


                            //Heure Début-Fin//
                            if (x.Comm_ToDateTime != null)//Deb
                            {
                                if (x.Comm_ToDateTime < heureForTESTD)
                                {
                                    heureDeb = x.Comm_ToDateTime.Value.ToString("HH:mm");
                                }
                                else
                                {
                                    heureDeb = heureForTESTD.Value.ToString("HH:mm");
                                }
                            }
                            if (x.comm_datefin2 != null)//Fin
                            {
                                if (x.comm_datefin2 > heureForTESTF)
                                {
                                    heureFin = x.comm_datefin2.Value.ToString("HH:mm");
                                }
                                else
                                {
                                    heureFin = heureForTESTF.Value.ToString("HH:mm");
                                }
                            }
                            listTaches.Add(listTache);
                            listPlanings.Add(listPlanAction);
                        }
                    }
                }

                list.Add(heureDeb);
                list.Add(heureFin);

                //TEST SUR LES NOMS ET MAILS CLIENT A ENVOYER//
                if (db.Crmcli_CR.Where(a => a.ID_CASES == id && a.DATE.Value.Day == dateTest.Day && a.DATE.Value.Month == dateTest.Month
                    && a.DATE.Value.Year == dateTest.Year).Count() != 0)
                {
                    var listClientNoms = new List<string>();
                    var listClientMails = new List<string>();

                    listCN = new List<string>();

                    listCM = new List<string>();

                    var saveCR = db.Crmcli_CR.Where(a => a.ID_CASES == id && a.DATE.Value.Day == dateTest.Day && a.DATE.Value.Month == dateTest.Month
                    && a.DATE.Value.Year == dateTest.Year).FirstOrDefault();

                    listClientNoms.Add(saveCR.DESTCLIENT);
                    listClientMails.Add(saveCR.DESTMAIL);

                    listCN.AddRange(listClientNoms);
                    listCM.AddRange(listClientMails);

                    //DOCREMIS//
                    list.Add(saveCR.DOCREM);
                }
                else
                {
                    if (id != 0)
                    {
                        if (db.Company.Where(a => a.Comp_CompanyId == idComp && a.Comp_Deleted != 1).Count() != 0)
                        {
                            if (db.Person.Where(a => a.Pers_CompanyId == idComp && a.pers_ssdi_estcontactn2 == "Y").Count() != 0)
                            {
                                foreach (var per in db.Person.Where(a => a.Pers_CompanyId == idComp && a.pers_ssdi_estcontactn2 == "Y").ToList())
                                {
                                    var listClientNoms = new List<string>();
                                    var listClientMails = new List<string>();

                                    var pers = db.Person.Where(a => a.Pers_PersonId == per.Pers_PersonId && a.pers_ssdi_estcontactn2 == "Y").FirstOrDefault();

                                    if (db.vPerson.Where(a => a.Pers_PersonId == pers.Pers_PersonId).Count() != 0)
                                    {
                                        //mailCLIENTDEST += db.vPerson.Where(a => a.Pers_PersonId == pers.Pers_PersonId).FirstOrDefault().Pers_EmailAddress + ",";
                                        listClientMails.Add(db.vPerson.Where(a => a.Pers_PersonId == pers.Pers_PersonId).FirstOrDefault().Pers_EmailAddress + ",");
                                    }

                                    var salutation = "";
                                    if (db.Custom_Captions.Where(a => a.Capt_Family.ToLower() == "pers_salutation" && a.Capt_Code == pers.Pers_Salutation && a.Capt_Deleted != 1).Count() != 0)
                                    {
                                        var sal = db.Custom_Captions.Where(a => a.Capt_Family.ToLower() == "pers_salutation" && a.Capt_Code == pers.Pers_Salutation && a.Capt_Deleted != 1).FirstOrDefault();
                                        salutation = sal.Capt_FR + " ";
                                    }
                                    else
                                    {
                                        if (pers.Pers_Salutation != null)
                                        {
                                            if (pers.Pers_Salutation.ToLower().Contains("mr"))
                                            {
                                                if (pers.Pers_Salutation.ToLower() == "mrs.")
                                                {
                                                    salutation = "Mme. ";
                                                }
                                                else if (pers.Pers_Salutation.ToLower() == "mr.")
                                                {
                                                    salutation = "M. ";
                                                }
                                            }
                                            else if (pers.Pers_Salutation.ToLower().Contains("mme"))
                                            {
                                                salutation = "Mme. ";
                                            }
                                            else if (pers.Pers_Salutation.ToLower().Contains("miss"))
                                            {
                                                salutation = "Mme. ";
                                            }
                                        }
                                    }
                                    //cltPRESENT += salutation + pers.Pers_LastName + " " + pers.Pers_FirstName + "-" + pers.Pers_Title + ", ";
                                    listClientNoms.Add(string.Format("{0}{1} {2}-{3}", salutation, pers.Pers_LastName, pers.Pers_FirstName, pers.Pers_Title));

                                    if (listCN.Contains(listClientNoms.FirstOrDefault()) == false)
                                    {
                                        listCN.AddRange(listClientNoms);
                                    }

                                    if (listCM.Contains(listClientMails.FirstOrDefault()) == false)
                                    {
                                        listCM.AddRange(listClientMails);
                                    }
                                }
                            }
                        }

                        /////////////////////////////////////////////////////////////////////////////
                        if (db.Communication.Where(a => a.Comm_CaseId == id && a.Comm_Deleted != 1 &&
                        (a.Comm_ToDateTime.Value.Day == dateTest.Day && a.Comm_ToDateTime.Value.Month == dateTest.Month && a.Comm_ToDateTime.Value.Year == dateTest.Year)).Count() != 0)
                        {
                            foreach (var x in db.Communication.Where(a => a.Comm_CaseId == id && a.Comm_Deleted != 1 &&
                                (a.Comm_ToDateTime.Value.Day == dateTest.Day && a.Comm_ToDateTime.Value.Month == dateTest.Month && a.Comm_ToDateTime.Value.Year == dateTest.Year)).ToList())
                            {
                                var zzzz = db.Comm_Link.Where(a => a.CmLi_Comm_CommunicationId == x.Comm_CommunicationId && a.CmLi_IsExternalAttendee == "Y" && a.CmLi_ExternalPersonID != null && a.CmLi_Deleted != 1
                            && a.CmLi_ExternalPersonID != null && a.CmLi_Deleted != 1).Count() != 0;
                                if (db.Comm_Link.Where(a => a.CmLi_Comm_CommunicationId == x.Comm_CommunicationId && a.CmLi_IsExternalAttendee == "Y"
                            && a.CmLi_ExternalPersonID != null && a.CmLi_Deleted != 1).Count() != 0)
                                {
                                    foreach (var clPr in db.Comm_Link.Where(a => a.CmLi_Comm_CommunicationId == x.Comm_CommunicationId && a.CmLi_IsExternalAttendee == "Y"
                                         && a.CmLi_ExternalPersonID != null && a.CmLi_Deleted != 1).ToList())
                                    {
                                        var listClientNoms = new List<string>();
                                        var listClientMails = new List<string>();

                                        if (db.Person.Where(a => a.Pers_PersonId == clPr.CmLi_ExternalPersonID).Count() != 0)
                                        {
                                            var pers = db.Person.Where(a => a.Pers_PersonId == clPr.CmLi_ExternalPersonID).FirstOrDefault();

                                            if (db.vPerson.Where(a => a.Pers_PersonId == pers.Pers_PersonId).Count() != 0)
                                            {
                                                //mailCLIENTDEST += db.vPerson.Where(a => a.Pers_PersonId == pers.Pers_PersonId).FirstOrDefault().Pers_EmailAddress + ",";
                                                listClientMails.Add(db.vPerson.Where(a => a.Pers_PersonId == pers.Pers_PersonId).FirstOrDefault().Pers_EmailAddress + ",");
                                            }

                                            var salutation = "";
                                            if (db.Custom_Captions.Where(a => a.Capt_Family.ToLower() == "pers_salutation" && a.Capt_Code == pers.Pers_Salutation && a.Capt_Deleted != 1).Count() != 0)
                                            {
                                                var sal = db.Custom_Captions.Where(a => a.Capt_Family.ToLower() == "pers_salutation" && a.Capt_Code == pers.Pers_Salutation && a.Capt_Deleted != 1).FirstOrDefault();
                                                salutation = sal.Capt_FR + " ";
                                            }
                                            else
                                            {
                                                if (pers.Pers_Salutation != null)
                                                {
                                                    if (pers.Pers_Salutation.ToLower().Contains("mr"))
                                                    {
                                                        if (pers.Pers_Salutation.ToLower() == "mrs.")
                                                        {
                                                            salutation = "Mme. ";
                                                        }
                                                        else if (pers.Pers_Salutation.ToLower() == "mr.")
                                                        {
                                                            salutation = "M. ";
                                                        }
                                                    }
                                                    else if (pers.Pers_Salutation.ToLower().Contains("mme"))
                                                    {
                                                        salutation = "Mme. ";
                                                    }
                                                    else if (pers.Pers_Salutation.ToLower().Contains("miss"))
                                                    {
                                                        salutation = "Mme. ";
                                                    }
                                                }
                                            }
                                            listClientNoms.Add(string.Format("{0}{1} {2}-{3}", salutation, pers.Pers_LastName, pers.Pers_FirstName, pers.Pers_Title));
                                        }

                                        if (listCN.Contains(listClientNoms.FirstOrDefault()) == false)
                                        {
                                            listCN.AddRange(listClientNoms);
                                        }

                                        if (listCM.Contains(listClientMails.FirstOrDefault()) == false)
                                        {
                                            listCM.AddRange(listClientMails);
                                        }
                                    }
                                }

                                //ADD DEST CLIENT + MAIL si pers_ssdi_estcontactn2 == "Y" (contact OBLIGATOIRE)
                                if (db.Person.Where(a => a.Pers_CompanyId == idComp).Count() != 0)
                                {
                                    var listClientNoms = new List<string>();
                                    var listClientMails = new List<string>();
                                    var ssss = db.Person.Where(a => a.Pers_CompanyId == idComp && a.pers_ssdi_estcontactn2 == "Y").ToList();
                                    foreach (var xy in db.Person.Where(a => a.Pers_CompanyId == idComp && a.pers_ssdi_estcontactn2 == "Y").ToList())//Mila atao Y ny any anaty base
                                    {
                                        if (db.vPerson.Where(a => a.Pers_PersonId == xy.Pers_PersonId).Count() != 0)
                                        {
                                            //mailCLIENTDEST += db.vPerson.Where(a => a.Pers_PersonId == pers.Pers_PersonId).FirstOrDefault().Pers_EmailAddress + ",";
                                            listClientMails.Add(db.vPerson.Where(a => a.Pers_PersonId == xy.Pers_PersonId).FirstOrDefault().Pers_EmailAddress + ",");
                                        }

                                        var salutation = "";
                                        if (db.Custom_Captions.Where(a => a.Capt_Family.ToLower() == "pers_salutation" && a.Capt_Code == xy.Pers_Salutation && a.Capt_Deleted != 1).Count() != 0)
                                        {
                                            var sal = db.Custom_Captions.Where(a => a.Capt_Family.ToLower() == "pers_salutation" && a.Capt_Code == xy.Pers_Salutation && a.Capt_Deleted != 1).FirstOrDefault();
                                            salutation = sal.Capt_FR + " ";
                                        }
                                        else
                                        {
                                            if (xy.Pers_Salutation != null)
                                            {
                                                if (xy.Pers_Salutation.ToLower().Contains("mr"))
                                                {
                                                    if (xy.Pers_Salutation.ToLower() == "mrs.")
                                                    {
                                                        salutation = "Mme. ";
                                                    }
                                                    else if (xy.Pers_Salutation.ToLower() == "mr.")
                                                    {
                                                        salutation = "M. ";
                                                    }
                                                }
                                                else if (xy.Pers_Salutation.ToLower().Contains("mme"))
                                                {
                                                    salutation = "Mme. ";
                                                }
                                                else if (xy.Pers_Salutation.ToLower().Contains("miss"))
                                                {
                                                    salutation = "Mme. ";
                                                }
                                            }
                                        }
                                        listClientNoms.Add(string.Format("{0}{1} {2}-{3}", salutation, xy.Pers_LastName, xy.Pers_FirstName, xy.Pers_Title));
                                    }

                                    if (listCN.Contains(listClientNoms.FirstOrDefault()) == false)
                                    {
                                        listCN.AddRange(listClientNoms);
                                    }

                                    if (listCM.Contains(listClientMails.FirstOrDefault()) == false)
                                    {
                                        listCM.AddRange(listClientMails);
                                    }
                                }

                            }
                        }
                    }

                    //DOCREMIS//
                    list.Add("");
                }

                listCases.Add(list);
            }

            var listDa = new List<string>();
            var dateInterne = "Non envoyé";
            var dateclient = "Non envoyé";
            if (db.Crmcli_EnvoiMails.Where(a => a.Case_CaseId == id && a.CommDATE.Value.Day == dateTest.Day
                && a.CommDATE.Value.Month == dateTest.Month && a.CommDATE.Value.Year == dateTest.Year).Count() != 0)
            {
                var fordate = db.Crmcli_EnvoiMails.Where(a => a.Case_CaseId == id && a.CommDATE.Value.Day == dateTest.Day
                && a.CommDATE.Value.Month == dateTest.Month && a.CommDATE.Value.Year == dateTest.Year).FirstOrDefault();
                if (fordate.DateClient != null)
                {
                    dateclient = fordate.DateClient.Value.ToShortDateString();
                }
                if (fordate.DateInterne != null)
                {
                    dateInterne = fordate.DateInterne.Value.ToShortDateString();
                }
            }
            listDa.Add(dateInterne);
            listDa.Add(dateclient);

            listDATE.Add(listDa);

            var lisdaT = listDATE;


            var lisCN = listCN;
            var lisCM = listCM;
            var lis = listCases;
            var lisT = listTaches;
            var lisP = listPlanings;
            //var lisCon = listConsultantIntervenants;
            var lisCon = listC;

            ViewBag.lisDA = lisdaT;
            ViewBag.lis = lis;
            ViewBag.lisT = lisT;
            ViewBag.lisP = lisP;
            ViewBag.lisCon = lisCon;

            ViewBag.lisCN = lisCN;
            ViewBag.lisCM = lisCM;

            return View();
        }

        private void CreatWordDoc(string filename,
           int id, string client, string type, string abrev, string module, string date, string hdeb, string hfin, string lieux,
            string sujet, string docrem, string docannex, string attachement, string destCLT, string destMAIL/*, string CONSULTINTERV*/)
        {
            string nameDir = "";
            if (Session["UserId"] != null)
            {
                nameDir = Session["UserId"].ToString();
            }

            Session["FILENAME"] = filename;
            try
            {
                var newPath = Server.MapPath("~/CRS/WTEMPLATEX.unzipped" + nameDir);

                if (System.IO.File.Exists(Server.MapPath(string.Format("~/TEMPLATES/TEMPCRS/WTEMPLATEX{0}.zip", nameDir))) == false)
                {
                    string sourceFile = Server.MapPath("~/WTEMPLATEX.zip");
                    string destinationFile = Server.MapPath(string.Format("~/TEMPLATES/TEMPCRS/WTEMPLATEX{0}.zip", nameDir));
                    System.IO.File.Copy(sourceFile, destinationFile);
                }

                using (ZipArchive archive = ZipFile.OpenRead(Server.MapPath(string.Format("~/TEMPLATES/TEMPCRS/WTEMPLATEX{0}.zip", nameDir))))
                {
                    if (Directory.Exists(newPath))
                    {
                        Directory.Delete(newPath, true);
                        archive.ExtractToDirectory(newPath);
                    }
                    else
                        archive.ExtractToDirectory(newPath);
                }

                string path = Path.Combine(newPath + @"\word\document.xml");

                string str = System.IO.File.ReadAllText(path);

                var TACHES = "";
                var PLANS = "";

                //FIND/REPLACE => CORPS
                str = str.Replace("VALDATE", date);

                var TTy = "";
                char c = char.Parse(type.Substring(0, 1));
                bool isVowel = "aeiouAEIOU".IndexOf(c) >= 0;
                if (isVowel == true)
                    TTy = "d'" + type;
                else
                    TTy = "de " + type;

                str = str.Replace("VALTYPE", TTy);
                str = str.Replace("VALABRV", abrev);

                str = str.Replace("VALDEB", hdeb);
                str = str.Replace("VALFIN", hfin);
                str = str.Replace("VALCLIENT", client);
                str = str.Replace("VALANNEXE", docannex);
                str = str.Replace("VALREMIS", docrem);

                str = str.Replace("LIVAL", lieux);

                str = str.Replace("CLVP", destCLT);

                List<string> listC = new List<string>();
                var TotalConsultant = "";

                List<string> listSOFT = new List<string>();
                var TotalSOFT = "";

                List<List<string>> listTaches = new List<List<string>>();
                List<List<string>> listPlannings = new List<List<string>>();

                //REDACTEUR//
                var agent = "";
                var demNum = "";
                if (id != 0)
                {
                    //CASES//
                    if (db.Cases.Where(a => a.Case_CaseId == id && a.Case_Deleted != 1).Count() != 0)
                    {
                        var cases = db.Cases.Where(a => a.Case_CaseId == id && a.Case_Deleted != 1).FirstOrDefault();

                        if (cases.case_numdemande != null)
                        {
                            demNum = cases.case_numdemande;
                        }

                        var salutation = "";
                        if (db.users.Where(a => a.User_UserId == cases.Case_AssignedUserId && a.User_Deleted != 1).Count() != 0)//ASSIGNEE AGENT = REDACTEUR CR
                        {
                            var userAssig = db.users.Where(a => a.User_UserId == cases.Case_AssignedUserId && a.User_Deleted != 1).FirstOrDefault();

                            if (db.Custom_Captions.Where(a => a.Capt_Family == "pers_salutation" && a.Capt_Code == userAssig.user_salutation && a.Capt_Deleted != 1).Count() != 0)
                            {
                                var sal = db.Custom_Captions.Where(a => a.Capt_Family == "pers_salutation" && a.Capt_Code == userAssig.user_salutation && a.Capt_Deleted != 1).FirstOrDefault();
                                salutation = sal.Capt_FR + " ";
                            }
                            else
                            {
                                if (userAssig.user_salutation != null)
                                {
                                    if (userAssig.user_salutation.ToLower().Contains("mr"))
                                    {
                                        if (userAssig.user_salutation.ToLower() == "mrs.")
                                        {
                                            salutation = "Mme. ";
                                        }
                                        else if (userAssig.user_salutation.ToLower() == "mr.")
                                        {
                                            salutation = "M. ";
                                        }
                                    }
                                    else if (userAssig.user_salutation.ToLower().Contains("mme"))
                                    {
                                        salutation = "Mme. ";
                                    }
                                    else if (userAssig.user_salutation.ToLower().Contains("miss"))
                                    {
                                        salutation = "Mme. ";
                                    }
                                }
                            }

                            agent = string.Format("{0}{1} {2}-{3}", salutation, userAssig.User_LastName, userAssig.User_FirstName, userAssig.user_title);
                        }

                        //VALCONSUSOFT = total des consultants SOFTWELL//
                        //COMMUNICATION//
                        DateTime dateTest = DateTime.Parse(date).Date;

                        if (db.Communication.Where(a => a.Comm_CaseId == id && a.Comm_Deleted != 1 &&
                            (a.Comm_ToDateTime.Value.Day == dateTest.Day && a.Comm_ToDateTime.Value.Month == dateTest.Month && a.Comm_ToDateTime.Value.Year == dateTest.Year)).Count() != 0)
                        {
                            foreach (var x in db.Communication.Where(a => a.Comm_CaseId == id && a.Comm_Deleted != 1 &&
                                (a.Comm_ToDateTime.Value.Day == dateTest.Day && a.Comm_ToDateTime.Value.Month == dateTest.Month && a.Comm_ToDateTime.Value.Year == dateTest.Year)).ToList())
                            {
                                var listTache = new List<string>();
                                var listPlanAction = new List<string>();

                                if (x.Comm_ToDateTime.Value.Date == dateTest)
                                {
                                    //Intervenants SOFTWELL et CONSULTANT intervenants
                                    string[] separators = { "," };

                                    if (x.comm_userintervenant != null)//Agents
                                    {
                                        string listUser = x.comm_userintervenant.ToString();

                                        string[] agentL = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                        foreach (var ag in agentL)
                                        {
                                            var listConsultant_Intervenant = new List<string>();
                                            int idA = int.Parse(ag);
                                            salutation = "";
                                            if (db.users.Where(a => a.User_UserId == idA && a.User_Deleted != 1).Count() != 0)
                                            {
                                                var userAssig = db.users.Where(a => a.User_UserId == idA && a.User_Deleted != 1).FirstOrDefault();

                                                if (db.Custom_Captions.Where(a => a.Capt_Family == "pers_salutation" && a.Capt_Code == userAssig.user_salutation && a.Capt_Deleted != 1).Count() != 0)
                                                {
                                                    var sal = db.Custom_Captions.Where(a => a.Capt_Family == "pers_salutation" && a.Capt_Code == userAssig.user_salutation && a.Capt_Deleted != 1).FirstOrDefault();
                                                    salutation = sal.Capt_FR + " ";
                                                }
                                                else
                                                {
                                                    if (userAssig.user_salutation != null)
                                                    {
                                                        if (userAssig.user_salutation.ToLower().Contains("mr"))
                                                        {
                                                            if (userAssig.user_salutation.ToLower() == "mrs.")
                                                            {
                                                                salutation = "Mme. ";
                                                            }
                                                            else if (userAssig.user_salutation.ToLower() == "mr.")
                                                            {
                                                                salutation = "M. ";
                                                            }
                                                        }
                                                        else if (userAssig.user_salutation.ToLower().Contains("mme"))
                                                        {
                                                            salutation = "Mme. ";
                                                        }
                                                        else if (userAssig.user_salutation.ToLower().Contains("miss"))
                                                        {
                                                            salutation = "Mme. ";
                                                        }
                                                    }
                                                }

                                                listConsultant_Intervenant.Add(string.Format("{0}{1} {2}-{3}", salutation, userAssig.User_LastName, userAssig.User_FirstName, userAssig.user_title));


                                                if (userAssig.user_Mailvalidation != null)//VALIDATION MAIL PAR AGENT
                                                {
                                                    string listU = userAssig.user_Mailvalidation.ToString();
                                                    string[] agentS = listU.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                                    foreach (var agS in agentS)
                                                    {
                                                        var listConsultantSOFT = new List<string>();

                                                        salutation = "";
                                                        if (db.users.Where(a => a.User_EmailAddress == agS && a.User_Deleted != 1 && a.User_UserId != 1).Count() != 0)
                                                        {
                                                            var soft = db.users.Where(a => a.User_EmailAddress == agS && a.User_Deleted != 1 && a.User_UserId != 1).FirstOrDefault();

                                                            if (soft.user_salutation != null)
                                                            {
                                                                if (db.Custom_Captions.Where(a => a.Capt_Family == "pers_salutation" && a.Capt_Code == soft.user_salutation && a.Capt_Deleted != 1).Count() != 0)
                                                                {
                                                                    var sal = db.Custom_Captions.Where(a => a.Capt_Family == "pers_salutation" && a.Capt_Code == soft.user_salutation && a.Capt_Deleted != 1).FirstOrDefault();
                                                                    salutation = sal.Capt_FR + " ";
                                                                }
                                                                else
                                                                {
                                                                    if (userAssig.user_salutation != null)
                                                                    {
                                                                        if (soft.user_salutation.ToLower().Contains("mr"))
                                                                        {
                                                                            if (soft.user_salutation.ToLower() == "mrs.")
                                                                            {
                                                                                salutation = "Mme. ";
                                                                            }
                                                                            else if (soft.user_salutation.ToLower() == "mr.")
                                                                            {
                                                                                salutation = "M. ";
                                                                            }
                                                                        }
                                                                        else if (soft.user_salutation.ToLower().Contains("mme"))
                                                                        {
                                                                            salutation = "Mme. ";
                                                                        }
                                                                        else if (soft.user_salutation.ToLower().Contains("miss"))
                                                                        {
                                                                            salutation = "Mme. ";
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                            listConsultantSOFT.Add(string.Format("{0}{1} {2}-{3}", salutation, soft.User_LastName, soft.User_FirstName, soft.user_title));
                                                        }

                                                        if (listSOFT.Contains(listConsultantSOFT.FirstOrDefault()) == false)
                                                        {
                                                            listSOFT.AddRange(listConsultantSOFT);
                                                        }
                                                    }
                                                }
                                            }

                                            //if (listC.Contains(listConsultant_Intervenant.FirstOrDefault()) == false)
                                            //{
                                            //    listC.AddRange(listConsultant_Intervenant);
                                            //}
                                        }
                                    }


                                    //TACHE//
                                    //TACHES//
                                    if (x.Comm_Subject != null)//Thème
                                    {
                                        listTache.Add(x.Comm_Subject);
                                    }
                                    else
                                        listTache.Add(""); //listTache.Add("Thème non renseigné");

                                    if (x.comm_probleme != null)//Probleme-Besoin
                                    {
                                        listTache.Add(x.comm_probleme);
                                    }
                                    else
                                        listTache.Add(""); //listTache.Add("Problèmes-Besoins non renseignés");

                                    if (x.Comm_Note != null)//Tache
                                    {
                                        listTache.Add(x.Comm_Note);
                                    }
                                    else
                                        listTache.Add(""); //listTache.Add("Tâches non renseignées");

                                    if (x.Comm_ToDateTime != null && x.comm_datefin2 != null)//Durée
                                    {
                                        var dure = (x.comm_datefin2 - x.Comm_ToDateTime);
                                        listTache.Add(dure.Value.ToString());
                                    }

                                    if (x.comm_obs1 != null)//Observation
                                    {
                                        listTache.Add(x.comm_obs1);
                                    }
                                    else
                                        listTache.Add("");

                                    //Intervenants SOFTWELL et CONSULTANT intervenants
                                    var interv = "";
                                    if (x.comm_userintervenant != null)//Agents
                                    {
                                        string listUser = x.comm_userintervenant.ToString();

                                        string[] agentT = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                        foreach (var ag in agentT)
                                        {
                                            var listConsultant_Intervenant = new List<string>();
                                            int idA = int.Parse(ag);
                                            salutation = "";
                                            if (db.users.Where(a => a.User_UserId == idA && a.User_Deleted != 1).Count() != 0)
                                            {
                                                var userAssig = db.users.Where(a => a.User_UserId == idA && a.User_Deleted != 1).FirstOrDefault();

                                                if (db.Custom_Captions.Where(a => a.Capt_Family == "pers_salutation" && a.Capt_Code == userAssig.user_salutation && a.Capt_Deleted != 1).Count() != 0)
                                                {
                                                    var sal = db.Custom_Captions.Where(a => a.Capt_Family == "pers_salutation" && a.Capt_Code == userAssig.user_salutation && a.Capt_Deleted != 1).FirstOrDefault();
                                                    salutation = sal.Capt_FR + " ";
                                                }
                                                else
                                                {
                                                    if (userAssig.user_salutation != null)
                                                    {
                                                        if (userAssig.user_salutation.ToLower().Contains("mr"))
                                                        {
                                                            if (userAssig.user_salutation.ToLower() == "mrs.")
                                                            {
                                                                salutation = "Mme. ";
                                                            }
                                                            else if (userAssig.user_salutation.ToLower() == "mr.")
                                                            {
                                                                salutation = "M. ";
                                                            }
                                                        }
                                                        else if (userAssig.user_salutation.ToLower().Contains("mme"))
                                                        {
                                                            salutation = "Mme. ";
                                                        }
                                                        else if (userAssig.user_salutation.ToLower().Contains("miss"))
                                                        {
                                                            salutation = "Mme. ";
                                                        }
                                                    }
                                                }

                                                interv += string.Format("{0}{1} {2}-{3}, ", salutation, userAssig.User_LastName, userAssig.User_FirstName, userAssig.user_title);

                                                listConsultant_Intervenant.Add(string.Format("{0}{1} {2}-{3}", salutation, userAssig.User_LastName, userAssig.User_FirstName, userAssig.user_title));
                                            }

                                            //if (listC.Contains(listConsultant_Intervenant.ToString()) == false)
                                            //{
                                            //    listC.AddRange(listConsultant_Intervenant);
                                            //}
                                            if (listC.Contains(listConsultant_Intervenant.FirstOrDefault()) == false)
                                            {
                                                listC.AddRange(listConsultant_Intervenant);
                                            }
                                        }
                                    }

                                    //Status//
                                    var stat = "";
                                    if (x.Comm_Status != null)
                                    {
                                        if (db.Custom_Captions.Where(a => a.Capt_Family.ToLower() == "comm_status" && a.Capt_Code == x.Comm_Status && a.Capt_Deleted != 1).Count() != 0)
                                        {
                                            var sta = db.Custom_Captions.Where(a => a.Capt_Family.ToLower() == "comm_status" && a.Capt_Code == x.Comm_Status && a.Capt_Deleted != 1).FirstOrDefault();
                                            stat = sta.Capt_FR;
                                        }
                                    }
                                    listTache.Add(stat);

                                    listTache.Add(interv);

                                    //Intervenant Client//
                                    //FONCTION GET INTERVEANT CLEINT PAR TACHE//
                                    var cltPRESENT = "";
                                    var tttt = db.Comm_Link.Where(a => a.CmLi_Comm_CommunicationId == x.Comm_CommunicationId && a.CmLi_IsExternalAttendee == "Y"
                                        && a.CmLi_ExternalPersonID != null && a.CmLi_Deleted != 1).Count() != 0;
                                    if (db.Comm_Link.Where(a => a.CmLi_Comm_CommunicationId == x.Comm_CommunicationId && a.CmLi_IsExternalAttendee == "Y"
                                        && a.CmLi_ExternalPersonID != null && a.CmLi_Deleted != 1).Count() != 0)
                                    {
                                        foreach (var clPr in db.Comm_Link.Where(a => a.CmLi_Comm_CommunicationId == x.Comm_CommunicationId && a.CmLi_IsExternalAttendee == "Y"
                                             && a.CmLi_ExternalPersonID != null && a.CmLi_Deleted != 1).ToList())
                                        {
                                            var listClientNoms = new List<string>();
                                            var listClientMails = new List<string>();

                                            if (db.Person.Where(a => a.Pers_PersonId == clPr.CmLi_ExternalPersonID).Count() != 0)
                                            {
                                                var pers = db.Person.Where(a => a.Pers_PersonId == clPr.CmLi_ExternalPersonID).FirstOrDefault();

                                                if (db.vPerson.Where(a => a.Pers_PersonId == pers.Pers_PersonId).Count() != 0)
                                                {
                                                    //mailCLIENTDEST += db.vPerson.Where(a => a.Pers_PersonId == pers.Pers_PersonId).FirstOrDefault().Pers_EmailAddress + ",";
                                                    listClientMails.Add(db.vPerson.Where(a => a.Pers_PersonId == pers.Pers_PersonId).FirstOrDefault().Pers_EmailAddress + ",");
                                                }

                                                salutation = "";
                                                if (db.Custom_Captions.Where(a => a.Capt_Family.ToLower() == "pers_salutation" && a.Capt_Code == pers.Pers_Salutation && a.Capt_Deleted != 1).Count() != 0)
                                                {
                                                    var sal = db.Custom_Captions.Where(a => a.Capt_Family.ToLower() == "pers_salutation" && a.Capt_Code == pers.Pers_Salutation && a.Capt_Deleted != 1).FirstOrDefault();
                                                    salutation = sal.Capt_FR + " ";
                                                }
                                                else
                                                {
                                                    if (pers.Pers_Salutation != null)
                                                    {
                                                        if (pers.Pers_Salutation.ToLower().Contains("mr"))
                                                        {
                                                            if (pers.Pers_Salutation.ToLower() == "mrs.")
                                                            {
                                                                salutation = "Mme. ";
                                                            }
                                                            else if (pers.Pers_Salutation.ToLower() == "mr.")
                                                            {
                                                                salutation = "M. ";
                                                            }
                                                        }
                                                        else if (pers.Pers_Salutation.ToLower().Contains("mme"))
                                                        {
                                                            salutation = "Mme. ";
                                                        }
                                                        else if (pers.Pers_Salutation.ToLower().Contains("miss"))
                                                        {
                                                            salutation = "Mme. ";
                                                        }
                                                    }
                                                }

                                                cltPRESENT += string.Format("{0}{1} {2}-{3}, ", salutation, pers.Pers_LastName, pers.Pers_FirstName, pers.Pers_Title);
                                                listClientNoms.Add(string.Format("{0}{1} {2}-{3}", salutation, pers.Pers_LastName, pers.Pers_FirstName, pers.Pers_Title));
                                            }
                                        }
                                    }

                                    listTache.Add(cltPRESENT);

                                    listTaches.Add(listTache);

                                    //Plan d'action//&& a.CmLi_ExternalPersonID != null && a.CmLi_Deleted != 1
                                    //if (x.comm_planid != null)
                                    //{
                                    //    if (db.PLANDACTION.Where(a => a.plan_PLANDACTIONid == x.comm_planid && a.plan_Deleted != 1).Count() != 0)
                                    //    {
                                    //        var planAct = db.PLANDACTION.Where(a => a.plan_PLANDACTIONid == x.comm_planid && a.plan_Deleted != 1).FirstOrDefault();

                                    //        //ID//
                                    //        listPlanAction.Add(planAct.plan_PLANDACTIONid.ToString());

                                    //        listPlanAction.Add(planAct.plan_plandaction);//OBJET-TACHES

                                    //        //INTERVENANT SOFT
                                    //        var intervSoft = "";
                                    //        if (planAct.plan_intervesoftw != null)
                                    //        {
                                    //            string listUser = planAct.plan_intervesoftw.ToString();

                                    //            string[] agentT = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                    //            foreach (var ag in agentT)
                                    //            {
                                    //                int idA = int.Parse(ag);
                                    //                salutation = "";
                                    //                if (db.users.Where(a => a.User_UserId == idA && a.User_Deleted != 1).Count() != 0)
                                    //                {
                                    //                    var userAssig = db.users.Where(a => a.User_UserId == idA && a.User_Deleted != 1).FirstOrDefault();

                                    //                    if (db.Custom_Captions.Where(a => a.Capt_Family == "pers_salutation" && a.Capt_Code == userAssig.user_salutation && a.Capt_Deleted != 1).Count() != 0)
                                    //                    {
                                    //                        var sal = db.Custom_Captions.Where(a => a.Capt_Family == "pers_salutation" && a.Capt_Code == userAssig.user_salutation && a.Capt_Deleted != 1).FirstOrDefault();
                                    //                        salutation = sal.Capt_FR + " ";
                                    //                    }
                                    //                    else
                                    //                    {
                                    //                        if (userAssig.user_salutation.ToLower().Contains("mr"))
                                    //                        {
                                    //                            if (userAssig.user_salutation.ToLower() == "mrs.")
                                    //                            {
                                    //                                salutation = "Mme. ";
                                    //                            }
                                    //                            else if (userAssig.user_salutation.ToLower() == "mr.")
                                    //                            {
                                    //                                salutation = "M. ";
                                    //                            }
                                    //                        }
                                    //                        else if (userAssig.user_salutation.ToLower().Contains("mme"))
                                    //                        {
                                    //                            salutation = "Mme. ";
                                    //                        }
                                    //                        else if (userAssig.user_salutation.ToLower().Contains("miss"))
                                    //                        {
                                    //                            salutation = "Mme. ";
                                    //                        }
                                    //                    }

                                    //                    intervSoft = salutation + userAssig.User_LastName + " " + userAssig.User_FirstName + "-" + userAssig.user_title + ",";
                                    //                }
                                    //            }
                                    //        }

                                    //        listPlanAction.Add("SOFTWELL: " + intervSoft);//Intervenant SOFT//

                                    //        //Intervenant client
                                    //        listPlanAction.Add(client.ToUpper() + ": " + planAct.plan_intervclient);

                                    //        //Echeance//
                                    //        listPlanAction.Add(planAct.plan_datef.Value.ToShortDateString());
                                    //    }
                                    //}
                                    if (db.PLANDACTION.Where(a => a.plan_comm_communicationid == x.Comm_CommunicationId && a.plan_Deleted != 1).Count() != 0)
                                    {
                                        foreach (var pl in db.PLANDACTION.Where(a => a.plan_comm_communicationid == x.Comm_CommunicationId && a.plan_Deleted != 1).ToList())
                                        {
                                            var planAct = db.PLANDACTION.Where(a => a.plan_comm_communicationid == x.Comm_CommunicationId && a.plan_Deleted != 1).FirstOrDefault();

                                            //ID//
                                            listPlanAction.Add(planAct.plan_PLANDACTIONid.ToString());

                                            listPlanAction.Add(planAct.plan_plandaction);//OBJET-TACHES

                                            //INTERVENANT SOFT
                                            var intervSoft = "";
                                            if (planAct.plan_intervesoftw != null)
                                            {
                                                string listUser = planAct.plan_intervesoftw.ToString();

                                                string[] agentT = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                                foreach (var ag in agentT)
                                                {
                                                    int idA = int.Parse(ag);
                                                    salutation = "";
                                                    if (db.users.Where(a => a.User_UserId == idA && a.User_Deleted != 1).Count() != 0)
                                                    {
                                                        var userAssig = db.users.Where(a => a.User_UserId == idA && a.User_Deleted != 1).FirstOrDefault();

                                                        if (db.Custom_Captions.Where(a => a.Capt_Family == "pers_salutation" && a.Capt_Code == userAssig.user_salutation && a.Capt_Deleted != 1).Count() != 0)
                                                        {
                                                            var sal = db.Custom_Captions.Where(a => a.Capt_Family == "pers_salutation" && a.Capt_Code == userAssig.user_salutation && a.Capt_Deleted != 1).FirstOrDefault();
                                                            salutation = sal.Capt_FR + " ";
                                                        }
                                                        else
                                                        {
                                                            if (userAssig.user_salutation != null)
                                                            {
                                                                if (userAssig.user_salutation.ToLower().Contains("mr"))
                                                                {
                                                                    if (userAssig.user_salutation.ToLower() == "mrs.")
                                                                    {
                                                                        salutation = "Mme. ";
                                                                    }
                                                                    else if (userAssig.user_salutation.ToLower() == "mr.")
                                                                    {
                                                                        salutation = "M. ";
                                                                    }
                                                                }
                                                                else if (userAssig.user_salutation.ToLower().Contains("mme"))
                                                                {
                                                                    salutation = "Mme. ";
                                                                }
                                                                else if (userAssig.user_salutation.ToLower().Contains("miss"))
                                                                {
                                                                    salutation = "Mme. ";
                                                                }
                                                            }
                                                        }

                                                        intervSoft = string.Format("{0}{1} {2}-{3},", salutation, userAssig.User_LastName, userAssig.User_FirstName, userAssig.user_title);
                                                    }
                                                }
                                            }

                                            listPlanAction.Add("SOFTWELL: " + intervSoft);//Intervenant SOFT//

                                            //Intervenant client
                                            listPlanAction.Add(string.Format("{0}: {1}", client.ToUpper(), planAct.plan_intervclient));

                                            //Echeance//
                                            if (planAct.plan_datef != null)
                                            {
                                                listPlanAction.Add(planAct.plan_datef.Value.ToShortDateString());
                                            }
                                            else
                                            {
                                                listPlanAction.Add("");
                                            }

                                            listPlannings.Add(listPlanAction);
                                        }
                                    }
                                }
                            }
                            //
                            if (listC.Count() != 0)
                            {
                                foreach (var x in listC)
                                {
                                    TotalConsultant += "<w:p w:rsidP=\"00E802F70\" w:rsidRDefault=\"00D12D950\" w:rsidR=\"00D12D950\" w14:textId=\"5F596DA70\" w14:paraId=\"14CE49170\"><w:pPr><w:numPr><w:ilvl w:val=\"0\"/><w:numId w:val=\"1\"/></w:numPr><w:spacing w:lineRule=\"auto\" w:line=\"240\" w:after=\"0\"/><w:jc w:val=\"both\"/><w:rPr><w:rFonts w:hAnsi=\"Cambria\" w:ascii=\"Cambria\"/><w:b/><w:bCs/></w:rPr></w:pPr><w:r><w:rPr><w:rFonts w:hAnsi=\"Cambria\" w:ascii=\"Cambria\"/><w:b/><w:bCs/></w:rPr><w:t>"
                                         + x + "</w:t></w:r></w:p>";
                                }
                            }

                            if (listSOFT.Count() != 0)
                            {
                                foreach (var x in listSOFT)
                                {
                                    TotalSOFT += "<w:p w:rsidP=\"00E802F70\" w:rsidRDefault=\"00D12D950\" w:rsidR=\"00D12D950\" w14:textId=\"5F596DA70\" w14:paraId=\"14CE49170\"><w:pPr><w:numPr><w:ilvl w:val=\"0\"/><w:numId w:val=\"1\"/></w:numPr><w:spacing w:lineRule=\"auto\" w:line=\"240\" w:after=\"0\"/><w:jc w:val=\"both\"/><w:rPr><w:rFonts w:hAnsi=\"Cambria\" w:ascii=\"Cambria\"/><w:b/><w:bCs/></w:rPr></w:pPr><w:r><w:rPr><w:rFonts w:hAnsi=\"Cambria\" w:ascii=\"Cambria\"/><w:b/><w:bCs/></w:rPr><w:t>"
                                         + x + "</w:t></w:r></w:p>";
                                }
                            }

                            string debTABF = "<w:p w14:paraId=\"499DF2E9\" w14:textId=\"77777777\" w:rsidR=\"00C9544B\" w:rsidRDefault=\"00C9544B\" w:rsidP=\"006E1182\"><w:pPr><w:spacing w:line=\"240\" w:lineRule=\"auto\"/><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\" w:cs=\"Arial\"/><w:b/><w:bCs/></w:rPr></w:pPr><w:r><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\" w:cs=\"Arial\"/><w:b/><w:bCs/></w:rPr><w:t>";
                            string finTABF = "</w:t></w:r></w:p>";

                            string[] separT = { "\r\n" };

                            if (listTaches.Count() != 0)
                            {
                                foreach (List<string> lstTa in listTaches)
                                {
                                    if (lstTa.Count() != 0)
                                    {
                                        //theme//
                                        string theme = lstTa[0];
                                        if (theme.Contains("\r\n"))
                                        {
                                            string[] them = theme.Split(separT, StringSplitOptions.RemoveEmptyEntries);
                                            theme = "";
                                            foreach (var al in them)
                                            {
                                                theme += debTABF + al + finTABF;
                                            }
                                        }


                                        //Probleme//
                                        string probleme = lstTa[1];
                                        if (probleme.Contains("\r\n"))
                                        {
                                            string[] proble = probleme.Split(separT, StringSplitOptions.RemoveEmptyEntries);
                                            probleme = "";
                                            foreach (var al in proble)
                                            {
                                                probleme += debTABF + al + finTABF;
                                            }
                                        }

                                        //Tache//
                                        string tache = lstTa[2];
                                        if (tache.Contains("\r\n"))
                                        {
                                            string[] tac = tache.Split(separT, StringSplitOptions.RemoveEmptyEntries);
                                            tache = "";
                                            foreach (var al in tac)
                                            {
                                                tache += debTABF + al + finTABF;
                                            }
                                        }

                                        //Duree//
                                        string duree = lstTa[3];

                                        //observation//
                                        string observation = lstTa[4];
                                        if (observation.Contains("\r\n"))
                                        {
                                            string[] observ = observation.Split(separT, StringSplitOptions.RemoveEmptyEntries);
                                            observation = "";
                                            foreach (var al in observ)
                                            {
                                                observation += debTABF + al + finTABF;
                                            }
                                        }

                                        //status//
                                        string status = lstTa[5];

                                        var participantsSOFTWELL = @lstTa[6];//SOFTWELL
                                        var participantsCLIENT = @lstTa[7];//CLIENT

                                        TACHES += "<w:tr w:rsidR=\"007B44E3\" w:rsidRPr=\"00A02EE2\" w14:paraId=\"35F1A051\" w14:textId=\"77777777\" w:rsidTr=\"00BF0377\"><w:trPr><w:trHeight w:val=\"5593\"/></w:trPr>" +
                                            "<w:tc><w:tcPr><w:tcW w:w=\"1641\" w:type=\"dxa\"/><w:shd w:val=\"clear\" w:color=\"auto\" w:fill=\"auto\"/></w:tcPr>" +
                                            "<w:p w14:paraId=\"31C16560\" w14:textId=\"137EA927\" w:rsidR=\"007B44E3\" w:rsidRPr=\"00A357D6\" w:rsidRDefault=\"00AA5BCE\" w:rsidP=\"00BF0377\">" +
                                            "<w:pPr><w:spacing w:line=\"240\" w:lineRule=\"auto\"/><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\" w:cs=\"Arial\"/><w:b/><w:bCs/></w:rPr></w:pPr>" +
                                            "<w:r><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\" w:cs=\"Arial\"/><w:b/><w:bCs/></w:rPr><w:t>" + theme + "</w:t></w:r></w:p></w:tc>" +
                                            "<w:tc><w:tcPr><w:tcW w:w=\"2612\" w:type=\"dxa\"/><w:shd w:val=\"clear\" w:color=\"auto\" w:fill=\"auto\"/></w:tcPr>" +
                                            "<w:p w14:paraId=\"635ACF24\" w14:textId=\"6970BF29\" w:rsidR=\"007B44E3\" w:rsidRPr=\"00A357D6\" w:rsidRDefault=\"00AA5BCE\" w:rsidP=\"00BF0377\">" +
                                            "<w:pPr><w:spacing w:line=\"240\" w:lineRule=\"auto\"/><w:jc w:val=\"both\"/><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\" w:cs=\"Arial\"/><w:b/><w:bCs/></w:rPr></w:pPr>" +
                                            "<w:r><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\" w:cs=\"Arial\"/><w:b/><w:bCs/></w:rPr><w:t>" + probleme + "</w:t></w:r></w:p></w:tc>" +
                                            "<w:tc><w:tcPr><w:tcW w:w=\"3260\" w:type=\"dxa\"/></w:tcPr><w:p w14:paraId=\"2DC98BEB\" w14:textId=\"4446D29C\" w:rsidR=\"007B44E3\" w:rsidRDefault=\"00AA5BCE\" w:rsidP=\"00BF0377\">" +
                                            "<w:pPr><w:spacing w:line=\"240\" w:lineRule=\"auto\"/><w:jc w:val=\"both\"/><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\" w:cs=\"Arial\"/><w:b/><w:bCs/></w:rPr>" +
                                            "</w:pPr><w:r><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\" w:cs=\"Arial\"/><w:b/><w:bCs/></w:rPr><w:t>" + tache + "</w:t></w:r></w:p></w:tc>" +
                                            "<w:tc><w:tcPr><w:tcW w:w=\"1418\" w:type=\"dxa\"/></w:tcPr><w:p w14:paraId=\"2809892B\" w14:textId=\"430D3A89\" w:rsidR=\"007B44E3\" w:rsidRPr=\"0053258A\" w:rsidRDefault=\"00AA5BCE\" w:rsidP=\"00BF0377\">" +
                                            "<w:pPr><w:tabs><w:tab w:val=\"left\" w:pos=\"420\"/></w:tabs><w:spacing w:line=\"240\" w:lineRule=\"auto\"/><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\" w:cs=\"Arial\"/><w:b/><w:bCs/><w:color w:val=\"000000\"/></w:rPr></w:pPr>" +
                                            "<w:r><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\" w:cs=\"Arial\"/><w:b/><w:bCs/><w:color w:val=\"000000\"/></w:rPr><w:t>" + duree + "</w:t></w:r></w:p></w:tc>" +
                                            "<w:tc><w:tcPr><w:tcW w:w=\"2933\" w:type=\"dxa\"/><w:shd w:val=\"clear\" w:color=\"auto\" w:fill=\"auto\"/></w:tcPr><w:p w14:paraId=\"10E00573\" w14:textId=\"54596956\" w:rsidR=\"007B44E3\" w:rsidRPr=\"00A357D6\" w:rsidRDefault=\"00AA5BCE\" w:rsidP=\"00BF0377\">" +
                                            "<w:pPr><w:spacing w:line=\"240\" w:lineRule=\"auto\"/><w:jc w:val=\"both\"/><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\" w:cs=\"Arial\"/><w:b/><w:bCs/></w:rPr></w:pPr>" +
                                            "<w:r><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\" w:cs=\"Arial\"/><w:b/><w:bCs/></w:rPr><w:t>" + observation + "</w:t></w:r>" +
                                            "</w:p><w:p w14:paraId=\"7D3DC38C\" w14:textId=\"77777777\" w:rsidR=\"007B44E3\" w:rsidRPr=\"00A357D6\" w:rsidRDefault=\"007B44E3\" w:rsidP=\"00BF0377\"><w:pPr><w:spacing w:line=\"240\" w:lineRule=\"auto\"/><w:jc w:val=\"both\"/><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\" w:cs=\"Arial\"/><w:b/><w:bCs/></w:rPr></w:pPr></w:p></w:tc>" +
                                            "<w:tc><w:tcPr><w:tcW w:w=\"1465\" w:type=\"dxa\"/></w:tcPr><w:p w14:paraId=\"4583C998\" w14:textId=\"2A34FD37\" w:rsidR=\"007B44E3\" w:rsidRDefault=\"00AA5BCE\" w:rsidP=\"00BF0377\"><w:pPr><w:spacing w:line=\"240\" w:lineRule=\"auto\"/><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\" w:cs=\"Arial\"/><w:b/>" +
                                            "<w:bCs/></w:rPr></w:pPr><w:r><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\" w:cs=\"Arial\"/><w:b/><w:bCs/></w:rPr><w:t>" + status + "</w:t></w:r></w:p></w:tc>" +
                                            "<w:tc><w:tcPr><w:tcW w:w=\"2449\" w:type=\"dxa\"/><w:shd w:val=\"clear\" w:color=\"auto\" w:fill=\"auto\"/></w:tcPr><w:p w14:paraId=\"6BD9B03F\" w14:textId=\"77777777\" w:rsidR=\"007B44E3\" w:rsidRDefault=\"007B44E3\" w:rsidP=\"00BF0377\">" +
                                            "<w:pPr><w:spacing w:line=\"240\" w:lineRule=\"auto\"/><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\" w:cs=\"Arial\"/><w:b/><w:bCs/></w:rPr></w:pPr>" +
                                            "<w:r><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\" w:cs=\"Arial\"/><w:b/><w:bCs/></w:rPr><w:t xml:space=\"preserve\">" + client + " : </w:t></w:r></w:p>" +
                                            "<w:p w14:paraId=\"194B4D89\" w14:textId=\"5CB8A569\" w:rsidR=\"007B44E3\" w:rsidRDefault=\"007B44E3\" w:rsidP=\"00BF0377\"><w:pPr><w:spacing w:line=\"240\" w:lineRule=\"auto\"/><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\" w:cs=\"Arial\"/><w:b/><w:bCs/></w:rPr></w:pPr>" +
                                            "<w:r><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\" w:cs=\"Arial\"/><w:bCs/></w:rPr><w:t>" + participantsCLIENT + "</w:t></w:r><w:r w:rsidR=\"004D152D\"><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\" w:cs=\"Arial\"/><w:bCs/></w:rPr><w:t></w:t></w:r></w:p>" + //<w:t>L</w:t>(emplacement du L à enlever//
                                            "<w:p w14:paraId=\"78EA42EE\" w14:textId=\"77777777\" w:rsidR=\"007B44E3\" w:rsidRDefault=\"007B44E3\" w:rsidP=\"00BF0377\"><w:pPr><w:spacing w:line=\"240\" w:lineRule=\"auto\"/><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\" w:cs=\"Arial\"/><w:b/><w:bCs/></w:rPr></w:pPr></w:p>" +
                                            "<w:p w14:paraId=\"4E168D36\" w14:textId=\"77777777\" w:rsidR=\"007B44E3\" w:rsidRDefault=\"007B44E3\" w:rsidP=\"00BF0377\"><w:pPr><w:spacing w:line=\"240\" w:lineRule=\"auto\"/><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\" w:cs=\"Arial\"/><w:bCs/></w:rPr></w:pPr>" +
                                            "<w:r w:rsidRPr=\"00A02EE2\"><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\" w:cs=\"Arial\"/><w:b/><w:bCs/><w:lang w:val=\"en-US\"/></w:rPr><w:t>SOFTWELL</w:t></w:r>" +
                                            "<w:r><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\" w:cs=\"Arial\"/><w:b/><w:bCs/><w:lang w:val=\"en-US\"/></w:rPr><w:t>:</w:t></w:r>" +
                                            "<w:r w:rsidRPr=\"00A02EE2\"><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\" w:cs=\"Arial\"/><w:b/><w:bCs/><w:lang w:val=\"en-US\"/></w:rPr><w:t xml:space=\"preserve\"/></w:r></w:p>" +
                                            "<w:p w14:paraId=\"5234280F\" w14:textId=\"6C9A01EC\" w:rsidR=\"007B44E3\" w:rsidRPr=\"003F4487\" w:rsidRDefault=\"007B44E3\" w:rsidP=\"00BF0377\">" +
                                            "<w:pPr><w:spacing w:line=\"240\" w:lineRule=\"auto\"/><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\" w:cs=\"Arial\"/><w:b/><w:bCs/><w:lang w:val=\"en-US\"/></w:rPr></w:pPr>" +
                                            "<w:r><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\" w:cs=\"Arial\"/><w:bCs/></w:rPr><w:t>" + participantsSOFTWELL + "</w:t></w:r></w:p></w:tc></w:tr>";
                                    }
                                }
                            }

                            if (listPlannings.Count() != 0)
                            {
                                foreach (List<string> lstPl in listPlannings)
                                {
                                    if (lstPl.Count() != 0)
                                    {
                                        //Objet/Tache//
                                        var obj = lstPl[1];

                                        //Echeance//
                                        var echeance = lstPl[4];

                                        var participantsSOFTWELL = @lstPl[2];//SOFTWELL

                                        var participantsCLIENT = @lstPl[3];//CLIENT

                                        PLANS += "<w:tr w:rsidR=\"007B44E3\" w:rsidRPr=\"00770B36\" w14:paraId=\"0A7E9FED\" w14:textId=\"77777777\" w:rsidTr=\"00BF0377\">" +
                                        "<w:tc>" + "<w:tcPr>" + "<w:tcW w:w=\"5457\" w:type=\"dxa\"/>" + "<w:tcBorders>" + "<w:left w:val=\"nil\"/>" + "<w:right w:val=\"nil\"/>" +
                                        "</w:tcBorders>" + "<w:shd w:val=\"clear\" w:color=\"auto\" w:fill=\"BFBFBF\"/>" +
                                        "</w:tcPr><w:p w14:paraId=\"0719F898\" w14:textId=\"3417A234\" w:rsidR=\"007B44E3\" w:rsidRDefault=\"00AA5BCE\" w:rsidP=\"007B44E3\">" +
                                        "<w:pPr><w:numPr><w:ilvl w:val=\"0\"/><w:numId w:val=\"4\"/></w:numPr><w:jc w:val=\"both\"/><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\"/></w:rPr></w:pPr>" +
                                        "<w:r><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\"/></w:rPr><w:t>" + obj + "</w:t></w:r></w:p></w:tc>" +

                                        "<w:tc><w:tcPr><w:tcW w:w=\"2461\" w:type=\"dxa\"/><w:shd w:val=\"clear\" w:color=\"auto\" w:fill=\"D8D8D8\"/></w:tcPr>" +
                                        "<w:p w14:paraId=\"134088BC\" w14:textId=\"5182C956\" w:rsidR=\"007B44E3\" w:rsidRDefault=\"00526E4C\" w:rsidP=\"00BF0377\">" +
                                        "<w:pPr><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\"/><w:b/></w:rPr></w:pPr>" +
                                        "<w:r><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\"/><w:b/></w:rPr><w:t>" + participantsSOFTWELL + "</w:t></w:r></w:p>" +
                                        "<w:p w14:paraId=\"0A62A3A2\" w14:textId=\"3DEA7AFC\" w:rsidR=\"00526E4C\" w:rsidRPr=\"00BB0DB3\" w:rsidRDefault=\"00526E4C\" w:rsidP=\"00BF0377\">" +
                                        "<w:pPr><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\"/><w:b/></w:rPr></w:pPr>" +
                                        "<w:r><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\"/>" +
                                        "<w:b/></w:rPr><w:t>" + participantsCLIENT + "</w:t></w:r></w:p></w:tc>" +

                                        "<w:tc><w:tcPr><w:tcW w:w=\"2255\" w:type=\"dxa\"/><w:shd w:val=\"clear\" w:color=\"auto\" w:fill=\"9BBB59\"/></w:tcPr><w:p w14:paraId=\"052C2B1D\" w14:textId=\"126AC8C5\" w:rsidR=\"007B44E3\" w:rsidRPr=\"00D55901\" w:rsidRDefault=\"00AA5BCE\" w:rsidP=\"00BF0377\">" +
                                        "<w:pPr><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\"/><w:b/></w:rPr></w:pPr><w:r><w:rPr><w:rFonts w:ascii=\"Cambria\" w:hAnsi=\"Cambria\"/><w:b/></w:rPr><w:t>" + echeance + "</w:t></w:r></w:p></w:tc></w:tr>";

                                    }
                                }
                            }
                        }
                    }
                }

                str = str.Replace("VDEMN", demNum);

                //Rubrique dans demande//
                /*var rbD = "SANS RUBRIQUE";
                if (!String.IsNullOrEmpty(demNum) && db.Crmcli_Demandes.Where(a => a.NumeroDemande == demNum).Count() != 0)
                {
                    var isRub = db.Crmcli_Demandes.Where(a => a.NumeroDemande == demNum).FirstOrDefault();
                    if (!String.IsNullOrEmpty(isRub.Rubrique))
                    {
                        var rrb = int.Parse(isRub.Rubrique);
                        if (db.SSDI_SERVICE.Where(a => a.serv_SSDI_SERVICEid == rrb).Count() != 0)
                        {
                            rbD = db.SSDI_SERVICE.Where(a => a.serv_SSDI_SERVICEid == rrb).FirstOrDefault().serv_name;
                        }
                        else if (isRub.Rubrique == "1")
                        {
                            rbD = "Dérogation";
                        }
                    }
                }
                str = str.Replace("VRUBD", rbD);*/

                str = str.Replace("TTTAATTT", TACHES);

                str = str.Replace("PPPBBPPP", PLANS);

                str = str.Replace("CONSUSOFT", TotalConsultant);

                str = str.Replace("VALDSOFTWELL", TotalSOFT);

                str = str.Replace("VALREDACT", agent);

                string isBODY = "Suite à la demande de " + client + ", une VALTYPP";
                string valtype = "intervention";
                string cedoc = "l'intervention";
                //OBJET CR//
                var nature = "";
                //Rubrique ticket//
                var rbC = "SANS RUBRIQUE";

                DateTime dateTestOBJ = DateTime.Parse(date).Date;
                if (db.Communication.Where(a => a.Comm_CaseId == id && a.Comm_Deleted != 1 &&
                    (a.Comm_ToDateTime.Value.Day == dateTestOBJ.Day && a.Comm_ToDateTime.Value.Month == dateTestOBJ.Month && a.Comm_ToDateTime.Value.Year == dateTestOBJ.Year)).Count() != 0)
                {
                    var isCCOM = db.Communication.Where(a => a.Comm_CaseId == id && a.Comm_Deleted != 1 &&
                    (a.Comm_ToDateTime.Value.Day == dateTestOBJ.Day && a.Comm_ToDateTime.Value.Month == dateTestOBJ.Month && a.Comm_ToDateTime.Value.Year == dateTestOBJ.Year)).FirstOrDefault();

                    if (!String.IsNullOrEmpty(isCCOM.comm_nature))
                        nature = db.Custom_Captions.Where(a => a.Capt_Family == "comm_nature" && a.Capt_Deleted == null && a.Capt_Code == isCCOM.comm_nature).FirstOrDefault().Capt_FR;

                    //Rubrique ticket//
                    if (isCCOM.Comm_SSDI_SERVICEId != null)
                    {
                        if (db.SSDI_SERVICE.Where(a => a.serv_SSDI_SERVICEid == isCCOM.Comm_SSDI_SERVICEId && a.serv_Deleted != 1).Count() != 0)
                        {
                            var rr = db.SSDI_SERVICE.Where(a => a.serv_SSDI_SERVICEid == isCCOM.Comm_SSDI_SERVICEId && a.serv_Deleted != 1).FirstOrDefault();

                            rbC = rr.serv_name;
                        }
                    }
                }

                str = str.Replace("VRUBD", rbC);
                //Rubrique ticket//
                //str = str.Replace("VRUBC", rbC);

                if (!String.IsNullOrEmpty(nature) && nature.ToLower().Contains("audit"))
                {
                    isBODY = "Dans le cadre de l’optimisation de l’utilisation du logiciel, une descente";
                    cedoc = "l'audit";
                }
                else if (!String.IsNullOrEmpty(nature) && nature.ToLower().Contains("formation"))
                {
                    valtype = "formation";
                    cedoc = "la formation";
                }

                str = str.Replace("VALINOB", isBODY);
                str = str.Replace("VALTYPP", valtype);
                str = str.Replace("VALINTRODUCTION", sujet);
                str = str.Replace("VALMODULE", module);
                str = str.Replace("VALUWA", cedoc);

                //HEADER//
                string pathHEADER = Path.Combine(newPath + @"\word\header1.xml");
                string strHEADER = System.IO.File.ReadAllText(pathHEADER);
                strHEADER = strHEADER.Replace("VALCLIENT", client);

                var TTTy = "";
                char cT = char.Parse(type.Substring(0, 1));
                bool isVowelT = "aeiouAEIOU".IndexOf(cT) >= 0;
                if (isVowelT == true)
                    TTTy = "d'" + type;
                else
                    TTTy = "de " + type;

                strHEADER = strHEADER.Replace("VALTYPE", TTTy.ToUpper());
                System.IO.File.WriteAllText(pathHEADER, strHEADER);
                //END HEADER//

                System.IO.File.WriteAllText(path, str);

                var pathFile = Server.MapPath("~/CRS/" + filename);

                if (System.IO.File.Exists(pathFile))
                {
                    System.IO.File.Delete(pathFile);
                    ZipFile.CreateFromDirectory(newPath, Server.MapPath("~/CRS/" + filename));
                }
                else
                    ZipFile.CreateFromDirectory(newPath, Server.MapPath("~/CRS/" + filename));

            }
            catch (Exception)
            {
                //var x = false;
            }
        }

        //[HttpPost]
        //public ActionResult OKFUNCT(Cr collection, string submitButton)
        //{
        //    switch (submitButton)
        //    {
        //        case "Générer CR":
        //            return Generer(collection);
        //        case "Envoyer CR pour validation":
        //            return ValidationInterne(collection);
        //        case "Envoyer CR au client":
        //            return ValidationClient(collection);
        //        case "Générer fiche d'intervention":
        //            return GenererFICHE(collection);
        //        default:
        //            return RedirectToAction("Index");
        //    }
        //}
        [HttpGet]
        public ActionResult GenererG(string filename)
        {
            string fullName = Server.MapPath("~/CRS/") + filename;
            return File(fullName, "application/vnd.ms-word", filename);
        }

        //POST: /Intervention/Generer//
        [HttpPost]
        public ActionResult Generer(Cr collection)
        {
            try
            {
                ////
                string ok = collection.DESTMAIL;

                string fullName = "";
                var dateForName = "";
                var dateForTEST = "";

                //GetALLVALUES//
                var client = "";
                var typeInterv = "";
                var modules = "";
                var date = "";
                var hdeb = "";
                var hfin = "";
                var lieu = "";
                var sujet = "";

                var descprobleme = "";
                var destCLT = "";
                var destCLTOK = "";

                var abrev = "";
                //var description = "";
                var CONSULTINTERV = "";
                //var cltnom = "";
                //var cltmail = "";
                var docrem = "";
                var docannex = "";
                var attachement = "";

                var destMAIL = "";

                if (collection.IdCases != String.Empty && collection.IdCases != "0")
                {
                    var id = int.Parse(collection.IdCases);

                    if (db.Cases.Where(a => a.Case_CaseId == id).Count() != 0)
                    {
                        if (collection.Client != null)
                            client = collection.Client;

                        if (collection.Type != null)
                        {
                            typeInterv = collection.Type;
                            typeInterv = typeInterv.Split(' ')[0];
                            if (typeInterv.ToUpper() == "PRESTATIONS")
                            {
                                typeInterv = "INTERVENTIONS ";

                            }
                            abrev = "CR" + typeInterv.Substring(0, 1);
                        }
                        if (collection.Module != null)
                            modules = collection.Module;

                        if (collection.DateIntervention != null)
                            date = collection.DateIntervention;

                        if (collection.Hdeb != null)
                            hdeb = collection.Hdeb;

                        if (collection.Hfin != null)
                            hfin = collection.Hfin;

                        if (collection.Lieu != null)
                            lieu = collection.Lieu;

                        if (collection.Sujet != null)
                            sujet = collection.Sujet;

                        if (collection.DocRem != null)
                            docrem = collection.DocRem;

                        if (collection.DocAnnex != null)
                            docannex = collection.DocAnnex;

                        if (collection.Attachement != null)
                            attachement = collection.Attachement;

                        if (collection.DESCPROBLEME != null)
                            descprobleme = collection.DESCPROBLEME;

                        if (collection.DESTCLT != null)
                            destCLT = collection.DESTCLT;

                        if (collection.CONSULTINTERV != null)
                            CONSULTINTERV = collection.CONSULTINTERV;


                        if (destCLT.Contains("\r\n"))
                        {
                            string note = destCLT.ToString();
                            string[] separators = { "\r\n" };
                            string[] nt = note.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                            foreach (var dst in nt)
                            {
                                destCLTOK += "<w:p w:rsidP=\"00E802F70\" w:rsidRDefault=\"00D12D950\" w:rsidR=\"00D12D950\" w14:textId=\"5F596DA70\" w14:paraId=\"14CE49170\"><w:pPr><w:numPr><w:ilvl w:val=\"0\"/><w:numId w:val=\"1\"/></w:numPr><w:spacing w:lineRule=\"auto\" w:line=\"240\" w:after=\"0\"/><w:jc w:val=\"both\"/><w:rPr><w:rFonts w:hAnsi=\"Cambria\" w:ascii=\"Cambria\"/><w:b/><w:bCs/></w:rPr></w:pPr><w:r><w:rPr><w:rFonts w:hAnsi=\"Cambria\" w:ascii=\"Cambria\"/><w:b/><w:bCs/></w:rPr><w:t>"
                                    + dst + "</w:t></w:r></w:p>";
                            }
                        }

                        if (collection.DESTMAIL != null)
                            destMAIL = collection.DESTMAIL;

                        dateForTEST = DateTime.Parse(collection.DateIntervention).ToShortDateString();

                        if (dateForTEST.Contains("/"))
                        {
                            dateForName = dateForTEST.Replace("/", "");
                        }

                        //ADD in base Crmcli_CR//
                        if (db.Crmcli_CR.Where(a => a.ID_CASES == id).Count() != 0)
                        {
                            foreach (var x in db.Crmcli_CR.Where(a => a.ID_CASES == id).ToList())
                            {
                                if (x.DATE.Value.Date == DateTime.Parse(date).Date)
                                {
                                    var formodif = db.Crmcli_CR.Where(a => a.ID == x.ID).FirstOrDefault();

                                    formodif.DESTCLIENT = destCLT;
                                    formodif.DESTMAIL = destMAIL;
                                    formodif.DOCREM = docrem;
                                    db.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            Crmcli_CR cr = new Crmcli_CR();

                            cr.DATE = DateTime.Parse(date).Date;
                            cr.ID_CASES = id;
                            cr.DESTCLIENT = destCLT;
                            cr.DESTMAIL = destMAIL;
                            cr.DOCREM = docrem;

                            db.Crmcli_CR.Add(cr);
                            db.SaveChanges();
                        }


                        CreatWordDoc(string.Format("CR_{0}_{1}.docx", client, dateForName),
                        id, client, typeInterv, abrev, modules, date, hdeb, hfin, lieu, sujet, docrem, docannex, attachement, destCLTOK, destMAIL/*, CONSULTINTERV*/);

                        //Generation download//
                        fullName = string.Format("{0}CR_{1}_{2}.docx", Server.MapPath("~/CRS/"), client, dateForName);

                        string NameFile = string.Format("CR_{0}_{1}.docx", client, dateForName);

                        //byte[] fileBytes = GetFile(fullName);


                        return Content(NameFile);
                        //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "CR" + "_" + client + "_" + dateForName + ".docx");
                    }
                    else
                        return Content("Erreur!");
                }
                else
                    return Content("Erreur!");
            }
            catch (Exception)
            {
                return Content("Erreur!");
            }
        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }

        //
        // ValidationInterne
        [HttpPost]
        public ActionResult ValidationInterne(Cr collection)
        {
            try
            {
                var dateForName = "";
                var dateForTEST = "";

                //GetALLVALUES//
                var client = "";
                var typeInterv = "";
                var modules = "";
                var date = "";
                var hdeb = "";
                var hfin = "";
                var lieu = "";
                var sujet = "";

                var descprobleme = "";
                var destCLT = "";
                var destCLTOK = "";

                var abrev = "";
                //var description = "";
                //var intervenant = "";
                //var cltnom = "";
                //var cltmail = "";
                var docrem = "";
                var docannex = "";
                var attachement = "";

                var destMAIL = "";

                var numticket = "";

                if (collection.IdCases != String.Empty && collection.IdCases != "0")
                {
                    var id = int.Parse(collection.IdCases);

                    if (db.Cases.Where(a => a.Case_CaseId == id).Count() != 0)
                    {
                        if (db.Cases.Where(a => a.Case_CaseId == id).FirstOrDefault().Case_ReferenceId != null)
                            numticket = db.Cases.Where(a => a.Case_CaseId == id).FirstOrDefault().Case_ReferenceId;

                        if (collection.Client != null)
                            client = collection.Client;

                        if (collection.Type != null)
                        {
                            typeInterv = collection.Type;
                            typeInterv = typeInterv.Split(' ')[0];
                            if (typeInterv.ToUpper() == "PRESTATIONS")
                            {
                                typeInterv = "INTERVENTIONS ";

                            }
                            abrev = "CR" + typeInterv.Substring(0, 1);
                        }

                        if (collection.Module != null)
                            modules = collection.Module;

                        if (collection.DateIntervention != null)
                            date = collection.DateIntervention;

                        if (collection.Hdeb != null)
                            hdeb = collection.Hdeb;

                        if (collection.Hfin != null)
                            hfin = collection.Hfin;

                        if (collection.Lieu != null)
                            lieu = collection.Lieu;

                        if (collection.Sujet != null)
                            sujet = collection.Sujet;

                        if (collection.DocRem != null)
                            docrem = collection.DocRem;

                        if (collection.DocAnnex != null)
                            docannex = collection.DocAnnex;

                        if (collection.Attachement != null)
                            attachement = collection.Attachement;

                        if (collection.DESCPROBLEME != null)
                            descprobleme = collection.DESCPROBLEME;

                        if (collection.DESTCLT != null)
                            destCLT = collection.DESTCLT;

                        if (destCLT.Contains("\r\n"))
                        {
                            string note = destCLT.ToString();
                            string[] separators = { "\r\n" };
                            string[] nt = note.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                            foreach (var dst in nt)
                            {
                                destCLTOK += "<w:p w:rsidP=\"00E802F70\" w:rsidRDefault=\"00D12D950\" w:rsidR=\"00D12D950\" w14:textId=\"5F596DA70\" w14:paraId=\"14CE49170\"><w:pPr><w:numPr><w:ilvl w:val=\"0\"/><w:numId w:val=\"1\"/></w:numPr><w:spacing w:lineRule=\"auto\" w:line=\"240\" w:after=\"0\"/><w:jc w:val=\"both\"/><w:rPr><w:rFonts w:hAnsi=\"Cambria\" w:ascii=\"Cambria\"/><w:b/><w:bCs/></w:rPr></w:pPr><w:r><w:rPr><w:rFonts w:hAnsi=\"Cambria\" w:ascii=\"Cambria\"/><w:b/><w:bCs/></w:rPr><w:t>"
                                    + dst + "</w:t></w:r></w:p>";
                            }
                        }

                        if (collection.DESTMAIL != null)
                            destMAIL = collection.DESTMAIL;

                        dateForTEST = DateTime.Parse(collection.DateIntervention).ToShortDateString();

                        if (dateForTEST.Contains("/"))
                        {
                            dateForName = dateForTEST.Replace("/", "");
                        }

                        //ADD in base Crmcli_CR//
                        if (db.Crmcli_CR.Where(a => a.ID_CASES == id).Count() != 0)
                        {
                            foreach (var x in db.Crmcli_CR.Where(a => a.ID_CASES == id).ToList())
                            {
                                if (x.DATE.Value.Date == DateTime.Parse(date).Date)
                                {
                                    var formodif = db.Crmcli_CR.Where(a => a.ID == x.ID).FirstOrDefault();

                                    formodif.DESTCLIENT = destCLT;
                                    formodif.DESTMAIL = destMAIL;
                                    formodif.DOCREM = docrem;
                                    db.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            Crmcli_CR cr = new Crmcli_CR();

                            cr.DATE = DateTime.Parse(date).Date;
                            cr.ID_CASES = id;
                            cr.DESTCLIENT = destCLT;
                            cr.DESTMAIL = destMAIL;
                            cr.DOCREM = docrem;

                            db.Crmcli_CR.Add(cr);
                            db.SaveChanges();
                        }


                        CreatWordDoc(string.Format("CR_{0}_{1}.docx", client, dateForName),
                        id, client, typeInterv, abrev, modules, date, hdeb, hfin, lieu, sujet, docrem, docannex, attachement, destCLTOK, destMAIL);

                        var Atta = string.Format("CR_{0}_{1}.docx", client, dateForName);

                        //GénérationCR//
                        string mdpMail = "";
                        string MailAdresse = "";

                        using (System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage())
                        {
                            if (Session["UserId"] != null)
                            {
                                var idA = int.Parse(Session["UserId"].ToString());

                                if (db.users.Where(a => a.User_UserId == idA && a.User_Deleted != 1 && a.user_pwdmail != null && a.User_EmailAddress != null).Count() != 0)
                                {
                                    var agent = db.users.Where(a => a.User_UserId == idA && a.User_Deleted != 1 && a.user_pwdmail != null && a.User_EmailAddress != null).FirstOrDefault();

                                    mdpMail = agent.user_pwdmail;
                                    MailAdresse = agent.User_EmailAddress;

                                    //mdpMail = "Riririnah06";
                                    //MailAdresse = "rinah.raharinosy@softwell.mg";

                                    mail.From = new MailAddress(MailAdresse);


                                    if (agent.user_Mailvalidation != null)
                                    {
                                        var eml = agent.user_Mailvalidation.Split(',');

                                        foreach (var item in eml)
                                        {
                                            if (!item.ToString().Contains("colombe"))
                                            {
                                                mail.To.Add(item);
                                            }
                                        }
                                    }

                                    //mail.To.Add("rinah.raharinosy@softwell.mg");
                                    var TTy = "";
                                    char c = char.Parse(collection.Type.Substring(0, 1));
                                    bool isVowel = "aeiouAEIOU".IndexOf(c) >= 0;
                                    string collecteo;
                                    if (isVowel == true)
                                    {
                                        collecteo = collection.Type.Split(' ')[0];
                                        TTy = "d'" + collecteo;
                                    }
                                    else
                                    {
                                        collecteo = collection.Type.Split(' ')[0];
                                        TTy = "de " + collecteo;
                                    }
                                    mail.Subject = string.Format("Compte rendu {0} du client {1} du {2}", TTy, client, collection.DateIntervention);

                                    if (Atta != null)
                                    {
                                        mail.Attachments.Add(new Attachment(Server.MapPath("~/CRS/") + Atta));
                                    }

                                    var pathFile = Server.MapPath(string.Format("~/SIGNATURES/{0}.jpg", idA));

                                    if (System.IO.File.Exists(pathFile))
                                    {
                                        mail.IsBodyHtml = true;
                                        string path = Server.MapPath(string.Format("~/SIGNATURES/{0}.jpg", idA));
                                        LinkedResource Img = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
                                        Img.ContentId = "MyImage";
                                        string str;
                                        if (collecteo == "Assistance")
                                        {
                                            str = @"  
                                            <table>  
                                                <tr>  
                                                    <td>" + "Bonjour,<br/><br>" +
                                                                "Suite à notre intervention du " + collection.DateIntervention + " " + collection.Hdeb +
                                                                " au " + collection.DateIntervention + " " + collection.Hfin + ", nous souhaitons avoir votre validation sur le compte rendu en jointure portant sur le ticket n° " + numticket + ".<br/><br>" + "Cordialement.<br/><br>" + @" 
                                                    </td>  
                                                </tr>  
                                                <tr>  
                                                    <td>  
                                                      <img src=cid:MyImage  id='img' alt='' width='450px' height='195px'/>   
                                                    </td>  
                                                </tr></table>  
                                            ";
                                        }
                                        else
                                        {
                                            str = @"  
                                        <table>  
                                            <tr>  
                                                <td>" + "Bonjour,<br/><br>" +
                                                            "Suite à notre " + collecteo + " du " + collection.DateIntervention + " " + collection.Hdeb +
                                                            " au " + collection.DateIntervention + " " + collection.Hfin + ", nous souhaitons avoir votre validation sur le compte rendu en jointure portant sur le ticket n° " + numticket + ".<br/><br>" + "Cordialement.<br/><br>" + @" 
                                                </td>  
                                            </tr>  
                                            <tr>  
                                                <td>  
                                                  <img src=cid:MyImage  id='img' alt='' width='450px' height='195px'/>   
                                                </td>  
                                            </tr></table>  
                                        ";
                                        }

                                        //string str = @"  
                                        //<table>  
                                        //    <tr>  
                                        //        <td>" + "Bonjour,<br/><br>" +
                                        //                    "Suite à notre "+ collecteo +" du " + collection.Client + " du " + collection.DateIntervention + " " + collection.Hdeb +
                                        //                    " au " + collection.DateIntervention + " " + collection.Hfin + ", nous souhaitons avoir votre validation sur le compte rendu en jointure portant sur le ticket n° " + numticket + ".<br/><br>" + "Cordialement.<br/><br>" + @" 
                                        //        </td>  
                                        //    </tr>  
                                        //    <tr>  
                                        //        <td>  
                                        //          <img src=cid:MyImage  id='img' alt='' width='450px' height='195px'/>   
                                        //        </td>  
                                        //    </tr></table>  
                                        //";

                                        AlternateView AV = AlternateView.CreateAlternateViewFromString(str, null, MediaTypeNames.Text.Html);
                                        AV.LinkedResources.Add(Img);

                                        mail.AlternateViews.Add(AV);
                                    }
                                    else
                                    {
                                        mail.IsBodyHtml = true;
                                        mail.Body = "Bonjour,<br/><br>" +
                                        "Suite à notre intervention du " + collection.DateIntervention + " " + collection.Hdeb +
                                        " au " + collection.DateIntervention + " " + collection.Hfin + ", nous souhaitons avoir votre validation sur le compte rendu en jointure portant sur le ticket n° " + numticket + ".<br/><br>" + "Cordialement.<br/><br>";
                                    }

                                    if (MailAdresse.Contains("gmail"))
                                    {
                                        var smtpClient = new SmtpClient("smtp.gmail.com")
                                        {
                                            Port = 587,
                                            Credentials = new System.Net.NetworkCredential(MailAdresse, mdpMail),
                                            EnableSsl = false,
                                            UseDefaultCredentials = false,
                                            //DeliveryMethod = SmtpDeliveryMethod.Network
                                        };

                                        smtpClient.Send(mail);//eto blem
                                    }
                                    else
                                    {
                                        SmtpClient smtp = new SmtpClient("smtpauth.moov.mg")
                                        {
                                            UseDefaultCredentials = true,
                                            Port = 587,
                                            Credentials = new System.Net.NetworkCredential(MailAdresse, mdpMail),
                                            EnableSsl = true
                                        };

                                        smtp.Send(mail);
                                    }
                                }
                            }
                            //Models.Mailing model = new Models.Mailing();
                            //model.TriggerOnLoad = true;
                            //model.TriggerOnLoadMessage = "Envoie avec succès!";
                            //return View("Add", model);
                        }

                        //SAUVE SUIVI ENVOIE MAIL//
                        DateTime dateFt = DateTime.Parse(collection.DateIntervention).Date;
                        int idcaS = int.Parse(collection.IdCases);
                        if (db.Crmcli_EnvoiMails.Where(a => a.Case_CaseId == idcaS && a.CommDATE.Value.Day == dateFt.Day
                            && a.CommDATE.Value.Month == dateFt.Month && a.CommDATE.Value.Year == dateFt.Year).Count() != 0)
                        {
                            var formodif = db.Crmcli_EnvoiMails.Where(a => a.Case_CaseId == idcaS && a.CommDATE.Value.Day == dateFt.Day
                            && a.CommDATE.Value.Month == dateFt.Month && a.CommDATE.Value.Year == dateFt.Year).FirstOrDefault();

                            formodif.DateInterne = DateTime.Now;
                            db.SaveChanges();
                        }
                        else
                        {
                            Crmcli_EnvoiMails cr = new Crmcli_EnvoiMails();

                            cr.CommDATE = dateFt;
                            cr.Case_CaseId = idcaS;
                            cr.DateInterne = DateTime.Now;

                            db.Crmcli_EnvoiMails.Add(cr);
                            db.SaveChanges();
                        }

                        return Content("Avec succès!");
                        //return RedirectToAction("Details", "Intervention", new { id = collection.IdCases, dateCom = collection.DateIntervention });
                    }
                    else
                        return Content("Erreur!");
                }
                else
                    return Content("Erreur!");
            }
            catch (Exception)
            {
                return Content("Erreur!");
            }
        }

        //[HttpPost]
        //public ActionResult ValidC(Cr collection)
        //{
        //    return RedirectToAction("ValidationClient","Intervention", collection);
        //}


        // ValidationClient
        [HttpPost]
        public ActionResult ValidationClient(Cr collection)
        {
            //Cr collection = new Cr();

            //UpdateModel(collection);

            try
            {
                //GENERATION CR//
                var dateForName = "";
                var dateForTEST = "";

                //GetALLVALUES//
                var client = "";
                var typeInterv = "";
                var modules = "";
                var date = "";
                var hdeb = "";
                var hfin = "";
                var lieu = "";
                var sujet = "";

                var descprobleme = "";
                var destCLT = "";
                var destCLTOK = "";

                var abrev = "";

                var docrem = "";
                var docannex = "";
                var attachement = "";

                var destMAIL = "";
                var Atta = "";

                if (collection.IdCases != String.Empty && collection.IdCases != "0")
                {
                    var id = int.Parse(collection.IdCases);

                    if (db.Cases.Where(a => a.Case_CaseId == id).Count() != 0)
                    {
                        if (collection.Client != null)
                            client = collection.Client;

                        if (collection.Type != null)
                        {
                            typeInterv = collection.Type;
                            typeInterv = typeInterv.Split(' ')[0];
                            if (typeInterv.ToUpper() == "PRESTATIONS")
                            {
                                typeInterv = "INTERVENTIONS ";

                            }
                            abrev = "CR" + typeInterv.Substring(0, 1);
                        }

                        if (collection.Module != null)
                            modules = collection.Module;

                        if (collection.DateIntervention != null)
                            date = collection.DateIntervention;

                        if (collection.Hdeb != null)
                            hdeb = collection.Hdeb;

                        if (collection.Hfin != null)
                            hfin = collection.Hfin;

                        if (collection.Lieu != null)
                            lieu = collection.Lieu;

                        if (collection.Sujet != null)
                            sujet = collection.Sujet;

                        if (collection.DocRem != null)
                            docrem = collection.DocRem;

                        if (collection.DocAnnex != null)
                            docannex = collection.DocAnnex;

                        if (collection.Attachement != null)
                            attachement = collection.Attachement;

                        if (collection.DESCPROBLEME != null)
                            descprobleme = collection.DESCPROBLEME;

                        if (collection.DESTCLT != null)
                            destCLT = collection.DESTCLT;

                        if (destCLT.Contains("\r\n"))
                        {
                            string note = destCLT.ToString();
                            string[] separators = { "\r\n" };
                            string[] nt = note.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                            foreach (var dst in nt)
                            {
                                destCLTOK += "<w:p w:rsidP=\"00E802F70\" w:rsidRDefault=\"00D12D950\" w:rsidR=\"00D12D950\" w14:textId=\"5F596DA70\" w14:paraId=\"14CE49170\"><w:pPr><w:numPr><w:ilvl w:val=\"0\"/><w:numId w:val=\"1\"/></w:numPr><w:spacing w:lineRule=\"auto\" w:line=\"240\" w:after=\"0\"/><w:jc w:val=\"both\"/><w:rPr><w:rFonts w:hAnsi=\"Cambria\" w:ascii=\"Cambria\"/><w:b/><w:bCs/></w:rPr></w:pPr><w:r><w:rPr><w:rFonts w:hAnsi=\"Cambria\" w:ascii=\"Cambria\"/><w:b/><w:bCs/></w:rPr><w:t>"
                                    + dst + "</w:t></w:r></w:p>";
                            }
                        }

                        if (collection.DESTMAIL != null)
                            destMAIL = collection.DESTMAIL;

                        dateForTEST = DateTime.Parse(collection.DateIntervention).ToShortDateString();

                        if (dateForTEST.Contains("/"))
                        {
                            dateForName = dateForTEST.Replace("/", "");
                        }

                        //ADD in base Crmcli_CR//
                        if (db.Crmcli_CR.Where(a => a.ID_CASES == id).Count() != 0)
                        {
                            foreach (var x in db.Crmcli_CR.Where(a => a.ID_CASES == id).ToList())
                            {
                                if (x.DATE.Value.Date == DateTime.Parse(date).Date)
                                {
                                    var formodif = db.Crmcli_CR.Where(a => a.ID == x.ID).FirstOrDefault();

                                    formodif.DESTCLIENT = destCLT;
                                    formodif.DESTMAIL = destMAIL;
                                    formodif.DOCREM = docrem;
                                    db.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            Crmcli_CR cr = new Crmcli_CR();

                            cr.DATE = DateTime.Parse(date).Date;
                            cr.ID_CASES = id;
                            cr.DESTCLIENT = destCLT;
                            cr.DESTMAIL = destMAIL;
                            cr.DOCREM = docrem;

                            db.Crmcli_CR.Add(cr);
                            db.SaveChanges();
                        }


                        CreatWordDoc(string.Format("CR_{0}_{1}.docx", client, dateForName),
                        id, client, typeInterv, abrev, modules, date, hdeb, hfin, lieu, sujet, docrem, docannex, attachement, destCLTOK, destMAIL);

                        Atta = string.Format("CR_{0}_{1}", client, dateForName);
                    }
                }

                //NEXT VIEW//
                List<List<string>> list = new List<List<string>>();
                List<string> listCM = new List<string>();

                var DESTINATEIREMAIL = "";
                var MailAdresse2 = "";
                var subject = "";
                var body = "";

                if (Session["UserId"] != null)
                {
                    var id = int.Parse(Session["UserId"].ToString());

                    if (db.users.Where(a => a.User_UserId == id && a.User_Deleted != 1 && a.user_pwdmail != null && a.User_EmailAddress != null).Count() != 0)
                    {
                        var agent = db.users.Where(a => a.User_UserId == id && a.User_Deleted != 1 && a.user_pwdmail != null && a.User_EmailAddress != null).FirstOrDefault();

                        MailAdresse2 = agent.User_EmailAddress;//MAILSENDER


                        if (agent.user_Mailvalidation != null)
                        {
                            DESTINATEIREMAIL += agent.user_Mailvalidation + ",";
                        }
                    }

                    var TTy = "";
                    char c = char.Parse(collection.Type.Substring(0, 1));
                    bool isVowel = "aeiouAEIOU".IndexOf(c) >= 0;
                    string collecteo;
                    if (isVowel == true)
                    {
                        collecteo = collection.Type.Split(' ')[0];
                        TTy = "d'" + collecteo;
                    }
                    else
                    {
                        collecteo = collection.Type.Split(' ')[0];
                        TTy = "de " + collecteo;
                    }
                    subject = string.Format("Compte rendu {0} du {1}", TTy, collection.DateIntervention);//SUJET

                    //BODY//
                    if (collecteo == "Assistance")
                    {
                        body = "Bonjour,\r\n" +
                        "Suite à notre intervention du " + collection.DateIntervention + " " + collection.Hdeb +
                        " au " + collection.DateIntervention + " " + collection.Hfin + ", veuillez trouver ci-joint le compte rendu des points que nous avons abordés et/ou traités.\r\n" +
                        "Sans commentaire de votre part dans les 2 jours de cet envoi, il sera considéré comme validé.\r\n" + "En vous souhaitant bonne réception et restant à votre disposition pour tout complément d’informations. \r\n" + "Cordialement.";

                    }
                    else
                    {
                        body = "Bonjour,\r\n" +
                        "Suite à notre " + collecteo + "du " + collection.DateIntervention + " " + collection.Hdeb +
                        " au " + collection.DateIntervention + " " + collection.Hfin + ", veuillez trouver ci-joint le compte rendu des points que nous avons abordés et/ou traités.\r\n" +
                        "Sans commentaire de votre part dans les 2 jours de cet envoi, il sera considéré comme validé.\r\n" + "En vous souhaitant bonne réception et restant à votre disposition pour tout complément d’informations. \r\n" + "Cordialement.";

                    }


                    //DESTMAILTOTAL//
                    DESTINATEIREMAIL += collection.DESTMAIL;
                }

                listCM.Add(MailAdresse2);
                listCM.Add(subject);
                listCM.Add(body);
                listCM.Add(DESTINATEIREMAIL + MailAdresse2);
                listCM.Add(Atta);
                listCM.Add(collection.IdCases);
                listCM.Add(collection.DateIntervention);

                list.Add(listCM);

                var lisCM = list;
                ViewBag.ldv = lisCM;

                return View("ValidationClient");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult ValidationClientT(Mailing collection, List<HttpPostedFileBase> ATTACHEM)
        {
            try
            {
                string mdpMail = "";
                string MailAdresse = "";
                using (System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage())
                {
                    if (Session["UserId"] != null)
                    {
                        var idA = int.Parse(Session["UserId"].ToString());

                        if (db.users.Where(a => a.User_UserId == idA && a.User_Deleted != 1 && a.user_pwdmail != null && a.User_EmailAddress != null).Count() != 0)
                        {
                            var agent = db.users.Where(a => a.User_UserId == idA && a.User_Deleted != 1 && a.user_pwdmail != null && a.User_EmailAddress != null).FirstOrDefault();

                            mdpMail = agent.user_pwdmail;
                            MailAdresse = collection.From;

                            SmtpClient smtp = new SmtpClient("smtpauth.moov.mg");

                            smtp.UseDefaultCredentials = true;

                            mail.From = new MailAddress(MailAdresse);

                            var mailCltIsNotNull = false;

                            if (collection.To != null)
                            {
                                foreach (var address in collection.To.Replace("\r\n", "").Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                                {
                                    if (!address.Contains("softwell"))
                                    {
                                        mailCltIsNotNull = true;
                                    }
                                }
                            }

                            if (mailCltIsNotNull == true)
                            {
                                foreach (var address in collection.To.Replace("\r\n", "").Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                                {
                                    mail.To.Add(address);
                                }
                            }
                            else
                                return Content("<script language='javascript' type='text/javascript'>alert('Mail client obligatoire pour les destinataires!');</script>");

                            //mail.To.Add("fiderana.rakotonirinarisoa@softwell.mg");

                            mail.Subject = collection.Sujet;

                            if (collection.Attachement != null && System.IO.File.Exists(string.Format("{0}{1}.docx", Server.MapPath("~/CRS/"), collection.Attachement)))
                            {
                                //RichEditDocumentServer server = new RichEditDocumentServer();
                                //server.LoadDocument(string.Format("{0}{1}.docx", Server.MapPath("~/CRS/"), collection.Attachement));
                                //FileStream fsOut = System.IO.File.Open(string.Format("{0}{1}.pdf", Server.MapPath("~/CRS/"), collection.Attachement), FileMode.Create);
                                //server.ExportToPdf(fsOut);
                                //fsOut.Close();
                                //return Content("<script language='javascript' type='text/javascript'>alert('Err!');</script>");
                                ////////////////////////////////
                                //RichEditDocumentServer wordProcessor = new RichEditDocumentServer();
                                //wordProcessor.LoadDocument(string.Format("{0}{1}.docx", Server.MapPath("~/CRS/"), collection.Attachement), DocumentFormat.OpenXml);

                                ////Specify export options:
                                //PdfExportOptions options = new PdfExportOptions();
                                //options.Compressed = false;
                                //options.ImageQuality = PdfJpegImageQuality.Highest;

                                ////Export the document to the stream:
                                //using (FileStream pdfFileStream = new FileStream(string.Format("{0}{1}.pdf", Server.MapPath("~/CRS/"), collection.Attachement), FileMode.Create))
                                //{
                                //    wordProcessor.ExportToPdf(pdfFileStream, options);
                                //}

                                //Loads an existing Word document
                                Syncfusion.DocIO.DLS.WordDocument wordDocument = new Syncfusion.DocIO.DLS.WordDocument(string.Format("{0}{1}.docx", Server.MapPath("~/CRS/"), collection.Attachement), FormatType.Docx);
                                //Creates an instance of the DocToPDFConverter
                                DocToPDFConverter converter = new DocToPDFConverter();
                                //Converts Word document into PDF document
                                Syncfusion.Pdf.PdfDocument pdfDocument = converter.ConvertToPDF(wordDocument);
                                //Releases all resources used by DocToPDFConverter
                                converter.Dispose();
                                //Closes the instance of document objects
                                wordDocument.Close();
                                //Saves the PDF file 
                                pdfDocument.Save(string.Format("{0}{1}.pdf", Server.MapPath("~/CRS/"), collection.Attachement));
                                //Closes the instance of document objects
                                pdfDocument.Close(true);

                                mail.Attachments.Add(new Attachment(string.Format("{0}{1}.pdf", Server.MapPath("~/CRS/"), collection.Attachement)));
                            }

                            //ATTACHEMENT//
                            if (ATTACHEM != null)
                            {
                                foreach (HttpPostedFileBase at in ATTACHEM)
                                {
                                    if (at != null)
                                    {
                                        string fileName = Path.GetFileName(at.FileName);
                                        mail.Attachments.Add(new Attachment(at.InputStream, fileName));
                                    }
                                }
                            }

                            var pathFile = Server.MapPath(string.Format("~/SIGNATURES/{0}.jpg", idA));

                            var bod = "";
                            if (collection.Body.Contains("\r\n"))
                            {
                                bod = collection.Body.Replace("\r\n", "<br/><br>");
                            }

                            if (System.IO.File.Exists(pathFile))
                            {
                                mail.IsBodyHtml = true;
                                string path = Server.MapPath(string.Format("~/SIGNATURES/{0}.jpg", idA));
                                LinkedResource Img = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
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

                                AlternateView AV = AlternateView.CreateAlternateViewFromString(str, null, MediaTypeNames.Text.Html);
                                AV.LinkedResources.Add(Img);

                                mail.AlternateViews.Add(AV);
                            }
                            else
                            {
                                mail.IsBodyHtml = true;
                                mail.Body = bod;
                            }

                            smtp.Port = 587;

                            smtp.Credentials = new System.Net.NetworkCredential(MailAdresse, mdpMail);
                            smtp.EnableSsl = true;

                            //SAUVE SUIVI ENVOIE MAIL//
                            DateTime dateFt = DateTime.Parse(collection.DateIntervention).Date;
                            int idcaS = int.Parse(collection.IdCases);
                            if (db.Crmcli_EnvoiMails.Where(a => a.Case_CaseId == idcaS &&
                                a.CommDATE.Value.Day == dateFt.Day && a.CommDATE.Value.Month == dateFt.Month && a.CommDATE.Value.Year == dateFt.Year).Count() != 0)
                            {
                                var formodif = db.Crmcli_EnvoiMails.Where(a => a.Case_CaseId == idcaS &&
                                        a.CommDATE.Value.Day == dateFt.Day && a.CommDATE.Value.Month == dateFt.Month && a.CommDATE.Value.Year == dateFt.Year).FirstOrDefault();

                                formodif.DateClient = DateTime.Now;
                                db.SaveChanges();
                            }
                            else
                            {
                                Crmcli_EnvoiMails cr = new Crmcli_EnvoiMails();

                                cr.CommDATE = dateFt;
                                cr.Case_CaseId = idcaS;
                                cr.DateClient = DateTime.Now;

                                db.Crmcli_EnvoiMails.Add(cr);
                                db.SaveChanges();
                            }

                            smtp.Send(mail);

                            return Content("<script language='javascript' type='text/javascript'>alert('Avec succès!');</script>");
                            //return RedirectToAction("Details", "Intervention", new { id = collection.IdCases, dateCom = collection.DateIntervention });
                        }
                        else
                            return Content("<script language='javascript' type='text/javascript'>alert('Utilisateur actuel non trouvé!');</script>");
                    }
                    else
                        return Content("<script language='javascript' type='text/javascript'>alert('Session expirée!');</script>");
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        // GET: /Intervention/Create
        public ActionResult Create(int id)
        {
            return View();
        }

        // POST: /Intervention/Create
        [HttpPost]
        public ActionResult Create(Crmcli_SousTaches collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: /Intervention/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: /Intervention/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Crmcli_Taches tache)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: /Intervention/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: /Intervention/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult GenererF(string filename)
        {
            string fullName = Server.MapPath("~/FICHESTECH/") + filename;
            return File(fullName, "application/vnd.ms-word", filename);
        }

        //POST: /Intervention/Generer//
        [HttpPost]
        public ActionResult GenererFICHE(Cr collection)
        {
            try
            {
                if (collection.IdCases != String.Empty && collection.IdCases != "0")
                {
                    var id = int.Parse(collection.IdCases);
                    DateTime dateCommunication = DateTime.Parse(collection.DateIntervention);

                    if (db.Crmcli_ValidateFiches.Where(a => a.ID_Cases == id && a.Date_Comm.Value.Day == dateCommunication.Day
                        && a.Date_Comm.Value.Month == dateCommunication.Month && a.Date_Comm.Value.Year == dateCommunication.Year).Count() != 0)
                    {
                        var CommValidate = db.Crmcli_ValidateFiches.Where(a => a.ID_Cases == id && a.Date_Comm.Value.Day == dateCommunication.Day
                        && a.Date_Comm.Value.Month == dateCommunication.Month && a.Date_Comm.Value.Year == dateCommunication.Year).FirstOrDefault();

                        string fullName = "";
                        var dateForName = "";
                        var dateForTEST = "";

                        var client = "";
                        var modules = "";
                        var date = "";
                        var hdeb = CommValidate.Debut.Value;
                        var hfin = CommValidate.Fin.Value;
                        var sujet = "";

                        //SIGNATAIRE//
                        var cltPRESENT = "";
                        if (db.Person.Where(a => a.Pers_PersonId == CommValidate.ID_Pers_Validateur).Count() != 0)
                        {
                            var pers = db.Person.Where(a => a.Pers_PersonId == CommValidate.ID_Pers_Validateur).FirstOrDefault();

                            var salutation = "";
                            if (db.Custom_Captions.Where(a => a.Capt_Family.ToLower() == "pers_salutation" && a.Capt_Code == pers.Pers_Salutation && a.Capt_Deleted != 1).Count() != 0)
                            {
                                var sal = db.Custom_Captions.Where(a => a.Capt_Family.ToLower() == "pers_salutation" && a.Capt_Code == pers.Pers_Salutation && a.Capt_Deleted != 1).FirstOrDefault();
                                salutation = sal.Capt_FR + " ";
                            }
                            else
                            {
                                if (pers.Pers_Salutation != null)
                                {
                                    if (pers.Pers_Salutation.ToLower().Contains("mr"))
                                    {
                                        if (pers.Pers_Salutation.ToLower() == "mrs.")
                                        {
                                            salutation = "Mme. ";
                                        }
                                        else if (pers.Pers_Salutation.ToLower() == "mr.")
                                        {
                                            salutation = "M. ";
                                        }
                                    }
                                    else if (pers.Pers_Salutation.ToLower().Contains("mme"))
                                    {
                                        salutation = "Mme. ";
                                    }
                                    else if (pers.Pers_Salutation.ToLower().Contains("miss"))
                                    {
                                        salutation = "Mme. ";
                                    }
                                }
                            }

                            cltPRESENT = string.Format("{0}{1} {2}-{3}", salutation, pers.Pers_LastName, pers.Pers_FirstName, pers.Pers_Title);
                        }

                        var CONSULTINTERV = "";

                        // Convert base 64 string to byte[]
                        string imagePATHSIGN = "";
                        byte[] imageBytes = null;
                        Image image = null;

                        if (db.Cases.Where(a => a.Case_CaseId == id).Count() != 0)
                        {
                            var cases = db.Cases.Where(a => a.Case_CaseId == id).FirstOrDefault();
                            //GET SIGNATURE IMAGE//
                            if (db.Crmcli_Signatures.Where(a => a.ID_Company == cases.Case_PrimaryCompanyId &&
                                (a.ID_Person1 == CommValidate.ID_Pers_Validateur || a.ID_Person2 == CommValidate.ID_Pers_Validateur ||
                                a.ID_Person3 == CommValidate.ID_Pers_Validateur || a.ID_Person4 == CommValidate.ID_Pers_Validateur)).Count() != 0)
                            {
                                var signTota = db.Crmcli_Signatures.Where(a => a.ID_Company == cases.Case_PrimaryCompanyId &&
                                (a.ID_Person1 == CommValidate.ID_Pers_Validateur || a.ID_Person2 == CommValidate.ID_Pers_Validateur ||
                                a.ID_Person3 == CommValidate.ID_Pers_Validateur || a.ID_Person4 == CommValidate.ID_Pers_Validateur)).FirstOrDefault();

                                if (signTota.ID_Person1 == CommValidate.ID_Pers_Validateur)
                                {
                                    //Convert Base64 String to byte[]
                                    string PPath = string.Format("{0}signbin{1}.jpg", Server.MapPath("~/SIGNCLIENTS/"), CommValidate.ID_Pers_Validateur.ToString());

                                    string base64String = Convert.ToBase64String(signTota.Sign1);

                                    if (System.IO.File.Exists(PPath))
                                    {
                                        System.IO.File.Delete(PPath);
                                    }

                                    imageBytes = null;
                                    image = null;

                                    imageBytes = Convert.FromBase64String(base64String);
                                    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                    //Convert byte[] to Image
                                    ms.Write(imageBytes, 0, imageBytes.Length);
                                    image = Image.FromStream(ms, true);
                                    image.Save(string.Format("{0}signbin{1}.jpg", Server.MapPath("~/SIGNCLIENTS/"), CommValidate.ID_Pers_Validateur.ToString()));
                                    image = null;
                                    ms.Close();

                                    imagePATHSIGN = string.Format("{0}signbin{1}.jpg", Server.MapPath("~/SIGNCLIENTS/"), CommValidate.ID_Pers_Validateur.ToString());
                                }
                                else if (signTota.ID_Person2 == CommValidate.ID_Pers_Validateur)
                                {
                                    //Convert Base64 String to byte[]
                                    string PPath = string.Format("{0}signbin{1}.jpg", Server.MapPath("~/SIGNCLIENTS/"), CommValidate.ID_Pers_Validateur.ToString());

                                    string base64String = Convert.ToBase64String(signTota.Sign2);

                                    if (System.IO.File.Exists(PPath))
                                    {
                                        System.IO.File.Delete(PPath);
                                    }

                                    imageBytes = null;
                                    image = null;

                                    imageBytes = Convert.FromBase64String(base64String);
                                    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                    //Convert byte[] to Image
                                    ms.Write(imageBytes, 0, imageBytes.Length);
                                    image = Image.FromStream(ms, true);
                                    image.Save(string.Format("{0}signbin{1}.jpg", Server.MapPath("~/SIGNCLIENTS/"), CommValidate.ID_Pers_Validateur.ToString()));
                                    image = null;
                                    ms.Close();

                                    imagePATHSIGN = string.Format("{0}signbin{1}.jpg", Server.MapPath("~/SIGNCLIENTS/"), CommValidate.ID_Pers_Validateur.ToString());
                                }
                                else if (signTota.ID_Person3 == CommValidate.ID_Pers_Validateur)
                                {
                                    //Convert Base64 String to byte[]
                                    string PPath = string.Format("{0}signbin{1}.jpg", Server.MapPath("~/SIGNCLIENTS/"), CommValidate.ID_Pers_Validateur.ToString());

                                    string base64String = Convert.ToBase64String(signTota.Sign3);

                                    if (System.IO.File.Exists(PPath))
                                    {
                                        System.IO.File.Delete(PPath);
                                    }

                                    imageBytes = null;
                                    image = null;

                                    imageBytes = Convert.FromBase64String(base64String);
                                    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                    //Convert byte[] to Image
                                    ms.Write(imageBytes, 0, imageBytes.Length);
                                    image = Image.FromStream(ms, true);
                                    image.Save(string.Format("{0}signbin{1}.jpg", Server.MapPath("~/SIGNCLIENTS/"), CommValidate.ID_Pers_Validateur.ToString()));
                                    image = null;
                                    ms.Close();

                                    imagePATHSIGN = string.Format("{0}signbin{1}.jpg", Server.MapPath("~/SIGNCLIENTS/"), CommValidate.ID_Pers_Validateur.ToString());
                                }
                                else if (signTota.ID_Person4 == CommValidate.ID_Pers_Validateur)
                                {
                                    //Convert Base64 String to byte[]
                                    string PPath = string.Format("{0}signbin{1}.jpg", Server.MapPath("~/SIGNCLIENTS/"), CommValidate.ID_Pers_Validateur.ToString());

                                    string base64String = Convert.ToBase64String(signTota.Sing4);

                                    if (System.IO.File.Exists(PPath))
                                    {
                                        System.IO.File.Delete(PPath);
                                    }

                                    imageBytes = null;
                                    image = null;

                                    imageBytes = Convert.FromBase64String(base64String);
                                    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                    //Convert byte[] to Image
                                    ms.Write(imageBytes, 0, imageBytes.Length);
                                    image = Image.FromStream(ms, true);
                                    image.Save(string.Format("{0}signbin{1}.jpg", Server.MapPath("~/SIGNCLIENTS/"), CommValidate.ID_Pers_Validateur.ToString()));
                                    image = null;
                                    ms.Close();

                                    imagePATHSIGN = string.Format("{0}signbin{1}.jpg", Server.MapPath("~/SIGNCLIENTS/"), CommValidate.ID_Pers_Validateur.ToString());
                                }
                            }

                            if (collection.Client != null)
                                client = collection.Client;

                            if (collection.Module != null)
                                modules = collection.Module;

                            if (collection.DateIntervention != null)
                                date = dateCommunication.ToShortDateString();

                            if (collection.Sujet != null)
                                sujet = collection.Sujet;

                            if (collection.CONSULTINTERV != null)
                                CONSULTINTERV = collection.CONSULTINTERV;

                            dateForTEST = dateCommunication.ToShortDateString();

                            if (dateForTEST.Contains("/"))
                            {
                                dateForName = dateForTEST.Replace("/", "");
                            }

                            string NameAgent = "";
                            if (Session["UserId"] != null)
                            {
                                int idAgent = int.Parse(Session["UserId"].ToString());

                                if (db.users.Where(a => a.User_UserId == idAgent && a.User_Deleted != 1).Count() != 0)
                                {
                                    NameAgent = db.users.Where(a => a.User_UserId == idAgent && a.User_Deleted != 1).FirstOrDefault().User_FirstName.ToUpper();
                                }
                            }

                            CreatWordDocFICHE(string.Format("FICHE_{0}_{1}_{2}.docx", client, NameAgent, dateForName), id, client, modules, date, hdeb.ToString(), hfin.ToString(), sujet, cltPRESENT/*, image*/);

                            //Generation download//

                            fullName = string.Format("{0}FICHE_{1}_{2}_{3}.docx", Server.MapPath("~/FICHESTECH/"), client, NameAgent, dateForName);


                            ////----------------------SIGNATURE--------------------------------
                            pathFileTEST = fullName;

                            string imagePath = imagePATHSIGN;

                            server.LoadDocument(pathFileTEST, DocumentFormat.Undefined);

                            DevExpress.XtraRichEdit.API.Native.Document doc = server.Document;

                            // Unité de mésure pouce (1" = 2.54cm)
                            doc.Unit = DevExpress.Office.DocumentUnit.Inch;

                            // Shape sary
                            DevExpress.XtraRichEdit.API.Native.Shape rectangle = doc.Shapes.InsertPicture(doc.Range.End, DocumentImageSource.FromFile(imagePath));

                            //width et height.
                            rectangle.ScaleX = 0.15f;
                            rectangle.ScaleY = 0.1f;

                            rectangle.RelativeHorizontalPosition = ShapeRelativeHorizontalPosition.Page;
                            rectangle.RelativeVerticalPosition = ShapeRelativeVerticalPosition.Page;

                            //Emplacement
                            rectangle.Offset = new PointF(8.3f, 7.1f);

                            server.Document.SaveDocument(pathFileTEST, DocumentFormat.OpenXml);

                            ////----------------------TACHES--------------------------------
                            pathFileTEST = fullName;


                            server.LoadDocument(pathFileTEST, DocumentFormat.Undefined);

                            DevExpress.XtraRichEdit.API.Native.Document document = server.Document;

                            // Unité de mésure pouce (1" = 2.54cm)
                            document.Unit = DevExpress.Office.DocumentUnit.Inch;

                            DocumentPosition myPosition = document.Paragraphs[0].Range.Start;

                            DevExpress.XtraRichEdit.API.Native.Shape myTextBox = document.Shapes.InsertTextBox(myPosition);

                            myTextBox.ScaleX = 7.6f;
                            myTextBox.ScaleY = 5f;

                            //Access the text box content
                            SubDocument textBoxDocument = document.Shapes[0].TextBox.Document;

                            //Insert text to the text box
                            if (TACHECON.ToString() != String.Empty)
                            {
                                textBoxDocument.AppendText(TACHECON);
                            }

                            CharacterProperties cp = textBoxDocument.BeginUpdateCharacters(textBoxDocument.Range);
                            cp.ForeColor = Color.Black;
                            cp.FontName = "Times New Roman";
                            cp.FontSize = 9;
                            textBoxDocument.EndUpdateCharacters(cp);

                            myTextBox.RelativeHorizontalPosition = ShapeRelativeHorizontalPosition.Page;
                            myTextBox.RelativeVerticalPosition = ShapeRelativeVerticalPosition.Page;

                            //Emplacement
                            myTextBox.Offset = new PointF(0.48f, 2.1f);

                            server.Document.SaveDocument(pathFileTEST, DocumentFormat.OpenXml);

                            ////----------------------OBSERVATION--------------------------------
                            pathFileTEST = fullName;


                            server.LoadDocument(pathFileTEST, DocumentFormat.Undefined);

                            DevExpress.XtraRichEdit.API.Native.Document documentO = server.Document;

                            // Unité de mésure pouce (1" = 2.54cm)
                            documentO.Unit = DevExpress.Office.DocumentUnit.Inch;

                            DocumentPosition myPositionO = documentO.Paragraphs[0].Range.Start;

                            DevExpress.XtraRichEdit.API.Native.Shape myTextBoxO = documentO.Shapes.InsertTextBox(myPositionO);

                            myTextBoxO.ScaleX = 7.6f;
                            myTextBoxO.ScaleY = 5f;

                            //Access the text box content
                            SubDocument textBoxDocumentO = documentO.Shapes[0].TextBox.Document;

                            //Insert text to the text box
                            if (OBSERVATIONCON.ToString() != String.Empty)
                            {
                                textBoxDocumentO.AppendText(OBSERVATIONCON);
                            }

                            CharacterProperties cpO = textBoxDocumentO.BeginUpdateCharacters(textBoxDocumentO.Range);
                            cpO.ForeColor = Color.Black;
                            cpO.FontName = "Times New Roman";
                            cpO.FontSize = 9;
                            textBoxDocumentO.EndUpdateCharacters(cpO);

                            myTextBoxO.RelativeHorizontalPosition = ShapeRelativeHorizontalPosition.Page;
                            myTextBoxO.RelativeVerticalPosition = ShapeRelativeVerticalPosition.Page;

                            //Emplacement
                            myTextBoxO.Offset = new PointF(5.9f, 2.1f);

                            server.Document.SaveDocument(pathFileTEST, DocumentFormat.OpenXml);

                            string NamePdf = "";
                            if (System.IO.File.Exists(pathFileTEST))
                            {
                                RichEditDocumentServer wordProcessor = new RichEditDocumentServer();
                                wordProcessor.LoadDocument(pathFileTEST, DocumentFormat.OpenXml);

                                //Specify export options:
                                PdfExportOptions options = new PdfExportOptions();
                                options.Compressed = false;
                                options.ImageQuality = PdfJpegImageQuality.Highest;

                                //Export the document to the stream:
                                using (FileStream pdfFileStream = new FileStream(string.Format("{0}FICHE_{1}_{2}_{3}.pdf", Server.MapPath("~/FICHESTECH/"), client, NameAgent, dateForName), FileMode.Create))
                                {
                                    wordProcessor.ExportToPdf(pdfFileStream, options);
                                }

                                NamePdf = string.Format("FICHE_{0}_{1}_{2}.pdf", client, NameAgent, dateForName);

                                return Content(NamePdf);
                            }
                            else
                            {
                                return Content("Erreur");
                            }
                        }
                        else
                            return Content("Erreur");
                    }
                    else
                    {
                        return Content("Erreur! Une fiche d'intervention non validée ne peut pas être générer");
                    }
                }
                else
                    return Content("Erreur!");
            }
            catch (Exception)
            {
                return Content("Erreur!");
            }
        }

        private void CreatWordDocFICHE(string filename, int id, string client, string module, string date, string hdeb, string hfin, string sujet, string cltPRESENT/*, Image image*/)
        {
            Session["FILENAME"] = filename;

            var pathFile = Server.MapPath("~/FICHESTECH/" + filename);

            string nameDir = "";
            if (Session["UserId"] != null)
            {
                nameDir = Session["UserId"].ToString();
            }

            try
            {
                var newPath = "";

                if (module.ToUpper().Contains("TOM"))
                {
                    if (System.IO.File.Exists(Server.MapPath(string.Format("~/TEMPLATES/TEMPFICHES/FTITOM{0}.zip", nameDir))) == false)
                    {
                        string sourceFile = Server.MapPath("~/FTITOM.zip");
                        string destinationFile = Server.MapPath(string.Format("~/TEMPLATES/TEMPFICHES/FTITOM{0}.zip", nameDir));
                        System.IO.File.Copy(sourceFile, destinationFile);
                    }

                    newPath = Server.MapPath("~/FICHESTECH/FTITOM.unzipped" + nameDir);

                    using (ZipArchive archive = ZipFile.OpenRead(Server.MapPath(string.Format("~/TEMPLATES/TEMPFICHES/FTITOM{0}.zip", nameDir))))
                    {
                        if (Directory.Exists(newPath))
                        {
                            Directory.Delete(newPath, true);
                            archive.ExtractToDirectory(newPath);
                        }
                        else
                            archive.ExtractToDirectory(newPath);
                    }
                }
                else
                {
                    if (System.IO.File.Exists(Server.MapPath(string.Format("~/TEMPLATES/TEMPFICHES/FTISAGE{0}.zip", nameDir))) == false)
                    {
                        string sourceFile = Server.MapPath("~/FTISAGE.zip");
                        string destinationFile = Server.MapPath(string.Format("~/TEMPLATES/TEMPFICHES/FTISAGE{0}.zip", nameDir));
                        System.IO.File.Copy(sourceFile, destinationFile);
                    }

                    newPath = Server.MapPath("~/FICHESTECH/FTISAGE.unzipped" + nameDir);

                    using (ZipArchive archive = ZipFile.OpenRead(Server.MapPath(string.Format("~/TEMPLATES/TEMPFICHES/FTISAGE{0}.zip", nameDir))))
                    {
                        if (Directory.Exists(newPath))
                        {
                            Directory.Delete(newPath, true);
                            archive.ExtractToDirectory(newPath);
                        }
                        else
                            archive.ExtractToDirectory(newPath);
                    }
                }

                string path = Path.Combine(newPath + @"\word\document.xml");

                string str = System.IO.File.ReadAllText(path);

                var TACHES = "";
                var OBSERVATION = "";

                //FIND/REPLACE => CORPS
                str = str.Replace("VALDATE", date);
                str = str.Replace("VALMODULE", module);
                str = str.Replace("VALHDEB", hdeb);
                str = str.Replace("VALHFIN", hfin);
                str = str.Replace("VALCLIENT", client);
                str = str.Replace("VALBES", sujet);

                str = str.Replace("VALSIGN", cltPRESENT);

                //Numéro Fiche//
                int idCa = id;

                DateTime Dte = DateTime.Parse(date.ToString());

                var numString = "";
                int numFiche = 0;

                if (db.Crmcli_ValidateFiches.Where(a => a.ID_Cases == idCa &&
                    (a.Date_Comm.Value.Day == Dte.Day && a.Date_Comm.Value.Month == Dte.Month && a.Date_Comm.Value.Year == Dte.Year)).Count() != 0)
                {
                    var getNF = db.Crmcli_ValidateFiches.Where(a => a.ID_Cases == idCa &&
                    (a.Date_Comm.Value.Day == Dte.Day && a.Date_Comm.Value.Month == Dte.Month && a.Date_Comm.Value.Year == Dte.Year)).FirstOrDefault();

                    numFiche = int.Parse(getNF.NumFiche);
                }

                //Format numéro à 5 chiffres//
                if (numFiche <= 99999)
                {
                    if (numFiche.ToString().Length == 1)
                    {
                        numString = "0000" + numFiche;
                    }
                    else if (numFiche.ToString().Length == 2)
                    {
                        numString = "000" + numFiche;
                    }
                    else if (numFiche.ToString().Length == 3)
                    {
                        numString = "00" + numFiche;
                    }
                    else if (numFiche.ToString().Length == 4)
                    {
                        numString = "0" + numFiche;
                    }
                    else if (numFiche.ToString().Length == 5)
                    {
                        numString = numFiche.ToString();
                    }
                }

                str = str.Replace("VANUM", numString);

                var etape = "";
                var facturabliliteY = "";
                var facturabliliteN = "";
                var TotalConsultant = "";

                List<string> listSOFT = new List<string>();
                var TotalSOFT = "";

                List<List<string>> listTaches = new List<List<string>>();

                if (id != 0)
                {
                    //CASES//
                    if (db.Cases.Where(a => a.Case_CaseId == id && a.Case_Deleted != 1).Count() != 0)
                    {
                        var cases = db.Cases.Where(a => a.Case_CaseId == id && a.Case_Deleted != 1).FirstOrDefault();

                        //Etapes//
                        if (cases.Case_Status != null)
                        {
                            if (db.Custom_Captions.Where(a => a.Capt_Family.ToLower() == "case_status" && a.Capt_Code == cases.Case_Status && a.Capt_Deleted != 1).Count() != 0)
                            {
                                var sta = db.Custom_Captions.Where(a => a.Capt_Family.ToLower() == "case_status" && a.Capt_Code == cases.Case_Status && a.Capt_Deleted != 1).FirstOrDefault();
                                etape = sta.Capt_FR;
                            }
                        }

                        //Facturabilité//
                        if (cases.case_prestation != null)
                        {
                            if (db.Custom_Captions.Where(a => a.Capt_Family.ToLower() == "case_prestation" && a.Capt_Code == cases.case_prestation && a.Capt_Deleted != 1).Count() != 0)
                            {
                                var sta = db.Custom_Captions.Where(a => a.Capt_Family.ToLower() == "case_prestation" && a.Capt_Code == cases.case_prestation && a.Capt_Deleted != 1).FirstOrDefault();
                                if (sta.Capt_Code == "NFC")
                                {
                                    facturabliliteN = "X";
                                }
                                else if (sta.Capt_Code == "FC")
                                {
                                    facturabliliteY = "X";
                                }
                            }
                        }

                        //VALCONSUSOFT = total des consultants SOFTWELL//
                        //COMMUNICATION//
                        DateTime dateTest = DateTime.Parse(date).Date;

                        if (db.Communication.Where(a => a.Comm_CaseId == id && a.Comm_Deleted != 1 &&
                            (a.Comm_ToDateTime.Value.Day == dateTest.Day && a.Comm_ToDateTime.Value.Month == dateTest.Month && a.Comm_ToDateTime.Value.Year == dateTest.Year)).Count() != 0)
                        {
                            foreach (var x in db.Communication.Where(a => a.Comm_CaseId == id && a.Comm_Deleted != 1 &&
                                (a.Comm_ToDateTime.Value.Day == dateTest.Day && a.Comm_ToDateTime.Value.Month == dateTest.Month && a.Comm_ToDateTime.Value.Year == dateTest.Year)).ToList())
                            {
                                var listTache = new List<string>();

                                if (x.Comm_ToDateTime.Value.Date == dateTest)
                                {
                                    //Intervenants SOFTWELL et CONSULTANT intervenants
                                    string[] separators = { "," };

                                    if (x.comm_userintervenant != null)//Agents
                                    {
                                        string listUser = x.comm_userintervenant.ToString();

                                        string[] agentL = listUser.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                                        foreach (var ag in agentL)
                                        {
                                            var listConsultantSOFT = new List<string>();

                                            int idA = int.Parse(ag);

                                            if (db.users.Where(a => a.User_UserId == idA && a.User_Deleted != 1).Count() != 0)
                                            {
                                                var userAssig = db.users.Where(a => a.User_UserId == idA && a.User_Deleted != 1).FirstOrDefault();

                                                listConsultantSOFT.Add(userAssig.User_FirstName);
                                            }

                                            if (listSOFT.Contains(listConsultantSOFT.FirstOrDefault()) == false)
                                            {
                                                listSOFT.AddRange(listConsultantSOFT);
                                            }
                                        }
                                    }

                                    //TACHE//
                                    if (x.Comm_Note != null)//Tache
                                    {
                                        listTache.Add(x.Comm_Note);
                                    }
                                    else
                                        listTache.Add("");

                                    if (x.comm_obs1 != null)//Observation
                                    {
                                        listTache.Add(x.comm_obs1);
                                    }
                                    else
                                        listTache.Add("");

                                    listTaches.Add(listTache);
                                }
                            }

                            if (listTaches.Count() != 0)
                            {
                                foreach (List<string> lstTa in listTaches)
                                {
                                    if (lstTa.Count() != 0)
                                    {
                                        //Tache//
                                        var tache = lstTa[0];

                                        //observation//
                                        var observation = lstTa[1];

                                        TACHES += tache;

                                        OBSERVATION += observation;
                                    }
                                }
                            }
                        }
                    }
                }

                if (TACHES.Contains("\r\n"))
                {
                    TACHECON = TACHES.Replace("\r\n", "\n");
                }
                else
                    TACHECON = TACHES;

                if (OBSERVATION.Contains("\r\n"))
                {
                    OBSERVATIONCON = OBSERVATION.Replace("\r\n", "\n");
                }
                else
                    OBSERVATIONCON = OBSERVATION;

                //var TACHELIGNE = "";
                //if (TACHES.Contains("\r\n"))
                //{
                //    string note = TACHES.ToString();
                //    string[] separators = { "\r\n" };
                //    string[] nt = note.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                //    foreach (var dst in nt)
                //    {
                //        if (module.ToUpper().Contains("TOM"))
                //        {
                //            TACHELIGNE += "<w:p w14:paraId=\"2E439309\" w14:textId=\"77777777\" w:rsidR=\"009814FF\" w:rsidRPr=\"00B56A14\" w:rsidRDefault=\"009814FF\" w:rsidP=\"009814FF\"><w:pPr><w:pStyle w:val=\"Paragraphedeliste\"/><w:numPr><w:ilvl w:val=\"0\"/><w:numId w:val=\"6\"/></w:numPr><w:rPr><w:rFonts w:ascii=\"Times New Roman\" w:hAnsi=\"Times New Roman\" w:cs=\"Times New Roman\"/><w:sz w:val=\"20\"/><w:szCs w:val=\"20\"/></w:rPr></w:pPr><w:r w:rsidRPr=\"00B56A14\"><w:rPr><w:rFonts w:ascii=\"Times New Roman\" w:hAnsi=\"Times New Roman\" w:cs=\"Times New Roman\"/><w:sz w:val=\"20\"/><w:szCs w:val=\"20\"/></w:rPr><w:t>"
                //            + dst.Replace("-", "")
                //            + "</w:t></w:r></w:p>";
                //        }
                //        else
                //        {
                //            TACHELIGNE += "<w:p w14:paraId=\"1D4B0915\" w14:textId=\"77777777\" w:rsidR=\"006A2CE6\" w:rsidRPr=\"00B56A14\" w:rsidRDefault=\"006A2CE6\" w:rsidP=\"006A2CE6\"><w:pPr><w:pStyle w:val=\"Paragraphedeliste\"/><w:numPr><w:ilvl w:val=\"0\"/><w:numId w:val=\"6\"/></w:numPr><w:rPr><w:rFonts w:ascii=\"Times New Roman\" w:hAnsi=\"Times New Roman\" w:cs=\"Times New Roman\"/><w:sz w:val=\"20\"/><w:szCs w:val=\"20\"/></w:rPr></w:pPr><w:r w:rsidRPr=\"00B56A14\"><w:rPr><w:rFonts w:ascii=\"Times New Roman\" w:hAnsi=\"Times New Roman\" w:cs=\"Times New Roman\"/><w:sz w:val=\"20\"/><w:szCs w:val=\"20\"/></w:rPr><w:t>"
                //            + dst.Replace("-", "")
                //            + "</w:t></w:r></w:p>";
                //        }
                //    }
                //}

                //var OBSERVATIONLIGNE = "";
                //if (OBSERVATION.Contains("\r\n"))
                //{
                //    string note = OBSERVATION.ToString();
                //    string[] separators = { "\r\n" };
                //    string[] nt = note.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                //    foreach (var dst in nt)
                //    {
                //        if (module.ToUpper().Contains("TOM"))
                //        {
                //            OBSERVATIONLIGNE += "<w:p w14:paraId=\"4256F8D1\" w14:textId=\"7D7B076B\" w:rsidR=\"009814FF\" w:rsidRPr=\"00B56A14\" w:rsidRDefault=\"009814FF\" w:rsidP=\"009814FF\"><w:pPr><w:pStyle w:val=\"Paragraphedeliste\"/><w:numPr><w:ilvl w:val=\"0\"/><w:numId w:val=\"6\"/></w:numPr><w:rPr><w:rFonts w:ascii=\"Times New Roman\" w:hAnsi=\"Times New Roman\" w:cs=\"Times New Roman\"/><w:sz w:val=\"20\"/><w:szCs w:val=\"20\"/></w:rPr></w:pPr><w:r><w:rPr><w:rFonts w:ascii=\"Times New Roman\" w:hAnsi=\"Times New Roman\" w:cs=\"Times New Roman\"/><w:sz w:val=\"20\"/><w:szCs w:val=\"20\"/></w:rPr><w:t>"
                //            + dst.Replace("-","")
                //            + "</w:t></w:r></w:p>";
                //        }
                //        else
                //        {
                //            OBSERVATIONLIGNE += "<w:p w14:paraId=\"62A82320\" w14:textId=\"77777777\" w:rsidR=\"006A2CE6\" w:rsidRPr=\"00B56A14\" w:rsidRDefault=\"006A2CE6\" w:rsidP=\"006A2CE6\"><w:pPr><w:pStyle w:val=\"Paragraphedeliste\"/><w:numPr><w:ilvl w:val=\"0\"/><w:numId w:val=\"6\"/></w:numPr><w:rPr><w:rFonts w:ascii=\"Times New Roman\" w:hAnsi=\"Times New Roman\" w:cs=\"Times New Roman\"/><w:sz w:val=\"20\"/><w:szCs w:val=\"20\"/></w:rPr></w:pPr><w:r><w:rPr><w:rFonts w:ascii=\"Times New Roman\" w:hAnsi=\"Times New Roman\" w:cs=\"Times New Roman\"/><w:sz w:val=\"20\"/><w:szCs w:val=\"20\"/></w:rPr><w:t>"
                //            + dst.Replace("-", "")
                //            + "</w:t></w:r></w:p>";
                //        }
                //    }
                //}

                //str = str.Replace("TTTATTT", TACHELIGNE);
                //str = str.Replace("OOOBOOO", OBSERVATIONLIGNE);

                str = str.Replace("CONSUSOFT", TotalConsultant);

                str = str.Replace("VETAT", etape);//ETAT (Etape)

                //facturable ou non//
                str = str.Replace("VNOT", facturabliliteN);
                str = str.Replace("VYES", facturabliliteY);

                if (listSOFT.Count() != 0)//PArticipants
                {
                    foreach (var x in listSOFT)
                    {
                        TotalSOFT += x + ",";
                    }
                }
                str = str.Replace("VPAR", TotalSOFT);

                //NOTE QUESTIONNAIRE DE SATISFACTION//
                if (db.Cases.Where(a => a.Case_CaseId == idCa && a.Case_Deleted != 1 && a.case_numdemande != null).Count() != 0)
                {
                    var numDemande = db.Cases.Where(a => a.Case_CaseId == idCa && a.Case_Deleted != 1 && a.case_numdemande != null).FirstOrDefault();

                    var getIdDemande = db.Crmcli_Demandes.Where(a => a.NumeroDemande == numDemande.case_numdemande).FirstOrDefault();

                    if (db.Crmcli_Quests.Where(a => a.ID_Demandes == getIdDemande.ID && a.Date_Comm.Value.Day == Dte.Day
                    && a.Date_Comm.Value.Month == Dte.Month && a.Date_Comm.Value.Year == Dte.Year).Count() != 0)
                    {
                        var resP = db.Crmcli_Quests.Where(a => a.ID_Demandes == getIdDemande.ID && a.Date_Comm.Value.Day == Dte.Day
                        && a.Date_Comm.Value.Month == Dte.Month && a.Date_Comm.Value.Year == Dte.Year).FirstOrDefault();

                        str = str.Replace("VCDE", resP.ClariteExplications.ToString());
                        str = str.Replace("VDDT", resP.DelaisTraitements.ToString());
                        str = str.Replace("VQDS", resP.QualiteSolutions.ToString());

                        str = str.Replace("VRCT", resP.Reactivites.ToString());
                        str = str.Replace("VDIP", resP.Disponibilites.ToString());
                        str = str.Replace("VPNE", resP.Ponctualites.ToString());
                        str = str.Replace("VCOP", resP.Competences.ToString());

                        str = str.Replace("CVCOM", resP.Commentaires.ToString());
                    }
                    else
                    {
                        str = str.Replace("VCDE", "PAS DE NOTE");
                        str = str.Replace("VDDT", "PAS DE NOTE");
                        str = str.Replace("VQDS", "PAS DE NOTE");

                        str = str.Replace("VRCT", "PAS DE NOTE");
                        str = str.Replace("VDIP", "PAS DE NOTE");
                        str = str.Replace("VPNE", "PAS DE NOTE");
                        str = str.Replace("VCOP", "PAS DE NOTE");

                        str = str.Replace("CVCOM", "PAS DE NOTE");
                    }
                }

                System.IO.File.WriteAllText(path, str);

                if (System.IO.File.Exists(pathFile))
                {
                    System.IO.File.Delete(pathFile);
                    ZipFile.CreateFromDirectory(newPath, Server.MapPath("~/FICHESTECH/" + filename));
                }
                else
                {
                    ZipFile.CreateFromDirectory(newPath, Server.MapPath("~/FICHESTECH/" + filename));
                }
            }
            catch (Exception)
            {
                //var x = false;
            }
        }
    }
}
