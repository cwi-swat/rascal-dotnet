package Landman.Rascal.CLRInfo;

import org.eclipse.imp.pdb.facts.IInteger;
import org.eclipse.imp.pdb.facts.IValue;
import org.eclipse.imp.pdb.facts.IValueFactory;
import org.eclipse.imp.pdb.facts.impl.fast.ValueFactory;
import org.eclipse.imp.pdb.facts.type.Type;
import org.eclipse.imp.pdb.facts.type.TypeFactory;
import org.eclipse.imp.pdb.facts.type.TypeStore;
import org.rascalmpl.interpreter.types.RascalTypeFactory;
import org.rascalmpl.values.ValueFactoryFactory;
 
public class HelloWorld {
	private static final TypeFactory TF = TypeFactory.getInstance();
	private static final IValueFactory VF = ValueFactoryFactory.getValueFactory();
	private static final TypeStore store = new TypeStore();
	
	public HelloWorld(IValueFactory vf) {
   }
     
      public static IValue getMagicNumber() {
    	  
  		//declare abstract data type
  		Type moreMagic = TF.abstractDataType(store, "MoreMagic");

  		//declare the concrete data types (constructors)
  		Type meaning = TF.constructor(store, moreMagic, "Meaning", TF.stringType());
  		Type of = TF.constructor(store, moreMagic, "Of", TF.listType(TF.stringType()));
  		Type life = TF.constructor(store, moreMagic, "Life", moreMagic);

  		//create instances of the constructors (instantiate!)
  		//note that you can cast this values to IConstructor
  		IValue meaningFoo = meaning.make(VF, VF.string("foo"));
  		IValue ofFoo = of.make(VF, VF.list(VF.string("foo"), VF.string("bar")));
  		IValue lifeFoo = life.make(VF, meaningFoo);
  		return meaningFoo;
   }
}
