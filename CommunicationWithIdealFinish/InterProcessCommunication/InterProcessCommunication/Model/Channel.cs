using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TQC.GOC.InterProcessCommunication.Model
{
    public enum ChannelType
    {
        Unknown =-1,
        Temperature = 0,
        Thickness,
    }

    public class Channel
    {
        public Channel(string name, ChannelType type)
        {
            ChannelName = name;
            ChannelType = type;
        }
        public string ChannelName { get; private set; }
        public ChannelType ChannelType { get; private set; }
    }
}
