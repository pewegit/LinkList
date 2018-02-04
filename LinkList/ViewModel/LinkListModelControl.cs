using System;
using System.ComponentModel;          // INotifyPropertyChanged
using LinkList.Model;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Reflection;

static class GuiConstants
{
    public const bool editModeIsEnabled = false;
}


namespace LinkList.ViewModel
{
    class LinkListModelControl : INotifyPropertyChanged
    {
        public LinkListModelControl()
        {
            LinkList = new LinkListModel();

            ReadXml  = new RelayCommand(p => CanReadXml(),  a => LinkListModelReadXml());
            SaveXml  = new RelayCommand(p => CanSaveXml(),  a => LinkListModelSaveXml());
            AddBelow = new RelayCommand(p => CanAddBelow(), a => LinkListModelAddBelow());
            CutLast  = new RelayCommand(p => CanCutLast(),  a => LinkListModelCutLast());
            OpenFolder = new RelayCommand(p => CanOpenFolder(),  a => LinkListModelOpenFolder());

            LinkListModelReadXml();
        }

        private void LinkListModelReadXml()
        {
            LinkList.ReadXml(ModelConstants.strLinksFile);
        }

        private async void LinkListModelSaveXml()
        {
            LinkList.SaveXml(ModelConstants.strLinksFile);
            BackgroundColorStatusLine = System.Windows.Media.Brushes.Yellow;
            await Task.Delay(500);
            BackgroundColorStatusLine = System.Windows.Media.Brushes.White;
        }

        private void LinkListModelAddBelow()
        {
            LinkList.AddBelow();
        }

        private void LinkListModelCutLast()
        {
            LinkList.CutLast();
        }

        private void LinkListModelOpenFolder()
        {
            Process.Start(@System.IO.Directory.GetCurrentDirectory());
        }

        private bool CanReadXml()
        {
            return true; // no further pre-condition required
        }

        private bool CanSaveXml()
        {
            return true; // no further pre-condition required
        }

        private bool CanAddBelow()
        {
            return true; // no further pre-condition required
        }

        private bool CanCutLast()
        {
            return true; // no further pre-condition required
        }

        private bool CanOpenFolder()
        {
            return true; // no further pre-condition required
        }

        public LinkListModel LinkList { get; set; }

        public RelayCommand ReadXml { get; set; }
        public RelayCommand SaveXml { get; set; }
        public RelayCommand AddBelow { get; set; }
        public RelayCommand CutLast { get; set; }
        public RelayCommand OpenFolder { get; set; }

        private LinkListModel.Link _selectedItem;
        public LinkListModel.Link SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem == value)
                {
                    return;
                }
                _selectedItem = value;

                if ( EditModeIsChecked == false )
                {
                    try
                    {
                        if ( !SelectedItem.LinkName.Equals(ModelConstants.strLinkNameEmpty) )
                        {
                            //Clipboard.SetText(SelectedItem.LinkName);
                        }
                        try
                        {
                            System.Diagnostics.Process.Start(SelectedItem.LinkName); // Starts default browser with given link
                            //SelectedItem = null; // Allow selecting same row again (no change of selected item required)
			    //->leads to mouse click bouncing (page opened twice)
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Could not open the url with standard browser:");
                            Console.WriteLine(ex.Message);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Selected item does not exist anymore");
                        Console.WriteLine(ex.Message);
                    }
                }
                OnPropertyChanged(nameof(SelectedItem));
                //SelectedItem = null; // Allow selecting same row again (no change of selected item required)
                //->leads to mouse click bouncing (page opened twice)
            }
        }

        private bool _editModeIsChecked = GuiConstants.editModeIsEnabled;
        public bool EditModeIsChecked
        {
            get { return _editModeIsChecked; }
            set
            {
                if ( _editModeIsChecked == value ) return;
                _editModeIsChecked = value;

                if (_editModeIsChecked == true)
                {
                    EditModeDisabled = false;
                    ForegroundColor = System.Windows.Media.Brushes.Red;
                }
                else
                {
                    EditModeDisabled = true;
                    ForegroundColor = System.Windows.Media.Brushes.Black;
                    SelectedItem = null; // Allow selecting same row again (no move to other row required)
                }

                OnPropertyChanged(nameof(EditModeIsChecked));
            }
        }

        private bool _editModeDisabled = !GuiConstants.editModeIsEnabled;
        public bool EditModeDisabled
        {
            get { return _editModeDisabled; }
            set
            {
                if (_editModeDisabled == value) return;
                _editModeDisabled = value;
                
                OnPropertyChanged(nameof(EditModeDisabled));
            }
        }

        private int _cellHeightValue = 20;
        public int CellHeightValue
        {
            get { return _cellHeightValue; }
            set
            {
                if ( _cellHeightValue == value ) return;
                _cellHeightValue = value;

                
                //DATAGRID_LINKS.Columns[i].Height = _cellHeightValue; // DATAGRID_LINKS nur bekannt in mainwindow.xaml.cs

                OnPropertyChanged(nameof(CellHeightValue));
            }
        }

        private System.Windows.Media.Brush _foregroundColor =
            GuiConstants.editModeIsEnabled ? System.Windows.Media.Brushes.Red : System.Windows.Media.Brushes.Black;
        public System.Windows.Media.Brush ForegroundColor
        {
            get { return _foregroundColor; }
            set
            {
                if (_foregroundColor == value) return;
                _foregroundColor = value;
                
                OnPropertyChanged(nameof(ForegroundColor));
            }
        }

        private System.Windows.Media.Brush _backgroundColorStatusLine =
            GuiConstants.editModeIsEnabled ? System.Windows.Media.Brushes.Red : System.Windows.Media.Brushes.White;
        public System.Windows.Media.Brush BackgroundColorStatusLine
        {
            get { return _backgroundColorStatusLine; }
            set
            {
                if (_backgroundColorStatusLine == value) return;
                _backgroundColorStatusLine = value;

                OnPropertyChanged(nameof(BackgroundColorStatusLine));
            }
        }

        private string _fileName = System.IO.Directory.GetCurrentDirectory() + "\\" + ModelConstants.strLinksFile;
        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (_fileName == value) return;
                _fileName = value;

                OnPropertyChanged(nameof(FileName));
            }
        }

        private string _version = "Version: " + FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
        public string Version
        {
            get { return _version; }
            set
            {
                if (_version == value) return;
                _version = value;

                OnPropertyChanged(nameof(Version));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if ( PropertyChanged != null )
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
