# Initial project setup

This project uses .NET Core on Linux.

```
dotnet new console -lang f#
dotnet restore
```

# Developing the application

Major parts of the development is done using FSI, the F# Interactive shell. From within the `Scratchpad.fsx` file, highlight some code and send it to the interactive shell using `Alt + Enter`.

# Compiling the application

```
dotnet build
```
# Running the final application

```
dotnet run
```

# Main lesson learned

Partially applying a curried function. See `auditAs` function in `Operation.fs`.