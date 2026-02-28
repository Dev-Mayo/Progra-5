namespace ApiGateway.Models
{
    public class ResumenOrden
    {
        public string OrdenId { get; set; }
        public int Cantidad { get; set; }
        public dynamic ProductoDetalle { get; set; }
    }
}
