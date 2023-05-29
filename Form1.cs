using System;
using System.Data.SqlClient;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace Library
{
    public partial class Form1 : Form
    {
        private SqlConnection connection;
        private SqlCommand cmd;
        private SqlDataReader reader;
        SqlDataAdapter drr;
        private bool Mode = true;
        private string id;
        private string sql;

        public Form1()
        {
            InitializeComponent();
            LoadConfiguration();
            connection = new SqlConnection(GetConnectionString());
        }


        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }


        private void LoadConfiguration()
        {
            string configFile = "appsettings.json";
            string json = File.ReadAllText(configFile);

            var config = JsonSerializer.Deserialize<JsonDocument>(json);

            if (config.RootElement.TryGetProperty("ConnectionStrings", out var connectionStrings))
            {
                var sqlServerConnection = connectionStrings.GetProperty("SqlServerConnection").GetString();

                if (!string.IsNullOrEmpty(sqlServerConnection))
                {
                    // Store the connection string
                    connection = new SqlConnection(sqlServerConnection);
                }
                else
                {
                    throw new Exception("Invalid connection string");
                }
            }
            else
            {
                throw new Exception("Invalid configuration");
            }
        }

        private string GetConnectionString()
        {
            // Return the stored connection string
            return connection.ConnectionString;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string title = textTitle.Text;
            string author = textAuthor.Text;
            string customer = textCustomer.Text;

            if (Mode)
            {
                sql = "insert into testing_book(BookTitle, Author, Customer) values(@title, @author, @customer)";
                connection.Open();
                cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@author", author);
                cmd.Parameters.AddWithValue("@customer", customer);
                MessageBox.Show("Record added");
                cmd.ExecuteNonQuery();

                textTitle.Clear();
                textAuthor.Clear();
                textTitle.Focus();
            }
            else
            {

            }

            connection.Close();
        }

        public void Load()
        {
            try
            {
                sql = "select * from testing_book";
                cmd = new SqlCommand(sql, connection);
                connection.Open();

                reader = cmd.ExecuteReader();

                dataGridLibrary.Rows.Clear();

                while(reader.Read())
                {
                    dataGridLibrary.Rows.Add(reader[0], reader[1], reader[2], reader[3]);
                }

                connection.Close();

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
