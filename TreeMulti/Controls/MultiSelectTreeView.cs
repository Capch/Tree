using System.Collections.Generic;
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
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty SelectCountProperty = DependencyProperty.Register(
            name: nameof(SelectCount),
            typeof(int),
            typeof(MultiSelectTreeView),
            new FrameworkPropertyMetadata(
                0,
                FrameworkPropertyMetadataOptions.AffectsMeasure |
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

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

        public int SelectCount
        {
            get => (int)GetValue(SelectCountProperty);
            set => SetValue(SelectCountProperty, value);
        }

        private bool IsCtrlPressed => Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
        private bool IsShiftPressed => Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

        public SelectionMode SelectMode
        {
            get => (SelectionMode)GetValue(SelectModeProperty);
            set => SetValue(SelectModeProperty, value);
        }

        private void SetSelectedProp()
        {
            var selectedTreeViewItems = GetTreeViewItems(this, true).Where(GetIsItemSelected);
            var selectedModelItems = selectedTreeViewItems.Select(treeViewItem => treeViewItem.Header)
                                                          .ToList();

            SelectedItems = selectedModelItems;
            SelectCount = selectedModelItems.Count;
        }
        
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);

            // If clicking on a tree branch expander...   || e.OriginalSource is Border
            if (e.OriginalSource is Shape || e.OriginalSource is Grid)
            {
                var items = GetTreeViewItems(this, true);
                foreach (var treeViewItem in items)
                    SetIsItemSelected(treeViewItem, false);
                SelectedItems = new List<object>();
                SelectCount = 0;
                return;
            }
            var item = GetTreeViewItemClicked((FrameworkElement)e.OriginalSource);
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
                    ExtendedItemChangedInternal(item);
                    break;
            }
        }

        public ItemsControl GetSelectedTreeViewItemParent(TreeViewItem item)
        {
            var parent = VisualTreeHelper.GetParent(item);
            while (!(parent is TreeViewItem || parent is TreeView))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent as ItemsControl;
        }

        //Multi
        private void MultiItemChangedInternal(TreeViewItem tvItem)
        {
            var isOneParent = GetTreeViewItems(this, true)
                .Where(x=>x.IsSelected)
                .All(x => GetSelectedTreeViewItemParent(x) == GetSelectedTreeViewItemParent(tvItem));
            if (!IsCtrlPressed || !isOneParent)
            {
                var items = GetTreeViewItems(this, true);
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
                    {
                        SetIsItemSelected(treeViewItem, true);
                    }
                    _lastItemSelected = items.Last();
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
            SetSelectedProp();
        }

        //Single
        private void SingleItemChangedInternal(TreeViewItem tvItem)
        {
            if (_lastItemSelected != null) SetIsItemSelected(_lastItemSelected, false);
            SetIsItemSelected(tvItem, true);
            _lastItemSelected = tvItem;

            SetSelectedProp();
        }

        //Extended
        private void ExtendedItemChangedInternal(TreeViewItem tvItem)
        {

            if (!IsCtrlPressed)
            {
                var items = GetTreeViewItems(this, true);
                foreach (var treeViewItem in items)
                    SetIsItemSelected(treeViewItem, false);

                SelectedItems = new List<object>();
            }

            if (IsShiftPressed && _lastItemSelected != null)
            {
                var items = GetTreeViewItemRange(_lastItemSelected, tvItem);
                if (items.Count > 0)
                {
                    foreach (var treeViewItem in items)
                        SetIsItemSelected(treeViewItem, true);
                    _lastItemSelected = items.Last();
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
            SetSelectedProp();
        }
        
        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            TreeViewItem tvItem = null;

            if (!IsCtrlPressed)
            {
                var items = GetTreeViewItems(this, true);
                foreach (var treeViewItem in items)
                    SetIsItemSelected(treeViewItem, false);

                SelectedItems = new List<object>();
            }
            tvItem = GetTreeViewItemClicked((FrameworkElement)e.OriginalSource);
            if (tvItem == null) return;

            if (IsShiftPressed && _lastItemSelected != null)
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
            SetSelectedProp();
            base.OnGotKeyboardFocus(e);
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