using Graph.Vertex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using templateGraph;

namespace Graph.Serialization {
    [Serializable]
    class GraphData {

        List<List<string>> myList = new List<List<string>>();


        private Dictionary<string, Pos> buttons = new Dictionary<string, Pos>();

        public GraphData(List<Relation> relations, List<Button> vertices, Visual visual, double width,
            double height) {

            MakeList(relations);


            VertexPosManager vs = new VertexPosManager(visual, width, height);

            foreach (var but in vertices) {
                buttons[but.Content.ToString()] = vs.GetLocation(but);

            }
          
        }
        private void MakeList(List<Relation> relations) {
            myList = new List<List<string>>();


            foreach (var relation in relations) {
                List<string> listToAdd = new List<string>();
                listToAdd.Add(relation.GetStartButton().Content.ToString());
                listToAdd.Add(relation.GetEndButton().Content.ToString());
                if (relation.linkType == Relation.LinkType.CurvedArrow) {
                    listToAdd.Add("2");
                }
                else {
                    listToAdd.Add("1");
                }

                listToAdd.Add(relation.getTransition().ToString());
                myList.Add(listToAdd);
            }
        }

        public Dictionary<string, Pos> getButtons() {
            return this.buttons;
        }




        public List<List<string>> getMyList() {
            return this.myList;
        }




    }
}
