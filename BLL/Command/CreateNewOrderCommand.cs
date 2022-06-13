using Entities.User;

namespace BLL.Command;

public class CreateNewOrderCommand : BasicCommand
{
    private static readonly string[] Names = { "cno", "order", "createno" };
    private static readonly string[] Parameters = { "-d", "-p" };
    private string _desc;
    private int _product;

    public CreateNewOrderCommand() : base(Names, Parameters)
    {
    }

    public override bool CanExecute(IUser user, string[] args)
    {
        User ??= user;
        return user is not null && TryParseDescAndProduct(args);
    }

    public override Task<string> Execute(string[] args=null)
    {
        return Task<string>.Factory.StartNew(()  =>
        {
            User.CreateNewOrder(_desc);
            User.Orders[^1].AddProduct(_product);
            return "OK!";
        });
    }

    private bool TryParseDescAndProduct(string[] args)
    {
        try
        {
            var dict = ParseArgs(args);
            return dict.TryGetValue(Parameters[0], out _desc)
                   && dict.TryGetValue(Parameters[1], out var indexOfProduct)
                   && int.TryParse(indexOfProduct, out _product);
        }
        catch (NullReferenceException)
        {
            return false;
        }
        catch (ArgumentNullException)
        {
            return false;
        }
        catch (IndexOutOfRangeException)
        {
            return false;
        }
    }

    public override Task<string> GetHelp()
    {
        return Task<string>.Factory.StartNew(()=>"Create new order \t cno or order or createno \t -d , -p");
    }
}