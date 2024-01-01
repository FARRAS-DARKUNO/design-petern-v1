using System.Data;
using final_project_be.DTO;
using final_project_be.Models;
using MySql.Data.MySqlClient;
namespace final_project_be.Repositories
{
    public class ProductRepository
    {
        private readonly string _connStr;
        public ProductRepository(IConfiguration configuration)
        {
            _connStr = configuration.GetConnectionString("Default");
        }

        public bool InsertProduct(ProductDTO data, string fileUrlPath)
        {
            MySqlConnection conn = new MySqlConnection(_connStr);
            try
            {
                conn.Open();
                string sql = "INSERT INTO product VALUES (DEFAULT, @Name, @Description, @Price, @Image, @status ,@FKIdCategory)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Name", data.TitleProduct);
                cmd.Parameters.AddWithValue("@Description", data.DescriptionProduct);
                cmd.Parameters.AddWithValue("@Price", data.Price);
                cmd.Parameters.AddWithValue("@Image", fileUrlPath);
                cmd.Parameters.AddWithValue("@status", data.IsActive);
                cmd.Parameters.AddWithValue("@FKIdCategory", data.CategoryId);

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

        public List<ProductModel> GetProduct(int? limit, int? categoryId, int? productId)
        {
            MySqlConnection conn = new MySqlConnection(_connStr);
            List<ProductModel> result = new List<ProductModel>();
            try
            {
                conn.Open();
                string sql = "";
                if (limit == null && categoryId == null && productId == null)
                {
                    sql = "SELECT product.id_product, product.NAME, product.image, product.DESCRIPTION, category.id_category, category.name as type, product.is_active, product.price FROM product JOIN category ON category.id_category = product.fk_id_category";
                }
                else if (limit != null && categoryId == null && productId == null)
                {
                    sql = "SELECT product.id_product, product.NAME, product.image, product.DESCRIPTION, category.id_category, category.name as type, product.is_active, product.price FROM product JOIN category ON category.id_category = product.fk_id_category WHERE product.is_active = true ORDER BY product.id_product DESC LIMIT @Limit";
                }
                else if (limit != null && categoryId != null && productId == null)
                {
                    sql = "SELECT product.id_product, product.NAME, product.image, product.DESCRIPTION, category.id_category, category.name as type, product.is_active, product.price FROM product JOIN category ON category.id_category = product.fk_id_category WHERE product.is_active = true AND category.id_category = @CategoryId ORDER BY product.id_product DESC LIMIT @Limit";
                }
                else
                {
                    sql = "SELECT product.id_product, product.NAME, product.image, product.DESCRIPTION, category.id_category, category.name as type, product.is_active, product.price FROM product JOIN category ON category.id_category = product.fk_id_category WHERE product.is_active = true AND category.id_category = @CategoryId AND product.id_product != @ProductId ORDER BY product.id_product DESC LIMIT @Limit";
                }
                Console.WriteLine("Hallo Hallo");


                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Limit", limit);
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                cmd.Parameters.AddWithValue("@ProductId", productId);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = reader.GetInt32("id_product");
                    string name = reader.GetString("name");
                    string image = reader.GetString("image");
                    string description = reader.GetString("description");
                    int idCategory = reader.GetInt32("id_category");
                    string type = reader.GetString("type");
                    bool isActive = reader.GetBoolean("is_active");
                    int price = reader.GetInt32("price");

                    result.Add(new ProductModel
                    {
                        CategoryId = idCategory,
                        Name = name,
                        Image = image,
                        Description = description,
                        CategoryName = type,
                        Id = id,
                        Price = price,
                        IsActive = isActive
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();

            return result;
        }

        public bool UpdateProduct(ProductDTO data, string fileUrlPath, int id)
        {
            MySqlConnection conn = new MySqlConnection(_connStr);
            try
            {
                conn.Open();
                string sql = "UPDATE product SET NAME=@Name, DESCRIPTION=@Description,price=@Price,image=@Image,is_active=@status,fk_id_category = @FKIdCategory WHERE id_product = @Id";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Name", data.TitleProduct);
                cmd.Parameters.AddWithValue("@Description", data.DescriptionProduct);
                cmd.Parameters.AddWithValue("@Price", data.Price);
                cmd.Parameters.AddWithValue("@Image", fileUrlPath);
                cmd.Parameters.AddWithValue("@status", data.IsActive);
                cmd.Parameters.AddWithValue("@FKIdCategory", data.CategoryId);

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
        public bool UpdateStatus(bool status, int id)
        {
            MySqlConnection conn = new MySqlConnection(_connStr);
            try
            {
                conn.Open();
                string sql = "UPDATE product SET is_active=@status WHERE id_product = @Id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@status", status);

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

        public ProductDetailModel GetDetailProduct(int id)
        {
            MySqlConnection conn = new MySqlConnection(_connStr);
            ProductDetailModel result = new ProductDetailModel();
            List<ScheduleModel> schedules = new List<ScheduleModel>();
            try
            {
                conn.Open();
                string sql = "SELECT product.id_product, product.NAME, product.image, product.DESCRIPTION, category.name, category.id_category, schedule.id_schedule, schedule.date_schedule, product.price FROM schedule Right JOIN product ON product.id_product = schedule.fk_id_product JOIN category ON category.id_category = product.fk_id_category WHERE product.id_product = @Id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Id = reader.GetInt32(0);
                    result.Name = reader.GetString(1);
                    result.Image = reader.GetString(2);
                    result.Description = reader.GetString(3);
                    result.Category = reader.GetString(4);
                    result.CategoryId = reader.GetInt32(5);
                    result.Price = reader.GetInt32(8);
                    schedules?.Add(new ScheduleModel
                        {
                            Date = DateTime.Parse(reader.GetString(7)),
                            Id = reader.GetInt32(6),
                            TitleProduct = reader.GetString(1)
                        });
                }
                result.Schedules = schedules ?? new List<ScheduleModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }
            conn.Close();

            return result;
        }
    }
}