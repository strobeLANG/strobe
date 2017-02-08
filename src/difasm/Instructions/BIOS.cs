using difasm.Structure;
namespace difasm.Instructions
{
    public static class BIOS
    {
        public static Instruction Write(string label, string var)
        {
            return new Instruction
            {
                inst = label + @":
mov eax, 0x4
mov ebx, 0x1
mov ecx, " + var + @"
mov edx, 0x1
int 0x80
ret"
            };
        }

        public static Instruction Exit(string label, string var)
        {
            return new Instruction
            {
                inst = label + @":
mov eax, 0x1
mov ebx, " + var + @" 
ret"
            };
        }
    }
}
