﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Fagprojekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        CanHandler can;

        bool running = true;

        private ViewModels.MainWindowModel viewModel;

        HeatMap heatMap;

        public MainWindow()
        {

            viewModel = new ViewModels.MainWindowModel();
            DataContext = viewModel;


            InitializeComponent();
          
            Closing += stop;
        }


        private void printData()
        {
            
            while (running)
            {
                if (can.updateDict())
                {

                    string temp = can.ToString();

                    
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        textBlock.Text = temp;
                    }));
                    
                }


                System.Threading.Thread.Sleep(100);
            }
        }

        private void stop(object sender, EventArgs e)
        {
            if (can != null)
            {
                can.stop();
            }
            running = false;
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            can = new CanHandler(textBox.Text,6666);

            Thread newThread = new Thread(printData);

            newThread.Start();

            can.start();


        }


        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (heatMap == null)
            {
                

                    heatMap = new HeatMap(can.Dict);
                    heatMap.Closed += HeatMap_Closed;
                    heatMap.Owner = this;
                    heatMap.Show();
                    heatMap.UpdateHeatmap();



            }
            else
            {
                heatMap.UpdateHeatmap();
            }


        }
        

        private void HeatMap_Closed(object sender, EventArgs e)
        {
            heatMap = null;
        }
    }


}
