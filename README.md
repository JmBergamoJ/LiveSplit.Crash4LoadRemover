# LiveSplit.Crash4LoadRemover
LiveSplit component to automatically detect and remove load times from Crash Bandicoot 4: It's About Time, on PC.

This is adapted from [Thomasneff's LiveSplit.CrashNSTLoadRemoval](https://github.com/thomasneff/LiveSplit.CrashNSTLoadRemoval "Thomasneff's LiveSplit.CrashNSTLoadRemoval")
and from [Grimelios's LiveSplit.Crash](https://github.com/Grimelios/LiveSplit.Crash "Grimelios's LiveSplit.Crash") Auto-Spliiter Component

This component scans the game's Memory data to automatically detect loading screens and pause the Game Timer.


# Special Thanks
Special thanks go to McCrodi from the Crash Speedrunning Discord,by providing Memory Data.
You can check out his Script for LiveSplit on the file **Crash_Bandicoot_4_Load_Remover.asl**

# How does it work?
The method works by scanning specific Memory Addresses where the game is considered to be in a Loading State.

These memory values, while not perfect, seem to work decently well with the current PC patch of the game.

The Loading Screen can be divided into 2 parts.
The first part of the loading screen, showing the level name, and a second part of the loading screen, showing a blue swirl, indicating some online operations.
During this Loading process, the player has no control.

The tool is designed to be as easy to use as possible, in most cases requiring zero configuration whatsoever (just activate and go).

# Known Issues
If you want to use the AutoSplitter functionality, **all your Splits need to have different names!**. If you have Splits that share the same name, the AutoSplitter is not able to differentiate between them.

Note that since this autosplitter reads game memory during runtime, it unfortuantely can't be used for console runs. If you're running Crash on a console, keep using the [image-based version](https://github.com/thomasneff/LiveSplit.Crash4LoadRemover).

## Installation instructions for LiveSplit
- Download LiveSplit.Crash4LoadRemover.dll from the Releases section
- Move the DLL to your LiveSplit/Components folder (you should see lots of other DLL's in there)
- Open LiveSplit, then right-click and select Edit Layout
- Add the autosplitter (under the Control category)
- If desired, click Layout Settings (or double-click the newly-added component) to configure the autosplitter

## Known deficiencies

- The timer doesn't start automatically and doesn't end automatically. This is due to the fact that different categories have different end criteria. For the time being, make sure to split manually during the start and end of your run.

# Crash Speedrunning Discord

Make sure to check out the [Crash HD Speedrunning Discord!](https://discord.gg/Rb9qjtU)
