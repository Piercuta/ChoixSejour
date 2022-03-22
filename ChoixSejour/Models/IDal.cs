using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChoixSejour.Models
{
	public interface IDal : IDisposable
	{
		void CreerSejour(string nom, string telephone, string ville, string description, int id = 0);
		void ModifierSejour(int id, string nom, string telephone, string ville, string description, string imagePath);
		void SupprimerSejour(int id);
		void SupprimerSejour(string nom, string telephone);
		bool SejourExiste(string nom);
		List<Sejour> ObtientTousLesSejours();
		// ajout fonctions à venir

		Utilisateur AjouterUtilisateur(string nom, string password, Role role=Role.ReadWrite);
		Utilisateur Authentifier(string nom, string password);
		Utilisateur ObtenirUtilisateur(int id);
		Utilisateur ObtenirUtilisateur(string idStr);

		int CreerUnSondage();
		void AjouterVote(int idSondage, int idSejour, int idUtilisateur);
		List<Resultats> ObtenirLesResultats(int idSondage);
		List<Sondage> ObtenirLesSondages();
		bool ADejaVote(int idSondage, string idStr);
	}
}
