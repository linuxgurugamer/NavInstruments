#!/usr/bin/env bash

source ./CONFIG.inc

check() {
	if [ ! -d "./GameData/$TARGETBINDIR/" ] ; then
		rm -f "./GameData/$TARGETBINDIR/"
		mkdir -p "./GameData/$TARGETBINDIR/"
	fi
}

deploy() {
	local DLL=$1

	if [ -f "./bin/Release/$DLL.dll" ] ; then
		cp "./bin/Release/$DLL.dll" "./GameData/$TARGETBINDIR/"
		if [ -d "${KSP_DEV}/GameData/$TARGETBINDIR/" ] ; then
			cp "./bin/Release/$DLL.dll" "${KSP_DEV/}GameData/$TARGETBINDIR/"
		fi
	fi
	if [ -f "./bin/Debug/$DLL.dll" ] ; then
		if [ -d "${KSP_DEV}/GameData/$TARGETBINDIR/" ] ; then
			cp "./bin/Debug/$DLL.dll" "${KSP_DEV}GameData/$TARGETBINDIR/"
		fi
	fi
}

VERSIONFILE=$PACKAGE.version

check
cp $VERSIONFILE "./GameData/$TARGETDIR"
cp CHANGE_LOG.md "./GameData/$TARGETDIR"
cp README.md  "./GameData/$TARGETDIR"
cp INSTALL.md  "./GameData/$TARGETDIR"
cp LICENSE "./GameData/$TARGETDIR"
cp NOTICE "./GameData/$TARGETDIR"
for dll in NavUtilLib NavUtilRPM ; do
    deploy $dll
done
