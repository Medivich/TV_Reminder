using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV_Reminder.Model;

namespace TV_Reminder.Control
{
    class DeleteFromDataBase
    {
        //Usuwa serial
        public void deleteSeries(int Id)
        {
            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);

            SqlCommand CommandDel = new SqlCommand(@"Delete Series WHERE Id = @Id", Connect);
            CommandDel.Parameters.AddWithValue("@Id", Id);

            Connect.Open();
            CommandDel.ExecuteNonQuery();
            Connect.Close();
        }

        //Usuwa odcinki
        public void deleteEpisodes(int SeriesId)
        {
            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);

            SqlCommand CommandDel = new SqlCommand(@"Delete Episode WHERE SeriesId = @SeriesId", Connect);
            CommandDel.Parameters.AddWithValue("@SeriesId", SeriesId);

            Connect.Open();
            CommandDel.ExecuteNonQuery();
            Connect.Close();
        }
    }
}
