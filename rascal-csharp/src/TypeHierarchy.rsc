module TypeHierarchy

import dotNet;
import CSharp;
import Set;
import Graph;
import Relation;
import vis::Figure;
import vis::Render;
import IO;

public map[Entity e, int depth] inheritanceDepth(rel[Entity sub, Entity sup] extends) {
  t = extends+;
  return (sub:size(t[sub]) | sub <- extends.sub);
}

public set[Entity] deepestInheritance(Resource r) {
  depths = inheritanceDepth(r@extends);
  return depths<depth,e>[max(depths.depth)];
}

public set[str] deepest(Resource r) {
  return { readable(e) | e <- deepestInheritance(r) };
}

public Figure extendsGraph(Resource info) {
  subtypes = info@extends /* + info@implements*/;
  reachable = {};
  reachable = reach(subtypes, deepestInheritance(info));
  subtypes = { <from,to> | <from,to> <- subtypes, from in reachable};
  return graph([box(text(readable(e),[]), [id(readable(e)),height(30)]) | e <- carrier(subtypes)], 
               [edge(readable(from),readable(to)) | <from,to> <- subtypes ],
               [width(600), height(600)]); 
}

public Figure extendsGraph2(Resource info) {
  subtypes = info@extends /*+  info@implements */;
  reachable = {};
  reachable = reach(subtypes, top(subtypes));
  /*subtypes += top(subtypes) * {Object};*/
  subtypes = { <from,to> | <from,to> <- subtypes, from in reachable};
  
  return graph([box(text(readable(e),[]), [id(readable(e)),height(30)]) | e <- carrier(subtypes)], 
               [edge(readable(from),readable(to)) | <from,to> <- subtypes ],
               [width(2000), height(2000)]); 
}