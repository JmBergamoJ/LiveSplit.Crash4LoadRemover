using LiveSplit.Crash4LoadRemover;
using LiveSplit.Model;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: ComponentFactory(typeof(Crash4LoadRemoverFactory))]
namespace LiveSplit.Crash4LoadRemover
{
    public class Crash4LoadRemoverFactory : IComponentFactory
    {
        public string ComponentName => "Crash 4: IAT Load Remover (Memory-Based)";

        public string Description => "Automatically detects and removes loads (GameTime) for Crash Bandicoot 4: It's About Time on PC.";

        public ComponentCategory Category => ComponentCategory.Control;

        public string UpdateName => ComponentName;

        public string XMLURL => UpdateURL + "update.LiveSplit.Crash4LoadRemover.xml";

        public string UpdateURL => "https://raw.githubusercontent.com/JmBergamoJ/LiveSplit.Crash4LoadRemover/master/LiveSplit.Crash4LoadRemover/";

        public Version Version => Version.Parse("1.0.2");

        public IComponent Create(LiveSplitState state)
        {
            return new Crash4LoadRemoverComponent(state);
        }
    }
}
