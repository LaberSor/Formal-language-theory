using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stateless;

namespace Text_Processor
{
    enum Triggers
    {
        H, F, T, P, S, ProtocolSeparator, 
        W, Dot, Symbol, 
        R, U, C, O, M,
        Slash,
        EndInput
    }

    enum States
    {
        q1, q2, q3, q4, q5, q6, q7, q8, q9, q10,
        q11, q12, q13, q14, q15, q16,
        q17, q18, q19, q20, 
        q21, q22,
        q23
    }

    class StateMachine
    {
        private StateMachine<States, Triggers> sm;
        
        string str;
        public string states { set; get; } = "q1->";

        public StateMachine()
        {
            sm = new StateMachine<States, Triggers>(States.q1);
            sm.Configure(States.q1).Permit(Triggers.H, States.q2)
                                   .Permit(Triggers.F, States.q7)
                                   .Permit(Triggers.Symbol, States.q15)
                                   .OnActivate(() => states += "q1->");
            sm.Configure(States.q2).Permit(Triggers.T, States.q3)
                                   .Permit(Triggers.Symbol, States.q15)
                                   .Permit(Triggers.Dot, States.q15)
                                   .OnEntry(() => states += "q2->");
            sm.Configure(States.q3).Permit(Triggers.T, States.q4)
                                   .Permit(Triggers.Symbol, States.q15)
                                   .Permit(Triggers.Dot, States.q15)
                                   .OnEntry(() => states += "q3->");
            sm.Configure(States.q4).Permit(Triggers.P, States.q5)
                                   .Permit(Triggers.Symbol, States.q15)
                                   .Permit(Triggers.Dot, States.q15)
                                   .OnEntry(() => states += "q4->");
            sm.Configure(States.q5).Permit(Triggers.ProtocolSeparator, States.q6)
                                   .Permit(Triggers.S, States.q10)
                                   .Permit(Triggers.Symbol, States.q15)
                                   .Permit(Triggers.Dot, States.q15)
                                   .OnEntry(() => states += "q5->");
            sm.Configure(States.q6).Permit(Triggers.W, States.q11)
                                   .Permit(Triggers.Symbol, States.q15)
                                   .OnEntry(() => states += "q6->");
            sm.Configure(States.q7).Permit(Triggers.T, States.q8)
                                   .Permit(Triggers.Symbol, States.q15)
                                   .Permit(Triggers.Dot, States.q15)
                                   .OnEntry(() => states += "q7->");
            sm.Configure(States.q8).Permit(Triggers.P, States.q9)
                                   .Permit(Triggers.Symbol, States.q15)
                                   .Permit(Triggers.Dot, States.q15)
                                   .OnEntry(() => states += "q8->");
            sm.Configure(States.q9).Permit(Triggers.ProtocolSeparator, States.q6)
                                   .Permit(Triggers.Symbol, States.q15)
                                   .Permit(Triggers.Dot, States.q15)
                                   .OnEntry(() => states += "q9->");
            sm.Configure(States.q10).Permit(Triggers.ProtocolSeparator, States.q6)
                                    .Permit(Triggers.Symbol, States.q15)
                                    .Permit(Triggers.Dot, States.q15)
                                    .OnEntry(() => states += "q10->");
            sm.Configure(States.q11).Permit(Triggers.W, States.q12)
                                    .Permit(Triggers.Symbol, States.q15)
                                    .Permit(Triggers.Dot, States.q15)
                                    .OnEntry(() => states += "q11->");
            sm.Configure(States.q12).Permit(Triggers.W, States.q13)
                                    .Permit(Triggers.Symbol, States.q15)
                                    .Permit(Triggers.Dot, States.q15)
                                    .OnEntry(() => states += "q12->");
            sm.Configure(States.q13).Permit(Triggers.Dot, States.q14)
                                    .Permit(Triggers.Symbol, States.q15)
                                    .OnEntry(() => states += "q13->");
            sm.Configure(States.q14).Permit(Triggers.Symbol, States.q15)
                                    .OnEntry(() => states += "q14->");
            sm.Configure(States.q15).Permit(Triggers.Dot, States.q16)
                                    .PermitReentry(Triggers.Symbol)
                                    .OnEntry(() => states += "q15->");
            sm.Configure(States.q16).Permit(Triggers.R, States.q17)
                                    .Permit(Triggers.C, States.q19)
                                    .Permit(Triggers.Symbol, States.q15)
                                    .OnEntry(() => states += "q16->");
            sm.Configure(States.q17).Permit(Triggers.U, States.q18)
                                    .OnEntry(() => states += "q17->");
            sm.Configure(States.q18).Permit(Triggers.Slash, States.q21)
                                    .OnEntry(() => states += "q18->")
                                    .Permit(Triggers.EndInput, States.q23);
            sm.Configure(States.q19).Permit(Triggers.O, States.q20)
                                    .OnEntry(() => states += "q19->");
            sm.Configure(States.q20).Permit(Triggers.M, States.q18)
                                    .OnEntry(() => states += "q20->");
            sm.Configure(States.q21).Permit(Triggers.Symbol, States.q22)
                                    .OnEntry(() => states += "q21->")
                                    .Permit(Triggers.EndInput, States.q23);
            sm.Configure(States.q22).Permit(Triggers.Symbol, States.q23)
                                    .OnEntry(() => states += "q22->")
                                    .Permit(Triggers.EndInput, States.q23);
            sm.Configure(States.q23).OnEntry(() => states += "q23");
        }

        public void H()
        { if (sm.CanFire(Triggers.H)) sm.Fire(Triggers.H); }
        public void F()
        { if (sm.CanFire(Triggers.F)) sm.Fire(Triggers.F); }
        public void T()
        { if (sm.CanFire(Triggers.T)) sm.Fire(Triggers.T); }
        public void P()
        { if (sm.CanFire(Triggers.P)) sm.Fire(Triggers.P); }
        public void S()
        { if (sm.CanFire(Triggers.S)) sm.Fire(Triggers.S); }
        public void ProtocolSeparator()
        { if (sm.CanFire(Triggers.ProtocolSeparator)) sm.Fire(Triggers.ProtocolSeparator); }
        public void W()
        { if (sm.CanFire(Triggers.W)) sm.Fire(Triggers.W); }
        public void Dot()
        { if (sm.CanFire(Triggers.Dot)) sm.Fire(Triggers.Dot); }
        public void Symbol()
        { if (sm.CanFire(Triggers.Symbol)) sm.Fire(Triggers.Symbol); }
        public void R()
        { if (sm.CanFire(Triggers.R)) sm.Fire(Triggers.R); }
        public void U()
        { if (sm.CanFire(Triggers.U)) sm.Fire(Triggers.U); }
        public void C()
        { if (sm.CanFire(Triggers.C)) sm.Fire(Triggers.C); }
        public void O()
        { if (sm.CanFire(Triggers.O)) sm.Fire(Triggers.O); }
        public void M()
        { if (sm.CanFire(Triggers.M)) sm.Fire(Triggers.M); }
        public void Slash()
        { if (sm.CanFire(Triggers.Slash)) sm.Fire(Triggers.Slash); }
        public void EndInput()
        { if (sm.CanFire(Triggers.EndInput)) sm.Fire(Triggers.EndInput); }

        public States getState()
        {
            var getState = sm.State;
            return getState;
        }
        public string getStr() => str;
        public bool SymbolChecking(char symbol)
        {
            //int kode = (int)symbol;
            str += symbol;
            bool SpecSymbolCheck = false;
            if (symbol == '\n' || symbol == '\r')
            {
                str = "";
                SpecSymbolCheck = true;
            }
            switch (symbol)
            {
                case 'h': { H(); SpecSymbolCheck = true; break; }
                case 't': { T(); SpecSymbolCheck = true; break; }
                case 'p': { P(); SpecSymbolCheck = true; break; }
                case 'f': { F(); SpecSymbolCheck = true; break; }
                case 's': { S(); SpecSymbolCheck = true; break; }
                case ':': { ProtocolSeparator(); SpecSymbolCheck = true; break; }
                case 'w': { W(); SpecSymbolCheck = true; break; }
                case '.': { Dot(); SpecSymbolCheck = true; break; }
                case '/': { Slash(); SpecSymbolCheck = true; break; }
                case 'r': { R(); SpecSymbolCheck = true; break; }
                case 'u': { U(); SpecSymbolCheck = true; break; }
                case 'c': { C(); SpecSymbolCheck = true; break; }
                case 'o': { O(); SpecSymbolCheck = true; break; }
                case 'm': { M(); SpecSymbolCheck = true; break; }
                case '#': { EndInput(); SpecSymbolCheck = true; break; }
            }
            if (((symbol >= 'a' && symbol <= 'z') ||
                (symbol >= '0' && symbol <= '9') ||
                symbol == '_' || symbol == '-' || symbol == '.') && SpecSymbolCheck == false)
                Symbol();
            else if (SpecSymbolCheck == false)
            {
                str = "";
                return false;
            }
            //if (symbol == ':')
                //str += "//";
            return true;
        }
    }
}
