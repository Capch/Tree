using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TreeMulti
{
    public sealed class MultiSelectTreeView : TreeView
    {

        private TreeViewItem _lastItemSelected;

        public static readonly DependencyProperty IsItemSelectedProperty =
            DependencyProperty.RegisterAttached("IsItemSelected", typeof(bool), typeof(MultiSelectTreeView));

        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(
            name: nameof(SelectedItems),
            typeof(IEnumerable<object>),
            typeof(MultiSelectTreeView),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsMeasure |
                FrameworkPropertyMetadataOptions.AffectsRender));
        
        public static readonly DependencyProperty SelectModeProperty = DependencyProperty.Register(nameof(SelectMode), typeof(SelectionMode),
            typeof(MultiSelectTreeView), new PropertyMetadata(SelectionMode.Single));
        
        public static void SetIsItemSelected(UIElement element, bool value)
        {
            element.SetValue(IsItemSelectedProperty, value);
        }

        public static bool GetIsItemSelected(UIElement element)
        {
            return (bool)element.GetValue(IsItemSelectedProperty);
        }

        public IEnumerable<object> SelectedItems
        {
            get => (IEnumerable<object>)GetValue(SelectedItemsProperty);
            set => SetValue(SelectedItemsProperty, value);
        }
        
        private bool IsCtrlPressed => Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
        private bool IsShiftPressed => Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

        public SelectionMode SelectMode
        {
            private get => (SelectionMode)GetValue(SelectModeProperty);
            set => SetValue(SelectModeProperty, value);
        }

        private void SetSelectedProp()
        {
            var selectedTreeViewItems = GetTreeViewItems(this, true).Where(GetIsItemSelected);
            var selectedModelItems = selectedTreeViewItems.Select(treeViewItem => treeViewItem.Header)
                                                          .ToList();

            SelectedItems = selectedModelItems;
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Shape || e.OriginalSource is Grid)
            {
                var items = GetTreeViewItems(this, true);
                foreach (var treeViewItem in items)
                {
                    SetIsItemSelected(treeViewItem, false);
                    treeViewItem.IsSelected = false;
                }
                SelectedItems = new List<object>() { };
            }
            base.OnPreviewMouseDown(e);
        }

        private ItemsControl GetSelectedTreeViewItemParent(TreeViewItem item)
        {
            var parent = VisualTreeHelper.GetParent(item);
            while (parent!=null && !(parent is TreeViewItem || parent is TreeView))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent as ItemsControl;
        }


        //Multiple or Extended mode
        private void MultiItemChangedInternal(TreeViewItem tvItem)
        {
            bool isOneParent;
            if (SelectMode == SelectionMode.Multiple)
            {
                isOneParent = GetTreeViewItems(this, true)
                    .Where(GetIsItemSelected)
                    .All(x => GetSelectedTreeViewItemParent(x) == GetSelectedTreeViewItemParent(tvItem));
            }
            else
            {
                isOneParent = true;
            }

            if (!IsCtrlPressed || !isOneParent)
            {
                var items = GetTreeViewItems(this, false);
                foreach (var treeViewItem in items)
                {
                    SetIsItemSelected(treeViewItem, false);
                }
                SelectedItems = new List<object>();
            }
            if (IsShiftPressed && _lastItemSelected != null && isOneParent)
            {
                var items = GetTreeViewItemRange(_lastItemSelected, tvItem);
                if (items.Count > 0)
                {
                    foreach (var treeViewItem in items)
                        SetIsItemSelected(treeViewItem, true);
                    if (items.Count == 1)
                    {
                        _lastItemSelected = items.Last();
                    }
                }
            }
            else
            {
                if (GetIsItemSelected(tvItem) == false)
                {
                    SetIsItemSelected(tvItem, true);
                }
                else
                {
                    SetIsItemSelected(tvItem, false);
                }
                _lastItemSelected = tvItem;
            }
        }
         
        //Single mode
        private void SingleItemChangedInternal(TreeViewItem tvItem)
        {
            if (_lastItemSelected != null) SetIsItemSelected(_lastItemSelected, false);
            SetIsItemSelected(tvItem, true);
            _lastItemSelected = tvItem;

        }
        
        protected override void OnSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e)
        {

            var item = GetTreeViewItemClicked(ItemContainerGenerator.ContainerFromItemRecursive(SelectedItem));
            if (item == null) return;

            switch (SelectMode)
            {
                case SelectionMode.Single:
                    SingleItemChangedInternal(item);
                    break;
                case SelectionMode.Multiple:
                    MultiItemChangedInternal(item);
                    break;
                case SelectionMode.Extended:
                    MultiItemChangedInternal(item);
                    break;
            }
            SetSelectedProp();
            base.OnSelectedItemChanged(e);
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            SetSelectedProp();
            base.OnItemsChanged(e);
        }


        private static TreeViewItem GetTreeViewItemClicked(DependencyObject sender)
        {
            while (sender != null && !(sender is TreeViewItem))
                sender = VisualTreeHelper.GetParent(sender);
            return sender as TreeViewItem;
        }
        private static List<TreeViewItem> GetTreeViewItems(ItemsControl parentItem, bool includeCollapsedItems, List<TreeViewItem> itemList = null)
        {
            if (itemList == null)
                itemList = new List<TreeViewItem>();

            for (var index = 0; index < parentItem.Items.Count; index++)
            {
                if (!(parentItem.ItemContainerGenerator.ContainerFromIndex(index) is TreeViewItem tvItem)) continue;
                itemList.Add(tvItem);
                if (includeCollapsedItems || tvItem.IsExpanded)
                    GetTreeViewItems(tvItem, includeCollapsedItems, itemList);
            }
            return itemList;
        }

        private List<TreeViewItem> GetTreeViewItemRange(TreeViewItem start, TreeViewItem end)
        {
            var items = GetTreeViewItems(this, false);

            var startIndex = items.IndexOf(start);
            var endIndex = items.IndexOf(end);
            var rangeStart = startIndex > endIndex || startIndex == -1 ? endIndex : startIndex;
            var rangeCount = startIndex > endIndex ? startIndex - endIndex + 1 : endIndex - startIndex + 1;

            if (startIndex == -1 && endIndex == -1)
                rangeCount = 0;

            else if (startIndex == -1 || endIndex == -1)
                rangeCount = 1;

            return rangeCount > 0 ? items.GetRange(rangeStart, rangeCount) : new List<TreeViewItem>();
        }

    }
}