using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Helpdesk.Controllers {
    public class HomeController : Controller {
        private readonly ModelHELPD _db;

        public HomeController() {
            _db = new ModelHELPD();
        }

        public async Task<ActionResult> Index(int id, string getxsoft) {
            try {
                Session["UserId"] = null;
                Session["NameSess"] = null;
                Session["IsAdmin"] = null;

                //var dep = "TECH TMT";
                //var tmt = _db.users.Where(n => n.User_Department == dep).Select(x=>x.User_UserId).ToList();
                var tmt = await _db.users.Where(n => n.user_deptsoft == "04").Select(x => x.User_UserId).ToListAsync();
                //if (id != 74 && id != 75 && id != 77 && id != 85 && id != 97 && id != 106 && id != 109 && id != 110 && id != 0)
                if (!tmt.Contains(id) && id != 0) {
                    var forSess = await _db.users.Where(a => a.User_UserId == id && a.User_Deleted != 1).FirstOrDefaultAsync();

                    Session["UserId"] = id.ToString();
                    Session["NameSess"] = string.Format("{0} {1}", forSess.User_FirstName, forSess.User_LastName);

                    int idUser = int.Parse(Session["UserId"].ToString());

                    if (await _db.Crmcli_Administrateurs.Where(a => a.ID_Pers == idUser).CountAsync() != 0) {
                        Session["IsAdmin"] = id.ToString();
                    }

                    return View();
                }
                else if (!string.IsNullOrEmpty(getxsoft)) {
                    string[] separators = { "," };
                    string[] agentL = getxsoft.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var ag in agentL) {
                        var forSess = await _db.users.Where(a => a.user_AddMac.Contains(ag) && a.User_Deleted != 1).FirstOrDefaultAsync();

                        if (forSess != null) {
                            Session["UserId"] = forSess.User_UserId.ToString();
                            Session["NameSess"] = string.Format("{0} {1}", forSess.User_FirstName, forSess.User_LastName);

                            int idUser = int.Parse(Session["UserId"].ToString());

                            if (await _db.Crmcli_Administrateurs.Where(a => a.ID_Pers == idUser).CountAsync() != 0) {
                                Session["IsAdmin"] = forSess.User_UserId.ToString();
                            }

                            return View();
                        }
                    }
                }
                else
                    return View();

                return View();
            }
            catch (Exception) {
                Session["UserId"] = null;
                Session["NameSess"] = null;

                return View();
            }
        }

        [HttpPost]
        public ActionResult Index(users model) {
            return View();
        }

        public ActionResult About() {
            ViewBag.Message = "Votre page de description d’application.";

            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Votre page de contact.";

            return View();
        }
    }
}
