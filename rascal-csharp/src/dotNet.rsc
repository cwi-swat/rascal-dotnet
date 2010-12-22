module dotNet

import CSharp;

@doc{relationship between entities}
public alias EntityRel = rel[Entity from, Entity to];

@doc{collection of entities}
public alias EntitySet = set[Entity];

@doc{maps an entity to its modifiers}
public alias ModifierRel = rel[Entity entity, Modifier modifier];

data Resource = file(loc id);

anno EntitySet Resource@types;
anno EntityRel Resource@implements;
anno EntityRel Resource@extends;  
anno EntityRel Resource@calls; 