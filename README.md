# CloseEnoughEquality
CloseEnoughEquality is a .net portable class library that can compare two objects for equality even if they are not of the same type.

```csharp
  var object1 = new { IntProp = 5, StringProp = "Hello" };
  var object2 = new Dictionary<string,object> { { "IntProp", "5" }, { "StringProp", "Hello" } };
  
  Assert.True(CloseEnough.Equals(object1, object2));
```

## Configuration
By default CloseEnough is somewhat stringent. It can be useful to relax some comparisons or even skip properties complete. CloseEnough can be configured by passing in a delegate to provide configuration.


### SkipProperty
Skip a property by providing a delegate to specify the exact property to skip
```csharp
  var object1 = new { IntProp = 5, StringProp = "Hello" };
  var object2 = new Dictionary<string,object> { { "IntProp", "5" }, { "StringProp", "GoodBye" } };
  
  Assert.True(CloseEnough.Equals(object1, object2, c => c.SkipProperty(p => p.StringProp));
```

### SkipPropertiesOfType
Skip properties of a specific type. You can provide a filter method for finer grained control. Open generics are supported.
```csharp
  var object1 = new { IntProp = 5, StringProp = "Hello" };
  var object2 = new Dictionary<string,object> { { "IntProp", "5" }, { "StringProp", "GoodBye" } };
  
  Assert.True(CloseEnough.Equals(object1, object2, c => c.SkipPropertiesOfType<string>());
  Assert.True(CloseEnough.Equals(object1, 
                                 object2, 
                                 c => c.SkipPropertiesOfType<object>(ForProperties.EndsWith("Prop").And.OfType<string>());
```

### DoubleEpsilon, DecimalEpsilon, FloatEpsilon
You can control the precision of double, decimal, and float comparison with the DoubleEpsilon, DecimalEpsilon, and FloatEpsilon method.
```csharp
  var object1 = new { DoubleValue = 3.0, StringProp = "Hello" };
  var object2 = new { DoubleValue = 2.99, StringProp = "Hello" };
  
  Assert.True(CloseEnough.Equals(object1, object2, c => c.DoubleEpsilon(0.01));
  Assert.True(CloseEnough.Equals(object1, object2, c => c.DoubleEpsilon(0.01, ForProperties.EndsWith("Value")));
```

### DateTimeComparisonMode
By default date comparison is down to the millisecond, comparison modes include, hour, day, month, year and more.
```csharp
 var class1 = new { Date = DateTime.Now };
 var class2 = new { Date = class1.Date };
 
 Assert.True(CloseEnough.Equals(class1, class2, c => c.DateTimeComparisonMode(DateTimeComparisonMode.Day)));
```

### StringCaseSensitive
String comparison is case sensitive by default, you can make it case insensitive for all properties or specify a properties filter.
```csharp
  var class1 = new { StringValue = "Hello" };
  var class2 = new { StringValue = "HELLO" };

  CloseEnough.Equals(class1, class2, c => c.StringCaseSensitive(false)).Should().BeTrue();
```
