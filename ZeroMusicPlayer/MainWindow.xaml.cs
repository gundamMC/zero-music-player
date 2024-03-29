﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace ZeroMusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ItemControl SelectedItemControl = null;
        public void SetSelectedItemControl(ItemControl control)
        {
            if (SelectedItemControl != null)
                SelectedItemControl.Selected = false;
            control.Selected = true;

            SelectedItemControl = control;
        }

        // Pages
        private FilePage filePage = new FilePage();

        public MainWindow()
        {
            InitializeComponent();
            DwmDropShadow.DropShadowToWindow(this);

            if (String.IsNullOrWhiteSpace(Properties.Settings.Default.DirectoryPath) || 
                !Directory.Exists(Properties.Settings.Default.DirectoryPath))
            {
                new PathMessageBox().ShowDialog();
            }

            // default file page
            ContentFrame.Navigate(filePage);

            App.Player = new MusicPlayer(Queue_Panel, History_Panel);
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItemControl is FolderItemControl)
            {
                ((FolderItemControl)SelectedItemControl).Host.Explore(((FolderItemControl)SelectedItemControl).Dir);
                return;
            }

            switch (App.Player.State())
            {
                case 0:
                    App.Player.Pause();
                    break;
                case 1:
                    App.Player.Resume();
                    break;
                case -1:
                    App.Player.PlayNow(new SongItem() { Name = SelectedItemControl.ItemName, Path = ((SongItemControl)SelectedItemControl).Path, Time = ((SongItemControl)SelectedItemControl).Time });
                    break;
            }

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) {
            App.Player.Dispose();
            App.Current.Shutdown();
        }

        private void QueueClose_Click(object sender, RoutedEventArgs e)
        {
            QueueGrid.Visibility = Visibility.Hidden;
        }

        private void Queue_Button_Click(object sender, RoutedEventArgs e)
        {
            QueueGrid.Visibility = (QueueGrid.Visibility == Visibility.Visible) ? Visibility.Hidden : Visibility.Visible;
        }

        private void Queue_Tab_Button_Click(object sender, RoutedEventArgs e)
        {
            Queue_TabControl.SelectedIndex = 0;
        }

        private void History_Tab_Button_Click(object sender, RoutedEventArgs e)
        {
            Queue_TabControl.SelectedIndex = 1;
        }
    }
}
