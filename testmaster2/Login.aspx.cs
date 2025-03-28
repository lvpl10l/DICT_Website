﻿//*******************************************************************
// Programmer :Jayabharathi
// Date: 14-10-2019
// Purpose: login into the forum .Provide correct username and password to login .then redirects to forum home page.
// Software:   Microsoft Visual Studio 2019 Community Edition
// Platform:   Microsoft Windows 
//******************************************************************* 
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DICT_Website
{
	public partial class Login : System.Web.UI.Page
	{
        string strConnString = ConfigurationManager.ConnectionStrings["DICTMySqlConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
		{

		}

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection sqlCon = new MySqlConnection(strConnString))
                {
                    sqlCon.Open();
                    MySqlCommand sqlCmd = new MySqlCommand("sp_CheckAuthUser", sqlCon);
                    sqlCmd.CommandType = CommandType.StoredProcedure;                    
                    sqlCmd.Parameters.AddWithValue("F_Register_ID", txtUserName.Text);                    
                    sqlCmd.Parameters.AddWithValue("F_Password", txtPassword.Text);
                    // sqlCmd.ExecuteNonQuery();
                    string output = sqlCmd.ExecuteScalar().ToString();
                    if (output == "1")
                    {
                        lblSuccessMessage.Text = "Login Successed";
                        
                        DataTable dtbl = new DataTable();
                        MySqlDataAdapter sqlPerson = new MySqlDataAdapter("sp_FristNameByID", sqlCon);
                        sqlPerson.SelectCommand.CommandType = CommandType.StoredProcedure;
                        sqlPerson.SelectCommand.Parameters.AddWithValue("registerID", txtUserName.Text);
                        sqlPerson.SelectCommand.CommandType = CommandType.StoredProcedure;
                        sqlPerson.Fill(dtbl);
                        string firstName = dtbl.Rows[0][0].ToString();
                        Session["RegID"] = Convert.ToInt32(txtUserName.Text);
                        Session["Username"] = firstName;
                        //Session["Role"] = reader["Role"].ToString();
                        Response.Redirect("~/ForumHomePage.aspx");
                    }
                    else
                    {
                        lblSuccessMessage.Text = "Sorry,Not authorised users.Check your username and password.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblSuccessMessage.Text = ex.Message;
            }
        }
    }
}