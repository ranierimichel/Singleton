# Singleton
# Summary
Making a "safe" singleton is easy:

Construct a static Lazy<'T> and return its Value;

Singletons are difficult to test;

Instead of directly using a singleton, consider depending on an abstraction (interface);

Consider defining singleton lifetime in DI container.


# Course by: Dmitri Nesteruk
https://www.udemy.com/course/design-patterns-csharp-dotnet/
