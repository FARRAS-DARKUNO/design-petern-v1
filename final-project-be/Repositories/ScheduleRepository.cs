

using final_project_be.DTO;
using final_project_be.Models;
using MySql.Data.MySqlClient;

namespace final_project_be.Repositories
{
    public class ScheduleRepository
    {
        private readonly string _connStr;
        public ScheduleRepository(IConfiguration configuration)
        {
            _connStr = configuration.GetConnectionString("Default");
        }

        public bool InsertSchedule(ScheduleDTO data)
        {
            MySqlConnection conn = new MySqlConnection(_connStr);

            try
            {
                conn.Open();
                string sql = "INSERT INTO schedule VALUES (DEFAULT,@DateTime, @IdProduct)";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@DateTime", data.Schedule);
                cmd.Parameters.AddWithValue("@IdProduct", data.ProductId);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                conn.Close();
                Console.WriteLine(ex.ToString());
                return false;
            }
            conn.Close();

            return true;
        }
        public List<ScheduleModel> GetSchedule()
        {

            MySqlConnection conn = new MySqlConnection(_connStr);
            List<ScheduleModel> result = new List<ScheduleModel>();

            try
            {
                conn.Open();
                string sql = "SELECT id_schedule, date_schedule, fk_id_product, product.name FROM schedule JOIN product ON product.id_product = schedule.fk_id_product";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                MySqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("harus nya masuk");

                while (reader.Read())
                {
                    int id = reader.GetInt32("id_schedule");
                    DateTime time = DateTime.Parse(reader.GetString("date_schedule"));
                    string name = reader.GetString("name");

                    result.Add(new ScheduleModel
                    {
                        Id = id,
                        Date = time,
                        TitleProduct = name
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Menghadeh");
                Console.WriteLine(ex.ToString());
            }
            conn.Close();

            return result;
        }
    }
}