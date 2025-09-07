# MyCustomDictionary

A custom implementation of a generic dictionary in C# with thread safety, rehashing, and common dictionary operations.

## Features

- Add, Remove, and Lookup by Key and Value
- Handles collisions via chaining (linked lists)
- Thread-safe with locking
- Supports enumeration of keys and values
- Rehashes when load factor exceeds threshold (default 0.9)
- Indexer support for easy access and modification
- Comprehensive NUnit test coverage including thread safety and edge cases

## Performance Benchmark

| Operation       | MyCustomDictionary | .NET Dictionary |
|-----------------|--------------------|-----------------|
| Add (1 million) | ~90 ms             | ~50 ms          |

*Performance results may vary based on hardware and runtime.*

## Usage

```csharp
var dict = new MyCustomDictionary<int, string>();
dict.Add(1, "one");

if (dict.GetValue(1, out var value))
{
    Console.WriteLine(value); // Output: one
}

// Using indexer
dict[2] = "two";
Console.WriteLine(dict[2]); // Output: two

// Enumerate all keys
foreach (var key in dict.GetAllKeys())
{
    Console.WriteLine(key);
}


NUnit Tests

The project includes NUnit tests that cover:

- Adding new entries and handling duplicates

- Removing by key and by value (single and multiple)

- Retrieving values by key

- Checking key containment

- Clearing the dictionary

- Enumeration correctness

- Rehashing behavior after load factor is exceeded

- Thread safety with concurrent add operations

Run tests using your favorite test runner or the dotnet test CLI command.

License

This project is open source under the MIT License.



---

