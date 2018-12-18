# Lopla language

# Syntax
Lines of code are sperated by new line.

<a name="Keywords"/>
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
There is a required convention on how to specify a function name. Each function name should have `namespace` part (which is the filename) `.` and  `name`. In the same line as the declaraion we should provide arguments and starting bracket `{` for method body.

`File.lpc`
```
a=2
b="test"
function File.FunctionName(a,b){
    return a
}
```
## function call
```
Namespce.Name()
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
### Indexing operator and strings
It is possible to modify strings by indexing operator: `[]`. Empty `' '` characters will be added if you will add your value at some nonexisitient index.
```
b="a" 
b[0]=1
b[2]=34

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
