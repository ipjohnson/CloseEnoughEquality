# CloseEnoughEquality
CloseEnoughEquality is a .net portable class library that can compare two objects for equality even if they are not of the same type.

```csharp
  var object1 = new { IntProp = 5, StringProp = "Hello" };
  var object2 = new Dictionary<string,object> { { "IntProp", "5" }, { "StringProp", "Hello" } };
  
  Assert.True(CloseEnough.Equals(object1, object2));
```

