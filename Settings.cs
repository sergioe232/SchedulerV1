using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace ProjectPickleRick
{
    public class Settings
    {
        [XmlElement("IsOverlayEnabled")]
        public bool isOverlayEnabled = true;
        [XmlElement("MaxOverlayTasks")]
        public int maxOverlayTasks = 4;
        [XmlElement("EventNoticeMinutes")]
        public int eventNoticeMinutes = 30;
        [XmlElement("OverlayBackgroundColor", Type = typeof(XmlColor))]
        public Color overlayBackgroundColor = Color.Violet;
        [XmlElement("OverlayTextColor", Type = typeof(XmlColor))]
        public Color overlayTextColor = Color.White;
        [XmlElement("SchedulerColor1", Type = typeof(XmlColor))]
        public Color schedulerColor1 = Color.DodgerBlue;
        [XmlElement("SchedulerColor2", Type = typeof(XmlColor))]
        public Color schedulerColor2 = Color.Green;
        [XmlElement("SchedulerTextColor", Type = typeof(XmlColor))]
        public Color schedulerTextColor = Color.White;

        public void Load()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Scheduler\\";
            var filename = "settings.xml";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if(!File.Exists(path + filename))
            {
                return;
            }

            var deserializer = new XmlSerializer(typeof(Settings), new XmlRootAttribute("Settings"));
            using (var file = new FileStream(path + filename, FileMode.OpenOrCreate))
            {
                using (XmlReader reader = new XmlTextReader(file))
                {
                    try
                    {
                        var temp = (Settings)deserializer.Deserialize(reader);
                        this.overlayBackgroundColor = temp.overlayBackgroundColor;
                        this.overlayTextColor = temp.overlayTextColor;
                        this.eventNoticeMinutes = temp.eventNoticeMinutes;
                        this.isOverlayEnabled = temp.isOverlayEnabled;
                        this.maxOverlayTasks = temp.maxOverlayTasks;
                        this.schedulerColor1 = temp.schedulerColor1;
                        this.schedulerColor2 = temp.schedulerColor2;
                        this.schedulerTextColor = temp.schedulerTextColor;
                    }
                    catch(Exception e)
                    {
                        MessageBox.Show("Your settings.xml save file is corrupted, certain elements may not have loaded properly", "Corrupted File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        public void Save()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Scheduler\\";
            var filename = "settings.xml";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var serializer = new XmlSerializer(typeof(Settings));
            using (var file = new FileStream(path + filename, FileMode.OpenOrCreate))
            {
                file.SetLength(0);
                using (StreamWriter writer = new StreamWriter(file))
                {
                    serializer.Serialize(writer, Globals.settings);
                }
            }
        }
    }

    public class XmlColor
    {
        private Color color_ = Color.Black;

        public XmlColor() { }
        public XmlColor(Color c) { color_ = c; }


        public Color ToColor()
        {
            return color_;
        }

        public void FromColor(Color c)
        {
            color_ = c;
        }

        public static implicit operator Color(XmlColor x)
        {
            return x.ToColor();
        }

        public static implicit operator XmlColor(Color c)
        {
            return new XmlColor(c);
        }

        [XmlAttribute]
        public string Web
        {
            get { return ColorTranslator.ToHtml(color_); }
            set
            {
                try
                {
                    if (Alpha == 0xFF) // preserve named color value if possible
                        color_ = ColorTranslator.FromHtml(value);
                    else
                        color_ = Color.FromArgb(Alpha, ColorTranslator.FromHtml(value));
                }
                catch (Exception)
                {
                    color_ = Color.Black;
                }
            }
        }

        [XmlAttribute]
        public byte Alpha
        {
            get { return color_.A; }
            set
            {
                if (value != color_.A) // avoid hammering named color if no alpha change
                    color_ = Color.FromArgb(value, color_);
            }
        }

        public bool ShouldSerializeAlpha() { return Alpha < 0xFF; }
    }
}
