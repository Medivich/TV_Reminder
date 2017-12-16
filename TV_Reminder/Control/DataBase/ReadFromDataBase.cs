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
using System.Collections;

namespace TV_Reminder.Control
{
    class ReadFromDataBase
    {
        //Pobiera wszystkie seriale
        public ObservableCollection<Series> getAllTvSeries()
        {
            ObservableCollection<Series> SeriesList = new ObservableCollection<Series>();

            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand czytajnik = new SqlCommand("SELECT * FROM Series", Connect);

            Connect.Open();
            SqlDataReader dr = czytajnik.ExecuteReader();

            while (dr.Read())
                SeriesList.Add(readSeries(dr));

            Connect.Close();

            return SeriesList;
        }

        //Pobiera wszystkie seriale
        public ObservableCollection<Series> getTvSeriesByName(string name)
        {
            ObservableCollection<Series> SeriesList = new ObservableCollection<Series>();

            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand czytajnik = new SqlCommand("SELECT * FROM Series WHERE Name Like @name", Connect);
            czytajnik.Parameters.AddWithValue("@name", "%" + name + "%");


            Connect.Open();
            SqlDataReader dr = czytajnik.ExecuteReader();

            while (dr.Read())
                SeriesList.Add(readSeries(dr));

            Connect.Close();

            return SeriesList;
        }

        //Sprawdza czy serial istnieje w bazie danych
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
                if (Convert.ToInt32(dr["Id"]) == id)
                    exist = true;

            Connect.Close();

            return exist;
        }

        //Sprawdza czy odcinek istnieje w bazie danych
        public bool EpisodeExist(int EpisodeId)
        {
            ObservableCollection<Series> SeriesList = new ObservableCollection<Series>();

            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand czytajnik = new SqlCommand("SELECT Id FROM Episode where Id = @EpisodeId", Connect);
            czytajnik.Parameters.AddWithValue("@EpisodeId", EpisodeId);

            Connect.Open();
            SqlDataReader dr = czytajnik.ExecuteReader();
            bool exist = false;

            while (dr.Read())
                if (Convert.ToInt32(dr["Id"]) == EpisodeId)
                    exist = true;

            Connect.Close();

            return exist;
        }

        //Zwraca kiedy odcinek byl aktualizowany ostatnio
        public int EpisodeLastUpdate(int EpisodeId)
        {
            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand czytajnik = new SqlCommand("SELECT LastUpdate FROM Episode where Id = @EpisodeId", Connect);
            czytajnik.Parameters.AddWithValue("@EpisodeId", EpisodeId);

            Connect.Open();
            SqlDataReader dr = czytajnik.ExecuteReader();

            int _lastUpdate = 0;
            while (dr.Read())
                _lastUpdate = Convert.ToInt32(dr["LastUpdate"]);

            Connect.Close();
            return _lastUpdate;
        }

        //Pobiera dane dotyczące serii
        public Series GetTvSeries(int id)
        {
            Series s = new Series();

            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand czytajnik = new SqlCommand("SELECT * FROM Series where Id = @id", Connect);
            czytajnik.Parameters.AddWithValue("@id", id);

            Connect.Open();
            SqlDataReader dr = czytajnik.ExecuteReader();

            while (dr.Read())
                s = readSeries(dr);

            Connect.Close();

            return s;
        }

        //Pobiera wszystkie odcinki
        public ObservableCollection<Episode> GetAllEpisodes(int SeriesId)
        {
            ObservableCollection<Episode> ep = new ObservableCollection<Episode>();

            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand czytajnik = new SqlCommand("SELECT * FROM Episode where SeriesId = @SeriesId", Connect);
            czytajnik.Parameters.AddWithValue("@SeriesId", SeriesId);

            Connect.Open();
            SqlDataReader dr = czytajnik.ExecuteReader();

            while (dr.Read())
                ep.Add(readEpisode(dr));

            Connect.Close();

            return ep;
        }

        //Pobiera wszystkie dostępne, nieobejrzane odcinki
        public ObservableCollection<Episode> GetAllUnWatchedEpisodes(int SeriesId)
        {
            ObservableCollection<Episode> ep = new ObservableCollection<Episode>();

            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand czytajnik = new SqlCommand("SELECT * FROM Episode where SeriesId = @SeriesId AND Watched = 0 AND Aired < @Current", Connect);
            czytajnik.Parameters.AddWithValue("@SeriesId", SeriesId);
            czytajnik.Parameters.AddWithValue("@Current", DateTime.Now);


            Connect.Open();
            SqlDataReader dr = czytajnik.ExecuteReader();

            while (dr.Read())
            {
                Episode episode = readEpisode(dr);

                if(episode._aired.Year > 1950)
                    ep.Add(episode);
            }
            Connect.Close();

            return ep;
        }

        //Pobiera wszystkie odcinki wemitowane między <Start, End>
        public ObservableCollection<Episode> GetAllEpisodesBetween(int SeriesId, DateTime Start, DateTime End)
        {
            ObservableCollection<Episode> ep = new ObservableCollection<Episode>();

            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand czytajnik = new SqlCommand(@"SELECT * FROM Episode where SeriesId = @SeriesId 
                    AND Watched = 0 AND Aired > @Start AND Aired < @End", Connect);

            czytajnik.Parameters.AddWithValue("@SeriesId", SeriesId);
            czytajnik.Parameters.AddWithValue("@Start", Start);
            czytajnik.Parameters.AddWithValue("@End", End);

            Connect.Open();
            SqlDataReader dr = czytajnik.ExecuteReader();

            while (dr.Read())
            {
                Episode episode = readEpisode(dr);

                if (episode._aired.Year > 1950)
                    ep.Add(episode);
            }
            Connect.Close();

            return ep;
        }


        //Zwróć najstarszy, nieobejrzany odcinek
        public Episode GetLastEpisode(int SeriesId)
        {
            List<Episode> ep = new List<Episode>();

            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand czytajnik = new SqlCommand("SELECT * FROM Episode where SeriesId = @SeriesId AND Watched = 0", Connect);
            czytajnik.Parameters.AddWithValue("@SeriesId", SeriesId);

            Connect.Open();
            SqlDataReader dr = czytajnik.ExecuteReader();

            while (dr.Read())
                ep.Add(readEpisode(dr));

            Connect.Close();

            //Sortowanie
            ep = ep.OrderBy(x => x._seasonNumber).ThenBy(y => y._episodeNumber).ToList();

            if (ep.Count > 0 && ep[0]._aired.Year > 1950)
                return ep[0];
            else
                return null;
        }

        //Pobiera najstarszy, nieobejrzany i dostępny odcinek
        public Episode GetLastAvaiableEpisode(int SeriesId)
        {
            List<Episode> ep = new List<Episode>();

            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand czytajnik = new SqlCommand("SELECT * FROM Episode " +
                                                  "where SeriesId = @SeriesId AND Watched = 0 AND (Aired < @Today OR Aired = @Today)" +
                                                  "ORDER BY CONVERT(DATE, Aired) ASC", Connect);
            czytajnik.Parameters.AddWithValue("@SeriesId", SeriesId);
            czytajnik.Parameters.AddWithValue("@Today", DateTime.Today);

            Connect.Open();
            SqlDataReader dr = czytajnik.ExecuteReader();

            while (dr.Read())
                ep.Add(readEpisode(dr));
            
            Connect.Close();

            ep = ep.OrderBy(x => x._seasonNumber).ThenBy(y => y._episodeNumber).ToList();

            if (ep.Count > 0 && ep[0]._aired.Year > 1950)
                return ep[0];
            else
                return null;
        }

        //Pobiera z bazy odcinek, sprawdza istnienie nieobowiązkowych komórek
        private Episode readEpisode(SqlDataReader dr)
        {
            Episode episode = new Episode();

            episode._id = Convert.ToInt32(dr["Id"]);
            episode._episodeNumber = Convert.ToInt32(dr["Number"]);
            episode._seasonNumber = Convert.ToInt32(dr["Season"]);
            episode._watched = Convert.ToBoolean(dr["Watched"]);

            if (dr["Overview"] != DBNull.Value)
                episode._overview = Convert.ToString(dr["Overview"]);
            if (dr["Title"] != DBNull.Value)
                episode._episodeName = Convert.ToString(dr["Title"]);            
            if (dr["Aired"] != DBNull.Value)
                episode._aired = Convert.ToDateTime(dr["Aired"]);

            return episode;
        }

        //Pobiera z bazy serial, sprawdza istnienie nieobowiązkowych komórek
        private Series readSeries(SqlDataReader dr)
        {
            Series series = new Series();

            series._id = Convert.ToInt32(dr["Id"]);
            series._seriesName = Convert.ToString(dr["Name"]);
            series._update = Convert.ToBoolean(dr["ShouldUpdate"]);
            series._rating = Convert.ToInt32(dr["Rating"]);

            if (dr["Overview"] != DBNull.Value)
                series._overview = Convert.ToString(dr["Overview"]);

            if (dr["Poster"] != DBNull.Value)
                series._poster = (byte[])dr["Poster"];
            else
                series._poster = null;

            if (dr["Banner"] != DBNull.Value)
                series._banner = (byte[])dr["Banner"];
            else
                series._banner = null;

            return series;
        }

        #region statystyka

        //Pobiera najstarszy, nieobejrzany i dostępny odcinek
        public int GetEpisodeNo()
        {
            List<Episode> ep = new List<Episode>();

            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand czytajnik = new SqlCommand("SELECT COUNT(*) FROM Episode", Connect);

            Connect.Open();
            SqlDataReader dr = czytajnik.ExecuteReader();

            dr.Read();
            int no = Convert.ToInt32(dr[0]);

            Connect.Close();
      
            return no;
        }

        public int GetSeriesNo()
        {
            List<Episode> ep = new List<Episode>();

            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand czytajnik = new SqlCommand("SELECT COUNT(*) FROM Series", Connect);

            Connect.Open();
            SqlDataReader dr = czytajnik.ExecuteReader();

            dr.Read();
            int no = Convert.ToInt32(dr[0]);

            Connect.Close();

            return no;
        }

        public int GetWatchedEpisodesNo(bool watched)
        {
            List<Episode> ep = new List<Episode>();

            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand czytajnik = new SqlCommand("SELECT COUNT(*) FROM Episode WHERE Watched = @Watched", Connect);
            czytajnik.Parameters.AddWithValue("@Watched", watched);

            Connect.Open();
            SqlDataReader dr = czytajnik.ExecuteReader();

            dr.Read();
            int no = Convert.ToInt32(dr[0]);

            Connect.Close();

            return no;
        }

        #endregion


        //Sprawdzenie połączenia
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
