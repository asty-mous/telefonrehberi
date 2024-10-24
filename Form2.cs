using System;
using System.Data;
using System.Windows.Forms;

namespace proje2334
{
    public partial class Form2 : Form
    {
        private int seciliGrupId = -1;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            grup grup = new grup();
            dataGridView1.DataSource = grup.GruplariListele();
            dataGridView1.Columns["grup_id"].HeaderText = "ID";
            dataGridView1.Columns["grup_id"].Width = 30;
            dataGridView1.Columns["grup_adi"].HeaderText = "Grup Adı";
            dataGridView1.Columns["grup_adi"].Width = 100;
            dataGridView1.Columns["aciklama"].HeaderText = "Açıklama";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            grup grup = new grup();
            if (textBox1.TextLength < 2)
            {
                MessageBox.Show("Grup adını giriniz.");
                return;
            }
            grup.GrupAdi = textBox1.Text;
            grup.Aciklama = textBox2.Text;
            grup.GrupEkle();
            dataGridView1.DataSource = grup.GruplariListele();
            FormuTemizle();
        }

        private void FormuTemizle()
        {
            textBox1.Clear();
            textBox2.Clear();
            dataGridView1.ClearSelection();
            seciliGrupId = -1;
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow != null)
                {
                    seciliGrupId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["grup_id"].Value);
                    textBox1.Text = dataGridView1.CurrentRow.Cells["grup_adi"].Value.ToString();
                    textBox2.Text = dataGridView1.CurrentRow.Cells["aciklama"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata Oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (seciliGrupId != -1)
            {
                grup grup = new grup { GrupId = seciliGrupId };
                grup.GrupSil();
                dataGridView1.DataSource = grup.GruplariListele();
                FormuTemizle();
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (seciliGrupId != -1)
            {
                if (textBox1.TextLength < 2)
                {
                    MessageBox.Show("Grup adını giriniz.");
                    return;
                }
                grup grup = new grup
                {
                    GrupId = seciliGrupId,
                    GrupAdi = textBox1.Text,
                    Aciklama = textBox2.Text
                };
                grup.GrubuGuncelle();
                dataGridView1.DataSource = grup.GruplariListele();
                FormuTemizle();
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            FormuTemizle();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Button5_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Geri gitmek istediğinize emin misiniz?", "Geri Git", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (DialogResult.Yes==result)
            {
                this.Visible = false;
            }
        }
    }
}
