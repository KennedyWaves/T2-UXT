﻿using System;
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
using System.Windows.Forms;
using System.IO;
using Lades.WebTracer;

namespace TraceConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        List<Node> allNodes = new List<Node>();
        string finalFile = "TIPO;TEMPO;X;Y;URL\n";
        string click = "";
        string move = "";
        string scroll = "";
        string freeze = "";
        string eye = "";

        public static List<Node> ordenadorTime(List<Node> source)
        {
            Node major = new Node();
            int pos = -1;
            for (int y = source.Count - 1; y > -1; y--)
            {
                major.Time = -3;
                for (int x = 0; x <= y; x++)
                {
                    if (major.Time < source[x].Time)
                    {

                        major = source[x].Copy();
                        pos = x;
                    }
                }
                Node heat = source[y].Copy();
                source[y] = major.Copy();
                source[pos] = heat.Copy();
            }
            //foreach (HeatPoint point in source) result.Insert(0, point);
            return source;
        }

        private void Cmd_convert_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    allNodes = Node.LoadNodes(fbd.SelectedPath, true);
                    List<Node> ordenados = ordenadorTime(allNodes);
                    allNodes = ordenados;
                    foreach(Node node in allNodes)
                    {
                        string line = node.Type + ";" + node.Time + ";" + node.X + ";" + node.Y + ";" + node.Url + "\n";
                        Txt_log.Text += line;
                        if (node.Type == "click")
                        {
                            click += line;
                        }
                        if (node.Type == "move")
                        {
                            move += line;
                        }
                        if (node.Type == "wheel")
                        {
                            scroll += line;
                        }
                        if (node.Type == "freeze")
                        {
                            freeze += line;
                        }
                        if (node.Type == "eye")
                        {
                            eye += line;
                        }
                    }
                    finalFile += click + move + scroll + freeze + eye;
                    Txt_log.Text += "Writing "+ fbd.SelectedPath + "\\trace.csv...";
                    System.IO.File.WriteAllText(fbd.SelectedPath + "\\trace.csv", finalFile);
                    Txt_log.Text += "_________DONE______________";
                }
            }
        }

    }
}
