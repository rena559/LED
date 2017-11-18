using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ThingM.Blink1;

namespace LED
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Blink1 m_blink1;
        private const string s_title = "LED";

        public MainWindow()
        {
            InitializeComponent();

            var connected = TryToConnect(out m_blink1);
            UpdateTitle(connected);
            Disconnect.IsEnabled = connected;
            Connect.IsEnabled = !connected;
        }

        private void RedButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((bool)FadeCheckBox.IsChecked)
                    m_blink1?.FadeToColor((Convert.ToUInt16(ComboBox.SelectionBoxItem)), 255, 0, 0, false);
                else
                    m_blink1?.SetColor(255, 0, 0);
            }
            catch(Exception exc)
            {
                MessageBox.Show("Something went wrong: \n" + exc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void GreenButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((bool)FadeCheckBox.IsChecked)
                m_blink1?.FadeToColor((Convert.ToUInt16(ComboBox.SelectionBoxItem)), 0, 255, 0, false);
            else
                m_blink1?.SetColor(0, 255, 0);nfgvbnghn
            }
            catch (Exception exc)
            {
                MessageBox.Show("Something went wrong: \n" + exc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void BlueButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((bool)FadeCheckBox.IsChecked)
                m_blink1?.FadeToColor((Convert.ToUInt16(ComboBox.SelectionBoxItem)), 0, 0, 255, false);
            else
                m_blink1?.SetColor(0, 0, 255);
        }
            catch(Exception exc)
            {
                MessageBox.Show("Something went wrong: \n" + exc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
}

        private bool TryToConnect(out Blink1 blink1)
        {
            blink1 = new Blink1();
            List<string> devicePaths = Blink1Info.GetDevicePath();

            if (devicePaths.Count == 0)
            {
                MessageBox.Show("Connection not possible - no device found!", "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                blink1 = null;
                return false;
            }

            blink1.Open(devicePaths.First());
            if (!blink1.IsConnected)
                blink1 = null;
            return blink1.IsConnected;
        }

        private void UpdateTitle(bool connected)
        {
            if (connected)
                Title = s_title + " - Connected";
            else
                Title = s_title + " - Disconnected";
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(m_blink1 != null && m_blink1.IsConnected)
                m_blink1?.Close();
        }

        private void FadeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ComboBox.IsEnabled = (bool)FadeCheckBox.IsChecked;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var connected = TryToConnect(out m_blink1);
            UpdateTitle(connected);

            if (!connected)
                return;

            Disconnect.IsEnabled = true;
            Connect.IsEnabled = false;
        }

        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {
            m_blink1?.Close();
            UpdateTitle(false);
            Disconnect.IsEnabled = false;
            Connect.IsEnabled = true;
        }
    }
}
