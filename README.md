HappyGB
===

A C# gameboy emulator.
---

[![Build status](https://ci.appveyor.com/api/projects/status/32r7s2skrgm9ubva/branch/master?svg=true&passingText=master%20-%20passing&failingText=master%20-%20failing&pendingText=master%20-%20pending)](https://ci.appveyor.com/project/corick/happygb/branch/master)
[![Build status](https://ci.appveyor.com/api/projects/status/32r7s2skrgm9ubva/branch/development?svg=true&passingText=development%20-%20passing&failingText=development%20-%20failing&pendingText=development%20-%20pending)](https://ci.appveyor.com/project/corick/happygb/branch/development)

Currently a WIP. Plan to support GB games (Specifically, Tetris) 
for now, but may add support for GBC / SGB in the future.

![Tetris title screen](http://imgur.com/sAysnCM.png)

The core of the emulator is mostly complete, but there are a few bugs to work out in the CPU before it will run anything more complicated than the boot rom.

This does not run any games yet and the file path for the rom is hard-coded. 

To use a boot ROM place it in the executable directory with the name 'DMG_ROM.bin'.  (Alternately, place it in the HappyGB.FNA\External directory and it will be copied on build.)

HappyGB is released under the MIT license. See the included LICENSE file for more info. 

