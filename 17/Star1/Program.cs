using System.Text.RegularExpressions;

class IntCode
{
    private int registerA;
    private int registerB;
    private int registerC;
    private int[] instructions;
    private int instructionLength;
    private int PC = 0;
    private List<int> outputStream;
    public IntCode(StreamReader r)
    {
        registerA = int.Parse(Regex.Split(r.ReadLine(), ": ")[1]);
        registerB = int.Parse(Regex.Split(r.ReadLine(), ": ")[1]);
        registerC = int.Parse(Regex.Split(r.ReadLine(), ": ")[1]);
        r.ReadLine();
        string rawInstructions = Regex.Split(r.ReadLine(), ": ")[1];
        string[] rawInstructionsArr = rawInstructions.Split(',');
        instructions = rawInstructionsArr.Select(x=>int.Parse(x)).ToArray();   
        instructionLength = instructions.Length;
        outputStream = new List<int>();
    }
    private int combo(int cOperand) {
        switch(cOperand)
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
    private void adv(int cOperand)  //division of A / 2^COperand -> Result stored in A
    {
        int dividend = registerA;
        int divisor = combo(cOperand);
        int result = (int)Math.Truncate((double)dividend/Math.Pow(2,divisor));
        registerA = result;
    }
    private void bxl(int lOperand)  //Register B XOR LOperand -> Result stored in B
    {
        int b = registerB;
        int result = b ^ lOperand;
        registerB = result;
    }
    private void bst(int cOperand)  //COperand Mod 8 (last 3 bits) -> Result stored in B
    {
        int operand = combo(cOperand);
        int result = operand % 8;
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
        int result = registerB ^ registerC;
        registerB = result;
    }
    private int output(int cOperand) //Outputs the COperand Mod 8 -> returns result
    {
        int operand = combo(cOperand);
        int result = operand % 8;
        return result;
    }
    private void bdv(int cOperand)  //division of A / 2^COperand -> Result stored in B
    {
        int dividend = registerA;
        int divisor = combo(cOperand);
        int result = (int)Math.Truncate((double)dividend / Math.Pow(2, divisor));
        registerB = result;
    }
    private void cdv(int cOperand)  //division of A / 2^COperand -> Result stored in C
    {
        int dividend = registerA;
        int divisor = combo(cOperand);
        int result = (int)Math.Truncate((double)dividend / Math.Pow(2, divisor));
        registerC = result;
    }
    public bool halted()
    {
        if(PC >= instructionLength)
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
        Console.WriteLine("PC: " + PC);
        Console.WriteLine("A: " + registerA);
        Console.WriteLine("B: " + registerB);
        Console.WriteLine("C: " + registerC);
    }
    public void printOutput()
    {
        Console.WriteLine("\n--Output--");
        Console.WriteLine(string.Join(',', outputStream));
    }
    static void Main(string[] args)
    {
        string cwd = "../../../../";
        StreamReader reader = new StreamReader(cwd + "sample.txt");
        IntCode program = new IntCode(reader);
        while (!program.halted())
        {
            program.process();
        }
        program.printOutput();
    }
}