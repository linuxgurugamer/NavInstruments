using KSP.Localization;
using System.Collections.Generic;

namespace KSP
{
	public static class ModuleManagerSupport
	{
		public static IEnumerable<string> ModuleManagerAddToModList()
		{
			string[] r = {Localizer.Format("#LOC_NavInst_NavInstruments")};
			return r;
		}
	}
}
