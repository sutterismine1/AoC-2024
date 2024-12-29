using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

class IntCode
{
    private long registerA;
    private long registerB;
    private long registerC;
    private int[] instructions;
    private int instructionLength;
    private int PC = 0;
    private List<int> outputStream;
    static long b = 549755813888;  //start for 64k+49
    static long c = 274877906944;  //start for 128k+22
    static long d = 137438953472;  //start for 256k+7
    static long e = 68719476736;   //start vfor 512k + 196
    static long f = 34359738368;   //start for 1024k + 261
    public IntCode(StreamReader r, long a)
    {
        registerA = a;
        r.ReadLine();
        registerB = int.Parse(Regex.Split(r.ReadLine(), ": ")[1]);
        registerC = int.Parse(Regex.Split(r.ReadLine(), ": ")[1]);
        r.ReadLine();
        string rawInstructions = Regex.Split(r.ReadLine(), ": ")[1];
        string[] rawInstructionsArr = rawInstructions.Split(',');
        instructions = rawInstructionsArr.Select(x => int.Parse(x)).ToArray();
        instructionLength = instructions.Length;
        outputStream = new List<int>();
    }
    public long nextValidNum() {
        long minNum = new List<long> { b * 64 + 49, c * 128 + 22, d * 256 + 7, e * 512 + 196, f * 1024 + 261 }.Min();
        Console.WriteLine(minNum);
        if (minNum % 64 == 49)
            b += 1;
        else if(minNum % 128 == 22)
            c += 1;
        else if(minNum % 256 == 7)
            d += 1;
        else if (minNum % 512 == 196)
            e += 1;
        else if(minNum % 1024 == 261)
            f += 1;
        return minNum;
    }
    private long combo(long cOperand)
    {
        switch (cOperand)
        {
            case 0: return 0;
            case 1: return 1;
            case 2: return 2;
            case 3: return 3;
            case 4: return registerA;
            case 5: return registerB;
            case 6: return registerC;
            default: throw new ArgumentException("Cannot have a combo operator of 7 or greater.");
        }
    }
    private void adv(long cOperand)  //division of A / 2^COperand -> Result stored in A
    {
        long dividend = registerA;
        long divisor = combo(cOperand);
        long result = (long)Math.Truncate((double)dividend / Math.Pow(2, divisor));
        registerA = result;
    }
    private void bxl(long lOperand)  //Register B XOR LOperand -> Result stored in B
    {
        long regb = registerB;
        long result = regb ^ lOperand;
        registerB = result;
    }
    private void bst(long cOperand)  //COperand Mod 8 (last 3 bits) -> Result stored in B
    {
        long operand = combo(cOperand);
        long result = operand % 8;
        registerB = result;
    }
    private void jnz(int lOperand)  //Jumps to PC of literal operand if A is not zero
    {
        if (registerA != 0)
        {
            PC = lOperand;
        }
        else
        {
            PC = PC + 2;
        }
    }
    private void bxc()  //B XOR C -> Result stored in B (operand not used)
    {
        long result = registerB ^ registerC;
        registerB = result;
    }
    private int output(long cOperand) //Outputs the COperand Mod 8 -> returns result
    {
        long operand = combo(cOperand);
        int result = (int)(operand % 8);
        return result;
    }
    private void bdv(long cOperand)  //division of A / 2^COperand -> Result stored in B
    {
        long dividend = registerA;
        long divisor = combo(cOperand);
        long result = (long)Math.Truncate((double)dividend / Math.Pow(2, divisor));
        registerB = result;
    }
    private void cdv(long cOperand)  //division of A / 2^COperand -> Result stored in C
    {
        long dividend = registerA;
        long divisor = combo(cOperand);
        long result = (long)Math.Truncate((double)dividend / Math.Pow(2, divisor));
        registerC = result;
    }
    public bool halted()
    {
        if (PC >= instructionLength)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void process()
    {
        int opcode = instructions[PC];
        int operand = instructions[PC + 1];
        switch (opcode)
        {
            case 0:
                adv(operand); PC += 2; break;
            case 1:
                bxl(operand); PC += 2; break;
            case 2:
                bst(operand); PC += 2; break;
            case 3:
                jnz(operand); break;    //do not increase PC for jump instructions, the jump operation handles this
            case 4:
                bxc(); PC += 2; break;           //operand not used
            case 5:
                outputStream.Add(output(operand)); PC += 2; break;
            case 6:
                bdv(operand); PC += 2; break;
            case 7:
                cdv(operand); PC += 2; break;
        }
        //Console.WriteLine("PC: " + PC);
        //Console.WriteLine("A: " + registerA);
        //Console.WriteLine("B: " + registerB);
        //Console.WriteLine("C: " + registerC);
    }
    public int[] printOutput()
    {
        return outputStream.ToArray();
    }
    static void Main(string[] args)
    {
        string cwd = "../../../../";
        long a = 190384609460224;
        while (a < (long)Math.Pow(8,16))
        { 
            StreamReader reader = new StreamReader(cwd + "input.txt");
            IntCode program = new IntCode(reader, a);
            while (!program.halted())
            {
                program.process();
            }
            int[] outp = program.printOutput();
            if (Enumerable.SequenceEqual(outp, program.instructions)) { break; }
            if(outp.Length > 15 && outp[15] == 0) { 
                if (outp[14] == 3) {                     
                    if (outp[13] == 5)
                    {
                        if (outp[12] == 5)
                        {
                            if (outp[11] == 7)
                            {
                                if (outp[10] == 1)
                                {
                                    if (outp[9] == 3)
                                    {
                                        if (outp[8] == 0)
                                        {
                                            if (outp[7] == 3)
                                            {
                                                if (outp[6] == 4)
                                                {
                                                    if (outp[5] == 5)
                                                    {
                                                        if (outp[4] == 7)
                                                        {
                                                            if (outp[3] == 2)
                                                            {
                                                                if (outp[2] == 1)
                                                                {
                                                                    if (outp[1] == 4)
                                                                    {
                                                                        if (outp[0] == 2)
                                                                        {
                                                                            
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }                         
                            }
                        }
                    }
                }
            }
            a += (long)Math.Pow(8, 0);

            reader.Close();
        }
        Console.WriteLine(a);
    }
}