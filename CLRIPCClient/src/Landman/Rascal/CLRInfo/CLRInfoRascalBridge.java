package Landman.Rascal.CLRInfo;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.net.Socket;
import java.net.UnknownHostException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.eclipse.imp.pdb.facts.IConstructor;
import org.eclipse.imp.pdb.facts.IList;
import org.eclipse.imp.pdb.facts.IListWriter;
import org.eclipse.imp.pdb.facts.IRelationWriter;
import org.eclipse.imp.pdb.facts.ISetWriter;
import org.eclipse.imp.pdb.facts.IString;
import org.eclipse.imp.pdb.facts.IValue;
import org.eclipse.imp.pdb.facts.IValueFactory;
import org.eclipse.imp.pdb.facts.type.Type;
import org.eclipse.imp.pdb.facts.type.TypeFactory;
import org.eclipse.imp.pdb.facts.type.TypeStore;
import org.rascalmpl.values.ValueFactoryFactory;

import com.google.protobuf.CodedInputStream;

import Landman.Rascal.CLRInfo.Protobuf.Clrinfo.Constrain;
import Landman.Rascal.CLRInfo.Protobuf.Clrinfo.ConstrainRel;
import Landman.Rascal.CLRInfo.Protobuf.Clrinfo.Entity;
import Landman.Rascal.CLRInfo.Protobuf.Clrinfo.EntityRel;
import Landman.Rascal.CLRInfo.Protobuf.Clrinfo.Id;
import Landman.Rascal.CLRInfo.Protobuf.Clrinfo.InformationRequest;
import Landman.Rascal.CLRInfo.Protobuf.Clrinfo.InformationResponse;
import Landman.Rascal.CLRInfo.Protobuf.Clrinfo.Id.IdKind;
import Landman.Rascal.CLRInfo.Protobuf.Clrinfo.InformationRequest.Builder;
import Landman.Rascal.CLRInfo.Protobuf.Clrinfo.Modifier;
import Landman.Rascal.CLRInfo.Protobuf.Clrinfo.ModifierRel;

public class CLRInfoRascalBridge {
	private static final TypeFactory TF = TypeFactory.getInstance();
	private static final IValueFactory VF = ValueFactoryFactory.getValueFactory();
	private static final TypeStore store = new TypeStore();

	private static final Type entityDataType;
	private static final Type namespace;
	private static final Type clazz;
	private static final Type entity;
	private static final Type idDataType;
	private static final Type genericClazz;
	private static final Type interfaze;
	private static final Type genericInterfaze;
	private static final Type enumz;
	private static final Type constrainDataType;
	private static final Type typeParameter;
	private static final Type none;
	private static final Type isClass;
	private static final Type isStruct;
	private static final Type hasDefaultConstructor;
	private static final Type implementz;
	private static final Type resourceDataType;
	private static final Type file;
	private static final Type entityRel;
	private static final Type method;
	private static final Type constructor;
	private static final Type displayClass;
	private static final Type anonymousClass;
	private static final Type arrayz;
	private static final Type constrainRel;
	private static final Type modifierDataType;
	private static final IValue publicModifier;
	private static final IValue protectedModifier;
	private static final IValue internalModifier;
	private static final IValue privateModifier;
	private static final IValue staticModifier;
	private static final IValue abstractModifier;
	private static final Type modifierRel;
	private static Map<Entity, IValue> valueStore = new HashMap<Entity, IValue>();
	private static final Type field;
	private static final Type property;

	static {
		entityDataType = TF.abstractDataType(store, "Entity");
		idDataType = TF.abstractDataType(store, "Id");
		constrainDataType = TF.abstractDataType(store, "Constrain");
		modifierDataType = TF.abstractDataType(store, "Modifier");
		resourceDataType = TF.abstractDataType(store, "Resource");

		entity = TF.constructor(store, entityDataType, "entity", TF.listType(idDataType), "id");
		namespace = TF.constructor(store, idDataType, "namespace", TF.stringType(), "name");

		clazz = TF.constructor(store, idDataType, "class", TF.stringType(), "name");
		genericClazz = TF.constructor(store, idDataType, "class", 
				TF.stringType(), "name", TF.listType(entity), "params");
		interfaze = TF.constructor(store, idDataType, "interface", TF.stringType(), "name");
		genericInterfaze = TF.constructor(store, idDataType, "interface", 
				TF.stringType(), "name", TF.listType(entity), "params");
		anonymousClass = TF.constructor(store, idDataType, "anonymousClass", TF.integerType(), "nr");
		displayClass = TF.constructor(store, idDataType, "displayClass", TF.integerType(), "nr");

		typeParameter = TF.constructor(store, idDataType, "typeParameter", TF.stringType(), "name");
		enumz = TF.constructor(store, idDataType, "enum", TF.stringType(), "name");
		arrayz = TF.constructor(store, idDataType, "array", entity, "elementType");
		property = TF.constructor(store, idDataType, "property", TF.stringType(), "name", entity, "setter", entity, "getter");
		field = TF.constructor(store, idDataType, "field", TF.stringType(), "name");

		method = TF.constructor(store, idDataType, "method", TF.stringType(), "name", 
				TF.listType(entity), "params", entity, "returnType");
		constructor = TF.constructor(store, idDataType, "constr", TF.listType(entity), "params");

		none = TF.constructor(store, constrainDataType, "none");
		isClass = TF.constructor(store, constrainDataType, "isClass");
		isStruct = TF.constructor(store, constrainDataType, "isStruct");
		hasDefaultConstructor = TF.constructor(store, constrainDataType, "hasDefaultConstructor");
		implementz = TF.constructor(store, constrainDataType, "implements", entityDataType, "entity");

		publicModifier = TF.constructor(store, modifierDataType, "public").make(VF);
		protectedModifier = TF.constructor(store, modifierDataType, "protected").make(VF);
		internalModifier = TF.constructor(store, modifierDataType, "internal").make(VF);
		privateModifier = TF.constructor(store, modifierDataType, "private").make(VF);
		staticModifier = TF.constructor(store, modifierDataType, "static").make(VF);
		abstractModifier = TF.constructor(store, modifierDataType, "abstract").make(VF);
		
		
		entityRel = TF.aliasType(store, "EntityRel", TF.tupleType(entityDataType, "from", entityDataType, "to"));
		constrainRel = TF.aliasType(store, "ConstrainRel", TF.tupleType(entityDataType, "entity", constrainDataType, "constrain"));
		modifierRel = TF.aliasType(store, "ModifierRel", TF.tupleType(entityDataType, "entity", modifierDataType, "modifier"));
		

		file = TF.constructor(store, resourceDataType, "file", TF.sourceLocationType(), "id");
		store.declareAnnotation(resourceDataType, "types", TF.setType(entity));
		store.declareAnnotation(resourceDataType, "properties", TF.setType(entity));
		store.declareAnnotation(resourceDataType, "fields", TF.setType(entity));
		store.declareAnnotation(resourceDataType, "implements", entityRel);
		store.declareAnnotation(resourceDataType, "extends", entityRel);
		store.declareAnnotation(resourceDataType, "calls", entityRel);		
		store.declareAnnotation(resourceDataType, "methods", TF.setType(entity));
		store.declareAnnotation(resourceDataType, "genericConstrains", constrainRel);
		store.declareAnnotation(resourceDataType, "modifiers", modifierRel);
		
		
	}
	
	public CLRInfoRascalBridge(IValueFactory vf) {
	}

	public static IValue readCLRInfo(IList assemblyNames) {
		try {
			ArrayList<String> actualAssemblies = new ArrayList<String>();
			IListWriter locs = VF.listWriter(TF.sourceLocationType());
			for (IValue val : assemblyNames) {
				if (val instanceof IString) {
					actualAssemblies.add(((IString) val).getValue());
					locs.append(VF.sourceLocation(((IString) val).getValue().replaceAll("\\s", "%20")));
				}
			}

			Socket clientSocket = new Socket("localhost", 5555);
			DataOutputStream outToServer = new DataOutputStream(clientSocket.getOutputStream());
			DataInputStream inFromServer = new DataInputStream(clientSocket.getInputStream());
			Builder req = InformationRequest.newBuilder();
			req.addAllAssemblies(actualAssemblies);
			InformationRequest actualRequest = req.build();
			actualRequest.writeDelimitedTo(outToServer);
			int amountOfGroups = CodedInputStream.newInstance(inFromServer).readRawVarint32();
			ISetWriter types = VF.setWriter(entityDataType);
			ISetWriter methods = VF.setWriter(entityDataType);
			ISetWriter properties = VF.setWriter(entityDataType);
			ISetWriter fields = VF.setWriter(entityDataType);
			IRelationWriter extendz = VF.relationWriter(entityRel);
			IRelationWriter implementz = VF.relationWriter(entityRel);
			IRelationWriter calls = VF.relationWriter(entityRel);
			IRelationWriter contrains = VF.relationWriter(constrainRel);
			IRelationWriter modifiers = VF.relationWriter(modifierRel);
			for (int groupIndex = 0; groupIndex < amountOfGroups; groupIndex++) {
				InformationResponse currentInformation = InformationResponse.parseDelimitedFrom(inFromServer);
				addToEntitySet(types, currentInformation.getTypesList());
				addToEntitySet(methods, currentInformation.getMethodsList());
				addToEntitySet(properties, currentInformation.getPropertiesList());
				addToEntitySet(fields, currentInformation.getFieldsList());
				addToEntityRels(extendz, currentInformation.getTypesInheritanceList());
				addToEntityRels(implementz, currentInformation.getTypesImplementingList());
				addToEntityRels(calls, currentInformation.getMethodCallsList());
				addToConstrainRels(contrains, currentInformation.getGenericConstrainsList());
				addToModifierRels(modifiers, currentInformation.getModifiersList());
			}
			

			IConstructor result = (IConstructor) file.make(VF, locs.done());
			result = result.setAnnotation("types", types.done());
			result = result.setAnnotation("methods", methods.done());
			result = result.setAnnotation("properties", properties.done());
			result = result.setAnnotation("fields", fields.done());
			result = result.setAnnotation("extends", extendz.done());
			result = result.setAnnotation("implements", implementz.done());
			result = result.setAnnotation("calls", calls.done());
			result = result.setAnnotation("genericConstrains", contrains.done());
			result = result.setAnnotation("modifiers", modifiers.done());
			return result;
		} catch (Exception ex) {
			System.err.print(ex.toString());
			return null;
		}
		finally {
			valueStore.clear();
		}
	}

	private static void addToModifierRels(IRelationWriter destination, List<ModifierRel> source) {
		for (ModifierRel rel : source) {
			destination.insert(VF.tuple(generateSingleEntityArray(rel.getEntity()), generateModifier(rel.getModifier())));
		}
	}


	private static void addToConstrainRels(IRelationWriter destination, List<ConstrainRel> source) {
		for (ConstrainRel rel : source) {
			destination.insert(VF.tuple(generateSingleEntityArray(rel.getEntity()), generateConstrain(rel.getConstrain())));
		}
	}
	
	private static void addToEntityRels(IRelationWriter destination, List<EntityRel> source) {
		for (EntityRel rel : source) {
			destination.insert(VF.tuple(generateSingleEntityArray(rel.getFrom()), generateSingleEntityArray(rel.getTo())));
		}
	}

	private static void addToEntitySet(ISetWriter destination, List<Entity> source) {
		for (Entity clrEntity : source) {
			destination.insert(generateSingleEntityArray(clrEntity));
		}
	}

	private static IList generateEntityList(List<Entity> typesList) {
		IListWriter result = VF.listWriter(entityDataType);
		for (Entity clrEntity : typesList) {
			result.append(generateSingleEntityArray(clrEntity));
		}
		return result.done();
	}

	private static IValue generateSingleEntityArray(Entity clrEntity) {
		IValue result = valueStore.get(clrEntity);
		if (result != null)
			return result;
		List<IValue> currentEntity = new ArrayList<IValue>();
		for (Id currentId : clrEntity.getIdsList()) {
			switch (currentId.getKind()) {
				case Namespace:
					currentEntity.add(createTypeWithName(namespace, currentId));
					break;
				case Class:
					currentEntity.add(createTypeWithName(clazz, currentId));
					break;
				case GenericClass:
					currentEntity.add(createTypeWithNameAndParameters(genericClazz, currentId));
					break;
				case Interface:
					currentEntity.add(createTypeWithName(interfaze, currentId));
					break;
				case GenericInterface:
					currentEntity.add(createTypeWithNameAndParameters(genericInterfaze, currentId));
					break;
				case AnonymousClass:
					currentEntity.add(anonymousClass.make(VF, currentId.getId()));
					break;
				case DisplayClass:
					currentEntity.add(displayClass.make(VF, currentId.getId()));
					break;
				case Enumeration:
					currentEntity.add(createTypeWithName(enumz, currentId));
					break;
				case TypeParameter:
					currentEntity.add(createTypeParameter(currentId));
					break;
				case Method:
					currentEntity.add(createMethodType(currentId));
					break;
				case Constructor:
					currentEntity.add(constructor.make(VF, generateEntityList(currentId.getParamsList())));
					break;
				case Array:
					currentEntity.add(arrayz.make(VF, generateSingleEntityArray(currentId.getElementType())));
					break;
				case Property:
					currentEntity.add(property.make(VF, VF.string(currentId.getName()),  
							generateSingleEntityArray(currentId.getSetter()), generateSingleEntityArray(currentId.getGetter())));
					break;
				case Field:
					currentEntity.add(createTypeWithName(field, currentId));
					break;
				default:
					throw new RuntimeException("You forgot to detect the id: " + currentId.getKind().toString());
			}
		}
		result = entity.make(VF, VF.list(currentEntity.toArray(new IValue[0])));
		valueStore.put(clrEntity, result);
		return result;
	}

	private static IValue createMethodType(Id currentId) {
		assert currentId.getKind() == IdKind.Method;
		return method.make(VF, VF.string(currentId.getName()), 
				generateEntityList(currentId.getParamsList()),
				generateSingleEntityArray(currentId.getReturnType()));
	}

	private static IValue createTypeParameter(Id currentId) {
		assert currentId.getKind() == IdKind.TypeParameter;
		return typeParameter.make(VF, VF.string(currentId.getName()));
	}

	private static IValue generateConstrain(Constrain constrain) {
		switch (constrain.getKind()) {
			case Entity:
				return implementz.make(VF, generateSingleEntityArray(constrain.getConstrainEntity()));
			case HasConstructor:
				return hasDefaultConstructor.make(VF);
			case IsClass:
				return isClass.make(VF);
			case IsStruct:
				return isStruct.make(VF);
			case None:
				return none.make(VF);
			default:
				throw new RuntimeException("Unkown constrain type");
		}
	}
	private static IValue generateModifier(Modifier modifier) {
		switch (modifier) {
		case Abstract:
			return abstractModifier;
		case Internal:
			return internalModifier;
		case Private:
			return privateModifier;
		case Protected:
			return protectedModifier;
		case Public:
			return publicModifier;
		case Static:
			return staticModifier;
		default:
			throw new RuntimeException("Unkown modifier type");
		}
	}

	

	private static IValue createTypeWithName(Type targetType, Id currentId) {
		return targetType.make(VF, VF.string(currentId.getName()));
	}

	private static IValue createTypeWithNameAndParameters(Type targetType, Id currentId) {
		return targetType.make(VF, VF.string(currentId.getName()), generateEntityList(currentId.getParamsList()));
	}

	public static void main(String[] args) throws UnknownHostException, IOException {
		//IValue result = readCLRInfo(VF.list(VF.string("/usr/lib/mono/2.0/System.dll")));
		//IValue result = readCLRInfo(VF.list(VF.string("/home/davy/MiscUtil.dll")));
		//IValue result = readCLRInfo(VF.list(VF.string("../../../TestProject/bin/Debug/TestProject.exe")));
		//IValue result = readCLRInfo(VF.list(VF.string("c:/Windows/Microsoft.NET/Framework/v2.0.50727/System.dll")));
		IValue result = readCLRInfo(VF.list(VF.string("/home/davy/Personal/Rascal CSharp/rascal-csharp/lib/ICSharpCode.NRefactory.dll")));
		
		System.out.print(((IConstructor) result).getAnnotation("properties"));
	}

}
