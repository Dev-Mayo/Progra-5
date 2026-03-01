using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ProyectoWebAPI.Abstract;
using ProyectoWebAPI.DataAccess;
using ProyectoWebAPI.DataAccess.Models;

namespace ProyectoWebAPI.BusinessLogic
{
    public class RolService : IRolService
    {
        private readonly RolesRepository _repo;
        private readonly IBitacoraClient _bitacoraClient;

        public RolService(RolesRepository repo, IBitacoraClient bitacoraClient)
        {
            _repo = repo;
            _bitacoraClient = bitacoraClient;
        }


        public async Task CrearRolAsync(Roles rol, string usuarioEjecutor, string token)
        {
            try
            {
                Validar(rol);

                _repo.Agregar(rol);

                string jsonNuevo = JsonSerializer.Serialize(rol);

                await _bitacoraClient.RegistrarAsync(
                    usuarioEjecutor,
                    $"Se creó el rol: {jsonNuevo}",
                    token 
                );
            }
            catch (Exception ex)
            {
                await _bitacoraClient.RegistrarAsync(
                    usuarioEjecutor,
                    $"Error al crear rol: {ex.Message}",
                    token 
                );
                throw;
            }
        }


        public async Task ModificarRolAsync(Roles rol, string usuarioEjecutor, string token) 
        {
            try
            {
                if (rol.ID_Rol <= 0)
                    throw new Exception("Id inválido");

                var rolAnterior = _repo.ObtenerPorId(rol.ID_Rol).FirstOrDefault();
                if (rolAnterior == null)
                    throw new Exception("Rol no existe");

                string jsonAnterior = JsonSerializer.Serialize(rolAnterior);

                Validar(rol);

                _repo.Modificar(rol);

                string jsonNuevo = JsonSerializer.Serialize(rol);

                await _bitacoraClient.RegistrarAsync(
                    usuarioEjecutor,
                    $"Se modificó el rol.\nANTES: {jsonAnterior}\nDESPUÉS: {jsonNuevo}",
                    token 
                );
            }
            catch (Exception ex)
            {
                await _bitacoraClient.RegistrarAsync(
                    usuarioEjecutor,
                    $"Error al modificar rol: {ex.Message}",
                    token 
                );
                throw;
            }
        }


        public async Task EliminarRolAsync(int rolId, string usuarioEjecutor, string token) 
        {
            try
            {
                var rol = _repo.ObtenerPorId(rolId).FirstOrDefault();
                if (rol == null)
                    throw new Exception("Rol no existe");

                string jsonEliminado = JsonSerializer.Serialize(rol);

                _repo.Eliminar(rolId);

                await _bitacoraClient.RegistrarAsync(
                    usuarioEjecutor,
                    $"Se eliminó el rol: {jsonEliminado}",
                    token 
                );
            }
            catch (Exception ex)
            {
                await _bitacoraClient.RegistrarAsync(
                    usuarioEjecutor,
                    $"Error al eliminar rol: {ex.Message}",
                    token 
                );
                throw;
            }
        }


        public async Task<List<Roles>> ObtenerRolPorIdAsync(int rolId, string usuarioEjecutor, string token) 
        {
            var resultado = _repo.ObtenerPorId(rolId);

            await _bitacoraClient.RegistrarAsync(
                usuarioEjecutor,
                $"El usuario consultó el rol con ID {rolId}",
                token 
            );

            return resultado;
        }


        public async Task<List<Roles>> ObtenerTodosAsync(string usuarioEjecutor, string token) 
        {
            var resultado = _repo.ObtenerTodos();

            await _bitacoraClient.RegistrarAsync(
                usuarioEjecutor,
                "El usuario consultó todos los roles",
                token 
            );

            return resultado;
        }

        private void Validar(Roles r)
        {
            if (string.IsNullOrWhiteSpace(r.Nombre))
                throw new Exception("Nombre del rol requerido");
        }
    }
}