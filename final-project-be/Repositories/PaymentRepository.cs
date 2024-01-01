using final_project_be.DTO;
using final_project_be.Models;
using MySql.Data.MySqlClient;

namespace final_project_be.Repositories
{
    public class PaymentRepository
    {
        private readonly string _connStr;

        public PaymentRepository(IConfiguration configuration)
        {
            _connStr = configuration.GetConnectionString("Default");
        }

        public bool InsertPayment(PaymentDTO data, string fileUrlPath)
        {
            MySqlConnection conn = new MySqlConnection(_connStr);

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO payment VALUE(DEFAULT, @Name, @Image, @IsActive)", conn);

                cmd.Parameters.AddWithValue("@Name", data.Name);
                cmd.Parameters.AddWithValue("@Image", fileUrlPath);
                cmd.Parameters.AddWithValue("@IsActive", data.IsActive);

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

        public List<PaymentModel> GetListPayment(bool? status)
        {
            MySqlConnection conn = new MySqlConnection(_connStr);

            List<PaymentModel> result = new List<PaymentModel>();

            try
            {
                conn.Open();
                string sql = "";
                if (status == null)
                {
                    sql = "SELECT * FROM payment";
                }
                else
                {
                    sql = "SELECT * FROM payment WHERE is_active = @Status";
                }

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Status", status);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = reader.GetInt32("id_payment");
                    string name = reader.GetString("name");
                    string image = reader.GetString("image");
                    bool isActive = reader.GetBoolean("is_active");

                    result.Add(new PaymentModel
                    {
                        Id = id,
                        Image = image,
                        IsActive = isActive,
                        Name = name
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");

            return result;
        }

        public bool UpdatePayment(PaymentDTO data, string fileUrlPath, int id)
        {
            MySqlConnection conn = new MySqlConnection(_connStr);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE payment SET name=@Name, image=@Image, is_active = @IsActive WHERE id_payment = @Id", conn);

                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Name", data.Name);
                cmd.Parameters.AddWithValue("@Image", fileUrlPath);
                cmd.Parameters.AddWithValue("@IsActive", data.IsActive);

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

        public bool UpdateStatusPayment(bool status, int id)
        {
            MySqlConnection conn = new MySqlConnection(_connStr);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE payment SET is_active = @IsActive WHERE id_payment = @Id", conn);

                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@IsActive", status);

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

        public PaymentModel GetListPaymentById(int idPayment)
        {
            MySqlConnection conn = new MySqlConnection(_connStr);

            PaymentModel result = new PaymentModel();

            try
            {
                conn.Open();
                string sql = "SELECT * FROM payment WHERE id_payment = @Id";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Id", idPayment);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = reader.GetInt32("id_payment");
                    string name = reader.GetString("name");
                    string image = reader.GetString("image");
                    bool isActive = reader.GetBoolean("is_active");

                    result.Id = id;
                    result.Name = name;
                    result.Image = image;
                    result.IsActive = isActive;

                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            Console.WriteLine("Done.");

            return result;
        }
    }
}