using BLL.Objects;
using DAL.Repositories;
using Entities;

namespace BLL.Extension;

public static class Repository
{
    public static void InsertOrUpdate<TTo, TFrom>(this IRepository<TTo> repository, TFrom from) 
        where TTo : BaseEntity
        where TFrom : BaseDto
    {
        var exp = repository.GetById(from.Id);
        if (exp is not null)
        {
            exp.SetProperty(from);
            repository.Update(exp);
        }
        else
        {
            repository.Add(Mapper.Map<TTo,TFrom>(from));
        }
        
    }
}