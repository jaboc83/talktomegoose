using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkToMeGoose
{
    class Configuration
    {
        public int IntervalInSec { get; set; } = 10;

        public bool RandomizeVoice { get; set; } = true;

        private int _volume = 100;
        public int Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                _volume = value > 100 ? 100 : value < 0 ? 0 : value;
            }
        }

        private int _speechRate = 0;
        public int SpeechRate {
            get
            {
                return _speechRate;
            }
            set
            {
                _speechRate = value > 10 ? 10 : value < -10 ? -10 : value;
            }
        }
    }
}
