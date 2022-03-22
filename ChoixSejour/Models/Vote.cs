using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChoixSejour.Models
{
	public class Vote
	{
		public int Id { get; set; }
		public int SejourId { get; set; }
		public Sejour sejour { get; set; }
		public int UtilisateurId { get; set; }
		public Utilisateur Utilisateur { get; set; }
	}
}
