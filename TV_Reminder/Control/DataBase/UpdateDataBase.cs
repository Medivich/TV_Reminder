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
        //Zmienia ocenę serii
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

        //Ustawia czy dana seria ma być aktualizowana
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

        //Ustawia zmienną "obejrzany" danego odcinka
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

        //Poprawki - nie da się obejrzeć niewyemitowanego odcinka
        public void AllAboveUnwatched(int SeriesId, int EpisodeNumber, int SeasonNumber, bool Watched)
        {
            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand Command = new SqlCommand(@"Update Episode set Watched = @Watched WHERE SeriesId = @SeriesId AND (Season > @SeasonNumber 
                        OR (Season = @SeasonNumber AND Number > @EpisodeNumber) OR Aired > @Today)", Connect);

            Command.Parameters.AddWithValue("@EpisodeNumber", EpisodeNumber);
            Command.Parameters.AddWithValue("@SeriesId", SeriesId);
            Command.Parameters.AddWithValue("@SeasonNumber", SeasonNumber);
            Command.Parameters.AddWithValue("@Watched", Watched);
            Command.Parameters.AddWithValue("@Today", DateTime.Today);

            Connect.Open();
            Command.ExecuteNonQuery();
            Connect.Close();
        }

        //Poprawki - nie da się obejrzeć niewyemitowanego odcinka
        public void AllNotAiredUnwatched(int SeriesId)
        {
            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand Command = new SqlCommand(@"Update Episode set Watched = @Watched WHERE SeriesId = @SeriesId AND Aired > @Today", Connect);

            Command.Parameters.AddWithValue("@SeriesId", SeriesId);
            Command.Parameters.AddWithValue("@Watched", false);
            Command.Parameters.AddWithValue("@Today", DateTime.Today);

            Connect.Open();
            Command.ExecuteNonQuery();
            Connect.Close();
        }

        //Wszystkie odcinki poniżej SxEx z danego serialu są oznaczane jako obejrzane (Watched)
        public void AllBelowWatched(int SeriesId, int EpisodeNumber, int SeasonNumber, bool Watched)
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

        //Wszystkie odcinki poniżej SxEx z danego serialu są oznaczane jako obejrzane (Watched)
        public void AllWatched(int SeriesId, bool Watched)
        {
            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand Command = new SqlCommand(@"Update Episode set Watched = @Watched WHERE SeriesId = @SeriesId", Connect);

            Command.Parameters.AddWithValue("@SeriesId", SeriesId);
            Command.Parameters.AddWithValue("@Watched", Watched);

            Connect.Open();
            Command.ExecuteNonQuery();
            Connect.Close();
        }

        //Dodaje plakat do serii
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

        //Dodaje baner do serii
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
            if (ep._aired.Year > 1950)
                UpdateEpisodeWithDate(ep);
            else
                UpdateEpisodeWithoutDate(ep);
        }

        //Aktualizuje odcinek
        private void UpdateEpisodeWithDate(Episode ep)
        {
            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand Command = new SqlCommand(@"Update Episode set Overview = @Overview, Title = @Title, Aired = @Aired, 
                        LastUpdate = @LastUpdate WHERE Id = @EpisodeId", Connect);

            Command.Parameters.AddWithValue("@EpisodeId", ep._id);
            Command.Parameters.AddWithValue("@Overview", ep._overview != null ? ep._overview : "");
            Command.Parameters.AddWithValue("@Title", ep._episodeName != null ? ep._episodeName : "");
            Command.Parameters.AddWithValue("@LastUpdate", ep._lastUpdate);
            Command.Parameters.AddWithValue("@Aired", ep._aired);

            Connect.Open();
            Command.ExecuteNonQuery();
            Connect.Close();
        }

        //Aktualizuje odcinek
        private void UpdateEpisodeWithoutDate(Episode ep)
        {
            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);
            SqlCommand Command = new SqlCommand(@"Update Episode set Overview = @Overview, Title = @Title, 
                        LastUpdate = @LastUpdate WHERE Id = @EpisodeId", Connect);

            Command.Parameters.AddWithValue("@EpisodeId", ep._id);
            Command.Parameters.AddWithValue("@Overview", ep._overview != null ? ep._overview : "");
            Command.Parameters.AddWithValue("@Title", ep._episodeName != null ? ep._episodeName : "");
            Command.Parameters.AddWithValue("@LastUpdate", ep._lastUpdate);

            Connect.Open();
            Command.ExecuteNonQuery();
            Connect.Close();
        }
    }
}
