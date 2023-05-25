using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CourseProject.Properties
{
    public partial class Administrator : Form
    {
        string connectionString = "Data Source=localhost;" +
            "Initial Catalog=databaseproject;" +
            "Integrated Security=True";

        SqlConnection connection;
        public Administrator()
        {
            InitializeComponent();
            connection = new SqlConnection(connectionString);

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'databaseprojectDataSet.AllSale' table. You can move, or remove it, as needed.
            this.allSaleTableAdapter.Fill(this.databaseprojectDataSet.AllSale);
            // TODO: This line of code loads data into the 'databaseprojectDataSet.product' table. You can move, or remove it, as needed.
            this.productTableAdapter.Fill(this.databaseprojectDataSet.product);

            RefreshDataGridview();
        }
        private void RefreshDataGridview()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string commandString = $"Select * from product";
                SqlCommand command = new SqlCommand(commandString, connection);

                SqlDataAdapter da = new SqlDataAdapter(command);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dataitem.DataSource = dt;
            }
        }
        private void btnsignup_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text) || string.IsNullOrWhiteSpace(UserType.Text))
            {
                MessageBox.Show("Please enter all the required fields.");
                return;
            }

            string commandString = $"INSERT INTO AllUser (Username, Password, userType) VALUES (@Username, @Password, @UserType)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(commandString, connection);
                command.Parameters.AddWithValue("@Username", txtUsername.Text);
                command.Parameters.AddWithValue("@Password", txtPassword.Text);
                command.Parameters.AddWithValue("@UserType", UserType.Text);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("User registration successful.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while registering the user: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            RefreshDataGridview();
            txtUsername.Text = "";
            txtPassword.Text = "";
            UserType.Text = "";

        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            string commandString = $"INSERT INTO product (product_Name, price) VALUES ('{txtName.Text}', '{txtPrice.Text}')";
            SqlCommand command = new SqlCommand(commandString, connection);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            RefreshDataGridview();
            txtID.Text = "";
            txtName.Text = "";
            txtPrice.Text = "";
            MessageBox.Show("The product has been added successfully");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string commandString = $"Update product Set product_Name = '{txtName.Text}', price = '{txtPrice.Text}' where id_product = {txtID.Text}";

            SqlCommand command = new SqlCommand(commandString, connection);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            RefreshDataGridview();
            txtID.Text = "";
            txtName.Text = "";
            txtPrice.Text = "";
            MessageBox.Show("The product has been update successfully");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string commandString = $"Delete from product where id_product = {txtID.Text}";
            SqlCommand command = new SqlCommand(commandString, connection);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            RefreshDataGridview();
            txtID.Text = "";
            txtName.Text = "";
            txtPrice.Text = "";
            MessageBox.Show("The product has been delete successfully");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtID.Clear();
            txtName.Clear();
            txtPrice.Clear();
        }
        private void RefreshDataGridview1()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string commandString = $"SELECT * FROM AllSale";
                SqlCommand command = new SqlCommand(commandString, connection);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dgvSales.DataSource = dataTable;
            }
        }

        private void btnViewAllSales_Click(object sender, EventArgs e)
        {
            string commandString = "SELECT * FROM AllSale";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(commandString, connection);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dgvSales.DataSource = dataTable;
            }
        }

        private void btnViewSales_Click(object sender, EventArgs e)
        {
            DateTime startDate = datePickerStartDate.Value;
            DateTime endDate = datePickerEndDate.Value;

            string commandString = $"SELECT * FROM AllSale WHERE SaleDate BETWEEN @startDate AND @endDate";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(commandString, connection);
                command.Parameters.AddWithValue("@startDate", startDate);
                command.Parameters.AddWithValue("@endDate", endDate);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dgvSales.DataSource = dataTable;
            }
        }
        private void btnAllSalesGraph_Click(object sender, EventArgs e)
        {
            string commandString = "SELECT SaleDate, Total FROM AllSale";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(commandString, connection);

                SqlDataReader reader = command.ExecuteReader();

                chartSales.Series.Clear();

                Series salesSeries = new Series("AllSale");
                salesSeries.ChartType = SeriesChartType.Line;

                while (reader.Read())
                {
                    DateTime saleDate = (DateTime)reader["SaleDate"];
                    int Total = (int)reader["Total"];

                    salesSeries.Points.AddXY(saleDate, Total);
                }
                chartSales.Series.Add(salesSeries);

                reader.Close();
            }
        }

        private void btnSalesGraph_Click(object sender, EventArgs e)
        {
            DateTime startDate = datePickerStart.Value;
            DateTime endDate = datePickerEnd.Value;

            string commandString = $"SELECT SaleDate, Total FROM AllSale WHERE SaleDate BETWEEN @StartDate AND @EndDate";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(commandString, connection);
                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@EndDate", endDate);

                SqlDataReader reader = command.ExecuteReader();

                chartSales.Series.Clear();

                Series salesSeries = new Series("AllSale");
                salesSeries.ChartType = SeriesChartType.Line;

                while (reader.Read())
                {
                    DateTime saleDate = (DateTime)reader["SaleDate"];
                    int Total = (int)reader["Total"];

                    salesSeries.Points.AddXY(saleDate, Total);
                }
                chartSales.Series.Add(salesSeries);

                reader.Close();
            }
        }

        private void UserType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dgvSales_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void signOutToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Login LoginForm = new Login();
            LoginForm.Show();
            Hide();
        }

        private void dataitem_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtID.Text = dataitem.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtName.Text = dataitem.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtPrice.Text = dataitem.Rows[e.RowIndex].Cells[2].Value.ToString();
        }
    }
}
