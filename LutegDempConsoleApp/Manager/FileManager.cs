using LutegDempConsoleApp.Object;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LutegDempConsoleApp.Manager
{
    class FileManager
    {
        private const String DIRECTORY_LOCATION = @"C:\Notlar\";

        // Klasördeki dosyaları bulur.
        public string[] getFilesDirectory()
        {
            string[] filePaths = null;

            try
            {   
                // sadece .csv uzantılı dosyaları alıyoruz.
                filePaths = Directory.GetFiles(DIRECTORY_LOCATION, "*.csv",
                                             SearchOption.TopDirectoryOnly);
            }
            catch (Exception ex)
            {
                // klasör bulunamadı.
                Console.Write(ex.Message);
            }

            return filePaths;
        }

        // filePath'deki dosya içerini geri dönzerir.
        public String readCsvFile(String filePath)
        {
            String result = "";
            
            try
            {
               result = File.ReadAllText(filePath);
            }
            catch(Exception ex)
            {
                // dosya bulunamadı.
                Console.Write(ex.Message);
            }
            
            return result;
        }

        // dosya içeriğinden kayıtları parse eder.
        public List<CsvObject> parseCsvFile(String csvFileContent)
        {
            List<CsvObject> records = new List<CsvObject>();

            // dosyayı satırlara ayırıyoruz
            foreach(String line in csvFileContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)){

                string[] rows = line.Split(',');

                if (rows.Count() != 6) continue;   // eğer sütün sayısı doğru değilse o satırı geçer.

                CsvObject record = new CsvObject();

                try
                {
                    record.Ders = rows[0];
                    record.Ogrenci_no = int.Parse(rows[1]);
                    record.Vize1 = int.Parse(rows[2]);
                    record.Vize2 = int.Parse(rows[3]);
                    record.Vize3 = int.Parse(rows[4]);
                    record.Final = int.Parse(rows[5]);
                }
                catch
                {
                    // satır yanlış kayıt içeriyor. Diğer satıra geç.
                    continue;
                }

                // eğer her şey yolunda ise listeye ekle.
                records.Add(record);
                
            }

            return records;
        }

        public void deleteFile(String filePath)
        {
            File.Delete(filePath);
        }

        // md5 hash checksum bulur.
        public string getFileMD5Checksum(string fileName)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(fileName))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                }
            }
        }

    }
}
