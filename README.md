# FALLING SAND GAME

**IMPORTANT: FOR MORE INFORMATION ON HOW TO BUILD PROJECT, SEE *BUILD* SECTION**

## BUILD

this project requires you have a few things:

**IMPORTANT** on windows, installing rustup may require extra
work, however there is an easy & lazy option I found on 
godot-rust bindings book that suggests using winget to download
rustup.

```pwsh
# install rustup
winget install -e --id Rustlang.Rustup

# install stable toolchain
rustup toolchain install stable
```

a rust compiler (https://rust-lang.org/learn/get-started/)
this should be simple as you just have to follow the instructions
on the website to install the rust compiler.

and C# (I assume you already have this installed)

you should be able to then run the project without issue using 
the standard run/build in visual studio code, since the dotnet
build command will compile the rust library as needed.