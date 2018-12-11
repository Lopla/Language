# Lopla language

[[_TOC_]]

# Syntax
Lines of code are sperated by new line.
# Keywords

## conditional execution
```
a=0
if(a==0){

}
```
## loop
```
a=10
while(a>0){
    a=a-1
}
```

## function declaration
```
a=2
b="test"
function namespace.name(a,b){
    return a
}
```
## function call
```
namespace.name()
```

# Types and variables
There are tree types in Lopla:
## Number 
Used to store any real number 
```
a=0
a=-1
a=122.12
```
## String 

Any list of characters 
``` 
b="a" 
```

## LoplaList

list of any types.
```
a=["one", "two", "..."]
b=[1, 2, 3]
c=["one", 2 , "4"]
d=[]
e=["one", 2, c]
```

## Scopes
All variables defined on file level are availble only as global variables wihion one file.

`a.lpc`
```
a=123

function a.test1(){
    IO.Write(a)
    return a
}
```

`b.lpc`
```
function b.test1(){
    /* line below causes error */
    IO.Write(a)
}
```

# Comments
All characters (multiline) between `/*` and `*/` are treated as comments.
