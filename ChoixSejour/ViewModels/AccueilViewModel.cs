using ChoixSejour.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChoixSejour.ViewModels
{
	public class AccueilViewModel
	{
		public string Message { get; set; }
		public DateTime Date { get; set; }
		public Sejour MainSejour { get; set; }
		public List<Sejour> ListeSejours { get; set; }

	}
}
