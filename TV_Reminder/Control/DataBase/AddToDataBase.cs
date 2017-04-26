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
        public void addTvSeries(string Name, int Id, string Overview)
        {
            ReadFromDataBase rd = new ReadFromDataBase();

            if (!rd.SeriesExist(Id))
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
            }
        }

        public void addTvSeries(string Name, int Id, string Overview, byte[] Poster)
        {
            ReadFromDataBase rd = new ReadFromDataBase();
            if (!rd.SeriesExist(Id))
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
            }
        }

        public void addEpisodes(int SeriesId, int Season, int Number, string Title, int Id, string Overview, int LastUpdate, DateTime Aired)
        {
            ReadFromDataBase rd = new ReadFromDataBase();
            if (!rd.SeriesExist(Id))
            {
                SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
                SqlCommand Command = new SqlCommand(@"Insert Into Episode(SeriesId, Season, Number, Title, Id, Overview, LastUpdate, Aired) 
                                                        Values(@SeriesId, @Season, @Number, @Title, @Id, @Overview, @LastUpdate, @Aired)", Connect);

                Command.Parameters.AddWithValue("@SeriesId", SeriesId);
                Command.Parameters.AddWithValue("@Season", Season);
                Command.Parameters.AddWithValue("@Number", Number);
                Command.Parameters.AddWithValue("@Title", Title != null ? Title : "");
                Command.Parameters.AddWithValue("@Id", Id);
                Command.Parameters.AddWithValue("@Overview", Overview != null ? Overview : "");
                Command.Parameters.AddWithValue("@LastUpdate", LastUpdate);
                Command.Parameters.AddWithValue("@Aired", Aired);

                Connect.Open();
                Command.ExecuteNonQuery();
                Connect.Close();
            }
        }

        public void addEpisodes(int SeriesId, int Season, int Number, string Title, int Id, string Overview, int LastUpdate)
        {
            ReadFromDataBase rd = new ReadFromDataBase();
            if (!rd.SeriesExist(Id))
            {
                SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
                SqlCommand Command = new SqlCommand(@"Insert Into Episode(SeriesId, Season, Number, Title, Id, Overview, LastUpdate) 
                                                        Values(@SeriesId, @Season, @Number, @Title, @Id, @Overview, @LastUpdate)", Connect);

                Command.Parameters.AddWithValue("@SeriesId", SeriesId);
                Command.Parameters.AddWithValue("@Season", Season);
                Command.Parameters.AddWithValue("@Number", Number);
                Command.Parameters.AddWithValue("@Title", Title != null ? Title : "");
                Command.Parameters.AddWithValue("@Id", Id);
                Command.Parameters.AddWithValue("@Overview", Overview != null ? Overview : "");
                Command.Parameters.AddWithValue("@LastUpdate", LastUpdate);


                Connect.Open();
                Command.ExecuteNonQuery();
                Connect.Close();
            }
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
