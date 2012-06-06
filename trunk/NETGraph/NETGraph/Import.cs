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
           if (_path != null)
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
                EventManagement.startTimer();
                StreamReader _sr = new StreamReader(file);
                //Write Number of Vertexes in Object Graph
                if ((_line = _sr.ReadLine()) != null)
                    _graph.NumberOfVertexes = Int32.Parse(_line);

                for (int i = 0; i < _graph.NumberOfVertexes; i++)
                {
                    // Alle Knoten werden angelegt
                    if ((i % 1000 == 0))
                    {
                        Console.WriteLine(i + " Knoten angelegt");
                    }

                    // Besser direkt einfügen, da Knoten nicht eh vorhanden sind. Viel viel schneller
                    //_graph.Vertexes.Add(new Vertex<string>(i.ToString()));
                    _graph.addVertex(new Vertex<string>(i.ToString()));
                }

                    // Read every line of file
                    while ((_line = _sr.ReadLine()) != null)
                    {
                        String[] _coloumnElements = _line.Split('\t');

                       /* if (_CountColoumnElements > 0 && _coloumnElements.Length != _CountColoumnElements)
                        {
                            throw new NotImplementedException("ERROR:transformFileToGraph");
                        }*/

                        //_CountColoumnElements = _coloumnElements.Length;


                        _data.Add(_line);
                    }
                // Decide the Type of input File Convertion
                int _counter = 0;
                int _counter2 = 0;
                foreach (String data in _data)
                {
                    String[] _coloumnElements = data.Split('\t');
                    _CountColoumnElements = _coloumnElements.Length;
                    
                    switch (_CountColoumnElements)
                    {
                        case 1:
                            String tmp = _data[_counter2].Replace(".", ",");
                            _graph.Vertexes[_counter2].Balance = Convert.ToDouble(tmp);
                            _counter2++;
                            break;
                        //break; //unereichbar wegen exception
                        case 0:
                        case 2:
                        case 3: //TODO: ANDERS ÜBERLEGEN DA SO 3x3 und 2x2 Matrix nicht erkannt wird
                        case 4:
                            if (_counter2 == 0 || _counter2 == 1)
                            {
                                EventManagement.GuiLog("parse file to edgelist");
                                Debug.Print("Kantenliste");
                                _counter2++;
                            }

                            //WENNN ES EINE Liste für eine Bipartiden Graphen ist
                        if (_counter2 == 1 && _graph.Vertexes.Count() != 1)
                        {
                            //sich den ersten zahlenwert wieder aus der Balance holen
                            int numoffirstgroup = Convert.ToInt32(_graph.Vertexes[_counter2 - 1].Balance);
                            
                            //Die erste Gruppe als Source definieren
                            for (int i = 0; i < numoffirstgroup; i++)
                            {
                                //SOURCE
                                _graph.Vertexes[i].Balance = 1;
                            }
                            //Die zweite Gruppe als Target
                            for (int i = numoffirstgroup; i < _graph.Vertexes.Count(); i++)
                            {
                                _graph.Vertexes[i].Balance = -1;
                            }
                            //Counter2 nochmal erhöhen damit nicht mehr in die schleife gesprungen wird....
                            _counter2++;
                        }

                            convertListLine(_coloumnElements, ref _graph);
                            break;
                        default:
                            if (_counter == 0)
                            {
                                EventManagement.GuiLog("parse file to Adjazensmatrix");
                                Debug.Print("Adjazensmatrix");
                            }
                            // test if it is a valid number of Row elements
                            if (_data.Count != _graph.NumberOfVertexes)
                                throw new NotImplementedException("ERROR:transformFileToGraph\n-->Invalid Row Elements!");

                                // test if it is a valid number of columns elements
                            if (_coloumnElements.Length != _graph.NumberOfVertexes)
                                    throw new NotImplementedException("ERROR:transformFileToGraph\n-->Invalid Column Elements!");

                            convertMatrixLine(_counter, _coloumnElements, ref _graph);
                            _counter++;
                            break;
                    }
                }
                EventManagement.stopTimer();
                return _graph;
            }
            catch (Exception ex)
            {
                EventManagement.GuiLog(ex.Message.ToString());
                EventManagement.stopTimer();
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

                case 4:
                    Elements[2] = Elements[2].Replace(".", ",");
                    Elements[3] = Elements[3].Replace(".", ",");
                    _graph.addEdge(new Vertex<string>(Elements[0]), new Vertex<string>(Elements[1]), Convert.ToDouble(Elements[3]), Convert.ToDouble(Elements[2]));
                    break;

                default:
                    Debug.Print("ConvertListLine: dieser Fall dürfte nicht eintreten ;)");
                    break;
            }
        }
       #endregion
   }
}
