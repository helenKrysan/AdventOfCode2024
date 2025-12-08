
using AdventOfCode2025.Utils;

namespace AdventOfCode2025.Days
{
    internal class Day8 : IDay<long>
    {
        Dictionary<int,Jbox> _jboxes = new Dictionary<int, Jbox>();
        Dictionary<(int,int), double> _distances = new Dictionary<(int, int), double>();
        List<HashSet<int>> _connections = new List<HashSet<int>>();
        int maxConnections = 1000;
        public async Task<long> SolvePart1Async()
        {
            await ReadInput();
            long res = 0;
            // Calculate distances
            foreach (Jbox box in _jboxes.Values) {
                foreach (Jbox otherBox in _jboxes.Values)
                {
                    if (box.Id != otherBox.Id)
                    {
                        var key = (Math.Min(box.Id, otherBox.Id), Math.Max(box.Id, otherBox.Id));
                        if (!_distances.ContainsKey(key))
                        {
                            _distances[key] = box.EuclidianDistance(otherBox);
                        }
                    }
                }
            }
            var sorted = _distances.OrderBy(kv => kv.Value).ToDictionary();
            for(int i = 0; i < maxConnections; i++) {
                var kv = sorted.ElementAt(i);
                var circleExists = false;
                var idToMerge = new List<int>();
                // Check if either box is already in a connection
                for (int j = 0; j < _connections.Count; j++)
                {
                    var conn = _connections[j];
                    if (conn.Contains(kv.Key.Item1) || conn.Contains(kv.Key.Item2))
                    {
                        idToMerge.Add(j);
                        _connections[j].Add(kv.Key.Item1);
                        _connections[j].Add(kv.Key.Item2);
                        circleExists = true;
                    }
                }
                // Merge connections if needed
                if (idToMerge.Count > 0)
                {
                    var firstId = idToMerge[0];

                    for (int j = 1; j < idToMerge.Count; j++)
                    {
                        var mergeId = idToMerge[j];
                        foreach (var id in _connections[mergeId])
                        {
                            _connections[firstId].Add(id);
                        }
                    }
                    // Remove merged connections
                    for (int j = idToMerge.Count - 1; j >= 1; j--)
                    {
                        _connections.RemoveAt(idToMerge[j]);
                    }
                }
                // If neither box is in a connection, create a new one
                if (!circleExists)
                {
                    var newConn = new HashSet<int>();
                    newConn.Add(kv.Key.Item1);
                    newConn.Add(kv.Key.Item2);
                    _connections.Add(newConn);
                }
            }
            //PrintConnections();
            _connections.Sort((a,b) =>
            {
                return b.Count - a.Count;
            });
            res = _connections[0].Count * _connections[1].Count * _connections[2].Count;
            return res;
        }

        internal void PrintConnections()
        {
            foreach (var conn in _connections)
            {
                var coords = conn.Select(id => $"({_jboxes[id].X},{_jboxes[id].Y},{_jboxes[id].Z})");
                Console.WriteLine(string.Join(",", coords));
            }
        }

        public async Task<long> SolvePart2Async()
        {
            await ReadInput();
            long res = 0;
            // Calculate distances
            foreach (Jbox box in _jboxes.Values)
            {
                foreach (Jbox otherBox in _jboxes.Values)
                {
                    if (box.Id != otherBox.Id)
                    {
                        var key = (Math.Min(box.Id, otherBox.Id), Math.Max(box.Id, otherBox.Id));
                        if (!_distances.ContainsKey(key))
                        {
                            _distances[key] = box.EuclidianDistance(otherBox);
                        }
                    }
                }
            }
            var sorted = _distances.OrderBy(kv => kv.Value).ToDictionary();
            var isAllConnected = false;
            int i = 0;
            while (!isAllConnected && i < _distances.Count)
            {
                var kv = sorted.ElementAt(i);
                var circleExists = false;
                var idToMerge = new List<int>();
                for (int j = 0; j < _connections.Count; j++)
                {
                    var conn = _connections[j];
                    if (conn.Contains(kv.Key.Item1) || conn.Contains(kv.Key.Item2))
                    {
                        idToMerge.Add(j);
                        _connections[j].Add(kv.Key.Item1);
                        _connections[j].Add(kv.Key.Item2);
                        circleExists = true;
                    }
                }
                if (idToMerge.Count > 0)
                {
                    var firstId = idToMerge[0];

                    for (int j = 1; j < idToMerge.Count; j++)
                    {
                        var mergeId = idToMerge[j];
                        foreach (var id in _connections[mergeId])
                        {
                            _connections[firstId].Add(id);
                        }
                    }
                    for (int j = idToMerge.Count - 1; j >= 1; j--)
                    {
                        _connections.RemoveAt(idToMerge[j]);
                    }
                }
                if (!circleExists)
                {
                    var newConn = new HashSet<int>
                    {
                        kv.Key.Item1,
                        kv.Key.Item2
                    };
                    _connections.Add(newConn);
                }
                if (_connections.Count == 1 && _connections[0].Count == _jboxes.Count)
                {
                    isAllConnected = true;
                    var pair1 = _jboxes[kv.Key.Item1];
                    var pair2 = _jboxes[kv.Key.Item2];
                    res = pair1.X * pair2.X;
                }
                i++;
            }
            return res;
        }

        private async Task ReadInput()
        {
            var input = await ReadFileUtils.ReadFileAsync(8);
            //var input = ReadFileUtils.ReadTestFile(8);
            _jboxes = new Dictionary<int, Jbox>();
            _distances = new Dictionary<(int, int), double>();
            _connections = new List<HashSet<int>>();
            var id = 0;
            foreach (var line in input)
            {
                var parts = line.Split(',');
                _jboxes.Add(id,new Jbox(long.Parse(parts[0]), long.Parse(parts[1]), long.Parse(parts[2]),id));
                id++;
            }

        }
    }

    internal class Jbox
    {
        public long X { get; set; }
        public long Y { get; set; }
        public long Z { get; set; }
        public int Id { get; set; }
        public Jbox(long x, long y, long z, int id)
        {
            X = x;
            Y = y;
            Z = z;
            Id = id;
        }
        public double EuclidianDistance(Jbox other)
        {
            return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2) + Math.Pow(Z - other.Z, 2));
        }
    }
}
