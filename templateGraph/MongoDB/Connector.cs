using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using templateGraph;

namespace Graph.MongoDB {
    class Connector {

        public enum DataType {
            MPM,
            appOpen,
            bellman,
            bellmanAme,
            floyd,
            dijkstra,
            showByRank
        }
        /*As of today's version of MongoDB (v2.0.1.27 for MongoDB.Driver), 
        there's no need to close or dispose of connections. The client handles it automatically.
        */

        public static string DatabaseName = "GraphIceDB";
        public static string CollectionName = "Collection1";

        public static MongoClient Client;
        public static IMongoDatabase Database;
        public static IMongoCollection<BsonDocument> Collection;
        public static BsonDocument FirstDocument;

        public static bool IsConnected = false;

        public static void EstablishConnection() {
            try {
                Client = new MongoClient("mongodb+srv://GraphICE:wd9c1PwaGldGYI6K@graphcluster.f67ke.mongodb.net/test");
                Database = Client.GetDatabase(DatabaseName);
                Collection = Database.GetCollection<BsonDocument>(CollectionName);
                IsConnected = true;
                Connector.IncrementData(Connector.DataType.appOpen);
            } catch (Exception) {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                    
                }));
            }
        }

        public static void IncrementData(DataType dataType) {
            if (!IsConnected)
                return;
            if (MainWindow.TestActive)
                return;
            FirstDocument = Collection.Find(new BsonDocument()).FirstOrDefault();
            var filter = Builders<BsonDocument>.Filter.Eq("graphApp", "GraphICE");
            var update = Builders<BsonDocument>.Update.Set(dataType.ToString(),
                Int32.Parse(FirstDocument[dataType.ToString()].ToString())
                + 1);
            Collection.UpdateOne(filter, update);


        }


    }
}
