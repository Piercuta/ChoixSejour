using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChoixSejour.ViewModels
{
    public class RestaurantCheckBoxViewModel
    {
        public int Id { get; set; }
        public string NomEtTelephone { get; set; }
        public bool EstSelectionne { get; set; }
    }
}