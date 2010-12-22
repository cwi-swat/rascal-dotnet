module CLRInfo

import dotNet;
import CSharp;


@javaClass{Landman.Rascal.CLRInfo.CLRInfoRascalBridge}
public Resource java readCLRInfo(str assemblyName, list[str] relatedAssemblies);