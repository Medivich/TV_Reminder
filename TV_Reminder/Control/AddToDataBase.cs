using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace TV_Reminder.Control
{
    class AddToDataBase
    {
        string connString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename="
                            + Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) +
                            @"\Other\DataBase\Tv_Reminder_Database.mdf;Integrated Security=True;Connect Timeout=30";

        public bool addTvSeries(string Name, int Id, string Overview)
        {
            SqlConnection Connect = new SqlConnection(connString);    
            SqlCommand Command = new SqlCommand(@"Insert Into Series(Name, Id, Overview) 
                                                    Values(@Name, @Id, @Overview)", Connect);

            Command.Parameters.AddWithValue("@Name", Name);
            Command.Parameters.AddWithValue("@Id", Id);
            Command.Parameters.AddWithValue("@Overview", Overview);

            Connect.Open();
            Command.ExecuteNonQuery();
            Connect.Close();

            return true;
            
        }

        public void trun()
        {
            SqlConnection Connect = new SqlConnection(connString);
            string sqlTrunc = "TRUNCATE TABLE Series";
            SqlCommand cmd = new SqlCommand(sqlTrunc, Connect);

            Connect.Open();
            cmd.ExecuteNonQuery();
            Connect.Close();
        }

        public void printTvShows()
        {
            SqlConnection Connect = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("select * from Series", Connect);

            Debug.WriteLine("TV showy");
            Connect.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Debug.WriteLine(dr["Name"]);
                Debug.WriteLine(dr["Id"]);
                Debug.WriteLine(dr["Overview"]);
            }
            Connect.Close();
        }
    }
}
