#LameDllWrapp

This project contains the actual wrapper used to interface to `libmp3lame.dll`.  In order to bypass the problems with binding to 32-bit or 64-bit code depending on the running assembly's configuration, two copies of this library (one for x86, one for x64) are included as resources in `NAudio.Lame` and the appropriate one is loaded at runtime.

### Usage

This project needs to be built for x86 and x64 targets and the two outputs included in `NAudio.Lame` with the Build Action `Embedded Resource`.  A Loader class in `NAudio.Lame` will use the path and filename to identify the version to load.

