using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Domain.Models.Mapping;
using Domain.Entites;
using Data.Models.Mapping;
using System.Data.Entity.Core.Objects;

namespace Domain.Models
{
    public partial class SteDataBaseContext : DbContext
    {
        static SteDataBaseContext()
        {
            Database.SetInitializer<SteDataBaseContext>(null);
        }

        public SteDataBaseContext()
            : base("Name=SteDataBaseContext")
        {
            // this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ProxyCreationEnabled = false;


        }

      
        public DbSet<BonDeLivraison> BonDeLivraisons { get; set; }
        public DbSet<BonDeReception> BonDeReceptions { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Devi> Devis { get; set; }
        public DbSet<Facture> Factures { get; set; }
        public DbSet<FactureFournisseur> FactureFournisseurs { get; set; }
        public DbSet<Fournisseur> Fournisseurs { get; set; }
        public DbSet<LigneBL> LigneBLs { get; set; }
        public DbSet<LigneBonReception> LigneBonReceptions { get; set; }
        public DbSet<LigneDevi> LigneDevis { get; set; }
        public DbSet<Produit> Produits { get; set; }
        public DbSet<Systeme> Systemes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<EcheanceDesFournisseur> EcheanceDesFournisseurs { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<LigneCommande> LigneCommandes { get; set; }
        public DbSet<Avoir> Avoirs { get; set; }
        public DbSet<LigneAvoir> LigneAvoirs { get; set; }
        public DbSet<LigneAvoirFourniseur> LigneAvoirsFournisseur { get; set; }
        public DbSet<FactureAvoirFournisseur> FactureAvoirsFournisseur { get; set; }
        public DbSet<AvoirFournisseur> AvoirsFournisseur { get; set; }
        public DbSet<AvoirFinancierFournisseur> AvoirFinancierFournisseur { get; set; }

      

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BonDeLivraisonMap());
            modelBuilder.Configurations.Add(new BonDeReceptionMap());
            modelBuilder.Configurations.Add(new ClientMap());
            modelBuilder.Configurations.Add(new DeviMap());
            modelBuilder.Configurations.Add(new FactureMap());
            modelBuilder.Configurations.Add(new FactureFournisseurMap());
            modelBuilder.Configurations.Add(new FournisseurMap());
            modelBuilder.Configurations.Add(new LigneBLMap());
            modelBuilder.Configurations.Add(new LigneBonReceptionMap());
            modelBuilder.Configurations.Add(new LigneDeviMap());
            modelBuilder.Configurations.Add(new ProduitMap());
            modelBuilder.Configurations.Add(new SystemeMap());
            modelBuilder.Configurations.Add(new TransactionMap());
            modelBuilder.Configurations.Add(new EcheanceDesFournisseurMap());
            modelBuilder.Configurations.Add(new AvoirMap());
            modelBuilder.Configurations.Add(new LigneAvoirMap());
            modelBuilder.Configurations.Add(new CommandeMap());
            modelBuilder.Configurations.Add(new LigneCommandeMap());
            modelBuilder.Configurations.Add(new AvoirFournisseurMap());
            modelBuilder.Configurations.Add(new LigneAvoirFournisseurMap());
            modelBuilder.Configurations.Add(new FactureAvoirFournisseurMap());
            modelBuilder.Configurations.Add(new AvoirFinancierFournisseurMap());

        }
    }
}
