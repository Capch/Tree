using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TreeMulti.Interfaces;
using TreeMulti.Data;

namespace TreeMulti
{
    public class JsonRepository : ITreeRepository
    {
        private readonly string _filepath;
        public JsonRepository(string path)
        {
            _filepath = path;
            using (var fs = new FileStream(_filepath, FileMode.OpenOrCreate))
            {

            }
        }
        public IEnumerable<Node> GetTree()
        {
            return ReadJson();
        }

        public void SaveTree(IEnumerable<Node> tree)
        {
            WriteJson(tree);
        }

        private IEnumerable<Node> ReadJson()
        {
            IEnumerable<Node> cards;
            using (var sr = new StreamReader(_filepath, System.Text.Encoding.Default))
            {
                var jsonText = sr.ReadToEnd();
                cards = JsonConvert.DeserializeObject<IEnumerable<Node>>(jsonText,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto,
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                        Formatting = Formatting.Indented
                    });
            }
            return cards ?? new List<Node>();
        }

        private void WriteJson(IEnumerable<Node> cards)
        {
            using (var sw = new StreamWriter(_filepath, false, System.Text.Encoding.Default))
            {
                sw.Write(JsonConvert.SerializeObject(cards,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto,
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                        Formatting = Formatting.Indented
                    }));
            }
        }
    }
}