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

                Client = new MongoClient("mongodb://GraphICE:wd9c1PwaGldGYI6K@graphcluster-shard-00-00.f67ke.mongodb.net:27017,graphcluster-shard-00-01.f67ke.mongodb.net:27017,graphcluster-shard-00-02.f67ke.mongodb.net:27017/<dbname>?ssl=true&replicaSet=atlas-dmyh1e-shard-0&authSource=admin&retryWrites=true&w=majority");
                Database = Client.GetDatabase(DatabaseName);

                Collection = Database.GetCollection<BsonDocument>(CollectionName);

                bool isMongoLive = Database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(2000);

                if (isMongoLive) {
                    //MessageBox.Show("Connected");
                    FirstDocument = Collection.Find(new BsonDocument()).FirstOrDefault();
                    IsConnected = true;
                } else {
                    //MessageBox.Show("Failed");
                }


            } catch (Exception e) {
                MessageBox.Show(e.Message);

            }

        }

        public static void IncrementData(DataType dataType) {
            if (!IsConnected)
                return;
            if (MainWindow.TestActive)
                return;

            var filter = Builders<BsonDocument>.Filter.Eq("graphApp", "GraphICE");
            var update = Builders<BsonDocument>.Update.Set(dataType.ToString(),
                Int32.Parse(FirstDocument[dataType.ToString()].ToString())
                + 1);
            Collection.UpdateOne(filter, update);


        }


    }
}
