using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace NETGraph
{
   static  class Import
   {
       #region functions
       public static void openFileDialog()
        {
            //OpenFileDialog
            string _path = string.Empty;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                _path = openFileDialog1.FileName; 
            // 

            transformFileToGraph(_path);


        }

        private static Graph transformFileToGraph(String file)
        {
            Graph _graph = new Graph();
            String _line;
            int _CountColoumnElements = 0;
            StreamReader _sr = new StreamReader(file);
            List<String> _data = new List<string>();

            //Write Number of Vertexes in Object Graph
            if ((_line = _sr.ReadLine()) != null)
                _graph.NumberOfVertexes = Int32.Parse(_line);
            
            // Read every line of file
            while ((_line = _sr.ReadLine()) != null)
            {
                String[] _coloumnElements = _line.Split('\t');
                if (_CountColoumnElements > 0 && _coloumnElements.Length != _CountColoumnElements)
                {
                    throw new NotImplementedException("ERROR:transformFileToGraph");
                }
                
                _CountColoumnElements = _coloumnElements.Length;


                _data.Add(_line);
            }

            // Decide the Type of input File Convertion
            switch (_CountColoumnElements)
            {
                case 0:
                case 1:
                    throw new NotImplementedException("ERROR:transformFileToGraph");
                case 2: 
                    Debug.Print("Kantenliste");
                    foreach (String data in _data)
                    {
 String[] _Elements = data.Split('\t');
                        
                        convertListLine(_Elements,ref _graph);
                    }
                    break;
                default:
                    Debug.Print("Adjazensmatrix");
                    
                    // test if it is a valid number of Row elements
                    if (_data.Count != _graph.NumberOfVertexes)
                        throw new NotImplementedException("ERROR:transformFileToGraph\n-->Invalid Row Elements!");

                    int _counter = 0;
                    foreach (String data in _data)
                    {
                        String[] _Elements = data.Split('\t');

                        // test if it is a valid number of columns elements
                        if (_Elements.Length != _graph.NumberOfVertexes)
                            throw new NotImplementedException("ERROR:transformFileToGraph\n-->Invalid Column Elements!");
                             
                        convertMatrixLine(_counter,_Elements,ref _graph);
                        _counter++;
                    }                        
                   break;             
            }

            return _graph;
        }

        private static void convertMatrixLine(int counter, string[] _Elements, ref Graph _graph)
        {
            throw new NotImplementedException();
        }

        private static void convertListLine(string[] Elements, ref Graph _graph)
        {
            throw new NotImplementedException();
        }
       #endregion
   }
}
