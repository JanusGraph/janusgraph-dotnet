#!/bin/bash
##########################################################################
# Bash bootstrapper for Cake with .Net Core.
##########################################################################

# Define directories.
SCRIPT_DIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )
TOOLS_DIR=$SCRIPT_DIR/tools
CAKE_VERSION_FILE=$TOOLS_DIR/cake_version
CAKE_VERSION=$(cat $CAKE_VERSION_FILE)
CAKE_FOLDER=$TOOLS_DIR/Cake.CoreCLR/$CAKE_VERSION/
CAKE_DLL=$CAKE_FOLDER/Cake.dll

# Define default arguments.
TARGET="Default"
CONFIGURATION="Release"
VERBOSITY="verbose"
DRYRUN=
SCRIPT_ARGUMENTS=()

# Parse arguments.
for i in "$@"; do
    case $1 in
        -t|--target) TARGET="$2"; shift ;;
        -c|--configuration) CONFIGURATION="$2"; shift ;;
        -v|--verbosity) VERBOSITY="$2"; shift ;;
        -d|--dryrun) DRYRUN="-dryrun" ;;
        --) shift; SCRIPT_ARGUMENTS+=("$@"); break ;;
        *) SCRIPT_ARGUMENTS+=("$1") ;;
    esac
    shift
done

# Make sure the tools folder exist.
if [ ! -d "$TOOLS_DIR" ]; then
    mkdir "$TOOLS_DIR"
fi

###########################################################################
# ENSURE .NET CORE CLI IS PRESENT
###########################################################################

if ! [ -x "$(command -v dotnet)" ]; then
    (>&2 echo "Cannot find dotnet executable on this system!")
    exit 1
fi

###########################################################################
# INSTALL CAKE
###########################################################################

if [ ! -f "$CAKE_DLL" ]; then
    ZIP_NAME=Cake.CoreCLR.$CAKE_VERSION.zip

    # Download the Cake.CoreCLR nuget package and save it as a *.zip file.
    if [ ! -f "$ZIP_NAME" ]; then
        wget -O $ZIP_NAME https://www.nuget.org/api/v2/package/Cake.CoreCLR/$CAKE_VERSION
    fi

    # Unpack the zip file into the tools folder.
    mkdir -p $CAKE_FOLDER
    unzip $ZIP_NAME -d $CAKE_FOLDER

    # Remove the zip
    rm -f $ZIP_NAME
fi

###########################################################################
# RUN BUILD SCRIPT
###########################################################################

# Start Cake
dotnet "$CAKE_DLL" build.cake --verbosity=$VERBOSITY --configuration=$CONFIGURATION --target=$TARGET $DRYRUN "${SCRIPT_ARGUMENTS[@]}"