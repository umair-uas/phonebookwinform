using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PhoneBook
{
    public partial class PhoneBook : Form
    {
        SqlConnection sqlcon = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\Umair\Documents\PhoneRecord.mdf;Integrated Security=True;Connect Timeout=30");

        public PhoneBook()
        {
            InitializeComponent();
        }

        private void Phone_Load(object sender, EventArgs e)
        {
            // fetch al the record on run
            Display();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Clear();
            textBox3.Text = "";
            textBox4.Clear();
            comboBox1.SelectedIndex = -1;
            textBox1.Focus();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Insert data in sql database
                if (sqlcon.State == ConnectionState.Closed)
                    sqlcon.Open();

             //   String insertquery = @"INSERT INTO Mobiles ( First,Last,Mobile,Email,Category)
               // VALUES ('" + textBox1.Text + "', '" + textBox2.Text + "', '" + textBox3.Text + " ','" + textBox4.Text + "','" + comboBox1.Text + "')";

            // parameterized way of adding the data with sql 
                String newinsertquery = @"INSERT INTO Mobiles ( First,Last,Mobile,Email,Category)
                VALUES (@FirstName, @LastName,@Mobile,@Email,@Category)";

        if(textBox3.Text!="")
        {    SqlCommand sqlcmd = new SqlCommand(newinsertquery, sqlcon);
                sqlcmd.Parameters.AddWithValue("@FirstName", textBox1.Text);
                sqlcmd.Parameters.AddWithValue("@LastName", textBox2.Text);
                sqlcmd.Parameters.AddWithValue("@Mobile", textBox3.Text);
                sqlcmd.Parameters.AddWithValue("@Email", textBox4.Text);
                sqlcmd.Parameters.AddWithValue("@Category", comboBox1.Text);
               

                sqlcmd.ExecuteNonQuery();

                sqlcon.Close();
                MessageBox.Show("Successfully Inserted in the Database...");
                //ShowDBDatainGridView();
                Display();
        } else {
            MessageBox.Show("Cannot enter data without Mobile details ...");
        }
        }

         void Display(){
            SqlDataAdapter sda = new SqlDataAdapter("Select * from Mobiles", sqlcon);
            DataTable dt = new DataTable();
            
            sda.Fill(dt);
           dataGridView1.Rows.Clear();
           //dataGridView1.DataSource = dt;
           foreach (DataRow item in dt.Rows)
                {

                    int n = dataGridView1.Rows.Add();
                    dataGridView1.Rows[n].Cells[0].Value = item["First"].ToString();
                    dataGridView1.Rows[n].Cells[1].Value = item[1].ToString();
                    dataGridView1.Rows[n].Cells[2].Value = item[2].ToString();
                    dataGridView1.Rows[n].Cells[3].Value = item[3].ToString();
                    dataGridView1.Rows[n].Cells[4].Value = item[4].ToString();
                }
          // sda.Update(dt);
            }

         private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
         {
             if (dataGridView1.Rows.Count > 0)
             {
                 textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                 textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                 textBox3.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                 textBox4.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                 comboBox1.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
             }
         }

         private void button3_Click(object sender, EventArgs e)
         {
             // deleteButton
             
                 if (sqlcon.State == ConnectionState.Closed)
                     sqlcon.Open();

                 String deletequery = @"DELETE FROM Mobiles 
                 WHERE (Mobile =@mobile)";

                 SqlCommand sqlCmd = new SqlCommand(deletequery, sqlcon);
                 sqlCmd.Parameters.AddWithValue("@Mobile", textBox3.Text);

                 sqlCmd.ExecuteNonQuery();
                 sqlcon.Close();

                 MessageBox.Show("Successfully Deleted from the  Database...");
                 Display();

         }

         private void button4_Click(object sender, EventArgs e)
         {
             //update query
            
                 if (sqlcon.State == ConnectionState.Closed)
                     sqlcon.Open();

                // String updatequery = "UPDATE  Mobiles SET First='"+ textBox1.Text + "', Last='"+ textBox2.Text + "', Mobile='"+textBox3.Text + "', Email='"+ textBox4.Text + "', Category=' " + comboBox1.Text +  " ' WHERE (Mobile = '"+ textBox3.Text + "') ";

             // new parameterized updatequery
                 String updatequery = "UPDATE  Mobiles SET First=@first, Last=@last,Mobile=@mobile,Email=@email,Category=@cat  WHERE (Mobile =@mobile) ";
                 if (textBox3.Text != "")
                 {
                     SqlCommand sqlCmd = new SqlCommand(updatequery, sqlcon);
                     sqlCmd.Parameters.AddWithValue("@first", textBox1.Text);
                     sqlCmd.Parameters.AddWithValue("@last", textBox2.Text);
                     sqlCmd.Parameters.AddWithValue("@mobile", textBox3.Text);
                     sqlCmd.Parameters.AddWithValue("@email", textBox4.Text);
                     sqlCmd.Parameters.AddWithValue("@cat", comboBox1.Text);

                     sqlCmd.ExecuteNonQuery();
                     sqlcon.Close();

                     MessageBox.Show("Successfully updated...");
                     Display();
                 }
                 else { MessageBox.Show("Cannot Enter Record without Mobile details ..."); }
         }

         private void textBox5_TextChanged(object sender, EventArgs e)
         {
             SqlDataAdapter sda = new SqlDataAdapter("Select * from Mobiles WHERE (Mobile like '" + textBox5.Text + "%') or (First like '" + textBox5.Text + "%') or (Last like '" + textBox5.Text + "%') ", sqlcon);
             DataTable dt = new DataTable();
             sda.Fill(dt);
             dataGridView1.Rows.Clear();
             foreach (DataRow item in dt.Rows)
             {

                 int n = dataGridView1.Rows.Add();
                 dataGridView1.Rows[n].Cells[0].Value = item["First"].ToString();
                 dataGridView1.Rows[n].Cells[1].Value = item[1].ToString();
                 dataGridView1.Rows[n].Cells[2].Value = item[2].ToString();
                 dataGridView1.Rows[n].Cells[3].Value = item[3].ToString();
                 dataGridView1.Rows[n].Cells[4].Value = item[4].ToString();
             }
            

         }

        
    }
}
