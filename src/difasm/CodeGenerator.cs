using difasm.Instructions;
using difasm.Structure;
using System.Collections.Generic;
using StrobeVM.DIF;
using System;

namespace difasm
{
    public class CodeGenerator
    {
        StrobeVM.Instruction[] instructions;
        ASM asm = new ASM();
        int freename = 0;

        public CodeGenerator(DIFExecuteable exe)
        {
            instructions = exe.CPU();
            Generate();
        }

        void Generate()
        {
            foreach(var x in instructions)
            {
                switch(x.Op)
                {
                    case StrobeVM.Instruction.OpType.Allocate:
                        Allocate(x.Param);
                        break;
                    default:
                        throw new Exception("Not implemented OpType: " + x.Op);
                }
            }
        }

        private void Allocate(byte[] param)
        {
            int[] i = TwoArgs(param);
            // TODO: Continue from here.
        }

        public int[] TwoArgs(byte[] ar)
        {
            bool isTwo = false;
            List<byte> one = new List<byte>();
            List<byte> two = new List<byte>();
            foreach (byte b in ar)
            {
                if (b == 254)
                {
                    isTwo = true;
                    continue;
                }
                if (isTwo == false)
                {
                    one.Add(b);
                }
                else
                {
                    two.Add(b);
                }
            }
            while (one.Count < 4)
            {
                one.Add(0);
            }
            while (two.Count < 4)
            {
                two.Add(0);
            }
            while (one.Count > 4)
            {
                one.RemoveAt(one.Count - 1);
            }
            while (two.Count > 4)
            {
                two.RemoveAt(two.Count - 1);
            }
            int x = BitConverter.ToInt32(one.ToArray(), 0);
            int y = BitConverter.ToInt32(two.ToArray(), 0);
            return new int[] {
                x,y
            };
        }

    }
}

/*

    Use this for inspiration

section .bss
    Char: resd 1

section .text

    global _start
    _start:
        call truth
        mov eax,1
        int 0x80
    ret

    write:
        mov eax, 0x4
        mov ebx, 0x1
        mov ecx, Char
        mov edx, 0x1
        int 0x80
    ret
    
    truth:
        mov word [Char],0x54
        call write
        
        mov word [Char],0x52
        call write
        
        mov word [Char],0x55
        call write
        
        mov word [Char],0x4D
        call write
        
        mov word [Char],0x50
        call write
        
        mov word [Char],0x0A
        call write
    ret
*/