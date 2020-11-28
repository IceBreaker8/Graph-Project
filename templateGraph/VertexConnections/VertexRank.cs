
using templateGraph.InputOutputWindow;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace templateGraph.VertexConnections {
    class VertexRank {

        private List<Button> buttons;
        private List<Relation> relations;
        public Dictionary<Button, int> Ranks = new Dictionary<Button, int>();
        public VertexRank(List<Button> buttons, List<Relation> relations) {
            this.buttons = new List<Button>(buttons);
            this.relations = relations;
            SearchForRanks();
        }

        

        public Dictionary<Button, int> getRanks() {
            return Ranks;
        }

        public void SearchForRanks() {
            List<Button> ToRemove = new List<Button>();
            int Rank = 0;
            while (buttons.Count != 0) {
                int Min = int.MaxValue;
                foreach (Button B in buttons) {
                    if (PredNumber(B) < Min) {
                        Min = PredNumber(B);
                        ToRemove.Clear();
                        ToRemove.Add(B);
                    }
                    else if (PredNumber(B) == Min) {
                        ToRemove.Add(B);
                    }
                }
                foreach (Button B in ToRemove) {
                    Ranks[B] = Rank;
                }
                RemoveButtons(ToRemove);
                ToRemove.Clear();
                Rank++;
            }
        }

        private void RemoveButtons(List<Button> ToRemove) {
            foreach (Button B in ToRemove) {
                buttons.Remove(B);
            }
        }


        private int PredNumber(Button button) {
            int count = 0;
            foreach (Relation R in relations)
                if (R.ConEnd == button && buttons.Contains(R.ConStart))
                    count++;
            return count;
        }



        private OutputText Output = new OutputText("Vertex Ranks");
        public void DisplayRanks() {
            Dictionary<int, List<Button>> d = new Dictionary<int, List<Button>>();
            foreach (var item in Ranks) {
                if (d.ContainsKey(item.Value)) {
                    d[item.Value].Add(item.Key);
                }
                else {
                    d.Add(item.Value, new List<Button>());
                    d[item.Value].Add(item.Key);
                }
            }
            foreach (var item in d) {
                String text = "" + item.Key;
                foreach (Button B in item.Value) {
                    text = "rank(" + B.Content.ToString() + ")=" + text;
                }
                Output.AddText(text + "\n", FontWeights.Normal, 16);
            }
            Output.Print();

        }


        private Button GetSmallerButton(List<Button> newButtonList) {
            int min = int.MaxValue;
            Button b = new Button();
            foreach (var item in Ranks) {
                if (item.Value < min && !newButtonList.Contains(item.Key)) {
                    b = item.Key;
                    min = item.Value;
                }
            }
            return b;


        }
        public void SortVerticesByRanks(ref List<Button> buttons) {
            List<Button> newButtonList = new List<Button>();
            while (newButtonList.Count < buttons.Count) {
                newButtonList.Add(GetSmallerButton(newButtonList));
            }
            buttons = newButtonList;
        }

        public int RankNumber() {
            int max = 0;
            foreach (var item in Ranks) {
                if (item.Value > max) {
                    max = item.Value;
                }
            }
            return max + 1;
        }





    }
}
