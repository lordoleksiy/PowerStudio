using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStudio.Models.Azure
{
    public class AppSettingsModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public AppSettingsType Type { get; set; }

    }

    public enum AppSettingsType
    {
        New,
        Update,
        ForDelete,
        NoChange
    }
}
