using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp8
{
    public partial class Form3 : Form
    {
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\admin\Documents\loginData.mdf;Integrated Security=True;Connect Timeout=30");

        public Form3()
        {
            InitializeComponent();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void register_login_Click(object sender, EventArgs e)
        {
            Form2 sform = new Form2();
            sform.Show();
            this.Hide();
        }

        private void register_btn_Click(object sender, EventArgs e)
        {
            if (register_number.Text == "" || register_name.Text == "" || register_pasword.Text == "")
            {
                MessageBox.Show("Please fill all blank fields", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (connect.State != ConnectionState.Open)
                {
                    try
                    {
                        connect.Open();

                        string checkUsernameQuery = "SELECT * FROM admin WHERE name = @name";
                        using (SqlCommand checkUser = new SqlCommand(checkUsernameQuery, connect))
                        {
                            checkUser.Parameters.AddWithValue("@name", register_name.Text.Trim());

                            SqlDataAdapter adapter = new SqlDataAdapter(checkUser);
                            DataTable table = new DataTable();
                            adapter.Fill(table);

                            if (table.Rows.Count >= 1)
                            {
                                MessageBox.Show(register_name.Text + " is already exist", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                string insertData = "INSERT INTO admin (number, name, pasword, date_created) " +
                                                    "VALUES (@number, @name, @pasword, @date)";

                                DateTime date = DateTime.Today;

                                using (SqlCommand cmd = new SqlCommand(insertData, connect))
                                {
                                    cmd.Parameters.AddWithValue("@number", register_number.Text.Trim());
                                    cmd.Parameters.AddWithValue("@name", register_name.Text.Trim());
                                    cmd.Parameters.AddWithValue("@pasword", register_pasword.Text.Trim()); // Hashing not yet added
                                    cmd.Parameters.AddWithValue("@date", date);

                                    cmd.ExecuteNonQuery();

                                    MessageBox.Show("Registered successfully", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    // Optional: Clear input fields
                                    register_number.Clear();
                                    register_name.Clear();
                                    register_pasword.Clear();

                                    // Switch to login form
                                    Form2 lForm = new Form2();
                                    lForm.Show();
                                    this.Hide();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error connecting to database: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connect.Close();
                    }
                }
            }
        }

        private void register_show_CheckedChanged(object sender, EventArgs e)
        {
            if (register_show.Checked)
            {
                register_pasword.PasswordChar = '\0';
            }
            else
            {
                register_pasword.PasswordChar = '*';
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
