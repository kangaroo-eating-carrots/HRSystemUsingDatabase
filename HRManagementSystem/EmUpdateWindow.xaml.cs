using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for EmUpdateWindow.xaml
    /// </summary>
    public partial class EmUpdateWindow : Window
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
        private string initalLabel;
        private string searchedID;

        private string sqlquery;
        private string familyName;
        private string givenName;
        private string dateOfBirth;
        private string gender;
        private string salary;
        private string branch;
        private string supervisorID;

        public bool IsConfirmed;
        public string OutputQuery;
        public string UpdatedID;


        private void ConnectDatabase()
        {
            dbConnectionString = $"server={dbServer}; user={dbUser}; database={dbName}; port={dbPort}; password={dbPassword}";
            conn = new MySqlConnection(dbConnectionString);
        }
        public EmUpdateWindow()
        {
            InitializeComponent();

            ConnectDatabase();

            initalLabel = "Please Search";
            LabelID.Content = initalLabel;
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

            sqlquery = $"SELECT id, family_name, given_name, YEAR(date_of_birth) AS birth_year, MONTH(date_of_birth) AS birth_Month, DAY(date_of_birth) as birth_day, gender_identity, gross_salary, branch_id, supervisor_id  FROM employees WHERE id = {id};";


            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sqlquery, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    LabelID.Content = $"{rdr["id"]}";
                    searchedID = TextboxID.Text;
                    TextboxFamilyName.Text = $"{rdr["family_name"]}";
                    TextboxGivenName.Text = $"{rdr["given_name"]}";
                    TextboxDOB.Text = $"{rdr["birth_day"]:00}/{rdr["birth_month"]:00}/{rdr["birth_year"]}";
                    TextboxSalary.Text = $"{rdr["gross_salary"]}";
                    TextboxSuperID.Text = $"{rdr["supervisor_id"]}";

                    if ($"{rdr["gender_identity"]}" == "O")
                    {
                        ComboboxGender.SelectedItem = GenderUnknown;
                    }

                    else if ($"{rdr["gender_identity"]}" == "M")
                    {
                        ComboboxGender.SelectedItem = GenderMale;
                    }
                    else
                    {
                        ComboboxGender.SelectedItem = GenderFemale;
                    }

                    if ($"{rdr["branch_id"]}" == "0")
                    {
                        ComboboxBranch.SelectedItem = NotAssigned;
                    }
                    else if ($"{rdr["branch_id"]}" == "1")
                    {
                        ComboboxBranch.SelectedItem = Branch1;
                    }
                    else if ($"{rdr["branch_id"]}" == "2")
                    {
                        ComboboxBranch.SelectedItem = Branch2;
                    }
                    else
                    {
                        ComboboxBranch.SelectedItem = Branch3;
                    }
                }
                else
                {
                    LabelID.Content = initalLabel;
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
            if (TextboxID.Text != searchedID || String.IsNullOrEmpty(TextboxID.Text) || LabelID.Content == initalLabel)
            {
                MessageBox.Show("NOTE: Please Search The ID First");
                return;
            }

            if (String.IsNullOrEmpty(TextboxFamilyName.Text))
            {
                MessageBox.Show("NOTE: Please Enter Family Name");
                return;
            }
            else
            {
                familyName = TextboxFamilyName.Text;
            }

            if (String.IsNullOrEmpty(TextboxGivenName.Text))
            {
                givenName = null;
            }
            else
            {
                givenName = TextboxGivenName.Text;
            }

            //To confirm to check whether DOB is correctly entered with DD/MM/YYYY
            if (TextboxDOB.Text.Length != 10)
            {
                MessageBox.Show("NOTE: Please Enter DOB Corretly (DD/MM/YYYY)");
                return;
            }
            else if (TextboxDOB.Text[2] != '/' || TextboxDOB.Text[5] != '/')
            {
                MessageBox.Show("NOTE: Please Enter DOB Corretly (DD/MM/YYYY)");
                return;
            }

            string strDay = $"{TextboxDOB.Text[0]}{TextboxDOB.Text[1]}";
            string strMonth = $"{TextboxDOB.Text[3]}{TextboxDOB.Text[4]}";
            string strYear = $"{TextboxDOB.Text[6]}{TextboxDOB.Text[7]}{TextboxDOB.Text[8]}{TextboxDOB.Text[9]}";
            string dateString = $"{strMonth}/{strDay}/{strYear}";

            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            DateTimeStyles styles = DateTimeStyles.None;

            if (!DateTime.TryParse(dateString, culture, styles, out DateTime birthDate))
            {
                MessageBox.Show("NOTE: Please Enter DOB Corretly  (DD/MM/YYYY)");
                return;
            }
            else if (birthDate.Year < 1950 || birthDate.Year > 2050)
            {
                MessageBox.Show("NOTE: Invalid Year For DOB (Valid: 1950~2050)");
                return;
            }
            else
            {
                dateOfBirth = $"{birthDate.Year}-{birthDate.Month}-{birthDate.Day}";
            }


            if (ComboboxGender.SelectedItem == null)
            {
                MessageBox.Show("NOTE: Please Select Gender");
                return;
            }
            else if (ComboboxGender.SelectedItem == GenderUnknown)
            {
                gender = "O";
            }
            else if (ComboboxGender.SelectedItem == GenderMale)
            {
                gender = "M";
            }
            else
            {
                gender = "F";
            }


            if (String.IsNullOrEmpty(TextboxSalary.Text))
            {
                salary = "0";
            }
            else if (!int.TryParse(TextboxSalary.Text, out int intSalary))
            {
                MessageBox.Show("NOTE: Salary Must Be Zero or Positive Number");
                return;
            }
            else if (intSalary < 0)
            {
                MessageBox.Show("NOTE: Salary Must Be Zero or Positive Number");
                return;
            }
            else
            {
                salary = $"{intSalary}";
            }

            if (ComboboxBranch.SelectedItem == null)
            {
                MessageBox.Show("NOTE: Please Select Branch");
                return;
            }
            else if (ComboboxBranch.SelectedItem == NotAssigned)
            {
                branch = "0";
            }
            else if (ComboboxBranch.SelectedItem == Branch1)
            {
                branch = "1";
            }
            else if (ComboboxBranch.SelectedItem == Branch2)
            {
                branch = "2";
            }
            else if (ComboboxBranch.SelectedItem == Branch3)
            {
                branch = "3";
            }


            if (String.IsNullOrEmpty(TextboxSuperID.Text))
            {
                supervisorID = "0";
            }
            else if (!int.TryParse(TextboxSuperID.Text, out int intSupervisorID))
            {
                MessageBox.Show("NOTE: Supervioser ID Must Be Positive Number");
                return;
            }
            else if (intSupervisorID < 1)
            {
                MessageBox.Show("NOTE: Supervioser ID Must Be Positive Number");
                return;
            }
            else
            {
                supervisorID = $"{intSupervisorID}";
            }


            sqlquery = $"UPDATE employees " +
                $"SET family_name = '{familyName}', given_name = '{givenName}', date_of_birth = '{dateOfBirth}', gender_identity = '{gender}', " +
                $"gross_salary = {salary}, branch_id = {branch}, supervisor_id = {supervisorID} " +
                $"WHERE id = {searchedID};";




            UpdatedID = searchedID;
            OutputQuery = sqlquery;
            IsConfirmed = true;
            Close();
        }
    }
}
