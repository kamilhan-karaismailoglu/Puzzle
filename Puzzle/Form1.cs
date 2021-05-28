using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Puzzle
{
    class cell : TextBox
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
    class ipucu : Label
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
    public partial class Form1 : Form
    {
        static int[,] CevapAnahtarı;
        static int genislik;
        static List<cell> txtBoxlar = new List<cell> { };
        static List<ipucu> ipuclari = new List<ipucu> { };
        public Form1()
        {
            InitializeComponent();
            trackBarIpucu.Maximum = 3;
            trackBarIpucu.Minimum = 1;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CevapAnahtarı = CevapAnahtarıOlustur();

            while (!tabloKontrol(CevapAnahtarı))
            {
                CevapAnahtarı = CevapAnahtarıOlustur();
            }
            PanelOlustur();
            if (durum == 0)
            {
                durum = 1;
                timer.Start();
            }
            lblBaslik.Location = new Point((panel.Width + groupBox1.Width) / 3, 9);
        }
        private void textBoxWidth_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        private void NewGame_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(textBoxWidth.Text)>8 || Convert.ToInt32(textBoxWidth.Text) < 2)
            {
                MessageBox.Show("Izgara İçin En Fazla 8x8, En Az 2x2 Büyüklüğünde Olucak Şekilde n Değeri Giriniz", "Tablo Oluşturulamadı");
            }
            else
            {
                txtBoxlar.Clear();
                ipuclari.Clear();
                panel.Controls.Clear();
                CevapAnahtarı = CevapAnahtarıOlustur();

                timer.Enabled = true;
                buttonKontrol.Enabled = true;
                while (!tabloKontrol(CevapAnahtarı))
                {
                    CevapAnahtarı = CevapAnahtarıOlustur();
                }


                PanelOlustur();

                if (Convert.ToInt32(textBoxWidth.Text) > 4)
                {
                    this.Size = new Size(panel.Width + 270, panel.Height+100);

                }
                else if (Convert.ToInt32(textBoxWidth.Text) <= 4)
                {
                    this.Size = new Size(panel.Width + 270, 301);
                }

                saat = 0;
                dakika = 0;
                saniye = 0;
                if (durum == 0)
                {
                    durum = 1;
                    timer.Start();
                }

                lblBaslik.Location = new Point((panel.Width+groupBox1.Width)/3, 9);
            }
        }
        int[,] CevapAnahtarıOlustur()
        {
            int gelenGenislik = Convert.ToInt16(textBoxWidth.Text);
            genislik = gelenGenislik;
            Random rnd = new Random();
           
            int[,] tablo = new int[genislik, genislik];

            int RandomDeger = rnd.Next(1, genislik + 1);
            int sayac = 0;

            for (int x = 0; x < genislik; x++)
            {
                List<int> satırDegerleri = new List<int> { };
                for (int i = 1; i < genislik+1; i++)
                {
                    satırDegerleri.Add(i);
                }

                for (int y = 0; y < genislik; y++)
                {
                    sayac = 0;
                    while (VarMı(RandomDeger, x, y, tablo)==false)
                    {
                        if (sayac>genislik)
                        {
                            RandomDeger = 0;
                            break;
                        }
                        RandomDeger = satırDegerleri[rnd.Next(0,satırDegerleri.Count)];
                        sayac++;
                    }
                    tablo[x, y] = RandomDeger;
                    satırDegerleri.Remove(RandomDeger);
                }
            }
            
            return tablo;          
        }
        bool tabloKontrol(int[,] tablo)
        {
            foreach (var item in tablo)
            {
                if (item == 0)
                {
                    return false;
                }
            }
            return true;
        }
        bool VarMı(int randomDeger, int x, int y, int[,] tablo)
        {
            for (int i = 0; i < genislik; i++)
            {            
                if (i!=x && tablo[i, y] == randomDeger)
                {
                    return false;
                }
                
                if (i != y && tablo[x, i] == randomDeger)
                {
                    return false;
                }             
            }
            return true;
        }
        void PanelOlustur() 
        {
            Random rnd = new Random();
            panel.Enabled = true;
            panel.Size = new Size(genislik*60,genislik*50-30);
            int sayac=0;

            for (int x = 0; x < genislik*60; x=x+60)
            {
                for (int y = 0; y < genislik * 50; y = y + 50)
                {
                    textBoxEkle(new Point(x+11, y));
                    
                      if (x != genislik * 60 - 60)
                      {
                            
                            AraLabelEkle(new Point(x, y));
                            sayac++;
                      }
                      if (y != genislik * 50 - 50)
                      {
                            
                            SatırLabelEkle(new Point(x - 30, y + 25));
                            sayac++;                     
                      }                                       
                }
            }

            int sınır = 0;
            int anlıkTotal = 0;

            if (trackBarIpucu.Value == 1)
            {
                sınır = (genislik-1) * (genislik-1)*2;
                if (genislik == 2)
                {
                    sınır = 3;
                }
            }
            else if (trackBarIpucu.Value == 2)
            {
                sınır = genislik * (genislik-2)+5;
                if (genislik < 4)
                {
                    sınır = genislik+2;
                }
                if (genislik==2)
                {
                    sınır = 2;
                }
            }
            else if (trackBarIpucu.Value == 3)
            {
                sınır = genislik*(genislik-3);
                if (genislik<4)
                {
                    sınır = genislik;
                }
                if (genislik == 2)
                {
                    sınır = 1;
                }
            }

            int random;
            while (anlıkTotal != sınır)
            {
                random = rnd.Next(0, ipuclari.Count);
                if (ipuclari[random].Visible == false)
                {
                    ipuclari[random].Visible = true;
                    anlıkTotal++;
                }
            }
        }
        void textBoxEkle(Point konum)
        {
            cell txtBox = new cell();
            txtBox.Name = "txtBox"+((konum.X-11)/60).ToString()+ (konum.Y/50).ToString();
            txtBox.Size = new Size(30, 30);
            txtBox.Location = konum;
            txtBox.TextAlign = HorizontalAlignment.Center;
            txtBox.KeyPress += textBoxWidth_KeyPress;
            txtBox.X = konum.X;
            txtBox.Y = konum.Y;
            txtBoxlar.Add(txtBox);
            panel.Controls.Add(txtBox);
        }
        void AraLabelEkle(Point konum)
        {
            ipucu lbl = new ipucu();
            lbl.Name = "lbl:"+konum.X + "," + konum.Y;
            lbl.Location = new Point(konum.X+50,konum.Y);
            lbl.Width = 10;
            lbl.Visible = false;
            lbl.X = konum.X;
            lbl.Y = konum.Y;
            if (CevapAnahtarı[lbl.Y / 50, lbl.X / 60] > CevapAnahtarı[lbl.Y / 50, lbl.X / 60 + 1])
            {
                lbl.Text = ">";
            }
            else if (CevapAnahtarı[lbl.Y / 50, lbl.X / 60] < CevapAnahtarı[lbl.Y / 50, lbl.X / 60 + 1])
            {
                lbl.Text = "<";
            }
            ipuclari.Add(lbl);
            panel.Controls.Add(lbl);
        }
        void SatırLabelEkle(Point konum)
        {
            ipucu lbl = new ipucu();
            lbl.Name = "lbl:" + konum.X + "," + konum.Y;
            lbl.Location = new Point(konum.X + 49, konum.Y);
            lbl.Width = 10;
            lbl.Height = 15;
            lbl.Visible = false;
            lbl.X = konum.X;
            lbl.Y = konum.Y;
            if (CevapAnahtarı[(lbl.Y-25) / 50, (lbl.X +30) / 60] > CevapAnahtarı[(lbl.Y - 25)/ 50+1, (lbl.X + 30) / 60])
            {
                lbl.Text = "∨";
            }
            else if (CevapAnahtarı[(lbl.Y - 25) / 50, (lbl.X + 30) / 60] < CevapAnahtarı[(lbl.Y - 25) / 50+1, (lbl.X + 30) / 60])
            {
                lbl.Text = "∧";
            }
            ipuclari.Add(lbl);
            panel.Controls.Add(lbl);
        }
        private void buttonKontrol_Click(object sender, EventArgs e)
        {   
            cell[,] textboxMatris = new cell[genislik, genislik];
            int sayac = 0;
            for (int x = 0; x < genislik; x++)
            {
                for (int y = 0; y < genislik; y++)
                {
                    textboxMatris[y, x] = txtBoxlar[sayac];
                    sayac++;
                }
            }
            
            int[,] CevapMatrisi = new int[genislik, genislik];
            for (int x = 0; x < genislik; x++)
            {
                for (int y = 0; y < genislik; y++)
                {
                    if (textboxMatris[x, y].Text != null && textboxMatris[x, y].Text != "")
                    {
                        CevapMatrisi[x, y] = Convert.ToInt32(textboxMatris[x, y].Text);
                    }
                    else
                    {
                        CevapMatrisi[x, y] = 0;
                    }
                }
            }

            int hatasayısı = 0;
            for (int x = 0; x < genislik; x++)
            {
                for (int y = 0; y < genislik; y++)
                {
                    textboxMatris[x, y].BackColor = Color.White;
                    textboxMatris[x, y].ForeColor = Color.White;

                    if (CevapMatrisi[x, y] != CevapAnahtarı[x, y])
                    {
                        textboxMatris[x, y].BackColor = Color.Red;
                        hatasayısı++;
                    }
                    else
                    {
                        textboxMatris[x, y].BackColor = Color.Green;
                    }
                }
            }
            if (hatasayısı==0)
            {
                panel.Enabled = false;
                buttonKontrol.Enabled = false;
                timer.Enabled = false;
                for (int x = 0; x < genislik; x++)
                {
                    for (int y = 0; y < genislik; y++)
                    {
                        textboxMatris[x, y].BackColor = Color.White;
                        textboxMatris[x, y].ForeColor = Color.Black;                   
                    }
                }
                MessageBox.Show("Tebrikler Kazandınız","Oyun Bitti");
            }
        }

        int durum = 0;
        int saat = 0, dakika = 0, saniye = 0;
        int sayacKapat = 0;
        private void timerKapat_Tick(object sender, EventArgs e)
        {
            sayacKapat++;
            if (sayacKapat == 4)
            {
                Application.Exit();
            }
        }
        private void buttonBitir_Click(object sender, EventArgs e)
        {
            cell[,] textboxMatris = new cell[genislik, genislik];
            int sayac = 0;
            for (int x = 0; x < genislik; x++)
            {
                for (int y = 0; y < genislik; y++)
                {
                    textboxMatris[y, x] = txtBoxlar[sayac];
                    sayac++;
                }
            }

            for (int x = 0; x < genislik; x++)
            {
                for (int y = 0; y < genislik; y++)
                {
                    textboxMatris[x, y].BackColor = Color.White;
                    textboxMatris[x, y].Text = CevapAnahtarı[x, y].ToString();                
                }
            }

            panel.Enabled = false;
            buttonBitir.Enabled = false;
            buttonKontrol.Enabled = false;
            NewGame.Enabled = false;
            trackBarIpucu.Enabled = false;
            textBoxWidth.Enabled = false;
            timer.Enabled = false;

            lblBaslik.Text = "OYUN SONLANDIRILIYOR...";
            timerKapat.Enabled = true;
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (saniye == 60)
            {
                saniye = 0;
                dakika++;
            }
            if (dakika == 60)
            {
                dakika = 0;
                saniye = 0;
                saat++;
            }
            lblTime.Text = String.Format("{0:D2}", saat) + ":" + String.Format("{0:D2}", dakika) + ":" + String.Format("{0:D2}", saniye);
            saniye++;
        }
    }
}
