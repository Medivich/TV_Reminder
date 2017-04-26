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
        public void deleteSeries(int Id)
        {
            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);

            SqlCommand CommandDel = new SqlCommand(@"Delete Series WHERE Id = @Id", Connect);
            CommandDel.Parameters.AddWithValue("@Id", Id);

            Connect.Open();
            CommandDel.ExecuteNonQuery();
            Connect.Close();
        }

        public void deleteEpisodes(int SeriesId)
        {
            SqlConnection Connect = new SqlConnection(DataBaseConnection.connString);

            SqlCommand CommandDel = new SqlCommand(@"Delete Episode WHERE SeriesId = @SeriesId", Connect);
            CommandDel.Parameters.AddWithValue("@SeriesId", SeriesId);

            Connect.Open();
            CommandDel.ExecuteNonQuery();
            Connect.Close();
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
    }
}
