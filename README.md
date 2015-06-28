# LazySingleton

.NET 4 provides `Lazy<T>` which allows us to implement a thread-safe singleton in C# *without the need to write an explicit double-checked locking*.  A canonical example is show below
```
/// .NET 4.0 or later example
```

Prior to .NET 4.0, double-checked locking was necessary to be thread safe.  Lots of extra code that we no longer need.
```
public sealed class Singleton
{
   private static volatile Singleton _instance;
   private static readonly object InstanceLoker= new Object();

   private Singleton() {}

   public static Singleton Instance
   {
      get 
      {
         if (_instance == null) 
         {
            lock (InstanceLoker) 
            {
               if (_instance == null) 
                  _instance = new Singleton();
            }
         }

         return _instance;
      }
   }
}
```
