using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace ConsoleApp
{

    internal class Day07 : IDay
    {

        class Wire
        {
            public bool SignalArrived = false;
            public string Name { get; set; }
            public Gate InputSignal;

            public ushort? CachedSignal;

            public ushort Signal => CachedSignal ??= InputSignal.Output;

        }

        interface Gate
        {
            public string Name { get; set; }
            public ushort Output { get; }
            public Wire[] Input
            {
                get;
                set;
            }
        }

        class InputGate(ushort state) : Gate
        {
            public string Name { get; set; }


            public ushort Output => state;
            public Wire[] Input { get; set; }
        }

        class ShiftGate(int shift) : Gate
        {
            public string Name { get; set; }
            public Wire[] Input
            {
                get;
                set;
            }

            public ushort Output => (ushort)(shift > 0 ? Input[0].Signal >> shift : Input[0].Signal << -shift);
        }

        class NotGate : Gate
        {
            public string Name { get; set; }
            public Wire[] Input
            {
                get;
                set;
            }

            public ushort Output => (ushort)~Input[0].Signal;
        }
        class OrGate : Gate
        {
            public string Name { get; set; }
            public Wire[] Input
            {
                get;
                set;
            }

            public ushort Output => (ushort)(Input[0].Signal | Input[1].Signal);
        }
        class AndGate : Gate
        {
            public string Name { get; set; }
            public Wire[] Input
            {
                get;
                set;
            }
            public ushort Output => (ushort)(Input[0].Signal & Input[1].Signal);
        }



        public void ReadInput()
        {

            var filename = Debugger.IsAttached ? "Example.txt" : "Input.txt";
            var data = File.ReadAllLines($"days/{GetType().Name.Substring(3)}/{filename}");

            foreach (var line in data)
            {
                var tokens = line.Split(' ');

                if (tokens[0] == "NOT")
                {
                    var outputWire = GetOrAddWire(tokens[3]);
                    var inputWire = GetOrAddWire(tokens[1]);

                    var gate = new NotGate()
                    {
                        Input = [inputWire],
                        Name = line
                    };
                    outputWire.InputSignal = gate;
                }
                else if (tokens[1] == "LSHIFT")
                {
                    var outputWire = GetOrAddWire(tokens[4]);
                    var inputWire1 = GetOrAddWire(tokens[0]);
                    var shiftLength = int.Parse(tokens[2]);

                    var gate = new ShiftGate(-shiftLength)
                    {
                        Input = [inputWire1],
                        Name = line
                    };
                    outputWire.InputSignal = gate;
                }
                else if (tokens[1] == "RSHIFT")
                {
                    var outputWire = GetOrAddWire(tokens[4]);
                    var inputWire1 = GetOrAddWire(tokens[0]);
                    var shiftLength = int.Parse(tokens[2]);

                    var gate = new ShiftGate(shiftLength)
                    {
                        Input = [inputWire1],
                        Name = line
                    };
                    outputWire.InputSignal = gate;
                }
                else if (tokens[1] == "AND")
                {
                    var outputWire = GetOrAddWire(tokens[4]);
                    var inputWire1 = GetOrAddWire(tokens[0]);
                    var inputWire2 = GetOrAddWire(tokens[2]);

                    var gate = new AndGate()
                    {
                        Input = [inputWire1, inputWire2],
                        Name = line
                    };
                    outputWire.InputSignal = gate;
                }
                else if (tokens[1] == "OR")
                {
                    var outputWire = GetOrAddWire(tokens[4]);
                    var inputWire1 = GetOrAddWire(tokens[0]);
                    var inputWire2 = GetOrAddWire(tokens[2]);

                    var gate = new OrGate()
                    {
                        Input = [inputWire1, inputWire2],
                        Name = line
                    };
                    outputWire.InputSignal = gate;
                }

                else
                {
                    if (ushort.TryParse(tokens[0], out var number))
                    {
                        var outputWire = GetOrAddWire(tokens[2]);
                        var gate = new InputGate(number) { Name = line };
                        outputWire.InputSignal = gate;
                    }
                    else
                    {
                        var outputWire = GetOrAddWire(tokens[2]);
                        var inputWire1 = GetOrAddWire(tokens[0]);

                        var gate = new ShiftGate(0)
                        {
                            Input = [inputWire1],
                            Name = line
                        };
                        outputWire.InputSignal = gate;
                    }
                }
            }

        }


        private Wire GetOrAddWire(string wireName)
        {
            if (wires.TryGetValue(wireName, out var wire)) return wire;

            var newWire = new Wire { Name = wireName };
            if (ushort.TryParse(wireName, out var number))
            {
                newWire.InputSignal = new InputGate(number);
            }

            wires.Add(wireName, newWire);
            return newWire;

        }

        private Dictionary<string, Wire> wires = new();




        public decimal Part1()
        {
            return wires.TryGetValue("a", out var wire) ? wire.Signal : 0;
        }

        public decimal Part2()
        {
            

            wires["b"].InputSignal = new InputGate(wires["a"].Signal);

            foreach (var wiresValue in wires.Values)
            {
                wiresValue.CachedSignal = null;
            }

           
            return wires["a"].Signal;
        }
    }
}
