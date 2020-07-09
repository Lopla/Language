# Lopla visual studio code plugin

Plugin for writing lopla code in visual studio code. Current information about lopla language can be found here: http://lopla.info 

## Features

* Keywords highlight. (if, while, function, return)
* Code completation for lopla functions. (namespaces: IO, Lp, Draw) with `ctrl + space` 

## Release Notes

### 0.0.1

- Initial release of lopla langugae plugin

### 0.0.2

- Added `Run lopla` button on lpc scripts

### 0.0.3

- Bug fixes on a lopla runner

### 0.0.4

- Added task support for lopla runner - you can now create new task in VS workspace. Also folders are supported (as a starting point). Sample task can look as below:
 ```
    "tasks": [
        {
            "type": "lopla",
            "folder": "Game",
            "label": "Game",
            "problemMatcher": []
        }
    ]
```
- Removed old button on status bar
<<<<<<< HEAD

### 0.0.6

- Smaller footprint (lopla.exe in one file)
=======
>>>>>>> 6d160ce6751457fd6d4a70d2eec890698381cbc4
