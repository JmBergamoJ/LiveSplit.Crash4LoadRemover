using LiveSplit.Crash4LoadRemover;
using LiveSplit.Model;
using LiveSplit.UI.Components;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: ComponentFactory(typeof(Crash4LoadRemoverFactory))]
namespace LiveSplit.Crash4LoadRemover
{
    public class Crash4LoadRemoverFactory : IComponentFactory
    {
        public string ComponentName => "Crash 4: IAT Load Remover (Memory-Based - Battle.Net)";

        public string Description => "Automatically detects and removes loads (GameTime) for Crash Bandicoot 4: It's About Time on PC (Battle.Net).";

        public ComponentCategory Category => ComponentCategory.Control;

        public string UpdateName => ComponentName;

        public string XMLURL => UpdateURL + "update.LiveSplit.Crash4LoadRemover.xml";

        public string UpdateURL => "https://raw.githubusercontent.com/JmBergamoJ/LiveSplit.Crash4LoadRemover/master/";

        public Version Version => Version.Parse(FileVersionInfo.GetVersionInfo(typeof(Crash4LoadRemoverFactory).Assembly.Location).FileVersion);

        public IComponent Create(LiveSplitState state)
        {
            return new Crash4LoadRemoverComponent(state);
        }
    }
}
