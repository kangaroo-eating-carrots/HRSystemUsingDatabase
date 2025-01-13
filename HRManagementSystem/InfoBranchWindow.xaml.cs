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
    /// Interaction logic for InfoBranchWindow.xaml
    /// </summary>
    public partial class InfoBranchWindow : Window
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
                DatagridBranch.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void CallStaffData(string query)
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
                DatagridStaff.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public InfoBranchWindow()
        {
            InitializeComponent();
            ConnectDatabase();

            string sqlQuery = "SELECT * FROM branches;";
            CallData(sqlQuery);

            ComboboxBranch.SelectedIndex = 0;
            sqlQuery = $"SELECT id as employee_id, given_name, family_name, gross_salary, branch_id, supervisor_id FROM employees WHERE branch_id = {ComboboxBranch.SelectedIndex};";
            CallStaffData(sqlQuery);

        }

        private void ComboboxBranch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string sqlQuery = $"SELECT id as employee_id, CONCAT(given_name, ' ', family_name) as employee_name, gross_salary, branch_id, supervisor_id FROM employees WHERE branch_id = {ComboboxBranch.SelectedIndex};";
            CallStaffData(sqlQuery);
        }


    }
}
