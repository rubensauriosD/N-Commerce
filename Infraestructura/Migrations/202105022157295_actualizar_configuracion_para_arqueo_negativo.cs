namespace Infraestructura.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class actualizar_configuracion_para_arqueo_negativo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Configuracion", "PermitirArqueoNegativo", c => c.Boolean(nullable: false));
            DropColumn("dbo.Configuracion", "ModificaPrecioVentaDesdeCompra");
            DropColumn("dbo.Configuracion", "ActivarRetiroDeCaja");
            DropColumn("dbo.Configuracion", "MontoMaximoRetiroCaja");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Configuracion", "MontoMaximoRetiroCaja", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Configuracion", "ActivarRetiroDeCaja", c => c.Boolean(nullable: false));
            AddColumn("dbo.Configuracion", "ModificaPrecioVentaDesdeCompra", c => c.Boolean(nullable: false));
            DropColumn("dbo.Configuracion", "PermitirArqueoNegativo");
        }
    }
}
