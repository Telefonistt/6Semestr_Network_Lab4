using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace ChatClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,ServiceChat.IServiceChatCallback
    {
        bool isConnected = false;
        ServiceChat.ServiceChatClient client;
        int ID;
        public MainWindow()
        {
            InitializeComponent();
        }


        void ConnectUser()
        {
            if(!isConnected)
            {
                client = new ServiceChat.ServiceChatClient(new System.ServiceModel.InstanceContext(this));
                ID =client.Connect(nicknameTextBox.Text);
                nicknameTextBox.IsEnabled = false;
                connectButton.Content = "Disconnect";
                isConnected = true;
            }
        }

        void DisconnectUser()
        {
            if (isConnected)
            {
                client.Disconnect(ID);
                client = null;
                nicknameTextBox.IsEnabled = true;
                connectButton.Content = "Connect";
                isConnected = false;
            }
        }

        void SendMesg()
        {
            if (client != null)
            {
                client.SendMsg(messageTextBox.Text, ID);
                messageTextBox.Text = string.Empty;
            }
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            if(isConnected)
            {
                DisconnectUser();
            }
            else
            {
                ConnectUser();
            }
        }

        public void MsgCallback(string msg)
        {
            chatListBox.Items.Add(msg);
            chatListBox.ScrollIntoView(chatListBox.Items[chatListBox.Items.Count]);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisconnectUser();
        }

        private void messageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
            {
                SendMesg();
                
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            SendMesg();
        }
    }
}
