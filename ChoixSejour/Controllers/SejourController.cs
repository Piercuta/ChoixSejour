using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ChoixSejour.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChoixSejour.Controllers
{
    [Authorize]
    public class SejourController : Controller
    {
        private IDal dal;
        private IWebHostEnvironment _webEnv;
        public SejourController(IWebHostEnvironment environment)
        {
            _webEnv = environment;
            this.dal = new Dal();
        }

        public ActionResult Index()
        {
            List<Sejour> listeDesSejours = dal.ObtientTousLesSejours();
            return View(listeDesSejours);
        }

        public ActionResult Creer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Creer(Sejour sejour)
        {
            if (dal.SejourExiste(sejour.Lieu))
            {
                ModelState.AddModelError("Nom", "Ce nom de sejour existe déjà");
                return View(sejour);
            }
            if (!ModelState.IsValid)
                return View(sejour);
            dal.CreerSejour(sejour.Lieu, sejour.Telephone, sejour.Ville, sejour.Description);
            return RedirectToAction("Index");
        }

        public ActionResult Modifier(int? id)
        {
            if (id.HasValue)
            {
                Sejour sejour = dal.ObtientTousLesSejours().FirstOrDefault(r => r.Id == id.Value);
                if (sejour == null)
                    return View("Error");

                //string fileName = sejour.ImagePath.Split('/').Last();
                //string uploads = Path.Combine(_webEnv.WebRootPath, "images");
                //string filePath = Path.Combine(uploads, fileName);
                //using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                //{
                //    var file = new FormFile(fileStream, 0, fileStream.Length, null, fileName);
                //    sejour.Image = file;

                //}
                return View(sejour);
            }
            else
                return NotFound();
        }

        [HttpPost]
        public ActionResult Modifier(Sejour sejour)
        {
            if (!ModelState.IsValid)
                return View(sejour);

            if (sejour.Image != null)
            {
                if(sejour.Image.Length != 0)
                {
                    string uploads = Path.Combine(_webEnv.WebRootPath, "images");
                    string filePath = Path.Combine(uploads, sejour.Image.FileName);
                    using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        sejour.Image.CopyTo(fileStream);
                    }
                    dal.ModifierSejour(sejour.Id, sejour.Lieu, sejour.Telephone, sejour.Ville, sejour.Description, "/images/" + sejour.Image.FileName);
                }
            }
            else
            {
                dal.ModifierSejour(sejour.Id, sejour.Lieu, sejour.Telephone, sejour.Ville, sejour.Description, sejour.ImagePath);
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Supprimer(int id)
        {
            dal.SupprimerSejour(id);
            return RedirectToAction("Index");
        }
    }
}