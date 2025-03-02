namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sala777 : DbMigration
    {
        public override void Up()
        {
           
            DropIndex("dbo.FactureAvoirFournisseur", new[] { "Num_FactureFournisseur" });
            AlterColumn("dbo.FactureAvoirFournisseur", "Num_FactureFournisseur", c => c.Int());
            RenameColumn(table: "dbo.FactureAvoirFournisseur", name: "Num_FactureFournisseur", newName: "FactureFournisseur_Num");
            CreateIndex("dbo.FactureAvoirFournisseur", "FactureFournisseur_Num");
        }
        
        public override void Down()
        {
            DropIndex("dbo.FactureAvoirFournisseur", new[] { "FactureFournisseur_Num" });
            RenameColumn(table: "dbo.FactureAvoirFournisseur", name: "FactureFournisseur_Num", newName: "Num_FactureFournisseur");
            AlterColumn("dbo.FactureAvoirFournisseur", "Num_FactureFournisseur", c => c.Int(nullable: false));
            CreateIndex("dbo.FactureAvoirFournisseur", "Num_FactureFournisseur");
        }
    }
}
