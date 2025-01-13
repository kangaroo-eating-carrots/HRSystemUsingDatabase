using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for EmAssignWindow.xaml
    /// </summary>
    public partial class EmAssignWindow : Window
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

        private string initialLabel;
        private string sqlquery;
        private string searchedEmployeeID;
        private string searchedClientID;

        public bool IsConfirmed;
        public string OutputQuery;
        public string EmployeeID;
        public string ClientID;
        public int Sales;


        private void ConnectDatabase()
        {
            dbConnectionString = $"server={dbServer}; user={dbUser}; database={dbName}; port={dbPort}; password={dbPassword}";
            conn = new MySqlConnection(dbConnectionString);
        }

        public EmAssignWindow()
        {
            InitializeComponent();
            ConnectDatabase();

            initialLabel = "Please Search";
            LabelEmployeeName.Content = initialLabel;
            LabelClientName.Content = initialLabel;
            IsConfirmed = false;
        }

        private void ButtonEmployeeSearch_Click(object sender, RoutedEventArgs e)
        {
            searchedEmployeeID = TextboxEmployeeID.Text;

            if (!int.TryParse(TextboxEmployeeID.Text, out int employeeID))
            {
                MessageBox.Show("NOTE: ID Must Be Positive Number");
                return;
            }
            else if (employeeID < 1)
            {
                MessageBox.Show("NOTE: ID Must Be Positive Number");
                return;
            }

            sqlquery = $"SELECT family_name, given_name FROM employees WHERE id = {employeeID}";


            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sqlquery, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    LabelEmployeeName.Content = $"{rdr["given_name"]} {rdr["family_name"]}";
                    searchedEmployeeID = TextboxEmployeeID.Text;
                }
                else
                {
                    LabelEmployeeName.Content = initialLabel;
                    MessageBox.Show("NOTE: Can't Find An Employee With This ID");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            conn.Close();
        }

        private void ButtonClientSearch_Click(object sender, RoutedEventArgs e)
        {
            searchedClientID = TextboxClientID.Text;

            if (!int.TryParse(TextboxClientID.Text, out int clientID))
            {
                MessageBox.Show("NOTE: ID Must Be Positive Number");
                return;
            }
            else if (clientID < 1)
            {
                MessageBox.Show("NOTE: ID Must Be Positive Number");
                return;
            }

            sqlquery = $"SELECT client_name FROM clients WHERE id = {clientID}";


            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sqlquery, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    LabelClientName.Content = $"{rdr["client_name"]}";
                    searchedClientID = TextboxClientID.Text;
                }
                else
                {
                    LabelClientName.Content = initialLabel;
                    MessageBox.Show("NOTE: Can't Find A Client With This ID");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            conn.Close();
        }

        private void ButtonAssign_Click(object sender, RoutedEventArgs e)
        {
            if (TextboxEmployeeID.Text != searchedEmployeeID || String.IsNullOrEmpty(TextboxEmployeeID.Text) || LabelEmployeeName.Content == initialLabel)
            {
                MessageBox.Show("NOTE: Please Search The Employee ID First");
                return;
            }

            if (TextboxClientID.Text != searchedClientID || String.IsNullOrEmpty(TextboxClientID.Text) || LabelClientName.Content == initialLabel)
            {
                MessageBox.Show("NOTE: Please Search The Client ID First");
                return;
            }

            if (!int.TryParse(TextboxSales.Text, out int sales) || sales <= 0)
            {
                MessageBox.Show("NOTE: Sales Must Be Positive Number");
                return;
            }
            else
            {
                Sales = sales;
            }

            EmployeeID = searchedEmployeeID;
            ClientID = searchedClientID;

            OutputQuery = $"INSERT INTO working_with(employee_id,client_id,total_sales) VALUES({EmployeeID}, {ClientID}, {Sales});";
            IsConfirmed = true;
            Close();

        }
    }
}
