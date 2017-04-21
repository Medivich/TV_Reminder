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
    }
}
