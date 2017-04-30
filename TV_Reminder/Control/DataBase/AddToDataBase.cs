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
        public void addTvSeries(Series s)
        {
            ReadFromDataBase rd = new ReadFromDataBase();

            if (!rd.SeriesExist(s._id))
            {
                SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
                SqlCommand Command = new SqlCommand(@"Insert Into Series(Name, Id, Overview, LastUpdate) 
                                                    Values(@Name, @Id, @Overview, @LastUpdate)", Connect);

                Command.Parameters.AddWithValue("@Name", s._seriesName);
                Command.Parameters.AddWithValue("@Id", s._id);
                Command.Parameters.AddWithValue("@Overview", s._overview);
                Command.Parameters.AddWithValue("@LastUpdate", s._lastUpdated);


                Connect.Open();
                Command.ExecuteNonQuery();
                Connect.Close();

                UpdateDataBase UB = new UpdateDataBase();

                if (s._poster != null)
                    UB.addPoster(s._id, s._poster);

                if (s._banner != null)
                    UB.addBanner(s._id, s._banner);

            }
        }

        //Zmienić jak wyżej
        public void addEpisode(int SeriesId, Episode e)
        {
            if (e._aired.Year > 1950)
                addEpisodeWithDate(SeriesId, e);
            else
                addEpisodeWithoutDate(SeriesId, e);
        }

        private void addEpisodeWithDate(int SeriesId, Episode e)
        {
            ReadFromDataBase rd = new ReadFromDataBase();
            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand Command = new SqlCommand(@"Insert Into Episode(SeriesId, Season, Number, Title, Id, Overview, LastUpdate, Aired) 
                                                    Values(@SeriesId, @Season, @Number, @Title, @Id, @Overview, @LastUpdate, @Aired)", Connect);

            Command.Parameters.AddWithValue("@SeriesId", SeriesId);
            Command.Parameters.AddWithValue("@Season", e._seasonNumber);
            Command.Parameters.AddWithValue("@Number", e._episodeNumber);
            Command.Parameters.AddWithValue("@Title", e._episodeName != null ? e._episodeName : "");
            Command.Parameters.AddWithValue("@Id", e._id);
            Command.Parameters.AddWithValue("@Overview", e._overview != null ? e._overview : "");
            Command.Parameters.AddWithValue("@LastUpdate", e._lastUpdate);
            Command.Parameters.AddWithValue("@Aired", e._aired);

            Connect.Open();
            Command.ExecuteNonQuery();
            Connect.Close();
        }

        private void addEpisodeWithoutDate(int SeriesId, Episode e)
        {
            ReadFromDataBase rd = new ReadFromDataBase();
            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand Command = new SqlCommand(@"Insert Into Episode(SeriesId, Season, Number, Title, Id, Overview, LastUpdate) 
                                                    Values(@SeriesId, @Season, @Number, @Title, @Id, @Overview, @LastUpdate)", Connect);

            Command.Parameters.AddWithValue("@SeriesId", SeriesId);
            Command.Parameters.AddWithValue("@Season", e._seasonNumber);
            Command.Parameters.AddWithValue("@Number", e._episodeNumber);
            Command.Parameters.AddWithValue("@Title", e._episodeName != null ? e._episodeName : "");
            Command.Parameters.AddWithValue("@Id", e._id);
            Command.Parameters.AddWithValue("@Overview", e._overview != null ? e._overview : "");
            Command.Parameters.AddWithValue("@LastUpdate", e._lastUpdate);

            Connect.Open();
            Command.ExecuteNonQuery();
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
