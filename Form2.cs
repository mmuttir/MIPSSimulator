using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSASemesterProject
{
    public partial class ElaborateForm : Form
    {
        private int index = 0;
        private List<String> Code = new List<String>();
        private List<String> BinaryCode = new List<String>();
        private List<Form1.InstructionType> CodeDataType = new List<Form1.InstructionType>();
        public ElaborateForm(List<String> Code, List<String> BinaryCode, List<Form1.InstructionType> CodeDataType)
        {
            InitializeComponent();
            this.Code = Code;
            this.BinaryCode = BinaryCode;
            this.CodeDataType = CodeDataType;
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if(index!=Code.Count()-1)
                index++;
            Invalidate();
        }

        private void DrawInstruction(PaintEventArgs e)
        {
            if(CodeDataType[index] == Form1.InstructionType.RType)
            {
                DrawRType(BinaryCode[index],e);
            }
            else if(CodeDataType[index] == Form1.InstructionType.IType)
            {
                DrawIType(BinaryCode[index],e);
            }
            else if (CodeDataType[index] == Form1.InstructionType.JType)
            {
                DrawJType(BinaryCode[index],e);
            }
        }

        private void DrawRType(string data, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int middlex = this.ClientSize.Width / 2;
            int middley = this.ClientSize.Height / 2;
            int width = 400;
            int height = 50;
            int part = width / 6;
            Pen pen = new Pen(Color.White);
            Font TitleFont = new Font("Segou UI", 10);
            Rectangle R = new Rectangle(middlex - width / 2 + part * 0, middley - height / 2, part, height);
            g.DrawRectangle(pen, R);
            g.DrawString(data.Substring(0,6), TitleFont, Brushes.White, new PointF(middlex - width / 2 + part * 0 + 5, middley - height / 2 + 2));
            g.DrawString("OpCode", TitleFont, Brushes.White, new PointF(middlex - width / 2 + part * 0 + 5, middley - height / 2 - 20));

            R = new Rectangle(middlex - width / 2 + part * 1, middley - height / 2, part, height);
            g.DrawRectangle(pen, R);
            g.DrawString(data.Substring(6, 5), TitleFont, Brushes.White, new PointF(middlex - width / 2 + part * 1 + 5, middley - height / 2 + 2));
            g.DrawString("RS", TitleFont, Brushes.White, new PointF(middlex - width / 2 + part * 1 + 5, middley - height / 2 - 20));

            R = new Rectangle(middlex - width / 2 + part * 2, middley - height / 2, part, height);
            g.DrawRectangle(pen, R);
            g.DrawString(data.Substring(11, 5), TitleFont, Brushes.White, new PointF(middlex - width / 2 + part * 2 + 5, middley - height / 2 + 2));
            g.DrawString("RT", TitleFont, Brushes.White, new PointF(middlex - width / 2 + part * 2 + 5, middley - height / 2 - 20));

            R = new Rectangle(middlex - width / 2 + part * 3, middley - height / 2, part, height);
            g.DrawRectangle(pen, R);
            g.DrawString(data.Substring(16, 5), TitleFont, Brushes.White, new PointF(middlex - width / 2 + part * 3 + 5, middley - height / 2 + 2));
            g.DrawString("RD", TitleFont, Brushes.White, new PointF(middlex - width / 2 + part * 3 + 5, middley - height / 2 - 20));

            R = new Rectangle(middlex - width / 2 + part * 4, middley - height / 2, part, height);
            g.DrawRectangle(pen, R);
            g.DrawString(data.Substring(21, 5), TitleFont, Brushes.White, new PointF(middlex - width / 2 + part * 4 + 5, middley - height / 2 + 2));
            g.DrawString("Shamt", TitleFont, Brushes.White, new PointF(middlex - width / 2 + part * 4 + 5, middley - height / 2 - 20)); 

            R = new Rectangle(middlex - width / 2 + part * 5, middley - height / 2, part, height);
            g.DrawRectangle(pen, R);
            g.DrawString(data.Substring(26, 6), TitleFont, Brushes.White, new PointF(middlex - width / 2 + part * 5 + 5, middley - height / 2 + 2));
            g.DrawString("Function", TitleFont, Brushes.White, new PointF(middlex - width / 2 + part * 5 + 5, middley - height / 2 - 20)); 
        }

        private void DrawIType(string data, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int middlex = this.ClientSize.Width / 2;
            int middley = this.ClientSize.Height / 2;
            int width = 400;
            int height = 50;
            int part = width / 6;
            Pen pen = new Pen(Color.White);
            Font TitleFont = new Font("Segou UI", 10);
            Rectangle R = new Rectangle(middlex - width / 2 + part * 0, middley - height / 2, part, height);
            g.DrawRectangle(pen, R);
            g.DrawString(data.Substring(0, 6), TitleFont, Brushes.White, new PointF(middlex - width / 2 + part * 0 + 5, middley - height / 2 + 2));
            g.DrawString("OpCode", TitleFont, Brushes.White, new PointF(middlex - width / 2 + part * 0 + 5, middley - height / 2 - 20));

            R = new Rectangle(middlex - width / 2 + part * 1, middley - height / 2, part, height);
            g.DrawRectangle(pen, R);
            g.DrawString(data.Substring(6, 5), TitleFont, Brushes.White, new PointF(middlex - width / 2 + part * 1 + 5, middley - height / 2 + 2));
            g.DrawString("RS", TitleFont, Brushes.White, new PointF(middlex - width / 2 + part * 1 + 5, middley - height / 2 - 20));

            R = new Rectangle(middlex - width / 2 + part * 2, middley - height / 2, part, height);
            g.DrawRectangle(pen, R);
            g.DrawString(data.Substring(11, 5), TitleFont, Brushes.White, new PointF(middlex - width / 2 + part * 2 + 5, middley - height / 2 + 2));
            g.DrawString("RT", TitleFont, Brushes.White, new PointF(middlex - width / 2 + part * 2 + 5, middley - height / 2 - 20));

            R = new Rectangle(middlex - width / 2 + part * 3, middley - height / 2, part*3, height);
            g.DrawRectangle(pen, R);
            g.DrawString(data.Substring(16, 16), TitleFont, Brushes.White, new PointF(middlex - width / 2 + part * 3 + 5, middley - height / 2 + 2));
            g.DrawString("Address/Immidiate", TitleFont, Brushes.White, new PointF(middlex - width / 2 + part * 3 + 5, middley - height / 2 - 20));
        }

        private void DrawJType(string data,PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int middlex = this.ClientSize.Width / 2;
            int middley = this.ClientSize.Height / 2;
            int width = 400;
            int height = 50;
            int part = width / 6;
            Font TitleFont = new Font("Segou UI", 10);
            Pen pen = new Pen(Color.White);
            Rectangle R = new Rectangle(middlex - width / 2 + part * 0, middley - height / 2, part, height);
            g.DrawRectangle(pen, R);
            g.DrawString(data.Substring(0, 6), TitleFont, Brushes.White, new PointF(middlex - width / 2 + part * 0 + 5, middley - height / 2 + 2));
            g.DrawString("OpCode", TitleFont, Brushes.White, new PointF(middlex - width / 2 + part * 0 + 5, middley - height / 2 - 20));

            R = new Rectangle(middlex - width / 2 + part * 1, middley - height / 2, part*5, height);
            g.DrawRectangle(pen, R);
            g.DrawString(data.Substring(6, 26), TitleFont, Brushes.White, new PointF(middlex - width / 2 + part * 1 + 5, middley - height / 2 + 2));
            g.DrawString("Target Address", TitleFont, Brushes.White, new PointF(middlex - width / 2 + part * 1 + 5, middley - height / 2 - 20));
        }

        private void ElaborateForm_Paint(object sender, PaintEventArgs e)
        {
            EnableDisableButtons();
            CommandLabel.Text = Code[index];
            InstTypeLabel.Text = CodeDataType[index].ToString();
            DrawInstruction(e);
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            if(index!=0)
                index--;
            Invalidate();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void EnableDisableButtons()
        {
            if(index == 0)
            {
                BackButton.Enabled = false;
            }
            else
            {
                BackButton.Enabled = true;
            }
            if(index==Code.Count()-1)
            {
                NextButton.Enabled = false;
            }
            else
            {
                NextButton.Enabled = true;
            }
        }

        private void ElaborateForm_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }
    }

}
