using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;

namespace LanzaTechOpcConfig
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadOpcConfig();
            RemoteDataGrid.ItemsSource = RemoteList;
            ServerDataGrid.ItemsSource = ServerList;
            GroupDataGrid.ItemsSource = GroupList;

        }

        static List<Remote> RemoteList = new List<Remote>();
        static List<Server> ServerList = new List<Server>();
        static List<GroupOpcConfigEntity> GroupList = new List<GroupOpcConfigEntity>();

        private static void LoadOpcConfig()
        {
            try
            {
                XmlDocument xmlContent = new XmlDocument();
                xmlContent.Load(Properties.Settings.Default.OpcConfig);

                XmlNodeList Host = xmlContent.DocumentElement.GetElementsByTagName("Hostname");

                for (int i = 0; i < Host.Count; i++)
                {
                    Remote aRemote = new Remote();
                    aRemote.RemoteHost = Host[i].Attributes["RemoteHost"].InnerText;
                    aRemote.Remotes = int.Parse(Host[i].Attributes["Remote"].InnerText);

                    RemoteList.Add(aRemote);
                }


                XmlNodeList Serve = xmlContent.DocumentElement.GetElementsByTagName("Server");

                for (int i = 0; i < Serve.Count; i++)
                {
                    Server aServer = new Server();
                    aServer.ServerName = Serve[i].Attributes["Name"].InnerText;
                    aServer.Connected = int.Parse(Serve[i].Attributes["Connected"].InnerText);
                    aServer.GroupCount = int.Parse(Serve[i].Attributes["GroupCount"].InnerText);

                    ServerList.Add(aServer);

                }

                XmlNodeList groups = xmlContent.DocumentElement.GetElementsByTagName("Group");

                for (int i = 0; i < groups.Count; i++)
                {
                    GroupOpcConfigEntity aGroup = new GroupOpcConfigEntity();
                    aGroup.GroupName = groups[i].Attributes["Name"].InnerText;
                    aGroup.GroupActive = int.Parse(groups[i].Attributes["Active"].InnerText);
                    aGroup.ReqUpdateRate = int.Parse(groups[i].Attributes["ReqUpdateRate"].InnerText);
                    aGroup.TimeBias = int.Parse(groups[i].Attributes["TimeBias"].InnerText);
                    aGroup.PercentDeadband = groups[i].Attributes["PercentDeadband"].InnerText;
                    aGroup.GroupConnected = int.Parse(groups[i].Attributes["Connected"].InnerText);
                   // aGroup.ItemCount = int.Parse(groups[i].Attributes["ItemCount"].InnerText);

                    XmlNodeList aItem = groups[i].SelectNodes("Item");

                    for (int j = 0; j < aItem.Count; j++)
                    {
                        OpcConfigEntity aComponent = new OpcConfigEntity();
                        aComponent.AccessPath = aItem[j].Attributes["AccessPath"].InnerText;
                        aComponent.Active = int.Parse(aItem[j].Attributes["Active"].InnerText);
                        aComponent.ReqDataType = int.Parse(aItem[j].Attributes["ReqDataType"].InnerText);
                        aComponent.Component = aItem[j].InnerText;

                        aGroup.dataList.Add(aComponent);
                    }

                    GroupList.Add(aGroup);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void GroupDataGrid_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            if (GroupDataGrid.SelectedIndex > -1)
            {
                GroupOpcConfigEntity bItem = (GroupOpcConfigEntity)GroupDataGrid.SelectedItem;
                ItemDataGrid.ItemsSource = bItem.dataList;
            }
        }

        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            try
            {
                XmlDocument originContent = new XmlDocument();
                originContent.Load(Properties.Settings.Default.OpcConfig);
                XmlElement root = originContent.DocumentElement;
                XmlNodeList groups = root.GetElementsByTagName("Group");

                for (int i = groups.Count - 1; i >= 0; i--)
                {
                    XmlNode node = groups[i];
                    node.ParentNode.RemoveChild(node);
                }

                XmlNode host = root.GetElementsByTagName("Hostname")[0];

                for (int i = 0; i < GroupList.Count; i++)
                {
                    XmlElement group = originContent.CreateElement("Group");
                    group.SetAttribute("Name", GroupList[i].GroupName);
                    group.SetAttribute("Active", GroupList[i].GroupActive.ToString());
                    group.SetAttribute("ReqUpdateRate", GroupList[i].ReqUpdateRate.ToString());
                    group.SetAttribute("TimeBias", GroupList[i].TimeBias.ToString());
                    group.SetAttribute("PercentDeadband", GroupList[i].PercentDeadband.ToString());
                    group.SetAttribute("Connected", GroupList[i].GroupConnected.ToString());

                    for (int j = 0; j < GroupList[i].dataList.Count; j++)
                    {
                        XmlElement item = originContent.CreateElement("Item");
                        item.SetAttribute("AccessPath", GroupList[i].dataList[j].AccessPath);
                        item.SetAttribute("Active", GroupList[i].dataList[j].Active.ToString());
                        item.SetAttribute("ReqDataType", GroupList[i].dataList[j].ReqDataType.ToString());
                        item.InnerText = GroupList[i].dataList[j].Component;

                        group.AppendChild(item);
                    }

                    host.AppendChild(group);
                }
                originContent.Save(@"C:\Users\xiaoyu.liu\Documents\Visual Studio 2013\Projects\LanzaTechModbusConfig\LanzaTechOpcConfig\OpcConfig.xml");
                MessageBox.Show("Save Successfully!", "OPC Config", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void BtnCancel(object sender, RoutedEventArgs e)
        {
            LoadOpcConfig();

            this.Close();
        }
      
        
    }
}
