using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace StokTakipUygulamasi
{
    public partial class FormKategoriSayfasi : Form
    {
        public FormKategoriSayfasi()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=192.168.1.11,1433;Network Library=DBMSSOCN;Initial Catalog=stok_takip;User Id=mert; Password=123");

        private void FormKategoriSayfasi_Load(object sender, EventArgs e)
        {

        }

        bool durum;
        private void kategoriKontrol()
        {
            durum = true;
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from kategoriBilgileri", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (textBox1.Text == read["kategori"].ToString() || textBox1.Text == "")
                {
                    durum = false;
                }
            }
            baglanti.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            kategoriKontrol();
            if(durum == true)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("insert into kategoriBilgileri(kategori) values('" + textBox1.Text + "')", baglanti);
                komut.ExecuteNonQuery();
                baglanti.Close();     
                MessageBox.Show("Kategori eklendi.");
            }
            else
            {
                MessageBox.Show("Böyle bir kategori var", "Uyarı");
            }
            textBox1.Text = "";

        }
    }
}
