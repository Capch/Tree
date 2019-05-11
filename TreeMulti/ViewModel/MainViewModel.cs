using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TreeMulti.Interfaces;
using TreeMulti.Model;

namespace TreeMulti.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        private ITreeRepository _repository;
        private readonly IDialogService _dialog;
        private readonly Func<Node, AddViewModel> _addEditVmFunc;
        private int _count;
        private IEnumerable<object> _selectedItems;
        private ObservableCollectionEx<Node> _tree;
        private bool _isTreeChanged = false;

        public MainViewModel(ITreeRepository treeRepository, IDialogService dialogService, Func<Node, AddViewModel> addEditVmFunc)
        {
            AddCommand = new Command(AddNewNode, IsSelectedOneOrNothing);
            DeleteCommand = new Command(DeleteNodes, IsSelectedMany);
            EditCommand = new Command(EditNodeExecute, IsSelectedOne);
            InitCommand = new Command(ResetInit);

            _repository = treeRepository;
            _dialog = dialogService;
            _addEditVmFunc = addEditVmFunc;

            if (_repository.GetTree().ToModel() is IEnumerable<Node> nodes)
            {
                Tree = new ObservableCollectionEx<Node>(nodes.ToList());
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


        private bool IsSelectedOneOrNothing(object obj)
        {
            if (SelectedItems == null) return true;
            return SelectedItems.Count() <= 1;
        }
        private bool IsSelectedOne(object obj)
        {
            if (SelectedItems == null) return false;
            return SelectedItems.Count() == 1;
        }
        private bool IsSelectedMany(object obj)
        {
            if (SelectedItems == null) return false;
            return SelectedItems.Any();
        }

        public virtual Node CreateNodeForAddEditVm(NodeTypes nodeTypes)
        {
            switch (nodeTypes)
            {
                case NodeTypes.GroupNode:
                   return new GroupNode();
                case NodeTypes.Node1:
                    return new Node1();
                case NodeTypes.Node2:
                    return new Node2();
                default:
                    return null;
            }
        }

        private void AddNewNode(object obj)
        {
            Node item;
            if (SelectedItems == null || !(SelectedItems.Count() <= 1))
            {
                item = null;
            }
            else
            {
                item = (Node)SelectedItems.ToList().FirstOrDefault();
            }
            
            Node resultNode;
            NodeTypes type;
            if (obj is NodeTypes types)
            {
                type = types;
            }
            else
            {
                return;
            }
            
            resultNode = CatchNode(_addEditVmFunc, CreateNodeForAddEditVm(type));

            if (resultNode == null)
            {
                return;
            }

            switch (item)
            {
                case null:
                    Tree.Add(resultNode);
                    Tree.Sort();

                    break;
                case GroupNode groupNode:
                    groupNode.AddChild(resultNode);
                    break;
                default:
                    if ((GroupNode)item.Parent != null)
                    {
                        ((GroupNode)item.Parent).AddChild(resultNode);
                    }
                    else
                    {
                        Tree.Add(resultNode);
                        Tree.Sort();
                    }
                    break;
            }
            TreeChanged();
        }

        private void EditNodeExecute(object obj)
        {
            if (!(obj is IEnumerable<object>) || ((IEnumerable<object>) obj).Count() != 1)
            {
                return;
            }

            var item = (Node)((IEnumerable<object>)obj).First();

            Node result = null;

            switch (item)
            {
                case GroupNode groupNode:
                    result = CatchNode(_addEditVmFunc, groupNode);
                    break;
                case Node1 node1:
                    result = CatchNode(_addEditVmFunc, node1);
                    break;
                case Node2 node2:
                    result = CatchNode(_addEditVmFunc, node2);
                    break;
            }

            if (result == null)
            {
                return;
            }
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
            TreeChanged();
        }

        private Node CatchNode(Func<Node, AddViewModel> vmFunc, Node baseNode)
        {
            var addViewModel = vmFunc.Invoke(baseNode);
            var dialogRes = _dialog.ShowDialog(addViewModel);

            if (dialogRes == true && addViewModel.NewNode.IsNotEmpty())
            {
                return addViewModel.NewNode;
            }

            return null;
        }

        private void DeleteNodes(object obj)
        {
            if (!(obj is IEnumerable<object>))
            {
                return;
            }
            var selectList = ((IEnumerable<object>)obj).OfType<Node>().ToList();
            if (selectList.Count < 1) return;
            for (var j = 0; j < selectList.Count; j++)
            {
                for (var i = 0; i < Tree.Count; i++)
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

            Count = 0;
            SelectedItems=new List<object>();
            TreeChanged();
        }

        private void ResetInit(object obj)
        {
            var g1 = new GroupNode("group1", "comment1");
            var n1_1 = new Node2("Node2", "comment1", "comment2", "comment3") { Parent = g1 };
            var n1_2 = new Node1("Node1", "comment1", "comment2") { Parent = g1 };

            g1.AddChild(n1_1);
            g1.AddChild(n1_2);

            var g2 = new GroupNode("group2", "comment1");
            var n1_3 = new Node2("Node2", "comment1", "comment2", "comment3") { Parent = g2 };
            var n1_4 = new Node1("Node1", "comment1", "comment2") { Parent = g2 };

            g2.AddChild(n1_3);
            g2.AddChild(n1_4);

            var g3 = new GroupNode("group2", "comment1") { Parent = g2 };
            var n1_5 = new Node2("Node2", "comment1", "comment2", "comment3") { Parent = g3 };
            var n1_6 = new Node1("Node1", "comment1", "comment2") { Parent = g3 };

            g3.AddChild(n1_5);
            g3.AddChild(n1_6);
            g2.AddChild(g3);

            Tree = new ObservableCollectionEx<Node> { g1, g2 };
            _repository.SaveTree(Tree.ToData());
            TreeChanged();
        }

        private void TreeChanged()
        {
            if (!_isTreeChanged)
            {
                _isTreeChanged = true;
            }
        }

        public override void OnClosing(object sender, EventArgs eventArgs)
        {
            if (_isTreeChanged)
            {
                _repository.SaveTree(Tree.ToData());
            }
            base.OnClosing(sender, eventArgs);
        }

        public override void Dispose()
        {
            _repository = null;
            base.Dispose();
        }

    }
}
