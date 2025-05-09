using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Main
{
    public partial class MainForm : Form
    {
        string FileName;
        public MainForm()
        {
            InitializeComponent();
            timer1.Start();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            AddRecord();
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            //определение выбранного компонента ListBox
            string s = "listBox" + (OrgTabControl.SelectedIndex + 1);
            ListBox CurrentListBox = (ListBox)Controls.Find(s, true)[0];
            //запись в окно редактирования выбранного значения
            RecordTextBox.Text = (string)CurrentListBox.SelectedItem;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            FileName = dateTimePicker1.Text + "org";
            LoadFromFile(FileName);
        }

        private void SaveToFile(string FileName)
        {
            try
            {
                //открываем файл для записи
                using (StreamWriter sw = new StreamWriter(FileName))
                {
                    //перебираем все компоненты ListBox
                    for (int i = 1; i <= 4; i++)
                    {
                        //задаем текущий компонент ListBox
                        ListBox CurListBox =
                        (ListBox)Controls.Find("listBox" + i, true)[0];
                        //записываем в файл кол во строк списка
                        sw.WriteLine(CurListBox.Items.Count.ToString());
                        //записываем в файл все записи из списка
                        for (int j = 0; j < CurListBox.Items.Count; j++)
                            sw.WriteLine(CurListBox.Items[j]);
                        //очищаем список записей текущего ListBox
                        CurListBox.Items.Clear();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Ошибка при сохранении!");
            }
        }

        private void LoadFromFile(string FileName)
        {
            try
            {
                using(StreamReader sr = new StreamReader(FileName))
                {
                    for(int i = 1; i <= 4; i++)
                    {
                        int ItemCount = int.Parse(sr.ReadLine());

                        ListBox CurListBox = (ListBox)Controls.Find("listBox" + i, true)[0];
                        for (int j = 0; j < ItemCount; j++)
                        {
                            string line = sr.ReadLine();
                            
                            CurListBox.Items.Add(line);
                        }
                    }

                }
            }
            catch (FileNotFoundException)
            {
                //MessageBox.Show("Error");
                listBox1.Items.Clear();
                listBox2.Items.Clear();
                listBox3.Items.Clear();
            }
            catch
            {
                MessageBox.Show("Ошибка при чтении!");
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            SaveToFile(FileName);
            FileName = dateTimePicker1.Text + "org";
            LoadFromFile(FileName);
        }

        private void ChangeButton_Click(object sender, EventArgs e)
        {
            string s = "listBox" + (OrgTabControl.SelectedIndex + 1);
            ListBox CurrentListBox = (ListBox)Controls.Find(s, true)[0];
            CurrentListBox.Items[CurrentListBox.SelectedIndex] = RecordTextBox.Text;
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            string s = "listBox" + (OrgTabControl.SelectedIndex + 1);
            ListBox CurrentListBox = (ListBox)Controls.Find(s, true)[0];
            int selectedIndex = CurrentListBox.SelectedIndex;

            CurrentListBox.Items.RemoveAt(selectedIndex);

            if (selectedIndex > 0)
            {
                CurrentListBox.SelectedIndex = selectedIndex - 1;
            }
            else if (CurrentListBox.Items.Count > 0)
            {
                // Если удалена первая строка, устанавливаем выделение на новую первую строку
                CurrentListBox.SelectedIndex = 0;
            }

            RecordTextBox.Text = (string)CurrentListBox.SelectedItem;
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            string s = "listBox" + (OrgTabControl.SelectedIndex + 1);
            ListBox CurrentListBox = (ListBox)Controls.Find(s, true)[0];
            CurrentListBox.Items.Clear();
        }

        private void AddRecord()
        {
            //определение выбранного компонента ListBox
            string s = "listBox" + (OrgTabControl.SelectedIndex + 1);
            ListBox CurrentListBox = (ListBox)Controls.Find(s, true)[0];
            //добавление записи на текущий ListBox
            CurrentListBox.Items.Add(RecordTextBox.Text);
            //очистка окна ввода
            RecordTextBox.Text = "";
        }

        private void RecordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                AddRecord();
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            SaveToFile(FileName);
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss dddd");
        }
    }
}
