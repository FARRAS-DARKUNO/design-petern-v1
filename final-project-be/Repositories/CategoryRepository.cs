using final_project_be.DTO;
using final_project_be.Models;
using MySql.Data.MySqlClient;

namespace final_project_be.Repositories
{
    public class CategoryRepository
    {
        private readonly string _connStr;

        public CategoryRepository(IConfiguration configuration)
        {
            _connStr = configuration.GetConnectionString("Default");
        }

        public bool InsertCategory(CategoryDTO data, string fileUrlPath)
        {
            MySqlConnection conn = new MySqlConnection(_connStr);

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO category VALUE(DEFAULT, @Name, @Description, @Image, @IsAvtive)", conn);

                cmd.Parameters.AddWithValue("@Name", data.Name);
                cmd.Parameters.AddWithValue("@Description", data.Description);
                cmd.Parameters.AddWithValue("@Image", fileUrlPath);
                cmd.Parameters.AddWithValue("@IsAvtive", data.IsActive);

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

        public List<CategoryModel> GetListCategoty(bool? status)
        {
            MySqlConnection conn = new MySqlConnection(_connStr);

            List<CategoryModel> result = new List<CategoryModel>();

            try
            {
                conn.Open();
                string sql = "";
                if (status == null)
                {
                    sql = "SELECT * FROM category";
                }
                else
                {
                    sql = "SELECT * FROM category WHERE is_active = @Status";
                }

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Status", status);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = reader.GetInt32("id_category");
                    string name = reader.GetString("name");
                    string description = reader.GetString("description");
                    string image = reader.GetString("image");
                    bool isActive = reader.GetBoolean("is_active");

                    result.Add(new CategoryModel
                    {
                        Id = id,
                        Description = description,
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

        public CategoryModel GetCategory(int id, string? type)
        {
            MySqlConnection conn = new MySqlConnection(_connStr);

            CategoryModel result = new CategoryModel();
            try
            {
                string sql = "";
                if (type == null)
                {
                    sql = "SELECT * FROM `category` WHERE id_category=@Id AND is_active = true";
                }
                else if (type == "all")
                {
                    sql = "SELECT * FROM `category` WHERE id_category=@Id";
                }
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Id = reader.GetInt32("id_category");
                    result.Name = reader.GetString("name");
                    result.Description = reader.GetString("description");
                    result.Image = reader.GetString("image");
                    result.IsActive = reader.GetBoolean("is_active");

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

        public bool UpdateCategory(CategoryDTO data, string fileUrlPath, int id)
        {
            MySqlConnection conn = new MySqlConnection(_connStr);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE category SET name=@Name, description=@Description,image=@Image, is_active = @IsAvtive WHERE id_category = @Id", conn);

                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Name", data.Name);
                cmd.Parameters.AddWithValue("@Description", data.Description);
                cmd.Parameters.AddWithValue("@Image", fileUrlPath);
                cmd.Parameters.AddWithValue("@IsAvtive", data.IsActive);

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

        public bool UpdateStatusCategory(int id, bool status)
        {
            MySqlConnection conn = new MySqlConnection(_connStr);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE category SET is_active = @IsAvtive WHERE id_category = @Id", conn);

                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@IsAvtive", status);

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
    }
}