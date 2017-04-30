using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV_Reminder.Model;

namespace TV_Reminder.Control
{
    class UpdateDataBase
    {
        public void ChangeTvSeriesRating(int SeriesId, int rate)
        {
            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);                  
            SqlCommand Command = new SqlCommand("Update Series set Rating = @rate WHERE Id = @SeriesId", Connect);

            Command.Parameters.AddWithValue("@SeriesId", SeriesId);
            Command.Parameters.AddWithValue("@rate", rate);

            Connect.Open();
            Command.ExecuteNonQuery();
            Connect.Close();
        }

        public void ChangeTvSeriesUpdate(int SeriesId, bool Update)
        {
            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand Command = new SqlCommand("Update Series set ShouldUpdate = @Update WHERE Id = @SeriesId", Connect);

            Command.Parameters.AddWithValue("@SeriesId", SeriesId);
            Command.Parameters.AddWithValue("@Update", Update);

            Connect.Open();
            Command.ExecuteNonQuery();
            Connect.Close();
        }

        public void SetWatched(int EpisodeId, bool Watched)
        {
            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand Command = new SqlCommand("Update Episode set Watched = @Watched WHERE Id = @EpisodeId", Connect);

            Command.Parameters.AddWithValue("@EpisodeId", EpisodeId);
            Command.Parameters.AddWithValue("@Watched", Watched);

            Connect.Open();
            Command.ExecuteNonQuery();
            Connect.Close();
        }

        public void AllAboveWatched(int SeriesId, int EpisodeNumber, int SeasonNumber, bool Watched)//Id = @EpisodeId
        {
            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand Command = new SqlCommand(@"Update Episode set Watched = @Watched WHERE SeriesId = @SeriesId AND (Season < @SeasonNumber 
                        OR (Season = @SeasonNumber AND Number < @EpisodeNumber))", Connect);

            Command.Parameters.AddWithValue("@EpisodeNumber", EpisodeNumber);
            Command.Parameters.AddWithValue("@SeriesId", SeriesId);
            Command.Parameters.AddWithValue("@SeasonNumber", SeasonNumber);
            Command.Parameters.AddWithValue("@Watched", Watched);

            Connect.Open();
            Command.ExecuteNonQuery();
            Connect.Close();
        }

        public void addPoster(int SeriesId, byte[] poster)
        {
            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand Poster = new SqlCommand(@"Update Series set Poster = @Poster WHERE Id = @SeriesId", Connect);
            Poster.Parameters.AddWithValue("@SeriesId", SeriesId);
            Poster.Parameters.AddWithValue("@Poster", poster);
            Connect.Open();
            Poster.ExecuteNonQuery();
            Connect.Close();
        }

        public void addBanner(int SeriesId, byte[] banner)
        {
            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand Poster = new SqlCommand(@"Update Series set Banner = @Banner WHERE Id = @SeriesId", Connect);
            Poster.Parameters.AddWithValue("@SeriesId", SeriesId);
            Poster.Parameters.AddWithValue("@Banner", banner);
            Connect.Open();
            Poster.ExecuteNonQuery();
            Connect.Close();
        }

        public void UpdateEpisode(Episode ep)
        {
            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand Command = new SqlCommand(@"Update Episode set Overview = @Overview, Title = @Title, Aired = @Aired, 
                        LastUpdate = @LastUpdate WHERE Id = @EpisodeId", Connect);

            Command.Parameters.AddWithValue("@EpisodeId", ep._id);
            Command.Parameters.AddWithValue("@Overview", ep._overview != null ? ep._overview : "");
            Command.Parameters.AddWithValue("@Title", ep._overview != null ? ep._overview : "");
            Command.Parameters.AddWithValue("@LastUpdate", ep._lastUpdate);

            if (ep._aired.Year > 1950)
                Command.Parameters.AddWithValue("@Aired", ep._aired);

            Connect.Open();
            Command.ExecuteNonQuery();
            Connect.Close();
        }
    }
}
