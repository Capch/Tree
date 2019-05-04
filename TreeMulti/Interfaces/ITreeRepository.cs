using System.Collections;
using System.Collections.ObjectModel;
using TreeMulti.Model;

namespace TreeMulti.Interfaces
{
    public interface ITreeRepository
    {
        IEnumerable GetTree();
        void SetTree(IEnumerable tree);
    }
}
