using final_project_be.DTO;
using final_project_be.Models;
using MySql.Data.MySqlClient;

namespace final_project_be.Repositories
{
    public class InvoiceRepository
    {
        private readonly string _connStr;
        public InvoiceRepository(IConfiguration configuration)
        {
            _connStr = configuration.GetConnectionString("Default");
        }

        public bool InsertInvoice(InvoiceDTO data, int idUser)
        {
            MySqlConnection conn = new MySqlConnection(_connStr);
            bool isSuccess = true;
            conn.Open();

            MySqlTransaction transaction = conn.BeginTransaction();
            try
            {
                List<InvoiceFromCart> invoiceFromCarts = data.ListData!;

                string sqlInsert = "INSERT INTO invoice VALUES (DEFAULT, @NoInvoice, @IdScedule , @IdPayment , @IdUser , @CreateAt)";
                string sqlDellete = "DELETE FROM cart WHERE cart.id_cart = @IdCart";

                foreach (InvoiceFromCart invoiceFromCart in invoiceFromCarts)
                {
                    using (var cmd = new MySqlCommand(sqlInsert, conn))
                    {
                        cmd.Parameters.AddWithValue("@NoInvoice", data.NoInvoice);
                        cmd.Parameters.AddWithValue("@IdScedule", invoiceFromCart.IdSchedule);
                        cmd.Parameters.AddWithValue("@IdPayment", data.PaymentId);
                        cmd.Parameters.AddWithValue("@IdUser", idUser);
                        cmd.Parameters.AddWithValue("@CreateAt", data.CreateAt);

                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();

                    }
                }
                foreach (InvoiceFromCart invoiceFromCart in invoiceFromCarts)
                {
                    using (var cmd = new MySqlCommand(sqlDellete, conn))
                    {
                        cmd.Parameters.AddWithValue("@IdCart", invoiceFromCart.IdChart);
                        cmd.Transaction = transaction;

                        cmd.ExecuteNonQuery();
                    }
                }

                transaction.Commit();

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine(ex.ToString());
                isSuccess = false;
            }
            conn.Close();

            return isSuccess;
        }

        public List<InvoiceModel> GetInvoiceById(int id)
        {
            MySqlConnection conn = new MySqlConnection(_connStr);
            List<InvoiceModel> result = new List<InvoiceModel>();

            try
            {
                conn.Open();
                string sql = "SELECT invoice.number_invoice, invoice.create_at, COUNT(invoice.number_invoice) AS total_product, SUM(product.price) AS totalPrice FROM invoice join schedule ON invoice.fk_id_schedule = schedule.id_schedule JOIN product ON product.id_product = schedule.fk_id_product WHERE invoice.fk_id_user = @IdUser GROUP BY invoice.number_invoice;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@IdUser", id);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new InvoiceModel
                    {
                        InvoiceNumber = reader.GetString(0),
                        CreatedAt = DateTime.Parse(reader.GetString(1)),
                        TotalProduct = reader.GetInt32(2),
                        TotalPrice = reader.GetInt32(3)
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

        public DetailInvoiceModel GetDetailInvoice(string InvoiceNumber)
        {
            MySqlConnection conn = new MySqlConnection(_connStr);
            DetailInvoiceModel result = new DetailInvoiceModel();
            List<InvoiceListModel> temp = new List<InvoiceListModel>();
            try
            {
                conn.Open();
                string sql = "SELECT invoice.number_invoice, invoice.create_at, product.NAME, category.name, schedule.date_schedule, product.price FROM invoice JOIN schedule ON invoice.fk_id_schedule = schedule.id_schedule JOIN product ON product.id_product = schedule.fk_id_product JOIN category ON category.id_category = product.fk_id_category WHERE invoice.number_invoice = @Number";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Number", InvoiceNumber);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.NoInvoice = reader.GetString(0);
                    result.CreateAt = DateTime.Parse(reader.GetString(1));

                    temp.Add(new InvoiceListModel
                    {
                        Name = reader.GetString(2),
                        Category = reader.GetString(3),
                        Shedule = DateTime.Parse(reader.GetString(4)),
                        Price = reader.GetInt32(5)
                    });
                }
                result.ListInvoice = temp;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Menghadeh");
                Console.WriteLine(ex.ToString());
            }
            conn.Close();

            return result;
        }

        public List<InvoiceListModel> GetAdminData()
        {
            MySqlConnection conn = new MySqlConnection(_connStr);
            List<InvoiceListModel> result = new List<InvoiceListModel>();

            try
            {
                conn.Open();
                string sql = "SELECT invoice.number_invoice, product.NAME, schedule.date_schedule, product.price FROM invoice JOIN schedule ON invoice.fk_id_schedule = schedule.id_schedule JOIN product ON product.id_product = schedule.fk_id_product JOIN category ON category.id_category = product.fk_id_category";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new InvoiceListModel
                    {
                        Name = reader.GetString(1),
                        Category = reader.GetString(0),
                        Shedule = DateTime.Parse(reader.GetString(2)),
                        Price = reader.GetInt32(3)
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

        public List<InvoiceModel> GetInvoiceAdmin()
        {
            MySqlConnection conn = new MySqlConnection(_connStr);
            List<InvoiceModel> result = new List<InvoiceModel>();

            try
            {
                conn.Open();
                string sql = "SELECT invoice.number_invoice, invoice.create_at, COUNT(invoice.number_invoice) AS total_product, SUM(product.price) AS totalPrice FROM invoice join schedule ON invoice.fk_id_schedule = schedule.id_schedule JOIN product ON product.id_product = schedule.fk_id_product GROUP BY invoice.number_invoice;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new InvoiceModel
                    {
                        InvoiceNumber = reader.GetString(0),
                        CreatedAt = DateTime.Parse(reader.GetString(1)),
                        TotalProduct = reader.GetInt32(2),
                        TotalPrice = reader.GetInt32(3)
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