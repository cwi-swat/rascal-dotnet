module CSharp

import List;

data Entity = entity(list[Id] id);

data Id = namespace(str name)
        | class(str name)
        | class(str name, list[Entity] params)
        | interface(str name)
        | interface(str name, list[Entity] params)
        | anonymousClass(int nr) // C# anonymous classes are only internal
        | displayClass(int nr) 
        | enum(str name)
        
        | method(str name, list[Entity] params, Entity returnType)
        | constr(list[Entity] params)
        | finalizer()
        | initializer()
        | initializer(int nr)

        | field(str name)
        | property(str name, Entity setter, Entity getter)
        | event(str name)
        | parameter(str name)
        | variable(str name, int id)
        | enumConstant(str name)
        
        | primitive(PrimitiveType primType)
        | array(Entity elementType)
        
        | typeParameter(str name, list[Constrain] constrains)
        | unkown(str name)
        | wildcard() // ?
        | wildcard(Bound bound) // ?
;

data Constrain = none() 
				| isClass()
				| isStruct()
				| hasDefaultConstructor()
				| implements(Entity entity);

@doc{the root of C#'s type hierarchy}
public Entity Object = entity([namespace("System"), class("Object")]);

@doc{these are the primitive types of C# allthough they are all derived from Object}
data PrimitiveType = \bool()
	| byte()
	| sbyte()
	| char()
	| decimal()
	| double()
	| float()
	| \int()
	| uint()
	| long()
	| ulong()
	| object()
	| short()
	| ushort()
	| string()
	| \void()
;

data Bound = extends(Entity extended)
           | super(Entity super)
;

data Modifier = \public()
	| protected()
	| internal()
	| \private()
	| static()
	| abstract()
;

public str readable(Entity entity) {
	str result = "";
	list[Id] ids = entity.id;
	
	if (size(ids) > 0) {
		result = readable(head(ids));	
		for (id <- tail(ids)) {
			result += "." + readable(id);	
		}
	}
	
	
	
	return result;
}


public str readable(list[Entity] entities) {
	str result = "";
	
	if (size(entities) > 0) {
		result = readable(head(entities));
		for (Entity entity <- tail(entities)) {
			result += ", " + readable(entity);	
		}
	}
	
	return result;
}

public str readable(Id id) {
	switch (id) {
		case class(name, params):
			return name + "\<<readable(params)>\>"; 		
		case interface(name, params):
			return name + "\<<readable(params)>\>"; 		
        case method(name, params, returnType):
			return name + "(<readable(params)>)"; 	
        	
	}

	try {
		return id.name;
	} catch : ;
	
	switch (id) {
		//case anonymousClass(nr): return "anonymousClass$" + "<nr>";		
		case constr(params): return "constructor(" + readable(params) + ")";		
		case initializer: return "initializer";
		case initializer(nr): return "initializer$" + "<nr>";		
		case primitive(p): return getName(p);
		case array(elementType): return readable(elementType) + "[]";		
		case wildcard: return "?";
		case wildcard(extends(bound)): return "? extends " + readable(bound);
		case wildcard(super(bound)): return "? super " + readable(bound);
		default : throw IllegalArgument(id);
	}
}
