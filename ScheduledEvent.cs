using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProjectPickleRick
{
    public class ScheduledEvent
    {
        [XmlElement("Time")]
        public DateTime time { get; set; }
        [XmlElement("Name")]
        public String taskName { get; set; }
        [XmlElement("Description")]
        public String taskDescription { get; set; }
        [XmlElement("HasPassed")]
        public bool taskPassed { get; set; } = false;
        [XmlElement("NoticePassed")]
        public bool NoticePassed { get; set; } = false;


        public ScheduledEvent(){}

        public ScheduledEvent(DateTime time, String taskName, String taskDescription = "")
        {
            this.time = time;
            this.taskName = taskName;
            this.taskDescription = taskDescription;
        }
    }
}
