using Autofac;
using DAL.DataContext;

namespace DAL.Util;

public class UnitOfWorkModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        //builder.RegisterType<DbContext>().As<IDbContext>().SingleInstance();
        //using moq instead collection
        builder.RegisterInstance(new MoqDbContext().MoqDb);
        builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
    }
}