
using final_project_be.DTO;
using final_project_be.Models;
using MySql.Data.MySqlClient;

namespace final_project_be.Repositories
{
    public class CartRepository
    {
        private readonly string _connStr;
        public CartRepository(IConfiguration configuration)
        {
            _connStr = configuration.GetConnectionString("Default");
        }

        public bool InsertCart(CartDTO data, int id)
        {
            MySqlConnection conn = new MySqlConnection(_connStr);

            try
            {
                conn.Open();
                string sql = "INSERT INTO cart VALUE (DEFAULT, @IdScedule, @IdUser)";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@IdScedule", data.IdSchedule);
                cmd.Parameters.AddWithValue("@IdUser", id);

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

        public List<CartModel> GetCart(int userId)
        {
            MySqlConnection conn = new MySqlConnection(_connStr);
            List<CartModel> result = new List<CartModel>();

            try
            {
                conn.Open();
                string sql = "SELECT cart.id_cart, cart.fk_id_user, cart.fk_id_schedule, product.image, product.NAME, schedule.date_schedule, product.price, category.name as type FROM cart JOIN schedule ON cart.fk_id_schedule = schedule.id_schedule JOIN product ON product.id_product = schedule.fk_id_product JOIN category ON category.id_category = product.fk_id_category WHERE cart.fk_id_user = @IdUser";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@IdUser", userId);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new CartModel
                    {
                        Id = reader.GetInt32(0),
                        UserId = reader.GetInt32(1),
                        ScheduleId = reader.GetInt32(2),
                        Image = reader.GetString(3),
                        Title = reader.GetString(4),
                        Schedule = DateTime.Parse(reader.GetString(5)),
                        Price = reader.GetInt32(6),
                        Category = reader.GetString(7),
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

        public bool DelleteCartById(int id)
        {
            MySqlConnection conn = new MySqlConnection(_connStr);
            try
            {
                conn.Open();
                string sql = "DELETE FROM cart WHERE cart.id_cart = @Id";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Id", id);

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

        public CartModel CheckCartByUser(int idUser, int idScedule)
        {
            MySqlConnection conn = new MySqlConnection(_connStr);

            CartModel? result = null;

            try
            {
                conn.Open();
                string sql = "SELECT cart.id_cart, cart.fk_id_user, cart.fk_id_schedule, product.image, product.NAME, schedule.date_schedule, product.price, category.name as type FROM cart JOIN schedule ON cart.fk_id_schedule = schedule.id_schedule JOIN product ON product.id_product = schedule.fk_id_product JOIN category ON category.id_category = product.fk_id_category WHERE cart.fk_id_user = @IdUser AND cart.fk_id_schedule = @IdSchedule";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@IdUser", idUser);
                cmd.Parameters.AddWithValue("@IdSchedule", idScedule);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result = new CartModel
                    {
                        Id = reader.GetInt32(0),
                        UserId = reader.GetInt32(1),
                        ScheduleId = reader.GetInt32(2),
                        Image = reader.GetString(3),
                        Title = reader.GetString(4),
                        Schedule = DateTime.Parse(reader.GetString(5)),
                        Price = reader.GetInt32(6),
                        Category = reader.GetString(7),
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Menghadeh");
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine(result);

            return result;
        }
    }
}