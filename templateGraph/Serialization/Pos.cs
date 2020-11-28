using System;

namespace Graph.Serialization {
    [Serializable]
    class Pos {


        public double a;
        public double b;
        public double c;
        public double d;


        public Pos(double a, double b, double c, double d) {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;

        }
    }
}