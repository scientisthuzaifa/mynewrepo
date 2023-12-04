using DocumentFormat.OpenXml.Wordprocessing;
using Irony;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gridviewpagewebform
{
    public partial class AddNewRecord : System.Web.UI.Page
    {
        string cs = System.Configuration.ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {
            string username = TextBox1.Text;
            string rollnumber = TextBox2.Text;
            
            string phonenumber = TextBox4.Text;
            string address = TextBox3.Text;
            // Validate if required fields are not empty
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(rollnumber) && !string.IsNullOrEmpty(phonenumber) && !string.IsNullOrEmpty(address))
            {
                // Insert data into the database
                InsertDataIntoDatabase(username, rollnumber, phonenumber, address);

                // Redirect to StudentPage.aspx after successful insert
                Response.Redirect("~/studentdata.aspx");
            }
            else
            {
                // Handle the case when required fields are empty
                //lblMessage.Text = "Please fill in all the required fields.";
            }
        }

        private void InsertDataIntoDatabase(string username, string rollnumber, string address, string phonenumber)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO studenttbl (name, rollnumber, phoenumber, address) VALUES (@name, @rollnumber, @phoenumber, @address)", con);

                
                cmd.Parameters.AddWithValue("@name", username);
                cmd.Parameters.AddWithValue("@rollnumber", rollnumber);
                cmd.Parameters.AddWithValue("@phoenumber", phonenumber);
                cmd.Parameters.AddWithValue("@address", address);

                cmd.ExecuteNonQuery();
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/studentdata.aspx");
        }
    }
}