using AdventOfCode2025.Utils;
namespace AdventOfCode2025.Days
{
    internal class Day11 : IDay<long>
    {
        private Dictionary<string, Node> _nodes;
        public async Task<long> SolvePart1Async()
        {
            await ReadInput();
            var currentNode = _nodes["you"];
            ProcessNode(currentNode, "out");
            return currentNode.PathToOut;
        }

        public void ProcessNode(Node currentNode, string end)
        {
            if (currentNode.ID == end)
            {
                currentNode.Processed = true;
                currentNode.PathToOut = 1;
                return;
            }
            foreach (var node in currentNode.Connected)
            {
                if (!node.Processed)
                {
                    ProcessNode(node, end);
                }
                currentNode.PathToOut += node.PathToOut;
            }
            currentNode.Processed = true;
        }

        public async Task<long> SolvePart2Async()
        {
            await ReadInput();
            ProcessNode(_nodes["dac"], "fft");
            var dac_fft = _nodes["dac"].PathToOut;

            var first = "";
            var second = "";
            if(dac_fft > 0)
            {
                first = "dac";
                second = "fft";
            }
            else
            {
                first = "fft";
                second = "dac";
            }

            await ReadInput();
            ProcessNode(_nodes["svr"], first);
            var svr_to_first = _nodes["svr"].PathToOut;

            await ReadInput();
            ProcessNode(_nodes[first], second);
            var first_to_second = _nodes[first].PathToOut; await ReadInput();

            await ReadInput();
            ProcessNode(_nodes[second], "out");
            var second_to_out = _nodes[second].PathToOut;
            return svr_to_first * first_to_second * second_to_out;
        }



        private async Task ReadInput()
        {
            var input = await ReadFileUtils.ReadFileAsync(11);
            //var input = ReadFileUtils.ReadTestFile(11);
            _nodes = new Dictionary<string, Node>();
            foreach (var line in input)
            {
                var nodeInfo = line.Split(":");
                var nodeId = nodeInfo[0];
                if (!_nodes.ContainsKey(nodeId))
                {
                    var newNode = new Node
                    {
                        ID = nodeId
                    };
                    _nodes.Add(nodeId, newNode);
                }
                var connections = nodeInfo[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                foreach (var conn in connections)
                {
                    if (!_nodes.ContainsKey(conn))
                    {
                        var newNode = new Node
                        {
                            ID = conn
                        };
                        _nodes.Add(conn, newNode);
                    }
                    _nodes[nodeId].Connected.Add(_nodes[conn]);
                }
            }
        }

        public class Node
        {
            public List<Node> Connected { get; set; } = new List<Node>();
            public string ID { get; set; }
            public long PathToOut { get; set; }
            public bool Processed { get; set; }
        }
    }
}