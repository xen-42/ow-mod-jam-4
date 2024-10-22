using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeRoomJam4
{
    public class SignalController : Puzzle
    {
        public static SignalController instance;

        private bool[] correctSignals = new bool[4];

        private void Awake()
        {
            instance = this;
        }

        public void ChangeCorrectFlag(int id, bool correct)
        {
            correctSignals[id] = correct;
            CheckIfSolved();
        }

        public override bool IsSolved()
        {
            foreach (bool isCorrect in correctSignals)
            {
                EscapeRoomJam4.WriteDebug(isCorrect.ToString());
                if (!isCorrect) return false;
            }
            return true;
        }
    }
}
