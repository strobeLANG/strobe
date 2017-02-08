using System.Collections.Generic;

namespace difasm.Structure
{
    public class ASM
    {
        string bss,data,text;
        public List<Variable> Variables = new List<Variable>();
        public List<Instruction> Functions = new List<Instruction>();
        public List<Instruction> Code = new List<Instruction>();

        public override string ToString()
        {
            bss = ""; data = ""; text = "";
            foreach(Variable v in Variables)
                if (v.init)
                    data += v.name + ": d" + GetType(v.value) + " " + GetTypeValue(v.value) + "\n";
                else
                    bss += v.name + ": res" + GetType(v.value) + " " + v.size + "\n";
            foreach (Instruction i in Functions)
                text += i.inst + "\n";
            text += "global _start\n_start:\n";
            foreach(Instruction i in Code)
            {
                text += i.inst + "\n";
            }
            text += "ret\n";

            return "section .bss\n" + bss + "section .data\n" + data + "section .text\n" + text;

        }

        string GetTypeValue(string value)
        {
            if (GetType(value) == "w")
            {
                return value;
            } else
            {
                return "'" + value + "'";
            }
        }

        string GetType(string value)
        {
            int x;
            if (int.TryParse(value,out x))
            {
                return "w";
            } else
            {
                return "b";
            }
        }
    }
}
