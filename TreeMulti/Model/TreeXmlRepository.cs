using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TreeMulti.Interfaces;

namespace TreeMulti.Model
{
    public class TreeXmlRepository : ITreeRepository
    { 
        public IEnumerable GetTree()
        {
            using (FileStream fs = new FileStream("tree.xml", FileMode.OpenOrCreate))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                if (fs.Length == 0)
                {
                    return null;
                }
                return (IEnumerable) formatter.Deserialize(fs);
            }
        }

        public void SetTree(IEnumerable tree)
        {
            using (FileStream fs = new FileStream("tree.xml", FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, tree);
            }
        }

    }
}
