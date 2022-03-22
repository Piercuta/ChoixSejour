using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Extensions.Configuration;

namespace ChoixSejour.Models
{
	public class BddContext : DbContext
	{
        public BddContext(): base()
        {

        }

        //public IConfiguration Configuration { get; }

        public DbSet<Utilisateur> Utilisateurs { get; set; }
		public DbSet<Sejour> Sejours { get; set; }
		public DbSet<Vote> Votes { get; set; }
		public DbSet<Sondage> Sondages { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
            if (System.Diagnostics.Debugger.IsAttached)
            {
                optionsBuilder.UseMySql("server=localhost;user id=root;password=rrrrr;database=ChoixSejourDebug");
            }
            else
            {
                IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
                optionsBuilder.UseMySql(configuration.GetConnectionString("DefaultConnection"));
            }
                
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // specification on configuration

            //Declare non nullable columns
            modelBuilder.Entity<Utilisateur>().Property(u => u.Prenom).IsRequired();
            //Add uniqueness constraint
            modelBuilder.Entity<Utilisateur>().HasIndex(u => u.Prenom).IsUnique();
        }

        public void InitializeDb()
        {
            this.Database.EnsureDeleted();
            this.Database.EnsureCreated();
            this.Sejours.AddRange(
                new Sejour
                {
                    Id = 1,
                    Lieu = "Au Cellier",
                    Telephone = "0299972050",
                    Ville = "Fougères",
                    Description = "bla bla bla" 
                },
                new Sejour
                {
                    Id = 2,
                    Lieu = "L'Eveil des sens",
                    Telephone = "0243304217",
                    Ville = "Mayenne",
                    Description = "bla bla bla"
                },
                new Sejour
                {
                    Id = 3,
                    Lieu = "Le Carré",
                    Telephone = "0223402121",
                    Ville = "Rennes",
                    Description = "bla bla bla"
                },
                 new Sejour
                 {
                     Id = 4,
                     Lieu = "Le Carré",
                     Telephone = "0223402121",
                     Ville = "Rennes",
                     Description = "bla bla bla"
                 },
                 new Sejour
                 {
                     Id = 5,
                     Lieu = "Le Carré",
                     Telephone = "0223402121",
                     Ville = "Rennes",
                     Description = "bla bla bla"
                 }
            );
            this.Utilisateurs.Add(
                new Utilisateur { 
                    Id = 1,
                    Prenom = "Pierre",
                    Password =Dal.EncodeMD5("ppppp"),
                    Role=Role.ReadWrite 
                }
            );
            this.Utilisateurs.Add(
                new Utilisateur {
                    Id = 2,
                    Prenom = "Louis",
                    Password = Dal.EncodeMD5("lllll"),
                    Role=Role.Admin
                }
            );
            this.Sondages.Add(new Sondage { Id = 1});
            this.SaveChanges();
        }
    }
}
