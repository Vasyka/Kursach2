using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DrawingLibrary;

namespace WindowsFormsKurs
{
    public partial class DeletingForm : Form
    {
        private int nodeIndex;
        public DeletingForm()
        {
            try {
                InitializeComponent();

                //Добавляем имена файлов в дерево
                TreeNode FileNode = new TreeNode("Файлы");
                foreach (string fileName in DrawingClass.NameList)
                {
                    FileNode.Nodes.Add(new TreeNode(fileName));
                }
                treeView1.Nodes.Add(FileNode);

                //Инициализируем поле с номером удаляемого графика
                Program.FileIndex = -1;
                nodeIndex = -1;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message,"Ошибка!",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Если выделен узел, сохраняем его номер
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = treeView1.SelectedNode;
            if (node.Level != 0)//если не корневой узел
            {
                nodeIndex = node.Index;
            }
        }

        //Посылаем номер графика для удаления, закрываем форму
        private void button1_Click(object sender, EventArgs e)
        {
            Program.FileIndex = nodeIndex;
            Close();
        }

        private void DeletingForm_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

    }
}
