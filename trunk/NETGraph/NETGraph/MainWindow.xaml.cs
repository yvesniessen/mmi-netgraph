using System;
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
using System.Windows.Forms;
using System.IO;

namespace NETGraph
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {



        public MainWindow()
        {
            InitializeComponent();
            tbx_filePath.Text = Environment.SpecialFolder.Desktop.ToString();
            tbx_out.Text = "Welcome to .NETGraph 0.1 \n";

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Import.openFileDialog();
        }

       

        private void button2_Click(object sender, RoutedEventArgs e)
        {

            String s1 = "0,9 0,13 1,4 1,8 1,14 2,7 2,11 2,12 3,5 3,6 3,9 4,7 5,10 6,9 7,12 8,14 8,14";
            String[] temp = s1.Split(' ');

            Graph g = new Graph();

            g.DirectedEdges = false;
            g.ParallelEdges = false;

            int i=0;
            while(i<15)
            {
                g.addVertex(new Vertex<String>(i.ToString()));
                i++;
            }

            foreach (String s in temp)
            {
                Vertex<String> p1 = new Vertex<String>(s.Split(',')[0]);
                Vertex<String> p2 = new Vertex<String>(s.Split(',')[1]);

                g.addEdge(p1, p2);
            }

            foreach (Edge edge in g.getEdges())
            {
                tbx_out.Text += edge.ToString() + "\n";
            }
            tbx_out.Text += g.getVertexes().Count.ToString() + "\n";
            tbx_out.Text += "Knoten-Kollisionen: " + g.CollisionOfVertexes.ToString() + "\n";
            tbx_out.Text += "Kanten-Kollisionen: " + g.CollisionOfEdges.ToString() + "\n";
        }
    }
}
