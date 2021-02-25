using System;
using System.Web;
using PubCiterAPI.Model.Utils;

namespace PubCiterAPI.Repositories
{
    using Model;
    using Interfaces;

    public class UtilsRepository : IUtilsRepository
    {
        /// <summary>
        /// Converts bibliographical record to other format
        /// </summary>
        /// <param name="data">String data to be converted</param>
        /// <param name="convertFrom">Format converting from</param>
        /// <param name="convertTo">Format converting to</param>
        /// <returns>Converted data as string</returns>
        public string Convert(string data, ConversionTypeEnum convertFrom, ConversionTypeEnum convertTo)
        {
            switch (convertFrom)
            {
                case ConversionTypeEnum.Bib:
                    switch (convertTo)
                    {
                        case ConversionTypeEnum.Ris:
                            return this.Bib2Ris(data);
                        default:
                            return this.PybTexConvert(data, convertFrom, convertTo);
                    }
                case ConversionTypeEnum.Ris:
                    return convertTo == ConversionTypeEnum.Bib  ? this.Ris2Bib(data) : this.Convert(this.Ris2Bib(data), ConversionTypeEnum.Bib, convertTo);
                default:
                    switch (convertTo)
                    {
                        case ConversionTypeEnum.Ris:
                            return this.Bib2Ris(this.Convert(data, convertFrom, ConversionTypeEnum.Bib));
                        default:
                            return this.PybTexConvert(data, convertFrom, convertTo);
                    }
            }
        }

        /// <summary>
        /// Formats bibliographical entry
        /// </summary>
        /// <param name="data">Data as string</param>
        /// <param name="inputType">Data type of input</param>
        /// <param name="formatAs">Format as</param>
        /// <returns>Formatted bibliographical entry</returns>
        public string Format(string data, ConversionTypeEnum inputType, FormatEnum formatAs)
        {
            return inputType == ConversionTypeEnum.Bib ? PybTexFormat(data, formatAs) : PybTexFormat(this.Convert(data, inputType, ConversionTypeEnum.Bib), formatAs);
        }

        /// <summary>
        /// Converts between types
        /// </summary>
        /// <param name="data">Data as string</param>
        /// <param name="convertFrom">Convert from type</param>
        /// <param name="convertTo">Convert to type</param>
        /// <returns>Converted bibliographical entry as string</returns>
        private string PybTexConvert(string data, ConversionTypeEnum convertFrom, ConversionTypeEnum convertTo)
        {
            data = data.TrimStart('\"').TrimEnd('\"').Replace("\\r\\n", Environment.NewLine);

            if (data.Contains("\\\""))
            {
                data = data.Replace("\\\"", "\"");
            }

            var process = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo()
                {
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                    FileName = @"cmd.exe",
                    Arguments = $"/C python {AppSettings.PybTexConvertScriptPath} --input-format \"{convertFrom.ToString().ToLower()}\" --output-format \"{convertTo.ToString().ToLower()}\" --data \"{data.TrimStart('\"').TrimEnd('\"').Replace("\"", "\\\"").Replace(Environment.NewLine, string.Empty)}\"",
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                }
            };

            process.Start();
            var dump = process.StandardOutput.ReadToEnd();
            var e = process.StandardError.ReadToEnd();
            process.WaitForExit();

            return e == string.Empty ? dump : e;
        }

        /// <summary>
        /// Formats bibliographical entry to specified format
        /// </summary>
        /// <param name="data">Data as string</param>
        /// <param name="formatAs">Format as</param>
        /// <returns></returns>
        private string PybTexFormat(string data, FormatEnum formatAs)
        {
            data = data.TrimStart('\"').TrimEnd('\"').Replace("\\r\\n", Environment.NewLine);

            if (data.Contains("\\\""))
            {
                data = data.Replace("\\\"", "\"");
            }

            var process = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo()
                {
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                    FileName = @"cmd.exe",
                    Arguments = $"/C python {AppSettings.PybTexFormatScriptPath} --input-format bib --output-format \"{formatAs.ToString().ToLower()}\" --data \"{data.Replace("\"", "\\\"").Replace(Environment.NewLine, string.Empty)}\"",
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                }
            };

            process.Start();
            var dump = process.StandardOutput.ReadToEnd();
            var e = process.StandardError.ReadToEnd();
            process.WaitForExit();

            return e == string.Empty ? dump : e;
        }

        /// <summary>
        /// Converts RIS record to BIB record
        /// </summary>
        /// <param name="data">Data as RIS string</param>
        /// <returns></returns>
        private string Ris2Bib(string data)
        {
            data = data.TrimStart('\"').TrimEnd('\"').Replace("\\r\\n", Environment.NewLine);

            if (data.Contains("\\\""))
            {
                data = data.Replace("\\\"", "\"");
            }

            var process = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo()
                {
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                    FileName = @"cmd.exe",
                    Arguments = $"/C python {AppSettings.Ris2BibScriptPath} --data \"{data.Replace("\"", "\\\"").Replace(Environment.NewLine, @"|")}\"",
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                }
            };

            process.Start();
            var dump = process.StandardOutput.ReadToEnd();
            var e = process.StandardError.ReadToEnd();
            process.WaitForExit();

            return e == string.Empty ? dump : e;
        }

        /// <summary>
        /// Converts Bib record to RIS record
        /// </summary>
        /// <param name="data">Data as BIB string</param>
        /// <returns>RIS string</returns>
        private string Bib2Ris(string data)
        {
            data = data.TrimStart('\"').TrimEnd('\"').Replace("\\r\\n", Environment.NewLine);

            if (data.Contains("\\\""))
            {
                data = data.Replace("\\\"", "\"");
            }

            var ris = string.Empty;

            // add virtual separator to distinguish records in multiple-record input + replace comma separator with semicolon to distinguish entries
            data = data.Replace(@"@", @"|@").TrimStart('|')
                .Replace("," + Environment.NewLine, @";" + Environment.NewLine);

            // iterate over records
            foreach (var bib in data.Split('|'))
            {
                // resolve bibliographical type
                if (bib.ToLower().Contains("@article"))
                {
                    ris += (@"TY  - JOUR" + Environment.NewLine);
                }
                else if (bib.ToLower().Contains("@book"))
                {
                    ris += (@"TY  - BOOK" + Environment.NewLine);
                }

                // rid of curly braces and split entries
                var splitBib = bib.Split('{')[1].Replace(@"}", string.Empty).Replace(Environment.NewLine, string.Empty).Split(';');

                for (var i = 1; i < splitBib.Length; i++)
                {
                    if (string.IsNullOrEmpty(splitBib[i]))
                    {
                        continue;
                    }

                    var entry = splitBib[i].Split(@"=");
                    ris += this.ResolveBibEntry(entry[0].Replace(@" ", string.Empty).Replace("\t", string.Empty), entry[1].TrimStart(' ').Trim('"'));
                }

                ris += @"ER  -" + Environment.NewLine + Environment.NewLine;
            }

            return ris;
        }

        /// <summary>
        /// Resolves bib entry and converts it to RIS format entry
        /// </summary>
        /// <param name="tag">BIB tag</param>
        /// <param name="value">Tag's value</param>
        /// <returns>RIS formatted entry</returns>
        private string ResolveBibEntry(string tag, string value)
        {
            switch (tag)
            {
                case @"title":
                    return @"T1  - " + value + Environment.NewLine;
                case @"author":
                    return @"AU  - " + value + Environment.NewLine;
                case @"publisher":
                    return @"PB  - " + value + Environment.NewLine;
                case @"doi":
                    return @"ID  - " + value + Environment.NewLine;
                case @"year":
                    return @"PY  - " + value + Environment.NewLine;
                case @"url":
                    return @"UR  - " + value + Environment.NewLine;
                case @"journal":
                    return @"JO  - " + value + Environment.NewLine;
                case @"volume":
                    return @"VL  - " + value + Environment.NewLine;
                case @"issue":
                    return @"IS  - " + value + Environment.NewLine;
                case @"startpage":
                    return @"SP  - " + value + Environment.NewLine;
                case @"endpage":
                    return @"EP  - " + value + Environment.NewLine;
                case @"abstract":
                    return @"AB  - " + value + Environment.NewLine;
                default:
                    return string.Empty;
            }
        }
    }
}
