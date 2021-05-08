namespace Infraestructura.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vincular_gasto_con_caja : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Gasto", "CajaId", c => c.Long(nullable: false));
            CreateIndex("dbo.Gasto", "CajaId");
            AddForeignKey("dbo.Gasto", "CajaId", "dbo.Caja", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Gasto", "CajaId", "dbo.Caja");
            DropIndex("dbo.Gasto", new[] { "CajaId" });
            DropColumn("dbo.Gasto", "CajaId");
        }
    }
}
