using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

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
                    Lieu = "Center Parcs",
                    Telephone = "0232605200",
                    Ville = "Les Barils",
                    ImagePath= "/images/center-parcs.png",
                    Description = "A seulement 1H30 de Paris, évadez-vous de la ville et venez profiter du charme de la campagne Normande. Logez en famille dans des cottages rénovés ou Pagodes VIP. En couple, installez-vous à l'Hotel La Maison du Lac."
                },
                new Sejour
                {
                    Id = 2,
                    Lieu = "Guédelon",
                    Telephone = "0386456666",
                    Ville = "Treigny",
                    ImagePath = "/images/guedelon.jpg",
                    Description = "Au cœur de la Puisaye, dans l’Yonne, en Bourgogne, une quarantaine d’œuvriers relèvent un défi hors-norme : construire aujourd’hui un château fort selon les techniques et avec les matériaux utilisés au Moyen Âge."
                },
                new Sejour
                {
                    Id = 3,
                    Lieu = "Parc Astérix",
                    Telephone = "0986868687",
                    Ville = "Plailly",
                    ImagePath = "/images/asterix.jpg",
                    Description = "Impossible n’est pas Gaulois : retrouvez toutes les offres pour faire de votre séjour au Parc un réel moment de partage et d’aventures… Sans pour autant dilapider tous vos sesterces!"
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
