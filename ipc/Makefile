csharp:
	protoc --descriptor_set_out=clrinfo.protobin --include_imports clrinfo.proto
	mono protobuf-csharp-port-0.9.1/Release/ProtoGen.exe clrinfo.protobin

java:
	protoc --java_out=../CLRIPCClient/src/ clrinfo.proto
