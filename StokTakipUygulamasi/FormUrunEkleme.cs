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
    public partial class FormUrunEkleme : Form
    {
        public FormUrunEkleme()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=192.168.1.11,1433;Network Library=DBMSSOCN;Initial Catalog=stok_takip;User Id=mert; Password=123");

        bool durum;
        private void barkodKontrol()
        {
            durum = true;
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from urun", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (txtBarkodNo.Text == read["barkodNo"].ToString() || txtBarkodNo.Text == "");
                {
                    durum = false;
                }
            }
            baglanti.Close();
        }
        private void kategoriGetir()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from kategoriBilgileri", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                comboKategori.Items.Add(read["kategori"].ToString());
            }
            baglanti.Close();
        }
        private void FormUrunEkleme_Load(object sender, EventArgs e)
        {
            kategoriGetir();
        }

        private void comboKategori_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboMarka.Items.Clear();
            comboMarka.Text = "";
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from markaBilgileri where kategori='"+comboKategori.SelectedItem+"'",baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                comboMarka.Items.Add(read["marka"].ToString());
            }
            baglanti.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            barkodKontrol();
            
                baglanti.Open();
                SqlCommand komut = new SqlCommand("insert into urun(barkodNo,kategori,marka,urunAdi,miktari,alisFiyati,satisFiyati,tarih) values(@barkodNo,@kategori,@marka,@urunAdi,@miktari,@alisFiyati,@satisFiyati,@tarih)", baglanti);
                komut.Parameters.AddWithValue("@barkodNo", txtBarkodNo.Text);
                komut.Parameters.AddWithValue("@kategori", comboKategori.Text);
                komut.Parameters.AddWithValue("@marka", comboMarka.Text);
                komut.Parameters.AddWithValue("@urunAdi", txtUrunAdi.Text);
                komut.Parameters.AddWithValue("@miktari", int.Parse(txtMiktar.Text));
                komut.Parameters.AddWithValue("@alisFiyati", double.Parse(txtAlisFiyati.Text));
                komut.Parameters.AddWithValue("@satisFiyati", double.Parse(txtSatisFiyati.Text));
                komut.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());

                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Ürün eklendi.");
           
            
            comboMarka.Items.Clear();
            
            foreach(Control item in groupBox1.Controls)
            {
                if(item is TextBox)
                {
                    item.Text = "";
                }
                if(item is ComboBox)
                {
                    item.Text = "";
                }
            }
        }

        private void varOlan_txtBarkodNo_TextChanged(object sender, EventArgs e)
        {

            if(varOlan_txtBarkodNo.Text == "")
            {
                labelMiktar.Text = "";
                foreach(Control item in groupBox2.Controls)
                {
                    if(item is TextBox)
                    {
                        item.Text = "";
                    }
                }
            }

            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from urun where barkodNo like '"+varOlan_txtBarkodNo.Text+"'",baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                varOlan_txtKategori.Text = read["kategori"].ToString();
                varOlan_txtMarka.Text = read["marka"].ToString();
                varOlan_txtUrunAdi.Text = read["urunAdi"].ToString();
                labelMiktar.Text = read["miktari"].ToString();
                varOlan_txtAlisFiyati.Text = read["alisFiyati"].ToString();
                varOlan_txtSatisFiyati.Text = read["satisFiyati"].ToString();
            }
            baglanti.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("update urun set miktari=miktari+'"+int.Parse(varOlan_txtMiktar.Text)+"' where barkodNo='"+varOlan_txtBarkodNo.Text+"'", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();

            foreach (Control item in groupBox2.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }
            MessageBox.Show("Var olan ürüne ekleme yapıldı.");
        }
    }
}
