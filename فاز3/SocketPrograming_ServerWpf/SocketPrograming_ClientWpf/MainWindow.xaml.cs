using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;


namespace SocketPrograming_ClientWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TcpClient client;
        private NetworkStream stream;
        private bool isConnected = false;
        public MainWindow()
        {
            InitializeComponent();
            btnSend.IsEnabled = false;
        }
        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client = new TcpClient();
                client.Connect(txtIP.Text, int.Parse(txtPort.Text));
                stream = client.GetStream();
                isConnected = true;

                btnSend.IsEnabled = true;
                btnConnect.IsEnabled = false;
                AddMessage("شما به سرور متصل شدید.");
                // شروع Thread برای دریافت پاسخ‌ها از سرور
                Thread receiveThread = new Thread(ReceiveData);
                receiveThread.IsBackground = true;
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                AddMessage($"خطا در اتصال: {ex.Message}");
            }
        }
        private void ReceiveData()
        {
            byte[] buffer = new byte[1024];

            try
            {
                while (isConnected)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    AddMessage(message);
                }
            }
            catch (Exception)
            {
                AddMessage("ارتباط با سرور قطع شد.");
            }
            finally
            {
                Disconnect();
            }
        }

       
        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            if (!isConnected || string.IsNullOrWhiteSpace(txtMessage.Text))
            {
                AddMessage("ابتدا به سرور متصل شوید و پیام وارد کنید.");
                return;
            }
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(txtMessage.Text);
                stream.Write(data, 0, data.Length);
                txtMessage.Clear();
            }
            catch (Exception ex)
            {
                AddMessage($"خطا در ارسال پیام: {ex.Message}");
                Disconnect();
            }
        }
        //قطع کردن ارتباط
        private void Disconnect()
        {
            try
            {
                isConnected = false;
                stream?.Close();
                client?.Close();
                
                    btnSend.IsEnabled = false;
                    btnConnect.IsEnabled = true;

                }catch { }
        }
        private void AddMessage(string message)
        {
            Dispatcher.Invoke(() =>
            {
                txtChatMessage.Items.Add($"{DateTime.Now:T} - {message}");
                txtChatMessage.ScrollIntoView(txtChatMessage.Items[txtChatMessage.Items.Count - 1]);
            });
        }
        private void TxtMessage_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                BtnSend_Click(sender, e);
            }
        }

        //خروج از برنامه
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Disconnect();
            Close();
        }
    }
}