using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities.Zlib;
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
    /// Interaction logic for EmDeleteWindow.xaml
    /// </summary>
    public partial class EmDeleteWindow : Window
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


        private string password = "0000";
        private string initialLabel;
        private string sqlquery;
        private string searchedID;

        public bool IsConfirmed;
        public string OutputQuery;
        public string DeletedID;

        private void ConnectDatabase()
        {
            dbConnectionString = $"server={dbServer}; user={dbUser}; database={dbName}; port={dbPort}; password={dbPassword}";
            conn = new MySqlConnection(dbConnectionString);
        }


        public EmDeleteWindow()
        {
            InitializeComponent();

            ConnectDatabase();

            initialLabel = "Please Search";
            LabelName.Content = initialLabel;
            IsConfirmed = false;

        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            searchedID = TextboxID.Text;

            if (!int.TryParse(TextboxID.Text, out int id))
            {
                MessageBox.Show("NOTE: ID Must Be Positive Number");
                return;
            }
            else if (id < 1)
            {
                MessageBox.Show("NOTE: ID Must Be Positive Number");
                return;
            }

            sqlquery = $"SELECT family_name, given_name FROM employees WHERE id = {id}";


            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sqlquery, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    LabelName.Content = $"{rdr["given_name"]} {rdr["family_name"]}";
                    searchedID = TextboxID.Text;
                }
                else
                {
                    LabelName.Content = initialLabel;
                    MessageBox.Show("NOTE: Can't Find An Employee With This ID");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            conn.Close();
        }

        private void ButtonConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (TextboxID.Text != searchedID || String.IsNullOrEmpty(TextboxID.Text) || LabelName.Content == initialLabel)
            {
                MessageBox.Show("NOTE: Please Search The ID First");
                return;
            }

            if (TextboxPassword.Text != password)
            {
                MessageBox.Show("NOTE: Wrong Password");
                return;
            }

            DeletedID = searchedID;
            OutputQuery = $"DELETE FROM `employees` WHERE id = {DeletedID}";
            IsConfirmed = true;
            Close();

        }
    }
}
