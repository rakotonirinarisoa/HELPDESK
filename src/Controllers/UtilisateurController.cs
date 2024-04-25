using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Helpdesk.Controllers {
    public class UtilisateurController : Controller {
        ModelHELPD db = new ModelHELPD();

        //
        // GET: /Utilisateur/

        public ActionResult Index() {
            return View();
        }

        //
        // GET: /Utilisateur/Details/5

        public ActionResult Details(int id) {
            return View();
        }

        //
        // GET: /Utilisateur/Create

        public ActionResult Create() {
            return View();
        }

        //
        // POST: /Utilisateur/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection) {
            try {
                // TODO: Add insert logic here
                return RedirectToAction("Index");
            }
            catch {
                return View();
            }
        }

        //
        // GET: /Utilisateur/Edit/5

        public ActionResult Edit(int id) {
            return View();
        }

        //
        // POST: /Utilisateur/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection) {
            try {
                // TODO: Add update logic here
                return RedirectToAction("Index");
            }
            catch {
                return View();
            }
        }

        //
        // GET: /Utilisateur/Delete/5

        public ActionResult Delete(int id) {
            return View();
        }

        //
        // POST: /Utilisateur/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection) {
            try {
                // TODO: Add delete logic here
                return RedirectToAction("Index");
            }
            catch {
                return View();
            }
        }

        public ActionResult Logout() {
            Session.Clear();

            Session.Abandon();
            FormsAuthentication.SignOut();

            Session.RemoveAll();

            Session.Contents.RemoveAll();

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.AppendHeader("Pragma", "no-cache");
            //Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            //Response.Cache.SetNoStore();

            Response.Cache.SetExpires(DateTime.MinValue);
            Response.Cache.SetNoStore();

            return RedirectToAction("Login", "Utilisateur");
        }
    }
}
