namespace Infraestructura.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RealacionFormaPago_MovimientoCuentaCorriente : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Movimiento_CuentaCorriente", "FormaPagoId", "dbo.FormaPago");
            DropIndex("dbo.Movimiento_CuentaCorriente", new[] { "FormaPagoId" });
            DropColumn("dbo.Movimiento_CuentaCorriente", "FormaPagoId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Movimiento_CuentaCorriente", "FormaPagoId", c => c.Long(nullable: false));
            CreateIndex("dbo.Movimiento_CuentaCorriente", "FormaPagoId");
            AddForeignKey("dbo.Movimiento_CuentaCorriente", "FormaPagoId", "dbo.FormaPago", "Id");
        }
    }
}
