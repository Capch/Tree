using System.Collections.Generic;
using TreeMulti.Data;


namespace TreeMulti.Interfaces
{
    public interface ITreeRepository
    {
        IEnumerable<Node> GetTree();
        void SaveTree(IEnumerable<Node> tree);
    }
}
