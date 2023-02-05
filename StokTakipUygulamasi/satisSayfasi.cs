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
    public partial class satisSayfasi : Form
    {
        public satisSayfasi()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=192.168.1.11,1433;Network Library=DBMSSOCN;Initial Catalog=stok_takip;User Id=mert; Password=123");
        DataSet dataSet = new DataSet();

        private void sepetListele()
        {
            baglanti.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("select *from sepet", baglanti);
            adapter.Fill(dataSet, "sepet");
            dataGridView1.DataSource = dataSet.Tables["sepet"];
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Visible = false;
            baglanti.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormMusteriEkle ekle = new FormMusteriEkle();
            ekle.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormMusteriListeleme formMusteriListeleme = new FormMusteriListeleme();
            formMusteriListeleme.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormUrunEkleme formUrunEkleme = new FormUrunEkleme();
            formUrunEkleme.ShowDialog();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            FormKategoriSayfasi formKategoriSayfasi = new FormKategoriSayfasi();
            formKategoriSayfasi.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FormMarkaSayfasi formMarkaSayfasi = new FormMarkaSayfasi();
            formMarkaSayfasi.ShowDialog();
        }

        private void hesapla()
        {
            try
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("select sum(toplamFiyat) from sepet", baglanti);
                labelGenelToplam.Text = komut.ExecuteScalar() + " TL";
                baglanti.Close();
            }
            catch (Exception)
            {
                ;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FormUrunListeleme formUrunListeleme = new FormUrunListeleme();
            formUrunListeleme.ShowDialog();
        }

        private void satisSayfasi_Load(object sender, EventArgs e)
        {
            sepetListele();
        }

        private void txtTc_TextChanged(object sender, EventArgs e)
        {
            if(txtTc.Text == "")
            {
                txtAdSoyad.Text = "";
                txtTelefon.Text = "";
            }
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from müsteri where tc like '"+txtTc.Text+"'",baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                txtAdSoyad.Text = read["adSoyad"].ToString();
                txtTelefon.Text = read["telefon"].ToString();
            }
            baglanti.Close();
        }

        private void temizle()
        {
            if (txtBarkodNo.Text == "")
            {
                foreach (Control item in groupBox2.Controls)
                {
                    if (item is TextBox)
                    {
                        if (item != txtMiktar)
                        {
                            item.Text = "";
                        }
                    }
                }

            }
        }
        private void txtBarkodNo_TextChanged(object sender, EventArgs e)
        {
            temizle();
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from urun where barkodNo like '" + txtBarkodNo.Text + "'", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                txtUrunAdi.Text = read["urunAdi"].ToString();
                txtSatisFiyati.Text = read["satisFiyati"].ToString();
            }
            baglanti.Close();
        }

        bool durum;
        private void barkodNoKontrol()
        {
            durum = true;
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from sepet",baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if(txtBarkodNo.Text == read["barkodNo"].ToString())
                {
                    durum = false;
                }
            }
            baglanti.Close();
        }
        private void btnEkle_Click(object sender, EventArgs e)
        {
            barkodNoKontrol();
            if(durum == true)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("insert into sepet(tc,adSoyad,telefon,barkodNo,urunAdi,miktari,satisFiyati,toplamFiyat,tarih) values(@tc,@adSoyad,@telefon,@barkodNo,@urunAdi,@miktari,@satisFiyati,@toplamFiyat,@tarih)", baglanti);
                komut.Parameters.AddWithValue("@tc", txtTc.Text);
                komut.Parameters.AddWithValue("@adSoyad", txtAdSoyad.Text);
                komut.Parameters.AddWithValue("@telefon", txtTelefon.Text);
                komut.Parameters.AddWithValue("@barkodNo", txtBarkodNo.Text);
                komut.Parameters.AddWithValue("@urunAdi", txtUrunAdi.Text);
                komut.Parameters.AddWithValue("@miktari", int.Parse(txtMiktar.Text));
                komut.Parameters.AddWithValue("@satisFiyati", double.Parse(txtSatisFiyati.Text));
                komut.Parameters.AddWithValue("@toplamFiyat", double.Parse(txtToplamFiyat.Text));
                komut.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());
                komut.ExecuteNonQuery();
                baglanti.Close();
            }
            else
            {
                baglanti.Open();
                SqlCommand komut2 = new SqlCommand("update sepet set miktari = miktari+ '"+int.Parse(txtMiktar.Text)+ "' where barkodNo='" + txtBarkodNo.Text + "'", baglanti);
                komut2.ExecuteNonQuery();
                SqlCommand komut3 = new SqlCommand("update sepet set toplamFiyat = miktari*satisFiyati where barkodNo='"+txtBarkodNo.Text+"'", baglanti);
                komut3.ExecuteNonQuery();
                baglanti.Close();
            }
            txtMiktar.Text = "1";
            dataSet.Tables["sepet"].Clear();
            sepetListele();
            hesapla();

            foreach (Control item in groupBox2.Controls)
            {
                if (item is TextBox)
                {
                    if (item != txtMiktar)
                    {
                        item.Text = "";
                    }
                }
            }
        }

        private void txtMiktar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtToplamFiyat.Text = (double.Parse(txtMiktar.Text) * double.Parse(txtSatisFiyati.Text)).ToString();
            }
            catch(Exception)
            {
                ;
            }
        }

        private void txtSatisFiyati_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtToplamFiyat.Text = (double.Parse(txtMiktar.Text) * double.Parse(txtSatisFiyati.Text)).ToString();
            }
            catch (Exception)
            {
                ;
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("delete from sepet where barkodNo= '"+dataGridView1.CurrentRow.Cells["barkodNo"].Value.ToString()+"'", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Ürün sepetten çıkarıldı.");
            dataSet.Tables["sepet"].Clear();
            sepetListele();
            hesapla();
        }

        private void btnSatisIptal_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("delete from sepet", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Ürünler sepetten çıkarıldı.");
            dataSet.Tables["sepet"].Clear();
            sepetListele();
            hesapla();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FormSatisListeleme formSatisListeleme = new FormSatisListeleme();
            formSatisListeleme.ShowDialog();
        }

        private void btnSatisYap_Click(object sender, EventArgs e)
        {
            for(int i=0; i<dataGridView1.Rows.Count-1 ; i++)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("insert into satis(tc,adSoyad,telefon,barkodNo,urunAdi,miktari,satisFiyati,toplamFiyat,tarih) values(@tc,@adSoyad,@telefon,@barkodNo,@urunAdi,@miktari,@satisFiyati,@toplamFiyat,@tarih)", baglanti);
                komut.Parameters.AddWithValue("@tc", txtTc.Text);
                komut.Parameters.AddWithValue("@adSoyad", txtAdSoyad.Text);
                komut.Parameters.AddWithValue("@telefon", txtTelefon.Text);
                komut.Parameters.AddWithValue("@barkodNo", dataGridView1.Rows[i].Cells["barkodNo"].Value.ToString());
                komut.Parameters.AddWithValue("@urunAdi", dataGridView1.Rows[i].Cells["urunAdi"].Value.ToString());
                komut.Parameters.AddWithValue("@miktari", int.Parse(dataGridView1.Rows[i].Cells["miktari"].Value.ToString()));
                komut.Parameters.AddWithValue("@satisFiyati", double.Parse(dataGridView1.Rows[i].Cells["satisFiyati"].Value.ToString()));
                komut.Parameters.AddWithValue("@toplamFiyat", double.Parse(dataGridView1.Rows[i].Cells["toplamFiyat"].Value.ToString()));
                komut.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());
                komut.ExecuteNonQuery();
                SqlCommand komut2 = new SqlCommand("update urun set miktari=miktari-'" + int.Parse(dataGridView1.Rows[i].Cells["miktari"].Value.ToString()) + "' where barkodNo='" + dataGridView1.Rows[i].Cells["barkodNo"].Value.ToString() + "'", baglanti);
                komut2.ExecuteNonQuery();
                baglanti.Close();
            }
            baglanti.Open();
            SqlCommand komut3 = new SqlCommand("delete from sepet", baglanti);
            komut3.ExecuteNonQuery();
            baglanti.Close();
            dataSet.Tables["sepet"].Clear();
            sepetListele();
            hesapla();
        }
    }
}
