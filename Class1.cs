using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace proje2334
{
    internal class VeriTabaniBaglantisi
    {
        string baglantiCumlesi = ConfigurationManager.ConnectionStrings["TelefonRehberiBaglantiCumlesi"].ConnectionString;
        public SqlConnection baglan()
        {
            SqlConnection baglanti = new SqlConnection(baglantiCumlesi);
            SqlConnection.ClearPool(baglanti);
            return baglanti;
        }
    }

    class Kisi
    {
        public int KisiId { get; set; }
        public string KisiAdi { get; set; }
        public string KisiSoyadi { get; set; }
        public string TelNo1 { get; set; }
        public string TelNo2 { get; set; }
        public string Mail { get; set; }
        public string Unvan { get; set; }
        public int GrupId { get; set; }

        VeriTabaniBaglantisi veriTabaniBaglantisi;
        SqlCommand komut;

        public Kisi()
        {
            veriTabaniBaglantisi = new VeriTabaniBaglantisi();
            komut = new SqlCommand();
        }

        public void KisiEkle()
        {
            using (var baglanti = veriTabaniBaglantisi.baglan())
            {
                try
                {
                    baglanti.Open();
                    komut.Connection = baglanti;
                    komut.CommandText = "INSERT INTO kisiler(ad,soyad,tel_no1,tel_no2,mail,unvan,grup_id) VALUES(@ad,@soyad,@telNo1,@telNo2,@mail,@unvan,@grupId)";
                    komut.Parameters.AddWithValue("@ad", KisiAdi);
                    komut.Parameters.AddWithValue("@soyad", KisiSoyadi);
                    komut.Parameters.AddWithValue("@telNo1", TelNo1);
                    komut.Parameters.AddWithValue("@telNo2", TelNo2);
                    komut.Parameters.AddWithValue("@mail", Mail);
                    komut.Parameters.AddWithValue("@unvan", Unvan);
                    komut.Parameters.AddWithValue("@grupId", GrupId);
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Kayıt işlemi başarılı");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Kayıt işleminde hata oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void KisiyiGuncelle()
        {
            using (var baglanti = veriTabaniBaglantisi.baglan())
            {
                try
                {
                    baglanti.Open();
                    komut.Connection = baglanti;
                    komut.CommandText = "UPDATE kisiler SET ad=@ad, soyad=@soyad, tel_no1=@telNo1, tel_no2=@telNo2, mail=@mail, unvan=@unvan, grup_id=@grupId WHERE kisi_id=@kisiId";
                    komut.Parameters.AddWithValue("@ad", KisiAdi);
                    komut.Parameters.AddWithValue("@soyad", KisiSoyadi);
                    komut.Parameters.AddWithValue("@telNo1", TelNo1);
                    komut.Parameters.AddWithValue("@telNo2", TelNo2);
                    komut.Parameters.AddWithValue("@mail", Mail);
                    komut.Parameters.AddWithValue("@unvan", Unvan);
                    komut.Parameters.AddWithValue("@grupId", GrupId);
                    komut.Parameters.AddWithValue("@kisiId", KisiId);
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Güncelleme işlemi başarılı");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Güncelleme işleminde hata oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void KisiSil()
        {
            using (var baglanti = veriTabaniBaglantisi.baglan())
            {
                try
                {
                    baglanti.Open();
                    komut.Connection = baglanti;
                    komut.CommandText = "DELETE FROM kisiler WHERE kisi_id=@kisiId";
                    komut.Parameters.AddWithValue("@kisiId", KisiId);
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Silme işlemi başarılı");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Silme işleminde hata oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public DataTable KisileriListele()
        {
            using (var baglanti = veriTabaniBaglantisi.baglan())
            {
                try
                {
                    komut.Connection = baglanti;
                    komut.CommandText = "SELECT kisi_id, ad, soyad, tel_no1, tel_no2, mail, unvan, grup_adi FROM kisiler INNER JOIN gruplar ON kisiler.grup_id = gruplar.grup_id ORDER BY ad ASC, soyad ASC";
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(komut);
                    DataTable kisiListesi = new DataTable();
                    dataAdapter.Fill(kisiListesi);
                    return kisiListesi;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Listeleme işleminde hata oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        public DataTable KisiAra()
        {
            using (var baglanti = veriTabaniBaglantisi.baglan())
            {
                try
                {
                    komut.Connection = baglanti;
                    if (GrupId == 0)
                    {
                        komut.CommandText = "SELECT kisi_id, ad, soyad, tel_no1, tel_no2, mail, unvan, grup_adi FROM kisiler INNER JOIN gruplar ON kisiler.grup_id = gruplar.grup_id WHERE ad LIKE @ad AND soyad LIKE @soyad AND (tel_no1 LIKE @telNo OR tel_no2 LIKE @telNo) ORDER BY ad ASC, soyad ASC";
                    }
                    else
                    {
                        komut.CommandText = "SELECT kisi_id, ad, soyad, tel_no1, tel_no2, mail, unvan, grup_adi FROM kisiler INNER JOIN gruplar ON kisiler.grup_id = gruplar.grup_id WHERE ad LIKE @ad AND soyad LIKE @soyad AND (tel_no1 LIKE @telNo OR tel_no2 LIKE @telNo) AND gruplar.grup_id=@grupId ORDER BY ad ASC, soyad ASC";
                    }

                    komut.Parameters.AddWithValue("@ad", KisiAdi + "%");
                    komut.Parameters.AddWithValue("@soyad", KisiSoyadi + "%");
                    komut.Parameters.AddWithValue("@telNo", TelNo1 + "%");
                    if (GrupId != 0)
                    {
                        komut.Parameters.AddWithValue("@grupId", GrupId);
                    }

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(komut);
                    DataTable arananKisiListesi = new DataTable();
                    dataAdapter.Fill(arananKisiListesi);
                    return arananKisiListesi;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Arama işleminde hata oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }
    }

    class grup
    {
        public int GrupId { get; set; }
        public string GrupAdi { get; set; }
        public string Aciklama { get; set; }

        VeriTabaniBaglantisi veriTabaniBaglantisi;
        SqlCommand komut;

        public grup()
        {
            veriTabaniBaglantisi = new VeriTabaniBaglantisi();
            komut = new SqlCommand();
        }

        public void GrupEkle()
        {
            using (var baglanti = veriTabaniBaglantisi.baglan())
            {
                try
                {
                    baglanti.Open();
                    komut.Connection = baglanti;
                    komut.CommandText = "INSERT INTO gruplar(grup_adi,aciklama) VALUES(@grupAdi,@aciklama)";
                    komut.Parameters.AddWithValue("@grupAdi", GrupAdi);
                    komut.Parameters.AddWithValue("@aciklama", Aciklama);
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Kayıt işlemi başarılı");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Kayıt işleminde hata oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void GrubuGuncelle()
        {
            using (var baglanti = veriTabaniBaglantisi.baglan())
            {
                try
                {
                    baglanti.Open();
                    komut.Connection = baglanti;
                    komut.CommandText = "UPDATE gruplar SET grup_adi=@grupAdi, aciklama=@aciklama WHERE grup_id=@grupId";
                    komut.Parameters.AddWithValue("@grupAdi", GrupAdi);
                    komut.Parameters.AddWithValue("@aciklama", Aciklama);
                    komut.Parameters.AddWithValue("@grupId", GrupId);
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Güncelleme işlemi başarılı");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Güncelleme işleminde hata oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void GrupSil()
        {
            using (var baglanti = veriTabaniBaglantisi.baglan())
            {
                try
                {
                    baglanti.Open();
                    komut.Connection = baglanti;
                    komut.CommandText = "DELETE FROM gruplar WHERE grup_id=@grupId";
                    komut.Parameters.AddWithValue("@grupId", GrupId);
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Silme işlemi başarılı");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Silme işleminde hata oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public DataTable GruplariListele()
        {
            using (var baglanti = veriTabaniBaglantisi.baglan())
            {
                try
                {
                    komut.Connection = baglanti;
                    komut.CommandText = "SELECT * FROM gruplar ORDER BY grup_adi ASC";
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(komut);
                    DataTable grupListesi = new DataTable();
                    dataAdapter.Fill(grupListesi);
                    return grupListesi;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Listeleme işleminde hata oluştu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }
    }
}
