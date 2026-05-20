using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AppMaestroDetalle
{
    public partial class MainPage : ContentPage
    {
        private Database _database;
        public ObservableCollection<LineaPedido> LineasTemporales { get; set; }
        public ICommand QuitarLineaCommand { get; private set; }

        public MainPage()
        {
            InitializeComponent();
            _database = new Database();
            LineasTemporales = new ObservableCollection<LineaPedido>();
            
            // Comando para eliminar una línea específica desde la 'X' en la lista
            QuitarLineaCommand = new Command<LineaPedido>(QuitarLinea);
            
            // Asignar contexto de datos para los bindings (como el Command en la lista)
            BindingContext = this;
            listaLineas.ItemsSource = LineasTemporales;
        }

        private void QuitarLinea(LineaPedido linea)
        {
            if (linea != null && LineasTemporales.Contains(linea))
            {
                LineasTemporales.Remove(linea);
            }
        }

        private void BtnAgregarLinea_Clicked(object sender, EventArgs e)
        {
            if (pickerProducto.SelectedIndex != -1 && int.TryParse(txtCantidad.Text, out int cantidad) && cantidad > 0)
            {
                string producto = pickerProducto.SelectedItem.ToString();
                LineasTemporales.Add(new LineaPedido { Producto = producto, Cantidad = cantidad });
                
                // Limpiar controles
                pickerProducto.SelectedIndex = -1;
                txtCantidad.Text = string.Empty;
            }
            else
            {
                DisplayAlertAsync("Error", "Seleccione un producto y digite una cantidad válida.", "OK");
            }
        }

        private void BtnLimpiarLineas_Clicked(object sender, EventArgs e)
        {
            LineasTemporales.Clear();
        }

        private void BtnQuitarUltimo_Clicked(object sender, EventArgs e)
        {
            if (LineasTemporales.Count > 0)
            {
                LineasTemporales.RemoveAt(LineasTemporales.Count - 1);
            }
        }

        private void BtnCrear_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCliente.Text) || string.IsNullOrWhiteSpace(txtEstado.Text) || LineasTemporales.Count == 0)
            {
                DisplayAlertAsync("Atención", "Complete el Cliente, Estado y al menos agregue una línea de pedido.", "OK");
                return;
            }

            var nuevoPedido = _database.CrearPedido(txtCliente.Text, txtEstado.Text, LineasTemporales.ToList());
            DisplayAlertAsync("Éxito", $"Pedido Creado! ID: {nuevoPedido.Id} - {nuevoPedido.Cliente} con {LineasTemporales.Count} items.", "OK");
            
            LimpiarFormulario();
        }

        private void BtnLeer_Clicked(object sender, EventArgs e)
        {
            if (int.TryParse(txtId.Text, out int id))
            {
                var pedido = _database.LeerPedido(id);
                if (pedido != null)
                {
                    txtCliente.Text = pedido.Cliente;
                    txtEstado.Text = pedido.Estado;
                    
                    var lineas = _database.LeerLineas(id);
                    LineasTemporales.Clear();
                    foreach (var linea in lineas)
                    {
                        LineasTemporales.Add(linea);
                    }
                    DisplayAlertAsync("Éxito", $"Pedido {pedido.Id} encontrado y cargado correctamente.", "OK");
                }
                else
                {
                    DisplayAlertAsync("Error", $"No se encontró un pedido con el ID {id}", "OK");
                }
            }
            else
            {
                DisplayAlertAsync("Atención", "Digite un ID válido para leer.", "OK");
            }
        }

        private void BtnActualizar_Clicked(object sender, EventArgs e)
        {
            if (int.TryParse(txtId.Text, out int id) && !string.IsNullOrWhiteSpace(txtCliente.Text) && !string.IsNullOrWhiteSpace(txtEstado.Text))
            {
                // Verificar si existe primero
                var pedido = _database.LeerPedido(id);
                if (pedido == null)
                {
                    DisplayAlertAsync("Error", $"No existe el pedido {id} para actualizar.", "OK");
                    return;
                }

                _database.ActualizarPedido(id, txtCliente.Text, txtEstado.Text, LineasTemporales.ToList());
                DisplayAlertAsync("Éxito", $"Pedido {id} actualizado correctamente con {LineasTemporales.Count} items.", "OK");
                LimpiarFormulario();
            }
            else
            {
                DisplayAlertAsync("Atención", "Debe digitar un ID válido y tener Cliente/Estado rellenos.", "OK");
            }
        }

        private void BtnEliminar_Clicked(object sender, EventArgs e)
        {
            if (int.TryParse(txtId.Text, out int id))
            {
                var pedido = _database.LeerPedido(id);
                if (pedido != null)
                {
                    _database.EliminarPedido(id);
                    DisplayAlertAsync("Éxito", $"Pedido {id} eliminado de la base de datos.", "OK");
                    LimpiarFormulario();
                }
                else
                {
                    DisplayAlertAsync("Error", $"No se encontró el pedido {id} para eliminar.", "OK");
                }
            }
            else
            {
                DisplayAlertAsync("Atención", "Digite un ID válido para eliminar.", "OK");
            }
        }

        private void LimpiarFormulario()
        {
            txtId.Text = string.Empty;
            txtCliente.Text = string.Empty;
            txtEstado.Text = string.Empty;
            pickerProducto.SelectedIndex = -1;
            txtCantidad.Text = string.Empty;
            LineasTemporales.Clear();
        }

        // Método de ayuda para compatibilidad con DisplayAlertAsync de forma asíncrona pero sin obligar al await
        private async void DisplayAlertAsync(string title, string message, string cancel)
        {
            await DisplayAlert(title, message, cancel);
        }
    }
}
