rascal-dotnet
=============

Like that little language called [Rascal-MPL](http://www.rascal-mpl.org/)? 
But want to use it to inspect .NET applications?
Well, this library aims to provide a reasonable insight into .NET assemblies.
It is however limited to analysis on compiled assemblies, a more powerful one
would work on a source based fact extractor, but this is much more work than the
current solution.

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

TODO
----

  - More error prone on the server side
  - Use LOC types in `readCLRInfo` function
  - Make deployment easier (especially the copying of the rascal files should be
	simpeler).
  - Start the `IPCServer.exe` automatically from the java side. (and randomize
	the port)
  - Create some sample projects to test the fact extraction better than just it
	doesn't crash-testing.
  - Write about 'architecture'.

