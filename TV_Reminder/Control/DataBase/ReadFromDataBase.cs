using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TV_Reminder.Model;

namespace TV_Reminder.Control
{
    class ReadFromDataBase
    {
        public ObservableCollection<Series> getAllTvSeries()
        {
            ObservableCollection<Series> SeriesList = new ObservableCollection<Series>();

            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand czytajnik = new SqlCommand("SELECT * FROM Series", Connect);

            Connect.Open();
            SqlDataReader dr = czytajnik.ExecuteReader();

            while (dr.Read())
            {
                Series s = new Series();

                if (dr["Name"] != DBNull.Value)
                    s._seriesName = Convert.ToString(dr["Name"]);
                if (dr["Poster"] != DBNull.Value)
                    s._poster = (byte[])dr["Poster"];
                if (dr["Id"] != DBNull.Value) 
                    s._id = Convert.ToInt32(dr["Id"]);
                if (dr["Overview"] != DBNull.Value) 
                    s._overview = Convert.ToString(dr["Overview"]);
                if (dr["Update"] != DBNull.Value) 
                    s._update = Convert.ToBoolean(dr["Update"]);
                if (dr["Rating"] != DBNull.Value)
                    s._rating = Convert.ToInt32(dr["Rating"]);

                SeriesList.Add(s);
            }
            Connect.Close();

            return SeriesList;
        }

        public bool SeriesExist(int id)
        {
            ObservableCollection<Series> SeriesList = new ObservableCollection<Series>();

            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand czytajnik = new SqlCommand("SELECT Id FROM Series where Id = @id", Connect);
            czytajnik.Parameters.AddWithValue("@id", id);

            Connect.Open();
            SqlDataReader dr = czytajnik.ExecuteReader();
            bool exist = false;
            while (dr.Read())
            {
                if (Convert.ToInt32(dr["Id"]) == id)
                    exist = true;
            }
            Connect.Close();

            return exist;
        }

        public Series GetTvSeries(int id)
        {
            Series s = new Series();

            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand czytajnik = new SqlCommand("SELECT * FROM Series where Id = @id", Connect);
            czytajnik.Parameters.AddWithValue("@id", id);

            Connect.Open();
            SqlDataReader dr = czytajnik.ExecuteReader();

            while (dr.Read())
            {
                if (dr["Name"] != DBNull.Value)
                    s._seriesName = Convert.ToString(dr["Name"]);
                if (dr["Poster"] != DBNull.Value)
                    s._poster = (byte[])dr["Poster"];
                if (dr["Id"] != DBNull.Value)
                    s._id = Convert.ToInt32(dr["Id"]);
                if (dr["Overview"] != DBNull.Value)
                    s._overview = Convert.ToString(dr["Overview"]);
                if (dr["Update"] != DBNull.Value)
                    s._update = Convert.ToBoolean(dr["Update"]);
                if (dr["Rating"] != DBNull.Value)
                    s._rating = Convert.ToInt32(dr["Rating"]);
            }
            Connect.Close();

            return s;
        }

        public ObservableCollection<Episode> GetAllEpisodes(int SeriesId)
        {
            ObservableCollection<Episode> ep = new ObservableCollection<Episode>();

            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand czytajnik = new SqlCommand("SELECT * FROM Episode where SeriesId = @SeriesId", Connect);
            czytajnik.Parameters.AddWithValue("@SeriesId", SeriesId);

            Connect.Open();
            SqlDataReader dr = czytajnik.ExecuteReader();

            while (dr.Read())
            {
                Episode episode = new Episode();

                if (dr["Id"] != DBNull.Value)
                    episode._id = Convert.ToInt32(dr["Id"]);
                if (dr["Number"] != DBNull.Value)
                    episode._episodeNumber = Convert.ToInt32(dr["Number"]);
                if (dr["Season"] != DBNull.Value)
                    episode._seasonNumber = Convert.ToInt32(dr["Season"]);
                if (dr["Overview"] != DBNull.Value)
                    episode._overview = Convert.ToString(dr["Overview"]);
                if (dr["Title"] != DBNull.Value)
                    episode._episodeName = Convert.ToString(dr["Title"]);
                if (dr["Watched"] != DBNull.Value)
                    episode._watched = Convert.ToBoolean(dr["Watched"]);
                if (dr["Aired"] != DBNull.Value)
                    episode._aired = Convert.ToDateTime(dr["Aired"]);

                ep.Add(episode);
            }
            Connect.Close();

            return ep;
        }

        //Zwróć najstarszy, nieobejrzany odcinek
        public Episode GetLastEpisode(int SeriesId)
        {
            Episode episode = new Episode();

            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand czytajnik = new SqlCommand("SELECT * FROM Episode where SeriesId = @SeriesId AND Aired = (SELECT MIN(Aired) FROM Episode where SeriesId = @SeriesId AND Watched = 0)", Connect);
            czytajnik.Parameters.AddWithValue("@SeriesId", SeriesId);

            Connect.Open();
            SqlDataReader dr = czytajnik.ExecuteReader();

            while (dr.Read())
            {
                if (dr["Id"] != DBNull.Value)
                    episode._id = Convert.ToInt32(dr["Id"]);
                if (dr["Number"] != DBNull.Value)
                    episode._episodeNumber = Convert.ToInt32(dr["Number"]);
                if (dr["Season"] != DBNull.Value)
                    episode._seasonNumber = Convert.ToInt32(dr["Season"]);
                if (dr["Overview"] != DBNull.Value)
                    episode._overview = Convert.ToString(dr["Overview"]);
                if (dr["Title"] != DBNull.Value)
                    episode._episodeName = Convert.ToString(dr["Title"]);
                if (dr["Watched"] != DBNull.Value)
                    episode._watched = Convert.ToBoolean(dr["Watched"]);
                if (dr["Aired"] != DBNull.Value)
                    episode._aired = Convert.ToDateTime(dr["Aired"]);
            }
            Connect.Close();

            return episode;
        }

        public bool DatabaseConnected()
        {
            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);

            try
            {
                Connect.Open();
                Connect.Close();
                return true;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
