using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TreeMulti.Helpers;
using TreeMulti.Interfaces;
using TreeMulti.Model;

namespace TreeMulti.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ITreeRepository _rep;
        private ObservableCollectionEx<Node> _tree;
        private int _count;
        private IEnumerable<object> _selectedItems;
        public MainViewModel(ITreeRepository treeRepository)
        {
            AddCommand = new Command(AddNewNode);
            DeleteCommand = new Command(DeleteNodes);
            EditCommand = new Command(EditNode);
            InitCommand = new Command(ResetInit);
            _rep = treeRepository;
            if (_rep.GetTree() is IEnumerable<Node> nodes)
            {
                Tree = new ObservableCollectionEx<Node>(nodes);
                Tree.CollectionChanged += TreeCollectionChanged;
                Tree.ItemPropertyChanged += TreeCollectionChanged;
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
        public ObservableCollectionEx<Node> Tree
        {
            get => _tree;
            set
            {
                _tree = value;
                OnPropertyChanged();
            }
        }

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
            Node parent;
            Node item;
            if (SelectedItems != null)
            {
                item = (Node)SelectedItems.ToList().First();
                parent = item.Parent;
            }
            else
            {
                item = null;
                parent = null;
            }

            Node result = null;
            var type = (obj is NodeTypes ? (NodeTypes)obj : NodeTypes.GroupNode);

            switch (type)
            {
                case NodeTypes.GroupNode:
                    result = CatchNode(new GroupNode());
                    break;
                case NodeTypes.Node1:
                    result = CatchNode(new Node1());
                    break;
                case NodeTypes.Node2:
                    result = CatchNode(new Node2());
                    break;
                default:
                    result = CatchNode(new GroupNode());
                    break;
            }
            

            if (!(item is GroupNode groupNode))
            {
                Tree.Add(result);
            }
            else
            {
                groupNode.Children.Add(result);
            }

        }

        private void EditNode(object obj)
        {
            var item = (Node)((IEnumerable<object>)obj).First();

            Node result = null;

            switch (item)
            {
                case GroupNode groupNode:
                    result = CatchNode(groupNode);
                    break;
                case Node1 node1:
                    result = CatchNode(node1);
                    break;
                case Node2 node2:
                    result = CatchNode(node2);
                    break;
            }
            if (result == null) return;
            if (item.Parent != null)
            {
                var parent = (GroupNode)item.Parent;
                result.Parent = item.Parent;
                var index = parent.Children.IndexOf(item);
                parent.Children[index] = result;
            }
            else
            {
                var index = Tree.IndexOf(item);
                Tree[index] = result;
            }
        }

        private Node CatchNode(Node baseNode)
        {
            var addViewModel = new AddViewModel(baseNode);
            var dialog = ((App) Application.Current).DialogService;
            var dialogRes = dialog.ShowDialog(addViewModel);
            
            if (dialogRes.Value == true)
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
            InitXml.Init();
            if (_rep.GetTree() is IEnumerable<Node> nodes)
            {
                Tree = new ObservableCollectionEx<Node>(nodes);
            }

        }
        private void ClearSelectedItems()
        {
            SelectedItems = null;
            Count = 0;
        }
        private void TreeCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            _rep.SetTree(Tree.ToArray());
        }

        private void TreeCollectionChanged(object sender, PropertyChangedEventArgs e)
        {
            _rep.SetTree(Tree.ToArray());
        }
    }
}
