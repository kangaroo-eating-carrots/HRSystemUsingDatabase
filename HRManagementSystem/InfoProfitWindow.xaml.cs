using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JMIctprg432
{
    /// <summary>
    /// Interaction logic for InfoProfitWindow.xaml
    /// </summary>
    public partial class InfoProfitWindow : Window
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

        private string searchedID;
        private int sales;
        private int salaryCost;
        private int salesCost;
        private int profit;

        private void ConnectDatabase()
        {
            dbConnectionString = $"server={dbServer}; user={dbUser}; database={dbName}; port={dbPort}; password={dbPassword}";
            conn = new MySqlConnection(dbConnectionString);
        }

        private void Reset()
        {
            LabelOutput.Content = initialLabel;
            TextboxSearch.Text = string.Empty;
            sqlquery = "SELECT SUM(employees.gross_salary) AS total_salary, SUM(working_with.total_sales) AS total_sales FROM employees JOIN working_with;";

            bool isReadable = false;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sqlquery, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    salaryCost = int.Parse($"-{rdr["total_salary"]}");
                    sales = int.Parse($"{rdr["total_sales"]}");
                    salesCost = -(sales / 10);
                    profit = sales + salesCost + salaryCost;
                    isReadable = true;
                }
                else
                {
                    MessageBox.Show("NOTE: Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            conn.Close();

            if (isReadable)
            {
                LabelSales.Content = $"{sales}";
                LabelSalesCost.Content = $"{salesCost}";
                LabelSalary.Content = $"{salaryCost}";
                LabelProfit.Content = $"{profit}";
            }
        }

        public InfoProfitWindow()
        {
            InitializeComponent();
            ConnectDatabase();

            initialLabel = "ALL EMPLOYEES";

            Reset();

        }



        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextboxSearch.Text))
            {
                MessageBox.Show("NOTE: No ID to Search");
                return;
            }
            else if (!int.TryParse(TextboxSearch.Text, out int employeeID))
            {
                MessageBox.Show("NOTE: ID Must Be Positive Number");
                return;
            }
            else if (employeeID < 1)
            {
                MessageBox.Show("NOTE: ID Must Be Positive Number");
                return;
            }
            else
            {
                sqlquery = $"SELECT id, family_name, given_name, gross_salary FROM employees WHERE id = {employeeID};";
            }

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sqlquery, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    LabelOutput.Content = $"({rdr["id"]}) {rdr["given_name"]} {rdr["family_name"]}";
                    searchedID = TextboxSearch.Text;
                }
                else
                {
                    MessageBox.Show("NOTE: Can't Find An Employee With This ID");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            conn.Close();

            if (LabelOutput.Content != initialLabel)
            {
                sqlquery = $"SELECT employees.gross_salary AS gross_salary, SUM(working_with.total_sales) AS total_sales FROM employees " +
                    $"LEFT JOIN working_with ON employees.id = working_with.employee_id " +
                    $"WHERE employees.id = {searchedID} GROUP BY employees.id, working_with.employee_id;";

                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(sqlquery, conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        if (!int.TryParse($"-{rdr["gross_salary"]}", out salaryCost))
                        {
                            salaryCost = 0;
                        }

                        if (!int.TryParse($"{rdr["total_sales"]}", out sales))
                        {
                            sales = 0;
                        }

                        salesCost = -(sales / 10);
                        profit = sales + salesCost + salaryCost;
                    }
                    else
                    {
                        MessageBox.Show("NOTE: Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                conn.Close();

                LabelSales.Content = $"{sales}";
                LabelSalesCost.Content = $"{salesCost}";
                LabelSalary.Content = $"{salaryCost}";
                LabelProfit.Content = $"{profit}";
            }

        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }
    }
}
