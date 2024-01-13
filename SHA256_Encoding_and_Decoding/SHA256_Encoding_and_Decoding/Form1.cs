using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using SHA256_Encoding_and_Decoding;
using System.Runtime.Remoting.Lifetime;

namespace SHA256_Encoding_and_Decoding
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        // Download the database in the SHA256 DATABASE AND IMAGES file and connect it to your own SQL server, then place your SQL connection address below.
        // Write your own sql server address here
        // You cannot log into the database even if you do not type
        SqlConnection connection = new SqlConnection("Write your own sql server address here");
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "")
            {
                string e_mail = textBox3.Text.ToLower();
                string mail = "@gmail";
                if (textBox1.Text != textBox2.Text )
                {
                    if (textBox4.Text.Length >= 6)
                    {
                        if (e_mail.Contains(mail))
                        {
                            string add = ("insert into User_Register (User_Name,User_Surname,User_Email,User_Password) values (@Name,@Surname,@Email,@Password)");
                            connection.Open();

                            string hashedpassword = Sha256convertor.sha256hash_(textBox4.Text);

                            SqlCommand cmd = new SqlCommand(add, connection);
                            cmd.Parameters.AddWithValue("@Name", textBox1.Text);
                            cmd.Parameters.AddWithValue("@Surname", textBox2.Text);
                            cmd.Parameters.AddWithValue("@Email", textBox3.Text);
                            cmd.Parameters.AddWithValue("@Password", hashedpassword);
                            cmd.ExecuteNonQuery();
                            connection.Close();
                            MessageBox.Show("Succesfully");
                        }
                        else
                        {
                            MessageBox.Show("E-mail should only be Gmail");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Password must be at least 6 characters");
                    }
                }
                else
                {
                    MessageBox.Show("Name and surname cannot be the same");
                }
                
            }
        }
        public class Sha256convertor
        {
            public static string sha256hash_(string rawData)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte [] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }
                    return builder.ToString();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {           
            if (textBox6.Text.Length >= 6)
            {
                if (textBox5.Text != "" && textBox6.Text != "")
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("Select * from User_Register where User_Name=@Name and User_Password=@Password", connection);

                    string hashedpassword = Sha256convertor.sha256hash_(textBox6.Text);

                    cmd.Parameters.AddWithValue("@Name", textBox5.Text);
                    cmd.Parameters.AddWithValue("@Password", hashedpassword);

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);

                    DataTable dt = new DataTable();

                    dataAdapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        MessageBox.Show("Succesfully");
                    }
                    else
                    {
                        MessageBox.Show("Name or Password incorrect");
                    }
                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("Password cannot be less than 6 characters");
            }
        }
    }
}
