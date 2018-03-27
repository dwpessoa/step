// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace IxMilia.Step.SchemaParser

type Resource(name:string, alias:string option) =
    member this.Name = name
    member this.Alias = alias

type ReferenceClause(schemaName:string, resources:Resource list option) =
    member this.SchemaName = schemaName
    member this.Resources = resources

type LiteralValue =
    | IntegerLiteral of int64
    | LogicalLiteral of bool option
    | RealLiteral of float
    | StringLiteral of string

type Expression =
    // static values
    | LiteralValue of LiteralValue
    | AttributeName of string
    // artithmetic
    | Negate of Expression
    | Add of Expression * Expression
    | Subtract of Expression * Expression
    | Multiply of Expression * Expression
    | Divide of Expression * Expression
    | Modulus of Expression * Expression
    | Exponent of Expression * Expression
    // other
    | Or of Expression * Expression
    | Xor of Expression * Expression
    | And of Expression * Expression

type SimpleType =
    | BinaryType of Expression option * bool // width * isFixed
    | BooleanType
    | IntegerType
    | LogicalType
    | NumberType
    | RealType of Expression option // precision
    | StringType of Expression option * bool // width * isFixed

type ConstructedType =
    | EnumerationType of string list
    | SelectType of BaseType list

and AggregationType =
    | ArrayType of BaseType * Expression * Expression option * bool * bool // type * lowerBound * upperBound * isOptional * isUnique
    | BagType of BaseType * Expression * Expression option // type * lowerBound * upperBound
    | ListType of BaseType * Expression * Expression option * bool // type * lowerBound * upperBound * isUnique
    | SetType of BaseType * Expression * Expression option // type * lowerBound * upperBound

and BaseType =
    | ConstructedType of ConstructedType
    | AggregationType of AggregationType
    | SimpleType of SimpleType
    | NamedType of string

type AttributeType(typ:BaseType, isOptional:bool) =
    member this.Type = typ
    member this.IsOptional = isOptional

type DerivedAttribute(name:string, typ:AttributeType, expression:Expression) =
    member this.Name = name
    member this.Type = typ
    member this.Expression = expression

type ExplicitAttribute(name:string, typ:AttributeType) =
    member this.Name = name
    member this.Type = typ

type Entity(name:string, attributes:ExplicitAttribute list, derivedAttributes:DerivedAttribute list) =
    member this.Name = name
    member this.Attributes = attributes
    member this.DerivedAttributes = derivedAttributes

type DomainRule(label:string, expression:Expression) =
    member this.Label = label
    member this.Expression = expression

type SchemaType(name:string, typ:BaseType, domainRules:DomainRule list) =
    member this.Name = name
    member this.Type = typ
    member this.DomainRules = domainRules

type Declaration =
    | EntityDeclaration of Entity
    | TypeDeclaration of SchemaType

type Constant(id:string, typ:BaseType, expression:Expression) =
    member this.Id = id
    member this.Type = typ
    member this.Expression = expression

type SchemaBody(interfaces:ReferenceClause list, constants:Constant list, entities:Entity list, types:SchemaType list) =
    member this.Interfaces = interfaces
    member this.Constants = constants
    member this.Entities = entities
    member this.Types = types

type Schema(id:string, version:string, body:SchemaBody) =
    member this.Id = id
    member this.Version = version
    member this.Body = body
    member this.Interfaces = body.Interfaces
    member this.Constants = body.Constants
    member this.Entities = body.Entities
    member this.Types = body.Types