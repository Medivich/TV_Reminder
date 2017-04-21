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
using TV_Reminder.Model;

namespace TV_Reminder.Control
{
    class AddToDataBase
    {
        public bool addTvSeries(string Name, int Id, string Overview)
        {
            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);    
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

        public bool addTvSeries(string Name, int Id, string Overview, byte[] Poster)
        {
            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand Command = new SqlCommand(@"Insert Into Series(Name, Id, Overview, Poster) 
                                                    Values(@Name, @Id, @Overview, @Poster)", Connect);

            Command.Parameters.AddWithValue("@Name", Name);
            Command.Parameters.AddWithValue("@Id", Id);
            Command.Parameters.AddWithValue("@Overview", Overview);
            Command.Parameters.AddWithValue("@Poster", Poster);

            Connect.Open();
            Command.ExecuteNonQuery();
            Connect.Close();

            return true;
        }

        public void trun()
        {
            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            string sqlTrunc = "TRUNCATE TABLE Series";
            SqlCommand cmd = new SqlCommand(sqlTrunc, Connect);

            Connect.Open();
            cmd.ExecuteNonQuery();
            Connect.Close();
        }

        public void printTvShows()
        {
            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
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
