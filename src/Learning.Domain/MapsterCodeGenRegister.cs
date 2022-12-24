using System.Reflection;

using Mapster;

namespace Learning.Domain;

public sealed class MapsterCodeGenRegister : ICodeGenerationRegister
{
    public void Register(CodeGenerationConfig config)
    {
        config.AdaptTo("[name]Dto")
            .ForAllTypesInNamespace(Assembly.GetExecutingAssembly(), "Learning.Domain.DTOs");
    }
}
