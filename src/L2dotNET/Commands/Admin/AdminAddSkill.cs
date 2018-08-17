﻿using System;
using System.Threading.Tasks;
using L2dotNET.Attributes;
using L2dotNET.Models.Player;

namespace L2dotNET.Commands.Admin
{
    [Command(CommandName = "setskill")]
    class AdminAddSkill : AAdminCommand
    {
        protected internal override async Task UseAsync(L2Player admin, string alias)
        {
            await Task.FromResult(1);
        }

        public AdminAddSkill(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}