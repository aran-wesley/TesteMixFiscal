namespace TesteMixFiscal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IniciandoTesteMixFiscal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbl_Nota",
                c => new
                    {
                        NItemId = c.Int(nullable: false, identity: true),
                        Descricao = c.String(),
                        CodItem = c.String(),
                        DescCompl = c.String(),
                        Ean = c.Long(),
                        Qtd = c.Int(nullable: false),
                        VlItem = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TipoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.NItemId)
                .ForeignKey("dbo.tbl_Tipo", t => t.TipoId, cascadeDelete: true)
                .Index(t => t.TipoId);
            
            CreateTable(
                "dbo.tbl_Tipo",
                c => new
                    {
                        TipoId = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                    })
                .PrimaryKey(t => t.TipoId);

            Sql("INSERT INTO tbl_Tipo VALUES ('NF-e'), ('SPED')");

            SqlFile(@"..\ScriptsSQL\ImportMixFiscal.sql");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbl_Nota", "TipoId", "dbo.tbl_Tipo");
            DropIndex("dbo.tbl_Nota", new[] { "TipoId" });
            DropTable("dbo.tbl_Tipo");
            DropTable("dbo.tbl_Nota");
        }
    }
}
