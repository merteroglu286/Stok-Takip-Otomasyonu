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
    public partial class FormSatisListeleme : Form
    {
        public FormSatisListeleme()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=192.168.1.11,1433;Network Library=DBMSSOCN;Initial Catalog=stok_takip;User Id=mert; Password=123");

        DataSet dataSet = new DataSet();

        private void satisListele()
        {
            baglanti.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("select *from satis", baglanti);
            adapter.Fill(dataSet, "satis");
            dataGridView1.DataSource = dataSet.Tables["satis"];
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Visible = false;
            baglanti.Close();

        }

        private void FormSatisListeleme_Load(object sender, EventArgs e)
        {
            satisListele();
        }
    }
}
