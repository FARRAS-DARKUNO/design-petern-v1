

using final_project_be.Models;
using MySql.Data.MySqlClient;

namespace final_project_be.Repositories
{
    public class UserClassRepository
    {
        private readonly string _connStr;
        public UserClassRepository(IConfiguration configuration)
        {
            _connStr = configuration.GetConnectionString("Default");
        }

        public List<UserClassModel> GetUserClass(int idUser)
        {

            MySqlConnection conn = new MySqlConnection(_connStr);
            List<UserClassModel> result = new List<UserClassModel>();

            try
            {
                conn.Open();
                string sql = "SELECT SCHEDULE.id_schedule, product.id_product, product.image, category.name, product.NAME, schedule.date_schedule FROM invoice JOIN schedule ON invoice.fk_id_schedule = schedule.id_schedule JOIN product ON product.id_product = schedule.fk_id_product JOIN category ON category.id_category = product.fk_id_category WHERE invoice.fk_id_user = @UserId";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@UserId", idUser);

                MySqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("harus nya masuk");

                while (reader.Read())
                {
                    result.Add(new UserClassModel
                    {
                        IdSchedule = reader.GetInt32(0),
                        ProductId = reader.GetInt32(1),
                        Image = reader.GetString(2),
                        Category = reader.GetString(3),
                        Title = reader.GetString(4),
                        Schedule = DateTime.Parse(reader.GetString(5)),
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