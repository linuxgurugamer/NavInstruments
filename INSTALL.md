# NavInstruments /L Unofficial

Adds a horizontal situation indicator with integrated ILS functionality to a popup window or your RPM cockpit.

Unofficial fork by Lisias.


## Installation Instructions

To install, place the GameData folder inside your Kerbal Space Program folder. Optionally, you can also do the same for the PluginData (be careful to do not overwrite your custom settings):

* **REMOVE ANY OLD VERSIONS OF THE PRODUCT BEFORE INSTALLING**, including any other fork:
	+ Delete `<KSP_ROOT>/GameData/net.lisias.ksp/NavInstruments`
* Extract the package's `GameData` folder into your KSP's root:
	+ `<PACKAGE>/GameData` --> `<KSP_ROOT>/GameData`
* Extract the package's `PluginData` folder (if available) into your KSP's root, taking precautions to do not overwrite your custom settings if this is not what you want to.
	+ `<PACKAGE>/PluginData` --> `<KSP_ROOT>/PluginData`
	+ You can safely ignore this step if you already had installed it previously and didn't deleted your custom configurable files.

The following file layout must be present after installation:

```
<KSP_ROOT>
	[GameData]
		[net.lisias.ksp]
			[NavInstruments]
				[Audio]
					...
				[MFD]
					...
				[PluginData]
					[Textures]
						...
					[Toolbar]
						...
				[Plugins]
					...
				[Props]
					...
				[Runways]
					...
				CHANGE_LOG.md
				LICENSE
				NOTICE
				NavInstruments.version
				Runways.cfg
				glidslopes.cfg
				...
		000_KSPe.dll
		...
	[PluginData]
		[net.lisias.ksp]
			[NavInstruments]<not present until you run it for the fist time>
				settings.cfg 
	KSP.log
	PartDatabase.cfg
	...
```


### Dependencies

* Hard Dependencies - Plugin will not work without it.
	+ [KSP API Extensions/L](https://github.com/net-lisias-ksp/KSPAPIExtensions) 2.1 or later
* Soft Dependencies - Plugin will work, by with limited features without it
	+ [Module Manager/L](https://github.com/net-lisias-ksp/ModuleManager) v3 or later
		- Mainstream Module Manager also works. :)
	+ [Raster Prop Monitor/L](https://github.com/net-lisias-kspu/RasterPropMonitor)
		- [Mainstream RPM](https://forum.kerbalspaceprogram.com/index.php?/topic/105821-16x-rasterpropmonitor-development-stopped-v0306-29-december-2018/) also works for now, but it's currently halted.
	
