# LiveSplit.Crash4LoadRemover
LiveSplit component to automatically detect and remove loads from Crash Bandicoot 4: It's About Time, on PC.

This is adapted from [Thomasneff's LiveSplit.CrashNSTLoadRemoval](https://github.com/thomasneff/LiveSplit.CrashNSTLoadRemoval "Thomasneff's LiveSplit.CrashNSTLoadRemoval")
and from [Grimelios's LiveSplit.Crash](https://github.com/Grimelios/LiveSplit.Crash "Grimelios's LiveSplit.Crash") Auto-Spliiter Component

This component uses Game Memory data to automatically detect loading screens and pause the Game Timer accordingly.


# Special Thanks
Special thanks go to McCrodi from the Crash Speedrunning Discord, who helped me by providing Memory Data and past experiences with similar tools.

# How does it work?
The method works by scanning specific Memory Addresses where the game is considered in a Loading State.

These memory values, while not perfect, seem to work decently well with the current PC patch of the game. 
**There's currently no way of knowing with 100% certainty that the addresses used by this component corresponds to the actual Loading of the levels. I.E.: it might be just a general tag for loss of player control.**

The Loading Screen can be divided into 2 parts.
The first part of the loading screen, showing the level name, and a second part of the loading screen, showing a blue swirl, indicating some online operations.
During this Loading process, the player has no control.

# Known Issues
If you want to use the AutoSplitter functionality, **all your Splits need to have different names!**. If you have Splits that share the same name, the AutoSplitter is not able to differentiate between them.
**32-bit testing pending.**


# Settings
The LiveSplit.Crash4LoadRemover.dll goes into your "Components" folder in your LiveSplit folder.

Add this to LiveSplit by going into your Layout Editor -> Add -> Control -> Crash4LoadRemover.

