Search Expression Syntax

\===============

[![Image 1: Datalust](https://datalust.co/assets/datalust-logo-dark.svg)![Image 2: Datalust](https://datalust.co/assets/datalust-logo.svg)](https://datalust.co/)[![Image 3: Seq](https://datalust.co/assets/seq-logo-dark.svg)![Image 4: Seq](https://datalust.co/assets/seq-logo-light.svg)](https://datalust.co/)

*   [Account](https://datalust.co/u/login)

*   [What is Seq?](https://datalust.co/)

*   [Pricing](https://datalust.co/pricing)

*   [Documentation](https://datalust.co/docs)

*   [Roadmap](https://datalust.co/roadmap)

*   [Blog](https://datalust.co/blog)

*   [Discussions](https://discuss.datalust.co/)

*   [Support](mailto:support@datalust.co)

*   [Download 2025.2](https://datalust.co/download)


[Hide table of contents](https://datalust.co/docs/query-syntax#)

*   # [An Overview of Seq](https://datalust.co/docs/an-overview-of-seq)

    *   [Getting Started](https://datalust.co/docs/getting-started)

***

```
*   [What's New in 2025.2?](https://datalust.co/docs/whats-new)
```

***

```
*   [Upgrading Seq](https://datalust.co/docs/upgrading)
```

***

*   # [Getting Logs into Seq](https://datalust.co/docs/getting-logs-into-seq)

    *   [API Keys](https://datalust.co/docs/api-keys)

***

```
*   [Logging from .NET](https://datalust.co/docs/logging-from-net)
```

***

```
    *   ### [Serilog](https://datalust.co/docs/using-serilog)

    *   ### [Microsoft.Extensions.Logging](https://datalust.co/docs/microsoft-extensions-logging)

    *   ### [NLog](https://datalust.co/docs/using-nlog)

    *   ### [log4net](https://datalust.co/docs/using-log4net)

    *   ### [OpenTelemetry .NET SDK](https://datalust.co/docs/opentelemetry-net-sdk-logs)

*   [Logging from Node.js](https://datalust.co/docs/using-nodejs)
```

***

```
*   [Logging from Python](https://datalust.co/docs/using-python)
```

***

```
*   [Logging from Java](https://datalust.co/docs/using-java)
```

***

```
*   [Logging from Ruby](https://datalust.co/docs/using-ruby)
```

***

```
*   [Logging from Go](https://datalust.co/docs/using-go)
```

***

```
*   [Logging from OpenTelemetry](https://datalust.co/docs/ingestion-with-opentelemetry)
```

***

```
*   [Ingestion with HTTP](https://datalust.co/docs/posting-raw-events)
```

***

```
*   [Ingestion with GELF](https://datalust.co/docs/using-gelf)
```

***

```
*   [Collecting Docker Container Logs](https://datalust.co/docs/collecting-docker-container-logs)
```

***

```
*   [Collecting Kubernetes Logs](https://datalust.co/docs/collecting-kubernetes-logs)
```

***

```
*   [Other Log Sources](https://datalust.co/docs/seq-input-apps)
```

***

```
    *   ### [Syslog](https://datalust.co/docs/syslog)

    *   ### [RabbitMQ](https://datalust.co/docs/using-rabbitmq)

    *   ### [Windows Event Logs](https://datalust.co/docs/from-the-windows-event-log)

    *   ### [PowerShell](https://datalust.co/docs/using-powershell)

    *   ### [Scripts & Automation](https://datalust.co/docs/seqcli)

    *   ### [Log Files](https://datalust.co/docs/importing-log-files)

*   [Application Health Checks](https://datalust.co/docs/health-checks)
```

***

*   # [Getting Traces into Seq](https://datalust.co/docs/getting-traces-into-seq)

    *   [Tracing from .NET](https://datalust.co/docs/tracing-from-net)

***
# Search Expression Syntax

> Read the primer?
>
> If you're new to Seq's query language, start with [Searching and Analyzing Logs and Spans](https://datalust.co/docs/the-seq-query-language) - it's a complete syntax primer for the busy developer.

The Seq _filter bar_ can be used find events containing certain text, or having properties with particular values.

If you're just getting started with Seq, you will find that a combination of text searching (simply type what you're looking for) and filtering with the _tick_ and _cross_ icons next to event properties will cover most of what you need, as well as show you how the basics of the filter syntax works.

When you're ready to learn more, this page will introduce you to the complete syntax so you can gain the full power of Seq's filtering capabilities. It's loosely organized around a few high-level topics:

*   **[Text](https://datalust.co/docs/query-syntax#text)** - finding strings within messages
*   **[Properties and Operators](https://datalust.co/docs/query-syntax#properties-and-operators)** - comparisons useful with structured event data
*   **[Event Types](https://datalust.co/docs/query-syntax#event-types)**
*   **[Working with Dates and Times](https://datalust.co/docs/query-syntax#working-with-dates-and-times)**
*   **[Collections](https://datalust.co/docs/query-syntax#collections)** - matching items within collection-valued properties

An additional reference describes the [built-in properties](https://datalust.co/docs/built-in-properties-and-functions) and [functions](https://datalust.co/docs/scalar-functions) that can be used in filters.

## Text

The simplest text queries in Seq are text fragments typed directly into the filter bar:

![Image 5: filter-syntax](https://datalust.co/assets/3956dca-filter-syntax.png)

When Seq determines that the filter isn't a valid expression, it searches log messages for the complete string of text.

To force Seq to search for text, even if that text happens to be a valid filter expression, enclose it in `"double quotes"`:

```csharp
"logged on as contact AUTO-3af7-dadc93"
```

Double-quoted _text fragments_ use the same backslash-based `\` escape sequences as C# and JavaScript do.

> Single vs double-quoted strings
>
> While text fragments are `"double-quoted"`, Seq filters and [SQL-style queries](https://datalust.co/docs/sql-queries) use SQL-style `'single-quoted'` string literals, for example in the expression `Environment = 'Test'`.

Searches based on text fragments are case-insensitive.

#### `and`, `or`, `not`

Seq provides familiar Boolean operators including `and` (logical _and_), `or` (logical _or_) and `not` (logical negation/_not_). These can be applied directly to text expressions to form more complex queries:

```sql
"logged on" and ("HIS-e531-5eb5e3" or "AUTO-3af7-dadc93")
```

#### Regular Expressions

Full regular expression searching is supported. Regular expressions are delimited with forward-slashes. To match "hello, world" and "hold":

```javascript
/h.*d/
```

Back-slashes can be used to escape an embedded forward-slash:

```javascript
/https:\/\/datalust\.co/
```

## Properties and Operators

Structured events usually come with a rich set of properties that can be used for filtering. Expanding an event will show the available properties, and clicking the green _tick_ beside the property name provides some basic filtering options:

![Image 6: structured-event](https://datalust.co/assets/b12e343-structured-event.png)

This is a useful way to become acquainted with the operators that can be applied to event properties. _Find_ will generate a filter expression like:

```csharp
ProductId = 'product-32'
```

> _Anywhere_ you see string literals in Seq, a regular expression can also be used - so `Application = /P.*l/` works as expected here, too.

#### Listing Available Properties

The built-in _Available Properties_ SQL query, accessible in the lower-right portion of the signal bar, will list the available properties on all events matched by the current filter.

![Image 7: available-properties](https://datalust.co/assets/30c383d-available-properties.png)

#### Basic Comparisons

Seq supports the typical set of comparison operators: `=`, `<>`, `<`, `<=`, `>`, `>=` have the meanings _equal_, _not equal_, _less than_, _less than or equal_, _greater than_ and _greater than or equal_.

Strings can also be compared using the SQL `like` operator; the wildcards `%` (any number of characters) and `_` (one character) are used:

```sql
Application like 'P%'
```

Comparisons including `=` and `like` are case-sensitive. For case-insensitive matching, append the `ci` modifier:

```sql
Application like 'P%' ci
```

The `ci` modifier works everywhere in Seq, for example, `Application = 'Test'` is case-sensitive, while `Application = 'test' ci` will match `TEST`, `test`, `TeSt` and so on.

Seq also recognizes function-style operators, called as `Name(arg0, arg1, ...)`. On string-valued properties for example, `StartsWith()`, `EndsWith()` and `Contains()` are useful:

```csharp
StartsWith(Application, 'P')
```

A full list of built-in functions appears in the [Built-in Properties and Functions](https://datalust.co/docs/built-in-properties-and-functions) reference.

#### Nested Properties

Seq uses dot `.` syntax to allow nested properties on complex objects to be queried. For example `Cart.Total` references the `Total` property of the object in the `Cart` property.

#### `IN` expressions

A property value can be matched against several alternatives using `IN`:

```sql
Application in ['Web', 'API']
```

#### Conditionals

Seq supports conditional expressions using `if`/`then`/`else` syntax:

```sql
if Quantity = 0 then 'None' else 'Plenty'
```

Conditionals can be chained:

```sql
if Quantity = 0 then 'None' else if Quantity < 20 'Some' else 'Plenty'
```

## Event Types

Perhaps the most useful, but seldom-noticed benefit of a structured event is having the first-class notion of an event type.

When analyzing a traditional text-based log, there’s no concrete relationship between the messages:

```
Pre-discount tax total calculated at $0.14
Pre-discount tax total calculated at $5.20
Customer paid using CreditCard
```

From a tooling perspective, each might as well be a unique block of arbitrary text.

In a structured event from Serilog, the _message template_ passed to the logging function is preserved along with the event. Since the first two come from the template:

```csharp
"Pre-discount tax total calculated at {TaxAmount}"
```

While the third comes from:

```csharp
"Customer paid using {PaymentMethod}"
```

We can use this information to unambiguously find or exclude either kind of event. Working with message templates is verbose though, so Seq produces a 32-bit hash of the message template and makes this available in the `@EventType` built-in property.

For example `"Pre-discount tax total calculated at {TaxAmount}"` → `0xA26D9943`, while `"Customer paid using {PaymentMethod}"` → `0x4A310040`.

> The type of an event can be viewed by clicking an event to expand it. The _Type_ drop-down menu displays the event type.

To find all of the events with a specific type is easy:

```csharp
@EventType = 0x4A310040
```

## Working with Dates and Times

To select events in a given date range, clicking the "Show or hide the histogram" button just below the search box provides quick and easy time-based navigation:

![Image 8: Histogram](https://datalust.co/assets/1178461-Histogram.png)

For more complex time-based operations, the built in `@Timestamp` property and `DateTime()` functions can be used.

In a query you might write:

```sql
@Timestamp > DateTime('2014-01-03')
```

This compares the event's `@Timestamp` against a **UTC** literal. To specify a timezone use `+`/`-` syntax:

```sql
@Timestamp > DateTime('2014-01-03 +10')
```

The `DateTime()` function can construct correct date/time objects from ISO 8601 and various other formatted properties on events:

```csharp
DateTime(FinishedAt) > DateTime(ExipresAt)
```

Durations formatted in `d.HH:mm:ss.fff` syntax can be manipulated in queries using the `TotalMilliseconds()` conversion function:

```csharp
TotalMilliseconds(Elapsed) > 3000
```

For a full description of Seq's date and time manipulation functions, see [Working with Dates and Times](https://datalust.co/docs/working-with-dates-and-times).

## Collections

It’s not uncommon for structured events to carry properties that are collections. For example we might log events like:

```csharp
Log.Information("Updated {ProductId} with categories {Categories}", "product-32", new[] { "drinks", "coffees" });
```

Resulting in events like:

![Image 9: Coffees](https://datalust.co/assets/d94f783-Coffees.PNG)

In this example each event carries a collection of zero-or-more strings in the `Categories` property.

#### Numeric Indexing

As you would expect, numeric indexing is supported:

```sql
Categories[0] = 'drinks'
```

#### Wildcard Filters

To find events with a specific element in the `Categories` collection, write a filter as though testing a single item, using a wildcard in place of the index that would normally appear:

```sql
Categories[?] = 'drinks'
```

The question mark wildcard `?` matches _any_ element in the collection, while an asterisk `*` wildcard only matches if _all_ elements satisfy the condition.

Wildcards work in any comparison where one side is a property path (including indexers and dotted sub-properties). Multiple wildcards along the path are supported.

You can write the following expression to find questions where all answers are `'Yes!'`:

```sql
Answers[*].Content = 'Yes!'
```

## Cheat Sheet

Need a handy reference to keep beside your desk? We've put together a cheat sheet with filtering basics. [Download the PDF here](https://github.com/datalust/seq-cheat-sheets).

![Image 10: seq-cheat-sheet-twitter-preview](https://datalust.co/assets/3325ed3-seq-cheat-sheet-twitter-preview.png)