
using UnityEngine;
using ToolbarControl_NS;
using KSP_Log;


namespace NavUtilLib
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class InitLog: MonoBehaviour
    {
        void Awake()
        {
            RegisterToolbar.InitLog();
        }
    }

    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar : MonoBehaviour
    {

        public static Log Log = null;
        public static void InitLog()
        {
            if (Log == null)
#if DEBUG
                Log = new Log("NavInstruments", Log.LEVEL.INFO);
#else
          Log = new Log("NavInstruments", Log.LEVEL.ERROR);
#endif

        }
        void Awake()
        {
            InitLog();
        }

        void Start()
        {
            ToolbarControl.RegisterMod(NavInstruments.NavUtilLib.NavUtilLibApp.MODID, NavInstruments.NavUtilLib.NavUtilLibApp.MODNAME);
        }

    }
}
