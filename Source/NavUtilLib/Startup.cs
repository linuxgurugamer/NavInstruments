using UnityEngine;

//using Log = NavInstruments.NavUtilLib.Log;
using static NavUtilLib.RegisterToolbar;

namespace NavInstruments
{
    internal class Startup : MonoBehaviour
	{
        private void Start()
        {
           // Log.Debug("Version "+Version.Text);

#if false
            try
            {
                //KSPe.Util.Compatibility.Check<Startup>(typeof(Version), typeof(Configuration));
                KSPe.Util.Installation.Check<Startup>(typeof(Version));
            }
            catch (KSPe.Util.InstallmentException e)
            {
                Log.ex(this, e);
                KSPe.Common.Dialogs.ShowStopperAlertBox.Show(e);
            }
#endif
        }
	}
}
