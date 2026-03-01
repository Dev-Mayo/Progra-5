using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using ProyectoWebAPI.Abstract;
using ProyectoWebAPI.DataAccess;
using ProyectoWebAPI.DataAccess.Models;

namespace ProyectoWebAPI.BusinessLogic
{
    public class PantallaService : IPantallaService
    {
        private readonly PantallaRepository _repo;
        private readonly IBitacoraClient _bitacoraClient;

        public PantallaService(PantallaRepository repo, IBitacoraClient bitacoraClient)
        {
            _repo = repo;
            _bitacoraClient = bitacoraClient;
        }


        public async Task CrearPantallaAsync(Pantalla pantalla, string usuarioEjecutor, string token) 
        {
            try
            {
                Validar(pantalla);

                _repo.Insertar(pantalla);

                string jsonNuevo = JsonSerializer.Serialize(pantalla);

                await _bitacoraClient.RegistrarAsync(
                    usuarioEjecutor,
                    $"Se creó la pantalla: {jsonNuevo}",
                    token 
                );
            }
            catch (Exception ex)
            {
                await _bitacoraClient.RegistrarAsync(
                    usuarioEjecutor,
                    $"Error al crear pantalla: {ex.Message}",
                    token 
                );
                throw;
            }
        }


        public async Task ModificarPantallaAsync(Pantalla pantalla, string usuarioEjecutor, string token) 
        {
            try
            {
                if (pantalla.ID_Pantalla <= 0)
                    throw new Exception("Id inválido");

                var pantallaAnterior = _repo.ObtenerPorId(pantalla.ID_Pantalla).FirstOrDefault();
                if (pantallaAnterior == null)
                    throw new Exception("Pantalla no existe");

                string jsonAnterior = JsonSerializer.Serialize(pantallaAnterior);

                Validar(pantalla);

                _repo.Modificar(pantalla);

                string jsonNuevo = JsonSerializer.Serialize(pantalla);

                await _bitacoraClient.RegistrarAsync(
                    usuarioEjecutor,
                    $"Se modificó la pantalla.\nANTES: {jsonAnterior}\nDESPUÉS: {jsonNuevo}",
                    token 
                );
            }
            catch (Exception ex)
            {
                await _bitacoraClient.RegistrarAsync(
                    usuarioEjecutor,
                    $"Error al modificar pantalla: {ex.Message}",
                    token 
                );
                throw;
            }
        }


        public async Task EliminarPantallaAsync(int pantallaId, string usuarioEjecutor, string token) 
        {
            try
            {
                var pantalla = _repo.ObtenerPorId(pantallaId).FirstOrDefault();
                if (pantalla == null)
                    throw new Exception("Pantalla no existe");

                string jsonEliminado = JsonSerializer.Serialize(pantalla);

                _repo.Eliminar(pantallaId);

                await _bitacoraClient.RegistrarAsync(
                    usuarioEjecutor,
                    $"Se eliminó la pantalla: {jsonEliminado}",
                    token 
                );
            }
            catch (Exception ex)
            {
                await _bitacoraClient.RegistrarAsync(
                    usuarioEjecutor,
                    $"Error al eliminar pantalla: {ex.Message}",
                    token 
                );
                throw;
            }
        }


        public async Task<List<Pantalla>> ObtenerPantallaPorIdAsync(int pantallaId, string usuarioEjecutor, string token) 
        {
            var resultado = _repo.ObtenerPorId(pantallaId);

            await _bitacoraClient.RegistrarAsync(
                usuarioEjecutor,
                $"El usuario consultó la pantalla con ID {pantallaId}",
                token 
            );

            return resultado;
        }


        public async Task<List<Pantalla>> ObtenerTodosAsync(string usuarioEjecutor, string token) 
        {
            var resultado = _repo.ObtenerTodos();

            await _bitacoraClient.RegistrarAsync(
                usuarioEjecutor,
                "El usuario consultó todas las pantallas",
                token 
            );

            return resultado;
        }


        private void Validar(Pantalla p)
        {
            if (string.IsNullOrWhiteSpace(p.Nombre_Pantalla))
                throw new Exception("Nombre requerido");

            if (string.IsNullOrWhiteSpace(p.Descripcion))
                throw new Exception("Descripción requerida");

            if (string.IsNullOrWhiteSpace(p.Ruta_Acceso))
                throw new Exception("Ruta de acceso requerida");
        }
    }
}