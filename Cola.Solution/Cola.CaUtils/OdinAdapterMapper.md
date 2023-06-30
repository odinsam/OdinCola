**Mapster 映射转换**

1. 基础类

```csharp
public class Student
{
    public string StuName { get; set; }
    public string StuAddress { get; set; }
}
public class Student_DbModel
{
    public string StudentName { get; set; }
    public string StudentAddress { get; set; }
}
```

2. Startup.cs 注册全局映射 Config，也可以不注册

```csharp
// 使用 OdinInjectCore 注入
services.AddOdinTypeAdapter(opt =>
    {
        opt.ForType<ErrorCode_DbModel, ErrorCode_Model>()
                .Map(dest => dest.ShowMessage, src => src.CodeShowMessage);
    });
// 使用 .net core DI 注入
services.AddSingleton<ITypeAdapterMapster>(provider => new TypeAdapterMapster(opt =>
    {
        opt.ForType<ErrorCode_DbModel, ErrorCode_Model>()
                .Map(dest => dest.ShowMessage, src => src.CodeShowMessage);
    }));
```

3. 使用时获取全局注册 Config

```csharp
// 使用 OdinInjectCore 获取 TypeAdapterMapster
var mapsterConfig = OdinInjectCore.GetService<ITypeAdapterMapster>().GetConfig();
// 使用 .net core 默认 DI 获取 TypeAdapterMapster
var mapsterConfig = services.BuildServiceProvider().GetService<ITypeAdapterMapster>().GetConfig();
```

4. 获取数据准备映射转换对象

```csharp
// 通过 SqlSugar 获取数据库中的数据
List<Student_DbModel> stuDbModels = DbScoped.Sugar.Queryable<Student_DbModel>().ToList();
Student_DbModel stuDbModel = stuDbModels[0];
```

6. 对象映射转换

```csharp
// 使用全局映射配置转换目标对象类型
var stu = stuDbModel.OdinAdapter<Student_DbModel, Student>(
        OdinInjectCore.GetService<ITypeAdapterMapster>().GetConfig()
    );

// 使用自定义映射配置转换目标对象类型
// 需要注意的是: 因为没有传全局映射配置, 此时虽然全局配置也有 StudentName 属性映射 StuName 的配置，但是会以当前自定义配置为准
var stu = stuDbModel.OdinAdapter<Student_DbModel, Student>(
        opt =>
        {
            opt.Map(dest => dest.StuName, src => src.StudentName);
        }
    );

// 使用自定义映射+全局映射配置转换目标对象对象类型
// 需要注意的是: 因为全局映射配置中有 StudentName 属性映射 StuName 的配置，所以当自定义配置与全局配置都存在时，以全局配置为准
var stu = stuDbModel.OdinAdapter<Student_DbModel, Student>(
        opt =>
        {
            opt.Map(dest => dest.StuName, src => src.StudentName);
            opt.Map(dest => dest.StuAddress, src => src.StudentAddress);
        },
        OdinInjectCore.GetService<ITypeAdapterMapster>().GetConfig()
    );
```

7. 集合映射转换

   将 stuDbModels List<ErrorCode_DbModel> 集合映射转换为 stuLst List<Student> 类型的集合

泛型参数说明:

| 参数名称            | 说明         |
|:----------------|:-----------|
| Student_DbModel | 映射的源类型     |
| Student         | 转换的目标类型    |
| List<Student>   | 最终转换后的集合类型 |

```csharp
// 使用全局映射配置转换目标对象类型
var stuLst = stuDbModels.OdinCollectionAdapter<Student_DbModel, Student, List<Student>>(
        OdinInjectCore.GetService<ITypeAdapterMapster>().GetConfig()
    );

// 使用自定义映射配置转换目标对象类型
// 需要注意的是: 因为没有传全局映射配置, 此时虽然全局配置也有 StudentName 属性映射 StuName 的配置，但是会以当前自定义配置为准
var stuLst = stuDbModels.OdinCollectionAdapter<Student_DbModel, Student, List<Student>>(
        opt =>
        {
            opt.Map(dest => dest.StuName, src => src.StudentName);
        }
    );

// 使用自定义映射+全局映射配置转换目标对象对象类型
// 需要注意的是: 因为全局映射配置中有 StudentName 属性映射 StuName 的配置，所以当自定义配置与全局配置都存在时，以全局配置为准
var stuLst = stuDbModels.OdinCollectionAdapter<Student_DbModel, Student, List<Student>>(
        opt =>
        {
            opt.Map(dest => dest.StuName, src => src.StudentName);
            opt.Map(dest => dest.StuAddress, src => src.StudentAddress);
        },
        OdinInjectCore.GetService<ITypeAdapterMapster>().GetConfig()
    );
```

关于 Mapster 更详细的用法，请参照 [Mapster](https://github.com/MapsterMapper/Mapster) 官网。

具体封装代码详见 [Github](https://github.com/odinsam/OdinPlugs.Utils)
