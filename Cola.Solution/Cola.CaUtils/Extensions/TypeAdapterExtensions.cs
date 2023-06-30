using Mapster;

namespace Cola.CaUtils.Extensions;

/// <summary>
///     TypeAdapterExtensions
/// </summary>
public static class TypeAdapterExtensions
{
    /// <summary>
    ///     集合类型对象转换类型映射
    /// </summary>
    /// <param name="source">需要转换的源对象</param>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TDestination">目标类型</typeparam>
    /// <typeparam name="T">最终映射转换后的类型</typeparam>
    /// <returns>通过映射规则转换后的对象, 具体使用请参看 readme</returns>
    public static T CollectionAdapter<TSource, TDestination, T>(this object source)
    {
        var config = new TypeAdapterConfig();
        config.NewConfig<TSource, TDestination>();
        return source.BuildAdapter(config).AdaptToType<T>();
        ;
    }

    /// <summary>
    ///     集合类型对象转换类型映射
    /// </summary>
    /// <param name="source">需要转换的源对象</param>
    /// <param name="options">自定义转换规则</param>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TDestination">目标类型</typeparam>
    /// <typeparam name="T">最终映射转换后的类型</typeparam>
    /// <returns>通过映射规则转换后的对象, 具体使用请参看 readme</returns>
    public static T CollectionAdapter<TSource, TDestination, T>(
        this object source,
        Action<TypeAdapterSetter<TSource, TDestination>>? options
    )
    {
        TypeAdapterSetter<TSource, TDestination>? adapterSetter = null;
        var config = new TypeAdapterConfig();
        adapterSetter = config.NewConfig<TSource, TDestination>();
        if (options != null)
            options(adapterSetter);
        return source.BuildAdapter(config).AdaptToType<T>();
    }

    /// <summary>
    ///     集合类型对象转换类型映射
    /// </summary>
    /// <param name="source">需要转换的源对象</param>
    /// <param name="config">全局映射规则</param>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TDestination">目标类型</typeparam>
    /// <typeparam name="T">最终映射转换后的类型</typeparam>
    /// <returns>通过映射规则转换后的对象, 具体使用请参看 readme</returns>
    public static T CollectionAdapter<TSource, TDestination, T>(
        this object source,
        TypeAdapterConfig? config
    )
    {
        if (config == null)
            config = new TypeAdapterConfig();
        return source.BuildAdapter(config).AdaptToType<T>();
    }

    /// <summary>
    ///     集合类型对象转换类型映射
    /// </summary>
    /// <param name="source">需要转换的源对象</param>
    /// <param name="options">自定义转换规则</param>
    /// <param name="config">全局映射规则</param>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TDestination">目标类型</typeparam>
    /// <typeparam name="T">最终映射转换后的类型</typeparam>
    /// <returns>通过映射规则转换后的对象, 具体使用请参看 readme</returns>
    public static T CollectionAdapter<TSource, TDestination, T>(
        this object source,
        Action<TypeAdapterSetter<TSource, TDestination>> options,
        TypeAdapterConfig config
    )
    {
        var adapterSetter = config.ForType<TSource, TDestination>();
        options(adapterSetter);
        return source.BuildAdapter(config).AdaptToType<T>();
    }


    /// <summary>
    ///     对象转换类型映射
    /// </summary>
    /// <param name="source">需要转换的源对象</param>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TDestination">目标类型</typeparam>
    /// <returns>通过映射规则转换后的对象, 具体使用请参看 readme</returns>
    public static TDestination Adapter<TSource, TDestination>(this object source)
    {
        var config = new TypeAdapterConfig();
        config.ForType<TSource, TDestination>();
        return source.BuildAdapter(config).AdaptToType<TDestination>();
    }


    /// <summary>
    ///     对象转换类型映射
    /// </summary>
    /// <param name="source">需要转换的源对象</param>
    /// <param name="options">自定义转换规则</param>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TDestination">目标类型</typeparam>
    /// <returns>通过映射规则转换后的对象, 具体使用请参看 readme</returns>
    public static TDestination Adapter<TSource, TDestination>(
        this TSource source,
        Action<TypeAdapterSetter<TSource, TDestination>>? options
    )
    {
        TypeAdapterSetter<TSource, TDestination>? adapterSetter = null;
        var config = new TypeAdapterConfig();
        adapterSetter = config.ForType<TSource, TDestination>();
        if (options != null)
            options(adapterSetter);
        return source.BuildAdapter(config).AdaptToType<TDestination>();
    }

    /// <summary>
    ///     对象转换类型映射
    /// </summary>
    /// <param name="source">需要转换的源对象</param>
    /// <param name="config">全局映射规则</param>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TDestination">目标类型</typeparam>
    /// <returns>通过映射规则转换后的对象, 具体使用请参看 readme</returns>
    public static TDestination Adapter<TSource, TDestination>(
        this TSource source,
        TypeAdapterConfig? config
    )
    {
        if (config == null)
            config = new TypeAdapterConfig();
        config.ForType<TSource, TDestination>();
        return source.BuildAdapter(config).AdaptToType<TDestination>();
    }

    /// <summary>
    ///     对象转换类型映射
    /// </summary>
    /// <param name="source">需要转换的源对象</param>
    /// <param name="options">自定义转换规则</param>
    /// <param name="config">全局映射规则</param>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TDestination">目标类型</typeparam>
    /// <returns>通过映射规则转换后的对象, 具体使用请参看 readme</returns>
    public static TDestination Adapter<TSource, TDestination>(
        this TSource source,
        Action<TypeAdapterSetter<TSource, TDestination>> options,
        TypeAdapterConfig config
    )
    {
        var adapterSetter = config.ForType<TSource, TDestination>();
        options(adapterSetter);
        return source.BuildAdapter(config).AdaptToType<TDestination>();
    }
}