using System.Security.Principal;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace WebPicture.Models
{
    public class DBmanager
    {
        private readonly string connStr = "Data Source=(localdb)\\MSSQLLocalDB;Database=picture;User ID=Kai86;Password=Password;Trusted_Connection=True";
        public List<picture> getAccounts()
        {
            List<picture> accounts = new List<picture>();

            SqlConnection sqlConnection = new SqlConnection(connStr);
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM pictureInformation");
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    picture picturet = new picture
                    {
                        id = reader.GetInt32(reader.GetOrdinal("id")),
                        title = reader.GetString(reader.GetOrdinal("title")),
                        url = reader.GetString(reader.GetOrdinal("url")),
                        width = reader.GetInt32(reader.GetOrdinal("width")),
                        height = reader.GetInt32(reader.GetOrdinal("height")),
                        theme = reader.GetString(reader.GetOrdinal("theme")),
                    };
                    accounts.Add(picturet);
                }
            }
            else
            {
                Console.WriteLine("資料庫為空！");
            }
            sqlConnection.Close();
            return accounts;
        }
        public void newAccount(Item user)
        {
            SqlConnection sqlconnection = new SqlConnection(connStr);
            SqlCommand sqlcommand = new SqlCommand(@"INSERT INTO pictureInformation(title,url,width,height,theme) VALUES(@title,@url,@width,@height,@theme)");
            sqlcommand.Connection = sqlconnection;

                sqlcommand.Parameters.Add(new SqlParameter("@title", user.title));
                sqlcommand.Parameters.Add(new SqlParameter("@url", user.url));
                sqlcommand.Parameters.Add(new SqlParameter("@width", user.width));
                sqlcommand.Parameters.Add(new SqlParameter("@height", user.height));
                sqlcommand.Parameters.Add(new SqlParameter("@theme", user.theme));

            sqlconnection.Open();
            sqlcommand.ExecuteNonQuery();
            sqlconnection.Close();
        }
    }
}
