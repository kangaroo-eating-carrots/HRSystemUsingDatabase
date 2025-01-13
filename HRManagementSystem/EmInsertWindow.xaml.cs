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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JMIctprg432
{
    /// <summary>
    /// Interaction logic for EmInsertWindow.xaml
    /// </summary>
    public partial class EmInsertWindow : Window
    {
        public bool IsConfirmed;
        public string SqlQuery;

        private string sqlInsert;
        private string sqlValue;

        private string familyName;
        private string givenName;
        private string dateOfBirth;
        private string gender;
        private string salary;
        private string branch;
        private string supervisorID;


        public EmInsertWindow()
        {
            InitializeComponent();
            IsConfirmed = false;
        }

        private void ButtonConfirm_Click(object sender, RoutedEventArgs e)
        {
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
                salary = null;
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
                supervisorID = null;
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


            // Nullable values: givenName, salary, suvervisorID
            sqlInsert = "INSERT INTO `employees` (`family_name`, `date_of_birth`,`branch_id`, `gender_identity`";
            sqlValue = $"VALUES ('{familyName}', '{dateOfBirth}', {branch}, '{gender}'";

            if (givenName != null)
            {
                sqlInsert = sqlInsert + ", `given_name`";
                sqlValue = sqlValue + $", '{givenName}'";
            }

            if (salary != null)
            {
                sqlInsert = sqlInsert + ", `gross_salary`";
                sqlValue = sqlValue + $", {salary}";
            }

            if (supervisorID != null)
            {
                sqlInsert = sqlInsert + ", `supervisor_id`";
                sqlValue = sqlValue + $", {supervisorID}";
            }

            sqlInsert = sqlInsert + ")\n";
            sqlValue = sqlValue + ");";

            SqlQuery = sqlInsert + sqlValue;

            IsConfirmed = true;
            Close();

        }

        private void TextboxDOB_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (TextboxDOB.Text == "DD/MM/YYYY")
            {
                TextboxDOB.Text = string.Empty;
            }
        }
    }
}
