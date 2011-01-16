module dotNet

import CSharp;

@doc{relationship between entities}
public alias EntityRel = rel[Entity from, Entity to];

@doc{collection of entities}
public alias EntitySet = set[Entity];

@doc{maps an entity to its modifiers}
public alias ModifierRel = rel[Entity entity, Modifier modifier];

public alias ConstrainRel = rel[Entity entity, Constrain constrain];

data Resource = file(list[loc] id);

anno EntitySet Resource@types;
anno EntitySet Resource@methods;
anno EntityRel Resource@implements;
anno EntityRel Resource@extends;
  
anno EntityRel Resource@calls; 

anno ConstrainRel Resource@genericConstrains;
anno ModifierRel Resource@modifiers;