using _02_CRUD_Interface;
using System.Data;
using System.Text;


namespace _03_data_access.Models
{
    public class PersonService
    {
        private DbHelper db;

        public PersonService(DbHelper db)
        {
            this.db = db;
        }

        public bool DeletePerson(bool isEmployee, int id)
        {
            using (var conn = db.GetConnection())
            {
                conn.Open();

                using (var checkCmd = conn.CreateCommand())
                {
                    if (isEmployee)
                    {
                        checkCmd.CommandText = "select count(1) from Salles where EmployeeId = @id";
                    }
                    else
                    {
                        checkCmd.CommandText = "select count(1) from Salles where ClientId = @id";
                    }
                    checkCmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    var countObj = checkCmd.ExecuteScalar();
                    int count = 0;
                    if (countObj != null)
                    {
                        count = Convert.ToInt32(countObj);
                    }
                    if (count > 0)
                    {
                        Console.OutputEncoding = Encoding.UTF8;
                        Console.WriteLine("Неможливо видалити: на цю особу посилаються записи в Salles.");
                        return false;
                    }
                }

                using (var delCmd = conn.CreateCommand())
                {
                    if (isEmployee)
                    {
                        delCmd.CommandText = "delete from Employees where Id = @id";
                    }
                    else
                    {
                        delCmd.CommandText = "delete from Clients where Id = @id";
                    }
                    delCmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    var affected = delCmd.ExecuteNonQuery();
                    return affected > 0;
                }
            }
        }
    }
}
