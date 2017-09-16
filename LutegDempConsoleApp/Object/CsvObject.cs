using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LutegDempConsoleApp.Object
{
    class CsvObject
    {

        private String ders;
        private int ogrenci_no;
        private int vize1;
        private int vize2;
        private int vize3;
        private int final;

        public CsvObject()
        {
        }

        public CsvObject(string ders, int ogrenci_no, int vize1, int vize2, int vize3, int final)
        {
            this.Ders = ders;
            this.Ogrenci_no = ogrenci_no;
            this.Vize1 = vize1;
            this.Vize2 = vize2;
            this.Vize3 = vize3;
            this.Final = final;
        }

        public string Ders { get => ders; set => ders = value; }
        public int Ogrenci_no { get => ogrenci_no; set => ogrenci_no = value; }
        public int Vize1 { get => vize1; set => vize1 = value; }
        public int Vize2 { get => vize2; set => vize2 = value; }
        public int Vize3 { get => vize3; set => vize3 = value; }
        public int Final { get => final; set => final = value; }
    }
}
