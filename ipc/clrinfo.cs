//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: clrinfo.proto
namespace Landman.Rascal.CLRInfo.Protobuf
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"EntityRel")]
  public partial class EntityRel : global::ProtoBuf.IExtensible
  {
    public EntityRel() {}
    
    private Landman.Rascal.CLRInfo.Protobuf.Entity _From;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"From", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public Landman.Rascal.CLRInfo.Protobuf.Entity From
    {
      get { return _From; }
      set { _From = value; }
    }
    private Landman.Rascal.CLRInfo.Protobuf.Entity _To;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"To", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public Landman.Rascal.CLRInfo.Protobuf.Entity To
    {
      get { return _To; }
      set { _To = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ModifierRel")]
  public partial class ModifierRel : global::ProtoBuf.IExtensible
  {
    public ModifierRel() {}
    
    private Landman.Rascal.CLRInfo.Protobuf.Entity _Entity;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"Entity", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public Landman.Rascal.CLRInfo.Protobuf.Entity Entity
    {
      get { return _Entity; }
      set { _Entity = value; }
    }
    private Landman.Rascal.CLRInfo.Protobuf.Modifier _Modifier;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"Modifier", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public Landman.Rascal.CLRInfo.Protobuf.Modifier Modifier
    {
      get { return _Modifier; }
      set { _Modifier = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Entity")]
  public partial class Entity : global::ProtoBuf.IExtensible
  {
    public Entity() {}
    
    private readonly global::System.Collections.Generic.List<Landman.Rascal.CLRInfo.Protobuf.Id> _Ids = new global::System.Collections.Generic.List<Landman.Rascal.CLRInfo.Protobuf.Id>();
    [global::ProtoBuf.ProtoMember(1, Name=@"Ids", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<Landman.Rascal.CLRInfo.Protobuf.Id> Ids
    {
      get { return _Ids; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Id")]
  public partial class Id : global::ProtoBuf.IExtensible
  {
    public Id() {}
    
    private Landman.Rascal.CLRInfo.Protobuf.Id.IdKind _Kind;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"Kind", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public Landman.Rascal.CLRInfo.Protobuf.Id.IdKind Kind
    {
      get { return _Kind; }
      set { _Kind = value; }
    }

    private string _Name = "";
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"Name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string Name
    {
      get { return _Name; }
      set { _Name = value; }
    }
    private readonly global::System.Collections.Generic.List<Landman.Rascal.CLRInfo.Protobuf.Entity> _Params = new global::System.Collections.Generic.List<Landman.Rascal.CLRInfo.Protobuf.Entity>();
    [global::ProtoBuf.ProtoMember(3, Name=@"Params", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<Landman.Rascal.CLRInfo.Protobuf.Entity> Params
    {
      get { return _Params; }
    }
  

    private Landman.Rascal.CLRInfo.Protobuf.Entity _ReturnType = null;
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"ReturnType", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public Landman.Rascal.CLRInfo.Protobuf.Entity ReturnType
    {
      get { return _ReturnType; }
      set { _ReturnType = value; }
    }

    private Landman.Rascal.CLRInfo.Protobuf.Entity _Setter = null;
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"Setter", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public Landman.Rascal.CLRInfo.Protobuf.Entity Setter
    {
      get { return _Setter; }
      set { _Setter = value; }
    }

    private Landman.Rascal.CLRInfo.Protobuf.Entity _Getter = null;
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"Getter", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public Landman.Rascal.CLRInfo.Protobuf.Entity Getter
    {
      get { return _Getter; }
      set { _Getter = value; }
    }

    private int __Id = default(int);
    [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name=@"_Id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(default(int))]
    public int _Id
    {
      get { return __Id; }
      set { __Id = value; }
    }

    private Landman.Rascal.CLRInfo.Protobuf.Id.PrimitiveType __PrimitiveType = Landman.Rascal.CLRInfo.Protobuf.Id.PrimitiveType.Bool;
    [global::ProtoBuf.ProtoMember(8, IsRequired = false, Name=@"_PrimitiveType", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    [global::System.ComponentModel.DefaultValue(Landman.Rascal.CLRInfo.Protobuf.Id.PrimitiveType.Bool)]
    public Landman.Rascal.CLRInfo.Protobuf.Id.PrimitiveType _PrimitiveType
    {
      get { return __PrimitiveType; }
      set { __PrimitiveType = value; }
    }

    private Landman.Rascal.CLRInfo.Protobuf.Entity _ElementType = null;
    [global::ProtoBuf.ProtoMember(9, IsRequired = false, Name=@"ElementType", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public Landman.Rascal.CLRInfo.Protobuf.Entity ElementType
    {
      get { return _ElementType; }
      set { _ElementType = value; }
    }
    private readonly global::System.Collections.Generic.List<Landman.Rascal.CLRInfo.Protobuf.Constrain> _Constrains = new global::System.Collections.Generic.List<Landman.Rascal.CLRInfo.Protobuf.Constrain>();
    [global::ProtoBuf.ProtoMember(10, Name=@"Constrains", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<Landman.Rascal.CLRInfo.Protobuf.Constrain> Constrains
    {
      get { return _Constrains; }
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"IdKind")]
    public enum IdKind
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"Namespace", Value=0)]
      Namespace = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Class", Value=1)]
      Class = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"GenericClass", Value=2)]
      GenericClass = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Interface", Value=3)]
      Interface = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"GenericInterface", Value=4)]
      GenericInterface = 4,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Enumeration", Value=5)]
      Enumeration = 5,
            
      [global::ProtoBuf.ProtoEnum(Name=@"AnonymousClass", Value=6)]
      AnonymousClass = 6,
            
      [global::ProtoBuf.ProtoEnum(Name=@"DisplayClass", Value=7)]
      DisplayClass = 7,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Method", Value=10)]
      Method = 10,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Constructor", Value=11)]
      Constructor = 11,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Finalizer", Value=12)]
      Finalizer = 12,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Initializer", Value=13)]
      Initializer = 13,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Field", Value=20)]
      Field = 20,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Property", Value=21)]
      Property = 21,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Event", Value=22)]
      Event = 22,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Parameter", Value=23)]
      Parameter = 23,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Variable", Value=24)]
      Variable = 24,
            
      [global::ProtoBuf.ProtoEnum(Name=@"EnumConstant", Value=25)]
      EnumConstant = 25,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Primitive", Value=30)]
      Primitive = 30,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Array", Value=31)]
      Array = 31,
            
      [global::ProtoBuf.ProtoEnum(Name=@"TypeParameter", Value=40)]
      TypeParameter = 40,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Unkown", Value=41)]
      Unkown = 41,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Wildcard", Value=42)]
      Wildcard = 42
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"PrimitiveType")]
    public enum PrimitiveType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"Bool", Value=0)]
      Bool = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Byte", Value=1)]
      Byte = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Sbyte", Value=2)]
      Sbyte = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Char", Value=3)]
      Char = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Decimal", Value=4)]
      Decimal = 4,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Double", Value=5)]
      Double = 5,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Float", Value=6)]
      Float = 6,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Int", Value=7)]
      Int = 7,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Uint", Value=8)]
      Uint = 8,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Long", Value=9)]
      Long = 9,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Ulong", Value=10)]
      Ulong = 10,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Object", Value=11)]
      Object = 11,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Short", Value=12)]
      Short = 12,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Ushort", Value=13)]
      Ushort = 13,
            
      [global::ProtoBuf.ProtoEnum(Name=@"String", Value=14)]
      String = 14,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Void", Value=15)]
      Void = 15
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Constrain")]
  public partial class Constrain : global::ProtoBuf.IExtensible
  {
    public Constrain() {}
    
    private Landman.Rascal.CLRInfo.Protobuf.Constrain.ConstrainKind _Kind;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"Kind", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public Landman.Rascal.CLRInfo.Protobuf.Constrain.ConstrainKind Kind
    {
      get { return _Kind; }
      set { _Kind = value; }
    }

    private Landman.Rascal.CLRInfo.Protobuf.Entity _ConstrainEntity = null;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"ConstrainEntity", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public Landman.Rascal.CLRInfo.Protobuf.Entity ConstrainEntity
    {
      get { return _ConstrainEntity; }
      set { _ConstrainEntity = value; }
    }
    [global::ProtoBuf.ProtoContract(Name=@"ConstrainKind")]
    public enum ConstrainKind
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"None", Value=0)]
      None = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Entity", Value=1)]
      Entity = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"IsClass", Value=2)]
      IsClass = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"IsStruct", Value=3)]
      IsStruct = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"HasConstructor", Value=4)]
      HasConstructor = 4
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"InformationRequest")]
  public partial class InformationRequest : global::ProtoBuf.IExtensible
  {
    public InformationRequest() {}
    
    private readonly global::System.Collections.Generic.List<string> _Assemblies = new global::System.Collections.Generic.List<string>();
    [global::ProtoBuf.ProtoMember(1, Name=@"Assemblies", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<string> Assemblies
    {
      get { return _Assemblies; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"InformationResponse")]
  public partial class InformationResponse : global::ProtoBuf.IExtensible
  {
    public InformationResponse() {}
    
    private readonly global::System.Collections.Generic.List<Landman.Rascal.CLRInfo.Protobuf.Entity> _Types = new global::System.Collections.Generic.List<Landman.Rascal.CLRInfo.Protobuf.Entity>();
    [global::ProtoBuf.ProtoMember(1, Name=@"Types", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<Landman.Rascal.CLRInfo.Protobuf.Entity> Types
    {
      get { return _Types; }
    }
  
    private readonly global::System.Collections.Generic.List<Landman.Rascal.CLRInfo.Protobuf.Entity> _Methods = new global::System.Collections.Generic.List<Landman.Rascal.CLRInfo.Protobuf.Entity>();
    [global::ProtoBuf.ProtoMember(2, Name=@"Methods", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<Landman.Rascal.CLRInfo.Protobuf.Entity> Methods
    {
      get { return _Methods; }
    }
  
    private readonly global::System.Collections.Generic.List<Landman.Rascal.CLRInfo.Protobuf.EntityRel> _TypesInheritance = new global::System.Collections.Generic.List<Landman.Rascal.CLRInfo.Protobuf.EntityRel>();
    [global::ProtoBuf.ProtoMember(10, Name=@"TypesInheritance", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<Landman.Rascal.CLRInfo.Protobuf.EntityRel> TypesInheritance
    {
      get { return _TypesInheritance; }
    }
  
    private readonly global::System.Collections.Generic.List<Landman.Rascal.CLRInfo.Protobuf.EntityRel> _TypesImplementing = new global::System.Collections.Generic.List<Landman.Rascal.CLRInfo.Protobuf.EntityRel>();
    [global::ProtoBuf.ProtoMember(11, Name=@"TypesImplementing", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<Landman.Rascal.CLRInfo.Protobuf.EntityRel> TypesImplementing
    {
      get { return _TypesImplementing; }
    }
  
    private readonly global::System.Collections.Generic.List<Landman.Rascal.CLRInfo.Protobuf.EntityRel> _MethodCalls = new global::System.Collections.Generic.List<Landman.Rascal.CLRInfo.Protobuf.EntityRel>();
    [global::ProtoBuf.ProtoMember(20, Name=@"MethodCalls", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<Landman.Rascal.CLRInfo.Protobuf.EntityRel> MethodCalls
    {
      get { return _MethodCalls; }
    }
  
    private readonly global::System.Collections.Generic.List<Landman.Rascal.CLRInfo.Protobuf.ModifierRel> _Modifiers = new global::System.Collections.Generic.List<Landman.Rascal.CLRInfo.Protobuf.ModifierRel>();
    [global::ProtoBuf.ProtoMember(30, Name=@"Modifiers", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<Landman.Rascal.CLRInfo.Protobuf.ModifierRel> Modifiers
    {
      get { return _Modifiers; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
    [global::ProtoBuf.ProtoContract(Name=@"Modifier")]
    public enum Modifier
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"Public", Value=0)]
      Public = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Protected", Value=1)]
      Protected = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Internal", Value=2)]
      Internal = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Private", Value=3)]
      Private = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Static", Value=4)]
      Static = 4,
            
      [global::ProtoBuf.ProtoEnum(Name=@"Abstract", Value=5)]
      Abstract = 5
    }
  
}
