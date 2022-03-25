using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using ChoixSejour.Models;
using ChoixSejour.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChoixSejour.Controllers
{
	public class VoteController : Controller
	{
        private IDal dal;
        public VoteController()
        {
            dal = new Dal();
        }
        public ActionResult Index(int id)
        {
            SejourVoteViewModel viewModel = new SejourVoteViewModel
            {
                ListeDesSejours = dal.ObtientTousLesSejours().Select(r => new SejourCheckBoxViewModel { Id = r.Id, LieuEtTelephone = string.Format("{0} ({1})", r.Lieu, r.Telephone) }).ToList()
            };
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (dal.ADejaVote(id, userId))
            {
                return RedirectToAction("AfficheResultat", new { id = id });
            }
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(SejourVoteViewModel viewModel, int id)
        {
            if (!ModelState.IsValid)
                return View(viewModel);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            Utilisateur utilisateur = dal.ObtenirUtilisateur(userId);
            if (utilisateur == null)
                return new NotFoundResult();
            foreach (SejourCheckBoxViewModel sejourCheckBoxViewModel in viewModel.ListeDesSejours.Where(r => r.EstSelectionne))
            {
                dal.AjouterVote(id, sejourCheckBoxViewModel.Id, utilisateur.Id);
            }
            return RedirectToAction("AfficheResultat", new { id = id });
        }

        public ActionResult AfficheResultat(int id)
        {
           var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!dal.ADejaVote(id, userId))
            {
                return RedirectToAction("Index", new { id = id });
            }
            List<Resultats> resultats = dal.ObtenirLesResultats(id);
            return View(resultats.OrderByDescending(r => r.NombreDeVotes).ToList());
        }

        [Authorize (Roles = "Admin")]
        public ActionResult CreateSondage()
        {
            int idSondage = dal.CreerUnSondage();
            return RedirectToAction("Index", "Vote", new { id = idSondage });
        }

        public ActionResult ListeSondages()
        {
            List<Sondage> listSondages = dal.ObtenirLesSondages();
            return View(listSondages);
        }
    }
}
