# Ekeel.Interop.Go

Allows for import and use of Go libraries.

```csharp
```

- [Ekeel.Interop.Go](#ekeelinteropgo)
  - [`Descriptor`](#descriptor)
  - [`EntryPoint`](#entrypoint)
  - [`Generator`](#generator)
    - [Static Methods](#static-methods)
      - [`string GenerateProject()`](#string-generateproject)
      - [`string GenerateStaticClass(Descriptor descriptor)`](#string-generatestaticclassdescriptor-descriptor)
  - [`Handler`](#handler)
    - [Constructors](#constructors)
      - [`Handler(string descriptorFile)`](#handlerstring-descriptorfile)
    - [Instance Methods](#instance-methods)
      - [`void CompileLibrary()`](#void-compilelibrary)
      - [`object GetClassInstance()`](#object-getclassinstance)
      - [`Type? GetType()`](#type-gettype)
  - [Notes](#notes)
    - [Descriptor](#descriptor-1)
    - [Go](#go)


## `Descriptor`

Class `Descriptor` represents a JSON descriptor file for a Go library interop.

__Descriptor File__

```json
{
  "namespace": "Ekeel.Interop.GoLang",
  "class": "GoHello",
  "entry_points": [
    {
      "dll": "D:\\tmp\\reverseStrGo\\reverseStrGo.dll",
      "signature": "IntPtr ReturnReversedString(byte[] input)"
    }
  ],
  "references": [
    "System"
  ]
}
```

## `EntryPoint`

Class `EntryPoint` represents an entry point for a Go library interop method.

## `Generator`

Class `Generator` generates the code for a Go library interop DLL.

### Static Methods

#### `string GenerateProject()`

> This method generates the project file for the Go library interop DLL.
> 
> Returns:
> - The project file for the Go library interop DLL.
>
> Examples:
> ```csharp
> var projectFile = Ekeel.Interop.Go.Generator.GenerateProject();
> ```

#### `string GenerateStaticClass(Descriptor descriptor)`

> This method generates the static class for the Go library interop DLL.
>
> Parameters:
> - `descriptor`
>   - The descriptor for the Go library interop.
>
> Returns:
> - The static class for the Go library interop DLL.
>
> Examples:
> ```csharp
> var descriptor = new Ekeel.Interop.Go.Descriptor();
> var staticClass = Ekeel.Interop.Go.Generator.GenerateStaticClass(descriptor);
> ```

## `Handler`

Class `Handler` handles the import of a Go library.

### Constructors

#### `Handler(string descriptorFile)`

> *This constructor initializes a new `Handler`.*
>
> Parameters:
> - `descriptorFile`
>   - The path to the descriptor file.
>
> Examples:
> ```csharp
> var goHandler = new Ekeel.Interop.Go.Handler(@"C:\tmp\descriptor.json");
> ```

### Instance Methods

#### `void CompileLibrary()`

> This method generates the code and compiles the DLL for importing the Go library specified by the descriptor.
>
> Examples:
> ```csharp
> var goHandler = new Ekeel.Interop.Go.Handler(@"C:\tmp\descriptor.json");
> goHandler.CompileLibrary();
> ```

#### `object GetClassInstance()`

> Get an instance of the generated interop class.
> 
> Returns:
> - An instance of the generated interop class referencing the Go library.
> 
> Examples:
> ```csharp
> var goHandler = new Ekeel.Interop.Go.Handler(@"C:\tmp\descriptor.json");
> goHandler.CompileLibrary();
> 
> var goInteropClass = goHandler.GetClassInstance();
> goInteropClass.SayHello();
> ```

#### `Type? GetType()`

> Get the Type of the generated go interop library.
> 
> Returns:
> - The Type of the generated go interop library.
> 
> Examples:
> ```csharp
> var goHandler = new Ekeel.Interop.Go.Handler(@"C:\tmp\descriptor.json");
> goHandler.CompileLibrary();
> 
> var type = goHandler.GetType();
> ```

## Notes

### Descriptor

```json
{
  "namespace": "Ekeel.Interop.GoLang",
  "class": "GoHello",
  "entry_points": [
    {
      "dll": "D:\\tmp\\reverseStrGo\\reverseStrGo.dll",
      "signature": "IntPtr ReturnReversedString(byte[] input)"
    }
  ],
  "references": [
    "System"
  ]
}
```

### Go

- Must contain empty main function.
- Must import "C"
- Exported functions must declare //export <FUNCTION_NAME>
- Functions should return IntPtr's

```go
package main

import "C"

//export ReturnReversedString
func ReturnReversedString(input *C.char) *C.char {
	str := C.GoString(input)
	runes := []rune(str)
	for i, j := 0, len(runes)-1; i < j; i, j = i+1, j-1 {
		runes[i], runes[j] = runes[j], runes[i]
	}
	retval := string(runes)
	return C.CString(retval)
}

func main() {}
```