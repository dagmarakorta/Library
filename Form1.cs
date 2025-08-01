﻿using System;
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
            Load();
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

                while (reader.Read())
                {
                    dataGridLibrary.Rows.Add(reader[0], reader[1], reader[2], reader[3]);
                }

                connection.Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
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
                textCustomer.Clear();
                textTitle.Focus();
            }
            else
            {

            }

            connection.Close();
            Load();
        }

        public void getID(string id)
        {
            sql = "select * from testing_book where id='" + id + "' ";
            cmd = new SqlCommand(sql, connection);
            connection.Open();
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                textTitle.Text = reader[1].ToString();
                textAuthor.Text = reader[2].ToString();
                textCustomer.Text = reader[3].ToString();
            }

            connection.Close(); 
        }

        private void dataGridLibrary_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridLibrary.Columns["Edit"].Index && e.RowIndex >= 0)
            {
                Mode = false;
                id = dataGridLibrary.CurrentRow.Cells[0].Value.ToString();
                getID(id);
            }
        }
    }
}
