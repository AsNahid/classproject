using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp8
{
    public partial class StudentForm : Form
    {
        public StudentForm()
        {
            InitializeComponent();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(EEEradio.Checked)
            {
                departmentcomboBox1.Items.Clear();
                departmentcomboBox1.Items.Add("EEE");
                departmentcomboBox1.Items.Add("CSE");


            }
        }

        private void CEradio_CheckedChanged(object sender, EventArgs e)
        {
            if (CEradio.Checked)
            {
                departmentcomboBox1.Items.Clear();
                departmentcomboBox1.Items.Add("CE");
                departmentcomboBox1.Items.Add("ARCH");


            }
        }

        private void MEradio_CheckedChanged(object sender, EventArgs e)
        {
            if (MEradio.Checked)
            {
                departmentcomboBox1.Items.Clear();
                departmentcomboBox1.Items.Add("ME");
                departmentcomboBox1.Items.Add("IP");
                departmentcomboBox1.Items.Add("MME");


            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
        }
        private void add_Click(object sender, EventArgs e)
        {
            try
            {

                if (studentid.Text == "" || studentname.Text == "" || studentnumber.Text =="" || (!EEEradio.Checked && !CEradio.Checked && !MEradio.Checked )|| departmentcomboBox1.SelectedIndex == -1)
                {
                    MessageBox.Show("Fill in all the fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\admin\Documents\studentdata.mdf;Integrated Security=True;Connect Timeout=30");
                con.Open();

                SqlCommand cnn = new SqlCommand("insert into studenttable values(@studentid,@studentname,@mobile,@birthday,@department,@faculty)", con);
                cnn.Parameters.AddWithValue("@studentid", studentid.Text);
                cnn.Parameters.AddWithValue("@studentname", studentname.Text);
                cnn.Parameters.AddWithValue("@mobile", studentnumber.Text);
                cnn.Parameters.AddWithValue("@birthday", dateTimePicker1.Value);
                cnn.Parameters.AddWithValue("@department", departmentcomboBox1.Text);
                string faculty = "";
                
                    if (EEEradio.Checked)
                    {
                        // if checked, assign "Engineer" to job
                        faculty = EEEradio.Text; // "Engineer"
                    }
                    else if (CEradio.Checked)
                {


                    faculty = CEradio.Text;
                }
                    else
                    {
                        // if radioButton1 is not checked, assign "Doctor" to job
                        faculty = MEradio.Text; // "Doctor"
                    }

                // job variable now contains either "Engineer" or "Doctor" based on the user's choice
                cnn.Parameters.AddWithValue("@faculty", faculty);


                cnn.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Record Saved Successfully", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch

            {

                MessageBox.Show("Something went wrong! Maybe the ID is duplicate or some fields are empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            SqlConnection cono = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\admin\Documents\studentdata.mdf;Integrated Security=True;Connect Timeout=30");
            cono.Open();

            SqlCommand cnna = new SqlCommand("select * from studenttable", cono);
            SqlDataAdapter da = new SqlDataAdapter(cnna);
            DataTable table = new DataTable();
            da.Fill(table);
            dataGridView1.DataSource = table;

            studentid.Clear();
            studentname.Clear();
            studentnumber.Clear();
           
           departmentcomboBox1.SelectedIndex = -1;
            EEEradio.Checked = false;
            CEradio.Checked = false;
            MEradio.Checked = false;
            dateTimePicker1.Value = DateTime.Today;





        }

        private void StudentForm_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\admin\Documents\studentdata.mdf;Integrated Security=True;Connect Timeout=30");
            conn.Open();
            SqlCommand cnno = new SqlCommand("select * from studenttable", conn);
            SqlDataAdapter da = new SqlDataAdapter(cnno);
            DataTable table = new DataTable();
            da.Fill(table);
            dataGridView1.DataSource = table;
        }

        private void update_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\admin\Documents\studentdata.mdf;Integrated Security=True;Connect Timeout=30");
            con.Open();


            try
            {
              

                // Correct SQL query with WHERE clause to match ID
                SqlCommand cnn = new SqlCommand("UPDATE studenttable SET studentname =@studentname, mobile = @mobile, birthday = @birthday, department = @department,faculty=@faculty WHERE studentid= @studentid", con);

                // Adding parameter values
                cnn.Parameters.AddWithValue("@studentid", studentid.Text);
                cnn.Parameters.AddWithValue("@studentname", studentname.Text);
                cnn.Parameters.AddWithValue("@mobile", studentnumber.Text);
                cnn.Parameters.AddWithValue("@birthday", dateTimePicker1.Value);
                cnn.Parameters.AddWithValue("@department", departmentcomboBox1.Text);
                string faculty = "";

                if (EEEradio.Checked)
                {
                    // if checked, assign "Engineer" to job
                    faculty = EEEradio.Text; // "Engineer"
                }
                else if (CEradio.Checked)
                {


                    faculty = CEradio.Text;
                }
                else
                {
                    // if radioButton1 is not checked, assign "Doctor" to job
                    faculty = MEradio.Text; // "Doctor"
                }

                // job variable now contains either "Engineer" or "Doctor" based on the user's choice
                cnn.Parameters.AddWithValue("@faculty", faculty);

                int rowsAffected = cnn.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Record Updated Successfully", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No record found with the given ID.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                SqlCommand cnna = new SqlCommand("select * from studenttable", con);
                SqlDataAdapter da = new SqlDataAdapter(cnna);
                DataTable table = new DataTable();
                da.Fill(table);
                dataGridView1.DataSource = table;


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            


            finally
            {
                con.Close();
            }


            studentid.Clear();
            studentname.Clear();
            studentnumber.Clear();

            departmentcomboBox1.SelectedIndex = -1;
            EEEradio.Checked = false;
            CEradio.Checked = false;
            MEradio.Checked = false;
            dateTimePicker1.Value = DateTime.Today;








        }

        private void delete_Click(object sender, EventArgs e)
        {




            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\admin\Documents\studentdata.mdf;Integrated Security=True;Connect Timeout=30");
            con.Open();

            SqlCommand cnn = new SqlCommand("delete studenttable where studentid=@studentid", con);
            cnn.Parameters.AddWithValue("@studentid", studentid.Text);

            cnn.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Record Deleted Successfully", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);


            SqlCommand cnna = new SqlCommand("select * from studenttable", con);
            SqlDataAdapter da = new SqlDataAdapter(cnna);
            DataTable table = new DataTable();
            da.Fill(table);
            dataGridView1.DataSource = table;

            studentid.Clear();
            studentname.Clear();
            studentnumber.Clear();

            departmentcomboBox1.SelectedIndex = -1;
            EEEradio.Checked = false;
            CEradio.Checked = false;
            MEradio.Checked = false;
            dateTimePicker1.Value = DateTime.Today;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            MainForm show = new MainForm();
            show.Show();
        }
    }
}
