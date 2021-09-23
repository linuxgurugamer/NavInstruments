# NavInstruments /L Unofficial :: Changes

* 2021-0520: v0.7.2.5 (Lisias) for >= KSP 1.4
	+ (finally) Fixed the Dessert Airfield `shortId` as [suggested](https://github.com/SerTheGreat/NavInstruments/pull/1#pullrequestreview-341004160) by [MatthieuLemaile](https://github.com/MatthieuLemaile)
	+ Added (proper) Module Manager support, needed as I moved the thing into the `net-lisias-kspu` file system hierarchy
* 2020-0811: v0.7.2.4 (Lisias) for >= KSP 1.4
	+ Moving on-demand loading assets into PluginData, saving a bit of time on KSP loading (as they would be loaded again anyway)
	+ Removed rogue files left behind on the last version packaging (shame on me...)
* 2020-0108: v0.7.2.3 (Lisias) for >= KSP 1.4
	+ Fixing a weird issue that was provoking the "Matrix stack full depth reached" bug from Unity.
	+ Adding Dessert Airfield runway, as suggested by [Sppion1](https://forum.kerbalspaceprogram.com/index.php?/profile/198924-sppion1/) [here](https://forum.kerbalspaceprogram.com/index.php?/topic/162967-140-181-navutilities-continued-ft-hsi-instrument-landing-system-v072-2018-apr-1/&do=findComment&comment=3716551). 
* 2020-0101: v0.7.2.2 (Lisias) for >= KSP 1.4
	+ Moving Add'On to `net.lisias.ksp` hierarchy
		- Preventing clashes with mainstream
		- Don't install both versions!
	+ Adding KSPe facilities:
		- Logging
		- Abstract File System
		- Unity independent Texture Loading
		- Installment checks
	+ User data and settings now lives on <KSP_ROOT>/PluginData
		- Including custom runways
	+ The instrument graphics doesn't deteriorates anymore when using Texture Quality less then 1/1 !
* 2018-0726: v0.7.2.1 (Lisias) for KSP 1.4.x LATEST
	+ Added KSP-AVC support
	+ Moved PluginData back to <KSP_ROOT> 
