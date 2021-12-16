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
    public partial class Data_input : Form
    {
        ListBox l = new ListBox();
        AutocompleteMenuNS.AutocompleteMenu menu = new AutocompleteMenuNS.AutocompleteMenu();
        public Data_input(ref ListBox l, ref AutocompleteMenuNS.AutocompleteMenu menu)
        {
            InitializeComponent();
            this.l = l;
            this.menu = menu;
            var source = new BindingSource();
            source.DataSource = Form1.Data;
            dataGridView1.DataSource = source;
            var cbsource = new BindingSource();
            List<String> enumVals = Enum.GetNames(typeof(Form1.MipsDataType)).ToList();
            cbsource.DataSource = enumVals;
            DataType.DataSource = cbsource;
        }

        private void AddData_Click(object sender, EventArgs e)
        {
            int dt = DataType.SelectedIndex;
            string d = DataBox.Text;
            string dn = DataName.Text;
            Form1.MipsDataType dtype = (Form1.MipsDataType)dt;
            if (dtype == Form1.MipsDataType.half || dtype == Form1.MipsDataType.mips_byte || dtype == Form1.MipsDataType.word)
            {
                int extra;
                if(!int.TryParse(d,out extra))
                {
                    MessageBox.Show("Value doesn't match datatype");
                    return;
                }
            }
            if(Form1.Data.Any(x=>x.name==dn))
            {
                MessageBox.Show("Data with same name already exist. Try using unique data name");
                return;
            }
            if(Form1.AllCommands.Any(x=>x==dn))
            {
                MessageBox.Show("Data name can't match mips command name");
            }
            Form1.Data.Add(new Form1.MipsData(dn, d, dt));
            var source = new BindingSource();
            source.DataSource = Form1.Data;
            dataGridView1.DataSource = source;
            var source2 = new BindingSource();
            List<String> varlist = new List<String>();
            for (int i = 0; i < Form1.Data.Count(); i++)
            {
                varlist.Add(Form1.Data[i].name + " = " + Form1.Data[i].value);
            }

            source2.DataSource = varlist;
            l.DataSource = source2;
            menu.AddItem(dn);
        }

        private void ClearData_Click(object sender, EventArgs e)
        {
            Form1.Data.Clear();
            var source = new BindingSource();
            source.DataSource = Form1.Data;
            dataGridView1.DataSource = source;

            var source2 = new BindingSource();
            List<String> varlist = new List<String>();
            source2.DataSource = varlist;
            l.DataSource = source2;
        }
    }
}
