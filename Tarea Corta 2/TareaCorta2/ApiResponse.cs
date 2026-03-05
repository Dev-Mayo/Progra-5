namespace TareaCorta2
{
    public class ApiResponse<T>
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public T? Data { get; set; }
    }
}
