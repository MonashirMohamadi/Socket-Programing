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
        private string userName;
        public MainWindow()
        {
            InitializeComponent();
            btnSend.IsEnabled = false;
        }
        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // دریافت نام کاربر
                userName = Microsoft.VisualBasic.Interaction.InputBox("لطفا نام خود را وارد کنید:", "نام کاربری", "کاربر");
                if (string.IsNullOrWhiteSpace(userName))
                {
                    AddMessage("برای اتصال باید نامی وارد کنید.");
                    return;
                }
                client = new TcpClient();
                client.Connect(txtIP.Text, int.Parse(txtPort.Text));
                stream = client.GetStream();
                isConnected = true;
                btnPrivate.IsEnabled = true;

                // ارسال نام به سرور
                byte[] nameData = Encoding.UTF8.GetBytes(userName);
                stream.Write(nameData, 0, nameData.Length);

                btnSend.IsEnabled = true;
                btnConnect.IsEnabled = false;
                AddMessage($"شما به سرور متصل شدید (به نام: {userName}).");

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
                    // اگر پیام خصوصی است با رنگ متفاوت نمایش داده شود
                    if (message.Contains("[پیام خصوصی"))
                    {
                        Dispatcher.Invoke(() =>
                        {
                            var item = new ListBoxItem
                            {
                                Content = $"{DateTime.Now:T} - {message}",
                                Foreground = Brushes.Red,
                                FontWeight = FontWeights.Bold
                            };
                            txtChatMessage.Items.Add(item);
                            txtChatMessage.ScrollIntoView(item);
                        });
                    }
                    else
                    {
                        AddMessage(message);
                    }
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
                string fullMessage = $"{userName}: {txtMessage.Text}";
                byte[] data = Encoding.UTF8.GetBytes(txtMessage.Text);
                stream.Write(data, 0, data.Length);
                AddMessage($"شما: {txtMessage.Text}");
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
                btnPrivate.IsEnabled = false;
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

        private void BtnPrivate_Click(object sender, RoutedEventArgs e)
        {
            if (!isConnected)
            {
                AddMessage("ابتدا به سرور متصل شوید.");
                return;
            }

            // دریافت نام کاربر مقصد
            string recipient = Microsoft.VisualBasic.Interaction.InputBox("نام کاربری که می‌خواهید به او پیام بفرستید:", "پیام خصوصی", "");
            if (string.IsNullOrWhiteSpace(recipient))
            {
                AddMessage("ارسال پیام خصوصی لغو شد.");
                return;
            }

            // دریافت متن پیام
            string message = Microsoft.VisualBasic.Interaction.InputBox("متن پیام خصوصی:", "پیام خصوصی", "");
            if (string.IsNullOrWhiteSpace(message))
            {
                AddMessage("ارسال پیام خصوصی لغو شد.");
                return;
            }

            try
            {
                string privateMessage = $"/private {recipient} {message}";
                byte[] data = Encoding.UTF8.GetBytes(privateMessage);
                stream.Write(data, 0, data.Length);
                AddMessage($"شما به {recipient}: {message} (پیام خصوصی)");
            }
            catch (Exception ex)
            {
                AddMessage($"خطا در ارسال پیام خصوصی: {ex.Message}");
            }
        }
    }
}