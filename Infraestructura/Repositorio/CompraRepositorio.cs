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

    public class CompraRepositorio : Repositorio<Compra>, ICompraRepositorio
    {
        public CompraRepositorio(DataContext context)
            : base(context)
        {
        }

        public override Compra Obtener(long entidadId, string propiedadNavegacion = "")
        {
            var resultado = propiedadNavegacion
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate<string, IQueryable<Compra>>(
                    _context.Set<Comprobante>().OfType<Compra>(),
                    (current, include) => current.Include(include)
                );

            return resultado.FirstOrDefault(x => x.Id == entidadId);
        }

        public override IEnumerable<Compra> Obtener(Expression<Func<Compra, bool>> filtro = null, string propiedadNavegacion = "")
        {
            var context = ((IObjectContextAdapter)_context).ObjectContext;
            var resultadoClient = context.CreateObjectSet<Comprobante>().OfType<Compra>();
            context.Refresh(RefreshMode.ClientWins, resultadoClient);

            var resultado = propiedadNavegacion
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate<string, IQueryable<Compra>>(
                    resultadoClient,
                    (current, include) => current.Include(include)
                );

            if (filtro != null) resultado = resultado.Where(filtro);

            return resultado.ToList();
        }
    }
}
