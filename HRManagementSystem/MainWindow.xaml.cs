using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace JMIctprg432
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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
                DatagridDisplay.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ResetWindow()
        {
            ComboboxBranch.SelectedItem = BranchAll;
            ComboboxMonth.SelectedItem = MonthAll;
            ComboboxOutput.SelectedItem = OutputAll;
            CheckboxMale.IsChecked = true;
            CheckboxFemale.IsChecked = true;
            CheckboxNotDefined.IsChecked = true;

            TextboxID.Text = string.Empty;
            TextboxName.Text = string.Empty;
            TextboxYear.Text = string.Empty;
            TextboxMinSalary.Text = string.Empty;
            TextboxMaxSalary.Text = string.Empty;
            TextboxSuper.Text = string.Empty;


            string sqlQuery = "SELECT * FROM employees;";
            CallData(sqlQuery);
        }

        private void Execute(string query)
        {
            MySqlCommand cmd = new MySqlCommand(query, conn);

            try
            {
                conn.Open();
                MySqlCommand cmd1 = new MySqlCommand(query, conn);
                cmd1.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            conn.Close();
        }




        public MainWindow()
        {
            InitializeComponent();

            ConnectDatabase();

            ResetWindow();

            MessageBox.Show("NOTE: Temporary Password Is '0000'\nAssuming: Sales cost which is 10% of sales (Excl. salary cost) incurs");
        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            ResetWindow();
        }

        private void ButtonInsert_Click(object sender, RoutedEventArgs e)
        {
            EmInsertWindow op1 = new EmInsertWindow();
            op1.ShowDialog();

            if (op1.IsConfirmed)
            {
                Execute(op1.SqlQuery);
                MessageBox.Show("NOTE: Employee Added");
                ResetWindow();
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            EmDeleteWindow op1 = new EmDeleteWindow();
            op1.ShowDialog();

            if (op1.IsConfirmed)
            {
                Execute(op1.OutputQuery);
                MessageBox.Show($"NOTE: {op1.DeletedID} Deleted");
                ResetWindow();
            }

        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            EmUpdateWindow op1 = new EmUpdateWindow();
            op1.ShowDialog();

            if (op1.IsConfirmed)
            {
                Execute(op1.OutputQuery);
                MessageBox.Show($"NOTE: {op1.UpdatedID} Updated");
                ResetWindow();
            }
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            string sqlQuery = "";
            if (ComboboxOutput.SelectedValue == OutputAll)
            {
                sqlQuery = "SELECT * From employees\n";
            }
            else if (ComboboxOutput.SelectedValue == OutputDOB)
            {
                sqlQuery = "SELECT id, CONCAT(given_name, ' ', family_name) as full_name, date_of_birth From employees\n";
            }
            else if (ComboboxOutput.SelectedValue == OutputBranch)
            {
                sqlQuery = "SELECT employees.id, CONCAT(employees.given_name, ' ', family_name) as full_name, branch_id, branches.branch_name From employees\n";
                sqlQuery = sqlQuery + "LEFT JOIN branches ON employees.branch_id = branches.id ";
            }
            else if (ComboboxOutput.SelectedValue == OutputSalary)
            {
                sqlQuery = "SELECT id, CONCAT(given_name, ' ', family_name) as full_name, gross_salary From employees\n";
            }

            if (CheckboxMale.IsChecked == false && CheckboxFemale.IsChecked == false && CheckboxNotDefined.IsChecked == false)
            {
                MessageBox.Show("Note: At least one gender needs to be checked");
                return;
            }

            sqlQuery = sqlQuery + "WHERE ";

            if (ComboboxOutput.SelectedValue != OutputBranch)
            {
                if (CheckboxMale.IsChecked == true && CheckboxFemale.IsChecked == true && CheckboxNotDefined.IsChecked == true)
                {
                    sqlQuery = sqlQuery + "(gender_identity = 'M' OR gender_identity = 'F' OR gender_identity = 'O')";
                }
                else if (CheckboxMale.IsChecked == true && CheckboxFemale.IsChecked == true && CheckboxNotDefined.IsChecked == false)
                {
                    sqlQuery = sqlQuery + "(gender_identity = 'M' OR gender_identity = 'F')";
                }
                else if (CheckboxMale.IsChecked == true && CheckboxFemale.IsChecked == false && CheckboxNotDefined.IsChecked == true)
                {
                    sqlQuery = sqlQuery + "(gender_identity = 'M' OR gender_identity = 'O')";
                }
                else if (CheckboxMale.IsChecked == false && CheckboxFemale.IsChecked == true && CheckboxNotDefined.IsChecked == true)
                {
                    sqlQuery = sqlQuery + "(gender_identity = 'F' OR gender_identity = 'O')";
                }
                else if (CheckboxMale.IsChecked == true && CheckboxFemale.IsChecked == false && CheckboxNotDefined.IsChecked == false)
                {
                    sqlQuery = sqlQuery + "(gender_identity = 'M')";
                }
                else if (CheckboxMale.IsChecked == false && CheckboxFemale.IsChecked == true && CheckboxNotDefined.IsChecked == false)
                {
                    sqlQuery = sqlQuery + "(gender_identity = 'F')";
                }
                else if (CheckboxMale.IsChecked == false && CheckboxFemale.IsChecked == false && CheckboxNotDefined.IsChecked == true)
                {
                    sqlQuery = sqlQuery + "(gender_identity = 'O')";
                }
            }
            else
            {
                if (CheckboxMale.IsChecked == true && CheckboxFemale.IsChecked == true && CheckboxNotDefined.IsChecked == true)
                {
                    sqlQuery = sqlQuery + "(employees.gender_identity = 'M' OR employees.gender_identity = 'F' OR employees.gender_identity = 'O')";
                }
                else if (CheckboxMale.IsChecked == true && CheckboxFemale.IsChecked == true && CheckboxNotDefined.IsChecked == false)
                {
                    sqlQuery = sqlQuery + "(gender_identity = 'M' OR gender_identity = 'F')";
                }
                else if (CheckboxMale.IsChecked == true && CheckboxFemale.IsChecked == false && CheckboxNotDefined.IsChecked == true)
                {
                    sqlQuery = sqlQuery + "(gender_identity = 'M' OR gender_identity = 'O')";
                }
                else if (CheckboxMale.IsChecked == false && CheckboxFemale.IsChecked == true && CheckboxNotDefined.IsChecked == true)
                {
                    sqlQuery = sqlQuery + "(gender_identity = 'F' OR gender_identity = 'O')";
                }
                else if (CheckboxMale.IsChecked == true && CheckboxFemale.IsChecked == false && CheckboxNotDefined.IsChecked == false)
                {
                    sqlQuery = sqlQuery + "(gender_identity = 'M')";
                }
                else if (CheckboxMale.IsChecked == false && CheckboxFemale.IsChecked == true && CheckboxNotDefined.IsChecked == false)
                {
                    sqlQuery = sqlQuery + "(gender_identity = 'F')";
                }
                else if (CheckboxMale.IsChecked == false && CheckboxFemale.IsChecked == false && CheckboxNotDefined.IsChecked == true)
                {
                    sqlQuery = sqlQuery + "(gender_identity = 'O')";
                }
            }


            if (String.IsNullOrEmpty(TextboxID.Text))
            {
                sqlQuery = sqlQuery;
            }
            else if (!int.TryParse(TextboxID.Text, out int searchID))
            {
                MessageBox.Show("Note: Only Positive Number Is Available for ID Search");
                return;
            }
            else if (searchID < 1)
            {
                MessageBox.Show("Note: Only Positive Number Is Available for ID Search");
                return;
            }
            else
            {
                if (ComboboxOutput.SelectedValue != OutputBranch)
                {
                    sqlQuery = sqlQuery + $" AND (id = {searchID})";
                }
                else
                {
                    sqlQuery = sqlQuery + $" AND (employees.id = {searchID})";
                }
            }

            if (String.IsNullOrEmpty(TextboxName.Text))
            {
                sqlQuery = sqlQuery;
            }
            else
            {
                if (ComboboxOutput.SelectedValue != OutputBranch)
                {
                    sqlQuery = sqlQuery + $" AND ((family_name LIKE '%{TextboxName.Text}%') OR (given_name LIKE '%{TextboxName.Text}%'))";
                }
                else
                {
                    sqlQuery = sqlQuery + $" AND ((employees.family_name LIKE '%{TextboxName.Text}%') OR (employees.given_name LIKE '%{TextboxName.Text}%'))";
                }
                
            }

            if (ComboboxBranch.SelectedValue == BranchAll)
            {
                sqlQuery = sqlQuery;
            }
            else
            {
                if (ComboboxOutput.SelectedValue != OutputBranch)
                {
                    if (ComboboxBranch.SelectedItem == Branch0)
                    {
                        sqlQuery = sqlQuery + " AND (branch_id = 0)";
                    }
                    else if (ComboboxBranch.SelectedItem == Branch1)
                    {
                        sqlQuery = sqlQuery + " AND (branch_id = 1)";
                    }
                    else if (ComboboxBranch.SelectedItem == Branch2)
                    {
                        sqlQuery = sqlQuery + " AND (branch_id = 2)";
                    }
                    else if (ComboboxBranch.SelectedItem == Branch3)
                    {
                        sqlQuery = sqlQuery + " AND (branch_id = 3)";
                    }
                }
                else
                {
                    if (ComboboxBranch.SelectedItem == Branch0)
                    {
                        sqlQuery = sqlQuery + " AND (employees.branch_id = 0)";
                    }
                    else if (ComboboxBranch.SelectedItem == Branch1)
                    {
                        sqlQuery = sqlQuery + " AND (employees.branch_id = 1)";
                    }
                    else if (ComboboxBranch.SelectedItem == Branch2)
                    {
                        sqlQuery = sqlQuery + " AND (employees.branch_id = 2)";
                    }
                    else if (ComboboxBranch.SelectedItem == Branch3)
                    {
                        sqlQuery = sqlQuery + " AND (employees.branch_id = 3)";
                    }
                }
            }

            if (String.IsNullOrEmpty(TextboxYear.Text))
            {
                sqlQuery = sqlQuery;
            }
            else if (!int.TryParse(TextboxYear.Text, out int birthYear))
            {
                MessageBox.Show("Note: Number Between 1950 And 2050 Are Available For Birth Year Search");
                return;
            }
            else if (birthYear < 1950 || birthYear > 2050)
            {
                MessageBox.Show("Note: Number Between 1950 And 2050 Are Available For Birth Year Search");
                return;
            }
            else
            {
                if (ComboboxOutput.SelectedValue != OutputBranch)
                {
                    sqlQuery = sqlQuery + $" AND (YEAR(date_of_birth) = {birthYear})";
                }
                else
                {
                    sqlQuery = sqlQuery + $" AND (YEAR(employees.date_of_birth) = {birthYear})";
                }
            }

            if (ComboboxMonth.SelectedValue == MonthAll)
            {
                sqlQuery = sqlQuery;
            }
            else
            {
                if (ComboboxOutput.SelectedValue != OutputBranch)
                {
                    sqlQuery = sqlQuery + $" AND (MONTH(date_of_birth) = {ComboboxMonth.SelectedIndex})";
                }
                else
                {
                    sqlQuery = sqlQuery + $" AND (MONTH(employees.date_of_birth) = {ComboboxMonth.SelectedIndex})";
                }
            }


            int minSalary = 0;
            int maxSalary = 0;
            bool isMinSalaryEntered = false;
            bool isMaxSalaryEntered = false;

            if (String.IsNullOrEmpty(TextboxMinSalary.Text))
            {
                sqlQuery = sqlQuery;
            }
            else if ((!int.TryParse(TextboxMinSalary.Text, out minSalary)) || minSalary < 0)
            {
                MessageBox.Show("Note: Min/Max Salary Range Shoule Be Zero Or Positive Number");
                return;
            }
            else
            {
                isMinSalaryEntered = true;

                if (ComboboxOutput.SelectedValue != OutputBranch)
                {
                    sqlQuery = sqlQuery + $" AND (gross_salary >= {minSalary})";
                }
                else
                {
                    sqlQuery = sqlQuery + $" AND (employees.gross_salary >= {minSalary})";
                }
            }

            if (String.IsNullOrEmpty(TextboxMaxSalary.Text))
            {
                sqlQuery = sqlQuery;
            }
            else if (((!int.TryParse(TextboxMaxSalary.Text, out maxSalary) || maxSalary < 0)))
            {
                MessageBox.Show("Note: Min/Max Salary Range Shoule Be Zero Or Positive Number");
                return;
            }
            else
            {
                isMaxSalaryEntered = true;

                if (ComboboxOutput.SelectedValue != OutputBranch)
                {
                    sqlQuery = sqlQuery + $" AND (gross_salary <= {maxSalary})";
                }
                else
                {
                    sqlQuery = sqlQuery + $" AND (employees.gross_salary <= {maxSalary})";
                }

            }

            if (isMinSalaryEntered && isMaxSalaryEntered && minSalary > maxSalary)
            {
                MessageBox.Show("Note: Max Salary in the Range Must Not Be Smaller Than Min Salary");
                return;
            }

            if (String.IsNullOrEmpty(TextboxSuper.Text))
            {
                sqlQuery = sqlQuery;
            }
            else if (!int.TryParse(TextboxSuper.Text, out int searchID))
            {
                MessageBox.Show("Note: Only Positive Number Is Available for Supervisor ID Search");
                return;
            }
            else if (searchID < 1)
            {
                MessageBox.Show("Note: Only Positive Number Is Available for Supervisor ID Search");
                return;
            }
            else
            {
                if (ComboboxOutput.SelectedValue != OutputBranch)
                {
                    sqlQuery = sqlQuery + $" AND (supervisor_id = {searchID})";
                }
                else
                {
                    sqlQuery = sqlQuery + $" AND (employees.supervisor_id = {searchID})";
                }
            }


            sqlQuery = sqlQuery + ";";

            CallData(sqlQuery);

;
        }

        private void ButtonBranch_Click(object sender, RoutedEventArgs e)
        {
            InfoBranchWindow op1 = new InfoBranchWindow();
            op1.ShowDialog();
        }

        private void ButtonSupplier_Click(object sender, RoutedEventArgs e)
        {
            InfoSupplierWindow op1 = new InfoSupplierWindow();
            op1.ShowDialog();
        }

        private void ButtonClient_Click(object sender, RoutedEventArgs e)
        {
            InfoClientWindow op1 = new InfoClientWindow();
            op1.ShowDialog();
        }

        private void ButtonWork_Click(object sender, RoutedEventArgs e)
        {
            InfoWorkWindow op1 = new InfoWorkWindow();
            op1.ShowDialog();
        }

        private void ButtonAssign_Click(object sender, RoutedEventArgs e)
        {
            EmAssignWindow op1 = new EmAssignWindow();
            op1.ShowDialog();


            if (op1.IsConfirmed)
            {
                Execute(op1.OutputQuery);
                MessageBox.Show($"NOTE: Employee (ID {op1.EmployeeID}) Starts Working With Client (ID {op1.ClientID}) For Sales ${op1.Sales}");
            }
        }

        private void ButtonProfit_Click(object sender, RoutedEventArgs e)
        {
            InfoProfitWindow op1 = new InfoProfitWindow();
            op1.ShowDialog();
        }
    }
}