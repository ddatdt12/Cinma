using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CinemaManagement.Views
{
    public partial class MessageBoxCustom : Window
    {
        public MessageBoxCustom(string Title,string Message, MessageType Type, MessageButtons Buttons)
        {
            InitializeComponent();
            txtMessage.Text = Message;
            if(txtMessage.Text.Length > 27)
            txtMessage.Margin = new Thickness(15, 5, 5, 5);
            if (txtMessage.Text.Length > 54)
            {
                txtMessage.Margin = new Thickness(4, 10, 5, 5);
                txtMessage.FontSize = 16;
                txtMessage.Width = 255;
                txtMessage.Height = 55;
                ImgMessage.Margin = new Thickness(0, 0, 0, 5);
            }
            txtTitle.Text = Title;
            switch (Type)
            {

                case MessageType.Info:
                    System.Media.SystemSounds.Beep.Play();
                    ChangeBackGround((Color)ColorConverter.ConvertFromString("#FF2196F3"));
                    ImgMessage.Source = new BitmapImage(new Uri("pack://application:,,,/CinemaManagement;component/Resources/Icon/info.png"));
                    break;
                case MessageType.Success:
                    System.Media.SystemSounds.Beep.Play();
                    ChangeBackGround((Color)ColorConverter.ConvertFromString("#FF4CAF50"));
                    ImgMessage.Source = new BitmapImage(new Uri("pack://application:,,,/CinemaManagement;component/Resources/Icon/succes.png"));
                    break;
                case MessageType.Warning:
                    System.Media.SystemSounds.Beep.Play();
                    ChangeBackGround((Color)ColorConverter.ConvertFromString("#FFF3BA0E"));
                    ImgMessage.Source = new BitmapImage(new Uri("pack://application:,,,/CinemaManagement;component/Resources/Icon/warning.png"));
                    break;
                case MessageType.Error:
                    System.Media.SystemSounds.Hand.Play();
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
            BackGroundTittle.Background = new SolidColorBrush(newcolor);
            btnOk.Background = new SolidColorBrush(newcolor);
            btnYes.Background = new SolidColorBrush(newcolor);
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
