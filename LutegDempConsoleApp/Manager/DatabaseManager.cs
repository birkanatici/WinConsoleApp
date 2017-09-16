using LutegDempConsoleApp.Object;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LutegDempConsoleApp.Manager
{
    class DatabaseManager
    {
        private const String NOTE_TABLE = "NOTLAR";
        private const String MD5_HASH_TABLE = "MD5_HASH_LOG";

        private SqlConnection connection;


        // sql server'da Database ile bağlantı açar.
        public void connectDatabase()
        {
            connection = new SqlConnection("Server=DESKTOP-9F14KR5;Database=DB;Trusted_Connection=True;");
            connection.Open();
        }

        public void disconnectDatabase()
        {
            connection.Close();
        }

        // yeni not bilgisini kayıt eder.
        public void insertRecord(CsvObject record)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "INSERT INTO " + NOTE_TABLE + "(ders, ogrenci_no, vize1, vize2, vize3, final)"
                                                          + " VALUES (@ders, @ogrenci_no, @vize1, @vize2, @vize3, @final);";

            cmd.Parameters.AddWithValue("@ders", record.Ders);
            cmd.Parameters.AddWithValue("@ogrenci_no", record.Ogrenci_no);
            cmd.Parameters.AddWithValue("@vize1", record.Vize1);
            cmd.Parameters.AddWithValue("@vize2", record.Vize2);
            cmd.Parameters.AddWithValue("@vize3", record.Vize3);
            cmd.Parameters.AddWithValue("@final", record.Final);

            cmd.ExecuteNonQuery();
        }

        // eski not bilgisini yenisiyle değiştirir.
        public void updateRecord(CsvObject record)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;

            cmd.CommandText = "UPDATE " + NOTE_TABLE + " SET vize1=@vize1, vize2=@vize2, vize3=@vize3, final=@final WHERE ders=@ders AND ogrenci_no=@ogrenci_no";

            cmd.Parameters.AddWithValue("@ders", record.Ders);
            cmd.Parameters.AddWithValue("@ogrenci_no", record.Ogrenci_no);
            cmd.Parameters.AddWithValue("@vize1", record.Vize1);
            cmd.Parameters.AddWithValue("@vize2", record.Vize2);
            cmd.Parameters.AddWithValue("@vize3", record.Vize3);
            cmd.Parameters.AddWithValue("@final", record.Final);

            cmd.ExecuteNonQuery();
        }

        // kayıtın database'de kayıtlı olup olmadığının kontrolünü yapar.
        public bool IsExistRecord(CsvObject record) {
            
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;

            cmd.CommandText = "SELECT COUNT(*) FROM " + NOTE_TABLE + " WHERE ders=@ders AND ogrenci_no=@ogr_no";

            cmd.Parameters.AddWithValue("@ders", record.Ders);
            cmd.Parameters.AddWithValue("@ogr_no", record.Ogrenci_no);

            int queryResult = (int)cmd.ExecuteScalar();
          

            if (queryResult > 0)
                return true;
            else
                return false;

        }

        // md5 hash checksum'ını veritabanına ekler
        public void insertMd5Hash(String md5Hash)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "INSERT INTO " + MD5_HASH_TABLE + "(md5_hash)"
                                                          + " VALUES (@md5_hash);";

            cmd.Parameters.AddWithValue("@md5_hash", md5Hash);

            cmd.ExecuteNonQuery();

        }

        // log'larda md5 kontrolü yapar.
        public bool isExistMd5Hash(String md5Hash)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;

            cmd.CommandText = "SELECT COUNT(*) FROM " + MD5_HASH_TABLE + " WHERE md5_hash=@md5_hash";

            cmd.Parameters.AddWithValue("@md5_hash", md5Hash);

            int queryResult = (int)cmd.ExecuteScalar();

            if (queryResult > 0)
                return true;
            else
                return false;

        }
        
    }
}
