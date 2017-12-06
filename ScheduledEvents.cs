using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace ProjectPickleRick
{
    class ScheduledEvents
    {
        public List<ScheduledEvent> UpcomingEventsList;
        public List<ScheduledEvent> PastEventsList;
        private Timer eventTimer;
        
        public ScheduledEvents()
        {
            this.UpcomingEventsList = new List<ScheduledEvent>();
            this.PastEventsList = new List<ScheduledEvent>();
            
            this.eventTimer = new Timer();
            eventTimer.Tick += (sender, e) => timerListener(sender, e);
            eventTimer.Interval = 1000;
            eventTimer.Start();
        }

        public event OnEventPastHandler OnEventPassed;
        public delegate void OnEventPastHandler(ScheduledEvent task, EventArgs e);

        public event EventNoticeHandler OnEventNotice;
        public delegate void EventNoticeHandler(ScheduledEvent task, EventArgs e);

        public void timerListener(object sender, EventArgs e)
        {
            if (UpcomingEventsList.Count > 0)
            {
                foreach(var task in UpcomingEventsList)
                {
                    if (task.NoticePassed == false && task.time <= DateTime.Now.AddMinutes(Globals.settings.eventNoticeMinutes))
                    {
                        task.NoticePassed = true;
                        eventNotice(task);
                    }
                }

                var firstTask = UpcomingEventsList[0];

                if (firstTask.time <= DateTime.Now)
                {
                    this.eventPast(firstTask);
                }
            }
        }

        public void sortUpcomingEvents()
        {
            UpcomingEventsList.Sort((a, b) => a.time.CompareTo(b.time));
        }

        public void sortPastEvents()
        {
            PastEventsList.Sort((a, b) => b.time.CompareTo(a.time));
        }
        
        public void Load()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Scheduler\\";
            var filename = "events.xml";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (!File.Exists(path + filename))
            {
                return;
            }

            List<ScheduledEvent> allEvents = new List<ScheduledEvent>();
            var deserializer = new XmlSerializer(typeof(List<ScheduledEvent>), new XmlRootAttribute("ScheduledEvents"));
            using (var file = new FileStream(path + filename, FileMode.OpenOrCreate))
            {
                using (XmlReader reader = new XmlTextReader(file))
                {
                    try
                    {
                        allEvents = (List<ScheduledEvent>)deserializer.Deserialize(reader);
                    }
                    catch(Exception e)
                    {
                        MessageBox.Show("Your events.xml save file is corrupted, certain elements may not have loaded properly", "Corrupted File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    
                }
            }

            foreach (var task in allEvents)
            {
                if(task.time <= DateTime.Now)
                {
                    if (task.taskPassed == true)
                        this.add(task);
                    else
                        this.eventPast(task);
                }
                else
                    this.add(task);
            }
        }

        public void Save()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Scheduler\\";
            var filename = "events.xml";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var serializer = new XmlSerializer(typeof(List<ScheduledEvent>), new XmlRootAttribute("ScheduledEvents"));
            using (var file = new FileStream(path + filename, FileMode.OpenOrCreate))
            {
                file.SetLength(0);
                using (StreamWriter writer = new StreamWriter(file))
                {
                    serializer.Serialize(writer, PastEventsList.Concat(UpcomingEventsList).ToList());
                }
            }
        }

        public void eventPast(ScheduledEvent task)
        {
            task.taskPassed = true;
            UpcomingEventsList.Remove(task);
            PastEventsList.Add(task);
            sortUpcomingEvents();
            sortPastEvents();
            OnEventPassed(task, EventArgs.Empty); //move upcoming event to past event
            this.Save();
        }

        public void eventNotice(ScheduledEvent task)
        {
            OnEventNotice(task, EventArgs.Empty);
        }

        public void add(ScheduledEvent task)
        {
            if (task.taskPassed == true)
            {
                PastEventsList.Add(task);
            }
            else
            {
                UpcomingEventsList.Add(task);
            }

            sortUpcomingEvents();
            sortPastEvents();
            this.OnAdd(task, EventArgs.Empty);
            this.Save();
        }

        public event OnAddHandler OnAdd;
        public delegate void OnAddHandler(ScheduledEvent task, EventArgs e);

        public void edit(ScheduledEvent task)
        {
            UpcomingEventsList.Remove(task);
            PastEventsList.Remove(task);

            if (task.taskPassed == true)
            {
                PastEventsList.Add(task);
            }
            else
            {
                UpcomingEventsList.Add(task);
            }

            sortUpcomingEvents();
            sortPastEvents();
            task.NoticePassed = false;
            this.OnEdit(task, EventArgs.Empty);
            this.Save();
        }

        public event OnEditHandler OnEdit;
        public delegate void OnEditHandler(ScheduledEvent task, EventArgs e);

        public void remove(ScheduledEvent task)
        {
            UpcomingEventsList.Remove(task);
            PastEventsList.Remove(task);
            sortUpcomingEvents();
            sortPastEvents();

            this.OnRemove(task, EventArgs.Empty);
            this.Save();
        }

        public event OnRemoveHandler OnRemove;
        public delegate void OnRemoveHandler(ScheduledEvent task, EventArgs e);

        public List<ScheduledEvent> getAllEvents()
        {
            return PastEventsList.Concat(UpcomingEventsList).ToList();
        }

        public List<ScheduledEvent> getUpcomingEvents()
        {
            return UpcomingEventsList.ToList();
        }

        public List<ScheduledEvent> getPastEvents()
        {
            return PastEventsList.ToList();
        }

        
    }
}
