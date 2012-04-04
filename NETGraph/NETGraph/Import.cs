using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace NETGraph
{
    static class Import
   {

   /*    #region Events
       // Gui-Log-Text Event handling
       public  delegate void GuiLogText( String logText);
       // Define an Event based on the above Delegate
       public static event GuiLogText GuiLogTextEvent;// = delegate {};
       // This function adds a string to the Gui-RichText-Log
       public static void OnGuiLogTextEvent( String logText)
       {
           if (GuiLogTextEvent != null)
           {
               GuiLogTextEvent( logText);
           }
       }
       #endregion
*/
       #region functions
       public static Graph openFileDialog()
        {
           
            //OpenFileDialog
            string _path = string.Empty;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                _path = openFileDialog1.FileName;
            EventManagement.GuiLog("open file: " + _path);
           // Prevent "Cancel-Button" Error 
           if (transformFileToGraph(_path) != null)
                return transformFileToGraph(_path);
            else
               return null;
        }

        private static Graph transformFileToGraph(String file)
        {
            if (file == null)
            {
                EventManagement.writeIntoLogFile("private static Graph transformFileToGraph(String file) --> File Handle Errror");
                throw new NotImplementedException("ERROR:transformFileToGraph");
            }


            Graph _graph = new Graph();
            String _line;
            int _CountColoumnElements = 0;          
            List<String> _data = new List<string>();


            try
            {
                StreamReader _sr = new StreamReader(file);
                //Write Number of Vertexes in Object Graph
                if ((_line = _sr.ReadLine()) != null)
                    _graph.NumberOfVertexes = Int32.Parse(_line);

                for (int i = 0; i < _graph.NumberOfVertexes; i++)
                {
                    _graph.addVertex(new Vertex<string>(i.ToString()));
                }

                    // Read every line of file
                    while ((_line = _sr.ReadLine()) != null)
                    {
                        String[] _coloumnElements = _line.Split('\t');

                        if (_CountColoumnElements > 0 && _coloumnElements.Length != _CountColoumnElements)
                        {
                            throw new NotImplementedException("ERROR:transformFileToGraph");
                        }

                        //_CountColoumnElements = _coloumnElements.Length;


                        _data.Add(_line);
                    }

                // Decide the Type of input File Convertion
                switch (_CountColoumnElements)
                {
                    case 1:
                        EventManagement.GuiLog("ERROR:transformFileToGraph");
                        EventManagement.writeIntoLogFile("ERROR:transformFileToGraph");
                        throw new NotImplementedException("ERROR:transformFileToGraph");
                        //break; //unereichbar wegen exception
                    case 0:
                    case 2:
                    case 3: //TODO: ANDERS ÜBERLEGEN DA SO 3x3 und 2x2 Matrix nicht erkannt wird
                        EventManagement.GuiLog("parse file to edgelist");
                        //Debug.Print("Kantenliste");
                        foreach (String data in _data)
                        {
                            String[] _Elements = data.Split('\t');   
                            convertListLine(_Elements, ref _graph);   
                        }
                        break;
                    default:
                        EventManagement.GuiLog("parse file to Adjazensmatrix");
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

                            convertMatrixLine(_counter, _Elements, ref _graph);
                            _counter++;
                        }
                        break;
                }

                return _graph;
            }
            catch (Exception ex)
            {
                EventManagement.GuiLog(ex.Message.ToString());
                return null;
            }
        }

        //counter == Zeile in der ich mich befinde, startend bei 0
        private static void convertMatrixLine(int counter, string[] _Elements, ref Graph _graph)
        {
            //Name Counter dient zum mitzählen der Spalten um später den Vertex korrekt zu benennen
            int nameCounter = 0;

            foreach (String vertex in _Elements)
            {
                //Wenn der Knoten ungleich "0" ist füge an dieser Stelle eine Kante hinzu
                if(!vertex.Equals("0"))
                {
                    //_graph.addEdge(new Vertex<string>(counter.ToString()), new Vertex<string>(nameCounter.ToString()));
                    _graph.addEdge(new Vertex<string>(counter.ToString()), new Vertex<string>(nameCounter.ToString()), Convert.ToDouble(vertex));
                }
                nameCounter++;
            }
        }

        private static void convertListLine(string[] Elements, ref Graph _graph)
        {
            switch (Elements.Count())
            {
                case 2:
                    _graph.addEdge(new Vertex<string>(Elements[0]), new Vertex<string>(Elements[1]));
                    break;
                case 3:

                    //Wenn kosten im Format 1.5 dann zu Format 1,5 wandeln für Convert.toString
                    Elements[2] = Elements[2].Replace(".", ",");

                    //Hier wäre Double.tryParse eher angebracht
                    _graph.addEdge(new Vertex<string>(Elements[0]), new Vertex<string>(Elements[1]), Convert.ToDouble(Elements[2]));
                    break;
                default:
                    Debug.Print("ConvertListLine: dieser Fall dürfte nicht eintreten ;)");
                    break;
            }
        }
       #endregion
   }
}
