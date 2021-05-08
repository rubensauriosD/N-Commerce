namespace Servicios.FormaPago
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Dominio.Entidades;
    using Dominio.UnidadDeTrabajo;
    using IServicio.BaseDto;
    using IServicios.FormaPago;
    using IServicios.FormaPago.DTOs;
    using Servicios.Base;

    public class ChequeServicio : IChequeServicio
    {
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public ChequeServicio(IUnidadDeTrabajo unidadDeTrabajo)
        {
            _unidadDeTrabajo = unidadDeTrabajo;
        }

        public bool DepositarCheque(long id, DateTime fecha)
        {
            try
            {
                var entidad = _unidadDeTrabajo.ChequeRepositorio.Obtener(id);

                if (entidad == null) throw new Exception("Ocurrio un Error al Obtener la Cheque");

                entidad.Depositado = true;
                entidad.FechaDeposito = fecha;

                _unidadDeTrabajo.ChequeRepositorio.Modificar(entidad);
                _unidadDeTrabajo.Commit();

                return true;
            }
            catch (Exception e)
            {
                throw new Exception($"Error en ChequeServicio.DepositarCheque. {Environment.NewLine}{e.Message}");
            }
        }

        public void Eliminar(long id)
        {
            try
            {
                _unidadDeTrabajo.ChequeRepositorio.Eliminar(id);
                _unidadDeTrabajo.Commit();
            }
            catch (Exception e)
            {
                throw new Exception($"Error en ChequeServicio.Eliminar: {Environment.NewLine}{e.Message}");
            }
        }

        public void Insertar(DtoBase dtoEntidad)
        {
            try
            {
                var dto = (ChequeDto)dtoEntidad;

                var entidad = new Dominio.Entidades.Cheque
                {
                    FechaVencimiento = dto.FechaVencimiento,
                    Numero = dto.Numero,
                    ClienteId = dto.ClienteId,
                    BancoId = dto.BancoId,
                    FechaDeposito = null,
                    Depositado = false,
                    EstaEliminado = false
                };

                _unidadDeTrabajo.ChequeRepositorio.Insertar(entidad);
                _unidadDeTrabajo.Commit();
            }
            catch (Exception e)
            {
                throw new Exception($"Error en ChequeServicio.Insertar. {Environment.NewLine}{e.Message}");
            }
        }

        public void Modificar(DtoBase dtoEntidad)
        {
            try
            {
                var dto = (ChequeDto)dtoEntidad;

                var entidad = _unidadDeTrabajo.ChequeRepositorio.Obtener(dto.Id);

                if (entidad == null) throw new Exception("Ocurrio un Error al Obtener la Cheque");

                entidad.FechaVencimiento = dto.FechaVencimiento;
                entidad.Numero = dto.Numero;
                entidad.ClienteId = dto.ClienteId;
                entidad.BancoId = dto.BancoId;

                _unidadDeTrabajo.ChequeRepositorio.Modificar(entidad);
                _unidadDeTrabajo.Commit();
            }
            catch (Exception e)
            {
                throw new Exception($"Error en ChequeServicio.Modificar {Environment.NewLine}{e.Message}");
            }
        }

        public DtoBase Obtener(long id)
        {
            var entidad = _unidadDeTrabajo.ChequeRepositorio.Obtener(id, "Banco, Cliente");

            return new ChequeDto
            {
                Id = entidad.Id,
                FechaVencimiento = entidad.FechaVencimiento,
                Numero = entidad.Numero,
                ClienteId = entidad.ClienteId,
                Cliente = entidad.Cliente.Apellido +" "+entidad.Cliente.Nombre,
                BancoId = entidad.BancoId,
                Banco = entidad.Banco.Descripcion,
                FechaDeposito = entidad.FechaDeposito,
                Depositado = entidad.Depositado,
                Eliminado = entidad.EstaEliminado
            };
        }

        public IEnumerable<DtoBase> Obtener(string cadenaBuscar, bool mostrarTodos = true)
        {
            Expression<Func<Dominio.Entidades.Cheque, bool>> filtro =
                x => x.Numero.Contains(cadenaBuscar)
                || x.Cliente.Nombre.Contains(cadenaBuscar)
                || x.Cliente.Apellido.Contains(cadenaBuscar)
                || x.Banco.Descripcion.Contains(cadenaBuscar);

            if (!mostrarTodos)
                filtro = filtro.And(x => !x.EstaEliminado);

            return _unidadDeTrabajo.ChequeRepositorio.Obtener(filtro, "Banco, Cliente")
                .Select(x => new ChequeDto
                {
                    Id = x.Id,
                    FechaVencimiento = x.FechaVencimiento,
                    Numero = x.Numero,
                    ClienteId = x.ClienteId,
                    Cliente = x.Cliente.Apellido + " " + x.Cliente.Nombre,
                    BancoId = x.BancoId,
                    Banco = x.Banco.Descripcion,
                    FechaDeposito = x.FechaDeposito,
                    Depositado = x.Depositado,
                    Eliminado = x.EstaEliminado
                })
                .OrderByDescending(x => x.FechaVencimiento)
                .ToList();
        }
    }
}
