# NavInstruments /L Unofficial :: Change Log

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
* 2018-0801: v0.7.2.1 (Lisias) for KSP 1.4.x LATEST
	+ Added KSP-AVC support
	+ Moved PluginData back to <KSP_ROOT> 
* 2018-0401: v0.7.2 (SerTheGreat) LATEST OFFICIAL
	+ KSP 1.4.1 and 1.4.2 compatible
	+ Fixed marker sounds positioning so they are not muffled by Audio Muffler mod anymore 
* 2017-0708: v0.7.1 (SerTheGreat)
	+ Added a runway selection window
	+ Fixed customRunways file overwrite when adding a new runway 
* 2017-0706: v0.7.0 (SerTheGreat)
	+ Continued version of NavUtilities
	+ Changes to the original version:
		- KSP 1.3.0 compatible
		- Added a setting to hide NavBall waypoint icon
		- Brought back Blizzy's toolbar support
		- Runways made loadable as regular KSP configs and thus modifiable by Module Manager
		- Only the runways belonging to the current celestial body are selectable
		- Fixed INS waypoint names displayed
		- Simplified folder structure for the mod not being that dependent on it 
* 2016-0420: v0.6.1 (kujuman) Release for KSP 1.1
* 2016-0402: v0.6.0 (kujuman) Alpha Build for 1.1 Experimentals PRE-RELEASE
* 2014-1231: v0.5 RC 3 (kujuman)
	+ Updated for 0.90. 
* 2014-0725: v0.4.3 (kujuman) Update for KSP .25 PRE-RELEASE
	+ Minor update to NavUtilities 
* 2014-0725: v0.4.2 (kujuman) PRE-RELEASE
	+ compiled against .24.1 
* 2014-0723: v0.4.1 (kujuman) PRE-RELEASE
	+ GS now only show +-25° of the runway heading.
	+ Loc has a fine mode: ADI needle disappears, loc needle turns yellow, 4x more sensitive
	+ Fine mode turns on within 7500 meters if loc deviation is < .75°
* 2014-0720: v0.4.0 (kujuman) PRE-RELEASE
* 2014-0715: v0.4.0 RC (kujuman) PRE-RELEASE
	+ New: Ability to add custom runways in game (WIP)
custom runways can be removed
	+ Changed: RPM button keys are now soft coded into the prop.cfg file.
* 2014-0709: v0.3.1 (kujuman) PRE-RELEASE
	+ vAdded missing MM cfg files for Hyomoto's RPM parts
	+ Added MM for KSO
	+ Added MM for RPM 0.17 basic monitor
* 2014-0709: v0.3.0 (kujuman) public beta PRE-RELEASE
	+ First public release of NavUtilities for KSP
* 2014-0705: v.1-beta (kujuman) PRE-RELEASE
	+ development version .1
