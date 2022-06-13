using System.Text;
using Entities.User;

namespace BLL.Command;

public class ViewOrderHistoryCommand : BasicCommand
{
    private static readonly string[] Names = { "view orders", "vo" };
    private static readonly string[] Parameters = { "-id" };
    private int _id;

    public ViewOrderHistoryCommand() : base(Names, Parameters)
    {
    }

    public override bool CanExecute(IUser user, string[] args=null)
    {
        User ??= user;
        return user is RegisteredUser;
    }

    public override Task<string> Execute(string[] args=null)
    {
        return Task<string>.Factory.StartNew(() =>
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("ID \t Description \t Is cancelled \t Is delivered\n");
            foreach (var order in User.Orders)
            {
                stringBuilder.Append(order);
            }

            return stringBuilder.ToString();
        });
    }

    private bool TryParseId(string[] args)
    {
        try
        {
            var dict = ParseArgs(args);
            return dict.TryGetValue(Parameters[0], out var id)
                   && int.TryParse(id, out _id);
        }
        catch (NullReferenceException)
        {
            return false;
        }
        catch (ArgumentNullException)
        {
            return false;
        }
    }

    public override Task<string> GetHelp()
    {
        return Task<string>.Factory.StartNew(() =>"View order history \t show orders, so \t none");
    }
}