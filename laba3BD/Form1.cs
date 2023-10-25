using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace laba3BD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string connstring = String.Format("Server={0};Port={1};" + "User Id ={2};Password={3};Database={4}", "localhost", 5432, "postgres", "18201905", "localbd");
        NpgsqlConnection conn;
        string sql;
        NpgsqlCommand cmd;
        DataTable dt;
        int rowIndex = -1;
        private void button1_Click(object sender, EventArgs e)//insert
        {
            rowIndex = -1;
            textBox1.Enabled = textBox2.Enabled = textBox3.Enabled = true;
            textBox1.Text = textBox2.Text = textBox3.Text = null;
            textBox1.Select();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connstring);
            Select();
        }
        private new void Select()
        {
            try 
            {
                conn.Open();
                sql = @"select * from st_select()";
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                conn.Close();
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                rowIndex = e.RowIndex;
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells["firstname"].Value.ToString();
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["midname"].Value.ToString();
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["lastname"].Value.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)//update
        {
            if (rowIndex < 0)
            {
                MessageBox.Show("Choose student to update");
                return;
            }
            textBox1.Enabled = textBox2.Enabled = textBox3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e) // delete
        {
            if (rowIndex < 0)
            {
                MessageBox.Show("Choose student to delete");
                return;
            }
             
            try
            {
                conn.Open();
                sql = @"select * from st_delete(:_id)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_id", int.Parse(dataGridView1.Rows[rowIndex].Cells["id"].Value.ToString()));
                if((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Delete student successfully");
                    rowIndex = -1;
                    conn.Close();
                    Select();
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show("Deleted fail. Error: " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e) //save
        {
            int result = 0;
            if (rowIndex < 0) //insert
            {
                try
                {
                    conn.Open();
                    sql = @"select * from st_insert(:_firstname, :_midname, :_lastname)";
                    cmd = new NpgsqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("_firstname", textBox1.Text);
                    cmd.Parameters.AddWithValue("_midname", textBox2.Text);
                    cmd.Parameters.AddWithValue("_lastname", textBox3.Text);
                    result = (int)cmd.ExecuteScalar();
                    conn.Close();
                    if (result == 1)
                    {
                        MessageBox.Show("Inserted new student successfully");
                        Select();
                    }
                    else
                    {
                        MessageBox.Show("Inserted fail");
                    }
                }
                catch (Exception ex)
                {
                    conn.Close() ;
                    MessageBox.Show("Inserted fail. Error: " + ex.Message);
                }
            }
            else //update
            {
                try
                {
                    conn.Open();
                    sql = @"select * from st_update(:_id, :_firstname, :_midname, :_lastname)";
                    cmd = new NpgsqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("_id", int.Parse(dataGridView1.Rows[rowIndex].Cells["id"].Value.ToString()));
                    cmd.Parameters.AddWithValue("_firstname", textBox1.Text);
                    cmd.Parameters.AddWithValue("_midname", textBox2.Text);
                    cmd.Parameters.AddWithValue("_lastname", textBox3.Text);
                    result= (int)cmd.ExecuteScalar();
                    conn.Close();
                    if (result == 1)
                    {
                        MessageBox.Show("Updated successfully");
                        Select();
                    }
                    else
                    {
                        MessageBox.Show("Updated failed");
                    }
                }
                catch (Exception ex)
                {
                    conn.Close();
                    MessageBox.Show("Deleted fail. Error: " + ex.Message);
                }
            }
            result = 0;
            textBox1.Text = textBox2.Text = textBox3.Text = null;
            textBox1.Enabled = textBox2.Enabled = textBox3.Enabled = false;
        }
    }
}
