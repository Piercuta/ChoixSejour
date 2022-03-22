using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ChoixSejour.Models
{
	public class Dal : IDal
	{
		private BddContext _bddContext;

		public Dal()
		{
			this._bddContext = new BddContext();
		}

		public void Dispose()
		{
			this._bddContext.Dispose();
		}
		public void CreerSejour(string nom, string telephone, string ville, string description, int id=0)
		{

			Sejour sejourToAdd = new Sejour { Lieu = nom, Telephone = telephone, Ville = ville, Description = description };
			if(id != 0)
			{
				sejourToAdd.Id = id;
			}

			this._bddContext.Sejours.Add(sejourToAdd);
			this._bddContext.SaveChanges();
		}


		public List<Sejour> ObtientTousLesSejours()
		{
			List<Sejour> listeSejours = this._bddContext.Sejours.ToList();
			return listeSejours;
		}

		public void SupprimerSejour(int id)
		{
			Sejour sejourToDelete = this._bddContext.Sejours.Find(id);
			this._bddContext.Sejours.Remove(sejourToDelete);
			this._bddContext.SaveChanges();
		}

		public void SupprimerSejour(string nom, string telephone)
		{
			Sejour sejourToDelete = this._bddContext.Sejours.Where(r => r.Lieu == nom && r.Telephone == telephone).FirstOrDefault();
			if (sejourToDelete != null)
			{
				this._bddContext.Sejours.Remove(sejourToDelete);
				this._bddContext.SaveChanges();
			}
		}

		public void ModifierSejour(int id, string nom, string telephone, string ville, string description)
		{
			Sejour sejourToUpdate = this._bddContext.Sejours.Find(id);
			if (sejourToUpdate != null)
			{
				sejourToUpdate.Lieu = nom;
				sejourToUpdate.Telephone = telephone;
				sejourToUpdate.Ville = ville; 
				sejourToUpdate.Description = description;
				this._bddContext.SaveChanges();
			}
		}


		// addeed for authentification

		public Utilisateur AjouterUtilisateur(string prenom, string password, Role role=Role.ReadWrite)
		{
			string motDePasse = EncodeMD5(password);
			Utilisateur user = new Utilisateur() { Prenom = prenom, Password = motDePasse, Role=role };
			this._bddContext.Utilisateurs.Add(user);
			this._bddContext.SaveChanges();

			return user;
		}

		public Utilisateur Authentifier(string prenom, string password)
		{
			string motDePasse = EncodeMD5(password);
			Utilisateur user = this._bddContext.Utilisateurs.FirstOrDefault(u => u.Prenom == prenom && u.Password == motDePasse);
			return user;
		}

		public Utilisateur ObtenirUtilisateur(int id)
		{
			return this._bddContext.Utilisateurs.FirstOrDefault(u => u.Id == id);
		}

		public Utilisateur ObtenirUtilisateur(string idStr)
		{
			int id;
			if (int.TryParse(idStr, out id))
			{
				return this.ObtenirUtilisateur(id);
			}
			return null;
		}

		public static string EncodeMD5(string motDePasse)
		{
			string motDePasseSel = "ChoixSejour" + motDePasse + "ASP.NET MVC";
			return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.Default.GetBytes(motDePasseSel)));
		}

		public int CreerUnSondage()
		{
			Sondage sondage = new Sondage { Date = DateTime.Now };
			_bddContext.Sondages.Add(sondage);
			_bddContext.SaveChanges();
			return sondage.Id;
		}

		public void AjouterVote(int idSondage, int idSejour, int idUtilisateur)
		{
			Vote vote = new Vote
			{
				sejour = _bddContext.Sejours.First(r => r.Id == idSejour),
				Utilisateur = _bddContext.Utilisateurs.First(u => u.Id == idUtilisateur)
			};
			Sondage sondage = _bddContext.Sondages.First(s => s.Id == idSondage);
			if (sondage.Votes == null)
				sondage.Votes = new List<Vote>();
			sondage.Votes.Add(vote);
			_bddContext.SaveChanges();
		}

		public List<Resultats> ObtenirLesResultats(int idSondage)
		{
			List<Sejour> sejours = ObtientTousLesSejours();
			List<Resultats> resultats = new List<Resultats>();
			Sondage sondage = _bddContext.Sondages.First(s => s.Id == idSondage);
			foreach (IGrouping<int, Vote> grouping in sondage.Votes.GroupBy(v => v.sejour.Id))
			{
				int idSejour = grouping.Key;
				Sejour sejour = sejours.First(r => r.Id == idSejour);
				int nombreDeVotes = grouping.Count();
				resultats.Add(new Resultats { Nom = sejour.Lieu, Telephone = sejour.Telephone, NombreDeVotes = nombreDeVotes });
			}
			return resultats;
		}

		public bool ADejaVote(int idSondage, string idStr)
		{
			int id;
			if (int.TryParse(idStr, out id))
			{
				Sondage sondage = _bddContext.Sondages.Include(s => s.Votes).First(s => s.Id == idSondage);
				if (sondage.Votes == null)
					return false;
				return sondage.Votes.Any(v => v.UtilisateurId != 0 && v.UtilisateurId == id);
			}
			return false;
		}

		public List<Sondage> ObtenirLesSondages()
		{
			return _bddContext.Sondages.ToList();
		}

		public bool SejourExiste(string nom)
		{
			return _bddContext.Sejours.ToList().Any(sejour => string.Compare(sejour.Lieu, nom, StringComparison.CurrentCultureIgnoreCase) == 0);
		}
	}
}
