using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FortuneBag
{
    enum GameSchema { Unknown, LaPlace, EdenEternal, GrandFantasia }

    class BagRow
    {
        public int Id;
        public int Sequence;
        public int Set;
        public int ItemId;
        public int ItemNum;
        public float Probability;
        public float AchProbability;
        public float ItemCounter;
        public int Bulletin;
        public int Embedded;
        public int Bind;
        public float White;
        public float Green;
        public float Blue;
        public float Yellow;
        public string Note = "";
        public GameSchema Schema;

        public string[] ToColumns()
        {
            switch (Schema)
            {
                case GameSchema.LaPlace:
                    return new[] {
                        Id.ToString(), Sequence.ToString(), Set.ToString(),
                        ItemId.ToString(), ItemNum.ToString(),
                        Probability.ToString(System.Globalization.CultureInfo.InvariantCulture),
                        AchProbability.ToString(System.Globalization.CultureInfo.InvariantCulture),
                        ItemCounter.ToString(System.Globalization.CultureInfo.InvariantCulture),
                        Bulletin.ToString(), Note
                    };
                case GameSchema.EdenEternal:
                    return new[] {
                        Id.ToString(), Sequence.ToString(), Set.ToString(),
                        ItemId.ToString(), ItemNum.ToString(),
                        Probability.ToString(System.Globalization.CultureInfo.InvariantCulture),
                        ItemCounter.ToString(System.Globalization.CultureInfo.InvariantCulture),
                        Bulletin.ToString(), Embedded.ToString(), Bind.ToString(), Note
                    };
                case GameSchema.GrandFantasia:
                    return new[] {
                        Id.ToString(), Sequence.ToString(), Set.ToString(),
                        ItemId.ToString(), ItemNum.ToString(),
                        Probability.ToString(System.Globalization.CultureInfo.InvariantCulture),
                        Bulletin.ToString(),
                        White.ToString(System.Globalization.CultureInfo.InvariantCulture),
                        Green.ToString(System.Globalization.CultureInfo.InvariantCulture),
                        Blue.ToString(System.Globalization.CultureInfo.InvariantCulture),
                        Yellow.ToString(System.Globalization.CultureInfo.InvariantCulture),
                        Note
                    };
                default:
                    return new[] { Id.ToString(), Sequence.ToString(), Note };
            }
        }
    }

    class SqlParser
    {
        public GameSchema Schema { get; private set; }

        static readonly System.Globalization.CultureInfo Inv = System.Globalization.CultureInfo.InvariantCulture;

        public List<BagRow> Parse(string filePath)
        {
            Encoding big5 = Encoding.GetEncoding("big5");
            string[] lines = File.ReadAllLines(filePath, big5);

            bool inCopy = false;
            var rows = new List<BagRow>();
            Schema = GameSchema.Unknown;

            foreach (string rawLine in lines)
            {
                string line = rawLine.Trim();

                if (line.StartsWith("COPY ", StringComparison.OrdinalIgnoreCase) &&
                    line.IndexOf("FROM stdin", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    inCopy = true;
                    continue;
                }
                if (line == "\\.")
                {
                    inCopy = false;
                    continue;
                }

                if (inCopy)
                {
                    string[] parts = rawLine.Split('\t');
                    if (parts.Length < 10) continue;
                    var row = ParseParts(parts);
                    if (row != null) rows.Add(row);
                }
                else if (line.StartsWith("INSERT INTO fortune_bag VALUES", StringComparison.OrdinalIgnoreCase))
                {
                    var row = ParseInsertLine(line);
                    if (row != null) rows.Add(row);
                }
            }

            return rows;
        }

        BagRow ParseParts(string[] parts)
        {
            if (Schema == GameSchema.Unknown)
                Schema = DetectSchema(parts.Length);

            try
            {
                var row = new BagRow { Schema = Schema };
                row.Id       = int.Parse(parts[0].Trim());
                row.Sequence = int.Parse(parts[1].Trim());
                row.Set      = int.Parse(parts[2].Trim());
                row.ItemId   = int.Parse(parts[3].Trim());
                row.ItemNum  = int.Parse(parts[4].Trim());
                row.Probability = float.Parse(parts[5].Trim(), Inv);

                switch (Schema)
                {
                    case GameSchema.LaPlace:
                        row.AchProbability = float.Parse(parts[6].Trim(), Inv);
                        row.ItemCounter    = float.Parse(parts[7].Trim(), Inv);
                        row.Bulletin       = int.Parse(parts[8].Trim());
                        row.Note           = parts.Length > 9 ? parts[9].Trim() : "";
                        break;
                    case GameSchema.EdenEternal:
                        row.ItemCounter = float.Parse(parts[6].Trim(), Inv);
                        row.Bulletin    = int.Parse(parts[7].Trim());
                        row.Embedded    = int.Parse(parts[8].Trim());
                        row.Bind        = int.Parse(parts[9].Trim());
                        row.Note        = parts.Length > 10 ? parts[10].Trim() : "";
                        break;
                    case GameSchema.GrandFantasia:
                        row.Bulletin = int.Parse(parts[6].Trim());
                        row.White    = float.Parse(parts[7].Trim(), Inv);
                        row.Green    = float.Parse(parts[8].Trim(), Inv);
                        row.Blue     = float.Parse(parts[9].Trim(), Inv);
                        row.Yellow   = float.Parse(parts[10].Trim(), Inv);
                        row.Note     = parts.Length > 11 ? parts[11].Trim() : "";
                        break;
                }
                return row;
            }
            catch { return null; }
        }

        BagRow ParseInsertLine(string line)
        {
            var match = Regex.Match(line, @"VALUES\s*\((.+)\)\s*;?\s*$", RegexOptions.IgnoreCase);
            if (!match.Success) return null;

            string[] parts = SplitValues(match.Groups[1].Value);
            if (parts.Length < 10) return null;

            if (Schema == GameSchema.Unknown)
                Schema = DetectSchema(parts.Length);

            try
            {
                var row = new BagRow { Schema = Schema };
                row.Id       = int.Parse(parts[0].Trim());
                row.Sequence = int.Parse(parts[1].Trim());
                row.Set      = int.Parse(parts[2].Trim());
                row.ItemId   = int.Parse(parts[3].Trim());
                row.ItemNum  = int.Parse(parts[4].Trim());
                row.Probability = float.Parse(parts[5].Trim(), Inv);

                switch (Schema)
                {
                    case GameSchema.LaPlace:
                        row.AchProbability = float.Parse(parts[6].Trim(), Inv);
                        row.ItemCounter    = float.Parse(parts[7].Trim(), Inv);
                        row.Bulletin       = int.Parse(parts[8].Trim());
                        row.Note           = parts.Length > 9 ? StripQuotes(parts[9].Trim()) : "";
                        break;
                    case GameSchema.EdenEternal:
                        row.ItemCounter = float.Parse(parts[6].Trim(), Inv);
                        row.Bulletin    = int.Parse(parts[7].Trim());
                        row.Embedded    = int.Parse(parts[8].Trim());
                        row.Bind        = int.Parse(parts[9].Trim());
                        row.Note        = parts.Length > 10 ? StripQuotes(parts[10].Trim()) : "";
                        break;
                    case GameSchema.GrandFantasia:
                        row.Bulletin = int.Parse(parts[6].Trim());
                        row.White    = float.Parse(parts[7].Trim(), Inv);
                        row.Green    = float.Parse(parts[8].Trim(), Inv);
                        row.Blue     = float.Parse(parts[9].Trim(), Inv);
                        row.Yellow   = float.Parse(parts[10].Trim(), Inv);
                        row.Note     = parts.Length > 11 ? StripQuotes(parts[11].Trim()) : "";
                        break;
                }
                return row;
            }
            catch { return null; }
        }

        string[] SplitValues(string inner)
        {
            var result = new List<string>();
            bool inQuote = false;
            var current = new StringBuilder();
            for (int i = 0; i < inner.Length; i++)
            {
                char c = inner[i];
                if (c == '\'' && !inQuote) { inQuote = true; current.Append(c); }
                else if (c == '\'' && inQuote) { inQuote = false; current.Append(c); }
                else if (c == ',' && !inQuote) { result.Add(current.ToString()); current.Clear(); }
                else { current.Append(c); }
            }
            if (current.Length > 0) result.Add(current.ToString());
            return result.ToArray();
        }

        string StripQuotes(string s)
        {
            if (s.Length >= 2 && s[0] == '\'' && s[s.Length - 1] == '\'')
                return s.Substring(1, s.Length - 2);
            return s;
        }

        GameSchema DetectSchema(int colCount)
        {
            if (colCount == 10) return GameSchema.LaPlace;
            if (colCount == 11) return GameSchema.EdenEternal;
            if (colCount == 12) return GameSchema.GrandFantasia;
            return GameSchema.Unknown;
        }
    }
}
