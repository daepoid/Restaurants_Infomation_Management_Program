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

namespace NoDGV
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            initialize_TLP(true);
            Text = "맛집 정보";
        }
        
        const string path = "../data.txt";
        int checked_Count = 0;
        List<Restaurant> restaurants = new List<Restaurant>();
        List<CheckBox> checkBoxes = new List<CheckBox>();

        Restaurant temp = new Restaurant("", "", "");

        public class Restaurant
        {
            public string st_name { get; set; }
            public string st_menu { get; set; }
            public string st_location { get; set; }

            public Restaurant(string _st_name, string _st_menu, string _st_location)
            {
                st_name = _st_name;
                st_menu = _st_menu;
                st_location = _st_location;
            }

            //public override bool Equals(object obj)
            //{
            //    return base.Equals(obj);
            //}

            //public override int GetHashCode()
            //{
            //    return base.GetHashCode();
            //}

            public static bool operator == (Restaurant r1, Restaurant r2)
            {
                if(r1.st_name.Equals(r2.st_name) && r1.st_menu.Equals(r2.st_menu) && r1.st_location.Equals(r2.st_location))
                {
                    return true;
                }
                return false;
            }

            public static bool operator != (Restaurant r1, Restaurant r2)
            {
                if (r1.st_name.Equals(r2.st_name) && r1.st_menu.Equals(r2.st_menu) && r1.st_location.Equals(r2.st_location))
                {
                    return false;
                }
                return true;
            }

        }

        private void set_LabelFont(Label label, string str, int font, bool bold)
        {
            label.Text = str;
            label.AutoSize = true;
            label.Size = new Size(130, 20);
            label.Font = new Font(label.Font.FontFamily, font, bold ? FontStyle.Bold : FontStyle.Regular);
        }

        private void clear_TextBoxs()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void clear_CheckBoxs()
        {
            for(int i = 0; i < checkBoxes.Count; i++)
            {
                checkBoxes[i].Checked = false;
            }
        }

        private bool check_DataOverlapping(Restaurant new_restaurant)
        {

            int index = restaurants.FindIndex(x => x == new_restaurant);
            if(index != -1)
            {
                string temp = "가게 이름 : " + new_restaurant.st_name + "\n메뉴 : " + new_restaurant.st_menu + "\n주소 : " + new_restaurant.st_location;
                MessageBox.Show(temp + "\n데이터가 중복입니다.");
                return true;
            }
            return false;
            //for (int i = 0; i < restaurants.Count; i++)
            //{
            //    if (restaurants[i] == new_restaurant)
            //    {
            //        string temp = "가게 이름 : " + new_restaurant.st_name + "\n메뉴 : " + new_restaurant.st_menu + "\n주소 : " + new_restaurant.st_location;
            //        MessageBox.Show(temp + "\n데이터가 중복입니다.");
            //        return true;
            //    }
            //}
            //return false;
        }

        private void add_Data_TLP(string st_name, string st_menu, string st_location)
        {
            Restaurant new_restaurant = new Restaurant(st_name, st_menu, st_location);
            if (!check_DataOverlapping(new_restaurant))
            {
                restaurants.Add(new_restaurant);
                add_Control_TLP(new_restaurant);
                clear_TextBoxs();
                clear_CheckBoxs();
            }                      
        }

        private void add_Control_TLP(Restaurant restaurant)
        {
            CheckBox checkbox = new CheckBox();
            checkbox.AutoSize = false;
            checkbox.Size = new Size(20, 15);
            checkBoxes.Add(checkbox);
            checkbox.CheckedChanged += delegate (object obj, EventArgs arg)
            {
                if (checkbox.Checked)
                {
                    checked_Count++;
                    textBox1.Text = restaurant.st_name;
                    textBox2.Text = restaurant.st_menu;
                    textBox3.Text = restaurant.st_location;

                    temp.st_name = restaurant.st_name;
                    temp.st_menu = restaurant.st_menu;
                    temp.st_location = restaurant.st_location;
                }
                else
                {
                    if(checked_Count == 1)
                    {
                        clear_TextBoxs();
                    }
                    checked_Count--;
                }
            };
            tableLayoutPanel1.Controls.Add(checkbox);

            Label label1 = new Label();
            set_LabelFont(label1, restaurant.st_name, 13, false);
            tableLayoutPanel1.Controls.Add(label1);

            Label label2 = new Label();
            set_LabelFont(label2, restaurant.st_menu, 13, false);
            tableLayoutPanel1.Controls.Add(label2);

            Label label3 = new Label();
            set_LabelFont(label3, restaurant.st_location, 13, false);
            tableLayoutPanel1.Controls.Add(label3);
        }

        private void initialize_TLP(bool restaurants_clear_bit)
        {
            if(restaurants_clear_bit)
            {
                restaurants.Clear();
            }            
            checkBoxes.Clear();
            tableLayoutPanel1.Controls.Clear();

            Panel panel = new Panel();
            tableLayoutPanel1.Controls.Add(panel);

            Label label1 = new Label();
            set_LabelFont(label1, "가게 이름", 16, true);
            tableLayoutPanel1.Controls.Add(label1);

            Label label2 = new Label();
            set_LabelFont(label2, "메뉴", 16, true);
            tableLayoutPanel1.Controls.Add(label2);

            Label label3 = new Label();
            set_LabelFont(label3, "위치", 16, true);
            tableLayoutPanel1.Controls.Add(label3);
        }

        private void button_load_Click(object sender, EventArgs e)
        {
            try
            {
                initialize_TLP(true);
                string[] lines = File.ReadAllLines(path, Encoding.Default);
                foreach (string line in lines)
                {
                    string[] parseLine = line.Split('|');
                    add_Data_TLP(parseLine[0], parseLine[1], parseLine[2]);
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("File NOT Found.");
            }
            catch (IOException)
            {
                MessageBox.Show("ERROR - IOException");
            }
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            try
            {
                string[] lines = new string[restaurants.Count];
                for(int i = 0; i < restaurants.Count; i++)
                {
                    lines[i] = restaurants[i].st_name + '|' + restaurants[i].st_menu + '|' + restaurants[i].st_location;
                }
                File.WriteAllLines(path, lines, Encoding.Default);
                MessageBox.Show("저장 완료");
            }
            catch(IOException)
            {
                MessageBox.Show("ERROR - IOException");
            }
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            try
            {
                if (restaurants.Count == 0)
                {
                    initialize_TLP(true);
                }
                if (textBox1.Text.Equals("") || textBox2.Text.Equals("") || textBox3.Text.Equals(""))
                {
                    MessageBox.Show("비어있는 요소가 있습니다.");
                    return;
                }
                add_Data_TLP(textBox1.Text, textBox2.Text, textBox3.Text);
                MessageBox.Show("Add Complete");
            }
            catch (Exception)
            {
                MessageBox.Show("Add Error");
            }
            
        }

        private void button_edit_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = checkBoxes.Count - 1; i >= 0; i--)
                {
                    if (checkBoxes[i].Checked && restaurants[i] == temp)
                    {
                        restaurants[i].st_name = textBox1.Text;
                        restaurants[i].st_menu = textBox2.Text;
                        restaurants[i].st_location = textBox3.Text;
                        break;
                    }
                }
                initialize_TLP(false);
                for (int i = 0; i < restaurants.Count; i++)
                {
                    add_Control_TLP(restaurants[i]);
                }
                clear_TextBoxs();
                checked_Count = 0;
                MessageBox.Show("Edit Complete");
            }
            catch (Exception)
            {
                MessageBox.Show("Edit Error");
            }            
        }

        private void button_remove_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = checkBoxes.Count - 1; i >= 0; i--)
                {
                    if (checkBoxes[i].Checked)
                    {
                        checkBoxes.RemoveAt(i);
                        restaurants.RemoveAt(i);
                        for (int j = 0; j < tableLayoutPanel1.ColumnCount; j++)
                        {
                            tableLayoutPanel1.Controls.RemoveAt((i + 1) * 4);
                        }
                    }
                }
                clear_TextBoxs();
                checked_Count = 0;

                MessageBox.Show("Remove Complete");
            }
            catch (Exception)
            {
                MessageBox.Show("Remove Error");
            }
            
        }
    }
}
