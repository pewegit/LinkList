using System;

using System.Xml.Serialization;       // XML class attributes and XmlSerializer
using System.IO;                      // FileStream
using System.ComponentModel;          // INotifyPropertyChanged
using System.Collections.ObjectModel; // ObservableCollection

static class ModelConstants
{
    public const string strLinksFile      = "Links.xml";
    public const string strCategoryEmpty = "<TYPE CATEGORY HERE>";
    public const string strShortNameEmpty = "<TYPE SHORTNAME HERE>";
    public const string strLinkNameEmpty  = "<TYPE LINKNAME HERE>";
    public const string strCommentEmpty   = "<TYPE COMMENT HERE>";
}

namespace LinkList.Model
{
    public class LinkListModel : INotifyPropertyChanged
    {
        // main data of the model
        private ObservableCollection<Link> _linkArray = new ObservableCollection<Link>();

        public ObservableCollection<Link> LinkArray
        {
            get { return _linkArray; }
            set { _linkArray = value; OnPropertyChanged(nameof(LinkArray)); }
        }

        public class Link
        {
            public string Category { get; set; }
            public string ShortName { get; set; }
            public string LinkName { get; set; }
            public string Comment { get; set; }

            // Default constructor:
            public Link()
            {
                Category = "N/A";
                ShortName = "N/A";
                LinkName = "N/A";
                Comment = "N/A";
            }

            public Link(string category, string shortName, string linkName, string comment)
            {
                Category = category;
                ShortName = shortName;
                LinkName = linkName;
                Comment = comment;
            }
        }

        public void ReadXml(string filename)
        {
            try
            {
                XmlSerializer xmls = new XmlSerializer(typeof(ObservableCollection<Link>));
                StreamReader rd = new StreamReader(filename);
                LinkArray = xmls.Deserialize(rd) as ObservableCollection<Link>;
                rd.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        public void SaveXml(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Create);
            XmlSerializer xmls = new XmlSerializer(typeof(ObservableCollection<Link>));
            xmls.Serialize(fs, LinkArray);
            fs.Close();
        }

        public void AddBelow()
        {
            Link lnk = new Link(ModelConstants.strCategoryEmpty, ModelConstants.strShortNameEmpty, ModelConstants.strLinkNameEmpty, ModelConstants.strCommentEmpty);
            LinkArray.Add(lnk);
        }

        public void CutLast()
        {
            if (LinkArray.Count > 0)
            {
                int idxLast = LinkArray.Count - 1;
                LinkArray.RemoveAt(idxLast);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
