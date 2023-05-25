using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using CourseProject.Properties;

namespace CourseProject
{

    public partial class Login : Form
    {
        string connectionString = "Data Source=localhost;" +
            "Initial Catalog=databaseproject;" +
            "Integrated Security=True";

        SqlConnection connection;
        public Login()
        {
            InitializeComponent();
        }

        public enum UserType
        {
            None,
            Administrator,
            Cashier
        }

        private void btnlogin_Click_1(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // Validate the credentials against the database
            UserType userType = AuthenticateUser(username, password);

                if (userType == UserType.Administrator)
                {
                    OpenAdministratorPage();
                }
                else if (userType == UserType.Cashier)
                {
                    OpenCashierPage();
                }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }
        }

        private UserType AuthenticateUser(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT userType FROM AllUser WHERE Username = @Username AND Password = @Password", connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    UserType userType;
                    Enum.TryParse(reader["userType"].ToString(), out userType);

                    return userType;
                }

                return UserType.None;
            }
        }


        private void OpenAdministratorPage()
        {
            Administrator adminForm = new Administrator();
            adminForm.Show();
            Hide();
        }

        private void OpenCashierPage()
        {
            Cashier cashierForm = new Cashier();
            cashierForm.Show();
            Hide();
        }
    }
}
