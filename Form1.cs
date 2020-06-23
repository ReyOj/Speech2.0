using System;
using System.Windows.Forms;
using Microsoft.Speech.Recognition;
using System.Threading;
using System.IO.Ports;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Media;
using System.Reflection;
using bot;
using System.ComponentModel;
using System.Speech.Synthesis;

namespace Speech
{
    public partial class Form1 : Form
    {
        static Label l;
        static TextBox maintxt;
        static bool flag = false;
        public static bool flag1;
        static bool music = true;
        static double tempint;
        static SoundPlayer sp = new SoundPlayer();
        BackgroundWorker bw = new BackgroundWorker();
        BackgroundWorker speech = new BackgroundWorker();
        SpeechRecognitionEngine sre;
        public Form1()
        {
            InitializeComponent();
            vk.Authorize();
            bw.DoWork += new DoWorkEventHandler(Bw_DoWork);
            speech.DoWork += new DoWorkEventHandler(Speech_Recognize);
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("ru-ru");
            sre = new SpeechRecognitionEngine(ci);
        }

        private void Speech_Recognize(object sender, DoWorkEventArgs e)
        {
            sre.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            vk.CheckMessage();
        }

        private void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            maintxt = main;
            maintxt.Text = "You: " + e.Result.Text + "\r\n" + maintxt.Text;
            if (e.Result.Confidence > 0.5 && e.Result.Text == "алиса")
            {
                SpeechSynthesizer ss = new SpeechSynthesizer();
                Random rnd = new Random();
                Debug.Print(rnd.Next(4).ToString());
                switch (rnd.Next(4))
                {
                    case 0:
                        ss.Volume = 100;
                        ss.Rate = 3;
                        ss.SpeakAsync("Чего вам надобно, мой господин?");
                        maintxt.Text = "Me: " + "Чего вам надобно, мой господин?" + "\r\n" + maintxt.Text;
                        //l.Text = e.Result.Text;
                        flag = true;
                        break;
                    case 1:
                        ss = new SpeechSynthesizer();
                        ss.Volume = 100;
                        ss.Rate = 3;
                        ss.SpeakAsync("Слушаю");
                        maintxt.Text = "Me: " + "Слушаю" + "\r\n" + maintxt.Text;
                        //l.Text = e.Result.Text;
                        flag = true;
                        break;
                    case 2:
                        ss = new SpeechSynthesizer();
                        ss.Volume = 100;
                        ss.Rate = 3;
                        ss.SpeakAsync("Алло, директор?");
                        maintxt.Text = "Me: " + "Алло, директор?" + "\r\n" + maintxt.Text;
                        //l.Text = e.Result.Text;
                        flag = true;
                        break;
                    case 3:
                        ss = new SpeechSynthesizer();
                        ss.Volume = 100;
                        ss.Rate = 3;
                        ss.SpeakAsync("Алло");
                        maintxt.Text = "Me: " + "Алло" + "\r\n" + maintxt.Text;
                        //l.Text = e.Result.Text;
                        flag = true;
                        break;
                    case 4:
                        ss = new SpeechSynthesizer();
                        ss.Volume = 100;
                        ss.Rate = 3;
                        ss.SpeakAsync("Панки хой");
                        maintxt.Text = "Me: " + "Панки хой" + "\r\n" + maintxt.Text;
                        //l.Text = e.Result.Text;
                        flag = true;
                        break;
                }
            }
            else if (e.Result.Confidence > 0.8 && e.Result.Text == "включи свет" && flag == true && flag1 == true)
            {
                SpeechSynthesizer ss = new SpeechSynthesizer();
                ss.Volume = 100;
                ss.Rate = 3;
                ss.SpeakAsync("Свет уже всключён, мой господин");
                maintxt.Text = "Me: " + "Свет уже всключён, мой господин" + "\r\n" + maintxt.Text;
                flag = false;
            }
            else if (e.Result.Confidence > 0.5 && e.Result.Text == "включи свет" && flag && !flag1)
            {
                SpeechSynthesizer ss = new SpeechSynthesizer();
                ss.Volume = 100;
                ss.Rate = 3;
                ss.SpeakAsync("Включаю свет, мой господин");
                maintxt.Text = "Me: " + "Включаю свет, мой господин" +"\r\n" + maintxt.Text;
                try
                {
                    using (SerialPort sp = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One))
                    {
                        sp.Open();
                        sp.Write("1");
                        sp.Close();
                    }
                }
                catch
                {
                    
                    Check();
                }
                flag = false;
                flag1 = true;
            }
            else if (e.Result.Confidence > 0.8 && e.Result.Text == "выключи свет" && flag == true && flag1 == false)
            {
                SpeechSynthesizer ss = new SpeechSynthesizer();
                ss.Volume = 100;
                ss.Rate = 3;
                ss.SpeakAsync("Свет уже выключен, мой господин");
                maintxt.Text = "Me: " + "Свет уже выключен, мой господин" +"\r\n" + maintxt.Text;
                flag = false;
            }
            else if (e.Result.Confidence > 0.5 && e.Result.Text == "выключи свет" && flag == true)
            {
                SpeechSynthesizer ss = new SpeechSynthesizer();
                ss.Volume = 100;
                ss.Rate = 3;
                ss.SpeakAsync("Выключаю свет, мой господин");
                maintxt.Text = "Me: " + "Выключаю свет, мой господин" +"\r\n" + maintxt.Text;
                try
                {
                    using (SerialPort sp = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One))
                    {
                        sp.Open();
                        sp.Write("0");
                        sp.Close();
                    }
                }
                catch
                {
                    Check();
                }
                flag = false;
                flag1 = false;
            }
            else if (e.Result.Confidence > 0.8 && e.Result.Text == "температура" && flag)
            {
                string temp;
                SpeechSynthesizer ss = new SpeechSynthesizer();
                ss.Volume = 100;
                ss.Rate = 3;
                try
                {
                    using (SerialPort sp = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One))
                    {
                        sp.Open();
                        sp.Write("2");
                        l.Text = sp.ReadLine();
                        sp.Close();
                    }
                }
                catch
                {
                    Check();
                }
                temp = "на данный момент в комнате температура равна " + l.Text;
                if (Convert.ToInt32(l.Text) % 10 == 1 && Convert.ToInt32(l.Text)/10 != 1)
                {
                    temp += " градус, мой господин";
                }
                else if ((Convert.ToInt32(l.Text) % 10 == 2 || Convert.ToInt32(l.Text) % 10 == 3 || Convert.ToInt32(l.Text) % 10 == 4) && Convert.ToInt32(l.Text)/10 != 10)
                {
                    temp += " градуса, мой господин";
                }
                else
                {
                    temp += " градусов, мой господин";
                }
                ss.SpeakAsync(temp);
                maintxt.Text = "Me: " + temp +"\r\n" + maintxt.Text;
                flag = false;
            }
            else if (e.Result.Confidence > 0.8 && e.Result.Text == "влажность" && flag)
            {
                string temp;
                SpeechSynthesizer ss = new SpeechSynthesizer();
                ss.Volume = 100;
                ss.Rate = 3;
                try
                {
                    using (SerialPort sp = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One))
                    {
                        sp.Open();
                        sp.Write("3");
                        l.Text = sp.ReadLine();
                        sp.Close();
                    }
                }
                catch
                {
                    Check();
                }
                temp = "на данный момент в комнате влажность равна " + l.Text;
                if (Convert.ToInt32(l.Text) % 10 == 1)
                {
                    temp += " процент, мой господин";
                }
                else if (Convert.ToInt32(l.Text) % 10 == 2 || Convert.ToInt32(l.Text) % 10 == 3 || Convert.ToInt32(l.Text) % 10 == 4)
                {
                    temp += " процента, мой господин";
                }
                else
                {
                    temp += " процентов, мой господин";
                }
                ss.SpeakAsync(temp);
                maintxt.Text = "Me: " + temp +"\r\n" + maintxt.Text;
                flag = false;
            }
            
            else if (e.Result.Confidence > 0.8 && e.Result.Text == "погода" && flag)
            {
                WebRequest request;
                request = WebRequest.Create(@"http://informer.gismeteo.ru/xml/34929_1.xml");
                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        string data = reader.ReadToEnd();
                        string temp, temp1;
                        string tempmin = null, tempmax = null;
                        Debug.WriteLine(data);
                        XmlDocument xdata = new XmlDocument();
                        xdata.LoadXml(data);
                        //Debug.WriteLine(xdata.InnerText);
                        XmlNodeList temperature = xdata.GetElementsByTagName("TEMPERATURE");
                        XmlNode tempmx = temperature[0].Attributes.GetNamedItem("max");
                        XmlNode tempmn = temperature[0].Attributes.GetNamedItem("min");
                        tempmax = tempmx.InnerText;
                        tempmin = tempmn.InnerText;
                        Debug.WriteLine(tempmax);
                        Debug.WriteLine(tempmin);
                        tempint = (Convert.ToInt32(tempmin) + Convert.ToInt32(tempmax)) / 2;
                        Debug.WriteLine(tempint);
                        SpeechSynthesizer ss = new SpeechSynthesizer();
                        ss.Volume = 100;
                        ss.Rate = 3;
                        temp = tempint.ToString();
                        temp1 = "на данный момент температура в Краснодаре равна " + temp;
                        if (tempint % 10 == 1 && (tempint > 19 || tempint < 10))
                        {
                            temp1 += " градус";
                        }
                        else if (tempint % 10 == 2 || tempint % 10 == 3 || tempint % 10 == 4 && (tempint > 19 || tempint < 10))
                        {
                            temp1 += " градуса";
                        }
                        else
                        {
                            temp1 += " градусов";
                        }
                        ss.SpeakAsync(temp1);
                        maintxt.Text = "Me: " + temp1 +"\r\n" + maintxt.Text;
                        flag = false;
                    }
                } 
            }
            else if (e.Result.Confidence > 0.6 && e.Result.Text == "что по нгуену")
            {
                sp.Stream = Properties.Resources.durak;
                sp.Play();
            }
            else if (e.Result.Confidence > 0.7 && e.Result.Text == "горшок включи свет")
            {
                sp.Stream = Properties.Resources.svet;
                sp.Play();
            }
            else if (e.Result.Confidence > 0.7 && e.Result.Text == "горшок воскресни")
            {
                sp.Stream = Properties.Resources.hoy;
                sp.Play();
            }
            else if (e.Result.Confidence > 0.8 && e.Result.Text == "закройся")
            {
                SpeechSynthesizer ss = new SpeechSynthesizer();
                ss.Volume = 100;
                ss.Rate = 2;
                ss.SpeakAsync("ариведерчи, надеюсь ты меня потом откроешь");
                maintxt.Text = "Me: " + "ариведерчи, надеюсь ты меня потом откроешь" +"\r\n" + maintxt.Text;
                flag = false;
                Process[] shpion = Process.GetProcessesByName("AIMP");
                if (shpion.Length > 0)
                {
                    foreach (Process process in shpion)
                    {
                        process.Kill();
                    }
                }
                Process[] shpion2 = Process.GetProcessesByName("wmplayer");
                if (shpion2.Length > 0)
                {
                    foreach (Process process in shpion2)
                    {
                        process.Kill();
                    }
                }
            }
            else if (e.Result.Confidence > 0.7 && e.Result.Text == "это что сейчас было")
            {
                SpeechSynthesizer ss = new SpeechSynthesizer();
                ss.Volume = 100;
                ss.Rate = 3;
                ss.SpeakAsync("извините, мой господин, позволила себе лишнего, можете наказать меня как хотите");
                maintxt.Text = "Me: " + "извините, мой господин, позволила себе лишнего, можете наказать меня как хотите" +"\r\n" + maintxt.Text;
            }
            else if (e.Result.Confidence > 0.85 && e.Result.Text == "вруби музон" && music)
            {
                sp.Stream = Properties.Resources.music;
                sp.Play();
                Thread.Sleep(11500);
                Process.Start(@"C:\Program Files (x86)\AIMP\AIMP.exe");
            }
            else if(e.Result.Confidence > 0.7 && e.Result.Text == "вруби музон"  && !music)
            {
                Process.Start(@"C:\Program Files (x86)\AIMP\AIMP.exe");
            }
            else if (e.Result.Confidence > 0.5 && e.Result.Text == "будь как сбербанк на украине")
            {
                sp.Stream = Properties.Resources.nu_ladno;
                sp.Play();
                Thread.Sleep(2000);
                Application.Exit();
            }
            else if (e.Result.Confidence > 0.7 && e.Result.Text == "режим диско")
            {
                SpeechSynthesizer ss = new SpeechSynthesizer();
                ss.Volume = 100;
                ss.Rate = 3;
                ss.SpeakAsync("ну ок");
                maintxt.Text = "Me: " + "ну ок" +"\r\n" + maintxt.Text;
                Process.Start(@"C://Users/Admin/Videos/disco.mp4");
            }
            else if (e.Result.Confidence > 0.7 && e.Result.Text == "без музыки")
            {
                SpeechSynthesizer ss = new SpeechSynthesizer();
                ss.Volume = 100;
                ss.Rate = 3;
                ss.SpeakAsync("ок, поняла");
                maintxt.Text = "Me: " + "ок, поняла" +"\r\n" + maintxt.Text;
                music = false;
            }
            else if (e.Result.Confidence > 0.7 && e.Result.Text == "с музыкой"){
                music = true;
            }
            else if (e.Result.Confidence > 0.5 && e.Result.Text == "дэмэха")
            {
                SpeechSynthesizer ss = new SpeechSynthesizer();
                ss.Volume = 100;
                ss.Rate = 3;
                ss.SpeakAsync("дебил, ты его снёс");
                maintxt.Text = "Me: " + "дебил, ты его снёс" + "\r\n" + maintxt.Text;
                //Process.Start(@"C://Users/Admin/Videos/disco.mp4");
            }
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            bw.RunWorkerAsync();
            l = lable1;

            sre.SetInputToDefaultAudioDevice();

            sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);

            Choices colors = new Choices();
            colors.Add(new string[] {"дэмэха", "с музыкой", "без музыки", "режим диско", "будь как сбербанк на украине", "вруби музон", "это что сейчас было", "закройся", "горшок воскресни", "алиса", "включи свет", "выключи свет", "температура", "влажность", "погода", "что по нгуену", "горшок включи свет" });

            GrammarBuilder gb = new GrammarBuilder();
            gb.Append(colors);

            Grammar g = new Grammar(gb);
            sre.LoadGrammar(g);

            speech.RunWorkerAsync();
        }
        private void Check()
        {
            wmp.URL = @"E:\myhlam\ТЫ, МУДИЛА ГОРОХОВАЯ! Как заряжен Как запечатанная колода может быть в другом п.mp4";
            wmp.Ctlcontrols.play();
            wmp.Visible = true;
        }
    }
}