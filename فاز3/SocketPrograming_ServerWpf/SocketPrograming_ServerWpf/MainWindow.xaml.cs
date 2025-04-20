using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SocketPrograming_ServerWpf
{
    public partial class MainWindow : Window
    {
        private TcpListener server;
        private List<TcpClient> clients = new List<TcpClient>();
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
                    clients.Add(client);

                    // نمایش اطلاعات کلاینت در UI
                    string clientInfo = client.Client.RemoteEndPoint.ToString();
                    AddServerMessage($"کلاینت جدید متصل شد: {clientInfo}");
                    BroadcastMessage($"کاربر جدید به چت پیوست: {clientInfo}", client);


                    // به‌روزرسانی لیست کاربران
                    UpdateClientList(); 

                    // ایجاد یک Thread جداگانه برای هر کلاینت
                    Thread clientThread = new Thread(() => HandleClient(client));
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
        private void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            string clientInfo = client.Client.RemoteEndPoint.ToString();

            try
            {
                while (isRunningServer && client.Connected)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        BroadcastMessage($"سرور قطع است: {clientInfo}", client);
                        break;
                    }
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    string formattedMessage = $"[{clientInfo}]: {message}";
                    AddServerMessage(formattedMessage);

                    // ارسال پیام به همه کلاینت‌ها (شامل خود فرستنده)
                    BroadcastMessage(formattedMessage, null); 
                }
            }
            catch (Exception ex)
            {
                AddServerMessage($"خطا در ارتباط با {clientInfo}: {ex.Message}");
            }
            finally
            {
                client.Close();
                clients.Remove(client);

                AddServerMessage($"کلاینت قطع شد: {clientInfo}");
                BroadcastMessage($"کاربر چت را ترک کرد: {clientInfo}", null);

                UpdateClientList();
            }
        }

        private void BroadcastMessage(string message, TcpClient sender)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            foreach (TcpClient client in clients.ToArray())
            {
                try
                {
                     if (client.Connected)
                    {
                        NetworkStream stream = client.GetStream();
                        stream.Write(messageBytes, 0, messageBytes.Length);
                    }
                }
                catch
                {
                    clients.Remove(client);
                }
            }
        }
        private void UpdateClientList()
        {
            Dispatcher.Invoke(() =>
            {
                txtListUser.Items.Clear();
                foreach (TcpClient client in clients)
                {
                    txtListUser.Items.Add(client.Client.RemoteEndPoint.ToString());
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
                // قطع تمام کلاینت‌ها
                foreach (var client in clients.ToArray())
                {
                    client.Close();
                }
                clients.Clear();

                server.Stop();
                txtListMessage.Items.Add("سرور متوقف شد.\n");
                txtListUser.Items.Clear();
            }
            catch (Exception ex)
            {
                AddServerMessage($"خطا در توقف سرور: {ex.Message}");
            }
        }

     
    }
}