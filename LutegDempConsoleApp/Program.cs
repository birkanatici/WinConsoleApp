using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LutegDempConsoleApp;
using LutegDempConsoleApp.Manager;
using LutegDempConsoleApp.Object;

namespace LutegDempConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("starting...");


            FileManager fileManager = new FileManager();
            DatabaseManager databaseManager = new DatabaseManager();

            // bağlantıyı açıyoruz.
            databaseManager.connectDatabase();

            // belirlediğimiz klasördeki csv formatlı dosyaları alıyoruz.
            String []files = fileManager.getFilesDirectory();

            foreach (String file in files) {

                // dosyanın md5 hash checksum'ını buluyoruz.
                // bu bize yinelenen dosyaları tekrar işlememek için gerekli olacak.
                String fileChecksum = fileManager.getFileMD5Checksum(file);

                // eğer aynı md5 hash checksumdan dosya, veritabanına kaydedilmişse bu dosyayı geç.
                if (databaseManager.isExistMd5Hash(fileChecksum)) {
                    fileManager.deleteFile(file);
                    continue;
                }
                    
                // dosya içeriğini aldık.
                String fileContent = fileManager.readCsvFile(file);

                // dosya içeriğinden, dosyadaki kayıtları parse ederek listaye alıyoruz.
                List<CsvObject> csvRecords = fileManager.parseCsvFile(fileContent);  
                
                foreach(CsvObject rec in csvRecords)           // her kayıt için..
                {
                    if (databaseManager.IsExistRecord(rec))    // eğer kayıt varsa 
                        databaseManager.updateRecord(rec);     // kaydı güncelle
                    else
                        databaseManager.insertRecord(rec);     // yoksa yeni kayıt olarak ekle
                }

                /* dosyayı veritabanına ekledikten sonra md5hash tablosuna
                log atıyoruz ki bir daha bu dosyaya bakmayalım. */
                databaseManager.insertMd5Hash(fileChecksum);

                // dosyayı siliyoruz.
                fileManager.deleteFile(file);
            }



            // bağlantıyı kapatıyoruz.
            databaseManager.disconnectDatabase();

            Console.WriteLine("stoping...");
            Console.ReadLine();


        }
    }
}
