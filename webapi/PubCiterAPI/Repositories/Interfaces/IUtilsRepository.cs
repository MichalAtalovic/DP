using PubCiterAPI.Model.Utils;

namespace PubCiterAPI.Repositories.Interfaces
{
    using PubCiterAPI.Model;

    /// <summary>
    /// ConversionRepository Interface
    /// </summary>
    public interface IUtilsRepository
    {
        /// <summary>
        /// Converts bibliographical record to other format
        /// </summary>
        /// <param name="data">String data to be converted</param>
        /// <param name="convertFrom">Format converting from</param>
        /// <param name="convertTo">Format converting to</param>
        /// <returns>Converted data as string</returns>
        public string Convert(string data, ConversionTypeEnum convertFrom, ConversionTypeEnum convertTo);

        /// <summary>
        /// Formats BibTex entry
        /// </summary>
        /// <param name="data">Data as string</param>
        /// <param name="inputType">Data type of input string</param>
        /// <param name="formatAs">Format as</param>
        /// <returns>Formatted bibliographical entry</returns>
        public string Format(string data, ConversionTypeEnum inputType, FormatEnum formatAs);
    }
}
