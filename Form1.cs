using System;
using System.Data;
using System.Windows.Forms;

namespace proje2334
{
    public partial class Form1 : Form
    {
        private int seciliKisiId = -1;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BilgileriYukle();
        }

        private void BilgileriYukle()
        {
            grup grup = new grup();
            DataTable grupDt = grup.GruplariListele();

            if (grupDt == null || grupDt.Rows.Count == 0)
            {
                MessageBox.Show("Gruplar yüklenemedi veya boş. Lütfen Grup İşlemleri kısmından grup oluşturunuz!","Hata", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            DataRow dr = grupDt.NewRow();
            dr["grup_adi"] = "Tümü";
            dr["grup_id"] = 0;
            grupDt.Rows.Add(dr);

            comboBox1.DataSource = grupDt;
            comboBox1.ValueMember = "grup_id";
            comboBox1.DisplayMember = "grup_adi";

            comboBox2.DataSource = grupDt;
            comboBox2.ValueMember = "grup_id";
            comboBox2.DisplayMember = "grup_adi";
            comboBox2.SelectedValue = 0;

            Kisi kisi = new Kisi();
            dataGridView1.DataSource = kisi.KisileriListele();
            AyarlaDataGridView();
        }

        private void AyarlaDataGridView()
        {
            dataGridView1.Columns["kisi_id"].HeaderText = "";
            dataGridView1.Columns["kisi_id"].Width = 0;
            dataGridView1.Columns["ad"].HeaderText = "Kişi Adı";
            dataGridView1.Columns["ad"].Width = 120;
            dataGridView1.Columns["soyad"].HeaderText = "Kişi Soyadı";
            dataGridView1.Columns["soyad"].Width = 90;
            dataGridView1.Columns["tel_no1"].HeaderText = "Telefon No 1";
            dataGridView1.Columns["tel_no1"].Width = 110;
            dataGridView1.Columns["tel_no2"].HeaderText = "Telefon No 2";
            dataGridView1.Columns["tel_no2"].Width = 110;
            dataGridView1.Columns["mail"].HeaderText = "Mail";
            dataGridView1.Columns["mail"].Width = 170;
            dataGridView1.Columns["unvan"].HeaderText = "Unvan";
            dataGridView1.Columns["unvan"].Width = 100;
            dataGridView1.Columns["grup_adi"].HeaderText = "Kişi Grubu";
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (KayitKontrol())
            {
                Kisi kisi = new Kisi
                {
                    KisiAdi = textBox1.Text,
                    KisiSoyadi = textBox3.Text,
                    TelNo1 = textBox2.Text,
                    TelNo2 = textBox4.Text,
                    Mail = textBox6.Text,
                    Unvan = textBox7.Text,
                    GrupId = Int32.Parse(comboBox1.SelectedValue.ToString())
                };
                kisi.KisiEkle();
                dataGridView1.DataSource = kisi.KisileriListele();
                FormuTemizle();
            }
        }

        private void FormuTemizle()
        {
            textBox1.Clear();
            textBox3.Clear();
            textBox2.Clear();
            textBox4.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
            dataGridView1.ClearSelection();
            seciliKisiId = -1;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Form2 frmGrup = new Form2();
            DialogResult result = frmGrup.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                BilgileriYukle();
            }
        }

        private bool KayitKontrol()
        {
            if (textBox1.TextLength < 2)
            {
                MessageBox.Show("Kişinin adını giriniz.");
                return false;
            }
            if (textBox3.TextLength < 2)
            {
                MessageBox.Show("Kişinin soyadını giriniz.");
                return false;
            }
            if (textBox2.TextLength < 10 || textBox2.TextLength > 14)
            {
                MessageBox.Show("Kişinin numarasını doğru giriniz. (10-14 karakter olmalıdır)");
                return false;
            }
            if (comboBox1.SelectedValue == null || (int)comboBox1.SelectedValue <= 0)
            {
                MessageBox.Show("Kişinin grubunu seçiniz.");
                return false;
            }
            return true;
        }

        private void TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void TextBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void TextBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            FormuTemizle();
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows != null)
                {
                    seciliKisiId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["kisi_id"].Value.ToString());
                    textBox1.Text = dataGridView1.CurrentRow.Cells["ad"].Value.ToString();
                    textBox3.Text = dataGridView1.CurrentRow.Cells["soyad"].Value.ToString();
                    textBox2.Text = dataGridView1.CurrentRow.Cells["tel_no1"].Value.ToString();
                    textBox4.Text = dataGridView1.CurrentRow.Cells["tel_no2"].Value.ToString();
                    textBox6.Text = dataGridView1.CurrentRow.Cells["mail"].Value.ToString();
                    textBox7.Text = dataGridView1.CurrentRow.Cells["grup_adi"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata Oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (seciliKisiId != -1)
            {
                Kisi kisi = new Kisi { KisiId = seciliKisiId };
                kisi.KisiSil();
                dataGridView1.DataSource = kisi.KisileriListele();
                FormuTemizle();
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            if (KayitKontrol())
            {
                Kisi kisi = new Kisi
                {
                    KisiId = seciliKisiId,
                    KisiAdi = textBox1.Text,
                    KisiSoyadi = textBox3.Text,
                    TelNo1 = textBox2.Text,
                    TelNo2 = textBox4.Text,
                    Mail = textBox6.Text,
                    Unvan = textBox7.Text,
                    GrupId = Int32.Parse(comboBox1.SelectedValue.ToString())
                };
                kisi.KisiyiGuncelle();
                dataGridView1.DataSource = kisi.KisileriListele();
                FormuTemizle();
            }
        }

        private void KisiAra()
        {
            Kisi kisi = new Kisi
            {
                KisiAdi = textBox8.Text,
                KisiSoyadi = textBox9.Text,
                TelNo1 = textBox10.Text,
                GrupId = Int32.Parse(comboBox2.SelectedValue.ToString())
            };
            dataGridView1.DataSource = kisi.KisiAra();
        }

        private void TextBox8_TextChanged(object sender, EventArgs e)
        {
            KisiAra();
        }

        private void TextBox9_TextChanged(object sender, EventArgs e)
        {
            KisiAra();
        }

        private void TextBox10_TextChanged(object sender, EventArgs e)
        {
            KisiAra();
        }

        private void ComboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            KisiAra();
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Uygulamayı kapatmak istediğinize emin misiniz?", "Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (DialogResult.Yes==result)
            {
                Application.Exit();
            }
        }
    }
}
