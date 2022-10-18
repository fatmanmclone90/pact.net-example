﻿using PactNet.Infrastructure.Outputters;
using Xunit.Abstractions;

namespace pact.consumer.test
{
    public class XUnitOutput : IOutput
    {
        private readonly ITestOutputHelper output;

        public XUnitOutput(ITestOutputHelper output)
        {
            this.output = output;
        }

        public void WriteLine(string line)
        {
            this.output.WriteLine(line);
        }
    }
}
