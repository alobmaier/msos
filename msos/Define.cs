﻿using CmdLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace msos
{
    [SupportedTargets(TargetType.DumpFile, TargetType.DumpFileNoHeap, TargetType.LiveProcess)]
    [Verb(".define", HelpText = "Define a helper method to be used in heap queries.")]
    class Define : ICommand
    {
        [RestOfInput(HelpText =
            "The full declaration of the helper method to define, including the parameters and " +
            "return type. For example: void Foo(ulong o) { return Object(o); }")]
        public string Declaration { get; set; }

        public void Execute(CommandExecutionContext context)
        {
            context.Defines.Add(Declaration);
        }
    }

    [SupportedTargets(TargetType.DumpFile, TargetType.DumpFileNoHeap, TargetType.LiveProcess)]
    [Verb(".listdefines", HelpText = "List the available helper methods.")]
    class ListDefines : ICommand
    {
        public void Execute(CommandExecutionContext context)
        {
            if (context.Defines.Count == 0)
            {
                context.WriteLine("You do not have any helper methods defined. Use .define to define some.");
                return;
            }
            context.WriteLine("{0,-6} {1}", "#", "Body");
            for (int i = 0; i < context.Defines.Count; ++i)
            {
                context.WriteLine("{0,-6} {1}", i, context.Defines[i]);
            }
        }
    }

    [SupportedTargets(TargetType.DumpFile, TargetType.DumpFileNoHeap, TargetType.LiveProcess)]
    [Verb(".undefine", HelpText = "Undefine a specific helper method.")]
    class Undefine : ICommand
    {
        [Value(0, Required = true, HelpText =
            "The index of the helper method to undefine. Use .listdefines to get the indexes.")]
        public int HelperIndex { get; set; }

        public void Execute(CommandExecutionContext context)
        {
            if (HelperIndex < 0 || HelperIndex >= context.Defines.Count)
            {
                context.WriteError(
                    "There is no helper method at index {0} defined. Use .listdefines to get the indexes.",
                    HelperIndex);
                return;
            }
            context.Defines.RemoveAt(HelperIndex);
        }
    }
}
