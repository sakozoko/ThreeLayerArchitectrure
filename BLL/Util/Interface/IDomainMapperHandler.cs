using AutoMapper;

namespace BLL.Util.Interface
{
    public interface IDomainMapperHandler
    {
        IMapper GetMapper();
    }
}