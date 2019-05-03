using Terrasoft.Tools.SvnUI.Model.Enums;

namespace Terrasoft.Tools.SvnUI.Model.Log
{
    public class LogInfo
    {
        public LogLevel Level { get; set; }
        public string Message { get; set; }

        public override string ToString() {
            return $"{Level} {Message}";
        }
    }
}