﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1.Contracts.Services
{
    interface ICommandExecuter
    {
        void Execute(string command);
    }
}
