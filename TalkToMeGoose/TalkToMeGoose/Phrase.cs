using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkToMeGoose
{
    internal class Phrase
    {
        /// <summary>
        /// After how many minutes have elapsed should this message enter the rotation
        /// </summary>
        internal int ActivateTimeInMin { get; set; }

        /// <summary>
        /// After how many minutes have elapsed should this message leave the rotation
        /// </summary>
        internal int? InactivateTimeInMin { get; set; }

        /// <summary>
        /// The message to be presented
        /// </summary>
        internal string Message { get; set; }

        // abstract number to indicate how often a phrase should be presented. Is relative to other phrase weights.
        internal int Weight { get; set; }

        public override string ToString()
        {
            return $"{this.Message} Active from {ActivateTimeInMin} min " + (InactivateTimeInMin != null ?
                $"to {InactivateTimeInMin} min. ({Weight} weight)" : $"to end of game. ({Weight} weight)");
        }
    }
}
