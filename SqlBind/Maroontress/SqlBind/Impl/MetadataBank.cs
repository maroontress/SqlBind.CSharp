namespace Maroontress.SqlBind.Impl;

using System;
using System.Collections.Concurrent;

/// <summary>
/// Provides cache for the reflection.
/// </summary>
public sealed class MetadataBank
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MetadataBank"/> class.
    /// </summary>
    public MetadataBank()
    {
        Cache = new();
    }

    private ConcurrentDictionary<Type, WildMetadata> Cache { get; }

    /// <summary>
    /// Gets the metadata associated with the specified type.
    /// </summary>
    /// <param name="type">
    /// The type representing the table.
    /// </param>
    /// <returns>
    /// The metadata.
    /// </returns>
    public WildMetadata GetMetadata(Type type)
    {
        /*
            See:
            https://learn.microsoft.com/en-us/dotnet/framework/reflection-and-codedom/how-to-examine-and-instantiate-generic-types-with-reflection#to-construct-an-instance-of-a-generic-type
        */

        static WildMetadata ToMetadata(Type t)
        {
            var u = typeof(Metadata<>).MakeGenericType(t);
            return (WildMetadata)Activator.CreateInstance(u);
        }

        return Cache.GetOrAdd(type, ToMetadata);
    }

    /// <summary>
    /// Gets the metadata associated with the specified type.
    /// </summary>
    /// <typeparam name="T">
    /// The type representing the table.
    /// </typeparam>
    /// <returns>
    /// The metadata.
    /// </returns>
    public Metadata<T> GetMetadata<T>()
        where T : notnull
    {
        var o = Cache.GetOrAdd(typeof(T), t => new Metadata<T>());
        return (Metadata<T>)o;
    }
}
