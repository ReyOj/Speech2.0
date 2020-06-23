using System;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.RequestParams;
using System.IO.Ports;
using System.Diagnostics;
using VkNet.Enums.Filters;
using VkNet.Model.Keyboard;
using System.Drawing;
using System.Collections.Generic;
using VkNet.Model.Attachments;
using System.Linq;
using System.Net;
using System.Text;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Threading;

namespace bot
{
    class vk
    {
        public static VkApi api = new VkApi();
        static string result;
        static long? userID;
        static BackgroundWorker spam = new BackgroundWorker();
        public static void Authorize()
        {
            api.Authorize(new ApiAuthParams()
            {
                AccessToken = "4f01f40c062bc3666c5bb98a64f9582ced5ee7740907be296c175e99ad078c07c9b1fd496b4b91cbe96b1" 
            });
            spam.DoWork += Spam_DoWork;
            spam.WorkerSupportsCancellation = true;
        }

        private static void Spam_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                SendPhoto("", userID);
                Thread.Sleep(500);
                if (spam.CancellationPending)
                {
                    break;
                }
            }
        }

        public static void CheckMessage() {
            while (true) // Бесконечный цикл, получение обновлений
            {
                var s = api.Groups.GetLongPollServer(194642153);

                var poll = api.Groups.GetBotsLongPollHistory(
                    new BotsLongPollHistoryParams()
                    { Server = s.Server, Ts = s.Ts, Key = s.Key, Wait = 10 });
                Console.WriteLine("1");
                // api.Wall.Post(new WallPostParams {Message = "Hi" } );
                //SendMessage("Дарова", 531075153);
                foreach (var a in poll.Updates)
                {
                    if(a.Type == GroupUpdateType.WallPostNew)
                    {
                        SendMessage("Новый пост в группе", 531075153);
                    }
                    if (a.Type == GroupUpdateType.MessageNew)
                    {
                        string message = a.Message.Body.ToLower();
                        userID = a.Message.UserId;
                        if (message == "миша кто?")
                        {
                            Console.WriteLine("2");
                            SendMessage("[misha4a|Миша]" + " бог", userID);
                        }
                        else if (message == "мирон кто?")
                        {
                            SendMessage("[coolfire_nr|Мирон]" + " долбанный яойщик", userID);
                        }
                        else if (message == "температура")
                        {
                            string temp,temp1;
                            int tempint;
                            using (SerialPort sp = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One))
                            {
                                sp.Open();
                                sp.Write("2");
                                temp = sp.ReadLine();
                                sp.Close();
                            }
                            tempint = Convert.ToInt32(temp);
                            temp1 = "Температура в комнате равна " + temp;
                            if (tempint%10 == 1 && tempint/10 != 1)
                            {
                                temp1 += " градус";
                            }
                            else if ((tempint%10 == 2 || tempint%10 == 3 || tempint%10 == 4) && tempint/10 != 1)
                            {
                                temp1 += " градуса";
                            }
                            else
                            {
                                temp1 += " градусов";
                            }
                            SendMessage(temp1, userID);
                        }
                        else if (message == "влажность")
                        {
                            string temp, temp1;
                            int tempint;
                            using (SerialPort sp = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One))
                            {
                                sp.Open();
                                sp.Write("3");
                                temp = sp.ReadLine();
                                sp.Close();
                            }
                            tempint = Convert.ToInt32(temp);
                            temp1 = "Влажность в комнате равна " + temp;
                            if (tempint % 10 == 1 && tempint / 10 != 1)
                            {
                                temp1 += " процент";
                            }
                            else if ((tempint % 10 == 2 || tempint % 10 == 3 || tempint % 10 == 4) && tempint / 10 != 1)
                            {
                                temp1 += " процента";
                            }
                            else
                            {
                                temp1 += " процентов";
                            }
                            SendMessage(temp1, userID);
                        }
                        else if(message == "свет включить")
                        {
                            using (SerialPort sp = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One))
                            {
                                sp.Open();
                                sp.Write("1");
                                sp.Close();
                            }
                            SendMessage("Включаю", userID);
                        }
                        else if(message == "свет выключить")
                        {
                            using (SerialPort sp = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One))
                            {
                                sp.Open();
                                sp.Write("0");
                                sp.Close();
                            }
                            SendMessage("Выключаю", userID);
                        }
                        else if (message == "включи музыку")
                        {
                            Process.Start(@"C:\Program Files (x86)\AIMP\AIMP.exe");
                        }
                        else if(message == "photo")
                        {
                            var uploadServer = api.Photo.GetMessagesUploadServer((long)userID);
                            var wc = new WebClient();
                            result = Encoding.ASCII.GetString(wc.UploadFile(uploadServer.UploadUrl, @"C:\\Users\Admin\Pictures\1232323.jpg"));
                            SendPhoto("", userID);
                        }
                        else if (message == "furry" || message == "gift")
                        {
                            var uploadServer = api.Photo.GetMessagesUploadServer((long)userID);
                            var wc = new WebClient();
                            result = Encoding.ASCII.GetString(wc.UploadFile(uploadServer.UploadUrl, @"C:\\Users\Admin\Pictures\gift1.jpg"));
                            spam.RunWorkerAsync();
                        }
                        else if (message == "miron")
                        {
                            spam.CancelAsync();
                        }
                        else
                        {
                            SendMessage("Не понимаешь - не лезь", userID);
                        }
                    }
                }
            }
        }

        public static void SendPhoto(string message, long? userID)
        {
            Random rnd = new Random();
            var photo = api.Photo.SaveMessagesPhoto(result);
            api.Messages.Send(new MessagesSendParams
            {
                RandomId = rnd.Next(),
                UserId = userID,
                Message = message,
                Attachments = new List<MediaAttachment>
                {
                    photo.FirstOrDefault()
                }
            });
        }
        public static void SendMessage(string message, long? userID)
        {
            Random rnd = new Random();
            KeyboardBuilder key = new KeyboardBuilder();
            key.AddButton("Температура", "456");
            key.AddButton("Свет включить", "456");
            key.AddButton("Свет выключить", "456");
            MessageKeyboard mess = key.Build();
            api.Messages.Send(new MessagesSendParams
            {
                Keyboard = mess,
                RandomId = rnd.Next(),
                UserId = userID,
                Message = message
            });
        }
    }
}