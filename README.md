# Touchpad Toggle Tool for Asus Zenbook Duo on Ubuntu

## Description

Since the Asus Zenbook Duo doesn't have a dedicated button to quickly turn the touchpad on or off, this tool for Linux is here to help you toggle it easily.  
  
It's been tested on the `Asus Zenbook Duo 2025 (UX8406CA)` on `Ubuntu 25.04`.

## Important

Currently this only works on Xorg, there is no Wayland support yet

## Installation

1. Download the binary and place it anywhere you like.  
   For example in your home directory or `/usr/local/bin/`.
2. Make sure the binary is executable by running:
   `chmod +x /path/to/your/zenbook-duo-touchpad-toggle`
3. In the Ubuntu keyboard settings, create a custom shortcut  
   that points to the tool, for example `CTRL+F12`.

## Compilation

If you want to compile the tool yourself,   
make sure you have .NET9 (or later) installed  
and run `dotnet build -c Release` in the project directory.