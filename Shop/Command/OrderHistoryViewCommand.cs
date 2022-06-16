﻿using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BLL;
using Entities;

namespace Shop.Command;

public class OrderHistoryViewCommand : BasicCommand
{
    private static readonly string[] Names = { "view orders", "vo" };
    private static readonly string[] Parameters = { "-u" };
    private readonly Service _service;
    private int _id;

    public OrderHistoryViewCommand(Service service) : base(Names, Parameters)
    {
        _service = service;
    }

    public override Task<string> Execute(string[] args)
    {
        return Task<string>.Factory.StartNew(() =>
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("ID \t Description \t Is cancelled \t Is delivered\n");
            Task<IEnumerable<Order>> res;
            if (TryParseId(args))
            {
                var taskUser = _service.Factory.UserService.GetById(Shop.AuthenticationData?.Token,_id);
                res = _service.Factory.OrderService.GetUserOrders(Shop.AuthenticationData?.Token, taskUser.Result);
            }
            else
            {
                res = _service.Factory.OrderService.GetUserOrders(Shop.AuthenticationData?.Token);
            }

            foreach (var order in res.Result) stringBuilder.Append("\n" + order);

            return stringBuilder.ToString();
        });
    }

    private bool TryParseId(string[] args)
    {
        if (TryParseArgs(args, out var dict))
            return dict.TryGetValue(Parameters[0], out var id) && int.TryParse(id, out _id);
        return false;
    }

    public override string GetHelp()
    {
        return "View order history \t show orders, so \t -u";
    }
}