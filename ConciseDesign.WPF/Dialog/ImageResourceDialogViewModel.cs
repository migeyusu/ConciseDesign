using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using ConciseDesign.WPF.Command;
using ConciseDesign.WPF.UserControls;
using FORCEBuild.Concurrency;
using Microsoft.Win32;

namespace ConciseDesign.WPF.Dialog
{
    /// <summary>
    /// img management vm
    /// </summary>
    public class ImageResourceDialogViewModel : INotifyPropertyChanged
    {
        private ImageSource _select;
        private int _selectIndex;
        private bool _isNullSelect;

        public DialogHostControl DialogHost { get; set; }

        public ResourcesManager<ImageSource> ResourcesManager { get; private set; }

        public int MinimumCount { get; set; }

        /// <summary>
        /// 当前是否可以删除
        /// </summary>
        public bool CanDelete { get; set; }

        /// <summary>
        /// 是否空值
        /// </summary>
        public bool IsNullSelect
        {
            get { return _isNullSelect; }
            set
            {
                _isNullSelect = value;
                OnPropertyChanged();
            }
        }

        public int SelectIndex
        {
            get { return _selectIndex; }
            set
            {
                _selectIndex = value;
                OnPropertyChanged();
            }
        }

        public string SelectSourceName => IsNullSelect ? null : ResourcesManager.NamesCollection[SelectIndex];

        public ImageSource SelectImageSource
        {
            get { return _select; }
            set
            {
                _select = value;
                OnPropertyChanged();
            }
        }

        public ImageResourceDialogViewModel(ResourcesManager<ImageSource> resourcesManager)
        {
            ResourcesManager = resourcesManager;
            DialogHost = new DialogHostControl();
        }

        public ICommand DeleteCommand => new BaseCommand(async o =>
        {
            if (ResourcesManager.NamesCollection.Count == MinimumCount)
            {
                await DialogHost.RaiseMessageAsync($"总图片数不能小于{MinimumCount}个");
                return;
            }

            ResourcesManager.RemoveAt(SelectIndex);
        }, o => SelectIndex > -1);

        public ICommand AddCommand => new BaseCommand(o =>
        {
            var ofd = new OpenFileDialog()
            {
                Filter = "图片|*.jpg;*.png;*.gif;*.jpeg;*.bmp;*.tiff"
            };
            if (ofd.ShowDialog() == true)
            {
                for (var i = 0; i < ofd.FileNames.Length; i++)
                {
                    ResourcesManager.AddFile(ofd.FileNames[i], ofd.SafeFileNames[i]);
                }
            }
        });

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}