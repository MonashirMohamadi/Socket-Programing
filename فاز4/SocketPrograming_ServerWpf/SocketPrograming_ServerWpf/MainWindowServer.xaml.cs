using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using static SocketPrograming_ServerWpf.MainWindow;

namespace SocketPrograming_ServerWpf
{
    public partial class MainWindow : Window
    {
        private TcpListener server;
        private List<ClientInfo> clients = new List<ClientInfo>();
        private bool isRunningServer = false;
        private int clientCounter = 0;

        public MainWindow()
        {
            InitializeComponent();
            btnStart.Click += StartServer;
            btnStop.Click += StopServer;
            btnExit.Click += ExitButton_Click;
        }

        void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            StopServer(null, null);
            Environment.Exit(0);
        }

        private async void StartServer(object sender, RoutedEventArgs e)
        {
            if (isRunningServer) return;

            try
            {
                IPAddress ip = IPAddress.Parse(txtIP.Text);
                int port = int.Parse(txtPort.Text);

                server = new TcpListener(ip, port);
                server.Start();
                isRunningServer= true;

                txtListMessage.Items.Add("سرور راه‌اندازی شد...\n");

                // شروع پذیرش کلاینت‌ها در یک تسک جداگانه
                Thread acceptThread = new Thread(AcceptClients);
                acceptThread.IsBackground = true;
                acceptThread.Start();

               
            }
            catch (Exception ex)
            {
                AddServerMessage($"خطا: {ex.Message}");
            }
        }
        // ui پذیرش یک کلاینت بدون توقف
        private void AcceptClients()
        {
            try
            {
                while (isRunningServer)
                {
                    TcpClient client = server.AcceptTcpClient();
                    // دریافت نام از کلاینت
                    NetworkStream stream = client.GetStream();
                    byte[] nameBuffer = new byte[1024];
                    int nameBytes = stream.Read(nameBuffer, 0, nameBuffer.Length);
                    string clientName = Encoding.UTF8.GetString(nameBuffer, 0, nameBytes);

                    var clientInfo = new ClientInfo { Client = client, Name = clientName };
                    clients.Add(clientInfo);

                    // نمایش اطلاعات کلاینت در UI
                    AddServerMessage($"کلاینت جدید متصل شد: {clientName} ({clientInfo.EndPoint})");
                    BroadcastMessage($"کاربر جدید به چت پیوست: {clientName}", clientInfo);


                    // به‌روزرسانی لیست کاربران
                    UpdateClientList(); 

                    // ایجاد یک Thread جداگانه برای هر کلاینت
                    Thread clientThread = new Thread(() => HandleClient(clientInfo));
                    clientThread.IsBackground = true;
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                if (isRunningServer)
                    AddServerMessage($"خطا در پذیرش کلاینت: {ex.Message}");
            }
        }
        private void HandleClient(ClientInfo clientInfo)
        {
            NetworkStream stream = clientInfo.Client.GetStream();
            byte[] buffer = new byte[1024];


            try
            {
                while (isRunningServer && clientInfo.Client.Connected)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    // بررسی پیام خصوصی
                    if (message.StartsWith("/private "))
                    {
                        HandlePrivateMessage(message, clientInfo);
                    }
                    else
                    {
                        string formattedMessage = $"[{clientInfo.Name}]: {message}";
                        AddServerMessage(formattedMessage);

                        // ارسال پیام به همه کلاینت‌ها (شامل خود فرستنده)
                        BroadcastMessage(formattedMessage, null);
                    }
                }
            }
            catch (Exception ex)
            {
                AddServerMessage($"خطا در ارتباط با {clientInfo.Name}: {ex.Message}");
            }
            finally
            {
                clientInfo.Client.Close();
                clients.Remove(clientInfo);

                AddServerMessage($"کلاینت قطع شد: {clientInfo.Name}");
                BroadcastMessage($"کاربر چت را ترک کرد: {clientInfo.Name}", null);

                UpdateClientList();
            }
        }
        private void HandlePrivateMessage(string message, ClientInfo sender)
        {
            try
            {
                // شکستن پیام به بخش‌های مختلف
                string[] parts = message.Split(new[] { ' ' }, 3);
                if (parts.Length < 3)
                {
                    SendPrivateMessage("فرمت پیام خصوصی نادرست است. از /private نام کاربر متن پیام استفاده کنید.", sender);
                    return;
                }

                string recipientName = parts[1];
                string privateMessage = parts[2];

                // پیدا کردن کاربر مقصد
                var recipient = clients.Find(c => c.Name.Equals(recipientName, StringComparison.OrdinalIgnoreCase));
                if (recipient == null)
                {
                    SendPrivateMessage($"کاربر '{recipientName}' یافت نشد.", sender);
                    return;
                }

                // ارسال پیام به مقصد
                string formattedMessage = $"[پیام خصوصی از {sender.Name}]: {privateMessage}";
                SendPrivateMessage(formattedMessage, recipient);

                // اطلاع به فرستنده
                SendPrivateMessage($"[پیام خصوصی به {recipient.Name} ارسال شد]: {privateMessage}", sender);

                AddServerMessage($"{sender.Name} پیام خصوصی به {recipient.Name} ارسال کرد");
            }
            catch (Exception ex)
            {
                AddServerMessage($"خطا در پردازش پیام خصوصی: {ex.Message}");
            }
        }

        private void SendPrivateMessage(string message, ClientInfo recipient)
        {
            try
            {
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                recipient.Client.GetStream().Write(messageBytes, 0, messageBytes.Length);
            }
            catch (Exception ex)
            {
                AddServerMessage($"خطا در ارسال پیام خصوصی به {recipient.Name}: {ex.Message}");
            }
        }

        private void BroadcastMessage(string message, ClientInfo sender)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);


            foreach (var clientInfo in clients.ToArray())
            {
                try
                {
                    if (clientInfo.Client.Connected && clientInfo != sender)
                    {
                        NetworkStream stream = clientInfo.Client.GetStream();
                        stream.Write(messageBytes, 0, messageBytes.Length);
                    }
                }
                catch
                {
                    clients.Remove(clientInfo);
                }
            }
        }
        private void UpdateClientList()
        {
            Dispatcher.Invoke(() =>
            {
                txtListUser.Items.Clear();
                foreach (var clientInfo in clients)
                {
                    txtListUser.Items.Add($"{clientInfo.Name} ({clientInfo.EndPoint})");
                }
            });
        }

        private void AddServerMessage(string message)
        {
            Dispatcher.Invoke(() =>
            {
                txtListMessage.Items.Add($"{DateTime.Now:T} - {message}");
                txtListMessage.ScrollIntoView(txtListMessage.Items[txtListMessage.Items.Count - 1]);
            });
        }
        private void StopServer(object sender, RoutedEventArgs e)
        {
            if (!isRunningServer) return;

            isRunningServer = false;
            try
            {
                // ارسال پیام خداحافظی به همه کلاینت‌ها قبل از قطع ارتباط
                BroadcastMessage("سرور در حال خاموش شدن است...", null);

                // قطع تمام کلاینت‌ها
                foreach (var clientInfo in clients.ToArray())
                {
                    try
                    {
                        if (clientInfo.Client.Connected)
                        {
                            // ارسال پیام قطع ارتباط به هر کلاینت
                            byte[] disconnectMsg = Encoding.UTF8.GetBytes("سرور قطع شد");
                            clientInfo.Client.GetStream().Write(disconnectMsg, 0, disconnectMsg.Length);

                            clientInfo.Client.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        AddServerMessage($"خطا در قطع ارتباط با {clientInfo.Name}: {ex.Message}");
                    }
                }

                clients.Clear();
                server.Stop();

                AddServerMessage("سرور متوقف شد.");
                Dispatcher.Invoke(() => txtListUser.Items.Clear());
            }
            catch (Exception ex)
            {
                AddServerMessage($"خطا در توقف سرور: {ex.Message}");
            }
        }
        public class ClientInfo
        {
            public TcpClient Client { get; set; }
            public string Name { get; set; }
            public string EndPoint => Client.Client.RemoteEndPoint.ToString();
        }

    }
}