
using final_project_be.DTO;
using final_project_be.Models;
using MySql.Data.MySqlClient;

namespace final_project_be.Repositories
{
    public class AuthRepository
    {
        private string connStr = string.Empty;

        //Dependencies Injection

        public AuthRepository(IConfiguration configuration)
        {
            connStr = configuration.GetConnectionString("Default");
        }

        public UserModel? GetByEmailAndPassword(string email, string password, bool isRegister)
        {
            UserModel? user = null;

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                string sql = "";
                if (isRegister)
                {
                    sql = "SELECT * FROM auth WHERE email=@Email";
                }
                else
                {
                    sql = "SELECT * FROM auth WHERE email=@Email and password=@Password AND is_active = true";
                }
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    user = new UserModel
                    {
                        Id = reader.GetInt32("id_user"),
                        Name = reader.GetString("name"),
                        Email = reader.GetString("email"),
                        Role = reader.GetString("role"),
                        IsActive = reader.GetBoolean("is_active")
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //required
            conn.Close();
            Console.WriteLine("Done.");

            return user;
        }

        public bool CreateRegister(RegisterDTO data, string password)
        {
            //get connection to database
            MySqlConnection conn = new MySqlConnection(connStr);
            bool result = true;
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO auth (id_user, name, email, password, role, is_active) VALUES (DEFAULT, @Name, @Email, @Password, @Role, @IsActive)", conn);

                cmd.Parameters.AddWithValue("@Name", data.Name);
                cmd.Parameters.AddWithValue("@Email", data.Email);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Role", data.Role);
                cmd.Parameters.AddWithValue("@IsActive", data.IsActive);

                cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                result = false;

            }
            conn.Close();
            Console.WriteLine("Done.");

            return result;
        }

        public UserModel? GetByEmail(string email)
        {
            UserModel? user = null;

            //get connection to database
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM auth WHERE email=@Email", conn);

                cmd.Parameters.AddWithValue("@Email", email);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    user = new UserModel
                    {
                        Id = reader.GetInt32("id_user"),
                        Name = reader.GetString("name"),
                        Email = reader.GetString("email"),
                        Role = reader.GetString("role")
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //required
            conn.Close();

            return user;
        }

        public bool InsertResetPasswordToken(int userId, string token)
        {
            bool isSuccess = false;

            //get connection to database
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE auth SET token_reset_pw=@Token WHERE id_user=@Id", conn);

                cmd.Parameters.AddWithValue("@Id", userId);
                cmd.Parameters.AddWithValue("@Token", token);

                cmd.ExecuteNonQuery();

                isSuccess = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //required
            conn.Close();

            return isSuccess;
        }

        public UserModel? GetByEmailAndResetToken(string email, string resetToken)
        {
            UserModel? user = null;

            //get connection to database
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM auth WHERE email=@Email AND token_reset_pw=@ResetToken", conn);

                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@ResetToken", resetToken);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    user = new UserModel
                    {
                        Id = reader.GetInt32("id_user"),
                        Name = reader.GetString("name"),
                        Email = reader.GetString("email"),
                        Role = reader.GetString("role")
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //required
            conn.Close();

            return user;
        }

        public bool UpdatePassword(int userId, string NewPassword)
        {
            bool isSuccess = false;

            //get connection to database
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE auth SET password=@NewPassword WHERE id_user=@Id", conn);

                cmd.Parameters.AddWithValue("@Id", userId);
                cmd.Parameters.AddWithValue("@NewPassword", NewPassword);

                cmd.ExecuteNonQuery();

                isSuccess = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //required
            conn.Close();

            return isSuccess;
        }

        public bool ActiveUser(string email)
        {
            bool isSuccess = false;

            //get connection to database
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE auth SET is_active=true WHERE email=@Email", conn);

                cmd.Parameters.AddWithValue("@Email", email);

                cmd.ExecuteNonQuery();

                isSuccess = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //required
            conn.Close();

            return isSuccess;

        }

        public bool ChangeStatus(bool status, int id)
        {
            bool isSuccess = false;

            //get connection to database
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE auth SET is_active=@Status WHERE id_user=@Id", conn);

                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();

                isSuccess = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //required
            conn.Close();

            return isSuccess;
        }

        public List<UserModel> getDataUser()
        {
            List<UserModel> user = new List<UserModel>();

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                string sql = "SELECT * FROM auth";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    user.Add(new UserModel
                    {
                        Id = reader.GetInt32("id_user"),
                        Name = reader.GetString("name"),
                        Email = reader.GetString("email"),
                        Role = reader.GetString("role"),
                        IsActive = reader.GetBoolean("is_active")
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //required
            conn.Close();
            Console.WriteLine("Done.");

            return user;
        }

        public UserModel GetDataUserById(int id)
        {
            UserModel user = new UserModel();

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                string sql = "SELECT * FROM auth WHERE id_user=@Id";


                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Id", id);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    user.Id = reader.GetInt32("id_user");
                    user.Name = reader.GetString("name");
                    user.Email = reader.GetString("email");
                    user.Role = reader.GetString("role");
                    user.IsActive = reader.GetBoolean("is_active");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //required
            conn.Close();
            Console.WriteLine("Done.");

            return user;
        }

        public bool UpdateUser(RegisterDTO data, int id)
        {
            bool isSuccess = false;

            //get connection to database
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE auth SET name=@Name, email=@Email, role=@Role, is_active =@IsActive WHERE id_user=@Id", conn);

                cmd.Parameters.AddWithValue("@Name", data.Name);
                cmd.Parameters.AddWithValue("@Email", data.Email);
                cmd.Parameters.AddWithValue("@Role", data.Role);
                cmd.Parameters.AddWithValue("@IsActive", data.IsActive);
                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();

                isSuccess = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //required
            conn.Close();

            return isSuccess;
        }
    }
}