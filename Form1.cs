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
    }
}
