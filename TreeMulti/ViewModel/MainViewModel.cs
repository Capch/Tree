using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TreeMulti.Interfaces;
using TreeMulti.Model;

namespace TreeMulti.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ITreeRepository _rep;
        private int _count;
        private IEnumerable<object> _selectedItems;
        public MainViewModel(ITreeRepository treeRepository)
        {
            AddCommand = new Command(AddNewNode);
            DeleteCommand = new Command(DeleteNodes);
            EditCommand = new Command(EditNode);
            InitCommand = new Command(ResetInit);
            _rep = treeRepository;

            if (_rep.GetTree().ToModel() is IEnumerable<Node> nodes)
            {
                Tree = new ObservableCollection<Node>(nodes);
            }
        }

        public ICommand InitCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand EditCommand { get; set; }

        public IEnumerable<object> SelectedItems
        {
            get => _selectedItems;
            set
            {
                _selectedItems = value;
                OnPropertyChanged();
            }
        }
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Node> Tree { get; set; }

        private bool IsSelectedOne(object obj)
        {
            if (obj == null) return true;
            if (!(obj is List<Node>)) return false;
            return ((List<Node>)obj).Count == 1;
        }
        private bool IsOneOrNothing(object obj)
        {
            if (SelectedItems == null) return true;
            return (SelectedItems.ToList().Count == 1) && (SelectedItems.FirstOrDefault() is GroupNode);
        }
        private bool IsSelectedMany(object obj)
        {
            if (!(obj is List<Node>)) return false;
            return ((List<Node>)obj).Count > 0;
        }

        private void AddNewNode(object obj)
        {
            //Node parent;
            //Node item;
            //if (SelectedItems != null)
            //{
            //    item = (Node)SelectedItems.ToList().FirstOrDefault();
            //    parent = item.Parent;
            //}
            //else
            //{
            //    item = null;
            //    parent = null;
            //}

            //Node result = null;
            //var type = (obj is NodeTypes ? (NodeTypes)obj : NodeTypes.GroupNode);

            //switch (type)
            //{
            //    case NodeTypes.GroupNode:
            //        result = CatchNode(new GroupNode());
            //        break;
            //    case NodeTypes.Node1:
            //        result = CatchNode(new Node1());
            //        break;
            //    case NodeTypes.Node2:
            //        result = CatchNode(new Node2());
            //        break;
            //    default:
            //        result = CatchNode(new GroupNode());
            //        break;
            //}
            

            //if (!(item is GroupNode groupNode))
            //{
            //    Tree.Add(result);
            //}
            //else
            //{
            //    groupNode.Children.Add(result);
            //}

        }

        private void EditNode(object obj)
        {
            //var item = (Node)((IEnumerable<object>)obj).First();

            //Node result = null;

            //switch (item)
            //{
            //    case GroupNode groupNode:
            //        result = CatchNode(groupNode);
            //        break;
            //    case Node1 node1:
            //        result = CatchNode(node1);
            //        break;
            //    case Node2 node2:
            //        result = CatchNode(node2);
            //        break;
            //}
            //if (result == null) return;
            //if (item.Parent != null)
            //{
            //    var parent = (GroupNode)item.Parent;
            //    result.Parent = item.Parent;
            //    var index = parent.Children.IndexOf(item);
            //    parent.Children[index] = result;
            //}
            //else
            //{
            //    var index = Tree.IndexOf(item);
            //    Tree[index] = result;
            //}
        }

        private Node CatchNode(Node baseNode)
        {
            var addViewModel = new AddViewModel(baseNode);
            var dialog = ((App) Application.Current).DialogService;
            var dialogRes = dialog.ShowDialog(addViewModel);
            
            if (dialogRes != null && dialogRes.Value == true)
            {
                return (Node)addViewModel.NewNode; 
            }
            else
            {
                return null;
            }
        }

        private void DeleteNodes(object obj)
        {
            var selectList = ((IEnumerable<object>) obj).OfType<Node>().ToList();

            for (int j = 0; j < selectList.Count; j++)
            {
                for (int i = 0; i < Tree.Count; i++)
                {
                    if (Tree[i] == selectList[j])
                    {
                        Tree.Remove(selectList[j]);
                        break;
                    }
                    else
                    {
                        (Tree[i] as GroupNode)?.Delete(selectList[j]);
                    }
                }
            }
        }
        private void ResetInit(object obj)
        {
            var g1 = new GroupNode("group1", "comment1");
            var n1_1 = new Node1("Node1", "comment1", "comment2") {Parent = g1};
            var n1_2 = new Node1("Node1", "comment1", "comment2") { Parent = g1 };
            
            g1.AddChild(n1_1);
            g1.AddChild(n1_2);

            var g2 = new GroupNode("group2", "comment1");
            var n1_3 = new Node1("Node1", "comment1", "comment2") { Parent = g2 };
            var n1_4 = new Node1("Node1", "comment1", "comment2") { Parent = g2 };
            
            g2.AddChild(n1_3);
            g2.AddChild(n1_4);

            var g3 = new GroupNode("group2", "comment1") { Parent = g2 };
            var n1_5 = new Node1("Node1", "comment1", "comment2") { Parent = g3 };
            var n1_6 = new Node1("Node1", "comment1", "comment2") { Parent = g3 };
            
            g3.AddChild(n1_5);
            g3.AddChild(n1_6);
            g2.AddChild(g3);

            Tree = new ObservableCollection<Node>();
            Tree.Add(g1);
            Tree.Add(g2);
            _rep.SaveTree(Tree.ToData());
            OnPropertyChanged(nameof(Tree));
        }
    }
}
