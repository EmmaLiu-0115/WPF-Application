using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanzaTechOpcConfig
{
    public class Remote
    {
        public string RemoteHost { get; set; }
        public int Remotes { get; set; }
    }

    public class Server
    {
        public string ServerName { get; set; }
        public int Connected { get; set; }
        public int GroupCount { get; set; }
    }

    public class OpcConfigEntity
    {
        public string AccessPath { get; set; }
        public int Active { get; set; }
        public int ReqDataType { get; set; }
        public string Component { get; set; }
        public string Alias { get; set; }
    }

    public class GroupOpcConfigEntity
    {
        public string GroupName { get; set; }
        public int GroupActive { get; set; }
        public int ReqUpdateRate { get; set; }
        public int TimeBias { get; set; }
        public string PercentDeadband { get; set; }
        public int GroupConnected { get; set; }
        public int ItemCount { get; set; }

        public BindingList<OpcConfigEntity> dataList { get; set; }

        public GroupOpcConfigEntity()
        {
            dataList = new BindingList<OpcConfigEntity>();
        }
    }
}
