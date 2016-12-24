using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities;
using GoogleMapsApi.StaticMaps;
using System.Xml;

namespace ProjetC
{
    public partial class ProjetC : Form
    {
        Donnes d = new Donnes();
       
        public ProjetC()
        {
            InitializeComponent();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Déclaration des variables 
            int i = 1;
            int j = 1;
            List<string> Listplc = new List<string>();
            List<string> ListDirc = new List<string>();
            List<string> ListDirc2 = new List<string>();
            List<string> ListDis = new List<string>();
            List<string> ListDur = new List<string>();

            if (textBox1.Text != "" && textBox2.Text != "" && textBox4.Text != "" && textBox3.Text != "" && comboBox1.Text != "Choisir" && comboBox2.Text != "Choisir" && comboBox3.Text != "Choisir")
            {
                d.Latitude = textBox1.Text;
                d.Longitude = textBox2.Text;
                d.Type = comboBox1.Text;
                d.Rayon = int.Parse(textBox4.Text);
                d.Transport = comboBox2.Text;
                d.Zoom = int.Parse(textBox3.Text);
                d.MapType = comboBox3.Text;
                //URl qui va nous retourner un fichier XML
                var url1 = string.Format("https://maps.googleapis.com/maps/api/place/search/xml?location={0},{1}&radius={2}&types={3}&key=AIzaSyCufh79pCwvNMcWVesrTKpVEGVyOa4__ks", d.Latitude, d.Longitude, d.Rayon, d.Type);
                //Fournit des méthodes communes pour l’envoi de données vers et de recevoir des données d’une ressource identifiée par un URI.
                var webClient = new WebClient();
                //Implémente TextReader qui lit les caractères à partir d'un flux d'octets dans un encodage particulier.
                StreamReader sr = new StreamReader(webClient.OpenRead(url1));
                //Représente un lecteur fournissant un accès rapide, non mis en cache et en avant uniquement vers les données XML.
                var xmlReader = new XmlTextReader(sr);
                listBox1.Items.Clear();
                //lit le nœud suivant à partir du flux.
                while (xmlReader.Read())
                {
                    //Test sur chaque balise du fichier XML
                    switch (xmlReader.Name)
                    {
                        case "name":
                            listBox1.Items.Add("Résultat " + i + ":");
                            listBox1.Items.Add(xmlReader.ReadString());
                            i++;
                            break;
                        case "vicinity":
                            listBox1.Items.Add(xmlReader.ReadString());
                            listBox1.Items.Add("");
                            break;
                        case "location":
                            xmlReader.ReadToDescendant("lat");
                            Listplc.Add(xmlReader.ReadString());
                            xmlReader.ReadToNextSibling("lng");
                            Listplc.Add(xmlReader.ReadString());
                            break;
                    }
                }
                var url2 = string.Format("https://maps.googleapis.com/maps/api/staticmap?center={0},{1}&zoom={2}&size=800x600&maptype={3}&markers=color:red%7Clabel:H%7C{4},{5}", d.Latitude, d.Longitude, d.Zoom, d.MapType, d.Latitude, d.Longitude);
                if (Listplc.Count == 0)
                {
                    listBox1.Items.Add("IL n'y a aucun(e) " + d.Type + " dans ce rayon!");
                    webBrowser1.Navigate(url2.ToString());
                }
                else
                {
                    for (i = 0; i <= (Listplc.Count); i++)
                    {
                        Listplc.Remove("");
                    }
                    for (i = 1; i <= ((Listplc.Count) / 2); i += 2)
                    {
                        string s = string.Format("&markers=color:blue%7Clabel:{0}%7C{1},{2}", j, Listplc[i - 1], Listplc[i]);
                        url2 += s;
                        j++;
                    }


                    for (i = 1; i <= ((Listplc.Count) / 2); i += 2)
                    {
                        var url3 = string.Format("https://maps.googleapis.com/maps/api/directions/xml?origin={0},{1}&destination={2},{3}&mode={4}&key=AIzaSyCufh79pCwvNMcWVesrTKpVEGVyOa4__ks", d.Latitude, d.Longitude, Listplc[i - 1], Listplc[i], d.Transport);
                        string path = string.Format("&path=color:0x0000ff|weight:2|{0},{1}", d.Latitude, d.Longitude);

                       
                        
                        WebClient client = new WebClient();
                        StreamReader readerD1 = new StreamReader(client.OpenRead(url3));
                        XmlTextReader TextXmlD1 = new XmlTextReader(readerD1);
                        while (TextXmlD1.Read())
                        {
                            switch (TextXmlD1.Name)
                            {
                                case "start_location":
                                    TextXmlD1.ReadToDescendant("lat");
                                    ListDirc.Add(TextXmlD1.ReadString());
                                    TextXmlD1.ReadToNextSibling("lng");
                                    ListDirc.Add(TextXmlD1.ReadString());
                                    break;
                            }
                        }
                        StreamReader readerD2 = new StreamReader(client.OpenRead(url3));
                        XmlTextReader TextXmlD2 = new XmlTextReader(readerD2);
                        while (TextXmlD2.Read())
                        {
                            switch (TextXmlD2.Name)
                            {
                                case "end_location":
                                    TextXmlD2.ReadToDescendant("lat");
                                    ListDirc2.Add(TextXmlD2.ReadString());
                                    TextXmlD2.ReadToNextSibling("lng");
                                    ListDirc2.Add(TextXmlD2.ReadString());
                                    break;
                            }
                        }

                        for (i = 0; i <= (ListDirc.Count); i++)
                        {
                            ListDirc.Remove("");
                            ListDirc2.Remove("");
                        }


                        for (i = 0; i < (ListDirc.Count) - 2; i += 2)
                        {
                            path += string.Format("|" + ListDirc[i] + "," + ListDirc[i + 1]);
                            path += string.Format("|" + ListDirc2[i] + "," + ListDirc2[i + 1]);
                        }
                        url2 += path;
                        ListDirc.Clear();
                        ListDirc2.Clear();
                    }


                    //Autorise l'application à parcourir des pages web
                    webBrowser1.Navigate(url2.ToString());
                }
            }
            else
                MessageBox.Show("Veulliez remplir tous les champs SVP!");

        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Projet_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        
        private void button2_Click_1(object sender, EventArgs e)
        {   
            //Vider tous les champs de l'application 
             textBox1.Clear();
             textBox2.Clear(); 
             comboBox1.Text="Choisir";
             textBox4.Text = "0";
             comboBox2.Text = "Choisir";
             textBox3.Text = "0";
             comboBox3.Text = "Choisir";
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox3.Text = (1 + int.Parse(textBox3.Text)).ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (int.Parse(textBox3.Text) > 0)
                textBox3.Text = (int.Parse(textBox3.Text) - 1).ToString();
            else
                MessageBox.Show("Le zoom ne peux pas étre un nombre négatif!");
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox4.Text = (50 + int.Parse(textBox4.Text)).ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (int.Parse(textBox4.Text) > 0)
                textBox4.Text = (int.Parse(textBox4.Text) - 50).ToString();
            else
                MessageBox.Show("Le rayon ne peux pas étre un nombre négatif!");
        }
    }
}