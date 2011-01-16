rascal-dotnet
=============

Like that little language called [Rascal-MPL](http://www.rascal-mpl.org/)? 
But want to use it to inspect .NET applications?
Well, this library aims to provide a reasonable insight into .NET assemblies.
It is however limited to analysis on compiled assemblies, a more powerful one
would work on a source based fact extractor, but this is much more work than the
current solution.

Currently the following facts are extracted:

  - All types (classes, enumerations and interfaces)
  - Inheritance relation between classes
  - Implements relation between interfaces & classes
  - All methods declarations
  - Complete call-graph for every method declared
  - Generic constrains for all types and methods
  - Accessibility modifiers for all declared types and methods

Deployment
-----------

  - Make sure you have an up-to-date working version of Rascal
  - Download deploy package
  - Extract the files
  - Copy the dirs inside of the `in_src_dir` directory to
	the `eclipse/plugins/rascal_*/src/ dir` (you might have to create the `src`
	dir)
  - Copy the files from the `copy_to_project` directory to you project `src`
	folder.
  - Copy the files from `run_server` to a place where you can find them again :)

Alternative deployment
------------

  - Open the rascal.jar with an archiver
  - Copy the dirs inside `in_src_dir` into this jar
  - Copy the file from `copy_to_project` into this jar
  
This is more destructive, and I don't know what will happen when rascal gets an update. 
But the plus side is that you don't have to copy the `copy_to_project` files to
every project using this library.
 
Using the library
-----------------

Example source:

     import CLRInfo;
	 import dotNet;
	 import CSharp;
	 import IO;

	 public void PrintAllTypes() {
		Resource res = readCLRInfo(["/usr/lib/mono/2.0/System.dll"]);
		for (tp <- res@types) {
			print(readable(tp));
		}
	 }

Just make sure you start the `Landman.Rascal.CLRInfo.IPCServer.exe` (using
`mono` on linux/mac).

Architecture
-------------
<a href="http://i.imgur.com/nbpMM.png">
<img src="http://i.imgur.com/nbpMM.png" width="50%" height="50%"
alt="architecture" />
</a>

TODO
----

  - More error prone on the server side
  - Rethink the way the facts are represented on the rascal side, now based on
	JDT, but there are some flaws with that model.
  - Improve performance & memory usage on java side by caching the generation of
	`IValue`'s I don't want to know how many rascal instances of `System.Object`
	& `System.String` are generated.
	Should be fairly simple (due to imutable objects) as soon as I think how to 
	derive a key from the incoming `Entity`.
  - Improve performance & memory usage on the c# side by chaching generated
	`Entity`'s, this too should be fairly simple due to imutable objects, but
	deriving a key from a `TypeDefinition` is not as straightforward as it might
	seem.
  - Use LOC types in `readCLRInfo` function
  - Make deployment easier (especially the copying of the rascal files should be
	simpeler).
  - Start the `IPCServer.exe` automatically from the java side. (and randomize
	the port)
  - Create some sample projects to test the fact extraction better than just it
	doesn't crash-testing.
  - Write about 'architecture'.
  - Refactor the .NET & Java part to be more OO and less 'everything in one
	file'

