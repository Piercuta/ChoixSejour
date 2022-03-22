﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChoixSejour.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChoixSejour.Controllers
{
    [Authorize]
    public class SejourController : Controller
    {
        private IDal dal;

        public SejourController()
        {
            this.dal = new Dal();
        }

        public ActionResult Index()
        {
            List<Sejour> listeDesRestaurants = dal.ObtientTousLesSejours();
            return View(listeDesRestaurants);
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
                ModelState.AddModelError("Nom", "Ce nom de restaurant existe déjà");
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
                Sejour restaurant = dal.ObtientTousLesSejours().FirstOrDefault(r => r.Id == id.Value);
                if (restaurant == null)
                    return View("Error");
                return View(restaurant);
            }
            else
                return NotFound();
        }

        [HttpPost]
        public ActionResult Modifier(Sejour sejour)
        {
            if (!ModelState.IsValid)
                return View(sejour);
            dal.ModifierSejour(sejour.Id, sejour.Lieu, sejour.Telephone, sejour.Ville, sejour.Description);
            return RedirectToAction("Index");
        }

        public ActionResult Supprimer(int id)
        {
            dal.SupprimerSejour(id);
            return RedirectToAction("Index");
        }
    }
}