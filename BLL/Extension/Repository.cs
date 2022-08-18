using AutoMapper;
using BLL.Objects;
using DAL.Repositories;
using Entities;

namespace BLL.Extension;

public static class Repository
{
    public static void InsertOrUpdate<TTo, TFrom>(this IRepository<TTo> repository, TFrom from, IMapper mapper)
        where TTo : BaseEntity
        where TFrom : BaseDto
    {
        if (from is null)
            return;
        var exp = repository.GetById(from.Id);
        if (exp is not null)
            repository.Update(mapper.Map<TFrom, TTo>(from));
        else
            repository.Add(mapper.Map<TFrom, TTo>(from));
    }
}