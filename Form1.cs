using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSASemesterProject
{
    
    public partial class Form1 : Form
    {


        #region Variables

        #region Form Style Variables
        
        int lines = 0;
        List<String> numbers = new List<String>();
        int view = 1;
        string filename = "";
        #endregion

        #region Mips Related Vars
        public static string[] AllCommands = { "nop", "add", "sub", "and", "or", "xor", "slt", "sll", "srl", "jr", "syscall", "addi", "andi", "ori", "slti", "lw", "sw", "j", "jal", "beq", "bgez", "bgtz", "bltz", "blez", "bne", "li", "la" };
        public enum MipsDataType
        {
            asciiz,
            ascii,
            mips_byte,
            half,
            word
        }
        public struct MipsData
        {
            public string name { get; set; }
            public string value { get; set; }
            public MipsDataType DataType { get; set; }
            public MipsData(string name, string value, int DataType)
            {
                this.name = name;
                this.value = value;
                this.DataType = (MipsDataType)DataType;
            }
        }
        //public static List<MipsData> Data = new List<MipsData>(1073741823);
        public static List<MipsData> Data = new List<MipsData>();
        List<String> code = new List<String>();
        List<String> BinaryCode = new List<String>();
        List<InstructionType> CodeDataType = new List<InstructionType>();
        Dictionary<string, int> Labels = new Dictionary<string, int>();
        int[] registers = new int[32];
        int[] registers_prev = new int[32];
        int ins_counter = 0;
        #endregion

        #region Mips Format Vars
        private int op = 0, rs = 0, rt = 0, rd = 0, shamt = 0, function = 0, offset = 0, immediate = 0, address = 0;
        public enum InstructionType
        {
            RType,
            IType,
            JType,
            Branch,
            None
        }
        enum Registers
        {
            zero,at,v0,v1,a0,a1,a2,a3,t0,t1,t2,t3,t4,t5,t6,t7, s0, s1, s2, s3, s4, s5, s6, s7, t8, t9, k0, k1, gp, sp, fp, ra
        }
        private InstructionType InstructionTypeVar = InstructionType.None;

        #endregion

        #endregion
        public Form1()
        {
            InitializeComponent();
            toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            toolStrip1.Renderer = new MySR();
            SetAllToNull();
            autocompleteMenu1.Items = AllCommands;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            int caret_position = CodeBox.SelectionStart;
            int lines = CodeBox.Lines.Length;
            if(lines!=this.lines)
            {
                if (this.lines > lines)
                    numbers.RemoveRange(lines, this.lines - lines);
                else if(this.lines<lines)
                    numbers.AddRange(Enumerable.Range(this.lines+1, lines-this.lines).Select(delegate(int i) { return i.ToString(); }));
            }
            this.lines = lines;
            LNoBox.Lines = numbers.ToArray();
            CodeBox.SelectAll();
            CodeBox.SelectionColor = Color.White;
            CodeBox.DeselectAll();
            foreach (String command in AllCommands)
            {
                int startindex = 0;
                while (startindex < CodeBox.TextLength)
                {
                    int wordstartIndex = CodeBox.Find(command, startindex, RichTextBoxFinds.WholeWord);
                    if (wordstartIndex != -1)
                    {
                        CodeBox.SelectionStart = wordstartIndex;
                        CodeBox.SelectionLength = command.Length;
                        CodeBox.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(204)))), ((int)(((byte)(126)))));
                    }
                    else
                        break;
                    startindex = wordstartIndex + command.Length;
                }
            }
            string regname;
            foreach (String reg_name in Enum.GetNames(typeof(Registers)))
            {
                regname = "$" + reg_name;
                int startindex = 0;
                while (startindex < CodeBox.TextLength)
                {
                    int wordstartIndex = CodeBox.Find(regname, startindex, RichTextBoxFinds.WholeWord);
                    if (wordstartIndex != -1)
                    {
                        CodeBox.SelectionStart = wordstartIndex;
                        CodeBox.SelectionLength = regname.Length;
                        //rgb(166, 123, 172)
                        CodeBox.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(123)))), ((int)(((byte)(172)))));
                    }
                    else
                        break;
                    startindex = wordstartIndex + regname.Length;
                }
            }
            foreach (var vars in Data)
            {
                int startindex = 0;
                while (startindex < CodeBox.TextLength)
                {
                    int wordstartIndex = CodeBox.Find(vars.name, startindex, RichTextBoxFinds.WholeWord);
                    if (wordstartIndex != -1)
                    {
                        CodeBox.SelectionStart = wordstartIndex;
                        CodeBox.SelectionLength = vars.name.Length;
                        CodeBox.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(164)))), ((int)(((byte)(238)))));
                    }
                    else
                        break;
                    startindex = wordstartIndex + vars.name.Length;
                }
            }
            if(CodeBox.Text.Length!=0)
                CodeBox.Select(caret_position, 0);
            int linenumber = CodeBox.GetLineFromCharIndex(caret_position);
            if(LNoBox.Lines.Length != 0)
            {

                LNoBox.Select(LNoBox.Text.IndexOf((linenumber + 1).ToString()), 0);
                LNoBox.ScrollToCaret();
            }
        }

        private void LNoBox_TextChanged(object sender, EventArgs e)
        {

        }




        private void SetRegistersToNull()
        {
            for(int l = 0; l<32;l++)
            {
                registers[l] = 0;
            }
            UpdateRegisters();
        }

        private void UpdateRegisters()
        {
            for(int i = 0; i<32;i++)
            {
                Controls.Find("reg" + i.ToString(), true)[0].Text = "0x" + Convert.ToString(registers[i], 16).PadLeft(8,'0');
                if(registers[i]!=registers_prev[i])
                {
                    Controls.Find("reg" + i.ToString(), true)[0].BackColor = Color.Green;
                }
            }
        }
        
        private void SetAllToNull()
        {
            BinaryCode.Clear();
            CodeDataType.Clear();
            Labels.Clear();
            code.Clear();
            OutputBox.Clear();
            SetRegistersToNull();
            ins_counter = 0;
        }

        private void ViewBinaryButton_Click(object sender, EventArgs e)
        {
            if(view == 1)
            {
                if(BinaryCode.Count() == 0)
                {
                    MessageBox.Show("Compile Code At Least Once to Switch Views!");
                    return;
                }
                CodeBox.Lines = BinaryCode.ToArray();
                CodeBox.ReadOnly = true;
                view = 2;
            }
            else
            {
                CodeBox.Lines = code.ToArray();
                CodeBox.ReadOnly = false;
                view = 1;
            }
        }

        private void openMipsFile_Click(object sender, EventArgs e)
        {
            var dl = new DialogResult();
            if(CodeBox.Lines.Length>0 && filename=="")
            {
                dl = MessageBox.Show("Your file is unsaved! Click OK to continue, Cancel to return to file","Unsaved Progress",MessageBoxButtons.OKCancel);
                if(dl == DialogResult.Cancel)
                {
                    return;
                }
            }
            else if(CodeBox.Lines.Length > 0 && filename != "")
            {
                File.WriteAllLines(filename,CodeBox.Lines.ToArray());
                return;
            }
            DialogResult open = openFile.ShowDialog();
            if(open == DialogResult.OK)
            {
                filename = openFile.FileName;
                string[] file_data = File.ReadAllLines(filename);
                CodeBox.Clear();
                CodeBox.Lines = file_data;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Data_input d = new Data_input(ref listBox1,ref autocompleteMenu1);
            d.Show();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if(filename!="")
            {
                File.WriteAllLines(filename, CodeBox.Lines.ToArray());
            }
            else
            {
                DialogResult d = saveFileDialog1.ShowDialog();
                if(d == DialogResult.OK)
                {
                    filename = saveFileDialog1.FileName;
                    /*if (!File.Exists(filename))
                        File.Create(filename);*/
                    File.WriteAllLines(filename, CodeBox.Lines.ToArray());
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawTitle(OutputBox, e, "Output");
            DrawTitle(LNoBox, e, "#");
            DrawTitle(CodeBox, e, "Code");
            DrawTitle(tableLayoutPanel1, e, "Registers");
            DrawTitle(listBox1, e, "Variables");
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.FromArgb(30, 30, 30));
            Pen pen2 = new Pen(Color.FromArgb(62, 62, 66));
            SolidBrush brush = new SolidBrush(Color.FromArgb(30, 30, 30));
            SolidBrush brush2 = new SolidBrush(Color.FromArgb(62, 62, 66));
            Rectangle nr = new Rectangle(LNoBox.Right, LNoBox.Top - 22, CodeBox.Left - LNoBox.Right, CodeBox.Height + 23);
            g.DrawRectangle(pen2, nr);
            g.FillRectangle(brush2, nr);
            Rectangle r = new Rectangle(LNoBox.Right, LNoBox.Top, CodeBox.Left - LNoBox.Right, CodeBox.Height);
            g.DrawRectangle(pen, r);
            g.FillRectangle(brush, r);
        }

        private void DrawTitle(Control obj,PaintEventArgs e, string Title)
        {
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.FromArgb(62, 62, 66));
            SolidBrush brush = new SolidBrush(Color.FromArgb(62, 62, 66));
            int top = obj.Location.Y - 22;
            Rectangle rect = new Rectangle(obj.Location.X - 1, top, obj.Width + 2, obj.Height + 23);
            g.DrawRectangle(pen, rect);
            g.FillRectangle(brush, rect);
            Font TitleFont = new Font("Segou UI", 10);
            g.DrawString(Title, TitleFont, Brushes.White, new PointF(obj.Location.X - 1 + 5, top + 2));
            

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void ElaborateButton_Click(object sender, EventArgs e)
        {
            if(code.Count==0)
            {
                MessageBox.Show("Run your code at least once");
                return;
            }
            ElaborateForm ef = new ElaborateForm(code,BinaryCode,CodeDataType);
            ef.Show();
        }

        private void CompileButton_Click(object sender, EventArgs e)
        {
            SetAllToNull();
            FetchCode();
            ReadInstructions();
            GenerateOutput();
        }

        private void FetchCode()
        {
            code = CodeBox.Lines.ToList();
            int lineNumber = 0;
            foreach(string s in code)
            {
                if(String.IsNullOrEmpty(s))
                {
                    continue;
                }
                char[] BreakPoints = { ' ', ',', '.', '\t', '(', ')' };
                String[] parts = s.Split(BreakPoints);
                parts = parts.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                if (parts.Length == 1 && parts[0].EndsWith(":"))
                {
                    Labels.Add(parts[0].Substring(0,parts[0].Length-1),lineNumber);
                }
                lineNumber++;
            }
        }

        private void ReadInstructions()
        {
            foreach(String line in code)
            {
                if (String.IsNullOrEmpty(line))
                    continue;
                ConvertToMips(line);
            }
        }

        private int RecognizeRegister(string register_string)
        {
            if(register_string[0] != '$')
            {
                return -1;
            }
            else
            {
                Registers r;
                register_string = register_string.Substring(1);
                if (Enum.TryParse<Registers>(register_string, out r))
                {
                    return (int)r;
                }
                else if(Convert.ToInt32(register_string) >=0 && Convert.ToInt32(register_string)<32)
                {
                    return Convert.ToInt32(register_string);
                }
            }
            return -1;
        }


        private void ConvertToMips(String instruction)
        {
            ResetMipsVars();
            char[] BreakPoints = { ' ', ',', '.', '\t', '(', ')' };
            //char[] BreakPoints = { ' ', ',', '.', ':', '\t', '$', '(', ')' };
            String[] parts = instruction.Split(BreakPoints);
            parts = parts.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            String operation = parts[0];
            String[] same = { "add", "sub", "slt", "and", "or", "xor" };
            bool[] parsed = new bool[6];
            Registers r;
            if (parts.Length >= 2)
            {
                if (Enum.TryParse<Registers>(parts[1], out r))
                {
                    parts[1] = ((int)r).ToString();
                }
            }
            if(parts.Length>=3)
            {
                if (Enum.TryParse<Registers>(parts[2], out r))
                {
                    parts[2] = ((int)r).ToString();
                }
            }
            if (parts.Length >= 4)
            {
                if (Enum.TryParse<Registers>(parts[3], out r))
                {
                    parts[3] = ((int)r).ToString();
                }
            }

            if (same.Contains(operation))
            {
                rd = RecognizeRegister(parts[1]);
                rs = RecognizeRegister(parts[2]);
                rt = RecognizeRegister(parts[3]);
                InstructionTypeVar = InstructionType.RType;
            }
            if(operation == "move")
            {
                int.TryParse(parts[1], out rd);
                int.TryParse(parts[2], out rs);
                rt = 0;
            }
            switch(operation)
            {
                case "nop":
                    InstructionTypeVar = InstructionType.RType;
                    break;
                case "move":
                case "add":
                    function = Convert.ToInt32("100000", 2);
                    break;
                case "sub":
                    function = Convert.ToInt32("100010", 2);
                    break;
                case "and":
                    function = Convert.ToInt32("100100", 2);
                    break;
                case "or":
                    function = Convert.ToInt32("100101", 2);
                    break;
                case "xor":
                    function = Convert.ToInt32("100110", 2);
                    break;
                case "slt":
                    function = Convert.ToInt32("101010", 2);
                    break;
                // sll 
                case "sll":
                    function = Convert.ToInt32("000000", 2);
                    rd = RecognizeRegister(parts[1]);
                    rt = RecognizeRegister(parts[2]);
                    /*int.TryParse(parts[1], out rd);
                    int.TryParse(parts[2], out rt);*/
                    int.TryParse(parts[3], out shamt);
                    InstructionTypeVar = InstructionType.RType;
                    break;
                case "srl":
                    function = Convert.ToInt32("000010", 2);
                    rd = RecognizeRegister(parts[1]);
                    rt = RecognizeRegister(parts[2]);
                    /*int.TryParse(parts[1], out rd);
                    int.TryParse(parts[2], out rt);*/
                    int.TryParse(parts[3], out shamt);
                    InstructionTypeVar = InstructionType.RType;
                    break;
                case "jr":
                    function = Convert.ToInt32("001000", 2);
                    rs = RecognizeRegister(parts[1]);
                    //int.TryParse(parts[1], out rs);
                    InstructionTypeVar = InstructionType.RType;
                    break;
                case "syscall":
                    function = Convert.ToInt32("001100", 2);
                    InstructionTypeVar = InstructionType.RType;
                    break;
                // addi $1, $2, 100
                case "addi":
                    op = 8;
                    rt = RecognizeRegister(parts[1]);
                    rs = RecognizeRegister(parts[2]);
                    /*int.TryParse(parts[1], out rt);
                    int.TryParse(parts[2], out rs);*/
                    int.TryParse(parts[3], out immediate);
                    InstructionTypeVar = InstructionType.IType;
                    break;
                
                case "andi":
                    op = 12;
                    rt = RecognizeRegister(parts[1]);
                    rs = RecognizeRegister(parts[2]);
                    /*int.TryParse(parts[1], out rt);
                    int.TryParse(parts[2], out rs);*/
                    int.TryParse(parts[3], out immediate);
                    InstructionTypeVar = InstructionType.IType;
                    break;
                case "ori":
                    op = 13;
                    rt = RecognizeRegister(parts[1]);
                    rs = RecognizeRegister(parts[2]);
                    /*int.TryParse(parts[1], out rt);
                    int.TryParse(parts[2], out rs);*/
                    int.TryParse(parts[3], out immediate);
                    InstructionTypeVar = InstructionType.IType;
                    break;
                    //Li Uses Ori so both will be same with slight changes
                case "li":
                    op = 8;
                    rt = RecognizeRegister(parts[1]);
                    //int.TryParse(parts[1], out rt);
                    rs =0;
                    int.TryParse(parts[2], out immediate);
                    InstructionTypeVar = InstructionType.IType;
                    break;
                case "la":
                    op = 8;
                    rt = RecognizeRegister(parts[1]);
                    int ind = Data.FindIndex(x => x.name == parts[2]);
                    rs = 0;
                    if (ind != -1)
                    {
                        if (Data[ind].DataType == MipsDataType.ascii || Data[ind].DataType == MipsDataType.asciiz)
                        {
                            immediate = ind;
                        }
                        else
                        {
                            immediate = int.Parse(Data[ind].value);
                        }
                    }
                    InstructionTypeVar = InstructionType.IType;
                    break;
                case "slti":
                    op = 10;
                    rt = RecognizeRegister(parts[1]);
                    rs = RecognizeRegister(parts[2]);
                    int.TryParse(parts[3], out immediate);
                    InstructionTypeVar = InstructionType.IType;
                    break;
                //lw $t1,10($t2)
                case "lw":
                    op = 35;
                    rt = RecognizeRegister(parts[1]);
                    int.TryParse(parts[2], out immediate);
                    rs = RecognizeRegister(parts[3]);
                    offset = immediate + registers[rs];
                    InstructionTypeVar = InstructionType.IType;
                    break;
                case "sw":
                    op = 43;
                    rt = RecognizeRegister(parts[1]);
                    int.TryParse(parts[2], out immediate);
                    rs = RecognizeRegister(parts[3]);
                    offset = immediate + registers[rs];
                    InstructionTypeVar = InstructionType.IType;
                    break;
                case "j":
                    op = 2;
                    address = Labels[parts[1]];
                    InstructionTypeVar = InstructionType.JType;
                    break;
                case "jal":
                    op = 3;
                    address = Labels[parts[1]];
                    InstructionTypeVar = InstructionType.JType;
                    break;
                case "beq":
                    op = 4;
                    rt = RecognizeRegister(parts[1]);
                    rs = RecognizeRegister(parts[2]);
                    int.TryParse(parts[3], out offset);
                    InstructionTypeVar = InstructionType.Branch;
                    break;
                case "bgez":
                    op = 1;
                    rs = RecognizeRegister(parts[1]);
                    rt = 1;
                    int.TryParse(parts[2], out offset);
                    InstructionTypeVar = InstructionType.Branch;
                    break;
                case "bgtz":
                    op = 7;
                    rs = RecognizeRegister(parts[1]);
                    rt = 0;
                    int.TryParse(parts[2], out offset);
                    InstructionTypeVar = InstructionType.Branch;
                    break;
                case "bltz":
                    op = 1;
                    rs = RecognizeRegister(parts[1]);
                    rt = 0;
                    int.TryParse(parts[2], out offset);
                    InstructionTypeVar = InstructionType.Branch;
                    break;
                case "blez":
                    op = 6;
                    rs = RecognizeRegister(parts[1]);
                    rt = 0;
                    int.TryParse(parts[2], out offset);
                    InstructionTypeVar = InstructionType.Branch;
                    break;
                case "bne":
                    op = 5;
                    rt = RecognizeRegister(parts[1]);
                    rs = RecognizeRegister(parts[2]);
                    int.TryParse(parts[3], out offset);
                    InstructionTypeVar = InstructionType.Branch;
                    break;

                default:
                    break;
            }
            GenerateBinaryCode();
        }

        public void ResetMipsVars()
        {
            rs = 0;
            rt = 0;
            rd = 0;
            op = 0;
            shamt = 0;
            function = 0;
            offset = 0;
            immediate = 0;
            address = 0;
        }

        void GenerateOutput()
        {
            while(ins_counter<BinaryCode.Count())
            {
                ExecuteBinaryCode(BinaryCode[ins_counter].ToString());
            }
        }

        public void GenerateBinaryCode()
        {
            string binary = "";
            binary += Convert.ToString(op, 2).PadLeft(6, '0');
            if (InstructionTypeVar == InstructionType.RType)
            {
                CodeDataType.Add(InstructionType.RType);
                binary += Convert.ToString(rs, 2).PadLeft(5, '0');
                binary += Convert.ToString(rt, 2).PadLeft(5, '0');
                binary += Convert.ToString(rd, 2).PadLeft(5, '0');
                binary += Convert.ToString(shamt, 2).PadLeft(5, '0');
                binary += Convert.ToString(function, 2).PadLeft(6, '0');
            }
            else if (InstructionTypeVar == InstructionType.IType)
            {
                CodeDataType.Add(InstructionType.IType);
                binary += Convert.ToString(rs, 2).PadLeft(5, '0');
                binary += Convert.ToString(rt, 2).PadLeft(5, '0');
                string ts = Convert.ToString(immediate, 2).PadLeft(16, '0');
                binary += ts.Substring(ts.Length-16);
            }
            else if (InstructionTypeVar == InstructionType.JType)
            {
                CodeDataType.Add(InstructionType.JType);
                binary += Convert.ToString(address, 2).PadLeft(26, '0');
            }
            else if (InstructionTypeVar == InstructionType.Branch)
            {
                CodeDataType.Add(InstructionType.IType);
                binary += Convert.ToString(rs, 2).PadLeft(5, '0');
                binary += Convert.ToString(rt, 2).PadLeft(5, '0');
                binary += Convert.ToString(offset, 2).PadLeft(16, '0');
            }
            BinaryCode.Add(binary);
        }

        public void ExecuteBinaryCode(string inst)
        {
            op = (int)Convert.ToInt32(inst.Substring(0, 6), 2);
            if (op == 8 || op == 35 || op == 43 || op == 4 || op == 5 || op==7 || op==1 || op == 6)
            {
                rs = (int)Convert.ToInt32(inst.Substring(6, 5), 2);
                rt = (int)Convert.ToInt32(inst.Substring(11, 5), 2);
                immediate = (int)Convert.ToInt16(inst.Substring(16, 16), 2);
            }

            //Add Opcode in condition when you create an instruction which can jump
            if (op!=1 && op != 2 && op != 3 && op != 4 && op != 5 && op!=7 && op!=6)
            {
                ins_counter++;
            }
            switch (op)
            {
                case 0:
                    #region Rtype
                    rs = (int)Convert.ToInt32(inst.Substring(6, 5), 2);
                    rt = (int)Convert.ToInt32(inst.Substring(11, 5), 2);
                    rd = (int)Convert.ToInt32(inst.Substring(16, 5), 2);
                    shamt = (int)Convert.ToInt32(inst.Substring(21, 5), 2);
                    function = (int)Convert.ToInt32(inst.Substring(26, 6), 2);

                    registers_prev[rd] = registers[rd];
                    switch (function)
                    {
                        case 42: //101010 :slt
                            if (registers[rs] < registers[rt])
                                registers[rd] = 1;
                            else
                                registers[rd] = 0;
                            break;
                        case 32: //100000 :add
                            registers[rd] = registers[rs] + registers[rt];
                            break;
                        case 34: //100010 :sub
                            registers[rd] = registers[rs] - registers[rt];
                            break;
                        case 37: //100101 :or
                            registers[rd] = registers[rs] | registers[rt];
                            break;
                        case 36: //100100 :and
                            registers[rd] = registers[rs] & registers[rt];
                            break;
                        case 38: //100110 :xor
                            registers[rd] = registers[rs] ^ registers[rt];
                            break;
                        case 00: //000000 :sll
                            registers[rd] = registers[rt] * (int)Math.Pow(2, shamt);
                            break;
                        case 02: //000010 :srl
                            registers[rd] = registers[rt] * (int)Math.Pow(2, -shamt);
                            break;
                        case 08: //001000 :jr
                            ins_counter = registers[rs];
                            break;
                        case 12: //001100 :syscall
                            switch(registers[(int)Registers.v0])
                            {
                                case 1: // Print integer in $a0
                                    int addr = registers[(int)Registers.a0];
                                    OutputBox.Text += addr.ToString();
                                    break;
                                case 2: // Print float in $f12
                                case 3: // Print double in $f12
                                    OutputBox.Lines.Append(registers[(int)Registers.fp].ToString());
                                    break;
                                case 4: // Print string at address $a0
                                    int addre = registers[(int)Registers.a0];
                                    OutputBox.Text += Data[addre].value;
                                    break;
                                case 5:
                                    int.TryParse(Prompt.ShowDialog("Enter Integer Value: ", "Prompt"), out registers[(int)Registers.v0]);
                                    break;
                                case 10: //exit
                                    ins_counter = sbyte.MaxValue;
                                    break;
                            }
                            break;
                    }
                    #endregion //Rtype                    
                    break;
                case 8: //addi
                    registers_prev[rt] = registers[rt];
                    registers[rt] = registers[rs] + immediate;
                    break;
                case 10: //slti
                    if (registers[rs] < immediate)
                        registers[rt] = 1;
                    else
                        registers[rt] = 0;
                    registers_prev[rt] = registers[rt];
                    registers[rt] = registers[rs] & immediate;
                    break;
                case 12: //andi
                    registers_prev[rt] = registers[rt];
                    registers[rt] = registers[rs] & immediate;
                    break;
                case 13: //ori
                    registers_prev[rt] = registers[rt];
                    registers[rt] = registers[rs] | immediate;
                    break;
                case 35: //lw                    
                    offset = immediate + registers[rs];
                    registers[rt] = Convert.ToInt32(Data[offset].value);
                    break;
                case 43: //sw
                    offset = immediate + registers[rs];
                    Data[offset] = new MipsData("",registers[rt].ToString(),(int)MipsDataType.asciiz);
                    break;
                case 1: //bgez && bltz
                    
                    if (registers[rt] != 0)
                    {
                        if (registers[rs] >= 0)
                            ins_counter += immediate;
                        else
                            ins_counter++;
                    }
                    else
                    {
                        if (registers[rs] < 0)
                            ins_counter += immediate;
                        else
                            ins_counter++;
                    }
                    break;
                case 2: //jump                    
                    ins_counter = (int)Convert.ToInt64(inst.Substring(6, 26), 2);
                    break;
                case 3: //jal
                    registers[(int)Registers.ra] = ins_counter+1;
                    ins_counter = (int)Convert.ToInt64(inst.Substring(6, 26), 2);
                    MessageBox.Show(ins_counter.ToString());
                    break;
                case 4: //beq
                    if (registers[rt] == registers[rs])
                        ins_counter += immediate;
                    else
                        ins_counter++;
                    break;
                case 5: //bne
                    if (registers[rt] != registers[rs])
                        ins_counter += immediate;
                    else
                        ins_counter++;
                    break;
                case 6: //blez
                    if (registers[rs] <= 0)
                        ins_counter += immediate;
                    else
                        ins_counter++;
                    break;
                
                case 7: //bgtz
                    if (registers[rs] > 0)
                    {
                        ins_counter += immediate;
                    }
                    else
                    {
                        ins_counter++;
                    }
                    break;
                
            }
            UpdateRegisters();
        }
    }


    //Some Styling Down below. Don't disturb
    public class MySR : ToolStripSystemRenderer
    {
        public MySR()
        {

        }
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            //base.OnRenderToolStripBorder(e);
        }
    }
    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}
