module Testje
 
 data MoreMagic = Meaning(str foo)
 				| Of(list[str] bar)
 				| Life(MoreMagic foobar)
 				;
 
@javaClass{Landman.Rascal.CLRInfo.HelloWorld}
public MoreMagic java getMagicNumber();
 
public MoreMagic main() {
      return getMagicNumber();
}