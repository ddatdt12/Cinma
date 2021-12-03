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
using System.Windows.Shapes;

namespace CinemaManagement.Views
{
    /// <summary>
    /// Interaction logic for MessageBoxCustom.xaml
    /// </summary>
    public partial class MessageBoxCustom : Window
    {
        public MessageBoxCustom(string Title,string Message, MessageType Type, MessageButtons Buttons)
        {
            InitializeComponent();
            txtMessage.Text = Message;
            if(txtMessage.Text.Length > 25)
            txtMessage.Margin = new Thickness(15, 5, 5, 5);
            if (txtMessage.Text.Length > 50)
            {
                txtMessage.Margin = new Thickness(4, 10, 5, 5);
                txtMessage.FontSize = 15;
                txtMessage.Width = 255;
                txtMessage.Height = 45;
                ImgMessage.Margin = new Thickness(0, 0, 0, 5);
            }
            txtTitle.Text = Title;
            switch (Type)
            {

                case MessageType.Info:
                    ChangeBackGround((Color)ColorConverter.ConvertFromString("#FF2196F3"));
                    ImgMessage.Source = new BitmapImage(new Uri("pack://application:,,,/CinemaManagement;component/Resources/Icon/info.png"));
                    break;
                case MessageType.Success:
                    ChangeBackGround((Color)ColorConverter.ConvertFromString("#FF4CAF50"));
                    ImgMessage.Source = new BitmapImage(new Uri("pack://application:,,,/CinemaManagement;component/Resources/Icon/succes.png"));
                    break;
                case MessageType.Warning:
                    ChangeBackGround((Color)ColorConverter.ConvertFromString("#FFF3BA0E"));
                    ImgMessage.Source = new BitmapImage(new Uri("pack://application:,,,/CinemaManagement;component/Resources/Icon/warning.png"));
                    break;
                case MessageType.Error:
                    ChangeBackGround((Color)ColorConverter.ConvertFromString("#FFED4538"));
                    ImgMessage.Source = new BitmapImage(new Uri("pack://application:,,,/CinemaManagement;component/Resources/Icon/ErrorIcon.png"));
                    break;
            }
            switch (Buttons)
            {
                case MessageButtons.OKCancel:
                    btnYes.Visibility = Visibility.Collapsed; btnNo.Visibility = Visibility.Collapsed;
                    break;
                case MessageButtons.YesNo:
                    btnOk.Visibility = Visibility.Collapsed; btnCancel.Visibility = Visibility.Collapsed;
                    break;
                case MessageButtons.OK:
                    btnOk.Visibility = Visibility.Visible;
                    btnCancel.Visibility = Visibility.Collapsed;
                    btnYes.Visibility = Visibility.Collapsed; btnNo.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        public void ChangeBackGround(Color newcolor)
        {
            btnYes.Background = new SolidColorBrush(newcolor);
            btnOk.Background = new SolidColorBrush(newcolor);
            btnNo.Background = new SolidColorBrush(newcolor);
            btnCancel.Background = new SolidColorBrush(newcolor);
            BackGroundTittle.Background = new SolidColorBrush(newcolor);
            btnClose.Foreground = new SolidColorBrush(newcolor);
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
    public enum MessageButtons
    {
        OKCancel,
        YesNo,
        OK,
    }
    public enum MessageType
    {
        Info,
        Success,
        Warning,
        Error,
    }
}
