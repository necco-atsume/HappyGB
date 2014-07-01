HappyGB
===

A C# gameboy emulator.
---

Currently a WIP. Plan to support GB games (Specifically, Tetris) 
for now, but may add support for GBC / SGB in the future.

![Tetris title screen](http://imgur.com/sAysnCM.png)

The core of the emulator is mostly complete, but there are a few bugs to work out in the CPU before it will run anything more complicated than the boot rom.

This does not run any games yet and the file path for the rom is hard-coded. It also expects there to be a DMG boot rom named 'DMG_ROM.bin' in the executable directory.

HappyGB is released under the MIT license. See the included LICENSE file for more info. 

