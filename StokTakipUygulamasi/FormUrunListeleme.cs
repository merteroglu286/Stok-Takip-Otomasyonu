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
    public partial class FormUrunListeleme : Form
    {

        public FormUrunListeleme()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=192.168.1.11,1433;Network Library=DBMSSOCN;Initial Catalog=stok_takip;User Id=mert; Password=123");
        DataSet dataSet = new DataSet();

        private void urunListele()
        {
            baglanti.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("select *from urun", baglanti);
            adapter.Fill(dataSet, "urun");
            dataGridView1.DataSource = dataSet.Tables["urun"];
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
        private void FormUrunListeleme_Load(object sender, EventArgs e)
        {
            urunListele();
            kategoriGetir();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            varOlan_txtBarkodNo.Text = dataGridView1.CurrentRow.Cells["barkodNo"].Value.ToString();
            varOlan_txtKategori.Text = dataGridView1.CurrentRow.Cells["kategori"].Value.ToString();
            varOlan_txtMarka.Text = dataGridView1.CurrentRow.Cells["marka"].Value.ToString();
            varOlan_txtUrunAdi.Text = dataGridView1.CurrentRow.Cells["urunAdi"].Value.ToString();
            varOlan_txtMiktar.Text = dataGridView1.CurrentRow.Cells["miktari"].Value.ToString();
            varOlan_txtAlisFiyati.Text = dataGridView1.CurrentRow.Cells["alisFiyati"].Value.ToString();
            varOlan_txtSatisFiyati.Text = dataGridView1.CurrentRow.Cells["satisFiyati"].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("update urun set urunAdi = @urunAdi,miktari=@miktari,alisFiyati=@alisFiyati,satisFiyati=@satisFiyati where barkodNo=@barkodNo", baglanti);
            komut.Parameters.AddWithValue("@barkodNo", varOlan_txtBarkodNo.Text);
            komut.Parameters.AddWithValue("@urunAdi", varOlan_txtUrunAdi.Text);
            komut.Parameters.AddWithValue("@miktari",int.Parse(varOlan_txtMiktar.Text));
            komut.Parameters.AddWithValue("@alisFiyati",double.Parse(varOlan_txtAlisFiyati.Text));
            komut.Parameters.AddWithValue("@satisfiyati", double.Parse(varOlan_txtSatisFiyati.Text));
            komut.ExecuteNonQuery();
            baglanti.Close();
            dataSet.Tables["urun"].Clear();
            urunListele();

            MessageBox.Show("Güncelleme başarıyla yapıldı.");
            
            foreach(Control item in this.Controls)
            {
                if(item is TextBox)
                {
                    item.Text = "";
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(varOlan_txtBarkodNo.Text != "")
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("update urun set kategori = @kategori, marka=@marka where barkodNo=@barkodNo", baglanti);
                komut.Parameters.AddWithValue("@barkodNo", varOlan_txtBarkodNo.Text);
                komut.Parameters.AddWithValue("@kategori", comboKategori.Text);
                komut.Parameters.AddWithValue("@marka", comboMarka.Text);
                komut.ExecuteNonQuery();
                baglanti.Close();
                dataSet.Tables["urun"].Clear();
                urunListele();

                MessageBox.Show("Güncelleme başarıyla yapıldı.");
            }
            else
            {
                MessageBox.Show("Barkod numarası yazılı değil.");
            }

            foreach (Control item in this.Controls)
            {
                if (item is ComboBox)
                {
                    item.Text = "";
                }
            }
        }

        private void comboKategori_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboMarka.Items.Clear();
            comboMarka.Text = "";
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from markaBilgileri where kategori='" + comboKategori.SelectedItem + "'", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                comboMarka.Items.Add(read["marka"].ToString());
            }
            baglanti.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("delete from urun where barkodNo='" + dataGridView1.CurrentRow.Cells["barkodNo"].Value.ToString() + "'", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            dataSet.Tables["urun"].Clear();
            urunListele();
            MessageBox.Show("Ürün kaydı Silindi.");
        }

        private void txtBarkodNoAra_TextChanged(object sender, EventArgs e)
        {
            DataTable tablo = new DataTable();
            baglanti.Open();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("select *from urun where barkodNo like '%" + txtBarkodNoAra.Text + "%'", baglanti);
            sqlDataAdapter.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }
    }
}
