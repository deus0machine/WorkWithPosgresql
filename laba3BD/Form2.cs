using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace laba3BD
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;database=testdb;User Id=postgres;password=18201905;");

            DataSet set1 = new DataSet();
            set1.Clear();
            // Открываем подключение 
            conn.Open();
            // Sql-запрос для формирования списка 
            NpgsqlCommand command1 = new NpgsqlCommand("select name ,id from student", conn);
            // Адаптер 
            NpgsqlDataAdapter da1 = new NpgsqlDataAdapter(command1);
            // Заполнение набора данных 
            da1.Fill(set1, "set2");
            // Связывание списка и набора данных 
            comboBox1.DataSource = set1.Tables["set2"];
            // Данные, которые будут отображены в списке 
            comboBox1.DisplayMember = "name";
            // Значения, соотвествующие отображенным элементам списка 
            comboBox1.ValueMember = "id";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;database=testdb;User Id=postgres;password=18201905;");
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("insert into student (name,id) values(:name,:id)", conn);
            // Данные из поля и списка добавляем в параметры запроса
            command.Parameters.Add(new NpgsqlParameter("name", DbType.String));
            command.Parameters[0].Value = Convert.ToString(textBox1.Text);
            command.Parameters.Add(new NpgsqlParameter("id", DbType.Int32));
            // Значение параметра=id, соответствующему выбранному значению из списка
            //command.Parameters[1].Value = Convert.ToInt32(comboBox1.SelectedValue);
            command.Parameters[1].Value = 1;
            // Выполнить команду
            command.ExecuteNonQuery();
            conn.Close();
            this.Close();

        }
    }
}
