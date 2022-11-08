using PeanutButter.EasyArgs.Attributes;

namespace watch
{
    public class Options
    {
        [ShortName('n')]
        [Default(2)]
        [Description("seconds to wait between updates")]
        public decimal Interval { get; set; }

        [ShortName('t')]
        [Description("turn off header")]
        public bool NoTitle { get; set; }

        [ShortName('e')]
        [LongName("--errexit")]
        public bool ExitOnError { get; set; }

        [ShortName('g')]
        [LongName("chgexit")]
        public bool ExitOnChange { get; set; }

        [ShortName('b')]
        [LongName("beep")]
        [Description("beep if command has a non-zero exit")]
        public bool BeepOnError { get; set; }
    }
}