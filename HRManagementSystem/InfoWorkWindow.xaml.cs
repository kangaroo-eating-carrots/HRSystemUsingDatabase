using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JMIctprg432
{
    /// <summary>
    /// Interaction logic for InfoWorkWindow.xaml
    /// </summary>
    public partial class InfoWorkWindow : Window
    {
        // Define Connection Details
        private static string dbName = "jm_ictprg432";
        private static string dbUser = "root";
        private static string dbPassword = "";
        private static int dbPort = 3306;
        private static string dbServer = "localhost";
        // Connection String and MySQL Connection
        private static string dbConnectionString = "";
        private static MySqlConnection conn;

        private void ConnectDatabase()
        {
            dbConnectionString = $"server={dbServer}; user={dbUser}; database={dbName}; port={dbPort}; password={dbPassword}";
            conn = new MySqlConnection(dbConnectionString);
        }

        private void CallData(string query)
        {
            try
            {
                //DatagridDisplay.Items.Clear();
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                conn.Close();
                //DatagridDisplay.DataContext = dt;
                DatagridWorking.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public InfoWorkWindow()
        {
            InitializeComponent();
            ConnectDatabase();

            string sqlQuery = "SELECT * FROM working_with;";
            CallData(sqlQuery);
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(TextboxSearch.Text))
            {
                MessageBox.Show("NOTE: No ID To Search");
                return;
            }
            else if ((!int.TryParse(TextboxSearch.Text, out int searchID)) || searchID <= 0)
            {
                MessageBox.Show("NOTE: Only Positive Number Is Valid For ID Search");
                return;
            }
            else
            {
                string sqlQuery = $"SELECT * FROM working_with WHERE employee_id = {TextboxSearch.Text} OR client_id = {TextboxSearch.Text};";
                CallData(sqlQuery);
            }
        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            TextboxSearch.Text = string.Empty;
            string sqlQuery = "SELECT * FROM working_with;";
            CallData(sqlQuery);
        }
    }
}
