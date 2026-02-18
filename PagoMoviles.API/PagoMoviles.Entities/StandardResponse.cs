namespace PagoMoviles.Entities
{
    public class StandardResponse
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }

    public class StandardResponse<T> : StandardResponse
    {
        public T? Data { get; set; }
    }
}