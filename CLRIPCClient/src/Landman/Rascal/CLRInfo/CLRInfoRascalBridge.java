package Landman.Rascal.CLRInfo;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.net.Socket;
import java.net.UnknownHostException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import org.eclipse.imp.pdb.facts.IConstructor;
import org.eclipse.imp.pdb.facts.IList;
import org.eclipse.imp.pdb.facts.IListWriter;
import org.eclipse.imp.pdb.facts.IRelation;
import org.eclipse.imp.pdb.facts.IRelationWriter;
import org.eclipse.imp.pdb.facts.ISet;
import org.eclipse.imp.pdb.facts.ISetWriter;
import org.eclipse.imp.pdb.facts.IString;
import org.eclipse.imp.pdb.facts.IValue;
import org.eclipse.imp.pdb.facts.IValueFactory;
import org.eclipse.imp.pdb.facts.impl.fast.ListWriter;
import org.eclipse.imp.pdb.facts.type.Type;
import org.eclipse.imp.pdb.facts.type.TypeFactory;
import org.eclipse.imp.pdb.facts.type.TypeStore;
import org.rascalmpl.values.ValueFactoryFactory;

import com.google.protobuf.InvalidProtocolBufferException;

import Landman.Rascal.CLRInfo.Protobuf.Clrinfo.Constrain;
import Landman.Rascal.CLRInfo.Protobuf.Clrinfo.Entity;
import Landman.Rascal.CLRInfo.Protobuf.Clrinfo.EntityRel;
import Landman.Rascal.CLRInfo.Protobuf.Clrinfo.Id;
import Landman.Rascal.CLRInfo.Protobuf.Clrinfo.InformationRequest;
import Landman.Rascal.CLRInfo.Protobuf.Clrinfo.InformationResponse;
import Landman.Rascal.CLRInfo.Protobuf.Clrinfo.Id.IdKind;
import Landman.Rascal.CLRInfo.Protobuf.Clrinfo.InformationRequest.Builder;

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

	static {
		 entityDataType =TF.abstractDataType(store, "Entity");
		 idDataType =TF.abstractDataType(store, "Id");
		 constrainDataType = TF.abstractDataType(store, "Constrain");
		 resourceDataType = TF.abstractDataType(store, "Resource");
		 
		 entity = TF.constructor(store, entityDataType, "entity", TF.listType(idDataType), "id");
		 namespace = TF.constructor(store, idDataType, "namespace", TF.stringType(), "name");
		 
		 clazz = TF.constructor(store, idDataType, "class", TF.stringType(), "name");
		 genericClazz = TF.constructor(store, idDataType, "class", TF.stringType(), "name", TF.listType(entity), "params");
		 interfaze = TF.constructor(store, idDataType, "interface", TF.stringType(), "name");
		 genericInterfaze = TF.constructor(store, idDataType, "interface", TF.stringType(), "name", TF.listType(entity), "params");
		 
		 typeParameter = TF.constructor(store, idDataType, "typeParameter", TF.stringType(), "name", TF.listType(constrainDataType), "constrains");
		 enumz = TF.constructor(store, idDataType, "enum", TF.stringType(), "name");

		 none = TF.constructor(store, constrainDataType, "none");
		 isClass = TF.constructor(store, constrainDataType, "isClass");
		 isStruct = TF.constructor(store, constrainDataType, "isStruct");
		 hasDefaultConstructor = TF.constructor(store, constrainDataType, "hasDefaultConstructor");
		 implementz = TF.constructor(store, constrainDataType, "implements", entityDataType, "entity");
		 
		 entityRel = TF.aliasType(store, "EntityRel", TF.tupleType(entityDataType, "from", entityDataType, "to"));
		 
		 file = TF.constructor(store, resourceDataType, "file", TF.sourceLocationType(),"id");
		 store.declareAnnotation(resourceDataType, "types", TF.setType(entity));
		 store.declareAnnotation(resourceDataType, "implements", TF.relType(entity, entity));
		 store.declareAnnotation(resourceDataType, "extends", TF.relType(entity, entity));
		 store.declareAnnotation(resourceDataType, "calls", TF.relType(entity, entity));
	}
	
	public CLRInfoRascalBridge(IValueFactory vf) {
	}

	public static IValue readCLRInfo(IList assemblyNames) {
		try{
			ArrayList<String> actualAssemblies = new ArrayList<String>();
			IListWriter locs = VF.listWriter(TF.sourceLocationType());
			for (IValue val: assemblyNames) {
				if (val instanceof IString) {
					actualAssemblies.add(((IString)val).getValue());
					locs.append(VF.sourceLocation(((IString)val).getValue()));
				}
			}
			
			InformationResponse actualResult = getInformationFromCLR(actualAssemblies.toArray(new String[0]));
			
			IConstructor result = (IConstructor)file.make(VF, locs.done());
			result = result.setAnnotation("types", generateEntitySet(actualResult.getTypesList()));
			result = result.setAnnotation("extends", generateEntityRel(actualResult.getTypesInheritanceList()));
			result = result.setAnnotation("implements", generateEntityRel(actualResult.getTypesImplementingList()));
			return result;
		}
		catch (Exception ex)
		{
			System.err.print(ex.toString());
			return null;
		}
	}

	private static IRelation generateEntityRel(List<EntityRel> typesInheritanceList) {
		IRelationWriter result = VF.relationWriter(entityRel);
		for (EntityRel rel : typesInheritanceList) {
			result.insert(VF.tuple(generateSingleEntityArray(rel.getFrom()), generateSingleEntityArray(rel.getTo())));
		}
		return result.done();
	}

	private static ISet generateEntitySet(List<Entity> typesList) {
		ISetWriter result = VF.setWriter(entityDataType);
		 for (Entity clrEntity : typesList) {
			result.insert(generateSingleEntityArray(clrEntity));
		 }
		 return result.done();
	}
	private static IList generateEntityList(List<Entity> typesList) {
		IListWriter result = VF.listWriter(entityDataType);
		 for (Entity clrEntity : typesList) {
			result.append(generateSingleEntityArray(clrEntity));
		 }
		 return result.done();
	}


	private static IValue generateSingleEntityArray(Entity clrEntity) {
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
			case Enumeration:
				currentEntity.add(createTypeWithName(enumz, currentId));
			case TypeParameter:
				currentEntity.add(createTypeParameter(currentId));
				break;
			default:
				break;
			}
		}
		return entity.make(VF, VF.list(currentEntity.toArray(new IValue[0])));
	}

	private static IValue createTypeParameter(Id currentId) {
		assert(currentId.getKind() == IdKind.TypeParameter);
		IListWriter constrains = VF.listWriter();
		for (Constrain con : currentId.getConstrainsList())
		{
			switch (con.getKind()) {
			case Entity:
				constrains.append(implementz.make(VF, generateSingleEntityArray(con.getConstrainEntity())));
				break;
			case HasConstructor:
				constrains.append(hasDefaultConstructor.make(VF));
				break;
			case IsClass:
				constrains.append(isClass.make(VF));
				break;
			case IsStruct:
				constrains.append(isStruct.make(VF));
				break;
			default:
				break;
			}
		}
		if (currentId.getConstrainsCount() == 0)
		{
			constrains.append(none.make(VF));
		}
		return typeParameter.make(VF, VF.string(currentId.getName()), constrains.done());
	}

	private static IValue createTypeWithName(Type targetType, Id currentId) {
		return targetType.make(VF, VF.string(currentId.getName()));
	}
	
	private static IValue createTypeWithNameAndParameters(Type targetType, Id currentId) {
		return targetType.make(VF, VF.string(currentId.getName()), generateEntityList(currentId.getParamsList()));
	}

	
	
	public static void main(String[] args) throws UnknownHostException, IOException {
		//ISet result = getTypes(VF.string("../../../TestProject/bin/Debug/TestProject.exe"), VF.list());
		IValue result = readCLRInfo(VF.list(VF.string("/usr/lib/mono/2.0/System.dll")));
		
		System.out.print(((IConstructor)result).getAnnotation("implements"));
	} 
	
	private static InformationResponse getInformationFromCLR(String... assemblies)
			throws UnknownHostException, IOException,
			InvalidProtocolBufferException {
		 Socket clientSocket = new Socket("localhost", 5555);
		 DataOutputStream outToServer = new DataOutputStream(clientSocket.getOutputStream());
		 DataInputStream inFromServer = new DataInputStream(clientSocket.getInputStream());
		 Builder req = InformationRequest.newBuilder();
		 req.addAllAssemblies(Arrays.asList(assemblies));
		 InformationRequest actualRequest = req.build();
		 byte[] data = actualRequest.toByteArray();
		 outToServer.writeInt(data.length);
		 outToServer.write(data);
		 int resultSize = inFromServer.readInt();
		 byte[] result = new byte[resultSize];
		 int bytesRead = 0;
		 while (bytesRead < resultSize) {
			 bytesRead += inFromServer.read(result, bytesRead, resultSize - bytesRead);
		 }
		 InformationResponse actualResult = InformationResponse.parseFrom(result);
		return actualResult;
	}
}
