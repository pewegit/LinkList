using System.Windows;

using LinkList.ViewModel;

namespace LinkList
{
    public partial class MainWindow : Window
    {
        LinkListModelControl llmc;

        public MainWindow()
        {
            InitializeComponent();

            llmc = new LinkListModelControl();
                        
            DataContext = llmc;
        }
    }
}
