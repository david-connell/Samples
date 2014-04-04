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
        Temperature = 1,
        Pressure = 2,
        Humidity = 3,
        WindSpeed = 4,
        PH=5,
        Oxygen=6,
        Nitrogen=7,
        Gloss=8,
        Cure=9,
        RelTemperature=11,
        Percentage=12,
        Nanometers=13,
        Millimeters=14,        
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
