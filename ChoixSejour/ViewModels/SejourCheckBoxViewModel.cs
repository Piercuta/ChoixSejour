using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChoixSejour.ViewModels
{
    public class SejourCheckBoxViewModel
    {
        public int Id { get; set; }
        public string LieuEtTelephone { get; set; }
        public bool EstSelectionne { get; set; }
    }
}