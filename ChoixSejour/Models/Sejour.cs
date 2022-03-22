using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChoixSejour.Models
{
	public class Sejour
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Le nom du sejour doit être rempli !")]
		[MaxLength(25)]
		public string Lieu { get; set; }

		[Display(Name="Téléphone")]
		[RegularExpression(@"^\d{10}$", ErrorMessage = "Le numéro de téléphone doit contenir 10 chiffres !")]
		public string Telephone { get; set; }

		public string Description { get; set; }
		[MaxLength(20)]
		public string Ville { get; set; }
		public string ImagePath { get; set; }
		[NotMapped]
		public IFormFile Image { get; set; }


	}
}
