﻿using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Media.Effects;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using Twitterizer;
namespace o3o
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
            MouseDown += delegate { if (MouseButtonState.Pressed == System.Windows.Input.Mouse.LeftButton) { DragMove(); } };
            this.Loaded += new RoutedEventHandler(Window1_Loaded);  
            
        }

        TweetStack o3o;
        void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            this.SetAeroGlass();
            if (String.IsNullOrEmpty(Properties.Settings.Default.OAuth_AccessToken))
            {
                 o3o = new TweetStack(false);            
                o3o.OAuth.AuthenticateTwitter();
                o3o.OAuth.SaveOAuth();
            }
            else
            {
                 o3o = new TweetStack(true);
            }

            if (Properties.Settings.Default.use_system_color == true)
            {
                checkBox1.IsChecked = true;
            }

            get_mentions();
            get_tweets();

           
        }

        [DllImport("dwmapi.dll", EntryPoint = "#127", PreserveSig = false)]
        public static extern void DwmGetColorizationParameters(out WDM_COLORIZATION_PARAMS parameters);
        public struct WDM_COLORIZATION_PARAMS
        {
            public uint Color1;
            public uint Color2;
            public uint Intensity;
            public uint Unknown1;
            public uint Unknown2;
            public uint Unknown3;
            public uint Opaque;
        }


        void get_tweets()
        {
            TweetElements.Items.Clear();
            Twitterizer.TwitterStatusCollection response = o3o.Twitter.GetTweets();
            foreach (Twitterizer.TwitterStatus tweet in response)
            {
                FillHome(tweet.Text, tweet.User.ScreenName, tweet.CreatedDate.ToString(), tweet.User.ProfileImageLocation, tweet.Id.ToString());
              
            }
            int index = response.Count - 1;
            Notification(response[index].Text, response[index].User.ScreenName, response[index].CreatedDate.ToString(), response[index].User.ProfileImageLocation, response[index].Id.ToString());
            
        }
        void get_mentions()
        {
            Twitterizer.TwitterStatusCollection menstruations = o3o.Twitter.GetMentions();

            TweetMentions.Items.Clear();
            foreach (Twitterizer.TwitterStatus drama in menstruations)
            {
                FillMentions(drama.Text, drama.User.ScreenName, drama.CreatedDate.ToString(), drama.User.ProfileImageLocation, drama.Id.ToString());
            }

        }

        private void testbutton_Click(object sender, RoutedEventArgs e)
        {
            if (textBox1.Text.Length <= 140)
            {
                if (!String.IsNullOrEmpty(textBox1.Text))
                {
                    o3o.Twitter.SendTweet(textBox1.Text);
                    textBox1.Text = "";
                    charleft.Text = "140";
                    get_tweets();
                }
                else
                {
                    charleft.Foreground = new SolidColorBrush(Colors.Red); 
                    charleft.Text = "no text";
                }
            }
            else
            {
                charleft.Foreground = new SolidColorBrush(Colors.Red); 
                charleft.Text = "too long";
            }
        }

        public void FillHome(string message, string user, string date, string url, string id) // image is fetched in Tweetelement.xaml.cs
        {
            TweetElement element = new TweetElement();
            element.Tweet = message;
            element.name = user;
            element.Date = date;
            element.imagelocation = url;
            element.ID = id;
            TweetElements.Items.Add(element);
        }

        public void FillMentions(string message, string user, string date, string url, string id) // image is fetched in Tweetelement.xaml.cs
        {
            TweetElement element = new TweetElement();
            element.Tweet = message;
            element.name = user;
            element.Date = date;
            element.imagelocation = url;
            element.ID = id;
            TweetMentions.Items.Add( element);
        }

        public void Notification(string message, string user, string date, string url, string id)
        {
            notify notification = new notify();
            TweetElement element = new TweetElement();
            element.Tweet = message;
            element.name = user;
            element.Date = date;
            element.imagelocation = url;
            element.ID = id;
            notification.content.Items.Add(element);
            
        }

        private void closebutton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void minimisebutton_Click(object sender, RoutedEventArgs e)
        {
             WindowState = WindowState.Minimized;
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            int left = 140 - (textBox1.Text.Length);
            charleft.Text = left.ToString();
            if (left < 0)
                charleft.Foreground = new SolidColorBrush(Colors.Red); 
            else
                charleft.Foreground = new SolidColorBrush(Colors.Black); 
        }

        private void btn_right_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            btn_right.Source = new BitmapImage(new Uri("/o3o;component/Images/rightarrow_over.png", UriKind.Relative));
        }

        private void btn_right_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            btn_right.Source = new BitmapImage(new Uri("/o3o;component/Images/rightarrow.png", UriKind.Relative));
        }

        private void btn_right_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void btn_Left_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            btn_Left.Source = new BitmapImage(new Uri("/o3o;component/Images/leftarrow_over.png", UriKind.Relative));
        }

        private void btn_Left_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            btn_Left.Source = new BitmapImage(new Uri("/o3o;component/Images/leftarrow.png", UriKind.Relative));
        }

        private void btn_Left_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AboutBox1 about = new AboutBox1();
            about.Show();
        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.use_system_color = true;
            Properties.Settings.Default.Save();
        }

        private void checkBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.use_system_color = false;
            Properties.Settings.Default.Save();
        }

        private void colorpicker_SelectedColorChanged(Color obj)
        {
            Properties.Settings.Default.system_color = System.Drawing.Color.FromArgb(obj.A,obj.R,obj.G,obj.B);
            Properties.Settings.Default.Save();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            get_mentions();
            get_tweets();

        }

        //private void reload_MouseDown(object sender, MouseButtonEventArgs e)
        //{

        //}

        //private void reload_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    reload.Source = new BitmapImage(new Uri("/o3o;component/Images/Reload_normal.png", UriKind.Relative));
        //}

        //private void reload_MouseEnter(object sender, MouseEventArgs e)
        //{

        //    reload.Source = new BitmapImage(new Uri("/o3o;component/Images/Reload_Hover.png", UriKind.Relative));
        //}
        


     

    }


    public class ImageButton : System.Windows.Controls.Button
    {
        public ImageSource Source
        {
            get { return base.GetValue(SourceProperty) as ImageSource; }
            set { base.SetValue(SourceProperty, value); }
        }
        public static readonly DependencyProperty SourceProperty =
          DependencyProperty.Register("Source", typeof(ImageSource), typeof(ImageButton));
    }
}
