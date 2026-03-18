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
    public class EntidadesService : IEntidadesService
    {
        private readonly EntidadesBancariasRepository _repo;
        private readonly IBitacoraClient _bitacoraClient;

        public EntidadesService(EntidadesBancariasRepository repo, IBitacoraClient bitacoraClient)
        {
            _repo = repo;
            _bitacoraClient = bitacoraClient;
        }


        public async Task CrearEntidadAsync(EntidadesBancarias entidad, string usuarioEjecutor, string token) // ← Agregar token
        {
            try
            {
                Validar(entidad);

                _repo.Crear(entidad);

                string jsonNuevo = JsonSerializer.Serialize(entidad);

                await _bitacoraClient.RegistrarAsync(
                    usuarioEjecutor,
                    $"Se creó la entidad: {jsonNuevo}",
                    token 
                );
            }
            catch (Exception ex)
            {
                await _bitacoraClient.RegistrarAsync(
                    usuarioEjecutor,
                    $"Error al crear entidad: {ex.Message}",
                    token 
                );
                throw;
            }
        }

        public async Task ModificarEntidadAsync(EntidadesBancarias entidad, string usuarioEjecutor, string token) // ← Agregar token
        {
            try
            {
                if (entidad.ID_Entidad <= 0)
                    throw new Exception("Id inválido");

                var entidadAnterior = _repo.ObtenerPorId(entidad.ID_Entidad).FirstOrDefault();
                if (entidadAnterior == null)
                    throw new Exception("Entidad no existe");

                string jsonAnterior = JsonSerializer.Serialize(entidadAnterior);

                Validar(entidad);

                _repo.Modificar(entidad);

                string jsonNuevo = JsonSerializer.Serialize(entidad);

                await _bitacoraClient.RegistrarAsync(
                    usuarioEjecutor,
                    $"Se modificó la entidad.\nANTES: {jsonAnterior}\nDESPUÉS: {jsonNuevo}",
                    token 
                );
            }
            catch (Exception ex)
            {
                await _bitacoraClient.RegistrarAsync(
                    usuarioEjecutor,
                    $"Error al modificar entidad: {ex.Message}",
                    token 
                );
                throw;
            }
        }


        public async Task EliminarEntidadAsync(int entidadId, string usuarioEjecutor, string token) 
        {
            try
            {
                var entidad = _repo.ObtenerPorId(entidadId).FirstOrDefault();
                if (entidad == null)
                    throw new Exception("Entidad no existe");

                string jsonEliminado = JsonSerializer.Serialize(entidad);

                _repo.Eliminar(entidadId);

                await _bitacoraClient.RegistrarAsync(
                    usuarioEjecutor,
                    $"Se eliminó la entidad: {jsonEliminado}",
                    token 
                );
            }
            catch (Exception ex)
            {
                await _bitacoraClient.RegistrarAsync(
                    usuarioEjecutor,
                    $"Error al eliminar entidad: {ex.Message}",
                    token 
                );
                throw;
            }
        }

        public async Task<List<EntidadesBancarias>> ObtenerEntidadPorIdAsync(int entidadId, string usuarioEjecutor, string token) 
        {
            var resultado = _repo.ObtenerPorId(entidadId);

            await _bitacoraClient.RegistrarAsync(
                usuarioEjecutor,
                $"El usuario consultó la entidad con ID {entidadId}",
                token 
            );

            return resultado;
        }


        public async Task<List<EntidadesBancarias>> ObtenerTodosAsync(string usuarioEjecutor, string token) 
        {
            var resultado = _repo.ObtenerTodos();

            await _bitacoraClient.RegistrarAsync(
                usuarioEjecutor,
                "El usuario consultó todas las entidades",
                token 
            );

            return resultado;
        }


        private void Validar(EntidadesBancarias e)
        {
            if (string.IsNullOrWhiteSpace(e.Nombre))
                throw new Exception("Nombre de la entidad requerido");
        }
    }
}