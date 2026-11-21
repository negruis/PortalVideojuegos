using System;
using System.Windows;
using System.Windows.Media;
using MySql.Data.MySqlClient;

namespace PortalVideojuegos
{
    public partial class Login : Window
    {
        private readonly DatabaseHelper dbHelper = new DatabaseHelper();

        public Login()
        {
            InitializeComponent();
        }

        // BOTÓN LOGIN
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string usuario = TxtUsuario.Text.Trim();
            string password = TxtPassword.Password.Trim();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password))
            {
                LblMensaje.Foreground = Brushes.Red;
                LblMensaje.Text = "Por favor, rellena usuario y contraseña.";
                return;
            }

            MySqlConnection conn = null;
            try
            {
                conn = dbHelper.OpenConnection();
                string query = "SELECT COUNT(*) FROM Usuarios WHERE nombre_usuario = @usuario AND password = @password";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    cmd.Parameters.AddWithValue("@password", password);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                    {
                        LblMensaje.Foreground = Brushes.Green;
                        LblMensaje.Text = $"¡Bienvenido, {usuario}!";

                        MainWindow main = new MainWindow();
                        main.Show();
                        this.Close();
                    }
                    else
                    {
                        LblMensaje.Foreground = Brushes.Red;
                        LblMensaje.Text = "Usuario o contraseña incorrectos.";
                    }
                }
            }
            catch (MySqlException ex)
            {
                LblMensaje.Foreground = Brushes.Red;
                LblMensaje.Text = "Error de conexión con MySQL.";
                MessageBox.Show("No se pudo conectar con la base de datos. Comprueba que MySQL esté activo.\nDetalle: " + ex.Message, "Error de conexión", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error inesperado: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                dbHelper.CloseConnection(conn);
            }
        }

        // BOTÓN NUEVO USUARIO
        private void BtnNuevoUsuario_Click(object sender, RoutedEventArgs e)
        {
            string usuario = TxtUsuario.Text.Trim();
            string password = TxtPassword.Password.Trim();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password))
            {
                LblMensaje.Foreground = Brushes.Red;
                LblMensaje.Text = "Introduce usuario y contraseña para registrarte.";
                return;
            }

            MySqlConnection conn = null;
            try
            {
                conn = dbHelper.OpenConnection();
                string query = "INSERT INTO Usuarios (nombre_usuario, password) VALUES (@usuario, @password)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    cmd.Parameters.AddWithValue("@password", password);

                    cmd.ExecuteNonQuery();

                    LblMensaje.Foreground = Brushes.Green;
                    LblMensaje.Text = $"Usuario '{usuario}' registrado correctamente.";
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    LblMensaje.Foreground = Brushes.OrangeRed;
                    LblMensaje.Text = $"El usuario '{usuario}' ya existe.";
                }
                else
                {
                    LblMensaje.Foreground = Brushes.Red;
                    LblMensaje.Text = "Error de conexión con MySQL.";
                    MessageBox.Show("No se pudo conectar con la base de datos.\nDetalle: " + ex.Message, "Error de conexión", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error inesperado: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                dbHelper.CloseConnection(conn);
            }
        }

        // BOTÓN SALIR
        private void BtnSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}