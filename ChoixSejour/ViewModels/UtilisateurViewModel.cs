using ChoixSejour.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChoixSejour.ViewModels
{
    public class UtilisateurViewModel
    {
        public Utilisateur Utilisateur { get; set; }
        public bool Authentifie { get; set; }
    }
}