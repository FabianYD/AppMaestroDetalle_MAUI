using Dapper;
using Microsoft.Data.Sqlite;
using System.Linq;

namespace AppMaestroDetalle
{
    public class Database
    {
        private string connectionString;
        private SqliteConnection connection;

        public Database()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "pedidos.db");
            connectionString = $"Data Source={dbPath}";
            connection = new SqliteConnection(connectionString);

            connection.Open();
            connection.Execute(@"
                CREATE TABLE IF NOT EXISTS Pedidos (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Cliente TEXT NOT NULL,
                    Estado TEXT NOT NULL
                );
                
                CREATE TABLE IF NOT EXISTS LineasPedido (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    PedidoId INTEGER NOT NULL,
                    Producto TEXT NOT NULL,
                    Cantidad INTEGER NOT NULL,
                    FOREIGN KEY(PedidoId) REFERENCES Pedidos(Id)
                );
            ");
        }

        public Pedido CrearPedido(string cliente, string estado, List<LineaPedido> lineas)
        {
            var pedidoId = connection.QuerySingle<int>(
                "INSERT INTO Pedidos (Cliente, Estado) VALUES (@Cliente, @Estado); SELECT last_insert_rowid();", 
                new { Cliente = cliente, Estado = estado });

            foreach (var linea in lineas)
            {
                connection.Execute(
                    "INSERT INTO LineasPedido (PedidoId, Producto, Cantidad) VALUES (@PedidoId, @Producto, @Cantidad)", 
                    new { PedidoId = pedidoId, Producto = linea.Producto, Cantidad = linea.Cantidad });
            }

            return connection.QuerySingle<Pedido>("SELECT * FROM Pedidos WHERE Id = @Id", new { Id = pedidoId });
        }

        public Pedido LeerPedido(int id)
        {
            return connection.QuerySingleOrDefault<Pedido>("SELECT * FROM Pedidos WHERE Id = @Id", new { Id = id });
        }

        public List<LineaPedido> LeerLineas(int pedidoId)
        {
            return connection.Query<LineaPedido>("SELECT * FROM LineasPedido WHERE PedidoId = @PedidoId", new { PedidoId = pedidoId }).ToList();
        }

        public void ActualizarPedido(int id, string cliente, string estado, List<LineaPedido> lineas)
        {
            connection.Execute("UPDATE Pedidos SET Cliente = @Cliente, Estado = @Estado WHERE Id = @Id", 
                new { Id = id, Cliente = cliente, Estado = estado });

            // Eliminar lineas viejas y meter las nuevas
            connection.Execute("DELETE FROM LineasPedido WHERE PedidoId = @Id", new { Id = id });
            
            foreach (var linea in lineas)
            {
                connection.Execute(
                    "INSERT INTO LineasPedido (PedidoId, Producto, Cantidad) VALUES (@PedidoId, @Producto, @Cantidad)", 
                    new { PedidoId = id, Producto = linea.Producto, Cantidad = linea.Cantidad });
            }
        }

        public void EliminarPedido(int id)
        {
            connection.Execute("DELETE FROM LineasPedido WHERE PedidoId = @Id", new { Id = id });
            connection.Execute("DELETE FROM Pedidos WHERE Id = @Id", new { Id = id });
        }
    }
}
