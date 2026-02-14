namespace PagoMoviles.Models.DTOs
{
    /// <summary>
    /// Respuesta estándar de la API.
    /// Todos los endpoints devuelven esta estructura para mantener consistencia.
    /// </summary>
    public class ApiResponse<T>
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public T? Data { get; set; }

        /// <summary>
        /// Crea una respuesta exitosa (código 0).
        /// </summary>
        public static ApiResponse<T> Ok(string descripcion, T? data = default)
        {
            return new ApiResponse<T>
            {
                Codigo = 0,
                Descripcion = descripcion,
                Data = data
            };
        }

        /// <summary>
        /// Crea una respuesta de error (código -1).
        /// </summary>
        public static ApiResponse<T> Error(string descripcion)
        {
            return new ApiResponse<T>
            {
                Codigo = -1,
                Descripcion = descripcion
            };
        }
    }

    /// <summary>
    /// Versión sin dato genérico para respuestas simples.
    /// </summary>
    public class ApiResponse : ApiResponse<object>
    {
        public new static ApiResponse Ok(string descripcion)
        {
            return new ApiResponse
            {
                Codigo = 0,
                Descripcion = descripcion
            };
        }

        public new static ApiResponse Error(string descripcion)
        {
            return new ApiResponse
            {
                Codigo = -1,
                Descripcion = descripcion
            };
        }
    }
}
