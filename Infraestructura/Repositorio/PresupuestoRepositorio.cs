namespace Infraestructura.Repositorio
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Linq.Expressions;
    using Dominio.Entidades;
    using Dominio.Repositorio;

    public class PresupuestoRepositorio : Repositorio<Presupuesto>, IPresupuestoRepositorio
    {
        public PresupuestoRepositorio(DataContext context)
            : base(context)
        {
        }

        public override Presupuesto Obtener(long entidadId, string propiedadNavegacion = "")
        {
            var resultado = propiedadNavegacion
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate<string, IQueryable<Presupuesto>>(
                    _context.Set<Comprobante>().OfType<Presupuesto>(),
                    (current, include) => current.Include(include)
                );

            return resultado.FirstOrDefault(x => x.Id == entidadId);
        }

        public override IEnumerable<Presupuesto> Obtener(Expression<Func<Presupuesto, bool>> filtro = null, string propiedadNavegacion = "")
        {
            var context = ((IObjectContextAdapter)_context).ObjectContext;
            var resultadoClient = context.CreateObjectSet<Comprobante>().OfType<Presupuesto>();
            context.Refresh(RefreshMode.ClientWins, resultadoClient);

            var resultado = propiedadNavegacion
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate<string, IQueryable<Presupuesto>>(
                    resultadoClient,
                    (current, include) => current.Include(include)
                );

            if (filtro != null) resultado = resultado.Where(filtro);

            return resultado.ToList();
        }
    }
}
