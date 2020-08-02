using System;
using System.Collections.Generic;
using System.Text;

namespace TranslatorWPF.Models
{
    class Language
    {
        public string Name { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string VoiceName { get; set; }

        public Language(string name, string to, string from, string voiceName)
        {
            Name = name;
            To = to;
            From = from;
            VoiceName = voiceName;
        }
    }
}
