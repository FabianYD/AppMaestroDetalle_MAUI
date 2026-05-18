namespace AppMaestroDetalle
{
    public class LineaPedido
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        
        // Propiedad calculada para la vista
        public string Descripcion => $"{Producto} (x{Cantidad})";
    }
}
