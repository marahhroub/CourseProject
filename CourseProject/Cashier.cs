using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

using System.Windows.Forms;

namespace CourseProject
{
    public partial class Cashier : Form
    {
        string connectionString = "Data Source=localhost;" +
    "Initial Catalog=databaseproject;" +
    "Integrated Security=True";

        SqlConnection connection;
        private SqlDataReader myreader;

        public Cashier()
        {
            InitializeComponent();
        }

        private void Cashier_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'databaseprojectDataSet.product' table. You can move, or remove it, as needed.
            this.productTableAdapter.Fill(this.databaseprojectDataSet.product);
            fillListbox();
        }

        
        private void fillListbox()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string commandString = $"Select * from product";
                SqlCommand command = new SqlCommand(commandString, connection);

                SqlDataAdapter da = new SqlDataAdapter(command);


                DataTable dt = new DataTable();

                da.Fill(dt);

                listBox1.DataSource = dt;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string commandString = $"SELECT * FROM product WHERE product_name = @ProductName;";
                SqlCommand command = new SqlCommand(commandString, connection);
                command.Parameters.AddWithValue("@ProductName", listBox1.Text);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    int id = (int)reader["Id_product"];
                    string name = (string)reader["product_Name"];
                    int price = (int)reader["price"];

                    txtname.Text = name;
                    txtprice.Text = price.ToString();
                }
                else
                {
                    txtname.Text = string.Empty;
                    txtprice.Text = string.Empty;
                }

                reader.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double t1, t2;
            t1 = Convert.ToDouble(numericUpDown1.Value);
            t2 = Convert.ToDouble(txtprice.Text);
            total.Text = (t1 * t2).ToString();
        }

        private void confirm_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string commandString = "INSERT INTO AllSale (SaleDate, item_Name, price, Quantity, Total) VALUES (@SaleDate, @ProductName, @price, @Quantity, @Total)";
                SqlCommand command = new SqlCommand(commandString, connection);

                // Convert the dateTime.Text to DateTime object
                DateTime saleDate;
                if (DateTime.TryParse(dateTime.Text, out saleDate))
                {
                    // Format the DateTime object to the desired string format for the database
                    string formattedDate = saleDate.ToString("yyyy-MM-dd HH:mm:ss");

                    // Add the formatted date to the parameter
                    command.Parameters.AddWithValue("@SaleDate", formattedDate);
                }
                else
                {
                    MessageBox.Show("Invalid date format.");
                    return;
                }

                command.Parameters.AddWithValue("@ProductName", txtname.Text);
                command.Parameters.AddWithValue("@price", txtprice.Text);
                command.Parameters.AddWithValue("@Quantity", numericUpDown1.Text);
                command.Parameters.AddWithValue("@Total", total.Text);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("The product has been added successfully");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while adding the product: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            fillListbox();
            txtname.Text = "";
            txtprice.Text = "";
            total.Text = "";
        }

        private void cancel_Click_1(object sender, EventArgs e)
        {
            txtname.Clear();
            txtprice.Clear();
            total.Clear();
        }

        private void fillBy1ToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.productTableAdapter.
                    FillBy1(this.databaseprojectDataSet.product);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login LoginForm = new Login();
            LoginForm.Show();
            Hide();
        }
    }

}
